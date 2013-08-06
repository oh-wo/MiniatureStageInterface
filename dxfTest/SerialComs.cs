using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace dxfTest
{
    interface SerialInterface
    {
        void MoveRight();
    }
    class SerialComs : SerialInterface
    {
        IForm1 Iform1 = new Form1();
        public static SerialPort sp = new SerialPort();
        Thread commsThread;
        public static bool isMoving = false;
        bool runWorkerThread;
        public static float xPos = 0;
        public static float yPos = 0;
        double increment = 0.25;
        public static Form1.StageBounds stageBounds;
        public static float[] LaserPt = new float[2];


        public void MoveRight()
        {
            //Send move command
         
                
            
        }
        
        public static string[] GetAvailableSerialPorts()
        {
            return SerialPort.GetPortNames();

        }
        public void ConnectSerialComs(bool Connect)
        {
            if (!Connect)
            {
                //Disconnect
                if (sp.IsOpen)
                {
                    sp.Close();
                }
                else
                {
                    //already closed
                }
            }
            else
            {
                //Connect
                if (!sp.IsOpen)
                {
                    try
                    {
                        sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                        sp.PortName = Iform1.SelectedSerialPort;
                        sp.Open();
                        
                        
                    }
                    catch
                    {
                        //no port available etc

                    }
                }
                else
                {
                    //already open
                }
            }

        }
        public static void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string input = sp.ReadExisting();
        }
        Form1 form1;
        public void StartComms(Form1 form)
        {

            try
            {
                sp.Open();
                sp.Handshake = Handshake.None;
                sp.RtsEnable = true;

                //sp.DiscardInBuffer();
                //sp.DiscardOutBuffer();
                form1 = form;

                commsThread = new Thread(new ThreadStart(commsWorker));
                runWorkerThread = true;
                commsThread.Start();
            }
            catch (Exception ex)
            {
                
            }

        }
        public void EndComms()
        {
            runWorkerThread = false;

            if (sp != null)
            {
                if (sp.IsOpen)
                    sp.Close();
            }
        }
        public void commsWorker()
        {
            Thread serListener = new Thread(serialListener);
            serListener.Start();
            ConnectSerialComs(true);
            ConfigureStageAfterPowerOff();
            Thread.Sleep(100);
            sp.WriteLine("1 pos?");
            Thread.Sleep(100);
            sp.WriteLine("2 pos?");
        }

        string serialInput;
        string serialGoodBits;
        public void serialListener()
        {
            //We can't use the data recieved event for the nanostage controllers, hence making our own data received event in the while loop below.
            while (runWorkerThread)
            {
                if (SerialComs.sp.BytesToRead > 0)
                {
                    serialInput += sp.ReadExisting();
                    
                    int delCharIndex = serialInput.LastIndexOf('\n');
                    if (delCharIndex > 0)
                    {
                        serialGoodBits = serialInput.Substring(0, serialInput.Length);
                        serialInput = serialInput.Substring(delCharIndex, serialInput.Length - delCharIndex);
                        
                    }
                    string[] splitInput = serialGoodBits.Split('\n');
                    foreach (string split in splitInput)
                    {
                        var action = SerialIdentify(split);
                        switch (action)
                        {
                            case "movement":
                                IsMoving(split);
                                break;
                            case "position":
                                GetPosition(split);
                                break;
                        }
                    }
                }
            }
        }
        public static string SerialIdentify(string input)
        {
            if (Regex.IsMatch(input, "^[0-9]+$"))
                return "movement";
            if (Regex.IsMatch(input, @"\d\s\d=[+-]?\d+(\.\d+)?"))
                return "position";

            return "";
        }
        public void ConfigureStageAfterPowerOff()
        {
            //Set in open loop mode
            sp.WriteLine("1 svo 1 1");
            sp.WriteLine("2 svo 1 1");
            //Put into original position
            isMoving = true;
            sp.WriteLine("1 frf");
            sp.WriteLine("2 frf");
            //Once has stopped moving
            while (isMoving)
            {
                sp.WriteLine(((char)5).ToString());
                Thread.Sleep(100);
            }
            stageBounds = new Form1.StageBounds()
            {
                X = new float[2] { (float)((LaserPt[0] - 7) / 100), (float)((LaserPt[0] + 7) / 100) },
                Y = new float[2] { (float)((LaserPt[1] - 7) / 100), (float)((LaserPt[1] + 7) / 100) },
            };
        }
        public static float GetAxisPosition(int axis)
        {
            sp.WriteLine(String.Format("{0} pos?", axis));
            System.Threading.Thread.Sleep(500);
            //string input = sp.ReadExisting();
            //Match[] output = Regex.Matches(input, @"[+-]?\d+(\.\d+)?").Cast<Match>().ToArray();
            return (float)0; // output.Length >= 4 ? float.Parse(output[3].ToString()) : (float)0;
        }
        public static void Move(int axis, bool positive)
        {
            sp.WriteLine(String.Format("{0} mov 1 {1}", 1, (float)(LaserPt[axis - 1] + (positive ? 0.5 : -0.5))));
            while (isMoving)
            {
                sp.WriteLine(((char)5).ToString());
                Thread.Sleep(100);
            }
            sp.WriteLine(String.Format("{0} pos?", axis));
        }
        public void IsMoving(string sumAxisMoving)
        {
            isMoving = int.Parse(sumAxisMoving.Substring(0,1)) > 0;
        }
        public void GetPosition(string posStr)
        {
            //input string in the form of "[?] [axis] [channel] = [(float)position]
            LaserPt[int.Parse(posStr.Substring(2, 1))-1] = float.Parse(posStr.Substring(6, posStr.Length - 6));

            form1.Invoke(form1.updatePosition, 5);
        }



        
    }

}
