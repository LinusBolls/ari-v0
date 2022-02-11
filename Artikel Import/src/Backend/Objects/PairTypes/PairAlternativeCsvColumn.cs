using log4net;
using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// Returns a value form a CSV row. Which column is being decided by finding the <see
    /// cref="columnName"/> in the header row,if Warnig got triggerd Import  <see cref="columnNameAlternative"/>.
    /// </summary>
    public class PairAlternativeCsvColumn : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "AlternativeCsvColumn";

        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnName;
        private readonly string columnNameAlternative;

        /// <summary>
        /// Create a new PairAlternativeCsvColumn  without a factor. The factor will be set to 1 and during
        /// import ignored.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="columnNameAlternative">ColumnName the values will be retrieved from if Warnig got triggerd in columnName </param>
        public PairAlternativeCsvColumn(string mappingName, bool isOverwrite, string targetField, string columnName, string columnNameAlternative) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;

            this.columnNameAlternative = columnNameAlternative;
        }

        /// <summary>
        /// returns the corresponding value from <paramref name="dataRow"/> finds index using.
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
                int index = Array.IndexOf(header, columnName);
                value = dataRow[index];
            }
            catch(Exception)
            {
                //using alternative column
                int index = Array.IndexOf(header, columnNameAlternative);
                if (index < 0)
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] -> columnName '{columnNameAlternative}' not found");
                if (index >= dataRow.Length)
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] -> columnName '{columnNameAlternative}' not found. Row is to short.");
                value = dataRow[index];
            }
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                //using alternative column
                int index = Array.IndexOf(header, columnNameAlternative);
                if(index < 0)
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] -> columnName '{columnNameAlternative}' not found");
                if(index >= dataRow.Length)
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] -> columnName '{columnNameAlternative}' not found. Row is to short.");
                value = dataRow[index];
            }
            Field field = Field.GetFieldByName(fields, targetField);
            if(field.GetSqlType().Equals("NUMBER"))
            {
                try
                {
                    value = Field.CleanPrice(value);
                }
                catch(FormatException)
                {
                    //log.Debug($"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] can't convert value '{value}' or factor '{factor}'");
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] can't convert value '{value}'");
                }
                if(!double.TryParse(value, out double _))
                {
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairAlternativeCsvColumn .GetValueFromRow [{targetField}] can't convert value '{value}'");
                }
            }
            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Converts the Pair to a string for easy debugging
        /// </summary>
        /// <returns>pair as a string</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnName: {columnName}; columnNameAlternative: {columnNameAlternative}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return columnNameAlternative.ToString();
        }

        internal override string GetSource()
        {
            return columnName;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks column and factor
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairAlternativeCsvColumn pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnName.Equals(pair.columnName))
                return false;
            if(!this.columnNameAlternative.Equals(pair.columnNameAlternative))
                return false;
            return true;
        }
    }
}