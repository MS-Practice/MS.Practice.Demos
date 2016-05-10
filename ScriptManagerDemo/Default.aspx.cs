using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Reflection;

namespace ScriptManagerDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Text = DateTime.Now.ToString();
            RegisterScripts();
            List<Products> listinfo = new List<Products>();
            Products info = new Products { ProductsId = 1, ProductsName = "苹果" };
            Products info1 = new Products { ProductsId = 2, ProductsName = "梨子" };
            Products info2 = new Products { ProductsId = 3, ProductsName = "火龙果" };
            listinfo.Add(info);
            listinfo.Add(info1);
            listinfo.Add(info2);
            var infoModel = from p in listinfo where p.ProductsId >1 select new { p.ProductsId, p.ProductsName };
            foreach (var item in infoModel) {
                Response.Write(item);
            }
        }
        protected void RegisterScripts()
        {
            //List<ScriptReference> arrayScript = new List<ScriptReference>();
            //ScriptReference[] sr = new ScriptReference[] { new ScriptReference("/Scripts/jquery-1.4.1-vsdoc.js"), new ScriptReference("/Scripts/processBarLoad.js") };
            //foreach (var item in sr) {
            //    this.ScriptManager.Scripts.Add(item);
            //}

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = DateTime.Now.ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager1.RegisterAsyncPostBackControl(this.Button2);
            Label2.Text = DateTime.Now.ToString();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public DateTime GetServerTime()
        {
            HttpCachePolicy cache = HttpContext.Current.Response.Cache;
            cache.SetCacheability(HttpCacheability.Private);
            cache.SetExpires(DateTime.Now.AddSeconds((double)10));
            cache.SetMaxAge(new TimeSpan(0, 0, 10));
            //通过反射机制直接修改类HttpCachePolicy中SetMaxAge方法中的_maxAge字段
            FieldInfo maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAgeField.SetValue(cache, new TimeSpan(0, 0, 10));
            return DateTime.Now;
        }
    }
    public class Products
    {
        public int ProductsId { get; set; }
        public string ProductsName { get; set; }
    }
}