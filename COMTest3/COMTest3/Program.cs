using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mylib;



namespace COMTest3
{
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            IMyClass a = null;
            Guid guid= new Guid("61D7B4BA-1597-4AAA-93D9-582412D2A04F");
            Type transactionType = Type.GetTypeFromCLSID(guid);   
            object transaction = Activator.CreateInstance(transactionType);
            a = transaction as IMyClass;
            int b =a.Add(2, 3);
            Console.WriteLine(b);
            Console.ReadKey();
            
            
        }

       
    }
}
