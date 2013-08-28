﻿using iTOLEDO.Classes;
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
            try
            {
                //string name;
                //  string message;
                StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
                Thread readThread = new Thread(Read);

                // Create a new SerialPort object with default settings.
                _serialPort = new SerialPort();

                _serialPort.PortName = iTOLODO.Properties.Settings.Default.PortName.ToString();
                _serialPort.BaudRate = (int)iTOLODO.Properties.Settings.Default.BaudRate;
                _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), iTOLODO.Properties.Settings.Default.Parity);
                _serialPort.DataBits = (int)iTOLODO.Properties.Settings.Default.DataBit;
                _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), iTOLODO.Properties.Settings.Default.StopBit);
                _serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), iTOLODO.Properties.Settings.Default.Handshak);

                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;
                _serialPort.Open();
                _continue = true;
                _Saved = false; 
                Console.WriteLine("Application Connected to " + iTOLODO.Properties.Settings.Default.PortName.ToString() + " Port");
                readThread.Start();

                readThread.Join();
                _serialPort.Close();
              
            }
            catch (Exception)
            {
                Console.WriteLine("Opning COM port Error. Device is under use of another application. Or Chech the Application settings.");
            }
          
        }

        public static void Read()
        {

            while (_continue)
            {
                try
                {
                    try
                    {
                        string message = _serialPort.ReadLine();
                        Program _prg = new Program();
                        _prg._setDatabase();
                    }
                    catch (TimeoutException)
                    {
                        Thread.Sleep(200);
                        /////Restatr appllication
                        //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        //// Closes the current process
                       // Environment.Exit(0);
                    }
                    catch (Exception)
                    { }
                   
                }
                catch (TimeoutException)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                        _serialPort.Open();
                    }
                }

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
                        Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxWeight);
                        Console.WriteLine("--------------------------------------------------------------------------------");
                        _measures = stringFromTOLEDO.SplitTOLEDOstring();
                        //Measurement Object Passed to the Save Database Fucntion That save the Measurements to Packing ID.
                        _Saved = mPackage.setPackageInfo(_measures);
                    }
                }
                catch (NullReferenceException)
                {
                    // Console.WriteLine("PackageID= " + _tempMeasures.PCKRowID.ToString() + Environment.NewLine + " Box length= " + _tempMeasures.BoxLength + Environment.NewLine + " Box Width= " + _tempMeasures.BoxWidth + Environment.NewLine + " Box heigh=" + _tempMeasures.BoxHeight + Environment.NewLine + " Box Weight=" + _tempMeasures.BoxHeight);
                    //Console.WriteLine("--------------------------------------------------------------------------------");
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