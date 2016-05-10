using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.CSharp.RuntimeBinder;

namespace RefectionProject
{
    class Program
    {
        private const BindingFlags c_bf = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        static void Main(string[] args)
        {
            Show("Before doing anything");
            //为MSCorlib.dll中的所有方法构建MethodInfo对象缓存
            List<MethodBase> methodInfos = new List<MethodBase>();
            foreach (Type tp in typeof(Object).Assembly.GetExportedTypes())
            {
                //跳过任何泛型类型
                if (tp.IsGenericType) continue;
                MethodBase[] mb = tp.GetMethods(c_bf);
                methodInfos.AddRange(mb);
            }
            //显示当绑定所有方法之后，方法的个数和堆的大小
            Console.WriteLine("# of methods={0:###,###}", methodInfos.Count);
            Show("After Building cache of MethodInfo objects");
            //为所有的methodInfos对象构造一个RuntimeMethodHandler缓存
            List<RuntimeMethodHandle> methodsHandle = methodInfos.ConvertAll<RuntimeMethodHandle>(mb => mb.MethodHandle);
            Show("Holding MethodInfo an RuntimeMethodHandle cache");
            GC.KeepAlive(methodInfos);    //阻止缓存被过早的垃圾回收
            methodInfos = null; //现在允许垃圾回收
            Show("After Free MethodInfo objects");
            methodInfos = methodsHandle.ConvertAll<MethodBase>(rmh => MethodBase.GetMethodFromHandle(rmh));
            Show("Size of heap after re-creating MethodInfo objects");
            GC.KeepAlive(methodInfos);
            GC.KeepAlive(methodsHandle);
            methodsHandle = null;
            methodInfos = null;
            Show("After MethodInfos And RuntimeMethodHandles");

            Type t = typeof(SomeType);
            UseInvokeMemberBindAndInvokeTheMember(t);
            Console.WriteLine();
            BindToMemberThenInvokeTheMember(t);
            Console.WriteLine();
            BindToMemberCreateDelegateToMemberThenInvokeTheMember(t);
            Console.WriteLine();
            UseDynamicToBindToAndInvokeTheMember(t);
            Type t = typeof(System.Data.Constraint);
            Console.WriteLine(t.Assembly.FullName);
            String dataAssembly = t.Assembly.FullName;
            LoadAssemblyAndShowPublicTypes(dataAssembly);
            Console.ReadLine();
            //为解决CLR找不到依赖的DLL程序集，在应用程序初始化向AppDomain的ResolveAssembly事件等一个回溯方法
            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            {
                String resourceName = "AssemblyLoadingReflection." +
                    new AssemblyName(eventArgs.Name) + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };

            Func<Type, String> ClassNameAndBase = null;
            ClassNameAndBase = tp => "-" + tp.FullName + ((tp.BaseType != typeof(Object)) ? ClassNameAndBase(tp.BaseType) : String.Empty);
            //定义查询 在这个AppDomain加载的所有程序集中查找从Exception派生的所有public类型
            var exceptionTree =
                (from a in AppDomain.CurrentDomain.GetAssemblies()
                 from tp in a.GetExportedTypes()
                 where tp.IsClass && tp.IsPublic && typeof(Exception).IsAssignableFrom(tp)
                 let typeHierarchyTemp = ClassNameAndBase(tp).Split('-').Reverse().ToArray()
                 let typeHierarchy =
                    String.Join("-", typeHierarchyTemp, 0, typeHierarchyTemp.Length - 1)
                 orderby typeHierarchy
                 select typeHierarchy).ToArray();

            Type openType = typeof(Dictionary<,>);
            Type closeType = openType.MakeGenericType(typeof(String), typeof(Int32));
            //构造封闭类型的一个实例
            Object o = Activator.CreateInstance(closeType);
            //证实能正常工作
            Console.WriteLine(o.GetType());
            //查找宿主EXE文件的路径
            String AddInDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //假定加载项程序集与宿主EXE在同一个文件夹
            String[] AddInAssemblies = Directory.GetFiles(AddInDir, "*.dll");
            //创建所有可用的加载项Type的一个集合
            List<Type> AddInTypes = new List<Type>();
            //加载加载项程序集，查看那些类型可用宿主使用
            foreach (String file in AddInAssemblies)
            {
                Assembly AddInAssembly = Assembly.LoadFrom(file);
                //检查每个公开导出的类型
                foreach (Type tp in AddInAssembly.GetExportedTypes())
                {
                    //如果类型是实现了 IAddIn的一个类，该类型就可以由宿主使用
                    if (tp.IsClass && typeof(IAddIn).IsAssignableFrom(tp))
                    {
                        AddInTypes.Add(tp);
                    }
                }
            }
        }

