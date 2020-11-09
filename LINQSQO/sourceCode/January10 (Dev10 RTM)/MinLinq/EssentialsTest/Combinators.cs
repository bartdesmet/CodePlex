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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLinq;

namespace Tests
{
    [TestClass]
    public class Combinators
    {
        [TestMethod]
        public void Where()
        {
            var res = FEnumerable.Range(0, 10).Where(n => n % 2 == 0);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e().Value == 4);
                Assert.IsTrue(e().Value == 6);
                Assert.IsTrue(e().Value == 8);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Select()
        {
            var res = FEnumerable.Range(0, 5).Select(n => n + 1);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e().Value == 3);
                Assert.IsTrue(e().Value == 4);
                Assert.IsTrue(e().Value == 5);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Cast()
        {
            int n = 1;
            Assert.AreEqual(n, FEnumerable.Return((object)n).Cast<int>()()().Value);
        }

        [TestMethod]
        public void OfType()
        {
            object[] os = new[] { (object)1, (object)"Hello", (object)2 };
            var src = FEnumerable.Ana(0, i => i < os.Length, i => i + 1, i => os[i]);
            var res = src.OfType<int>();
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Concat()
        {
            var src = FEnumerable.Ana(1, i => i <= 3, i => i + 1, i => FEnumerable.Range(0, i));
            var res = src.Concat();
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Distinct()
        {
            int[] xs = new[] { 2, 3, 4, 3, 2, 1, 0, 2, 3, 4, 5 };
            var src = FEnumerable.Ana(0, i => i < xs.Length, i => i + 1, i => xs[i]);
            var res = src.Distinct();
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e().Value == 3);
                Assert.IsTrue(e().Value == 4);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 5);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void DistinctComparer()
        {
            int[] xs = new[] { 2, 3, 4, 3, 2, 1, 0, 2, 3, 4, 5 };
            var src = FEnumerable.Ana(0, i => i < xs.Length, i => i + 1, i => xs[i]);
            var res = src.Distinct(EqualityComparer<int>.Default);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e().Value == 3);
                Assert.IsTrue(e().Value == 4);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 5);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Skip()
        {
            var res = FEnumerable.Range(0, 10).Skip(8);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 8);
                Assert.IsTrue(e().Value == 9);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SkipOutOfRange()
        {
            FEnumerable.Return(1).Skip(-1);
        }

        [TestMethod]
        public void SkipWhile()
        {
            var res = FEnumerable.Range(0, 10).SkipWhile(n => n < 8);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 8);
                Assert.IsTrue(e().Value == 9);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Take()
        {
            var res = FEnumerable.Range(0, 10).Take(2);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeOutOfRange()
        {
            FEnumerable.Return(1).Take(-1);
        }

        [TestMethod]
        public void TakeWhile()
        {
            var res = FEnumerable.Range(0, 10).TakeWhile(n => n < 2);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void Reverse()
        {
            var res = FEnumerable.Range(0, 3).Reverse();
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 2);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void SelectManyIndex1()
        {
            var res = FEnumerable.Return(42).Snoc(24).SelectMany((_, i) => FEnumerable.Return(i));
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 1);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void SelectManyIndex2()
        {
            var res = FEnumerable.Return(42).Snoc(24).SelectMany((_, i) => FEnumerable.Return(i), (i, j) => i + j);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 24 + 0);
                Assert.IsTrue(e().Value == 42 + 1);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void SelectMany1()
        {
            var res = FEnumerable.Return(42).Snoc(24).SelectMany(x => FEnumerable.Return(x));
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 24);
                Assert.IsTrue(e().Value == 42);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }

        [TestMethod]
        public void SelectMany2()
        {
            var res = FEnumerable.Return(42).Snoc(24).SelectMany(x => FEnumerable.Return(x), (o, i) => o - i);
            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e().Value == 0);
                Assert.IsTrue(e() is Maybe<int>.None);
            }
        }
    }
}
