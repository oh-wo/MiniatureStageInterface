using System;
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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Threading;
/*
 * 
using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.Interop;
using Microsoft.Expression.Encoder;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
*/
namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    public partial class CameraWindow : MahApps.Metro.Controls.MetroWindow
    {
        public bool CrossHairConfigured = false;
        
        public CameraWindow()
        {
            InitializeComponent();
            this.Left = System.Windows.Forms.Screen.AllScreens[0].Bounds.Left;
            this.Top = System.Windows.Forms.Screen.AllScreens[0].Bounds.Top;
            this.Width = System.Windows.Forms.Screen.AllScreens[0].Bounds.Width;
            this.Height = System.Windows.Forms.Screen.AllScreens[0].Bounds.Height;
            StartCamera("UI154xLE-M_4002785771");
        }

        
        public void ConfigureCrossHair()
        {
            if (frameHolder.ActualHeight > 0 && frameHolder.ActualWidth > 0)
            {
                mousePos = (Mouse.GetPosition(this));
                CrossHairConfigured = true;

                double height = 300;
                double width = 300;
                double bottom = frameHolder.ActualHeight / 2 - height / 2;
                double top = frameHolder.ActualHeight / 2 + height / 2;
                double left = frameHolder.ActualWidth / 2 - width / 2;
                double right = frameHolder.ActualWidth / 2 + width / 2;

                Line lineX1 = new Line();
                lineX1.X1 = left;
                lineX1.X2 = right;
                lineX1.Y1 = top;
                lineX1.Y2 = top;
                lineX1.Uid = "x1";
                lineX1.StrokeThickness = 5;
                lineX1.Stroke = System.Windows.Media.Brushes.Red;
                lineX1.MouseDown += lineX_MouseDown;
                cameraOverlay.Children.Add(lineX1);
                Line lineX2 = new Line();
                lineX2.X1 = left;
                lineX2.X2 = right;
                lineX2.Y1 = bottom;
                lineX2.Y2 = bottom;
                lineX2.StrokeThickness = 5;
                lineX2.Uid = "x2";
                lineX2.Stroke = System.Windows.Media.Brushes.Red;
                lineX2.MouseDown += lineX_MouseDown;
                cameraOverlay.Children.Add(lineX2);
                Line lineY1 = new Line();
                lineY1.X1 = left;
                lineY1.X2 = left;
                lineY1.Y1 = top;
                lineY1.Y2 = bottom;
                lineY1.StrokeThickness = 5;
                lineY1.Uid = "y1";
                lineY1.Stroke = System.Windows.Media.Brushes.Red;
                lineY1.MouseDown += lineY_MouseDown;
                cameraOverlay.Children.Add(lineY1);
                Line lineY2 = new Line();
                lineY2.X1 = right;
                lineY2.X2 = right;
                lineY2.Y1 = top;
                lineY2.Y2 = bottom;
                lineY2.StrokeThickness = 5;
                lineY2.Uid = "y2";
                lineY2.Stroke = System.Windows.Media.Brushes.Red;
                lineY2.MouseDown += lineY_MouseDown;
                cameraOverlay.Children.Add(lineY2);


                frameHolder.MouseMove += cameraOverlay_MouseMove;
            }
        }

        void lineY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lineYMoving = true;
            mousePos = (Mouse.GetPosition(this));
        }
        System.Windows.Point mousePos;
        void cameraOverlay_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (lineXMoving)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    System.Windows.Point newMousePos = (Mouse.GetPosition(this));
                    double yDiff = newMousePos.Y-mousePos.Y;
                    foreach (Line line in cameraOverlay.Children)
                    {
                        
                        if (line.Uid.Contains("x1"))
                        {
                            line.Y1 += yDiff;
                            line.Y2 += yDiff;
                        }
                        if (line.Uid.Contains("x2"))
                        {
                            line.Y1 -= yDiff;
                            line.Y2 -= yDiff;
                        }
                        if (line.Uid.Contains("y1"))
                        {
                            line.Y1 += yDiff;
                            line.Y2 -= yDiff;
                        }
                        if (line.Uid.Contains("y2"))
                        {
                            line.Y1 += yDiff;
                            line.Y2 -= yDiff;
                        }
                    }
                    mousePos = newMousePos;
                }
                else
                {
                    lineXMoving = false;
                }
            }
            if (lineYMoving)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    System.Windows.Point newMousePos = (Mouse.GetPosition(this));
                    double xDiff = newMousePos.X - mousePos.X;
                    foreach (Line line in cameraOverlay.Children)
                    {

                        if (line.Uid.Contains("x1"))
                        {
                            line.X1 += xDiff;
                            line.X2 -= xDiff;
                        }
                        if (line.Uid.Contains("x2"))
                        {
                            line.X1 += xDiff;
                            line.X2 -= xDiff;
                        }
                        if (line.Uid.Contains("y1"))
                        {
                            line.X1 += xDiff;
                            line.X2 += xDiff;
                        }
                        if (line.Uid.Contains("y2"))
                        {
                            line.X1 -= xDiff;
                            line.X2 -= xDiff;
                        }
                    }
                    mousePos = newMousePos;
                }
                else
                {
                    lineXMoving = false;
                }
            }
        }

        bool lineXMoving = false;
        bool lineYMoving = false;
        void lineX_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lineXMoving = true;
            mousePos = (Mouse.GetPosition(this));
        }

        public void StartCamera(string videoSource)
        {
            try
            {

                LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                LocalWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
                LocalWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);

                LocalWebCam.Start();
               
            }
            catch (Exception ex)
            {
             //   MessageBox.Show("Device is in use by another application");
            }
        }
        /* OLD START CAMERA CODE
         * 
         *  DsDevice[] capDevices;
                capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                DsDevice dev = capDevices.FirstOrDefault();
                videoCapElement.VideoCaptureSource = dev.Name;
                

              //  DsDevice dev1 = MultimediaUtil.VideoInputDevices.FirstOrDefault();
              //  videoCapElement.VideoCaptureDevice = dev;


                ////object o;
                ////Guid IID_IBaseFilter = new Guid("56a86895-0ad4-11ce-b03a-0020af0ba770");
                ////dev.Mon.BindToObject(null, null, ref IID_IBaseFilter, out o);
                ////IAMVideoProcAmp vpa = (IAMVideoProcAmp)o;


               
                //WebCamCtrl.VidFormat = VideoFormat.mp4;
            //    videoCapElement.FPS = 15;
              //  videoCapElement.Height = 500;
              // videoCapElement.Width = 500;


                // Display webcam video on control.
                videoCapElement.Play();
         * 
         * 
         * */


        float brightness = 1; // no change in brightness
        float contrast = 2; // twice the contrast
        bool recording = false;
        string videoTitle = "";
        bool showDateTimeInVideo = true;
        VideoCaptureDevice LocalWebCam;
        public FilterInfoCollection LoaclWebCamsCollection;

        Font drawFont = new Font("Arial", 16);
        BitmapImage bi2 = new BitmapImage();
        void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();
                //using (Bitmap bmp = BitmapImage2Bitmap(bi))
                //{
                //    bi2 = bi;
                //    if (showDateTimeInVideo)
                //    {
                        
                //        Graphics g = Graphics.FromImage(bmp);
                //        g.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), drawFont, System.Drawing.Brushes.Red, new System.Drawing.PointF(((float)bmp.Width * (float)0.1), (float)bmp.Height * (float)0.8));
                //        bi2 = Bitmap2BitmapImage(bmp);
                //        g.Dispose();
                //    }
                //    bi2.Freeze(); 
                //    bmp.Dispose();
                   

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        frameHolder.Source = bi;
                    }));
                //}
                    if (!CrossHairConfigured)
                    {
                        var d = System.Windows.Application.Current.Dispatcher;
                        if (d.CheckAccess())
                            ConfigureCrossHair();
                        else
                            d.BeginInvoke((Action)ConfigureCrossHair);
                        ;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new Bitmap(bitmap);
            }
        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            (bitmap).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        private void buttonChangeContrast_Click(object sender, EventArgs e)
        {
            if (float.TryParse(this.textboxContrast.Text, out contrast))
            {

            }
            else
            {
                this.textboxContrast.Text = "1";
            }
        }

        private void buttonChangeBrightness_Click(object sender, EventArgs e)
        {
            if (float.TryParse(this.textboxBrightness.Text, out brightness))
            {

            }
            else
            {
                this.textboxBrightness.Text = "1";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LocalWebCam.Stop();
            StartCamera("UI154xLE-M_4002785771");
        }



    }
}
