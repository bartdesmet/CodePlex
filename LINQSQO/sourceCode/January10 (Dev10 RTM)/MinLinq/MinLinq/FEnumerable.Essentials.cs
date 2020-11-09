//
// Project "MinLINQ" - Bart De Smet (C) 2010
//
// http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx
//
// This project is meant as an illustration of how an academically satifying layering
// of a LINQ to Objects implementation can be realized using monadic concepts and only
// three primitives: anamorphism, bind and catamorphism.
//
// The code in this project is not meant to be used in production and no guarantees are
// made about its functionality. Use it for academic stimulation purposes only. To use
// LINQ for real, use System.Linq in .NET 3.5 or higher.
//
// All of the source code may be used in presentations of LINQ or for other educational
// purposes, but references to http://www.codeplex.com/LINQSQO and the blog post referred
// to above - "The Essence of LINQ - MinLINQ" - are required.
//

using System;

namespace MinLinq
{
    /// <summary>
    /// FEnumerable essential query operators:
    /// - Anamorphism:  T --> (T --> bool) --> (T --> T) --> (T --> R) --> M{R}
    /// - Bind:         M{T} --> (T --> bool) --> (T --> M{C}) --> (T --> C --> R) --> M{R}
    /// - Catamorphism: M{T} --> R --> (R --> bool) --> (R --> T --> R) --> R
    /// </summary>
    public static partial class FEnumerable
    {
        /// <summary>
        /// Anamorphism.
        /// </summary>
        /// <typeparam name="T">Source generator input type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="seed">Seed value.</param>
        /// <param name="condition">Terminating condition.</param>
        /// <param name="next">Iteration function.</param>
        /// <param name="result">Result selector.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Ana<T, R>(T seed, Func<T, bool> condition, Func<T, T> next, Func<T, R> result)
        {
            return () =>
            {
                Maybe<T> value = new Maybe<T>.None();
                return () =>
                    condition((value = new Maybe<T>.Some(
                        value is Maybe<T>.None
                        ? seed
                        : next(value.Value))).Value)
                    ? (Maybe<R>)new Maybe<R>.Some(result(value.Value))
                    : (Maybe<R>)new Maybe<R>.None();
            };
        }

        /// <summary>
        /// Bind.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="C">Selected sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="condition">Terminating condition for outer enumeration.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <param name="result">Result selector.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Bind<T, C, R>(this Func<Func<Maybe<T>>> source, Func<T, bool> condition, Func<T, Func<Func<Maybe<C>>>> selector, Func<T, C, R> result)
        {
            return () =>
            {
                var e = source();

                Maybe<T> lastOuter = new Maybe<T>.None();
                Maybe<C> lastInner = new Maybe<C>.None();
                Func<Maybe<C>> innerE = null;

                return () =>
                {
                    do
                    {
                        while (lastInner is Maybe<C>.None)
                        {
                            lastOuter = e();

                            if (lastOuter is Maybe<T>.None || !condition(lastOuter.Value))
                            {
                                return new Maybe<R>.None();
                            }
                            else
                            {
                                innerE = selector(lastOuter.Value)();
                            }

                            lastInner = innerE();
                            if (lastInner is Maybe<C>.Some)
                            {
                                return new Maybe<R>.Some(result(lastOuter.Value, lastInner.Value));
                            }
                        }

                        lastInner = innerE();
                    } while (lastInner is Maybe<C>.None);

                    return new Maybe<R>.Some(result(lastOuter.Value, lastInner.Value));
                };
            };
        }

        /// <summary>
        /// Catamorphism.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result object type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Seed value.</param>
        /// <param name="condition">Terminating condition.</param>
        /// <param name="f">Aggregator function.</param>
        /// <returns>Result of the catamorphic operation on the sequence.</returns>
        public static R Cata<T, R>(this Func<Func<Maybe<T>>> source, R seed, Func<R, bool> condition, Func<R, T, R> f)
        {
            var e = source();

            Maybe<T>.Some value;
            R result = seed;
            while (condition(result) && (value = e() as Maybe<T>.Some) != null)
            {
                result = f(result, value.Value);
            }

            return result;
        }
    }
}
