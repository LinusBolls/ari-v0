using log4net;
using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// Pair that takes two columns ( <see cref="columnNameA"/> and <see cref="columnNameB"/>) and
    /// returns the first column and the second column in one string.
    /// </summary>
    public class PairConcatCsvColumns : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "ConcatCsvColumns";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnNameA;
        private readonly string columnNameB;

        /// <summary>
        /// Create a new PairConcatCsvColumns.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnNameA">ColumnName the value will be retrieved from</param>
        /// <param name="columnNameB">ColumnName the value will be added to value</param>
        public PairConcatCsvColumns(string mappingName, bool isOverwrite, string targetField, string columnNameA, string columnNameB) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnNameA = columnNameA;
            this.columnNameB = columnNameB;
        }

        /// <summary>
        /// returns the corresponding value by adding two values from <paramref name="dataRow"/>
        /// from <see cref="columnNameA"/> and <see cref="columnNameB"/>.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public override Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping)
        {
            string value;
            try
            {
                value = dataRow[Array.IndexOf(header, columnNameA)];
            }
            catch(IndexOutOfRangeException)
            {
                log.Warn($"ColumnName '{columnNameA}' not found");
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"columnName '{columnNameA}' not found");
            }
            try
            {
                value += " " + dataRow[Array.IndexOf(header, columnNameB)];
            }
            catch(IndexOutOfRangeException)
            {
                log.Warn($"ColumnName '{columnNameB}' not found");
                return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), false, $"columnName '{columnNameA}' not found");
            }
            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Converts the Pair to a string for easy debugging
        /// </summary>
        /// <returns>pair as a string</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnNameA: {columnNameA}; columnNameB: {columnNameB}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return columnNameB;
        }

        internal override string GetSource()
        {
            return columnNameA;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks columns A and B
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairConcatCsvColumns pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnNameA.Equals(pair.columnNameA))
                return false;
            if(!this.columnNameB.Equals(pair.columnNameB))
                return false;
            return true;
        }
    }
}