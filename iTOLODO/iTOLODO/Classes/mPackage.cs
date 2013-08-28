
using iTOLODO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTOLEDO.Classes
{
   public static class mPackage
    {
       /// <summary>
       /// Set the Package information to the Database
       /// </summary>
       /// <param name="TMeasures">Measurements came from the machin</param>
       /// <returns>Tru if save successfully else fail</returns>
       public static Boolean setPackageInfo(Measures TMeasures )
       {
           x3v6Entities x3ent = new x3v6Entities();
           Boolean _retutn = false;
           try
           {
               Package _rPackage = x3ent.Packages.SingleOrDefault(i => i.PCKROWID == TMeasures.PCKRowID);
               _rPackage.BoxDimension =TMeasures.BoxDimension;
               _rPackage.BoxLength =TMeasures. BoxLength;
               _rPackage.BoxWidth = TMeasures.BoxWidth;
               _rPackage.BoxHeight = TMeasures.BoxHeight;
               _rPackage.BoxWeight = TMeasures.BoxWeight;
               x3ent.SaveChanges();
               _retutn = true;
           }
           catch (Exception)
           {}
           return _retutn;
       }
    }
}
