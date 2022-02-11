using Artikel_Import.Properties;
using Artikel_Import.src.Backend;
using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// Used for displaying progress when importing or exporting. Hosts a <see
    /// cref="BackgroundWorker"/> that can be canceled.
    /// </summary>
    public partial class ProgressPopUp : Form
    {
        /// <summary>
        /// Used to signal the <see cref="ProgressPopUp"/> that it will export. Value: EXPORT
        /// </summary>
        public const string Export = "EXPORT";

        /// <summary>
        /// Used to signal the <see cref="ProgressPopUp"/> that it will import. Value: IMPORT
        /// </summary>
        public const string Import = "IMPORT";

        /// <summary>
        /// Used to signal the <see cref="ProgressPopUp"/> that it will import. Value: IMPORT
        /// </summary>
        public const string RenameArticles = "RENAME";

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string csvPath;
        private readonly Mapping mapping;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Create a new ProgressPopUp for importing. Starts <see
        /// cref="ImportFromCsvToTempDb.Import(Mapping, string, BackgroundWorker, DoWorkEventArgs)"/>
        /// </summary>
        /// <param name="task">has to be <see cref="Import"/></param>
        /// <param name="mapping"><see cref="Mapping"/> that fits the CSV headerRow</param>
        /// <param name="csvPath">Path to the CSV</param>
        /// <exception cref="Exception">when <paramref name="task"/> has the wrong value</exception>
        public ProgressPopUp(string task, Mapping mapping, string csvPath)
        {
            if(!Import.Equals(task))
                throw new Exception("Wrong task");
            log.Info("ProgressPopUp: " + task + " Mapping: " + mapping.GetName());
            InitializeComponent();
            InitializeBackgroundWorker();
            this.mapping = mapping;
            this.csvPath = csvPath;
            label.Text = $"Importing {mapping.GetName()}...";
            backgroundWorker.RunWorkerAsync(Import);
        }

        /// <summary>
        /// Create a new ProgressPopUp for exporting. Starts <see
        /// cref="ExportFromTempDbToRealDb.Export(BackgroundWorker, DoWorkEventArgs)"/>.
        /// </summary>
        /// <param name="task">has to be <see cref="Export"/></param>
        /// <exception cref="Exception">when <paramref name="task"/> has the wrong value</exception>
        public ProgressPopUp(string task)
        {
            if(RenameArticles.Equals(task))
            {
                log.Info("ProgressPopUp: " + task);
                InitializeComponent();
                InitializeBackgroundWorker();
                label.Text = "Renaming articles...";
                backgroundWorker.RunWorkerAsync(RenameArticles);
                return;
            }
            if(!Export.Equals(task))
                throw new Exception("Wrong task");
            log.Info("ProgressPopUp: " + task);
            InitializeComponent();
            InitializeBackgroundWorker();
            label.Text = "Exporting into database...";
            backgroundWorker.RunWorkerAsync(Export);
        }

        /// <summary>
        /// This event handler is where the actual, potentially time-consuming work is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception">when an unknown argument was given</exception>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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
            else if(e.Argument.Equals(RenameArticles))
                e.Result = CleanArticlesInRealDb.RenameArticles(worker, e);
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
            if(e.UserState != null)
                label.Text = e.UserState.ToString();
            int progress = e.ProgressPercentage;
            if(progress == progressBar.Value)
                return;
            if(progress > 0)
            {
                double timePerProgress = stopwatch.ElapsedTicks / (progress * 1.0);
                double ticksRemaining = timePerProgress * ((double)progressBar.Maximum - progress);
                TimeSpan timeSpan = new TimeSpan((long)Math.Round(ticksRemaining));
                //labelTime.Text = timeSpan.ToString(@"d\d hh\h mm\m ss\s");
                labelTime.Text = timeSpan.ToString("g");
            }
            progressBar.Maximum = 10000;
            progressBar.Value = progress;
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
                log.Info("ProgressPopUp Completed: error");
                label.Text = "Error";
                log.Error(e.Error.Message);
                log.Error(e.Error.StackTrace);
                new MessagePopUp(Resources.Error + e.Error.Message, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
            }
            else if(e.Cancelled)
            {
                // Next, handle the case where the user canceled the operation. Note that due to a
                // race condition in the DoWork event handler, the Canceled flag may not have been
                // set, even though CancelAsync was called.
                log.Info("ProgressPopUp Completed: canceled");
                label.Text = "Canceled";
            }
            else
            {
                // Finally, handle the case where the operation succeeded.
                log.Info("ProgressPopUp Completed: successful");
                new MessagePopUp(e.Result.ToString(), Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
            }
            backgroundWorker.Dispose();
            Close();
        }

        /// <summary>
        /// Cancels the task the <see cref="backgroundWorker"/> executes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            backgroundWorker.CancelAsync();

            // Disable the Cancel button.
            buttonCancel.Enabled = false;

            Close();
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