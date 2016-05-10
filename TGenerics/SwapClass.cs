using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGenerics
{
    internal static class SwapClass
    {
        public static void Swap<T>(ref T o1, ref T o2)
        {
            T temp = o1;
            o1 = o2;
            o2 = temp;
        }
        public static T Min<T>(T o1, T o2) where T : IComparable<T>
        {
            if (o1.CompareTo(o2) < 0) return o1;
            return o2;
        }
        public static List<TBase> ConvertIList<T, TBase>(IList<T> list) where T : TBase
        {
            List<TBase> baseList = new List<TBase>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                baseList.Add(list[i]);
            }
            return baseList;
        }
    }
    //public static class Interlocked
    //{
    //    public static T Exchange<T>(ref T location, T value) where T : class;
    //    public static T CompareExchange<T>(ref T location, T value, T comparand) where T : class;
    //}
}
