﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Expression.Encoder.Devices;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        public static float lineSpacing = 20;
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
        public string fullFile = String.Format("{0}\\Files\\Drawing1.dxf", System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
        public int? selectedLineParentIndex = null;
        public int? selectedLaserLineIndex = null;
        public string selectedType;
        public float scrollOffsetX = 0;
        public float scrollOffsetY = 0;
        public Image image;
        private DrawingContext _Context;



        private delegate void TextChanger();
        public MainWindow()
        {
            InitializeComponent();
            /*General initializing */
            // canvasDielectric.Children.Add(new Line() { X1 = 5, X2 = 100, Y1 = 5, Y2 = 100, StrokeThickness = 2, Stroke = System.Windows.Media.Brushes.Orange });
            //DrawLine(6, 101, 6, 101, 0, 0, 0, false);
            FindDevices();
            DisplayAvailableSerialPorts();

            //Canvas background colour opaque by default
            canvasDielectric.Background = new SolidColorBrush(Color.FromRgb(34, 41, 51));
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



        }

        #region Camera

        private void FindDevices()
        {
            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);

            foreach (EncoderDevice dvc in vidDevices)
            {
                comboVideo.Items.Add(dvc.Name);
            }
            if (comboVideo.Items.Count > 0)
            {
                comboVideo.SelectedIndex = 0;
                RoutedEventArgs e = new RoutedEventArgs();
                object sender = new object();
                buttonCameraStart_Click(sender, e);
            };

        }


        #endregion

        #region Femtosecond Machining
        public void DisplayAvailableSerialPorts()
        {

            this.comboSerial.Items.Clear();
            foreach (string comPort in SerialPort.GetPortNames())
            {
                this.comboSerial.Items.Add(comPort);
            }
            if (this.comboSerial.Items.Count > 0)
            {
                this.comboSerial.SelectedIndex = 1;
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
                Uid = "MyLine",
            };
            li.Stroke = b;
            
            li.MouseDown += new MouseButtonEventHandler(line_MouseDown);
            canvasDielectric.Children.Add(li);
            if (fromAutoCad){
                MyEllipse ellipse = new MyEllipse() {
                    Clickable = true,
                    Uid = "MyEllipse",
                    ParentIndex = line.parentIndex,
                    Center = new Point() { X = (double)x2, Y = (double)y2 },
                    Radius = 2,
                    Stroke = b,
                    StrokeThickness = 5,
                    
                };
                ellipse.MouseDown+= new MouseButtonEventHandler(ellipse_MouseDown);
                canvasDielectric.Children.Add(ellipse);}

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
        public List<Dxf.Line> ChangeLaserLineSpacing(List<Dxf.Line> _lines, float NewSpacing)
        {
            /*Function:
             * finds the index of the lines which are to be changed
             * removes those lines
             * splits the span of the lines up into new lines for that span
             * inserts these new lines at the previously found index, giving them the same parent index before
             * */
            int? linesStartIndex = null;
            int? linesEndIndex = null;
            bool firstFound = false;
            Dxf.Line OriginalLine = new Dxf.Line();
            List<Dxf.Line> NewLines = new List<Dxf.Line>();
            for (int i = 0; i < _lines.Count; i++)
            {
                if (_lines[i].parentIndex == selectedLineParentIndex)
                {

                    if (!firstFound)
                    {
                        linesStartIndex = i;
                        firstFound = true;
                        OriginalLine.p1 = _lines[i].p1;
                        OriginalLine.parentIndex = selectedLineParentIndex ?? 0;

                    }
                    OriginalLine.p2 = _lines[i].p2;//will get overwritten again and again until last line is found
                    linesEndIndex = i;
                }
                else
                {
                    //error
                }
            }
            if (linesStartIndex != null && linesEndIndex != null)
            {
                Dxf dxf = new Dxf();
                NewLines = dxf.ConvertLineToLaserLines(OriginalLine, NewSpacing, (selectedLineParentIndex ?? 0));
                foreach (Dxf.Line line in NewLines)
                {
                    line.laserSpacing = NewSpacing;
                }
                //remove the old lines
                for (int i = linesEndIndex ?? 0; i >= linesStartIndex; i--)
                {
                    _lines.Remove(_lines[i]);
                }
                _lines.InsertRange(linesStartIndex ?? 0, NewLines);

            }
            return _lines;
        }
        public Dxf.Polyline ChangePolylineSpacing(Dxf.Polyline pline, int parentIndex, float _lineSpacing)
        {
            Dxf dxf = new Dxf();

            pline.laserLines = new List<Dxf.Line>();
            pline.laserSpacing = _lineSpacing;
            if (pline.noVerticies > 0)
            {
                for (int i = 0; i < pline.noVerticies; i++)
                {
                    //display verticies to the user
                    //this.textOutput.Text += String.Format("          Vertex {0}: ({1},{2}) {3}\r\n", i, pline.verticies[i].Point.X, pline.verticies[i].Point.Y, pline.verticies[i].Buldge != null ? pline.verticies[i].Buldge.ToString() : "");


                    //convert vertex to laser line and convert bulges to lines if necessary. 

                    if (pline.verticies[i].Bulge == null)
                    {
                        Dxf.Line laserLine = new Dxf.Line();
                        //straight line
                        laserLine.p1 = new PointF
                        {
                            X = pline.verticies[i].Point.X,
                            Y = pline.verticies[i].Point.Y,
                        };
                        if (i != (pline.noVerticies - 1))
                        {
                            //link to next vertex in list
                            laserLine.p2 = new PointF
                            {
                                X = pline.verticies[i + 1].Point.X,
                                Y = pline.verticies[i + 1].Point.Y,
                            };
                        }
                        else
                        {
                            if (pline.closed)
                            {
                                //link back to the original vertex
                                laserLine.p2 = new PointF
                                {
                                    X = pline.verticies[0].Point.X,
                                    Y = pline.verticies[0].Point.Y,
                                };
                            }
                            else
                            {
                                //do nothing
                            }
                        };
                        pline.laserLines.Add(laserLine);
                    }
                    else
                    {
                        if (i != (pline.noVerticies - 1))
                        {
                            //link to next vertex in list
                            pline.laserLines.AddRange(dxf.ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[i + 1].Point, pline.verticies[i].Bulge ?? (float)0, _lineSpacing, parentIndex));
                        }
                        else
                        {
                            if (pline.closed)
                            {
                                //link back to the original vertex
                                pline.laserLines.AddRange(dxf.ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[0].Point, pline.verticies[i].Bulge ?? (float)0, _lineSpacing, parentIndex));
                            }
                        };
                    };

                }

            };
            return pline;
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
                        Polylines[j].laserLines[i] = DrawLine(Polylines[j].laserLines[i].p1.X, Polylines[j].laserLines[i].p1.Y, Polylines[j].laserLines[i].p2.X, Polylines[j].laserLines[i].p2.Y, xOffset, yOffset, scale, true, brush, Polylines[j].laserLines[i], true, "polyline");
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
            MyLine _selLine = ((MyLine)sender);
            selectedType = "line";

            foreach (UIElement ui in canvasDielectric.Children)
            {
                if (ui.Uid.StartsWith("MyLine"))
                {
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
                }
                if (ui.Uid.StartsWith("MyEllipse"))
                {
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
                }

            }
            ShowSelectedLine();
        }
        void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MyEllipse _selEll = ((MyEllipse)sender);
            selectedType = "line";

            foreach (UIElement ui in canvasDielectric.Children)
            {
                if (ui.Uid.StartsWith("MyLine"))
                {
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

            }
            ShowSelectedLine();
        }
        void canvasDielectric_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType().Name == "Canvas")
            {
                ClearSelectedLine();

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
        void checkCamera_Checked(object sender, EventArgs e)
        {
            canvasDielectric.Background = new SolidColorBrush(Colors.Transparent);
        }
        void checkCamera_Unchecked(object sender, EventArgs e)
        {
            canvasDielectric.Background = new SolidColorBrush(Color.FromRgb(34, 41, 51));
        }
        void textScale_LostFocus(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void textLineSpacing_LostFocus(object sender, EventArgs e)
        {
            ClearSelectedLine();
            int userInput = int.Parse(this.textLineSpacing.Text);
            lineSpacing = userInput <= 0 ? 1 : userInput;
            Dxf dxf = new Dxf();
            this.SubscribeDxf(dxf);
            Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
            dxfThread.Name = "Dxf";
            dxfThread.Start();
        }
        void textSegmentLaserSpacing_LostFocus(object sender, EventArgs e)
        {
            float laserSpacing = 0;
            float.TryParse(this.textSegmentLaserSpacing.Text, out laserSpacing);
            if (laserSpacing != 0 && selectedLineParentIndex != null)
            {
                switch (selectedType)
                {
                    case "line":
                        Lines = ChangeLaserLineSpacing(Lines, laserSpacing);
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
            this.vScrollBar1.Value = 50;
            this.hScrollBar1.Value = 50;
            scrollOffsetX = 0;
            scrollOffsetY = 0;
            DrawDrawing();
        }
        void buttonRefreshSerial_Click(object sender, RoutedEventArgs e)
        {
            DisplayAvailableSerialPorts();
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
                this.labelSegmentName.Content = String.Format("Index:{4} ({0},{1}) -> ({2},{3})", _selectedLine.p1.X, _selectedLine.p1.Y, _selectedLine.p2.X, _selectedLine.p2.Y, selectedLaserLineIndex);
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
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                dist = Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
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
            this.labelSegmentName.Content = "";
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

        #endregion

        private void buttonCameraStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Binding bndg_1 = new Binding("SelectedValue");
                bndg_1.Source = comboVideo;

                videoCapElement.SetBinding(WPFMediaKit.DirectShow.Controls.VideoCaptureElement.VideoCaptureSourceProperty, bndg_1);
                //WebCamCtrl.VidFormat = VideoFormat.mp4;
                videoCapElement.FPS = 500;
                videoCapElement.Height = 500;
                videoCapElement.Width = 500;
                // Display webcam video on control.
                videoCapElement.Play();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
        }



    }
    public class MyLine : Shape
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public int ParentIndex { get; set; }
        public bool Clickable { get; set; }

        protected override Geometry
                          DefiningGeometry
        {
            get
            {
                LineGeometry line = new LineGeometry(
                   new Point(X1, Y1),
                      new Point(X2, Y2));
                return line;
            }
        }
    }
    public class MyEllipse : Shape
    {

        public Point Center { get; set; }
        public bool Clickable { get; set; }
        public double Radius { get; set; }
        public int ParentIndex { get; set; }

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