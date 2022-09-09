using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using SKT.Glossary.Biz;
using System.Text.RegularExpressions;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SKT.Glossary.Web
{
    public partial class CheckWebServer_Web_DB : System.Web.UI.Page
    {
        private const string connectionStringName = "ConnGlossary";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                //SqlDbAgent agent = null;
                //DataSet ds = new DataSet();

                //agent = new SqlDbAgent("SKTDB_XONE");

                Database db = DatabaseFactory.CreateDatabase(connectionStringName);

                DbCommand cmd = db.GetSqlStringCommand("select top 1 * from TB_DEPARTMENT_ORGCHART");

                StringBuilder sb = new StringBuilder();
                //sb.AppendLine("SELECT top 1 * FROM [dbo].[ep_organizationt] ");

                StringBuilder sbR = new StringBuilder();
                sbR.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sbR.Append("<rows>");
                sbR.Append("<row>");
                sbR.Append("<col type=\"ServerName\">");
                sbR.Append("<![CDATA[ 150.19.41.226 ]]> ");
                sbR.Append("</col>");
                sbR.Append("<col type=\"Result\">");

                try
                {
                    //ds = agent.GetDataSet(sb.ToString());
                    db.ExecuteDataSet(cmd);
                    sbR.Append("<![CDATA[OK]]> ");

                }
                catch
                {
                    sbR.Append("<![CDATA[ERROR]]> ");

                }
                sbR.Append("</col>");
                sbR.Append("</row>");
                sbR.Append("</rows>");

                Response.Clear();
                Response.Write(sbR.ToString());
                Response.End();
            }
        }
    }
}