using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Artikel_Import.src.Backend.Tests
{
    /// <summary>
    /// Test class for <see cref="SQL"/>
    /// </summary>
    [TestClass()]
    public class SQLTests
    {
        /// <summary>
        /// Test method for <see cref="SQL.PreventSQLInjection(string)"/>
        /// </summary>
        [TestMethod()]
        public void PreventSQLInjectionTest()
        {
            string[] tests = new string[] { "test", "'test'", "te'st" };
            string correctResult = "test";
            foreach(string test in tests)
            {
                string result = SQL.PreventSQLInjection(test);
                Assert.AreEqual(correctResult, result, $"Failed test: convert '{test}' to '{result}' instead of '{correctResult}'");
            }
        }
    }
}