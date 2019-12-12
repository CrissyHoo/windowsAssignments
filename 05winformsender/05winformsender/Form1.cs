using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05winformsender
{
    public partial class Form1 : Form
    {
        public const int WM_COPYDATA = 0x004A;
        public Form1()
        {
            InitializeComponent();
        }


        [DllImport("User32.dll",EntryPoint ="SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        //重新定义一个结构，跟lparam有关


        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }


        //找到接收消息的窗体的函数
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr WINDOW_HANDLER = FindWindow(null, "receiver3");
            if (WINDOW_HANDLER != IntPtr.Zero)
            {
                string text = this.textBox1.Text;
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
