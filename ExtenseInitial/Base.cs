using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ExtenseInitial
{
    class Base
    {
        private int i = 2;

        public Base()
        {
            Display();
        }

        private void Display()
        {
            WriteLine(i);
        }
    }
    class Derived : Base
    {
        private int i = 22;

        public Derived()
        {
            i = 222;
        }
        public void Display()
        {
            WriteLine(i);
        }
    }
}
