using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Representation of a table in the database. Has a <see cref="name"/> and contains <see
    /// cref="SqlColumn"/> s.
    /// </summary>
    public class SqlTable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Collection of all <see cref="SqlColumn"/> s that are in this table
        /// </summary>
        private readonly SqlColumn[] columns;

        /// <summary>
        /// <see cref="string"/> reference of this table.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Loads table from the database. Fills <see cref="columns"/>;
        /// </summary>
        /// <param name="tableName"><see cref="name"/> of the table</param>
        /// <returns>new SqlTable object</returns>
        public SqlTable(string tableName)
        {
            name = tableName;
            using(SQL sql = new SQL())
            {
                //Load columns from the database
                string[][] results = sql.ExecuteMultiLineQuery($"SELECT column_name, table_name, data_type, TO_CHAR(data_length), NVL('0', TO_CHAR(data_scale)), NVL('N', nullable) FROM USER_TAB_COLUMNS WHERE table_name = '{tableName}'", 6);
                List<SqlColumn> columnList = new List<SqlColumn>();
                foreach(string[] columnStr in results)
                {
                    columnList.Add(new SqlColumn(
                        columnStr[0],
                        columnStr[1],
                        columnStr[2],
                        columnStr[3],
                        columnStr[4],
                        "Y".Equals(columnStr[5])
                        ));
                }
                columns = columnList.ToArray();
            }
        }

        /// <summary>
        /// Creates a new SqlTable object with <paramref name="tableName"/> and <paramref name="columns"/>.
        /// </summary>
        /// <param name="tableName">name of the table</param>
        /// <param name="columns"><see cref="SqlColumn"/> s of the table</param>
        public SqlTable(string tableName, SqlColumn[] columns)
        {
            name = tableName;
            this.columns = columns;
        }

        /// <summary>
        /// Returns <see cref="SqlColumn"/> s that are needed in the TempDb <see
        /// cref="Constants.TableImportArticles"/> using <see cref="Field"/> s.
        /// </summary>
        /// <returns></returns>
        public static SqlColumn[] GetSqlColumnsForImportArticles()
        {
            List<SqlColumn> columns = new List<SqlColumn>();
            try
            {
                Field[] fields = Field.LoadFields();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                return new SqlColumn[0];
            }
            foreach(Field field in Field.LoadFields())
            {
                try
                {
                    if(field.GetTargetInRuntime().StartsWith(Constants.TableEinkauf))
                        continue; //everything else gets saved in Articles
                    columns.Add(new SqlColumn(field.GetName(), Constants.TableImportArticles, field.GetSqlType(), field.GetSize(), field.GetScale(), field.IsNullable()));
                }
                catch(Exception ex)
                {
                    log.Error(ex.Message);
                    log.Error(ex.StackTrace);
                }
            }
            return columns.ToArray();
        }

        /// <summary>
        /// Returns <see cref="SqlColumn"/> s that are needed in the TempDb <see
        /// cref="Constants.TableImportEinkauf"/> using <see cref="Field"/> s.
        /// </summary>
        /// <returns></returns>
        public static SqlColumn[] GetSqlColumnsForImportEinkauf()
        {
            List<SqlColumn> columns = new List<SqlColumn>();
            try
            {
                Field[] fields = Field.LoadFields();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                return new SqlColumn[0];
            }
            foreach(Field field in Field.LoadFields())
            {
                if(!field.GetTargetInRuntime().StartsWith(Constants.TableEinkauf))
                    continue; //everything else gets saved in Articles
                columns.Add(new SqlColumn(field.GetName(), Constants.TableImportEinkauf, field.GetSqlType(), field.GetSize(), field.GetScale(), field.IsNullable()));
            }
            return columns.ToArray();
        }

        /// <summary>
        /// Compares columns between tables and returns <see cref="SqlColumn"/> s of the <paramref
        /// name="sqlTableNeeded"/> that are not in the <paramref name="sqlTable"/>.
        /// </summary>
        /// <param name="sqlTableNeeded">containing all columns</param>
        /// <param name="sqlTable">might be missing columns</param>
        /// <returns><see cref="SqlColumn"/> s if there are some missing or null</returns>
        public static SqlColumn[] MissingColumns(SqlTable sqlTableNeeded, SqlTable sqlTable)
        {
            List<SqlColumn> missingColumns = new List<SqlColumn>();
            foreach(SqlColumn columnNeeded in sqlTableNeeded.columns)
            {
                if(sqlTable.HasColumn(columnNeeded))
                {
                    SqlColumn column = sqlTable.columns.Where(c => c.GetName() == columnNeeded.GetName()).ToArray()[0]; //should only have one
                    if(!columnNeeded.Equals(column))
                    {
                        //log.Debug(columnNeeded.GetName());
                        missingColumns.Add(columnNeeded);
                    }
                }
                else
                {
                    missingColumns.Add(columnNeeded);
                }
            }
            return missingColumns.ToArray();
        }

        /// <summary>
        /// Compares this <see cref="SqlTable"/> with another table.
        /// </summary>
        /// <param name="sqlTableComparing">the table to compare with</param>
        /// <returns>if the table are equal</returns>
        public bool Equals(SqlTable sqlTableComparing)
        {
            if(!name.Equals(sqlTableComparing.GetTableName()))
                return false;
            //check if comparing table has all columns
            foreach(SqlColumn column in columns)
            {
                if(!sqlTableComparing.HasColumn(column))
                    return false;
                if(!column.Equals(sqlTableComparing.GetColumn(column.GetName())))
                    return false;
            }
            //check if this table has the same columns as comparing table
            foreach(SqlColumn column in sqlTableComparing.columns)
            {
                if(!HasColumn(column))
                    return false;
                if(!column.Equals(GetColumn(column.GetName())))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the string <see cref="name"/> of the table
        /// </summary>
        /// <returns><see cref="name"/> of the table</returns>
        public string GetTableName()
        {
            return name;
        }

        /// <summary>
        /// Inserts the table with all its <see cref="SqlColumn"/> s into the database.
        /// </summary>
        /// <returns>report of success</returns>
        public SqlReport Insert()
        {
            List<string> columnsQueryStrings = new List<string>();
            foreach(SqlColumn column in columns)
            {
                columnsQueryStrings.Add(column.GetQueryString());
            }
            string cmd = $"CREATE TABLE {name}({string.Join(", ", columnsQueryStrings)})";
            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        /// <summary>
        /// Returns a <see cref="SqlColumn"/> that has the name <paramref name="columnName"/>.
        /// </summary>
        /// <param name="columnName">name of the column that will get returned</param>
        /// <returns>new column</returns>
        private SqlColumn GetColumn(string columnName)
        {
            SqlColumn[] foundColumns = columns.Where(c => c.GetName().Equals(columnName)).ToArray();
            return foundColumns[0]; //should be only one
        }

        /// <summary>
        /// Checks if this table has the <paramref name="columnNeeded"/>.
        /// </summary>
        /// <param name="columnNeeded">the column to check for</param>
        /// <returns>if table contains the <paramref name="columnNeeded"/></returns>
        private bool HasColumn(SqlColumn columnNeeded)
        {
            string columnNeededName = columnNeeded.GetName();
            SqlColumn[] foundColumns = columns.Where(c => c.GetName().Equals(columnNeededName)).ToArray();
            return foundColumns.Length > 0;
        }
    }
}