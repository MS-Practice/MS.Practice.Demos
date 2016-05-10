using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics.Contracts;

namespace ExceptionProject
{
    class Program
    {
        public delegate void MyEventHandler(object sender, EventArgs e);

        public event EventHandler MyEvent;
        public static event EventHandler MyStaticEvent;
        static void Main(string[] args)
        {
            //TestException();
            Program pa = new Program();
            //var myClass = new MyClass();
            //var ev = new DelegateEvent<EventHandler>(handler => myClass.MyEvent += handler, handler => myClass.MyEvent -= handler);
            //EventHandler eh = new EventHandler(FaxMsg);
            //ev._add(eh);
            //myClass.OnMailMsg(myClass);

            var sde = EventFactory.Create(() => pa.MyEvent);
            //ev._add(
            //myClass.OnMailMsg(myClass);
            
            ////实例+事件名
            //var de = new DelegateEvent<EventHandler>(myClass, "MyEvent");

            ////类型+事件名
            //de = new DelegateEvent<EventHandler>(typeof(MyClass), "MyEvent");
        }

        private static void FaxMsg(object sender, EventArgs e)
        {
            Console.WriteLine("MyClass Event Starts");
            Console.ReadLine();
        }

        private static void M() {
            //强迫将finally中的方法提前准备好
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                Console.WriteLine("In try");
            }
            finally { 
                //Type2的静态构造器在这里隐式调用
                Type2.M();
            }
        }
        internal class Type2 { 
            static Type2(){
                Console.WriteLine("Type2's static ctor called");
            }
            //应用System.Runtime.ConstrainedExecution命名中定义这个Attribute
            [ReliabilityContract(Consistency.WillNotCorruptState,System.Runtime.ConstrainedExecution.Cer.Success)]
            public static void M() { 
                
            }
        }
        private static void TestException() {
            try
            {
                throw new Exception<DiskFullExceptionArgs>(new DiskFullExceptionArgs(@"C:\"), "The disk is full");
            }
            catch (Exception<DiskFullExceptionArgs> e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //捕捉异常之后回滚到之前的状态（状态恢复）
        private static void SerializeObjectGraph(FileStream fs, IFormatter formatter, Object rootObj) {
            Int64 beforeSerialization = fs.Position;
            try
            {
                //尝试将文件图序列化到文件中
                formatter.Serialize(fs, rootObj);
            }
            catch(Exception ex) { 
                //任何事情出错，就将文件返回之前的状态
                fs.Position = beforeSerialization;
                //截断文件
                fs.SetLength(fs.Position);
                throw ex;
            }
        }
    }
    internal sealed class Exception<TExceptionArgs> : Exception, ISerializable
        where TExceptionArgs : ExceptionArgs
    {
        private const String c_args = "Args";   //用于（反）序列化
        private readonly TExceptionArgs m_args;

        public Exception(String message = null, Exception innerException = null)
            : this(null, message, innerException) { }
        public Exception(TExceptionArgs args, String message = null, Exception innerException = null) : base(message, innerException) {
            m_args = args;
        }
        //这个构造器用于反序列化：由于类是密封的，所以构造器是私有的。
        //如果这个类不是密封的，这个构造器应该是受保护的.
        [SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.SerializationFormatter)]
        private Exception(SerializationInfo info, StreamingContext context) : base(info, context) {
            m_args = (TExceptionArgs)info.GetValue(c_args, typeof(TExceptionArgs));
        }
        [SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(c_args, m_args);
            base.GetObjectData(info, context);
        }
        public override string Message
        {
            get
            {
                String baseMsg = base.Message;
                return (m_args == null) ? baseMsg : baseMsg + "(" + m_args.Message + ")";
            }
        }
        public override bool Equals(object obj)
        {
            Exception<TExceptionArgs> other = obj as Exception<TExceptionArgs>;
            if (obj == null) return false;
            return Object.Equals(m_args, other.m_args) && (base.Equals(obj));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    internal sealed class Item { /*...*/}
    internal sealed class ShoppingCart {
        private List<Item> m_cart = new List<Item>();
        private static Decimal m_totalCost = 0;

        public ShoppingCart() { 
        }
        public void AddItem(Item item) {
            AddItemHelper(m_cart, item, ref m_totalCost);
        }
        private static void AddItemHelper(List<Item> m_cart, Item newItem, ref Decimal totalCost)
        { 
            //前提条件
            Contract.Requires(newItem != null);
            Contract.Requires(Contract.ForAll(m_cart, s => s != newItem));
            //后置条件
            Contract.Ensures(Contract.Exists(m_cart, s => s == newItem));
            Contract.Ensures(totalCost >= Contract.OldValue(m_totalCost));
            Contract.EnsuresOnThrow<IOException>(totalCost == Contract.OldValue(m_totalCost));
            //做一些事情（可能抛出IOException）
            m_cart.Add(newItem);
            totalCost += 1.00M;
        }
        [ContractInvariantMethod]
        private void ObjectInvariant() {
            Contract.Invariant(m_totalCost >= 0);
        }
    }
}
