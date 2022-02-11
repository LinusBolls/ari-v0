using Artikel_Import.src.Backend.Objects;
using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Connection to the OracleSQL Database used for any interaction between the program and the
    /// database. Always returns a <see cref="SqlReport"/>, to show how the execution went.
    /// </summary>
    public class SQL : IDisposable
    {
        private readonly string connectionString = $"DATA SOURCE = {Properties.Settings.Default.DatabaseName}; PASSWORD = {Constants.DatabasePassword}; PERSIST SECURITY INFO = True; USER ID = SYSADM; STATEMENT CACHE SIZE = 1";
        private readonly string connectionStringTest = $"DATA SOURCE = NVTEST; PASSWORD = Ad4qQ7fWlb; PERSIST SECURITY INFO = True; USER ID = SYSADM; STATEMENT CACHE SIZE = 1";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private OracleConnection con;

        /// <summary>
        /// Removes command chars in order to prevent a SQLInjection via values.
        /// </summary>
        /// <param name="value">the value that will get inserted into a query</param>
        /// <returns>cleaned value</returns>
        public static string PreventSQLInjection(string value)
        {
            if(value == null)
            {
                value = "";
            }
            while(value.Contains('\''))
                value = value.Remove(value.IndexOf('\''), 1);
            return value;
        }

        /// <summary>
        /// close function
        /// </summary>
        public void Close()
        {
            if(con != null)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }

        /// <summary>
        /// Opens connection to the Database.
        /// </summary>
        /// <returns>true when connection was created</returns>
        public bool Connect()
        {
            if(con != null)
            {
                if(con.State == ConnectionState.Open)
                    return true;
            }
            try
            {
                con = new OracleConnection();
                con.ConnectionString = connectionString;
                if(Constants.isDebug)
                    con.ConnectionString = connectionStringTest;
                con.Open();
                if(con.State != ConnectionState.Open)
                {
                    log.Error("Connection could not be opened.");
                    log.Error("Connection state: " + con.State);
                }
                return con.State == ConnectionState.Open;
            }
            catch(Exception ex)
            {
                log.Error("Error when trying to connect to Database.", ex);
                return false;
            }
        }

        /// <summary>
        /// Closes the <see cref="con"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>report</returns>
        public SqlReport ExecuteCommand(string cmd)
        {
            if(!Connect())
                return new SqlReport(1, 0, 0);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int successful = 0;
            using(OracleTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                OracleCommand c = new OracleCommand(cmd, con)
                {
                    Transaction = transaction
                };
                c.Prepare();
                try
                {
                    c.ExecuteNonQuery();
                    successful = 1;
                }
                catch(Exception ex)
                {
                    log.Error(c.CommandText);
                    log.Error(ex.Message);
                }
                if(successful == 1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback(); //undo command execution
                    log.Debug($"Rollback 1 command");
                }
            }
            //Close();
            stopWatch.Stop();
            return new SqlReport(1, successful, stopWatch.ElapsedMilliseconds / 1000);
        }

        /// <summary>
        /// Executes multiple commands
        /// </summary>
        /// <param name="cmds">array of commands</param>
        /// <param name="rollbackOnError"> if true, is there an error in any command, all will be rolled back</param>
        /// <param name="isError">if true, on exception throws error instead of warning</param>
        /// <returns>report</returns>
        public SqlReport ExecuteCommands(string[] cmds, bool rollbackOnError = true, bool isError = true)
        {
            //log.Info($"Executing {cmds.Length} SQL Commands");
            if(!Connect())
                return new SqlReport(cmds.Length, 0, 0);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int successful = cmds.Count();
                using(OracleTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    for(int i = 0;i < cmds.Length;i++)
                    {
                        using(OracleCommand c = new OracleCommand(cmds[i], con))
                        {
                            c.Transaction = transaction;
                            try
                            {
                                c.ExecuteNonQuery();
                            }
                            catch(Exception ex)
                            {
                                if(isError)
                                    log.Error($"Execute Command {c.CommandText} created an error.", ex);
                                else
                                    log.Warn($"Execute Command {c.CommandText} created an error.", ex);
                                successful -= 1;
                                if(rollbackOnError)
                                    break;
                            }
                        }
                    }
                    if(successful == cmds.Length)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        if(rollbackOnError)
                        {
                            transaction.Rollback(); //undo command execution
                            log.Info($"Rollback {cmds.Length} commands");
                        }
                    }
                }
            stopWatch.Stop();
            return new SqlReport(cmds.Count(), successful, stopWatch.ElapsedMilliseconds / 1000);
        }

        /// <summary>
        /// Executes multiple commands and shows progress in a <paramref name="progressBar"/> and
        /// the Task bar
        /// </summary>
        /// <param name="cmds">array of commands</param>
        /// <param name="progressBar">to display progress</param>
        /// <param name="rollbackOnError"> if true, is there an error in any command, all will be rolled back</param>
        /// <returns>report</returns>
        public SqlReport ExecuteCommands(string[] cmds, ProgressBar progressBar, bool rollbackOnError = true)
        {
            log.Info($"Executing {cmds.Length} SQL Commands");
            if(!Connect())
                return new SqlReport(cmds.Length, 0, 0);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            progressBar.Maximum = cmds.Count();
            progressBar.Value = 0;
            int successful = cmds.Count();
            using(OracleTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach(string s in cmds)
                {
                    OracleCommand c = new OracleCommand(s, con)
                    {
                        Transaction = transaction
                    };
                    try
                    {
                        c.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        log.Error(c.CommandText);
                        log.Error(ex.Message);
                        successful -= 1;
                        if(rollbackOnError)
                            break;
                    }
                    try
                    {
                        progressBar.Value += 1;
                    }
                    catch(ArgumentOutOfRangeException ex)
                    {
                        log.Error(ex.ParamName + " " + ex.Message);
                    }
                    if(progressBar.Value % (progressBar.Maximum / 5) == 0)
                    {
                        log.Info($"Progress: {progressBar.Value}/{progressBar.Maximum} ETR:{Math.Round(((double)stopWatch.ElapsedMilliseconds / 1000) / progressBar.Value * (progressBar.Maximum - progressBar.Value))}s l/s: {Math.Round(progressBar.Value / ((double)stopWatch.ElapsedMilliseconds / 1000))}");
                    }
                }
            if(successful == cmds.Length)
            {
                transaction.Commit();
            }
            else
            {
                if(rollbackOnError)
                {
                    transaction.Rollback(); //undo command execution
                    log.Info($"Rollback {cmds.Length} commands");
                }
            } 
            }
            //Close();
            stopWatch.Stop();
            return new SqlReport(cmds.Count(), successful, stopWatch.ElapsedMilliseconds / 1000);
        }

        /// <summary>
        /// Executes multiple commands in the background using a <paramref name="worker"/>.
        /// </summary>
        /// <param name="worker">schedules task in the background</param>
        /// <param name="e">arguments for the worker</param>
        /// <param name="cmds">array of commands</param>
        /// <param name="rollbackOnError"> if true, is there an error in any command, all will be rolled back</param>
        /// <returns>report</returns>
        public SqlReport ExecuteCommands(BackgroundWorker worker, DoWorkEventArgs e, string[] cmds, bool rollbackOnError = true)
        {
            //log.Info($"Executing {cmds.Length} SQL Commands");
            if(!Connect())
                return new SqlReport(cmds.Length, 0, 0);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            worker.ReportProgress(0, $"Executing {cmds.Length} SQL commands...");
            int successful = cmds.Length;
            int counter = 0;
            using(OracleTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted))
            {

                foreach(string s in cmds)
                {
                    if(worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return new SqlReport(cmds.Length, cmds.Length - successful, stopWatch.ElapsedMilliseconds / 1000);
                    }
                    OracleCommand c = new OracleCommand(s, con)
                    {
                        Transaction = transaction
                    };
                    try
                    {
                        c.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        log.Error(c.CommandText);
                        log.Error(ex.Message);
                        successful -= 1;
                        if(rollbackOnError)
                            break;
                    }
                    counter += 1;
                    worker.ReportProgress(counter * 10000 / cmds.Length);
                }
                if(successful == cmds.Length)
                {
                    transaction.Commit();
                }
                else
                {
                    if(rollbackOnError)
                    {
                        transaction.Rollback(); //undo command execution
                        log.Info($"Rollback {cmds.Length} commands");
                    }
                }
            }
            stopWatch.Stop();
            return new SqlReport(cmds.Length, successful, stopWatch.ElapsedMilliseconds / 1000);
        }

        /// <summary>
        /// Executes multiple Queries used for retrieving data
        /// </summary>
        /// <param name="commandStr">query to be executed</param>
        /// <param name="columns">amount of columns per query</param>
        /// <returns>array of string array where every string array is for every result/row</returns>
        public string[][] ExecuteMultiLineQuery(string commandStr, int columns)
        {
            if(!Connect())
            {
                log.Error("not connected");
                return new string[0][];
            }
            OracleCommand cmd = new OracleCommand
            {
                Connection = con,
                CommandText = commandStr,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            if(!dr.HasRows)
            {
                //The query returned no rows
                return new string[0][];
            }

            List<string[]> s = new List<string[]>();
            while(dr.Read())
            {
                List<string> row = new List<string>();
                try
                {
                    for(int i = 0;i < columns;i++)
                    {
                        row.Add(dr.GetString(i));
                    }
                }
                catch(InvalidCastException)
                {
                    //do nothing
                }
                s.Add(row.ToArray());
            }
            con.Dispose();
            //Close();
            return s.ToArray();
        }

        /// <summary>
        /// Executes a single Query used for retrieving data
        /// </summary>
        /// <param name="commandStr">query to be executed</param>
        /// <param name="responseIndex">column of the query that should get returned</param>
        /// <returns>a string for every result/row</returns>
        public string[] ExecuteQuery(string commandStr, int responseIndex)
        {
            if(!Connect())
            {
                log.Error("not connected");
                return new string[0];
            }
            OracleCommand cmd = new OracleCommand
            {
                Connection = con,
                CommandText = commandStr,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            if(!dr.HasRows)
            {
                //log.Error("Has no rows");
                return new string[0];
            }

            List<string> s = new List<string>();
            while(dr.Read())
            {
                try
                {
                    s.Add(dr.GetString(responseIndex));
                }
                catch(InvalidCastException)
                {
                    //do nothing
                }
            }
            con.Dispose();
            //Close();
            return s.ToArray();
        }

        /// <summary>
        /// Executes a single Query used for retrieving data. Always returns the content of the
        /// first column.
        /// </summary>
        /// <param name="commandStr">query to be executed</param>
        /// <returns>a string for every result/row</returns>
        public string[] ExecuteQuery(string commandStr)
        {
            //log.Debug(commandStr);
            if(!Connect())
            {
                log.Error("not connected");
                return new string[0];
            }
            OracleCommand cmd = new OracleCommand
            {
                Connection = con,
                CommandText = commandStr,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            if(!dr.HasRows)
            {
                //log.Error("Has no rows");
                return new string[0];
            }

            List<string> s = new List<string>();
            while(dr.Read())
            {
                try
                {
                    s.Add(dr.GetString(0));
                }
                catch(InvalidCastException)
                {
                    //do nothing
                }
            }
            con.Dispose();
            //Close();
            return s.ToArray();
        }

        /// <summary>
        /// computes PflegeNr given data supplier ID and order ID
        /// </summary>
        /// <param name="dataSupplierID"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetPflegeNr(string dataSupplierID, string orderID)
        {
            orderID = orderID.Replace("'", string.Empty);
            int length = dataSupplierID.Length + 1 + orderID.Length;
            if(length > 30)
            {
                //PflegeNr can have a maximum length in the Database of 30 chars.
                //If its longer I don't want to shorten the dataSupplierID, because they are always 5 chars
                //I decided to rather cut the orderID at the beginning than the end, because the last char is probably the one making the number distinct and not the first one.
                //The chance of having another orderID of the dataSupplier that matches the in the beginning shortened orderID
                return $"{dataSupplierID}X{orderID.Substring(length - 30)}";
            }
            else
            {
                return $"{dataSupplierID}X{orderID}";
            }
        }
    }
}