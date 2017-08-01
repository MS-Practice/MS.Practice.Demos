using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommomCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace TestMs
{
    class Program
    {
        static void Main(string[] args)
        {
            ChildClass cc = new ChildClass();
            cc.Method();
            BaseClass bc = new BaseClass();
            bc.Method();
            //String ss = "";
            //Type t = ss.GetType();
            //Console.WriteLine(t);

            //Console.WriteLine("Before JIT.");
            //Console.ReadLine();
            
            //TestClass.TestMethod(1);

            //Console.WriteLine("After JIT");
            //Console.ReadLine();

            //TestClass.TestMethod(1);
            //string[] dateStrings = 
            //{
            //    "2009-01-01", "2009-01-02", "2009-01-03",
            //    "2009-01-04", "2009-01-05", "2009-01-06",
            //};
            //DateTime[] dates = Array.ConvertAll<string, DateTime>(dateStrings, new Converter<string, DateTime>(TestClass.StringToDateTime));
            //DateTime[] dates1 = Array.ConvertAll<string, DateTime>(dateStrings, delegate(string s)
            //{
            //    return DateTime.ParseExact(s, "yyyy-MM-dd", null);
            //});
        }
    }
    public static class TestClass
    {
        public static int TestMethod(int i)
        {
            return i;
        }
        public static T TestMethod<T>(object value)
        {
            return (T)value;
        }
        public static DateTime StringToDateTime(string s)
        {
            return DateTime.ParseExact(s, "yyyy-MM-dd", null);
        }
    }

    internal class BaseClass
    {
        public void Method()
        {
            Console.WriteLine("Parent Method");
            Console.ReadLine();
        }
        public virtual void Method1() {
            Console.WriteLine("Parent Method");
            Console.WriteLine();
        }
    }
    internal class ChildClass : BaseClass
    {
        public void Method() {
            Console.WriteLine("Child Method");
            Console.ReadLine();
        }
        //new public void Method1()
        //{
        //    Console.WriteLine("Child Method");
        //    Console.WriteLine();
        //}
        //public void Method()
        //{
        //    Console.WriteLine("Child Method");
        //    Console.WriteLine();
        //}
    }

    public sealed class OSHandler:IDisposable {
        private IntPtr handler;
        public OSHandler(IntPtr handler) {
            this.handler = handler;
        }
        ~OSHandler()
        {
            CloseHandler(handler);
        }
        public IntPtr ToHandler() { return handler; }
        public static implicit operator IntPtr(OSHandler osHandler) {
            return osHandler.ToHandler();
        }
        [DllImport("Kernel32")]
        private extern static bool CloseHandler(IntPtr handler);

        public void Dispose()
        {
            
        }
    }
}
