using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace WpfApplication1
{
    public class ConfigureOpenLoop
    {
        SerialPort sp;
        public void Configure(SerialPort SP)
        {
            sp = SP;
            sp.Close();
            if (!sp.IsOpen)
                sp.Open();

            List<Command> Commands = new List<Command>();
            Commands.Add(new Command() { Value = "[d]* \n", ExpReply=true,ParseFunction=ParseError, });//clear any existing errors
            Commands.Add(new Command() { Value = "[a 0]* \n", ExpReply = false, });//put into open loop mode
            Commands.Add(new Command() { Value = "[r]* \n", ExpReply = true, ParseFunction=ParseVolatileMemoryParameters,LinesToRead=44,});//put into open loop mode
            //Commands.Add(new Command() { Value = "[f]* \n", ExpReply = true, ParseFunction = ParseVelocity });//put into open loop mode

            foreach(Command command in Commands)
            {
                Thread.Sleep(100);
                sp.Write(command.Value);
                if (command.ExpReply)
                {
                    string readString = "";
                    int n = 1;
                    int linesRead = 0;
                    while (n < 20 && linesRead < (command.LinesToRead ?? 1))
                    {
                        try
                        {
                            if (sp.BytesToRead > 0)
                            {
                                readString += sp.ReadExisting();
                                linesRead++;
                            }
                            else
                            {
                                //hasn't found anything... give it 20-n more goes and then quit trying to find
                                Thread.Sleep(100);
                                n++;
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                    command.ParseFunction(readString);

                }

                
                
            }
            sp.Close();
        }

        public class Command
        {
            public string Value { get; set; } //the command to be sent
            public bool ExpReply { get; set; } //expecting a reply?
            public Func<string,bool> ParseFunction { get; set; }
            public int? LinesToRead { get; set; }
        }

        public bool ParseError(string readString)
        {
            if (readString.Length > 1) { 
            string[] arguments = readString.Split(' ');
            int ErrorCode = Int32.Parse(arguments[arguments.Length - 1]);
        };
            return true;
        }
        public bool ParseVelocity(string readString)
        {
            string[] arguments = readString.Split('=');
            float Value = float.Parse(arguments[arguments.Length - 1]);
            
            OpenLoopArgs e = new OpenLoopArgs() { Value = Value, Control = "textVelocity" };
            olEvent(this, e);
            return true;
        }
        public bool ParseVolatileMemoryParameters(string readString)
        {
            string[] lines = readString.Split('\n');
            string[] arguments;
            arguments = lines[23].Split('=');
            OpenLoopArgs e;
            e= new OpenLoopArgs() { Value = float.Parse(arguments[arguments.Length-1]), Control = "textVelocity" };
            olEvent(this, e);
            arguments = lines[24].Split('=');
            e = new OpenLoopArgs() { Value = float.Parse(arguments[arguments.Length - 1]), Control = "textAcceleration" };
            olEvent(this, e);
            return true;
        }

        public class OpenLoopArgs : EventArgs
        {
            public float Value { get; set; }
            public String Control { get; set; }
        }
        public delegate void OpenLoopHandler(ConfigureOpenLoop ol, OpenLoopArgs e);
        public OpenLoopHandler olEvent;
    }
}
