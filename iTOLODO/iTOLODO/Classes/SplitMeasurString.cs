using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           try
           {
               String TempString = TOLEDOstring.Replace("\r","");
               String[] _charSplit = TempString.Split(new char[] { '=', ';' });
               String[] _DimSplit = _charSplit[7].Split(new char[] { ',' });
               _measures.PCKRowID = _charSplit[17];
               _measures.BoxLength = Convert.ToDouble(_DimSplit[0]);
               _measures.BoxWidth = Convert.ToDouble(_DimSplit[1]);
               _measures.BoxHeight = Convert.ToDouble(_DimSplit[2]);
               _measures.BoxWeight = Convert.ToDouble(_charSplit[13]);
               _measures.BoxDimension =DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss tt");
           }
           catch (Exception)
           {}
           return _measures;
       }
    }
}
