using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    /// <summary>
    /// Summary description for ComplianceCheck
    /// </summary>
    [TestClass]
    public class ComplianceCheck
    {
        public ComplianceCheck()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ComplianceCheckTest()
        {
            var res1 = from mi in typeof(System.Linq.Enumerable).GetMethods() orderby mi.Name group mi by mi.Name into g select new { Name = g.Key, Overloads = g.Count() };
            var res2 = from mi in typeof(BdsSoft.Linq.Enumerable).GetMethods() orderby mi.Name group mi by mi.Name into g select new { Name = g.Key, Overloads = g.Count() };

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(res1, res2), "Implementation doesn't match the official LINQ to Objects standard query operators.");

            /*
            var mismatches = from m1 in res1
                             join m2 in res2 on m1.Name equals m2.Name
                             where m1.Overloads != m2.Overloads
                             select new { m1.Name, Theirs = m1.Overloads, Ours = m2.Overloads };

            foreach (var m in mismatches)
                Console.WriteLine("{0} has {1} overloads instead of {2}", m.Name, m.Ours, m.Theirs);
             */
        }
    }
}
