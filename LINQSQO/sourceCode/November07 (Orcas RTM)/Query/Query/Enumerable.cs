#define CS30

using System;
using System.Collections;
using System.Collections.Generic;

/*=======================================================================
  Custom implementation of the LINQ Standard Query Operators.

  Bart De Smet - November 07
=======================================================================*/

namespace BdsSoft
{
    namespace Linq
    {
        #region 1.1 The Func delegate types

        /// <summary>
        /// Generic delegate with 0 parameters useful to construct delegate types on the fly, e.g. for implicit usage in lambda expressions.
        /// </summary>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <returns></returns>
        public delegate TResult Func<TResult>();

        /// <summary>
        /// Generic delegate with 1 parameter useful to construct delegate types on the fly, e.g. for implicit usage in lambda expressions.
        /// </summary>
        /// <typeparam name="TArg0">Type of the first parameter.</typeparam>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <param name="a0">Dummy variable to denote the first parameter.</param>
        /// <returns></returns>
        public delegate TResult Func<TArg0, TResult>(TArg0 a0);

        /// <summary>
        /// Generic delegate with 2 parameters useful to construct delegate types on the fly, e.g. for implicit usage in lambda expressions.
        /// </summary>
        /// <typeparam name="TArg0">Type of the first parameter.</typeparam>
        /// <typeparam name="TArg1">Type of the second parameter.</typeparam>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <param name="a0">Dummy variable to denote the first parameter.</param>
        /// <param name="a1">Dummy variable to denote the second parameter.</param>
        /// <returns></returns>
        public delegate TResult Func<TArg0, TArg1, TResult>(TArg0 a0, TArg1 a1);

        /// <summary>
        /// Generic delegate with 3 parameters useful to construct delegate types on the fly, e.g. for implicit usage in lambda expressions.
        /// </summary>
        /// <typeparam name="TArg0">Type of the first parameter.</typeparam>
        /// <typeparam name="TArg1">Type of the second parameter.</typeparam>
        /// <typeparam name="TArg2">Type of the third parameter.</typeparam>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <param name="a0">Dummy variable to denote the first parameter.</param>
        /// <param name="a1">Dummy variable to denote the second parameter.</param>
        /// <param name="a2">Dummy variable to denote the third parameter.</param>
        /// <returns></returns>
        public delegate TResult Func<TArg0, TArg1, TArg2, TResult>(TArg0 a0, TArg1 a1, TArg2 a2);

        /// <summary>
        /// Generic delegate with 4 parameters useful to construct delegate types on the fly, e.g. for implicit usage in lambda expressions.
        /// </summary>
        /// <typeparam name="TArg0">Type of the first parameter.</typeparam>
        /// <typeparam name="TArg1">Type of the second parameter.</typeparam>
        /// <typeparam name="TArg2">Type of the third parameter.</typeparam>
        /// <typeparam name="TArg3">Type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <param name="a0">Dummy variable to denote the first parameter.</param>
        /// <param name="a1">Dummy variable to denote the second parameter.</param>
        /// <param name="a2">Dummy variable to denote the third parameter.</param>
        /// <param name="a3">Dummy variable to denote the fourth parameter.</param>
        /// <returns></returns>
        public delegate TResult Func<TArg0, TArg1, TArg2, TArg3, TResult>(TArg0 a0, TArg1 a1, TArg2 a2, TArg3 a3);

        #endregion

        #region 1.2 The Enumerable class

        /// <summary>
        /// Static class with definitions of the LINQ Standard Query Operators. The majority of the Standard Query Operators are extension methods that extend <c>IEnumerable&lt;T&gt;</c>. Taken together, the methods compose to form a complete query language for arrays and collections that implement <c>IEnumerable&lt;T&gt;</c>.
        /// </summary>
        public static class Enumerable
        {
            #region 1.3 Restriction operators

            #region 1.3.1 Where

            /// <summary>
            /// Filters a sequence based on a predicate. When the returned sequence is enumerated, it enumerates the source sequence and yields those elements for which the <paramref name="predicate">predicate</paramref> function returns true.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to filter.</param>
            /// <param name="predicate">Predicate to filter elements of the source Enumerable. The first argument of the predicate function represents the element to test.</param>
            /// <returns>Filtered Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Where<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _Where(source, predicate);
            }

            private static IEnumerable<TSource> _Where<TSource>(
                IEnumerable<TSource> source, 
                Func<TSource, bool> predicate)
            {
                foreach (TSource item in source)
                    if (predicate(item))
                        yield return item;
            }

            /// <summary>
            /// Filters a sequence based on a predicate. When the returned sequence is enumerated, it enumerates the source sequence and yields those elements for which the predicate function returns true.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to filter.</param>
            /// <param name="predicate">Predicate to filter elements of the source Enumerable. The first argument of the predicate function represents the element to test. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <returns>Filtered Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Where<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _Where(source, predicate);
            }

            private static IEnumerable<TSource> _Where<TSource>(
                IEnumerable<TSource> source, 
                Func<TSource, int, bool> predicate)
            {
                int i = 0;
                foreach (TSource item in source)
                    if (predicate(item, i++))
                        yield return item;
            }

            #endregion

            #endregion

            #region 1.4 Projection operators

            #region 1.4.1 Select

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When the returned sequence is enumerated, it enumerates the source sequence and yields the results of evaluating the <paramref name="selector">selector</paramref> function for each element.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="selector">Selector function to generate projections for the elements in the source Enumerable. The first argument of the selector function represents the element to process.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> Select<TSource, TResult>(
#if CS30
this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TResult> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                return _Select(source, selector);
            }

            private static IEnumerable<TResult> _Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                foreach (TSource item in source)
                    yield return selector(item);
            }

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When returned sequence is enumerated, it enumerates the source sequence and yields the results of evaluating the <paramref name="selector">selector</paramref> function for each element.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="selector">Selector function to generate projections for the elements in the source Enumerable. The first argument of the selector function represents the element to process. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> Select<TSource, TResult>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, TResult> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                return _Select(source, selector);
            }

            private static IEnumerable<TResult> _Select<TSource, TResult>(
                IEnumerable<TSource> source, 
                Func<TSource, int, TResult> selector)
            {
                int i = 0;
                foreach (TSource item in source)
                    yield return selector(item, i++);
            }

            #endregion

            #region 1.4.2 SelectMany

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When the returned sequence is enumerated, it enumerates the source sequence, maps each element to an enumerable object using the <paramref name="selector">selector</paramref> function, and enumerates and yields the elements of each such enumerable object.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="selector">Selector function to generate projection sequences for the elements in the source Enumerable. The first argument of the selector function represents the element to process.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> SelectMany<TSource, TResult>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, IEnumerable<TResult>> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                return _SelectMany(source, selector);
            }

            private static IEnumerable<TResult> _SelectMany<TSource, TResult>(
                IEnumerable<TSource> source, 
                Func<TSource, IEnumerable<TResult>> selector)
            {
                foreach (TSource item in source)
                    foreach (TResult child in selector(item))
                        yield return child;
            }

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When the returned sequence is enumerated, it enumerates the source sequence, maps each element to an enumerable object using the <paramref name="selector">selector</paramref> function, and enumerates and yields the elements of each such enumerable object.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="selector">Selector function to generate projection sequences for the elements in the source Enumerable. The first argument of the selector function represents the element to process. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> SelectMany<TSource, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, IEnumerable<TResult>> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                return _SelectMany(source, selector);
            }

            private static IEnumerable<TResult> _SelectMany<TSource, TResult>(
                IEnumerable<TSource> source, 
                Func<TSource, int, IEnumerable<TResult>> selector)
            {
                int i = 0;
                foreach (TSource item in source)
                    foreach (TResult child in selector(item, i++))
                        yield return child;
            }

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When the returned sequence is enumerated, it enumerates the source sequence, maps each element to an enumerable object using the <paramref name="selector">selector</paramref> function, and enumerates and yields the elements of each such enumerable object.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <typeparam name="TCollection">Type of the collection to flatten items for.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="collectionSelector">Selector function to generate collection sequences for the elements in the source Enumerable.</param>
            /// <param name="resultSelector">Selector function to generate projection sequences for the elements obtained from the <paramref name="collectionSelector">collection selector</paramref>. The first argument of the selector function represents the element to process. The second argument represents the collection item retrieved via the <paramref name="collectionSelector">collection selector</paramref>.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, IEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
            {
                if (source == null || collectionSelector == null || resultSelector == null)
                    throw new ArgumentNullException();

                return _SelectMany(source, collectionSelector, resultSelector);
            }

            private static IEnumerable<TResult> _SelectMany<TSource, TCollection, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, IEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
            {
                foreach (TSource item in source)
                    foreach (TCollection coll in collectionSelector(item))
                        yield return resultSelector(item, coll);
            }

            /// <summary>
            /// Performs a projection over a sequence with a one-to-many source-projection mapping. When the returned sequence is enumerated, it enumerates the source sequence, maps each element to an enumerable object using the <paramref name="selector">selector</paramref> function, and enumerates and yields the elements of each such enumerable object.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the projection results.</typeparam>
            /// <typeparam name="TCollection">Type of the collection to flatten items for.</typeparam>
            /// <param name="source">Sequence to perform the projection over.</param>
            /// <param name="collectionSelector">Selector function to generate collection sequences for the elements in the source Enumerable. The first argument of the selector function represents the element to process. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <param name="resultSelector">Selector function to generate projection sequences for the elements obtained from the <paramref name="collectionSelector">collection selector</paramref>. The first argument of the selector function represents the element to process. The second argument represents the collection item retrieved via the <paramref name="collectionSelector">collection selector</paramref>.</param>
            /// <returns>Sequence with the projected elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
            {
                if (source == null || collectionSelector == null || resultSelector == null)
                    throw new ArgumentNullException();

                return _SelectMany(source, collectionSelector, resultSelector);
            }

            private static IEnumerable<TResult> _SelectMany<TSource, TCollection, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
            {
                int i = 0;
                foreach (TSource item in source)
                    foreach (TCollection coll in collectionSelector(item, i++))
                        yield return resultSelector(item, coll);
            }

            #endregion

            #endregion

            #region 1.5 Partitioning operators

            #region 1.5.1 Take

            /// <summary>
            /// Yields a given number of elements from a sequence and then skips the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence and yields elements until the number of elements given by the <paramref name="count">count</paramref> argument have been yielded or the end of the source is reached. If the <paramref name="count">count</paramref> argument is less than or equal to zero, the source sequence is not enumerated and no elements are yielded.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="count">Number of elements to yield from the source Enumerable.</param>
            /// <returns>Sequence with the first <paramref name="count">count</paramref> elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>Take</c> and <c>Skip</c> operators are functional complements: For a given sequence <c>s</c>, the concatenation of <c>s.Take(n)</c> and <c>s.Skip(n)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> Take<TSource>(
