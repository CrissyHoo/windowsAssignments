using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using calcus;
 
namespace Calculator
{
   
    public partial class Form1 : Form
    {
        Class1 t = new Class1();
        double num1, num2, result;
        string myOp;
        bool isFloat = false;

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("8");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("9");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("7");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("6");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("5");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("4");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("1");


        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("3");
        }

        public Form1()
        {
            InitializeComponent();
        }

       

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "0.";
            }
            //如果再次输入.则返回都输入的字符后边并提示信息
            else if (textBox1.Text.IndexOf(".") >= 0)
            {
                MessageBox.Show("已经输入小数点,无须再次输入", "提示");
            }
            else
            {
                //前边有数字时，则直接在后边加上.
                textBox1.AppendText(".");
            }
            isFloat = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //+
            myOp = "+";
            if (isFloat == false)
            {
                num1 = int.Parse(textBox1.Text);
            }
            else
            {
                num1 = double.Parse(textBox1.Text);
            }

            textBox1.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            myOp = "*";
            if (isFloat == false)
            {
                num1 = int.Parse(textBox1.Text);
            }
            else
            {
                num1 = double.Parse(textBox1.Text);
            }
            textBox1.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            myOp = "/";
            if (isFloat == false)
            {
                num1 = int.Parse(textBox1.Text);
            }
            else
            {
                num1 = double.Parse(textBox1.Text);
            }
            textBox1.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);

            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("0");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //=
            try
            {
                if (isFloat == false)
                {
                    num2 = int.Parse(textBox1.Text);
                }
                else
                {
                    num2 = double.Parse(textBox1.Text);
                }
            }catch (Exception)
            {
                MessageBox.Show("还没输入数字呢", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            switch (myOp)//用switch进行响应的分支操作
            {
                case "+"://加号

                    if (isFloat == false)
                    {
                        result = (int)t.plus(num1, num2);
                    }
                    else
                    {
                        result = t.plus(num1, num2);
                    }
                    textBox1.Text = result.ToString();
                    break;
                case "-"://减号
                    if (isFloat == false)
                    {
                        result = (int)t.minus(num1, num2);
                    }
                    else
                    {
                        result = t.minus(num1, num2);
                    }
                    textBox1.Text = result.ToString();
                    break;
                case "*"://乘号
                    if (isFloat == false)
                    {
                        result = (int)t.mult(num1, num2);
                    }
                    else
                    {
                        result = t.mult(num1, num2);
                    }
                    textBox1.Text = result.ToString();
                    break;
                case "/"://除号
                    if (num2 == 0||num2==0.0)//除数为0报错
                    {
                        textBox1.Text = "除数不能为“0”!";
                    }
                    else
                    {
                        if (isFloat == false)
                        {
                            result = (int)t.div(num1, num2);
                        }
                        else
                        {
                            result = t.div(num1, num2);
                        }
                        textBox1.Text = result.ToString();
                    }
                    break;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            myOp = "-";
            if (isFloat == false)
            {
                num1 = int.Parse(textBox1.Text);
            }
            else
            {
                num1 = double.Parse(textBox1.Text);
            }
            textBox1.Clear();
        }

      

    }
}
