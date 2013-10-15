using iTOLEDO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTOLEDO.Classes
{
   public class mShipmant : iSaveToDatabase
    {
       public override bool setPackageInfo(Measures measurements)
       {
           RealX3Entity x3ent = new RealX3Entity();
           Boolean _retutn = false;
           try
           {
               String Datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");

               try
               {
                   x3ent.ExecuteStoreCommand("UPDATE [dbo].[shipmentDimensions] set [weight]='" + measurements.BoxWeight + "',[height]='" + measurements.BoxHeight + "',[width]='" + measurements.BoxWidth + "',[lenght]='" + measurements.BoxLength + "',[datecreated]='" + Datetime + "' WHERE [ShipmentNumber]='" + measurements.BOXNUM + "';");
                   _retutn = true;
               }
               catch (Exception)
               {
                   x3ent.ExecuteStoreCommand("INSERT INTO [dbo].[shipmentDimensions]([ShipmentNumber],[weight],[height],[width],[lenght],[datecreated]) VALUES('" + measurements.BOXNUM + "','" + measurements.BoxWeight + "','" + measurements.BoxHeight + "','" + measurements.BoxWidth + "','" + measurements.BoxLength + "','" + Datetime + "');");
                   _retutn = true;
               }
               
              
           }
           catch (Exception ex)
           {
               logFile.Add("Save data Error:", ex.ToString());
           }
           return _retutn;
       }
      
    }
}
