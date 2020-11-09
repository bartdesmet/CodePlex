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
        /// <summary>
        /// Produces an IEnumerable sequence for a FEnumerable one.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>IEnumerable sequence.</returns>
        public static IEnumerable<T> AsEnumerable<T>(this Func<Func<Maybe<T>>> source)
        {
            var e = source();
            Maybe<T>.Some current;
            while ((current = e() as Maybe<T>.Some) != null)
                yield return current.Value;
        }

        /// <summary>
        /// Produces a FEnumerable sequence for an IEnumerable one.
        /// </summary>
        /// <typeparam name="T">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>FEnumerable sequence.</returns>
        public static Func<Func<Maybe<T>>> AsFEnumerable<T>(this IEnumerable<T> source)
        {
            return () =>
            {
                // Iterate is part of our exception strategy
                var e = source.Iterate().GetEnumerator();
                return () => e.MoveNext() ? (Maybe<T>)new Maybe<T>.Some(e.Current) : new Maybe<T>.None();
            };
        }
    }
}
