using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace iTOLEDO.Classes
{
   public static class SplitMeasurString
    {

       /// <summary>
       /// Split the string to the measurement object format.
       /// </summary>
       /// <param name="TOLEDOstring">String in TOLEDO Format.</param>
       /// <returns>Object of measurement </returns>
       public static Measures SplitTOLEDOstring(this string TOLEDOstring)
       {
           Measures _measures = new Measures();
           //Log
           logFile.Add("SplitTOLEDOstring(0) Function Call start", "SplitTOLEDOstring(0)");
           try
           {

              // string[] Tstring = Regex.Split(TOLEDOstring, @"(=)");
               String TempString = TOLEDOstring.Replace("\t","");//Last Chanractor Removed.
               String[] _DimSplit=null;
               String[] _charSplit = TempString.Split(new char[] { '=', ';', '¦', '|', '?', '♥', '☻', '♥' });//Split string from multiple charactors.
               
               //Set always box number is scanning
               Global.IsBoxNumber = true;
               for (int i = 0; i < _charSplit.Count()-1; i++)
               {
                   if (_charSplit[i].Contains("DIM"))
                   {
                       _DimSplit = _charSplit[i + 1].Split(new char[] { ',' });//7th charactor is Dimentions again split in to Height, length, Width
                   }
                   if (_charSplit[i].Contains("B"))
                   {
                       try
                       {
                           if (_charSplit[i + 1].Contains("BOX"))
                           {

                               _measures.BOXNUM = _charSplit[i + 1].Substring(0, 11);
                           }
                           else
                           {
                               //Set Box Number not scanned.
                               Global.IsBoxNumber = false;
                               _measures.BOXNUM = _charSplit[i + 1];
                           }

                       }
                       catch (Exception)
                       {
                           logFile.Add("Incorrect Packing ID", _charSplit[i+1]);
                       }
                       
                   }
                   if (_charSplit[i].Contains("WT"))
                   {
                       _measures.BoxWeight = Convert.ToDouble(_charSplit[i + 1]);
                   }
               }
               // = _charSplit[17].Substring(0,11);//17th Charactor is Packing number that is PCKROWID from Package Table.
               _measures.BoxLength = Convert.ToDouble(_DimSplit[0]);
               _measures.BoxWidth = Convert.ToDouble(_DimSplit[1]);
               _measures.BoxHeight = Convert.ToDouble(_DimSplit[2]);
              // _measures.BoxWeight = Convert.ToDouble(_charSplit[13]);
               _measures.BoxDimension =DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss tt");
           }
           catch (Exception Ex)
           {//Log
               logFile.Add("SplitTOLEDOstring (1) Function Call start", Ex.Message);
           }
           return _measures;
       }
    }
}
