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
            WindowPositioner.PositionWindow(this);
        }
        public void StartCamera(string videoSource)
        {
            try
            {
                Binding bndg_1 = new Binding("SelectedValue");
                bndg_1.Source = videoSource;

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
}
