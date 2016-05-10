using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    internal static class StringBuilderExtensions
    {
        public static Int32 IndexOf(this StringBuilder sb, Char value)
        {
            for (Int32 index = 0; index < sb.Length; index++)
            {
                if (sb[index] == value) return index;
            }
            return -1;
        }
    }
}
