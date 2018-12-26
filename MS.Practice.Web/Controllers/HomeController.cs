using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MS.Practice.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public bool ImageUpload() {
            var parser = new HttpMultipartParser.MultipartFormDataParser(HttpContext.Request.InputStream);
            var custordNo = parser.GetParameterValue("custOrdNo");
            var files = parser.Files;
            return true;
        }
    }
}