using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO.Pipes;
using Wintellect.Threading.AsyncProgModel;

namespace ThreadPro
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    //Thread t = new Thread(Worker);
        //    //t.IsBackground = true;
        //    //t.Start();
        //    //Console.WriteLine("Returning from Main");
        //    //CallContext.LogicalSetData("Name", "Marson");
        //    //ThreadPool.QueueUserWorkItem(state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));
        //    ////现在阻止Main线程的执行上下文的流动
        //    //ExecutionContext.SuppressFlow();

        //    //ThreadPool.QueueUserWorkItem(state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));
        //    //ExecutionContext.RestoreFlow();

        //    //CancellationDemo.Go();
        //    //CancellationDemo.Register();

        //    TimerDemo.Go();
        //    //using (var task = new Task<Int32>(() => { Thread.Sleep(2000); return 5; }))
        //    //{
        //    //    task.Start();
        //    //    while (!task.IsCompleted)
        //    //    {
        //    //        //没有完成
        //    //        Console.WriteLine("......");
        //    //    }
        //    //    Console.WriteLine(task.Result);
        //    //    task.Wait();
        //    //    task.ContinueWith(tasks => "dsdsd" + tasks.Result, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
        //    //};
        //    Console.ReadLine();
        //}
        private static void Worker()
        {
            Thread.Sleep(5000);
            Console.WriteLine("Returning from Workers");
        }
    }
    internal static class CancellationDemo
    {
        public static void Go()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));
            Console.WriteLine("Press <Enter> to cancel the operation");
            Console.WriteLine();
            cts.Cancel();   //如果Count已经返回，Cancel没有任何效果
        }

        private static void Count(CancellationToken cancellationToken, Int32 countTo)
        {
            for (Int32 count = 0; count < countTo; count++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Count is Cancelled");
                    break;  //退出循环以停止操作
                }
                Console.WriteLine(count);
                Thread.Sleep(200);
            }
            Console.WriteLine("Count is done");
        }

        public static void Register()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("Canceled 1"));
            cts.Token.Register(() => Console.WriteLine("Canceled 2"));
            //cts.Cancel();
        }
        public static void CancelLinked()
        {
            var cts1 = new CancellationTokenSource();
            cts1.Token.Register(() => Console.WriteLine("cts1 canceled"));
            var cts2 = new CancellationTokenSource();
            cts2.Token.Register(() => Console.WriteLine("cts1 canceled"));
            var linkedcts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);
            linkedcts.Token.Register(() => Console.WriteLine("LinkedCts canceled"));
            //取消其中一个CancellationTokenSource对象
            cts2.Cancel();
            Console.WriteLine("cts1 canceled={0}, cts2 canceled={1}, ctsLinked={2}", cts1.IsCancellationRequested, cts2.IsCancellationRequested, linkedcts.IsCancellationRequested);
            //显示那个被取消了
        }
    }

    internal class AsyncTask
    {
        public static void Go()
        {

        }
        private static void AsyncTaskUseAttachedToParent()
        {
            Task<Int32[]> parent = new Task<Int32[]>(() =>
            {
                var results = new Int32[3];
                new Task(() => results[0] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                return results;
            });
            var cwt = parent.ContinueWith(parentTask => Array.ForEach(parentTask.Result, Console.WriteLine));
            parent.Start();
        }

        private static Int32 Sum(Int32 n)
        {
            Int32 sum = 0;
            for (; n > 0; n--) checked { sum += n; }
            return sum;
        }

        private static Int64 DirectoryBytes(String path, String searchPattern, SearchOption searchOption)
        {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            Int64 masterTotal = 0;
            ParallelLoopResult result = Parallel.ForEach<String, Int64>(files, () =>
            {
                //LocalInit:每个任务开始之前调用一次
                return 0;
            },
            (file, loopState, index, taskLocalTotal) =>
            {
                //Body:每个工作项调用一次
                Int64 fileLength = 0;
                FileStream fs = null;
                try
                {
                    fs = File.OpenRead(file);
                    fileLength = fs.Length;
                }
                catch (IOException) { }
                finally
                {
                    if (fs != null) fs.Dispose();
                }
                return taskLocalTotal + fileLength;
            }, taskLocalTotal =>
            {
                //localFinally:每个任务完成时调用一次
                Interlocked.Add(ref masterTotal, taskLocalTotal);
            });
            return masterTotal;
        }

        private static void ObsoleteMethod(Assembly assembly)
        {
            var query =
                from type in assembly.GetExportedTypes().AsParallel()
                from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                let obsoleteAttrType = typeof(ObsoleteAttribute)
                where Attribute.IsDefined(method, obsoleteAttrType)
                orderby type.FullName
                let obsoleteAttrObj = (ObsoleteAttribute)Attribute.GetCustomAttribute(method, obsoleteAttrType)
                select String.Format("Type={0}\nMethod={1}\nMessage={2}\n", type.FullName, method.ToString(), obsoleteAttrObj.Message);
            //显示结果
            foreach (var result in query)
            {
                Console.WriteLine(result);
            }
        }
    }

    internal sealed class TimerDemo
    {
        private static Timer s_timer;
        public static void Go()
        {
            Console.WriteLine("Main thread:starting a timer");
            using (s_timer = new Timer(ComputeBoundOp, 5, 0, Timeout.Infinite))
            {
                Console.WriteLine("Main thread:Doing other work here...");
                Thread.Sleep(10000);
            }
        }

        private static void ComputeBoundOp(Object state)
        {
            //这个方法由一个线程池线程执行
            Console.WriteLine("In ComputeBoundOp:state={0}", state);
            Thread.Sleep(1000);
            //timer两秒后在调用这个方法
            s_timer.Change(2000, Timeout.Infinite);
            //这个方法返回时，线程回归池中，等下一个工作项
        }
    }

    internal static class FalseSharing
    {
        [StructLayout(LayoutKind.Explicit)]
        private class Data
        {
            //这两个字段是相邻的，并极有可能在同一个缓存行（cache line）
            [FieldOffset(0)]
            public Int32 field1;
            [FieldOffset(64)]
            public Int32 field2;
        }
        private const Int32 interations = 100000000;
        private static Int32 s_operations = 2;
        private static Int64 s_startTime;
#if fase
        public static void Main()
        {
            //开始分配对象 记录时间
            Data data = new Data();
            s_startTime = Stopwatch.GetTimestamp();
            //让两个线程访问在对象中的字段
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 0));
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 1));
            Console.ReadLine();
        } 
