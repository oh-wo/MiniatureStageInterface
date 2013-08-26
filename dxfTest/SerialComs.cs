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
    public class SerialComs
    {
        SerialPort _sPort;

        public event UIEventHandler specEvent;
        public class myEventArgs : EventArgs
        {
            //Use this to update the ui thread - could have any data as required
            public int Value { get; set; }
        }
        public delegate void UIEventHandler(SerialComs sComs, myEventArgs e);
        public void SpecialEvent()
        {

        }
        public void Start(SerialPort sPort)
        {
            _sPort = sPort;
            /*Use to update the ui thread if required */

            /*myEventArgs e = new myEventArgs();
            
            for (int i = 0; i < 10; i++)
            {
                e.Value=i;
                Thread.Sleep(1000);
                specEvent(this, e);
            }
             */
        }


        public void Move(float X, float Y, bool LaserOn, float TimeDelay){
            //Structure: [move xPos, yPos, laser on?, time delay, waitForPositionBeforeNextCommand?]
            _sPort.Write("[move 0 0 1 0.5 1]");
        }

    }
}


