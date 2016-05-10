using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;
using System.Reflection;

namespace DelegateSolution
{
    class Program
    {
        //定义一个查询状态的委托
        private delegate String GetStatus();

        internal delegate Object TwoInt32s(Int32 n1, Int32 n2);
        internal delegate Object OneString(String s1);
        static void Main(string[] args)
        {
            //GetStatus getStatus = null;
            ////构造三个组件，将他们的状态方法添加到委托链中
            //getStatus += new GetStatus(new Light().SwitchPostion);
            //getStatus += new GetStatus(new Fan().Speed);
            //getStatus += new GetStatus(new Speaker().Volume);
            ////显示整理好的状态报告，反映这三个组件的状态
            //Console.WriteLine(GetComponentStatusReport(getStatus));
            //AClass.CallBackWithOutNewingDelegateObject();

            //使用CreateDelegate和DynamicInvoke方法
            if (args.Length < 2)
            {
                String fileName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
                String usage =
                    @"Usage:" +
                    "{0}{1} delType methodName [Arg1][Arg2]" +
                    "{0}    where delType must be TwoInt32s or OneString" +
                    "{0}    if delType is TwoInt32s,methodName must be Add or Substract" +
                    "{0}    if delType is OneString,methodName must be NumberChars or Reverse" +
                    "{0}" +
                    "{0}Examples:" +
                    "{0}   {1} TwoInt32s Add 123 321" +
                    "{0}   {1} TwoInt32s Subtract 123 321" +
                    "{0}   {1} OneString NumChars \"Hello there\"" +
                    "{0}   {1} OneString Reverse  \"Hello there\"";
                Console.WriteLine(usage, Environment.NewLine, fileName);
                return;
            }
            //讲delType参数转化为一个委托类型
            Type delType = Type.GetType(args[0]);
            if (delType == null)
            {
                Console.WriteLine("Invalid delType argument:" + args[0]);
                return;
            }
            Delegate d;
            try
            {
                //将Args参数转换一个方法
                MethodInfo mi = typeof(Program).GetMethod(args[1],
                    BindingFlags.NonPublic | BindingFlags.Static);
                //创建包装了静态方法的一个委托对象
                d = Delegate.CreateDelegate(delType, mi);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid methodName argument:" + args[1]);
                return;
            }
            //创建一个数组，其中只包含要通过委托对象传递给方法的参数
            Object[] callbackArgs = new Object[args.Length - 2];
            if (d.GetType() == typeof(TwoInt32s)) {
                try
                {
                    //讲String类型的参数传递给Int32类型的参数
                    for (Int32 a = 2; a < args.Length; a++)
                    {
                        callbackArgs[a - 2] = Int32.Parse(args[a]);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Parameters must be intergers.");
                    return;
                }
            }
            if (d.GetType() == typeof(OneString)) { 
                //只复制String参数
                Array.Copy(args, 2, callbackArgs, 0, callbackArgs.Length);
            }
            try
            {
                //调用委托并显示结果
                Object result = d.DynamicInvoke(callbackArgs);
                Console.WriteLine("Result = " + result);
            }
            catch (TargetParameterCountException)
            {
                Console.WriteLine("Incorrect number of parameters specified.");
            }
        }

        private static string GetComponentStatusReport(GetStatus status)
        {
            //如果委托链为空,并返回一个状态报告
            if (status == null) return null;
            //用下面的变量创建状态报告
            StringBuilder report = new StringBuilder();
            //获得一个数组，其中每个元素都是委托链中的委托
            Delegate[] arrayOfDelegates = status.GetInvocationList();
            //遍历数组中的每一个委托
            foreach (GetStatus getStatus in arrayOfDelegates)
            {
                try
                {
                    //获得一个组件的状态字符串，把它追加到报告中
                    report.AppendFormat("{0} {1} {1}", getStatus(), Environment.NewLine);
                }
                catch (InvalidOperationException e)
                {
                    //在状态报告中为该组件生成一条出错记录
                    Object component = getStatus.Target;
                    report.AppendFormat("Failed to get status from {0}{2}{1} Error:{3}{0}{0}", Environment.NewLine, ((component == null) ? "" : component.GetType() + "."), getStatus.Method.Name, e.Message);
                }
            }
            return report.ToString();
        }
    }
    //定义一个Light(灯)组件
    internal sealed class Light
    {
        //该方法返回灯的状态
        public string SwitchPostion()
        {
            return "The light is off";
        }
    }
    //定义一个Fan(风扇)组件
    internal sealed class Fan
    {
        //该方法返回风扇的状态
        public string Speed()
        {
            throw new InvalidOperationException("The fan broken due to overheating");
        }
    }
    //定义一个Speaker扬声器
    internal sealed class Speaker
    {
        //该方法返回扬声器的状态
        public string Volume()
        {
            return "The Volume is loud";
        }
    }
    internal sealed class AClass
    {
        public static void CallBackWithOutNewingDelegateObject()
        {
            //ThreadPool.QueueUserWorkItem(SomeAsyncTask1(5));
            //ThreadPool.QueueUserWorkItem(SomeAsyncTask,5);
            ThreadPool.QueueUserWorkItem(SomeAsyncTask1(), 5);
        }
        private static WaitCallback SomeAsyncTask1()
        {
            //return new WaitCallback(p => {
            //    SomeAsyncTask(o);
            //});
            return new WaitCallback(SomeAsyncTask);
        }
        public static void SomeAsyncTask(Object o)
        {
            Console.WriteLine(o);
        }
    }
    ////编译之后的AClass代码
    //internal sealed class AClassAfterCompile { 
    //    //创建该私有字段的目的是缓存对象
    //    //有点：每次调用时，CallBackWithOutNewingDelegateObject不会创建一个对象
    //    //缺点：缓存的对象永远不会被垃圾回收
    //    private static WaitCallback <>9__CachedAnonymousMethodDelegate1;
    //    public static void CallBackWithOutNewingDelegateObject()
    //    {
    //        if(<>9__CachedAnonymousMethodDelegate1==null)
    //        {
    //            //第一次调用，创建委托，并缓存它
    //            <>9__CachedAnonymousMethodDelegate1 = 
    //                new WaitCallback(<CallBackWithOutNewingDelegateObject>b__0);
    //        }
    //        ThreadPool.QueueUserWorkItem(<>9__CachedAnonymousMethodDelegate1,5);
    //    }
    //    [CompilerGenerated]
    //    private static void <CallBackWithOutNewingDelegateObject>b__0(Object obj)
    //    {
    //        Console.WriteLine(obj);
    //    }
    //}
    internal sealed class BClass
    {
        public static void UsingLocalVariablesIntheCallbackCode(Int32 numToDo)
        {
            //一些局部变量
            Int32[] squares = new Int32[numToDo];
            AutoResetEvent done = new AutoResetEvent(false);
            //在其他线程上执行一系列任务
            for (Int32 n = 0; n < squares.Length; n++)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    Int32 num = (Int32)obj;
                    //该任务通常更耗时
                    squares[num] = num * num;
                    //如果是最后一个任务，就让主线程继续运行
                    if (Interlocked.Decrement(ref numToDo) == 0)
                    {
                        done.Set();
                    }
                }, n);
            }
            //等待其他所有线程结束运行
            done.WaitOne();
            //显示结果
            for (Int32 n = 0; n < squares.Length; n++)
            {
                Console.WriteLine("Index {0},Square={1}", n, squares[n]);
            }
        }
    }

}
