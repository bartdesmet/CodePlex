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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLinq;

namespace Tests
{
    [TestClass]
    public class Cheaters
    {
        [TestMethod]
        public void Order()
        {
            var src = new[] { 1, 5, 9, 3, 2, 7, 0, 4, 8, 6 }.AsFEnumerable();
            var res = from i in src
                      orderby i
                      select i;
            Assert.IsTrue(res.AsEnumerable().SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void OrderNested1()
        {
            var src = new[] {
                new { a = 1, b = 0 },
                new { a = 0, b = 0 },
                new { a = 1, b = 1 },
                new { a = 0, b = 1 },
            }.AsFEnumerable();

            var res = (from i in src
                       orderby i.a, i.b descending
                       select i).ToList();

            Assert.IsTrue(res[0].a == 0 && res[0].b == 1);
            Assert.IsTrue(res[1].a == 0 && res[1].b == 0);
            Assert.IsTrue(res[2].a == 1 && res[2].b == 1);
            Assert.IsTrue(res[3].a == 1 && res[3].b == 0);
        }

        [TestMethod]
        public void OrderNested2()
        {
            var src = new[] {
                new { a = 1, b = 0 },
                new { a = 0, b = 0 },
                new { a = 1, b = 1 },
                new { a = 0, b = 1 },
            }.AsFEnumerable();

            var res = (from i in src
                       orderby i.a descending, i.b
                       select i).ToList();

            Assert.IsTrue(res[0].a == 1 && res[0].b == 0);
            Assert.IsTrue(res[1].a == 1 && res[1].b == 1);
            Assert.IsTrue(res[2].a == 0 && res[2].b == 0);
            Assert.IsTrue(res[3].a == 0 && res[3].b == 1);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void ThenByInvalid()
        {
            FEnumerable.Return(1).ThenBy(x => x);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void ThenByDescendingInvalid()
        {
            FEnumerable.Return(1).ThenByDescending(x => x);
        }

        [TestMethod]
        public void DefaultIfEmptyEmpty()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Empty<int>().DefaultIfEmpty(n).First());
        }

        [TestMethod]
        public void DefaultIfEmptyReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return<int>(n).DefaultIfEmpty().First());
        }
    }
}
