using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;
using WebcamControl;
using System.Text.RegularExpressions;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        //Form form2 = new WindowsFormsApplication1.Form2();

        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
        // Global variables
        string[] file;
        public PointF Origin = new PointF() { X = 0, Y = 0 };
        float bound = (float)0.075;
        Dxf.StageBounds stageBounds;
        public const int LaserPrecision = 10;
        public Dxf.Units units = new Dxf.Units();
        public List<Dxf.Line> Lines = new List<Dxf.Line>();
        public List<Dxf.Polyline> Polylines = new List<Dxf.Polyline>();
        public List<Dxf.Arc> Arcs = new List<Dxf.Arc>();
        public List<Dxf.Circle> Circles = new List<Dxf.Circle>();
        public static float lineSpacing = 10;
        //Global parameters
        SerialPort sPort;
        public SolidColorBrush brush = new SolidColorBrush(Colors.White);
        public SolidColorBrush selectedBrush = new SolidColorBrush(Colors.Orange);
        public SolidColorBrush yaxisBrush = new SolidColorBrush(Colors.Green);
        public SolidColorBrush xaxisBrush = new SolidColorBrush(Colors.Red);
        public SolidColorBrush stageBoundsBrush = new SolidColorBrush(Colors.Blue);
        public float drawingHeight = (float)0;
        public float drawingWidth = (float)0;
        public string fileDirectory = String.Format("{0}\\Files\\", System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
        public string fullFile = String.Format("{0}\\Files\\Drawing1_2.dxf", System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
        public int? selectedLineParentIndex = null;
        public int? selectedLaserLineIndex = null;
        public string selectedType;
        public float scrollOffsetX = 0;
        public float scrollOffsetY = 0;
        public System.Windows.Controls.Image image;
        private DrawingContext _Context;
        SerialPort arduinoSerial;

        private delegate void TextChanger();
        public MainWindow()
        {
            InitializeComponent();
            /*General initializing */
            // canvasDielectric.Children.Add(new Line() { X1 = 5, X2 = 100, Y1 = 5, Y2 = 100, StrokeThickness = 2, Stroke = System.Windows.Media.Brushes.Orange });
            //DrawLine(6, 101, 6, 101, 0, 0, 0, false);
            //connectToDue();
            configurePIStages();
            try
            {
                arduinoSerial = new SerialPort();
                arduinoSerial.BaudRate = 9600;
                arduinoSerial.PortName = "COM5";
                if (!arduinoSerial.IsOpen)
                    arduinoSerial.Open();
                this.arduinoConnectionState.Content = "Connected to arduino (shutter)";
            }
            catch (Exception ex)
            {
                this.arduinoConnectionState.Content = "Not  connected to arduino (shutter)";
            }
            //Canvas background colour opaque by default
            canvasDielectric.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 41, 51));
            //chipCanvas.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 41, 51));
            vScrollBar1.Value = 0.5;
            hScrollBar1.Value = 0.5;

            //Prepare drawing
            image = new System.Windows.Controls.Image();
            image.Height = canvasDielectric.Height;
            image.Width = canvasDielectric.Width;
            stageBounds = new Dxf.StageBounds()
            {
                X = new float[2] { -bound, bound },
                Y = new float[2] { -bound, bound },
            };
            /*Dxf stuff - to be shifted later to 'open file' etc */
            Dxf dxf = new Dxf();
            this.SubscribeDxf(dxf);
            Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
            dxfThread.Name = "Dxf thread";
            dxfThread.Start();


            /*
            //Serial Communication begins 
            SerialComs sComs = new SerialComs();
            this.Subscribe(sComs);
            Thread x = new Thread(() => sComs.Start(sPort));
            x.Start();
            */


            /*General UI setup */
            this.labelCurrentFile.Content = System.IO.Path.GetFileName(fullFile);
            this.textLineSpacing.Text = lineSpacing.ToString();
            this.tbOffsetX.Text = Properties.Settings.Default["offsetX"].ToString();
            this.tbOffsetY.Text = Properties.Settings.Default["offsetY"].ToString();
            try
            {
                CameraWindow cWindow = new CameraWindow();
                cWindow.Show();
            }
            catch (Exception ex)
            {

            }

        }

        public void configurePIStages()
        {
            try
            {
                sp = new SerialPort();
                sp.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                sp.BaudRate = 9600;
                sp.PortName = "COM4";
                sp.NewLine = "\n";
                sp.ReceivedBytesThreshold = 4;
                sp.Parity = Parity.None;

                if (!sp.IsOpen)
                    sp.Open();
                SerialWriteLine("1 err?");
                SerialWriteLine("2 err?");
                SerialWriteLine("1 svo 1 1");
                SerialWriteLine("2 svo 1 1");
                Thread.Sleep(500);
                SerialWriteLine("1 frf 1");
                SerialWriteLine("2 frf 1");
                this.stageConnectionState.Content = "Connected to stages";
                Thread.Sleep(1000);
                MoveAbsolute(7.5, 9.5);
            }
            catch (Exception ex)
            {
                this.stageConnectionState.Content = "Not connected to stages";
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (sp != null)
            {
                if (sp.IsOpen)
                    sp.Close();
                sp.Dispose();
            }
            Application.Current.Shutdown(0);
            base.OnClosing(e);
        }
        #region Camera




        #endregion

        #region Femtosecond Machining
        public void connectToDue()
        {
            this.stageConnectionState.Content = "not connected to stages";
            SerialPort _sp = new SerialPort()
            {
                BaudRate = 9600,
            };
            string inString = "";
            foreach (string spName in SerialPort.GetPortNames())
            {
                if (_sp.IsOpen)
                {
                    _sp.Close();
                }
                _sp.PortName = spName;
                _sp.Open();
                _sp.Write("[x]*");
                int timeout = 10;
                int counter = 0;
                while (timeout > counter)
                {
                    inString += _sp.ReadExisting();
                    Thread.Sleep(100);
                    counter++;
                }
                _sp.Close();
                if (inString.Contains("dielectric control due"))
                {
                    if (sp == null)
                    {
                        sp = new SerialPort();
                    }
                    if (sp.IsOpen)
                        sp.Close();
                    sp = _sp;

                    sp.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                    sp.Open();
                    this.stageConnectionState.Content = "connected to stages";
                    break;
                }
            }
        }
        public void SubscribeDxf(Dxf dxf)
        {
            dxf.dxfEvent += new Dxf.DxfEventHandler(dxfUploaded);
        }
        public void dxfUploaded(Dxf dxf, Dxf.DxfEventArgs e)
        {
            //Save event variables to the global Form1 variables
            Polylines = e.Polylines;
            Lines = e.Lines;
            Circles = e.Circles;
            Arcs = e.Arcs;
            Origin = e.Origin;
            units = e.Units;
            drawingHeight = e.DrawingHeight;
            drawingWidth = e.DrawingWidth;
            //Display the units detected to the user
            DisplayUnits();
            DrawDrawing();
        }

        //Dxf stuff

        public Dxf.Line DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad, SolidColorBrush b, Dxf.Line line, bool Clickable, string specifiedType)
        {
            //if from autocad then have to flip the y-axis
            float x1 = fromAutoCad ? X1 * scale * units.LinearConversionFactor + xOffset + scrollOffsetX : X1 * scale + xOffset + scrollOffsetX;
            float y1 = fromAutoCad ? (float)canvasDielectric.Height - ((Y1) * scale * units.LinearConversionFactor + yOffset) + scrollOffsetY : Y1 * scale + yOffset + scrollOffsetY;
            float x2 = fromAutoCad ? X2 * scale * units.LinearConversionFactor + xOffset + scrollOffsetX : X2 * scale + xOffset + scrollOffsetX;
            float y2 = fromAutoCad ? (float)canvasDielectric.Height - ((Y2) * scale * units.LinearConversionFactor + yOffset) + scrollOffsetY : Y2 * scale + yOffset + scrollOffsetY;
            MyLine li = new MyLine()
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                StrokeThickness = 2,
                Stroke = System.Windows.Media.Brushes.Orange,
                StrokeEndLineCap = PenLineCap.Triangle,
                ParentIndex = line.parentIndex,
                Clickable = Clickable,
                Uid = specifiedType,//"MyLine"
                Cursor = Cursors.Hand,
            };
            li.Stroke = b;

            li.MouseDown += new MouseButtonEventHandler(line_MouseDown);
            canvasDielectric.Children.Add(li);
            if (fromAutoCad)
            {
                MyEllipse ellipse = new MyEllipse()
                {
                    Clickable = true,
                    Uid = "MyEllipse",
                    ParentIndex = line.parentIndex,
                    Center = new System.Windows.Point() { X = (double)x2, Y = (double)y2 },
                    Radius = 2,
                    Stroke = b,
                    StrokeThickness = 5,
                    Cursor = Cursors.Hand,

                };
                ellipse.MouseDown += new MouseButtonEventHandler(ellipse_MouseDown);
                canvasDielectric.Children.Add(ellipse);
            }

            line.plotted1 = new PointF()
            {
                X = fromAutoCad ? X1 * scale * units.LinearConversionFactor + xOffset : X1 * scale + xOffset,
                Y = fromAutoCad ? (float)canvasDielectric.Height - ((Y1) * scale * units.LinearConversionFactor + yOffset) : Y1 * scale + yOffset,
            };
            line.plotted2 = new PointF()
            {
                X = fromAutoCad ? X2 * scale * units.LinearConversionFactor + xOffset : X2 * scale + xOffset,
                Y = fromAutoCad ? (float)canvasDielectric.Height - ((Y2) * scale * units.LinearConversionFactor + yOffset) : Y2 * scale + yOffset,
            };
            return line;
        }
        public Dxf.Line DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad)
        {
            Dxf.Line line = new Dxf.Line();
            line = DrawLine(X1, Y1, X2, Y2, xOffset, yOffset, scale, fromAutoCad, brush, line, false, "");
            return line;
        }
        public void DrawOrigin(float xOffset, float yOffset, float scale)
        {
            Dxf.Line line = new Dxf.Line();
            if (stageBounds != null)
            {
                float xWidth = (stageBounds.X[1] - stageBounds.X[0]) / 32;
                float yWidth = (stageBounds.Y[1] - stageBounds.Y[0]) / 32;
                //x-axis
                DrawLine(Origin.X - xWidth, Origin.Y, Origin.X + xWidth, Origin.Y, xOffset, yOffset, scale, false, xaxisBrush, line, false, "");
                //y-axis
                DrawLine(Origin.X, Origin.Y - yWidth, Origin.X, Origin.Y + yWidth, xOffset, yOffset, scale, false, yaxisBrush, line, false, "");
            }
        }
        public void DrawStageBounds(float xOffset, float yOffset, float scale)
        {
            Dxf.Line line = new Dxf.Line();
            if (stageBounds != null)
            {
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[0], xOffset, yOffset, scale, false, stageBoundsBrush, line, false, "");//bottom
                DrawLine(stageBounds.X[0], stageBounds.Y[1], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsBrush, line, false, "");//top
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[0], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsBrush, line, false, "");//left
                DrawLine(stageBounds.X[1], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsBrush, line, false, "");//right
            }
        }
        public void DrawDrawing()
        {

            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                canvasDielectric.Children.Clear();

                float scale;
                try
                {
                    scale = float.Parse(this.textScale.Text.Trim());
                    if (scale == (float)0) { scale = 1; };
                }
                catch
                {
                    scale = (float)1.0;
                }
                scale /= (float)(0.075 / canvasDielectric.Width);
                float xOffset = (float)canvasDielectric.Width / 2; // // +drawingWidth / (2 * scale);(float)this.hScrollBar1.Value / 100 * 
                float yOffset = (float)canvasDielectric.Height / 2;//(float)this.vScrollBar1.Value / 100 * 
                for (int j = 0; j < Polylines.Count; j++)
                {
                    for (int i = 0; i < Polylines[j].laserLines.Count; i++)
                    {
                        try
                        {
                            Polylines[j].laserLines[i] = DrawLine(Polylines[j].laserLines[i].p1.X, Polylines[j].laserLines[i].p1.Y, Polylines[j].laserLines[i].p2.X, Polylines[j].laserLines[i].p2.Y, xOffset, yOffset, scale, true, brush, Polylines[j].laserLines[i], true, "MyPolyline");
                        }
                        catch (Exception ex) { }
                    }
                }
                for (int j = 0; j < Lines.Count; j++)
                {
                    Lines[j] = DrawLine(Lines[j].p1.X, Lines[j].p1.Y, Lines[j].p2.X, Lines[j].p2.Y, xOffset, yOffset, scale, true, brush, Lines[j], true, "line");
                }
                foreach (Dxf.Circle circle in Circles)
                {
                    for (int i = 0; i < circle.laserLines.Count; i++)
                    {
                        circle.laserLines[i] = DrawLine(circle.laserLines[i].p1.X, circle.laserLines[i].p1.Y, circle.laserLines[i].p2.X, circle.laserLines[i].p2.Y, xOffset, yOffset, scale, true, brush, circle.laserLines[i], true, "circle");
                    }
                }
                if (this.checkDisplayOrigin.IsChecked ?? false)
                {
                    DrawOrigin(xOffset, yOffset, scale);
                }
                else
                {
                    //user doesn't want to display the origin
                }
                if (this.checkStageBounds.IsChecked ?? false)
                {
                    DrawStageBounds(xOffset, yOffset, scale);
                }
                else
                {
                    //user doesn't want to display the origin
                }
            }));
        }


        //UI Events
        void buttonChangeFile_Clicked(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            Polylines.Clear();
            ofd.InitialDirectory = String.Format("{0}\\Files\\", System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
            if (ofd.ShowDialog() ?? false)
            {
                fullFile = ofd.FileName;
                Dxf dxf = new Dxf();
                this.SubscribeDxf(dxf);
                Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
                dxfThread.Name = "Dxf";
                dxfThread.Start();
                this.labelCurrentFile.Content = System.IO.Path.GetFileName(fullFile);
            }
        }
        void line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            textSegmentLaserSpacing.IsEnabled = true;
            MyLine _selLine = ((MyLine)sender);


            foreach (UIElement ui in canvasDielectric.Children)
            {
                switch (ui.Uid)
                {
                    case "MyLine":
                        selectedType = "line";
                        MyLine _uiLine = ((MyLine)ui);
                        if (_uiLine.Clickable)
                        {
                            if (_uiLine.ParentIndex == _selLine.ParentIndex)
                            {
                                _uiLine.Stroke = selectedBrush;
                                selectedLineParentIndex = _selLine.ParentIndex;
                            }

                            else
                            {
                                _uiLine.Stroke = brush;
                            }
                        }
                        break;
                    case "MyEllipse":
                        MyEllipse _selEll = ((MyEllipse)ui);
                        if (_selEll.Clickable)
                        {
                            if (_selEll.ParentIndex == _selLine.ParentIndex)
                            {
                                _selEll.Stroke = selectedBrush;
                                selectedLineParentIndex = _selLine.ParentIndex;
                            }

                            else
                            {
                                _selEll.Stroke = brush;
                            }
                        }
                        break;
                    case "MyPolyline":
                        selectedType = "polyline";
                        MyLine _uiPLine = ((MyLine)ui);
                        if (_uiPLine.Clickable)
                        {
                            if (_uiPLine.ParentIndex == _selLine.ParentIndex)
                            {
                                _uiPLine.Stroke = selectedBrush;
                                selectedLineParentIndex = _selLine.ParentIndex;
                            }

                            else
                            {
                                _uiPLine.Stroke = brush;
                            }
                        }
                        break;
                }

            }
            ShowSelectedLine();

        }
        void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            textSegmentLaserSpacing.IsEnabled = true;
            MyEllipse _selEll = ((MyEllipse)sender);
            foreach (UIElement ui in canvasDielectric.Children)
            {
                if (ui.Uid.StartsWith("MyLine"))
                {
                    selectedType = "line";
                    MyLine _uiLine = ((MyLine)ui);
                    if (_uiLine.Clickable)
                    {
                        if (_uiLine.ParentIndex == _selEll.ParentIndex)
                        {
                            _uiLine.Stroke = selectedBrush;
                            selectedLineParentIndex = _selEll.ParentIndex;
                        }

                        else
                        {
                            _uiLine.Stroke = brush;
                        }
                    }
                }
                if (ui.Uid.StartsWith("MyEllipse"))
                {
                    MyEllipse _uiEll = ((MyEllipse)ui);
                    if (_uiEll.Clickable)
                    {
                        if (_uiEll.ParentIndex == _selEll.ParentIndex)
                        {
                            _uiEll.Stroke = selectedBrush;
                            selectedLineParentIndex = _selEll.ParentIndex;
                        }

                        else
                        {
                            _uiEll.Stroke = brush;
                        }
                    }
                }
                if (ui.Uid.StartsWith("MyPolyline"))
                {
                    selectedType = "polyline";
                    MyLine _uiLine = ((MyLine)ui);
                    if (_uiLine.Clickable)
                    {
                        if (_uiLine.ParentIndex == _selEll.ParentIndex)
                        {
                            _uiLine.Stroke = selectedBrush;
                            selectedLineParentIndex = _selEll.ParentIndex;
                        }

                        else
                        {
                            _uiLine.Stroke = brush;
                        }
                    }
                }


            }
            ShowSelectedLine();
        }
        void canvasDielectric_MouseDown(object sender, EventArgs e)
        {
            ClearSelectedLine();
            DrawDrawing();
            lastPos = null;

            textSegmentLaserSpacing.Text = "";
            textSegmentLaserSpacing.IsEnabled = false;
        }
        PointF lastPos;
        void canvasDielectric_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PointF currentPos = new PointF()
                {
                    X = (float)e.GetPosition(canvasDielectric).X,
                    Y = (float)e.GetPosition(canvasDielectric).Y,
                };
                if (lastPos != null)
                {
                    scrollOffsetX += currentPos.X - lastPos.X;
                    scrollOffsetY += currentPos.Y - lastPos.Y;
                    DrawDrawing();
                }
                lastPos = currentPos;
            }
        }
        void checkStageBounds_Checked(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void checkStageBounds_Unchecked(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void checkDisplayOrigin_Checked(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void checkDisplayOrigin_Unchecked(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void textScale_LostFocus(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void textLineSpacing_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                /*ClearSelectedLine();
                int userInput = int.Parse(this.textLineSpacing.Text);
                lineSpacing = userInput <= 0 ? 1 : userInput;
                Dxf dxf = new Dxf();
                this.SubscribeDxf(dxf);
                Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
                dxfThread.Name = "Dxf";
                dxfThread.Start();*/
                decimal userInput = 0;
                if (decimal.TryParse(this.textLineSpacing.Text, out userInput))
                {
                    int _laserSpacing = int.Parse((Math.Round(userInput)).ToString());
                    for (int i = 0; i < Polylines.Count; i++)
                    {
                        ChangePolylineSpacing(Polylines[i], i, _laserSpacing);
                    }
                    DrawDrawing();
                }
            }
        }
        void textSegmentLaserSpacing_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                float laserSpacing = 0;
                float.TryParse(this.textSegmentLaserSpacing.Text, out laserSpacing);
                if (laserSpacing != 0)//&& selectedLineParentIndex != null
                {
                    switch (selectedType)
                    {
                        case "line":
                            //  Lines = ChangeLaserLineSpacing(Lines, laserSpacing);
                            DrawDrawing();
                            break;
                        case "polyline":
                            Polylines[selectedLineParentIndex ?? 0] = ChangePolylineSpacing(Polylines[selectedLineParentIndex ?? 0], selectedLineParentIndex ?? 0, laserSpacing);
                            DrawDrawing();
                            break;
                        case "circle":
                            break;
                        case "arc":
                            break;
                    }
                }
                else
                {

                }
            }


        }
        WpfApplication1.Dxf.Polyline ChangePolylineSpacing(WpfApplication1.Dxf.Polyline pLine, int parentIndex, float laserSpacing)
        {
            pLine.laserLines = new List<WpfApplication1.Dxf.Line>();
            pLine.laserSpacing = laserSpacing;
            if (pLine.noVerticies > 0)
            {
                for (int i = 0; i < pLine.noVerticies; i++)
                {
                    float X = 0;
                    float Y = 0;
                    if (i != (pLine.noVerticies - 1))
                    {
                        X = pLine.verticies[i + 1].Point.X - pLine.verticies[i].Point.X;
                        Y = pLine.verticies[i + 1].Point.Y - pLine.verticies[i].Point.Y;
                    }
                    else
                    {
                        //is the last bit of the loop
                        if (pLine.closed)
                        {
                            X = pLine.verticies[0].Point.X - pLine.verticies[i].Point.X;
                            Y = pLine.verticies[0].Point.Y - pLine.verticies[i].Point.Y;
                        }
                        else
                        {//do nothing
                        }
                    }
                    float lineLength = (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));


                    int numberLines = int.Parse(Math.Round((lineLength / laserSpacing)).ToString());
                    numberLines = numberLines == 0 ? 1 : numberLines;//can't have 0 lines
                    float dX = X == 0 ? 0 : X / numberLines;
                    float dY = Y == 0 ? 0 : Y / numberLines;

                    if (pLine.verticies[i].Bulge == null)
                    {


                        for (int j = 0; j < numberLines; j++)
                        {
                            WpfApplication1.Dxf.Line laserLine = new WpfApplication1.Dxf.Line();
                            laserLine.parentIndex = parentIndex;
                            //straight line

                            laserLine.p1 = new PointF
                            {
                                X = pLine.verticies[i].Point.X + dX * j,
                                Y = pLine.verticies[i].Point.Y + dY * j,
                            };
                            laserLine.p2 = new PointF
                            {
                                X = pLine.verticies[i].Point.X + dX * (j + 1),
                                Y = pLine.verticies[i].Point.Y + dY * (j + 1),
                            };

                            pLine.laserLines.Add(laserLine);
                        }
                    }
                    else
                    {
                        Dxf _dxf = new Dxf();
                        if (i != (pLine.noVerticies - 1))
                        {
                            //link to next vertex in list
                            pLine.laserLines.AddRange(_dxf.ConvertBulgeToLines(pLine.verticies[i].Point, pLine.verticies[i + 1].Point, pLine.verticies[i].Bulge ?? (float)0, MainWindow.lineSpacing, parentIndex));
                        }
                        else
                        {
                            if (pLine.closed)
                            {
                                //link back to the original vertex
                                pLine.laserLines.AddRange(_dxf.ConvertBulgeToLines(pLine.verticies[i].Point, pLine.verticies[0].Point, pLine.verticies[i].Bulge ?? (float)0, MainWindow.lineSpacing, parentIndex));
                            }
                        };
                    };

                }

            }

            return pLine;
        }

        void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            float currentScale = 0;
            float.TryParse(this.textScale.Text, out currentScale);
            if (currentScale != 0)
            {
                currentScale += (float)0.1 * (e.Delta > 0 ? 1 : -1);
            }
            if (currentScale > 0)
            {
                this.textScale.Text = currentScale.ToString();
                DrawDrawing();
            }
        }
        void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (canvasDielectric.IsFocused)
                this.Focus();
        }
        void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (canvasDielectric.IsFocused)
                canvasDielectric.Focus();
        }
        void vScrollBar1_Scroll(object sender, RoutedEventArgs e)
        {
            scrollOffsetY = -1000 * ((float)this.vScrollBar1.Value - (float)0.5);
            DrawDrawing();
        }
        void hScrollBar1_Scroll(object sender, RoutedEventArgs e)
        {
            scrollOffsetX = -1000 * ((float)this.hScrollBar1.Value - (float)0.5);
            DrawDrawing();
        }
        void buttonCenterCanvas_Click(object sender, EventArgs e)
        {
            this.vScrollBar1.Value = 0.5;
            this.hScrollBar1.Value = 0.5;
            scrollOffsetX = 0;
            scrollOffsetY = 0;
            DrawDrawing();
        }
        void buttonRefreshSerial_Click(object sender, RoutedEventArgs e)
        {
            connectToDue();
        }

        //UI Methods
        private void ShowSelectedLine()
        {
            if (selectedLineParentIndex != null)
            {
                Dxf.Line _selectedLine = new Dxf.Line();
                float _laserSpacing = 0;
                switch (selectedType)
                {
                    case "line":
                        foreach (Dxf.Line line in Lines)
                        {
                            if (line.parentIndex == selectedLineParentIndex)
                            {
                                _selectedLine = line;//will always be defined so can do this (set index to 0), though bad practice...
                                _laserSpacing = _selectedLine.laserSpacing;
                            }
                        }
                        break;
                    case "polyline":
                        _selectedLine = Polylines[selectedLineParentIndex ?? 0].laserLines[selectedLaserLineIndex ?? 0];//will always be defined so can do this (set index to 0), though bad practice...
                        _laserSpacing = Polylines[selectedLineParentIndex ?? 0].laserSpacing;
                        break;
                    case "circle":
                        break;
                    case "arc":
                        break;

                }

                this.textSegmentLaserSpacing.Text = _laserSpacing.ToString();
            }
        }
        public void DisplayUnits()
        {
            labelUnits.Dispatcher.BeginInvoke(new Action(delegate
  {
      this.labelUnits.Content = String.Format("units detected: {0}{1}{2}", units.LinearUnitsString, (units.LinearUnitsString != null && units.AngularUnitsString != null) ? ", " : "", units.AngularUnitsString);

  }));
        }
        private double FindDistanceToSegment(PointF pt, PointF p1, PointF p2)
        {
            PointF closest;
            double dist;
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a System.Windows.Point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                dist = Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a System.Windows.Point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }
            dist = Math.Sqrt(dx * dx + dy * dy);
            return dist;
        }
        public void ClearSelectedLine()
        {
            selectedLineParentIndex = null;
            selectedLaserLineIndex = null;
            selectedType = null;
            this.textSegmentLaserSpacing.Text = "";

            foreach (UIElement ui in canvasDielectric.Children)
            {
                if (ui.Uid.StartsWith("MyLine"))
                {
                    if (((MyLine)ui).Clickable)
                    {
                        ((MyLine)ui).Stroke = brush;
                    }
                }
                if (ui.Uid.StartsWith("MyEllipse"))
                {
                    if (((MyEllipse)ui).Clickable)
                    {
                        ((MyEllipse)ui).Stroke = brush;
                    }
                }
            }
        }


        private void x(object sender, DependencyPropertyChangedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        private void y(object sender, DependencyPropertyChangedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        public class Command
        {
            public int index { get; set; }
            public string command { get; set; }
            public FocalLocation pos { get; set; }
        }
        public List<Command> commands = new List<Command>();
        void GenerateCommands()
        {
            commands.Clear();
            int i = 0;
            foreach (Dxf.Line line in Lines)
            {
                //outputCommands.Text += String.Format("[w {0} {1} 0.1]", (float)Math.Round(line.p1.X / 10 + 7, 5), (float)Math.Round(line.p1.Y / 10 + 7, 5));
                float x = (float)Math.Round(line.p1.X, 5);
                float y = (float)Math.Round(line.p1.Y, 5);
                Command command = new Command()
                {
                    index = i,
                    pos = new FocalLocation() { X = x, Y = y },
                    command = String.Format("1 mov 1 {0:0.0000} \n 2 mov 1 {1:0.0000}", x, y),
                };
                commands.Add(command);
                i++;
            }
            foreach (Dxf.Polyline pLine in Polylines)
            {
                foreach (Dxf.Line laserLine in pLine.laserLines)
                {
                    // outputCommands.Text += String.Format("[w {0} {1} 0.1]", (float)Math.Round(laserLine.p1.X / 10 + 7, 5), (float)Math.Round(laserLine.p1.Y / 10 + 7, 5));
                    float x = (float)Math.Round(laserLine.p1.X, 5);
                    float y = (float)Math.Round(laserLine.p1.Y, 5);
                    Command command = new Command()
                    {
                        index = i,
                        pos = new FocalLocation() { X = x, Y = y },
                        command = String.Format("1 mov 1 {0:0.0000} \n 2 mov 1 {1:0.0000}\n", x, y),
                    };
                    commands.Add(command);
                    i++;
                }

            }
        }

        SerialPort sp;
        private void pewpew(object sender, RoutedEventArgs e)
        {
            Thread x = new Thread(() => ExecutePewPew());
            x.Start();
        }
        private void ExecutePewPew()
        {
            GenerateCommands();
            currentPos.isValid = false;
            // string commands = String.Format("{0}*\"", outputCommands.Text);
            if (!sp.IsOpen)
                sp.Open();
            // SerialWrite(commands);
            //sp.Close();
            for (int i = 0; i < commands.Count(); i++)
            {
                SerialWriteLine(commands[i].command);
                while (!isInPosition(commands[i].pos, 0.00001))
                {
                    currentPos.isValid = false;
                    if (!currentPos.isValid)
                        GetCurrentPosition();
                    Thread.Sleep(100);
                    while (!currentPos.isValid) { }
                }
                ShootLaser();

            }

        }
        private void ShootLaser()
        {
            if (arduinoSerial.IsOpen)
                arduinoSerial.Write("r\r\n");
        }
        private bool isInPosition(FocalLocation pos, double tolerance)
        {
            bool _isInPos = false;
            if (pos.X - tolerance <= currentPos.X && currentPos.X <= pos.X + tolerance &&
                pos.Y - tolerance <= currentPos.Y && currentPos.Y <= pos.Y + tolerance)
            {
                _isInPos = true;
            }
            return _isInPos;
        }

        string inputString = "";
        string newCommand = "";
        int indexLineEnds = -1;
        String pos1Regex = "0 1 1=[0-9]+.[0-9]+\n";
        String pos2Regex = "0 2 1=[0-9]+.[0-9]+\n";
        String error1Regex = "0 1 [0-9]+\n";
        String error2Regex = "0 2 [0-9]+\n";
        int serialCount = 0;
        int inputStringLength = 1000000;
        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //this reads in each line. 
            //then, if we're trying to get the current information, we can pick out information
            inputString += sp.ReadExisting();
            indexLineEnds = inputString.IndexOf("\n");
            while (indexLineEnds != -1)
            {
                    Match pos1match = Regex.Match(inputString, pos1Regex);
                    if (pos1match.Value != "")
                    {
                        try
                        {
                            currentPos.X = double.Parse(pos1match.Value.Replace("\n", "").Replace("0 1 1=",""));
                            inputString = inputString.Remove(0, pos1match.Index + pos1match.Value.Length);
                            readingPosition = false;
                            currentPos.isValid = true;
                            Thread x = new Thread(() => updatePos(currentPos.X, currentPos.Y));
                            x.Start();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    Match pos2match = Regex.Match(inputString, pos2Regex);
                    if (pos2match.Value != "")
                    {
                        try
                        {
                            currentPos.Y = double.Parse(pos2match.Value.Replace("\n", "").Replace("0 2 1=", ""));
                            inputString = inputString.Remove(0, pos2match.Index + pos2match.Value.Length);
                            readingPosition = false;
                            currentPos.isValid = true;
                            Thread x = new Thread(() => updatePos(currentPos.X, currentPos.Y));
                            x.Start();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                Match error1match = Regex.Match(inputString, error1Regex);
                if (error1match.Value != "")
                {
                    try
                    {
                        inputString = inputString.Remove(0, error1match.Index + error1match.Value.Length);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Match error2match = Regex.Match(inputString, error2Regex);
                if (error2match.Value != "")
                {
                    try
                    {
                        inputString = inputString.Remove(0, error2match.Index + error2match.Value.Length);
                    }
                    catch (Exception ex)
                    {
                    }
                    
                }
                //LEAVE THE THIS IN!!!!
                indexLineEnds = inputString.IndexOf("\n");
                if (inputString.Length == inputStringLength)
                {
                    serialCount++;
                    if (serialCount > 10)
                    {
                        indexLineEnds = -1;
                    }
                }
                else
                {
                    serialCount = 0;
                    inputStringLength = inputString.Length;
                }
                //LEAVE THE THIS IN!!!!
            }
        }
        #endregion



        private void updatePos(double x,double y)
        {
            this.laserCurrentPosition.Dispatcher.Invoke(new Action(delegate
            {
                this.laserCurrentPosition.Content = String.Format("Current position: ({0:0.00000},{1:0.00000})", x,y);
            }));
        }

        private void textLineSpacing_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        public void Move(double xIncrement, double yIncrement)
        {
            double x = currentPos.X + xIncrement;
            double y = currentPos.Y + yIncrement;
            if (0 <= y && y <= 15 && 0 <= x && x <= 19)
            {
                SerialWrite(String.Format("1 err? \n"));
                SerialWrite(String.Format("2 err? \n"));
                SerialWrite(String.Format("1 mov 1 {0:0.0000}\n", x));
                SerialWrite(String.Format("2 mov 1 {0:0.0000}\n", y));

                FocalLocation fl = new FocalLocation()
                {
                    X = x,
                    Y = y,
                };
                int count = 0;
                while (!isInPosition(fl, 0.00001))
                {
                    currentPos.isValid = false;
                    if (!currentPos.isValid)
                    {
                        GetCurrentPosition();
                    }
                    Thread.Sleep(100);
                    if (count > 5)
                    {
                        break;
                    }
                    count++;
                }
            }
            clickingArrowButtons = false;
        }
        public void SerialWrite(string text)
        {
            if (sp.IsOpen)
            {
                sp.Write(text);
                Thread.Sleep(500);
            }
        }
        public void SerialWriteLine(string text)
        {
            if (sp.IsOpen)
            {
                sp.WriteLine(text);
                Thread.Sleep(500);
            }
        }
        private static readonly Object serialWriteLock = new Object();
        bool dontStop = false;
        public void MoveAbsolute(double x, double y)
        {
            if (0 <= y && y <= 15 && 0 <= x && x <= 19)
            {
                dontStop = true;
                SerialWrite(String.Format("1 err? \n"));
                SerialWrite(String.Format("2 err?\n"));
                SerialWrite(String.Format("1 mov 1 {0:0.0000}\n", x));
                SerialWrite(String.Format("2 mov 1 {0:0.0000}\n", y));

                FocalLocation fl = new FocalLocation()
                {
                    X = x,
                    Y = y,
                };
                int count = 0;
                dontStop = false;
                while (!isInPosition(fl, 0.00001)&&!dontStop)
                {
                    currentPos.isValid = false;
                    if (!currentPos.isValid)
                    {
                        GetCurrentPosition();
                    }
                    Thread.Sleep(100);
                    if (count > 5)
                    {
                        SerialWrite(String.Format("1 err? \n"));
                        SerialWrite(String.Format("2 err? \n"));
                        SerialWrite(String.Format("1 mov 1 {0:0.0000}\n", x));
                        SerialWrite(String.Format("2 mov 1 {0:0.0000}\n", y));
                    }
                    count++;
                }
            }
        }
        bool readingPosition = false;
        public void GetCurrentPosition()
        {
            readingPosition = true;
            SerialWrite("1 pos?\n");
            SerialWrite("2 pos?\n");

        }
        FocalLocation currentPos = new FocalLocation()
        {
            isValid = false,
            X = 0,
            Y = 0
        };
        public class ScrollingTextBox : System.Windows.Controls.TextBox
        {

            protected override void OnInitialized(EventArgs e)
            {
                base.OnInitialized(e);
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            protected override void OnTextChanged(TextChangedEventArgs e)
            {
                base.OnTextChanged(e);
                CaretIndex = Text.Length;
                ScrollToEnd();
            }

        }

        private void button_MoveRight_Click(object sender, RoutedEventArgs e)
        {
            Thread x = new Thread(() => Move((courseMovementSelected ? coarseMovement : fineMovement), 0));
            x.Start();
            //if (!readingPosition)
            //{
            //    if (!currentPos.isValid)
            //        GetCurrentPosition();
            //    while (!currentPos.isValid) { }
               
            //    Thread.Sleep(500);
            //    GetCurrentPosition();
            //    while (!currentPos.isValid) { }
            //}
        }
        private void buttonMoveUp_click(object sender, RoutedEventArgs e)
        {
            Thread x = new Thread(() => Move(0, (courseMovementSelected ? coarseMovement : fineMovement)));
            x.Start();
            //if (!readingPosition)
            //{
            //    if (!currentPos.isValid)
            //        GetCurrentPosition();
            //    while (!currentPos.isValid) { }
            //    Move(0, (courseMovementSelected ? coarseMovement : fineMovement));
            //    Thread.Sleep(500);
            //    while (!currentPos.isValid) { }
            //}
        }
        bool clickingArrowButtons = false;
        private void button_MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (!clickingArrowButtons)
            {
                clickingArrowButtons = true;
                Thread x = new Thread(() => Move(-(courseMovementSelected ? coarseMovement : fineMovement), 0));
                x.Start();
            }
        }

        private void button_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            Thread x = new Thread(() => Move(0, -(courseMovementSelected ? coarseMovement : fineMovement)));
            x.Start();
            
            //if (!readingPosition)
            //{
            //    if (!currentPos.isValid)
            //        GetCurrentPosition();
            //    while (!currentPos.isValid) { }
            //    
            //    Thread.Sleep(500);
            //    GetCurrentPosition();
            //    while (!currentPos.isValid) { }
            //}
        }
        double coarseMovement = 2;
        double fineMovement = 0.1;
        bool courseMovementSelected = true;
        private void movementChanged(object sender, RoutedEventArgs e)
        {
            courseMovementSelected = this.movementCoarse.IsChecked ?? false;
        }

        private void button_Pew_Click(object sender, RoutedEventArgs e)
        {
            ShootLaser();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SerialWrite("[y]*");
        }

        private void textSegmentLaserSpacing_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void tbCoarseMovement_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                if (double.TryParse(this.tbCoarseMovement.Text, out coarseMovement))
                {
                    //all good
                }
                else
                {
                    //oop! 
                    this.tbCoarseMovement.Text = coarseMovement.ToString();
                }

            }
        }
        private void tbFineMovement_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                if (double.TryParse(this.tbFineMovement.Text, out fineMovement))
                {
                    //all good
                }
                else
                {
                    //oop! 
                    this.tbFineMovement.Text = fineMovement.ToString();
                }

            }
        }

        private void tbAbsMoveX_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                double newXPos = 0;
                if (double.TryParse(this.tbAbsMoveX.Text, out newXPos))
                {
                    //all good
                    Thread X = new Thread(()=>MoveAbsolute(newXPos, currentPos.Y));
                    X.Start();
                    
                }
                else
                {
                    //oop! 
                    this.tbAbsMoveX.Text = currentPos.X.ToString();
                }

            }
        }
        private void tbAbsMoveY_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                double newYPos = 0;
                if (double.TryParse(this.tbAbsMoveY.Text, out newYPos))
                {
                    //all good
                    Thread X = new Thread(() => MoveAbsolute(currentPos.X, newYPos));
                    X.Start();

                }
                else
                {
                    //oop! 
                    this.tbAbsMoveY.Text = currentPos.Y.ToString();
                }

            }
        }
        private void tbOffsetY_KeyUp(object sender, KeyEventArgs e)
        {
            double offsetY=0;
            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                if (double.TryParse(this.tbOffsetY.Text, out offsetY))
                {
                    //all good
                    Properties.Settings.Default["offsetY"] = offsetY;
                }
                else
                {
                    //oop! 
                    this.tbOffsetY.Text = Properties.Settings.Default["offsetY"].ToString();
                }

            }
        }
        private void tbOffsetX_KeyUp(object sender, KeyEventArgs e)
        {
            double offsetX = 0;
            if (e.Key == Key.Enter)
            {
                this.focusLabel.Focus();
                if (double.TryParse(this.tbOffsetX.Text, out offsetX))
                {
                    //all good
                }
                else
                {
                    //oop! 
                    this.tbOffsetX.Text = Properties.Settings.Default["offsetX"].ToString();
                }

            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Move(0, 0);
        }
        private void offsetObjective_checked(object sender, RoutedEventArgs e)
        {
            Thread X = new Thread(() => Move((double)Properties.Settings.Default["offsetX"], (double)Properties.Settings.Default["offsetY"]));
            X.Start();
        }

        private void offsetMicroscope_checked(object sender, RoutedEventArgs e)
        {
            Thread X = new Thread(() => Move(-(double)Properties.Settings.Default["offsetX"], -(double)Properties.Settings.Default["offsetY"]));
            X.Start();
        }



    }
    public class FocalLocation
    {
        public bool isValid { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class MyLine : Shape
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public int? ParentIndex { get; set; }
        public bool Clickable { get; set; }

        protected override Geometry
                          DefiningGeometry
        {
            get
            {
                LineGeometry line = new LineGeometry(
                   new System.Windows.Point(X1, Y1),
                      new System.Windows.Point(X2, Y2));
                return line;
            }
        }
    }
    public class MyEllipse : Shape
    {

        public System.Windows.Point Center { get; set; }
        public bool Clickable { get; set; }
        public double Radius { get; set; }
        public int? ParentIndex { get; set; }

        protected override Geometry
                          DefiningGeometry
        {
            get
            {
                EllipseGeometry ellipse = new EllipseGeometry()
                {
                    Center = Center,
                    RadiusX = Radius,
                    RadiusY = Radius,


                };
                return ellipse;
            }
        }

    }

}
