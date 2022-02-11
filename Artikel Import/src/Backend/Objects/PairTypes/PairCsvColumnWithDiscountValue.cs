using log4net;
using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// A pair that gets information from a <see cref="columnName"/>, where a discountvalue
    /// is being applied to the value.
    /// </summary>
    public class PairCsvColumnWithDiscountValue : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "CsvColumnWithDiscountValue";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnName;
        private readonly string discountColumnName;

        /// <summary>
        /// Create a new PairCsvColumnWithDiscountValue.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="discountColumnName">ColumnName of the <see cref="Discount"/> values</param>
        public PairCsvColumnWithDiscountValue(string mappingName, bool isOverwrite, string targetField, string columnName, string discountColumnName) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
            this.discountColumnName = discountColumnName;
        }

        /// <summary>
        /// Finds the value in the <paramref name="dataRow"/> and deducts the <see cref="Discount"/>
        /// from it.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="mapping"></param>
        /// <returns>Tuple containing value and bool isSuccessful</returns>
        public override Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping)
        {
            //gets value using the header
            string value;
            try
            {
                value = dataRow[Array.IndexOf(header, columnName)];
            }
            catch(IndexOutOfRangeException)
            {
                log.Error($"ColumnName '{columnName}' not found");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"columnName '{columnName}' not found");
            }
            //gets discount value
            string discount;
            try
            {
                discount = dataRow[Array.IndexOf(header, discountColumnName)];
            }
            catch(IndexOutOfRangeException)
            {
                log.Error($"ColumnName '{discountColumnName}' not found");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"columnName '{discountColumnName}' not found");
            }
            discount = Field.CleanPrice(discount);
            if(!double.TryParse(discount, out double discountValue))
            {
                log.Error($"Discount '{discount}' can't be converted to a number");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"Discount '{discount}' can't be converted to a number");
            }
            //multiply value with discountvalue
            discountValue = 1.0 - (discountValue / 100.0);
            try
            {
                value = "" + Math.Round(double.Parse(Field.CleanPrice(value)) * discountValue, 2);
            }
            catch(FormatException)
            {
                log.Warn($"Value '{Field.CleanPrice(value)}' not able to parse. Clean Price malfunction -> original value: '{value}'");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"value '{Field.CleanPrice(value)}' not able to parse. Clean Price malfunction -> original value: '{value}'");
            }
            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Returns a string that contains all the important information to this pair.
        /// </summary>
        /// <returns>string representation of this pair</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnName: {columnName}; discountColumnName: {discountColumnName}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return discountColumnName;
        }

        internal override string GetSource()
        {
            return columnName;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks discount and column name
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairCsvColumnWithDiscountValue pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnName.Equals(pair.columnName))
                return false;
            if(!this.discountColumnName.Equals(pair.discountColumnName))
                return false;
            return true;
        }
    }
}