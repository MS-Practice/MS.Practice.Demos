using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Runtime.Remoting;

namespace GColletionProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建一个Timer对象，没隔2000毫秒就执行一次TimerCallBack方法
            //System.Threading.Timer t = new System.Threading.Timer(TimerCallBack, null, 0, 2000);
            //Console.ReadLine();
            //t.Dispose();

            //Object o = new Object().GCWatch("My Object Create at " + DateTime.Now);
            //GC.Collect();
            //GC.KeepAlive(o);    //表示o对象还存活
            //o = null;
            //GC.Collect();

            Marshalling();
        }
        private static void TimerCallBack(Object o)
        {
            Console.WriteLine("In TimerCallBack:" + DateTime.Now);
            GC.Collect();
        }
        public static Object s_ObjHolder;
        private static void Marshalling()
        {
            //获取对AppDomaind的一个引用（“调用线程”当前正在执行AppDomain中执行）
            AppDomain adCallingThreadDomain = Thread.GetDomain();
            //每个AppDomain都有一个名称 方便调试
            //获取该AppDomain的友好名称
            String callingDomainName = adCallingThreadDomain.FriendlyName;
            Console.WriteLine("Default AppDomain's friendly name={0}", callingDomainName);
            //获取&显示我们的AppDomain的main方法的程序集
            String exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("Main assembly={0}", exeAssembly);
            //定一个局部变量来引用AppDomain
            AppDomain ad2 = null;
            //DEMO1: 使用Marshal-by-reference进行跨AppDomain通信
            Console.WriteLine("{0} Demo #1", Environment.NewLine);
            //新建一个AppDomain(安全性和配置匹配于当前AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2", null, null);
            MarshalByRefType mbrt = null;
            //将我们的程序集加载到新AppDomain中，构造一个对象，把它封送到AppDomain中（实际是得到一个代理的引用）
            mbrt = (MarshalByRefType)ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");
            Console.WriteLine("Type={0}", mbrt.GetType());
            //证明得到的是一个代理对象的引用
            Console.WriteLine("Is Proxy={0}", RemotingServices.IsTransparentProxy(mbrt));
            mbrt.SomeMethod();
            //卸载新的AppDomain
            AppDomain.Unload(ad2);
            try
            {
                // We're calling a method on an object; no exception is thrown
                Console.WriteLine("Returned object created " + mbrt.ToString());
                Console.WriteLine("Successful call.");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Failed call.");
            }

            Console.WriteLine("{0}Demo #2", Environment.NewLine);

            // Create new AppDomain (security & configuration match current AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2", null, null);

            // Load our assembly into the new AppDomain, construct an object, marshal 
            // it back to our AD (we really get a reference to a proxy)
            mbrt = (MarshalByRefType)
               ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");

            // The object's method returns a COPY of the returned object; 
            // the object is marshaled by value (not be reference).
            MarshalByValType mbvt = mbrt.MethodWithReturn();

            // Prove that we did NOT get a reference to a proxy object
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbvt));

            // This looks like we're calling a method on MarshalByValType and we are.
            Console.WriteLine("Returned object created " + mbvt.ToString());

            // Unload the new AppDomain
            AppDomain.Unload(ad2);
            try
            {
                // We're calling a method on an object; no exception is thrown
                Console.WriteLine("Returned object created " + mbvt.ToString());
                Console.WriteLine("Successful call.");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Failed call.");
            }

            // DEMO 3: Cross-AppDomain Communication using non-marshalable type
            Console.WriteLine("{0}Demo #3", Environment.NewLine);

            // Create new AppDomain (security & configuration match current AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2", null, null);

            // Load our assembly into the new AppDomain, construct an object, marshal 
            // it back to our AD (we really get a reference to a proxy)
            mbrt = (MarshalByRefType)
               ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");

            // The object's method returns an non-marshalable object; exception
            NonMarshalableType nmt = mbrt.MethodArgAndReturn(callingDomainName);
            // We won't get here...

        }
    }
    public sealed class NonMarshalableType : Object
    {
        public NonMarshalableType()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }
    }
    [Serializable]
    public sealed class MarshalByValType : Object
    {
        private DateTime m_creationTime = DateTime.Now; // NOTE: DateTime is [Serializable]

        public MarshalByValType()
        {
            Console.WriteLine("{0} ctor running in {1}, Created on {2:D}",
               this.GetType().ToString(),
               Thread.GetDomain().FriendlyName,
               m_creationTime);
        }

        public override String ToString()
        {
            return m_creationTime.ToLongDateString();
        }
    }
    internal sealed class MarshalByRefType : MarshalByRefObject
    {
        public MarshalByRefType()
        {
            Console.WriteLine("{0} ctor running in {1}",
               this.GetType().ToString(), Thread.GetDomain().FriendlyName);
        }
        public void SomeMethod()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }

        internal MarshalByValType MethodWithReturn()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
            MarshalByValType t = new MarshalByValType();
            return t;
        }

        internal NonMarshalableType MethodArgAndReturn(String callingDomainName)
        {
            Console.WriteLine("Calling from '{0}' to '{1}.'",callingDomainName,Thread.GetDomain().FriendlyName);
            NonMarshalableType t = new NonMarshalableType();
            return t;
        }
    }

    internal class SomeType
    {
        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint = "CreateEvent")]
        private static extern IntPtr CreateEventBad(IntPtr pSecurityAttributes, Boolean manualReset, Boolean initialState, String name);
        //有可能发生线程异常(ThreadAbortException)
        public static void SomeMethod()
        {
            IntPtr hanle = CreateEventBad(IntPtr.Zero, false, false, null);
        }
    }
    //创建一个轻量级的WeakReference泛型类
    internal struct WeakReference<T> : IDisposable where T : class
    {
        private WeakReference m_weakReference;
        public WeakReference(T target) { m_weakReference = new WeakReference(target); }
        public T Target { get { return (T)m_weakReference.Target; } }
        public void Dispose() { m_weakReference = null; }
    }
    //创建一个弱委托   一个对象向另一个对象的事件登记一个回调委托，并且不想让这个对象存活
    internal sealed class DoNotJustForTheEvent
    {
        //事件
        public void Clicked(Object sender, EventArgs e)
        {
            MessageBox.Show("Test got notified of button click");
        }
    }
    internal static class WeakEventHanlerDemo
    {
        public static void Go()
        {
            var form = new Form()
            {
                Text = "Weak Delegate Test",
                FormBorderStyle = FormBorderStyle.FixedSingle
            };

            var btnTest = new Button()
            {
                Text = "Click me",
                Width = form.Width / 2
            };

            var btnGC = new Button()
            {
                Text = "Force GC",
                Left = btnTest.Width,
                Width = btnTest.Width
            };
            //WeakEventHanler将一个EventHandler委托转化成一个自动的弱版本
            btnTest.Click += new WeakEventHandler(new DoNotJustForTheEvent().Clicked)
            {
                RemoveDelegateCode = eh => btnTest.Click -= eh
            };
            form.Controls.Add(btnTest);
            form.Controls.Add(btnGC);
            form.ClientSize = new Size(btnTest.Width * 2, btnTest.Height);
            form.ShowDialog();
        }
    }

    public abstract class WeakDelegate<TDelegate> where TDelegate : class /*多播委托*/
    {
        private WeakReference<TDelegate> m_weakDelegate;
        private Action<TDelegate> m_removeDelegateCode;

        public WeakDelegate(TDelegate @delegate)
        {
            var md = (MulticastDelegate)(Object)@delegate;
            if (md.Target == null)
                throw new ArgumentException("There is no reason to make a WeakDelegate to a static method.");
            //保存对委托的一个弱引用
            m_weakDelegate = new WeakReference<TDelegate>(@delegate);
        }
        public Action<TDelegate> RemoveDelegateCode
        {
            set
            {
                //保存一个委托，它引用的代码知道在非弱委托对象被垃圾回收时删除WeakDelegate对象
                m_removeDelegateCode = value;
            }
        }
        protected TDelegate GetRealDelegate()
        {
            //如果真的委托未被垃圾回收，就返回它
            TDelegate realDelegate = m_weakDelegate.Target;
            if (realDelegate != null) return realDelegate;
            //如果被回收了，我们就不需要它的弱引用
            //弱引用可以被垃圾回收
            m_weakDelegate.Dispose();
            //从委托链中删除委托（需要用户告诉我们怎么做）
            if (m_removeDelegateCode != null)
            {
                m_removeDelegateCode(GetDelegate());
                m_removeDelegateCode = null;    //让负责WeakDelegate对象的委托被垃圾GC回收
            }
            return null;    //  真委托被回收，不能在调用
        }
        //所有的派生类都必须返回一个私有方法的委托，该方法与TDelegate类型匹配
        public abstract TDelegate GetDelegate();
        //隐式转换操作符 用于将一个WeakDelegate转化为真正的委托
        public static implicit operator TDelegate(WeakDelegate<TDelegate> @delegate)
        {
            return @delegate.GetDelegate();
        }
    }

    public sealed class WeakEventHandler : WeakDelegate<EventHandler>
    {
        public WeakEventHandler(EventHandler @delegate) : base(@delegate) { }

        public override EventHandler GetDelegate()
        {
            return CallBack;
        }
        private void CallBack(Object sender, EventArgs e)
        {
            var eh = base.GetRealDelegate();
            if (eh != null) eh(sender, e);
        }

    }

    internal static class GCWatcher
    {
        //注意 用String会存在字符串驻留和MarshalByRefObject代理对象 所以要小心
        private readonly static ConditionalWeakTable<Object, NotifyWhenGCd<String>> s_cwt = new ConditionalWeakTable<object, NotifyWhenGCd<string>>();

        private sealed class NotifyWhenGCd<T>
        {
            private readonly T m_value;
            internal NotifyWhenGCd(T value) { m_value = value; }
            public override string ToString()
            {
                return m_value.ToString();
            }
            ~NotifyWhenGCd()
            {
                Console.WriteLine("GC'd:" + m_value);
            }
        }
        public static T GCWatch<T>(this T @object, String tag) where T : class
        {
            //s_cwt
            s_cwt.Add(@object, new NotifyWhenGCd<String>(tag));
            return @object;
        }
    }
    internal class SomeTypeGC
    {
        ~SomeTypeGC()
        {
            Program.s_ObjHolder = this;
            GC.ReRegisterForFinalize(this); //前者调用了Program 使之复活  这个函数是将指定对象this放到终结列表末尾 已让GC进行垃圾回收 从而达到了防止对象复活
        }
    }

    internal sealed class GCNotification
    {
        private static Action<Int32> s_gcDone = null;
        public static event Action<Int32> GCDone
        {
            add
            {
                //如果之前没有登记的委托，则就开报告
                if (s_gcDone == null) { new GenObject(0); new GenObject(2); }
                s_gcDone += value;
            }
            remove { s_gcDone -= value; }
        }
        internal sealed class GenObject
        {
            private Int32 m_gerneration;
            public GenObject(Int32 generation) { m_gerneration = generation; }
            ~GenObject()
            {
                //如果这个对象在我们期望的代（或更高的代）中
                //就通知委托一次GC刚刚完成
                if (GC.GetGeneration(this) >= m_gerneration)
                {
                    Action<Int32> temp = Interlocked.CompareExchange(ref s_gcDone, null, null);
                    if (temp != null) temp(m_gerneration);
                }
                //如果至少还有一个已经登记的委托，而且appDomain并非正在卸载
                //而且进程并非正在关闭，则继续报告
                if ((s_gcDone != null) && !AppDomain.CurrentDomain.IsFinalizingForUnload() && !Environment.HasShutdownStarted)
                {                    //对于第0带 创建一个对象，对于第二代，复活对象
                    //使第二代下次回收时，GC会再次调用Finalize(this)
                    if (m_gerneration == 0) new GenObject(0);
                    else GC.ReRegisterForFinalize(this);
                }
                else { /*放过对象，让其被回收*/}
            }
        }
    }
}
