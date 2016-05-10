using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Mvc;
using System.Web.UI;

namespace CommomCore.dll
{
    public static class JQueryExtensions
    {
        //public static JQueryHelper JQuery(this ViewPage page) {
        //    var key = typeof(JQueryHelper);
        //    var jquery = page.Items[key] as JQueryHelper;
        //    if (jquery == null)
        //    {
        //        page.Items[key] = jquery = new JQueryHelper(page.ViewContext, page);
        //    }
        //    return jquery;
        //}
        public static JQueryHelper JQuery(this Page page,string type) {
            var key = typeof(JQueryHelper);
            var jquery = page.Items[key] as JQueryHelper;
            if (jquery == null)
            {
                page.Items[key] = jquery = new JQueryHelper(page);
            }
            return jquery;
        }
    }
}