#if CS30
this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 int count)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _Take(source, count);
            }

            private static IEnumerable<TSource> _Take<TSource>(
                IEnumerable<TSource> source,
                int count)
            {
                IEnumerator<TSource> enumerator = source.GetEnumerator();
                for (int i = 0; i < count && enumerator.MoveNext(); i++)
                    yield return enumerator.Current;
            }

            #endregion

            #region 1.5.2 Skip

            /// <summary>
            /// Skips a given number of elements from a sequence and then yields the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, skipping the number of elements given by the <paramref name="count">count</paramref> argument and yielding the rest. If the source sequence contains fewer elements than given by the <paramref name="count">count</paramref> argument, nothing is yielded. If the <paramref name="count">count</paramref> argument is less an or equal to zero, all elements of the source sequence are yielded.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="count">Number of elements to skip before yielding from the source Enumerable.</param>
            /// <returns>Sequence with the remaining elements after skipping <paramref name="count">count</paramref> elements.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>Take</c> and <c>Skip</c> operators are functional complements: For a given sequence <c>s</c>, the concatenation of <c>s.Take(n)</c> and <c>s.Skip(n)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> Skip<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 int count)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _Skip(source, count);
            }

            private static IEnumerable<TSource> _Skip<TSource>(
                IEnumerable<TSource> source,
                int count)
            {
                IEnumerator<TSource> enumerator = source.GetEnumerator();
                for (int i = 0; i < count && enumerator.MoveNext(); i++)
                    ;

                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }

            #endregion

            #region 1.5.3 TakeWhile

            /// <summary>
            /// Yields elements from a sequence while a test is true and then skips the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, testing each element using the <paramref name="predicate">predicate</paramref> function and yielding the element if the result was true. The enumeration stops when the <paramref name="predicate">predicate</paramref> function returns false or the end of the source sequence is reached.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="predicate">Predicate to decide when to stop yielding (on first return of <c>false</c>) from the source Enumerable. The first argument of the predicate function represents the element to test.</param>
            /// <returns>Head of the source sequence, ending with the last element for which the <paramref name="predicate">predicate</paramref> function evaluates to true.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>TakeWhile</c> and <c>SkipWhile</c> operators are functional complements: For a given sequence <c>s</c> and a predicate function <c>p</c>, the concatenation of <c>s.TakeWhile(p)</c> and <c>s.SkipWhile(p)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> TakeWhile<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _TakeWhile(source, predicate);
            }

            private static IEnumerable<TSource> _TakeWhile<TSource>(
                IEnumerable<TSource> source,
                Func<TSource, bool> predicate)
            {
                foreach (TSource item in source)
                    if (predicate(item))
                        yield return item;
                    else
                        break;
            }

            /// <summary>
            /// Yields elements from a sequence while a test is true and then skips the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, testing each element using the <paramref name="predicate">predicate</paramref> function and yielding the element if the result was true. The enumeration stops when the <paramref name="predicate">predicate</paramref> function returns false or the end of the source sequence is reached.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="predicate">Predicate to decide when to stop yielding (on first return of <c>false</c>) from the source Enumerable. The first argument of the predicate function represents the element to test. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <returns>Head of the source sequence, ending with the last element for which the <paramref name="predicate">predicate</paramref> function evaluates to true.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>TakeWhile</c> and <c>SkipWhile</c> operators are functional complements: For a given sequence <c>s</c> and a predicate function <c>p</c>, the concatenation of <c>s.TakeWhile(p)</c> and <c>s.SkipWhile(p)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> TakeWhile<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _TakeWhile(source, predicate);
            }

            private static IEnumerable<TSource> _TakeWhile<TSource>(
                IEnumerable<TSource> source,
                Func<TSource, int, bool> predicate)
            {
                int i = 0;
                foreach (TSource item in source)
                    if (predicate(item, i++))
                        yield return item;
                    else
                        break;
            }

            #endregion

            #region 1.5.4 SkipWhile

            /// <summary>
            /// Skips elements from a sequence while a test is true and then yields the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, testing each element using the <paramref name="predicate">predicate</paramref> function and skipping the element if the result was true. Once the predicate function returns false for an element, that element and the remaining elements are yielded with no further invocations of the <paramref name="predicate">predicate</paramref> function. If the <paramref name="predicate">predicate</paramref> function returns true for all elements in the sequence, no elements are yielded.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="predicate">Predicate to decide when to start yielding (on first return of <c>false</c>) from the source Enumerable. The first argument of the predicate function represents the element to test.</param>
            /// <returns>Tail of the source sequence, starting with the first element for which the <paramref name="predicate">predicate</paramref> function evaluates to false.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>TakeWhile</c> and <c>SkipWhile</c> operators are functional complements: For a given sequence <c>s</c> and a predicate function <c>p</c>, the concatenation of <c>s.TakeWhile(p)</c> and <c>s.SkipWhile(p)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> SkipWhile<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _SkipWhile(source, predicate);
            }

            private static IEnumerable<TSource> _SkipWhile<TSource>(
                IEnumerable<TSource> source,
                Func<TSource, bool> predicate)
            {
                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    do
                    {
                        if (!predicate(enumerator.Current))
                        {
                            yield return enumerator.Current;
                            break;
                        }
                    } while (enumerator.MoveNext());

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
            }

            /// <summary>
            /// Skips elements from a sequence while a test is true and then yields the remainder of the Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, testing each element using the <paramref name="predicate">predicate</paramref> function and skipping the element if the result was true. Once the predicate function returns false for an element, that element and the remaining elements are yielded with no further invocations of the <paramref name="predicate">predicate</paramref> function. If the <paramref name="predicate">predicate</paramref> function returns true for all elements in the sequence, no elements are yielded.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to yield elements from.</param>
            /// <param name="predicate">Predicate to decide when to start yielding (on first return of <c>false</c>) from the source Enumerable. The first argument of the predicate function represents the element to test. The second argument represents the zero based index of the element within the source Enumerable.</param>
            /// <returns>Tail of the source sequence, starting with the first element for which the <paramref name="predicate">predicate</paramref> function evaluates to false.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>TakeWhile</c> and <c>SkipWhile</c> operators are functional complements: For a given sequence <c>s</c> and a predicate function <c>p</c>, the concatenation of <c>s.TakeWhile(p)</c> and <c>s.SkipWhile(p)</c> yields the same sequence as <c>s</c>.</remarks>
            public static IEnumerable<TSource> SkipWhile<TSource>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                return _SkipWhile(source, predicate);
            }

            private static IEnumerable<TSource> _SkipWhile<TSource>(
                IEnumerable<TSource> source,
                Func<TSource, int, bool> predicate)
            {
                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    int i = 0;
                    do
                    {
                        if (!predicate(enumerator.Current, i++))
                        {
                            yield return enumerator.Current;
                            break;
                        }
                    } while (enumerator.MoveNext());

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
            }

            #endregion

            #endregion

            #region 1.6 Join operators

            #region 1.6.1 Join

            /// <summary>
            /// Performs an inner join of two sequences based on matching keys extracted from the elements. When the returned sequence is enumerated, it first enumerates the <paramref name="inner">inner</paramref> sequence and evaluates the <paramref name="innerKeySelector">innerKeySelector</paramref> function once for each inner element, collecting the elements by their keys in a hash table. Once all inner elements and keys have been collected, the <paramref name="outer">outer</paramref> sequence is enumerated. For each outer element, the <paramref name="outerKeySelector">outerKeySelector</paramref> function is evaluated and the resulting key is used to look up the corresponding inner elements in the hash table. For each matching inner element (if any), the <paramref name="resultSelector">resultSelector</paramref> function is evaluated for the outer and inner element pair, and the resulting object is yielded.
            /// </summary>
            /// <typeparam name="TOuter">Type of the elements in the outer source Enumerable.</typeparam>
            /// <typeparam name="TInner">Type of the elements in the inner source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key field to perform the join on.</typeparam>
            /// <typeparam name="TElement">Type of the elements in the result Enumerable.</typeparam>
            /// <param name="outer">Outer sequence to perform the join with.</param>
            /// <param name="inner">Inner sequence to perform the join with.</param>
            /// <param name="outerKeySelector">Function that extracts the join key values from elements of the <paramref name="outer">outer</paramref> Enumerable.</param>
            /// <param name="innerKeySelector">Function that extracts the join key values from elements of the <paramref name="inner">inner</paramref> Enumerable.</param>
            /// <param name="resultSelector">Function that generates a join result of type <typeparamref name="TElement">TElement</typeparamref> based on a pair of one outer element (type <typeparamref name="TOuter">TOuter</typeparamref>) and one inner element (type <typeparamref name="TInner">TInner</typeparamref>).</param>
            /// <returns>Sequence with elements resulting from a join operation on the outer and inner sequence based on a specified key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TOuter">TOuter</typeparamref>&gt;. The <c>Join</c> operator preserves the order of the <paramref name="outer">outer</paramref> sequence elements, and for each outer element, the order of the matching <paramref name="inner">inner</paramref> sequence elements. In relational database terms, the <c>Join</c> operator implements an inner equijoin. Other join operations, such as left outer join and right outer join have no dedicated standard query operators, but are subsets of the capabilities of the <see cref="GroupJoin&lt;TOuter, TInner, TKey, TResult&gt;(IEnumerable&lt;TOuter&gt;, IEnumerable&lt;TInner&gt;, Func&lt;TOuter, TKey&gt;, Func&lt;TInner, TKey&gt;, Func&lt;TOuter, IEnumerable&lt;TInner&gt;, TResult&gt;)">GroupJoin</see> operator.</remarks>
            public static IEnumerable<TElement> Join<TOuter, TInner, TKey, TElement>(
#if CS30
                this IEnumerable<TOuter> outer,
#else
IEnumerable<TOuter> outer,
#endif
 IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, TInner, TElement> resultSelector)
            {
                return Join<TOuter, TInner, TKey, TElement>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Performs an inner join of two sequences based on matching keys extracted from the elements. When the returned sequence is enumerated, it first enumerates the <paramref name="inner">inner</paramref> sequence and evaluates the <paramref name="innerKeySelector">innerKeySelector</paramref> function once for each inner element, collecting the elements by their keys in a hash table. Once all inner elements and keys have been collected, the <paramref name="outer">outer</paramref> sequence is enumerated. For each outer element, the <paramref name="outerKeySelector">outerKeySelector</paramref> function is evaluated and the resulting key is used to look up the corresponding inner elements in the hash table. For each matching inner element (if any), the <paramref name="resultSelector">resultSelector</paramref> function is evaluated for the outer and inner element pair, and the resulting object is yielded.
            /// </summary>
            /// <typeparam name="TOuter">Type of the elements in the outer source Enumerable.</typeparam>
            /// <typeparam name="TInner">Type of the elements in the inner source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key field to perform the join on.</typeparam>
            /// <typeparam name="TElement">Type of the elements in the result Enumerable.</typeparam>
            /// <param name="outer">Outer sequence to perform the join with.</param>
            /// <param name="inner">Inner sequence to perform the join with.</param>
            /// <param name="outerKeySelector">Function that extracts the join key values from elements of the <paramref name="outer">outer</paramref> Enumerable.</param>
            /// <param name="innerKeySelector">Function that extracts the join key values from elements of the <paramref name="inner">inner</paramref> Enumerable.</param>
            /// <param name="resultSelector">Function that generates a join result of type <typeparamref name="TElement">TElement</typeparamref> based on a pair of one outer element (type <typeparamref name="TOuter">TOuter</typeparamref>) and one inner element (type <typeparamref name="TInner">TInner</typeparamref>).</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence with elements resulting from a join operation on the outer and inner sequence based on a specified key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TOuter">TOuter</typeparamref>&gt;. The <c>Join</c> operator preserves the order of the <paramref name="outer">outer</paramref> sequence elements, and for each outer element, the order of the matching <paramref name="inner">inner</paramref> sequence elements. In relational database terms, the <c>Join</c> operator implements an inner equijoin. Other join operations, such as left outer join and right outer join have no dedicated standard query operators, but are subsets of the capabilities of the <see cref="GroupJoin&lt;TOuter, TInner, TKey, TResult&gt;(IEnumerable&lt;TOuter&gt;, IEnumerable&lt;TInner&gt;, Func&lt;TOuter, TKey&gt;, Func&lt;TInner, TKey&gt;, Func&lt;TOuter, IEnumerable&lt;TInner&gt;, TResult&gt;)">GroupJoin</see> operator.</remarks>
            public static IEnumerable<TElement> Join<TOuter, TInner, TKey, TElement>(
#if CS30
                this IEnumerable<TOuter> outer, 
#else
IEnumerable<TOuter> outer,
#endif
 IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, TInner, TElement> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (outer == null || inner == null || outerKeySelector == null || innerKeySelector == null || resultSelector == null || comparer == null)
                    throw new ArgumentNullException();

                return _Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            private static IEnumerable<TElement> _Join<TOuter, TInner, TKey, TElement>(
                IEnumerable<TOuter> outer,
                IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, TInner, TElement> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                Lookup<TKey, TInner> innerLookup = ToLookup(inner, innerKeySelector, comparer);

                foreach (TOuter o in outer)
                    foreach (TInner i in innerLookup[outerKeySelector(o)])
                        yield return resultSelector(o, i);
            }

            #endregion

            #region 1.6.2 GroupJoin

            /// <summary>
            /// Performs a grouped join of two sequences based on matching keys extracted from the elements. When the returned sequence is enumerated, it first enumerates the <paramref name="inner">inner</paramref> sequence and evaluates the <paramref name="innerKeySelector">innerKeySelector</paramref> function once for each inner element, collecting the elements by their keys in a hash table. Once all inner elements and keys have been collected, the <paramref name="outer">outer</paramref> sequence is enumerated. For each outer element, the <paramref name="outerKeySelector">outerKeySelector</paramref> function is evaluated, the resulting key is used to look up the corresponding inner elements in the hash table, the <paramref name="resultSelector">resultSelector</paramref> function is evaluated for the outer element and the (possibly empty) sequence of matching inner elements, and the resulting object is yielded.
            /// </summary>
            /// <typeparam name="TOuter">Type of the elements in the outer source Enumerable.</typeparam>
            /// <typeparam name="TInner">Type of the elements in the inner source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key field to perform the join on.</typeparam>
            /// <typeparam name="TResult">Type of the elements in the result Enumerable.</typeparam>
            /// <param name="outer">Outer sequence to perform the join with.</param>
            /// <param name="inner">Inner sequence to perform the join with.</param>
            /// <param name="outerKeySelector">Function that extracts the join key values from elements of the <paramref name="outer">outer</paramref> Enumerable.</param>
            /// <param name="innerKeySelector">Function that extracts the join key values from elements of the <paramref name="inner">inner</paramref> Enumerable.</param>
            /// <param name="resultSelector">Function that generates a join result of type <typeparamref name="TResult">TResult</typeparamref> based on a pair of one outer element (type <typeparamref name="TOuter">TOuter</typeparamref>) and a (possibly empty) sequence of inner elements (type <typeparamref name="TInner">TInner</typeparamref>).</param>
            /// <returns>Sequence with elements resulting from a group join operation on the outer and inner sequence based on a specified key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TOuter">TOuter</typeparamref>&gt;. The <c>GroupJoin</c> operator preserves the order of the <paramref name="outer">outer</paramref> sequence elements, and for each outer element, the order of the matching <paramref name="inner">inner</paramref> sequence elements. The <c>GroupJoin</c> operator produces hierarchical results (outer elements paired with sequences of matching inner elements) and has no direct equivalent in traditional relational database terms.</remarks>
            public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
#if CS30
                this IEnumerable<TOuter> outer,
#else
IEnumerable<TOuter> outer,
#endif
 IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
            {
                return GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Performs a grouped join of two sequences based on matching keys extracted from the elements. When the returned sequence is enumerated, it first enumerates the <paramref name="inner">inner</paramref> sequence and evaluates the <paramref name="innerKeySelector">innerKeySelector</paramref> function once for each inner element, collecting the elements by their keys in a hash table. Once all inner elements and keys have been collected, the <paramref name="outer">outer</paramref> sequence is enumerated. For each outer element, the <paramref name="outerKeySelector">outerKeySelector</paramref> function is evaluated, the resulting key is used to look up the corresponding inner elements in the hash table, the <paramref name="resultSelector">resultSelector</paramref> function is evaluated for the outer element and the (possibly empty) sequence of matching inner elements, and the resulting object is yielded.
            /// </summary>
            /// <typeparam name="TOuter">Type of the elements in the outer source Enumerable.</typeparam>
            /// <typeparam name="TInner">Type of the elements in the inner source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key field to perform the join on.</typeparam>
            /// <typeparam name="TResult">Type of the elements in the result Enumerable.</typeparam>
            /// <param name="outer">Outer sequence to perform the join with.</param>
            /// <param name="inner">Inner sequence to perform the join with.</param>
            /// <param name="outerKeySelector">Function that extracts the join key values from elements of the <paramref name="outer">outer</paramref> Enumerable.</param>
            /// <param name="innerKeySelector">Function that extracts the join key values from elements of the <paramref name="inner">inner</paramref> Enumerable.</param>
            /// <param name="resultSelector">Function that generates a join result of type <typeparamref name="TResult">TResult</typeparamref> based on a pair of one outer element (type <typeparamref name="TOuter">TOuter</typeparamref>) and a (possibly empty) sequence of inner elements (type <typeparamref name="TInner">TInner</typeparamref>).</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence with elements resulting from a group join operation on the outer and inner sequence based on a specified key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TOuter">TOuter</typeparamref>&gt;. The <c>GroupJoin</c> operator preserves the order of the <paramref name="outer">outer</paramref> sequence elements, and for each outer element, the order of the matching <paramref name="inner">inner</paramref> sequence elements. The <c>GroupJoin</c> operator produces hierarchical results (outer elements paired with sequences of matching inner elements) and has no direct equivalent in traditional relational database terms.</remarks>
            public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
#if CS30
                this IEnumerable<TOuter> outer, 
#else
IEnumerable<TOuter> outer,
#endif
 IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (outer == null || inner == null || outerKeySelector == null || innerKeySelector == null || resultSelector == null || comparer == null)
                    throw new ArgumentNullException();

                return _GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            private static IEnumerable<TResult> _GroupJoin<TOuter, TInner, TKey, TResult>(
                IEnumerable<TOuter> outer,
                IEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                Lookup<TKey, TInner> innerLookup = ToLookup(inner, innerKeySelector, comparer);

                foreach (TOuter o in outer)
                {
                    TKey key = outerKeySelector(o);
                    if (innerLookup.Contains(key))
                        yield return resultSelector(o, innerLookup[key]);
                    else
                        yield return resultSelector(o, new List<TInner>());
                }
            }

            #endregion

            #endregion

            #region 1.7 Concatenation operator

            #region 1.7.1 Concat

            /// <summary>
            /// Concatenates two sequences. When the returned sequence is enumerated, it enumerates the first sequence, yielding each element, and then enumerates the second sequence, yielding each element.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="first">First sequence to concatenate.</param>
            /// <param name="second">Second sequence to concatenate.</param>
            /// <returns>Sequence yielding the first and second sequence in a consecutive order.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Concat<TSource>(
#if CS30
                this IEnumerable<TSource> first, 
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second)
            {
                if (first == null || second == null)
                    throw new ArgumentNullException();

                return _Concat(first, second);
            }

            private static IEnumerable<TSource> _Concat<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
            {
                foreach (TSource item in first)
                    yield return item;

                foreach (TSource item in second)
                    yield return item;
            }

            #endregion

            #endregion

            #region 1.8 Ordering operators

            #region 1.8.1 OrderBy / ThenBy

            #region OrderBy

            /// <summary>
            /// Sorts a sequence based on extracted key values in ascending order using the default comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in ascending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <returns>Sequence yielding the source sequence elements in ascending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>OrderBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>OrderBy</c> operator establishes a primary ordering, for subsequent orderings use the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> OrderBy<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return OrderBy(source, keySelector, Comparer<TKey>.Default);
            }

            /// <summary>
            /// Sorts a sequence based on extracted key values in ascending order using the specified comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in ascending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <param name="comparer">Comparer object to compare the extracted key values for sorting purposes.</param>
            /// <returns>Sequence yielding the source sequence elements in ascending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>OrderBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>OrderBy</c> operator establishes a primary ordering, for subsequent orderings use the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> OrderBy<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IComparer<TKey> comparer)
            {
                return OrderByInternal(source, keySelector, comparer, false);
            }

            /// <summary>
            /// Sorts a sequence based on extracted key values in descending order using the default comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in descending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <returns>Sequence yielding the source sequence elements in descending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>OrderBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>OrderBy</c> operator establishes a primary ordering, for subsequent orderings use the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> OrderByDescending<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return OrderByDescending(source, keySelector, Comparer<TKey>.Default);
            }

            /// <summary>
            /// Sorts a sequence based on extracted key values in descending order using the specified comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in descending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <param name="comparer">Comparer object to compare the extracted key values for sorting purposes.</param>
            /// <returns>Sequence yielding the source sequence elements in descending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>OrderBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>OrderBy</c> operator establishes a primary ordering, for subsequent orderings use the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> OrderByDescending<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source, 
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IComparer<TKey> comparer)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return OrderByInternal(source, keySelector, comparer, true);
            }

            #endregion

            #region ThenBy

            /// <summary>
            /// Sorts a sequence, which has been sorted before using an <c>OrderBy</c> operator invocation and one or more <c>ThenBy</c> operator invocations, based on extracted key values in ascending order using the default comparer for the key type <typeparamref name="K">K</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in ascending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <returns>Sequence yielding the source sequence elements in ascending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>ThenBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>ThenBy</c> operator establishes an n-ary (n &gt; 1) ordering, after a primary ordering using the <c>OrderBy</c> family of operators and a series of higher order m-ary orderings (1 &lt; m &lt; n) using the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> ThenBy<TSource, TKey>(
