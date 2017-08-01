using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProgressBar.Commom;
using System.Web.UI;
using System.Reflection;
using ProgressBar.UsercontrolToHtml;
using ProgressBar.Commom.Enum;

namespace ProgressBar.IHandler
{
    /// <summary>
    /// UserControlRenderingHandler 的摘要说明
    /// </summary>
    public class UserControlRenderingHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string appRelativaPath = context.Request.AppRelativeCurrentExecutionFilePath;
            string controlPath = appRelativaPath.ToLower().Replace(".ucr", "ascx");
            var viewManager = new ViewManager<UserControl>();
            var control = viewManager.LoadViewControl(controlPath);
            SetPropertyValues(control, context);
        }

        public static void SetPropertyValues(UserControl control, HttpContext context)
        {
            var metadata = GetMetadata(control.GetType());
            foreach (var property in metadata.Keys)
            {
                object value = GetValue(metadata[property], context);
                if (value != null) {
                    property.SetValue(control, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
        }

        private static object s_mutex = new object();

        private static Dictionary<PropertyInfo, List<UserControlRenderingPropertyAttribute>> GetMetadata(Type type)
        {
            if (!s_metadataCache.ContainsKey(type)) {
                lock (s_mutex) {
                    if (!s_metadataCache.ContainsKey(type)) {
                        s_metadataCache[type] = LoadMetadata(type);
                    }
                }
            }
            return s_metadataCache[type];
        }

        private static Dictionary<PropertyInfo, List<UserControlRenderingPropertyAttribute>> LoadMetadata(Type type)
        {
            var result = new Dictionary<PropertyInfo, List<UserControlRenderingPropertyAttribute>>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            foreach (var p in properties) {
                var attributes = p.GetCustomAttributes(typeof(UserControlRenderingPropertyAttribute), true);
                if (attributes.Length > 0) {
                    result[p] = new List<UserControlRenderingPropertyAttribute>(attributes.Cast<UserControlRenderingPropertyAttribute>());
                }
                //result[p] = new List<UserControlRenderingPropertyAttribute>(attributes
            }
            return result;
        }

        private static Dictionary<Type,Dictionary<PropertyInfo,List<UserControlRenderingPropertyAttribute>>> s_metadataCache = new Dictionary<Type,Dictionary<
            PropertyInfo,List<UserControlRenderingPropertyAttribute>>>();

        private static object GetValue(IEnumerable<UserControlRenderingPropertyAttribute> metadata,HttpContext context)
        {
            foreach (var att in metadata)
            {
                var collection = (att.Source == UserControlRenderingPropertySource.QueryString) ?
                        context.Request.QueryString : context.Request.Params;
                object value = collection[att.Key];

                if (value != null) return value;
            }

            return null;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}