#endif

        private static void AccessData(Data data, Int32 field)
        {
            //这里的线程各自访问他们在Data对象中的字段
            for (Int32 x = 0; x < interations; x++)
            {
                if (field == 0) data.field1++; else data.field2++;
            }
            //不管那个线程结束，都显示它们所花的时间
            if (Interlocked.Decrement(ref s_operations) == 0)
                Console.WriteLine("Access time: {0:N0}", (Stopwatch.GetTimestamp() - s_startTime) / (Stopwatch.Frequency / 1000));
        }
    }

    internal static class ApmExceptionHandling
    {
#if false
        public static void Main()
        {
            //尝试访问一个无效的网址
            //WebRequest webRequest = WebRequest.Create("http://0.0.0.0/");
            //webRequest.BeginGetResponse(ProcessWebResponse, webRequest);
            Console.WriteLine("...");
            ImplementedViaRawApm();
            Console.ReadLine();
        } 
#endif
        private static void ImplementedViaRawApm()
        {
            //for (Int32 n = 0; n < Environment.ProcessorCount; n++)
            //    new PipeServer();

            // Now make a 100 client requests against the server
            for (Int32 n = 0; n < 100; n++)
                new PipeClient("localhost", "Request #" + n);
        }
        private static void ProcessWebResponse(IAsyncResult ir)
        {
            WebRequest webRequest = (WebRequest)ir.AsyncState;
            WebResponse webResponse = null;
            try
            {
                webResponse = webRequest.EndGetResponse(ir);
                Console.WriteLine("Content length: " + webResponse.ContentLength);
            }
            catch (WebException we)
            {
                Console.WriteLine(we.GetType() + ": " + we.Message);
            }
            finally
            {
                if (webResponse != null) webResponse.Close();
            }
        }
    }

    internal static class ThreadIO
    {
        //public static BackgroundProcessingDisposer BeginBackgroundProcessing { 

        //}
    }
}

