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
using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.Interop;
using System.Runtime.InteropServices;
using Microsoft.Expression.Encoder;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CameraWindow : MahApps.Metro.Controls.MetroWindow
    {
        public CameraWindow()
        {
            InitializeComponent();
            this.Left = System.Windows.Forms.Screen.AllScreens[2].Bounds.Left;
            this.Top = System.Windows.Forms.Screen.AllScreens[2].Bounds.Top;
            this.Width = System.Windows.Forms.Screen.AllScreens[2].Bounds.Width;
            this.Height = System.Windows.Forms.Screen.AllScreens[2].Bounds.Height;
            StartCamera("UI154xLE-M_4002785771");
        }
        public void StartCamera(string videoSource)
        {
            try
            {

                DsDevice[] capDevices;
                capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                DsDevice dev = capDevices.FirstOrDefault();
                videoCapElement.VideoCaptureSource = dev.Name;
                

              //  DsDevice dev1 = MultimediaUtil.VideoInputDevices.FirstOrDefault();
              //  videoCapElement.VideoCaptureDevice = dev;
                object o;
                Guid IID_IBaseFilter = new Guid("56a86895-0ad4-11ce-b03a-0020af0ba770");
                dev.Mon.BindToObject(null, null, ref IID_IBaseFilter, out o);
                IAMVideoProcAmp vpa = (IAMVideoProcAmp)o;


               
                //WebCamCtrl.VidFormat = VideoFormat.mp4;
            //    videoCapElement.FPS = 15;
              //  videoCapElement.Height = 500;
              // videoCapElement.Width = 500;


                // Display webcam video on control.
                videoCapElement.Play();
               
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
             //   MessageBox.Show("Device is in use by another application");
            }
        }
        private void FindDevices()
        {
            //var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
           // var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);

            /*foreach (EncoderDevice dvc in vidDevices)
            {
                //comboVideo.Items.Add(dvc.Name);
            }
            /*if (comboVideo.Items.Count > 0)
            {
                comboVideo.SelectedIndex = 0;
            };*/

        }
    }
}
