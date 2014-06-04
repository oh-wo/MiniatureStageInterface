using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public SerialPort sp;
        System.Threading.Tasks.TaskScheduler scheduler;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            scheduler = System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext();
            configurePIStages();


            //UI
            //listbox properties
            this.listBoxCommands.Format += listBoxCommands_Format;
            this.listBoxCommands.AllowDrop = true;
            this.listBoxCommands.MouseDown += listBoxCommands_MouseDown;
            this.listBoxCommands.MouseUp += listBoxCommands_MouseUp;
            this.listBoxCommands.MouseMove += listBoxCommands_MouseMove;
            this.listBoxCommands.DragDrop += listBoxCommands_DragDrop;
            this.listBoxCommands.DragOver += listBoxCommands_DragOver;
            //movebox properties
            this.textBoxMoveProp.KeyUp += textBoxMoveProp_KeyUp;
            this.textBoxLoopN.KeyUp += textBoxLoopN_KeyUp;
            this.textBoxDelay.KeyUp += textBoxDelay_KeyUp;
            
        }

        void listBoxCommands_MouseUp(object sender, MouseEventArgs e)
        {
         
        }

        void textBoxDelay_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectedCommand != null)
                {
                    long delay = 0;
                    if (long.TryParse(this.textBoxDelay.Text, out delay))
                    {
                        SelectedCommand.Param1 = double.Parse(delay.ToString());//should be fine
                    }
                }
            }
        }

        void textBoxLoopN_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectedCommand != null)
                {
                    int n = 0;
                    if (int.TryParse(this.textBoxLoopN.Text, out n))
                    {
                        SelectedCommand.N = n;
                    }
                }
            }
        }

        void textBoxMoveProp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectedCommand != null)
                {
                    double _selectedCommandParam1 = 0;
                    if (double.TryParse(this.textBoxMoveProp.Text, out _selectedCommandParam1))
                    {
                        SelectedCommand.Param1 = _selectedCommandParam1;
                    }
                }
            }
        }

       

        void listBoxCommands_Format(object sender, ListControlConvertEventArgs e)
        {
            Command cmmd = e.ListItem as Command;

            e.Value = cmmd.Name;
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (expecting.Count == 0)
            {
                sp.Write("1 pos?\n");
            }
        }

        public void configurePIStages()
        {
            try
            {
                sp = new SerialPort();
                sp.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                sp.BaudRate = 9600;
                sp.PortName = "COM13";
                sp.NewLine = "\n";
                sp.ReceivedBytesThreshold = 1;
                sp.Parity = Parity.None;
                if (!sp.IsOpen)
                {
                    sp.Open();
                }
            }
            catch (Exception ex)
            {

            }
            sp.Write("1 err?\n");
            expecting.Add(0);
            sp.Write("1 svo?\n");
            expecting.Add(1);
            sp.Write("1 frf?\n");

            timer.Tick += OnTimerTick;
            timer.Interval = 500;
            timer.Start();
        }

        public string tempInput = "";
        public string allInput = "";

        string ErrorRegex = @"[0-9] [0-9] [0-9]+\n";
        string ErrorRegexDetail = @"[0-9]+\n";

        string PosRegex = @"0 1 1=-?[0-9]+\.[0-9]+\n";
        string PosRegexDetail = @"-?[0-9]+\.[0-9]+\n";

        string ExpectingRegex = @"0 1 1=(0|1)\n";
        string ExpectingRegexDetail = @"(0|1)\n";

        string LatestError = "";

        bool isFrf = false;
        bool isSvo = false;
        List<int> expecting = new List<int>();
        int expectingResult = -1;

        public void Move(double x)
        {
            sp.Write(String.Format("1 mov 1 {0:0.0000}\n 1 pos?\n", x));

        }

        Command SelectedCommand;

        public void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            tempInput = sp.ReadExisting();
            allInput += tempInput;



            Match errorMatch = Regex.Match(allInput, ErrorRegex);
            if (errorMatch.Success)
            {

                LatestError = Regex.Match(errorMatch.Value, ErrorRegexDetail).Value.Replace("\n", "");
                allInput = allInput.Remove(errorMatch.Index, errorMatch.Length);
            }

            Match posMatch = Regex.Match(allInput, PosRegex);
            if (posMatch.Success)
            {
                double.TryParse(Regex.Match(posMatch.Value, PosRegexDetail).Value.Replace("\n", ""), out currentXPos);
                allInput = allInput.Remove(posMatch.Index, posMatch.Length);
                var task = new System.Threading.Tasks.Task(() => this.labelCurrentXPos.Text = currentXPos.ToString("0.000000"));
                task.Start(scheduler);
            }
            Match expectingMatch = Regex.Match(allInput, ExpectingRegex);
            while (expectingMatch.Success)
            {
                int.TryParse(Regex.Match(expectingMatch.Value, ExpectingRegexDetail).Value.Replace("\n", ""), out expectingResult);
                allInput = allInput.Remove(expectingMatch.Index, expectingMatch.Length);


                int exp = expecting.FirstOrDefault();
                if (exp != null)
                {
                    switch (exp)
                    {
                        case 0://svo
                            isSvo = expectingResult == 0 ? false : true;
                            expecting.Remove(expecting.First());
                            if (!isSvo)
                            {
                                sp.Write("1 svo 1 1\n");
                            }
                            break;
                        case 1://frf
                            isFrf = expectingResult == 0 ? false : true;
                            expecting.Remove(expecting.First());
                            if (isFrf)
                            {
                                sp.Write("1 frf\n");
                            }
                            break;
                    }
                }
                expectingMatch = Regex.Match(allInput, ExpectingRegex);
            }

        }
        double currentXPos = 0;
        double xPos = 5;
        private void button1_Click(object sender, EventArgs e)
        {
            Move(xPos += 0.1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Move(xPos -= 0.1);
        }

        public class Command
        {
            public Command(string name, int type)
            {
                Name = name;
                Type = type;
            }
            public Command(string name, int type, bool start)
            {
                Name = name;
                Type = type;
                Start = start;
                Param2 = start;
            }
            public Command(string name, int type, int param1)
            {
                Name = name;
                Type = type;
                Param1 = param1;
                Param2 = true;
            }
            public string Name { get; set; }
            public int Type { get; set; }
            public int N { get; set; }//for loops only
            public bool Start = true;
            public double Param1 { get; set; }/*
             * 0 = loop
             * 1 = move
             * 2 = pause
             * */
            public bool Param2 { get; set; }/*true=start loop, false = end loop*/

            
        }

        private void loopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listBoxCommands.Items.Add(new Command("Start Loop n=1", 0, true));
            this.listBoxCommands.Items.Add(new Command("End Loop", 0, false));
        }
        private void HideCommandProperties()
        {
            this.groupBoxLoopProperties.Visible = false;
            this.groupBoxMoveProperties.Visible = false;
            this.groupBoxDelayProperties.Visible = false;
        }
        private void listBoxCommands_MouseDown(object sender, MouseEventArgs e)
        {
            HideCommandProperties();
            //get properties
            int index = this.listBoxCommands.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                SelectedCommand = ((Command)this.listBoxCommands.Items[index]);
                switch (SelectedCommand.Type)
                {
                    case 0://loop
                        this.textBoxLoopN.Text = SelectedCommand.N.ToString();
                        this.groupBoxLoopProperties.Visible = true;
                        break;
                    case 1://move
                        this.textBoxMoveProp.Text = SelectedCommand.Param1.ToString();
                        this.groupBoxMoveProperties.Visible = true;
                        break;
                    case 2://delay
                        this.textBoxDelay.Text = SelectedCommand.Param1.ToString();
                        this.groupBoxDelayProperties.Visible = true;
                        break;
                }
            }
            else
            {
                SelectedCommand = null;
                this.groupBoxMoveProperties.Visible = false;
            }
            originalMouseLocation = e.Location;

        }
        Point originalMouseLocation = new Point();
        private void listBoxCommands_MouseMove(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.Location.X-originalMouseLocation.X) > 10 || Math.Abs(e.Location.Y-originalMouseLocation.Y) > 10)
            {
                //drag drop
                if (this.listBoxCommands.SelectedItem == null) return;
                this.listBoxCommands.DoDragDrop(this.listBoxCommands.SelectedItem, DragDropEffects.Move);
            }
        }
        private void listBoxCommands_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            
        }
        private void listBoxCommands_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBoxCommands.PointToClient(new Point(e.X, e.Y));
            int index = this.listBoxCommands.IndexFromPoint(point);
            if (index < 0) { index = this.listBoxCommands.Items.Count - 1; }
            object data = e.Data.GetData(typeof(Command));
            this.listBoxCommands.Items.Remove(data);
            this.listBoxCommands.Items.Insert(index, data);
            //this.listBoxCommands.SelectedIndex = index;
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listBoxCommands.Items.Add(new Command("Move", 1, 1));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.buttonExecute.Text == "Execute")
            {
                stopRunningCommands = false;
                this.buttonExecute.Text = "Cancel";
                Task x = new Task(() => RunCommands());
                x.Start();
            }
            else
            {
                stopRunningCommands = true;
                this.buttonExecute.Text = "Execute";
            }
        }
        bool stopRunningCommands = false;
        public void RunCommands()
        {
            
            var task = new System.Threading.Tasks.Task(() => this.buttonExecute.Text = "Execute");
            object[] commands = new object[this.listBoxCommands.Items.Count];
            this.listBoxCommands.Items.CopyTo(commands, 0);

            ExecuteParseCommands(commands);
            task.Start(scheduler);
            
        }
        private static bool IsEndOfLoop(object x)
        {
            Command cmd = (Command)x;
            return cmd.Type == 0 && !cmd.Param2;
        }
        public void ExecuteParseCommands(object[] Commands)
        {
            foreach (Command command in Commands)
            {
                if (!stopRunningCommands)
                {
                    switch (command.Type)
                    {
                        case 0://loop
                            //find corresponding end
                            if (command.Param2)
                            {
                                int startIndex = Array.IndexOf<object>(Commands, command) + 1;
                                object endObject = Array.Find<object>(Commands, IsEndOfLoop);
                                int endIndex = Array.IndexOf<object>(Commands, endObject);

                                object[] newCollection = new object[endIndex - startIndex];
                                for (int i = 0; i < (endIndex - startIndex); i++)
                                {
                                    newCollection[i] = Commands[i + startIndex];
                                }
                                ExecuteParseCommands(newCollection);
                            }
                            else
                            {
                                //is end of loop so do nada
                            }
                            break;
                        case 1://move
                            Move(command.Param1);
                            while (command.Param2 && !IsInPosition(command.Param1) && !stopRunningCommands)
                            {
                                //if wait(param2) and not in pos, then wait til is in pos
                            };
                            break;
                        case 2://delay
                            System.Threading.Thread.Sleep(new TimeSpan(long.Parse(Math.Floor(command.Param1).ToString())));
                            break;
                    }
                }
            }
            stopRunningCommands = false;
        }

        public double tol = 0.001;
        public bool IsInPosition(double xPosition)
        {
            return ((xPosition - tol) < currentXPos && currentXPos < (xPosition + tol));
        }

        private void delayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listBoxCommands.Items.Add(new Command("Delay", 2, 1000));
        }
    }
}
