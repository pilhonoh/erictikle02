using SKT.Glossary.Biz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SKT.Glossary.Web
{
    public partial class dbconn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();

            try
            {
                GlossaryMainBiz biz = new GlossaryMainBiz();
                ds = biz.GlossaryInterfaceTnet();

                string tmp = string.Empty;

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string _title = string.Empty;
                        Row row = new Row();
                        row.CommonID = dr["CommonID"].ToString();
                        row.Title = dr["Title"].ToString();
                        _title = dr["Title"].ToString();

                        //Response.Write(_title + "<br>");

                        tmp = tmp + _title + "<br>";
                    }
                }
                //tmp = string.Empty;
                if (!string.IsNullOrEmpty(tmp))
                {
                    this.lbl1.Text = tmp;
                }
            } catch(Exception ex)
            {
                this.lbl1.Text = ex.ToString();
            }



        }
    }
}