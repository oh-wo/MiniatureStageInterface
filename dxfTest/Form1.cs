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
using Microsoft.Expression;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using Microsoft.Expression.Encoder.Plugins;
using System.Runtime.InteropServices;

namespace dxfTest
{

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        // Global variables
        string[] file;
        public PointF Origin;
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
        private Graphics _Graphics;
        private Bitmap image;
        public Pen pen;
        public Pen selectedPen;
        public Pen yaxisPen;
        public Pen xaxisPen;
        public Pen _laserPen;
        public Pen stageBoundsPen;
        public Color drawingBackgroundColour = Color.FromArgb(34, 41, 51);
        public Color drawingPenColour = Color.White;
        public float drawingHeight = (float)0;
        public float drawingWidth = (float)0;
        public string fileDirectory = String.Format("{0}\\Files\\", Application.StartupPath);
        public string fullFile = String.Format("{0}\\Files\\Drawing1_1.dxf", Application.StartupPath);
        public int? selectedLineParentIndex = null;
        public int? selectedLaserLineIndex = null;
        public string selectedType;
        public float scrollOffsetX = 0;
        public float scrollOffsetY = 0;
        List<EncoderDevice> lstVideoDevices = new List<EncoderDevice>();
        List<EncoderDevice> lstAudioDevices = new List<EncoderDevice>();
        private LiveJob _job;
        private LiveDeviceSource _deviceSource;
        private bool _bStartedRecording = false;
        ImageList imgList = new ImageList();
        public Form1()
        {
            
            
            InitializeComponent();
            /*General initializing */
            DisplayAvailableSerialPorts();

            //Prepare drawing
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            stageBounds = new Dxf.StageBounds()
            {
                X = new float[2] { -bound, bound },
                Y = new float[2] { -bound, bound },
            };
            pen = new Pen(drawingPenColour, 1);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
            selectedPen = new Pen(System.Drawing.Color.Orange, 1);
            selectedPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
            xaxisPen = new Pen(System.Drawing.Color.Red, 1);
            yaxisPen = new Pen(System.Drawing.Color.Green, 1);
            stageBoundsPen = new Pen(System.Drawing.Color.Green, 1);
            /*Dxf stuff - to be shifted later to 'open file' etc */
            Dxf dxf = new Dxf();
            this.SubscribeDxf(dxf);
            Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
            dxfThread.Name = "Dxf thread";
            dxfThread.Start();

            /*Serial Communication begins */
            SerialComs sComs = new SerialComs();
            this.Subscribe(sComs);
            Thread x = new Thread(() => sComs.Start(sPort));
            x.Start();

            /*General UI setup */
            this.labelCurrentFile.Text = System.IO.Path.GetFileName(fullFile);
            this.textLineSpacing.Text = lineSpacing.ToString();
            ImageList z = new ImageList();
            z.TransparentColor = Color.Blue;
            z.Images.Add(image);

            /*Prepare Cameras*/
            
            foreach (EncoderDevice edv in EncoderDevices.FindDevices(EncoderDeviceType.Video))
            {
                lstVideoDevices.Add(edv);
            }
            foreach (EncoderDevice edv in EncoderDevices.FindDevices(EncoderDeviceType.Audio))
            {
                lstAudioDevices.Add(edv);
            }
            StartCamera();
        }
        #region Femtosecond Machining
        public void StartCamera()
        {
            // Starts new job for preview window
            _job = new LiveJob();
            // Create a new device source. We use the first audio and video devices on the system
            _deviceSource = _job.AddDeviceSource(lstVideoDevices[0], lstAudioDevices[0]);

            // Sets preview window to winform panel hosted by xaml window
            _deviceSource.PreviewWindow = new PreviewWindow(new HandleRef(panel1, panel1.Handle));

            // Make this source the active one
            _job.ActivateSource(_deviceSource);
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
            this.comboSerialPorts.Items.Clear();
            foreach (string comPort in SerialPort.GetPortNames())
            {
                this.comboSerialPorts.Items.Add(comPort);
            }
            if (this.comboSerialPorts.Items.Count > 0)
            {
                this.comboSerialPorts.SelectedIndex = 1;
            }
        }
        public void GetDxfData(List<Dxf.Polyline> polylines)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { GetDxfData(polylines); }));
            }
            else
            {
                Polylines = polylines;
            }
        }

        //Dxf stuff
        public void DrawDrawing()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { DrawDrawing(); }));
            }
            else
            {
                _Graphics = Graphics.FromImage(image);
                _Graphics.Clear(drawingBackgroundColour);

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
                scale /= (float)(0.075 / pictureBox1.Width);
                float xOffset = pictureBox1.Width / 2 ; // // +drawingWidth / (2 * scale);(float)this.hScrollBar1.Value / 100 * 
                float yOffset = pictureBox1.Height / 2;//(float)this.vScrollBar1.Value / 100 * 
                for (int j = 0; j < Polylines.Count; j++)
                {
                    for (int i = 0; i < Polylines[j].laserLines.Count; i++)
                    {
                        Polylines[j].laserLines[i] = DrawLine(Polylines[j].laserLines[i].p1.X, Polylines[j].laserLines[i].p1.Y, Polylines[j].laserLines[i].p2.X, Polylines[j].laserLines[i].p2.Y, xOffset, yOffset, scale, true, pen, Polylines[j].laserLines[i], true, "polyline");
                    }
                }
                for (int j = 0; j < Lines.Count; j++)
                {
                    Lines[j] = DrawLine(Lines[j].p1.X, Lines[j].p1.Y, Lines[j].p2.X, Lines[j].p2.Y, xOffset, yOffset, scale, true, pen, Lines[j], true, "line");
                }
                foreach (Dxf.Circle circle in Circles)
                {
                    for (int i = 0; i < circle.laserLines.Count; i++)
                    {
                        circle.laserLines[i] = DrawLine(circle.laserLines[i].p1.X, circle.laserLines[i].p1.Y, circle.laserLines[i].p2.X, circle.laserLines[i].p2.Y, xOffset, yOffset, scale, true, pen, circle.laserLines[i], true, "circle");
                    }
                }
                if (this.checkDisplayOrigin.Checked)
                {
                    DrawOrigin(xOffset, yOffset, scale);
                }
                else
                {
                    //user doesn't want to display the origin
                }
                if (this.checkStageBounds.Checked)
                {
                    DrawStageBounds(xOffset, yOffset, scale);
                }
                else
                {
                    //user doesn't want to display the origin
                }
                pictureBox1.BackColor = Color.Transparent;
                this.pictureBox1.Invoke(new Action(() =>
                {
                    this.pictureBox1.Image = image;
                    this.pictureBox1.Refresh();
                    this.pictureBox1.Invalidate();
                }));
            }
        }
        public Dxf.Line DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad, Pen pen, Dxf.Line line, bool Clickable, string specifiedType)
        {
            //if from autocad then have to flip the y-axis
            float x1 = fromAutoCad ? X1 * scale * units.LinearConversionFactor + xOffset + scrollOffsetX : X1 * scale + xOffset + scrollOffsetX;
            float y1 = fromAutoCad ? pictureBox1.Height - ((Y1) * scale * units.LinearConversionFactor + yOffset) + scrollOffsetY : Y1 * scale + yOffset + scrollOffsetY;
            float x2 = fromAutoCad ? X2 * scale * units.LinearConversionFactor + xOffset + scrollOffsetX : X2 * scale + xOffset + scrollOffsetX;
            float y2 = fromAutoCad ? pictureBox1.Height - ((Y2) * scale * units.LinearConversionFactor + yOffset) + scrollOffsetY : Y2 * scale + yOffset + scrollOffsetY;
            if (selectedLineParentIndex != null && Clickable)
            {
                _Graphics.DrawLine((selectedLineParentIndex ?? 0) == line.parentIndex && selectedType == specifiedType ? selectedPen : pen, x1, y1, x2, y2);
            }
            else
            {
                _Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            line.plotted1 = new PointF()
            {
                X = fromAutoCad ? X1 * scale * units.LinearConversionFactor + xOffset : X1 * scale + xOffset,
                Y = fromAutoCad ? pictureBox1.Height - ((Y1) * scale * units.LinearConversionFactor + yOffset) : Y1 * scale + yOffset,
            };
            line.plotted2 = new PointF()
            {
                X = fromAutoCad ? X2 * scale * units.LinearConversionFactor + xOffset : X2 * scale + xOffset,
                Y = fromAutoCad ? pictureBox1.Height - ((Y2) * scale * units.LinearConversionFactor + yOffset) : Y2 * scale + yOffset,
            };
            return line;
        }
        public Dxf.Line DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad)
        {
            Dxf.Line line = new Dxf.Line();
            line = DrawLine(X1, Y1, X2, Y2, xOffset, yOffset, scale, fromAutoCad, pen, line, false, "");
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
                DrawLine(Origin.X - xWidth, Origin.Y, Origin.X + xWidth, Origin.Y, xOffset, yOffset, scale, false, xaxisPen, line, false, "");
                //y-axis
                DrawLine(Origin.X, Origin.Y - yWidth, Origin.X, Origin.Y + yWidth, xOffset, yOffset, scale, false, yaxisPen, line, false, "");
            }
        }
        public void DrawStageBounds(float xOffset, float yOffset, float scale)
        {
            Dxf.Line line = new Dxf.Line();
            if (stageBounds != null)
            {
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[0], xOffset, yOffset, scale, false, stageBoundsPen, line, false, "");//bottom
                DrawLine(stageBounds.X[0], stageBounds.Y[1], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false, "");//top
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[0], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false, "");//left
                DrawLine(stageBounds.X[1], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false, "");//right
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
        public Dxf.Polyline ChangePolylineSpacing(Dxf.Polyline pline, int parentIndex,float _lineSpacing)
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

        //UI Event stuff
        void buttChangeFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = String.Format("{0}\\Files\\", Application.StartupPath);
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fullFile = this.openFileDialog1.FileName;
                Dxf dxf = new Dxf();
                this.SubscribeDxf(dxf);
                Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
                dxfThread.Name = "Dxf";
                dxfThread.Start();
                this.labelCurrentFile.Text = System.IO.Path.GetFileName(fullFile);
            }
        }
        void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool thereIsALineSelected = false;
            Point mouseDownLocation = new Point(e.X, e.Y);
            for (int j = 0; j < Lines.Count; j++)
            {
                if (FindDistanceToSegment(new PointF() { X = e.X, Y = e.Y }, Lines[j].plotted1, Lines[j].plotted2) < 10)
                {
                    selectedLineParentIndex = Lines[j].parentIndex;
                    selectedType = "line";
                    DrawDrawing();
                    ShowSelectedLine();
                    thereIsALineSelected = true;
                    break;
                }
                else
                {
                }
            }
            for (int i = 0; i < Polylines.Count; i++)
            {
                for (int j = 0; j < Polylines[i].laserLines.Count; j++)
                {
                    if (FindDistanceToSegment(new PointF() { X = e.X, Y = e.Y }, Polylines[i].laserLines[j].plotted1, Polylines[i].laserLines[j].plotted2) < 10)
                    {
                        selectedLineParentIndex = i;
                        selectedLaserLineIndex = j;
                        selectedType = "polyline";
                        DrawDrawing();
                        ShowSelectedLine();
                        thereIsALineSelected = true;
                        break;
                    }
                    else
                    {
                    }
                }
            }
            if (!thereIsALineSelected)
            {
                ClearSelectedLine();
                DrawDrawing();

            }
        }
        void checkStageBounds_CheckStateChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        void checkDisplayOrigin_CheckStateChanged(object sender, EventArgs e)
        {
            DrawDrawing();
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
                        Polylines[selectedLineParentIndex??0]=ChangePolylineSpacing(Polylines[selectedLineParentIndex??0],selectedLineParentIndex??0,laserSpacing);
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
        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
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
            if (pictureBox1.Focused)
                pictureBox1.Parent.Focus();
        }
        void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (!pictureBox1.Focused)
                pictureBox1.Focus();
        }
        void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            scrollOffsetY = -10*(this.vScrollBar1.Value - 50);
            DrawDrawing();
        }
        void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            scrollOffsetX = -10 * (this.hScrollBar1.Value - 50);
            DrawDrawing();
        }
        void buttonCenterPicturebox1_Click(object sender, EventArgs e)
        {
            this.vScrollBar1.Value = 50;
            this.hScrollBar1.Value = 50;
            scrollOffsetX = 0;
            scrollOffsetY = 0;
            DrawDrawing();
        }
        void buttonRefreshSerial_Click(object sender, EventArgs e)
        {
            DisplayAvailableSerialPorts();
        }

        //UI Methods
        private void ShowSelectedLine()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { ShowSelectedLine(); }));
            }
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
                this.labelSegmentName.Text = String.Format("Index:{4} ({0},{1}) -> ({2},{3})", _selectedLine.p1.X, _selectedLine.p1.Y, _selectedLine.p2.X, _selectedLine.p2.Y, selectedLaserLineIndex);
                this.textSegmentLaserSpacing.Text = _laserSpacing.ToString();
            }
        }
        public void DisplayUnits()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { DisplayUnits(); }));
            }
            else
            {
                this.labelUnits.Text = String.Format("units detected: {0}{1}{2}", units.LinearUnitsString, (units.LinearUnitsString != null && units.AngularUnitsString != null) ? ", " : "", units.AngularUnitsString);
            }
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
            this.labelSegmentName.Text = "";
            this.textSegmentLaserSpacing.Text = "";
        }

        #endregion

        public class TransparentPicture : PictureBox
        {
            public bool IsTransparent { get; set; }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;

                    if (this.IsTransparent)
                    {
                        cp.ExStyle |= 0x20;
                    }

                    return cp;
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                if (!this.IsTransparent)
                {
                    base.OnPaintBackground(e);
                }
            }

            protected override void OnMove(EventArgs e)
            {
                if (this.IsTransparent)
                {
                    RecreateHandle();
                }
                else
                {
                    base.OnMove(e);
                }
            }
        }
    }
}