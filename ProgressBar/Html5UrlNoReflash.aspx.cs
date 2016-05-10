using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgressBar
{
    public partial class Html5UrlNoReflash : System.Web.UI.Page
    {
        public int page = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["page"] == null)
            {
                page = 1;
            }
            else
            {
                page = Convert.ToInt16(Request.QueryString["page"]) + 1;
            }
        }
    }
}