        private static void Show(string s)
        {
            Console.WriteLine("Heap size={0,12:##,###,###} - {1}", GC.GetTotalMemory(true), s);
        }

        private static void UseDynamicToBindToAndInvokeTheMember(Type t)
        {
            Console.WriteLine("UseDynamicToBindToAndInvokeTheMember");
            //不能创建一个构造器的委托
            Object[] args = new Object[] { 12 };//  构造器实参
            Console.WriteLine("x before constructor called:" + args[0]);
            dynamic obj = Activator.CreateInstance(t, args);    //给构造函数传参
            Console.WriteLine("Type:" + obj.GetType().ToString());
            Console.WriteLine("x after constructor returns:" + args[0]);
            //读写字段
            try
            {
                obj.m_someField = 5;
                Int32 v = (Int32)obj.m_someField;
                Console.WriteLine("m_someField:" + v);
            }
            catch (RuntimeBinderException e)
            {
                //由于m_someField是私有的 所以报错
                Console.WriteLine("Failed to access the Field:" + e.Message);
            }
            //调用一个方法
            String s = (String)obj.ToString();
            Console.WriteLine("ToString:" + s);
            //读写一个属性
            try
            {
                obj.SomeProp = 0;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Property set catch");
            }
            obj.SomeProp = 2;
            Int32 val = (Int32)obj.SomeProp;
            Console.WriteLine("SomeProp:" + val);
            obj.SomeEvent += new EventHandler(EventCallback);
            obj.SomeEvent -= new EventHandler(EventCallback);
        }

        private static void BindToMemberCreateDelegateToMemberThenInvokeTheMember(Type t)
        {
            Console.WriteLine("BindToMemberCreateDelegateToMemberThenInvokeTheMember");
            //够一个实例（不能创建一个构造函数的委托）
            Object[] args = new Object[] { 12 };    //构造函数参数
            Console.WriteLine("x before constructor called:" + args[0]);
            Object obj = Activator.CreateInstance(t, args);
            Console.WriteLine("Type:" + obj.GetType().ToString());
            Console.WriteLine("x after constructor returns:" + args[0]);
            //注意：不能创建一个构造函数的委托
            //调用一个方法
            MethodInfo mi = obj.GetType().GetMethod("ToString", c_bf);
            var toString = (Func<String>)Delegate.CreateDelegate(typeof(Func<String>), obj, mi);
            String s = toString();
            Console.WriteLine("ToString:" + s);
            //读写一个属性
            PropertyInfo pi = obj.GetType().GetProperty("SomeProp", typeof(Int32));
            var setSomeProp = (Action<Int32>)Delegate.CreateDelegate(typeof(Action<Int32>),obj, pi.GetSetMethod());
            try
            {
                setSomeProp(33);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Property set catch");
            }
            setSomeProp(2);
            var getSomeProp = (Action<Int32>)Delegate.CreateDelegate(typeof(Action<Int32>), obj, pi.GetGetMethod());
            Console.WriteLine("SomeProp:" + getSomeProp);
            //从事件中添加和删除一个委托
            EventInfo ei = obj.GetType().GetEvent("SomeEvent", c_bf);
            var addSomeEvent = (Action<EventHandler>)Delegate.CreateDelegate(typeof(Action<EventHandler>), obj, ei.GetAddMethod());
            addSomeEvent(EventCallback);
            var resolveSomeEvent = (Action<EventHandler>)Delegate.CreateDelegate(typeof(Action<EventHandler>), obj, ei.GetRemoveMethod());
            resolveSomeEvent(EventCallback);
        }

