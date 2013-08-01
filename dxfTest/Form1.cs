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

namespace dxfTest
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public float drawingWidth = (float)0;
        public Color drawingBackgroundColour = Color.FromArgb(34, 41, 51);
        public Color drawingPenColour = Color.White;
        public float drawingHeight = (float)0;
        public float lineSpacing = 100;
        public Pen _pen;
        public Pen yaxisPen;
        public Pen xaxisPen;
        public Pen stageBoundsPen;
        string[] file;
        public Form1()
        {
            InitializeComponent();
            /*panel1.AutoScroll = true;
            panel1.AutoScrollMinSize = new Size(1000, 1000);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
            panel1.Scroll += new ScrollEventHandler(panel1_Scroll);*/
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _pen = new Pen(drawingPenColour, 1);
            xaxisPen = new Pen(System.Drawing.Color.Red, 1);
            yaxisPen = new Pen(System.Drawing.Color.Green, 1);
            stageBoundsPen = new Pen(System.Drawing.Color.Green, 1);
            //stageBoundsPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            file = Readfile();
            InterpretFile();
            this.textLineSpacing.Text = lineSpacing.ToString();
            
            DisplayUnits();
            DrawDrawing();
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
        public List<Circle> Circles = new List<Circle>();
        public const int LaserPrecision = 10;
        public class Units
        {
            //Angular units
            public int AngularUnitsCode { get; set; }
            public string AngularUnitsString { get {return _getAngularUnits(AngularUnitsCode);}}
            private string _getAngularUnits(int code){
                string angularUnits="";
                switch(code){
                    case 0:
                        angularUnits = "Decimal degrees";
                        break;
                    case 1:
                        angularUnits = "Degrees/minutes/seconds";
                        break;
                    case 2:
                        angularUnits = "Gradians";
                        break;
                    case 3:
                        angularUnits = "Radians";
                        break;
                    case 4:
                        angularUnits = "Surveyor's units";
                        break;

                }
                return angularUnits;
            }
            public float AngularConversionFactor { get { return _getAngularConversionFactor(AngularUnitsCode); } }
            private float _getAngularConversionFactor(int code)
            {
                /*
                 * converts any unit into meters; the working unit of this program
                 * */
                float factor = 0;
                switch (code)
                {
                    case 0:
                        factor = (float)(Math.PI/180);//"Decimal degrees";
                        break;
                    case 1:
                        //factor = (float);//"Degrees/minutes/seconds";
                        break;
                    case 2:
                        factor = (float)(Math.PI/200);//"Gradians";
                        break;
                    case 3:
                        factor = (float)1;//"Radians";
                        break;
                    case 4:
                        //factor = (float);//"Surveyor's units";
                        break;
                }
                return factor;
            }
            //Linear units
            public int LinearUnitsCode { get; set; }
            public string LinearUnitsString { get { return _getLinearUnits(LinearUnitsCode); } }
            private string _getLinearUnits(int code)
            {
                string linearUnits = "";
                switch (code)
                {
                    case 0:
                        linearUnits = "Unitless";
                        break;
                    case 1:
                        linearUnits = "Inches";
                        break;
                    case 2:
                        linearUnits = "Feet";
                        break;
                    case 3:
                        linearUnits = "Miles";
                        break;
                    case 4:
                        linearUnits = "Millimeters";
                        break;
                    case 5:
                        linearUnits = "Centimeters";
                        break;
                    case 6:
                        linearUnits = "Meters";
                        break;
                    case 7:
                        linearUnits = "Kilometers";
                        break;
                    case 8:
                        linearUnits = "Microinches";
                        break;
                    case 9:
                        linearUnits = "Mils";
                        break;
                    case 10:
                        linearUnits = "Yards";
                        break;
                    case 11:
                        linearUnits = "Angstroms";
                        break;
                    case 12:
                        linearUnits = "Nanometers";
                        break;
                    case 13:
                        linearUnits = "Microns";
                        break;
                    case 14:
                        linearUnits = "Decimeters";
                        break;
                    case 15:
                        linearUnits = "Decameters";
                        break;
                    case 16:
                        linearUnits = "Hectometers";
                        break;
                    case 17:
                        linearUnits = "Gigameters";
                        break;
                    case 18:
                        linearUnits = "Astronomical units";
                        break;
                    case 19:
                        linearUnits = "Light years";
                        break;
                    case 20:
                        linearUnits = "Parsecs";
                        break;

                }
                return linearUnits;
            }
            public float LinearConversionFactor { get { return _getLinearConversionFactor(LinearUnitsCode); } }
            private float _getLinearConversionFactor(int code)
            {
                /*
                 * converts any unit into meters; the working unit of this program
                 * */
                float factor = 0;
                switch (code)
                {
                    case 0:
                        //"Unitless"
                        break;
                    case 1:
                        factor = (float)0.0254;//"Inches";
                        break;
                    case 2:
                        factor = (float)0.3048;//"Feet";
                        break;
                    case 3:
                        factor = (float)1609.34;//"Miles";
                        break;
                    case 4:
                        factor = (float)0.001;//"Millimeters";
                        break;
                    case 5:
                        factor = (float)0.01;//"Centimeters";
                        break;
                    case 6:
                        factor = (float)1;//"Meters";
                        break;
                    case 7:
                        factor = (float)1000;//"Kilometers";
                        break;
                    case 8:
                        factor = (float)(2.5400*Math.Pow(10,-8));//"Microinches";
                        break;
                    case 9:
                        factor = (float)(2.54*Math.Pow(10,-5));//"Mils";
                        break;
                    case 10:
                        factor = (float)0.9144;//"Yards";
                        break;
                    case 11:
                        factor = (float)(1.0*Math.Pow(10,-10));//"Angstroms";
                        break;
                    case 12:
                        factor = (float)(1.0*Math.Pow(10,-9));//"Nanometers";
                        break;
                    case 13:
                        factor = (float)(1.0*Math.Pow(10,-6));//"Microns";
                        break;
                    case 14:
                        factor = (float)0.1;//"Decimeters";
                        break;
                    case 15:
                        factor = (float)10;//"Decameters";
                        break;
                    case 16:
                        factor = (float)100;//"Hectometers";
                        break;
                    case 17:
                        factor = (float)(1.0*Math.Pow(10,9));//"Gigameters";
                        break;
                    case 18:
                        factor = (float)149597870700;//"Astronomical units";
                        break;
                    case 19:
                        factor = (float)(9.4605284*Math.Pow(10,15));//"Light years";
                        break;
                    case 20:
                        factor = (float)(3.08567758*Math.Pow(10,16));//"Parsecs";
                        break;
                }
                return factor;
            }
            
        }
        public class Polyline
        {
            public int noVerticies { get; set; }//number of verticies in the poyline
            public bool closed { get; set; }//is the polyline closed or not
            public List<polyPoint> verticies { get; set; }//all the verticies 
            public List<Line> laserLines { get; set; }//list of lines used to describe the arc
            public bool readComplete { get; set; }//all data has been read in ok
        }
        public Units units = new Units();
        public class polyPoint
        {
            public PointF Point {get;set;}
            public float? Buldge { get; set; }
        }
        public PointF Origin;
        public class StageBounds
        {
            public float[] X { get; set; }//min, max
            public float[] Y { get; set; }//min, max
        }
        StageBounds stageBounds = new StageBounds()
        {
            X = new float[2] { (float)-0.04, (float)0.04 },
            Y = new float[2] { (float)-0.04, (float)0.04 },
        };

        public string[] Readfile()
        {
            return System.IO.File.ReadAllLines(String.Format("{0}\\Files\\Drawing1.dxf",Application.StartupPath));
        }
        public void InterpretFile()
        {
            //clear text in output textbox
            this.textOutput.Clear();
            //clear all polylines
            Lines.Clear(); Polylines.Clear(); Arcs.Clear(); Circles.Clear();
            //Read in the file and get all the data out
            for (int i = 0; i < file.Length; i++)
            {
                switch (file[i])
                {
                        //Header properties
                    case "$UCSORG"://location of machining origin
                        GetOrigin(file,i);
                        break;
                    case "$DIMAUNIT":
                        GetAngleUnits(file, i);
                        break;
                    case "$INSUNITS":
                        GetLinearUnits(file, i);
                        break;
                        //Entities
                    case "AcDbLine":
                        GetLineProperties(file, i);//in future return i
                        break;
                    case "AcDbPolyline":
                        GetPolylineProperties(file, i);
                        break;
                    case "AcDbCircle":
                        GetCircleProperties(file, i);
                        break;
                }
            }
            //Display the results to the user
            //g.DrawLine(Pens.Black, 0, 100, 0, 100);
            
            foreach (Line line in Lines)
            {
                this.textOutput.Text += String.Format("Line: ({0}, {1}),({2}, {3}), length: {4} \r\n", line.p1.X, line.p1.Y, line.p2.X, line.p2.Y, line.GetLength);
            };
            this.textOutput.Text += String.Format("\r\n");
            foreach (Circle circle in Circles)
            {
                circle.laserLines = ConvertCircleToLines(circle, lineSpacing);
            }
            foreach (Polyline pline in Polylines)
            {
                this.textOutput.Text += String.Format("Polyline: {0} vertices, {1} \r\n", pline.noVerticies, pline.closed ? "closed" : "open");
                pline.laserLines = new List<Line>();
                for (int i = 0; i < pline.noVerticies; i++)
                {
                    //display verticies to the user
                    this.textOutput.Text += String.Format("          Vertex {0}: ({1},{2}) {3}\r\n", i, pline.verticies[i].Point.X, pline.verticies[i].Point.Y, pline.verticies[i].Buldge != null ? pline.verticies[i].Buldge.ToString() : "");


                    //convert vertex to laser line and convert bulges to lines if necessary. 

                    if (pline.verticies[i].Buldge == null)
                    {
                        Line laserLine = new Line();
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
                            pline.laserLines.AddRange(ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[i + 1].Point, pline.verticies[i].Buldge ?? (float)0, lineSpacing));
                        }
                        else
                        {
                            if (pline.closed)
                            {
                                //link back to the original vertex
                                pline.laserLines.AddRange(ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[0].Point, pline.verticies[i].Buldge ?? (float)0, lineSpacing));
                            }
                        };
                    };

                }

            };
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
        public class Circle
        {
            public PointF center { get; set; }
            public double diameter { get; set; }
            public double? startAngle { get; set; }
            public double? endAngle { get; set; }
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
                        line.p2 = tempPoint;
                        break;
                    case "0":
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
                        polyPoint pt = new polyPoint()
                        {
                            Point = new PointF
                            {
                                X = (float.Parse(file[lineNo + 1].Trim())),//* units.LinearConversionFactor
                                Y = (float.Parse(file[lineNo + 3].Trim())),
                            },
                            Buldge = file[lineNo + 4].Trim() == "42" ? (Val42 * units.LinearConversionFactor) : null,
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
       /* public void GetArcProperties(string[] file, int startLine)
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
        }*/
        public void GetCircleProperties(string[] file, int startLine)
        {
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            Circle circle = new Circle();
            bool readComplete = false;
            PointF tempPoint; 

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "39"://thickness, optional & default =0
                        //disregard
                        break;
                    case "10"://center point x value
                        circle.center = new PointF
                        {
                            X = float.Parse(file[lineNo + 1].Trim()),
                        };
                        break;
                    case "20"://center point y value
                        tempPoint = circle.center;
                        circle.center = new PointF()
                        {
                            X=tempPoint.X,
                            Y = float.Parse(file[lineNo + 1].Trim()),
                        };
                        break;
                    case "30"://center point z value
                        //neglect
                        break;
                    case "40"://radius
                        circle.diameter = double.Parse(file[lineNo + 1].Trim()) * 2;
                        break;
                    case "AcDbArc":
                        //is an arc- not a circle.. do nothing. 
                        break;
                    case "50":
                        circle.startAngle = double.Parse(file[lineNo + 1].Trim());
                        break;
                    case"51":
                        circle.endAngle = double.Parse(file[lineNo + 1].Trim());
                        break;
                    case "0"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            Circles.Add(circle);
        }
        public void GetOrigin(string[] file, int startLine)
        {
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            bool readComplete = false;
            PointF tempPoint;

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "10"://center point x value
                        Origin = new PointF(){
                            X = float.Parse(file[lineNo + 1].Trim())
                        };
                        break;
                    case "20"://center point y value
                        tempPoint = Origin;
                        Origin = new PointF()
                        {
                            X = tempPoint.X,
                            Y = float.Parse(file[lineNo + 1].Trim()),
                        };
                        break;
                    case "30"://center point z value
                        //neglect
                        break;
                    case "9"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
        }
        public void GetAngleUnits(string[] file, int startLine)
        {//
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            bool readComplete = false;

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "70"://center point x value
                        units.AngularUnitsCode = int.Parse(file[lineNo + 1].Trim());
                        break;
                    case "9"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
        }
        public void GetLinearUnits(string[] file, int startLine)
        {//
            //read from startLine until end of line is reached
            bool done = false;
            int lineNo = startLine;
            bool readComplete = false;

            while (!done)
            {
                switch (file[lineNo].Trim())//assumes that line is always in the following structure.
                {
                    case "70"://center point x value
                        units.LinearUnitsCode = int.Parse(file[lineNo + 1].Trim());
                        break;
                    case "9"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
        }
        public void DisplayUnits()
        {
            this.labelUnits.Text = String.Format("units detected: {0}{1}{2}", units.LinearUnitsString, (units.LinearUnitsString != null && units.AngularUnitsString != null) ? ", " : "", units.AngularUnitsString);
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
                Y = (start.Y + end.Y) / 2 - K * (start.X - end.X),
            };
            
            //Find the radius of the circle
            double radius = Math.Sqrt(Math.Pow(start.X - center.X, (double)2) + Math.Pow(start.Y - center.Y, (double)2));

            //Now we have a circle, but we need an arc. So, relative to the center of the circle and in the direction of the x axis, find the starting angle and ending angle
             double startTheta = Math.Atan((start.Y-center.Y)/(start.X-center.X));
             double endTheta = Math.Atan((end.Y - center.Y) / (end.X - center.X));
             if ((start.X - center.X) < 0)
             {
                 startTheta += Math.PI;
             }
             if ((end.X - center.X) < 0)
             {
                 endTheta += Math.PI;
             }
            //Apply direction from bulge sign
             if (bulge < 0 && (endTheta - startTheta)>0)
             {
                 endTheta -= 2*Math.PI;
             }
            //Find the arc length
             double arcLength = (double)radius * Math.Abs(endTheta - startTheta);//this is incorrect - wrong arcs.. (basic math ok..)
            //Find number of lines to split the arc up into (this is actually n-1)
            int noLines = int.Parse(Math.Round((arcLength / lineSpacing)).ToString());
            if(noLines==0){
                //need at least one line
                noLines++;
            }else{
                //all good
            };

            double thetaIncrement = (endTheta - startTheta) / noLines;//the increment for each line wrt the center point
            double currentTheta = startTheta;//current theta, eqivalent to theoretical angles[i]
            double nextTheta;//angle following the current theta, equivalent to theoretical angles[i+1]

            //Make all the lines from the given information. (Finally!)
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
        public List<Line> ConvertCircleToLines(Circle circle, float lineSpacing)
        {
            /*
             * Function converts a circle (ie defined by a center point and radius)
             * to a bunch of lines approximating the circle's perimeter
             * */
            //create output list
            List<Line> output = new List<Line>();
            //convert incoming properties
            float radius = (float)circle.diameter / 2;
            //working variables

            double startTheta = circle.startAngle != null ? (double)circle.startAngle : 0;
            double endTheta = circle.endAngle != null ? (double)circle.endAngle : 0;
            double currentTheta = startTheta;//current theta, eqivalent to theoretical angles[i]
            double nextTheta;//angle following the current theta, equivalent to theoretical angles[i+1]
            double arcLength = circle.startAngle == null ? (Math.PI * 2) * radius : Math.Abs((double)(circle.endAngle - circle.startAngle)) * radius * Math.PI / 180;//need to convert to radians
            int noLines = int.Parse(Math.Round((arcLength / lineSpacing)).ToString());
            double thetaIncrement = (arcLength/radius) / noLines;//the increment for each line wrt the center point

            //Make all the lines from the given information. (Finally!)
            for (int i = 0; i < noLines; i++)
            {
                nextTheta = currentTheta + thetaIncrement;
                Line line = new Line()
                {
                    p1 = new PointF()
                    {
                        X = (float)(radius * Math.Cos(currentTheta) + circle.center.X),
                        Y = (float)(radius * Math.Sin(currentTheta) + circle.center.Y),
                    },
                    p2 = new PointF()
                    {
                        X = (float)(radius * Math.Cos(nextTheta) + circle.center.X),
                        Y = (float)(radius * Math.Sin(nextTheta) + circle.center.Y),
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

        public void textScale_LostFocus(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        public void vScrollBar1_valueChanged(object sender, EventArgs e)
        {
                DrawDrawing();
            
        }
        public void hScrollBar1_valueChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        public void textLineSpacing_LostFocus(object sender, EventArgs e)
        {
            this.textLineSpacing.Enabled = false;
            try
            {

                lineSpacing = float.Parse(this.textLineSpacing.Text.Trim());
            }
            catch
            {
                lineSpacing = 1;
            }
            InterpretFile();
            DrawDrawing();
            this.textLineSpacing.Enabled = true;
        }
        public void butSetStageZero_Click(object sender, EventArgs e)
        {
            //
        }
        public void pictureBox1_Paint(object sender, EventArgs e)
        {
            // pictureBox1.Image = image;

        }
        public void checkDisplayOrigin_CheckedChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }
        public void checkStageBounds_CheckedChanged(object sender, EventArgs e)
        {
            DrawDrawing();
        }

        private Graphics _Graphics;
        private Bitmap image;
        
        public void DrawDrawing()
        {
             _Graphics = Graphics.FromImage(image);
             
             _Graphics.Clear(drawingBackgroundColour);
             
            float scale;
            try
            {
                 scale= float.Parse(this.textScale.Text.Trim());
                 if (scale == (float)0) { scale = 1; };
            }
            catch
            {
                scale = (float)1.0;
            }
            float xOffset = (float)this.hScrollBar1.Value / 100 * drawingWidth/scale;
            float yOffset = (float)this.vScrollBar1.Value / 100 * drawingHeight/scale;
            foreach (Polyline pLine in Polylines)
            {
                for (int i = 0; i < pLine.laserLines.Count; i++)
                {
                    DrawLine(pLine.laserLines[i].p1.X, pLine.laserLines[i].p1.Y, pLine.laserLines[i].p2.X, pLine.laserLines[i].p2.Y, xOffset, yOffset, scale,true);
                }
            }
            foreach (Line line in Lines)
            {
                DrawLine(line.p1.X, line.p1.Y, line.p2.X, line.p2.Y, xOffset, yOffset, scale, true);
            }
            foreach(Circle circle in Circles)
            {
                for (int i = 0; i < circle.laserLines.Count; i++)
                {
                    DrawLine(circle.laserLines[i].p1.X, circle.laserLines[i].p1.Y, circle.laserLines[i].p2.X, circle.laserLines[i].p2.Y, xOffset, yOffset, scale, true);
                }
            }
            if (this.checkDisplayOrigin.Checked)
            {
                DrawOrigin();
            }
            else
            {
                //user doesn't want to display the origin
            }
            if (this.checkStageBounds.Checked)
            {
                DrawStageBounds(xOffset,yOffset,scale);
            }
            else
            {
                //user doesn't want to display the origin
            }
            this.pictureBox1.Image = image;
            this.pictureBox1.Refresh();
            this.pictureBox1.Invalidate();
        }

        public void DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale,bool fromAutoCad, Pen pen)
        {
            //if from autocad then have to flip the y-axis
            int x1 = int.Parse(Math.Round((X1 - xOffset) / scale).ToString());
            int y1 = !fromAutoCad ? int.Parse(Math.Round((Y1 - yOffset) / scale).ToString()) : int.Parse(Math.Round((drawingHeight - Y1 - yOffset) / scale).ToString());
            int x2 = int.Parse(Math.Round((X2 - xOffset) / scale).ToString());
            int y2 = !fromAutoCad ? int.Parse(Math.Round((Y2 - yOffset) / scale).ToString()) : int.Parse(Math.Round((drawingHeight - Y2 - yOffset) / scale).ToString());
            _Graphics.DrawLine(pen, x1, y1, x2, y2);
        }
        public void DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale, bool fromAutoCad)
        {
            DrawLine(X1, Y1, X2, Y2, xOffset, yOffset, scale, fromAutoCad, _pen);
        }
        public void DrawOrigin()
        {
            //x-axis
            _Graphics.DrawLine(xaxisPen, 0, Origin.Y, drawingWidth, Origin.Y);
            //y-axis
            _Graphics.DrawLine(yaxisPen, Origin.X, 0, Origin.X, drawingHeight);
        }
        public void DrawStageBounds(float xOffset, float yOffset, float scale)
        {
            DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[0],xOffset,yOffset,scale,false,stageBoundsPen);//bottom
            DrawLine(stageBounds.X[0], stageBounds.Y[1], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen);//top
            DrawLine(stageBounds.X[0], stageBounds.Y[0], stageBounds.X[0], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen);//left
            DrawLine(stageBounds.X[1], stageBounds.Y[0], stageBounds.X[1], stageBounds.Y[1], xOffset, yOffset, scale, false, stageBoundsPen);//right

        }
        
        
    }
}
