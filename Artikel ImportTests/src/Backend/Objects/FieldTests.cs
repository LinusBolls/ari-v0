using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Artikel_Import.src.Backend.Objects.Tests
{
    /// <summary>
    /// Test class for <see cref="Field"/>.
    /// </summary>
    [TestClass()]
    public class FieldTests
    {
        /// <summary>
        /// Test method for <see cref="Field.CleanPrice(string)"/>
        /// </summary>
        [TestMethod()]
        public void CleanPriceTest()
        {
            string[] tests = new string[] { "1000,00", "1000", "1000.00", "1000 €", "1000 EUR", "1000,00 €", "1000,00 EUR", "1.000,00", "1,000.00" };
            double correctResult = 1000;
            foreach(string price in tests)
            {
                Assert.IsTrue(double.TryParse(Field.CleanPrice(price), out double parsed), $"Failed to Parge {price}");
                Assert.AreEqual(parsed, correctResult, $"Failed test: parsed wrong value '{price}' to '{parsed}' instead of '{correctResult}'");
            }
        }

        /// <summary>
        /// Test method for <see cref="Field.MatchValueSize(string)"/>
        /// </summary>
        [TestMethod()]
        public void MatchValueSizeTest()
        {
            string[] tests = new string[] { "12345", "1234", "123456" };
            int correctResultLength = 5;
            Field testField = new Field("name", "target", "descr", "sqltype", 5, 0, false, false);
            foreach(string test in tests)
            {
                string value = testField.MatchValueSize(test);
                Assert.IsTrue(value.Length <= correctResultLength, $"Failed test: '{test}' to  '{value}' does not have {correctResultLength} chars or less");
            }
        }
    }
}