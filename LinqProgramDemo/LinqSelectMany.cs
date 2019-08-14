using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LinqProgramDemo
{
    class LinqSelectMany
    {
        public static IEnumerable<string> GetResouces()
        {
            var query = from file in Directory.GetFiles("F:\\MS\\Work\\YHProject\\YH.Web-git\\YH.Web\\yhweb\\logs\\2019-08-13")
                        from line in File.ReadLines(file)
                        select Path.GetFileName(file) + ": " + line;
            return query;
        }

        public static IEnumerable<string> GetResourcesConvert()
        {
            var query = Directory.GetFiles("F:\\MS\\Work\\YHProject\\YH.Web-git\\YH.Web\\yhweb\\logs")
                .SelectMany(file => File.ReadLines(file),
                (file, line) => Path.GetFileName(file) + ": " + line);
            return query;
        }
    }

    public static class OwnerLinqSelectMany
    {
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            return SelectManyImpl(source,
                                  (value, index) => selector(value),
                                  (originalElement, subsequenceElement) => subsequenceElement);
        }
        // Simplest overload 
        private static IEnumerable<TResult> SelectManyImpl<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            foreach (TSource item in source)
            {
                foreach (TResult result in selector(item))
                {
                    yield return result;
                }
            }
        }
        private static IEnumerable<TResult> SelectManyImpl<TSource, TCollection, TResult>(
            IEnumerable<TSource> source, 
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector, 
            Func<TSource, TCollection, TResult> resultSelector)
        {
            int index = 0;
            foreach (TSource item in source)
            {
                foreach (TCollection collectionItem in collectionSelector(item, index++))
                {
                    yield return resultSelector(item, collectionItem);
                }
            }
        }
    }
}
