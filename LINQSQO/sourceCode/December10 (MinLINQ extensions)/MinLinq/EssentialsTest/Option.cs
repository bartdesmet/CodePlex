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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLinq;

namespace Tests
{
    [TestClass]
    public class Option
    {
        [TestMethod]
        public void NoneHasValue()
        {
            Assert.IsFalse(new Maybe<int>.None().HasValue);
        }

        [TestMethod]
        public void SomeHasValue()
        {
            Assert.IsTrue(new Maybe<int>.Some(1).HasValue);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void NoneValue()
        {
            var x = new Maybe<int>.None().Value;
        }

        [TestMethod]
        public void SomeValue()
        {
            int n = 42;
            Assert.AreEqual(n, new Maybe<int>.Some(42).Value);
        }

        [TestMethod]
        public void NoneToString()
        {
            Assert.AreEqual("None<Int32>()", new Maybe<int>.None().ToString());
        }

        [TestMethod]
        public void SomeToString()
        {
            int n = 42;
            Assert.AreEqual("Some<Int32>(" + n + ")", new Maybe<int>.Some(n).ToString());
            Assert.AreEqual("Some<String>(null)", new Maybe<string>.Some(null).ToString());
        }
    }
}
