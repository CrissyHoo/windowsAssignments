using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFReceiver;

namespace _05WPFsendMsg
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class send : Window
    {
        public const int WM_COPYDATA = 0x004A;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        //重新定义一个结构，跟lparam有关
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData; //句柄
            public int cbData;//传递内容的长度
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;//要传递的内容
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string  lpWindowName);

        public send()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            IntPtr WINDOW_HANDLER = FindWindow(null, "Receiver");
            if (WINDOW_HANDLER != IntPtr.Zero)
            {
                string text = this.sendBox.Text;
                byte[] sarr = System.Text.Encoding.Default.GetBytes(text);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = text;
                cds.cbData = len + 1;
                SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref cds);
            }

        }
    }
}
