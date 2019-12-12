using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mylib
{
    [ComVisible(true)]
    [Guid("61D7B4BA-1597-4AAA-93D9-582412D2A04F")]
    public interface IMyClass
    {
       
        int Add(int x, int y);
    }
    [ComVisible(true)]
    [Guid("61D7B4BA-1597-4AAA-93D9-582412D2A04F")]
    [ProgId("MyLib.MyClass")]

    public class Class1 : IMyClass
    {

        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
