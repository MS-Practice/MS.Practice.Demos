using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MS.Practice.Demos.CustomerAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class MyselfAttribute : Attribute
    {
        //定义字段
        private string _name;
        private int _age;
        private string _memo;
        //必须定义构造函数 不提供 有编译器自动提供默认的无参构造函数
        public MyselfAttribute() { }
        public MyselfAttribute(string name, int age)
        {
            _name = name;
            _age = age;
        }
        //定义属性
        //特性和属性不是一回事
        public string Name
        {
            get { return _name == null ? string.Empty : _name; }
        }
        public int Age
        {
            get { return _age; }
        }
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
        //定义方法
        public void ShowName()
        {
            Console.WriteLine("Hello, {0}", _name == null ? "world." : _name);
        }
    }
    //应用自定义属性
    [Myself("Emma", 25, Memo = "Emma is my gool girl")]
    public class MyTest
    {
        public void SayHello()
        {
            Console.WriteLine("Hello, my.net world.");
        }
    }
    public class Myrun {
        public static void Main1(string[] arg)
        { 
            //如何反射确定特性信息
            Type tp = typeof(MyTest);
            MemberInfo info = tp;
            MyselfAttribute myAttribute = (MyselfAttribute)Attribute.GetCustomAttribute(tp, typeof(MyselfAttribute));
            if (myAttribute != null) {
                Console.WriteLine("Name: {0}", myAttribute.Name);
                Console.WriteLine("Age: {0}", myAttribute.Age);
                Console.WriteLine("Memo of {0} is {1}", myAttribute.Name, myAttribute.Memo);
                myAttribute.ShowName();
            }
            //多点反射
            object obj = Activator.CreateInstance(typeof(MyTest));
            MethodInfo mi = tp.GetMethod("SayHello");
            mi.Invoke(obj, null);

            DisplayRunningMessage();
            DisplayDebugMessage();
            MessageBox(0, "Hello", "Message", 0);
            Console.ReadLine();
        }
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int MessageBox(int hParent, string Message, string Caption, int Type);
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DisplayRunningMessage()
        {
            Console.WriteLine("开始运行Main子程序。当前时间是" + DateTime.Now);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        [Obsolete]
        public static void DisplayDebugMessage()
        {
            Console.WriteLine("开始Main子程序");
        }
    }
}
