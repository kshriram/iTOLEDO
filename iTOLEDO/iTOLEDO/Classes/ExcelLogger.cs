using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iTOLODO.Classes
{
    public class ExcelLogger
    {
        private static Microsoft.Office.Interop.Excel.Workbook mWorkBook;
        private static Microsoft.Office.Interop.Excel.Sheets mWorkSheets;
        private static Microsoft.Office.Interop.Excel.Worksheet mWSheet1;
        private static Microsoft.Office.Interop.Excel.Application oXL;

      /// <summary>
      /// Excel Writre constructor to save data in excel file.
      /// </summary>
        /// <param name="StringTOWriteInFile">String Of TOLEDO Format</param>
        /// <param name="SavedFlag">Boolean Flag that Shows this string Saved or not</param>
       public ExcelLogger(String StringTOWriteInFile,Boolean SavedFlag)
        {
            try
            {
                ReadExistingExcel(StringTOWriteInFile, SavedFlag);
            }
            catch (Exception)
            { }
                
        }

        /// <summary>
        /// Save the string To the excel File.
        /// </summary>
        /// <param name="ToFile">String Of TOLEDO Format</param>
        /// <param name="SavedFlag">Boolean Flag that Shows this string Saved or not</param>
       public void ReadExistingExcel(String ToFile, Boolean SavedFlag)
       {
           try
           {
               string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\Resources\\iTOLEDO_Loggs.xls";
               oXL = new Microsoft.Office.Interop.Excel.Application();
               // oXL.Visible = true;
               oXL.DisplayAlerts = false;

               try
               {
                   mWorkBook = oXL.Workbooks.Open(path, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
               }
               catch (Exception)
               {
                   //   mWorkBook = oXL.Workbooks.Open(path, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
               }

               //Get all the sheets in the workbook
               mWorkSheets = mWorkBook.Worksheets;
               //Get the allready exists sheet
               mWSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
               Microsoft.Office.Interop.Excel.Range range = mWSheet1.UsedRange;
               int colCount = range.Columns.Count;
               int rowCount = range.Rows.Count;

               ///Add Columns To the Excel file.
               for (int index = 1; index < 2; index++)
               {
                   mWSheet1.Cells[rowCount + index, 1] = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt");
                   mWSheet1.Cells[rowCount + index, 2] = ToFile;
                   mWSheet1.Cells[rowCount + index, 3] = SavedFlag.ToString();
               }

               mWorkBook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
               Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value);
               mWorkBook.Close(Missing.Value, Missing.Value, Missing.Value);
               mWSheet1 = null;
               mWorkBook = null;
               oXL.Quit();
               GC.WaitForPendingFinalizers();
               GC.Collect();
               GC.WaitForPendingFinalizers();
               GC.Collect();
           }
           catch (Exception)
           { }
       }
    }
}
