using Artikel_Import.src.Backend.Objects;
using log4net;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Artikel_Import.src.Frontend
{
    public partial class PreviewForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initialize a new PreviewForm using a fieldValue dictionary
        /// </summary>
        /// <param name="fieldValuePairs">String Dictionary of field name and value</param>
        public PreviewForm(Dictionary<string, string> fieldValuePairs)
        {
            log.Info("PreviewForm");
            InitializeComponent();
            //set string names for TextBoxes -> must be the names of the field
            Dictionary<string, TextBox> textBoxFieldnameDict = new Dictionary<string, TextBox> { };
            textBoxFieldnameDict.Add("ArtikelNr", ArtikelNr);
            textBoxFieldnameDict.Add("ArtikelGruppe", Artikelgruppe);
            textBoxFieldnameDict.Add("Bezeichnung", Bezeichnung);
            textBoxFieldnameDict.Add("Beschreibung", Beschreibung);
            textBoxFieldnameDict.Add("BestellNr", Bestellnummer);
            textBoxFieldnameDict.Add("BreiteBrutto", BreiteBrutto);
            textBoxFieldnameDict.Add("BreiteNetto", BreiteNetto);
            textBoxFieldnameDict.Add("Code1", Code1);
            textBoxFieldnameDict.Add("Code2", Code2);
            textBoxFieldnameDict.Add("Code3", Code3);
            textBoxFieldnameDict.Add("Dispo", Dispo);
            textBoxFieldnameDict.Add("DatenLieferant", Datenlieferant);
            textBoxFieldnameDict.Add("EAN", EAN);
            textBoxFieldnameDict.Add("EinheitLager", EinheitLager);
            //textBoxFieldnameDict.Add("EinheitVerpackung", EinheitVerp);
            textBoxFieldnameDict.Add("EinheitVerkauf", EinheitVk);
            textBoxFieldnameDict.Add("EK", EK);
            textBoxFieldnameDict.Add("EinheitEinkauf", EinheitEk);
            textBoxFieldnameDict.Add("EkPro", EkPro);
            textBoxFieldnameDict.Add("FaktorVerkauf", FaktorVerkauf);
            textBoxFieldnameDict.Add("FAKTORVL", FaktorEk);
            textBoxFieldnameDict.Add("GEK", GEK);
            textBoxFieldnameDict.Add("GueltigVon", GueltigVon);
            textBoxFieldnameDict.Add("GueltigBis", GueltigBis);
            textBoxFieldnameDict.Add("GewichtBrutto", GewichtBrutto);
            textBoxFieldnameDict.Add("GewichtNetto", GewichtNetto);
            textBoxFieldnameDict.Add("HoeheBrutto", HoeheBrutto);
            textBoxFieldnameDict.Add("HoeheNetto", HoeheNetto);
            textBoxFieldnameDict.Add("LEK", LEK);
            textBoxFieldnameDict.Add("LEK_Alt", LEK_Alt);
            textBoxFieldnameDict.Add("LieferantenNr", Hauptlieferant);
            //textBoxFieldnameDict.Add("MengeVp", MengeVP);
            textBoxFieldnameDict.Add("PflegeNr", PflegeNr);
            textBoxFieldnameDict.Add("PreisGruppeEk", PreisGruppeEk);
            textBoxFieldnameDict.Add("PreisGruppeVk", PreisGruppeVK);
            textBoxFieldnameDict.Add("PG_EkPreis", PGEkPreis);
            textBoxFieldnameDict.Add("PG_VkPreis", PGVkPreis);
            textBoxFieldnameDict.Add("Preisdatum", PreisdatumVk);
            textBoxFieldnameDict.Add("PreisdatumEk", PreisdatumEk);
            textBoxFieldnameDict.Add("RabattGruppe", RabattText);
            textBoxFieldnameDict.Add("Rabatt", Rabatt);
            textBoxFieldnameDict.Add("TiefeBrutto", TiefeBrutto);
            textBoxFieldnameDict.Add("TiefeNetto", TiefeNetto);
            textBoxFieldnameDict.Add("Ursprungsland", Ursprungsland);
            textBoxFieldnameDict.Add("VK1_EDE", VK1);
            textBoxFieldnameDict.Add("VkRabattGruppe", VKRabattGruppe);
            textBoxFieldnameDict.Add("VK2", VK2);
            textBoxFieldnameDict.Add("VK3", VK3);
            textBoxFieldnameDict.Add("VkPro", VKpro);
            textBoxFieldnameDict.Add("ZolltarifNrImport", ZollNrIm);
            textBoxFieldnameDict.Add("ZolltarifNrExport", ZollNrEx);
            //load fields
            Field[] fields = Field.LoadFields();
            //set values given to the TextBox
            for(int i = 0;i < fields.Length;i++)
            {
                string fieldName = fields[i].GetName();
                if(textBoxFieldnameDict.ContainsKey(fieldName))
                {
                    if(fieldValuePairs.ContainsKey(fieldName))
                    {
                        textBoxFieldnameDict[fieldName].Text = fieldValuePairs[fieldName];
                    }
                }
            }
            //set duplicate displayed values
            GueltigBis2.Text = GueltigBis.Text;
            GueltigVon2.Text = GueltigVon.Text;
            ArtikelNr2.Text = ArtikelNr.Text;
            ArtikelNrPG.Text = ArtikelNr.Text;
            ArtikelNrPG2.Text = ArtikelNr.Text;
            ArtikelNrPG3.Text = ArtikelNr.Text;
            BestellNr2.Text = Bestellnummer.Text;
            EAN2.Text = EAN.Text;
            VkProPG.Text = VKpro.Text;
            VkProPG2.Text = VKpro.Text;
            EkPro2.Text = EkPro.Text;
            EinheitVkPG.Text = EinheitVk.Text;
            EinheitVkPG2.Text = EinheitVk.Text;
            Bezeichnung2.Text = Bezeichnung.Text;
            Bezeichnung3.Text = Bezeichnung.Text;
            LieferantenNr.Text = Hauptlieferant.Text;
        }
    }
}