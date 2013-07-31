using System;
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
        public float lineSpacing = 100;
        public Form1()
        {
            InitializeComponent();
            /*panel1.AutoScroll = true;
            panel1.AutoScrollMinSize = new Size(1000, 1000);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
            panel1.Scroll += new ScrollEventHandler(panel1_Scroll);*/
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            

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
                    case "AcDbCircle":
                        GetCircleProperties(file, i);
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
            foreach (Circle circle in Circles)
            {
                circle.laserLines = ConvertCircleToLines(circle, lineSpacing);
            }
            foreach (Polyline pline in Polylines)
            {
                this.textOutput.Text += String.Format("Polyline: {0} vertices, {1} \r\n", pline.noVerticies, pline.closed?"closed":"open");
                pline.laserLines = new List<Line>();
                for(int i=0; i<pline.noVerticies; i++)
                {
                    //display verticies to the user
                    this.textOutput.Text += String.Format("          Vertex {0}: ({1},{2}) {3}\r\n", i,pline.verticies[i].Point.X,pline.verticies[i].Point.Y,pline.verticies[i].Buldge!=null?pline.verticies[i].Buldge.ToString():"");
                  

                    //convert vertex to laser line and convert bulges to lines if necessary. 
                    
                    if(pline.verticies[i].Buldge==null){
                        Line laserLine = new Line();
                        //straight line
                        laserLine.p1=new PointF{
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
                        }else{
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
                    }else{
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
            return System.IO.File.ReadAllLines(String.Format("{0}\\Files\\Drawing1.dxf",Application.StartupPath));
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

        public void textScale_textChanged(object sender, EventArgs e)
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

        Pen _pen = new Pen(Color.Black, 1);
        private Graphics _Graphics;
        private Bitmap image;
        
        public void DrawDrawing()
        {
             _Graphics = Graphics.FromImage(image);
            _Graphics.Clear(System.Drawing.Color.White);
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
            float xOffset = (float)this.hScrollBar1.Value / 100 * drawingWidth/scale+2000;
            float yOffset = (float)this.vScrollBar1.Value / 100 * drawingHeight/scale;
            foreach (Polyline pLine in Polylines)
            {
                for (int i = 0; i < pLine.laserLines.Count; i++)
                {
                    DrawLine(pLine.laserLines[i].p1.X, pLine.laserLines[i].p1.Y, pLine.laserLines[i].p2.X, pLine.laserLines[i].p2.Y,xOffset,yOffset,scale);
                }
            }
            foreach (Line line in Lines)
            {
                DrawLine(line.p1.X, line.p1.Y, line.p2.X, line.p2.Y, xOffset, yOffset, scale);
            }
            foreach(Circle circle in Circles)
            {
                for (int i = 0; i < circle.laserLines.Count; i++)
                {
                    DrawLine(circle.laserLines[i].p1.X, circle.laserLines[i].p1.Y, circle.laserLines[i].p2.X, circle.laserLines[i].p2.Y, xOffset, yOffset, scale);
                }
            }
            this.pictureBox1.Image = image;
            this.pictureBox1.Refresh();
            this.pictureBox1.Invalidate();
        }

        public void DrawLine(float X1, float Y1, float X2, float Y2, float xOffset, float yOffset, float scale)
        {
            _Graphics.DrawLine(_pen, int.Parse(Math.Round((X1 - xOffset) / scale).ToString()), int.Parse(Math.Round((drawingHeight - Y1 - yOffset) / scale).ToString()), int.Parse(Math.Round((X2 - xOffset) / scale).ToString()), int.Parse(Math.Round((drawingHeight - Y2 - yOffset) / scale).ToString()));
        }
        public void pictureBox1_Paint(object sender, EventArgs e)
        {
            // pictureBox1.Image = image;

        }
        
    }
}
