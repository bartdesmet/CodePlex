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
    /// FEnumerable anamorphic constructor functions.
    /// </summary>
    public static partial class FEnumerable
    {
        /// <summary>
        /// Empty sequence constructor.
        /// </summary>
        /// <typeparam name="T">Result sequence element type.</typeparam>
        /// <returns>Empty sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Func<Func<Maybe<T>>> Empty<T>()
        {
            /* Direct implementation:
             * 
             * return () => () => new Option<T>.None()
             */
            return Ana(0, _ => false, _ => _, _ => default(T));
        }

        /// <summary>
        /// Infinite sequence constructor.
        /// </summary>
        /// <typeparam name="T">Result sequence element type.</typeparam>
        /// <param name="value">Element to repeat forever.</param>
        /// <returns>Infinite sequence.</returns>
        public static Func<Func<Maybe<T>>> Repeat<T>(T value)
        {
            /* Direct implementation:
             *
             * return () => () => new Option<T>.Some(value);
             */
            return Ana(0, _ => true, _ => _, _ => value);
        }

        /// <summary>
        /// Element repeating sequence constructor.
        /// </summary>
        /// <typeparam name="T">Result sequence element type.</typeparam>
        /// <param name="value">Element to repeat.</param>
        /// <param name="count">Number of times to repeat the element.</param>
        /// <returns>Element repeating sequence.</returns>
        public static Func<Func<Maybe<T>>> Repeat<T>(T value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Ana(0, n => n < count, n => n + 1, _ => value);
        }

        /// <summary>
        /// Single element sequence constructor.
        /// </summary>
        /// <typeparam name="T">Result sequence element type.</typeparam>
        /// <param name="value">Single element.</param>
        /// <returns>Single element sequence.</returns>
        public static Func<Func<Maybe<T>>> Return<T>(T value)
        {
            /* Direct implementation:
             *
             * return () =>
             * {
             *     int i = 0;
             *     return () => i++ == 0 ? (Option<T>)new Option<T>.Some(value) : new Option<T>.None();
             * };
             */
            return Repeat(value, 1);
        }

        /// <summary>
        /// Integral value range sequence constructor.
        /// </summary>
        /// <param name="start">First element in sequence.</param>
        /// <param name="count">Number of elements in sequence.</param>
        /// <returns>Integral value range sequence.</returns>
        public static Func<Func<Maybe<int>>> Range(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Ana(0, n => n < count, n => n + 1, n => start + n);
        }

        /// <summary>
        /// Generates the infinite sequence of natural numbers.
        /// </summary>
        /// <returns>Infinite sequence of natural numbers.</returns>
        public static Func<Func<Maybe<int>>> Naturals()
        {
            return Ana(0, _ => true, n => checked(n + 1), n => n);
        }
    }
}
