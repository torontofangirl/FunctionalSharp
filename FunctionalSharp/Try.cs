﻿using System;

namespace FunctionalSharp {
    public delegate Exceptional<T> Try<T>();

    public static partial class F {
        public static Try<T> Try<T>(Func<T> f) => () => f();
    }

    public static class TryExt {
        public static Exceptional<T> Run<T>(this Try<T> self) {
            try {
                return self();
            } catch (Exception ex) {
                return ex;
            }
        }

        public static Try<R> Bind<T, R>(this Try<T> self, Func<T, Try<R>> f)
            => ()
                => self.Run()
                    .Match(ex => ex, t => f(t).Run());

        public static Try<R> Map<T, R>(this Try<T> self, Func<T, R> f)
            => ()
                => self.Run()
                    .Match<Exceptional<R>>(ex => ex, t => f(t));

        public static Try<Func<T2, R>> Map<T1, T2, R>(this Try<T1> self, Func<T1, T2, R> f)
            => self.Map(f.CurryFirst());

        //query pattern
        public static Try<R> Select<T, R>(this Try<T> self, Func<T, R> f)
            => self.Map(f);

        public static Try<RR> SelectMany<T, R, RR>(this Try<T> self, Func<T, Try<R>> f, Func<T, R, RR> project)
            => ()
                => self.Run()
                    .Match(ex => ex, t => f(t).Run()
                        .Match<Exceptional<RR>>(
                            ex => ex,
                            r => project(t, r)));
    }
}