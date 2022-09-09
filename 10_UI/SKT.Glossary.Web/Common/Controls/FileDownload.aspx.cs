using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Tnet.Framework.Security;


namespace SKT.Glossary.Web.Common.Controls
{
    public partial class FileDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileKey = Request.QueryString["FileKey"] != null ? Request.QueryString["FileKey"].ToString() : "";
            string fileName = Request.QueryString["FileName"] != null ? Request.QueryString["FileName"].ToString() : "";
            string filePath = Request.QueryString["FilePath"] != null ? Request.QueryString["FilePath"].ToString() : "";

            fileName = HttpUtility.UrlPathEncode(fileName);
            filePath = HttpUtility.HtmlDecode(filePath);

            Impersonation im = new Impersonation();
            im.ImpersonationStart();

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            Response.TransmitFile(Server.MapPath(filePath));
            Response.End();

            im.ImpersonationEnd(); 
        }
    }
}