#if CS30
                this IOrderedSequence<TSource> source, 
#else
IOrderedSequence<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return source.CreateOrderedSequence<TKey>(keySelector, Comparer<TKey>.Default, false);
            }

            /// <summary>
            /// Sorts a sequence, which has been sorted before using an <c>OrderBy</c> operator invocation and one or more <c>ThenBy</c> operator invocations, based on extracted key values in ascending order using the specified comparer for the key type <typeparamref name="K">K</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in ascending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <param name="comparer">Comparer object to compare the extracted key values for sorting purposes.</param>
            /// <returns>Sequence yielding the source sequence elements in ascending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>ThenBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>ThenBy</c> operator establishes an n-ary (n &gt; 1) ordering, after a primary ordering using the <c>OrderBy</c> family of operators and a series of higher order m-ary orderings (1 &lt; m &lt; n) using the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> ThenBy<TSource, TKey>(
#if CS30
                this IOrderedSequence<TSource> source, 
#else
IOrderedSequence<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IComparer<TKey> comparer)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return source.CreateOrderedSequence<TKey>(keySelector, comparer, false);
            }

            /// <summary>
            /// Sorts a sequence, which has been sorted before using an <c>OrderBy</c> operator invocation and one or more <c>ThenBy</c> operator invocations, based on extracted key values in descending order using the default comparer for the key type <typeparamref name="K">K</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in descending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <returns>Sequence yielding the source sequence elements in descending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>ThenBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>ThenBy</c> operator establishes an n-ary (n &gt; 1) ordering, after a primary ordering using the <c>OrderBy</c> family of operators and a series of higher order m-ary orderings (1 &lt; m &lt; n) using the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> ThenByDescending<TSource, TKey>(
#if CS30
                this IOrderedSequence<TSource> source, 
#else
IOrderedSequence<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return source.CreateOrderedSequence<TKey>(keySelector, Comparer<TKey>.Default, true);
            }

            /// <summary>
            /// Sorts a sequence, which has been sorted before using an <c>OrderBy</c> operator invocation and one or more <c>ThenBy</c> operator invocations, based on extracted key values in descending order using the specified comparer for the key type <typeparamref name="K">K</typeparamref>. When the returned sequence is enumerated, it first enumerates the source sequence, collecting all elements; then evaluates the <paramref name="keySelector">keySelector</paramref> function once for each element, collecting the key values to order by; then sorts the elements according to the collected key values in descending order; and finally, yields the elements in the resulting order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the sort operation is performed.</typeparam>
            /// <param name="source">Sequence to sort elements from.</param>
            /// <param name="keySelector">Function that extracts the sort key values from the source sequence to perform the sort operation on.</param>
            /// <param name="comparer">Comparer object to compare the extracted key values for sorting purposes.</param>
            /// <returns>Sequence yielding the source sequence elements in descending order according to the extracted key values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The <c>ThenBy</c> operator performs an unstable sort; that is, if the key values of two elements are equal, the order of the elements might not be preserved. In contrast, a stable sort preserves the order of elements that have equal key values. The <c>ThenBy</c> operator establishes an n-ary (n &gt; 1) ordering, after a primary ordering using the <c>OrderBy</c> family of operators and a series of higher order m-ary orderings (1 &lt; m &lt; n) using the <c>ThenBy</c> family of operators.</remarks>
            public static IOrderedSequence<TSource> ThenByDescending<TSource, TKey>(
#if CS30
                this IOrderedSequence<TSource> source, 
#else
IOrderedSequence<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IComparer<TKey> comparer)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return source.CreateOrderedSequence<TKey>(keySelector, comparer, true);
            }

            #endregion

            #region Internal implementation

            internal static IOrderedSequence<T> OrderByInternal<T, K>(IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> comparer, bool descending)
            {
                if (source == null || keySelector == null)
                    throw new ArgumentNullException();

                return OrderByInternal2(source, keySelector, comparer, descending);
            }

            internal static IOrderedSequence<T> OrderByInternal2<T, K>(IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> comparer, bool descending)
            {
                SortedList<K, List<T>> lst = new SortedList<K, List<T>>((descending ? new ReverseComparer<K>(comparer) : comparer));

                foreach (T item in source)
                {
                    K key = keySelector(item);

                    if (!lst.ContainsKey(key))
                        lst[key] = new List<T>();
                    lst[key].Add(item);
                }

                return new OrderedSequence<T>(lst.Values);
            }

            #endregion

            #endregion

            #region 1.8.2 Reverse

            /// <summary>
            /// Reverses the elements of a Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, collecting all elements, and then yields the elements of the source sequence in reverse order.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to be yielded in reverse order.</param>
            /// <returns>Sequence yielding the source sequence elements in reverse order.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Reverse<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _Reverse(source);
            }

            private static IEnumerable<TSource> _Reverse<TSource>(IEnumerable<TSource> source)
            {
                List<TSource> lst = new List<TSource>();
                foreach (TSource item in source)
                    lst.Add(item);

                for (int i = lst.Count - 1; i >= 0; i--)
                    yield return lst[i];
            }

            #endregion

            #endregion

            #region 1.9 Grouping operators

            #region 1.9.1 GroupBy

            /// <summary>
            /// Groups a sequence based on extracted key values using the default equality comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it enumerates source and evaluates the <paramref name="keySelector">keySelector</paramref> function once for each source element. Once all key and destination element pairs have been collected, a sequence of <see cref="IGrouping&lt;K, T&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; instances are yielded. Each <see cref="IGrouping&lt;K, T&gt;">IGrouping</see>&lt;<typeparamref name="K">K</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; instance represents a sequence of destination elements with a particular key value. The groupings are yielded in the order that their key values first occurred in the source sequence, and destination elements within a grouping are yielded in the order their source elements occurred in the source Enumerable. When creating the groupings, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the group operation is performed.</typeparam>
            /// <param name="source">Sequence to group elements from.</param>
            /// <param name="keySelector">Function that extracts the group key values from the source sequence to perform the group operation on.</param>
            /// <returns>Sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; elements each representing a sequence of elements with a particular key value.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                return GroupBy<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Groups a sequence based on extracted key values using the specified equality comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it enumerates source and evaluates the <paramref name="keySelector">keySelector</paramref> function once for each source element. Once all key and destination element pairs have been collected, a sequence of <see cref="IGrouping&lt;K, T&gt;">IGrouping</see>&lt;<typeparamref name="K">K</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; instances are yielded. Each <see cref="IGrouping&lt;K, T&gt;">IGrouping</see>&lt;<typeparamref name="K">K</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; instance represents a sequence of destination elements with a particular key value. The groupings are yielded in the order that their key values first occurred in the source sequence, and destination elements within a grouping are yielded in the order their source elements occurred in the source Enumerable. When creating the groupings, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="K">K</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the group operation is performed.</typeparam>
            /// <param name="source">Sequence to group elements from.</param>
            /// <param name="keySelector">Function that extracts the group key values from the source sequence to perform the group operation on.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values for grouping purposes.</param>
            /// <returns>Sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TSource">TSource</typeparamref>&gt; elements each representing a sequence of elements with a particular key value.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> comparer)
            {
                return GroupBy<TSource, TKey, TSource>(source, keySelector, delegate(TSource t) { return t; } /* e => e */, comparer);
            }

            /// <summary>
            /// Groups a sequence based on extracted key values using the default equality comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it enumerates source and evaluates the <paramref name="keySelector">keySelector</paramref> and <paramref name="elementSelector">elementSelector</paramref> functions once for each source element. Once all key and destination element pairs have been collected, a sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; instances are yielded. Each <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; instance represents a sequence of destination elements with a particular key value. The groupings are yielded in the order that their key values first occurred in the source sequence, and destination elements within a grouping are yielded in the order their source elements occurred in the source Enumerable. When creating the groupings, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the group operation is performed.</typeparam>
            /// <typeparam name="TElement">Type of the result elements.</typeparam>
            /// <param name="source">Sequence to group elements from.</param>
            /// <param name="keySelector">Function that extracts the group key values from the source sequence to perform the group operation on.</param>
            /// <param name="elementSelector">Function that generates result elements for the elements in the source Enumerable.</param>
            /// <returns>Sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; elements each representing a sequence of elements with a particular key value.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector)
            {
                return GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Groups a sequence based on extracted key values using the specified equality comparer for the key type <typeparamref name="TKey">TKey</typeparamref>. When the returned sequence is enumerated, it enumerates source and evaluates the <paramref name="keySelector">keySelector</paramref> and <paramref name="elementSelector">elementSelector</paramref> functions once for each source element. Once all key and destination element pairs have been collected, a sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; instances are yielded. Each <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; instance represents a sequence of destination elements with a particular key value. The groupings are yielded in the order that their key values first occurred in the source sequence, and destination elements within a grouping are yielded in the order their source elements occurred in the source Enumerable. When creating the groupings, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key on which the group operation is performed.</typeparam>
            /// <typeparam name="TElement">Type of the result elements.</typeparam>
            /// <param name="source">Sequence to group elements from.</param>
            /// <param name="keySelector">Function that extracts the group key values from the source sequence to perform the group operation on.</param>
            /// <param name="elementSelector">Function that generates result elements for the elements in the source Enumerable.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values for grouping purposes.</param>
            /// <returns>Sequence of <see cref="IGrouping&lt;TKey, TSource&gt;">IGrouping</see>&lt;<typeparamref name="TKey">TKey</typeparamref>, <typeparamref name="TElement">TElement</typeparamref>&gt; elements each representing a sequence of elements with a particular key value.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (source == null || keySelector == null || elementSelector == null) //comparer may be null
                    throw new ArgumentNullException();

                return _GroupBy(source, keySelector, elementSelector, comparer);
            }

            private static IEnumerable<IGrouping<TKey, TElement>> _GroupBy<TSource, TKey, TElement>(
                IEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                IEqualityComparer<TKey> comparer)
            {
                Lookup<TKey, TElement> lookup = ToLookup(source, keySelector, elementSelector, comparer);
                foreach (TKey key in lookup.keys)
                    yield return lookup.dictionary[key];
            }

            /// <summary>
            /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.
            /// </summary>
            /// <typeparam name="TSource">The type of the elements of source.</typeparam>
            /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
            /// <typeparam name="TResult">The type of the result value returned by resultSelector.</typeparam>
            /// <param name="source">An System.Collections.Generic.IEnumerable&lt;T&gt; whose elements to group.</param>
            /// <param name="keySelector">A function to extract the key for each element.</param>
            /// <param name="resultSelector">A function to create a result value from each group.</param>
            /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that has a type argument of TResult and where each element represents a projection over a group and its key.</returns>
            public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
            {
                return GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.
            /// </summary>
            /// <typeparam name="TSource">The type of the elements of source.</typeparam>
            /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
            /// <typeparam name="TResult">The type of the result value returned by resultSelector.</typeparam>
            /// <param name="source">An System.Collections.Generic.IEnumerable&lt;T&gt; whose elements to group.</param>
            /// <param name="keySelector">A function to extract the key for each element.</param>
            /// <param name="resultSelector">A function to create a result value from each group.</param>
            /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;T&gt; to compare keys with.</param>
            /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that has a type argument of TResult and where each element represents a projection over a group and its key.</returns>
            public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (source == null || keySelector == null || resultSelector == null) //comparer may be null
                    throw new ArgumentNullException();

                return _GroupBy(source, keySelector, resultSelector, comparer);
            }

            private static IEnumerable<TResult> _GroupBy<TSource, TKey, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                Lookup<TKey, TSource> lookup = ToLookup(source, keySelector, comparer);
                foreach (TKey key in lookup.keys)
                    yield return resultSelector(key, lookup[key]);
            }

            /// <summary>
            /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The elements of each group are projected by using a specified function.
            /// </summary>
            /// <typeparam name="TSource">The type of the elements of source.</typeparam>
            /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
            /// <typeparam name="TElement">The type of the elements in each System.Linq.IGrouping&lt;TKey,TElement&gt;.</typeparam>
            /// <typeparam name="TResult">The type of the result value returned by resultSelector.</typeparam>
            /// <param name="source">An System.Collections.Generic.IEnumerable&lt;T&gt; whose elements to group.</param>
            /// <param name="keySelector">A function to extract the key for each element.</param>
            /// <param name="elementSelector">A function to map each source element to an element in an BdsSoft.Linq.IGrouping&lt;TKey,TElement&gt;.</param>
            /// <param name="resultSelector">A function to create a result value from each group.</param>
            /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that has a type argument of TResult and where each element represents a projection over a group and its key.</returns>
            public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
            {
                return GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.
            /// </summary>
            /// <typeparam name="TSource">The type of the elements of source.</typeparam>
            /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
            /// <typeparam name="TElement">The type of the elements in each System.Linq.IGrouping&lt;TKey,TElement&gt;.</typeparam>
            /// <typeparam name="TResult">The type of the result value returned by resultSelector.</typeparam>
            /// <param name="source">An System.Collections.Generic.IEnumerable&lt;T&gt; whose elements to group.</param>
            /// <param name="keySelector">A function to extract the key for each element.</param>
            /// <param name="elementSelector">A function to map each source element to an element in an BdsSoft.Linq.IGrouping&lt;TKey,TElement&gt;.</param>
            /// <param name="resultSelector">A function to create a result value from each group.</param>
            /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;T&gt; to compare keys with.</param>
            /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that has a type argument of TResult and where each element represents a projection over a group and its key.</returns>
            public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (source == null || keySelector == null || elementSelector == null || resultSelector == null) //comparer may be null
                    throw new ArgumentNullException();

                return _GroupBy(source, keySelector, elementSelector, resultSelector, comparer);
            }

            private static IEnumerable<TResult> _GroupBy<TSource, TKey, TElement, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                Lookup<TKey, TElement> lookup = ToLookup(source, keySelector, elementSelector, comparer);
                foreach (TKey key in lookup.keys)
                    yield return resultSelector(key, lookup[key]);
            }

            #endregion

            #endregion

            #region 1.10 Set operators

            #region 1.10.1 Distinct

            /// <summary>
            /// Eliminates duplicate elements from a Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, yielding each element that hasn't previously been yielded. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to eliminate duplicate elements from.</param>
            /// <returns>Sequence yielding the distinct elements from the source sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Distinct<TSource>(
