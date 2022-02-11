using log4net;
using System;
using System.Linq;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// A pair that gets information from a <see cref="columnName"/>, where a <see cref="Discount"/>
    /// is being applied to the value. This discount is being determined by the <see cref="Mapping"/>.
    /// </summary>
    public class PairCsvColumnWithDiscount : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "CsvColumnWithDiscount";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnName;
        private readonly string discountColumnName;

        /// <summary>
        /// Create a new PairCsvColumnWithDiscount.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="discountColumnName">ColumnName of the <see cref="Discount"/> keys</param>
        public PairCsvColumnWithDiscount(string mappingName, bool isOverwrite, string targetField, string columnName, string discountColumnName) : base(type, mappingName, isOverwrite, targetField)
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
                string log_str = $"ColumnName '{columnName}' not found in the header: " + String.Join(",", header);
                log.Error(log_str);
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, log_str);
            }
            //gets discount value using header
            string discountKey;
            if(header.Any(s => s.Equals(discountColumnName)))
                discountKey = dataRow[Array.IndexOf(header, discountColumnName)];
            else
            {
                string log_str = $"DiscountKey '{discountColumnName}' not found in the header: " + String.Join(",", header);
                log.Warn(log_str);
                //new MessagePopUp($"Discount name '{discountColumnName}' missing in the mapping.", Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, log_str);
            }
            //gets discount form the discount value
            Discount discount = Discount.GetDiscountByKey(mappingName, discountKey, mapping.GetDiscounts());
            if(discount == null)
            {
                log.Warn($"discount not found: '{discountKey}'");
                //new MessagePopUp($"Discount name '{discountKey}' missing in the mapping.", Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"Discount name '{discountKey}' missing");
            }
            else
            {
                //applies the discount to the value
                double discountValue = 1.0 - (discount.GetDiscountAmount() / 100.0);
                try
                {
                    double double_value;
                    if(Constants.CheckStringForPreisAufAnfrage.Any(checkStr => checkStr.Equals(value)))
                        double_value = Constants.CheckValueForPreisAufAnfrage;
                    else
                        double_value = double.Parse(Field.CleanPrice(value)) * discountValue;
                    value = "" + Math.Round(double_value, 2);
                }
                catch(FormatException)
                {
                    var logStr = $"Value '{Field.CleanPrice(value)}' not able to parse. Clean Price malfunction -> original value: '{value}'";
                    log.Warn(logStr);
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, logStr);
                }
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
        public bool Equals(PairCsvColumnWithDiscount pair)
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