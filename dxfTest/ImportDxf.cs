using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
namespace dxfTest
{
    public class Dxf
    {
        string[] _file;
        List<Line> _lines=new List<Line>();
        List<Polyline> _polylines = new List<Polyline>();
        List<Arc> _arcs = new List<Arc>();
        List<Circle> _circles = new List<Circle>();
        PointF _origin;
        Units _units = new Units();
        float _drawingMinX = 9999999;
        float _drawingMaxX = 0;
        float _drawingWidth = 0;
        float _drawingHeight = 0;
        float _drawingMaxY = 0;
        float _drawingMinY = 9999999;

        
        //Dxf classes
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
            public PointF Point { get; set; }
            public float? Bulge { get; set; }
        }
        public class StageBounds
        {
            public float[] X { get; set; }//min, max
            public float[] Y { get; set; }//min, max
        }
        public class Line
        {
            public PointF p1 { get; set; }
            public PointF p2 { get; set; }
            //disregard z
            public bool readComplete { get; set; }
            public double GetLength { get { return length(p1, p2); } }
            private static double length(PointF p1, PointF p2)
            {
                return Math.Sqrt(Math.Pow((p2.X - p1.X), 2.0) + Math.Pow((p2.Y - p1.Y), 2.0));
            }
            public PointF plotted1 { get; set; }
            public PointF plotted2 { get; set; }
            public bool Selected { get; set; }
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
        public class Units
        {
            //Angular units
            public int AngularUnitsCode { get; set; }
            public string AngularUnitsString { get { return _getAngularUnits(AngularUnitsCode); } }
            private string _getAngularUnits(int code)
            {
                string angularUnits = "";
                switch (code)
                {
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
                        factor = (float)(Math.PI / 180);//"Decimal degrees";
                        break;
                    case 1:
                        //factor = (float);//"Degrees/minutes/seconds";
                        break;
                    case 2:
                        factor = (float)(Math.PI / 200);//"Gradians";
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
                        factor = (float)(2.5400 * Math.Pow(10, -8));//"Microinches";
                        break;
                    case 9:
                        factor = (float)(2.54 * Math.Pow(10, -5));//"Mils";
                        break;
                    case 10:
                        factor = (float)0.9144;//"Yards";
                        break;
                    case 11:
                        factor = (float)(1.0 * Math.Pow(10, -10));//"Angstroms";
                        break;
                    case 12:
                        factor = (float)(1.0 * Math.Pow(10, -9));//"Nanometers";
                        break;
                    case 13:
                        factor = (float)(1.0 * Math.Pow(10, -6));//"Microns";
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
                        factor = (float)(1.0 * Math.Pow(10, 9));//"Gigameters";
                        break;
                    case 18:
                        factor = (float)149597870700;//"Astronomical units";
                        break;
                    case 19:
                        factor = (float)(9.4605284 * Math.Pow(10, 15));//"Light years";
                        break;
                    case 20:
                        factor = (float)(3.08567758 * Math.Pow(10, 16));//"Parsecs";
                        break;
                }
                return factor;
            }

        }
        public class Data
        {
            public List<Line> Lines { get; set; }
            public List<Polyline> Polylines { get; set; }
            public List<Arc> Arcs { get; set; }
            public List<Circle> Circles { get; set; }
            public PointF Origin { get; set; }
            public Units Units { get; set; }
            public float DrawingHeight { get; set; }
            public float DrawingWidth { get; set; }
        }
        //Main read and interpret functions
        public void Start(string fileDirectory)
        {
            _file = ReadFile(fileDirectory);
            InterpretFile();
            DxfEventArgs e = new DxfEventArgs();
            e.Circles = _circles;
            e.Arcs = _arcs;
            e.Origin = _origin;
            e.Polylines = _polylines;
            e.Lines = _lines;
            e.Units = _units;
            e.DrawingWidth = _drawingMaxX-_drawingMinX;
            e.DrawingHeight = _drawingMaxY - _drawingMinY;
            dxfEvent(this, e);
            Thread.CurrentThread.Abort();
        }
        //Read and interpret functions
        public string[] ReadFile(string fileDirectory)
        {
            return System.IO.File.ReadAllLines(fileDirectory);
        }
        public void InterpretFile()
        {
            //clear text in output textbox
            //this.textOutput.Clear();
            //clear all polylines
            _lines.Clear(); _polylines.Clear(); _arcs.Clear(); _circles.Clear();
            //Read in the file and get all the data out
            for (int i = 0; i < _file.Length; i++)
            {
                switch (_file[i])
                {
                    //Header properties
                    case "$UCSORG"://location of machining origin
                        GetOrigin(_file, i);
                        break;
                    case "$DIMAUNIT":
                        GetAngleUnits(_file, i);
                        break;
                    case "$INSUNITS":
                        GetLinearUnits(_file, i);
                        break;
                    //Entities
                    case "AcDbLine":
                        GetLineProperties(_file, i);//in future return i
                        break;
                    case "AcDbPolyline":
                        GetPolylineProperties(_file, i);
                        break;
                    case "AcDbCircle":
                        GetCircleProperties(_file, i);
                        break;
                }
            }
            //Display the results to the user
            //g.DrawLine(Pens.Black, 0, 100, 0, 100);

            foreach (Line line in _lines)
            {
                //this.textOutput.Text += String.Format("Line: ({0}, {1}),({2}, {3}), length: {4} \r\n", line.p1.X, line.p1.Y, line.p2.X, line.p2.Y, line.GetLength);
            };
            //this.textOutput.Text += String.Format("\r\n");
            foreach (Circle circle in _circles)
            {
                circle.laserLines = ConvertCircleToLines(circle, Form1.lineSpacing);
            }
            foreach (Polyline pline in _polylines)
            {
                //this.textOutput.Text += String.Format("Polyline: {0} vertices, {1} \r\n", pline.noVerticies, pline.closed ? "closed" : "open");
                pline.laserLines = new List<Line>();
                if (pline.noVerticies > 0)
                {
                    for (int i = 0; i < pline.noVerticies; i++)
                    {
                        //display verticies to the user
                        //this.textOutput.Text += String.Format("          Vertex {0}: ({1},{2}) {3}\r\n", i, pline.verticies[i].Point.X, pline.verticies[i].Point.Y, pline.verticies[i].Buldge != null ? pline.verticies[i].Buldge.ToString() : "");


                        //convert vertex to laser line and convert bulges to lines if necessary. 

                        if (pline.verticies[i].Bulge == null)
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
                                pline.laserLines.AddRange(ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[i + 1].Point, pline.verticies[i].Bulge ?? (float)0, Form1.lineSpacing));
                            }
                            else
                            {
                                if (pline.closed)
                                {
                                    //link back to the original vertex
                                    pline.laserLines.AddRange(ConvertBulgeToLines(pline.verticies[i].Point, pline.verticies[0].Point, pline.verticies[i].Bulge ?? (float)0, Form1.lineSpacing));
                                }
                            };
                        };

                    }

                };
            }
        }
        //Help read and interpret functions
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
            FindDrawingDims(line.p1.X, line.p1.Y);
            FindDrawingDims(line.p2.X, line.p2.Y);
            _lines.Add(line);
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
                        lineNo++;//otherwise this sets of the readcomplete switch if the polyline is open (0)
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
                            Bulge = file[lineNo + 4].Trim() == "42" ? (Val42) : null,
                        };
                        pline.verticies.Add(pt);
                        break;
                    case "0"://all done now.. 
                        pline.readComplete = true;
                        break;
                }
                if (pline.readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            /*This was to remove double points if in autocad the user had effectively closed the polyline, but then pressed "cl" afterwards - putting two points on the same location
             * 
             * if (pline.verticies.First().Point.X == pline.verticies.Last().Point.X && pline.verticies.First().Point.Y == pline.verticies.Last().Point.Y)
            {
                pline.verticies.Remove(pline.verticies.Last());
                pline.noVerticies--;
            }*/
            _polylines.Add(pline);
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
                            X = tempPoint.X,
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
                    case "51":
                        circle.endAngle = double.Parse(file[lineNo + 1].Trim());
                        break;
                    case "0"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
            _circles.Add(circle);
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
                        _origin = new PointF()
                        {
                            X = float.Parse(file[lineNo + 1].Trim())
                        };
                        break;
                    case "20"://center point y value
                        tempPoint = _origin;
                        _origin = new PointF()
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
                        _units.AngularUnitsCode = int.Parse(file[lineNo + 1].Trim());
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
                        _units.LinearUnitsCode = int.Parse(file[lineNo + 1].Trim());
                        break;
                    case "9"://all done now.. 
                        readComplete = true;
                        break;
                }
                if (readComplete) { break; }
                lineNo++;   //in future can move in 2s probably
            };
        }
        public void GetDrawingUnits(string[] file, int startLine)
        {
            //$INSUNITS
        }
        //Convert for machining
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
            double thetaIncrement = (arcLength / radius) / noLines;//the increment for each line wrt the center point

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
            double startTheta = Math.Abs(Math.Atan((Math.Abs(start.Y - center.Y) / (Math.Abs(start.X - center.X)))));//get absolute accute angles
            double endTheta = Math.Abs(Math.Atan((Math.Abs(end.Y - center.Y) / (Math.Abs(end.X - center.X)))));//get absolute accute angles

            //by default - modify later on
            bool endThetaPos = true;
            bool startThetaPos = true;

            //Find out which quadrant each angle is in and convert from acute to in the range 0<180 and 0>-180
            if (center.X > start.X)
            {
                if (start.Y > center.Y)
                {
                    //2nd quadrant
                    startTheta = Math.PI - startTheta;
                    startThetaPos = true;
                }
                else
                {
                    //3rd quadrant
                    startTheta = Math.Abs(-(Math.PI - startTheta));
                    startThetaPos = false;
                }
            }
            if (start.Y < center.Y)
                startThetaPos = false;//4th quadrant

            if (center.X > end.X)
            {
                if (end.Y > center.Y)
                {
                    //2nd quadrant
                    endTheta = Math.PI - endTheta;
                    endThetaPos = true;
                }
                else
                {
                    //3rd quadrant
                    endTheta = Math.Abs(-(Math.PI - endTheta));
                    endThetaPos = false;
                }
            }
            if (end.Y < center.Y)
                endThetaPos = false;//4th quadrant.

            //Apply direction from bulge sign
            /*if (bulge < 0 && (endTheta - startTheta)>0)
            {
                endTheta-= 2*Math.PI;
            }*/

            //Find the arc length
            double rawAngle = (startThetaPos && endThetaPos) || (!startThetaPos && !endThetaPos) ? Math.Abs(Math.Abs(endTheta) - Math.Abs(startTheta)) : Math.Abs(Math.Abs(endTheta) + Math.Abs(startTheta));
            double includedAngle = (Math.Abs(bulge) > 1) ? (rawAngle > Math.PI ? rawAngle : Math.PI * 2 - rawAngle) : (rawAngle < Math.PI ? rawAngle : Math.PI * 2 - rawAngle);//Greater than 1, return major radius
            double arcLength = (double)radius * Math.Abs(includedAngle);//this is incorrect - wrong arcs.. (basic math ok..)
            //Find number of lines to split the arc up into (this is actually n-1)
            int noLines = int.Parse(Math.Round((arcLength / (lineSpacing))).ToString());
            if (noLines == 0)
            {
                //need at least one line
                noLines++;
            }
            else
            {
                //all good
            };

            double thetaIncrement = (bulge < 0 ? -1 : 1) * arcLength / (radius * noLines);//the increment for each line wrt the center point
            double currentTheta = (startThetaPos ? 1 : -1) * startTheta;//current theta, eqivalent to theoretical angles[i]
            double nextTheta;//angle following the current theta, equivalent to theoretical angles[i+1]

            //Make all the lines from the given information. (Finally!)
            for (int i = 0; i < noLines; i++)
            {
                nextTheta = currentTheta + thetaIncrement;

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
                FindDrawingDims(line.p1.X, line.p1.Y);
                FindDrawingDims(line.p2.X, line.p2.Y);
                currentTheta = nextTheta;
                output.Add(line);
            }

            return output;
        }
        public void FindDrawingDims(float X, float Y)
        {
            //Drawing width
            if (X < _drawingMinX) { _drawingMinX = X; };
            if (X > _drawingMaxX) { _drawingMaxX = X; };
            //Drawing height
            if (Y < _drawingMinY) { _drawingMinY = Y; };
            if (Y > _drawingMaxY) { _drawingMaxY = Y; };
        }
        public event DxfEventHandler dxfEvent;
        public class DxfEventArgs : EventArgs
        {
            //Use this to update the ui thread - could have any data as required
            public List<Polyline> Polylines { get; set; }
            public List<Line> Lines { get; set; }
            public List<Arc> Arcs { get; set; }
            public List<Circle> Circles { get; set; }
            public PointF Origin { get; set; }
            public float DrawingHeight { get; set; }
            public float DrawingWidth { get; set; }
            public Units Units { get; set; }
        }
        public delegate void DxfEventHandler(Dxf dxf, DxfEventArgs e);
        public void DxfEventMethod()
        {

        }
    }
}
