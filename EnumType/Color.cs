using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumType
{
    internal enum Color : byte
    {
        White,
        Red,
        Green,
        Blue,
        Orange
    }
    internal class EnumHandler
    {
        public static void EnumFormat(Type enumType, Object value, String format)
        {
            Console.WriteLine(Enum.Format(enumType, value, format));
        }
        public static void EnumGetValues(Type enumType)
        {
            Color[] colors = (Color[])Enum.GetValues(enumType);
            Console.WriteLine("Number of symbol defined: " + colors.Length);
            Console.WriteLine("Value\tSymbol\n-----\t------");
            foreach (Color c in colors)
            {
                //以十进制和常规格式显示每个符号
                Console.WriteLine("{0,5:D}\t{0:G}", c);
            }
        }
    }
}