public sealed class PipeClient
{
    // Each client object performs asynchronous operations on this pipe
    private readonly NamedPipeClientStream m_pipe;

    public PipeClient(String serverName, String message)
    {
        m_pipe = new NamedPipeClientStream(serverName, "Echo",
           PipeDirection.InOut, PipeOptions.Asynchronous | PipeOptions.WriteThrough);
        m_pipe.Connect(); // Must Connect before setting ReadMode
        m_pipe.ReadMode = PipeTransmissionMode.Message;

        // Asynchronously send data to the server
        Byte[] output = Encoding.UTF8.GetBytes(message);
        m_pipe.BeginWrite(output, 0, output.Length, WriteDone, null);
    }

    private void WriteDone(IAsyncResult result)
    {
        // The data was sent to the server
        m_pipe.EndWrite(result);

        // Asynchronously read the server's response
        Byte[] data = new Byte[1000];
        m_pipe.BeginRead(data, 0, data.Length, GotResponse, data);
    }

    private void GotResponse(IAsyncResult result)
    {
        // The server responded, display the response and close out connection
        Int32 bytesRead = m_pipe.EndRead(result);

        Byte[] data = (Byte[])result.AsyncState;
        Console.WriteLine("Server response: " + Encoding.UTF8.GetString(data, 0, bytesRead));
        m_pipe.Close();
    }

    private static IEnumerator<Int32> PipeServerAsyncEnumerator(AsyncEnumerator ae)
    {
        //using (var pipe = new NamedPipeServerStream())
        //{

        //}
        return null;
    }
}

internal class APMTurnToTaskMethodOfFromAsync
{
    private static Boolean m_stopWork = false;
    public static void Main()
    {
        Go();
    }

    private static void Go()
    {
        Console.WriteLine("Main: letting worker run for 5 secends");
        Thread t = new Thread(Worker);
        t.Start();
        Thread.Sleep(5000);
        m_stopWork = true;
        Console.WriteLine("Main: waiting for worker to stop");
        t.Join();
    }
    private static void MethodOfGeneralAsync()
    {
        WebRequest webRequest = WebRequest.Create("http://www.bing.com");
        webRequest.BeginGetResponse(result =>
        {
            WebResponse webResponse = null;
            try
            {
                webResponse = webRequest.EndGetResponse(result);
                Console.WriteLine("Content Length:" + webResponse.ContentType);
            }
            catch (WebException we)
            {
                Console.WriteLine("Content length:" + we.GetBaseException().Message);
            }
            finally
            {
                if (webResponse != null) webResponse.Close();
            }
        }, null);
    }
    private static void MethodOfTaskFromAsync()
    {
        WebRequest webRequest = WebRequest.Create("http://www.bing.com");
        Task.Factory.FromAsync<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse, null, TaskCreationOptions.None).ContinueWith(task =>
        {
            WebResponse webResponse = null;
            try
            {
                webResponse = task.Result;
                Console.WriteLine("Content Length:" + webResponse.ContentLength);
            }
            catch (AggregateException ae)
            {
                if (ae.GetBaseException() is WebException)
                    Console.WriteLine("Failed:" + ae.GetBaseException().Message);
                else throw;
            }
            finally
            {
                if (webResponse != null) webResponse.Close();
            }
        });
    }
    private static void Worker(Object o)
    {
        Int32 x = 0;
        while (!m_stopWork)
        {
            x++;
            Console.WriteLine("Worker:stopped when x={0}", x);
        }
    }
}
internal class ThreadSharingData
{
    private Int32 m_flag = 0;
    private Int32 m_value = 0;
    private void Thread1()
    {
        //在1写入flag之前 必须将5写入value
        m_value = 5;
        Thread.VolatileWrite(ref m_flag, 1);
    }
    private void Thread2()
    {
        //value 必须读取了flag之后读取
        if (Thread.VolatileRead(ref m_flag) == 1)
            Console.WriteLine(m_value);
    }
}

