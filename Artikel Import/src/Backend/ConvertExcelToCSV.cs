using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using log4net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artikel_Import.src.Backend
{
    public static class ConvertExcelToCSV
    {


        //internal static void ConvertExcelToCsv2(string excelFilePath, string csvOutputFile, ILog logger, int worksheetNumber = 1)
        //{
        //    if (!File.Exists(excelFilePath)) throw new FileNotFoundException(excelFilePath);
        //    //if (File.Exists(csvOutputFile)) throw new ArgumentException("File exists: " + csvOutputFile);
        //    logger.Info("Start converting Excel File '"+excelFilePath+"' to CSV");

        //    // connection string
        //    var cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"", excelFilePath);
        //    var cnn = new OleDbConnection(cnnStr);

        //    // get schema, then data
        //    var dt = new DataTable();
        //    try
        //    {
        //        cnn.Open();
        //        var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        if (schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
        //        string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
        //        string sql = String.Format("select * from [{0}]", worksheet);
        //        var da = new OleDbDataAdapter(sql, cnn);
        //        da.Fill(dt);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($@"Die Excel-Datei '{excelFilePath} konnte nicht verarbeitet werden. Bitte Konvertieren Sie die Excel-Datei selber in 
        //                               eine CSV-Datei und starten sie den Artikel-Import neu mit der selbst konvertierten Datei. Fehlermeldung: '{e}'");
        //        logger.Error("Could not convert Excel file, got Exception: " + e.ToString());
        //        //throw e;
        //    }
        //    finally
        //    {
        //        // free resources
        //        logger.Info("loaded Excel file, in order to convert to CSV: " + csvOutputFile);
        //        cnn.Close();
        //    }

        //    // write out CSV data
        //    using (var wtr = new StreamWriter(csvOutputFile))
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            bool firstLine = true;
        //            foreach (DataColumn col in dt.Columns)
        //            {
        //                if (!firstLine) { wtr.Write("\t"); } else { firstLine = false; }
        //                var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
        //                wtr.Write(String.Format("\"{0}\"", data));
        //            }
        //            wtr.WriteLine();
        //        }
        //    }
        //    logger.Info("Converted Excel file sucessfully to CSV: " + csvOutputFile);
        //}


        public static void ConvertExcelToCsv(string excelFilePath, string csvOutputFile, ILog logger, int worksheetNumber = 1)
        {
            if (!File.Exists(excelFilePath)) throw new FileNotFoundException(excelFilePath);
            logger.Info("Start converting Excel File '" + excelFilePath + "' to CSV");

            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application(); // .ApplicationClass();
                Microsoft.Office.Interop.Excel.Workbook wbWorkbook = app.Workbooks.Open(excelFilePath); // Type.Missing);
                Microsoft.Office.Interop.Excel.Sheets wsSheet = wbWorkbook.Worksheets;
                Microsoft.Office.Interop.Excel.Worksheet CurSheet = (Microsoft.Office.Interop.Excel.Worksheet)wsSheet[worksheetNumber];


                if (File.Exists(csvOutputFile))
                {
                    File.Delete(csvOutputFile);
                    logger.Info("found old existing temporary csv-file and deleted it");
                }

                wbWorkbook.SaveAs(csvOutputFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV);  //, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                wbWorkbook.Close(false, "", true);
                logger.Info("Converted Excel file sucessfully to temporary CSV: " + csvOutputFile);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Die Excel-Datei '{excelFilePath} konnte nicht verarbeitet werden. Bitte Konvertieren Sie die Excel-Datei selber in 
                                       eine CSV-Datei und starten sie den Artikel-Import neu mit der selbst konvertierten Datei. Fehlermeldung: '{e}'");
                logger.Error("Could not convert Excel file, got Exception: " + e.ToString());
                //throw e;
            }
        }



    }
}
