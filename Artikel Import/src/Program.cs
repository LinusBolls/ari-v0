using Artikel_Import.src.Backend;
using Artikel_Import.src.Backend.Automatic;
using Artikel_Import.src.Frontend;
using log4net;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Artikel_Import.src
{
    /// <summary>
    /// If any questions come up please read the ReadMe.md on https://github.com/inha-gmbh/Artikel-Import
    /// please make sure to keep this code documented and update the documentation in the read me :)
    ///
    /// https://github.com/inha-gmbh/Artikel-Import/blob/master/README.md
    ///
    /// Entry point of the application starting <see cref="MainForm"/>. On Exception it sends an E-Mail to the address saved in the settings.
    /// </summary>
    public static class Program
    {


        /*
         * TODO:
         * 
         * - "Einheit/Verpackung", "Menge/Verpackung" (so heissen eine Zeile im GUI beim Mapping) entfernen!
         *      - zugehoeriges Feld in DB soll NULL werden (erledigt)
         *     NICHT DIE BREAKPOIONT ENTFERNEN: SIE MARKIEREN BEREITS ALLE STELLEN AN DEN EINE UMSTELLUNG AUF PFLEGENUMMER NOETIG WAERE
         * - Abgleich ueber (EAN und ArtikelNr.) (Pflegenummer)
         * - einlesen von Excel Tabellen soll moeglich sein
         * - Rabatte werden derzeit mit integers eingelsenen, zumindest sind keine gebrochenen Zahlen moeglich -> auf floats gehen
         *
         *
         *
         * */


        /*
        - Fragen:

            - Ist das Rabattsystem in Enventa auf 100 verschiedene Rabatte beschraenkt ??
            - Wie laeuft der Prozess des Ausrollens ab ?
            - Kann das chaotische Lager nur eingepflegt werden wenn wir das Hauptlager einmal deleten?
            - wie heisst die tabelle in der die möglichen lagerorte defniniert werden? wir haben vor sie hart in die db zu schreiben 
        */






        //TODO: add functionality -> option making a pair optional (or do that everywhere when the pair is not essential), that when there is an error the row will be imported anyway
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Executes the whole program automatically. Settings are set in <see cref="Properties.Settings.Default"/>.
        /// </summary>
        private static void Automatic()
        {
            log.Info("Automatic Start");
            //Clear TempDatabase
            ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
            import.ClearTempDatabase();
            //Import all mappings to TempDatabase
            MassImportFromCsvToTempDb.MassImport(Properties.Settings.Default.MassImportFilePath);
            //create RealDatabase backup
            string timeStr = DateTime.Now.ToString("yyyyMMdd");
            string[] cmds = new string[]
            {
                $"CREATE TABLE {Constants.TableArtikel}_{timeStr} AS SELECT * FROM {Constants.TableArtikel}",
                $"CREATE TABLE {Constants.TableEinkauf}_{timeStr} AS SELECT * FROM {Constants.TableEinkauf}",
                $"CREATE TABLE {Constants.TablePreisGruppen}_{timeStr} AS SELECT * FROM {Constants.TablePreisGruppen}"
            };
            using(SQL sql = new SQL())
                sql.ExecuteCommands(cmds);
            //Export to RealDatabase
            ExportFromTempDbToRealDb.Export(null, null);
            if(Properties.Settings.Default.AutomaticallyCleanArticles)
                CleanArticlesInRealDb.CleanAll(Properties.Settings.Default.MaximumArticleRenamingTimeInHours);
            log.Info("Automatic Done");
        }

        /// <summary>
        /// Import of the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {

            //ebiss.write_ebis_tz_column();
            //Automatic();
            //return;


            //initialize log4net
            log4net.Config.XmlConfigurator.Configure();
            log.Info("Application start");
            log.Info($"Arguments:" +
                $"\n{string.Join(";", args)}" +
                $"\nSettings:" +
                $"\n\tAutomaticallyRenameArticles:{Properties.Settings.Default.AutomaticallyCleanArticles}" +
                $"\n\tDatabaseName:{Properties.Settings.Default.DatabaseName}" +
                $"\n\tInitialDirectory:{Properties.Settings.Default.InitialDirectory}" +
                $"\n\tMaximumArticleRenamingTimeInHours:{Properties.Settings.Default.MaximumArticleRenamingTimeInHours}" +
                $"\n\tMassImportFilePath:{Properties.Settings.Default.MassImportFilePath}" +
                $"\n\tShowTimeErrorSec:{Properties.Settings.Default.ShowTimeErrorSec}" +
                $"\n\tShowTimeWarningSec:{Properties.Settings.Default.ShowTimeWarningSec}");
            //start

            
            if (args.Length > 0)
            {
                if(args.Any("h".Contains))
                {
                    Console.WriteLine("Help Message for Artikel Import:\nd -> test database will be used\na -> automatic import will be executed\nc 1 -> articles will be cleaned for one hour\nfor more information look at the inline documentation");
                }

                if(args.Any("d".Contains) || args.Any("D".Contains))
                {
                    Constants.isDebug = true;
                }

                if(args.Any("a".Contains))
                {
                    Automatic();
                    return;
                }

                if(args.Any("c".Contains))
                {
                    int maxTimeHours = Properties.Settings.Default.MaximumArticleRenamingTimeInHours;
                    int index = Array.IndexOf(args, "c");
                    if(args.Length > 1)
                        int.TryParse(args[index + 1], out maxTimeHours);
                    CleanArticlesInRealDb.CleanAll(maxTimeHours);
                    return;
                }

                //no arguments given
                Console.WriteLine("Use 'Artikel Import.exe -h' for help regarding arguments");
            }

            if(Constants.isDebug)
            {
                /*Setup setup = new Setup();
                if (!setup.IsConnectedToDatabase())
                {
                    log.Info(Properties.Resources.NoInternetConnection);
                    new MessagePopUp(Properties.Resources.NoInternetConnection).ShowDialog();
                    return;
                }
                if (!setup.IsDatabaseSetup())
                setup.SetupDatabase();
                log.Info("Application setup complete");*/
                log.Info("Application Start in Debug Mode");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                //catches every error
                try
                {
                    Setup setup = new Setup();
                    if(!setup.IsConnectedToDatabase())
                    {
                        new MessagePopUp(Properties.Resources.NoInternetConnection).ShowDialog();
                        return;
                    }
                    //if (!setup.IsDatabaseSetup())
                    //    setup.SetupDatabase();

                    //if the programm is used by a regular user, hide console.
                    //TODO: WARNING the programm is still going to be super slow. Please use Windows Application as Application Output Type instead of console application
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.GetAppender("ConsoleAppender").Close();
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
                    log.Info("Application setup complete");
                    log.Info("Application Start in Production Mode");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                catch(Exception ex)
                {
                    try
                    {
                        log.Fatal($"Fatal not caught Error.", ex);
                        new MessagePopUp(Properties.Resources.Error + $": {ex.Message}").ShowDialog();
                    }
                    catch(Exception e)
                    {
                        log.Fatal("Error when trying to show error", e);
                    }
                }
            }
        }
    }
}