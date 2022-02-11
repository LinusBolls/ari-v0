using log4net;
using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// A field value that returns the <see cref="Discount"/> amount linked to the discount name
    /// </summary>
    public class PairDiscountValue : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "DiscountValue";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnName;

        /// <summary>
        /// Create a new PairDiscountVaule that returns the value of a <see cref="Discount"/>
        /// depending on its key
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName that contains the <see cref="Discount"/> keys</param>
        public PairDiscountValue(string mappingName, bool isOverwrite, string targetField, string columnName) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
        }

        /// <summary>
        /// Finds the discount key in the <paramref name="dataRow"/> and returns the <see
        /// cref="Discount"/> value
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="mapping"></param>
        /// <returns>string discount value and bool isSuccessful</returns>
        public override Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping)
        {
            string discountKey;
            string value;
            try
            {
                discountKey = dataRow[Array.IndexOf(header, columnName)];
            }
            catch(IndexOutOfRangeException)
            {
                log.Warn($"DiscountKey '{columnName}' not found");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"discountKey '{columnName}' not found");
            }
            //gets discount form the discount value
            Discount discount = Discount.GetDiscountByKey(mappingName, discountKey, mapping.GetDiscounts());
            if(discount == null)
            {
                log.Warn($"discount not found: '{discountKey}' in mapping {mapping.GetName()}");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"discount not found: '{discountKey}' in mapping {mapping.GetName()}");
            }
            else
            {
                //return discount as value
                value = discount.GetDiscountAmount().ToString();
            }
            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Returns a string that contains all the important information to this pair.
        /// </summary>
        /// <returns>string representation of this pair</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnName: {columnName}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return columnName;
        }

        internal override string GetSource()
        {
            return columnName;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks column
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairDiscountValue pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnName.Equals(pair.columnName))
                return false;
            return true;
        }
    }
}