#if CS30
this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                return Distinct<TSource>(source, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Eliminates duplicate elements from a Enumerable. When the returned sequence is enumerated, it enumerates the source sequence, yielding each element that hasn't previously been yielded. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to eliminate duplicate elements from.</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence yielding the distinct elements from the source sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Distinct<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 IEqualityComparer<TSource> comparer
                )
            {
                if (source == null || comparer == null)
                    throw new ArgumentNullException();

                return _Distinct(source, comparer);
            }

            private static IEnumerable<TSource> _Distinct<TSource>(
                IEnumerable<TSource> source,
                IEqualityComparer<TSource> comparer)
            {
                HashSet<TSource> set = new HashSet<TSource>(comparer);
                foreach (TSource item in source)
                {
                    if (!set.Contains(item))
                    {
                        set.Add(item);
                        yield return item;
                    }
                }
            }

            #endregion

            #region 1.10.2 Union

            /// <summary>
            /// Produces the set union of two sequences. When the returned sequence is enumerated, it enumerates the first and second sequences, in that order, yielding each element that hasn't previously been yielded. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to include in the set union.</param>
            /// <param name="second">Second sequence to include in the set union.</param>
            /// <returns>Sequence yielding the set union of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Union<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second)
            {
                return Union<TSource>(first, second, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Produces the set union of two sequences. When the returned sequence is enumerated, it enumerates the first and second sequences, in that order, yielding each element that hasn't previously been yielded. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to include in the set union.</param>
            /// <param name="second">Second sequence to include in the set union.</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence yielding the set union of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Union<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                if (first == null || second == null || comparer == null)
                    throw new ArgumentNullException();

                return _Union(first, second, comparer);
            }

            private static IEnumerable<TSource> _Union<TSource>(
                IEnumerable<TSource> first,
                IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                HashSet<TSource> set = new HashSet<TSource>(comparer);
                foreach (TSource item in first)
                {
                    if (!set.Contains(item))
                    {
                        set.Add(item);
                        yield return item;
                    }
                }
                foreach (TSource item in second)
                {
                    if (!set.Contains(item))
                    {
                        set.Add(item);
                        yield return item;
                    }
                }
            }

            #endregion

            #region 1.10.3 Intersect

            /// <summary>
            /// Produces the set intersection of two sequences. When the returned sequence is enumerated, it enumerates the first sequence, collecting all distinct elements of that Enumerable. It then enumerates the second sequence, marking those elements that occur in both sequences. It finally yields the marked elements in the order in which they were collected. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to apply the set intersection operation on.</param>
            /// <param name="second">Second sequence to apply the set intersection operation on.</param>
            /// <returns>Sequence yielding the set intersection of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Intersect<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second)
            {
                return Intersect<TSource>(first, second, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Produces the set intersection of two sequences. When the returned sequence is enumerated, it enumerates the first sequence, collecting all distinct elements of that Enumerable. It then enumerates the second sequence, marking those elements that occur in both sequences. It finally yields the marked elements in the order in which they were collected. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to apply the set intersection operation on.</param>
            /// <param name="second">Second sequence to apply the set intersection operation on.</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence yielding the set intersection of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Intersect<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                if (first == null || second == null || comparer == null)
                    throw new ArgumentNullException();

                return _Intersect(first, second, comparer);
            }

            private static IEnumerable<TSource> _Intersect<TSource>(
                IEnumerable<TSource> first,
                IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                Dictionary<TSource, bool> tbl = new Dictionary<TSource, bool>(comparer);
                List<TSource> lst = new List<TSource>();

                foreach (TSource item in first)
                    if (!tbl.ContainsKey(item))
                    {
                        tbl.Add(item, false);
                        lst.Add(item);
                    }

                foreach (TSource item in second)
                    if (tbl.ContainsKey(item))
                        tbl[item] = true;

                foreach (TSource item in lst)
                    if (tbl[item])
                        yield return item;
            }

            #endregion

            #region 1.10.4 Except

            /// <summary>
            /// Produces the set difference between two sequences. When the returned sequence is enumerated, it enumerates the first sequence, collecting all distinct elements of that Enumerable. It then enumerates the second sequence, removing those elements that were also contained in the first Enumerable. It finally yields the remaining elements in the order in which they were collected. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to apply the subtraction of the <paramref name="second">second</paramref> sequence on.</param>
            /// <param name="second">Second sequence to be subtracted from the <paramref name="first">first</paramref> Enumerable.</param>
            /// <returns>Sequence yielding the set difference of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Except<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second)
            {
                return Except<TSource>(first, second, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Produces the set difference between two sequences. When the returned sequence is enumerated, it enumerates the first sequence, collecting all distinct elements of that Enumerable. It then enumerates the second sequence, removing those elements that were also contained in the first Enumerable. It finally yields the remaining elements in the order in which they were collected. Elements are compared using their <c>GetHashCode</c> and <c>Equals</c> methods.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">First sequence to apply the subtraction of the <paramref name="second">second</paramref> sequence on.</param>
            /// <param name="second">Second sequence to be subtracted from the <paramref name="first">first</paramref> Enumerable.</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>Sequence yielding the set difference of the <paramref name="first">first</paramref> sequence and <paramref name="second">second</paramref> sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> Except<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                if (first == null || second == null || comparer == null)
                    throw new ArgumentNullException();

                return _Except(first, second, comparer);
            }

            private static IEnumerable<TSource> _Except<TSource>(
                IEnumerable<TSource> first,
                IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                Dictionary<TSource, bool> tbl = new Dictionary<TSource, bool>(comparer);
                List<TSource> lst = new List<TSource>();

                foreach (TSource item in first)
                    if (!tbl.ContainsKey(item)) //O(1)
                    {
                        tbl.Add(item, true); //[O(1), O(n)]
                        lst.Add(item); //[O(1), O(n)]
                    }

                foreach (TSource item in second)
                    if (tbl.ContainsKey(item)) //O(1)
                        tbl[item] = false; //O(1)

                foreach (TSource item in lst)
                    if (tbl[item]) //O(1)
                        yield return item;
            }

            #endregion

            #endregion

            #region 1.11 Conversion operators

            #region 1.11.1 AsEnumerable

            /// <summary>
            /// Returns the specified sequence typed as IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. The operator has no effect other than to change the compile-time type of the source argument to IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to return.</param>
            /// <returns>The source sequence typed as IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> AsEnumerable<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                return source;
            }

            #endregion

            #region 1.11.2 ToArray

            /// <summary>
            /// Creates an array from a sequence by enumerating the source sequence and returning an array of type <typeparamref name="TSource">TSource</typeparamref>[] containing the elements of the Enumerable.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to create an array for.</param>
            /// <returns>Array of type <typeparamref name="TSource">TSource</typeparamref>[] containing the elements of the source sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TSource[] ToArray<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                List<TSource> lst = new List<TSource>();
                foreach (TSource item in source)
                    lst.Add(item);

                return lst.ToArray();
            }

            #endregion

            #region 1.11.3 ToList

            /// <summary>
            /// Creates a list from a sequence by enumerating the source sequence and returning a list of type List&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; containing the elements of the Enumerable.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to create a list for.</param>
            /// <returns>List of type List&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; containing the elements of the source sequence in order of occurrence.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static List<TSource> ToList<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                List<TSource> lst = new List<TSource>();
                foreach (TSource item in source)
                    lst.Add(item);

                return lst;
            }

            #endregion

            #region 1.11.4 ToDictionary

            /// <summary>
            /// Enumerates a sequence and creates a Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on one element from the source Enumerable. If the produced key value is null or a duplicate key is encountered, a <c>ArgumentNullException</c> is thrown. In the resulting dictionary, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key used in the resulting dictionary.</typeparam>
            /// <param name="source">Sequence to create a dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting dictionary.</param>
            /// <returns>A Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the source element, for every the source element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                return ToDictionary<TSource, TKey, TSource>(source, keySelector, delegate(TSource t) { return t; } /* t => t */, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on one element from the source Enumerable. If the produced key value is null or a duplicate key is encountered, a <c>ArgumentNullException</c> is thrown. In the resulting dictionary, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key used in the resulting dictionary.</typeparam>
            /// <param name="source">Sequence to create a dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting dictionary.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values in the resulting dictionary.</param>
            /// <returns>A Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the source element, for every the source element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> comparer)
            {
                return ToDictionary<TSource, TKey, TSource>(source, keySelector, delegate(TSource t) { return t; } /* t => t */, comparer);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the result of the <paramref name="elementSelector">elementSelector</paramref> function invocation on the source element. If the produced key value is null or a duplicate key is encountered, a <c>ArgumentNullException</c> is thrown. In the resulting dictionary, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key used in the resulting dictionary.</typeparam>
            /// <typeparam name="TElement">Type of the value stored in the resulting dictionary.</typeparam>
            /// <param name="source">Sequence to create a dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting dictionary.</param>
            /// <param name="elementSelector">Function to generate a result object for each source object.</param>
            /// <returns>A Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the result of the <paramref name="elementSelector">elementSelector</paramref> function invocation, for every the source element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector)
            {
                return ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the result of the <paramref name="elementSelector">elementSelector</paramref> function invocation on the source element. If the produced key value is null or a duplicate key is encountered, a <c>ArgumentNullException</c> is thrown. In the resulting dictionary, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key used in the resulting dictionary.</typeparam>
            /// <typeparam name="TElement">Type of the value stored in the resulting dictionary.</typeparam>
            /// <param name="source">Sequence to create a dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting dictionary.</param>
            /// <param name="elementSelector">Function to generate a result object for each source object.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values in the resulting dictionary.</param>
            /// <returns>A Dictionary&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the result of the <paramref name="elementSelector">elementSelector</paramref> function invocation, for every the source element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (source == null || keySelector == null || elementSelector == null) //comparer may be null
                    throw new ArgumentNullException();

                Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
                foreach (TSource item in source)
                {
                    TKey key = keySelector(item);
                    if (key == null)
                        throw new ArgumentNullException();
                    if (dictionary.ContainsKey(key))
                        throw new ArgumentException();
                    dictionary.Add(key, elementSelector(item));
                }

                return dictionary;
            }

            #endregion

            #region 1.11.5 ToLookup

            /// <summary>
            /// Enumerates a sequence and creates a Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the corresponding source elements. In the resulting one-to-many dictionary, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key values in the one-to-many dictionary.</typeparam>
            /// <param name="source">Sequence to create a one-to-many dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting one-to-many dictionary.</param>
            /// <returns>A Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the source elements matching the key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Lookup<TKey, TSource> ToLookup<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector)
            {
                return ToLookup<TSource, TKey, TSource>(source, keySelector, delegate(TSource t) { return t; } /* t => t */, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the corresponding source elements. In the resulting one-to-many dictionary, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key values in the one-to-many dictionary.</typeparam>
            /// <param name="source">Sequence to create a one-to-many dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting one-to-many dictionary.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values in the resulting one-to-many dictionary.</param>
            /// <returns>A Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TSource">TSource</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the source elements matching the key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Lookup<TKey, TSource> ToLookup<TSource, TKey>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> comparer)
            {
                return ToLookup<TSource, TKey, TSource>(source, keySelector, delegate(TSource t) { return t; } /* t => t */, comparer);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the results of the <paramref name="elementSelector">elementSelector</paramref> function invocations on the corresponding source elements. In the resulting one-to-many dictionary, key values are compared using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key values in the one-to-many dictionary.</typeparam>
            /// <typeparam name="TElement">Type of the value stored in the resulting one-to-many dictionary.</typeparam>
            /// <param name="source">Sequence to create a one-to-many dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting one-to-many dictionary.</param>
            /// <param name="elementSelector">Function to generate a result object for each source object.</param>
            /// <returns>A Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the results of the <paramref name="elementSelector">elementSelector</paramref> function invocations for every source element matching the key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Lookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector)
            {
                return ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
            }

            /// <summary>
            /// Enumerates a sequence and creates a Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; object out of it, mapping each key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the results of the <paramref name="elementSelector">elementSelector</paramref> function invocations on the corresponding source elements. In the resulting one-to-many dictionary, key values are compared using the given <paramref name="comparer">comparer</paramref>, or, if a null <paramref name="comparer">comparer</paramref> was specified, using the default equality comparer, EqualityComparer&lt;<typeparamref name="TKey">TKey</typeparamref>&gt;.Default.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TKey">Type of the key values in the one-to-many dictionary.</typeparam>
            /// <typeparam name="TElement">Type of the value stored in the resulting one-to-many dictionary.</typeparam>
            /// <param name="source">Sequence to create a one-to-many dictionary from.</param>
            /// <param name="keySelector">Function that extracts the key values from the source sequence for usage in the resulting one-to-many dictionary.</param>
            /// <param name="elementSelector">Function to generate a result object for each source object.</param>
            /// <param name="comparer">Equality comparer to compare the extracted key values in the resulting one-to-many dictionary.</param>
            /// <returns>A Lookup&lt;<typeparamref name="TKey">TKey</typeparamref>,<typeparamref name="TElement">TElement</typeparamref>&gt; collection mapping a key value extracted by the <paramref name="keySelector">keySelector</paramref> function on the results of the <paramref name="elementSelector">elementSelector</paramref> function invocations for every source element matching the key.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static Lookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TKey> keySelector,
                Func<TSource, TElement> elementSelector,
                IEqualityComparer<TKey> comparer)
            {
                if (source == null || keySelector == null || elementSelector == null) //comparer may be null
                    throw new ArgumentNullException();

                Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
                foreach (TSource item in source)
                {
                    TKey key = keySelector(item);
                    Grouping<TKey, TElement> group;
                    if (lookup.Contains(key))
                        group = ((Grouping<TKey, TElement>)lookup.dictionary[key]);
                    else
                    {
                        group = new Grouping<TKey, TElement>(key);
                        lookup.Add(group);
                    }
                    group.Add(elementSelector(item));
                }
                return lookup;
            }

            #endregion

            #region 1.11.6 OfType

            /// <summary>
            /// Filters the elements of a sequence based on a type. When the returned sequence is enumerated, it enumerates the source sequence and yields those elements that are of type <typeparamref name="TResult">TResult</typeparamref>. Specifically, each element <c>e</c> for which <c>e is TResult</c> evaluated to true is yielded by evaluating <c>(TResult)e</c>.
            /// </summary>
            /// <typeparam name="TResult">Type of the elements in the result Enumerable.</typeparam>
            /// <param name="source">Source sequence (non-generic) to filter by the given type <typeparamref name="TResult">TResult</typeparamref>.</param>
            /// <returns>Sequence with the elements from the source sequence that are of type <typeparamref name="TResult">TResult</typeparamref>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TResult">TResult</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> OfType<TResult>(
#if CS30
                this IEnumerable source
#else
IEnumerable source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _OfType<TResult>(source);
            }

            private static IEnumerable<TResult> _OfType<TResult>(IEnumerable source)
            {
                foreach (object e in source)
                    if (e is TResult)
                        yield return (TResult)e;
            }

            #endregion

            #region 1.11.7 Cast

            /// <summary>
            /// Casts the elements of a sequence to a given type. When the returned sequence is enumerated, it enumerates the source sequence and yields each element cast to type <typeparamref name="TResult">TResult</typeparamref>. An <c>InvalidCastException</c> is thrown if an element in the sequence cannot be cast to type <typeparamref name="TResult">TResult</typeparamref>.
            /// </summary>
            /// <typeparam name="TResult">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Source sequence to cast elements to type <typeparamref name="TResult">TResult</typeparamref>.</param>
            /// <returns>Sequence with the elements of the source sequence casted to type <typeparamref name="TResult">TResult</typeparamref>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TResult">TResult</typeparamref>&gt;.</remarks>
            public static IEnumerable<TResult> Cast<TResult>(
#if CS30
                this IEnumerable source
#else
IEnumerable source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _Cast<TResult>(source);
            }

            private static IEnumerable<TResult> _Cast<TResult>(IEnumerable source)
            {
                foreach (object e in source)
                    yield return (TResult)e;
            }

            #endregion

            #endregion

            #region 1.12 Equality operator

            #region 1.12.1 SequenceEqual

            /// <summary>
            /// Checks whether two sequences are equal by enumerating the two source sequences in parallel and comparing corresponding elements using the <c>Equals</c> static method in <c>System.Object</c>. The method returns true if all corresponding elements compare equal and the two sequences are of equal length. Otherwise, the method returns false. An <c>ArgumentNullException</c> is thrown if either argument is null.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">The first sequence to compare for equality.</param>
            /// <param name="second">The second sequence to compare for equality.</param>
            /// <returns>True if both sequences are of the same length and all corresponding elements of both sequences are equal, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool SequenceEqual<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second)
            {
                return SequenceEqual<TSource>(first, second, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Checks whether two sequences are equal by enumerating the two source sequences in parallel and comparing corresponding elements using the <c>Equals</c> static method in <c>System.Object</c>. The method returns true if all corresponding elements compare equal and the two sequences are of equal length. Otherwise, the method returns false. An <c>ArgumentNullException</c> is thrown if either argument is null.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
            /// <param name="first">The first sequence to compare for equality.</param>
            /// <param name="second">The second sequence to compare for equality.</param>
            /// <param name="comparer">Comparer used for equality checks.</param>
            /// <returns>True if both sequences are of the same length and all corresponding elements of both sequences are equal, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool SequenceEqual<TSource>(
#if CS30
                this IEnumerable<TSource> first,
#else
IEnumerable<TSource> first,
#endif
 IEnumerable<TSource> second,
                IEqualityComparer<TSource> comparer)
            {
                if (first == null || second == null || comparer == null)
                    throw new ArgumentNullException();

                if (Enumerable.Count(first) != Enumerable.Count(second))
                    return false;

                for (IEnumerator<TSource> e1 = first.GetEnumerator(), e2 = second.GetEnumerator(); e1.MoveNext() && e2.MoveNext(); )
                    if (!comparer.Equals(e1.Current, e2.Current))
                        return false;
                return true;
            }

            #endregion

            #endregion

            #region 1.13 Element operators

            #region 1.13.1 First

            /// <summary>
            /// Returns the first element of a Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the first element from.</param>
            /// <returns>The first element from the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="FirstOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">FirstOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty.</remarks>
            public static TSource First<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();
                else
                    return enumerator.Current;
            }

            /// <summary>
            /// Returns the first element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the first element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The first element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The first element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="FirstOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;, Func&lt;TSource, bool&gt;)">FirstOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource First<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                foreach (TSource item in source)
                    if (predicate(item))
                        return item;

                throw new InvalidOperationException();
            }

            #endregion

            #region 1.13.2 FirstOrDefault

            /// <summary>
            /// Returns the first element of a Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the first element from.</param>
            /// <returns>The first element from the source Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="First&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">First</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty.</remarks>
            public static TSource FirstOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    return default(TSource);
                else
                    return enumerator.Current;
            }

            /// <summary>
            /// Returns the first element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the first element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The first element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The first element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="First&lt;TSource&gt;(IEnumerable&lt;TSource&gt;, Func&lt;TSource, bool&gt;)">First</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource FirstOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                foreach (TSource item in source)
                    if (predicate(item))
                        return item;

                return default(TSource);
            }

            #endregion

            #region 1.13.3 Last

            /// <summary>
            /// Returns the last element of a Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the last element from.</param>
            /// <returns>The last element from the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="LastOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">LastOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty.</remarks>
            public static TSource Last<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();
                else
                {
                    TSource last = enumerator.Current;
                    while (enumerator.MoveNext())
                        last = enumerator.Current;
                    return last;
                }
            }

            /// <summary>
            /// Returns the last element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the last element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The last element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The last element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="LastOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;, Func&lt;TSource, bool&gt;)">LastOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource Last<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                TSource last = default(TSource);
                bool found = false;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                    {
                        found = true;
                        last = item;
                    }
                }

                if (!found)
                    throw new InvalidOperationException();
                else
                    return last;
            }

            #endregion

            #region 1.13.4 LastOrDefault

            /// <summary>
            /// Returns the last element of a Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the last element from.</param>
            /// <returns>The last element from the source Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="Last&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">Last</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty.</remarks>
            public static TSource LastOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    return default(TSource);
                else
                {
                    TSource last = enumerator.Current;
                    while (enumerator.MoveNext())
                        last = enumerator.Current;
                    return last;
                }
            }

            /// <summary>
            /// Returns the last element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the last element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The last element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The last element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="Last&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">Last</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource LastOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                TSource last = default(TSource);
                bool found = false;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                    {
                        found = true;
                        last = item;
                    }
                }

                if (!found)
                    return default(TSource);
                else
                    return last;
            }

            #endregion

            #region 1.13.5 Single

            /// <summary>
            /// Returns the single element of a Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty or if the source sequence contains more than one element.
            /// </summary>
            /// <typeparam name="TSource">Type of the element in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the single element from.</param>
            /// <returns>The single element from the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty or if the source sequence contains more than one element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="SingleOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">SingleOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty.</remarks>
            public static TSource Single<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    TSource single = enumerator.Current;
                    if (!enumerator.MoveNext()) //only one
                        return single;
                    else //more than one
                        throw new InvalidOperationException();
                }
                else //empty
                    throw new InvalidOperationException();
            }

            /// <summary>
            /// Returns the single element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref>, if more than one element matching the <paramref name="predicate">predicate</paramref> is found, or if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the element in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the single element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The single element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The single element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. An <c>InvalidOperationException</c> is thrown if no element matches the <paramref name="predicate">predicate</paramref>, if more than one element matching the <paramref name="predicate">predicate</paramref> is found, or if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="SingleOrDefault&lt;TSource&gt;(IEnumerable&lt;TSource&gt;, Func&lt;TSource, bool&gt;)">SingleOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource Single<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                TSource result = default(TSource);
                bool found = false;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                    {
                        if (found == true) //more than one
                            throw new InvalidOperationException();

                        found = true;
                        result = item;
                    }
                }

                if (found)
                    return result;
                else //no match
                    throw new InvalidOperationException();
            }

            #endregion

            #region 1.13.6 SingleOrDefault

            /// <summary>
            /// Returns the single element of a Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned. An <c>InvalidOperationException</c> is thrown if the source sequence contains more than one element.
            /// </summary>
            /// <typeparam name="TSource">Type of the element in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the single element from.</param>
            /// <returns>The single element from the source Enumerable. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned. An <c>InvalidOperationException</c> is thrown if the source sequence contains more than one element.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="Single&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)">Single</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty.</remarks>
            public static TSource SingleOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    TSource single = enumerator.Current;
                    if (!enumerator.MoveNext()) //only one
                        return single;
                    else //more than one
                        throw new InvalidOperationException();
                }
                else //empty
                    return default(TSource);
            }

            /// <summary>
            /// Returns the single element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned. An <c>InvalidOperationException</c> is thrown if more than one element matching the <paramref name="predicate">predicate</paramref> is found.
            /// </summary>
            /// <typeparam name="TSource">Type of the element in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the single element from that matches the <paramref name="predicate">predicate</paramref> condition.</param>
            /// <param name="predicate">Predicate function to check elements of the source sequence to meet certain criteria. The single element of a sequence for which the <paramref name="predicate">predicate</paramref> function returns true will be returned.</param>
            /// <returns>The single element from the source sequence, matching the <paramref name="predicate">predicate</paramref>. If no element matches the <paramref name="predicate">predicate</paramref> or if the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned. An <c>InvalidOperationException</c> is thrown if more than one element matching the <paramref name="predicate">predicate</paramref> is found.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="Single&lt;TSource&gt;(IEnumerable&lt;TSource&gt;, Func&lt;TSource, bool&gt;)">Single</see> to throw an <c>ArgumentNullException</c> in case the sequence is empty or no matching element is found.</remarks>
            public static TSource SingleOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                TSource result = default(TSource);
                bool found = false;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                    {
                        if (found == true) //more than one
                            throw new InvalidOperationException();

                        found = true;
                        result = item;
                    }
                }

                if (found)
                    return result;
                else //no match
                    return default(TSource);
            }

            #endregion

            #region 1.13.7 ElementAt

            /// <summary cref="ElementAt">
            /// Return the element at a given index in a Enumerable. The operator first checks whether the source sequence implements IList&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. If so, the source sequence’s implementation of IList&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; is used to obtain the element at the given index. Otherwise, the source sequence is enumerated until index elements have been skipped, and the element found at that position in the sequence is returned. An <c>ArgumentOutOfRangeException</c> is thrown if the index is less than zero or greater than or equal to the number of elements in the Enumerable.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the element on the specified position from.</param>
            /// <param name="index">Index of the element to retrieve from the Enumerable.</param>
            /// <returns>The element on the position indicated by <paramref name="index">index</paramref> in the Enumerable. An <c>ArgumentOutOfRangeException</c> is thrown if the index is less than zero or greater than or equal to the number of elements in the Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="ElementAtOrDefault">ElementAtOrDefault</see> to return <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> in case the <paramref name="index">index</paramref> is less than zero or greater than or equal to the number of elements in the Enumerable.</remarks>
            public static TSource ElementAt<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 int index)
            {
                if (source == null)
                    throw new ArgumentNullException();

                if (index < 0)
                    throw new ArgumentOutOfRangeException();

                if (source is IList<TSource>)
                    return ((IList<TSource>)source)[index];

                int i = 0;
                foreach (TSource item in source)
                    if (i++ == index)
                        return item;

                throw new ArgumentOutOfRangeException();
            }

            #endregion

            #region 1.13.8 ElementAtOrDefault

            /// <summary cref="ElementAtOrDefault">
            /// Return the element at a given index in a Enumerable. The operator first checks whether the source sequence implements IList&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. If so, the source sequence’s implementation of IList&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; is used to obtain the element at the given index. Otherwise, the source sequence is enumerated until index elements have been skipped, and the element found at that position in the sequence is returned. If the index is less than zero or greater than or equal to the number of elements in the sequence, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to get the element on the specified position from.</param>
            /// <param name="index">Index of the element to retrieve from the Enumerable.</param>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;. Use <see cref="ElementAt">ElementAt</see> to throw an <c>ArgumentOutOfRangeException</c> in case the <paramref name="index">index</paramref> is less than zero or greater than or equal to the number of elements in the Enumerable.</remarks>
            public static TSource ElementAtOrDefault<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 int index)
            {
                if (source == null)
                    throw new ArgumentNullException();

                if (index < 0)
                    return default(TSource);

                IList<TSource> lst = source as IList<TSource>;
                if (lst != null)
                {
                    if (index >= lst.Count)
                        return default(TSource);
                    else
                        return lst[index];
                }

                int i = 0;
                foreach (TSource item in source)
                    if (i++ == index)
                        return item;

                return default(TSource);
            }

            #endregion

            #region 1.13.9 DefaultIfEmpty

            /// <summary>
            /// Supplies a default element for an empty Enumerable. When the returned object is enumerated, it enumerates the source sequence and yields its elements. If the source sequence is empty, <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> is yielded in place of an empty Enumerable.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to be yielded if not empty. Otherwise <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c> will be yielded.</param>
            /// <returns>A sequence identical to the source sequence in case it's not empty, otherwise a singleton sequence yielding <c>default(<typeparamref name="TSource">TSource</typeparamref>)</c>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                return DefaultIfEmpty<TSource>(source, default(TSource));
            }

            /// <summary>
            /// Supplies a default element for an empty Enumerable. When the returned object is enumerated, it enumerates the source sequence and yields its elements. If the source sequence is empty, the <paramref name="defaultValue">defaultValue</paramref> is yielded in place of an empty Enumerable.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to be yielded if not empty. Otherwise <paramref name="defaultValue">defaultValue</paramref> will be yielded.</param>
            /// <param name="defaultValue">The value to be yielded if the source sequence is empty.</param>
            /// <returns>A sequence identical to the source sequence in case it's not empty, otherwise a singleton sequence yielding <paramref name="defaultValue">defaultValue</paramref>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 TSource defaultValue)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return _DefaultIfEmpty(source, defaultValue);
            }

            private static IEnumerable<TSource> _DefaultIfEmpty<TSource>(
                IEnumerable<TSource> source,
                TSource defaultValue)
            {
                IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    yield return defaultValue;
                else
                    do
                    {
                        yield return enumerator.Current;
                    } while (enumerator.MoveNext());
            }

            #endregion

            #endregion

            #region 1.14 Generation operators

            #region 1.14.1 Range

            /// <summary>
            /// Generates a sequence of integral numbers. An <c>ArgumentOutOfRangeException</c> is thrown if <c><paramref name="count">count</paramref></c> is less than zero or if <c><paramref name="start">start</paramref> + <paramref name="count">count</paramref> - 1</c> is larger than <c>int.MaxValue</c>. When the returned object is enumerated, it yields <paramref name="count">count</paramref> sequential integers starting with the value <paramref name="start">start</paramref>.
            /// </summary>
            /// <param name="start">First integer to be yielded.</param>
            /// <param name="count">Number of integers to be yielded.</param>
            /// <returns>A sequence with <paramref name="count">count</paramref> sequential integers starting with the value <paramref name="start">start</paramref>.</returns>
            public static IEnumerable<int> Range(int start, int count)
            {
                if (count < 0 || (long)start + count - 1 > int.MaxValue)
                    throw new ArgumentOutOfRangeException();

                return _Range(start, count);
            }

            private static IEnumerable<int> _Range(int start, int count)
            {
                for (int i = start; i <= start + count - 1; i++)
                    yield return i;
            }

            #endregion

            #region 1.14.2 Repeat

            /// <summary>
            /// Generates a sequence by repeating a <paramref name="element">value</paramref> a <paramref name="count">number</paramref> of times.
            /// </summary>
            /// <typeparam name="TResult">Type of the <paramref name="element">element</paramref> to be repeated in the result Enumerable.</typeparam>
            /// <param name="element">Element to be repeated <paramref name="count">count</paramref> times in the result Enumerable.</param>
            /// <param name="count">Number of times to repeat the <paramref name="element">element</paramref>.</param>
            /// <returns>A sequence with the <paramref name="element">element</paramref> repeated <paramref name="count">count</paramref> times.</returns>
            public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
            {
                if (count < 0)
                    throw new ArgumentOutOfRangeException();

                return _Repeat(element, count);
            }

            private static IEnumerable<TResult> _Repeat<TResult>(TResult element, int count)
            {
                for (int i = 0; i < count; i++)
                    yield return element;
            }

            #endregion

            #region 1.14.3 Empty

            /// <summary>
            /// Returns an empty sequence of a given type <typeparamref name="TResult">TResult</typeparamref>. When the returned object is enumerated, nothing is yielded.
            /// </summary>
            /// <typeparam name="TResult">Type of the empty sequence to be yielded.</typeparam>
            /// <returns>An empty sequence of the given type <typeparamref name="TResult">TResult</typeparamref>.</returns>
            public static IEnumerable<TResult> Empty<TResult>()
            {
                yield break;
            }

            #endregion

            #endregion

            #region 1.15 Quantifiers

            #region 1.15.1 Any

            /// <summary>
            /// Checks whether a sequence contains any elements.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to check.</param>
            /// <returns>True if the sequence contains any elements, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool Any<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                return source.GetEnumerator().MoveNext();
            }

            /// <summary>
            /// Checks whether any element of a sequence satisfies a <paramref name="predicate">condition</paramref>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to check for any elements matching the <paramref name="predicate">condition</paramref>.</param>
            /// <param name="predicate">Predicate used to check every element whether it matches the underlying condition.</param>
            /// <returns>True if the sequence contains any elements matching the <paramref name="predicate">predicate</paramref>, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool Any<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                foreach (TSource item in source)
                    if (predicate(item))
                        return true;

                return false;
            }

            #endregion

            #region 1.15.2 All

            /// <summary>
            /// Checks whether all elements of a sequence satisfy a <paramref name="predicate">condition</paramref>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to check whether all elements match the <paramref name="predicate">condition</paramref>.</param>
            /// <param name="predicate">Predicate used to check every element whether it matches the underlying condition.</param>
            /// <returns>True if all elements in the sequence match the <paramref name="predicate">predicate</paramref>, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool All<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                foreach (TSource item in source)
                    if (!predicate(item))
                        return false;

                return true;
            }

            #endregion

            #region 1.15.3 Contains

            /// <summary>
            /// Checks whether a sequence contains a <paramref name="value">value</paramref>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to be searched for the <paramref name="value">value</paramref>.</param>
            /// <param name="value">Value to be searched for in the Enumerable.</param>
            /// <returns>True if the <paramref name="value">value</paramref> was found, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool Contains<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 TSource value)
            {
                return Contains<TSource>(source, value, EqualityComparer<TSource>.Default);
            }

            /// <summary>
            /// Checks whether a sequence contains a <paramref name="value">value</paramref>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to be searched for the <paramref name="value">value</paramref>.</param>
            /// <param name="value">Value to be searched for in the Enumerable.</param>
            /// <param name="comparer">Comparer used to check for equality.</param>
            /// <returns>True if the <paramref name="value">value</paramref> was found, false otherwise.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static bool Contains<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 TSource value,
                IEqualityComparer<TSource> comparer)
            {
                if (source == null || comparer == null)
                    throw new ArgumentNullException();

                ICollection<TSource> collection = source as ICollection<TSource>;
                if (collection != null)
                    return collection.Contains(value);

                foreach (TSource item in source)
                    if (comparer.Equals(item, value))
                        return true;

                return false;
            }

            #endregion

            #endregion

            #region 1.16 Aggregate operators

            #region 1.16.1 Count

            /// <summary>
            /// Counts the number of elements in a Enumerable. If the source sequence implements ICollection&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;, the sequence's implementation of ICollection&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; is used to obtain the element count. Otherwise, the source sequence is enumerated to count the number of elements. An <c>OverflowException</c> is thrown is the count exceeds <c>int.MaxValue</c>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to count the elements of.</param>
            /// <returns>The number of elements in the Enumerable. An <c>OverflowException</c> is thrown is the count exceeds <c>int.MaxValue</c>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int Count<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                ICollection<TSource> collection = source as ICollection<TSource>;
                if (collection != null)
                    return collection.Count;

                checked
                {
                    int i = 0;
                    foreach (TSource item in source)
                        i++;

                    return i;
                }
            }

            /// <summary>
            /// Counts the number of elements in a sequence that match a <paramref name="predicate">predicate</paramref>. An <c>OverflowException</c> is thrown is the count exceeds <c>int.MaxValue</c>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to count the elements of.</param>
            /// <param name="predicate">Predicate to evaluate for each element in the source Enumerable. If the evaluation returns true, the element is included in the count.</param>
            /// <returns>The number of elements in the sequence that match the <paramref name="predicate">predicate</paramref>. An <c>OverflowException</c> is thrown is the count exceeds <c>int.MaxValue</c>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int Count<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                int i = 0;
                foreach (TSource item in source)
                    if (predicate(item))
                        i++;

                return i;
            }

            #endregion

            #region 1.16.2 LongCount

            /// <summary>
            /// Counts the number of elements in a sequence by enumerating it.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to count the elements of.</param>
            /// <returns>The number of elements in the Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long LongCount<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                long i = 0;
                foreach (TSource item in source)
                    i++;

                return i;
            }

            /// <summary>
            /// Counts the number of elements in a sequence that match a <paramref name="predicate">predicate</paramref>.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to count the elements of.</param>
            /// <param name="predicate">Predicate to evaluate for each element in the source Enumerable. If the evaluation returns true, the element is included in the count.</param>
            /// <returns>The number of elements in the sequence that match the <paramref name="predicate">predicate</paramref>.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long LongCount<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, bool> predicate)
            {
                if (source == null || predicate == null)
                    throw new ArgumentNullException();

                long i = 0;
                foreach (TSource item in source)
                    if (predicate(item))
                        i++;

                return i;
            }

            #endregion

            #region 1.16.3 Sum

            #region int

            /// <summary>
            /// Computes the integer sum of a sequence of integer values. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The integer sum of the sequence, 0 if the sequence is empty. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int&gt;.</remarks>
            public static int Sum(
#if CS30
                this IEnumerable<int> source
#else
IEnumerable<int> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    int sum = 0;
                    foreach (int i in source)
                        sum += i;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the integer sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source Enumerable.</param>
            /// <returns>The integer sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    int sum = 0;
                    foreach (TSource item in source)
                        sum += selector(item);

                    return sum;
                }
            }

            #endregion

            #region int?

            /// <summary>
            /// Computes the integer sum of a sequence of integer values excluding null values. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The integer sum of the sequence excluding null values, 0 if the sequence is empty. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int?&gt;.</remarks>
            public static int? Sum(
#if CS30
                this IEnumerable<int?> source
#else
IEnumerable<int?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    int sum = 0;
                    foreach (int? i in source)
                        if (i != null)
                            sum += i.Value;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the integer sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source Enumerable.</param>
            /// <returns>The integer sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as an <c>int</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int? Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    int sum = 0;
                    foreach (TSource item in source)
                    {
                        int? i = selector(item);
                        if (i != null)
                            sum += i.Value;
                    }

                    return sum;
                }
            }

            #endregion

            #region long

            /// <summary>
            /// Computes the long sum of a sequence of long values. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The long sum of the sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long&gt;.</remarks>
            public static long Sum(
#if CS30
                this IEnumerable<long> source
#else
IEnumerable<long> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    foreach (long i in source)
                        sum += i;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the long sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source Enumerable.</param>
            /// <returns>The long sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    foreach (TSource item in source)
                        sum += selector(item);

                    return sum;
                }
            }

            #endregion

            #region long?

            /// <summary>
            /// Computes the long sum of a sequence of long values excluding null values. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The long sum of the sequence excluding null values, 0 if the sequence is empty. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long?&gt;.</remarks>
            public static long? Sum(
#if CS30
                this IEnumerable<long?> source
#else
IEnumerable<long?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    foreach (long? i in source)
                        if (i != null)
                            sum += i.Value;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the long sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source Enumerable.</param>
            /// <returns>The long sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>long</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long? Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    foreach (TSource item in source)
                    {
                        long? i = selector(item);
                        if (i != null)
                            sum += i.Value;
                    }

                    return sum;
                }
            }

            #endregion

            #region double

            /// <summary>
            /// Computes the double sum of a sequence of double values. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The double sum of the sequence, 0.0 if the sequence is empty. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double&gt;.</remarks>
            public static double Sum(
#if CS30
                this IEnumerable<double> source
#else
IEnumerable<double> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                double sum = 0.0;
                foreach (double i in source)
                    sum += i;

                return sum;
            }

            /// <summary>
            /// Computes the double sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source Enumerable.</param>
            /// <returns>The double sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0.0 if the sequence is empty. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                double sum = 0.0;
                foreach (TSource item in source)
                    sum += selector(item);

                return sum;
            }

            #endregion

            #region double?

            /// <summary>
            /// Computes the double sum of a sequence of double values excluding null values. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The double sum of the sequence excluding null values, 0.0 if the sequence is empty. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double?&gt;.</remarks>
            public static double? Sum(
#if CS30
                this IEnumerable<double?> source
#else
IEnumerable<double?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                double sum = 0.0;
                foreach (double? i in source)
                    if (i != null)
                        sum += i.Value;

                return sum;
            }

            /// <summary>
            /// Computes the double sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source Enumerable.</param>
            /// <returns>The double sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0.0 if the sequence is empty. If the sum is too large to represent as a <c>double</c>, a positive or negative infinity is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                double sum = 0.0;
                foreach (TSource item in source)
                {
                    double? i = selector(item);
                    if (i != null)
                        sum += i.Value;
                }

                return sum;
            }

            #endregion

            #region decimal

            /// <summary>
            /// Computes the decimal sum of a sequence of decimal values. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The decimal sum of the sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal&gt;.</remarks>
            public static decimal Sum(
#if CS30
                this IEnumerable<decimal> source
#else
IEnumerable<decimal> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    foreach (decimal i in source)
                        sum += i;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the decimal sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an decimal value for each element of the source Enumerable.</param>
            /// <returns>The decimal sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    foreach (TSource item in source)
                        sum += selector(item);

                    return sum;
                }
            }

            #endregion

            #region decimal?

            /// <summary>
            /// Computes the decimal sum of a sequence of decimal values excluding null values. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The decimal sum of the sequence excluding null values, 0 if the sequence is empty. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static decimal? Sum(
