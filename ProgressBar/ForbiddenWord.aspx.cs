using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommomCore;
using CommomCore.dll;
using System.Linq.Expressions;
using System.Threading;

namespace ProgressBar
{
    public partial class ForbiddenWord : System.Web.UI.Page, IForbiddenWordFilter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBox1.Text = "FORBIDDEN_WORD Hi,FORBIDDEN_WORD";
                IForbiddenWordFilter wf = new ForbiddenWord();
                wf.GetFilterType(TextBox1.Text);

                string s = "2015年06月";
                string[] yearMonth = s.Split(new string[] { "年", "月" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        protected void Click(object sender, EventArgs e)
        {

        }
        
        FilterForbiddenWordType IForbiddenWordFilter.GetFilterType(string key)
        {
            //if (key.EndsWith(this.TextBox2.ID)) return FilterForbiddenWordType.Ignored;
            //return FilterForbiddenWordType.Normal;

            Expression<Func<object>> action = () => this.TextBox2;
            var name = (action.Body as MemberExpression).Member.Name;

            if (key.EndsWith(name)) return FilterForbiddenWordType.Ignored;
            return FilterForbiddenWordType.Normal;
        }
    }
}