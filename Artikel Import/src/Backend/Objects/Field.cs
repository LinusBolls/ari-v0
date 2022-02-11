using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// Used for sorting fields.
    /// </summary>
    public class CompareFieldTarget : IComparer<Field>
    {
        int IComparer<Field>.Compare(Field f, Field f2)
        {
            return new System.Collections.CaseInsensitiveComparer().Compare(f.GetTargetInRuntime(), f2.GetTargetInRuntime());
        }
    }

    /// <summary>
    ///A field shows the relation of the TempDB to the RuntimeDB and is also being used in making sure values fit the RuntimeDB.
    ///Saved in the database <see cref="Constants.TableImportFields"/>.
    ///The field can be edited in the <see cref="Frontend.MainForm.tabPageFields"/>.
    /// </summary>
    public class Field
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Additional information to the field
        /// </summary>
        private readonly string Description;

        /// <summary>
        /// if is true, null can be inserted into the field.
        /// </summary>
        private readonly bool isNullable;

        /// <summary>
        /// if this is true, the field does not insert null, but keeps the old value
        /// </summary>
        private readonly bool isNVL;

        /// <summary>
        /// the name of the field this gets referenced by the pairs
        /// </summary>
        private readonly string Name;

        /// <summary>
        /// amount of digits after the comma, default is 0
        /// </summary>
        private readonly int Scale;

        /// <summary>
        /// the size of the field in the real time db, important so that it doesn't trow errors when
        /// trying to but in a bigger value
        /// </summary>
        private readonly int Size;

        /// <summary>
        /// either number, varchar2 or date used for converting values
        /// </summary>
        private readonly string SqlType;

        /// <summary>
        /// in the real time db tablename.columnname
        /// </summary>
        private readonly string TargetInRuntime;

        /// <summary>
        /// Create a new Field. A field shows the relation of the TempDB to the RuntimeDB and is
        /// also being used in making sure values fit the RuntimeDB.
        /// </summary>
        /// <param name="Name">this gets referenced by the <see cref="Pair"/> s</param>
        /// <param name="TargetInRuntime">tablename.columnname</param>
        /// <param name="Description"></param>
        /// <param name="SqlType">either number, varchar2 or date used for converting values</param>
        /// <param name="Size">
        /// important so that it doesn't trow errors when trying to but in a bigger value
        /// </param>
        /// <param name="Scale">amount of digits after the comma, default is 0</param>
        /// <param name="isNVL">
        /// if this is true, the field does not insert null, but keeps the old value
        /// </param>
        /// <param name="isNullable">when this is true, null can be inserted into the field</param>
        public Field(string Name, string TargetInRuntime, string Description, string SqlType, int Size, int Scale, bool isNVL, bool isNullable)
        {
            this.Name = Name;
            this.TargetInRuntime = TargetInRuntime;
            this.Description = Description;
            this.SqlType = SqlType;
            this.Size = Size;
            this.Scale = Scale;
            this.isNVL = isNVL;
            this.isNullable = isNullable;
        }

        /// <summary>
        /// Changes a value so it gets accepted by the database
        /// </summary>
        /// <param name="price"></param>
        /// <returns>cleaned <paramref name="price"/></returns>
        public static string CleanPrice(string price)
        {
            Regex priceReg = new Regex(@"[^\d+,\d\d+]");
            //remove spaces
            while(price.Contains(" "))
                price = price.Replace(" ", "");
            //remove question marks
            while(price.Contains("?"))
                price = price.Replace("?", "");
            //clean period and comma so that the final result is 10000,00
            if(price.Contains(".") && price.Contains(","))
            {
                if(price.IndexOf(".") < price.IndexOf(","))
                {
                    price = price.Replace(".", "");
                }
                else
                {
                    price = price.Replace(",", "");
                    price = price.Replace(".", ",");
                }
            }
            else
            {
                if(price.Contains("."))
                    price = price.Replace('.', ',');
            }
            //apply reg-ex
            price = priceReg.Replace(price, "");
            //test by parsing
            if(!double.TryParse(price, out double dPrice))
            {
                return "";
            }
            price = "" + dPrice;
            price = price.Replace('.', ','); //convert period and comma back
            return price;
        }

        /// <summary>
        /// Get a Field instance by name
        /// </summary>
        /// <param name="fields">array of fields to search</param>
        /// <param name="name">name to search</param>
        /// <returns>Field with the searched name</returns>
        public static Field GetFieldByName(Field[] fields, string name)
        {
            return Array.Find(fields, i => name.Equals(i.GetName()));
        }

        /// <summary>
        /// Get a field instance by target column in the real time db
        /// </summary>
        /// <param name="fields">array of fields to search</param>
        /// <param name="target">target column to search</param>
        /// <returns>Field with the searched target</returns>
        public static Field GetFieldByTarget(Field[] fields, string target)
        {
            return Array.Find(fields, i => target.Equals(i.GetTargetInRuntime()));
        }

        /// <summary>
        /// Get all fields with a certain name
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="name"></param>
        /// <returns>Array of fields with matching name</returns>
        public static Field[] GetFieldsByName(Field[] fields, string name)
        {
            return fields
                .Where(f => f.GetName() == name)
                .ToArray();
        }

        /// <summary>
        /// returns true if the <paramref name="value"/> has the correct date format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateFormat(string value)
        {
            if(value.StartsWith("TO_DATE("))
            {
                value = value.Substring(9, 10); //TO_DATE('2022-01-31', 'yyyy-mm-dd')
            }
            return DateTime.TryParseExact(value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Returns all field names
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames()
        {
            using(SQL sql = new SQL())
                return sql.ExecuteQuery($"select NAME from {Constants.TableImportFields} group by NAME");
        }

        /// <summary>
        /// Returns all fields in the database <see cref="Constants.TableImportFields"/>
        /// </summary>
        /// <returns>all fields in the database</returns>
        public static Field[] LoadFields()
        {
            using(SQL sql = new SQL())
            {
                //this could be solved with only one query instead
                string[] names = sql.ExecuteQuery($"select NAME from {Constants.TableImportFields} order by NAME");
                string[] targets = sql.ExecuteQuery($"select TARGET_FIELD from {Constants.TableImportFields} order by NAME");
                string[] descriptions = sql.ExecuteQuery($"select DESCRIPTION from {Constants.TableImportFields} order by NAME");
                string[] sqlTypes = sql.ExecuteQuery($"select TYPE from {Constants.TableImportFields} order by NAME");
                string[] sqlSizesStr = sql.ExecuteQuery($"select COLUMN_SIZE from {Constants.TableImportFields} order by NAME");
                int[] sqlSizes = sqlSizesStr.Select(i => int.Parse(i)).ToArray();
                string[] sqlIsNvlStr = sql.ExecuteQuery($"select IS_NVL from {Constants.TableImportFields} order by NAME");
                bool[] sqlIsNvl = sqlIsNvlStr.Select(i => i == "1").ToArray();
                string[] sqlScaleStr = sql.ExecuteQuery($"select COLUMN_SCALE from {Constants.TableImportFields} order by NAME");
                int[] sqlScales = sqlScaleStr.Select(i => int.Parse(i)).ToArray();
                string[] sqlIsNullableStr = sql.ExecuteQuery($"select IS_NULLABLE from {Constants.TableImportFields} order by NAME");
                bool[] sqlIsNullable = sqlIsNullableStr.Select(i => i == "1").ToArray();
                List<Field> fields = new List<Field>();
                for(int i = 0;i < names.Count();i++)
                {
                    fields.Add(new Field(names[i], targets[i], descriptions[i], sqlTypes[i], sqlSizes[i], sqlScales[i], sqlIsNvl[i], sqlIsNullable[i]));
                }
                fields.Sort(delegate (Field a, Field b)
                {
                    return a.GetName().CompareTo(b.GetName());
                });
                return fields.ToArray();
            }
        }

        /// <summary>
        /// Matches a string value to the maximum char size of the field. Cuts of the value at the
        /// end if its to long.
        /// </summary>
        /// <param name="value">that will be shortened in order to match size</param>
        /// <param name="size">maximum amount of chars</param>
        /// <returns>value cut of at the end</returns>
        public static string MatchValueSize(string value, int size)
        {
            if(value.Length <= size)
                return value;
            return value.Substring(0, size);
        }

        /// <summary>
        /// Adds the value type conversion for SQL to the value. Depending on the <see
        /// cref="SqlType"/> of this field.
        /// </summary>
        /// <example>value = TO_NUMBER('value')</example>
        /// <param name="value"></param>
        /// <returns><paramref name="value"/> with type conversion</returns>
        public string AddValueTypeConversion(string value)
        {
            value = value.Replace('\'', ' '); //this would lead to wrong syntax in the SQL command -> SQL injection
            switch(SqlType)
            {
                case "VARCHAR2":
                    return $"'{value}'";

                case "NUMBER":
                    return $"TO_NUMBER('{CleanPrice(value)}')";

                case "DATE":
                    return $"TO_DATE('{value}', 'yyyy-mm-dd')";

                default:
                    return $"'{value}'";
            }
        }

        /// <summary>
        /// Additional information to the field
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return Description;
        }

        /// <summary>
        /// the name of the field this gets referenced by the <see cref="Pair"/> s (key value)
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// amount of digits after the comma, default is 0. Important for prices.
        /// </summary>
        /// <returns></returns>
        public int GetScale()
        {
            return Scale;
        }

        /// <summary>
        /// size of the field in the real time db, important so that it doesn't trow errors when
        /// trying to but in a bigger value
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return Size;
        }

        /// <summary>
        /// either number, varchar2 or date used for converting values
        /// </summary>
        /// <returns></returns>
        public string GetSqlType()
        {
            return SqlType;
        }

        /// <summary>
        /// in the real time db tablename.columnname
        /// </summary>
        /// <returns></returns>
        public string GetTargetInRuntime()
        {
            return TargetInRuntime;
        }

        /// <summary>
        /// Adds the field into the database <see cref="Constants.TableImportFields"/> and adds a
        /// column <see cref="Constants.TableImportArticles"/> or <see cref="Constants.TableImportEinkauf"/>
        /// </summary>
        public void Insert()
        {
            string nvl = "0";
            if(isNVL)
                nvl = "1";
            string nullable = "0";
            if(isNullable)
                nullable = "1";
            string cmd = $"insert into {Constants.TableImportFields} values ('{Name}', '{TargetInRuntime}', '{Description}', '{SqlType}', '{Size}', '{Scale}', '{nvl}', '{nullable}')";
            string cmd1 = "ERROR";
            if(TargetInRuntime.StartsWith(Constants.TableEinkauf))
            {
                cmd1 = $"alter table {Constants.TableImportEinkauf} add {Name} {SqlType}";
            }
            if(TargetInRuntime.StartsWith(Constants.TableArtikel))
            {
                cmd1 = $"alter table {Constants.TableImportArticles} add {Name} {SqlType}";
            }
            using(SQL sql = new SQL())
            {
                sql.ExecuteCommand(cmd);
                if(!"ERROR".Equals(cmd1))
                {
                    if(SqlType.Equals("VARCHAR2"))
                        cmd1 += $"({Size})";
                    if(SqlType.Equals("NUMBER"))
                    {
                        if(Scale > 0)
                            cmd1 += $"({Size}:{Scale})";
                        else
                            cmd1 += $"({Size})";
                    }
                    if(!isNullable)
                        cmd1 += " NOT NULL";
                    sql.ExecuteCommand(cmd1);
                }
                else
                {
                    log.Error("Alter table command creation failed");
                }
            }
        }

        /// <summary>
        /// is true, when null can be inserted into the field.
        /// </summary>
        /// <returns></returns>
        public bool IsNullable()
        {
            return isNullable;
        }

        /// <summary>
        /// if this is true, the field does not insert null, but keeps the old value
        /// </summary>
        /// <returns></returns>
        public bool IsNVL()
        {
            return isNVL;
        }

        /// <summary>
        /// Matches a string value to the maximum char size of the field. Cuts of the value at the
        /// end if its to long.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>value cut of at the end</returns>
        public string MatchValueSize(string value)
        {
            return MatchValueSize(value, Size);
        }

        /// <summary>
        /// Removes the Field from the database <see cref="Constants.TableImportFields"/> and also
        /// removes the column from either <see cref="Constants.TableImportArticles"/> or <see cref="Constants.TableImportEinkauf"/>.
        /// </summary>
        public void Remove()
        {
            string[] cmd = new string[2];
            cmd[0] = $"delete from {Constants.TableImportFields} where NAME='{Name}' and TARGET_FIELD='{TargetInRuntime}'";
            if(TargetInRuntime.StartsWith(Constants.TableEinkauf))
            {
                cmd[1] = $"alter table {Constants.TableImportEinkauf} drop column {Name}";
            }
            if(TargetInRuntime.StartsWith(Constants.TableArtikel))
            {
                cmd[1] = $"alter table {Constants.TableImportArticles} drop column {Name}";
            }
            using(SQL sql = new SQL())
                sql.ExecuteCommands(cmd);
        }

        /// <summary>
        /// Returns a string that contains all the important information to this field.
        /// </summary>
        /// <returns>string representation of this field</returns>
        public override string ToString()
        {
            return $"Field: [Name: {Name}; SqlType: {SqlType}; TargetInRuntime: {TargetInRuntime}; Description: {Description}; Size: {Size}; Scale: {Scale}; isNVL: {isNVL}; isNullable: {isNullable}]";
        }
    }
}