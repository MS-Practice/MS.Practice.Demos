using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace InterfaceDemo
{
    public class Children
    {
        static string assemblyName = ConfigurationManager.AppSettings["assemblyName"];
        static string typeName = ConfigurationManager.AppSettings["typeName"];
        public static IDemo idemo
        {
            get {
                return Assembly.Load(assemblyName).CreateInstance(typeName) as IDemo;
            }
        }
        public void Method() {
            Console.WriteLine("实现了对接口成员方法的实现");
        }
    }
}
