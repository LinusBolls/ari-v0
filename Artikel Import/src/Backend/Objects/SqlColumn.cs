using Artikel_Import.src.Backend.Objects;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Part of a <see cref="SqlTable"/> contains information on a tables column. Representation of
    /// a column in the database;
    /// </summary>
    public class SqlColumn
    {
        /// <summary>
        /// Maximum length of the columns. For <see cref="dataType"/> VARCHAR2 it's the char count.
        /// </summary>
        private readonly int dataLength;

        /// <summary>
        /// Positions after the comma for <see cref="dataType"/> NUMBER.
        /// </summary>
        private readonly int dataScale;

        /// <summary>
        /// Data type of the column either VARCHAR2, NUMBER or DATE
        /// </summary>
        private readonly string dataType;

        /// <summary>
        /// When true null can be inserted into the column
        /// </summary>
        private readonly bool isNullable;

        /// <summary>
        /// <see cref="string"/> representation of the column
        /// </summary>
        private readonly string name;

        /// <summary>
        /// <see cref="string"/> name of the <see cref="SqlTable"/> the column is in.
        /// </summary>
        private readonly string tableName;

        /// <summary>
        /// Creates a new representation of a column
        /// </summary>
        /// <param name="name">of the column</param>
        /// <param name="tableName">string name of the <see cref="SqlTable"/></param>
        /// <param name="dataType"></param>
        /// <param name="dataLength"></param>
        /// <param name="dataScale"></param>
        /// <param name="isNullable"></param>
        public SqlColumn(string name, string tableName, string dataType, string dataLength, string dataScale, bool isNullable)
        {
            this.name = name;
            this.tableName = tableName;
            this.dataType = dataType;
            this.dataLength = int.Parse(dataLength);
            if(dataScale == null)
                this.dataScale = 0;
            else
                this.dataScale = int.Parse(dataScale);
            this.isNullable = isNullable;
        }

        /// <summary>
        /// Creates a new representation of a column
        /// </summary>
        /// <param name="name">of the column</param>
        /// <param name="tableName">string name of the <see cref="SqlTable"/></param>
        /// <param name="dataType"></param>
        /// <param name="dataLength"></param>
        /// <param name="dataScale"></param>
        /// <param name="isNullable"></param>
        public SqlColumn(string name, string tableName, string dataType, int dataLength, int dataScale, bool isNullable)
        {
            this.name = name;
            this.tableName = tableName;
            this.dataType = dataType;
            this.dataLength = dataLength;
            this.dataScale = dataScale;
            this.isNullable = isNullable;
        }

        /// <summary>
        /// Compares this column with the <paramref name="columnComparing"/>.
        /// </summary>
        /// <param name="columnComparing">the column to compare with</param>
        /// <returns>if this column is equal to <paramref name="columnComparing"/></returns>
        public bool Equals(SqlColumn columnComparing)
        {
            if(name != columnComparing.name)
            {
                //log.Debug($"Name is different: {name} != {columnComparing.name}");
                return false;
            }
            if(tableName != columnComparing.tableName)
            {
                //log.Debug($"TableName is different: {tableName} != {columnComparing.tableName}");
                return false;
            }
            if(dataLength != columnComparing.dataLength)
            {
                //log.Debug($"DataLength is different: {dataLength} != {columnComparing.dataLength}");
                return false;
            }
            if(dataScale != columnComparing.dataScale)
            {
                //log.Debug($"DataScale is different: {dataScale} != {columnComparing.dataScale}");
                return false;
            }
            if(dataType != columnComparing.dataType)
            {
                //log.Debug($"DataType is different: {dataType} != {columnComparing.dataType}");
                return false;
            }
            if(isNullable != columnComparing.isNullable)
            {
                //log.Debug($"IsNullable is different: {isNullable} != {columnComparing.isNullable}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the <see cref="name"/> of this column
        /// </summary>
        /// <returns>string name</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Creates a string to add this column to a database, that can be executed by <see cref="SQL"/>.
        /// </summary>
        /// <returns>query string representation of this column</returns>
        public string GetQueryString()
        {
            string cmd = $"{name} {dataType}";
            if("NUMBER".Equals(dataType))
            {
                cmd += $"({dataLength},{ dataScale})";
            }
            if("VARCHAR2".Equals(dataType))
            {
                cmd += $"({dataLength})";
            }
            //issue: this can only be added to an empty database with no not null
            if(!isNullable)
                cmd += " NOT NULL";
            return cmd;
        }

        /// <summary>
        /// Inserts this column into the database using <see cref="SQL"/>.
        /// </summary>
        /// <returns>report of success</returns>
        public SqlReport Insert()
        {
            //log.Debug($"ALTER TABLE {tableName} ADD {GetQueryString()}");
            using(SQL sql = new SQL())
                return sql.ExecuteCommand($"ALTER TABLE {tableName} ADD {GetQueryString()}");
        }

        /// <summary>
        /// Removes this column from the database using <see cref="SQL"/>.
        /// </summary>
        /// <returns>report of success</returns>
        public SqlReport Remove()
        {
            //log.Debug($"ALTER TABLE {tableName} DROP COLUMN {name}");
            using(SQL sql = new SQL())
                return sql.ExecuteCommand($"ALTER TABLE {tableName} DROP COLUMN {name}");
        }

        /// <summary>
        /// <see cref="string"/> reference of the column.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"SqlColumn {name}[table: {tableName}; type: {dataType}; length: {dataLength}; scale: {dataScale}; isNullable: {isNullable}]";
        }
    }
}