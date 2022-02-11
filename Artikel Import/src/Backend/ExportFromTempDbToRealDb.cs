using Artikel_Import.src.Backend.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Artikel_Import.src.Backend
{
    /// <summary>
    /// Moves articles from the temp database to the real time database using <see cref="Field"/> s.
    /// </summary>
    public class ExportFromTempDbToRealDb
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This moves all articles form the TempDB to the runtime DB using the <see cref="Field"/>
        /// s. If <paramref name="worker"/> or <paramref name="e"/> is null the export will be silent.
        /// </summary>
        /// <param name="worker">schedules the task in the background</param>
        /// <param name="e">Worker arguments</param>
        /// <returns>Report of success</returns>
        /// <exception cref="Exception">if not all commands where executed successfully</exception>
        public static SqlReport Export(BackgroundWorker worker = null, DoWorkEventArgs e = null)
        {
            bool isSilent = true;
            if(worker != null && e != null)
            {
                isSilent = false;
            }
            if(!isSilent)
            {
                if(worker.CancellationPending)
                {
                    e.Cancel = true;
                    return new SqlReport(0, 1, 0);
                }
            }
            log.Info("Export");
            using(var sql = new SQL())
            {

                // TODO: Anpassung: E.Fengler update artikelNr not only by the ean-nr but also by xxxx-nr which are combined the 'Pflegenummer'-nr.               
                sql.ExecuteCommand($"update {Constants.TableImportArticles} aa set aa.artikelnr = nvl(aa.artikelnr, (select a.artikelnr from {Constants.TableArtikel} a where a.ean = aa.ean and a.artikelnr != null and ))");

                //S. Van Laak: update artikelNr, to avoid duplicates
                sql.ExecuteCommand($"update {Constants.TableImportArticles} aa set aa.artikelnr = nvl(aa.artikelnr, (select a.artikelnr from {Constants.TableArtikel} a where a.ean = aa.ean and a.artikelnr != null))");



                //push data into NV DB
                var cmds = new List<string>();



                Field[] fields = Field.LoadFields();
                var aFields = new List<Field>();
                var pgFields = new List<Field>();
                var erFields = new List<Field>();
                //split targets of fields into the different tables
                foreach(Field field in fields)
                {
                    if(field.GetTargetInRuntime().Equals($"{Constants.TableArtikel}.BESCHREIBUNG"))
                        continue;
                    if(field.GetTargetInRuntime().StartsWith($"{Constants.TableArtikel}."))
                        aFields.Add(field);
                    if(field.GetTargetInRuntime().StartsWith($"{Constants.TablePreisGruppen}."))
                        pgFields.Add(field);
                    if(field.GetTargetInRuntime().StartsWith($"{Constants.TableEinkauf}."))
                        erFields.Add(field);
                }
                //remove table fields and convert to array
                Field[] einkRabattFields = erFields.Select(i => new Field(i.GetName(), i.GetTargetInRuntime().Split('.')[1], i.GetDescription(), i.GetSqlType(), i.GetSize(), i.GetScale(), i.IsNVL(), i.IsNullable())).ToArray();
                Field[] artikelFields = aFields.Select(i => new Field(i.GetName(), i.GetTargetInRuntime().Split('.')[1], i.GetDescription(), i.GetSqlType(), i.GetSize(), i.GetScale(), i.IsNVL(), i.IsNullable())).ToArray();
                Field[] preisGruppenFields = pgFields.Select(i => new Field(i.GetName(), i.GetTargetInRuntime().Split('.')[1], i.GetDescription(), i.GetSqlType(), i.GetSize(), i.GetScale(), i.IsNVL(), i.IsNullable())).ToArray();

                log.Info("Generating commands");
                //artikel beschreibung
                cmds.Add(GetExportArtikelTextCmd(fields));
                //artikel
                cmds.Add(GetExportArtikelCmd(artikelFields));
                //remove all price groups where the articles are being found in the temp database
                cmds.Add($"delete from {Constants.TablePreisGruppen} where artikelnr in (select artikelnr from {Constants.TableImportArticles})");
                //preisgruppen ek
                cmds.Add(GetExportPreisGruppenEkCmd(preisGruppenFields));
                //preisgruppen vk
                cmds.Add(GetExportPreisGruppenVkCmd(preisGruppenFields));
                //einkauf rabatt
                string seq_cmd1 = "drop SEQUENCE einkRabattNr_seq";
                cmds.Add(seq_cmd1);
                string seq_cmd2 = "CREATE SEQUENCE einkRabattNr_seq INCREMENT BY 1 START WITH 1 NOCACHE NOCYCLE";
                cmds.Add(seq_cmd2);
                cmds.Add(GetExportEinkRabattCmd(einkRabattFields));
                //deal with old articles
                string[] dataSuppliers = sql.ExecuteQuery($"select DATENLIEFERANT from {Constants.TableImportArticles} group by DATENLIEFERANT");
                foreach(string dataSupplier in dataSuppliers)
                {
                    //set articles to expired when they should have been on this list but weren't
                    cmds.Add($"update {Constants.TableArtikel} set STATUS=3 where DATASUPPLIERID='{dataSupplier}' and AENDERUNGSDATUM < sysdate - interval '1' hour");
                    //set articles to deleted too when they have no entry in the warehouse and no open orders contain the article
                    cmds.Add($"update {Constants.TableArtikel} set GELOESCHT=1 where DATASUPPLIERID='{dataSupplier}' and AENDERUNGSDATUM < sysdate - interval '1' hour and artikelnr in (select artikelnr as Artikel_ohne_Bestand from(select artikelnr, sum(buchbestand) as bestand from LAGER where artikelnr in(select artikelnr from {Constants.TableArtikel} where ZUSTAENDIG = 'torsten.wulff')group by artikelnr)where bestand <= 0 or bestand is null) and not EXISTS(select * from BESTELLPOS b where b.ARTIKELNR=artikelnr and b.STATUS < 4) AND NOT EXISTS(SELECT 1 FROM auftragspos p WHERE p.ARTIKELNR = artikelnr AND p.STATUS < 4)");
                }
                //deal with daily prices and errors in prices
                cmds.Add($"update {Constants.TableArtikel} set MEMO = 'ACHTUNG: Tagespreis' where VK1 < LEK and ZUSTAENDIG = 'torsten.wulff'");
                cmds.Add($"update {Constants.TableArtikel} set MEMO = 'ACHTUNG: Tagespreis' where VK1 = 0.02 and LEK = 0.01 and DATASUPPLIERID=99669 and ZUSTAENDIG = 'torsten.wulff'");
                cmds.Add($"update {Constants.TableArtikel} set VK1 = LEK * 2 where VK1 < LEK and ZUSTAENDIG = 'torsten.wulff'");
                const string bez = "BEZEICHNUNG";
                cmds.Add($"update {Constants.TableArtikel} set {bez} = concat({bez}, '{Constants.PreisAufAnfrageBezeichnungStr}') where VK1 = {Constants.CheckValueForPreisAufAnfrage} and {bez} not like '%{Constants.PreisAufAnfrageBezeichnungStr}'");
                log.Info("Generating commands done");
                //execute all generated commands
                SqlReport report;
                if(isSilent)
                    report = sql.ExecuteCommands(cmds.ToArray());
                else
                    report = sql.ExecuteCommands(worker, e, cmds.ToArray());

                if(report.GetSuccessful() != cmds.Count())
                {
                    throw new Exception("Not all commands have been executed successfully.");
                }
                log.Info("Commands executed");
                //return report;
                return new SqlReport(1, 1, 1);
            }
        }

        private static string GetExportArtikelCmd(Field[] artikelFields)
        {
            UpsertCommand artikel = new UpsertCommand(Constants.TableArtikel);
            artikel.AddKey("x.EAN", "y.EAN");
            artikel.AddArgument("x.SACHBEARBEITERNR", "'ADMIN'");
            artikel.AddArgument("x.AENDERUNGSDATUM", "SYSDATE");
            artikel.AddArgument("x.STATUS", "1");
            artikel.AddArgument("x.KALKBASIS", "0");
            artikel.AddArgument("x.ZUSTAENDIG", "'torsten.wulff'");
            artikel.AddArgument("x.GELOESCHT", "NULL");
            artikel.AddArgumentOnlyInsert("x.ARTIKELNR", "UPPER(y.ARTIKELNR)");
            artikel.AddArgumentOnlyInsert("x.ERFASSUNGSDATUM", "y.EINTRAGUNGSDATUM");
            artikel.AddArgumentOnlyInsert("x.ARTICLEIDOFDATASUPPLIER", "y.PflegeNr");
            foreach(Field field in artikelFields)
            {
                //log.Debug(field.GetTargetInRuntime());
                if(field.GetTargetInRuntime().Equals("ARTIKELNR"))
                {
                    //log.Debug("Skipp");
                    continue;
                }
                if(field.IsNVL())
                    artikel.AddArgumentOnlyInsert("x." + field.GetTargetInRuntime(), "y." + field.GetName());
                artikel.AddArgument("x." + field.GetTargetInRuntime(), "y." + field.GetName());
            }
            return artikel.ToStringUsingSecondTable(Constants.TableImportArticles);
        }

        private static string GetExportArtikelTextCmd(Field[] fields)
        {
            UpsertCommand command = new UpsertCommand(Constants.TableArtikelBeschreibung);
            command.AddKey("x.ARTIKELNR", $"y.{Field.GetFieldByTarget(fields, $"{Constants.TableArtikel}.ARTIKELNR").GetName()}");
            command.AddKey("x.SPRACHE", "1");
            command.AddArgument("x.TEXT", "y." + Field.GetFieldByTarget(fields, $"{Constants.TableArtikel}.BESCHREIBUNG").GetName());
            command.AddArgument("x.BEZEICHNUNG", "y." + Field.GetFieldByTarget(fields, $"{Constants.TableArtikel}.BEZEICHNUNG").GetName());
            return command.ToStringUsingSecondTable(Constants.TableImportArticles);
        }

        private static string GetExportEinkRabattCmd(Field[] einkRabattFields)
        {
            /*
             * This is not the intended way for discount groups. I did not choose that way, because it's only needed if the discounts
             * for each group get changed by users a lot. This is not the case as this program will change the discount values.
             *
             * The intended way for discount groups:
             *  check in PurchDiscountGroupDef for a row with matching PurchDiscountGroupAcronym & SupplierID -> if not found create PurchDiscountGroup
             *  upsert into PurchDiscountGroupAssign using SupplierID & ArticleID and add the PurchDiscountGroup
             *  create a new Group in EinkRabatt -> upsert using LieferantenNr & RabattGruppe=PurchDiscountGroup
             *  add article to EinkRabatt -> SupplierID & ArticleID
             *
             * I choose an alternative way. The discount groups get ignored and the discounts just get added to each article in EinkRabatt.
            */
            UpsertCommand ekRabCmd = new UpsertCommand(Constants.TableEinkauf);
            ekRabCmd.AddKey("x.LIEFERANTENNR", "y." + Field.GetFieldByTarget(einkRabattFields, "LIEFERANTENNR").GetName());
            ekRabCmd.AddKey("x.BESTELLNR", "y." + Field.GetFieldByTarget(einkRabattFields, "BESTELLNR").GetName());
            ekRabCmd.AddArgument("x.SACHBEARBEITERNR", "'ADMIN'");
            ekRabCmd.AddArgument("x.AENDERUNGSDATUM", "SYSDATE");
            ekRabCmd.AddArgument("x.GRABATT2", "NULL");
            ekRabCmd.AddArgument("x.GRABATTEXT2", "NULL");
            ekRabCmd.AddArgumentOnlyInsert("x.ERFASSUNGSDATUM", "SYSDATE");
            ekRabCmd.AddArgumentOnlyInsert("x.WERTSTAFFEL", "'0'");
            ekRabCmd.AddArgumentOnlyInsert("x.KONDITIONSART", "'2'");
            ekRabCmd.AddArgumentOnlyInsert("x.RABATTNR", $"(SELECT MAX( RABATTNR ) FROM {Constants.TableEinkauf}) + seq_nextval_on_demand('einkRabattNr_seq')");
            foreach(Field field in einkRabattFields)
            {
                if(field.IsNVL())
                    ekRabCmd.AddArgumentOnlyInsert("x." + field.GetTargetInRuntime(), "y." + field.GetName());
                ekRabCmd.AddArgument("x." + field.GetTargetInRuntime(), "y." + field.GetName());
            }
            return ekRabCmd.ToStringUsingSecondTable(Constants.TableImportEinkauf);
        }

        private static string GetExportPreisGruppenEkCmd(Field[] preisGruppenFields)
        {
            UpsertCommand pgEkCmd = new UpsertCommand(Constants.TablePreisGruppen);
            pgEkCmd.AddKey("x.ARTIKELNR", $"y.{Field.GetFieldByTarget(preisGruppenFields, "ARTIKELNR").GetName()}");
            pgEkCmd.AddKey("x.GRUPPE", "y." + Field.GetFieldByTarget(preisGruppenFields, "GRUPPEEK").GetName());
            pgEkCmd.AddArgument("x.GUELTIGVON", "y." + Field.GetFieldByTarget(preisGruppenFields, "GUELTIGVON").GetName());
            pgEkCmd.AddArgument("x.GUELTIGBIS", "y." + Field.GetFieldByTarget(preisGruppenFields, "GUELTIGBIS").GetName());
            pgEkCmd.AddArgument("x.PREIS", "y." + Field.GetFieldByTarget(preisGruppenFields, "PREISEK").GetName());
            pgEkCmd.AddArgument("x.CLERKID", "'ADMIN'");
            pgEkCmd.AddArgument("x.ALTERATIONDATE", "SYSDATE");
            pgEkCmd.AddArgumentOnlyInsert("x.ENTRYDATE", "SYSDATE");
            foreach(Field field in preisGruppenFields)
            {
                if(field.GetTargetInRuntime().StartsWith("PREIS"))
                    continue;
                if(field.GetTargetInRuntime().StartsWith("GRUPPE"))
                    continue;

                if(field.IsNVL())
                    pgEkCmd.AddArgumentOnlyInsert("x." + field.GetTargetInRuntime(), "y." + field.GetName());
                pgEkCmd.AddArgument("x." + field.GetTargetInRuntime(), "y." + field.GetName());
            }
            return pgEkCmd.ToStringUsingSecondTable(Constants.TableImportArticles);
        }

        private static string GetExportPreisGruppenVkCmd(Field[] preisGruppenFields)
        {
            UpsertCommand pgVkCmd = new UpsertCommand(Constants.TablePreisGruppen);
            pgVkCmd.AddKey("x.ARTIKELNR", $"y.{Field.GetFieldByTarget(preisGruppenFields, "ARTIKELNR").GetName()}");
            pgVkCmd.AddKey("x.GRUPPE", "y." + Field.GetFieldByTarget(preisGruppenFields, "GRUPPEVK").GetName());
            pgVkCmd.AddArgument("x.GUELTIGVON", "y." + Field.GetFieldByTarget(preisGruppenFields, "GUELTIGVON").GetName());
            pgVkCmd.AddArgument("x.GUELTIGBIS", "y." + Field.GetFieldByTarget(preisGruppenFields, "GUELTIGBIS").GetName());
            pgVkCmd.AddArgument("x.PREIS", "y." + Field.GetFieldByTarget(preisGruppenFields, "PREISVK").GetName());
            pgVkCmd.AddArgument("x.CLERKID", "'ADMIN'");
            pgVkCmd.AddArgument("x.ALTERATIONDATE", "SYSDATE");
            pgVkCmd.AddArgumentOnlyInsert("x.ENTRYDATE", "SYSDATE");
            foreach(Field field in preisGruppenFields)
            {
                if(field.GetTargetInRuntime().StartsWith("PREIS"))
                    continue;
                if(field.GetTargetInRuntime().StartsWith("GRUPPE"))
                    continue;

                if(field.IsNVL())
                    pgVkCmd.AddArgumentOnlyInsert("x." + field.GetTargetInRuntime(), "y." + field.GetName());
                pgVkCmd.AddArgument("x." + field.GetTargetInRuntime(), "y." + field.GetName());
            }
            return pgVkCmd.ToStringUsingSecondTable(Constants.TableImportArticles);
        }
    }
}