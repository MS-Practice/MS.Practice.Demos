using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewProgram
{
    class Number
    {
        public static int i = 120;
        public virtual void ShowInfo()
        {
            Console.WriteLine("bass class---");
        }
        public virtual void ShowNumber()
        {
            Console.WriteLine(i.ToString());
        }
    }
}
