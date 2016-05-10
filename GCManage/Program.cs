using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GCManage
{
    class Program
    {
        static void Main(string[] args)
        {
            GCClassA a = new GCClassA(new GCClassB(new GCClassC()));
            //手动强制执行GC回收
            GC.Collect(0);
            GC.WaitForPendingFinalizers();

            //多线程 利用线程池对线程的封装
            Program th = new Program();
            ThreadPool.QueueUserWorkItem(th.MyProcOne, "线程1");
            Thread.Sleep(1000);
            ThreadPool.QueueUserWorkItem(th.MyProcTwo, "线程2");
        }
        public void MyProcOne(object stateinfo)
        {
            Console.WriteLine(stateinfo.ToString());
            Console.WriteLine("起床了！");
        }
        public void MyProcTwo(object stateinfo)
        {
            Console.WriteLine(stateinfo.ToString());
            Console.WriteLine("刷牙洗脸！");
        }
    }
    //利用析构函数处理垃圾回收
    class GCClassA
    {
        private GCClassB objB;
        public GCClassA(GCClassB b) {
            objB = b;
        }
        ~GCClassA()
        {
            Console.WriteLine("类GCClassA被回收");
        }
    }
    class GCClassB
    {
        private GCClassC objC;
        public GCClassB(GCClassC c) {
            objC = c;
        }
        ~GCClassB()
        {
            Console.WriteLine("类GCClassB被回收");
        }
    }
    class GCClassC
    {
        ~GCClassC()
        {
            Console.WriteLine("类GCClassC被回收");
        }
    }
}