#if CS30
                this IEnumerable<decimal?> source
#else
IEnumerable<decimal?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    foreach (decimal? i in source)
                        if (i != null)
                            sum += i.Value;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the decimal sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an decimal value for each element of the source Enumerable.</param>
            /// <returns>The decimal sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>decimal</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal? Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    foreach (TSource item in source)
                    {
                        decimal? i = selector(item);
                        if (i != null)
                            sum += i.Value;
                    }

                    return sum;
                }
            }

            #endregion

            #region float

            /// <summary>
            /// Computes the decimal sum of a sequence of floating comma values. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The floating comma sum of the sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float&gt;.</remarks>
            public static float Sum(
#if CS30
                this IEnumerable<float> source
#else
IEnumerable<float> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    float sum = 0;
                    foreach (float i in source)
                        sum += i;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the decimal sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an float value for each element of the source Enumerable.</param>
            /// <returns>The floating comma sum of the <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    float sum = 0;
                    foreach (TSource item in source)
                        sum += selector(item);

                    return sum;
                }
            }

            #endregion

            #region float?

            /// <summary>
            /// Computes the floating comma sum of a sequence of decimal values excluding null values. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the sum for.</param>
            /// <returns>The floating comma sum of the sequence excluding null values, 0 if the sequence is empty. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float?&gt;.</remarks>
            public static float? Sum(
#if CS30
                this IEnumerable<float?> source
#else
IEnumerable<float?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    float sum = 0;
                    foreach (float? i in source)
                        if (i != null)
                            sum += i.Value;

                    return sum;
                }
            }

            /// <summary>
            /// Computes the decimal sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source Enumerable. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the sum operation on.</param>
            /// <param name="selector">Function returning an decimal value for each element of the source Enumerable.</param>
            /// <returns>The floating comma sum of the non-null <paramref name="selector">selector</paramref> function evaluations for each element in the source sequence, 0 if the sequence is empty. If the sum is too large to represent as a <c>float</c>, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float? Sum<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    float sum = 0;
                    foreach (TSource item in source)
                    {
                        float? i = selector(item);
                        if (i != null)
                            sum += i.Value;
                    }

                    return sum;
                }
            }

            #endregion

            #endregion

            #region 1.16.4 Min

            #region general

            /// <summary>
            /// Finds the minimum of a sequence of values by enumerating the sequence and comparing the values using their IComparable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; implementation, or, if the values do not implement that interface, the non-generic IComparable interface.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TSource Min<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                TSource min = enumerator.Current;

                if (min is IComparable<TSource>)
                {
                    IComparable<TSource> mmin = min as IComparable<TSource>;

                    while (enumerator.MoveNext())
                    {
                        TSource current = enumerator.Current;
                        if (mmin.CompareTo(current) > 0)
                            mmin = current as IComparable<TSource>;
                    }

                    return (TSource)mmin;
                }
                else if (min is IComparable)
                {
                    IComparable mmin = min as IComparable;

                    while (enumerator.MoveNext())
                    {
                        TSource current = enumerator.Current;
                        if (mmin.CompareTo(current) > 0)
                            mmin = current as IComparable;
                    }

                    return (TSource)mmin;
                }
                else
                    return default(TSource);
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting values, by comparing the values using their IComparable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; implementation, or, if the values do not implement that interface, the non-generic IComparable interface.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the <paramref name="selector">selector</paramref> result value to compute the minimum for.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a value of type <paramref name="TResult">TResult</paramref> for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TResult Min<TSource, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TResult> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                TResult min = selector(enumerator.Current);

                if (min is IComparable<TResult>)
                {
                    IComparable<TResult> mmin = min as IComparable<TResult>;

                    while (enumerator.MoveNext())
                    {
                        TResult current = selector(enumerator.Current);
                        if (mmin.CompareTo(current) > 0)
                            mmin = current as IComparable<TResult>;
                    }

                    return (TResult)mmin;
                }
                else if (min is IComparable)
                {
                    IComparable mmin = min as IComparable;

                    while (enumerator.MoveNext())
                    {
                        TResult current = selector(enumerator.Current);
                        if (mmin.CompareTo(current) > 0)
                            mmin = current as IComparable;
                    }

                    return (TResult)mmin;
                }
                else
                    return default(TResult);
            }

            #endregion

            #region int

            /// <summary>
            /// Finds the minimum of a sequence of integer values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int&gt;.</remarks>
            public static int Min(
#if CS30
                this IEnumerable<int> source
#else
IEnumerable<int> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<int> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                int min = enumerator.Current;
                while (enumerator.MoveNext())
                    min = Math.Min(min, enumerator.Current);

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting integer values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                int min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    min = Math.Min(min, selector(enumerator.Current));

                return min;
            }

            #endregion

            #region long

            /// <summary>
            /// Finds the minimum of a sequence of long values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long&gt;.</remarks>
            public static long Min(
#if CS30
                this IEnumerable<long> source
#else
IEnumerable<long> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<long> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                long min = enumerator.Current;
                while (enumerator.MoveNext())
                    min = Math.Min(min, enumerator.Current);

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting long values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                long min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    min = Math.Min(min, selector(enumerator.Current));

                return min;
            }

            #endregion

            #region double

            /// <summary>
            /// Finds the minimum of a sequence of double values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double&gt;.</remarks>
            public static double Min(
#if CS30
                this IEnumerable<double> source
#else
IEnumerable<double> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<double> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                double min = enumerator.Current;
                while (enumerator.MoveNext())
                    min = Math.Min(min, enumerator.Current);

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting double values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                double min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    min = Math.Min(min, selector(enumerator.Current));

                return min;
            }

            #endregion

            #region decimal

            /// <summary>
            /// Finds the minimum of a sequence of decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal&gt;.</remarks>
            public static decimal Min(
#if CS30
this IEnumerable<decimal> source
#else
IEnumerable<decimal> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<decimal> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                decimal min = enumerator.Current;
                while (enumerator.MoveNext())
                    min = Math.Min(min, enumerator.Current);

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal Min<TSource>(
#if CS30
this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                decimal min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    min = Math.Min(min, selector(enumerator.Current));

                return min;
            }

            #endregion

            #region float

            /// <summary>
            /// Finds the minimum of a sequence of floating comma values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float&gt;.</remarks>
            public static float Min(
#if CS30
                this IEnumerable<float> source
#else
IEnumerable<float> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<float> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                float min = enumerator.Current;
                while (enumerator.MoveNext())
                    min = Math.Min(min, enumerator.Current);

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                float min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    min = Math.Min(min, selector(enumerator.Current));

                return min;
            }

            #endregion

            #region int?

            /// <summary>
            /// Finds the minimum of a sequence of integer values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int?&gt;.</remarks>
            public static int? Min(
#if CS30
                this IEnumerable<int?> source
#else
IEnumerable<int?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<int?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                int? min = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    int? current = enumerator.Current;
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting integer values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int? Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                int? min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    int? current = selector(enumerator.Current);
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            #endregion

            #region long?

            /// <summary>
            /// Finds the minimum of a sequence of long values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long?&gt;.</remarks>
            public static long? Min(
#if CS30
                this IEnumerable<long?> source
#else
IEnumerable<long?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<long?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                long? min = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    long? current = enumerator.Current;
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting long values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long? Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                long? min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    long? current = selector(enumerator.Current);
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            #endregion

            #region double?

            /// <summary>
            /// Finds the minimum of a sequence of double values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double?&gt;.</remarks>
            public static double? Min(
#if CS30
                this IEnumerable<double?> source
#else
IEnumerable<double?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<double?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                double? min = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    double? current = enumerator.Current;
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting double values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                double? min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    double? current = selector(enumerator.Current);
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            #endregion

            #region decimal?

            /// <summary>
            /// Finds the minimum of a sequence of decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static decimal? Min(
#if CS30
this IEnumerable<decimal?> source
#else
IEnumerable<decimal?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<decimal?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                decimal? min = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    decimal? current = enumerator.Current;
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal? Min<TSource>(
#if CS30
this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                decimal? min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    decimal? current = selector(enumerator.Current);
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            #endregion

            #region float?

            /// <summary>
            /// Finds the minimum of a sequence of floating comma values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the minimum for.</param>
            /// <returns>The minimum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static float? Min(
#if CS30
                this IEnumerable<float?> source
#else
IEnumerable<float?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<float?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                float? min = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    float? current = enumerator.Current;
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            /// <summary>
            /// Finds the minimum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the minimum of the resulting decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the minimum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the minimum.</param>
            /// <returns>The minimum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float? Min<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                float? min = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    float? current = selector(enumerator.Current);
                    if (min != null && current != null)
                        min = Math.Min(min.Value, current.Value);
                }

                return min;
            }

            #endregion

            #endregion

            #region 1.16.5 Max

            #region general

            /// <summary>
            /// Finds the maximum of a sequence of values by enumerating the sequence and comparing the values using their IComparable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; implementation, or, if the values do not implement that interface, the non-generic IComparable interface.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TSource Max<TSource>(
#if CS30
                this IEnumerable<TSource> source
#else
IEnumerable<TSource> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                TSource max = enumerator.Current;

                if (max is IComparable<TSource>)
                {
                    IComparable<TSource> mmax = max as IComparable<TSource>;

                    while (enumerator.MoveNext())
                    {
                        TSource current = enumerator.Current;
                        if (mmax.CompareTo(current) < 0)
                            mmax = current as IComparable<TSource>;
                    }

                    return (TSource)mmax;
                }
                else if (max is IComparable)
                {
                    IComparable mmax = max as IComparable;

                    while (enumerator.MoveNext())
                    {
                        TSource current = enumerator.Current;
                        if (mmax.CompareTo(current) < 0)
                            mmax = current as IComparable;
                    }

                    return (TSource)mmax;
                }
                else
                    return default(TSource);
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting values, by comparing the values using their IComparable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt; implementation, or, if the values do not implement that interface, the non-generic IComparable interface.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TResult">Type of the <paramref name="selector">selector</paramref> result value to compute the maximum for.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a value of type <paramref name="TResult">TResult</paramref> for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TResult Max<TSource, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TResult> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                TResult max = selector(enumerator.Current);

                if (max is IComparable<TResult>)
                {
                    IComparable<TResult> mmax = max as IComparable<TResult>;

                    while (enumerator.MoveNext())
                    {
                        TResult current = selector(enumerator.Current);
                        if (mmax.CompareTo(current) < 0)
                            mmax = current as IComparable<TResult>;
                    }

                    return (TResult)mmax;
                }
                else if (max is IComparable)
                {
                    IComparable mmax = max as IComparable;

                    while (enumerator.MoveNext())
                    {
                        TResult current = selector(enumerator.Current);
                        if (mmax.CompareTo(current) < 0)
                            mmax = current as IComparable;
                    }

                    return (TResult)mmax;
                }
                else
                    return default(TResult);
            }

            #endregion

            #region int

            /// <summary>
            /// Finds the maximum of a sequence of integer values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int&gt;.</remarks>
            public static int Max(
#if CS30
                this IEnumerable<int> source
#else
IEnumerable<int> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<int> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                int max = enumerator.Current;
                while (enumerator.MoveNext())
                    max = Math.Max(max, enumerator.Current);

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting integer values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                int max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    max = Math.Max(max, selector(enumerator.Current));

                return max;
            }

            #endregion

            #region long

            /// <summary>
            /// Finds the maximum of a sequence of long values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long&gt;.</remarks>
            public static long Max(
#if CS30
                this IEnumerable<long> source
#else
IEnumerable<long> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<long> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                long max = enumerator.Current;
                while (enumerator.MoveNext())
                    max = Math.Max(max, enumerator.Current);

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting long values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                long max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    max = Math.Max(max, selector(enumerator.Current));

                return max;
            }

            #endregion

            #region double

            /// <summary>
            /// Finds the maximum of a sequence of double values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double&gt;.</remarks>
            public static double Max(
#if CS30
                this IEnumerable<double> source
#else
IEnumerable<double> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<double> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                double max = enumerator.Current;
                while (enumerator.MoveNext())
                    max = Math.Max(max, enumerator.Current);

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting double values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                double max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    max = Math.Max(max, selector(enumerator.Current));

                return max;
            }

            #endregion

            #region decimal

            /// <summary>
            /// Finds the maximum of a sequence of decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal&gt;.</remarks>
            public static decimal Max(
#if CS30
                this IEnumerable<decimal> source
#else
IEnumerable<decimal> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<decimal> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                decimal max = enumerator.Current;
                while (enumerator.MoveNext())
                    max = Math.Max(max, enumerator.Current);

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                decimal max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    max = Math.Max(max, selector(enumerator.Current));

                return max;
            }

            #endregion

            #region float

            /// <summary>
            /// Finds the maximum of a sequence of floating comma values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float&gt;.</remarks>
            public static float Max(
#if CS30
                this IEnumerable<float> source
#else
IEnumerable<float> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<float> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                float max = enumerator.Current;
                while (enumerator.MoveNext())
                    max = Math.Max(max, enumerator.Current);

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting decimal values. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                float max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                    max = Math.Max(max, selector(enumerator.Current));

                return max;
            }

            #endregion

            #region int?

            /// <summary>
            /// Finds the maximum of a sequence of integer values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int?&gt;.</remarks>
            public static int? Max(
#if CS30
                this IEnumerable<int?> source
#else
IEnumerable<int?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<int?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                int? max = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    int? current = enumerator.Current;
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting integer values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static int? Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                int? max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    int? current = selector(enumerator.Current);
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            #endregion

            #region long?

            /// <summary>
            /// Finds the maximum of a sequence of long values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long?&gt;.</remarks>
            public static long? Max(
#if CS30
                this IEnumerable<long?> source
#else
IEnumerable<long?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<long?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                long? max = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    long? current = enumerator.Current;
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting long values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static long? Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                long? max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    long? current = selector(enumerator.Current);
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            #endregion

            #region double?

            /// <summary>
            /// Finds the maximum of a sequence of double values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double?&gt;.</remarks>
            public static double? Max(
#if CS30
                this IEnumerable<double?> source
#else
IEnumerable<double?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<double?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                double? max = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    double? current = enumerator.Current;
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting double values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                double? max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    double? current = selector(enumerator.Current);
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            #endregion

            #region decimal?

            /// <summary>
            /// Finds the maximum of a sequence of decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static decimal? Max(
#if CS30
this IEnumerable<decimal?> source
#else
IEnumerable<decimal?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<decimal?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                decimal? max = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    decimal? current = enumerator.Current;
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal? Max<TSource>(
#if CS30
this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                decimal? max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    decimal? current = selector(enumerator.Current);
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            #endregion

            #region float?

            /// <summary>
            /// Finds the maximum of a sequence of floating comma values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <param name="source">Sequence to compute the maximum for.</param>
            /// <returns>The maximum value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static float? Max(
#if CS30
                this IEnumerable<float?> source
#else
IEnumerable<float?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                IEnumerator<float?> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                float? max = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    float? current = enumerator.Current;
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            /// <summary>
            /// Finds the maximum of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and finding the maximum of the resulting decimal values. A null value is returned if the source sequence is empty or if the source sequence only contains null values.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the maximum computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the maximum.</param>
            /// <returns>The maximum <paramref name="selector">selector</paramref> result value encountered while enumerating the source Enumerable. A null value is returned if the source sequence is empty or if the source sequence only contains null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float? Max<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    return null;

                float? max = selector(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    float? current = selector(enumerator.Current);
                    if (max != null && current != null)
                        max = Math.Max(max.Value, current.Value);
                }

                return max;
            }

            #endregion

            #endregion

            #region 1.16.6 Average

            #region int

            /// <summary>
            /// Computes the average of a sequence of integer values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int&gt;.</remarks>
            public static double Average(
#if CS30
                this IEnumerable<int> source
#else
IEnumerable<int> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (int item in source)
                    {
                        sum += item;
                        n++;
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the resulting values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        sum += selector(item);
                        n++;
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            #endregion

            #region long

            /// <summary>
            /// Computes the average of a sequence of long values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long&gt;.</remarks>
            public static double Average(
#if CS30
                this IEnumerable<long> source
#else
IEnumerable<long> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (int item in source)
                    {
                        sum += item;
                        n++;
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the resulting values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        sum += selector(item);
                        n++;
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            #endregion

            #region double

            /// <summary>
            /// Computes the average of a sequence of double values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double&gt;.</remarks>
            public static double Average(
#if CS30
                this IEnumerable<double> source
#else
IEnumerable<double> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                double sum = 0;
                long n = 0;
                foreach (double item in source)
                {
                    sum += item;
                    n++;
                }

                if (n != 0)
                    return sum / n;
                else
                    throw new InvalidOperationException();
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the resulting values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                double sum = 0;
                long n = 0;
                foreach (TSource item in source)
                {
                    sum += selector(item);
                    n++;
                }

                if (n != 0)
                    return sum / n;
                else
                    throw new InvalidOperationException();
            }

            #endregion

            #region decimal

            /// <summary>
            /// Computes the average of a sequence of decimal values.  If the sequence is empty, an <c>ArgumentNullException</c> is thrown.If the sum of the elements is too large to represent in a decimal, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source Enumerable.  If the sequence is empty, an <c>ArgumentNullException</c> is thrown.If the sum of the elements is too large to represent in a decimal, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal&gt;.</remarks>
            public static decimal Average(
#if CS30
                this IEnumerable<decimal> source
#else
IEnumerable<decimal> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    long n = 0;
                    foreach (decimal item in source)
                    {
                        sum += item;
                        n++;
                    }

                    if (n != 0)
                        return sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the resulting values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a decimal, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a decimal, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    decimal sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        sum += selector(item);
                        n++;
                    }

                    if (n != 0)
                        return sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            #endregion

            #region float

            /// <summary>
            /// Computes the average of a sequence of floating comma values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float&gt;.</remarks>
            public static float Average(
#if CS30
                this IEnumerable<float> source
#else
IEnumerable<float> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    double sum = 0;
                    long n = 0;
                    foreach (float item in source)
                    {
                        sum += item;
                        n++;
                    }

                    if (n != 0)
                        return (float)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the resulting values. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty, an <c>ArgumentNullException</c> is thrown. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    double sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        sum += selector(item);
                        n++;
                    }

                    if (n != 0)
                        return (float)sum / n;
                    else
                        throw new InvalidOperationException();
                }
            }

            #endregion

            #region int?

            /// <summary>
            /// Computes the average of a sequence of integer values excluding null values. If the sequence is empty or contains only null values, null is returned. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source sequence excluding null values, null if the sequence is empty or contains only null values. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;int?&gt;.</remarks>
            public static double? Average(
#if CS30
                this IEnumerable<int?> source
#else
IEnumerable<int?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (int? item in source)
                    {
                        if (item != null)
                        {
                            sum += item.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        return null;
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the non-null resulting values. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the non-null <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, int?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        int? res = selector(item);
                        if (res != null)
                        {
                            sum += res.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        return null;
                }
            }

            #endregion

            #region long?

            /// <summary>
            /// Computes the average of a sequence of long values excluding null values. If the sequence is empty or contains only null values, null is returned. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source sequence excluding null values, null if the sequence is empty or contains only null values. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;long?&gt;.</remarks>
            public static double? Average(
#if CS30
                this IEnumerable<long?> source
#else
IEnumerable<long?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (long? item in source)
                    {
                        if (item != null)
                        {
                            sum += item.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        return null;
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the non-null resulting values. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a long value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the non-null <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, long?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    long sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        long? res = selector(item);
                        if (res != null)
                        {
                            sum += res.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (double)sum / n;
                    else
                        return null;
                }
            }

            #endregion

            #region double?

            /// <summary>
            /// Computes the average of a sequence of double values excluding null values. If the sequence is empty or contains only null values, null is returned.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source sequence excluding null values, null if the sequence is empty or contains only null values.</returns>
            /// <remarks>Extension method for IEnumerable&lt;double?&gt;.</remarks>
            public static double? Average(
#if CS30
                this IEnumerable<double?> source
#else
IEnumerable<double?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                double sum = 0;
                long n = 0;
                foreach (double? item in source)
                {
                    if (item != null)
                    {
                        sum += item.Value;
                        n++;
                    }
                }

                if (n != 0)
                    return sum / n;
                else
                    return null;
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the non-null resulting values. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a double value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the non-null <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static double? Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, double?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                double sum = 0;
                long n = 0;
                foreach (TSource item in source)
                {
                    double? res = selector(item);
                    if (res != null)
                    {
                        sum += res.Value;
                        n++;
                    }
                }

                if (n != 0)
                    return sum / n;
                else
                    return null;
            }

            #endregion

            #region decimal?

            /// <summary>
            /// Computes the average of a sequence of decimal values excluding null values. If the sequence is empty or contains only null values, null is returned. If the sum of the elements is too large to represent in a decimal, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source sequence excluding null values, null if the sequence is empty or contains only null values. If the sum of the elements is too large to represent in a decimal, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;decimal?&gt;.</remarks>
            public static decimal? Average(
#if CS30
                this IEnumerable<decimal?> source
#else
IEnumerable<decimal?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                decimal sum = 0;
                long n = 0;
                foreach (decimal? item in source)
                {
                    if (item != null)
                    {
                        sum += item.Value;
                        n++;
                    }
                }

                if (n != 0)
                    return sum / n;
                else
                    return null;
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the non-null resulting values. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a decimal, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning a decimal value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the non-null <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a decimal, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static decimal? Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, decimal?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                decimal sum = 0;
                long n = 0;
                foreach (TSource item in source)
                {
                    decimal? res = selector(item);
                    if (res != null)
                    {
                        sum += res.Value;
                        n++;
                    }
                }

                if (n != 0)
                    return sum / n;
                else
                    return null;
            }

            #endregion

            #region float?

            /// <summary>
            /// Computes the average of a sequence of floating comma values excluding null values. If the sequence is empty or contains only null values, null is returned. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <param name="source">Sequence to compute the average for.</param>
            /// <returns>The average of the source sequence excluding null values, null if the sequence is empty or contains only null values. If the sum of the elements is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;float?&gt;.</remarks>
            public static double? Average(
#if CS30
                this IEnumerable<float?> source
#else
IEnumerable<float?> source
#endif
)
            {
                if (source == null)
                    throw new ArgumentNullException();

                checked
                {
                    double sum = 0;
                    long n = 0;
                    foreach (float? item in source)
                    {
                        if (item != null)
                        {
                            sum += item.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (float)sum / n;
                    else
                        return null;
                }
            }

            /// <summary>
            /// Computes the average of a sequence of by enumerating the sequence, invoking the <paramref name="selector">selector</paramref> function for each element and calculating the average of the non-null resulting values. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to perform the average computation on.</param>
            /// <param name="selector">Function returning an integer value for each element of the source sequence, used to compute the average.</param>
            /// <returns>The average of the non-null <paramref name="selector">selector</paramref> result values encountered while enumerating the source Enumerable. If the sequence is empty or if the <paramref name="selector">selector</paramref> only returned null values, null is returned. If the sum of the result values is too large to represent in a long, an <c>OverflowException</c> is thrown.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static float? Average<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, float?> selector)
            {
                if (source == null || selector == null)
                    throw new ArgumentNullException();

                checked
                {
                    float sum = 0;
                    long n = 0;
                    foreach (TSource item in source)
                    {
                        float? res = selector(item);
                        if (res != null)
                        {
                            sum += res.Value;
                            n++;
                        }
                    }

                    if (n != 0)
                        return (float)sum / n;
                    else
                        return null;
                }
            }

            #endregion

            #endregion

            #region 1.16.7 Aggregate

            /// <summary>
            /// Applies an aggregation function over a Enumerable. The operator uses the first element of the sequence as the seed value which is then assigned to an internal accumulator. It then enumerates the source sequence, repeatedly computing the next accumulator value by invoking the specified <paramref name="func">function</paramref> with the current accumulator value as the first argument and the current sequence element as the second argument. The final accumulator value is returned as the result. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <param name="source">Sequence to compute the aggregation for.</param>
            /// <param name="func">Function to calculate the aggregated value based on the current internal accumulator value and the current element from the source Enumerable. function is called repeatedly for each element of the source sequence in order to obtain the aggregation value.</param>
            /// <returns>The aggregated value of the source sequence obtained by calling the <paramref name="func">aggregation function</paramref> repeatedly for each element in the sequence, starting with the first element as a seed value and using an internal accumulator. An <c>InvalidOperationException</c> is thrown if the source sequence is empty.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TSource Aggregate<TSource>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 Func<TSource, TSource, TSource> func)
            {
                if (source == null || func == null)
                    throw new ArgumentNullException();

                IEnumerator<TSource> enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new InvalidOperationException();

                TSource result = enumerator.Current;

                while (enumerator.MoveNext())
                    result = func(result, enumerator.Current);

                return result;
            }

            /// <summary>
            /// Applies an aggregation function over a Enumerable. The operator starts by assigning the <paramref name="seed">seed</paramref> value to an internal accumulator. It then enumerates the source sequence, repeatedly computing the next accumulator value by invoking the specified <paramref name="func">function</paramref> with the current accumulator value as the first argument and the current sequence element as the second argument. The final accumulator value is returned as the result.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TAccumulate">Type of the <paramref name="func">aggregation function</paramref>'s first parameter and the <paramref name="seed">seed</paramref> value.</typeparam>
            /// <param name="source">Sequence to compute the aggregation for.</param>
            /// <param name="seed">Seed value to assign to the internal accumulator before starting the aggregation calculation.</param>
            /// <param name="func">Function to calculate the aggregated value based on the current internal accumulator value and the current element from the source Enumerable. function is called repeatedly for each element of the source sequence in order to obtain the aggregation value.</param>
            /// <returns>The aggregated value of the source sequence obtained by calling the <paramref name="func">aggregation function</paramref> repeatedly for each element in the sequence, starting with the specified <paramref name="seed">seed</paramref> value and using an internal accumulator.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TAccumulate Aggregate<TSource, TAccumulate>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 TAccumulate seed,
                Func<TAccumulate, TSource, TAccumulate> func)
            {
                if (source == null || func == null)
                    throw new ArgumentNullException();

                TAccumulate result = seed;

                foreach (TSource item in source)
                    result = func(result, item);

                return result;
            }

            /// <summary>
            /// Applies an aggregation function over a Enumerable. The operator starts by assigning the <paramref name="seed">seed</paramref> value to an internal accumulator. It then enumerates the source sequence, repeatedly computing the next accumulator value by invoking the specified <paramref name="func">function</paramref> with the current accumulator value as the first argument and the current sequence element as the second argument. The final accumulator value is returned as the result. The specified function is used to select the result value.
            /// </summary>
            /// <typeparam name="TSource">Type of the elements in the source Enumerable.</typeparam>
            /// <typeparam name="TAccumulate">Type of the <paramref name="func">aggregation function</paramref>'s first parameter and the <paramref name="seed">seed</paramref> value.</typeparam>
            /// <typeparam name="TResult">Aggregation result after selection by <param name="resultSelector">the result selector</param>.</typeparam>
            /// <param name="source">Sequence to compute the aggregation for.</param>
            /// <param name="seed">Seed value to assign to the internal accumulator before starting the aggregation calculation.</param>
            /// <param name="func">Function to calculate the aggregated value based on the current internal accumulator value and the current element from the source Enumerable. function is called repeatedly for each element of the source sequence in order to obtain the aggregation value.</param>
            /// <returns>The aggregated value of the source sequence obtained by calling the <paramref name="func">aggregation function</paramref> repeatedly for each element in the sequence, starting with the specified <paramref name="seed">seed</paramref> value and using an internal accumulator.</returns>
            /// <remarks>Extension method for IEnumerable&lt;<typeparamref name="TSource">TSource</typeparamref>&gt;.</remarks>
            public static TResult Aggregate<TSource, TAccumulate, TResult>(
#if CS30
                this IEnumerable<TSource> source,
#else
IEnumerable<TSource> source,
#endif
 TAccumulate seed,
                Func<TAccumulate, TSource, TAccumulate> func,
                Func<TAccumulate, TResult> resultSelector)
            {
                if (source == null || func == null)
                    throw new ArgumentNullException();

                TAccumulate result = seed;

                foreach (TSource item in source)
                    result = func(result, item);

                return resultSelector(result);
            }

            #endregion

            #endregion
        }

        #endregion

        #region 1.8.1 OrderBy / ThenBy (bis)

        #region OrderedSequence class

        /// <summary>
        /// Represents a (hierarchically) ordered Enumerable.
        /// </summary>
        /// <typeparam name="TElement">Type of the elements in the ordered Enumerable.</typeparam>
        internal class OrderedSequence<TElement> : System.Collections.Generic.IEnumerable<TElement>, BdsSoft.Linq.IOrderedSequence<TElement>
        {
            /// <summary>
            /// Used when ordered sequence is at the leaf level. At the leaf level, an ordered sequence contains a list with the ordered groups of elements (each represented as a List&lt;<typeparamref name="TElement">TElement</typeparamref>&gt;).
            /// </summary>
            /// <example>
            /// Assume a <c>new List&lt;T&gt; { "AAA", "AAB", "ABA", "ABB", "BAA", "BAB", "BBA", "BBB" }</c> sorted by the first character of the strings. The corresponding <c>OrderedSequence&lt;T&gt;</c> then contains: 
            /// <c>_lst = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "AAA", "AAB", "ABA", "ABB" }, new List&lt;T&gt; { "BAA", "BAB", "BBA", "BBB" } }</c>.
            /// </example>
            private IList<List<TElement>> _lst;

            /// <summary>
            /// Used when ordered sequence is not at the leaf level. Inside the tree, an ordered sequence contains a list of ordered sequences (each represented as a OrderedSequence&lt;<typeparamref name="TElement">TElement</typeparamref>&gt;) one level deeper in the tree.
            /// </summary>
            /// <example>
            /// Assume a <c>new List&lt;T&gt; { "AAA", "AAB", "ABA", "ABB", "BAA", "BAB", "BBA", "BBB" }</c> sorted by the first character of the strings and then by the second character of the strings. The corresponding <c>OrderedSequence&lt;T&gt;</c> then contains: 
            /// <c>_children = new List&lt;OrderedSequence&lt;T&gt;&gt; { a, b }</c> 
            /// where 
            /// <c>a = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "AAA", "AAB" }, new List&lt;T&gt; { "ABA", "ABB" } }</c> and <c>b = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "BAA", "BAB" }, new List&lt;T&gt; { "BBA", "BBB" } }</c> 
            /// are both leaf-level ordered sequences.
            /// </example>
            private IList<IOrderedSequence<TElement>> _children;

            /// <summary>
            /// Indicates whether ordered sequence is at the leaf level or not.
            /// </summary>
            private bool _leaf;

            /// <summary>
            /// Creates a leaf-level ordered sequence with the sorted list of element groups.
            /// </summary>
            /// <param name="lst">
            /// Sorted list of element groups.
            /// <example><c>new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "AAA", "AAB", "ABA", "ABB" }, new List&lt;T&gt; { "BAA", "BAB", "BBA", "BBB" } }</c></example>
            /// </param>
            internal OrderedSequence(IList<List<TElement>> lst)
            {
                _lst = lst;
                _leaf = true;
            }

            /// <summary>
            /// Creates a non leaf-level ordered sequence with the sorted list of children ordered sequences.
            /// </summary>
            /// <param name="children">
            /// Sorted list of children ordered sequences.
            /// <example><c>new List&lt;OrderedSequence&lt;T&gt;&gt; { a, b }</c> where <c>a</c> and <c>b</c> are both <c>OrderedSequence&lt;T&gt;</c> instances</example>
            /// </param>
            internal OrderedSequence(IList<IOrderedSequence<TElement>> children)
            {
                _children = children;
                _leaf = false;
            }

            /// <summary>
            /// Indicates whether ordered sequence is at the leaf level.
            /// </summary>
            internal bool IsLeaf
            {
                get { return _leaf; }
            }

            /// <summary>
            /// Returns the sorted list of element groups if the ordered sequence is at the leaf level, null otherwise.
            /// </summary>
            internal IList<List<TElement>> Items
            {
                get { return _lst; }
            }

            /// <summary>
            /// Returns the sorted list of child OrderedSequence objects if the ordered sequence isn't at the leaf level, null otherwise.
            /// </summary>
            internal IList<IOrderedSequence<TElement>> Children
            {
                get { return _children; }
            }

            /// <summary>
            /// Promotes a leaf level ordered sequence
            /// </summary>
            /// <param name="children"></param>
            /// <example>
            /// Used when the leaf level ordered sequence characterized by 
            /// <c>_lst = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "AAA", "AAB", "ABA", "ABB" }, new List&lt;T&gt; { "BAA", "BAB", "BBA", "BBB" } }</c> 
            /// is sorted on the second character of the strings, resulting in 
            /// <c>_children = new List&lt;OrderedSequence&lt;T&gt;&gt; { a, b }</c> with <c>a = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "AAA", "AAB" }, new List&lt;T&gt; { "ABA", "ABB" } }</c> and <c>b = new List&lt;List&lt;T&gt;&gt; { new List&lt;T&gt; { "BAA", "BAB" }, new List&lt;T&gt; { "BBA", "BBB" } }</c>, 
            /// where the leaf level ordered sequence was patched to point to two new leaf level ordered sequences <c>a</c> and <c>b</c>.
            /// </example>
            internal void Patch(IList<IOrderedSequence<TElement>> children)
            {
                _children = children;
                _leaf = false;
            }

            /// <summary>
            /// Retrieves the generic enumerator used to enumerate the elements in the ordered sequence by traversing the OrderedSequence&lt;T&gt; tree on the leaf level (using recursion).
            /// </summary>
            /// <returns>Generic enumerator that yields the elements on the leaf level of the ordered Enumerable.</returns>
            public System.Collections.Generic.IEnumerator<TElement> GetEnumerator()
            {
                if (_leaf)
                    foreach (IEnumerable<TElement> item in _lst)
                        foreach (TElement child in item)
                            yield return child;
                else
                    foreach (IEnumerable<TElement> item in _children)
                        foreach (TElement child in item)
                            yield return child;
            }

            /// <summary>
            /// Retrieves the non-generic enumerator.
            /// </summary>
            /// <returns>Non-generic enumerator.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Makes a clone (deep copy) of the current ordered sequence (using recursion) in order to be able to <see cref="Patch">Patch</see> an <c>OrderedSequence&lt;T&gt;</c> tree without loss of the original tree.
            /// </summary>
            /// <returns>Deep copy of the current ordered Enumerable.</returns>
            internal OrderedSequence<TElement> Clone()
            {
                if (_leaf)
                    return new OrderedSequence<TElement>(new List<List<TElement>>(_lst));
                else
                {
                    List<IOrderedSequence<TElement>> lst = new List<IOrderedSequence<TElement>>();
                    foreach (OrderedSequence<TElement> child in _children)
                        lst.Add(child.Clone());
                    return new OrderedSequence<TElement>(lst);
                }
            }

            #region IOrderedSequence<T> Members

            public IOrderedSequence<TElement> CreateOrderedSequence<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            {
                if (keySelector == null)
                    throw new ArgumentNullException();

                OrderedSequence<TElement> res = this.Clone();
                ThenByInternal2<TKey>(res, keySelector, comparer, descending);
                return res;
            }

            internal static void ThenByInternal2<K>(OrderedSequence<TElement> o, Func<TElement, K> keySelector, IComparer<K> comparer, bool descending)
            {
                if (o.IsLeaf)
                {
                    List<IOrderedSequence<TElement>> lst = new List<IOrderedSequence<TElement>>();
                    foreach (IList<TElement> bucket in o.Items)
                        lst.Add(Enumerable.OrderByInternal2(bucket, keySelector, comparer, descending));

                    o.Patch(lst);
                }
                else
                {
                    foreach (OrderedSequence<TElement> child in o.Children)
                        ThenByInternal2<K>(child, keySelector, comparer, descending);
                }
            }


            #endregion
        }

        #endregion

        #endregion

        #region 1.9.1 GroupBy (bis)

        #region IGrouping interface

        /// <summary>
        /// Represents a grouping of a key element and a sequence of value elements.
        /// </summary>
        /// <typeparam name="TKey">Type of the key element.</typeparam>
        /// <typeparam name="TElement">Type of the value sequence elements.</typeparam>
        public interface IGrouping<TKey, TElement> : IEnumerable<TElement>
        {
            /// <summary>
            /// Gets the key of the grouping.
            /// </summary>
            TKey Key { get; }
        }

        #endregion

        #region IGrouping implementation

        /// <summary>
        /// Implementation of the <see cref="BdsSoft.Linq.IGrouping&lt;K, T&gt;">IGrouping</see> interface. Uses a List&lt;<typeparamref name="TElement">TElement</typeparamref>&gt; to store the values corresponding to the grouping key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key element.</typeparam>
        /// <typeparam name="TElement">Type of the value sequence elements.</typeparam>
        internal class Grouping<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
        {
            /// <summary>
            /// Key of the grouping.
            /// </summary>
            private TKey _key;

            /// <summary>
            /// Creates a new grouping for the specified key.
            /// </summary>
            /// <param name="key">Key of the grouping.</param>
            internal Grouping(TKey key)
            {
                _key = key;
            }

            /// <summary>
            /// Gets the key of the grouping.
            /// </summary>
            public TKey Key
            {
                get { return _key; }
            }
        }

        #endregion

        #endregion

        #region 1.11.5 ToLookup (bis)

        #region Lookup class

        /// <summary>
        /// Implements a one-to-many dictionary that maps keys to sequences of values.
        /// </summary>
        /// <typeparam name="TKey">Type of the keys.</typeparam>
        /// <typeparam name="TElement">Type of the values.</typeparam>
        public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
        {
            /// <summary>
            /// Dictionary mapping key values on an <c>IGrouping&lt;K,T&gt;</c> object containing a sequence of values.
            /// </summary>
            internal Dictionary<TKey, IGrouping<TKey, TElement>> dictionary;

            /// <summary>
            /// Keeps an ordered list of the keys, in order of addition.
            /// </summary>
            internal List<TKey> keys;

            /// <summary>
            /// Initializes a new one-to-many dictionary using the given <paramref name="comparer">comparer</paramref> for key comparison.
            /// </summary>
            /// <param name="comparer">Comparer used to compare keys when storing elements in the one-to-many dictionary.</param>
            internal Lookup(IEqualityComparer<TKey> comparer)
            {
                dictionary = new Dictionary<TKey, IGrouping<TKey, TElement>>(comparer);
                keys = new List<TKey>();
            }

            /// <summary>
            /// Gets the number of <c>IGrouping&lt;K,T&gt;</c> objects stored in the one-to-many dictionary.
            /// </summary>
            public int Count
            {
                get { return dictionary.Count; }
            }

            /// <summary>
            /// Retrieves a sequence of values corresponding to the given key.
            /// </summary>
            /// <param name="key">Key to return the value sequence for.</param>
            /// <returns>Value sequence corresponding to the given key.</returns>
            public IEnumerable<TElement> this[TKey key]
            {
                get { return dictionary[key]; }
            }

            /// <summary>
            /// Checks whether the one-to-many dictionary contains the specified key.
            /// </summary>
            /// <param name="key">Key to check for in the one-to-many dictionary.</param>
            /// <returns>True if the key was found in the one-to-many dictionary, false otherwise.</returns>
            public bool Contains(TKey key)
            {
                return dictionary.ContainsKey(key);
            }

            /// <summary>
            /// Retrieves the generic enumerator used to enumerate the elements in the one-to-many dictionary.
            /// </summary>
            /// <returns>Enumerator yielding the <c>IGrouping&lt;K,T&gt;</c> objects stored in the one-to-many dictionary in the order of addition.</returns>
            public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
            {
                foreach (TKey key in keys)
                    yield return dictionary[key];
            }

            /// <summary>
            /// Retrieves the non-generic enumerator used to enumerate the elements in the one-to-many dictionary.
            /// </summary>
            /// <returns>Enumerator yielding the <c>IGrouping&lt;K,T&gt;</c> objects stored in the one-to-many dictionary in the order of addition.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Adds an <c>IGrouping&lt;K,T&gt;</c> object to the one-to-many dictionary based on its key.
            /// </summary>
            /// <param name="item"><c>IGrouping&lt;K,T&gt;</c> object to be added to the one-to-many dictionary.</param>
            internal void Add(IGrouping<TKey, TElement> item)
            {
                TKey key = item.Key;
                keys.Add(key);
                dictionary.Add(key, item);
            }
        }

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Reverses the comparison of elements to result in a descending order.
        /// </summary>
        /// <typeparam name="TElement">Type of the elements to compare.</typeparam>
        internal class ReverseComparer<TElement> : IComparer<TElement>
        {
            /// <summary>
            /// Original comparer that will be reversed by ReverseComparer.
            /// </summary>
            private IComparer<TElement> _comparer;

            /// <summary>
            /// Creates a new reverse comparer based on a given comparer.
            /// </summary>
            /// <param name="comparer">Comparer to reverse the results from.</param>
            public ReverseComparer(IComparer<TElement> comparer)
            {
                _comparer = comparer;
            }

            /// <summary>
            /// Compares two objects x and y in reverse order as the original comparer.
            /// </summary>
            /// <param name="x">First object to be compared.</param>
            /// <param name="y">Second object to be compared.</param>
            /// <returns>If x &lt; y according to the original comparer, a positve value is returned. If x &gt; y according to the original comparer, a negative value is returned. If x and y are equal according to the original comparer, 0 is returned.</returns>
            public int Compare(TElement x, TElement y)
            {
                if (_comparer != null)
                    return _comparer.Compare(y, x);
                else if (x != null && x is IComparable)
                    return -(x as IComparable).CompareTo(y);
                else if (y != null && y is IComparable)
                    return (y as IComparable).CompareTo(x);
                else
                    throw new ArgumentException();
            }
        }

#if !CS30
        /// <summary>
        /// Naive HashSet&lt;T&gt; implementation for use in .NET 2.0.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <remarks>
        /// This class won't perform as good as the System.Collections.Generic.HashSet&lt;T&gt; class that ships in the .NET Framework 3.5.
        /// One could also replace the use of HashSet&lt;T&gt; with Set&lt;T&gt; from Wintellect PowerCollections at http://www.wintellect.com/PowerCollections.aspx.
        /// </remarks>
        internal class HashSet<T> : Dictionary<T, object>
        {
            public HashSet()
                : base()
            {
            }

            public HashSet(IEqualityComparer<T> comparer)
                : base(comparer)
            {
            }

            public void Add(T item)
            {
                base.Add(item, null);
            }

            public bool Contains(T item)
            {
                return base.ContainsKey(item);
            }
        }
#endif

        #endregion
    }
}
