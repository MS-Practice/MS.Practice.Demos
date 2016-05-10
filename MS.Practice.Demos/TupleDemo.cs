using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    public class TupleDemo
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public delegate void TupleProgramDemosDelegate(string Y);
    public class TupleProgram
    {
        public void TupleProgramDemo(string Y)
        {
            TupleDemo td = new TupleDemo { X = 10, Y = 20 };
            Tuple<int, int> tp = new Tuple<int, int>(10, 20);
            Console.WriteLine(tp.Item1 + tp.Item2);
            Console.WriteLine(td.X + td.Y);
        }

        public void DelegateTupleProgram(string X, TupleProgramDemosDelegate TupleProgramDemo)
        {
            TupleProgramDemo(X);
        }

        public void Run() {
            DelegateTupleProgram("2", TupleProgramDemo1);
        }
        public void TupleProgramDemo1(string T)
        {
            //1 member
            Tuple<int> test = new Tuple<int>(1);
            Tuple<int, int> test2 = new Tuple<int, int>(1, 2);
            Tuple<int, int, int, int, int, int, int, Tuple<int>> test3 = new Tuple<int, int, int, int, int, int, int, Tuple<int>>(1, 2, 3, 4, 5, 6, 7, new Tuple<int>(8));
            Console.WriteLine(test.Item1);
            Console.WriteLine(test2.Item1 + test2.Item2);
            Console.WriteLine(test3.Item1 + test3.Item2 + test3.Item3 + test3.Item4 + test3.Item5 + test3.Item6 + test3.Item7 + test3.Rest.Item1);
        }
    }
}
