using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
namespace WpfApplication1
{
    static public class WindowPositioner
    {
        
        public static void PositionWindow(System.Windows.Window window)
        {

            Screen[] screens = Screen.AllScreens;
            window.Left = screens[2].Bounds.Left;
            window.Width = screens[2].WorkingArea.Width;
            window.Top = screens[2].WorkingArea.Top;
            window.Height = screens[2].WorkingArea.Height;
        }
    }
}
