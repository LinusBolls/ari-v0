using Artikel_Import.Properties;
using Artikel_Import.src.Backend.Objects;
using Artikel_Import.src.Frontend;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// This class handles the importing of data from the <see cref="CSV"/> using a <see cref="Mapping"/>.
    /// </summary>
    public class ImportFromCsvToTempDb
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Completely wipes the TempDatabase and removes every entry
        /// </summary>
        /// <returns>Report</returns>
        public SqlReport ClearTempDatabase()
        {
            string[] cmds = new string[]{
                "delete " + Constants.TableImportArticles,
                "delete " + Constants.TableImportEinkauf
            };
            using(SQL sql = new SQL())
                return sql.ExecuteCommands(cmds);
        }

        /// <summary>
        /// Used for creating unique ArtikelNr string.
        /// </summary>
        /// <param name="artikelNr">last not unique artikelNr</param>
        /// <param name="artikelNrIndex">the value that gets added to the artikelNr</param>
        /// <returns>a unique ArtikelNr</returns>
        public static string GetNextArtikelNr(string artikelNr, int artikelNrIndex)
        {
            //remove '
            artikelNr = artikelNr.Remove(artikelNr.Length - 1);
            if(artikelNrIndex == 0)
                return artikelNr += "-0'";
            //remove last added index
            string lastIndex = (artikelNrIndex - 1).ToString();
            artikelNr = artikelNr.Remove(artikelNr.Length - (1 + lastIndex.Length));
            //add new index
            return artikelNr + $"-{artikelNrIndex}'";
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey, TValue}"/> that contains a value from the row for
        /// each <see cref="Field"/>.
        /// </summary>
        /// <param name="mapping">that will be used to map the row to the <see cref="Field"/> s</param>
        /// <param name="header">used to map the row</param>
        /// <param name="row">row of a CSV containing the values that will be put into the Dictionary</param>
        /// <returns>contains a value from the row for each <see cref="Field"/></returns>
        public Dictionary<string, string> GetValueForEachField(Mapping mapping, string[] header, string[] row)
        {
            Dictionary<string, string> fieldValueDict = new Dictionary<string, string>();
            Field[] fields = Field.LoadFields();
            Pair[] pairs = mapping.GetPairs();

            for(int i = 0;i < fields.Length;i++)
            {
                Field field = fields[i];
                Pair pair = Pair.GetPairByTargetField(pairs, field.GetName());
                if(pair == null)
                    continue;
                string fieldName = field.GetName();

                var valueFromPair = pair.GetValueFromRow(row, header, fields, mapping);
                string value = valueFromPair.Item1;
                if(!valueFromPair.Item2)
                {
                    return null;
                }

                //remove SQL adjustments from values
                value = value.Replace("\'", string.Empty);
                //TO_NUMBER(value)
                if(value.StartsWith("TO_NUMBER("))
                {
                    value = value.Substring("TO_NUMBER(".Length);
                    value = value.Substring(0, value.Length - ")".Length);
                }
                //TO_DATE(value, yyyy-MM-dd)
                if(value.StartsWith("TO_DATE("))
                {
                    value = value.Substring("TO_DATE(".Length);
                    value = value.Substring(0, value.Length - ", yyyy-MM-dd)".Length);
                }

                if(fieldValueDict.ContainsKey(fieldName))
                    fieldValueDict[fieldName] = value;
                else
                    fieldValueDict.Add(fieldName, value);
            }

            return fieldValueDict;
        }

        /// <summary>
        /// Imports all the rows of a <see cref="CSV"/> from a <paramref name="csvPath"/> into the
        /// TempDb using a <paramref name="mapping"/>. If <paramref name="worker"/> or <paramref
        /// name="e"/> is null the import will be silent.
        /// </summary>
        /// <param name="worker">
        /// schedules the task in the background, if not added the operation will be silent
        /// </param>
        /// <param name="e">Worker arguments, if not added the operation will be silent</param>
        /// <param name="csvPath">Path to the CSV</param>
        /// <param name="mapping"><see cref="Mapping"/> that fits the CSV headerRow</param>
        /// <returns>Report of Success</returns>
        public SqlReport Import(Mapping mapping, string csvPath, BackgroundWorker worker = null, DoWorkEventArgs e = null)
        {
            int errorCount;
            List<string[]> errorCsv = new List<string[]>();
            bool isSilent = true;
            if(worker != null && e != null)
                isSilent = false;
            if(!isSilent)
            {
                if(worker.CancellationPending)
                {
                    e.Cancel = true;
                    return new SqlReport(0, 1, 0);
                }
            }

            var stopwatchImport = Stopwatch.StartNew();
            errorCount = 0;
            Field[] fields = Field.LoadFields();
            Field[] artikelFields = fields.Where(f => f.GetTargetInRuntime().StartsWith(Constants.TableArtikel) || f.GetTargetInRuntime().StartsWith(Constants.TablePreisGruppen)).ToArray();
            Field[] einkaufFields = fields.Where(f => f.GetTargetInRuntime().StartsWith(Constants.TableEinkauf)).ToArray();

            //sort pairs for faster importing -> pairs that tend to throw errors come first
            Pair[] unsortedPairs = mapping.GetPairs();
            Pair[] pairs = Pair.Sort(unsortedPairs);

            List<string> cmds = new List<string>();
            string[] cmdsArray = new string[] { };
            List<string> articleNumbers = new List<string>();
            List<string> orderNumbers = new List<string>();
            string[][] csv = CSV.GetCsv(csvPath);
            string[] header = CSV.GetHeaderRow(csv);

            if(!isSilent)
                worker.ReportProgress(0, $"Importing {csv.Length} rows for {mapping.GetName()}...");

            if(csv.Length == 0)
            {
                log.Warn("CSV is empty, canceling import");
                return new SqlReport(1, 0, 1);
            }

            log.Info($"Generating commands for {csv.Length - 1} articles");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Pair eanPair = Pair.GetPairByTargetField(pairs, "EAN");
            Pair datenLieferantPair = Pair.GetPairByTargetField(pairs, "DatenLieferant");
            Pair artikelNrPair = Pair.GetPairByTargetField(pairs, "ArtikelNr");
            Pair bestellNrPair = Pair.GetPairByTargetField(pairs, "BestellNr");

            using(SQL sql = new SQL())
            {
                for(int rowNumber = 1;rowNumber < csv.Count();rowNumber++)
                {
                    if(!isSilent)
                    {
                        if(worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return new SqlReport(csv.Length, rowNumber, stopWatch.ElapsedMilliseconds / 1000);
                        }
                    }

                    string bestellNr = GetUniqueBestellNr(bestellNrPair, orderNumbers, csv[rowNumber], header, fields, mapping, isSilent);
                    if(bestellNr == null)
                    {
                        errorCount++;
                        continue;
                    }

                    string datenlNr = datenLieferantPair.GetSource(); // datenlieferant is always a fixed value
                    if(datenlNr == null)
                    {
                        errorCount++;
                        continue;
                    }

                    string pflegeNr = sql.GetPflegeNr(datenlNr, bestellNr);

                    string eanNr = GetEanNr(eanPair, csv[rowNumber], header, fields, mapping);
                    //check if the EAN was found and if the value matches the constraints
                    if(eanNr == null)
                    {
                        eanNr = GenerateMissingEAN(pflegeNr, datenlNr, bestellNr); // generate missing EAN numbers
                    }
                    else if(string.IsNullOrEmpty(eanNr.Replace("\'", string.Empty).Replace(" ", string.Empty))) //EAN is empty
                    {
                        eanNr = GenerateMissingEAN(pflegeNr, datenlNr, bestellNr); // generate missing EAN numbers
                    }
                    if("0".Equals(eanNr.Replace("\'", string.Empty).Replace(" ", string.Empty))) //EAN is 0
                    {
                        eanNr = GenerateMissingEAN(pflegeNr, datenlNr, bestellNr); // generate missing EAN numbers
                    }
                    if(!double.TryParse(eanNr.Replace("\'", string.Empty).Replace(" ", string.Empty), out _)) //EAN is not a number
                    {
                        eanNr = GenerateMissingEAN(pflegeNr, datenlNr, bestellNr); // generate missing EAN numbers
                    }

                    string artikelNr = GetUniqueArtikelNr(artikelNrPair, eanNr, articleNumbers, csv[rowNumber], header, fields, mapping, isSilent);
                    if(artikelNr == null)
                    {
                        errorCount++;
                        continue;
                    }
                    string originalArtikelNr = artikelNr.Substring(1, artikelNr.Length - 2);

                    //get the artikelnr thats being used right now and if one is found use that and change original to the old one
                    string[] rtArtikelNr = sql.ExecuteQuery($"select artikelnr from {Constants.TableArtikel} where ean={eanNr}");
                    if(rtArtikelNr.Length > 0)
                    {
                        if(!"".Equals(rtArtikelNr[0]))
                        {
                            artikelNr = $"'{rtArtikelNr[0]}'";
                        }
                    }

                    //article
                    string artikelCommand = GetImportArtikelCommand(eanNr, artikelNr, originalArtikelNr, pflegeNr, pairs, artikelFields, csv[rowNumber], header, mapping, errorCsv);
                    if(artikelCommand == null)
                    {
                        errorCount++;
                        continue;
                    }
                    //einkauf
                    string einkaufCommand = GetImportEinkaufCommand(datenLieferantPair, bestellNr, artikelNr, pairs, csv[rowNumber], header, einkaufFields, mapping, errorCsv, isSilent);
                    if(einkaufCommand == null)
                    {
                        errorCount++;
                        continue;
                    }
                    //add to commands to execute
                    cmds.Add(artikelCommand);
                    cmds.Add(einkaufCommand);

                    if(!isSilent)
                    {
                        worker.ReportProgress(rowNumber * 10000 / csv.Length);
                    }
                }
                cmdsArray = cmds.ToArray();
                cmds.Clear();
                stopWatch.Stop();
                double errorPercentage = 0;
                try
                {
                    errorPercentage = Math.Round(double.Parse(errorCount + "") / (csv.Length * csv[0].Length), 4);
                }
                catch(Exception ex)
                {
                    log.Error("Calculating errorPercentage: " + ex.Message + "\n" + ex.StackTrace);
                }
                log.Info($"Command generation threw {errorCount} error(s).  Rate {errorPercentage}%");
                if(errorPercentage >= 0.1d)
                {
                    log.Info($"High amount of errors while importing: {errorCount}/{csv.Length * csv[0].Length} -> {errorPercentage}% in {mapping.GetName()}");
                    if(!isSilent)
                        new MessagePopUp(Resources.ErrorsWhileImporting + $": {errorCount}/{csv.Length * csv[0].Length} -> {errorPercentage}%", Settings.Default.ShowTimeErrorSec).ShowDialog();
                }
                //handle errorCsv
                if(errorCsv.Count > 0)
                    WriteErrorCsv(errorCsv, csvPath, isSilent);
                //empty memory
                int csvLength = csv.Length;

                log.Info($"Generating commands done in {mapping.GetName()} took {stopWatch.ElapsedMilliseconds / 1000}s ({Math.Round((double)stopWatch.ElapsedMilliseconds / csvLength, 2)}ms per row)");
                SqlReport report;
                if(isSilent)
                {
                    report = sql.ExecuteCommands(cmdsArray);
                }
                else
                {
                    report = sql.ExecuteCommands(worker, e, cmdsArray);
                }
                log.Info("SQL Report: " + report.ToString());

                if(errorCount > csvLength - 1)
                    errorCount = csvLength - 1;
                stopwatchImport.Stop();
                long timeElapsedSec = report.GetExecTime() + stopwatchImport.ElapsedMilliseconds / 1000;
                return new SqlReport(report.GetInitiated() + csvLength - 1, report.GetSuccessful() + (csvLength - 1 - errorCount), timeElapsedSec);
            }
        }

        /// <summary>
        /// Adds the <paramref name="row"/> to the <paramref name="errorCsv"/>. This will later be
        /// saved in a error file.
        /// </summary>
        /// <param name="header">header of the CSV</param>
        /// <param name="row">that cause the error</param>
        /// <param name="error">Error message</param>
        /// <param name="errorCsv">list with all the errors</param>
        private void AddToErrorCsv(string[] header, string[] row, string error, List<string[]> errorCsv)
        {
            if(errorCsv.Count == 0)
            {
                List<string> headerWithError = header.ToList();
                headerWithError.Add("ERROR_MESSAGE");
                errorCsv.Add(headerWithError.ToArray());
            }
            List<string> rowWithError = row.ToList();
            rowWithError.Add(error);
            errorCsv.Add(rowWithError.ToArray());
        }

        /// <summary>
        /// Replace missing EANs with corresponding ones from runtime database via <paramref
        /// name="pflegeNr"/> [article ID of data supplier] generate missing EANs from runtime
        /// database via oracle sql
        /// sequence: CREATE sequence generateEan_seq increment by 1 start with 1000000000000
        /// nocache nocycle;
        /// </summary>
        /// <param name="pflegeNr">Pflegenummer</param>
        /// <param name="supplierid">used for finding the EAN from the <see cref="Constants.TableArtikel"/></param>
        /// <param name="supplierarticleid">used for finding the EAN from the <see cref="Constants.TableArtikel"/></param>
        /// <returns>new generated EAN or EAN from <see cref="Constants.TableArtikel"/></returns>
        private string GenerateMissingEAN(string pflegeNr, string supplierid, string supplierarticleid)
        {
            using(SQL sql = new SQL())
            {
                // get EANs corresponding to Pflegenummer from runtime database
                string[] results = sql.ExecuteQuery($"Select EAN from {Constants.TableArtikel} where ARTICLEIDOFDATASUPPLIER = '{pflegeNr}'");
                string EAN = null;
                if(results.Length > 0)
                    EAN = results[0]; // choosing results[0] as representative for identical articles
                if(string.IsNullOrEmpty(EAN) || "0".Equals(EAN))
                {
                    // get EANs corresponding to supplierid and supplier article id from runtime database
                    results = sql.ExecuteQuery($"Select EAN from {Constants.TableArtikel} where DATASUPPLIERID = '{supplierid}' and PurchOrderNumber = {supplierarticleid}");
                    if(results.Length > 0)
                        EAN = results[0]; // choosing results[0] as representative for identical articles
                }
                if(string.IsNullOrEmpty(EAN) || "0".Equals(EAN))
                {
                    // get EANs corresponding to main supplier and supplier article id from runtime database
                    results = sql.ExecuteQuery($"Select EAN from {Constants.TableArtikel} where HLIEFERANT = '{supplierid}' and PurchOrderNumber = {supplierarticleid}");
                    if(results.Length > 0)
                        EAN = results[0]; // choosing results[0] as representative for identical articles
                }
                if(string.IsNullOrEmpty(EAN) || "0".Equals(EAN)) // generate new EAN via oracle SQL sequence (SQL code in summary)
                {
                    string newEAN = sql.ExecuteQuery("select TO_CHAR(seq_nextval_on_demand('generateEan_seq')) from dual")[0]; // generate new EAN using dual instead of creating a new table
                    string[] EANchecks = sql.ExecuteQuery($"select artikelnr from artikel where ean='{newEAN}'"); // check if new EAN already exists in runtime database
                    int counter = 0;
                    while(EANchecks.Length != 0) //ean is already assigned to a article
                    {
                        newEAN = sql.ExecuteQuery("select seq_nextval_on_demand('generateEan_seq') from dual")[0]; // generate new EAN using dual instead of creating a new table
                        EANchecks = sql.ExecuteQuery($"select artikelnr from artikel where ean='{newEAN}'"); // check if new EAN already exists in runtime database
                        counter++;
                        if(counter > 20) // if more than 20 newly generated EANs already exist in runtime database throw a warning there is something wrong with SQL sequence
                        {
                            log.Warn($"Newly generated EANs '{newEAN}' already exist");
                            counter = 0;
                        }
                    }
                    EAN = newEAN;
                }
                return $"'{EAN}'";
            }
        }

        private string GetEanNr(Pair eanPair, string[] article, string[] header, Field[] fields, Mapping mapping)
        {
            string eanStr = null;
            if(eanPair != null)
            {
                try
                {
                    var valueFromArticle = eanPair.GetValueFromRow(article, header, fields, mapping);
                    eanStr = valueFromArticle.Item1;
                    eanStr = eanStr.Replace(" ", string.Empty);
                }
                catch(Exception)
                {
                    eanStr = null;
                }
            }
            return eanStr;
        }

        private string GetImportArtikelCommand(string eanNr, string artikelNr, string originalArtikelNr, string pflegeNr, Pair[] pairs, Field[] fields, string[] row, string[] header, Mapping mapping, List<string[]> errorCsv)
        {
            UpsertCommand articleCommand = new UpsertCommand(Constants.TableImportArticles);
            if(!double.TryParse(eanNr.Replace("'", string.Empty).Replace(" ", string.Empty), out _))
            {
                //EAN is not a number - do not import
                AddToErrorCsv(header, row, "EAN is not a number.", errorCsv);
                return null;
            }
            articleCommand.AddKey("EAN", eanNr);
            articleCommand.AddArgument("AenderungsDatum", "SYSDATE");
            articleCommand.AddArgument("Geloescht", "NULL");
            articleCommand.AddArgument("ArtikelNr", $"{artikelNr}");
            articleCommand.AddArgument("OriginalArtikelNr", $"'{originalArtikelNr}'");
            articleCommand.AddArgument("Kalkbasis", "TO_NUMBER('0')");
            articleCommand.AddArgument("BezeichnungFix", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("PflegeNr", $"'{pflegeNr}'");
            articleCommand.AddArgumentOnlyInsert("EintragungsDatum", "SYSDATE");
            for(int p = 0;p < pairs.Length;p++)
            {
                Field[] matchedFileds = Field.GetFieldsByName(fields, pairs[p].GetTargetField());
                if(matchedFileds.Length > 0)
                {
                    Tuple<string, bool, string> valueFromArticle;
                    try
                    {
                        valueFromArticle = pairs[p].GetValueFromRow(row, header, fields, mapping);
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                        AddToErrorCsv(header, row, ex.Message, errorCsv);
                        return null;
                    }
                    string value = valueFromArticle.Item1;
                    if(!valueFromArticle.Item2)
                    {
                        //log.Debug(valueFromArticle.Item3);
                        AddToErrorCsv(header, row, valueFromArticle.Item3, errorCsv);
                        return null;
                    }
                    if("DATE".Equals(matchedFileds[0].GetSqlType()))
                    {
                        //Check format
                        if(!Field.IsDateFormat(value))
                        {
                            AddToErrorCsv(header, row, $"Value {value} does not match date format for pair {pairs[p].GetName()} from source {pairs[p].GetSource()}", errorCsv);
                            return null;
                        }
                    }
                    //special case
                    if(pairs[p].GetTargetField().Equals("Bezeichnung"))
                    {
                        if(string.IsNullOrEmpty(value.Replace(" ", string.Empty).Replace("'", string.Empty)))
                            value = "'Bezeichnung Fehlt'";
                    }
                    if(pairs[p].IsOverwrite())
                        articleCommand.AddArgument(pairs[p].GetTargetField(), value);
                    else
                    {
                        articleCommand.AddArgumentOnlyInsert(pairs[p].GetTargetField(), value);
                    }
                }
            }

            // add columns that have to be filled. Normally the columns are filled automatically by eNVenta
            articleCommand.AddArgumentOnlyInsert("Bezeichnung", "'Bezeichnung Fehlt'");
            articleCommand.AddArgumentOnlyInsert("Preisdatum", "SYSDATE");
            articleCommand.AddArgumentOnlyInsert("VK1_EDE", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("VK2", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("VK3", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("VkPro", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("EinheitVerkauf", "'Stück'");
            articleCommand.AddArgumentOnlyInsert("LEK", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("GEK", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("EkPro", "TO_NUMBER('1')");
            //articleCommand.AddArgumentOnlyInsert("MengeVP", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("EinheitLager", "'Stück'");
            //articleCommand.AddArgumentOnlyInsert("EinheitVerpackung", "'Stück'");
            articleCommand.AddArgumentOnlyInsert("Dispo", "TO_NUMBER('2')");
            articleCommand.AddArgumentOnlyInsert("Stckliste", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("VK1_Brutto", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("PREISDATUMEK", "SYSDATE");
            articleCommand.AddArgumentOnlyInsert("LEK_ALT", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("FAKTORVerkauf", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("FAKTORVL", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("StuecklisteKonfig", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("GEWICHTGROESSEPRO", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("RZGEWICHTPRO", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("FIXEDSET", "TO_NUMBER('0')");
            articleCommand.AddArgumentOnlyInsert("FaktorVolumen", "TO_NUMBER('1')");
            articleCommand.AddArgumentOnlyInsert("ITEMSLISTPRICEONSUBITEM", "TO_NUMBER('1')");
            string cmd = articleCommand.ToString();
            return cmd;
        }

        private string GetImportEinkaufCommand(Pair datenLieferantPair, string bestellNr, string artikelNr, Pair[] pairs, string[] row, string[] header, Field[] fields, Mapping mapping, List<string[]> errorCsv, bool isSilent = false)
        {
            UpsertCommand einkauf = new UpsertCommand(Constants.TableImportEinkauf);
            Tuple<string, bool, string> valueFromArticle = datenLieferantPair.GetValueFromRow(row, header, fields, mapping);
            einkauf.AddKey("DatenLieferant", valueFromArticle.Item1);
            if(!valueFromArticle.Item2)
            {
                log.Warn($"Get Datenlieferant failed source: '{datenLieferantPair.GetSource()}'");
                if(!isSilent)
                    new MessagePopUp(Resources.MissingDatenlieferant, Settings.Default.ShowTimeErrorSec).ShowDialog();
                AddToErrorCsv(header, row, Resources.MissingDatenlieferant, errorCsv);
                return null;
            }
            einkauf.AddKey("BestellNr", bestellNr);
            einkauf.AddArgument("ArtikelNr", $"{artikelNr}");
            einkauf.AddArgument("AenderungsDatum", "SYSDATE");
            einkauf.AddArgumentOnlyInsert("ErstellungsDatum", "SYSDATE");
            for(int p = 0;p < pairs.Length;p++)
            {
                Field[] matchedFileds = Field.GetFieldsByName(fields, pairs[p].GetTargetField());
                if(matchedFileds.Length > 0)
                {
                    try
                    {
                        valueFromArticle = pairs[p].GetValueFromRow(row, header, fields, mapping);
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                        AddToErrorCsv(header, row, valueFromArticle.Item3, errorCsv);
                        return null;
                    }
                    string value = valueFromArticle.Item1;
                    if(!valueFromArticle.Item2)
                    {
                        //log.Debug(valueFromArticle.Item3);
                        AddToErrorCsv(header, row, valueFromArticle.Item3, errorCsv);
                        return null;
                    }
                    if(pairs[p].IsOverwrite())
                        einkauf.AddArgument(pairs[p].GetTargetField(), value);
                    else
                        einkauf.AddArgumentOnlyInsert(pairs[p].GetTargetField(), value);
                }
            }
            //these are the values that are being inserted, when none where entered
            einkauf.AddArgumentOnlyInsert("EinkaufKundenNR", "TO_Number('0')");
            einkauf.AddArgumentOnlyInsert("EinkaufWertstaffel", "TO_Number('0')");
            einkauf.AddArgumentOnlyInsert("EinkaufKonditionsart", "TO_Number('0')");
            string cmd = einkauf.ToString();
            return cmd;
        }

        private string GetUniqueArtikelNr(Pair artikelNrPair, string ean, List<string> articleNumbers, string[] row, string[] header, Field[] fields, Mapping mapping, bool silent = false)
        {
            //get artikelNr
            var valueFromArticle = artikelNrPair.GetValueFromRow(row, header, fields, mapping);
            string artikelNr = valueFromArticle.Item1;
            artikelNr = artikelNr.ToUpper(); //ArtikelNr needs to be in upper chars only, otherwise eNVenta throws errors.
            if(!valueFromArticle.Item2)
            {
                log.Warn($"Failed to get ArtikelNr in {artikelNrPair.GetMappingName()} returning null");
                if(!silent)
                    new MessagePopUp(Resources.MissingArtikelNr, Settings.Default.ShowTimeWarningSec).ShowDialog();
                return null;
            }
            //make it unique
            bool isUnique = false;
            int artikelNrIndex = 0;
            using(SQL sql = new SQL())
            {
                while(!isUnique)
                {
                    //testing for article number uniqueness -> in this order so the testing takes less time
                    isUnique = !articleNumbers.Contains(artikelNr);
                    if(isUnique)
                        isUnique = sql.ExecuteQuery($"select 1 from {Constants.TableImportArticles} where ArtikelNr={artikelNr} and EAN != {ean}").Length == 0;
                    if(isUnique)
                        isUnique = sql.ExecuteQuery($"select 1 from {Constants.TableArtikel} where ArtikelNr={artikelNr} and EAN != {ean}").Length == 0;
                    //done checking everything
                    if(isUnique)
                    {
                        articleNumbers.Add(artikelNr);
                    }
                    else
                    {
                        artikelNr = GetNextArtikelNr(artikelNr, artikelNrIndex);
                        artikelNrIndex += 1;
                    }
                }
            }
            return artikelNr;
        }

        private string GetUniqueBestellNr(Pair bestellNrPair, List<string> orderNumbers, string[] row, string[] header, Field[] fields, Mapping mapping, bool silent = false)
        {
            //make bestellNr unique too
            var valueFromArticle = bestellNrPair.GetValueFromRow(row, header, fields, mapping);
            string bestellNr = valueFromArticle.Item1;
            if(!valueFromArticle.Item2)
            {
                log.Warn($"Failed to get BestellNr in {bestellNrPair.GetMappingName()} returning null");
                if(!silent)
                    new MessagePopUp(Resources.MissingBestellNr, Settings.Default.ShowTimeWarningSec).ShowDialog();
                return null;
            }

            bestellNr = bestellNr.Replace("'", string.Empty);

            int size = Field.GetFieldByTarget(fields, "EINKRABATT.BESTELLNR").GetSize();
            bestellNr = Field.MatchValueSize(bestellNr, size);

            bool isUnique = !orderNumbers.Contains(bestellNr);
            int bestellNrIndex = 0;
            if(!isUnique)
            {
                bestellNr = Field.MatchValueSize(bestellNr, size - 2); //make sure size does not get to big, when chars are being added to make bestellNr unique
            }
            while(!isUnique)
            {
                //testing for order number uniqueness -> in this order so the testing takes less time
                isUnique = !orderNumbers.Contains(bestellNr);
                //done checking everything
                if(isUnique)
                {
                    orderNumbers.Add(bestellNr);
                }
                else
                {
                    bestellNr = GetNextArtikelNr(bestellNr, bestellNrIndex);
                    bestellNrIndex += 1;
                }
            }
            if(string.IsNullOrWhiteSpace(bestellNr) || string.IsNullOrEmpty(bestellNr))
            {
                log.Warn($"Failed to get BestellNr in {bestellNrPair.GetMappingName()} returning null");
                if(!silent)
                    new MessagePopUp(Resources.MissingBestellNr, Settings.Default.ShowTimeWarningSec).ShowDialog();
                return null;
            }
            return $"'{bestellNr}'";
        }

        /// <summary>
        /// Writes all rows that where added to <paramref name="errorCsv"/> into an error file.
        /// </summary>
        /// <param name="errorCsv">List of error lines</param>
        /// <param name="csvPath">
        /// Path of the CSV that created errors. Used for placing the error file
        /// </param>
        /// <param name="isSilent">if true, does not show any <see cref="MessagePopUp"/> s</param>
        private void WriteErrorCsv(List<string[]> errorCsv, string csvPath, bool isSilent = false)
        {
            string[][] file = errorCsv.ToArray();
            errorCsv.Clear();
            errorCsv = new List<string[]>();
            string[] lines = file.Select(r => string.Join(";", r)).ToArray();
            string errorCsvPath = csvPath.Substring(0, csvPath.Length - 4) + "_ERROR_not_imported.csv";
            log.Info($"Writing CSV with {lines.Length} lines to {errorCsvPath}");
            System.IO.File.WriteAllLines(errorCsvPath, lines);
            if(!isSilent)
                new MessagePopUp(Resources.ImportErrorRow + $" {lines.Length - 1} gesichert als {errorCsvPath}", Settings.Default.ShowTimeErrorSec).ShowDialog();
        }
    }
}