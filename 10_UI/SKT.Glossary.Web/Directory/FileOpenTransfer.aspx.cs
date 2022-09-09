using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using System.ServiceModel;
using System.ServiceModel.Channels;

using System.Web.Script.Serialization;

using System.IO;
using SKT.Common.TikleDocManagerService;
using System.Configuration;

namespace SKT.Glossary.Web.Directory
{
    public partial class FileOpenTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["file"] != null)
                {
                    string fileName = HttpUtility.UrlDecode(Request.Params["file"]);
                    if (Request.Params["tikle"] != null && Request.Params["tikle"] == "1")
                    {
                        fileName = SKT.Common.CryptoHelper.AESDecryptString(fileName.Replace(" ", "+"), "sktelecom_tikle2");

                    }

                    if (fileName.ToLower().IndexOf(".pdf") > -1)
                    {
                        byte[] fstrem = null;
                        
                        string[] arrFile = fileName.Split('/');
                        DirectoryCommon dirCommon = new DirectoryCommon();

                        fstrem = dirCommon.FileDownload(arrFile[0], arrFile[1]);

                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", Server.UrlEncode(fileName).Replace("+", "%20")));
                        Response.BinaryWrite(fstrem);
                        Response.End();
                    }
                    else
                    {
                        NameValueCollection data = new NameValueCollection();

                        string DocsUrl = ConfigurationManager.AppSettings["TikleDocsURL"].ToString();
                        string fileUrl = DocsUrl + "/tikledocs/" + fileName;

                        if (Request.Params["tikle"] != null && Request.Params["tikle"] == "2")
                        {
                            fileUrl = fileName;
                        }
                        data.Add("file", fileUrl);
                        data.Add("tikle", "31163105310731083101");

                        HttpHelper.RedirectAndPOST(this.Page, DocsUrl+"/_layouts/fileopen.aspx", data);
                    }

                }
            }
        }
    }
}