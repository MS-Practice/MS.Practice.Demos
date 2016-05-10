using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArraySolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Go();
        }

        private static void Go()
        {
            int[] myControls;
            myControls = new int[20];
            MyDemo[] myDemos;
            myDemos = new MyDemo[20];

            MyValueType[] src = new MyValueType[100];
            Int32[] arrayInt = new int[100];
            //Array.ConstrainedCopy(src, 0, arrayInt, 0, src.Length); //Array.ConstrainedCopy不破坏目标数组的数据 不会执行装箱拆箱（两者数组类型相同）
            //IComparable[] dest = new IComparable[src.Length];
            //Array.Copy(dest, src, src.Length);
            //Point[][]
            AsReadOnly();
            ShowWhere(arrayInt, -1);
            DynamicArray();
            SaftIntersectAndUnsafe();

            StackallocDemo();
            InlineArrayDemo();
        }
        private static void StackallocDemo()
        {
            unsafe
            {
                const Int32 width = 20;
                Char* pc = stackalloc Char[width];  // 在线程栈上分配数组
                String s = "Marson Swift";  // 12个字符
                for (Int32 index = 0; index < width; index++)
                {
                    pc[width - index - 1] =
                        (index < s.Length) ? s[index] : '.';
                }
                Console.WriteLine(new String(pc, 0, width));
            }
        }
        private static void InlineArrayDemo()
        {
            unsafe
            {
                CharArray ca;   //在线程栈上分配数组
                Int32 widthInBytes = sizeof(CharArray);
                Int32 width = widthInBytes / 2;
                String s = "Marson Swift";
                for (Int32 index = 0; index < width; index++)
                {
                    ca.Characters[width - index - 1] =
                        (index < s.Length) ? s[index] : '.';
                }
                Console.WriteLine(new String(ca.Characters, 0, width));
            }
        }
        internal unsafe struct CharArray
        {
            //这个数组以内联的方式嵌入结构
            public fixed Char Characters[20];
        }
        private const Int32 c_numElements = 10000;
        private static void SaftIntersectAndUnsafe()
        {
            const Int32 testCount = 10;
            Stopwatch sw;
            //声明一个二维数组
            Int32[,] a2Dim = new Int32[c_numElements, c_numElements];
            //将一个二维数组申明为一个交错数组（向量构成的向量）
            Int32[][] aJagged = new Int32[c_numElements][];
            for (Int32 x = 0; x < c_numElements; x++)
            {
                aJagged[x] = new Int32[c_numElements];
            }
            //1：用普通的安全技术访问数组中的元素
            sw = Stopwatch.StartNew();
            for (Int32 test = 0; test < testCount; test++)
            {
                Safe2DimArrayAccess(a2Dim);
            }
            Console.WriteLine("{0}: Safe2DimArrayAccess", sw.Elapsed);
            //2：用交错数组技术访问数组中的所有元素
            sw = Stopwatch.StartNew();
            for (Int32 test = 0; test < testCount; test++)
            {
                SafeJaggedArrayAccess(aJagged);
            }
            Console.WriteLine("{0}: SafeJaggedArrayAccess", sw.Elapsed);
            //3：用不安全代码访问数组中的元素
            sw = Stopwatch.StartNew();
            for (Int32 test = 0; test < testCount; test++)
            {
                Unsafe2DimArrayAccess(a2Dim);
            }
            Console.WriteLine("{0}: Unsafe2DimArrayAccess", sw.Elapsed);
        }
        private static unsafe Int32 Unsafe2DimArrayAccess(Int32[,] aDim)
        {
            Int32 sum = 0;
            fixed (Int32* pi = aDim)
            {
                for (Int32 x = 0; x < c_numElements; x++)
                {
                    Int32 baseDim = x * c_numElements;
                    for (Int32 y = 0; y < c_numElements; y++)
                    {
                        sum += pi[baseDim + y];
                    }
                }
            }
            return sum;
        }
        private static Int32 Safe2DimArrayAccess(Int32[,] aDim)
        {
            Int32 sum = 0;
            for (Int32 x = 0; x < c_numElements; x++)
            {
                for (Int32 y = 0; y < c_numElements; y++)
                    sum += aDim[x, y];
            }
            return sum;
        }
        private static Int32 SafeJaggedArrayAccess(Int32[][] aJagged)
        {
            Int32 sum = 0;
            for (Int32 x = 0; x < c_numElements; x++)
            {
                for (Int32 y = 0; y < c_numElements; y++)
                    sum += aJagged[x][y];
            }
            return sum;
        }
        private static void DynamicArray()
        {
            Int32[] lowerBounds = { 2005, 1 };
            Int32[] lengths = { 5, 4 };
            Decimal[,] quarterlyRevenue = (Decimal[,])Array.CreateInstance(typeof(Decimal), lengths, lowerBounds);
            Console.WriteLine("{0,4} {1,9} {2,9} {3,9} {4,9}", "Year", "Q1", "Q2", "Q3", "Q4");
            Int32 firstYear = quarterlyRevenue.GetLowerBound(0);
            Int32 lastYear = quarterlyRevenue.GetUpperBound(0);
            Int32 firstQuarter = quarterlyRevenue.GetLowerBound(1);
            Int32 lastQuarter = quarterlyRevenue.GetUpperBound(1);
            for (Int32 Year = firstYear; Year <= lastYear; Year++)
            {
                Console.WriteLine(Year + " ");
                for (Int32 quarter = firstQuarter; quarter <= lastQuarter; quarter++)
                {
                    Console.WriteLine("{0,9:C} ", quarterlyRevenue[Year, quarter]);
                }
                Console.WriteLine();
            }
        }
        private static void BinarySearch()
        {
            string[] dinosaurs = {"Pachycephalosaurus", 
                              "Amargasaurus", 
                              "Tyrannosaurus", 
                              "Mamenchisaurus", 
                              "Deinonychus", 
                              "Edmontosaurus"};
            Console.WriteLine();
            foreach (string dinosaur in dinosaurs)
            {
                Console.WriteLine(dinosaur);
            }
            Console.WriteLine("\nSort");
            //排序
            Array.Sort(dinosaurs);
            Console.WriteLine();
            foreach (string dinosaur in dinosaurs)
            {
                Console.WriteLine(dinosaur);
            }
            Console.WriteLine("\nBinarySearch for 'Coelophysis':");
            int index = Array.BinarySearch(dinosaurs, "Coelophysis");
            ShowWhere(dinosaurs, index);
            Console.WriteLine("\nBinarySearch for 'Tyrannosaurus':");
            index = Array.BinarySearch(dinosaurs, "Tyrannosaurus");
            ShowWhere<string>(dinosaurs, index);
        }
        private static void ShowWhere<T>(T[] array, int index)
        {
            if (index < 0)
            {
                index = ~index;
                Console.Write("Not found. Sorts between: ");
                if (index == 0)
                    Console.Write("beginning of array and ");
                else
                    Console.Write("{0} and ", array[index - 1]);
                if (index == array.Length)
                    Console.WriteLine("end of array.");
                else
                    Console.WriteLine("{0}.", array[index]);
            }
            else
            {
                Console.WriteLine("Found at index {0}.", index);
            }
        }

        //Array.AsReadOnly函数  返回指定数组的只读包装
        private static void AsReadOnly()
        {
            String[] myArr = { "The", "quick", "brown", "fox" };
            Console.WriteLine("The string array initially contains the following values:");
            PrintsIndexAndValues(myArr);

            IList<String> myList = Array.AsReadOnly(myArr);
            Console.WriteLine("The read-only IList contains the following values:");
            PrintsIndexAndValues(myList);
            try
            {
                myList[3] = "Cat";
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine("{0} - {1}", e.GetType(), e.Message);
                Console.WriteLine();
            }
            // Change a value in the original array.
            myArr[2] = "RED";

            // Display the values of the array.
            Console.WriteLine("After changing the third element, the string array contains the following values:");
            PrintsIndexAndValues(myArr);
            // Display the values of the read-only IList.
            Console.WriteLine("After changing the third element, the read-only IList contains the following values:");
            PrintsIndexAndValues(myList);

        }

        private static void PrintsIndexAndValues(String[] myArr)
        {
            for (int i = 0; i < myArr.Length; i++)
            {
                Console.WriteLine("   [{0}] : {1}", i, myArr[i]);
            }
            Console.WriteLine();
        }
        private static void PrintsIndexAndValues(IList<String> myList)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                Console.WriteLine("   [{0}] : {1}", i, myList[i]);
            }
            Console.WriteLine();
        }
    }
    internal class MyDemo
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    internal struct MyValueType : IComparable
    {
        public Int32 CompareTo(Object obj)
        {
            return 1;
        }
    }
}
