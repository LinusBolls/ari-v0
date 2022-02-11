using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Artikel_Import.src.Backend.Automatic
{
    internal class MassImportFromCsvToTempDb
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly List<string> massImportLog = new List<string>();

        /// <summary>
        /// Imports a number of <see cref="Mapping"/> s and <see cref="CSV"/> s. They are in a CSV
        /// path \t mapping name
        /// </summary>
        /// <param name="path">Path to the CSV with mappingNames and paths</param>
        public static void MassImport(string path)
        {
            massImportLog.Add("MassImport Start");
            string[][] mappingsAndPaths = CSV.GetCsv(path).Skip(1).ToArray();
            string folderPath = path.Replace("MassImport.csv", string.Empty);
            massImportLog.Add($"Found {mappingsAndPaths.Length} files to import.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int progress = 0;
            for(int i = 0;i < mappingsAndPaths.Length;i++)
            {
                try
                {
                    log.Info($"Importing {mappingsAndPaths[i][1]}");
                    string mappingPath = folderPath + mappingsAndPaths[i][0];
                    string mappingName = mappingsAndPaths[i][1];
                    Mapping mapping = new Mapping(mappingName);
                    if(mapping == null)
                    {
                        log.Error($"Could not find mapping {mappingName}");
                        progress++;
                        continue;
                    }
                    Pair[] missingPairs = CSV.Verify(CSV.GetHeaderRow(mappingPath), mapping);
                    if(missingPairs.Length != 0)
                    {
                        massImportLog.Add($"Failed to import {mappingsAndPaths[i][1]} could not verify CSV {mappingsAndPaths[i][0]}");
                        log.Info("CSV failed to verify.");
                        progress++;
                        continue;
                    }
                    ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
                    SqlReport report = import.Import(mapping, mappingPath);
                    log.Info($"Imported {mappingsAndPaths[i][1]} Total: {report.GetInitiated()} Success: {Math.Round((double)report.GetSuccessful() / report.GetInitiated() * 100, 2)}%");
                    massImportLog.Add($"Imported {mappingsAndPaths[i][1]} Total: {report.GetInitiated()} Success: {Math.Round((double)report.GetSuccessful() / report.GetInitiated() * 100, 2)}%");
                }
                catch(Exception ex)
                {
                    try
                    {
                        massImportLog.Add($"Failed to import {mappingsAndPaths[i][1]}");
                        log.Fatal($"Fatal error in mapping {mappingsAndPaths[i][1]} for file {mappingsAndPaths[i][0]}.", ex);
                        progress++;
                        continue;
                    }
                    catch
                    {
                        log.Error($"Error while trying to show error.");
                        progress++;
                        continue;
                    }
                }
                progress++;
                log.Info($"Mapping imported {mappingsAndPaths[i][1]}");
                log.Info($"Progress: {progress}/{mappingsAndPaths.Length} Time left: {Math.Round((double)stopwatch.ElapsedMilliseconds / progress * (mappingsAndPaths.Length - progress) / 60000, 2)}min");
            }
            SaveLogFile(Path.GetDirectoryName(path));
            log.Info("Done");
        }

        private static void SaveLogFile(string directoryPath)
        {
            massImportLog.Add("Done");
            string path = Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM-dd") + "_MassImportLog.txt");
            File.WriteAllLines(path, massImportLog.ToArray());
            massImportLog.Clear();
        }
    }
}