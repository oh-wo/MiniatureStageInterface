using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using System.Threading;
using System.IO.Ports;
namespace dxfTest
{

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        SerialPort sPort;

        public Form1()
        {
            InitializeComponent();
            /*General initializing */
            DisplayAvailableSerialPorts();
            /*Dxf stuff - to be shifted later to 'open file' etc */
            /*Serial Communication begins */
            SerialComs sComs = new SerialComs();
            this.Subscribe(sComs);
            Thread x = new Thread(sComs.Start);
            x.Start();
        }
        /*Stuff to communicate between classes & threads */
        public void Subscribe(SerialComs sComs)
        {
            sComs.specEvent += new SerialComs.UIEventHandler(HeardIt);
        }
        private void HeardIt(SerialComs sComs, dxfTest.SerialComs.myEventArgs e)
        {
            //use this to update a ui control from the serialcoms thread
            if (this.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    //Change this to whatever control is required
                    //this.textBox1.Text = e.Value.ToString();
                });
            }

        }
        public void DisplayAvailableSerialPorts()
        {
            foreach (string comPort in SerialPort.GetPortNames())
            {
                this.comboSerialPorts.Items.Add(comPort);
            }
            this.comboSerialPorts.SelectedIndex = 1;
        }
        /**/

    }
}