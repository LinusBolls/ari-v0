using System;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// Shows the success of the execution of <see cref="SQL"/> queries and commands more easily.
    /// </summary>
    public class SqlReport
    {
        private readonly long executionTimeSec;
        private readonly int initiatedCommands;
        private readonly double msPerCommand;
        private readonly int successfulCommands;
        private readonly double successRate;

        /// <summary>
        /// Create a SqlReport to more easily show the success of the execution of <see cref="SQL"/>
        /// queries and commands.
        /// </summary>
        /// <param name="initiatedCommands">amount of commands initiated</param>
        /// <param name="successfulCommands">amount of successful executed commands</param>
        /// <param name="executionTimeSec">amount of seconds the execution took</param>
        public SqlReport(int initiatedCommands, int successfulCommands, long executionTimeSec)
        {
            this.initiatedCommands = initiatedCommands;
            this.successfulCommands = successfulCommands;
            this.executionTimeSec = executionTimeSec;
            successRate = Math.Round((double)successfulCommands / initiatedCommands, 4);
            if(executionTimeSec == 0)
                msPerCommand = 1;
            else
                msPerCommand = Math.Round(1000.0 / (initiatedCommands / (int)executionTimeSec), 2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Empty SqlReport</returns>
        public static SqlReport Empty()
        {
            return new SqlReport(1, 1, 1);
        }

        /// <summary>
        /// Seconds it took to execute the commands
        /// </summary>
        /// <returns></returns>
        public long GetExecTime()
        {
            return executionTimeSec;
        }

        /// <summary>
        /// amount of commands that got initiated/executed
        /// </summary>
        /// <returns></returns>
        public int GetInitiated()
        {
            return initiatedCommands;
        }

        /// <summary>
        /// amount of successful executed commands
        /// </summary>
        /// <returns></returns>
        public int GetSuccessful()
        {
            return successfulCommands;
        }

        /// <summary>
        /// Returns the Report as a string.
        /// </summary>
        /// <returns>Report string</returns>
        override public string ToString()
        {
            string report = Properties.Resources.ExecutionTook + $" {Math.Round(executionTimeSec / 60.0, 1)}min; " +
                    Properties.Resources.Successfull + $": {successfulCommands}/{initiatedCommands}; " +
                    Properties.Resources.Rate + $": {successRate * 100}%; " +
                    $"Time per command: {msPerCommand}ms";
            return report;
        }

        /// <summary>
        /// Returns true when the report shows that every command was executed successfully
        /// </summary>
        /// <returns></returns>
        public bool WasSuccessful()
        {
            if(successfulCommands < initiatedCommands)
                return false;
            return true;
        }
    }
}