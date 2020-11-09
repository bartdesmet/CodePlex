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
    public class Conversions
    {
        [TestMethod]
        public void Roundtrip()
        {
            var res = Enumerable.Range(0, 10).AsFEnumerable().AsEnumerable();
            Assert.IsTrue(Enumerable.SequenceEqual(res, Enumerable.Range(0, 10)));
        }
    }
}
