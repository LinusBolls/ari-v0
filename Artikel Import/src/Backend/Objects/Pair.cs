using Artikel_Import.src.Backend.Objects.PairTypes;
using log4net;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// A pair is part of a <see cref="Mapping"/> and shows the connection between a <see
    /// cref="CSV"/> file and the Tables <see cref="Constants.TableArtikel"/> or <see
    /// cref="Constants.TableEinkauf"/>. It is a child of the <see cref="MappingObject"/>. The Pairs
    /// can be edited in the <see cref="Frontend.MainForm.tabPageEditMapping"/>.
    /// </summary>
    public abstract class Pair : MappingObject
    {
        /// <summary>
        /// when True the value in the temp database <see cref="Constants.TableImportArticles"/>/
        /// <see cref="Constants.TableImportEinkauf"/> gets overwritten, when False the value gets
        /// only added on.
        /// </summary>
        protected bool isOverwrite;

        /// <summary>
        /// <see cref="Mapping"/> this pair belongs to
        /// </summary>
        protected string mappingName;

        /// <summary>
        /// <see cref="PairTypes"/> of this pair.
        /// </summary>
        protected string pairType;

        /// <summary>
        /// Name of the <see cref="Field"/> the value gets inserted into.
        /// </summary>
        protected string targetField;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Used for removing duplicate spaces
        /// </summary>
        private readonly Regex regexRemoveDuplicateSpaces = new Regex("[ ]{2,}");

        /// <summary>
        /// Creates a new pair.
        /// </summary>
        /// <param name="pairType">type of pair</param>
        /// <param name="mappingName"><see cref="Mapping"/> the pair belongs to</param>
        /// <param name="isOverwrite">
        /// when True the value in the temp database gets overwritten, when False the value gets
        /// only added on insert
        /// </param>
        /// <param name="targetField">column the value gets inserted into</param>
        internal Pair(string pairType, string mappingName, bool isOverwrite, string targetField)
        {
            this.pairType = pairType;
            this.mappingName = mappingName;
            this.isOverwrite = isOverwrite;
            this.targetField = targetField;
        }

        /// <summary>
        /// Gets a <paramref name="pairs"/> by its <paramref name="targetField"/> string
        /// </summary>
        /// <param name="pairs">array of pairs that will be searched</param>
        /// <param name="targetField">string thats being looked for</param>
        /// <returns>
        /// <see cref="Pair"/> that was found. Returns null if nothing was returned, you need to
        /// handle that!
        /// </returns>
        public static Pair GetPairByTargetField(Pair[] pairs, string targetField)
        {
            Pair pair = Array.Find(pairs, i => targetField.Equals(i.targetField));
            if(pair == null)
            {
                log.Warn($"GetPairByTargetField no pair found: {targetField}");
            }
            return pair;
        }

        /// <summary>
        /// Creates a Pair out of values
        /// </summary>
        /// <param name="mappingName">Name of the <see cref="Mapping"/> the pair belongs to</param>
        /// <param name="pairType">The type of pair <see cref="PairTypes"/></param>
        /// <param name="targetField">Name of the <see cref="Field"/> the pair moves values into</param>
        /// <param name="source">The values that get inserted into the <paramref name="targetField"/></param>
        /// <param name="overwrite">If the pair overwrites values in the TempDb</param>
        /// <param name="additionalSource">Additional information needed</param>
        /// <returns>A pair containing all the values</returns>
        public static Pair GetUnknownPair(string mappingName, string pairType, string targetField, string source, string overwrite, string additionalSource)
        {
            bool isOverwrite = "1".Equals(overwrite);
            switch(pairType)
            {
                //TODO_NEWPAIRTYPE add pair here for initialization
                case PairCsvColumn.type:
                    return new PairCsvColumn(mappingName, isOverwrite, targetField, source, additionalSource);

                case PairFixedValue.type:
                    return new PairFixedValue(mappingName, isOverwrite, targetField, source);

                case PairCsvColumnWithDiscount.type:
                    return new PairCsvColumnWithDiscount(mappingName, isOverwrite, targetField, source, additionalSource);

                case PairCsvColumnWithDiscountValue.type:
                    return new PairCsvColumnWithDiscountValue(mappingName, isOverwrite, targetField, source, additionalSource);

                case PairChangingFixedValue.type:
                    return new PairChangingFixedValue(mappingName, isOverwrite, targetField, source);

                case PairDiscountValue.type:
                    return new PairDiscountValue(mappingName, isOverwrite, targetField, source);

                case PairDictionaryValue.type:
                    return new PairDictionaryValue(mappingName, isOverwrite, targetField, source, additionalSource);

                case PairConcatCsvColumns.type:
                    return new PairConcatCsvColumns(mappingName, isOverwrite, targetField, source, additionalSource);

                case PairAlternativeCsvColumn.type:
                    return new PairAlternativeCsvColumn(mappingName, isOverwrite, targetField, source, additionalSource);

                default:
                    //log.Debug($"Mapping: {mappingName}; PairType: {pairType}; Target: {targetField} Source: {source}: Overwrite: {overwrite}; AddSource: {additionalSource}");
                    log.Error($"Pair.UnknownPair pair type unknown: '{pairType}'");
                    return new PairFixedValue(mappingName, isOverwrite, Constants.ErrorValue, Constants.ErrorValue);
            }
        }

        /// <summary>
        /// Sorts an array of pairs by their pair type
        /// </summary>
        /// <param name="unsortedPairs"></param>
        /// <returns></returns>
        public static Pair[] Sort(Pair[] unsortedPairs)
        {
            Dictionary<string, List<Pair>> pairsByType = new Dictionary<string, List<Pair>>();
            foreach(Pair pair in unsortedPairs)
            {
                if(pairsByType.ContainsKey(pair.pairType))
                {
                    pairsByType[pair.pairType].Add(pair);
                }
                else
                {
                    pairsByType.Add(pair.pairType, new List<Pair>() { pair });
                }
            }

            List<Pair> sortedPairs = new List<Pair>();
            //pair types that are most prone to errors are added first. This way imports get canceled faster if any error happens
            if(pairsByType.ContainsKey(PairDiscountValue.type))
                sortedPairs.AddRange(pairsByType[PairDiscountValue.type]);
            if(pairsByType.ContainsKey(PairDictionaryValue.type))
                sortedPairs.AddRange(pairsByType[PairDictionaryValue.type]);
            if(pairsByType.ContainsKey(PairCsvColumnWithDiscount.type))
                sortedPairs.AddRange(pairsByType[PairCsvColumnWithDiscount.type]);
            if(pairsByType.ContainsKey(PairCsvColumnWithDiscountValue.type))
                sortedPairs.AddRange(pairsByType[PairCsvColumnWithDiscountValue.type]);
            if(pairsByType.ContainsKey(PairConcatCsvColumns.type))
                sortedPairs.AddRange(pairsByType[PairConcatCsvColumns.type]);
            if(pairsByType.ContainsKey(PairAlternativeCsvColumn.type))
                sortedPairs.AddRange(pairsByType[PairAlternativeCsvColumn.type]);
            if(pairsByType.ContainsKey(PairCsvColumn.type))
                sortedPairs.AddRange(pairsByType[PairCsvColumn.type]);
            if(pairsByType.ContainsKey(PairChangingFixedValue.type))
                sortedPairs.AddRange(pairsByType[PairChangingFixedValue.type]);
            if(pairsByType.ContainsKey(PairFixedValue.type))
                sortedPairs.AddRange(pairsByType[PairFixedValue.type]);
            //TODO_NEWPAIRTYPE: app pair type here

            foreach(Pair pair in unsortedPairs)
            {
                if(!sortedPairs.Contains(pair))
                    sortedPairs.Add(pair);
            }
            return sortedPairs.ToArray();
        }

        /// <summary>
        /// Name of the <see cref="Mapping"/> this pair belongs to.
        /// </summary>
        /// <returns>name of the mapping</returns>
        public string GetMappingName()
        {
            return mappingName;
        }

        /// <summary>
        /// Returns a unique name for this pair.
        /// </summary>
        /// <returns>Name of the Pair</returns>
        public override string GetName()
        {
            return targetField;
        }

        /// <summary>
        /// Returns the type of pair as a string <see cref="PairTypes"/>.
        /// </summary>
        /// <returns>PairType name</returns>
        public string GetPairType()
        {
            return pairType;
        }

        /// <summary>
        /// Returns the <see cref="Field"/> the pair references.
        /// </summary>
        /// <returns>name of the field</returns>
        public string GetTargetField()
        {
            return targetField;
        }

        /// <summary>
        /// Returns a value from an <paramref name="dataRow"/>, which value is being chosen depends
        /// on the pair and <paramref name="fields"/>. This is the heart of the CSV import.
        /// </summary>
        /// <param name="dataRow">the row of the CSV</param>
        /// <param name="header">the header/first row of the CSV</param>
        /// <param name="fields">all fields</param>
        /// <param name="mapping"><see cref="Mapping"/> thats being imported on</param>
        /// <returns>the string is the value and the bool shows if the function was successful</returns>
        public abstract Tuple<string, bool, string> GetValueFromRow(string[] dataRow, string[] header, Field[] fields, Mapping mapping);

        /// <summary>
        /// Inserts the pair into the database <see cref="Constants.TableImportMappings"/>
        /// </summary>
        /// <returns>SqlReport</returns>
        public override SqlReport Insert()
        {
            string overwriteStr;
            if(isOverwrite)
                overwriteStr = "1";
            else
                overwriteStr = "0";
            string factorStr = GetAdditionalSource();
            if("".Equals(factorStr))
                factorStr = "1";
            string cmd = $@"insert into {Constants.TableImportMappings} (NAME, PAIR_TARGET_FIELD, PAIR_SOURCE_FIELD, OVERWRITE, FACTOR, PAIR_TYPE) values(
                    '{SQL.PreventSQLInjection(mappingName)}',
                    '{SQL.PreventSQLInjection(targetField)}',
                    '{SQL.PreventSQLInjection(GetSource())}',
                    '{SQL.PreventSQLInjection(overwriteStr)}',
                    '{SQL.PreventSQLInjection(factorStr)}',
                    '{SQL.PreventSQLInjection(pairType)}')";

            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        /// <summary>
        /// when True the value in the temp database gets overwritten, when False the value gets
        /// only added on insert
        /// </summary>
        /// <returns></returns>
        public bool IsOverwrite()
        {
            return isOverwrite;
        }

        /// <summary>
        /// Removes the pair from the database <see cref="Constants.TableImportMappings"/>.
        /// </summary>
        /// <returns>SqlReport</returns>
        public override SqlReport Remove()
        {
            string cmd = $"delete from {Constants.TableImportMappings} where NAME='{mappingName}' and PAIR_TARGET_FIELD='{targetField}'";
            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        internal abstract string GetAdditionalSource();

        internal string GetQueryReadyValue(string value, Field[] fields)
        {
            Field field = Field.GetFieldByName(fields, targetField);
            if(value == null)
            {
                log.Error("Pair.GetQueryReadyValue value is null");
                value = Constants.ErrorValue;
            }
            value = SQL.PreventSQLInjection(value);
            value = RemoveDuplicateSpaces(value);
            value = field.MatchValueSize(value); //the value can't be too big

            if ("DATE".Equals(field.GetSqlType()))
            {

                // in order to avoid warnings like this:  "2021-xy-ab cd:ef:gh,318 WARN  Artikel_Import.src.Backend.Objects.Pair.GetQueryReadyValue
                //          Artikel_Import.src.Backend.Objects.Pair [(null)] - Value 2022-31-01 has not the correct date format, or is not a date"
                value = CheckForDayMonthSwitch(value);  // sometimes month and day are switched

                if (!Field.IsDateFormat(value))
                {
                    log.Warn($"Value {value} has not the correct date format, or is not a date");
                    return Constants.ErrorValue;
                }                
            }
            value = field.AddValueTypeConversion(value); //add different method depending on the value type -> f.ex. TO_NUMBER('value')
            return value;
        }


        private static string CheckForDayMonthSwitch(string date_str)
        {
            if (date_str.StartsWith("TO_DATE("))  date_str = date_str.Substring(9, 10); //TO_DATE('2022-01-31', 'yyyy-mm-dd')  // 2022-31-01            
            
            if ((date_str.StartsWith("20") || date_str.StartsWith("19")) && date_str[4].Equals('-') && date_str[7].Equals('-'))
            {
                if (Int32.Parse(date_str.Substring(5, 2)) > 12)

                    date_str = date_str.Substring(0, 5) + "-" + date_str.Substring(9, 2) + "-" + date_str.Substring(5, 2);
            }
            return date_str;
        }

        internal abstract string GetSource();

        internal void SetMappingName(string name)
        {
            mappingName = name;
        }

        private string RemoveDuplicateSpaces(string value)
        {
            return regexRemoveDuplicateSpaces.Replace(value, " ");
        }

        /// <summary>
        /// Returns true if all values are equal
        /// </summary>
        /// <param name="pair">pair that will be compared with this pair</param>
        /// <returns></returns>
        public bool Equals(Pair pair)
        {
            if(!this.isOverwrite.Equals(pair.isOverwrite))
                return false;
            if(!this.mappingName.Equals(pair.mappingName))
                return false;
            if(!this.pairType.Equals(pair.pairType))
                return false;
            if(!this.targetField.Equals(pair.targetField))
                return false;
            return true;
        }
    }
}