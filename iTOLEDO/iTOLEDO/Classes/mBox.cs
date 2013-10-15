

using iTOLEDO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTOLEDO.Classes
{
   public class mBox : iSaveToDatabase
    {
       /// <summary>
       /// Set the Package information to the Database
       /// </summary>
       /// <param name="TMeasures">Measurements came from the machin</param>
       /// <returns>Tru if save successfully else fail</returns>
       public override Boolean setPackageInfo(Measures TMeasures )
       {
           x3v6Entities x3ent = new x3v6Entities();
           Boolean _retutn = false;
           try
           {
               BoxPackage _rPackage = x3ent.BoxPackages.SingleOrDefault(i => i.BOXNUM == TMeasures.BOXNUM);
               _rPackage.BoxMeasurementTime =Convert.ToDateTime( TMeasures.BoxDimension);
               _rPackage.BoxLength =TMeasures. BoxLength;
               _rPackage.BoxWidth = TMeasures.BoxWidth;
               _rPackage.BoxHeight = TMeasures.BoxHeight;
               _rPackage.BoxWeight = TMeasures.BoxWeight;
               x3ent.SaveChanges();
               _retutn = true;
           }
           catch (Exception ex)
           {
               logFile.Add("Save data Error:", ex.ToString());
           }
           return _retutn;
       }
    }
}
