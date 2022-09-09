using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

public partial class common_module_akc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region wisenut
        string query;

        // 자동완성 대상이 되는 검색어를 받아오는 메소드
        query = Request["q"];
        Request.ContentEncoding = System.Text.Encoding.UTF8;

        if (query != null)
        {
            akc httpresult = new akc();

            string akc_rtn = httpresult.akc_call(query);

            System.Diagnostics.Debug.WriteLine("akc_rtn:" + akc_rtn);

            Response.Write(akc_rtn);
        }
        else
        {
            Response.Write("");
        }


        #endregion


        #region konan
        /*
        string query, flag;

        query = Request.QueryString.Get("q");
        flag = Request.QueryString.Get("s");

        string server_ip_port = ConfigurationManager.AppSettings["SERVER_IP_PORT_KONAN"];

        if (query != null)
        {
            akc httpresult = new akc();
            Response.Write(httpresult.akc_call(server_ip_port, query, Convert.ToInt32(flag), 10, 0));
        }
        else
        {
            Response.Write("");
        }
        */
        #endregion

    }
}