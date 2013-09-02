using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iTOLEDO.Classes
{
  public static class logFile
    {
      /// <summary>
      /// Appand Line to error file
      /// </summary>
      /// <param name="Error">Error Message</param>
      /// <param name="ErrorLocation">Location of Error</param>
      /// <returns>Boolean true if saved in file else false.</returns>
        public static Boolean Add(string Error,String ErrorLocation)
        {
            Boolean _return = false;
            try
            {
                String[] _Line = new string[1];
                _Line[0] = Error+" :- "+ErrorLocation;
                File.AppendAllLines(Environment.CurrentDirectory + "\\Resources\\ErrorLog.sys", _Line);
                _return = true;
            }
            catch(Exception) { }
            return _return;
        }
    }
}
