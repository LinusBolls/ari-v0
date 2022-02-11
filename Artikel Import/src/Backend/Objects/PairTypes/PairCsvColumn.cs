using log4net;
using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// Returns a value form a CSV row. Which column is being decided by finding the <see
    /// cref="columnName"/> in the header row.
    /// </summary>
    public class PairCsvColumn : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "CsvColumn";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string columnName;
        private readonly double factor;

        /// <summary>
        /// Create a new PairCsvColumn.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="factor">Factor to multiply the values with. Will be skipped if 1</param>
        public PairCsvColumn(string mappingName, bool isOverwrite, string targetField, string columnName, double factor) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
            this.factor = factor;
        }

        /// <summary>
        /// Create a new PairCsvColumn. Will create FormatException if the <paramref name="factor"/>
        /// can't be parsed to <see cref="double"/>.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="factor">Factor to multiply the values with. Will be skipped if "1"</param>
        public PairCsvColumn(string mappingName, bool isOverwrite, string targetField, string columnName, string factor) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
            try
            {
                this.factor = double.Parse(factor);
            }
            catch(FormatException)
            {
                log.Error($"Can't parse factor {factor}");
                this.factor = 1;
            }
        }

        /// <summary>
        /// Create a new PairCsvColumn without a factor. The factor will be set to 1 and during
        /// import ignored.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        public PairCsvColumn(string mappingName, bool isOverwrite, string targetField, string columnName) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
            factor = 1;
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
            int index = Array.IndexOf(header, columnName);
            if(index < 0)
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairCsvColumn.GetValueFromRow [{targetField}] -> columnName '{columnName}' not found");
            if(index >= dataRow.Length)
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairCsvColumn.GetValueFromRow [{targetField}] -> columnName '{columnName}' not found. Row is to short.");
            value = dataRow[index];

            Field field = Field.GetFieldByName(fields, targetField);
            if(field.GetSqlType().Equals("NUMBER"))
            {
                try
                {
                    value = Field.CleanPrice(value);
                }
                catch(FormatException)
                {
                    //log.Debug($"PairCsvColumn.GetValueFromRow [{targetField}] can't convert value '{value}' or factor '{factor}'");
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairCsvColumn.GetValueFromRow [{targetField}] can't convert value '{value}' or factor '{factor}'");
                }
                if(!double.TryParse(value, out double dValue))
                {
                    return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"PairCsvColumn.GetValueFromRow [{targetField}] can't convert value '{value}' or factor '{factor}'");
                }
                value = Math.Round(dValue * factor, 2).ToString();
            }
            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Converts the Pair to a string for easy debugging
        /// </summary>
        /// <returns>pair as a string</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnName: {columnName}; factor: {factor}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return factor.ToString();
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
        public bool Equals(PairCsvColumn pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnName.Equals(pair.columnName))
                return false;
            if(!this.factor.Equals(pair.factor))
                return false;
            return true;
        }
    }
}