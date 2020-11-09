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
    /// Internal helpers.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Iterates over the sequence, yielding its elements.
        /// Used for the side-effect of proper enumerator disposal.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Elements in the source sequence.</returns>
        public static IEnumerable<T> Iterate<T>(this IEnumerable<T> source)
        {
            // Will ensure proper cleanup of the source's enumerator
            // through a fault handler.
            foreach (var item in source)
                yield return item;
        }

        /// <summary>
        /// Let construct as an extension method.
        /// </summary>
        /// <typeparam name="T">Function argument type.</typeparam>
        /// <typeparam name="R">Function result type.</typeparam>
        /// <param name="value">Function argument value.</param>
        /// <param name="f">Function.</param>
        /// <returns>Function evaluation result.</returns>
        public static R Let<T, R>(this T value, Func<T, R> f)
        {
            return f(value);
        }

        /// <summary>
        /// With construct as an extension method.
        /// </summary>
        /// <typeparam name="T">Action argument type.</typeparam>
        /// <param name="value">Action argument value.</param>
        /// <param name="f">Action.</param>
        /// <returns>The original value.</returns>
        public static T With<T>(this T value, Action<T> f)
        {
            f(value);
            return value;
        }
    }
}
