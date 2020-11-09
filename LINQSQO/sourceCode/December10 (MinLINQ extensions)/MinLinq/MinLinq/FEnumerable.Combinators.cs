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
using System.Collections.Generic;

namespace MinLinq
{
    /// <summary>
    /// FEnumerable combinator functions, based on Bind.
    /// </summary>
    public static partial class FEnumerable
    {
        #region Derived binds

        /// <summary>
        /// Bind.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="C">Selected sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <param name="result">Result selector.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Bind<T, C, R>(this Func<Func<Maybe<T>>> source, Func<T, Func<Func<Maybe<C>>>> selector, Func<T, C, R> result)
        {
            return Bind(source, _ => true, selector, result);
        }

        /// <summary>
        /// Bind.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Bind<T, R>(this Func<Func<Maybe<T>>> source, Func<T, Func<Func<Maybe<R>>>> selector)
        {
            return Bind(source, selector, (_, result) => result);
        }

        #endregion

        #region Derived anamorphisms

        /// <summary>
        /// Zips two sequences together.
        /// </summary>
        /// <typeparam name="T1">Left sequence element type.</typeparam>
        /// <typeparam name="T2">Right sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="left">Left sequence.</param>
        /// <param name="right">Right sequence.</param>
        /// <param name="selector">Zip function.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Zip<T1, T2, R>(this Func<Func<Maybe<T1>>> left, Func<Func<Maybe<T2>>> right, Func<T1, T2, R> selector)
        {
            return () => Ana(new { L = left(), R = right() }.Let(es => new { E = es, Lc = es.L(), Rc = es.R() }), c => !(c.Lc is Maybe<T1>.None) && !(c.Rc is Maybe<T2>.None), c => new { E = c.E, Lc = c.E.L(), Rc = c.E.R() }, c => selector(c.Lc.Value, c.Rc.Value))();
        }

        #endregion

        /// <summary>
        /// SelectMany is Bind in disguise.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> SelectMany<T, R>(this Func<Func<Maybe<T>>> source, Func<T, Func<Func<Maybe<R>>>> selector)
        {
            return Bind(source, selector);
        }

        /// <summary>
        /// SelectMany is Bind in disguise.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="C">Selected sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <param name="result">Result selector.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> SelectMany<T, C, R>(this Func<Func<Maybe<T>>> source, Func<T, Func<Func<Maybe<C>>>> selector, Func<T, C, R> result)
        {
            return Bind(source, selector, result);
        }

        /// <summary>
        /// SelectMany is Bind in disguise.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> SelectMany<T, R>(this Func<Func<Maybe<T>>> source, Func<T, int, Func<Func<Maybe<R>>>> selector)
        {
            return Bind(source.Zip(Naturals(), (t, i) => new { t, i }), p => selector(p.t, p.i));
        }

        /// <summary>
        /// SelectMany is Bind in disguise.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="C">Selected sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <param name="result">Result selector.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> SelectMany<T, C, R>(this Func<Func<Maybe<T>>> source, Func<T, int, Func<Func<Maybe<C>>>> selector, Func<T, C, R> result)
        {
            return Bind(source.Zip(Naturals(), (t, i) => new { t, i }), p => selector(p.t, p.i), (p, c) => result(p.t, c));
        }

        /// <summary>
        /// Filters elements in a sequence given a predicate.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to apply to source elements.</param>
        /// <returns>Filtered sequence.</returns>
        public static Func<Func<Maybe<T>>> Where<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return Where(source, (t, _) => predicate(t));
        }

        /// <summary>
        /// Filters elements in a sequence given a predicate.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to apply to source elements.</param>
        /// <returns>Filtered sequence.</returns>
        public static Func<Func<Maybe<T>>> Where<T>(this Func<Func<Maybe<T>>> source, Func<T, int, bool> predicate)
        {
            return source.Zip(Naturals(), (t, i) => new { t, i }).Bind(item => predicate(item.t, item.i) ? FEnumerable.Return(item.t) : FEnumerable.Empty<T>());
        }

        /// <summary>
        /// Projects elements in a sequence given a selector function.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to apply to source elements.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Select<T, R>(this Func<Func<Maybe<T>>> source, Func<T, R> selector)
        {
            return Select(source, (t, _) => selector(t));
        }

        /// <summary>
        /// Projects elements in a sequence given a selector function.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector to apply to source elements.</param>
        /// <returns>Result sequence.</returns>
        public static Func<Func<Maybe<R>>> Select<T, R>(this Func<Func<Maybe<T>>> source, Func<T, int, R> selector)
        {
            return source.Zip(Naturals(), (t, i) => new { t, i }).Bind(item => FEnumerable.Return(selector(item.t, item.i)));
        }

        /// <summary>
        /// Casts elements in a sequence to a given type.
        /// </summary>
        /// <typeparam name="R">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Result sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Func<Func<Maybe<R>>> Cast<R>(this Func<Func<Maybe<object>>> source)
        {
            return Select(source, item => (R)item);
        }

        /// <summary>
        /// Filters elements of a given type in a sequence.
        /// </summary>
        /// <typeparam name="R">Type to filter on.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Filtered sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Func<Func<Maybe<R>>> OfType<R>(this Func<Func<Maybe<object>>> source)
        {
            return Where(source, t => t is R).Select(t => (R)t);
        }

