using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace TestProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Byte b = 100;
            ////b = checked((Byte)(b + 200)); //报错 溢出
            //b = (Byte)checked(b + 200);
            ////以上等价于
            //checked
            //{
            //    Byte c = 100;
            //    c = (Byte)(c + 200);
            //}

            Point p = new Point(1, 1);
            Console.WriteLine(p);
            p.Change(2, 2);
            Console.WriteLine(p);
            Object o = p;   //对p进行装箱
            Console.WriteLine(o);
            ((Point)o).Change(3, 3);//已装箱的o进行拆箱，会赋值o中的字段到临时的Point中并调用Change  注：对已装箱的o还存在
            Console.WriteLine(o);   //由于已装箱的o还在，那么输出的还是2，2
            //对p进行装箱，更改已装箱的对象 然后丢弃它
            ((IChangeBoxedPoint)p).Change(4, 4); //p转化为接口：装箱 值会变4 但是调用Change之后 对已装箱的对象准备好进行垃圾回收
            Console.WriteLine(p);   //所以还是会显示2，2
            //更改已装箱的对象，并显示它
            ((IChangeBoxedPoint)o).Change(5, 5);
            Console.WriteLine(o);

            for (Int32 demo = 0; demo < 2; demo++)
            {
                dynamic arg = (demo == 0) ? (dynamic)5 : (dynamic)"A";
                dynamic result = DynamicClass.Plus(arg);
                DynamicClass.M(result);
            }
            Object target = "Marson Swift";
            Object arg1 = "ff";
            //Boolean result2 = ((String)target).Contains(arg1.ToString());
            //在目标上查找和希望的实参的类型匹配的一个方法
            Type[] types = new Type[] { arg1.GetType() };
            MethodInfo method = target.GetType().GetMethod("Contains", types);
            //在目标调用方法，传递希望的实参
            Object[] arguments = new object[] { arg1 };
            Boolean result1 = Convert.ToBoolean(method.Invoke(target, arguments));

            dynamic target_v2 = "Marson Swift";
            dynamic arg_v2 = "ff";
            target_v2.Contains(arg_v2);
            
            IsAccepetArgument(name);
            AStaticClass.AStaticMethod("ZQ");
            Console.WriteLine(AStaticClass.AStaticProperty);
            const Int32 iterations = 1000 * 1000 * 1000;
            PerTest1(iterations);
            PerTest2(iterations);

            ConversionOperator.Go();
            StringBuilder sb = new StringBuilder();
            Console.WriteLine(sb.Append("asdajieufidjfk").IndexOf('s'));

            DefaultsParameters(str: "MS", c: 'S');
        }
        private static string name = "MS";
        static void IsAccepetArgument(string name)
        {
            Console.WriteLine(name);
        }
        /// <summary>
        /// 这个函数被调用时，BeforFieldInit和Precise类的类型构造器没有被执行，
        /// 所以这些类型构造器的调用将嵌入这个方法的代码中，使它的运行变慢
        /// </summary>
        /// <param name="iterations"></param>
        private static void PerTest1(Int32 iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();

            for (Int32 i = 0; i < iterations; i++)
            {
                //JIT编译器优化调用BeforeFieldInit的类型构造器的代码
                //所以对这些构造器的调用嵌入这个方法当中，使它的运行较慢
                BeforeFieldInit.s_x = 1;
            }
            Console.WriteLine("PerTest1:{0} BeforeFieldInit", sw.Elapsed);
            sw = Stopwatch.StartNew();
            for (Int32 i = 0; i < iterations; i++)
            {
                //JIT编译器在这里生成调用Presice类的类型构造器的代码
                //所以每次循环迭代，它都要核实一遍是否需要调用构造器
                Precise.s_x = 1;
            }
            Console.WriteLine("PerTest1:{0} Percise", sw.Elapsed);
        }
        /// <summary>
        /// 当这个方法进行JIT编译时，BeforeFieldInit和Percise类的类型构造器已经执行过了。所以这个方法的代码中
        /// 不会在生成对这些构造器的调用了，使它运行得更快
        /// </summary>
        /// <param name="iterations"></param>
        private static void PerTest2(Int32 iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (Int32 i = 0; i < iterations; i++)
            {
                BeforeFieldInit.s_x = 1;
            }
            Console.WriteLine("PerTest2:{0} BeforeFieldInit", sw.Elapsed);

            sw = Stopwatch.StartNew();
            for (Int32 i = 0; i < iterations; i++)
            {
                Precise.s_x = 1;
            }
            Console.WriteLine("PerTest2:{0} Precise", sw.Elapsed);

            Console.ReadLine();
        }

        private static void DefaultsParameters(int n = 5, string str = "Marson Swift", char c = 'M')
        {
            Console.WriteLine("n={0},str={1},c={2}", n, str, c);
        }
    }
    /// <summary>
    /// 由于这个类没有显示定义类型构造器，所以C#中的元数据用berforefieldinit来标记类型定义
    /// </summary>
    internal sealed class BeforeFieldInit {
        public static Int32 s_x = 123;
    }
    /// <summary>
    /// 这个类由于显示调用类型构造器，所以不用beforefieldinit标记类定义
    /// </summary>
    internal sealed class Precise {
        public static Int32 s_x;
        //显示调用类型构造器
        static Precise() {
            s_x = 123;
        }
    }
}
