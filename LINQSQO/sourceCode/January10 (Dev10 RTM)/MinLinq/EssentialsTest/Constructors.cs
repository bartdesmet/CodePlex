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
    public class Constructors
    {
        [TestMethod]
        public void Empty()
        {
            Assert.IsTrue(FEnumerable.Empty<int>()()() is Maybe<int>.None);
        }

        [TestMethod]
        public void Return()
        {
            var e = FEnumerable.Return(1)();
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e() is Maybe<int>.None);
        }

        [TestMethod]
        public void RepeatInfinite()
        {
            var e = FEnumerable.Repeat(1)();
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e().Value == 1);
        }

        [TestMethod]
        public void Repeat()
        {
            var e = FEnumerable.Repeat(1, 2)();
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e() is Maybe<int>.None);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatOutOfRange()
        {
            FEnumerable.Repeat(0, -1);
        }

        [TestMethod]
        public void Range()
        {
            var e = FEnumerable.Range(1, 3)();
            Assert.IsTrue(e().Value == 1);
            Assert.IsTrue(e().Value == 2);
            Assert.IsTrue(e().Value == 3);
            Assert.IsTrue(e() is Maybe<int>.None);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RangeOutOfRange()
        {
            FEnumerable.Range(0, -1);
        }
    }
}