        private static void BindToMemberThenInvokeTheMember(Type t)
        {
            Console.WriteLine("BindToMemberThenInvokeTheMember");
            //构造一个实例
            ConstructorInfo ctor = t.GetConstructor(new Type[] { Type.GetType("System.Int32&") });
            //上面这一行也可以用下面的写法
            ConstructorInfo ctor1 = t.GetConstructor(new Type[] { typeof(Int32).MakeByRefType() });
            //构造器的实参
            Object[] args = new Object[] { 12 };
            Console.WriteLine("x before constructor called:" + args[0]);
            Object obj = ctor.Invoke(args);
            Console.WriteLine("Type:" + obj.GetType().ToString());
            Console.WriteLine("x after constructor returns:" + args[0]);
            //读写一个字段
            FieldInfo fi = t.GetField("m_someField", c_bf);
            fi.SetValue(obj, 33);
            Console.WriteLine("m_someField:" + fi.GetValue(obj));
            //调用一个方法
            MethodInfo mi = t.GetMethod("ToString", c_bf);
            String s = (String)mi.Invoke(obj, null);
            Console.WriteLine("ToString:" + s);
            //读写一个属性

        }

        private static void UseInvokeMemberBindAndInvokeTheMember(Type t)
        {
            Console.WriteLine("UseInvokeMemberBindAndInvokeTheMember");
            //构造一个Type实例
            Object[] args = new Object[] { 12 };//构造器实参
            Console.WriteLine("x before constructor called:" + args[0]);
            Object obj = t.InvokeMember(null, c_bf | BindingFlags.CreateInstance, null, null, args);
            Console.WriteLine("Type:" + obj.GetType().ToString());
            Console.WriteLine("x after constructor returns:" + args[0]);
            //读写一个字段
            t.InvokeMember("m_someField", c_bf | BindingFlags.SetField, null, obj, new Object[] { 5 });
            Int32 v = (Int32)t.InvokeMember("m_someField", c_bf | BindingFlags.GetField, null, obj, null);
            Console.WriteLine("someField:" + v);
            //调用一个方法
            String s = (String)t.InvokeMember("ToString", c_bf | BindingFlags.InvokeMethod, null, obj, null);
            Console.WriteLine("ToString:" + s);
            //读写一个属性
            try
            {
                t.InvokeMember("SomeProp", c_bf | BindingFlags.SetProperty, null, obj, new Object[] { 0 });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException.GetType() != typeof(ArgumentOutOfRangeException)) throw;
                Console.WriteLine("Property set catch");
            }
            v = (Int32)t.InvokeMember("SomeProp", c_bf | BindingFlags.GetProperty, null, obj, null);
            Console.WriteLine("SomeProp:" + v);
            //调用事件的add/resolve方法
            EventInfo ei = obj.GetType().GetEvent("SomeEvent", c_bf);
            EventHandler ts = new EventHandler(EventCallback);  //检查ei.EventHandlerType
            ei.AddEventHandler(obj, ts);
            ei.RemoveEventHandler(obj, ts);
        }
        private static void EventCallback(object sender, EventArgs e)
        { 
            
        }
        private static void LoadAssemblyAndShowPublicTypes(string dataAssembly)
        {
            Assembly a = Assembly.Load(dataAssembly);
            foreach (Type t in a.GetExportedTypes())
            {
                //显示类型全名
                Console.WriteLine(t.FullName);
            }
        }
    }

    internal sealed class SomeType
    {
        private Int32 m_someField;
        public SomeType(ref Int32 x) { x *= 2; }
        public override string ToString()
        {
            return m_someField.ToString();
        }
        public Int32 SomeProp
        {
            get { return m_someField; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("value");
                m_someField = value;
            }
        }
        public event EventHandler SomeEvent;
        private void NoCompilerWarnings() { SomeEvent.ToString(); }
    }
}
