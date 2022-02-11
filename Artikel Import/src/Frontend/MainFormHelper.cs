using Artikel_Import.src.Backend;
using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// A collection of static methods that get used by the <see cref="MainForm"/>.
    /// </summary>
    public class MainFormHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the <see cref="TabPage"/> that should get displayed after pressing the <see cref="MainForm.buttonBack"/>.
        /// </summary>
        /// <param name="selectedTab">Name of the currently selected <see cref="TabPage"/></param>
        /// <param name="tabPages">Array of all <see cref="TabPage"/> s</param>
        /// <returns>Next <see cref="TabPage"/> to display</returns>
        public static TabPage GetBackTab(string selectedTab, TabPage[] tabPages)
        {
            if(selectedTab.Equals(Constants.TabNameMappings))
            {
                return GetTabPage(selectedTab, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameValues))
            {
                return GetTabPage(Constants.TabNameMappings, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameFields))
            {
                return GetTabPage(Constants.TabNameMappings, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameEditMapping))
            {
                return GetTabPage(Constants.TabNameMappings, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameDiscounts))
            {
                return GetTabPage(Constants.TabNameEditMapping, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameUpload))
            {
                return GetTabPage(Constants.TabNameMappings, tabPages);
            }
            return GetTabPage(selectedTab, tabPages);
        }

        /// <summary>
        /// Returns the next <see cref="TabPage"/> after pressing the <see cref="MainForm.buttonNext"/>.
        /// </summary>
        /// <param name="selectedTab">Name of the currently selected <see cref="TabPage"/></param>
        /// <param name="tabPages">Array of all <see cref="TabPage"/> s</param>
        /// <returns>Next <see cref="TabPage"/> to display</returns>
        public static TabPage GetNextTab(string selectedTab, TabPage[] tabPages)
        {
            if(selectedTab.Equals(Constants.TabNameMappings))
            {
                return GetTabPage(Constants.TabNameValues, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameValues))
            {
                return GetTabPage(Constants.TabNameUpload, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameFields))
            {
                return GetTabPage(Constants.TabNameFields, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameEditMapping))
            {
                return GetTabPage(Constants.TabNameValues, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameDiscounts))
            {
                return GetTabPage(Constants.TabNameEditMapping, tabPages);
            }
            if(selectedTab.Equals(Constants.TabNameUpload))
            {
                return tabPages[0]; //no next button
            }
            return tabPages[0];
        }

        /// <summary>
        /// Selects a <see cref="TabPage"/> by name from a tabPage array
        /// </summary>
        /// <param name="tabName">Name of the <see cref="TabPage"/> to return</param>
        /// <param name="tabPages">Array of <see cref="TabPage"/> s</param>
        /// <returns>Selected <see cref="TabPage"/></returns>
        public static TabPage GetTabPage(string tabName, TabPage[] tabPages)
        {
            TabPage[] results = tabPages.Where(tabPage => tabPage.Name == tabName).ToArray();
            if(results.Length > 1)
                log.Error($"MainFormHelper.GetTabPage there where multiple tabPages found for {tabName}");
            return results[0]; //there should only be one found
        }

        /// <summary>
        /// Loads the <see cref="CustomDictionary"/> of the selected mapping into the <see cref="MainForm.listViewDictionary"/>.
        /// </summary>
        /// <param name="selectedMapping"><see cref="Mapping"/> containing the dictionary</param>
        /// <param name="selectedDictionaryName">Name of the <see cref="CustomDictionary"/></param>
        /// <param name="listViewDictionary"><see cref="ListView"/> that will be displayed</param>
        /// <param name="columnName">Name of the column the dictionary key is found in</param>
        /// <param name="pathToCsv">Path to the CSV containing the dictionary keys</param>
        /// <returns>Filled listView</returns>
        public static ListView LoadDictionaryList(Mapping selectedMapping, string selectedDictionaryName, ListView listViewDictionary, string columnName, string pathToCsv)
        {
            if(!selectedMapping.dictionaries.ContainsKey(selectedDictionaryName))
            {
                selectedMapping.dictionaries.Add(selectedDictionaryName, new CustomDictionary(selectedMapping.GetName(), selectedDictionaryName));
            }
            CustomDictionary customDictionary = selectedMapping.dictionaries[selectedDictionaryName];

            listViewDictionary.Items.Clear();
            try
            {
                //get dictionaryKeys from CSV
                string[][] csv = CSV.GetCsv(pathToCsv);
                int index = Array.IndexOf(csv[0], columnName);
                if(index < 0)
                    throw new Exception($"Could not find columnName '{columnName}'");
                string[] column = csv.Skip(1).Select(r => r[index]).ToArray();
                string[] csvKeys = column.Distinct().ToArray();

                string[] dictKeys = customDictionary.GetKeys();
                foreach(string key in csvKeys)
                {
                    string value = customDictionary.GetValue(key);
                    if(value == null || value == string.Empty)
                    {
                        value = "empty";
                        customDictionary.AddPair(key, value);
                    }
                    ListViewItem item = new ListViewItem(key);
                    if(value == "empty")
                        item.BackColor = Color.Red;
                    item.SubItems.Add(value);
                    listViewDictionary.Items.Add(item);
                }
            }
            catch(Exception ex)
            {
                log.Warn($"Exception when trying to load Dictionary Keys from CSV. Getting keys from mapping instead. PathToCsv:{pathToCsv} columnName:{columnName}", ex);
                new MessagePopUp(Properties.Resources.FailedToLoadDictionaryKeyFromCSV, Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
                foreach(string key in customDictionary.GetKeys())
                {
                    ListViewItem item = new ListViewItem(key);
                    item.SubItems.Add(customDictionary.GetValue(key));
                    listViewDictionary.Items.Add(item);
                }
            }
            if(listViewDictionary.Items.Count > 0)
                listViewDictionary.Items[0].Selected = true;
            return listViewDictionary;
        }

        /// <summary>
        /// Loads the <see cref="Discount"/> of the selected mapping into the <see cref="MainForm.listViewDictionary"/>.
        /// </summary>
        /// <param name="selectedMapping"><see cref="Mapping"/> to display</param>
        /// <param name="listViewDiscounts">
        /// <see cref="ListView"/> that the <see cref="Discount"/> s will be displayed on
        /// </param>
        /// <param name="columnName">Nave of the column the discount key is found in</param>
        /// <param name="pathToCsv">Path to the CSV containing the discount keys</param>
        /// <returns>Filled listView</returns>
        public static ListView LoadDiscountList(Mapping selectedMapping, ListView listViewDiscounts, string columnName, string pathToCsv)
        {
            Discount[] discounts = selectedMapping.LoadDiscounts();
            listViewDiscounts.Items.Clear();
            string[] discountKeys = new string[] { };
            //get discountKeys from CSV
            try
            {
                string[][] csv = CSV.GetCsv(pathToCsv);
                if(csv.Length < 1)
                {
                    throw new Exception($"CSV.GetCsv({pathToCsv}) returned nothing");
                }
                int index = Array.IndexOf(CSV.GetHeaderRow(csv), columnName);
                if(index < 0)
                {
                    throw new Exception($"ColumnName '{columnName}' not found in {selectedMapping.GetName()} CSV header.");
                }
                List<string> columns = new List<string>();
                string[] _columns = csv.Skip(1).Select(r => r[index]).Distinct().ToArray();
                foreach(string s in _columns)
                {
                    if(s.Length > 8)
                        columns.Add(s.Substring(0, 8));
                    else
                        columns.Add(s);
                }
                discountKeys = columns.Distinct().ToArray();
            }
            catch(Exception ex)
            {
                log.Error("Error when trying to read discountKeys.", ex);
                foreach(Discount discount in discounts)
                {
                    ListViewItem item = new ListViewItem(discount.GetName());
                    item.SubItems.Add(discount.GetDiscountAmount().ToString());
                    listViewDiscounts.Items.Add(item);
                }
                return listViewDiscounts;
            }

            foreach(string key in discountKeys)
            {
                Discount discount = null;
                foreach(Discount d in discounts)
                {
                    if(key.Equals(d.GetName()))
                        discount = d;
                }
                if(discount == null)
                {
                    discount = new Discount(selectedMapping.GetName(), key, 0);
                    try
                    {
                        discount.Insert();
                    }
                    catch(Exception ex)
                    {
                        log.Error("Failed to insert discount.", ex);
                    }
                }

                ListViewItem item = new ListViewItem(discount.GetName());
                if(discount.GetDiscountAmount() == 0)
                    item.BackColor = Color.Red;
                item.SubItems.Add(discount.GetDiscountAmount().ToString());
                listViewDiscounts.Items.Add(item);
            }
            return listViewDiscounts;
        }

        /// <summary>
        /// Load the <see cref="Field"/> s into the <see cref="MainForm.listViewFields"/>
        /// </summary>
        /// <param name="listViewFields">
        /// <see cref="ListView"/> where the <see cref="Field"/> s will be displayed
        /// </param>
        /// <returns>Filled listView</returns>
        public static ListView LoadFieldsList(ListView listViewFields)
        {
            listViewFields.Items.Clear();
            foreach(Field field in Field.LoadFields())
            {
                ListViewItem item = new ListViewItem(field.GetName());
                item.SubItems.Add(field.GetTargetInRuntime());
                item.SubItems.Add(field.GetDescription());
                item.SubItems.Add(field.GetSqlType());
                item.SubItems.Add(field.GetSize().ToString());
                item.SubItems.Add(field.IsNVL().ToString());
                listViewFields.Items.Add(item);
            }
            return listViewFields;
        }

        /// <summary>
        /// Loads the <see cref="Mapping"/> s into the <see cref="MainForm.listViewMappings"/>
        /// </summary>
        /// <param name="listViewMappings"><see cref="ListView"/> that will be filled</param>
        /// <returns>Filled listView</returns>
        public static ListView LoadMappingsList(ListView listViewMappings)
        {
            listViewMappings.Items.Clear();
            foreach(Mapping mapping in Mapping.GetMappings())
            {
                ListViewItem item = new ListViewItem(mapping.GetName())
                {
                    Name = mapping.GetName()
                };
                listViewMappings.Items.Add(item);
            }
            return listViewMappings;
        }

        /// <summary>
        /// Loads the <see cref="Pair"/> s into the <see cref="MainForm.listViewPairs"/>
        /// </summary>
        /// <param name="selectedMapping">
        /// <see cref="Mapping"/> that contains the <see cref="Pair"/> s
        /// </param>
        /// <param name="listViewPairs"><see cref="ListView"/> that will be filled</param>
        /// <returns>Filled listView</returns>
        public static ListView LoadPairsList(Mapping selectedMapping, ListView listViewPairs)
        {
            listViewPairs.Items.Clear();
            foreach(Pair pair in selectedMapping.GetPairs())
            {
                ListViewItem item = new ListViewItem(pair.GetTargetField());
                item.SubItems.Add(pair.GetPairType());
                item.SubItems.Add(pair.GetSource());
                if(pair.IsOverwrite())
                {
                    item.SubItems.Add("Y");
                }
                else
                {
                    item.SubItems.Add("N");
                }
                item.SubItems.Add(pair.GetAdditionalSource());
                listViewPairs.Items.Add(item);
            }
            listViewPairs.Items[0].Focused = true;
            return listViewPairs;
        }

        /// <summary>
        /// Loads the <see cref="Backend.Objects.PairTypes.PairChangingFixedValue"/> s into the <see cref="MainForm.listViewValues"/>
        /// </summary>
        /// <param name="selectedMapping"><see cref="Mapping"/> that contains the <see cref="Backend.Objects.PairTypes.PairChangingFixedValue"/></param>
        /// <param name="listViewValues"><see cref="ListView"/> that will be filled</param>
        /// <returns>Filled listView</returns>
        public static ListView LoadValuesList(Mapping selectedMapping, ListView listViewValues)
        {
            listViewValues.Items.Clear();
            bool hasChangeValues = false;
            foreach(Pair pair in selectedMapping.GetPairs())
            {
                if(!pair.GetPairType().Equals(Backend.Objects.PairTypes.PairChangingFixedValue.type))
                    continue; //only show the pairs that need a value entered
                ListViewItem item = new ListViewItem(pair.GetTargetField());
                item.SubItems.Add(pair.GetSource());
                listViewValues.Items.Add(item);
                hasChangeValues = true;
            }
            if(!hasChangeValues)
            {
                return null;
            }
            listViewValues.Items[0].Focused = true;
            return listViewValues;
        }
    }
}