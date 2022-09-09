using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Common;

namespace SKT.Glossary.Web
{
    public partial class Error : System.Web.UI.Page
    {
        public string currentTime = null;
        protected string RootURL = string.Empty;
        protected string Message = string.Empty;
        protected string ErrCode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //litErrorMessage.Text = "알수없는 에러...";
            //InitializeCultureHelper.Initialize();
            
            //보안 조치
            UserInfo u = new UserInfo(this.Page);

            currentTime = DateTime.Now.ToString();
            string IsTestServer = ConfigurationManager.AppSettings["IsTestServer"];
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            Message = Request["Message"] ?? string.Empty;

            if (Request["ErrCode"] == null || Request["ErrCode"].ToString() == "")
            {
                ErrCode = string.Empty;
            }
            else
            {
                ErrCode = Request["ErrCode"].ToString();
            }

            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    ViewState["PreviousPage"] = HttpContext.Current.Request.UrlReferrer.ToString();
                }
            }
            
            if(HttpContext.Current.Request.UrlReferrer !=null)
            {
                litPreUrl.Text = ViewState["PreviousPage"].ToString();
            }

            if (!string.IsNullOrEmpty(Request["err"]))
            {
                //보안 조치
                if (u.isAdmin)
                {
                    litErrorMessage.Text = Request["err"].ToString();
                    if (!string.IsNullOrEmpty(Request["errAll"]))
                    {
                        litErrorDetail.Text = Request["errAll"].ToString();
                    }
                }
                else
                {
                    litErrorMessage.Text = ViewState["PreviousPage"].ToString();
                }
            }

            /* 불필요
            if (IsTestServer == "Y")
            {
                if (!string.IsNullOrEmpty(Request["errAll"]))
                {
                    litErrorDetail.Text = Request["errAll"].ToString();
                }
            }        
            */

            string InfoMessage = (Request["InfoMessage"] ?? string.Empty).ToString();   //이메세지가 잇으면 에러가 아니라 화면에 뿌려준다.

            if (ErrCode.Equals("99999"))
            {
                ErrCode = "100";
                InfoMessage = "사내 시스템 접속을 위한 인증 정보를 찾을 수 없습니다. <br /><br /><font style='font-size:12px;'>정상적인 시스템 사용을 위해서 홈으로 이동해주시기 바랍니다.</font>";
            }
           
            if (InfoMessage != string.Empty)
            {
                //if (u.isAdmin)
                //{
                //    litUserDetail.Text = "<h3>" + InfoMessage + "</h3>";
                //}else
                //{
                //    litUserDetail.Text = " 관리자에게 문의 주십시오.";
                //}
                litUserDetail.Text = "<h3>" + InfoMessage + "</h3>";
            }

            // 2014-05-26 Mr.No
            if (!String.IsNullOrEmpty(Message)) { litErrorMessage.Text = "<span style='color:black;font-size:20px;font-weight:bold;'>" + Message + "</span>"; }
        }
    }
}