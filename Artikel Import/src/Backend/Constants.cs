namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Here are all values being stored that are not changing during runtime. ALl values that can
    /// be changed by the user are saved in <see cref="Properties.Settings"/>.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// this is the database password for the oracle database. It's not being save in the <see
        /// cref="Properties.Settings"/> due to the low security of settings
        /// </summary>
        public const string DatabasePassword = "ZtQZJ8SW";

        /// <summary>
        /// When a function throws an error and the real value can't get returned, return this.
        /// Value: ERROR
        /// </summary>
        public const string ErrorValue = "ERROR";

        /// <summary>
        /// When True: ask for database password before sending to runtime. Value: true
        /// </summary>
        public const bool RequirePassword = true;

        /// <summary>
        /// Table in the rtDb where all the articles are being stored. Value: ARTIKEL
        /// </summary>
        public const string TableArtikel = "ARTIKEL";

        /// <summary>
        /// Table in the rtDb where the long text descriptions of articles are being saved. Value: ARTIKELTEXT
        /// </summary>
        public const string TableArtikelBeschreibung = "ARTIKELTEXT";

        /// <summary>
        /// Table in the rtDb where the articles depended on the supplier are being stored (not on
        /// the article number). Value: EINKRABATT
        /// </summary>
        public const string TableEinkauf = "EINKRABATT";

        /// <summary>
        /// Table in the tempDb where articles are being saved and prepared for the TableArtikel.
        /// Value: ARTIKELIMPORT_ARTICLES
        /// </summary>
        public const string TableImportArticles = "ARTIKELIMPORT_ARTICLES";

        /// <summary>
        /// Table where all the <see cref="Objects.CustomDictionary"/> are being stored. Value: ARTIKELIMPORT_DICTIONARY
        /// </summary>
        public const string TableImportDictionary = "ARTIKELIMPORT_DICTIONARY";

        /// <summary>
        /// Table where all the <see cref="Objects.Discount"/> of the <see cref="Objects.Mapping"/>
        /// s are being stored. Value: ARTIKELIMPORT_DISCOUNTS
        /// </summary>
        public const string TableImportDiscounts = "ARTIKELIMPORT_DISCOUNTS";

        /// <summary>
        /// Table in the tempDb where articles depended on the supplier are being stored for the
        /// TableEinkauf. Value: ARTIKELIMPORT_EINKAUF
        /// </summary>
        public const string TableImportEinkauf = "ARTIKELIMPORT_EINKAUF";

        /// <summary>
        /// Table where all the Fields are being stored used for moving articles from the tempDb
        /// into the rtDb. Value: ARTIKELIMPORT_FIELDS
        /// </summary>
        public const string TableImportFields = "ARTIKELIMPORT_FIELDS";

        /// <summary>
        /// Table where all the <see cref="Objects.Pair"/> s are being stored. Value: ARTIKELIMPORT_MAPPINGS
        /// </summary>
        public const string TableImportMappings = "ARTIKELIMPORT_MAPPINGS";

        /// <summary>
        /// Table in the rtDb where prices are being stored. Value: PREISGRUPPEN
        /// </summary>
        public const string TablePreisGruppen = "PREISGRUPPEN";

        /// <summary>
        /// the <see cref="Frontend.MainForm.tabPageDictionary"/> gives the user the option to
        /// choose different price units or units of measure for specific <see cref="Objects.Mapping"/>.
        /// </summary>
        public const string TabNameDictionary = "tabPageDictionary";

        /// <summary>
        /// The <see cref="Frontend.MainForm.tabPageDiscounts"/> gives the user the ability to enter
        /// a specific <see cref="Objects.Discount"/> and shows different discount groups for an mapping.
        /// </summary>
        public const string TabNameDiscounts = "tabPageDiscounts";

        /// <summary>
        /// the <see cref="Frontend.MainForm.tabPageEditMapping"/> shows <see cref="Objects.Pair"/>
        /// of a <see cref="Objects.Mapping"/> and gives the user the ability to edit them.
        /// </summary>
        public const string TabNameEditMapping = "tabPageEditMapping";

        /// <summary>
        /// the <see cref="Frontend.MainForm.tabPageFields"/> shows all <see cref="Objects.Field"/>,
        /// their targets in the runtime database and a short description to the user the fields are
        /// then used from the <see cref="Objects.Mapping"/> the field are also the columns in tables.
        /// </summary>
        public const string TabNameFields = "tabPageFields";

        /// <summary>
        /// the <see cref="Frontend.MainForm.tabPageMappings"/> shows all suppliers, supplier
        /// specific price lists and supplier IDs to the user.
        /// </summary>
        public const string TabNameMappings = "tabPageMappings";

        /// <summary>
        /// the <see cref="Frontend.MainForm.tabPageUpload"/> asks the user to choose the fitting
        /// CSV file and to verify it before its upload.
        /// </summary>
        public const string TabNameUpload = "tabPageUpload";

        /// <summary>
        /// The <see cref="Frontend.MainForm.tabPageValues"/> is opened when a mapping contains a
        /// <see cref="Objects.PairTypes.PairChangingFixedValue"/> and gives the user the ability to
        /// enter values before upload.
        /// </summary>
        public const string TabNameValues = "tabPageValues";

        /// <summary>
        /// When isDebug is true, the test database will be used
        /// </summary>
        public static bool isDebug = false;

        /// <summary>
        /// When there are Preisinfos where the Listenpreis is set to 'a.A.' then the database will contain CheckValueForPreisAufAnfrage as
        /// Value and the Bezeichnung will be concated with " (9 Milliarden = Preis auf Anfrage!)
        /// </summary>
        public static double CheckValueForPreisAufAnfrage = 9e9;

        public static string[] CheckStringForPreisAufAnfrage = { "a.A.", "auf Anfrage", "P.a.A." , ""};

        public static string PreisAufAnfrageBezeichnungStr = "(Preis auf Anfrage!)";


        /// <summary>
        /// Possible endings of filenames when using Excel-Spreadsheets
        /// </summary>
        public static string[] ExcelExtensions = { "xls", "xlsx", "xlsm" };

    }
}