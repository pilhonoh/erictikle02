using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SKT.Glossary.Web.Directory;

namespace SKT.Glossary.Web.Gathering
{
    public partial class FileOpenTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["file"] != null)
                {
                    NameValueCollection data = new NameValueCollection();
                    string fileName = Request.Params["file"];
                    if (Request.Params["tikle"] != null && Request.Params["tikle"] == "1")
                    {
                        fileName = SKT.Common.CryptoHelper.AESDecryptString(fileName.Replace(" ", "+"), "sktelecom_tikle2");

                    }

                    string fileUrl = @"http://tikledocs.sktelecom.com/tikledocs/" + fileName;

                    if (Request.Params["tikle"] != null && Request.Params["tikle"] == "2")
                    {
                        fileUrl = fileName;
                    }


                    data.Add("file", fileUrl);
                    data.Add("tikle", "31163105310731083101");

                    //HttpHelper.RedirectAndPOST(this.Page, "http://p056874/_layouts/fileopen.aspx", data);

                    HttpHelper.RedirectAndPOST(this.Page, "http://tikledocs.sktelecom.com/_layouts/fileopen.aspx", data);

                }
            }
        }
    }
}