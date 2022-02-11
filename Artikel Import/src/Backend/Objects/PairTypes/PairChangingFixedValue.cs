using System;

namespace Artikel_Import.src.Backend.Objects.PairTypes
{
    /// <summary>
    /// A changing fixed value pair is similar to a <see cref="PairFixedValue"/>, but the value
    /// needs to be changed every time when a new price list gets imported, for example price
    /// expiration dates. This will also lead to showing the <see
    /// cref="Frontend.MainForm.tabPageValues"/> before upload.
    /// </summary>
    public class PairChangingFixedValue : Pair
    {
        /// <summary>
        /// PairType as a string
        /// </summary>
        public const string type = "ChangingFixedValue";

        private readonly string value;

        /// <summary>
        /// Create a new PairChangingFixedValue with a value
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        /// <param name="value"></param>
        public PairChangingFixedValue(string mappingName, bool isOverwrite, string targetField, string value) : base(type, mappingName, isOverwrite, targetField)
        {
            this.value = value;
        }

        /// <summary>
        /// Create a new PairChangingFixedValue without a value. The value will need to be entered
        /// before import.
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="targetField"></param>
        public PairChangingFixedValue(string mappingName, bool isOverwrite, string targetField) : base(type, mappingName, isOverwrite, targetField)
        {
            this.value = null;
        }

        /// <summary>
        /// Returns the value
        /// </summary>
        /// <param name="dataRow">ignored</param>
        /// <param name="header">ignored</param>
        /// <param name="fields"></param>
        /// <param name="mapping">ignored</param>
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
        public bool Equals(PairChangingFixedValue pair)
        {
            if(!base.Equals(pair))
                return false;
            if(!this.value.Equals(pair.value))
                return false;
            return true;
        }
    }
}