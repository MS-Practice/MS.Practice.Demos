using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqProgramDemo
{
    public static class LinqCount
    {
        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            //性能更好 泛型
            var collection = source as ICollection<TSource>;
            if (collection != null) return collection.Count;
            //非泛型
            var nonGeneric = source as ICollection;
            if (nonGeneric != null) return collection.Count;
            checked
            {
                int count = 0;
                foreach (TSource item in source)
                {
                    if (predicate(item))
                        count++;
                }
                return count;
            }
        }
    }

    //自定义实现Count方法

}
