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
    public class Aggregates
    {
        [TestMethod]
        public void CountEmpty()
        {
            Assert.AreEqual(0, FEnumerable.Empty<int>().Count());
        }

        [TestMethod]
        public void CountReturn()
        {
            Assert.AreEqual(1, FEnumerable.Return(42).Count());
        }

        [TestMethod]
        public void CountFew()
        {
            int n = 10;
            Assert.AreEqual(n, FEnumerable.Range(0, n).Count());
        }

        [TestMethod]
        public void CountRepeat()
        {
            int n = 10;
            Assert.AreEqual(n, FEnumerable.Repeat(0, n).Count());
        }

        [TestMethod]
        public void LongCountEmpty()
        {
            Assert.AreEqual(0, FEnumerable.Empty<int>().LongCount());
        }

        [TestMethod]
        public void LongCountReturn()
        {
            Assert.AreEqual(1, FEnumerable.Return(42).LongCount());
        }

        [TestMethod]
        public void LongCountFew()
        {
            int n = 10;
            Assert.AreEqual(n, FEnumerable.Range(0, n).LongCount());
        }

        [TestMethod]
        public void LongCountRepeat()
        {
            int n = 10;
            Assert.AreEqual(n, FEnumerable.Repeat(0, n).LongCount());
        }

        [TestMethod]
        public void AllEmpty()
        {
            Assert.IsTrue(FEnumerable.Empty<int>().All(_ => false));
        }

        [TestMethod]
        public void AnyEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Any(_ => false));
        }

        [TestMethod]
        public void AllReturn()
        {
            Assert.IsTrue(FEnumerable.Return(1).All(x => x == 1));
            Assert.IsFalse(FEnumerable.Return(1).All(x => x == 0));
        }

        [TestMethod]
        public void AnyReturn()
        {
            Assert.IsTrue(FEnumerable.Return(1).Any(x => x == 1));
            Assert.IsFalse(FEnumerable.Return(1).Any(x => x == 0));
        }

        [TestMethod]
        public void AllSome()
        {
            Assert.IsTrue(FEnumerable.Range(0, 10).All(x => x < 10));
            Assert.IsFalse(FEnumerable.Range(0, 10).All(x => x > 10));
            Assert.IsFalse(FEnumerable.Range(0, 10).All(x => x == 5));
        }

        [TestMethod]
        public void AnySome()
        {
            Assert.IsTrue(FEnumerable.Range(0, 10).Any(x => x < 10));
            Assert.IsFalse(FEnumerable.Range(0, 10).Any(x => x > 10));
            Assert.IsTrue(FEnumerable.Range(0, 10).Any(x => x == 5));
        }

        [TestMethod]
        public void ContainsEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Contains(42));
        }

        [TestMethod]
        public void ContainsReturn()
        {
            int n = 42;
            Assert.IsTrue(FEnumerable.Return(n).Contains(n));
        }

        [TestMethod]
        public void ContainsSome()
        {
            Assert.IsTrue(FEnumerable.Range(0, 10).Contains(0));
            Assert.IsTrue(FEnumerable.Range(0, 10).Contains(5));
            Assert.IsTrue(FEnumerable.Range(0, 10).Contains(9));
            Assert.IsFalse(FEnumerable.Range(0, 10).Contains(10));
        }

        [TestMethod]
        public void SumEmpty()
        {
            Assert.AreEqual(0, FEnumerable.Empty<int>().Sum());
        }

        [TestMethod]
        public void SumReturn()
        {
            Assert.AreEqual(42, FEnumerable.Return(42).Sum());
        }

        [TestMethod]
        public void SumSome()
        {
            Assert.AreEqual(45, FEnumerable.Range(0, 10).Sum());
        }

        [TestMethod]
        public void SumSomeProjected()
        {
            Assert.AreEqual(55, FEnumerable.Range(0, 10).Sum(x => x + 1));
        }

        [TestMethod]
        public void AverageEmpty()
        {
            Assert.AreEqual(0, FEnumerable.Empty<int>().Average());
        }

        [TestMethod]
        public void AverageReturn()
        {
            Assert.AreEqual(42, FEnumerable.Return(42).Average());
        }

        [TestMethod]
        public void AverageSome()
        {
            Assert.AreEqual(4.5, FEnumerable.Range(0, 10).Average());
        }

        [TestMethod]
        public void AverageSomeProjected()
        {
            Assert.AreEqual(5.5, FEnumerable.Range(0, 10).Average(x => x + 1));
        }

        [TestMethod]
        public void MinEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Min().HasValue);
        }

        [TestMethod]
        public void MinReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).Min().Value);
        }

        [TestMethod]
        public void MinSome()
        {
            int min = 3;
            int[] values = new[] { 7, min, 5 };
            var src = FEnumerable.Ana(0, n => n < values.Length, n => n + 1, n => values[n]);
            Assert.AreEqual(min, src.Min().Value);
        }

        [TestMethod]
        public void MinEmptyProjected()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Min(x => x + 1).HasValue);
        }

        [TestMethod]
        public void MinReturnProjected()
        {
            int n = 42;
            Func<int, int> inc = x => x + 1;
            Assert.AreEqual(inc(n), FEnumerable.Return(n).Min(inc).Value);
        }

        [TestMethod]
        public void MinSomeProjected()
        {
            int min = 3;
            int[] values = new[] { 7, min, 5 };
            Func<int, int> inc = x => x + 1;
            var src = FEnumerable.Ana(0, n => n < values.Length, n => n + 1, n => values[n]);
            Assert.AreEqual(inc(min), src.Min(inc).Value);
        }

        [TestMethod]
        public void MaxEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Max().HasValue);
        }

        [TestMethod]
        public void MaxReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).Max().Value);
        }

        [TestMethod]
        public void MaxSome()
        {
            int max = 9;
            int[] values = new[] { 7, max, 5 };
            var src = FEnumerable.Ana(0, n => n < values.Length, n => n + 1, n => values[n]);
            Assert.AreEqual(max, src.Max().Value);
        }

        [TestMethod]
        public void MaxEmptyProjected()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().Max(x => x + 1).HasValue);
        }

        [TestMethod]
        public void MaxReturnProjected()
        {
            int n = 42;
            Func<int, int> inc = x => x + 1;
            Assert.AreEqual(inc(n), FEnumerable.Return(n).Max(inc).Value);
        }

        [TestMethod]
        public void MaxSomeProjected()
        {
            int max = 9;
            int[] values = new[] { 7, max, 5 };
            Func<int, int> inc = x => x + 1;
            var src = FEnumerable.Ana(0, n => n < values.Length, n => n + 1, n => values[n]);
            Assert.AreEqual(inc(max), src.Max(inc).Value);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void FirstEmpty()
        {
            FEnumerable.Empty<int>().First();
        }

        [TestMethod]
        public void FirstOrDefaultEmpty()
        {
            Assert.AreEqual(default(int), FEnumerable.Empty<int>().FirstOrDefault());
        }

        [TestMethod]
        public void FirstOrNoneEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().FirstOrNone().HasValue);
        }

        [TestMethod]
        public void FirstReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(42).First());
        }

        [TestMethod]
        public void FirstOrDefaultReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).FirstOrDefault());
        }

        [TestMethod]
        public void FirstOrNoneReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).FirstOrNone().Value);
        }

        [TestMethod]
        public void FirstSome()
        {
            int fst = 0;
            Assert.AreEqual(fst, FEnumerable.Range(fst, 5).First());
        }

        [TestMethod]
        public void FirstSomePredicate()
        {
            int fst = 0;
            Assert.AreEqual(3, FEnumerable.Range(fst, 5).First(n => n > 2));
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void FirstSomePredicateNeverTrue()
        {
            FEnumerable.Range(0, 5).First(n => n > 5);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void LastEmpty()
        {
            FEnumerable.Empty<int>().Last();
        }

        [TestMethod]
        public void LastOrDefaultEmpty()
        {
            Assert.AreEqual(default(int), FEnumerable.Empty<int>().LastOrDefault());
        }

        [TestMethod]
        public void LastOrNoneEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().LastOrNone().HasValue);
        }

        [TestMethod]
        public void LastReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(42).Last());
        }

        [TestMethod]
        public void LastOrDefaultReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).LastOrDefault());
        }

        [TestMethod]
        public void LastOrNoneReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).LastOrNone().Value);
        }

        [TestMethod]
        public void LastSome()
        {
            int lst = 5;
            Assert.AreEqual(lst, FEnumerable.Range(0, lst + 1).Last());
        }

        [TestMethod]
        public void LastSomePredicate()
        {
            int lst = 5;
            Assert.AreEqual(3, FEnumerable.Range(0, lst + 1).Last(n => n <= 3));
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void LastSomePredicateNeverTrue()
        {
            FEnumerable.Range(0, 5).Last(n => n > 5);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void SingleEmpty()
        {
            FEnumerable.Empty<int>().Single();
        }

        [TestMethod]
        public void SingleOrDefaultEmpty()
        {
            Assert.AreEqual(default(int), FEnumerable.Empty<int>().SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrNoneEmpty()
        {
            Assert.IsFalse(FEnumerable.Empty<int>().SingleOrNone().HasValue);
        }

        [TestMethod]
        public void SingleReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(42).Single());
        }

        [TestMethod]
        public void SingleOrDefaultReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrNoneReturn()
        {
            int n = 42;
            Assert.AreEqual(n, FEnumerable.Return(n).SingleOrNone().Value);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void SingleSome()
        {
            FEnumerable.Repeat(0, 2).Single();
        }

        [TestMethod]
        public void SingleSomePredicate()
        {
            Assert.AreEqual(3, FEnumerable.Range(0, 5).Single(n => n == 3));
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void SingleSomePredicateNeverTrue()
        {
            FEnumerable.Range(0, 5).Single(n => n > 5);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void SingleSomePredicateTrueMultiple()
        {
            FEnumerable.Range(0, 5).Single(n => n < 5);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void ElementAtEmpty()
        {
            FEnumerable.Empty<int>().ElementAt(0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ElementAtOutOfRange()
        {
            FEnumerable.Empty<int>().ElementAt(-1);
        }

        [TestMethod]
        public void ElementAtSome()
        {
            int f = 0;
            Assert.AreEqual(f, FEnumerable.Range(0, 10).ElementAt(f));

            int n = 5;
            Assert.AreEqual(n, FEnumerable.Range(0, 10).ElementAt(n));

            int l = 9;
            Assert.AreEqual(l, FEnumerable.Range(0, 10).ElementAt(l));
        }

        [TestMethod]
        public void ElementAtOrDefaultEmpty()
        {
            Assert.AreEqual(default(int), FEnumerable.Empty<int>().ElementAtOrDefault(0));
        }

        [TestMethod]
        public void ElementAtOrDefaultSome()
        {
            int f = 0;
            Assert.AreEqual(f, FEnumerable.Range(0, 10).ElementAtOrDefault(f));

            int n = 5;
            Assert.AreEqual(n, FEnumerable.Range(0, 10).ElementAtOrDefault(n));

            int l = 9;
            Assert.AreEqual(l, FEnumerable.Range(0, 10).ElementAtOrDefault(l));

            int o = 10;
            Assert.AreEqual(default(int), FEnumerable.Range(0, 10).ElementAtOrDefault(o));
        }

        [TestMethod]
        public void Run()
        {
            int sum = 0;
            FEnumerable.Range(0, 10).Run(x => sum += x);
            Assert.AreEqual(45, sum);
        }

        [TestMethod]
        public void ToArray()
        {
            var res = FEnumerable.Range(0, 10).ToArray();
            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(res, System.Linq.Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Aggregate1()
        {
            Assert.AreEqual(45, FEnumerable.Range(0, 10).Aggregate(0, (sum, i) => sum + i));
        }

        [TestMethod]
        public void Aggregate2()
        {
            Assert.AreEqual(90, FEnumerable.Range(0, 10).Aggregate(0, (sum, i) => sum + i, sum => sum * 2));
        }
    }
}