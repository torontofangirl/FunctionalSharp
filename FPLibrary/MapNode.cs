﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using FPLibrary;
using static FPLibrary.F;

namespace FPLibrary {
    public sealed partial class Map<K, V> where K : IComparable<K> {
        internal sealed class Node {
            internal static readonly Node Empty = new();
            private readonly K key = default!;
            private readonly V value = default!;
            private bool frozen;
            //AVL tree max height is 1.44 * log2(n + 2)
            private byte height;
            private Node? left;
            private Node? right;

            #region Properties

            public bool IsEmpty => left is null;

            #endregion

            #region Ctors
            private Node() => frozen = true;

            private Node(K key, V val, Node left, Node right, bool frozen=false) {
                this.key = key;
                value = val;
                this.left = left;
                this.right = right;
                height = checked((byte)(1 + Math.Max(left.height, right.height)));
                this.frozen = frozen;
            }

            #endregion

            internal void Freeze() {
                if (frozen) return;

                left!.Freeze();
                right!.Freeze();
                frozen = true;
            }

            internal (Node Node, bool Mutated) Add(IComparer<K> keyComparer,
                IEqualityComparer<V> valComparer, K _key, V val) {

                (Node node, _, bool mutated) = SetOrAdd(keyComparer, valComparer, false, _key, val);

                return (node, mutated);
            }

            internal (Node Node, bool Replaced, bool Mutated) Set(IComparer<K> keyComparer, 
                IEqualityComparer<V> valComparer, K _key, V val) {

                (Node node, bool replaced, bool mutated) = SetOrAdd(keyComparer, valComparer, true, _key, val);

                return (node, replaced, mutated);
            }

            #region Balancing methods

            /*
            a
             \
              b
               \
                c
            
            b becomes the new root
            a takes b's left node (null)
            b takes a as left
            */
            private static Node RotateLeft(Node tree) {
                if (tree is null) throw new ArgumentNullException(nameof(tree));
                Debug.Assert(!tree.IsEmpty);

                if (tree.right!.IsEmpty)
                    return tree;

                Node right = tree.right;
                return right.Mutate(
                    //right.left = tree
                    _left: tree.Mutate(
                        //tree.left = right.left
                        _right : right.left));

            }

            private static Node RotateRight(Node tree) {

            }

            private static Node DoubleLeft(Node tree) {

            }

            private static Node DoubleRight(Node tree) {

            }

            private static int Balance(Node tree)
                => tree.right!.height - tree.left!.height;

            private static bool IsRightHeavy(Node tree)
                => Balance(tree) >= 2;

            private static bool IsLeftHeavy(Node tree)
                => Balance(tree) <= -2;

            private static Node MakeBalanced(Node tree) {

            }

            #endregion

            private Node Mutate(Node? _left=null, Node? _right=null) {
                Debug.Assert(left is not null && right is not null);

                if (frozen)
                    return new(key, value, _left ?? left, _right ?? right);

                if (_left is not null)
                    left = _left;
                if (_right is not null)
                    right = _right;
                height = checked((byte) (1 + Math.Max(left.height, right.height)));

                return this;
            }

            private (Node Node, bool Replaced, bool Mutated) SetOrAdd(IComparer<K> keyComparer,
                IEqualityComparer<V> valComparer, bool overwrite, K _key, V val) {

                //no arg validation because recursive

                if (IsEmpty)
                    return (new(key, value, this, this), false, true);
                else {
                    //TODO: mix assignment and creation in deconstruction and/or better chaining

                    (Node Node, bool Replaced, bool Mutated) setOrAdd(Node node)
                        => node.SetOrAdd(keyComparer, valComparer, overwrite, key, val);

                    switch (keyComparer.Compare(_key, key)) {
                        case > 0:
                            (Node newRight, bool replaced, bool mutated) = setOrAdd(right!);

                            Node res = Mutate(_right: newRight);

                            return (mutated
                                ? MakeBalanced(res)
                                : res, replaced, mutated);
                        case < 0:
                            (Node newLeft, bool lreplaced, bool lmutated) = setOrAdd(left!);

                            Node lres = Mutate(_left: newLeft);

                            return (lmutated
                                ? MakeBalanced(lres)
                                : lres, lreplaced, lmutated);
                        default:
                            if (valComparer.Equals(value, val))
                                return (this, false, false);
                            else if (overwrite)
                                return (new(key, value, left!, right!), true, true);
                            else
                                throw new ArgumentException("Duplicate key: ", nameof(key));
                    }
                }
            }

            private Node Search(IComparer<K> keyComparer, K _key) {
                if (IsEmpty) 
                    return this;

                return keyComparer.Compare(_key, key) switch {
                    0 => this,
                    > 0 => right!.Search(keyComparer, key),
                    _ => left!.Search(keyComparer, key),
                };
            }
        }
    }
}