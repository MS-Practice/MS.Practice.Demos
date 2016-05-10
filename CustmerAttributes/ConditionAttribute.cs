using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CustmerAttributes
{
    [Conditional("TEST")]
    [Conditional("VERIFY")]
    internal sealed class ConditionAttribute : Attribute
    {

    }
    [Condition]
    public sealed class Program
    {
        public static void Main()
        {
            Console.WriteLine("ConditionAttribute is {0} applied to Program type.", Attribute.IsDefined(typeof(Program), typeof(ConditionAttribute)) ? "" : "not ");
        }
    }
}
