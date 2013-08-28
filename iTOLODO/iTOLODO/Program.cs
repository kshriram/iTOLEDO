using iTOLEDO.Classes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iTOLODO
{
    class Program
    {
        static bool _continue;
        static bool _Saved;
        static SerialPort _serialPort;
       static Measures _measures;
       static String stringFromTOLEDO = "";

        public static void Main()
        {
            //string name;
          //  string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            //_serialPort.PortName = SetPortName(_serialPort.PortName);
            //_serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            //_serialPort.Parity = SetPortParity(_serialPort.Parity);
            //_serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            //_serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            //_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

            _serialPort.PortName = SetPortName("COM15");
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            _Saved = false;
            readThread.Start();

            readThread.Join();
            _serialPort.Close();
        }

        public static void Read()
        {
            
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                   
                    // _continue = false;
                    Program _prg = new Program();
                    _prg._setDatabase();
                }
                catch (TimeoutException) { }
            }
        }

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
                    stringFromTOLEDO = _serialPort.ReadLine();
                    //Split the string from TOLEDO and return measurement Objects.
                    Measures _tempMeasures = new Measures();
                    _tempMeasures = stringFromTOLEDO.SplitTOLEDOstring();
                    try
                    {
                        if (_tempMeasures.PCKRowID != _measures.PCKRowID)
                        {
                            Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxHeight);
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            _measures = stringFromTOLEDO.SplitTOLEDOstring();
                            //Measurement Object Passed to the Save Database Fucntion That save the Measurements to Packing ID.
                            _Saved = mPackage.setPackageInfo(_measures);
                        }
                    }
                    catch (NullReferenceException )
                    {
                        Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxHeight);
                        Console.WriteLine("--------------------------------------------------------------------------------");
                        _measures = stringFromTOLEDO.SplitTOLEDOstring();
                        //Measurement Object Passed to the Save Database Fucntion That save the Measurements to Packing ID.
                        _Saved = mPackage.setPackageInfo(_measures);
                    }
                    
            }
            catch (Exception)
            { }
          
        }
    }
}
