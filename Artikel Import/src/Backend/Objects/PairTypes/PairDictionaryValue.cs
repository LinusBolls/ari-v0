using System;
using System.Collections.Generic;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// A Dictionary value returns the value that the <see cref="CustomDictionary"/> selected
    /// returns when the source field contains a name of the dictionary.
    /// </summary>
    public class PairDictionaryValue : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "DictionaryValue";

        private readonly string columnName;
        private readonly string dictionaryName;

        /// <summary>
        /// Create a new PairDictionaryValue.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="columnName">ColumnName the values will be retrieved from</param>
        /// <param name="dictionaryName">ColumnName of the <see cref="CustomDictionary"/> keys</param>
        public PairDictionaryValue(string mappingName, bool isOverwrite, string targetField, string columnName, string dictionaryName) : base(type, mappingName, isOverwrite, targetField)
        {
            this.columnName = columnName;
            this.dictionaryName = dictionaryName;
        }

        /// <summary>
        /// Finds the dictionary key in the <paramref name="dataRow"/> and returns the <see
        /// cref="CustomDictionary"/> value
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="mapping"></param>
        /// <returns>string value and bool isSucessful</returns>
        public override Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping)
        {
            int index = Array.IndexOf(header, columnName);
            if(index < 0)
                return new Tuple<string, bool, string>(null, false, $"ColumnName[{columnName}] not found in header");

            string dictionaryKey = dataRow[index];
            if(!mapping.dictionaries.ContainsKey(dictionaryName))
                return new Tuple<string, bool, string>(null, false, $"Mapping '{mapping.GetName()}' does not contain dictionary {dictionaryKey}");

            CustomDictionary dict = mapping.dictionaries[dictionaryName];
            string value;
            value = dict.GetValue(dictionaryKey);
            if(value == null)
            {
                return new Tuple<string, bool, string>(GetQueryReadyValue(Constants.ErrorValue, fields), false, $"Dictionary '{dictionaryName}' does not contain key '{dictionaryKey}'");
            }

            return new Tuple<string, bool, string>(GetQueryReadyValue(value, fields), true, "");
        }

        /// <summary>
        /// Returns a string that contains all the important information to this pair.
        /// </summary>
        /// <returns>string representation of this pair</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; columnName: {columnName}; dictionaryName: {dictionaryName}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return dictionaryName;
        }

        internal override string GetSource()
        {
            return columnName;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks column and dictionary
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairDictionaryValue pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.columnName.Equals(pair.columnName))
                return false;
            if(!this.dictionaryName.Equals(pair.dictionaryName))
                return false;
            return true;
        }
    }
}