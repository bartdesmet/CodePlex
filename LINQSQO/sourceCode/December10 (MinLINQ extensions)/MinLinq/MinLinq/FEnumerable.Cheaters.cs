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
using System.Linq;
using System.Reflection;

namespace MinLinq
{
    /// <summary>
    /// FEnumerable cheating functions, based on AsEnumerable/AsFNumerable.
    /// </summary>
    /// <remarks>Contains TODO items that can be brought to our world.</remarks>
    public static partial class FEnumerable
    {
        /// <summary>
        /// Sorts the elements in the sequence by the selected key value in ascending order.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> OrderBy<T, K>(this Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            return new OrderedWrapper<T>(source.AsEnumerable().OrderBy(keySelector)).GetEnumerator;
        }

        /// <summary>
        /// Sorts the elements in the sequence by the selected key value in descending order.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> OrderByDescending<T, K>(this Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            return new OrderedWrapper<T>(source.AsEnumerable().OrderByDescending(keySelector)).GetEnumerator;
        }

        /// <summary>
        /// Applies an n-ary ordering to the sequence, by the selected key value in ascending order.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> ThenBy<T, K>(this Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            return OrderedWrapper<T>.ThenBy(source, keySelector);
        }

        /// <summary>
        /// Applies an n-ary ordering to the sequence, by the selected key value in descending order.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> ThenByDescending<T, K>(this Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            return OrderedWrapper<T>.ThenByDescending(source, keySelector);
        }

        /// <summary>
        /// Returns the sequence or a singleton default value sequence if it's empty.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Original sequence or the default value.</returns>
        public static Func<Func<Maybe<T>>> DefaultIfEmpty<T>(this Func<Func<Maybe<T>>> source, T defaultValue)
        {
            return source.AsEnumerable().DefaultIfEmpty(defaultValue).AsFEnumerable();
        }

        /// <summary>
        /// Returns the sequence or a singleton default(T) sequence if it's empty.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Original sequence or the default value.</returns>
        public static Func<Func<Maybe<T>>> DefaultIfEmpty<T>(this Func<Func<Maybe<T>>> source)
        {
            return DefaultIfEmpty(source, default(T));
        }
    }

    /// <summary>
    /// Helper class to keep track of (cheating) IOrderedEnumerable objects.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    class OrderedWrapper<T>
    {
        /// <summary>
        /// Underlying source.
        /// </summary>
        private readonly IOrderedEnumerable<T> _source;

        /// <summary>
        /// Creates a new IOrderedEnumerable wrapper.
        /// </summary>
        /// <param name="source">Underlying source.</param>
        public OrderedWrapper(IOrderedEnumerable<T> source)
        {
            _source = source;
        }

        /// <summary>
        /// Creates a new wrapper for an n-ary ascending ordering.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>New n-ary ordering wrapper.</returns>
        public OrderedWrapper<T> ThenBy<K>(Func<T, K> keySelector)
        {
            return new OrderedWrapper<T>(_source.ThenBy(keySelector));
        }

        /// <summary>
        /// Creates a new wrapper for an n-ary descending ordering.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>New n-ary ordering wrapper.</returns>
        public OrderedWrapper<T> ThenByDescending<K>(Func<T, K> keySelector)
        {
            return new OrderedWrapper<T>(_source.ThenByDescending(keySelector));
        }

        /// <summary>
        /// Gets a FEnumerator for the source.
        /// </summary>
        /// <returns>FEnumerator delegate.</returns>
        public Func<Maybe<T>> GetEnumerator()
        {
            return _source.AsFEnumerable()();
        }

        /// <summary>
        /// Factory for an n-ary ascendingly ordered sequence.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> ThenBy<K>(Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            var src = source.Target as OrderedWrapper<T>;
            if (src == null)
                throw new InvalidOperationException();

            return src.ThenBy(keySelector).GetEnumerator;
        }

        /// <summary>
        /// Factory for an n-ary descendingly ordered sequence.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Ordered sequence.</returns>
        public static Func<Func<Maybe<T>>> ThenByDescending<K>(Func<Func<Maybe<T>>> source, Func<T, K> keySelector)
        {
            var src = source.Target as OrderedWrapper<T>;
            if (src == null)
                throw new InvalidOperationException();

            return src.ThenByDescending(keySelector).GetEnumerator;
        }
    }
}
