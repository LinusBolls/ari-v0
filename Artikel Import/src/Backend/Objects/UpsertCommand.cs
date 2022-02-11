using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// Upsert Commands are being used to interact with the OrcaleSQL Database using <see
    /// cref="SQL"/>. Every time a <see cref="Field"/>/ <see cref="Pair"/>/ <see cref="Mapping"/>/
    /// <see cref="CustomDictionary"/>/ <see cref="Discount"/> gets changed or articles get moved in
    /// the TempDB by <see cref="ImportFromCsvToTempDb"/> or RuntimeDB by <see
    /// cref="ExportFromTempDbToRealDb"/> this command are being used.
    /// </summary>
    public class UpsertCommand
    {
        private readonly List<string> columns = new List<string>() { "" };
        private readonly string table;
        private List<Tuple<string, string>> arguments;
        private List<Tuple<string, string>> keyArguments;
        private List<Tuple<string, string>> onlyInsertArguments;

        /// <summary>
        /// Prepare new command generation.
        /// </summary>
        /// <param name="table">Target table of the command</param>
        public UpsertCommand(string table)
        {
            this.table = table;
            keyArguments = new List<Tuple<string, string>>();
            arguments = new List<Tuple<string, string>>();
            onlyInsertArguments = new List<Tuple<string, string>>();
        }

        /// <summary>
        /// add a <paramref name="column"/> that is being filled with a <paramref name="value"/> on
        /// insert and update.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void AddArgument(string column, string value)
        {
            column = column.ToUpper();
            if(columns.Contains(column))
                return;
            arguments.Add(new Tuple<string, string>(column, value));
            List<Tuple<string, string>> sorted = arguments.OrderBy(o => o.Item1).Distinct().ToList();
            arguments.Clear();
            arguments.AddRange(sorted);
            columns.Add(column);
        }

        /// <summary>
        /// add a <paramref name="column"/> that is being filled with a <paramref name="value"/>
        /// only on insert.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void AddArgumentOnlyInsert(string column, string value)
        {
            column = column.ToUpper();
            if(columns.Contains(column))
                return;
            onlyInsertArguments.Add(new Tuple<string, string>(column, value));
            onlyInsertArguments = onlyInsertArguments.OrderBy(o => o.Item1).Distinct().ToList();
            columns.Add(column);
        }

        /// <summary>
        /// Add a <paramref name="keyColumn"/>, that is being used to determine if an update or
        /// insert is going to be used.
        /// </summary>
        /// <param name="keyColumn"></param>
        /// <param name="keyValue"></param>
        public void AddKey(string keyColumn, string keyValue)
        {
            //key arguments are columns that must fit  in order to update
            keyColumn = keyColumn.ToUpper();
            if(columns.Contains(keyColumn))
                return;
            keyArguments.Add(new Tuple<string, string>(keyColumn, keyValue));
            keyArguments = keyArguments.OrderBy(o => o.Item1).Distinct().ToList();
            columns.Add(keyColumn);
        }

        /// <summary>
        /// Execute this upsert command
        /// </summary>
        public void Execute()
        {
            string cmd = ToString();
            using(SQL sql = new SQL())
                sql.ExecuteCommands(new string[] { cmd });
        }

        /// <summary>
        /// Convert this command to a string making it ready for execution
        /// </summary>
        /// <returns>command string</returns>
        override public string ToString()
        {
            //order arguments by alphabet
            keyArguments = keyArguments.OrderBy(o => o.Item1).ToList();
            arguments = arguments.OrderBy(o => o.Item1).ToList();
            onlyInsertArguments = onlyInsertArguments.OrderBy(o => o.Item1).ToList();

            StringBuilder stringBuilder = new StringBuilder($"merge into {table} using dual on(");
            stringBuilder.Append(string.Join(" and ", keyArguments.Select(t => $"{t.Item1} = {t.Item2}")));
            stringBuilder.Append(") when matched then update set ");
            List<string> temp = arguments.Select(t => $"{t.Item1} = {t.Item2}").ToList();
            temp.AddRange(onlyInsertArguments.Select(t => $"{t.Item1} = nvl({t.Item1}, {t.Item2})"));
            stringBuilder.Append(string.Join(", ", temp));
            stringBuilder.Append(" when not matched then insert(");
            List<Tuple<string, string>> insertArgs = arguments;
            insertArgs.AddRange(keyArguments);
            insertArgs.AddRange(onlyInsertArguments);
            stringBuilder.Append(string.Join(", ", insertArgs.Select(t => t.Item1)));
            stringBuilder.Append(") values (");
            stringBuilder.Append(string.Join(", ", insertArgs.Select(t => t.Item2)));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
            ;
        }

        /// <summary>
        /// This is being used when moving values between two tables ( <see cref="table"/> and
        /// <paramref name="table2"/>) rather than from scratch.
        /// </summary>
        /// <param name="table2">table that will be used to move data from</param>
        /// <returns>oracle <see cref="SQL"/> command string</returns>
        public string ToStringUsingSecondTable(string table2)
        {
            //order arguments by alphabet
            keyArguments = keyArguments.OrderBy(o => o.Item1).ToList();
            arguments = arguments.OrderBy(o => o.Item1).ToList();
            onlyInsertArguments = onlyInsertArguments.OrderBy(o => o.Item1).ToList();

            StringBuilder stringBuilder = new StringBuilder($"merge into {table} x using {table2} y on(");
            stringBuilder.Append(string.Join(" and ", keyArguments.Select(t => $"{t.Item1} = {t.Item2}")));
            stringBuilder.Append(") when matched then update set ");
            List<string> temp = arguments.Select(t => $"{t.Item1} = {t.Item2}").ToList();
            temp.AddRange(onlyInsertArguments.Select(t => $"{t.Item1} = nvl({t.Item1}, {t.Item2})"));
            stringBuilder.Append(string.Join(", ", temp));
            stringBuilder.Append(" when not matched then insert(");
            List<Tuple<string, string>> insertArgs = keyArguments;
            insertArgs.AddRange(arguments);
            insertArgs.AddRange(onlyInsertArguments);
            stringBuilder.Append(string.Join(", ", insertArgs.Select(t => t.Item1)));
            stringBuilder.Append(") values (");
            stringBuilder.Append(string.Join(", ", insertArgs.Select(t => t.Item2)));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
            ;
        }
    }
}