using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Index
{
    class Program
    {
        static void Main(string[] args)
        {
            BitArray ba = new BitArray(14);
            for (int i = 0; i < 14; i++)
            {
                ba[i] = (i % 2 == 0);
            }
            for (int i = 0; i < 14; i++)
            {
                Console.WriteLine("Bit " + i + " is " + (ba[i] ? "On" : "Off"));
            }
        }
    }
}
