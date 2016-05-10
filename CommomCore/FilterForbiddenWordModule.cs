using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;

namespace CommomCore
{
    public class FilterForbiddenWordModule : IHttpModule
    {
        void IHttpModule.Dispose() { }
        public void Init(HttpApplication context)
        {
            context.PostMapRequestHandler += new EventHandler(context_PostMapRequestHandler);
            //别着急这进行过滤
            //context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        void context_PostMapRequestHandler(object sender, EventArgs e)
        {
            var context = (sender as HttpApplication).Context;      
            var handlerType = context.Handler.GetType();
            //iforb.GetFilterType()
            //var filter = (IForbiddenWordFilter)handlerType.GetInterface("IForbiddenWordFilter", true);
            //var filter = ((FilterForbiddenWordAttribute[])handlerType.GetCustomAttributes(typeof(FilterForbiddenWordAttribute), true)).FirstOrDefault();
            //ProcessCollection(context.Request.QueryString, filter);
            //ProcessCollection(context.Request.Form, filter);
            //throw new NotImplementedException();
        }
        private static PropertyInfo s_isReadOnlyPropertyInfo;

        static FilterForbiddenWordModule()
        {
            Type type = typeof(NameObjectCollectionBase);
            s_isReadOnlyPropertyInfo = type.GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void OnBeginRequest(object sender, EventArgs e)
        {
            var reqeust = (sender as HttpApplication).Request;
            //reqeust.
            var response = (sender as HttpRequest);
            ProcessCollection(reqeust.QueryString);
            ProcessCollection(reqeust.Form);
        }

        private static void ProcessCollection(NameValueCollection collection)
        {
            var copy = new NameValueCollection();
            foreach (string key in collection.AllKeys)
            {
                Array.ForEach(collection.GetValues(key), v => copy.Add(key, ForbiddenWord.Filter(v)));
            }
            //利用反射设置NameValueCollectionBase基类中的IsReadOnly属性
            s_isReadOnlyPropertyInfo.SetValue(collection, false, null);
            collection.Clear();
            collection.Add(copy);
            s_isReadOnlyPropertyInfo.SetValue(collection, true, null);
        }
        private static void ProcessCollection(NameValueCollection collection, IForbiddenWordFilter iw) {
            var copy = new NameValueCollection();
            foreach (string key in collection.AllKeys)
            {
                var filterType = (iw == null) ? FilterForbiddenWordType.Normal : iw.GetFilterType(key);
                Array.ForEach(collection.GetValues(key), v => copy.Add(key, ForbiddenWord.Filter(v,filterType)));
            }
        }

        //private static void ProcessCollection(NameValueCollection collection, FilterForbiddenWordAttribute filter)
        //{
        //    var copy = new NameValueCollection();
        //    foreach (var key in collection.AllKeys)
        //    {
        //        var filterType = (filter == null) ? FilterForbiddenWordType.Normal : filter.GetFilterType(key);
        //        Array.ForEach(collection.GetValues(key), v => copy.Add(key, ForbiddenWord.Filter(v)));
        //    }
        //}
    }

    public class ForbiddenWord
    {
        public static string Filter(string original)
        {
            return original.Replace("FORBIDDEN_WORD", "**");
        }
        public static string Filter(string original, FilterForbiddenWordType type) {
            return original.Replace("FORBIDDEN_WORD", "**");
        }
    }
    public enum FilterForbiddenWordType
    {
        Ignored,
        Normal,
        Json,
        Html
    }
    //public abstract class FilterForbiddenWordAttribute : Attribute
    //{
    //    public abstract FilterForbiddenWordType GetFilterType(string key);
    //}
    //有特别的需求，就可以通过定义一个FilterForbiddenWordHandlerAttribute的子类，重载GetFilterType方法，然后标记在HttpHandler上
    //public class DefaultFilterForbiddenWordAttribute : FilterForbiddenWordAttribute
    //{
    //    public override FilterForbiddenWordType GetFilterType(string key)
    //    {
    //        if (key.EndsWith("txtPassword"))
    //        {
    //            return FilterForbiddenWordType.Ignored;
    //        }
    //        return FilterForbiddenWordType.Normal;
    //    }
    //}
    //不用Customer Attribute形式 采用接口访问形式 最大的好处就是让接口成员访问页面内部的非公开成员（NoPublic）
    public interface IForbiddenWordFilter
    {
        FilterForbiddenWordType GetFilterType(string key);
    }
}
