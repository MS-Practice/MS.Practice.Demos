using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObsceneReplace;

namespace MS.Practice.Demos
{
    //在泛型中，这样起到约束作用
    //new 约束指定泛型类声明中的任何类型参数都必须有公共的无参数构造函数
    public class NewProperty<T> where T : new()
    {
        public T GetItem()
        {
            return new T();
        }
    }
    public class MyCls
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public MyCls()
        {
            _name = "Marson";
        }
    }
    public class MyNewProperty
    {
        public static void Main1(string[] arg)
        {
            //NewProperty<MyCls> mycls = new NewProperty<MyCls>();
            //Console.WriteLine(mycls.GetItem());

            //AppDomain appDomain1 = AppDomain.CreateDomain("MS.AppDomain1");
            //AppDomain appDomain2 = AppDomain.CreateDomain("MS.AppDomain2");
            //MarshalByRefType marshalByRefObj1 = appDomain1.CreateInstanceAndUnwrap("MS.Practice.Demos", "MS.Practice.Demos.MarshalByRefType") as MarshalByRefType;
            //MarshalByRefType marshalByRefObj2 = appDomain2.CreateInstanceAndUnwrap("MS.Practice.Demos", "MS.Practice.Demos.MarshalByRefType") as MarshalByRefType;
            //object obj = new object();
            ////marshalByRefObj1.StringLockHelper = "Hello World";
            ////marshalByRefObj2.StringLockHelper = "Hello World!";
            //marshalByRefObj1.ObjectLockHelper = obj;
            //marshalByRefObj2.ObjectLockHelper = obj;
            //Thread thread1 = new Thread(new ParameterizedThreadStart(Execute));
            //Thread thread2 = new Thread(new ParameterizedThreadStart(Execute));
            //thread1.Start(marshalByRefObj1);
            //thread2.Start(marshalByRefObj2);
            #region 值类型和引用类型
            ////内存分配于线程的堆栈上
            ////创建了值为等价"0"的实例
            //MyStruct myStruct = new MyStruct();
            ////在线程的堆栈上创建了引用，但未指向任何实例
            //MyClass myClass;
            ////内存分配于托管堆上
            //myClass = new MyClass();

            ////在堆栈上修改成员
            //myStruct.MyNo = 1;
            ////将指针指向托管堆
            //myClass.MyNo = 2;

            //myStruct.ShowNo();
            //myClass.ShowNo();

            ////在堆栈上新建内存，并执行成员拷贝
            //MyStruct myStruct2 = myStruct;
            ////拷贝引用地址
            //MyClass myClass2 = myClass;

            ////在堆栈上修改myStruct成员
            //myStruct.MyNo = 3;
            ////在托管堆上修改成员
            //myClass.MyNo = 4;

            //myStruct.ShowNo();
            //myClass.ShowNo();
            //myStruct2.ShowNo();
            //myClass2.ShowNo();

            //#region 03. 类型转换
            //MyStruct MyNum;
            //int i = 100;
            //MyNum = (MyStruct)i;
            //Console.WriteLine("整形显式转换为MyStruct型---");
            //Console.WriteLine(i);

            //MyClass MyCls = new MyClass(200);
            //string str = MyCls;
            //Console.WriteLine("MyClass型隐式转换为string型---");
            //Console.WriteLine(str);
            //#endregion

            //#region 04. 使用sizeof判断值类型大小
            ////unsafe
            ////{
            ////    Console.WriteLine("使用sizeof判断值类型大小---");
            ////    Console.WriteLine(sizeof(MyStruct));
            ////}
            //#endregion


            //#region 05 类型判等
            //Console.WriteLine("类型判等---");
            //// 05.1 ReferenceEquals判等
            ////值类型总是返回false，经过两次装箱的myStruct不可能指向同一地址
            //Console.WriteLine(ReferenceEquals(myStruct, myStruct));
            ////同一引用类型对象，将指向同样的内存地址
            //Console.WriteLine(ReferenceEquals(myClass, myClass));
            ////RefenceEquals认为null等于null，因此返回true
            //Console.WriteLine(ReferenceEquals(null, null));

            //// 05.2 Equals判等
            ////重载的值类型判等方法，成员大小不同
            //Console.WriteLine(myStruct.Equals(myStruct2));

            ////重载的引用类型判等方法，指向引用相同
            //Console.WriteLine(myClass.Equals(myClass2));

            //#endregion

            //#region 06 垃圾回收的简单阐释
            ////实例定义及初始化
            //MyClass mc1 = new MyClass();
            ////声明但不实体化
            //MyClass mc2;
            ////拷贝引用，mc2和mc1指向同一托管地址
            //mc2 = mc1;
            ////定义另一实例，并完成初始化
            //MyClass mc3 = new MyClass();
            ////引用拷贝，mc1、mc2指向了新的托管地址
            ////那么原来的地址成为GC回收的对象，在
            //mc1 = mc3;
            //mc2 = mc3;
            //#endregion 
            #endregion
            //int a = 10;
            //Add(a);

            //ArgsByRef abf = new ArgsByRef();
            //AddRef(abf);
            //Console.WriteLine(abf.i);

            //string str = "One";
            //ShowInfo(str);
            //ShowInfo(ref str);
            //Console.WriteLine(str);

            //VIPUser aUser;
            //aUser = new VIPUser();
            //aUser.isVip = true;
            //Console.WriteLine(aUser.IsVipUser());

            //string s1 = "abc";
            //string s2 = "ab";
            //string s3 = s2 + "c";
            //Console.WriteLine(ReferenceEquals(s1, s3));
            int n = 1 << 10;
            int[,] array = new int[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    array[x, y] = x;
                }
            }
            AddTwoArray.TestLocality(array, 1000);
            Console.Read();
        }
        private static string GetStr()
        {
            return "abc";
        }
        public static string s1 = "abc";
        static void Execute(object obj)
        {
            MarshalByRefType marshalByRefObj = obj as MarshalByRefType;
            //marshalByRefObj.ExecuteWithStringLocked();
            marshalByRefObj.ExecuteWithObjectLocked();
        }
        private static void Add(int j)
        {
            j = j + 1;
            Console.WriteLine(j);
        }
        static void AddRef(ArgsByRef abf)
        {
            abf.i = 20;
            Console.WriteLine(abf.i);
        }
        static void ShowInfo(string str)
        {
            str = "cuahs";
            Console.WriteLine(str);
        }
        static void ShowInfo(ref string str)
        {
            str = "cuahs";
            Console.WriteLine(str);
        }
    }
    public class ArgsByRef
    {
        public int i = 10;
    }
}
