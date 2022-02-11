using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Artikel_Import.src.Backend.Automatic
{
    internal class Worker
    {/// <summary>
     /// Used to signal the <see cref="Worker"/> that it will export. Value: EXPORT </summary>
        public const string Export = "EXPORT";

        /// <summary>
        /// Used to signal the <see cref="Worker"/> that it will import. Value: IMPORT
        /// </summary>
        public const string Import = "IMPORT";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string csvPath;
        private readonly Mapping mapping;
        private readonly bool renameArticles;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Create a new Worker for exporting. Starts <see
        /// cref="ExportFromTempDbToRealDb.Export(BackgroundWorker, DoWorkEventArgs)"/>.
        /// </summary>
        /// <param name="task">has to be <see cref="Export"/></param>
        /// <param name="renameArticles">
        /// when true, starts <see cref="ArtikelNrReplace"/> after export
        /// </param>
        /// <exception cref="Exception">when <paramref name="task"/> has the wrong value</exception>
        public Worker(string task, bool renameArticles = false)
        {
            if(!Export.Equals(task))
                throw new Exception("Wrong task");
            log.Info("Task: " + task);
            this.renameArticles = renameArticles;
            InitializeBackgroundWorker();
            backgroundWorker.RunWorkerAsync(Export);
        }

        /// <summary>
        /// Create a new Worker for importing. Starts <see cref="MassImportFromCsvToTempDb.MassImport(string)"/>.
        /// </summary>
        /// <param name="task">has to be <see cref="Import"/></param>
        /// <param name="mapping"><see cref="Mapping"/> that fits the CSV headerRow</param>
        /// <param name="csvPath">Path to the CSV</param>
        /// <exception cref="Exception">when <paramref name="task"/> has the wrong value</exception>
        public Worker(string task, Mapping mapping, string csvPath)
        {
            if(!Import.Equals(task))
                throw new Exception("Wrong task");
            log.Info("Task: " + task);
            InitializeBackgroundWorker();
            this.mapping = mapping;
            this.csvPath = csvPath;
            backgroundWorker.RunWorkerAsync(Import);
        }

        /// <summary>
        /// Cancels the task the <see cref="backgroundWorker"/> executes.
        /// </summary>
        public void Cancel()
        {
            // Cancel the asynchronous operation.
            backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// This event handler is where the actual, potentially time-consuming work is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception">when an unknown argument was given</exception>
        private void BackgroundWorker_DoWork(object sender,
            DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;
            stopwatch.Restart();
            // Assign the result of the computation to the Result property of the DoWorkEventArgs
            // object. This is will be available to the RunWorkerCompleted event handler.
            if(e.Argument.Equals(Import))
            {
                ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
                e.Result = import.Import(mapping, csvPath, worker, e);
            }
            else if(e.Argument.Equals(Export))
                e.Result = ExportFromTempDbToRealDb.Export(worker, e);
            else
                throw new Exception($"Task unknown: {e.Argument}");
        }

        /// <summary>
        /// This event handler updates the progress bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //ignore
        }

        /// <summary>
        /// This event handler deals with the results of the background operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if(e.Error != null)
            {
                log.Info("Completed: error", e.Error);
            }
            else if(e.Cancelled)
            {
                // Next, handle the case where the user canceled the operation. Note that due to a
                // race condition in the DoWork event handler, the Canceled flag may not have been
                // set, even though CancelAsync was called.
                log.Info("Completed: canceled");
            }
            else
            {
                // Finally, handle the case where the operation succeeded.
                log.Info("Completed: successful");
            }
            backgroundWorker.Dispose();
        }

        /// <summary>
        /// Set up the BackgroundWorker object by attaching event handlers.
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
        }
    }
}