using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Artikel_Import.src.Backend.Tests
{
    ///<summary>
    ///Test class for <see cref="ImportFromCsvToTempDb"/>
    ///</summary>
    [TestClass()]
    public class ImportFromCsvToTempDbTests
    {
        /// <summary>
        /// Teste method for <see cref="ImportFromCsvToTempDb.GetNextArtikelNr"/>
        /// </summary>
        [TestMethod()]
        public void GetNextArtikelNrTest()
        {
            string[] articleNrs = new string[] { "'0000'", "'0000-0'", "'0000-9'", "'0000-99'", "'0000-999'" };
            int[] tests = new int[] { 0, 1, 10, 100, 1000 };
            string[] correctResults = new string[] { "'0000-0'", "'0000-1'", "'0000-10'", "'0000-100'", "'0000-1000'" };
            for(int i = 0;i < tests.Length;i++)
            {
                string result = ImportFromCsvToTempDb.GetNextArtikelNr(articleNrs[i], tests[i]);
                Assert.AreEqual(correctResults[i], result);
            }
        }
    }
}