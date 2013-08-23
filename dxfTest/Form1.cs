﻿using System;
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
        public string selectedType;

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
            Thread dxfThread = new Thread(() => dxf.Start(fullFile,lineSpacing));
            dxfThread.Name = "Dxf thread";
            dxfThread.Start();
            /*Serial Communication begins */
            SerialComs sComs = new SerialComs();
            this.Subscribe(sComs);
            Thread x = new Thread(sComs.Start);
            x.Start();
            this.labelCurrentFile.Text = System.IO.Path.GetFileName(fullFile);
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
                float xOffset = pictureBox1.Width / 2; // // +drawingWidth / (2 * scale);(float)this.hScrollBar1.Value / 100 * 
                float yOffset = pictureBox1.Height / 2;//(float)this.vScrollBar1.Value / 100 * 
                for (int j = 0; j < Polylines.Count; j++ )
                {
                    for (int i = 0; i < Polylines[j].laserLines.Count; i++)
                    {
                        Polylines[j].laserLines[i] = DrawLine(Polylines[j].laserLines[i].p1.X, Polylines[j].laserLines[i].p1.Y, Polylines[j].laserLines[i].p2.X, Polylines[j].laserLines[i].p2.Y, xOffset, yOffset, scale, true, pen, Polylines[j].laserLines[i],true);
                    }
                }
                for (int j = 0; j < Lines.Count; j++)
                {
                    Lines[j] = DrawLine(Lines[j].p1.X, Lines[j].p1.Y, Lines[j].p2.X, Lines[j].p2.Y, xOffset, yOffset, scale, true,pen,Lines[j],true);
                }
                foreach (Dxf.Circle circle in Circles)
                {
                    for (int i = 0; i < circle.laserLines.Count; i++)
                    {
                        circle.laserLines[i]=DrawLine(circle.laserLines[i].p1.X, circle.laserLines[i].p1.Y, circle.laserLines[i].p2.X, circle.laserLines[i].p2.Y, xOffset, yOffset, scale, true, pen, circle.laserLines[i], true);
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
                this.pictureBox1.Invoke(new Action(() =>
                {
                    this.pictureBox1.Image = image;
                    this.pictureBox1.Refresh();
                    this.pictureBox1.Invalidate();
                }));
            }
        }
        public Dxf.Line DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad, Pen pen, Dxf.Line line,bool Clickable)
        {
            //if from autocad then have to flip the y-axis
            float x1 = fromAutoCad? X1 * scale * units.LinearConversionFactor + xOffset : X1 * scale + xOffset;
            float y1 = fromAutoCad ? pictureBox1.Height -((Y1) * scale * units.LinearConversionFactor + yOffset) : Y1 * scale + yOffset;
            float x2 = fromAutoCad ? X2 * scale * units.LinearConversionFactor + xOffset : X2 * scale + xOffset;
            float y2 = fromAutoCad ? pictureBox1.Height -((Y2) * scale * units.LinearConversionFactor + yOffset) : Y2 * scale + yOffset;
            if (selectedLineParentIndex != null && Clickable)
            {
                _Graphics.DrawLine((selectedLineParentIndex ?? 0)==line.parentIndex ? selectedPen : pen, x1, y1, x2, y2);
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
            line = DrawLine(X1, Y1, X2, Y2, xOffset, yOffset, scale, fromAutoCad, pen,line,false);
            return line;
        }
        public void DrawOrigin(float xOffset, float yOffset, float scale)
        {
            Dxf.Line line = new Dxf.Line();
            if (stageBounds != null)
            {
                float xWidth = (stageBounds.X[1] - stageBounds.X[0]) / 4;
                float yWidth = (stageBounds.Y[1] - stageBounds.Y[0]) / 4;
                //x-axis
                DrawLine(Origin.X - xWidth, Origin.Y, Origin.X + xWidth, Origin.Y, xOffset, yOffset, scale, false, xaxisPen,line,false);
                //y-axis
                DrawLine(Origin.X, Origin.Y - yWidth, Origin.X, Origin.Y + yWidth, xOffset, yOffset, scale, false, yaxisPen, line, false);
            }
        }
        public void DrawStageBounds(float xOffset, float yOffset, float scale)
        {
            Dxf.Line line = new Dxf.Line();
            if (stageBounds != null)
            {
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[0], xOffset, yOffset, scale, false, stageBoundsPen, line, false);//bottom
                DrawLine(stageBounds.X[0], stageBounds.Y[1], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false);//top
                DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[0], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false);//left
                DrawLine(stageBounds.X[1], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen, line, false);//right
            }
        }

        //UI Event stuff
        private void buttChangeFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = String.Format("{0}\\Files\\", Application.StartupPath);
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fullFile = this.openFileDialog1.FileName;
                Dxf dxf = new Dxf();
                this.SubscribeDxf(dxf);
                Thread dxfThread = new Thread(() => dxf.Start(fullFile,lineSpacing));
                dxfThread.Name = "Dxf";
                dxfThread.Start();
                this.labelCurrentFile.Text = System.IO.Path.GetFileName(fullFile);
            }
        }
        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool thereIsALineSelected = false;
            Point mouseDownLocation = new Point(e.X, e.Y);
            for (int j = 0; j < Lines.Count; j++)
            {
                if (FindDistanceToSegment(new PointF(){X=e.X,Y=e.Y},Lines[j].plotted1, Lines[j].plotted2)<10)
                {
                    selectedLineParentIndex = Lines[j].parentIndex;
                    DrawDrawing();
                    ShowSelectedLine();
                    thereIsALineSelected = true;
                    selectedType = "line";
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
                    if (FindDistanceToSegment(new PointF() { X = e.X, Y = e.Y }, Lines[j].plotted1, Lines[j].plotted2) < 10)
                    {
                        selectedLineParentIndex = Polylines[i].laserLines[i].parentIndex;
                        DrawDrawing();
                        ShowSelectedLine();
                        thereIsALineSelected = true;
                        selectedType = "polyline";
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
        private void checkStageBounds_CheckStateChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        private void checkDisplayOrigin_CheckStateChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        public void textScale_LostFocus(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        private void textLineSpacing_LostFocus(object sender, EventArgs e)
        {
            int userInput = int.Parse(this.textLineSpacing.Text);
            lineSpacing = userInput <= 0 ? 1 : userInput;
            Dxf dxf = new Dxf();
            this.SubscribeDxf(dxf);
            Thread dxfThread = new Thread(() => dxf.Start(fullFile, lineSpacing));
            dxfThread.Name = "Dxf";
            dxfThread.Start();
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
                Dxf.Line _selectedLine = Lines[selectedLineParentIndex ?? 0];//will always be defined so can do this (set index to 0), though bad practice...
                this.labelSegmentName.Text = String.Format("({0},{1}) -> ({2},{3})", _selectedLine.p1.X, _selectedLine.p1.Y, _selectedLine.p2.X, _selectedLine.p2.Y);
                this.textSegmentLaserSpacing.Text = _selectedLine.laserSpacing.ToString();
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
    }
}