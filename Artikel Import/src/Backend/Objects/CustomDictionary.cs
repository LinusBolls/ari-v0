using log4net;
using System;
using System.Collections.Generic;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// A Dictionary with a <see cref="Mapping"/> name and a <see cref="name"/> describing itself.
    /// The dictionary can be edited in the <see cref="Frontend.MainForm.tabPageDictionary"/>.
    /// </summary>
    public class CustomDictionary
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string mapping;
        private readonly string name;
        private Dictionary<string, string> keyValuePairs;

        /// <summary>
        /// Get a dictionary from the database or an empty one if none exists
        /// </summary>
        /// <param name="mapping">name of the <see cref="Mapping"/> this dictionary belongs to</param>
        /// <param name="name">name of the new dictionary</param>
        public CustomDictionary(string mapping, string name)
        {
            this.mapping = mapping;
            this.name = name;
            GetKeyValuePairs();
        }

        /// <summary>
        /// Create a filled CustomDictionary
        /// </summary>
        /// <param name="mapping">name of the <see cref="Mapping"/> this dictionary belongs to</param>
        /// <param name="name">name of the new dictionary</param>
        /// <param name="keyValuePairs">dictionary this CustomDictionary contains</param>
        public CustomDictionary(string mapping, string name, Dictionary<string, string> keyValuePairs)
        {
            this.mapping = mapping;
            this.name = name;
            this.keyValuePairs = keyValuePairs;
        }

        /// <summary>
        /// List of all available dictionaries for the <paramref name="mapping"/>
        /// </summary>
        /// <param name="mapping"><see cref="Mapping"/> that gets searched</param>
        /// <returns>list of dictionary <see cref="name"/> s</returns>
        public static string[] GetList(string mapping)
        {
            string cmd = $"select name from {Constants.TableImportDictionary} where mapping='{mapping}' group by name";
            using(SQL sql = new SQL())
                return sql.ExecuteQuery(cmd);
        }

        /// <summary>
        /// Adds a <see cref="Pair"/> to the selected dictionary
        /// </summary>
        /// <param name="key">value that gets replaced</param>
        /// <param name="value">value that will get returned</param>
        /// <returns></returns>
        public SqlReport AddPair(string key, string value)
        {
            keyValuePairs.Add(key, value);
            string cmd = $"insert into {Constants.TableImportDictionary} values('{mapping}', '{name}', '{key}', '{value}')";
            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        /// <summary>
        /// Get a list of keys of the dictionary
        /// </summary>
        /// <returns>string array of keys</returns>
        public string[] GetKeys()
        {
            string[] keys = new string[keyValuePairs.Keys.Count];
            keyValuePairs.Keys.CopyTo(keys, 0);
            Array.Sort(keys);
            return keys;
        }

        /// <summary>
        /// Name of the <see cref="CustomDictionary"/>
        /// </summary>
        /// <returns><see cref="System.String"/> representation of the dictionary</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Returns the value that the key links to
        /// </summary>
        /// <param name="key">key to the value of the <see cref="CustomDictionary"/></param>
        /// <returns>string value</returns>
        public string GetValue(string key)
        {
            if(!keyValuePairs.ContainsKey(key))
            {
                //log.Warn($"Mapping {mapping} Dictionary {name} - Key '{key}' not found");
                return null;
            }
            try
            {
                return keyValuePairs[key];
            }
            catch(ArgumentNullException)
            {
                //log.Warn($"ArgumentNullException: Mapping {mapping} Dictionary {name} - key '{key}' not found.", ex);
                return null;
            }
            catch(Exception ex)
            {
                log.Error(key, ex);
                return null;
            }
        }

        /// <summary>
        /// Deletes the complete <see cref="CustomDictionary"/> from the database
        /// </summary>
        /// <returns><see cref="SqlReport"/> of success</returns>
        public SqlReport Remove()
        {
            string cmd = $"delete from {Constants.TableImportDictionary} where mapping='{mapping}' and name='{name}'";
            keyValuePairs.Clear();
            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        /// <summary>
        /// Deletes a keyValuePair from the dictionary in the database
        /// </summary>
        /// <param name="key">key of the pair that gets removed</param>
        /// <returns>SqlReport of success</returns>
        public SqlReport RemovePair(string key)
        {
            keyValuePairs.Remove(key);
            string cmd = $"delete from {Constants.TableImportDictionary} where mapping='{mapping}' and name='{name}' and key='{key}'";
            using(SQL sql = new SQL())
                return sql.ExecuteCommand(cmd);
        }

        private void GetKeyValuePairs()
        {
            string cmd = $"select key, value from {Constants.TableImportDictionary} where mapping='{mapping}' and name='{name}' order by key";
            using(SQL sql = new SQL())
            {
                string[] keys = sql.ExecuteQuery(cmd);
                string[] values = sql.ExecuteQuery(cmd, 1);
                keyValuePairs = new Dictionary<string, string>();
                for(int i = 0;i < keys.Length;i++)
                {
                    keyValuePairs.Add(keys[i], values[i]);
                }
                if(keys.Length == 0)
                {
                    log.Info($"The Dictionary [{mapping}-{name}] has no entry in the database");
                }
            }
        }
    }
}