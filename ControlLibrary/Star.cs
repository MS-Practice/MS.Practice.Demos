using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ControlLibrary
{
    public class Star:WebControl
    {
        [DefaultValue(0)]
        public int Score
        {
            get
            {
                object obj = ViewState["Score"];
                return obj == null ? 0 : Convert.ToInt32(obj);
            }
            set
            {
                ViewState["Score"] = value;
            }
        }
        public string Comment
        {
            get
            {
                object obj = ViewState["Comment"];
                return obj == null ? string.Empty : Convert.ToString(obj);
            }
            set
            {
                ViewState["Comment"] = value;
            }
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            //CreateControlHierarchy();
            //this.Page.InitComplete += new EventHandler(CompleteEvent);
        }
        //protected void CompleteEvent(object sender, EventArgs e) { 
            
        //}

        protected virtual void CreateControlHierarchy()
        {
            Table table = new Table();
            TableRow row = new TableRow();
            table.Rows.Add(row);
            TableCell comment = new TableCell();
            CreateComment(comment);
            row.Cells.Add(comment);

            TableCell stars = new TableCell();

            CreateStars(stars);

            row.Cells.Add(stars);

            this.Controls.Add(table);
        }
        private void CreateComment(TableCell cell)
        {
            cell.Text = Comment;
        }
        private void CreateStars(TableCell cell)
        {
            string starPath = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ControlLibrary.Image.stars.gif");

            Panel panBg = new Panel();
            panBg.Style.Add(HtmlTextWriterStyle.Width, "80px");
            panBg.Style.Add(HtmlTextWriterStyle.Height, "16px");
            panBg.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            panBg.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
            panBg.Style.Add(HtmlTextWriterStyle.BackgroundImage, starPath);
            panBg.Style.Add("background-position", "0px -32px");
            panBg.Style.Add("background-repeat", "repeat-x");

            cell.Controls.Add(panBg);

            Panel panCur = new Panel();
            string width = Score * 16 + "px";
            panCur.Style.Add(HtmlTextWriterStyle.Width, width);
            panCur.Style.Add(HtmlTextWriterStyle.Height, "16px");
            panCur.Style.Add(HtmlTextWriterStyle.BackgroundImage, starPath);
            panCur.Style.Add("background-position", "0px 0px");
            panCur.Style.Add("background-repeat", "repeat-x");

            panBg.Controls.Add(panCur);
        }
        // 重写父类的Render方法，在该方法中将服务器控件的内容传递给HtmlTextWriter对象以在客户端呈现内容，在该方法中调用了PrepareControlForRender方法：
        protected override void Render(HtmlTextWriter writer)
        {
            PrepareControlForReader();

            base.Render(writer);
        }
        //实现PrepareControlForRender方法，该方法用于在呈现前进行其他样式的设置，在本控件中，只是简单的设置了表格的CellSpacing和CellPadding属性：
        private void PrepareControlForReader()
        {
            if (this.Controls.Count < 1)
                return;

            Table table = (Table)this.Controls[0];

            table.CellSpacing = 0;
            table.CellPadding = 0;
        }
    }
}