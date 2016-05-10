using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    public class RefAndValueType
    {
    }

    //class是引用类型
    public class RefType {
        public int Value;
    }
    //struct是值类型
    public struct ValueType {
        public int Value;
    }

    public struct MyStruct
    {
        private int _myNo;
        public int MyNo
        {
            get { return _myNo; }
            set { _myNo = value; }
        }

        public MyStruct(int myNo)
        {
            _myNo = myNo;
        }

        public void ShowNo()
        {
            Console.WriteLine(_myNo);
        }

        // 01.1 值类型的类型判等
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        // 01.2 自定义类型转：整形->MyStruct型
        static public explicit operator MyStruct(int myNo)
        {
            return new MyStruct(myNo);
        }
    }

    public class MyClass
    {
        private int _myNo;

        public int MyNo
        {
            get { return _myNo; }
            set { _myNo = value; }
        }

        public MyClass()
        {
            _myNo = 0;
        }

        public MyClass(int myNo)
        {
            _myNo = myNo;
        }


        public void ShowNo()
        {
            Console.WriteLine(_myNo);
        }

        // 02.1 引用类型的类型判等
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        // 02.2 自定义类型转换：MyClass->string型
        static public implicit operator string(MyClass mc)
        {
            return mc.ToString();
        }

        public override string ToString()
        {
            return _myNo.ToString();
        }
    }

    // 04 ref和out转换
    public class RefAndOut
    {
        //public static void Main()
        //{
        //    //必须进行初始化，才能使用ref方式传递
        //    int x = 10;
        //    ValueWithRef(ref x);
        //    Console.WriteLine(x);

        //    //使用out方式传递，不必初始化
        //    int y;
        //    ValueWithOut(out y);
        //    Console.WriteLine(y);

        //    object oRef = new object();
        //    RefWithRef(ref oRef);
        //    Console.WriteLine(oRef.ToString());

        //    object owith;
        //    RefWithOut(out owith);
        //    Console.WriteLine(owith.ToString());
        //}
        static void ValueWithRef(ref int i)
        {
            i = 100;
            Console.WriteLine(i.ToString());
        }

        static void ValueWithOut(out int i)
        {
            i = 200;
            Console.WriteLine(i.ToString());
        }

        static void RefWithRef(ref object o)
        {
            o = new MyStruct();
            Console.WriteLine(o.ToString());
        }

        static void RefWithOut(out object o)
        {
            o = new String('a', 10);
            Console.WriteLine(o.ToString());
        }
    }

}
