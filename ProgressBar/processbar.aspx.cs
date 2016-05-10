using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace ProgressBar
{
    public partial class processbar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataColumn column = null;
            column = dt.Columns.Add("ID", Type.GetType("System.Int32"));
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;//起始为1
            column.AutoIncrementStep = 1;//步长为1
            column.AllowDBNull = false;
            column = dt.Columns.Add("Product", Type.GetType("System.String"));
            column = dt.Columns.Add("Version", Type.GetType("System.String"));
            column = dt.Columns.Add("Description", Type.GetType("System.String"));
            DataRow newRow;
            newRow = dt.NewRow();
            newRow["Product"] = "大话西游";
            newRow["Version"] = "2.0";
            newRow["Description"] = "我很喜欢";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow["Product"] = "梦幻西游";
            newRow["Version"] = "3.0";
            newRow["Description"] = "比大话更幼稚";
            dt.Rows.Add(newRow);
            List<DataTable> info = new List<DataTable>();
            info.Add(dt);
            var query = dt.AsEnumerable().Where(p => p["Product"].ToString() == "大话西游");
            var query1 = dt.AsEnumerable().Where(p => p.Table.Rows[0]["Product"].ToString() == "大话西游"); //这种写法没有效果
            DataTable dt1 = query.CopyToDataTable();
            DataTable dt2 = query1.CopyToDataTable();
            //DataTable dtnew = dt.AsEnumerable().ToList<DataRow>().Where(row => row.Table.Rows[0]["Product"].ToString() == "大话西游").CopyToDataTable();
            //Console.WriteLine(dtnew);
        }
    }
}