using Artikel_Import.Properties;
using Artikel_Import.src.Backend.Objects;
using Artikel_Import.src.Frontend;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Used for handling the CSV files on <see cref="ImportFromCsvToTempDb"/>.
    /// </summary>
    public class CSV
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Loads and splits the CSV from the <paramref name="path"/>. The CSV must be split by tabs[\t].
        /// </summary>
        /// <param name="path">Path of the CSV</param>
        /// <returns>two dimensional string array of the cells</returns>
        public static string[][] GetCsv(string path)
        {
            List<string[]> csv = new List<string[]>();
            try
            {
                int differentLenght = 0;
                string fileText = File.ReadAllText(path, Encoding.Unicode);
                string[] csvLines = fileText.Split('\n');
                int headerSize = CleanLine(csvLines[0]).Split('\t').Length;
                foreach(string line in csvLines)
                {
                    string[] lineSplit = CleanLine(line).Split('\t');
                    if(lineSplit.Length > 1000)
                    {
                        //this could lead to errors later on. ALl the CSV this needs to handle do not have more that 200 columns. If this error does get called, there are empty cells that get detected ass filled ones.
                        log.Error($"The CSV [{path}] line cell amount {lineSplit.Length} is bigger than 1000 columns. Cancel operation.");
                        //log.Debug(line);
                        continue;//skip the line
                    }
                    if(lineSplit.Length != headerSize)
                    {
                        differentLenght++;
                        continue;
                    }
                    bool empty = true;
                    foreach(string cell in lineSplit)
                    {
                        if(!string.IsNullOrWhiteSpace(cell))
                        {
                            empty = false;
                            break;
                        }
                    }
                    if(empty)
                        continue;
                    csv.Add(lineSplit);
                }
                if(differentLenght > 10)
                {
                    log.Warn($"Skipped {differentLenght} lines with different length than the header line.");
                }
                string[][] csvArray = csv.ToArray();
                csv.Clear();
                return csvArray;
            }
            catch(IOException e)
            {
                log.Info($"File still open: " + e.Message);
                new MessagePopUp(Resources.ChooseFileFirst, Settings.Default.ShowTimeErrorSec).ShowDialog();
                return new string[][] { };
            }
        }

        /// <summary>
        /// Returns the first row of a CSV by loading it from a path.
        /// </summary>
        /// <remarks>If possible use <see cref="GetHeaderRow(string[][])"/> as it is more efficient</remarks>
        /// <param name="path">Path to the CSV</param>
        /// <returns>First row of the CSV</returns>
        public static string[] GetHeaderRow(string path)
        {
            try
            {
                return GetCsv(path)[0];
            }
            catch(IndexOutOfRangeException)
            {
                return new string[] { };
            }
            catch(ArgumentException)
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// Returns the first row of a CSV
        /// </summary>
        /// <param name="csv">CSV</param>
        /// <returns>First row of the CSV</returns>
        public static string[] GetHeaderRow(string[][] csv)
        {
            try
            {
                return csv[0];
            }
            catch(IndexOutOfRangeException)
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// Verifies the header of the CSV with the <see cref="Pair"/> s of the <see
        /// cref="Mapping"/>. Also tests if all <see cref="Mapping.essentialPairs"/> are in the
        /// <paramref name="mapping"/>.
        /// </summary>
        /// <param name="headerRow">first row of the CSV</param>
        /// <param name="mapping">the <see cref="Mapping"/> that is used to verify the csv</param>
        /// <returns>
        /// <see cref="Pair"/> array of missing pairs. Count = 0 when there is nothing missing
        /// </returns>
        public static Pair[] Verify(string[] headerRow, Mapping mapping)
        {
            if(headerRow.Length == 0)
            {
                return new Pair[] { }; //The file is still open
            }
            Pair[] pairs = mapping.GetPairs();
            Pair[] essentialPairs = mapping.essentialPairs;

            //check if all the columns match the mapping
            List<Pair> missingPairs = new List<Pair>();
            headerRow = headerRow.Select(t => SQL.PreventSQLInjection(t)).ToArray();
            foreach(Pair pair in pairs)
            {
                //TODO_NEWPAIRTYPE: if the pair uses columns add it here
                if(Objects.PairTypes.PairCsvColumn.type.Equals(pair.GetPairType()) ||
                    Objects.PairTypes.PairCsvColumnWithDiscount.type.Equals(pair.GetPairType()) ||
                    Objects.PairTypes.PairCsvColumnWithDiscountValue.type.Equals(pair.GetPairType()) ||
                    Objects.PairTypes.PairConcatCsvColumns.type.Equals(pair.GetPairType()) || 
                    Objects.PairTypes.PairAlternativeCsvColumn.type.Equals(pair.GetPairType())) //the pair needs a column
                {
                    if(!headerRow.Contains(pair.GetSource()))
                    {
                        missingPairs.Add(pair);
                    }
                }
                if(Objects.PairTypes.PairConcatCsvColumns.type.Equals(pair.GetPairType()))
                {
                    if(!headerRow.Contains(pair.GetAdditionalSource()))
                    {
                        missingPairs.Add(pair);
                    }
                }
            }
            if(missingPairs.Count != 0)
                return missingPairs.ToArray();

            //check if the mapping contains all the essential pairs
            string[] targets = pairs.Select(p => p.GetTargetField()).ToArray();
            foreach(Pair essentialPair in essentialPairs)
            {
                if("EAN".Equals(essentialPair.GetName().ToUpper()))
                    continue; //mapping can get verified without EAN but it is still an essential pair
                if(!targets.Contains(essentialPair.GetTargetField()))
                    missingPairs.Add(essentialPair);
            }
            return missingPairs.ToArray();
        }

        /// <summary>
        /// Cleans the <paramref name="line"/> of a CSV to prevent errors.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>A cleaned line.</returns>
        private static string CleanLine(string line)
        {
            line = line.Replace("\r", string.Empty);
            line = line.Replace("\\", "/");
            return line;
        }
    }
}