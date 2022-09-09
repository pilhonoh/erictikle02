using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using System.Configuration;
using SKT.Common;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class GNBControl : System.Web.UI.UserControl
    {
        protected string RootURL = string.Empty;
        protected string UserID = string.Empty;
        protected bool isAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserID = (Request["UserID"] ?? string.Empty).ToString();
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;

            //20130103 , 관리자 기능 관련
            gnbAdmin.Visible = false;
            
            UserInfo u = new UserInfo(this.Page);            
            //UserInfo uSub = ((SKT.Glossary.Web.MasterPages.Glossary)Page.Master).u;            

            // 2014-06-17 Mr.No
            //if (Request.Path.ToUpper() != "/MAINPAGE.ASPX" && Request.Path.ToUpper() != "/MAIN.ASPX" && string.IsNullOrEmpty(UserID))
            if (Request.Path.ToUpper() != "/MAINPAGE.ASPX" && Request.Path.ToUpper() != "/MAIN.ASPX" && string.IsNullOrEmpty(UserID))
            {
                switch (Request.Path.Split('/')[2].ToUpper())
                {
                    case "MYPROFILE.ASPX":
                        this.LnbClick.Attributes["Class"] = "user";
                        break;

                    case "MYDOCUMENTSLIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "archive";
                        break;

                    case "MYSCRAPLIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "scrap";
                        break;
                    case "MYTEMPLIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "temp";
                        break;
                    case "MYSHARELIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "share";
                        break;
                    case "MYFOLLOWLIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "follow";
                        break;
                    case "QNALIST.ASPX":
                        this.LnbClick.Attributes["Class"] = "qna";
                        break;
                    case "GLOSSARYSETUPPAGE.ASPX":
                        this.LnbClick.Attributes["Class"] = "setup";
                        break;
                    case "GLOSSARYADMINSTATTOTAL.ASPX":
                        this.LnbClick.Attributes["Class"] = "admin";
                        break;
                    default:
                        this.LnbClick.Attributes["Class"] = "";
                        break;
                }
            }
            else
            {
                this.LnbClick.Attributes["Class"] = "";
            }

            //차 후 각자 다른 처리를 할 수 있으므로 미리 분리해 놓음
            if (u.isAdmin)
            {
                AdminProcess();
            }
            if (u.isManager)
            {
                ManagerProcess();
            }
            if (u.isTiklei)
            {
                ManagerProcess();
            }

            
        }    
        protected void AdminProcess()
        {
            gnbAdmin.Visible = true;
        }
        protected void ManagerProcess()
        {
            gnbAdmin.Visible = true;
        }
        protected void TikleiProcess()
        {
            gnbAdmin.Visible = true;
        }
    }
}
