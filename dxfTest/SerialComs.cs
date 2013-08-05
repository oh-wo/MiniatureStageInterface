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
        bool runWorkerThread;
        public static float xPos = 0;
        public static float yPos = 0;
        double increment = 0.25;
        public static Form1.StageBounds stageBounds;
        public static PointF LaserPt;


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
        public void StartComms()
        {

            try
            {
                sp.Open();
                sp.Handshake = Handshake.None;
                sp.RtsEnable = true;

                //sp.DiscardInBuffer();
                //sp.DiscardOutBuffer();


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
            ConnectSerialComs(true);
            ConfigureStageAfterPowerOff();
        }
        public static void SetLaserPointValues(){
            if(sp.IsOpen){
                PointF tempLaserPoint = new System.Drawing.PointF()
                {
                    X = GetAxisPosition(1),
                    Y = GetAxisPosition(2),
                };
                LaserPt = tempLaserPoint;
            }
            //Iform1.DrawDrawing(); 
        }
        public void ConfigureStageAfterPowerOff()
        {
            //Set in open loop mode
            sp.WriteLine("1 svo 1 1");
            sp.WriteLine("2 svo 1 1");
            //Put into original position
            sp.WriteLine("1 frf");
            sp.WriteLine("2 frf");
            //Once has stopped moving
            Thread.Sleep(5000);
            SetLaserPointValues();
            stageBounds = new Form1.StageBounds()
            {
                X = new float[2] { (float)((LaserPt.X - 4) / 100), (float)((LaserPt.X + 4) / 100) },
                Y = new float[2] { (float)((LaserPt.Y - 4) / 100), (float)((LaserPt.Y + 4) / 100) },
            };
        }
        public static float GetAxisPosition(int axis)
        {
            sp.WriteLine(String.Format("{0} pos?", axis));
            System.Threading.Thread.Sleep(500);
            string input = sp.ReadExisting();
            Match[] output = Regex.Matches(input, @"[+-]?\d+(\.\d+)?").Cast<Match>().ToArray();
            return output.Length >= 4 ? float.Parse(output[3].ToString()) : (float)0;
        }

        public void MoveToEnds()
        {
            sp.WriteLine("1 mov 1 0");
            sp.WriteLine("2 mov 1 0");
        }

        public bool IsMoving()
        {
            string mes = ((char)5).ToString();
            sp.WriteLine(mes);
            Thread.Sleep(100);
            string input = sp.ReadExisting();
            string[] output = Regex.Split(input, @"[-0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            return output.Length > 0 ? (int.Parse(output[0]) == 1) : false;
        }
    }
}
