using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgressBar
{
    public partial class weather : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.Repeater1.DataSource = Commom.Comtent.GetComment(this.PageSize, this.PageIndex, out this.m_totalCount);
            this.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        private int m_totalCount;
        public int TotalCount
        {
            get
            {
                return this.m_totalCount;
            }
        }
    }
}