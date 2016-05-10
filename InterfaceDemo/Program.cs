using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace InterfaceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IDemo im = Children.idemo;
            Console.ReadLine();
            //SomeValueType v = new SomeValueType(0);
            //Object obj = new object();
            //Int32 n = v.CompareTo(v);
            //n = v.CompareTo(obj);

            //EIMI
            SomeValueType v = new SomeValueType(0);
            Object o = new object();
            Int32 n = v.CompareTo(v);
            //n = v.CompareTo(o); //类型是安全的 不符合的类型会显示报错

            Int32 x = 5;
            //Single s = x.ToSingle(null);
            Single s = ((IConvertible)x).ToSingle(null);

        }
    }
}
