using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    internal static class DynamicClass
    {
        public static dynamic Plus(dynamic arg) {
            return arg + arg;
        }
        public static void M(Int32 n) {
            Console.WriteLine("M(Int32):" + n);
        }
        public static void M(string n)
        {
            Console.WriteLine("M(string):" + n);
        }
    }
}
