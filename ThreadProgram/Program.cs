using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadProgram
{
    class Program
    {
        private static int num = 0;

        #region 原子性操作
        static Int32 count;//计数值，用于线程同步 （注意原子性，所以本例中使用int32）
        static Int32 value;//实际运算值，用于显示计算结果
        #endregion
        static void Main(string[] args)
        {
            //初始化10个线程去访问num
            //for (int i = 0; i < 10; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(Run));
            //}
            //读线程
            Thread thread2 = new Thread(new ThreadStart(Read));
            thread2.Start();
            //写线程
            for (var i = 0; i < 10; i++) {
                Thread.Sleep(20);
                Thread thread = new Thread(new ThreadStart(Write));
                thread.Start();
            }
            Console.ReadKey();
        }

        static void Run(object state)
        {
            Console.WriteLine("当前数字：" + ++num);
        }

        private static void Write() {
            Int32 temp = 0;
            for (int i = 0; i < 10; i++)
            {
                temp += 1;
            }
            //真正写入
            value += temp;
            Thread.VolatileWrite(ref count, 1);
            //Console.WriteLine("Write方法:{1}", Thread.CurrentThread.ManagedThreadId, value);
        }

        private static void Read() {
            while (true) {
                //死循环监听写操作线执行完毕后立刻显示操作结果
                if (Thread.VolatileRead(ref count) > 0)
                {
                    Console.WriteLine("Read方法:{0}", Thread.VolatileRead(ref count));
                    Console.WriteLine("累计计数:{1}", Thread.CurrentThread.ManagedThreadId, value);
                    count = 0;
                }
            }
        }

        //private delegate int NewTaskDelegate(int ms);
        //private static int newTask(int ms) {
        //    Console.WriteLine("任务开始");
        //    Thread.Sleep(ms);
        //    Random random = new Random();
        //    int n = random.Next(10000);
        //    Console.WriteLine("任务完成");
        //    return n;
        //}
    }
    //原子操作同步原理
    //Thread类中的VolatileRead和VolatileWrite方法：
    //VolatileRead：当线程在共享区（临界区）来传递参数时，通过此方法原子性的读取第一个值
    //VolatileWrite：当线程在共享区（林姐区）传递参数时，通过此方法原子性的写入最后一个值
}
