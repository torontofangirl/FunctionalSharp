﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static FPLibrary.F;

namespace FPLibrary {
    public sealed partial class Map<K, V> : IReadOnlyDictionary<K, V> where K : notnull {
        public V this[K key] => throw new NotImplementedException();

        public IEnumerable<K> Keys => throw new NotImplementedException();
        public IEnumerable<V> Values => throw new NotImplementedException();
    }
    
    // public sealed partial class Map<K, V> : IDictionary, IDictionary<K, V> where K : notnull {
    //     #region IDictionary<K, V> Properties
    //
    //     ICollection<K> IDictionary<K, V>.Keys => throw new NotImplementedException();
    //     ICollection<V> IDictionary<K, V>.Values => throw new NotImplementedException();
    //
    //     V IDictionary<K, V>.this[K key] {
    //         get => this[key];
    //         set => throw new NotSupportedException();
    //     }
    //
    //     #endregion
    //
    //     #region IDictionary<K, V> Methods
    //
    //     void IDictionary<K, V>.Add(K key, V val) => throw new NotSupportedException();
    //     
    //     bool IDictionary<K, V>.Remove(K key) => throw new NotSupportedException();
    //
    //     #endregion
    //
    //     #region IDictionary Properties
    //
    //     bool IDictionary.IsFixedSize => true;
    //     
    //     bool IDictionary.IsReadOnly => true;
    //
    //     ICollection IDictionary.Keys => throw new NotImplementedException();
    //
    //     ICollection IDictionary.Values => throw new NotImplementedException();
    //
    //     #endregion
    //
    //     #region IDictionary Methods
    //
    //     bool IDictionary.Contains(object key) => ContainsKey((K) key);
    //
    //     void IDictionary.Add(object key, object? value) => throw new NotSupportedException();
    //
    //     void IDictionary.Clear() => throw new NotSupportedException();
    //     
    //     void IDictionary.Remove(object key) => throw new NotSupportedException();
    //
    //     object? IDictionary.this[object key] {
    //         get => this[(K) key];
    //         set => throw new NotSupportedException();
    //     }
    //
    //     IDictionaryEnumerator IDictionary.GetEnumerator()
    //         => throw new NotImplementedException();
    //
    //     #endregion
    // }
}