internal sealed class MultiWebRequests
{
    //这个辅助类用于协调所有异步操作
    private AsyncCordinator m_ac = new AsyncCordinator();
    //这是想要查询Web服务器集合
    private WebRequest[] m_requests = new WebRequest[]{
        WebRequest.Create("http://www.baidu.com"),
        WebRequest.Create("http://www.bing.com")
    };
    //创建响应数组，每个请求一个响应
    private WebResponse[] m_responses = new WebResponse[2];
    public MultiWebRequests(Int32 timeout = Timeout.Infinite)
    {
        //以异步方式一次性发起所有请求
        for (Int32 n = 0; n < m_requests.Length; n++)
        {
            m_ac.AboutToBegin(1);
            m_requests[n].BeginGetResponse(EndGetResponse, n);
        }
        //告诉辅助类 所有操作都已经开启，并在所有操作完成
        //调用了Cancel或者发生了超时的时候调用AllDone
        m_ac.AllBegun(AllDone, timeout);
    }
    //调用这个方法指出结果已经无关紧要了
    public void Cancel()
    {
        m_ac.Cancel();
    }
    private void EndGetResponse(IAsyncResult result)
    {
        //获取与请求对应的索引
        Int32 n = (Int32)result.AsyncState;
        //将响应保存在和请求相同的索引中
        m_responses[n] = m_requests[n].EndGetResponse(result);
        //告诉辅助类Web服务器已经响应
        m_ac.JustEnded();
    }
    //这个方法在所有Web服务器都响应，调用了Cancel
    //或者发生超时的时候调用
    private void AllDone(CordinationStatus status)
    {
        switch (status)
        {
            case CordinationStatus.Cancel:
                Console.WriteLine("The operation was canceled"); break;
            case CordinationStatus.Timeout:
                Console.WriteLine("The operation was time-out"); break;
            case CordinationStatus.AllDone:
                Console.WriteLine("Here are the results form all the web servers");
                for (Int32 i = 0; i < m_requests.Length; i++)
                {
                    Console.WriteLine("{0} returned {1} bytes.", m_responses[i].ResponseUri, m_responses[i].ContentLength);
                }
                break;
            default:
                break;
        }
    }
}
internal sealed class AsyncCordinator
{
    private Int32 m_opCount = 1;        //AllBegun内部调用JustEnded来调用它
    private Int32 m_statusReported = 0;  //0 is false 1 is true
    private Action<CordinationStatus> m_callback;
    private Timer m_timer;
    //必须在调用BeginXXX方法之前调用这个方法
    public void AboutToBegin(Int32 opsToAdd = 1)
    {
        Interlocked.Add(ref m_opCount, opsToAdd);
    }
    //必须调用一个EndXXX方法之后调用这个方法
    public void JustEnded()
    {
        if (Interlocked.Decrement(ref m_opCount) == 0)
            ReportsStatus(CordinationStatus.AllDone);
    }
    //必须调用了所有的BeginXXX方法之后调用这个 方法
    public void AllBegun(Action<CordinationStatus> callback, Int32 timeout = Timeout.Infinite)
    {
        m_callback = callback;
        if (timeout != Timeout.Infinite)
            m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
        JustEnded();
    }

    private void TimeExpired(Object o)
    {
        ReportsStatus(CordinationStatus.Timeout);
    }
    public void Cancel() { ReportsStatus(CordinationStatus.Cancel); }
    private void ReportsStatus(CordinationStatus status)
    {
        //如果状态从未报过，就报告它；否则忽略它
        if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
            m_callback(status);
    }
}
internal enum CordinationStatus { AllDone, Timeout, Cancel };

