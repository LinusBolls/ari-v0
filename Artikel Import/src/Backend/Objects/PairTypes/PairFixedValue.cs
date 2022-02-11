using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// Always returns a <see cref="value"/>. Ignoring the dataRow.
    /// </summary>
    public class PairFixedValue : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "FixedValue";

        private readonly string value;

        /// <summary>
        /// Create a new PairFixedValue.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="value">The value that will be returned</param>
        public PairFixedValue(string mappingName, bool isOverwrite, string targetField, string value) : base(type, mappingName, isOverwrite, targetField)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the <see cref="value"/>
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public override Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping)
        {
            return new Tuple<string, bool, string>(
                GetQueryReadyValue(value, fields),
                true,
                "");
        }

        /// <summary>
        /// Converts the Pair to a string for easy debugging
        /// </summary>
        /// <returns>pair as a string</returns>
        public override string ToString()
        {
            return $"Pair{type}[mappingName: {mappingName}; targetField: {targetField}; value: {value}; isOverwrite: {isOverwrite}]";
        }

        internal override string GetAdditionalSource()
        {
            return "";
        }

        internal override string GetSource()
        {
            return value;
        }

        /// <summary>
        /// same as <see cref="Pair.Equals(Pair)"/>, but also checks value
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool Equals(PairFixedValue pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.value.Equals(pair.value))
                return false;
            return true;
        }
    }
}