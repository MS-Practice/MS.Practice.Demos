using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.Diagnostics;
using FileLoader;
using System.Xml.Serialization;
using System.IO;

namespace TGenerics
{
    public class User
    {
        public string UserName { get; set; }
        public string Age { get; set; }
        public string School { get; set; }
        public GradeType[] Grade { get; set; }
    }
    public class GradeType
    {
        public string Grade { get; set; }
        public string Subject { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleTypeMethod();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //CompareMethodA();
            //CompareMethodB();
            //sw.Stop();
            //Console.WriteLine("时间：" + sw.ElapsedMilliseconds);
            //Action ac = CompareMethodA;
            //sw.Restart();
            //IAsyncResult async =  ac.BeginInvoke(null, null);
            //CompareMethodB();
            //ac.EndInvoke(async);
            //sw.Stop();
            //Console.WriteLine("异步时间：" + sw.ElapsedMilliseconds);
            //Console.ReadLine();
            User user = new User
            {
                Age = "22",
                Grade = new GradeType[]{
                    new GradeType{
                        Grade="123",
                        Subject="语文"
                    },
                    new GradeType{
                        Grade="150",
                        Subject="数学"
                    }
                },
                School = "岳阳市第一中学",
                UserName = "Marson"
            };
            //XmlSerializer serializer = new XmlSerializer(typeof(User));
            string xml = FileLoader.XmlHelper<User>.Serializer(typeof(User), user);
            Console.ReadLine();
        }
        private static void CallingSwap()
        {
            Int32 n1 = 1, n2 = 2;
            Console.WriteLine("n1={0},n2={1}", n1, n2);
            SwapClass.Swap<Int32>(ref n1, ref n2);
            Console.WriteLine("n1={0},n2={1}", n1, n2);

            string s1 = "Aidan", s2 = "Kristin";
            Console.WriteLine("s1={0},s2={1}", s1, s2);
            SwapClass.Swap<String>(ref s1, ref s2);
            Console.WriteLine("s1={0},s2={1}", s1, s2);
        }
        private static void CallingConvertIList()
        {
            IList<String> ls = new List<String>();
            ls.Add("A String");
            IList<Object> lo = SwapClass.ConvertIList<String, Object>(ls);
            IList<IComparable> lc = SwapClass.ConvertIList<String, IComparable>(ls);
            IList<IComparable<String>> lcs = SwapClass.ConvertIList<String, IComparable<String>>(ls);
            IList<String> ls2 = SwapClass.ConvertIList<String, String>(ls);
            //IList<Exception> le = SwapClass.ConvertIList<String, Exception>(ls);
        }
        //private static T Sum<T>(T num) where T : struct {
        //    T sum = default(T);
        //    for (T n = default(T); n < num; n++) {
        //        sum += n;
        //    }
        //    return sum;
        //}
        private static void IMethod()
        {
            String s = "Marson";
            ICloneable cloneable = s;
            IComparable comparable = s;
            IEnumerable enumerable = (IEnumerable)comparable;
        }
        public static void SimpleTypeMethod()
        {
            SimpleType st = new SimpleType();
            st.Dispose();
            IDisposable d = st;
            d.Dispose();
            Int32 x = 1, y = 2;
            IComparable<Int32> c = x;
            c.CompareTo(y);
        }
        public static void CompareMethodA()
        {
            int num = 0;
            for (int i = 0; i < 1000 * 1000 * 1000; i++)
            {
                num++;
            }
        }
        public static void CompareMethodB()
        {
            int num = 0;
            for (int i = 0; i < 1000 * 1000 * 1000; i++)
            {
                num++;
            }
        }
    }
}