internal sealed struct SimpleSpinLock
{
    private Int32 m_ResuorceInUse;  //0 = false(默认) 1=true
    public void Enter()
    {
        //将资源设置为“正在使用”（1），如果这个线程是把它从“自由使用”（0）
        //编程“正在使用”就返回，以便执行Enter之后的代码
        while (Interlocked.Exchange(ref m_ResuorceInUse, 1) != 0)
        {
            /*额外的逻辑*/
        }
    }
    public void Leave()
    {
        //将资源设置为“自由使用”（0）
        Thread.VolatileWrite(ref m_ResuorceInUse, 0);
    }
    public static Int32 Maximum(ref Int32 target, Int32 value)
    {
        Int32 currentVal = target, startVal, desiredVal;
        //不要在这个循环迭代访问目标target，除非是另一个线程得到它，所以要改变它
        do
        {
            //记录在循环中迭代的起始值（startVal）
            startVal = currentVal;
            //基于startVal和value计算期望值desiredVal
            desiredVal = Math.Max(startVal, value);
            //注意：线程这里可能被抢占（preempted）
            //if(target==statrVal) target =desireVal
            //在可能的改变之前的值被返回
            currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
        } while (startVal != currentVal);
        //在这个线程尝试设置它之前返回最大值
        return desiredVal;
    }

    delegate Int32 Morpher<TResult, TArgument>(Int32 startValue, TArgument argument, out TResult morphResult);

    static TResult Morph<TResult, TArgument>(ref Int32 target, TArgument argument, Morpher<TResult, TArgument> morpher)
    {
        TResult morphResult;
        Int32 currentVal = target, startVal, desireVal;
        do
        {
            startVal = currentVal;
            desireVal = morpher(startVal, argument, out morphResult);
            currentVal = Interlocked.CompareExchange(ref target, desireVal, startVal);
        } while (startVal != currentVal);
        return morphResult;
    }

}
internal sealed class SomeResource
{
    private SimpleSpinLock m_sl = new SimpleSpinLock();
    public void AccessResource()
    {
        m_sl.Enter();
        //一次只有一个线程才能进入这里访问资源
        //等同于Monitor.Enter()?????? 有待验证
        m_sl.Leave();
    }
}

internal sealed class SomeClass : IDisposable
{
    private readonly Mutex m_lock = new Mutex();
    public void Method1()
    {
        m_lock.WaitOne();
        //随便做什么事
        Method2();
        m_lock.ReleaseMutex();
    }
    public void Method2()
    {
        m_lock.WaitOne();
        //随便做什么
        m_lock.ReleaseMutex();
    }
    public void Dispose() { m_lock.Dispose(); }
}

internal sealed class RecursiveAutoResetEvent : IDisposable
{
    private AutoResetEvent m_lock = new AutoResetEvent(true);
    private Int32 m_owningThreadId = 0;
    private Int32 m_recurisonCount = 0;
    public void Enter()
    {
        //获取调用线程的唯一Int32ID
        Int32 currentThreadId = Thread.CurrentThread.ManagedThreadId;
        //如果调用线程拥有锁，就递增递归计数
        if (m_owningThreadId == currentThreadId)
        {
            m_recurisonCount++;
            return;
        }
        //调用线程不拥有它，则等待
        m_lock.WaitOne();
        //调用线程现在拥有了索，初始化拥有线程的ID和计数
        m_owningThreadId = currentThreadId;
        m_recurisonCount--;
    }
    public void Leave()
    {
        //如果调用线程不拥有锁，就出错了
        if (m_owningThreadId != Thread.CurrentThread.ManagedThreadId)
        {
            throw new InvalidOperationException();
        }
        //从递归计数中减1
        if (--m_recurisonCount == 0)
        {
            //如果递归计数为0，表明没有线程拥有锁
            m_owningThreadId = 0;
            m_lock.Set();
        }
    }
    public void Dispose() { m_lock.Dispose(); }
}

internal static class RegisteredWaitHandleDemo
{
    public static void Main()
    {
        //构造一个最初的AutoRestEvent(最初为false)
        AutoResetEvent are = new AutoResetEvent(false);
        //告诉线程池在AutoResetEvent等待
        RegisteredWaitHandle rwh = ThreadPool.RegisterWaitForSingleObject(are, EventOperation, null, 5000, false);//为true，都调用EventOperation
        //开始我们的循环
        Char operation = (Char)0;
        while (operation != 'Q')
        {
            Console.WriteLine("S=Signal,Q=Quit?");
            operation = Char.ToUpper(Console.ReadKey(true).KeyChar);
            if (operation == 'S')
            {
                are.Set();  //用户想设置事件
            }
        }
        //告诉线程池停止所有事件上的等待
        rwh.Unregister(null);
    }

