using Artikel_Import.src.Frontend;
using log4net;
using System;
using System.Collections.Generic;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Used to prepare the application and checks if everything is correctly set up in order for
    /// the application to work. Is being started by <see cref="Program"/> before the <see
    /// cref="Frontend.MainForm"/> start.
    /// </summary>
    public class Setup
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Collection of <see cref="SqlTable"/> that are needed for this application to run properly
        /// </summary>
        private static SqlTable[] sqlTables;

        /// <summary>
        /// Initialize the setup
        /// </summary>
        public Setup()
        {
            log.Info("Application Setup");
            try
            {
                sqlTables = new SqlTable[] {
            new SqlTable(Constants.TableImportDictionary, new SqlColumn[]
            {
                new SqlColumn("MAPPING", Constants.TableImportDictionary, "VARCHAR2", 255, 0, false),
                new SqlColumn("NAME", Constants.TableImportDictionary, "VARCHAR2", 255, 0, false),
                new SqlColumn("KEY", Constants.TableImportDictionary, "VARCHAR2", 255, 0, false),
                new SqlColumn("VALUE", Constants.TableImportDictionary, "VARCHAR2", 255, 0, false)
            }),
            new SqlTable(Constants.TableImportDiscounts, new SqlColumn[]
            {
                new SqlColumn("MAPPING", Constants.TableImportDiscounts, "VARCHAR2", 255, 0, false),
                new SqlColumn("KEY", Constants.TableImportDiscounts, "VARCHAR2", 255, 0, false),
                new SqlColumn("DISCOUNT", Constants.TableImportDiscounts, "VARCHAR2", 255, 0, false)
            }),
            new SqlTable(Constants.TableImportFields, new SqlColumn[]
            {
                new SqlColumn("NAME", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("TARGET_FIELD", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("DESCRIPTION", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("TYPE", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("COLUMN_SIZE", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("COLUMN_SCALE", Constants.TableImportFields, "VARCHAR2", 255, 0, false),
                new SqlColumn("IS_NVL", Constants.TableImportFields, "VARCHAR2", 1, 0, false),
                new SqlColumn("IS_NULLABLE", Constants.TableImportFields, "VARCHAR2", 1, 0, false)
            }),
            new SqlTable(Constants.TableImportMappings, new SqlColumn[]
            {
                new SqlColumn("NAME", Constants.TableImportMappings, "VARCHAR2", 255, 0, false),
                new SqlColumn("PAIR_TYPE", Constants.TableImportMappings, "VARCHAR2", 255, 0, false),
                new SqlColumn("PAIR_TARGET_FIELD", Constants.TableImportMappings, "VARCHAR2", 255, 0, false),
                new SqlColumn("PAIR_SOURCE_FIELD", Constants.TableImportMappings, "VARCHAR2", 255, 0, false),
                new SqlColumn("OVERWRITE", Constants.TableImportMappings, "VARCHAR2", 255, 0, false),
                new SqlColumn("FACTOR", Constants.TableImportMappings, "VARCHAR2", 255, 0, false)
            }),
            new SqlTable(Constants.TableImportArticles, SqlTable.GetSqlColumnsForImportArticles()),
            new SqlTable(Constants.TableImportEinkauf, SqlTable.GetSqlColumnsForImportEinkauf())
        };
            }
            catch(Exception ex)
            {
                new MessagePopUp(Properties.Resources.Error + ": " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Checks if there is a connection to the database possible.
        /// </summary>
        /// <returns>true if connection is possible</returns>
        public bool IsConnectedToDatabase()
        {
            using(SQL sql = new SQL())
            {
                return sql.Connect();
            }
        }

        /// <summary>
        /// Checks if the TempDb is setup.
        /// </summary>
        /// <returns>is the TempDb setup</returns>
        public bool IsDatabaseSetup()
        {
            foreach(SqlTable sqlTableNeeded in sqlTables)
            {
                if(!HasTable(sqlTableNeeded))
                {
                    log.Warn($"Setup.IsDatabaseSetup Missing table {sqlTableNeeded.GetTableName()}");
                    return false;
                }
                SqlTable sqlTable = new SqlTable(sqlTableNeeded.GetTableName());
                if(!sqlTable.Equals(sqlTableNeeded))
                {
                    SqlColumn[] columns = SqlTable.MissingColumns(sqlTable, sqlTableNeeded);
                    foreach(SqlColumn column in columns)
                    {
                        log.Warn($"Setup.IsDatabaseSetup {sqlTable.GetTableName()} column missing {column}");
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets up the TempDb. Creates all needed Tables.
        /// </summary>
        public void SetupDatabase()
        {
            log.Info("Setup.SetupDatabase setting up database. OracelError ORA-01430, ORA-00904 can be ignored.");
            //empty database
            ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
            import.ClearTempDatabase();
            //check for missing tables
            List<SqlTable> missingTables = new List<SqlTable>();
            foreach(SqlTable neededTable in sqlTables)
            {
                if(!HasTable(neededTable))
                {
                    missingTables.Add(neededTable);
                }
            }
            //add missing tables
            foreach(SqlTable missingTable in missingTables)
            {
                missingTable.Insert();
            }
            //check for missing columns
            List<SqlColumn> missingColumns = new List<SqlColumn>();
            foreach(SqlTable sqlTable in sqlTables)
            {
                SqlColumn[] missingColumnsInThisTable = SqlTable.MissingColumns(sqlTable, new SqlTable(sqlTable.GetTableName()));
                missingColumns.AddRange(missingColumnsInThisTable);
            }
            //add missing columns
            foreach(SqlColumn missingColumn in missingColumns)
            {
                missingColumn.Remove(); //Column might exist but with wrong parameters
                missingColumn.Insert();
            }
        }

        /// <summary>
        /// Checks database if it contains a <see cref="SqlTable"/> with the name of the table
        /// <paramref name="neededTable"/>.
        /// </summary>
        /// <param name="neededTable">table that is needed</param>
        /// <returns>does table exists</returns>
        private bool HasTable(SqlTable neededTable)
        {
            using(SQL sql = new SQL())
            {
                string[] result = sql.ExecuteQuery($"select table_name from user_tables where table_name = upper('{neededTable.GetTableName()}')");
                return result.Length > 0;
            }
        }
    }
}