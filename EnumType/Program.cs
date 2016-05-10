using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumType
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Enum.GetUnderlyingType(typeof(Color)));
            //EnumHandler.EnumFormat(typeof(Color), (byte)3, "G");
            //EnumHandler.EnumGetValues(typeof(Color));
            ////Orange定义为4 'c'被初始化为4
            //Color c = (Color)Enum.Parse(typeof(Color), "Orange", true);
            ////因为没有定义Brown，所以抛出一个ArgumentException异常
            //Color c1 = (Color)Enum.Parse(typeof(Color), "Brown", true);
            ////创建一个值为1的Color枚举类型实例
            //Enum.TryParse<Color>("1", false, out c);
            ////创建一个值为23的Color枚举类型实例
            //Enum.TryParse<Color>("23", false, out c);

            EnumFlagFileter.Go();
        }
    }
}
