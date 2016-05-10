using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace CommomCore.dll
{
    public class JQueryHelper
    {
        public ViewContext ViewContext { get; private set; }
        public ViewPage Page { get; private set; }
        public RouteCollection RouteCollection { get; private set; }
        //用于Page
        public Page AspNetPage { get; private set; }

        public JQueryHelper(ViewContext viewContext, ViewPage page)
            : this(viewContext, page, RouteTable.Routes) { }
        public JQueryHelper(Page page) {
            this.AspNetPage = page;
        }

        public JQueryHelper(ViewContext viewContext, ViewPage page, RouteCollection routeCollection)
        {
            // TODO: Complete member initialization
            this.ViewContext = viewContext;
            this.Page = page;
            this.RouteCollection = routeCollection;
        }
    }
}
