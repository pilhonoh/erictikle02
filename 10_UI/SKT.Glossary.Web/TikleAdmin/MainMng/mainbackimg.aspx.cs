using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using SKT.Common;
using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Controls;
using System.Security.Cryptography;

namespace SKT.Glossary.Web.TikleAdmin.MainMng
{
    public partial class mainbackimg : System.Web.UI.Page
    {
        protected UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);

            btnSubmit.Attributes.Add("style", "padding:0px;text-align:center; background-color:lightgray; ");
            btnMove.Attributes.Add("style", "padding:0px;text-align:center; background-color:lightgray; ");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (fileMainimg.FileName.ToUpper().Equals("BG.GIF"))
            {
                try
                {
                    string NAS_VirtualDirectory = SKT.Tnet.Framework.Configuration.ConfigReader.GetString("NAS_VirtualDirectory");
                    string NAS_PhysicalPath = HttpContext.Current.Server.MapPath("/" + NAS_VirtualDirectory);

                    Impersonation im = new Impersonation();
                    im.ImpersonationStart();

                    fileMainimg.SaveAs(NAS_PhysicalPath + "\\Glossary\\tiklemain\\bg_after.gif");
                    im.ImpersonationEnd();

                    msg = "alert('저장 되었습니다.');";
                }
                catch 
                {
                    msg = "alert('저장 중 오류가 발생하였습니다.');";
                }
            }
            else
            {
                msg = "alert('파일명을 bg.gif로 저장하여 주시기 바랍니다.');";
            }

            //Response.Write("<script language='javascript'>" + msg + "</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", msg, true); 

        }


        protected void btnMove_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            try
            {
                string NAS_VirtualDirectory = SKT.Tnet.Framework.Configuration.ConfigReader.GetString("NAS_VirtualDirectory");
                string NAS_PhysicalPath = HttpContext.Current.Server.MapPath("/" + NAS_VirtualDirectory);

                string Beforefile = NAS_PhysicalPath + "\\Glossary\\tiklemain\\bg_before.gif";
                string Afterfile = NAS_PhysicalPath + "\\Glossary\\tiklemain\\bg_after.gif";
                string Realfile = NAS_PhysicalPath + "\\Glossary\\tiklemain\\bg.gif";

                Impersonation im = new Impersonation();
                im.ImpersonationStart();
                File.Copy(Realfile, Beforefile, true);
                File.Copy(Afterfile, Realfile, true);

                string filePath = NAS_PhysicalPath + "\\Glossary\\tiklemain\\bg_history.txt";
                if (File.Exists(filePath))
                {
                    FileStream aFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(aFile);
                    sw.WriteLine("사용자 : " + u.UserID + " /   시간 : " + System.DateTime.Now.ToString());
                    sw.WriteLine("**********************************************************************");

                    sw.Close();
                    aFile.Close();
                }
                else
                {
                    FileStream aFile = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(aFile);

                    sw.WriteLine("사용자 : " + u.UserID + " /   시간 : " + System.DateTime.Now.ToString());
                    sw.WriteLine("**********************************************************************");

                    sw.Close();
                    aFile.Close();
                }

                im.ImpersonationEnd();

                msg = "alert('정상등록 되었습니다.');";
            }
            catch (Exception ex)
            {
                msg = "alert('저장 중 오류가 발생하였습니다.');";
            }
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", msg, true); 
        }
    }
}