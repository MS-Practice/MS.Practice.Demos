using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqProgramDemo
{
    public static class LinqConcat
    {
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first,IEnumerable<TSource> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            return ConcatImpl(first, second);
        }
        public static T ReturnAndSetToNull<T>(ref T value) where T : class
        {
            T tmp = value;
            value = null;
            return tmp;
        }

        private static IEnumerable<TSource> ConcatImpl<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            foreach (var item in ReturnAndSetToNull(ref first))
            {
                yield return item;
            }
            //优化，遍历完 first 应对 first = null 利于 GC (不必要
            first = null;
            foreach (var item in second)
            {
                yield return item;
            }
        }
    }
}
