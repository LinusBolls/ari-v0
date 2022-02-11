namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// Previews a row of an CSV using a mapping in a eNVenta simulation
    /// </summary>
    partial class PreviewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ArtikelNr = new System.Windows.Forms.TextBox();
            this.Bezeichnung = new System.Windows.Forms.TextBox();
            this.Bezeichnung2 = new System.Windows.Forms.TextBox();
            this.Beschreibung = new System.Windows.Forms.TextBox();
            this.Artikelgruppe = new System.Windows.Forms.TextBox();
            this.EAN = new System.Windows.Forms.TextBox();
            this.EinheitLager = new System.Windows.Forms.TextBox();
            this.Code1 = new System.Windows.Forms.TextBox();
            this.Code2 = new System.Windows.Forms.TextBox();
            this.Code3 = new System.Windows.Forms.TextBox();
            this.Ursprungsland = new System.Windows.Forms.TextBox();
            this.ZollNrIm = new System.Windows.Forms.TextBox();
            this.ZollNrEx = new System.Windows.Forms.TextBox();
            this.HoeheBrutto = new System.Windows.Forms.TextBox();
            this.HoeheNetto = new System.Windows.Forms.TextBox();
            this.BreiteNetto = new System.Windows.Forms.TextBox();
            this.BreiteBrutto = new System.Windows.Forms.TextBox();
            this.TiefeNetto = new System.Windows.Forms.TextBox();
            this.TiefeBrutto = new System.Windows.Forms.TextBox();
            this.GewichtNetto = new System.Windows.Forms.TextBox();
            this.GewichtBrutto = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Allgemein = new System.Windows.Forms.TabPage();
            this.Verkauf = new System.Windows.Forms.TabPage();
            this.FaktorVerkauf = new System.Windows.Forms.TextBox();
            this.PreisdatumVk = new System.Windows.Forms.TextBox();
            //this.MengeVP = new System.Windows.Forms.TextBox();
            //this.EinheitVerp = new System.Windows.Forms.TextBox();
            this.EinheitVk = new System.Windows.Forms.TextBox();
            this.VKpro = new System.Windows.Forms.TextBox();
            this.VK3 = new System.Windows.Forms.TextBox();
            this.VK2 = new System.Windows.Forms.TextBox();
            this.VK1 = new System.Windows.Forms.TextBox();
            this.Einkauf = new System.Windows.Forms.TabPage();
            this.GEK = new System.Windows.Forms.TextBox();
            this.LEK_Alt = new System.Windows.Forms.TextBox();
            this.PreisdatumEk = new System.Windows.Forms.TextBox();
            this.Dispo = new System.Windows.Forms.TextBox();
            this.Zustaending = new System.Windows.Forms.TextBox();
            this.PflegeNr = new System.Windows.Forms.TextBox();
            this.Datenlieferant = new System.Windows.Forms.TextBox();
            this.HProdNr = new System.Windows.Forms.TextBox();
            this.HerstellerNr = new System.Windows.Forms.TextBox();
            this.Bestellnummer = new System.Windows.Forms.TextBox();
            this.Hauptlieferant = new System.Windows.Forms.TextBox();
            this.EkPro = new System.Windows.Forms.TextBox();
            this.LEK = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Artikel = new System.Windows.Forms.TabPage();
            this.VKRabattGruppe = new System.Windows.Forms.TextBox();
            this.EinkaufRabatt = new System.Windows.Forms.TabPage();
            this.EinheitEk = new System.Windows.Forms.TextBox();
            this.Bezeichnung3 = new System.Windows.Forms.TextBox();
            this.FaktorEk = new System.Windows.Forms.TextBox();
            this.DatumVon = new System.Windows.Forms.TextBox();
            this.DatumBis = new System.Windows.Forms.TextBox();
            this.EAN2 = new System.Windows.Forms.TextBox();
            this.BestellNr2 = new System.Windows.Forms.TextBox();
            this.EkPro2 = new System.Windows.Forms.TextBox();
            this.EK = new System.Windows.Forms.TextBox();
            this.Rabatt = new System.Windows.Forms.TextBox();
            this.RabattText = new System.Windows.Forms.TextBox();
            this.LieferantenNr = new System.Windows.Forms.TextBox();
            this.ArtikelNr2 = new System.Windows.Forms.TextBox();
            this.Preisgruppen = new System.Windows.Forms.TabPage();
            this.EinheitVkPG2 = new System.Windows.Forms.TextBox();
            this.EinheitVkPG = new System.Windows.Forms.TextBox();
            this.VkProPG2 = new System.Windows.Forms.TextBox();
            this.VkProPG = new System.Windows.Forms.TextBox();
            this.GueltigBis2 = new System.Windows.Forms.TextBox();
            this.GueltigVon2 = new System.Windows.Forms.TextBox();
            this.GueltigBis = new System.Windows.Forms.TextBox();
            this.GueltigVon = new System.Windows.Forms.TextBox();
            this.PreisGruppeEk = new System.Windows.Forms.TextBox();
            this.PreisGruppeVK = new System.Windows.Forms.TextBox();
            this.PGEkPreis = new System.Windows.Forms.TextBox();
            this.PGVkPreis = new System.Windows.Forms.TextBox();
            this.ArtikelNrPG3 = new System.Windows.Forms.TextBox();
            this.ArtikelNrPG2 = new System.Windows.Forms.TextBox();
            this.ArtikelNrPG = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.Allgemein.SuspendLayout();
            this.Verkauf.SuspendLayout();
            this.Einkauf.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.Artikel.SuspendLayout();
            this.EinkaufRabatt.SuspendLayout();
            this.Preisgruppen.SuspendLayout();
            this.SuspendLayout();
            // 
            // ArtikelNr
            // 
            this.ArtikelNr.Location = new System.Drawing.Point(100, 84);
            this.ArtikelNr.Name = "ArtikelNr";
            this.ArtikelNr.Size = new System.Drawing.Size(220, 20);
            this.ArtikelNr.TabIndex = 0;
            // 
            // Bezeichnung
            // 
            this.Bezeichnung.Location = new System.Drawing.Point(100, 107);
            this.Bezeichnung.Name = "Bezeichnung";
            this.Bezeichnung.Size = new System.Drawing.Size(220, 20);
            this.Bezeichnung.TabIndex = 1;
            // 
            // Bezeichnung2
            // 
            this.Bezeichnung2.Location = new System.Drawing.Point(100, 129);
            this.Bezeichnung2.Name = "Bezeichnung2";
            this.Bezeichnung2.Size = new System.Drawing.Size(220, 20);
            this.Bezeichnung2.TabIndex = 2;
            // 
            // Beschreibung
            // 
            this.Beschreibung.HideSelection = false;
            this.Beschreibung.Location = new System.Drawing.Point(100, 152);
            this.Beschreibung.Multiline = true;
            this.Beschreibung.Name = "Beschreibung";
            this.Beschreibung.Size = new System.Drawing.Size(220, 61);
            this.Beschreibung.TabIndex = 3;
            // 
            // Artikelgruppe
            // 
            this.Artikelgruppe.Location = new System.Drawing.Point(416, 84);
            this.Artikelgruppe.Name = "Artikelgruppe";
            this.Artikelgruppe.Size = new System.Drawing.Size(159, 20);
            this.Artikelgruppe.TabIndex = 4;
            // 
            // EAN
            // 
            this.EAN.Location = new System.Drawing.Point(416, 128);
            this.EAN.Name = "EAN";
            this.EAN.Size = new System.Drawing.Size(159, 20);
            this.EAN.TabIndex = 5;
            // 
            // EinheitLager
            // 
            this.EinheitLager.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EinheitLager.Location = new System.Drawing.Point(91, 5);
            this.EinheitLager.Name = "EinheitLager";
            this.EinheitLager.Size = new System.Drawing.Size(100, 18);
            this.EinheitLager.TabIndex = 6;
            // 
            // Code1
            // 
            this.Code1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code1.Location = new System.Drawing.Point(406, 5);
            this.Code1.Name = "Code1";
            this.Code1.Size = new System.Drawing.Size(159, 18);
            this.Code1.TabIndex = 7;
            // 
            // Code2
            // 
            this.Code2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code2.Location = new System.Drawing.Point(406, 27);
            this.Code2.Name = "Code2";
            this.Code2.Size = new System.Drawing.Size(159, 18);
            this.Code2.TabIndex = 8;
            // 
            // Code3
            // 
            this.Code3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code3.Location = new System.Drawing.Point(406, 49);
            this.Code3.Name = "Code3";
            this.Code3.Size = new System.Drawing.Size(159, 18);
            this.Code3.TabIndex = 9;
            // 
            // Ursprungsland
            // 
            this.Ursprungsland.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ursprungsland.Location = new System.Drawing.Point(406, 203);
            this.Ursprungsland.Name = "Ursprungsland";
            this.Ursprungsland.Size = new System.Drawing.Size(159, 18);
            this.Ursprungsland.TabIndex = 10;
            // 
            // ZollNrIm
            // 
            this.ZollNrIm.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZollNrIm.Location = new System.Drawing.Point(406, 247);
            this.ZollNrIm.Name = "ZollNrIm";
            this.ZollNrIm.Size = new System.Drawing.Size(100, 18);
            this.ZollNrIm.TabIndex = 11;
            // 
            // ZollNrEx
            // 
            this.ZollNrEx.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZollNrEx.Location = new System.Drawing.Point(406, 269);
            this.ZollNrEx.Name = "ZollNrEx";
            this.ZollNrEx.Size = new System.Drawing.Size(100, 18);
            this.ZollNrEx.TabIndex = 12;
            // 
            // HoeheBrutto
            // 
            this.HoeheBrutto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoeheBrutto.Location = new System.Drawing.Point(91, 114);
            this.HoeheBrutto.Name = "HoeheBrutto";
            this.HoeheBrutto.Size = new System.Drawing.Size(80, 18);
            this.HoeheBrutto.TabIndex = 13;
            // 
            // HoeheNetto
            // 
            this.HoeheNetto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoeheNetto.Location = new System.Drawing.Point(176, 114);
            this.HoeheNetto.Name = "HoeheNetto";
            this.HoeheNetto.Size = new System.Drawing.Size(80, 18);
            this.HoeheNetto.TabIndex = 14;
            // 
            // BreiteNetto
            // 
            this.BreiteNetto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BreiteNetto.Location = new System.Drawing.Point(176, 136);
            this.BreiteNetto.Name = "BreiteNetto";
            this.BreiteNetto.Size = new System.Drawing.Size(80, 18);
            this.BreiteNetto.TabIndex = 16;
            // 
            // BreiteBrutto
            // 
            this.BreiteBrutto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BreiteBrutto.Location = new System.Drawing.Point(91, 136);
            this.BreiteBrutto.Name = "BreiteBrutto";
            this.BreiteBrutto.Size = new System.Drawing.Size(80, 18);
            this.BreiteBrutto.TabIndex = 15;
            // 
            // TiefeNetto
            // 
            this.TiefeNetto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TiefeNetto.Location = new System.Drawing.Point(176, 158);
            this.TiefeNetto.Name = "TiefeNetto";
            this.TiefeNetto.Size = new System.Drawing.Size(80, 18);
            this.TiefeNetto.TabIndex = 18;
            // 
            // TiefeBrutto
            // 
            this.TiefeBrutto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TiefeBrutto.Location = new System.Drawing.Point(91, 158);
            this.TiefeBrutto.Name = "TiefeBrutto";
            this.TiefeBrutto.Size = new System.Drawing.Size(80, 18);
            this.TiefeBrutto.TabIndex = 17;
            // 
            // GewichtNetto
            // 
            this.GewichtNetto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GewichtNetto.Location = new System.Drawing.Point(176, 181);
            this.GewichtNetto.Name = "GewichtNetto";
            this.GewichtNetto.Size = new System.Drawing.Size(80, 18);
            this.GewichtNetto.TabIndex = 20;
            // 
            // GewichtBrutto
            // 
            this.GewichtBrutto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GewichtBrutto.Location = new System.Drawing.Point(91, 181);
            this.GewichtBrutto.Name = "GewichtBrutto";
            this.GewichtBrutto.Size = new System.Drawing.Size(80, 18);
            this.GewichtBrutto.TabIndex = 19;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Allgemein);
            this.tabControl1.Controls.Add(this.Verkauf);
            this.tabControl1.Controls.Add(this.Einkauf);
            this.tabControl1.Location = new System.Drawing.Point(0, 237);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 398);
            this.tabControl1.TabIndex = 21;
            // 
            // Allgemein
            // 
            this.Allgemein.BackgroundImage = global::Artikel_Import.Properties.Resources.Artikel_Allgemein;
            this.Allgemein.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Allgemein.Controls.Add(this.Code1);
            this.Allgemein.Controls.Add(this.GewichtNetto);
            this.Allgemein.Controls.Add(this.EinheitLager);
            this.Allgemein.Controls.Add(this.GewichtBrutto);
            this.Allgemein.Controls.Add(this.Code2);
            this.Allgemein.Controls.Add(this.TiefeNetto);
            this.Allgemein.Controls.Add(this.Code3);
            this.Allgemein.Controls.Add(this.TiefeBrutto);
            this.Allgemein.Controls.Add(this.Ursprungsland);
            this.Allgemein.Controls.Add(this.BreiteNetto);
            this.Allgemein.Controls.Add(this.ZollNrIm);
            this.Allgemein.Controls.Add(this.BreiteBrutto);
            this.Allgemein.Controls.Add(this.ZollNrEx);
            this.Allgemein.Controls.Add(this.HoeheNetto);
            this.Allgemein.Controls.Add(this.HoeheBrutto);
            this.Allgemein.Location = new System.Drawing.Point(4, 22);
            this.Allgemein.Name = "Allgemein";
            this.Allgemein.Padding = new System.Windows.Forms.Padding(3);
            this.Allgemein.Size = new System.Drawing.Size(803, 372);
            this.Allgemein.TabIndex = 0;
            this.Allgemein.Text = "Allegmein";
            this.Allgemein.UseVisualStyleBackColor = true;
            // 
            // Verkauf
            // 
            this.Verkauf.BackgroundImage = global::Artikel_Import.Properties.Resources.Artikel_Verkauf;
            this.Verkauf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Verkauf.Controls.Add(this.FaktorVerkauf);
            this.Verkauf.Controls.Add(this.PreisdatumVk);
            //this.Verkauf.Controls.Add(this.MengeVP);
            //this.Verkauf.Controls.Add(this.EinheitVerp);
            this.Verkauf.Controls.Add(this.EinheitVk);
            this.Verkauf.Controls.Add(this.VKpro);
            this.Verkauf.Controls.Add(this.VK3);
            this.Verkauf.Controls.Add(this.VK2);
            this.Verkauf.Controls.Add(this.VK1);
            this.Verkauf.Location = new System.Drawing.Point(4, 22);
            this.Verkauf.Name = "Verkauf";
            this.Verkauf.Padding = new System.Windows.Forms.Padding(3);
            this.Verkauf.Size = new System.Drawing.Size(803, 372);
            this.Verkauf.TabIndex = 1;
            this.Verkauf.Text = "Verkauf";
            this.Verkauf.UseVisualStyleBackColor = true;
            // 
            // FaktorVerkauf
            // 
            this.FaktorVerkauf.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FaktorVerkauf.Location = new System.Drawing.Point(94, 114);
            this.FaktorVerkauf.Name = "FaktorVerkauf";
            this.FaktorVerkauf.Size = new System.Drawing.Size(60, 18);
            this.FaktorVerkauf.TabIndex = 12;
            // 
            // PreisdatumVk
            // 
            this.PreisdatumVk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreisdatumVk.Location = new System.Drawing.Point(664, 224);
            this.PreisdatumVk.Name = "PreisdatumVk";
            this.PreisdatumVk.Size = new System.Drawing.Size(99, 18);
            this.PreisdatumVk.TabIndex = 11;
            // 
            // MengeVP
            // 
            //this.MengeVP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.MengeVP.Location = new System.Drawing.Point(409, 27);
            //this.MengeVP.Name = "MengeVP";
            //this.MengeVP.Size = new System.Drawing.Size(99, 18);
            //this.MengeVP.TabIndex = 10;
            // 
            // EinheitVerp
            // 
            //this.EinheitVerp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.EinheitVerp.Location = new System.Drawing.Point(410, 4);
            //this.EinheitVerp.Name = "EinheitVerp";
            //this.EinheitVerp.Size = new System.Drawing.Size(99, 18);
            //this.EinheitVerp.TabIndex = 9;
            // 
            // EinheitVk
            // 
            this.EinheitVk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EinheitVk.Location = new System.Drawing.Point(94, 92);
            this.EinheitVk.Name = "EinheitVk";
            this.EinheitVk.Size = new System.Drawing.Size(81, 18);
            this.EinheitVk.TabIndex = 8;
            // 
            // VKpro
            // 
            this.VKpro.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VKpro.Location = new System.Drawing.Point(94, 70);
            this.VKpro.Name = "VKpro";
            this.VKpro.Size = new System.Drawing.Size(60, 18);
            this.VKpro.TabIndex = 6;
            // 
            // VK3
            // 
            this.VK3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VK3.Location = new System.Drawing.Point(94, 48);
            this.VK3.Name = "VK3";
            this.VK3.Size = new System.Drawing.Size(60, 18);
            this.VK3.TabIndex = 5;
            // 
            // VK2
            // 
            this.VK2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VK2.Location = new System.Drawing.Point(94, 26);
            this.VK2.Name = "VK2";
            this.VK2.Size = new System.Drawing.Size(60, 18);
            this.VK2.TabIndex = 4;
            // 
            // VK1
            // 
            this.VK1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VK1.Location = new System.Drawing.Point(94, 4);
            this.VK1.Name = "VK1";
            this.VK1.Size = new System.Drawing.Size(60, 18);
            this.VK1.TabIndex = 0;
            // 
            // Einkauf
            // 
            this.Einkauf.BackgroundImage = global::Artikel_Import.Properties.Resources.Artikel_Einkauf;
            this.Einkauf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Einkauf.Controls.Add(this.GEK);
            this.Einkauf.Controls.Add(this.LEK_Alt);
            this.Einkauf.Controls.Add(this.PreisdatumEk);
            this.Einkauf.Controls.Add(this.Dispo);
            this.Einkauf.Controls.Add(this.Zustaending);
            this.Einkauf.Controls.Add(this.PflegeNr);
            this.Einkauf.Controls.Add(this.Datenlieferant);
            this.Einkauf.Controls.Add(this.HProdNr);
            this.Einkauf.Controls.Add(this.HerstellerNr);
            this.Einkauf.Controls.Add(this.Bestellnummer);
            this.Einkauf.Controls.Add(this.Hauptlieferant);
            this.Einkauf.Controls.Add(this.EkPro);
            this.Einkauf.Controls.Add(this.LEK);
            this.Einkauf.Location = new System.Drawing.Point(4, 22);
            this.Einkauf.Name = "Einkauf";
            this.Einkauf.Size = new System.Drawing.Size(803, 372);
            this.Einkauf.TabIndex = 2;
            this.Einkauf.Text = "Einkauf";
            this.Einkauf.UseVisualStyleBackColor = true;
            // 
            // GEK
            // 
            this.GEK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GEK.Location = new System.Drawing.Point(91, 49);
            this.GEK.Name = "GEK";
            this.GEK.Size = new System.Drawing.Size(98, 18);
            this.GEK.TabIndex = 12;
            // 
            // LEK_Alt
            // 
            this.LEK_Alt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LEK_Alt.Location = new System.Drawing.Point(92, 27);
            this.LEK_Alt.Name = "LEK_Alt";
            this.LEK_Alt.Size = new System.Drawing.Size(98, 18);
            this.LEK_Alt.TabIndex = 11;
            // 
            // PreisdatumEk
            // 
            this.PreisdatumEk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreisdatumEk.Location = new System.Drawing.Point(662, 71);
            this.PreisdatumEk.Name = "PreisdatumEk";
            this.PreisdatumEk.Size = new System.Drawing.Size(100, 18);
            this.PreisdatumEk.TabIndex = 10;
            // 
            // Dispo
            // 
            this.Dispo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dispo.Location = new System.Drawing.Point(407, 5);
            this.Dispo.Name = "Dispo";
            this.Dispo.Size = new System.Drawing.Size(119, 18);
            this.Dispo.TabIndex = 9;
            // 
            // Zustaending
            // 
            this.Zustaending.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Zustaending.Location = new System.Drawing.Point(407, 28);
            this.Zustaending.Name = "Zustaending";
            this.Zustaending.Size = new System.Drawing.Size(119, 18);
            this.Zustaending.TabIndex = 8;
            // 
            // PflegeNr
            // 
            this.PflegeNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PflegeNr.Location = new System.Drawing.Point(408, 291);
            this.PflegeNr.Name = "PflegeNr";
            this.PflegeNr.Size = new System.Drawing.Size(119, 18);
            this.PflegeNr.TabIndex = 7;
            // 
            // Datenlieferant
            // 
            this.Datenlieferant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Datenlieferant.Location = new System.Drawing.Point(407, 270);
            this.Datenlieferant.Name = "Datenlieferant";
            this.Datenlieferant.Size = new System.Drawing.Size(119, 18);
            this.Datenlieferant.TabIndex = 6;
            // 
            // HProdNr
            // 
            this.HProdNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HProdNr.Location = new System.Drawing.Point(92, 268);
            this.HProdNr.Name = "HProdNr";
            this.HProdNr.Size = new System.Drawing.Size(100, 18);
            this.HProdNr.TabIndex = 5;
            // 
            // HerstellerNr
            // 
            this.HerstellerNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HerstellerNr.Location = new System.Drawing.Point(91, 225);
            this.HerstellerNr.Name = "HerstellerNr";
            this.HerstellerNr.Size = new System.Drawing.Size(100, 18);
            this.HerstellerNr.TabIndex = 4;
            // 
            // Bestellnummer
            // 
            this.Bestellnummer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bestellnummer.Location = new System.Drawing.Point(91, 203);
            this.Bestellnummer.Name = "Bestellnummer";
            this.Bestellnummer.Size = new System.Drawing.Size(122, 18);
            this.Bestellnummer.TabIndex = 3;
            // 
            // Hauptlieferant
            // 
            this.Hauptlieferant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hauptlieferant.Location = new System.Drawing.Point(92, 137);
            this.Hauptlieferant.Name = "Hauptlieferant";
            this.Hauptlieferant.Size = new System.Drawing.Size(100, 18);
            this.Hauptlieferant.TabIndex = 2;
            // 
            // EkPro
            // 
            this.EkPro.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EkPro.Location = new System.Drawing.Point(92, 115);
            this.EkPro.Name = "EkPro";
            this.EkPro.Size = new System.Drawing.Size(100, 18);
            this.EkPro.TabIndex = 1;
            // 
            // LEK
            // 
            this.LEK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LEK.Location = new System.Drawing.Point(93, 5);
            this.LEK.Name = "LEK";
            this.LEK.Size = new System.Drawing.Size(98, 18);
            this.LEK.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Artikel);
            this.tabControl.Controls.Add(this.EinkaufRabatt);
            this.tabControl.Controls.Add(this.Preisgruppen);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(828, 664);
            this.tabControl.TabIndex = 22;
            // 
            // Artikel
            // 
            this.Artikel.BackgroundImage = global::Artikel_Import.Properties.Resources.Artikel;
            this.Artikel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Artikel.Controls.Add(this.VKRabattGruppe);
            this.Artikel.Controls.Add(this.tabControl1);
            this.Artikel.Controls.Add(this.EAN);
            this.Artikel.Controls.Add(this.Beschreibung);
            this.Artikel.Controls.Add(this.Artikelgruppe);
            this.Artikel.Controls.Add(this.ArtikelNr);
            this.Artikel.Controls.Add(this.Bezeichnung);
            this.Artikel.Controls.Add(this.Bezeichnung2);
            this.Artikel.Location = new System.Drawing.Point(4, 22);
            this.Artikel.Name = "Artikel";
            this.Artikel.Padding = new System.Windows.Forms.Padding(3);
            this.Artikel.Size = new System.Drawing.Size(820, 638);
            this.Artikel.TabIndex = 0;
            this.Artikel.Text = "Artikel";
            this.Artikel.UseVisualStyleBackColor = true;
            // 
            // VKRabattGruppe
            // 
            this.VKRabattGruppe.Location = new System.Drawing.Point(416, 106);
            this.VKRabattGruppe.Name = "VKRabattGruppe";
            this.VKRabattGruppe.Size = new System.Drawing.Size(159, 20);
            this.VKRabattGruppe.TabIndex = 22;
            // 
            // EinkaufRabatt
            // 
            this.EinkaufRabatt.BackgroundImage = global::Artikel_Import.Properties.Resources.RabattEinkauf;
            this.EinkaufRabatt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.EinkaufRabatt.Controls.Add(this.EinheitEk);
            this.EinkaufRabatt.Controls.Add(this.Bezeichnung3);
            this.EinkaufRabatt.Controls.Add(this.FaktorEk);
            this.EinkaufRabatt.Controls.Add(this.DatumVon);
            this.EinkaufRabatt.Controls.Add(this.DatumBis);
            this.EinkaufRabatt.Controls.Add(this.EAN2);
            this.EinkaufRabatt.Controls.Add(this.BestellNr2);
            this.EinkaufRabatt.Controls.Add(this.EkPro2);
            this.EinkaufRabatt.Controls.Add(this.EK);
            this.EinkaufRabatt.Controls.Add(this.Rabatt);
            this.EinkaufRabatt.Controls.Add(this.RabattText);
            this.EinkaufRabatt.Controls.Add(this.LieferantenNr);
            this.EinkaufRabatt.Controls.Add(this.ArtikelNr2);
            this.EinkaufRabatt.Location = new System.Drawing.Point(4, 22);
            this.EinkaufRabatt.Name = "EinkaufRabatt";
            this.EinkaufRabatt.Padding = new System.Windows.Forms.Padding(3);
            this.EinkaufRabatt.Size = new System.Drawing.Size(820, 638);
            this.EinkaufRabatt.TabIndex = 1;
            this.EinkaufRabatt.Text = "Einkauf Rabatt";
            this.EinkaufRabatt.UseVisualStyleBackColor = true;
            // 
            // EinheitEk
            // 
            this.EinheitEk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EinheitEk.Location = new System.Drawing.Point(416, 152);
            this.EinheitEk.Name = "EinheitEk";
            this.EinheitEk.Size = new System.Drawing.Size(163, 18);
            this.EinheitEk.TabIndex = 12;
            // 
            // Bezeichnung3
            // 
            this.Bezeichnung3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bezeichnung3.Location = new System.Drawing.Point(101, 174);
            this.Bezeichnung3.Name = "Bezeichnung3";
            this.Bezeichnung3.Size = new System.Drawing.Size(178, 18);
            this.Bezeichnung3.TabIndex = 11;
            // 
            // FaktorEk
            // 
            this.FaktorEk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FaktorEk.Location = new System.Drawing.Point(101, 440);
            this.FaktorEk.Name = "FaktorEk";
            this.FaktorEk.Size = new System.Drawing.Size(101, 18);
            this.FaktorEk.TabIndex = 10;
            // 
            // DatumVon
            // 
            this.DatumVon.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatumVon.Location = new System.Drawing.Point(101, 330);
            this.DatumVon.Name = "DatumVon";
            this.DatumVon.Size = new System.Drawing.Size(101, 18);
            this.DatumVon.TabIndex = 9;
            // 
            // DatumBis
            // 
            this.DatumBis.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatumBis.Location = new System.Drawing.Point(101, 352);
            this.DatumBis.Name = "DatumBis";
            this.DatumBis.Size = new System.Drawing.Size(101, 18);
            this.DatumBis.TabIndex = 8;
            // 
            // EAN2
            // 
            this.EAN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EAN2.Location = new System.Drawing.Point(670, 484);
            this.EAN2.Name = "EAN2";
            this.EAN2.Size = new System.Drawing.Size(101, 18);
            this.EAN2.TabIndex = 7;
            // 
            // BestellNr2
            // 
            this.BestellNr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BestellNr2.Location = new System.Drawing.Point(417, 463);
            this.BestellNr2.Name = "BestellNr2";
            this.BestellNr2.Size = new System.Drawing.Size(80, 18);
            this.BestellNr2.TabIndex = 6;
            // 
            // EkPro2
            // 
            this.EkPro2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EkPro2.Location = new System.Drawing.Point(101, 418);
            this.EkPro2.Name = "EkPro2";
            this.EkPro2.Size = new System.Drawing.Size(101, 18);
            this.EkPro2.TabIndex = 5;
            // 
            // EK
            // 
            this.EK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EK.Location = new System.Drawing.Point(101, 308);
            this.EK.Name = "EK";
            this.EK.Size = new System.Drawing.Size(101, 18);
            this.EK.TabIndex = 4;
            // 
            // Rabatt
            // 
            this.Rabatt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rabatt.Location = new System.Drawing.Point(206, 264);
            this.Rabatt.Name = "Rabatt";
            this.Rabatt.Size = new System.Drawing.Size(73, 18);
            this.Rabatt.TabIndex = 3;
            // 
            // RabattText
            // 
            this.RabattText.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RabattText.Location = new System.Drawing.Point(101, 264);
            this.RabattText.Name = "RabattText";
            this.RabattText.Size = new System.Drawing.Size(101, 18);
            this.RabattText.TabIndex = 2;
            // 
            // LieferantenNr
            // 
            this.LieferantenNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LieferantenNr.Location = new System.Drawing.Point(101, 108);
            this.LieferantenNr.Name = "LieferantenNr";
            this.LieferantenNr.Size = new System.Drawing.Size(160, 18);
            this.LieferantenNr.TabIndex = 1;
            // 
            // ArtikelNr2
            // 
            this.ArtikelNr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArtikelNr2.Location = new System.Drawing.Point(101, 152);
            this.ArtikelNr2.Name = "ArtikelNr2";
            this.ArtikelNr2.Size = new System.Drawing.Size(160, 18);
            this.ArtikelNr2.TabIndex = 0;
            // 
            // Preisgruppen
            // 
            this.Preisgruppen.BackgroundImage = global::Artikel_Import.Properties.Resources.Preisgruppen;
            this.Preisgruppen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Preisgruppen.Controls.Add(this.EinheitVkPG2);
            this.Preisgruppen.Controls.Add(this.EinheitVkPG);
            this.Preisgruppen.Controls.Add(this.VkProPG2);
            this.Preisgruppen.Controls.Add(this.VkProPG);
            this.Preisgruppen.Controls.Add(this.GueltigBis2);
            this.Preisgruppen.Controls.Add(this.GueltigVon2);
            this.Preisgruppen.Controls.Add(this.GueltigBis);
            this.Preisgruppen.Controls.Add(this.GueltigVon);
            this.Preisgruppen.Controls.Add(this.PreisGruppeEk);
            this.Preisgruppen.Controls.Add(this.PreisGruppeVK);
            this.Preisgruppen.Controls.Add(this.PGEkPreis);
            this.Preisgruppen.Controls.Add(this.PGVkPreis);
            this.Preisgruppen.Controls.Add(this.ArtikelNrPG3);
            this.Preisgruppen.Controls.Add(this.ArtikelNrPG2);
            this.Preisgruppen.Controls.Add(this.ArtikelNrPG);
            this.Preisgruppen.Location = new System.Drawing.Point(4, 22);
            this.Preisgruppen.Name = "Preisgruppen";
            this.Preisgruppen.Padding = new System.Windows.Forms.Padding(3);
            this.Preisgruppen.Size = new System.Drawing.Size(820, 638);
            this.Preisgruppen.TabIndex = 2;
            this.Preisgruppen.Text = "Preisgruppen";
            this.Preisgruppen.UseVisualStyleBackColor = true;
            // 
            // EinheitVkPG2
            // 
            this.EinheitVkPG2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EinheitVkPG2.Location = new System.Drawing.Point(614, 187);
            this.EinheitVkPG2.Name = "EinheitVkPG2";
            this.EinheitVkPG2.Size = new System.Drawing.Size(59, 18);
            this.EinheitVkPG2.TabIndex = 14;
            // 
            // EinheitVkPG
            // 
            this.EinheitVkPG.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EinheitVkPG.Location = new System.Drawing.Point(614, 171);
            this.EinheitVkPG.Name = "EinheitVkPG";
            this.EinheitVkPG.Size = new System.Drawing.Size(59, 18);
            this.EinheitVkPG.TabIndex = 13;
            // 
            // VkProPG2
            // 
            this.VkProPG2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VkProPG2.Location = new System.Drawing.Point(554, 188);
            this.VkProPG2.Name = "VkProPG2";
            this.VkProPG2.Size = new System.Drawing.Size(59, 18);
            this.VkProPG2.TabIndex = 12;
            // 
            // VkProPG
            // 
            this.VkProPG.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VkProPG.Location = new System.Drawing.Point(554, 171);
            this.VkProPG.Name = "VkProPG";
            this.VkProPG.Size = new System.Drawing.Size(59, 18);
            this.VkProPG.TabIndex = 11;
            // 
            // GueltigBis2
            // 
            this.GueltigBis2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GueltigBis2.Location = new System.Drawing.Point(744, 189);
            this.GueltigBis2.Name = "GueltigBis2";
            this.GueltigBis2.Size = new System.Drawing.Size(59, 18);
            this.GueltigBis2.TabIndex = 10;
            // 
            // GueltigVon2
            // 
            this.GueltigVon2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GueltigVon2.Location = new System.Drawing.Point(682, 189);
            this.GueltigVon2.Name = "GueltigVon2";
            this.GueltigVon2.Size = new System.Drawing.Size(59, 18);
            this.GueltigVon2.TabIndex = 9;
            // 
            // GueltigBis
            // 
            this.GueltigBis.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GueltigBis.Location = new System.Drawing.Point(744, 171);
            this.GueltigBis.Name = "GueltigBis";
            this.GueltigBis.Size = new System.Drawing.Size(59, 18);
            this.GueltigBis.TabIndex = 8;
            // 
            // GueltigVon
            // 
            this.GueltigVon.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GueltigVon.Location = new System.Drawing.Point(682, 171);
            this.GueltigVon.Name = "GueltigVon";
            this.GueltigVon.Size = new System.Drawing.Size(59, 18);
            this.GueltigVon.TabIndex = 7;
            // 
            // PreisGruppeEk
            // 
            this.PreisGruppeEk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreisGruppeEk.Location = new System.Drawing.Point(40, 187);
            this.PreisGruppeEk.Name = "PreisGruppeEk";
            this.PreisGruppeEk.Size = new System.Drawing.Size(59, 18);
            this.PreisGruppeEk.TabIndex = 6;
            // 
            // PreisGruppeVK
            // 
            this.PreisGruppeVK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreisGruppeVK.Location = new System.Drawing.Point(40, 171);
            this.PreisGruppeVK.Name = "PreisGruppeVK";
            this.PreisGruppeVK.Size = new System.Drawing.Size(59, 18);
            this.PreisGruppeVK.TabIndex = 5;
            // 
            // PGEkPreis
            // 
            this.PGEkPreis.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PGEkPreis.Location = new System.Drawing.Point(441, 187);
            this.PGEkPreis.Name = "PGEkPreis";
            this.PGEkPreis.Size = new System.Drawing.Size(59, 18);
            this.PGEkPreis.TabIndex = 4;
            // 
            // PGVkPreis
            // 
            this.PGVkPreis.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PGVkPreis.Location = new System.Drawing.Point(441, 171);
            this.PGVkPreis.Name = "PGVkPreis";
            this.PGVkPreis.Size = new System.Drawing.Size(59, 18);
            this.PGVkPreis.TabIndex = 3;
            // 
            // ArtikelNrPG3
            // 
            this.ArtikelNrPG3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArtikelNrPG3.Location = new System.Drawing.Point(99, 187);
            this.ArtikelNrPG3.Name = "ArtikelNrPG3";
            this.ArtikelNrPG3.Size = new System.Drawing.Size(89, 18);
            this.ArtikelNrPG3.TabIndex = 2;
            // 
            // ArtikelNrPG2
            // 
            this.ArtikelNrPG2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArtikelNrPG2.Location = new System.Drawing.Point(99, 171);
            this.ArtikelNrPG2.Name = "ArtikelNrPG2";
            this.ArtikelNrPG2.Size = new System.Drawing.Size(89, 18);
            this.ArtikelNrPG2.TabIndex = 1;
            // 
            // ArtikelNrPG
            // 
            this.ArtikelNrPG.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArtikelNrPG.Location = new System.Drawing.Point(101, 87);
            this.ArtikelNrPG.Name = "ArtikelNrPG";
            this.ArtikelNrPG.Size = new System.Drawing.Size(100, 18);
            this.ArtikelNrPG.TabIndex = 0;
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(847, 688);
            this.Controls.Add(this.tabControl);
            this.Name = "PreviewForm";
            this.Text = "PreviewForm";
            this.tabControl1.ResumeLayout(false);
            this.Allgemein.ResumeLayout(false);
            this.Allgemein.PerformLayout();
            this.Verkauf.ResumeLayout(false);
            this.Verkauf.PerformLayout();
            this.Einkauf.ResumeLayout(false);
            this.Einkauf.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.Artikel.ResumeLayout(false);
            this.Artikel.PerformLayout();
            this.EinkaufRabatt.ResumeLayout(false);
            this.EinkaufRabatt.PerformLayout();
            this.Preisgruppen.ResumeLayout(false);
            this.Preisgruppen.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ArtikelNr;
        private System.Windows.Forms.TextBox Bezeichnung;
        private System.Windows.Forms.TextBox Bezeichnung2;
        private System.Windows.Forms.TextBox Beschreibung;
        private System.Windows.Forms.TextBox Artikelgruppe;
        private System.Windows.Forms.TextBox EAN;
        private System.Windows.Forms.TextBox EinheitLager;
        private System.Windows.Forms.TextBox Code1;
        private System.Windows.Forms.TextBox Code2;
        private System.Windows.Forms.TextBox Code3;
        private System.Windows.Forms.TextBox Ursprungsland;
        private System.Windows.Forms.TextBox ZollNrIm;
        private System.Windows.Forms.TextBox ZollNrEx;
        private System.Windows.Forms.TextBox HoeheBrutto;
        private System.Windows.Forms.TextBox HoeheNetto;
        private System.Windows.Forms.TextBox BreiteNetto;
        private System.Windows.Forms.TextBox BreiteBrutto;
        private System.Windows.Forms.TextBox TiefeNetto;
        private System.Windows.Forms.TextBox TiefeBrutto;
        private System.Windows.Forms.TextBox GewichtNetto;
        private System.Windows.Forms.TextBox GewichtBrutto;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Allgemein;
        private System.Windows.Forms.TabPage Verkauf;
        // private System.Windows.Forms.TextBox MengeVP;
        // private System.Windows.Forms.TextBox EinheitVerp;
        private System.Windows.Forms.TextBox EinheitVk;
        private System.Windows.Forms.TextBox VKpro;
        private System.Windows.Forms.TextBox VK3;
        private System.Windows.Forms.TextBox VK2;
        private System.Windows.Forms.TextBox VK1;
        private System.Windows.Forms.TabPage Einkauf;
        private System.Windows.Forms.TextBox LEK;
        private System.Windows.Forms.TextBox Hauptlieferant;
        private System.Windows.Forms.TextBox EkPro;
        private System.Windows.Forms.TextBox Dispo;
        private System.Windows.Forms.TextBox Zustaending;
        private System.Windows.Forms.TextBox PflegeNr;
        private System.Windows.Forms.TextBox Datenlieferant;
        private System.Windows.Forms.TextBox HProdNr;
        private System.Windows.Forms.TextBox HerstellerNr;
        private System.Windows.Forms.TextBox Bestellnummer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Artikel;
        private System.Windows.Forms.TabPage EinkaufRabatt;
        private System.Windows.Forms.TextBox ArtikelNr2;
        private System.Windows.Forms.TextBox EinheitEk;
        private System.Windows.Forms.TextBox Bezeichnung3;
        private System.Windows.Forms.TextBox FaktorEk;
        private System.Windows.Forms.TextBox DatumVon;
        private System.Windows.Forms.TextBox DatumBis;
        private System.Windows.Forms.TextBox EAN2;
        private System.Windows.Forms.TextBox BestellNr2;
        private System.Windows.Forms.TextBox EkPro2;
        private System.Windows.Forms.TextBox EK;
        private System.Windows.Forms.TextBox Rabatt;
        private System.Windows.Forms.TextBox RabattText;
        private System.Windows.Forms.TextBox LieferantenNr;
        private System.Windows.Forms.TabPage Preisgruppen;
        private System.Windows.Forms.TextBox PGEkPreis;
        private System.Windows.Forms.TextBox PGVkPreis;
        private System.Windows.Forms.TextBox ArtikelNrPG3;
        private System.Windows.Forms.TextBox ArtikelNrPG2;
        private System.Windows.Forms.TextBox ArtikelNrPG;
        private System.Windows.Forms.TextBox VKRabattGruppe;
        private System.Windows.Forms.TextBox PreisGruppeEk;
        private System.Windows.Forms.TextBox PreisGruppeVK;
        private System.Windows.Forms.TextBox PreisdatumVk;
        private System.Windows.Forms.TextBox PreisdatumEk;
        private System.Windows.Forms.TextBox GueltigBis2;
        private System.Windows.Forms.TextBox GueltigVon2;
        private System.Windows.Forms.TextBox GueltigBis;
        private System.Windows.Forms.TextBox GueltigVon;
        private System.Windows.Forms.TextBox LEK_Alt;
        private System.Windows.Forms.TextBox GEK;
        private System.Windows.Forms.TextBox FaktorVerkauf;
        private System.Windows.Forms.TextBox VkProPG2;
        private System.Windows.Forms.TextBox VkProPG;
        private System.Windows.Forms.TextBox EinheitVkPG2;
        private System.Windows.Forms.TextBox EinheitVkPG;
    }
}