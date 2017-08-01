using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgressBar
{
    public partial class RepeaterTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<StringDats> lists = new List<StringDats>{
                new StringDats{ ID="1", Name="Marson"},
                new StringDats{ ID="2", Name="Swift"}
            };
            Repeater1.DataSource = lists;
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
    internal class StringDats {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}