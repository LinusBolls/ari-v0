using Artikel_Import.src.Backend;
using Artikel_Import.src.Backend.Objects;
using Artikel_Import.src.Backend.Objects.PairTypes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// Main Form of the application. Started by <see cref="Program"/>.
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TabPage[] tabPages;
        private string path = string.Empty;
        private string selectedDictionary;
        private Discount selectedDiscount;
        private Field selectedField;
        private Mapping selectedMapping;
        private Pair selectedPair;

        #region MainForm

        /// <summary>
        /// Main Form of the application. Started by <see cref="Program"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            listViewMappings = MainFormHelper.LoadMappingsList(listViewMappings);
            tabPages = tabControlMainForm.TabPages.Cast<TabPage>().ToArray(); //save all tabPages
            //remove all tab pages, that should not be visible to the user at the start of the application
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameFields, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameDiscounts, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameDictionary, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameEditMapping, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameUpload, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameValues, tabPages));
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.ButtonBack_Click");
            string selectedTab = tabControlMainForm.SelectedTab.Name;
            SelectTab(MainFormHelper.GetBackTab(selectedTab, tabPages));
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.ButtonNext_Click");
            SelectTab(MainFormHelper.GetNextTab(tabControlMainForm.SelectedTab.Name, tabPages));
        }

        private void GetCsvPath()
        {
            // TODO here add support for excel files
            bool isPathSelected = false;
            //get path
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"{selectedMapping.GetName()} oeffnen...";
                openFileDialog.InitialDirectory = Properties.Settings.Default.InitialDirectory;
                openFileDialog.Filter = "csv Dateien (*.csv)|*.csv|Csv Dateien (*.csv*)|*.csv* | Excel Dateien|*.xls;*.xlsx;*.xlsm";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    log.Info($"File path choosen: '{path}'");
                    isPathSelected = true;
                }
            }
            //load csv
            if(!isPathSelected)
            {
                SelectTab(MainFormHelper.GetTabPage(Constants.TabNameMappings, tabPages));
                return;
            }

            bool convertFileToCSV = false;
            char delimiter = '\t';
            string tempCSV = Environment.CurrentDirectory + "\\temporary_csv.csv";
            try
            {
                string used_path =  path;
                if( Constants.ExcelExtensions.Any( extension => path.EndsWith('.'+extension) ) )
                {
                    log.Info("found excel file to convert");

                    ConvertExcelToCSV.ConvertExcelToCsv(path, tempCSV , log);
                    convertFileToCSV = true;
                    used_path = tempCSV;
                    delimiter = ',';
                }


                using(var reader = new StreamReader(used_path))
                {
                    string line = reader.ReadLine();
                    comboBoxPairSource.Items.Clear();
                    comboBoxPairDiscountKey.Items.Clear();
                    log.Info($"the csv has a header count of {line.Split(delimiter).Length}");
                    if(line.Split(delimiter).Length > 1000)
                    {
                        log.Error("The csv length is bigger than 1000 columns. Cancel operation.");
                        new MessagePopUp(Properties.Resources.CSVTooManyColumns).ShowDialog();
                        return;
                    }
                    foreach(string s in line.Split(delimiter))
                    {
                        comboBoxPairSource.Items.Add(s);
                        comboBoxPairDiscountKey.Items.Add(s);
                    }
                }

                if( convertFileToCSV )
                {
                    if (File.Exists(tempCSV))
                    {
                        File.Delete(tempCSV);
                        log.Info("deleted temporary csv file");
                    }
                }
            }
            catch(IOException)
            {
                new MessagePopUp("Csv is being used by another process right now");
                return;
            }
        }

        private void SelectTab(TabPage tabPage)
        {
            if(!tabControlMainForm.TabPages.Contains(tabPage))
                tabControlMainForm.TabPages.Add(tabPage);
            tabControlMainForm.SelectTab(tabPage);
        }

        private void TabControlMainForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Info($"Changed to '{tabControlMainForm.SelectedTab}'");
            string selectedTab = tabControlMainForm.SelectedTab.Name;
            buttonBack.Enabled = true;
            buttonNext.Enabled = true;
            //actions that need to happen, when a tab gets opened
            if(selectedTab.Equals(Constants.TabNameMappings))
            {
                path = string.Empty;
                listViewMappings = MainFormHelper.LoadMappingsList(listViewMappings);
                buttonBack.Enabled = false;
                if(selectedMapping != null)
                {
                    int index = listViewMappings.Items.IndexOfKey(selectedMapping.GetName());
                    listViewMappings.Items[index].Focused = true;
                    listViewMappings.Items[index].Selected = true;
                }
                return;
            }
            if(selectedTab.Equals(Constants.TabNameFields))
            {
                listViewFields = MainFormHelper.LoadFieldsList(listViewFields);
                return;
            }
            if(selectedTab.Equals(Constants.TabNameDiscounts))
            {
                if(selectedMapping == null)
                {
                    tabControlMainForm.SelectedTab.Enabled = false;
                    new MessagePopUp(Properties.Resources.SelectMapping, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
                    tabControlMainForm.SelectTab(0);
                    return;
                }
                if(string.Empty.Equals(path))
                {
                    GetCsvPath();
                }
                tabControlMainForm.SelectedTab.Enabled = true;
                labelDiscountsMappingName.Text = selectedMapping.GetName();

                int selectedDiscountIndex = 0;
                if(listViewDiscounts.SelectedIndices.Count > 0)
                    selectedDiscountIndex = listViewDiscounts.SelectedIndices[0];

                if(PairDiscountValue.type.Equals(selectedPair.GetPairType())) //bug fix, no idea why this happens
                    selectedPair = new PairDiscountValue(selectedPair.GetMappingName(), selectedPair.IsOverwrite(), selectedPair.GetTargetField(), comboBoxPairDiscountKey.Text);

                listViewDiscounts = MainFormHelper.LoadDiscountList(selectedMapping, listViewDiscounts, selectedPair.GetAdditionalSource(), path);
                if(selectedDiscountIndex >= listViewDiscounts.Items.Count)
                    selectedDiscountIndex = listViewDiscounts.Items.Count - 1;
                listViewDiscounts.Items[selectedDiscountIndex].Selected = true;
                listViewDiscounts.Items[selectedDiscountIndex].Focused = true;
                return;
            }
            if(selectedTab.Equals(Constants.TabNameEditMapping))
            {
                if(selectedMapping == null)
                {
                    tabControlMainForm.SelectedTab.Enabled = false;
                    new MessagePopUp(Properties.Resources.SelectMapping, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
                    tabControlMainForm.SelectTab(0);
                    return;
                }
                if(string.Empty.Equals(path))
                {
                    GetCsvPath();
                }
                tabControlMainForm.SelectedTab.Enabled = true;
                labelPairsMappingName.Text = selectedMapping.GetName();
                listViewPairs = MainFormHelper.LoadPairsList(selectedMapping, listViewPairs);
                comboBoxPairTarget.Items.Clear();
                foreach(string field in Field.GetNames())
                {
                    comboBoxPairTarget.Items.Add(field);
                }
                selectedPair = null;
                comboBoxPairTarget.Visible = false;
                comboBoxPairType.Visible = false;
                labelPairsTargetfield.Visible = false;
                labelPairsType.Visible = false;
                textBoxPairSourceField.Visible = false;
                comboBoxPairSource.Visible = false;
                textBoxPairsFactor.Visible = false;
                labelPairsFactor.Visible = false;
                labelPairsSourcefield.Visible = false;
                comboBoxPairDiscountKey.Visible = false;
                buttonPairsOpenDiscounts.Visible = false;
                comboBoxPairDictionaries.Visible = false;
                return;
            }
            if(selectedTab.Equals(Constants.TabNameUpload))
            {
                if(selectedMapping == null)
                {
                    tabControlMainForm.SelectedTab.Enabled = false;
                    new MessagePopUp(Properties.Resources.SelectMapping, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
                    tabControlMainForm.SelectTab(0);
                    return;
                }
                buttonNext.Enabled = false;
                tabControlMainForm.SelectedTab.Enabled = true;
                textBoxPathUpload.Text = path;
                labelUploadSelectedMapping.Text = selectedMapping.GetName();
                return;
            }
            if(selectedTab.Equals(Constants.TabNameValues))
            {
                if(selectedMapping == null)
                {
                    tabControlMainForm.SelectedTab.Enabled = false;
                    new MessagePopUp(Properties.Resources.SelectMapping, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
                    tabControlMainForm.SelectTab(0);
                    return;
                }
                tabControlMainForm.SelectedTab.Enabled = true;
                labelValuesMappingName.Text = selectedMapping.GetName();
                if(MainFormHelper.LoadValuesList(selectedMapping, listViewValues) == null)
                {
                    //there are no pairs that need a value entered, continue to upload
                    log.Info("there are no pairs that need a value entered, continue to upload");
                    SelectTab(MainFormHelper.GetTabPage(Constants.TabNameUpload, tabPages));
                    //this is a workaround, because the event TabControlMainForm_SelectedIndexChanged does not get triggered for some me unknown reason
                    //selected index is upload
                    buttonNext.Enabled = false;
                    tabControlMainForm.SelectedTab.Enabled = true;
                    textBoxPathUpload.Text = path;
                    labelUploadSelectedMapping.Text = selectedMapping.GetName();
                    //end of loading upload workaround
                    tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameValues, tabPages));
                    return;
                }
                else
                {
                    listViewValues = MainFormHelper.LoadValuesList(selectedMapping, listViewValues);
                }
                return;
            }
            if(selectedTab.Equals(Constants.TabNameDictionary))
            {
                if(selectedMapping == null)
                {
                    tabControlMainForm.SelectedTab.Enabled = false;
                    new MessagePopUp(Properties.Resources.SelectMapping, Properties.Settings.Default.ShowTimeErrorSec).ShowDialog();
                    tabControlMainForm.SelectTab(0);
                    return;
                }
                if(string.Empty.Equals(path))
                {
                    GetCsvPath();
                }
                tabControlMainForm.SelectedTab.Enabled = true;
                labelDictionaryMapping.Text = selectedMapping.GetName() + " - " + selectedDictionary;
                listViewDictionary = MainFormHelper.LoadDictionaryList(selectedMapping, selectedDictionary, listViewDictionary, selectedPair.GetSource(), path);
                return;
            }
            log.Error($"Selected tab '{selectedTab}' not found. Switching to tab 0.");
            tabControlMainForm.SelectTab(0);
        }

        private void ToolStripMenuItemArtikelNrReplace(object sender, EventArgs e)
        {
            log.Info("ArtikelNrReplace");
            var f = new ConfirmationPopUp("Are you sure, you want to rename all articles?");
            f.ShowDialog();
            if(f.confirmed)
            {
                if(!Constants.RequirePassword)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    new ProgressPopUp(ProgressPopUp.RenameArticles).ShowDialog();
#pragma warning restore CS0162 // Unreachable code detected
                }
                else
                {
                    //ask for password
                    EnterValuePopUp enterName = new EnterValuePopUp(Properties.Resources.EnterPassword);
                    enterName.ShowDialog();
                    if(Constants.DatabasePassword.Equals(enterName.value))
                    {
                        new ProgressPopUp(ProgressPopUp.RenameArticles).ShowDialog();
                    }
                    else
                    {
                        new MessagePopUp(Properties.Resources.WrongPassword).ShowDialog();
                    }
                }
            }
        }

        private void ToolStripMenuItemClearDatabase_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.ToolStripMenuItemClearDatabase_Click");
            var f = new ConfirmationPopUp(Properties.Resources.ConfirmClearDatabase);
            f.ShowDialog();
            if(f.confirmed)
            {
                if(!Constants.RequirePassword)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    Cursor.Current = Cursors.WaitCursor;
                    ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
                    SqlReport report = import.ClearTempDatabase();
                    Cursor.Current = Cursors.Arrow;
                    new MessagePopUp(report.ToString()).ShowDialog();
                    return;
#pragma warning restore CS0162 // Unreachable code detected
                }
                //ask for password
                EnterValuePopUp enterName = new EnterValuePopUp(Properties.Resources.EnterPassword);
                enterName.ShowDialog();
                if(Constants.DatabasePassword.Equals(enterName.value))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
                    SqlReport report = import.ClearTempDatabase();
                    Cursor.Current = Cursors.Arrow;
                    new MessagePopUp(report.ToString()).ShowDialog();
                }
                else
                {
                    new MessagePopUp(Properties.Resources.WrongPassword).ShowDialog();
                }
            }
        }

        private void ToolStripMenuItemFields_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.ToolStripMenuItemFields_Click");
            SelectTab(MainFormHelper.GetTabPage(Constants.TabNameFields, tabPages));
        }

        private void ToolStripMenuItemSendToRuntime_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.ToolStripMenuItemSendToRuntime_Click");
            //check for any EDE articles -> those need to be imported because they overwrite.
            //string[] response = SQL.ExecuteQuery($"select datenlieferant from {Constants.TableImportArticles} where datenlieferant='99669'");
            //if (response.Length == 0)
            //new MessagePopUp(Properties.Resources.RememberEDE).ShowDialog(); //found none, show warning.
            //check for confirmation
            ConfirmationPopUp confirmation = new ConfirmationPopUp(Properties.Resources.ConfirmSendToRuntime);
            confirmation.ShowDialog();
            if(confirmation.confirmed)
            {
#pragma warning disable CS0162 // Unreachable code detected
                if(!Constants.RequirePassword)
                {
                    new ProgressPopUp(ProgressPopUp.Export).ShowDialog();
                    return;
                }
                //ask for password
                EnterValuePopUp enterName = new EnterValuePopUp(Properties.Resources.EnterPassword);
                enterName.ShowDialog();
                if(Constants.DatabasePassword.Equals(enterName.value))
                {
                    new ProgressPopUp(ProgressPopUp.Export).ShowDialog();
                }
                else
                {
                    new MessagePopUp(Properties.Resources.WrongPassword).ShowDialog();
                }
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        #endregion MainForm

        /// <summary>
        /// For <see cref="tabPageDictionary"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Dictionary

        private void ButtonDictionaryDelete_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDictionaryDelete_Click");
            selectedMapping.dictionaries[selectedDictionary].RemovePair(labelDictionaryKey.Text);
            listViewDictionary = MainFormHelper.LoadDictionaryList(selectedMapping, selectedDictionary, listViewDictionary, selectedPair.GetSource(), path);
            if(listViewDictionary.Items.Count > 0)
            {
                listViewDictionary.Items[0].Focused = true;
                listViewDictionary.Items[0].Selected = true;
            }
        }

        private void ButtonDictionaryNew_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDictionaryNew_Click");
            listViewDictionary.Items.Add("empty");
            listViewDictionary.SelectedItems.Clear();
            listViewDictionary.Items[listViewDictionary.Items.Count - 1].Focused = true;
            listViewDictionary.Items[listViewDictionary.Items.Count - 1].Selected = true;
        }

        private void ButtonDictionarySave_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDictionarySave_Click");
            string key = labelDictionaryKey.Text;
            string value = textBoxDictionaryValue.Text;
            selectedMapping.dictionaries[selectedDictionary].RemovePair(key);
            selectedMapping.dictionaries[selectedDictionary].AddPair(key, value);
            listViewDictionary = MainFormHelper.LoadDictionaryList(selectedMapping, selectedDictionary, listViewDictionary, selectedPair.GetSource(), path);
        }

        private void ListViewDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listViewDictionary.Items.Count > 0)
            {
                int selectedIndex = 0;
                if(listViewDictionary.FocusedItem != null)
                    selectedIndex = listViewDictionary.Items.IndexOf(listViewDictionary.FocusedItem);
                else if(listViewDictionary.SelectedItems.Count > 0)
                {
                    selectedIndex = listViewDictionary.SelectedIndices[0];
                }
                if(selectedIndex >= listViewDictionary.Items.Count && listViewDictionary.Items.Count > 0)
                    selectedIndex = listViewDictionary.Items.Count - 1;
                listViewDictionary.Items[selectedIndex].Focused = true;
                log.Info($"ListViewDictionary_SelectedIndexChanged to '{listViewDictionary.FocusedItem.Text}'");
                string key = listViewDictionary.FocusedItem.Text;
                labelDictionaryKey.Text = key;
                textBoxDictionaryValue.Text = selectedMapping.dictionaries[selectedDictionary].GetValue(key);
                if(string.IsNullOrEmpty(textBoxDictionaryValue.Text))
                    textBoxDictionaryValue.Text = "empty";
            }
            else
            {
                log.Info($"ListViewDictionary_SelectedIndexChanged to 'null'");
                labelDictionaryKey.Text = "";
                textBoxDictionaryValue.Text = "";
            }
        }

        #endregion Dictionary

        /// <summary>
        /// For <see cref="tabPageDiscounts"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Discounts

        private void ButtonDiscountsCreateNew_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDiscountsCreateNew_Click");
            listViewDiscounts.Items.Add("empty");
            listViewDiscounts.SelectedItems.Clear();
            selectedDiscount = new Discount(selectedMapping.GetName(), "", 100);
            listViewDiscounts.Items[listViewDiscounts.Items.Count - 1].Focused = true;
            listViewDiscounts.Items[listViewDiscounts.Items.Count - 1].Selected = true;
        }

        private void ButtonDiscountsDelete_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDiscountsDelete_Click");
            selectedMapping.Delete(selectedDiscount);

            int selectedDiscountIndex = 0;
            if(listViewDiscounts.SelectedIndices.Count > 0)
                selectedDiscountIndex = listViewDiscounts.SelectedIndices[0];
            listViewDiscounts = MainFormHelper.LoadDiscountList(selectedMapping, listViewDiscounts, selectedPair.GetAdditionalSource(), path);
            if(selectedDiscountIndex >= listViewDiscounts.Items.Count)
                selectedDiscountIndex = listViewDiscounts.Items.Count - 1;
            listViewDiscounts.Items[selectedDiscountIndex].Selected = true;
            listViewDiscounts.Items[selectedDiscountIndex].Focused = true;
        }

        private void ButtonDiscountsSave_Click(object sender, EventArgs e)
        {
            log.Info("ButtonDiscountsSave_Click");
            Discount discount = new Discount(selectedMapping.GetName(), textBoxDiscountsKey.Text, textBoxDiscountsDiscount.Text);
            selectedMapping.Delete(discount);
            selectedMapping.Add(discount);
            int selectedDiscountIndex = 0;
            if(listViewDiscounts.SelectedIndices.Count > 0)
                selectedDiscountIndex = listViewDiscounts.SelectedIndices[0];
            listViewDiscounts = MainFormHelper.LoadDiscountList(selectedMapping, listViewDiscounts, selectedPair.GetAdditionalSource(), path);
            if(selectedDiscountIndex >= listViewDiscounts.Items.Count)
                selectedDiscountIndex = listViewDiscounts.Items.Count - 1;
            listViewDiscounts.Items[selectedDiscountIndex].Selected = true;
            listViewDiscounts.Items[selectedDiscountIndex].Focused = true;
        }

        private void ListViewDiscounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listViewDiscounts.Items.Count > 0)
            {
                int selectedIndex = 0;
                if(listViewDiscounts.FocusedItem != null)
                    selectedIndex = listViewDiscounts.Items.IndexOf(listViewDiscounts.FocusedItem);
                else if(listViewDiscounts.SelectedItems.Count > 0)
                {
                    selectedIndex = listViewDiscounts.SelectedIndices[0];
                }
                if(selectedIndex >= listViewDiscounts.Items.Count && listViewDiscounts.Items.Count > 0)
                    selectedIndex = listViewDiscounts.Items.Count - 1;
                listViewDiscounts.Items[selectedIndex].Focused = true;
                log.Info($"ListViewDiscounts_SelectedIndexChanged to '{listViewDiscounts.FocusedItem.Text}'");
                string discountKey = listViewDiscounts.FocusedItem.Text;
                foreach(Discount d in selectedMapping.GetDiscounts())
                {
                    if(d.GetName() == discountKey)
                    {
                        selectedDiscount = d;
                        break;
                    }
                }
                if(selectedDiscount == null)
                    selectedDiscount = selectedMapping.GetDiscounts()[0];
            }
            else
            {
                log.Info($"ListViewDiscounts_SelectedIndexChanged to 'null'");
                selectedDiscount = new Discount(selectedMapping.GetName(), "", "");
            }
            textBoxDiscountsKey.Text = selectedDiscount.GetName();
            textBoxDiscountsDiscount.Text = selectedDiscount.GetDiscountAmount().ToString();
        }

        #endregion Discounts

        /// <summary>
        /// For <see cref="tabPageFields"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Field

        private void ButtonFieldCreateNew_Click(object sender, EventArgs e)
        {
            log.Info("ButtonFieldCreateNew_Click");
            //create empty field and select that index
            ListViewItem item = new ListViewItem("empty");
            item.SubItems.Add("");
            listViewFields.Items.Add(item);
            listViewFields.SelectedItems.Clear();
            selectedField = new Field("", "", "", "", 0, 0, false, true);
            listViewFields.Items[listViewFields.Items.Count - 1].Focused = true;
            listViewFields.Items[listViewFields.Items.Count - 1].Selected = true;
        }

        private void ButtonFieldDelete_Click(object sender, EventArgs e)
        {
            log.Info("ButtonFieldDelete_Click");
            selectedField.Remove();
            textBoxFieldName.Text = "";
            textBoxFieldTarget.Text = "";
            textBoxFieldDescription.Text = "";
            listViewFields = MainFormHelper.LoadFieldsList(listViewFields);
            listViewFields.Items[0].Focused = true;
            listViewFields.Items[0].Selected = true;
        }

        private void ButtonFieldSave_Click(object sender, EventArgs e)
        {
            log.Info("ButtonFieldSave_Click");
            try
            {
                selectedField.Remove();
            }
            catch(Exception)
            {
                //ignore
            }
            Field field = new Field(textBoxFieldName.Text, textBoxFieldTarget.Text, textBoxFieldDescription.Text, comboBoxFieldType.Text, int.Parse(textBoxFieldSize.Text), int.Parse(textBoxFieldScale.Text), checkboxFieldsDontInsertNull.Checked, checkBoxFieldIsNullable.Checked);
            field.Insert();
            listViewFields = MainFormHelper.LoadFieldsList(listViewFields);
        }

        private void ListViewFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Info($"ListViewFields_SelectedIndexChanged to '{listViewFields.FocusedItem.Text}'");
            string sFieldName = listViewFields.FocusedItem.Text;
            string sFieldTarget = listViewFields.FocusedItem.SubItems[1].Text;
            foreach(Field f in Field.LoadFields())
            {
                if(sFieldName.Equals(f.GetName()) && sFieldTarget.Equals(f.GetTargetInRuntime()))
                {
                    selectedField = f;
                }
            }
            textBoxFieldName.Text = selectedField.GetName();
            textBoxFieldTarget.Text = selectedField.GetTargetInRuntime();
            textBoxFieldDescription.Text = selectedField.GetDescription();
            comboBoxFieldType.Text = selectedField.GetSqlType();
            textBoxFieldSize.Text = selectedField.GetSize().ToString();
            textBoxFieldScale.Text = selectedField.GetScale().ToString();
            checkBoxFieldIsNullable.Checked = selectedField.IsNullable();
            checkboxFieldsDontInsertNull.Checked = selectedField.IsNVL();
        }

        #endregion Field

        /// <summary>
        /// For <see cref="tabPageMappings"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Mappings

        private void ButtonMappingsCreateNew_Click(object sender, EventArgs e)
        {
            log.Info("ButtonMappingsCreateNew_Click");
            //open field to enter name
            var p = new EnterValuePopUp(Properties.Resources.EnterName);
            p.ShowDialog();
            if(p.value.Equals("")) //no name entered
            {
                new MessagePopUp(Properties.Resources.EnterName).ShowDialog();
                return;
            }
            selectedMapping = new Mapping(p.value);
            //open Pairs form
            SelectTab(MainFormHelper.GetTabPage(Constants.TabNameEditMapping, tabPages));
        }

        private void ButtonMappingsEdit_Click(object sender, EventArgs e)
        {
            log.Info("ButtonMappingsEdit_Click");
            SelectTab(MainFormHelper.GetTabPage(Constants.TabNameEditMapping, tabPages));
        }

        private void ListViewMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Info($"ListViewMappings_SelectedIndexChanged to '{listViewMappings.FocusedItem.Text}'");
            string sMappingName = listViewMappings.FocusedItem.Text;
            foreach(Mapping m in Mapping.GetMappings())
            {
                if(m.GetName() == sMappingName)
                {
                    selectedMapping = m;
                }
            }
            buttonNext.Enabled = true;
            buttonMappingsEdit.Enabled = true;
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameDiscounts, tabPages));
            tabControlMainForm.TabPages.Remove(MainFormHelper.GetTabPage(Constants.TabNameDictionary, tabPages));
        }

        private void TextBoxMappingsSearch_TextChanged(object sender, EventArgs e)
        {
            if(textBoxMappingsSearch.Text != "")
            {
                foreach(ListViewItem item in listViewMappings.Items)
                {
                    if(!item.Text.ToLower().Contains(textBoxMappingsSearch.Text.ToLower()))
                    {
                        listViewMappings.Items.Remove(item);
                    }
                }
                if(listViewMappings.SelectedItems.Count == 1)
                {
                    listViewMappings.Focus();
                }
            }
            else
            {
                listViewMappings = MainFormHelper.LoadMappingsList(listViewMappings);
            }
        }

        #endregion Mappings

        /// <summary>
        /// For <see cref="tabPageEditMapping"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Pair

        private void ButtonPairCreateNew_Click(object sender, EventArgs e)
        {
            log.Info("ButtonPairCreateNew_Click");
            //create empty one and select that index
            listViewPairs.Items.Add("empty");
            listViewPairs.SelectedItems.Clear();
            selectedPair = new PairCsvColumn(selectedMapping.GetName(), false, "empty", "empty");
            listViewPairs.Items[listViewPairs.Items.Count - 1].Focused = true;
            listViewPairs.Items[listViewPairs.Items.Count - 1].Selected = true;
        }

        private void ButtonPairDelete_Click(object sender, EventArgs e)
        {
            log.Info("ButtonPairDelete_Click");
            selectedMapping.Delete(selectedPair);
            listViewPairs = MainFormHelper.LoadPairsList(selectedMapping, listViewPairs);
            listViewPairs.Items[0].Focused = true;
            listViewPairs.Items[0].Selected = true;
        }

        private void ButtonPairPreview_Click(object sender, EventArgs e)
        {
            string[] header;
            string[] row;
            EnterValuePopUp enterValuePopUp = new EnterValuePopUp("Welche Zeile soll angezeigt werden?");
            enterValuePopUp.ShowDialog();
            try
            {
                int.TryParse(enterValuePopUp.value, out int selectedRow);
                if(selectedRow < 1)
                    selectedRow = 1;
                selectedRow += 1;
                //get CSV
                header = CSV.GetHeaderRow(path);
                string[][] csv = CSV.GetCsv(path);
                if(selectedRow >= csv.Length)
                    selectedRow = csv.Length - 1;
                log.Info($"Preview selected row: {selectedRow}");
                row = csv[selectedRow];
            }
            catch(Exception ex)
            {
                log.Warn("Exception when trying to load CSV for preview. Aborting preview.", ex);
                MessagePopUp popUp = new MessagePopUp($"Es ist ein Error beim laden der Vorschau aufgetreten.");
                popUp.ShowDialog();
                return;
            }
            //get values for each field
            Dictionary<string, string> fieldValueDict;
            ImportFromCsvToTempDb import = new ImportFromCsvToTempDb();
            fieldValueDict = import.GetValueForEachField(selectedMapping, header, row);
            if(fieldValueDict == null)
            {
                MessagePopUp popUp = new MessagePopUp("Error while importing. Can't preview. This row will not be imported.");
                popUp.ShowDialog();
                return;
            }
            //show preview
            PreviewForm previewForm = new PreviewForm(fieldValueDict);
            previewForm.ShowDialog();
        }

        private void ButtonPairSave_Click(object sender, EventArgs e)
        {
            log.Info("ButtonPairSave_Click");
            string mappingName = selectedMapping.GetName();
            bool isOverwrite = checkBoxPairsOverwrite.Checked;
            string targetField = comboBoxPairTarget.Text;
            string source = comboBoxPairSource.Text;
            Pair pair;
            switch(comboBoxPairType.Text)
            {
                case PairCsvColumn.type:
                    pair = new PairCsvColumn(mappingName, isOverwrite, targetField, source, textBoxPairsFactor.Text);
                    break;

                case PairConcatCsvColumns.type:
                    pair = new PairConcatCsvColumns(mappingName, isOverwrite, targetField, source, comboBoxPairDiscountKey.Text);
                    break;

                case PairAlternativeCsvColumn.type:
                    pair = new PairAlternativeCsvColumn(mappingName, isOverwrite, targetField, source, comboBoxPairDiscountKey.Text);
                    break;

                case PairFixedValue.type:
                    if("DATE".Equals(Field.GetFieldByName(Field.LoadFields(), targetField).GetSqlType()))
                    {
                        if(!Field.IsDateFormat(source))
                        {
                            new MessagePopUp("Falsches Datumformat. Bitte 2021-05-31 verwenden.").ShowDialog();
                            return;
                        }
                    }
                    pair = new PairFixedValue(mappingName, isOverwrite, targetField, textBoxPairSourceField.Text);
                    break;

                case PairCsvColumnWithDiscount.type:
                    pair = new PairCsvColumnWithDiscount(mappingName, isOverwrite, targetField, source, comboBoxPairDiscountKey.Text);
                    break;

                case PairCsvColumnWithDiscountValue.type:
                    pair = new PairCsvColumnWithDiscountValue(mappingName, isOverwrite, targetField, source, comboBoxPairDiscountKey.Text);
                    break;

                case PairChangingFixedValue.type:
                    pair = new PairChangingFixedValue(mappingName, isOverwrite, targetField);
                    break;

                case PairDiscountValue.type:
                    pair = new PairDiscountValue(mappingName, isOverwrite, targetField, comboBoxPairDiscountKey.Text);
                    break;

                case PairDictionaryValue.type:
                    pair = new PairDictionaryValue(mappingName, isOverwrite, targetField, source, comboBoxPairDictionaries.Text);
                    break;
                //TODO_NEWPAIRTYPE: add pairtype here

                default:
                    log.Error($"MainForm.ButtonPairSave_Click pair type unknown: '{comboBoxPairType.Text}'");
                    return; //don't save
            }
            selectedMapping.Delete(pair);
            bool successful = selectedMapping.Add(pair);
            if(!successful)
                new MessagePopUp("Speichern fehlgeschlagen").ShowDialog();
            listViewPairs = MainFormHelper.LoadPairsList(selectedMapping, listViewPairs);
        }

        private void ButtonPairsDeleteMapping_Click(object sender, EventArgs e)
        {
            log.Info("ButtonPairsDeleteMapping_Click");
            selectedMapping.Delete();
            SelectTab(tabControlMainForm.TabPages[0]);
        }

        private void ButtonPairsOpenDiscounts_Click(object sender, EventArgs e)
        {
            log.Info("ButtonPairsOpenDiscounts_Click");
            if(comboBoxPairType.Text.Equals(PairDiscountValue.type))
            {
                SelectTab(MainFormHelper.GetTabPage(Constants.TabNameDiscounts, tabPages));
                return;
            }
            if(comboBoxPairType.Text.Equals(PairCsvColumnWithDiscount.type))
            {
                SelectTab(MainFormHelper.GetTabPage(Constants.TabNameDiscounts, tabPages));
                return;
            }
            if(comboBoxPairType.Text.Equals(PairDictionaryValue.type))
            {
                if(comboBoxPairDictionaries.Text.Equals(""))
                {
                    new MessagePopUp(Properties.Resources.EnterDictName).ShowDialog();
                    return;
                }
                else
                {
                    selectedDictionary = comboBoxPairDictionaries.Text;
                    SelectTab(MainFormHelper.GetTabPage(Constants.TabNameDictionary, tabPages));
                    return;
                }
            }
            //TODO_NEWPAIRTYPE: if pair type contains a discount button add it here
            log.Error($"MainForm.ButtonPairsOpenDiscounts_Click PairType unknown: '{comboBoxPairType.Text}'");
        }

        private void ComboBoxPairType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //log.Info($"ComboBoxPairType_SelectedIndexChanged '{comboBoxPairType.SelectedItem}'"); //commented out due to too many calls
            string source = selectedPair.GetSource();
            string target = selectedPair.GetTargetField();
            if(selectedPair == null)
                selectedPair = new PairCsvColumn(selectedMapping.GetName(), false, target, source);
            string isOverwrite = "0";
            if(selectedPair.IsOverwrite())
                isOverwrite = "1";
            selectedPair = Pair.GetUnknownPair(
                selectedMapping.GetName(),
                comboBoxPairType.Text,
                comboBoxPairTarget.Text,
                comboBoxPairSource.Text,
                isOverwrite,
                selectedPair.GetAdditionalSource()
                );
            FillBoxesEditMapping(selectedPair);
        }

        /// <summary>
        /// Fills the textBox, comboBox, ect. with values of the <paramref name="pair"/>.
        /// </summary>
        /// <param name="pair">containing info to be filled</param>
        private void FillBoxesEditMapping(Pair pair)
        {
            textBoxPairSourceField.Visible = false;
            comboBoxPairSource.Visible = false;
            textBoxPairsFactor.Visible = false;
            labelPairsFactor.Visible = false;
            labelPairsSourcefield.Visible = false;
            comboBoxPairDiscountKey.Visible = false;
            buttonPairsOpenDiscounts.Visible = false;
            comboBoxPairDictionaries.Visible = false;

            if(comboBoxPairTarget.Items.Contains(pair.GetTargetField()))
                comboBoxPairTarget.SelectedIndex = comboBoxPairTarget.Items.IndexOf(pair.GetTargetField());
            else
                comboBoxPairTarget.Text = pair.GetTargetField();
            if(comboBoxPairType.Items.Contains(pair.GetPairType()))
                comboBoxPairType.SelectedIndex = comboBoxPairType.Items.IndexOf(pair.GetPairType());
            else
                comboBoxPairType.Text = pair.GetPairType();
            checkBoxPairsOverwrite.Checked = pair.IsOverwrite();
            comboBoxPairTarget.Visible = true;
            comboBoxPairType.Visible = true;
            labelPairsTargetfield.Visible = true;
            labelPairsType.Visible = true;
            switch(pair.GetPairType())
            {
                case PairFixedValue.type:
                    textBoxPairSourceField.Text = pair.GetSource();
                    labelPairsSourcefield.Text = "Value";
                    textBoxPairSourceField.Visible = true;
                    labelPairsSourcefield.Visible = true;
                    break;

                case PairCsvColumn.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();
                    textBoxPairsFactor.Text = pair.GetAdditionalSource();
                    labelPairsSourcefield.Text = "Sourcefield";
                    labelPairsFactor.Text = "Factor";
                    textBoxPairsFactor.Visible = true;
                    labelPairsFactor.Visible = true;
                    labelPairsSourcefield.Visible = true;
                    comboBoxPairSource.Visible = true;
                    break;

                case PairCsvColumnWithDiscount.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();

                    if(comboBoxPairDiscountKey.Items.Contains(pair.GetAdditionalSource()))
                        comboBoxPairDiscountKey.SelectedIndex = comboBoxPairDiscountKey.Items.IndexOf(pair.GetAdditionalSource());
                    else
                        comboBoxPairDiscountKey.Text = pair.GetAdditionalSource();
                    labelPairsFactor.Text = "DiscountField";
                    labelPairsSourcefield.Text = "SourceField";
                    buttonPairsOpenDiscounts.Text = "Discounts";
                    comboBoxPairSource.Visible = true;
                    comboBoxPairDiscountKey.Visible = true;
                    labelPairsFactor.Visible = true;
                    labelPairsSourcefield.Visible = true;
                    buttonPairsOpenDiscounts.Visible = true;
                    break;

                case PairCsvColumnWithDiscountValue.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();

                    if(comboBoxPairDiscountKey.Items.Contains(pair.GetAdditionalSource()))
                        comboBoxPairDiscountKey.SelectedIndex = comboBoxPairDiscountKey.Items.IndexOf(pair.GetAdditionalSource());
                    else
                        comboBoxPairDiscountKey.Text = pair.GetAdditionalSource();
                    labelPairsFactor.Text = "DiscountField";
                    labelPairsSourcefield.Text = "SourceField";
                    comboBoxPairSource.Visible = true;
                    comboBoxPairDiscountKey.Visible = true;
                    labelPairsFactor.Visible = true;
                    labelPairsSourcefield.Visible = true;
                    break;

                case PairChangingFixedValue.type:
                    textBoxPairSourceField.Text = pair.GetSource();
                    labelPairsSourcefield.Text = "Value";
                    textBoxPairSourceField.Visible = true;
                    labelPairsSourcefield.Visible = true;
                    break;

                case PairDiscountValue.type:
                    if(comboBoxPairDiscountKey.Items.Contains(pair.GetSource()))
                        comboBoxPairDiscountKey.SelectedIndex = comboBoxPairDiscountKey.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairDiscountKey.Text = pair.GetSource();
                    labelPairsFactor.Text = "DiscountField";
                    comboBoxPairDiscountKey.Visible = true;
                    labelPairsFactor.Visible = true;
                    buttonPairsOpenDiscounts.Visible = true;
                    break;

                case PairDictionaryValue.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();
                    comboBoxPairDictionaries.Items.Clear();
                    comboBoxPairDictionaries.Items.AddRange(CustomDictionary.GetList(selectedMapping.GetName()));
                    if(comboBoxPairDictionaries.Items.Contains(pair.GetAdditionalSource()))
                        comboBoxPairDictionaries.SelectedIndex = comboBoxPairDictionaries.Items.IndexOf(pair.GetAdditionalSource());
                    else
                        comboBoxPairDictionaries.Text = pair.GetAdditionalSource();
                    labelPairsSourcefield.Text = "SourceField";
                    labelPairsFactor.Text = "Dictionary";
                    buttonPairsOpenDiscounts.Text = "Edit";
                    labelPairsSourcefield.Visible = true;
                    labelPairsFactor.Visible = true;
                    comboBoxPairSource.Visible = true;
                    buttonPairsOpenDiscounts.Visible = true;
                    comboBoxPairDictionaries.Visible = true;
                    break;

                case PairConcatCsvColumns.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();

                    if(comboBoxPairDiscountKey.Items.Contains(pair.GetAdditionalSource()))
                        comboBoxPairDiscountKey.SelectedIndex = comboBoxPairDiscountKey.Items.IndexOf(pair.GetAdditionalSource());
                    else
                        comboBoxPairDiscountKey.Text = pair.GetAdditionalSource();
                    labelPairsSourcefield.Text = "SourcefieldA";
                    labelPairsFactor.Text = "SourcefieldB";
                    labelPairsSourcefield.Visible = true;
                    labelPairsFactor.Visible = true;
                    comboBoxPairSource.Visible = true;
                    comboBoxPairDiscountKey.Visible = true;
                    break;

                case PairAlternativeCsvColumn.type:
                    if(comboBoxPairSource.Items.Contains(pair.GetSource()))
                        comboBoxPairSource.SelectedIndex = comboBoxPairSource.Items.IndexOf(pair.GetSource());
                    else
                        comboBoxPairSource.Text = pair.GetSource();

                    if(comboBoxPairDiscountKey.Items.Contains(pair.GetAdditionalSource()))
                        comboBoxPairDiscountKey.SelectedIndex = comboBoxPairDiscountKey.Items.IndexOf(pair.GetAdditionalSource());
                    else
                        comboBoxPairDiscountKey.Text = pair.GetAdditionalSource();
                    labelPairsSourcefield.Text = "Main Column";
                    labelPairsFactor.Text = "Alternative Column";
                    labelPairsSourcefield.Visible = true;
                    labelPairsFactor.Visible = true;
                    comboBoxPairSource.Visible = true;
                    comboBoxPairDiscountKey.Visible = true;
                    break;

                //TODO_NEWPAIRTYPE: add pair type here to change frontent when its selected

                default:
                    //log.Debug("Pair: " + selectedPair.ToString());
                    log.Error($"MainForm.ComboBoxPairType_SelectedIndexChanged pair type unknown: '{comboBoxPairType.Text}'");
                    break;
            }
        }

        private void ListViewPairs_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Info($"MainForm.ListViewPairs_SelectedIndexChanged '{listViewPairs.FocusedItem.Text}'");
            string sPairName = listViewPairs.FocusedItem.Text;
            foreach(Pair p in selectedMapping.LoadPairs())
            {
                if(p.GetTargetField() == sPairName)
                {
                    selectedPair = p;
                    break;
                }
            }

            FillBoxesEditMapping(selectedPair);
        }

        #endregion Pair

        /// <summary>
        /// For <see cref="tabPageUpload"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Upload

        private void ButtonUpload_Click(object sender, EventArgs e)
        {
            log.Info("ButtonUpload_Click");
            //sort CSV and insert articles into program own database this is the heart of this program
            new ProgressPopUp(ProgressPopUp.Import, selectedMapping, textBoxPathUpload.Text).ShowDialog();
            buttonUpload.Enabled = false;
        }

        private void ButtonUploadBrowse_Click(object sender, EventArgs e)
        {
            log.Info("ButtonUploadBrowse_Click");
            //browse data to choose CSV
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"{selectedMapping.GetName()} oeffnen...";
                if(string.Empty.Equals(path))
                {
                    openFileDialog.InitialDirectory = Properties.Settings.Default.InitialDirectory;
                }
                else
                {
                    openFileDialog.InitialDirectory = path.Remove(path.LastIndexOf('\\'));
                }
                openFileDialog.Filter = "csv files (*.csv)|*.csv|Csv files (*.csv*)|*.csv*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxPathUpload.Text = openFileDialog.FileName;
                    path = openFileDialog.FileName;
                    log.Info($"File path chosen: '{path}'");
                    buttonVerifyUpload.Enabled = true;
                }
                else
                {
                    if(string.Empty.Equals(path))
                    {
                        buttonVerifyUpload.Enabled = false;
                        log.Info("no path selected");
                    }
                }
            }
        }

        private void ButtonUploadVerify_Click(object sender, EventArgs e)
        {
            log.Info("ButtonUploadVerify_Click");
            string[] headerRow = CSV.GetHeaderRow(textBoxPathUpload.Text);
            if(headerRow.Length == 0)
            {
                new MessagePopUp("Could not open the file.", Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
                return; //The file is still open
            }
            Pair[] missingPairs = CSV.Verify(headerRow, selectedMapping);
            if(missingPairs.Length != 0)
            {
                buttonUpload.Enabled = false;
                string message = Properties.Resources.CsvIsMissingRows + "\n" + string.Join("\n ", missingPairs.Select(p => p.GetTargetField() + "->" + p.GetSource()));
                new MessagePopUp(message, Properties.Settings.Default.ShowTimeWarningSec).ShowDialog();
                return;
            }
            //update massImport csv
            var f = new ConfirmationPopUp("Do you want to update the massimport file?");
            f.ShowDialog();
            if(f.confirmed)
            {
                string[][] massImportCsv = CSV.GetCsv(Properties.Settings.Default.MassImportFilePath);
                int index = -1;
                for(int i = 0;i < massImportCsv.Length;i++)
                {
                    if(selectedMapping.GetName().Equals(massImportCsv[i][1]))
                    {
                        index = i;
                        break;
                    }
                }
                string pathValue = Path.Combine(Path.GetFileName(Path.GetDirectoryName(textBoxPathUpload.Text)), Path.GetFileName(textBoxPathUpload.Text));
                pathValue = pathValue.Replace("\\", "/");
                if(index < 0)
                {
                    log.Info("Add row to massImportCsv");
                    List<string[]> l = massImportCsv.ToList();
                    l.Add(new string[] { pathValue, selectedMapping.GetName() });
                    massImportCsv = l.ToArray();
                }
                else
                {
                    log.Info("Updated path in massImportCsv");
                    massImportCsv[index][0] = pathValue;
                }
                string[] lines = massImportCsv.Select(s => string.Join("\t", s)).ToArray();
                File.WriteAllLines(Properties.Settings.Default.MassImportFilePath, lines, System.Text.Encoding.Unicode);
            }
            buttonUpload.Enabled = true;
        }

        #endregion Upload

        /// <summary>
        /// For <see cref="tabPageValues"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Values

        private void ButtonValuesSave_Click(object sender, EventArgs e)
        {
            log.Info("ButtonValuesSave_Click");
            string value = textBoxValuesValue.Text;
            Field field = Field.GetFieldByName(Field.LoadFields(), selectedPair.GetTargetField());
            if("DATE".Equals(field.GetSqlType()))
            {
                if(!Field.IsDateFormat(value))
                {
                    new MessagePopUp("Falsches Datumformat. Bitte 2021-05-31 verwenden.").ShowDialog();
                    return;
                }
            }
            Pair pair = new PairChangingFixedValue(selectedMapping.GetName(), selectedPair.IsOverwrite(), selectedPair.GetTargetField(), value);
            selectedMapping.Delete(pair);
            bool successful = selectedMapping.Add(pair);
            if(!successful)
                new MessagePopUp("Speichern fehlgeschlagen").ShowDialog();
            listViewValues = MainFormHelper.LoadValuesList(selectedMapping, listViewValues);
        }

        private void ListViewValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Info($"MainForm.ListViewValues_SelectedIndexChanged to '{listViewValues.FocusedItem.Text}'");
            string sPairName = listViewValues.FocusedItem.Text;
            foreach(Pair p in selectedMapping.GetPairs())
            {
                if(p.GetTargetField() == sPairName)
                {
                    selectedPair = p;
                }
            }
            labelValuesTarget.Text = selectedPair.GetTargetField();
            textBoxValuesValue.Text = selectedPair.GetSource();
            if("-".Equals(textBoxValuesValue.Text))
                textBoxValuesValue.Text = ""; //don't show the '-', it's just annoying to remove when entering values
        }

        #endregion Values
    }
}