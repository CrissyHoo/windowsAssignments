using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RegDll
{
    class Program
    {
        

    static void Main(string[] args)
        {
           
            string Winrarpath = @"Software\MyTestKey";
            string key = "MyKeyName";
            string value = RegDll.GetRegistryValue(Winrarpath, key);
            
            
            RegDll.SetRegistryKey("HKEY_CURRENT_USER", Winrarpath, "MyKeyName", "hello");
            
            Console.ReadLine();
        }

       
    }
}
