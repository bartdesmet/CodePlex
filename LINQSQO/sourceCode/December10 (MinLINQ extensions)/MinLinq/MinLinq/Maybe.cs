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
    /// Optional object type.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public abstract class Maybe<T>
    {
        /// <summary>
        /// Gets whether an object is present.
        /// </summary>
        public abstract bool HasValue { get; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        public abstract T Value { get; }

        /// <summary>
        /// None optional type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public sealed class None : Maybe<T>
        {
            /// <summary>
            /// Gets whether an object is present. Always returns false for None.
            /// </summary>
            public override bool HasValue
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets the object. Always throws InvalidOperationException for None.
            /// </summary>
            public override T Value
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }

            /// <summary>
            /// Provides a friendly string representation.
            /// </summary>
            /// <returns>None&lt;T&gt;</returns>
            public override string ToString()
            {
                return "None<" + typeof(T).Name + ">()";
            }
        }

        /// <summary>
        /// Some optional type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public sealed class Some : Maybe<T>
        {
            /// <summary>
            /// Object stored in Some.
            /// </summary>
            private T _value;

            /// <summary>
            /// Creates a new Some object.
            /// </summary>
            /// <param name="value">Object stored in Some.</param>
            public Some(T value)
            {
                _value = value;
            }

            /// <summary>
            /// Gets whether an object is present. Always returns true for Some.
            /// </summary>
            public override bool HasValue
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets the object.
            /// </summary>
            public override T Value
            {
                get { return _value; }
            }

            /// <summary>
            /// Provides a friendly string representation.
            /// </summary>
            /// <returns>None&lt;T&gt;(Value)</returns>
            public override string ToString()
            {
                return "Some<" + typeof(T).Name + ">(" + (_value == null ? "null" : _value.ToString()) + ")";
            }
        }
    }
}
