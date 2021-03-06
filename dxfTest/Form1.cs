﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dxfTest
{
    public partial class Form1 : Form
    {
        public float drawingWidth = (float)0;
        public float drawingHeight = (float)0;
        public Form1()
        {
            InitializeComponent();
            /*panel1.AutoScroll = true;
            panel1.AutoScrollMinSize = new Size(1000, 1000);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
            panel1.Scroll += new ScrollEventHandler(panel1_Scroll);*/
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            PointF start = new PointF(){
                X = (float)3994.449,
                Y = (float)2363.032,
            };
            PointF end = new PointF(){
                X = (float)3894.407,
                Y = (float)2193.638,
            };
            ConvertBulgeToLines(start, end, (float)-0.7179035,40);

            string[] file = Readfile();
            //Read in the file and get all the data out
            for (int i = 0; i < file.Length; i++)
            {
                switch (file[i])
                {
                    case "AcDbLine":
                        GetLineProperties(file, i);//in future return i
                        break;
                    case "AcDbPolyline":
                        GetPolylineProperties(file, i);
                        break;
                }
            }
            //Display the results to the user
            //g.DrawLine(Pens.Black, 0, 100, 0, 100);
            foreach (Line line in Lines)
            {
                this.textOutput.Text += String.Format("Line: ({0}, {1}),({2}, {3}), length: {4} \r\n", line.p1.X,line.p1.Y,line.p2.X,line.p2.Y, line.GetLength);
            };
            this.textOutput.Text += String.Format("\r\n");
            foreach (Polyline pline in Polylines)
            {
                this.textOutput.Text += String.Format("Polyline: {0} vertices, {1} \r\n", pline.noVerticies, pline.closed?"closed":"open");
                int i = 0;
                foreach (polyPoint pt in pline.verticies)
                {
                    this.textOutput.Text += String.Format("          Vertex {0}: ({1},{2}) {3}\r\n", i,pt.Point.X,pt.Point.Y,pt.Buldge!=null?pt.Buldge.ToString():"");
                    i++;
                }
                
            };
            DrawPolyLines();
            //panel1.Invalidate();
        }
        void panel1_Scroll(object sender, ScrollEventArgs e)
        {
           // panel1.Invalidate();
        }
        void panel1_Paint(object sender, PaintEventArgs e)
        {
           // e.Graphics.TranslateTransform(panel1.AutoScrollPosition.X, panel1.AutoScrollPosition.Y);
            
          // e.Graphics.DrawLine(Pens.Black, 0, 0, 1000, 1000);
        }
        public List<Line> Lines = new List<Line>();
        public List<Polyline> Polylines = new List<Polyline>();
        public List<Arc> Arcs = new List<Arc>();
        public const int LaserPrecision = 10;
        
        public class Polyline
        {
            public int noVerticies { get; set; }//number of verticies in the poyline
            public bool closed { get; set; }//is the polyline closed or not
            public List<polyPoint> verticies { get; set; }//all the verticies 
            public List<Line> laserLines { get; set; }//list of lines used to describe the arc
            public bool readComplete { get; set; }//all data has been read in ok
        }

        public class polyPoint
        {
            public PointF Point {get;set;}
            public float? Buldge { get; set; }
        }

        public string[] Readfile()
        {
            return System.IO.File.ReadAllLines(@"C:\Users\obod001\Documents\GitHub\MiniatureStageInterface\dxfTest\bin\Debug\Files\Drawing1.dxf");
        }
        public class Line
        {
            public PointF p1 { get; set; }
            public PointF p2 { get; set; }
            //disregard z
            public bool readComplete { get; set; }
            public double GetLength { get { return length(p1,p2); } }
            private static double length(PointF p1, PointF p2)
            {
                return Math.Sqrt(Math.Pow((p2.X - p1.X), 2.0) + Math.Pow((p2.Y-p1.Y), 2.0));
            }
        }
        public class Arc
        {
            public PointF center { get; set; }
            public double diameter { get; set; }
            public double startAngle { get; set; }
            public double sweepAngle { get; set; }
            public List<Line> laserLines { get; set; }
        }

        public void GetLineProperties(string[] file, int startLine)
        {
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            PointF tempPoint;
            Line line = new Line()
            {
                p1 = new PointF(),
                p2 = new PointF(),
            };

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "10"://first x coord
                        line.p1 = new PointF()
                        {
                            X = float.Parse(file[lineNo + 1].Trim())
                        };
                        break;
                    case "11"://second x coord
                        line.p2 = new PointF()
                        {
                            X = float.Parse(file[lineNo + 1].Trim())
                        };
                        break;
                    case "20"://first y coord
                        tempPoint = line.p1;
                        tempPoint.Y = float.Parse(file[lineNo + 1].Trim());
                        line.p1 = tempPoint;
                        break;
                    case "21"://second y coord
                        tempPoint = line.p2;
                        tempPoint.Y = float.Parse(file[lineNo + 1].Trim());
                        line.p1 = tempPoint;
                        line.readComplete = true;//we're all done now
                        break;
                }
                if (line.readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            Lines.Add(line);
        }
        public void GetPolylineProperties(string[] file, int startLine)
        {
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            Polyline pline = new Polyline();
            pline.verticies = new List<polyPoint>();

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "90"://number of verticies
                        pline.noVerticies = int.Parse(file[lineNo + 1].Trim());
                        break;
                    case "70"://polyline flag - 0:open, 1:closed, 128:Plinegen
                        pline.closed = (file[lineNo + 1].Trim() == "1");//assuming Plinegen never comes up
                        break;
                    case "43"://constant width ?
                        //disregard because this is laser machining after all
                        //would be 0 if constant, not included if not constant - see codes 40 & 41
                        break;
                    case "10"://get x&y coordinates
                        float? Val42 = null;
                        if (file[lineNo + 4].Trim() == "42")
                        {
                            Val42 = float.Parse(file[lineNo + 5].Trim());
                        };
                        polyPoint pt = new polyPoint(){
                            Point = new PointF{
                                X = float.Parse(file[lineNo + 1].Trim()),
                                Y = float.Parse(file[lineNo + 3].Trim()),
                            },
                            Buldge = file[lineNo+4].Trim()=="42"?Val42:null,
                        };
                        if(pt.Point.Y>drawingHeight){drawingHeight=pt.Point.Y;};//find drawing height
                        if(pt.Point.Y>drawingWidth){drawingWidth=pt.Point.X;};//find drawing width
                        pline.verticies.Add(pt);
                        break;
                    case "0"://all done now.. 
                        pline.readComplete = true;
                        break;
                }
                if (pline.readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            Polylines.Add(pline);
        }
        public void GetArcProperties(string[] file, int startLine)
        {
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            Arc arc = new Arc();
            bool readComplete = false;

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "39"://thickness, optional & default =0
                        //disregard
                        break;
                    case "10"://center point x value
                        arc.center = new Point()
                        {
                            X = int.Parse(file[lineNo + 1].Trim()),
                        };
                        break;
                    case "20"://center point y value
                        arc.center = new Point()
                        {
                            Y = int.Parse(file[lineNo + 1].Trim()),
                        };
                        break;
                    case "40"://radius
                        arc.diameter = double.Parse(file[lineNo+1].Trim())*2;
                        break;
                    
                    case "AcDbEntity"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            Arcs.Add(arc);
        }

        public void GetDrawingUnits(string[] file, int startLine)
        {
            //$INSUNITS
        }

        public List<Line> ConvertBulgeToLines(PointF start, PointF end, float bulge, float lineSpacing)
        {
            /*
             * Function to convert a dxf bulge arc to a series of lines which approximate the arc. 
             * (dxf stores arcs in polylines as 2 points, the first which is saved with a bulge for the segment)
             * we need a series of lines which will be plotted onscreen for the user and then subsequently used to 
             * control the stages. 
             * 
             * start = first point from dxf
             * bulge = bulge from first point in dxf file
             * end = subsequent point in dxf (or alternatively the start of the polyline if the loop is closed)
             * noLines = minimum length of line between points 
             * */
            List<Line> output = new List<Line>();

            //Get center of circle formed by the bulge and two points
            float K = (float)0.25 * (1 / bulge - bulge);
            PointF center = new PointF()
            {
                X = (start.X + end.X) / 2 + K * (start.Y - end.Y),
                Y = (start.X + end.X) / 2 - K * (start.X - end.X),
            };
            
            //Find the radius of the circle
            double radius = Math.Sqrt(Math.Pow(start.X - center.X, (double)2) + Math.Pow(start.Y - center.Y, (double)2));

            //Now we have a circle, but we need an arc. So, relative to the center of the circle and in the direction of the x axis, find the starting angle and ending angle
            double startTheta = Math.Acos((start.X-center.X) / radius);
            double endTheta = Math.Acos((end.X -center.X)/ radius);

            //So now we need to move between startTheta and endTheta, at points of size "lineSpacing", saving each of these points to the output line list

            //Find the arc length
            double arcLength = radius*(endTheta-startTheta);
            //Find number of lines to split the arc up into (this is actually n-1)
            int noLines = int.Parse(Math.Round((arcLength / lineSpacing)).ToString());
            if(noLines==0){
                //need at least one line
                noLines++;
            }else{
                //all good
            };

            double thetaIncrement = (endTheta - startTheta) / noLines;
            double currentTheta = startTheta;
            double nextTheta;
            for (int i = 0; i <noLines ; i++)
            {
                nextTheta = currentTheta+thetaIncrement;
                Line line = new Line()
                {
                    p1 = new PointF()
                    {
                        X = (float)(radius * Math.Cos(currentTheta) + center.X),
                        Y = (float)(radius * Math.Sin(currentTheta) + center.Y),
                    },
                    p2 = new PointF()
                    {
                        X = (float)(radius * Math.Cos(nextTheta) + center.X),
                        Y = (float)(radius * Math.Sin(nextTheta) + center.Y),
                    },
                };
                currentTheta = nextTheta;
                output.Add(line);
            }

            return output;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void textScale_textChanged(object sender, EventArgs e)
        {
            DrawPolyLines();
        }
        public void vScrollBar1_valueChanged(object sender, EventArgs e)
        {
                DrawPolyLines();
            
        }
        public void hScrollBar1_valueChanged(object sender, EventArgs e)
        {
            DrawPolyLines();
        }

        Pen _pen = new Pen(Color.Black, 1);
        private Graphics _Graphics;
        private Bitmap image;

        public List<Line> CreateLines()
        {
            List<Line> _Lines = new List<Line>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                Line line = new Line()
                {
                    p1 = new PointF(){
                        X= float.Parse(rand.Next(image.Width).ToString()),
                        Y = float.Parse(rand.Next(image.Height).ToString()),
                    },
                    p2= new PointF(){
                        X = float.Parse(rand.Next(image.Width).ToString()),

                        Y = float.Parse(rand.Next(image.Height).ToString()),
                    },
                };
                _Lines.Add(line);
            }
            return _Lines;
        }
        public void DrawPolyLines()
        {
             _Graphics = Graphics.FromImage(image);
            _Graphics.Clear(System.Drawing.Color.White);
            float scale;
            try
            {
                 scale= float.Parse(this.textScale.Text.Trim());
            }
            catch
            {
                scale = (float)1.0;
            }
            float xOffset = (float)this.hScrollBar1.Value / 100 * drawingWidth/scale;
            float yOffset = (float)this.vScrollBar1.Value / 100 * drawingHeight/scale;
            foreach (Polyline pLine in Polylines)
            {


                int noLinesToDraw = pLine.closed ? pLine.noVerticies : (pLine.noVerticies - 1);
                for (int i = 0; i < noLinesToDraw; i++)
                {
                    if (i != (noLinesToDraw - 1))
                    {
                        //draw indexed point, connect this to the next indexed point
                        _Graphics.DrawLine(_pen, int.Parse(Math.Round((pLine.verticies[i].Point.X - xOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[i].Point.Y - yOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[i+1].Point.X - xOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[i + 1].Point.Y - yOffset) / scale).ToString()));
                    }
                    else
                    {
                        //draw indexed point, connect this to the very first point
                        _Graphics.DrawLine(_pen, int.Parse(Math.Round((pLine.verticies[i].Point.X - xOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[i].Point.Y - yOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[0].Point.X - xOffset) / scale).ToString()), int.Parse(Math.Round((pLine.verticies[0].Point.Y - yOffset) / scale).ToString()));
                    };


                }
            }
            this.pictureBox1.Image = image;
            this.pictureBox1.Refresh();
            this.pictureBox1.Invalidate();
        }
        public void pictureBox1_Paint(object sender, EventArgs e)
        {
            // pictureBox1.Image = image;

        }
        
    }
}