    //任何时候事件为true，或者自从上一次回调/超时以来过了5秒，就调用这个方法
    private static void EventOperation(Object state, Boolean timeOut)
    {
        Console.WriteLine(timeOut ? "Timeout" : "Event became true");
    }
}

internal sealed class SimpleHybridLock : IDisposable
{
    //由int32基元用户模式构造（InterLocked方法）使用
    private Int32 m_waiters = 0;
    //AutoResetEvent是基元内核模式构造
    private AutoResetEvent m_waiterLock = new AutoResetEvent(false);

    public void Enter()
    {
        //指出这个线程想要获得锁
        if (Interlocked.Increment(ref m_waiters) == 1)
        {
            return; //锁可自由使用，无竞争，直接返回
        }
        //另一个线程正在等待。这代表一个竞争，因此阻塞这个线程
        m_waiterLock.WaitOne();
        //WaitOne返回后，这个线程现在拥有了锁
    }
    public void Leave()
    {
        //这个线程准备释放锁
        if (Interlocked.Decrement(ref m_waiters) == 0)
        {
            return; // 没有其他线程阻塞，直接返回
        }
        //有其他线程争用，存在阻塞状态，唤醒其中的一个
        m_waiterLock.Set();
    }
    public void Dispose() { m_waiterLock.Dispose(); }
}

internal sealed class AnotherHybridLock : IDisposable
{
    //Int32 由基元用户模式构造（InterLocked方法）使用
    private Int32 m_waiters = 0;
    //AutoResetEvent由基元内核模式构造的使用
    private AutoResetEvent m_waiterLock = new AutoResetEvent(false);
    //这个字段控制用户模式的自旋 希望提升性能
    private Int32 m_spincount = 4000;    //任意的选择一个计数
    //这些字段只能那个线程拥有锁，以及拥有了多少次
    private Int32 m_owningThreadId = 0, m_recursion = 0;
    public void Enter()
    {
        //如果调用线程已经拥有锁，递增递归计数
        Int32 threadId = Thread.CurrentThread.ManagedThreadId;
        if (threadId == m_owningThreadId)
        {
            m_recursion++; return;
        }
        //调用线程不拥有锁，尝试获取它
        SpinWait spinwait = new SpinWait();
        for (Int32 spinCount = 0; spinCount < m_spincount; spinCount++)
        {
            //如果锁自由使用了，这个线程就获取它，设置一些状态并返回
            if (Interlocked.CompareExchange(ref m_waiters, 1, 0) == 0)
            {
                goto GotLock;
            }
            spinwait.SpinOnce();
        }
        //自旋结束，锁任然没有获得，在获取一次
        if (Interlocked.Increment(ref m_waiters) > 1)   //？
        {
            //其他线程被阻塞，这个线程也必须阻塞
            m_waiterLock.WaitOne();
            //等这个线程醒来时，它拥有锁；设置一些状态返回
        }
    GotLock:
        //一个线程获得锁时，我们记录它的ID，并指出线程拥有一次索
        m_owningThreadId = threadId; m_recursion = 1;
    }
    public void Leave() { 
        //如果调用线程不拥有一个锁，表明是个bug
        Int32 threadId = Thread.CurrentThread.ManagedThreadId;
        if (threadId!=m_owningThreadId)
        {
            throw new SynchronizationLockException("Lock not owned by calling thread");
        }
        //递减递归计数，如果这个线程任然还有锁，那么直接返回
        if (--m_spincount > 0) return;
        m_owningThreadId = 0;   //现在线程没有拥有锁
        if (Interlocked.Decrement(ref m_waiters)==0)
        {
            return;
        }
        //有其他线程被阻塞，唤醒其中一个
        m_waiterLock.Set();
    }
    public void Dispose()
    {
        m_waiterLock.Dispose();
    }
}
