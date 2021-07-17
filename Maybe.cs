﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FPLibrary.Maybe;
using Microsoft.VisualBasic.CompilerServices;
using Unit = System.ValueTuple;

namespace FPLibrary {
    using static F;

    public static partial class F {
        public static Nothing Nothing => default;

        public static Maybe<T> Just<T>([NotNull] T value) 
            => new(value ?? throw new ArgumentNullException(nameof(value)));
    }

    public readonly struct Maybe<T> : IEquatable<Nothing>, IEquatable<Maybe<T>> {
        private readonly bool isJust;
        private readonly T value;

        internal Maybe(T t) {
            isJust = true;
            value = t;
        }

        public static implicit operator Maybe<T>(Nothing _) => new();

        public static implicit operator Maybe<T>(T value)
            => value is null ? Nothing : Just(value);

        public R Match<R>(Func<R> nothing, Func<T, R> just)
            => isJust ? just(value) : nothing();

        public IEnumerable<T> AsEnumerable() {
            if (isJust) yield return value;
        }

        public bool Equals(Maybe<T> other)
            => isJust == other.isJust && (!isJust || value.Equals(other.value));
        public bool Equals(Nothing _) => !isJust;
        public override bool Equals(object other)
            => other is Maybe<T> && Equals(other);
        public override int GetHashCode()
            => isJust ? value.GetHashCode() : 0;
        public static bool operator ==(Maybe<T> self, Maybe<T> other) => self.Equals(other);
        public static bool operator !=(Maybe<T> self, Maybe<T> other) => !self.Equals(other);

        public override string ToString() => isJust ? $"Just({value})" : "Nothing";
    }

    namespace Maybe {
        //you can instantiate but not access internal fields.
        public readonly struct Nothing { }

        public readonly struct Just<T> {
            internal T Value { get; }

            internal Just(T value) {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), 
                        "Cannot wrap null in Just. Did you mean to use Nothing");
                Value = value;
            }
        }
    }

    public static class MaybeExt {
        public static Maybe<R> Map<T, R>(this Maybe<T> maybeT, Func<T, R> f)
            => maybeT.Match(() => Nothing, t => Just(f(t)));

        public static Maybe<R> Map<T, R>(this Nothing _, Func<T, R> __) => Nothing;

        public static Maybe<R> Map<T, R>(this Just<T> just, Func<T, R> f)
            => Just(f(just.Value));

        public static Maybe<Unit> ForEach<T>(this Maybe<T> maybe, Action<T> act)
            => Map(maybe, act.ToFunc());

        public static Maybe<R> Bind<T, R>(this Maybe<T> maybeT, Func<T, Maybe<R>> f)
            => maybeT.Match(() => Nothing, f);

        //give IEnumerable<R> instead of Maybe<IEnumerable<R>>
        public static IEnumerable<R> Bind<T, R>(this Maybe<T> maybe, Func<T, IEnumerable<R>> f)
            => maybe.AsEnumerable().Bind(f);

        public static Maybe<T> Where<T>(this Maybe<T> maybeT, Predicate<T> predicate)
            => maybeT.Match(() => Nothing, t => predicate(t) ? maybeT : Nothing);

        public static Either<L, R> ToEither<L, R>(this Maybe<R> self, Func<L> left)
            => self.Match<Either<L, R>>(() => left(), r => r);

        public static Either<L, R> ToEither<L, R>(this Maybe<R> self, L defaultLeftVal)
            => self.Match<Either<L, R>>(() => defaultLeftVal, r => r);

        //function application
        //apply a Maybe<T> to a Maybe<Func<T, R>> if both are Just
        public static Maybe<R> Apply<T, R>(this Maybe<Func<T, R>> self, Maybe<T> arg)
            => self.Match(
                () => Nothing,
                (f) => arg.Match(
                    () => Nothing,
                    (val) => Just(f(val))));

        public static Maybe<Func<T2, R>> Apply<T1, T2, R>(this Maybe<Func<T1, T2, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Maybe<Func<T1, T2, T3, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Maybe<Func<T1, T2, T3, T4, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Maybe<Func<T1, T2, T3, T4, T5, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>(this Maybe<Func<T1, T2, T3, T4, T5, T6, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>(this Maybe<Func<T1, T2, T3, T4, T5, T6, T7, R>> self, Maybe<T1> arg)
            => Apply(self.Map(F.CurryFirst), arg);
    }
}
