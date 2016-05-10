using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullableProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CompareNullableAndValueType(null, false));
            Console.WriteLine(CompareNullableAndValueType(null, true, ""));

            Point? p1 = new Point(1, 1);
            Point? p2 = new Point(2, 2);
            Console.WriteLine("Are points Equal? " + (p1 == p2).ToString());
            Console.WriteLine("Are points not Equal? " + (p1 != p2).ToString());

            String s = SomeMethod() ?? SomeMethod1() ?? "Swift";
            Console.WriteLine("The String is: " + s);

            Int32? n = 5;
            Int32 result = ((IComparable)n).CompareTo(5);

            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private static string SomeMethod()
        {
            return null;
        }
        private static string SomeMethod1()
        {
            return "Marson";
        }
        private static bool? CompareNullableAndValueType(Nullable<bool> param1, Nullable<bool> param2)
        {
            return (param1 & param2);
        }
        private static bool? CompareNullableAndValueType(Nullable<bool> param1, Nullable<bool> param2, string kind)
        {
            return (param1 | param2);
        }
    }
    internal struct Point
    {
        private Int32 m_x, m_y;
        public Point(Int32 x, Int32 y)
        {
            m_x = x;
            m_y = y;
        }
        public static Boolean operator ==(Point p1, Point p2)
        {
            return (p1.m_x == p2.m_x) && (p1.m_y == p2.m_y);
        }
        public static Boolean operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}
