using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Configuration;
using System.Collections;

namespace MS.Practice.Demos
{
    class Program
    {
        public delegate void GreetingDelegate(string name);

        public class GreetingManager
        {
            public GreetingDelegate delegate1;
            public void GreetPeople(string name, GreetingDelegate MakeGreeting)
            {
                MakeGreeting(name);
            }
            public void GreetPeople(string name) {
                //如果存在委托变量，则直接调用委托
                if (delegate1 != null) {
                    delegate1(name);
                }
            }
        }
        static void Main(string[] args)
        {
            IDictionary IDTest1 = (IDictionary)ConfigurationSettings.GetConfig("Test1");
            string str = (string)IDTest1["setting1"] + " " + (string)IDTest1["setting2"];
            Console.WriteLine(str);
            //方法2
            string[] values = new string[IDTest1.Count];
            IDTest1.Values.CopyTo(values, 0);
            //获取Test2
            IDictionary IDTest2 = (IDictionary)ConfigurationSettings.GetConfig("Test2");
            string[] keys = new string[IDTest2.Keys.Count];
            string[] _values = new string[IDTest2.Values.Count];
            IDTest2.Keys.CopyTo(keys, 0);
            IDTest2.Values.CopyTo(values, 0);
            Console.WriteLine(keys[0] + " " + values[0]);
            
#if RELEASE
            #region 方法2
            //GreetingDelegate delegate1;
            //delegate1 = EnglishGreeting; // 先给委托类型的变量赋值
            //delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //GreetPeople("Jimmy Zhang", delegate1);   
            #endregion

            #region 绕过GreetPeople方法直接调用委托变量
            //GreetingDelegate delegate1;
            //delegate1 = EnglishGreeting; // 先给委托类型的变量赋值
            //delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //delegate1("Jimmy Zhang");

            //GreetingDelegate delegate1 = new GreetingDelegate(EnglishGreeting);
            //delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //GreetPeople("Jimmy Zhang", delegate1);
            //Console.WriteLine();

            //delegate1 -= EnglishGreeting; //取消对EnglishGreeting方法的绑定
            //// 将仅调用 ChineseGreeting 
            //GreetPeople("张子阳", delegate1); 
            #endregion

            #region 方法3委托的封装
            //GreetingManager gm = new GreetingManager();
            //gm.GreetPeople("毛帅", EnglishGreeting);
            //gm.GreetPeople("毛帅", ChineseGreeting); 
            //改进版
            GreetingManager gm = new GreetingManager();
            gm.delegate1 = EnglishGreeting;
            gm.delegate1 += ChineseGreeting;
            //gm.GreetPeople("毛帅", gm.delegate1);
            gm.GreetPeople("毛帅");
            #endregion 
#endif

            //GreetPeople("Jackey", EnglishGreeting);
            //GreetPeople("毛帅", ChineseGreeting);
            Console.Read();
        }

        public static void EnglishGreeting(string name)
        {
            Console.WriteLine("hellow," + name);
        }

        public static void ChineseGreeting(string name)
        {
            Console.WriteLine("你好，" + name);
        }
        //它接受一个GreetingDelegate类型的方法作为参数
        public static void GreetPeople(string name, GreetingDelegate MakeGreeting)
        {
            MakeGreeting(name);
        }

        public void Call(object o1, object o2, object o3) { }
        static void Main1(string[] arg) {
            int times = 1000000;
            Program program = new Program();
            object[] parameters = new object[] { new object(), new object(), new object() };
            program.Call(null, null, null); // force JIT-compile
        }
        public Func<object, object[], object> GetVolidDelegate() {
            Expression<Action<object, object[]>> exp = (instance, parameters) => ((Program)instance).Call(parameters[0], parameters[1], parameters[2]);
            Action<object, object[]> action = exp.Compile();
            return (instance, parameters) =>
            {
                action(instance, parameters);
                return null;
            };
        }
    }
}
