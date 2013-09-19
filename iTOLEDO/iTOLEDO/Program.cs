using iTOLEDO.Classes;
using iTOLODO.Classes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iTOLEDO
{
    class Program
    {

        static bool _continue;
        static SerialPort _serialPort;
        static Measures _measures;
        static String stringFromTOLEDO = "";

        public static void Main()
        {
            try
            {

                StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
                Thread readThread = new Thread(Read);

                // Create a new SerialPort object with default settings.
                _serialPort = new SerialPort();

                //assign from the Setting File.
                _serialPort.PortName = iTOLEDO.Properties.Settings.Default.PortName.ToString();
                _serialPort.BaudRate = (int)iTOLEDO.Properties.Settings.Default.BaudRate;
                _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), iTOLEDO.Properties.Settings.Default.Parity);
                _serialPort.DataBits = (int)iTOLEDO.Properties.Settings.Default.DataBit;
                _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), iTOLEDO.Properties.Settings.Default.StopBit);
                _serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), iTOLEDO.Properties.Settings.Default.Handshak);
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;
                _serialPort.Open();
                _continue = true;

                //Add to Log
                logFile.Add("Port Opened" + iTOLEDO.Properties.Settings.Default.PortName.ToString(), "Main ()");

                Console.WriteLine("Application Connected to " + iTOLEDO.Properties.Settings.Default.PortName.ToString() + " Port" + Environment.NewLine + "Waiting for data in Buffer.");
                readThread.Start();
                readThread.Join();
                _serialPort.Close();

            }
            catch (Exception ex)
            {
                //Add to Log
                logFile.Add("Port Opening Error", ex.ToString());
                Console.WriteLine("Opning COM port Error. Device is under use of another application. Or Check the Application settings.");
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Read line from the port
        /// </summary>
        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    try
                    {
                        logFile.Add("--", "****--");
                        string message = _serialPort.ReadLine();   
                        //Add to Log
                        logFile.Add("*Reading Data0 ", message);
                        if (message == "")
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            Console.WriteLine(Environment.NewLine + "DATA := " + message);
                            logFile.Add("**Data Received: ", message);
                            stringFromTOLEDO = message;
                            Program _prg = new Program();
                            _prg._setDatabase();
                            
                        }
                        Thread.Sleep(1000);
                    }
                    catch (TimeoutException)
                    {
                       
                        //Add to Log
                        logFile.Add("Port Reading Data1 TimeoutException","["+DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")+"]");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex1)
                    {
                      
                        //Add to Log
                        logFile.Add("Port Reading Data2", ex1.ToString());
                    }
                }
                catch (TimeoutException ex1)
                {
                    Console.Write(".");
                    //Add to Log
                    logFile.Add("Port Reading Data3", ex1.ToString());
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                        _serialPort.Open();
                    }
                }
            }
        }

        /// <summary>
        /// Split String get from buffer and Save to database.
        /// </summary>
        public void _setDatabase()
        {
            try
            {
                //Log
                logFile.Add("_setDatabase Function Call start", "_setDatabase(0)");

               // stringFromTOLEDO = _serialPort.ReadLine();
                Thread.Sleep(1000);
                //Split the string from TOLEDO and return measurement Objects.
                Measures _tempMeasures = new Measures();
                _tempMeasures = stringFromTOLEDO.SplitTOLEDOstring();
                try
                {
                    //plit string to Measurement class format.
                    try
                    {
                        _measures = stringFromTOLEDO.SplitTOLEDOstring();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("String Split Error");
                        logFile.Add("String Split Error", "_setDatabase(0)");
                    }

                    //Measurement Object Passed to the Save Database Fucntion That save the Measurements to Packing ID.
                    Boolean _savedFlag = mPackage.setPackageInfo(_measures);
                    logFile.Add("_save", "Data Save '" + _savedFlag + "'");

                    //Save Log to the Ecxel File.
                    try
                    {
                        ExcelLogger Exel = new ExcelLogger(stringFromTOLEDO, _savedFlag);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Excel file writing");
                        logFile.Add("Excel file writing", "_setDatabase(0)");
                    }

                    Console.WriteLine("\nPackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + "Box length= " + _tempMeasures.BoxLength + Environment.NewLine + "Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + "Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + "Box Weight=" + _tempMeasures.BoxWeight);
                    Console.WriteLine("-----------------------Save in Database= " + _savedFlag + " ------------------------------");
                }
                catch (NullReferenceException)
                { //Log
                    logFile.Add("NullReferenceException", " Catch call in _setDatabase");
                    try
                    {
                        _measures = stringFromTOLEDO.SplitTOLEDOstring();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("String Split Error @ Chach Null Rererence");
                        logFile.Add("String Split Error", "_setDatabase(1)");
                    }

                    //Measurement Object Passed to the Save Database Fucntion That save the Measurements to Packing ID.
                    Boolean _savedFlag = mPackage.setPackageInfo(_measures);
                    logFile.Add("_save", "Data Save '" + _savedFlag + "'");

                    try
                    {
                        ExcelLogger Exel = new ExcelLogger(stringFromTOLEDO, _savedFlag);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Excel file writing");
                        logFile.Add("Excel file writing", "_setDatabase(1)");
                    }

                    Console.WriteLine("\nPackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + "Box length= " + _tempMeasures.BoxLength + Environment.NewLine + "Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + "Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + "Box Weight=" + _tempMeasures.BoxWeight);
                    Console.WriteLine("----------------------- Save in Database= " + _savedFlag + " -------------------------------");
                }
            }
            catch (Exception)
            { }

        }
    }
}
