using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;

namespace CommomCore
{
    class AsyncMvcRouteHandler
    {
        //public IHttpHandler GetHttpHandler(RequestContext requestContext)
        //{
        //    string controllerName = requestContext.RouteData.GetRequiredString("controller");
        //    var factory = ControllerBuilder.Current.GetControllerFactory();
        //    var controller = factory.CreateController(requestContext, controllerName);
        //    if (controller == null)
        //    {
        //        throw new InvalidOperationException("没有控制器");
        //    }
        //    var coreController = controller as Controller;
        //    if (coreController == null)
        //    {
        //        //return new SyncMvcHandler(controller, factory, requestContext);
        //    }
        //}
    }
}
