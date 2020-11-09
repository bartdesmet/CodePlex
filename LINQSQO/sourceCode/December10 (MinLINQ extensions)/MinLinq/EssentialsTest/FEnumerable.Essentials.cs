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
    public class Essentials
    {
        [TestMethod]
        public void AnaEmpty()
        {
            var empty = FEnumerable.Ana(0, _ => false, _ => _, x => x);

            for (int i = 0; i < 2; i++)
            {
                var e = empty();
                Assert.IsTrue(e() is Maybe<int>.None, "Non-empty (" + i + ")");
            }
        }

        [TestMethod]
        public void AnaInfinite()
        {
            var inc = FEnumerable.Ana(0, _ => true, x => x + 1, x => x.ToString());

            for (int i = 0; i < 2; i++)
            {
                var e = inc();
                Assert.IsTrue(e().Value == "0", "Invalid first (" + i + ")");
                Assert.IsTrue(e().Value == "1", "Invalid second (" + i + ")");
                Assert.IsTrue(e().Value == "2", "Invalid third (" + i + ")");
            }
        }

        [TestMethod]
        public void AnaFinite()
        {
            var inc = FEnumerable.Ana(0, x => x < 3, x => x + 1, x => x.ToString());

            for (int i = 0; i < 2; i++)
            {
                var e = inc();
                Assert.IsTrue(e().Value == "0", "Invalid first (" + i + ")");
                Assert.IsTrue(e().Value == "1", "Invalid second (" + i + ")");
                Assert.IsTrue(e().Value == "2", "Invalid third (" + i + ")");
                Assert.IsTrue(e() is Maybe<string>.None, "Not at end (" + i + ")");
            }
        }

        [TestMethod]
        public void BindEmptyOuter()
        {
            Func<Func<Maybe<int>>> source = () => () => new Maybe<int>.None();
            Func<Func<Maybe<string>>> some = () => () => new Maybe<string>.Some("");
            var res = source.Bind(_ => some, (x, s) => s + x);

            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e() is Maybe<string>.None, "Not empty (" + i + ")");
            }
        }

        [TestMethod]
        public void BindEmptyInner()
        {
            Func<Func<Maybe<int>>> source = () =>
            {
                int n = 0;
                return () => n == 0 ? (Maybe<int>)new Maybe<int>.Some(n++) : new Maybe<int>.None();
            };
            Func<Func<Maybe<string>>> none = () => () => new Maybe<string>.None();
            var res = source.Bind(_ => none, (x, s) => s + x);

            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e() is Maybe<string>.None, "Not empty (" + i + ")");
            }
        }

        [TestMethod]
        public void BindFull()
        {
            Func<Func<Maybe<int>>> source = () =>
            {
                int n = 0;
                return () => n < 3 ? (Maybe<int>)new Maybe<int>.Some(n++) : new Maybe<int>.None();
            };
            
            Func<string, int, Func<Func<Maybe<string>>>> repeat = (s, n) => () =>
            {
                int k = 0;
                return () => k++ < n ? (Maybe<string>)new Maybe<string>.Some(s) : new Maybe<string>.None();
            };

            var res = source.Bind(x => repeat(((char)('a' + x)).ToString(), x), (x, s) => s + x);

            for (int i = 0; i < 2; i++)
            {
                var e = res();
                Assert.IsTrue(e().Value == "b1", "Expected b1 (" + i + ")");
                Assert.IsTrue(e().Value == "c2", "Expected c2 (" + i + ")");
                Assert.IsTrue(e().Value == "c2", "Expected c2 again (" + i + ")");
                Assert.IsTrue(e() is Maybe<string>.None, "Expected end (" + i + ")");
            }
        }

        [TestMethod]
        public void CataEmpty()
        {
            Func<Func<Maybe<int>>> source = () => () => new Maybe<int>.None();
            Assert.IsTrue(source.Cata(42, _ => true, (_, __) => _) == 42, "Invalid result.");
        }

        [TestMethod]
        public void CataFinite()
        {
            Func<Func<Maybe<int>>> source = () =>
            {
                int n = 0;
                return () => n <= 3 ? (Maybe<int>)new Maybe<int>.Some(n++) : new Maybe<int>.None();
            };

            for (int i = 0; i < 2; i++)
            {
                Assert.IsTrue(source.Cata(0, _ => true, (sum, x) => sum + x) == 6, "Invalid result (" + i + ").");
            }
        }

        [TestMethod]
        public void All()
        {
            // 0, 1, 2
            var outer = FEnumerable.Ana(0, x => x < 3, x => x + 1, x => x.ToString());
            // 1, 2, 4, 8
            var inner = FEnumerable.Ana(1, x => x < 10, x => x * 2, x => x.ToString());
            // 1, 2, 4, 8, 2, 3, 5, 8, 3, 4, 6, 10
            var reslt = outer.Bind(_ => inner, (o, i) => int.Parse(o) + int.Parse(i));

            Assert.AreEqual(57, reslt.Cata(0, _ => true, (sum, x) => sum + x));
        }
    }
}
