using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqProgramDemo
{
    class LinqEmpty
    {
        public static IEnumerable<int> Empty => Enumerable.Empty<int>();
        public static IEnumerable<int> Empty2 => Enumerable.Empty<int>();   //Enumerable.Empty<int>() 是会被缓存的

        public static bool IsEquals()
        {
            return Empty == Empty2; // return true
        }

        public static bool IsEqualsByNoCache()
        {
            return OwnerEmptyInumerable.Empty<int>() == OwnerEmptyInumerable.Empty<int>();
        }

        public static bool IsEqualsByCache()
        {
            return OwnerEmptyInumerable.EmptyCache<int>() == OwnerEmptyInumerable.EmptyCache<int>();
        }
    }

    public static class OwnerEmptyInumerable
    {
        //这是没有缓存效果的
        public static IEnumerable<TResult> Empty<TResult>()
        {
            yield break;
        }
        //缓存的
        public static IEnumerable<TResult> EmptyCache<TResult>() {
            return CacheInumerableEmpty<TResult>.Array;
        }
    }

    static class CacheInumerableEmpty<T>
    {
        internal static readonly T[] Array = new T[0];
    }
}
