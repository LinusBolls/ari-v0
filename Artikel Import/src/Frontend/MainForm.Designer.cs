using System;
using System.Configuration;
using System.Windows.Forms.VisualStyles;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// The main Form of the program responsible for all of the user interaction
    /// </summary>
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonInha = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemClearDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSendToRuntime = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFields = new System.Windows.Forms.ToolStripMenuItem();
            this.renameArticlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMainForm = new System.Windows.Forms.TabControl();
            this.tabPageMappings = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxMappingsSearch = new System.Windows.Forms.TextBox();
            this.buttonMappingsEdit = new System.Windows.Forms.Button();
            this.listViewMappings = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonMappingsCreateNew = new System.Windows.Forms.Button();
            this.tabPageFields = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxFieldIsNullable = new System.Windows.Forms.CheckBox();
            this.labelFieldScale = new System.Windows.Forms.Label();
            this.textBoxFieldScale = new System.Windows.Forms.TextBox();
            this.checkboxFieldsDontInsertNull = new System.Windows.Forms.CheckBox();
            this.labelFieldsTargetfield = new System.Windows.Forms.Label();
            this.labelFieldsDescription = new System.Windows.Forms.Label();
            this.textBoxFieldTarget = new System.Windows.Forms.TextBox();
            this.labelFieldsName = new System.Windows.Forms.Label();
            this.textBoxFieldDescription = new System.Windows.Forms.TextBox();
            this.listViewFields = new System.Windows.Forms.ListView();
            this.columnHeaderFieldsName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFieldsTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFieldsDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFieldsType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFieldsSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFieldsIsNVL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxFieldName = new System.Windows.Forms.TextBox();
            this.buttonFieldsCreateNew = new System.Windows.Forms.Button();
            this.labelFieldsType = new System.Windows.Forms.Label();
            this.buttonFieldsSave = new System.Windows.Forms.Button();
            this.comboBoxFieldType = new System.Windows.Forms.ComboBox();
            this.buttonFieldsDelete = new System.Windows.Forms.Button();
            this.labelFieldsSize = new System.Windows.Forms.Label();
            this.textBoxFieldSize = new System.Windows.Forms.TextBox();
            this.tabPageDiscounts = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxDiscountsKey = new System.Windows.Forms.TextBox();
            this.labelDiscountsFormatHint = new System.Windows.Forms.Label();
            this.listViewDiscounts = new System.Windows.Forms.ListView();
            this.columnHeaderKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDiscount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxDiscountsDiscount = new System.Windows.Forms.TextBox();
            this.buttonDiscountsCreateNew = new System.Windows.Forms.Button();
            this.labelDiscountsMappingName = new System.Windows.Forms.Label();
            this.labelDiscountsDiscount = new System.Windows.Forms.Label();
            this.buttonDiscountsSave = new System.Windows.Forms.Button();
            this.buttonDiscountsDelete = new System.Windows.Forms.Button();
            this.tabPageEditMapping = new System.Windows.Forms.TabPage();
            this.buttonPairPreview = new System.Windows.Forms.Button();
            this.comboBoxPairDictionaries = new System.Windows.Forms.ComboBox();
            this.buttonPairsOpenDiscounts = new System.Windows.Forms.Button();
            this.comboBoxPairDiscountKey = new System.Windows.Forms.ComboBox();
            this.labelPairsFactor = new System.Windows.Forms.Label();
            this.textBoxPairsFactor = new System.Windows.Forms.TextBox();
            this.checkBoxPairsOverwrite = new System.Windows.Forms.CheckBox();
            this.comboBoxPairSource = new System.Windows.Forms.ComboBox();
            this.comboBoxPairType = new System.Windows.Forms.ComboBox();
            this.textBoxPairSourceField = new System.Windows.Forms.TextBox();
            this.labelPairsType = new System.Windows.Forms.Label();
            this.buttonPairsSave = new System.Windows.Forms.Button();
            this.labelPairsSourcefield = new System.Windows.Forms.Label();
            this.buttonPairsDelete = new System.Windows.Forms.Button();
            this.comboBoxPairTarget = new System.Windows.Forms.ComboBox();
            this.labelPairsTargetfield = new System.Windows.Forms.Label();
            this.listViewPairs = new System.Windows.Forms.ListView();
            this.columnHeaderPairsTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPairsType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPairsSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPairsOverwrite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCreateNewPair = new System.Windows.Forms.Button();
            this.labelPairsMappingName = new System.Windows.Forms.Label();
            this.tabPageUpload = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBrowseUpload = new System.Windows.Forms.Button();
            this.textBoxPathUpload = new System.Windows.Forms.TextBox();
            this.labelUploadSelectedMapping = new System.Windows.Forms.Label();
            this.buttonVerifyUpload = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.tabPageValues = new System.Windows.Forms.TabPage();
            this.textBoxValuesValue = new System.Windows.Forms.TextBox();
            this.listViewValues = new System.Windows.Forms.ListView();
            this.columnHeaderValuesName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderValuesValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelValuesTarget = new System.Windows.Forms.Label();
            this.buttonValuesSave = new System.Windows.Forms.Button();
            this.labelValuesMappingName = new System.Windows.Forms.Label();
            this.labelValuesValue = new System.Windows.Forms.Label();
            this.tabPageDictionary = new System.Windows.Forms.TabPage();
            this.listViewDictionary = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxDictionaryValue = new System.Windows.Forms.TextBox();
            this.labelDictionaryMapping = new System.Windows.Forms.Label();
            this.labelDictionaryKey = new System.Windows.Forms.Label();
            this.labelDictionaryValue = new System.Windows.Forms.Label();
            this.buttonDictionarySave = new System.Windows.Forms.Button();
            this.buttonDictionaryDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.tabControlMainForm.SuspendLayout();
            this.tabPageMappings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageFields.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPageDiscounts.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPageEditMapping.SuspendLayout();
            this.tabPageUpload.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPageValues.SuspendLayout();
            this.tabPageDictionary.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonInha,
            this.toolStripDropDownButtonOptions});
            this.toolStrip1.Name = "toolStrip1";
            this.toolTip1.SetToolTip(this.toolStrip1, resources.GetString("toolStrip1.ToolTip"));
            // 
            // toolStripButtonInha
            // 
            resources.ApplyResources(this.toolStripButtonInha, "toolStripButtonInha");
            this.toolStripButtonInha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInha.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.toolStripButtonInha.Name = "toolStripButtonInha";
            // 
            // toolStripDropDownButtonOptions
            // 
            resources.ApplyResources(this.toolStripDropDownButtonOptions, "toolStripDropDownButtonOptions");
            this.toolStripDropDownButtonOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButtonOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemClearDatabase,
            this.ToolStripMenuItemSendToRuntime,
            this.ToolStripMenuItemFields,
            this.renameArticlesToolStripMenuItem});
            this.toolStripDropDownButtonOptions.Name = "toolStripDropDownButtonOptions";
            // 
            // ToolStripMenuItemClearDatabase
            // 
            resources.ApplyResources(this.ToolStripMenuItemClearDatabase, "ToolStripMenuItemClearDatabase");
            this.ToolStripMenuItemClearDatabase.Name = "ToolStripMenuItemClearDatabase";
            this.ToolStripMenuItemClearDatabase.Click += new System.EventHandler(this.ToolStripMenuItemClearDatabase_Click);
            // 
            // ToolStripMenuItemSendToRuntime
            // 
            resources.ApplyResources(this.ToolStripMenuItemSendToRuntime, "ToolStripMenuItemSendToRuntime");
            this.ToolStripMenuItemSendToRuntime.Name = "ToolStripMenuItemSendToRuntime";
            this.ToolStripMenuItemSendToRuntime.Click += new System.EventHandler(this.ToolStripMenuItemSendToRuntime_Click);
            // 
            // ToolStripMenuItemFields
            // 
            resources.ApplyResources(this.ToolStripMenuItemFields, "ToolStripMenuItemFields");
            this.ToolStripMenuItemFields.Name = "ToolStripMenuItemFields";
            this.ToolStripMenuItemFields.Click += new System.EventHandler(this.ToolStripMenuItemFields_Click);
            // 
            // renameArticlesToolStripMenuItem
            // 
            resources.ApplyResources(this.renameArticlesToolStripMenuItem, "renameArticlesToolStripMenuItem");
            this.renameArticlesToolStripMenuItem.Name = "renameArticlesToolStripMenuItem";
            this.renameArticlesToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemArtikelNrReplace);
            // 
            // toolStripTextBox1
            // 
            resources.ApplyResources(this.toolStripTextBox1, "toolStripTextBox1");
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // tabControlMainForm
            // 
            resources.ApplyResources(this.tabControlMainForm, "tabControlMainForm");
            this.tabControlMainForm.Controls.Add(this.tabPageMappings);
            this.tabControlMainForm.Controls.Add(this.tabPageFields);
            this.tabControlMainForm.Controls.Add(this.tabPageDiscounts);
            this.tabControlMainForm.Controls.Add(this.tabPageEditMapping);
            this.tabControlMainForm.Controls.Add(this.tabPageUpload);
            this.tabControlMainForm.Controls.Add(this.tabPageValues);
            this.tabControlMainForm.Controls.Add(this.tabPageDictionary);
            this.tabControlMainForm.Name = "tabControlMainForm";
            this.tabControlMainForm.SelectedIndex = 0;
            this.toolTip1.SetToolTip(this.tabControlMainForm, resources.GetString("tabControlMainForm.ToolTip"));
            this.tabControlMainForm.SelectedIndexChanged += new System.EventHandler(this.TabControlMainForm_SelectedIndexChanged);
            // 
            // tabPageMappings
            // 
            resources.ApplyResources(this.tabPageMappings, "tabPageMappings");
            this.tabPageMappings.Controls.Add(this.panel1);
            this.tabPageMappings.Name = "tabPageMappings";
            this.toolTip1.SetToolTip(this.tabPageMappings, resources.GetString("tabPageMappings.ToolTip"));
            this.tabPageMappings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.textBoxMappingsSearch);
            this.panel1.Controls.Add(this.buttonMappingsEdit);
            this.panel1.Controls.Add(this.listViewMappings);
            this.panel1.Controls.Add(this.ButtonMappingsCreateNew);
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // textBoxMappingsSearch
            // 
            resources.ApplyResources(this.textBoxMappingsSearch, "textBoxMappingsSearch");
            this.textBoxMappingsSearch.Name = "textBoxMappingsSearch";
            this.toolTip1.SetToolTip(this.textBoxMappingsSearch, resources.GetString("textBoxMappingsSearch.ToolTip"));
            this.textBoxMappingsSearch.TextChanged += new System.EventHandler(this.TextBoxMappingsSearch_TextChanged);
            // 
            // buttonMappingsEdit
            // 
            resources.ApplyResources(this.buttonMappingsEdit, "buttonMappingsEdit");
            this.buttonMappingsEdit.Name = "buttonMappingsEdit";
            this.toolTip1.SetToolTip(this.buttonMappingsEdit, resources.GetString("buttonMappingsEdit.ToolTip"));
            this.buttonMappingsEdit.UseVisualStyleBackColor = true;
            this.buttonMappingsEdit.Click += new System.EventHandler(this.ButtonMappingsEdit_Click);
            // 
            // listViewMappings
            // 
            resources.ApplyResources(this.listViewMappings, "listViewMappings");
            this.listViewMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewMappings.HideSelection = false;
            this.listViewMappings.Name = "listViewMappings";
            this.toolTip1.SetToolTip(this.listViewMappings, resources.GetString("listViewMappings.ToolTip"));
            this.listViewMappings.UseCompatibleStateImageBehavior = false;
            this.listViewMappings.View = System.Windows.Forms.View.List;
            this.listViewMappings.SelectedIndexChanged += new System.EventHandler(this.ListViewMappings_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // ButtonMappingsCreateNew
            // 
            resources.ApplyResources(this.ButtonMappingsCreateNew, "ButtonMappingsCreateNew");
            this.ButtonMappingsCreateNew.Name = "ButtonMappingsCreateNew";
            this.toolTip1.SetToolTip(this.ButtonMappingsCreateNew, resources.GetString("ButtonMappingsCreateNew.ToolTip"));
            this.ButtonMappingsCreateNew.UseVisualStyleBackColor = true;
            this.ButtonMappingsCreateNew.Click += new System.EventHandler(this.ButtonMappingsCreateNew_Click);
            // 
            // tabPageFields
            // 
            resources.ApplyResources(this.tabPageFields, "tabPageFields");
            this.tabPageFields.Controls.Add(this.tableLayoutPanel3);
            this.tabPageFields.Name = "tabPageFields";
            this.toolTip1.SetToolTip(this.tabPageFields, resources.GetString("tabPageFields.ToolTip"));
            this.tabPageFields.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.toolTip1.SetToolTip(this.tableLayoutPanel3, resources.GetString("tableLayoutPanel3.ToolTip"));
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.checkBoxFieldIsNullable);
            this.panel2.Controls.Add(this.labelFieldScale);
            this.panel2.Controls.Add(this.textBoxFieldScale);
            this.panel2.Controls.Add(this.checkboxFieldsDontInsertNull);
            this.panel2.Controls.Add(this.labelFieldsTargetfield);
            this.panel2.Controls.Add(this.labelFieldsDescription);
            this.panel2.Controls.Add(this.textBoxFieldTarget);
            this.panel2.Controls.Add(this.labelFieldsName);
            this.panel2.Controls.Add(this.textBoxFieldDescription);
            this.panel2.Controls.Add(this.listViewFields);
            this.panel2.Controls.Add(this.textBoxFieldName);
            this.panel2.Controls.Add(this.buttonFieldsCreateNew);
            this.panel2.Controls.Add(this.labelFieldsType);
            this.panel2.Controls.Add(this.buttonFieldsSave);
            this.panel2.Controls.Add(this.comboBoxFieldType);
            this.panel2.Controls.Add(this.buttonFieldsDelete);
            this.panel2.Controls.Add(this.labelFieldsSize);
            this.panel2.Controls.Add(this.textBoxFieldSize);
            this.panel2.Name = "panel2";
            this.toolTip1.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
            // 
            // checkBoxFieldIsNullable
            // 
            resources.ApplyResources(this.checkBoxFieldIsNullable, "checkBoxFieldIsNullable");
            this.checkBoxFieldIsNullable.Name = "checkBoxFieldIsNullable";
            this.toolTip1.SetToolTip(this.checkBoxFieldIsNullable, resources.GetString("checkBoxFieldIsNullable.ToolTip"));
            this.checkBoxFieldIsNullable.UseVisualStyleBackColor = true;
            // 
            // labelFieldScale
            // 
            resources.ApplyResources(this.labelFieldScale, "labelFieldScale");
            this.labelFieldScale.Name = "labelFieldScale";
            this.toolTip1.SetToolTip(this.labelFieldScale, resources.GetString("labelFieldScale.ToolTip"));
            // 
            // textBoxFieldScale
            // 
            resources.ApplyResources(this.textBoxFieldScale, "textBoxFieldScale");
            this.textBoxFieldScale.Name = "textBoxFieldScale";
            this.toolTip1.SetToolTip(this.textBoxFieldScale, resources.GetString("textBoxFieldScale.ToolTip"));
            // 
            // checkboxFieldsDontInsertNull
            // 
            resources.ApplyResources(this.checkboxFieldsDontInsertNull, "checkboxFieldsDontInsertNull");
            this.checkboxFieldsDontInsertNull.Name = "checkboxFieldsDontInsertNull";
            this.toolTip1.SetToolTip(this.checkboxFieldsDontInsertNull, resources.GetString("checkboxFieldsDontInsertNull.ToolTip"));
            this.checkboxFieldsDontInsertNull.UseVisualStyleBackColor = true;
            // 
            // labelFieldsTargetfield
            // 
            resources.ApplyResources(this.labelFieldsTargetfield, "labelFieldsTargetfield");
            this.labelFieldsTargetfield.Name = "labelFieldsTargetfield";
            this.toolTip1.SetToolTip(this.labelFieldsTargetfield, resources.GetString("labelFieldsTargetfield.ToolTip"));
            // 
            // labelFieldsDescription
            // 
            resources.ApplyResources(this.labelFieldsDescription, "labelFieldsDescription");
            this.labelFieldsDescription.Name = "labelFieldsDescription";
            this.toolTip1.SetToolTip(this.labelFieldsDescription, resources.GetString("labelFieldsDescription.ToolTip"));
            // 
            // textBoxFieldTarget
            // 
            resources.ApplyResources(this.textBoxFieldTarget, "textBoxFieldTarget");
            this.textBoxFieldTarget.Name = "textBoxFieldTarget";
            this.toolTip1.SetToolTip(this.textBoxFieldTarget, resources.GetString("textBoxFieldTarget.ToolTip"));
            // 
            // labelFieldsName
            // 
            resources.ApplyResources(this.labelFieldsName, "labelFieldsName");
            this.labelFieldsName.Name = "labelFieldsName";
            this.toolTip1.SetToolTip(this.labelFieldsName, resources.GetString("labelFieldsName.ToolTip"));
            // 
            // textBoxFieldDescription
            // 
            resources.ApplyResources(this.textBoxFieldDescription, "textBoxFieldDescription");
            this.textBoxFieldDescription.Name = "textBoxFieldDescription";
            this.toolTip1.SetToolTip(this.textBoxFieldDescription, resources.GetString("textBoxFieldDescription.ToolTip"));
            // 
            // listViewFields
            // 
            resources.ApplyResources(this.listViewFields, "listViewFields");
            this.listViewFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFieldsName,
            this.columnHeaderFieldsTarget,
            this.columnHeaderFieldsDescription,
            this.columnHeaderFieldsType,
            this.columnHeaderFieldsSize,
            this.columnHeaderFieldsIsNVL});
            this.listViewFields.GridLines = true;
            this.listViewFields.HideSelection = false;
            this.listViewFields.Name = "listViewFields";
            this.toolTip1.SetToolTip(this.listViewFields, resources.GetString("listViewFields.ToolTip"));
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.View = System.Windows.Forms.View.Details;
            this.listViewFields.SelectedIndexChanged += new System.EventHandler(this.ListViewFields_SelectedIndexChanged);
            // 
            // columnHeaderFieldsName
            // 
            resources.ApplyResources(this.columnHeaderFieldsName, "columnHeaderFieldsName");
            // 
            // columnHeaderFieldsTarget
            // 
            resources.ApplyResources(this.columnHeaderFieldsTarget, "columnHeaderFieldsTarget");
            // 
            // columnHeaderFieldsDescription
            // 
            resources.ApplyResources(this.columnHeaderFieldsDescription, "columnHeaderFieldsDescription");
            // 
            // columnHeaderFieldsType
            // 
            resources.ApplyResources(this.columnHeaderFieldsType, "columnHeaderFieldsType");
            // 
            // columnHeaderFieldsSize
            // 
            resources.ApplyResources(this.columnHeaderFieldsSize, "columnHeaderFieldsSize");
            // 
            // columnHeaderFieldsIsNVL
            // 
            resources.ApplyResources(this.columnHeaderFieldsIsNVL, "columnHeaderFieldsIsNVL");
            // 
            // textBoxFieldName
            // 
            resources.ApplyResources(this.textBoxFieldName, "textBoxFieldName");
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.toolTip1.SetToolTip(this.textBoxFieldName, resources.GetString("textBoxFieldName.ToolTip"));
            // 
            // buttonFieldsCreateNew
            // 
            resources.ApplyResources(this.buttonFieldsCreateNew, "buttonFieldsCreateNew");
            this.buttonFieldsCreateNew.Name = "buttonFieldsCreateNew";
            this.toolTip1.SetToolTip(this.buttonFieldsCreateNew, resources.GetString("buttonFieldsCreateNew.ToolTip"));
            this.buttonFieldsCreateNew.UseVisualStyleBackColor = true;
            this.buttonFieldsCreateNew.Click += new System.EventHandler(this.ButtonFieldCreateNew_Click);
            // 
            // labelFieldsType
            // 
            resources.ApplyResources(this.labelFieldsType, "labelFieldsType");
            this.labelFieldsType.Name = "labelFieldsType";
            this.toolTip1.SetToolTip(this.labelFieldsType, resources.GetString("labelFieldsType.ToolTip"));
            // 
            // buttonFieldsSave
            // 
            resources.ApplyResources(this.buttonFieldsSave, "buttonFieldsSave");
            this.buttonFieldsSave.Name = "buttonFieldsSave";
            this.toolTip1.SetToolTip(this.buttonFieldsSave, resources.GetString("buttonFieldsSave.ToolTip"));
            this.buttonFieldsSave.UseVisualStyleBackColor = true;
            this.buttonFieldsSave.Click += new System.EventHandler(this.ButtonFieldSave_Click);
            // 
            // comboBoxFieldType
            // 
            resources.ApplyResources(this.comboBoxFieldType, "comboBoxFieldType");
            this.comboBoxFieldType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxFieldType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFieldType.FormattingEnabled = true;
            this.comboBoxFieldType.Items.AddRange(new object[] {
            resources.GetString("comboBoxFieldType.Items"),
            resources.GetString("comboBoxFieldType.Items1"),
            resources.GetString("comboBoxFieldType.Items2")});
            this.comboBoxFieldType.Name = "comboBoxFieldType";
            this.toolTip1.SetToolTip(this.comboBoxFieldType, resources.GetString("comboBoxFieldType.ToolTip"));
            // 
            // buttonFieldsDelete
            // 
            resources.ApplyResources(this.buttonFieldsDelete, "buttonFieldsDelete");
            this.buttonFieldsDelete.Name = "buttonFieldsDelete";
            this.toolTip1.SetToolTip(this.buttonFieldsDelete, resources.GetString("buttonFieldsDelete.ToolTip"));
            this.buttonFieldsDelete.UseVisualStyleBackColor = true;
            this.buttonFieldsDelete.Click += new System.EventHandler(this.ButtonFieldDelete_Click);
            // 
            // labelFieldsSize
            // 
            resources.ApplyResources(this.labelFieldsSize, "labelFieldsSize");
            this.labelFieldsSize.Name = "labelFieldsSize";
            this.toolTip1.SetToolTip(this.labelFieldsSize, resources.GetString("labelFieldsSize.ToolTip"));
            // 
            // textBoxFieldSize
            // 
            resources.ApplyResources(this.textBoxFieldSize, "textBoxFieldSize");
            this.textBoxFieldSize.Name = "textBoxFieldSize";
            this.toolTip1.SetToolTip(this.textBoxFieldSize, resources.GetString("textBoxFieldSize.ToolTip"));
            // 
            // tabPageDiscounts
            // 
            resources.ApplyResources(this.tabPageDiscounts, "tabPageDiscounts");
            this.tabPageDiscounts.Controls.Add(this.panel5);
            this.tabPageDiscounts.Name = "tabPageDiscounts";
            this.toolTip1.SetToolTip(this.tabPageDiscounts, resources.GetString("tabPageDiscounts.ToolTip"));
            this.tabPageDiscounts.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.textBoxDiscountsKey);
            this.panel5.Controls.Add(this.labelDiscountsFormatHint);
            this.panel5.Controls.Add(this.listViewDiscounts);
            this.panel5.Controls.Add(this.textBoxDiscountsDiscount);
            this.panel5.Controls.Add(this.buttonDiscountsCreateNew);
            this.panel5.Controls.Add(this.labelDiscountsMappingName);
            this.panel5.Controls.Add(this.labelDiscountsDiscount);
            this.panel5.Controls.Add(this.buttonDiscountsSave);
            this.panel5.Controls.Add(this.buttonDiscountsDelete);
            this.panel5.Name = "panel5";
            this.toolTip1.SetToolTip(this.panel5, resources.GetString("panel5.ToolTip"));
            // 
            // textBoxDiscountsKey
            // 
            resources.ApplyResources(this.textBoxDiscountsKey, "textBoxDiscountsKey");
            this.textBoxDiscountsKey.Name = "textBoxDiscountsKey";
            this.toolTip1.SetToolTip(this.textBoxDiscountsKey, resources.GetString("textBoxDiscountsKey.ToolTip"));
            // 
            // labelDiscountsFormatHint
            // 
            resources.ApplyResources(this.labelDiscountsFormatHint, "labelDiscountsFormatHint");
            this.labelDiscountsFormatHint.Name = "labelDiscountsFormatHint";
            this.toolTip1.SetToolTip(this.labelDiscountsFormatHint, resources.GetString("labelDiscountsFormatHint.ToolTip"));
            // 
            // listViewDiscounts
            // 
            resources.ApplyResources(this.listViewDiscounts, "listViewDiscounts");
            this.listViewDiscounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderKey,
            this.columnHeaderDiscount});
            this.listViewDiscounts.GridLines = true;
            this.listViewDiscounts.HideSelection = false;
            this.listViewDiscounts.Name = "listViewDiscounts";
            this.toolTip1.SetToolTip(this.listViewDiscounts, resources.GetString("listViewDiscounts.ToolTip"));
            this.listViewDiscounts.UseCompatibleStateImageBehavior = false;
            this.listViewDiscounts.View = System.Windows.Forms.View.Details;
            this.listViewDiscounts.SelectedIndexChanged += new System.EventHandler(this.ListViewDiscounts_SelectedIndexChanged);
            // 
            // columnHeaderKey
            // 
            resources.ApplyResources(this.columnHeaderKey, "columnHeaderKey");
            // 
            // columnHeaderDiscount
            // 
            resources.ApplyResources(this.columnHeaderDiscount, "columnHeaderDiscount");
            // 
            // textBoxDiscountsDiscount
            // 
            resources.ApplyResources(this.textBoxDiscountsDiscount, "textBoxDiscountsDiscount");
            this.textBoxDiscountsDiscount.Name = "textBoxDiscountsDiscount";
            this.toolTip1.SetToolTip(this.textBoxDiscountsDiscount, resources.GetString("textBoxDiscountsDiscount.ToolTip"));
            // 
            // buttonDiscountsCreateNew
            // 
            resources.ApplyResources(this.buttonDiscountsCreateNew, "buttonDiscountsCreateNew");
            this.buttonDiscountsCreateNew.Name = "buttonDiscountsCreateNew";
            this.toolTip1.SetToolTip(this.buttonDiscountsCreateNew, resources.GetString("buttonDiscountsCreateNew.ToolTip"));
            this.buttonDiscountsCreateNew.UseVisualStyleBackColor = true;
            this.buttonDiscountsCreateNew.Click += new System.EventHandler(this.ButtonDiscountsCreateNew_Click);
            // 
            // labelDiscountsMappingName
            // 
            resources.ApplyResources(this.labelDiscountsMappingName, "labelDiscountsMappingName");
            this.labelDiscountsMappingName.Name = "labelDiscountsMappingName";
            this.toolTip1.SetToolTip(this.labelDiscountsMappingName, resources.GetString("labelDiscountsMappingName.ToolTip"));
            // 
            // labelDiscountsDiscount
            // 
            resources.ApplyResources(this.labelDiscountsDiscount, "labelDiscountsDiscount");
            this.labelDiscountsDiscount.Name = "labelDiscountsDiscount";
            this.toolTip1.SetToolTip(this.labelDiscountsDiscount, resources.GetString("labelDiscountsDiscount.ToolTip"));
            // 
            // buttonDiscountsSave
            // 
            resources.ApplyResources(this.buttonDiscountsSave, "buttonDiscountsSave");
            this.buttonDiscountsSave.Name = "buttonDiscountsSave";
            this.toolTip1.SetToolTip(this.buttonDiscountsSave, resources.GetString("buttonDiscountsSave.ToolTip"));
            this.buttonDiscountsSave.UseVisualStyleBackColor = true;
            this.buttonDiscountsSave.Click += new System.EventHandler(this.ButtonDiscountsSave_Click);
            // 
            // buttonDiscountsDelete
            // 
            resources.ApplyResources(this.buttonDiscountsDelete, "buttonDiscountsDelete");
            this.buttonDiscountsDelete.Name = "buttonDiscountsDelete";
            this.toolTip1.SetToolTip(this.buttonDiscountsDelete, resources.GetString("buttonDiscountsDelete.ToolTip"));
            this.buttonDiscountsDelete.UseVisualStyleBackColor = true;
            this.buttonDiscountsDelete.Click += new System.EventHandler(this.ButtonDiscountsDelete_Click);
            // 
            // tabPageEditMapping
            // 
            resources.ApplyResources(this.tabPageEditMapping, "tabPageEditMapping");
            this.tabPageEditMapping.Controls.Add(this.buttonPairPreview);
            this.tabPageEditMapping.Controls.Add(this.comboBoxPairDictionaries);
            this.tabPageEditMapping.Controls.Add(this.buttonPairsOpenDiscounts);
            this.tabPageEditMapping.Controls.Add(this.comboBoxPairDiscountKey);
            this.tabPageEditMapping.Controls.Add(this.labelPairsFactor);
            this.tabPageEditMapping.Controls.Add(this.textBoxPairsFactor);
            this.tabPageEditMapping.Controls.Add(this.checkBoxPairsOverwrite);
            this.tabPageEditMapping.Controls.Add(this.comboBoxPairSource);
            this.tabPageEditMapping.Controls.Add(this.comboBoxPairType);
            this.tabPageEditMapping.Controls.Add(this.textBoxPairSourceField);
            this.tabPageEditMapping.Controls.Add(this.labelPairsType);
            this.tabPageEditMapping.Controls.Add(this.buttonPairsSave);
            this.tabPageEditMapping.Controls.Add(this.labelPairsSourcefield);
            this.tabPageEditMapping.Controls.Add(this.buttonPairsDelete);
            this.tabPageEditMapping.Controls.Add(this.comboBoxPairTarget);
            this.tabPageEditMapping.Controls.Add(this.labelPairsTargetfield);
            this.tabPageEditMapping.Controls.Add(this.listViewPairs);
            this.tabPageEditMapping.Controls.Add(this.buttonCreateNewPair);
            this.tabPageEditMapping.Controls.Add(this.labelPairsMappingName);
            this.tabPageEditMapping.Name = "tabPageEditMapping";
            this.toolTip1.SetToolTip(this.tabPageEditMapping, resources.GetString("tabPageEditMapping.ToolTip"));
            this.tabPageEditMapping.UseVisualStyleBackColor = true;
            // 
            // buttonPairPreview
            // 
            resources.ApplyResources(this.buttonPairPreview, "buttonPairPreview");
            this.buttonPairPreview.Name = "buttonPairPreview";
            this.toolTip1.SetToolTip(this.buttonPairPreview, resources.GetString("buttonPairPreview.ToolTip"));
            this.buttonPairPreview.UseVisualStyleBackColor = true;
            this.buttonPairPreview.Click += new System.EventHandler(this.ButtonPairPreview_Click);
            // 
            // comboBoxPairDictionaries
            // 
            resources.ApplyResources(this.comboBoxPairDictionaries, "comboBoxPairDictionaries");
            this.comboBoxPairDictionaries.FormattingEnabled = true;
            this.comboBoxPairDictionaries.Name = "comboBoxPairDictionaries";
            this.toolTip1.SetToolTip(this.comboBoxPairDictionaries, resources.GetString("comboBoxPairDictionaries.ToolTip"));
            // 
            // buttonPairsOpenDiscounts
            // 
            resources.ApplyResources(this.buttonPairsOpenDiscounts, "buttonPairsOpenDiscounts");
            this.buttonPairsOpenDiscounts.Name = "buttonPairsOpenDiscounts";
            this.toolTip1.SetToolTip(this.buttonPairsOpenDiscounts, resources.GetString("buttonPairsOpenDiscounts.ToolTip"));
            this.buttonPairsOpenDiscounts.UseVisualStyleBackColor = true;
            this.buttonPairsOpenDiscounts.Click += new System.EventHandler(this.ButtonPairsOpenDiscounts_Click);
            // 
            // comboBoxPairDiscountKey
            // 
            resources.ApplyResources(this.comboBoxPairDiscountKey, "comboBoxPairDiscountKey");
            this.comboBoxPairDiscountKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxPairDiscountKey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPairDiscountKey.FormattingEnabled = true;
            this.comboBoxPairDiscountKey.Name = "comboBoxPairDiscountKey";
            this.toolTip1.SetToolTip(this.comboBoxPairDiscountKey, resources.GetString("comboBoxPairDiscountKey.ToolTip"));
            // 
            // labelPairsFactor
            // 
            resources.ApplyResources(this.labelPairsFactor, "labelPairsFactor");
            this.labelPairsFactor.Name = "labelPairsFactor";
            this.toolTip1.SetToolTip(this.labelPairsFactor, resources.GetString("labelPairsFactor.ToolTip"));
            // 
            // textBoxPairsFactor
            // 
            resources.ApplyResources(this.textBoxPairsFactor, "textBoxPairsFactor");
            this.textBoxPairsFactor.Name = "textBoxPairsFactor";
            this.toolTip1.SetToolTip(this.textBoxPairsFactor, resources.GetString("textBoxPairsFactor.ToolTip"));
            // 
            // checkBoxPairsOverwrite
            // 
            resources.ApplyResources(this.checkBoxPairsOverwrite, "checkBoxPairsOverwrite");
            this.checkBoxPairsOverwrite.Name = "checkBoxPairsOverwrite";
            this.toolTip1.SetToolTip(this.checkBoxPairsOverwrite, resources.GetString("checkBoxPairsOverwrite.ToolTip"));
            this.checkBoxPairsOverwrite.UseVisualStyleBackColor = true;
            // 
            // comboBoxPairSource
            // 
            resources.ApplyResources(this.comboBoxPairSource, "comboBoxPairSource");
            this.comboBoxPairSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxPairSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPairSource.FormattingEnabled = true;
            this.comboBoxPairSource.Name = "comboBoxPairSource";
            this.toolTip1.SetToolTip(this.comboBoxPairSource, resources.GetString("comboBoxPairSource.ToolTip"));
            // 
            // comboBoxPairType
            // 
            resources.ApplyResources(this.comboBoxPairType, "comboBoxPairType");
            this.comboBoxPairType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxPairType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPairType.FormattingEnabled = true;
            this.comboBoxPairType.Items.AddRange(new object[] {
            resources.GetString("comboBoxPairType.Items"),
            resources.GetString("comboBoxPairType.Items1"),
            resources.GetString("comboBoxPairType.Items2"),
            resources.GetString("comboBoxPairType.Items3"),
            resources.GetString("comboBoxPairType.Items4"),
            resources.GetString("comboBoxPairType.Items5"),
            resources.GetString("comboBoxPairType.Items6"),
            resources.GetString("comboBoxPairType.Items7"),
            resources.GetString("comboBoxPairType.Items8")});
            this.comboBoxPairType.Name = "comboBoxPairType";
            this.toolTip1.SetToolTip(this.comboBoxPairType, resources.GetString("comboBoxPairType.ToolTip"));
            this.comboBoxPairType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPairType_SelectedIndexChanged);
            // 
            // textBoxPairSourceField
            // 
            resources.ApplyResources(this.textBoxPairSourceField, "textBoxPairSourceField");
            this.textBoxPairSourceField.Name = "textBoxPairSourceField";
            this.toolTip1.SetToolTip(this.textBoxPairSourceField, resources.GetString("textBoxPairSourceField.ToolTip"));
            // 
            // labelPairsType
            // 
            resources.ApplyResources(this.labelPairsType, "labelPairsType");
            this.labelPairsType.Name = "labelPairsType";
            this.toolTip1.SetToolTip(this.labelPairsType, resources.GetString("labelPairsType.ToolTip"));
            // 
            // buttonPairsSave
            // 
            resources.ApplyResources(this.buttonPairsSave, "buttonPairsSave");
            this.buttonPairsSave.Name = "buttonPairsSave";
            this.toolTip1.SetToolTip(this.buttonPairsSave, resources.GetString("buttonPairsSave.ToolTip"));
            this.buttonPairsSave.UseVisualStyleBackColor = true;
            this.buttonPairsSave.Click += new System.EventHandler(this.ButtonPairSave_Click);
            // 
            // labelPairsSourcefield
            // 
            resources.ApplyResources(this.labelPairsSourcefield, "labelPairsSourcefield");
            this.labelPairsSourcefield.Name = "labelPairsSourcefield";
            this.toolTip1.SetToolTip(this.labelPairsSourcefield, resources.GetString("labelPairsSourcefield.ToolTip"));
            // 
            // buttonPairsDelete
            // 
            resources.ApplyResources(this.buttonPairsDelete, "buttonPairsDelete");
            this.buttonPairsDelete.Name = "buttonPairsDelete";
            this.toolTip1.SetToolTip(this.buttonPairsDelete, resources.GetString("buttonPairsDelete.ToolTip"));
            this.buttonPairsDelete.UseVisualStyleBackColor = true;
            this.buttonPairsDelete.Click += new System.EventHandler(this.ButtonPairDelete_Click);
            // 
            // comboBoxPairTarget
            // 
            resources.ApplyResources(this.comboBoxPairTarget, "comboBoxPairTarget");
            this.comboBoxPairTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxPairTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPairTarget.FormattingEnabled = true;
            this.comboBoxPairTarget.Name = "comboBoxPairTarget";
            this.toolTip1.SetToolTip(this.comboBoxPairTarget, resources.GetString("comboBoxPairTarget.ToolTip"));
            // 
            // labelPairsTargetfield
            // 
            resources.ApplyResources(this.labelPairsTargetfield, "labelPairsTargetfield");
            this.labelPairsTargetfield.Name = "labelPairsTargetfield";
            this.toolTip1.SetToolTip(this.labelPairsTargetfield, resources.GetString("labelPairsTargetfield.ToolTip"));
            // 
            // listViewPairs
            // 
            resources.ApplyResources(this.listViewPairs, "listViewPairs");
            this.listViewPairs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPairsTarget,
            this.columnHeaderPairsType,
            this.columnHeaderPairsSource,
            this.columnHeaderPairsOverwrite});
            this.listViewPairs.GridLines = true;
            this.listViewPairs.HideSelection = false;
            this.listViewPairs.Name = "listViewPairs";
            this.toolTip1.SetToolTip(this.listViewPairs, resources.GetString("listViewPairs.ToolTip"));
            this.listViewPairs.UseCompatibleStateImageBehavior = false;
            this.listViewPairs.View = System.Windows.Forms.View.Details;
            this.listViewPairs.SelectedIndexChanged += new System.EventHandler(this.ListViewPairs_SelectedIndexChanged);
            // 
            // columnHeaderPairsTarget
            // 
            resources.ApplyResources(this.columnHeaderPairsTarget, "columnHeaderPairsTarget");
            // 
            // columnHeaderPairsType
            // 
            resources.ApplyResources(this.columnHeaderPairsType, "columnHeaderPairsType");
            // 
            // columnHeaderPairsSource
            // 
            resources.ApplyResources(this.columnHeaderPairsSource, "columnHeaderPairsSource");
            // 
            // columnHeaderPairsOverwrite
            // 
            resources.ApplyResources(this.columnHeaderPairsOverwrite, "columnHeaderPairsOverwrite");
            // 
            // buttonCreateNewPair
            // 
            resources.ApplyResources(this.buttonCreateNewPair, "buttonCreateNewPair");
            this.buttonCreateNewPair.Name = "buttonCreateNewPair";
            this.toolTip1.SetToolTip(this.buttonCreateNewPair, resources.GetString("buttonCreateNewPair.ToolTip"));
            this.buttonCreateNewPair.UseVisualStyleBackColor = true;
            this.buttonCreateNewPair.Click += new System.EventHandler(this.ButtonPairCreateNew_Click);
            // 
            // labelPairsMappingName
            // 
            resources.ApplyResources(this.labelPairsMappingName, "labelPairsMappingName");
            this.labelPairsMappingName.Name = "labelPairsMappingName";
            this.toolTip1.SetToolTip(this.labelPairsMappingName, resources.GetString("labelPairsMappingName.ToolTip"));
            // 
            // tabPageUpload
            // 
            resources.ApplyResources(this.tabPageUpload, "tabPageUpload");
            this.tabPageUpload.Controls.Add(this.tableLayoutPanel6);
            this.tabPageUpload.Name = "tabPageUpload";
            this.toolTip1.SetToolTip(this.tabPageUpload, resources.GetString("tabPageUpload.ToolTip"));
            this.tabPageUpload.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.labelUploadSelectedMapping, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonVerifyUpload, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.buttonUpload, 0, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.toolTip1.SetToolTip(this.tableLayoutPanel6, resources.GetString("tableLayoutPanel6.ToolTip"));
            // 
            // tableLayoutPanel7
            // 
            resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
            this.tableLayoutPanel7.Controls.Add(this.buttonBrowseUpload, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.textBoxPathUpload, 0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.toolTip1.SetToolTip(this.tableLayoutPanel7, resources.GetString("tableLayoutPanel7.ToolTip"));
            // 
            // buttonBrowseUpload
            // 
            resources.ApplyResources(this.buttonBrowseUpload, "buttonBrowseUpload");
            this.buttonBrowseUpload.Name = "buttonBrowseUpload";
            this.toolTip1.SetToolTip(this.buttonBrowseUpload, resources.GetString("buttonBrowseUpload.ToolTip"));
            this.buttonBrowseUpload.UseVisualStyleBackColor = true;
            this.buttonBrowseUpload.Click += new System.EventHandler(this.ButtonUploadBrowse_Click);
            // 
            // textBoxPathUpload
            // 
            resources.ApplyResources(this.textBoxPathUpload, "textBoxPathUpload");
            this.textBoxPathUpload.Name = "textBoxPathUpload";
            this.toolTip1.SetToolTip(this.textBoxPathUpload, resources.GetString("textBoxPathUpload.ToolTip"));
            // 
            // labelUploadSelectedMapping
            // 
            resources.ApplyResources(this.labelUploadSelectedMapping, "labelUploadSelectedMapping");
            this.labelUploadSelectedMapping.Name = "labelUploadSelectedMapping";
            this.toolTip1.SetToolTip(this.labelUploadSelectedMapping, resources.GetString("labelUploadSelectedMapping.ToolTip"));
            // 
            // buttonVerifyUpload
            // 
            resources.ApplyResources(this.buttonVerifyUpload, "buttonVerifyUpload");
            this.buttonVerifyUpload.Name = "buttonVerifyUpload";
            this.toolTip1.SetToolTip(this.buttonVerifyUpload, resources.GetString("buttonVerifyUpload.ToolTip"));
            this.buttonVerifyUpload.UseVisualStyleBackColor = true;
            this.buttonVerifyUpload.Click += new System.EventHandler(this.ButtonUploadVerify_Click);
            // 
            // buttonUpload
            // 
            resources.ApplyResources(this.buttonUpload, "buttonUpload");
            this.buttonUpload.Name = "buttonUpload";
            this.toolTip1.SetToolTip(this.buttonUpload, resources.GetString("buttonUpload.ToolTip"));
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.ButtonUpload_Click);
            // 
            // tabPageValues
            // 
            resources.ApplyResources(this.tabPageValues, "tabPageValues");
            this.tabPageValues.Controls.Add(this.textBoxValuesValue);
            this.tabPageValues.Controls.Add(this.listViewValues);
            this.tabPageValues.Controls.Add(this.labelValuesTarget);
            this.tabPageValues.Controls.Add(this.buttonValuesSave);
            this.tabPageValues.Controls.Add(this.labelValuesMappingName);
            this.tabPageValues.Controls.Add(this.labelValuesValue);
            this.tabPageValues.Name = "tabPageValues";
            this.toolTip1.SetToolTip(this.tabPageValues, resources.GetString("tabPageValues.ToolTip"));
            this.tabPageValues.UseVisualStyleBackColor = true;
            // 
            // textBoxValuesValue
            // 
            resources.ApplyResources(this.textBoxValuesValue, "textBoxValuesValue");
            this.textBoxValuesValue.Name = "textBoxValuesValue";
            this.toolTip1.SetToolTip(this.textBoxValuesValue, resources.GetString("textBoxValuesValue.ToolTip"));
            // 
            // listViewValues
            // 
            resources.ApplyResources(this.listViewValues, "listViewValues");
            this.listViewValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderValuesName,
            this.columnHeaderValuesValue});
            this.listViewValues.GridLines = true;
            this.listViewValues.HideSelection = false;
            this.listViewValues.Name = "listViewValues";
            this.toolTip1.SetToolTip(this.listViewValues, resources.GetString("listViewValues.ToolTip"));
            this.listViewValues.UseCompatibleStateImageBehavior = false;
            this.listViewValues.View = System.Windows.Forms.View.Details;
            this.listViewValues.SelectedIndexChanged += new System.EventHandler(this.ListViewValues_SelectedIndexChanged);
            // 
            // columnHeaderValuesName
            // 
            resources.ApplyResources(this.columnHeaderValuesName, "columnHeaderValuesName");
            // 
            // columnHeaderValuesValue
            // 
            resources.ApplyResources(this.columnHeaderValuesValue, "columnHeaderValuesValue");
            // 
            // labelValuesTarget
            // 
            resources.ApplyResources(this.labelValuesTarget, "labelValuesTarget");
            this.labelValuesTarget.Name = "labelValuesTarget";
            this.toolTip1.SetToolTip(this.labelValuesTarget, resources.GetString("labelValuesTarget.ToolTip"));
            // 
            // buttonValuesSave
            // 
            resources.ApplyResources(this.buttonValuesSave, "buttonValuesSave");
            this.buttonValuesSave.Name = "buttonValuesSave";
            this.toolTip1.SetToolTip(this.buttonValuesSave, resources.GetString("buttonValuesSave.ToolTip"));
            this.buttonValuesSave.UseVisualStyleBackColor = true;
            this.buttonValuesSave.Click += new System.EventHandler(this.ButtonValuesSave_Click);
            // 
            // labelValuesMappingName
            // 
            resources.ApplyResources(this.labelValuesMappingName, "labelValuesMappingName");
            this.labelValuesMappingName.Name = "labelValuesMappingName";
            this.toolTip1.SetToolTip(this.labelValuesMappingName, resources.GetString("labelValuesMappingName.ToolTip"));
            // 
            // labelValuesValue
            // 
            resources.ApplyResources(this.labelValuesValue, "labelValuesValue");
            this.labelValuesValue.Name = "labelValuesValue";
            this.toolTip1.SetToolTip(this.labelValuesValue, resources.GetString("labelValuesValue.ToolTip"));
            // 
            // tabPageDictionary
            // 
            resources.ApplyResources(this.tabPageDictionary, "tabPageDictionary");
            this.tabPageDictionary.Controls.Add(this.listViewDictionary);
            this.tabPageDictionary.Controls.Add(this.textBoxDictionaryValue);
            this.tabPageDictionary.Controls.Add(this.labelDictionaryMapping);
            this.tabPageDictionary.Controls.Add(this.labelDictionaryKey);
            this.tabPageDictionary.Controls.Add(this.labelDictionaryValue);
            this.tabPageDictionary.Controls.Add(this.buttonDictionarySave);
            this.tabPageDictionary.Controls.Add(this.buttonDictionaryDelete);
            this.tabPageDictionary.Name = "tabPageDictionary";
            this.toolTip1.SetToolTip(this.tabPageDictionary, resources.GetString("tabPageDictionary.ToolTip"));
            this.tabPageDictionary.UseVisualStyleBackColor = true;
            // 
            // listViewDictionary
            // 
            resources.ApplyResources(this.listViewDictionary, "listViewDictionary");
            this.listViewDictionary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listViewDictionary.GridLines = true;
            this.listViewDictionary.HideSelection = false;
            this.listViewDictionary.Name = "listViewDictionary";
            this.toolTip1.SetToolTip(this.listViewDictionary, resources.GetString("listViewDictionary.ToolTip"));
            this.listViewDictionary.UseCompatibleStateImageBehavior = false;
            this.listViewDictionary.View = System.Windows.Forms.View.Details;
            this.listViewDictionary.SelectedIndexChanged += new System.EventHandler(this.ListViewDictionary_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // textBoxDictionaryValue
            // 
            resources.ApplyResources(this.textBoxDictionaryValue, "textBoxDictionaryValue");
            this.textBoxDictionaryValue.Name = "textBoxDictionaryValue";
            this.toolTip1.SetToolTip(this.textBoxDictionaryValue, resources.GetString("textBoxDictionaryValue.ToolTip"));
            // 
            // labelDictionaryMapping
            // 
            resources.ApplyResources(this.labelDictionaryMapping, "labelDictionaryMapping");
            this.labelDictionaryMapping.Name = "labelDictionaryMapping";
            this.toolTip1.SetToolTip(this.labelDictionaryMapping, resources.GetString("labelDictionaryMapping.ToolTip"));
            // 
            // labelDictionaryKey
            // 
            resources.ApplyResources(this.labelDictionaryKey, "labelDictionaryKey");
            this.labelDictionaryKey.Name = "labelDictionaryKey";
            this.toolTip1.SetToolTip(this.labelDictionaryKey, resources.GetString("labelDictionaryKey.ToolTip"));
            // 
            // labelDictionaryValue
            // 
            resources.ApplyResources(this.labelDictionaryValue, "labelDictionaryValue");
            this.labelDictionaryValue.Name = "labelDictionaryValue";
            this.toolTip1.SetToolTip(this.labelDictionaryValue, resources.GetString("labelDictionaryValue.ToolTip"));
            // 
            // buttonDictionarySave
            // 
            resources.ApplyResources(this.buttonDictionarySave, "buttonDictionarySave");
            this.buttonDictionarySave.Name = "buttonDictionarySave";
            this.toolTip1.SetToolTip(this.buttonDictionarySave, resources.GetString("buttonDictionarySave.ToolTip"));
            this.buttonDictionarySave.UseVisualStyleBackColor = true;
            this.buttonDictionarySave.Click += new System.EventHandler(this.ButtonDictionarySave_Click);
            // 
            // buttonDictionaryDelete
            // 
            resources.ApplyResources(this.buttonDictionaryDelete, "buttonDictionaryDelete");
            this.buttonDictionaryDelete.Name = "buttonDictionaryDelete";
            this.toolTip1.SetToolTip(this.buttonDictionaryDelete, resources.GetString("buttonDictionaryDelete.ToolTip"));
            this.buttonDictionaryDelete.UseVisualStyleBackColor = true;
            this.buttonDictionaryDelete.Click += new System.EventHandler(this.ButtonDictionaryDelete_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(159)))));
            this.tableLayoutPanel2.Controls.Add(this.tabControlMainForm, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.toolTip1.SetToolTip(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.ToolTip"));
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Controls.Add(this.buttonNext, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonBack, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // buttonNext
            // 
            resources.ApplyResources(this.buttonNext, "buttonNext");
            this.buttonNext.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonNext.Name = "buttonNext";
            this.toolTip1.SetToolTip(this.buttonNext, resources.GetString("buttonNext.ToolTip"));
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // buttonBack
            // 
            resources.ApplyResources(this.buttonBack, "buttonBack");
            this.buttonBack.Name = "buttonBack";
            this.toolTip1.SetToolTip(this.buttonBack, resources.GetString("buttonBack.ToolTip"));
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.toolStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HelpButton = true;
            this.Name = "MainForm";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControlMainForm.ResumeLayout(false);
            this.tabPageMappings.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageFields.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPageDiscounts.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabPageEditMapping.ResumeLayout(false);
            this.tabPageEditMapping.PerformLayout();
            this.tabPageUpload.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tabPageValues.ResumeLayout(false);
            this.tabPageValues.PerformLayout();
            this.tabPageDictionary.ResumeLayout(false);
            this.tabPageDictionary.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControlMainForm;
        private System.Windows.Forms.TabPage tabPageMappings;
        private System.Windows.Forms.TabPage tabPageFields;
        private System.Windows.Forms.TabPage tabPageUpload;
        private System.Windows.Forms.Button buttonFieldsSave;
        private System.Windows.Forms.ListView listViewFields;
        private System.Windows.Forms.Label labelFieldsDescription;
        private System.Windows.Forms.Button buttonFieldsCreateNew;
        private System.Windows.Forms.Label labelFieldsTargetfield;
        private System.Windows.Forms.Button buttonFieldsDelete;
        private System.Windows.Forms.TextBox textBoxFieldTarget;
        private System.Windows.Forms.TextBox textBoxFieldName;
        private System.Windows.Forms.TextBox textBoxFieldDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsName;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsTarget;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelFieldsType;
        private System.Windows.Forms.ComboBox comboBoxFieldType;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsType;
        private System.Windows.Forms.ToolStripButton toolStripButtonInha;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsSize;
        private System.Windows.Forms.Label labelFieldsSize;
        private System.Windows.Forms.TextBox textBoxFieldSize;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonOptions;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClearDatabase;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSendToRuntime;
        private System.Windows.Forms.TabPage tabPageDiscounts;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFields;
        private System.Windows.Forms.TabPage tabPageEditMapping;
        private System.Windows.Forms.TabPage tabPageValues;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonMappingsEdit;
        private System.Windows.Forms.ListView listViewMappings;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button ButtonMappingsCreateNew;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button buttonBrowseUpload;
        private System.Windows.Forms.TextBox textBoxPathUpload;
        private System.Windows.Forms.Label labelUploadSelectedMapping;
        private System.Windows.Forms.Button buttonVerifyUpload;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Label labelFieldsName;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelDiscountsFormatHint;
        private System.Windows.Forms.ListView listViewDiscounts;
        private System.Windows.Forms.ColumnHeader columnHeaderKey;
        private System.Windows.Forms.ColumnHeader columnHeaderDiscount;
        private System.Windows.Forms.TextBox textBoxDiscountsDiscount;
        private System.Windows.Forms.Button buttonDiscountsCreateNew;
        private System.Windows.Forms.Label labelDiscountsMappingName;
        private System.Windows.Forms.Label labelDiscountsDiscount;
        private System.Windows.Forms.Button buttonDiscountsSave;
        private System.Windows.Forms.Button buttonDiscountsDelete;
        private System.Windows.Forms.Button buttonPairsOpenDiscounts;
        private System.Windows.Forms.ComboBox comboBoxPairDiscountKey;
        private System.Windows.Forms.Label labelPairsFactor;
        private System.Windows.Forms.TextBox textBoxPairsFactor;
        private System.Windows.Forms.CheckBox checkBoxPairsOverwrite;
        private System.Windows.Forms.ComboBox comboBoxPairSource;
        private System.Windows.Forms.ComboBox comboBoxPairType;
        private System.Windows.Forms.TextBox textBoxPairSourceField;
        private System.Windows.Forms.Label labelPairsType;
        private System.Windows.Forms.Button buttonPairsSave;
        private System.Windows.Forms.Label labelPairsSourcefield;
        private System.Windows.Forms.Button buttonPairsDelete;
        private System.Windows.Forms.ComboBox comboBoxPairTarget;
        private System.Windows.Forms.Label labelPairsTargetfield;
        private System.Windows.Forms.ListView listViewPairs;
        private System.Windows.Forms.ColumnHeader columnHeaderPairsTarget;
        private System.Windows.Forms.ColumnHeader columnHeaderPairsType;
        private System.Windows.Forms.ColumnHeader columnHeaderPairsSource;
        private System.Windows.Forms.ColumnHeader columnHeaderPairsOverwrite;
        private System.Windows.Forms.Button buttonCreateNewPair;
        private System.Windows.Forms.Label labelPairsMappingName;
        private System.Windows.Forms.TextBox textBoxValuesValue;
        private System.Windows.Forms.ListView listViewValues;
        private System.Windows.Forms.ColumnHeader columnHeaderValuesName;
        private System.Windows.Forms.ColumnHeader columnHeaderValuesValue;
        private System.Windows.Forms.Label labelValuesTarget;
        private System.Windows.Forms.Button buttonValuesSave;
        private System.Windows.Forms.Label labelValuesMappingName;
        private System.Windows.Forms.Label labelValuesValue;
        private System.Windows.Forms.CheckBox checkboxFieldsDontInsertNull;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldsIsNVL;
        private System.Windows.Forms.TabPage tabPageDictionary;
        private System.Windows.Forms.ListView listViewDictionary;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox textBoxDictionaryValue;
        private System.Windows.Forms.Label labelDictionaryMapping;
        private System.Windows.Forms.Label labelDictionaryKey;
        private System.Windows.Forms.Label labelDictionaryValue;
        private System.Windows.Forms.Button buttonDictionarySave;
        private System.Windows.Forms.Button buttonDictionaryDelete;
        private System.Windows.Forms.ComboBox comboBoxPairDictionaries;
        private System.Windows.Forms.CheckBox checkBoxFieldIsNullable;
        private System.Windows.Forms.Label labelFieldScale;
        private System.Windows.Forms.TextBox textBoxFieldScale;
        private System.Windows.Forms.TextBox textBoxMappingsSearch;
        private System.Windows.Forms.Button buttonPairPreview;
        private System.Windows.Forms.TextBox textBoxDiscountsKey;
        private System.Windows.Forms.ToolStripMenuItem renameArticlesToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

