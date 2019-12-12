using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DLLTestCs;  //对用csharp写的dll的引用

namespace test2
{
    class Program
    {
        //对cpp写的dll的引用
        [DllImport(@"C:\Users\JudyHu\source\repos\DLLTest\Release\DLLTest.dll", EntryPoint = "test1", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        extern static int test1(int a, int b);

        static void Main(string[] args)
        {
            //用cpp写的dll的应用
            int r1 = test1(1, 2);
            //用C#写的dll的应用
            Test t = new Test();
            int r2 = t.mul(3,4);
            Console.WriteLine("testcpp结果：" + r1.ToString());
            Console.WriteLine("testcsharp结果：" + r2.ToString());
            Console.ReadKey();
        }
    }
}
