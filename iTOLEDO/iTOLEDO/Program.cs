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

        //  [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "Full")]
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

                Console.WriteLine("Application Connected to " + iTOLEDO.Properties.Settings.Default.PortName.ToString() + " Port");
                readThread.Start();

                readThread.Join();
                _serialPort.Close();

            }
            catch (Exception ex)
            {
                //Add to Log
                logFile.Add("Port Opening Error",ex.ToString() );
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
                        
                        Console.WriteLine(Environment.NewLine+"DATA =" + _serialPort.ReadLine());
                        //Add to Log
                        logFile.Add("*Readind Data0", _serialPort.ReadLine());

                        string message = _serialPort.ReadLine();
                        Program _prg = new Program();
                        _prg._setDatabase();
                        Thread.Sleep(1000);
                    }
                    catch (TimeoutException ex)
                    {
                      
                        //Add to Log
                        logFile.Add("Port Readind Data1", ex.ToString());

                        ///Restatr appllication
                        //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        // Closes the current process
                        //Environment.Exit(0); 
                        Thread.Sleep(200);
                    }
                    catch (Exception ex1)
                    {
                        //Add to Log
                        logFile.Add("Port Readind Data2", ex1.ToString());
                    }


                }
                catch (TimeoutException ex1)
                {
                    //Add to Log
                    logFile.Add("Port Readind Data3", ex1.ToString());
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                        _serialPort.Open();
                    }
                }

            }
        }

        /// <summary>
        /// Set Port Name
        /// </summary>
        /// <param name="defaultPortName"> String Port Name to set.</param>
        /// <returns>String</returns>
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("COM port({0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "")
            {
                portName = defaultPortName;
            }
            return portName;
        }

        /// <summary>
        /// Set Port Baud Rate
        /// </summary>
        /// <param name="defaultPortBaudRate">int Baud Rate of Port to set</param>
        /// <returns>intiger BaudRate</returns>
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate({0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        /// <summary>
        /// Set Port Parity
        /// </summary>
        /// <param name="defaultPortParity">Enum Port Parity</param>
        /// <returns>Enum of Parity Value </returns>
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Parity({0}):", defaultPortParity.ToString());
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity);
        }

        /// <summary>
        /// Set Port Databitd
        /// </summary>
        /// <param name="defaultPortDataBits">int Data Bits</param>
        /// <returns></returns>
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Data Bits({0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits);
        }

        /// <summary>
        /// Set Stop Bit for port
        /// </summary>
        /// <param name="defaultPortStopBits">Enum StopBit value</param>
        /// <returns>Enum Conved Stop Bit Value</returns>
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available Stop Bits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Stop Bits({0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits);
        }

        /// <summary>
        /// Set Handshake Method of Port
        /// </summary>
        /// <param name="defaultPortHandshake">Enum Handshake value</param>
        /// <returns>Enum of type Handshake</returns>
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Handshake({0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }
            return (Handshake)Enum.Parse(typeof(Handshake), handshake);
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

                stringFromTOLEDO = _serialPort.ReadLine();
                //Split the string from TOLEDO and return measurement Objects.

                Measures _tempMeasures = new Measures();
                _tempMeasures = stringFromTOLEDO.SplitTOLEDOstring();
                try
                {
                    if (_tempMeasures.PCKRowID != _measures.PCKRowID)
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
                        

                        Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxWeight);
                        Console.WriteLine("---------------------------------" + _savedFlag + "----------------------------------------");
                    }
                    else
                    {
                        logFile.Add("Same Data in buffer reading continue", "_setDatabase(0)");
                    }
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

                    Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxWeight);
                    Console.WriteLine("---------------------------------" + _savedFlag + "----------------------------------------");
                }
            }
            catch (Exception)
            { }

        }
    }
}
