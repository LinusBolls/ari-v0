using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Artikel_Import.src.Backend
{
    internal class CleanArticlesInRealDb
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Executes all clean operations that are available
        /// </summary>
        public static void CleanAll(int maxTimeHours)
        {
            log.Info("CleanAll(maxTimeHours:{maxTimeHours}) started");
            SqlReport report = MergeArticles();
            if(!report.WasSuccessful())
                log.Warn("MergeArticles: " + report);

            report = WrongEnteredData();
            if(!report.WasSuccessful())
                log.Warn("WrongEnteredData: " + report);

            if(maxTimeHours < 1)
            {
                log.Info("Not renaming articles, because maxTimeHours was smaller than one.");
                return;
            }
            else
            {
                report = RenameArticles(null, null, maxTimeHours);
                if(report.WasSuccessful())
                {
                    log.Info("RenameArticles Successful: " + report);
                }
                else
                {
                    log.Warn("RenameArticles threw an error: " + report);
                }
            }
            log.Info("CleanAll done");
        }

        #region components

        public static SqlReport WrongEnteredData()
        {
            string[] cmds = new string[]
            {
                "UPDATE artikel SET manufactoringlength=NULL WHERE manufactoringlength = 0",
                "UPDATE artikel SET basiccontentquantity=NULL WHERE basiccontentquantity=0 AND basicquantityunit IS NULL",
                "DELETE FROM artikeltext WHERE artikelnr NOT IN (SELECT artikelnr FROM artikel)",
                "UPDATE artikel SET gruppe='000-000' WHERE gruppe NOT IN (SELECT gruppe FROM artikelgruppe)"
            };
            using(SQL sql = new SQL())
                return sql.ExecuteCommands(cmds, false);
        }

        /// <summary>
        /// Renames Articles
        /// </summary>
        /// <param name="maxRunningtimeHours">
        /// amount of hours the renaming will run for, if it didn't finish before that
        /// </param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns>report of all the renaming commands</returns>
        public static SqlReport RenameArticles(BackgroundWorker worker = null, DoWorkEventArgs e = null, int maxRunningtimeHours = -1)
        {
            bool isSilent = true;
            if(worker != null && e != null)
            {
                isSilent = false;
            }
            if(!isSilent)
            {
                if(worker.CancellationPending)
                {
                    e.Cancel = true;
                    return new SqlReport(1, 0, 1);
                }
            }
            log.Info($"Renaming - silent:{isSilent}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using(SQL sql = new SQL())
            {
                log.Info("Reloading article numbers");
                int successful = 0;
                //update original artikelNr
                sql.ExecuteCommand($"update {Constants.TableImportArticles} a set a.artikelnr = NVL((select min(artikelnr) FROM {Constants.TableArtikel} WHERE ean = a.ean), a.originalartikelnr)");
                //get numbers AND orders for renaming.
                //first replace all starting with 999DIV
                //then all other articles ORDER BY total storage movement
                log.Info($"Loading articles to rename");
                string cmd = $"SELECT a.ArtikelNr as ArtikelNrOld, a.OriginalArtikelNr as ArtikelNrNew FROM {Constants.TableImportArticles} a WHERE a.ArtikelNr!=a.OriginalArtikelNr ORDER BY (SELECT 1 FROM dual WHERE a.artikelnr LIKE '999DIV%'), (SELECT sum(abs(menge)) FROM lagerjournal WHERE artikelnr=a.ARTIKELNR) desc nulls last fetch first 1000 rows only";
                string[][] artikelNrsOldAndNew = sql.ExecuteMultiLineQuery(cmd, 2);
                List<string> articleNumbers = new List<string>();
                ArtikelNrReplace artikelNrReplace = new ArtikelNrReplace();
                log.Info($"Renaming {artikelNrsOldAndNew.Length} articles");
                if(!isSilent)
                    worker.ReportProgress(0, $"Renaming {artikelNrsOldAndNew.Length} articles...");
                for(int i = 0;i < artikelNrsOldAndNew.Length;i++)
                {
                    try
                    {
                        string newArticleId = $"'{artikelNrsOldAndNew[i][1]}'";
                        //make it unique
                        bool isUnique = false;
                        int artikelNrIndex = 0;
                        while(!isUnique)
                        {
                            //testing for article number uniqueness -> in this order so the testing takes less time
                            isUnique = !articleNumbers.Contains(newArticleId);
                            if(isUnique)
                                isUnique = sql.ExecuteQuery($"select 1 from {Constants.TableImportArticles} where OriginalArtikelNr={newArticleId}").Length <= 1;
                            if(isUnique)
                                isUnique = sql.ExecuteQuery($"select 1 from {Constants.TableArtikel} where ArtikelNr={newArticleId}").Length == 0;
                            //done checking everything
                            if(isUnique)
                            {
                                articleNumbers.Add(newArticleId);
                            }
                            else
                            {
                                newArticleId = ImportFromCsvToTempDb.GetNextArtikelNr(newArticleId, artikelNrIndex);
                                artikelNrIndex += 1;
                            }
                        }
                        newArticleId = newArticleId.Replace("'", string.Empty);
                        artikelNrReplace.Replace(artikelNrsOldAndNew[i][0], newArticleId);
                        successful += 1;
                    }
                    catch(Exception ex)
                    {
                        //only for testing
                        log.Error(ex.Message);
                        log.Error(ex.StackTrace);
                    }

                    if(!isSilent)
                    {
                        worker.ReportProgress((int)(i * 10000.0 / artikelNrsOldAndNew.Length));
                        if(worker.CancellationPending)
                        {
                            e.Cancel = true;
                            log.Info("Canceled. Reason: user cancellation.");
                            break;
                        }
                    }
                    if(maxRunningtimeHours > 0 && stopwatch.ElapsedMilliseconds > maxRunningtimeHours * 3600000)
                    {
                        log.Info("Canceled. Reason: reached maximum running time");
                        break;
                    }
                }
                stopwatch.Stop();
                return new SqlReport(artikelNrsOldAndNew.Length, successful, stopwatch.ElapsedMilliseconds / 1000);
            }
        }

        /// <summary>
        /// Finds articles that have duplicates AND loads them into the eNVenta merger. This needs to be executed in eNVenta later at Werkzeuge/Artikel zusammenfuehren.
        /// Only fills it up, because merging more than 100 articles leads to errors in eNVenta.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static SqlReport MergeArticles(BackgroundWorker worker = null, DoWorkEventArgs e = null)
        {
            bool isSilent = true;
            if(worker != null && e != null)
            {
                isSilent = false;
            }
            if(!isSilent)
            {
                if(worker.CancellationPending)
                {
                    e.Cancel = true;
                    return new SqlReport(1, 0, 1);
                }
            }
            string getArticlesToMergeCmd = @"
SELECT
    d.artikelnr AS source,
    (SELECT artikelnr FROM artikel WHERE artikelnr not like '%DIV%' AND HLIEFERANT = d.HLIEFERANT AND PURCHORDERNUMBER = d.PURCHORDERNUMBER AND EINHEITL = d.EINHEITL AND geloescht is null ORDER BY artikelnr fetch first 1 rows only) as target
FROM
    artikel d
WHERE
    artikelnr LIKE '%DIV%'
    AND hlieferant IS NOT NULL
    AND PURCHORDERNUMBER IS NOT NULL
    AND geloescht is null
    AND (select artikelnr FROM artikel WHERE artikelnr not like '%DIV%' AND HLIEFERANT= d.HLIEFERANT AND PURCHORDERNUMBER = d.PURCHORDERNUMBER ORDER BY artikelnr fetch first 1 rows only) IS NOT NULL
ORDER BY (SELECT sum(abs(menge)) FROM lagerjournal WHERE artikelnr=d.artikelnr) desc nulls last";
            List<string> mergeCmds = new List<string>();
            SqlReport report;
            using(SQL sql = new SQL())
            {
                string[][] mergeArticleIds = sql.ExecuteMultiLineQuery(getArticlesToMergeCmd, 2);
                int amountReady = int.Parse(sql.ExecuteQuery($"SELECT count(*) FROM nv7_articlemerge WHERE status=1")[0]);
                int amountToInsert = 100 - amountReady;
                if(amountToInsert < 1)
                {
                    log.Info($"Found {mergeArticleIds.Length} articles to merge, but adding none because there are already articles loaded to merge.");
                    return SqlReport.Empty();
                }
                log.Info($"Found {mergeArticleIds.Length} articles to merge, but andding only {amountToInsert} to queue:");
                int amountInserted = 0;
                for(int i = 0;i < mergeArticleIds.Length;i++)
                {
                    if(amountInserted >= amountToInsert)
                        break;
                    if(mergeArticleIds[i].Length != 2)
                        continue;
                    string sourceArticleId = mergeArticleIds[i][0];
                    string targetArticleId = mergeArticleIds[i][1];
                    string cmd = $"insert into NV7_ARTICLEMERGE(ARTICLEIDSOURCE, ARTICLEIDTARGET, STATUS, CLERKID) VALUES('{sourceArticleId}', '{targetArticleId}', 1, 'ADMIN')";
                    log.Info($"merge {sourceArticleId} -> {targetArticleId}");
                    mergeCmds.Add(cmd);
                    amountInserted++;
                }
                if(isSilent)
                {
                    report = sql.ExecuteCommands(mergeCmds.ToArray(), false);
                }
                else
                {
                    report = sql.ExecuteCommands(worker, e, mergeCmds.ToArray(), false);
                }
                log.Info($"Added {amountToInsert} articles to merge queue. Please go to eNVenta  Werkzeuge/Artikel zusammenfuehren to merge the articles");
            }
            return report;
        }

        #endregion components
    }

    /// <summary>
    /// Replace Article numbers in every table AND column in order to rename it without any issue.
    /// </summary>
    public class ArtikelNrReplace
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlLocation[] sqlLocationsToSearch;

        /// <summary>
        /// Prepare ArtikelNrReplace AND create instance
        /// </summary>
        public ArtikelNrReplace()
        {
            sqlLocationsToSearch = GetLocations(Properties.Settings.Default.ArticleCsvRename);//"articleCsvRename.csv"
        }

        /// <summary>
        /// This does an update on every location, not only the one containing the <paramref
        /// name="oldArtikelNr"/>. This will lead to a less than 100% success rate.
        /// </summary>
        /// <remarks>Warning this will take a long time to execute</remarks>
        /// <param name="oldArtikelNr">The ArtikelNr that should be replaced</param>
        /// <param name="newArtikelNr">Will be used to replace the <paramref name="oldArtikelNr"/></param>
        /// <returns>SqlReport of command execution success</returns>
        public SqlReport Replace(string oldArtikelNr, string newArtikelNr)
        {
            log.Info($"Replace {oldArtikelNr} -> {newArtikelNr}");
            //generate update command for every location
            List<string> updateCmds = new List<string>
            {
                //remember old article number so the article will still be found using the old number
                //$"INSERT INTO ARTIKELIMPORT_RENAMED(RENAMEDATE, OLDARTICLEID, NEWARTICLEID, SUCCESSFULEXECUTION) VALUES(SYSDATE, '{oldArtikelNr}', '{newArtikelNr}', NULL)",
                $"delete from EXTERNALARTICLEID where ARTICLEID = '{newArtikelNr}' and ARTICLEIDEXTERNAL='{oldArtikelNr}'",
                $"INSERT INTO EXTERNALARTICLEID(ARTICLEID, ARTICLEIDTYPE, ARTICLEIDEXTERNAL) VALUES('{newArtikelNr}', 'Alte EAN', '{oldArtikelNr}')",
                $"delete from ARTIKELLAGER where artikelnr='{newArtikelNr}'",
                $"delete from ARTICLEINDEX where articleid='{newArtikelNr}'",
                $"delete from PROGNOSE where ARTIKELNR='{newArtikelNr}'",
                $"delete from REPLACEMENTTIMEVALUE where articleid='{newArtikelNr}'"
            };
            foreach(SqlLocation sqlLocation in sqlLocationsToSearch)
            {
                try
                {
                    //log.Debug(sqlLocation);
                    string cmd = $"UPDATE {sqlLocation.table} SET {sqlLocation.column}='{newArtikelNr}' WHERE {sqlLocation.column}='{oldArtikelNr}'";
                    updateCmds.Add(cmd);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
            }
            updateCmds.Add($"delete from ARTICLEINDEX where ARTICLEID='{newArtikelNr}' and SEARCHKEY='{oldArtikelNr}'");
            updateCmds.Add($"insert into ARTICLEINDEX(ARTICLEID, SEARCHKEY) VALUES('{newArtikelNr}', '{oldArtikelNr}')");
            updateCmds.Add($"update {Constants.TableArtikel} set CODE3='{oldArtikelNr}', ArtikelNR='{newArtikelNr}' WHERE ArtikelNr='{oldArtikelNr}'");
            updateCmds.Add($"update ARTIKELIMPORT_RENAMED set SUCCESSFULEXECUTION=1 where OLDARTICLEID='{oldArtikelNr}' and NEWARTICLEID='{newArtikelNr}'");
            using(SQL sql = new SQL())
            {
                try
                {
                    //Execute the commands
                    return sql.ExecuteCommands(updateCmds.ToArray(), true, false);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    return new SqlReport(1, 0, 1);
                }
            }
        }

        /// <summary>
        /// Loads the <see cref="SqlLocation"/> s FROM a <paramref name="csvPath"/>.
        /// </summary>
        /// <param name="csvPath">the CSV file (TABLE;COLUMN)</param>
        /// <returns>Array of <see cref="SqlLocation"/></returns>
        private SqlLocation[] GetLocations(string csvPath)
        {
            bool first_line = true;
            List<SqlLocation> list = new List<SqlLocation>();
            using(var reader = new StreamReader(csvPath))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if(!first_line)
                    {
                        list.Add(new SqlLocation(values[0], values[1]));
                    }
                    else
                    {
                        first_line = false;
                    }
                }
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// A Location in the SQL Tables containing the Table AND Column
    /// </summary>
    public class SqlLocation
    {
        /// <summary>
        /// Column of the SqlLocation
        /// </summary>
        public string column;

        /// <summary>
        /// Table of the SqlLocation
        /// </summary>
        public string table;

        /// <summary>
        /// Create an instance of the SqlLocation
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        public SqlLocation(string table, string column)
        {
            this.table = table.ToUpper();
            this.column = column.ToUpper();
        }

        /// <summary>
        /// Creates string representation
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return $"SqlLocation: table {table} column {column}";
        }
    }
}