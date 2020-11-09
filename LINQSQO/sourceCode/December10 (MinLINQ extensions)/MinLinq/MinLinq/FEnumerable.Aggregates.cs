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
    /// FEnumerable catamorphic aggregation functions.
    /// </summary>
    public static partial class FEnumerable
    {
        #region Derived catamorphisms

        /// <summary>
        /// Catamorphism.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result object type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Seed value.</param>
        /// <param name="f">Aggregator function.</param>
        /// <returns>Result of the catamorphic operation on the sequence.</returns>
        public static R Cata<T, R>(this Func<Func<Maybe<T>>> source, R seed, Func<R, T, R> f)
        {
            return Cata(source, seed, _ => true, f);
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
        public static Maybe<R> CataOrDefault<T, R>(this Func<Func<Maybe<T>>> source, R seed, Func<R, bool> condition, Func<R, T, R> f)
        {
            var result = Cata(source, new { HasResult = false, Value = seed }, res => condition(res.Value), (res, t) => new { HasResult = true, Value = f(res.Value, t) });
            return !result.HasResult ? (Maybe<R>)new Maybe<R>.None() : new Maybe<R>.Some(result.Value);
        }

        /// <summary>
        /// Catamorphism.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result object type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Seed value.</param>
        /// <param name="f">Aggregator function.</param>
        /// <returns>Result of the catamorphic operation on the sequence.</returns>
        public static Maybe<R> CataOrDefault<T, R>(this Func<Func<Maybe<T>>> source, R seed, Func<R, T, R> f)
        {
            return CataOrDefault(source, seed, _ => true, f);
        }

        #endregion

        /// <summary>
        /// Aggregate is a catamorphism.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="R">Result object type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Seed value.</param>
        /// <param name="func">Aggregator function.</param>
        /// <returns>Result of the catamorphic operation on the sequence.</returns>
        public static R Aggregate<T, R>(this Func<Func<Maybe<T>>> source, R seed, Func<R, T, R> func)
        {
            return Cata(source, seed, func);
        }

        /// <summary>
        /// Aggregate is a catamorphism.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="A">Accumulator object type.</typeparam>
        /// <typeparam name="R">Result object type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Seed value.</param>
        /// <param name="func">Aggregator function.</param>
        /// <param name="result">Result function.</param>
        /// <returns>Result of the catamorphic operation on the sequence.</returns>
        public static R Aggregate<T, A, R>(this Func<Func<Maybe<T>>> source, A seed, Func<A, T, A> func, Func<A, R> result)
        {
            return result(Aggregate(source, seed, func));
        }

        /// <summary>
        /// Counts the number of elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Number of elements in the sequence.</returns>
        public static int Count<T>(this Func<Func<Maybe<T>>> source)
        {
            return Cata(source, 0, (n, _) => checked(n + 1));
        }

        /// <summary>
        /// Counts the number of elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Number of elements in the sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long")]
        public static long LongCount<T>(this Func<Func<Maybe<T>>> source)
        {
            return Cata(source, 0L, (n, _) => checked(n + 1));
        }

        /// <summary>
        /// Checks the predicate holds for all elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on every element.</param>
        /// <returns>true if all elements pass the predicate; false otherwise.</returns>
        public static bool All<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return Cata(source, true, all => all, (all, item) => all && predicate(item));
        }

        /// <summary>
        /// Checks the predicate holds any of the elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on every element.</param>
        /// <returns>true if any of the elements passes the predicate; false otherwise.</returns>
        public static bool Any<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return Cata(source, false, any => !any, (any, item) => any || predicate(item));
        }

        /// <summary>
        /// Checks whether the specified element is present in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="value">Element to check for.</param>
        /// <returns>true if the element is in the sequence; false otherwise.</returns>
        public static bool Contains<T>(this Func<Func<Maybe<T>>> source, T value)
        {
            return Contains(source, value, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Checks whether the specified element is present in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="value">Element to check for.</param>
        /// <param name="comparer">Comparer to compare elements.</param>
        /// <returns>true if the element is in the sequence; false otherwise.</returns>
        public static bool Contains<T>(this Func<Func<Maybe<T>>> source, T value, IEqualityComparer<T> comparer)
        {
            return Any(source, item => comparer.Equals(item, value));
        }

        /// <summary>
        /// Computes the sum of the elements in the sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sum of the elements in the sequence.</returns>
        public static int Sum(this Func<Func<Maybe<int>>> source)
        {
            return Sum(source, x => x);
        }

        /// <summary>
        /// Computes the sum of values computed from the elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Sum of values computed from the elements in the sequence.</returns>
        public static int Sum<T>(this Func<Func<Maybe<T>>> source, Func<T, int> selector)
        {
            return Cata(source, 0, (sum, item) => checked(sum + selector(item)));
        }

        /// <summary>
        /// Computes the average of the elements in the sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>Average of the elements in the sequence.</returns>
        public static double Average(this Func<Func<Maybe<int>>> source)
        {
            return Average(source, x => x);
        }

        /// <summary>
        /// Computes the average of values computed from the elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Average of values computed from the elements in the sequence.</returns>
        public static double Average<T>(this Func<Func<Maybe<T>>> source, Func<T, int> selector)
        {
            return
                Cata(source, new { Count = 0, Sum = 0L }, _ => true, (avg, item) => new { Count = checked(avg.Count + 1), Sum = checked(avg.Sum + selector(item)) })
                .Let(average => average.Count == 0 ? 0.0 : (double)average.Sum / average.Count);
        }

        /// <summary>
        /// Computes the minimum of the elements in the sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>Minimum of the elements in the sequence.</returns>
        public static Maybe<int> Min(this Func<Func<Maybe<int>>> source)
        {
            return Min(source, x => x);
        }

        /// <summary>
        /// Computes the maximum of values computed from the elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Maximum of values computed from the elements in the sequence.</returns>
        public static Maybe<int> Min<T>(this Func<Func<Maybe<T>>> source, Func<T, int> selector)
        {
            return CataOrDefault(source, int.MaxValue, (min, x) => Math.Min(min, selector(x)));
        }

        /// <summary>
        /// Computes the maximum of the elements in the sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>Minimum of the elements in the sequence.</returns>
        public static Maybe<int> Max(this Func<Func<Maybe<int>>> source)
        {
            return Max(source, x => x);
        }

        /// <summary>
        /// Computes the minimum of values computed from the elements in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector for source elements.</param>
        /// <returns>Maximum of values computed from the elements in the sequence.</returns>
        public static Maybe<int> Max<T>(this Func<Func<Maybe<T>>> source, Func<T, int> selector)
        {
            return CataOrDefault(source, int.MinValue, (max, x) => Math.Max(max, selector(x)));
        }

        /// <summary>
        /// Gets the first element in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>First element in the sequence.</returns>
        public static T First<T>(this Func<Func<Maybe<T>>> source)
        {
            return First(source, item => true);
        }

        /// <summary>
        /// Gets the first element in the sequence for which the predicate holds.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>First element in the sequence for which the predicate holds.</returns>
        public static T First<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                FirstOrNone(source, predicate)
                .Let(first => first.Value);
        }

        /// <summary>
        /// Gets the first element in the sequence, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>First element in the sequence, or default(T) if none is found.</returns>
        public static T FirstOrDefault<T>(this Func<Func<Maybe<T>>> source)
        {
            return FirstOrDefault(source, _ => true);
        }

        /// <summary>
        /// Gets the first element in the sequence for which the predicate holds, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>First element in the sequence for which the predicate holds, or default(T) if none is found.</returns>
        public static T FirstOrDefault<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                FirstOrNone(source, predicate)
                .Let(res => res.HasValue ? res.Value : default(T));
        }

        /// <summary>
        /// Gets the first element in the sequence, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>First element in the sequence, or None if none is found.</returns>
        public static Maybe<T> FirstOrNone<T>(this Func<Func<Maybe<T>>> source)
        {
            return FirstOrNone(source, _ => true);
        }

        /// <summary>
        /// Gets the first element in the sequence for which the predicate holds, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>First element in the sequence for which the predicate holds, or None if none is found.</returns>
        public static Maybe<T> FirstOrNone<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                Cata(source, new { Break = false, Value = default(T) }, fst => !fst.Break, (fst, t) => predicate(t) ? new { Break = true, Value = t } : fst)
                .Let(first => first.Break ? (Maybe<T>)new Maybe<T>.Some(first.Value) : new Maybe<T>.None());
        }

        /// <summary>
        /// Gets the last element in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Last element in the sequence.</returns>
        public static T Last<T>(this Func<Func<Maybe<T>>> source)
        {
            return Last(source, item => true);
        }

        /// <summary>
        /// Gets the last element in the sequence for which the predicate holds.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Last element in the sequence for which the predicate holds.</returns>
        public static T Last<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                LastOrNone(source, predicate)
                .Let(last => last.Value);
        }

        /// <summary>
        /// Gets the last element in the sequence, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Last element in the sequence, or default(T) if none is found.</returns>
        public static T LastOrDefault<T>(this Func<Func<Maybe<T>>> source)
        {
            return LastOrDefault(source, _ => true);
        }

        /// <summary>
        /// Gets the last element in the sequence for which the predicate holds, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Last element in the sequence for which the predicate holds, or default(T) if none is found.</returns>
        public static T LastOrDefault<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                LastOrNone(source, predicate)
                .Let(res => res.HasValue ? res.Value : default(T));
        }

        /// <summary>
        /// Gets the last element in the sequence, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Last element in the sequence, or None if none is found.</returns>
        public static Maybe<T> LastOrNone<T>(this Func<Func<Maybe<T>>> source)
        {
            return LastOrNone(source, _ => true);
        }

        /// <summary>
        /// Gets the last element in the sequence for which the predicate holds, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Last element in the sequence for which the predicate holds, or None if none is found.</returns>
        public static Maybe<T> LastOrNone<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                Cata(source, new { Found = false, Value = default(T) }, (lst, t) => predicate(t) ? new { Found = true, Value = t } : lst)
                .Let(last => last.Found ? (Maybe<T>)new Maybe<T>.Some(last.Value) : new Maybe<T>.None());
        }

        /// <summary>
        /// Gets the single element in the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Single element in the sequence.</returns>
        public static T Single<T>(this Func<Func<Maybe<T>>> source)
        {
            return Single(source, item => true);
        }

        /// <summary>
        /// Gets the single element in the sequence for which the predicate holds.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Single element in the sequence for which the predicate holds.</returns>
        public static T Single<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                SingleOrNone(source, predicate)
                .Let(single => single.Value);
        }

        /// <summary>
        /// Gets the single element in the sequence, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Single element in the sequence, or default(T) if none is found.</returns>
        public static T SingleOrDefault<T>(this Func<Func<Maybe<T>>> source)
        {
            return SingleOrDefault(source, _ => true);
        }

        /// <summary>
        /// Gets the single element in the sequence for which the predicate holds, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Single element in the sequence for which the predicate holds, or default(T) if none is found.</returns>
        public static T SingleOrDefault<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                SingleOrNone(source, predicate)
                .Let(res => res.HasValue ? res.Value : default(T));
        }

        /// <summary>
        /// Gets the single element in the sequence, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Single element in the sequence, or None if none is found.</returns>
        public static Maybe<T> SingleOrNone<T>(this Func<Func<Maybe<T>>> source)
        {
            return SingleOrNone(source, _ => true);
        }

        /// <summary>
        /// Gets the single element in the sequence for which the predicate holds, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to evaluate on elements.</param>
        /// <returns>Single element in the sequence for which the predicate holds, or None if none is found.</returns>
        public static Maybe<T> SingleOrNone<T>(this Func<Func<Maybe<T>>> source, Func<T, bool> predicate)
        {
            return
                Cata(source, new { Count = 0, Value = default(T) }, sng => sng.Count < 2, (sng, t) => predicate(t) ? new { Count = sng.Count + 1, Value = t } : sng)
                .Let(single => single.Count == 1 ? (Maybe<T>)new Maybe<T>.Some(single.Value) : new Maybe<T>.None());
        }

        /// <summary>
        /// Gets the element in the sequence at the given index.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="index">Index of the element to retrieve</param>
        /// <returns>Element in the sequence at the given index.</returns>
        public static T ElementAt<T>(this Func<Func<Maybe<T>>> source, int index)
        {
            return
                ElementAtOrNone(source, index)
                .Let(element => element.Value);
        }

        /// <summary>
        /// Gets the element in the sequence at the given index, or default(T) if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="index">Index of the element to retrieve</param>
        /// <returns>Element in the sequence at the given index, or default(T) if none is found.</returns>
        public static T ElementAtOrDefault<T>(this Func<Func<Maybe<T>>> source, int index)
        {
            return
                ElementAtOrNone(source, index)
                .Let(res => res.HasValue ? res.Value : default(T));
        }

        /// <summary>
        /// Gets the element in the sequence at the given index, or None if none is found.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="index">Index of the element to retrieve</param>
        /// <returns>Element in the sequence at the given index, or None if none is found.</returns>
        public static Maybe<T> ElementAtOrNone<T>(this Func<Func<Maybe<T>>> source, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            return
                Cata(source, new { LastIndex = 0, Value = default(T) }, elem => elem.LastIndex <= index, (elem, t) => elem.LastIndex == index ? new { LastIndex = index + 1, Value = t } : new { LastIndex = elem.LastIndex + 1, Value = default(T) })
                .Let(element => element.LastIndex > index ? (Maybe<T>)new Maybe<T>.Some(element.Value) : new Maybe<T>.None());
        }

        /// <summary>
        /// Runs the action on all elements of the sequence.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to run on all elements.</param>
        public static void Run<T>(this Func<Func<Maybe<T>>> source, Action<T> action)
        {
            Cata(source, 0, (_, t) => { action(t); return _; });
        }

        /// <summary>
        /// Coverts the sequence in a list.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>List with the elements of the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<T> ToList<T>(this Func<Func<Maybe<T>>> source)
        {
            return Cata(source, new List<T>(), (res, t) => res.With(lst => lst.Add(t)));
        }

        /// <summary>
        /// Coverts the sequence in an array.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Array with the elements of the source sequence.</returns>
        public static T[] ToArray<T>(this Func<Func<Maybe<T>>> source)
        {
            return ToList(source).ToArray();
        }
    }
}
