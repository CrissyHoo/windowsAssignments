using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireEventTest
{
    public partial class FireEventTest : Form
    {
        public static FireEventTest form;
        public FireEventTest()
        {
            InitializeComponent();
            form = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //创建火警警报实例

        FireAlarm myFireAlarm = new FireAlarm();
             
        // 创建解决火情的实例，alarm是参数

        FireHandlerClass myFireHandler = new FireHandlerClass(myFireAlarm);
            
        //创建一些火情
        myFireAlarm.ActivateFireAlarm("Kitchen", 3);
        myFireAlarm.ActivateFireAlarm("Study", 1);
        myFireAlarm.ActivateFireAlarm("Porch", 5);
       
        return;

        }
    }   

    public class FireEventArgs : EventArgs
    {
        public FireEventArgs(string room, int ferocity)
        {
            this.room = room;
            this.ferocity = ferocity;
        }

        // 这个事件有两个内容，一个是发生地点，二是灾情等级

        public string room;
        public int ferocity;

    }

    public class FireAlarm
    {

        // 将火情处理定义为FireEventHandler 代理(delegate) 类型，这个代理声明的事件的参数列表

        public delegate void FireEventHandler(object sender, FireEventArgs fe);

        // 定义FireEvent 为FireEventHandler delegate 事件(event) 类型.

        public event FireEventHandler FireEvent;

        // 激活事件的方法，创建了FireEventArgs 对象，发起事件，并将事件参数对象传递过去

        public void ActivateFireAlarm(string room, int ferocity)
        {

            FireEventArgs fireArgs = new FireEventArgs(room, ferocity);

            // 执行对象事件处理函数指针，必须保证处理函数要和声明代理时的参数列表相同

            FireEvent(this, fireArgs);
        }
    }


    // 处理事件的类

    class FireHandlerClass
    {
        
        public FireHandlerClass(FireAlarm fireAlarm)
        {


            // ExtinguishFire.添加一个包含处理火情的函数的委托，，所以当警报响起时，会接下来执行这个函数

            fireAlarm.FireEvent += new FireAlarm.FireEventHandler(ExtinguishFire);
        }


        // 火情alarm时这个函数将会执行

        void ExtinguishFire(object sender, FireEventArgs fe)
        {
            
            FireEventTest.form.textBox1.AppendText("处理火情的函数是被这个调用的： " + sender.ToString()+"\n");

            if (fe.ferocity < 2)
                FireEventTest.form.textBox1.AppendText(Environment.NewLine+"发生在" + fe.room+"的火情已经没事了。我将通过泼水来挽救."+ Environment.NewLine);
            else if (fe.ferocity < 5)
                FireEventTest.form.textBox1.AppendText(Environment.NewLine+"我用灭火器扑灭" + fe.room+"里的火。"+ Environment.NewLine);
            else
                FireEventTest.form.textBox1.AppendText(Environment.NewLine+fe.room + "里的火情失控了!我将打电话给消防局!"+ Environment.NewLine);
        }
    }    //end of class FireHandlerClass



    
}