        /// <summary>
        /// Filters to retain distinct elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence with distinct elements.</returns>
        public static Func<Func<Maybe<T>>> Distinct<T>(this Func<Func<Maybe<T>>> source)
        {
            return Distinct(source, item => item);
        }

        /// <summary>
        /// Filters to retain distinct elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer to compare keys.</param>
        /// <returns>Sequence with distinct elements.</returns>
        public static Func<Func<Maybe<T>>> Distinct<T>(this Func<Func<Maybe<T>>> source, IEqualityComparer<T> comparer)
        {
            return Distinct(source, item => item, comparer);
        }

        /// <summary>
        /// Filters to retain distinct elements in the sequence, based on a given key selector.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Comparison key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Key selector.</param>
        /// <returns>Sequence with distinct elements.</returns>
        public static Func<Func<Maybe<T>>> Distinct<T, R>(this Func<Func<Maybe<T>>> source, Func<T, R> selector)
        {
            return Distinct(source, selector, EqualityComparer<R>.Default);
        }

        /// <summary>
        /// Filters to retain distinct elements in the sequence, based on a given key selector.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Comparison key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Key selector.</param>
        /// <param name="comparer">Comparer to compare keys.</param>
        /// <returns>Sequence with distinct elements.</returns>
        public static Func<Func<Maybe<T>>> Distinct<T, R>(this Func<Func<Maybe<T>>> source, Func<T, R> selector, IEqualityComparer<R> comparer)
        {
            return () =>
            {
                HashSet<R> seen = new HashSet<R>(comparer);
                return Where(source, item => seen.Add(selector(item)))();
            };
        }

        /// <summary>
        /// Concatenates the given sequences into a single flat sequence.
        /// </summary>
        /// <typeparam name="T">Source sequences element type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        /// <returns>Concatenated sequence.</returns>
        public static Func<Func<Maybe<T>>> Concat<T>(this Func<Func<Maybe<T>>> first, Func<Func<Maybe<T>>> second)
        {
            return Concat(new[] { first, second });
        }

        /// <summary>
        /// Concatenates the given sequences into a single flat sequence.
        /// </summary>
        /// <typeparam name="T">Source sequences element type.</typeparam>
        /// <param name="sources">Array of sequences.</param>
        /// <returns>Concatenated sequence.</returns>
        public static Func<Func<Maybe<T>>> Concat<T>(this Func<Func<Maybe<T>>>[] sources)
        {
            return Concat(sources.AsFEnumerable());
        }

        /// <summary>
        /// Concatenates the given sequences into a single flat sequence.
        /// </summary>
        /// <typeparam name="T">Source sequences element type.</typeparam>
        /// <param name="sources">Sequence of source sequences.</param>
        /// <returns>Concatenated sequence.</returns>
        public static Func<Func<Maybe<T>>> Concat<T>(this Func<Func<Maybe<Func<Func<Maybe<T>>>>>> sources)
        {
            return Bind(sources, source => source);
        }

        /// <summary>
        /// Skips a number of elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to skip.</param>
        /// <returns>Sequence after skipping elements.</returns>
        public static Func<Func<Maybe<T>>> Skip<T>(this Func<Func<Maybe<T>>> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Where(source, (_, i) => i >= count);
        }

        /// <summary>
        /// Skips the elements in the sequence that pass the predicate.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate for elements to skip.</param>
        /// <returns>Sequence after skipping elements.</returns>
        public static Func<Func<Maybe<T>>> SkipWhile<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return () =>
            {
                bool yield = false;
                return Where(source, t => yield || (yield = !predicate(t)))();
            };
        }

        /// <summary>
        /// Takes a number of elements from the start of the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to take.</param>
        /// <returns>Sequence of the elements taken.</returns>
        public static Func<Func<Maybe<T>>> Take<T>(this Func<Func<Maybe<T>>> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Bind(source.Zip(Naturals(), (t, i) => new { t, i }), p => p.i < count, p => FEnumerable.Return(p.t), (_, c) => c);
        }

        /// <summary>
        /// Takes the elements from the start of the sequence that pass the predicate.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate for elements to take.</param>
        /// <returns>Sequence of the elements taken.</returns>
        public static Func<Func<Maybe<T>>> TakeWhile<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return Bind(source, t => predicate(t), t => FEnumerable.Return(t), (_, t) => t);
        }

        /// <summary>
        /// Reverses the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence in reverse.</returns>
        public static Func<Func<Maybe<T>>> Reverse<T>(this Func<Func<Maybe<T>>> source)
        {
            return source.Cata(Empty<T>(), (r, t) => r.Snoc(t));
        }

        /// <summary>
        /// Cons in reverse.
        /// </summary>
        /// <typeparam name="T">Sequence element type.</typeparam>
        /// <param name="tail">Tail sequence.</param>
        /// <param name="head">Head element.</param>
        /// <returns>Concatenation of the head element with the tail sequence.</returns>
        public static Func<Func<Maybe<T>>> Snoc<T>(this Func<Func<Maybe<T>>> tail, T head)
        {
            return FEnumerable.Return(head).Concat(tail);
        }
    }
}
