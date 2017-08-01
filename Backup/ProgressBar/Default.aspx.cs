using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using CommomCore;

namespace ProgressBar
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SqlCommand command;
            //IAsyncResult ar = command.BeginExecuteNonQuery();
            //int result = command.EndExecuteNonQuery(ar);
            //Response.Write();
        }
        [WebMethod(CacheDuration=10)]
        protected void CecheMethod() { 
            
        }
    }
}