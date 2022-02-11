using Artikel_Import.src.Backend.Objects.PairTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Artikel_Import.src.Backend.Objects.Tests
{/// <summary>
/// Tests pairs
/// </summary>
    [TestClass()]
    public class PairTests
    {
        /// <summary>
        /// Also tests Pair.Equals a little bit
        /// </summary>
        [TestMethod()]
        public void SortTest()
        {
            Pair[] correctResults = new Pair[]
            {
                //TODO_NEWPAIRTYPE: add pair here to test pair sorting
                new PairDiscountValue("mapping", true, "target", "column"),
                new PairDictionaryValue("mapping", true, "target", "column", "dictionary"),
                new PairDictionaryValue("mapping", true, "target", "column", "dictionary"),
                new PairCsvColumnWithDiscount("mapping", true, "target", "column", "discountColumn"),
                new PairConcatCsvColumns("mapping", true, "target", "columnA", "columnB"),
                new PairAlternativeCsvColumn("mapping", true, "target", "columnA", "columnB"),
                new PairCsvColumn("mapping", true, "target", "column"),
                new PairChangingFixedValue("mapping", true, "target"),
                new PairChangingFixedValue("mapping", true, "target"),
                new PairFixedValue("mapping", true, "target", "value")
            };

            Random rnd = new Random();
            Pair[] unsortedPairs = correctResults.OrderBy(x => rnd.Next()).ToArray();

            Pair[] sortedPairs = Pair.Sort(unsortedPairs);
            for(int i = 0;i < sortedPairs.Length;i++)
            {
                Assert.IsTrue(sortedPairs[i].Equals(correctResults[i]), $"Found {sortedPairs[i]} but expected {correctResults[i]}");
            }
        }
    }
}