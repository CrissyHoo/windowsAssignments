using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace redirect
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 


    public delegate void DelReadStdOutput(string result);
    

    public partial class MainWindow : Window
    {

        public event DelReadStdOutput ReadStdOutput;
       
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //将相应函数注册到委托事件中
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
            
        }





        private void RealAction(string StartFileName, string StartFileArg,string cmd)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数
           

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出
                                                                //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            
            CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件
            CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件

            CmdProcess.Start();
            CmdProcess.StandardInput.WriteLine(cmd);
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();

               
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                // 4. 异步调用，需要invoke
                this.Dispatcher.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }

        

        private void ReadStdOutputAction(string result)
        {
           this.textBox1.AppendText(result + "\r\n");
        }

        

        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            // 执行结束后触发
        }
    


        private void Button_Click(object sender, RoutedEventArgs e)
        {


            RealAction("cmd.exe", textBox1.Text,"ping www.whu.edu.cn -n 20");



        }

        
       


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            string strCmd1 = "Shutdown -s -t 7200";
            process.Start();
            process.StandardInput.WriteLine(strCmd1);
            process.StandardInput.WriteLine("exit");
            //显示
            textBox1.Text = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
        }
    }
}
