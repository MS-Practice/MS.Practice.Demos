using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewProgram
{
    class IntNumber : Number
    {
        new public static int i = 456;
        public new virtual void ShowInfo()
        {
            Console.WriteLine("Derived class---");
        }
        public override void ShowNumber()
        {
            Console.WriteLine("Base number is {0}", Number.i.ToString());
            Console.WriteLine("New number is {0}", i.ToString());
        }
    }
}
