using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SKT.Common;
using System.Configuration;

namespace SKT.Glossary.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected string RootURL = string.Empty;

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;

            //1. 최근 Exception 조회e
            Exception serverEx = Server.GetLastError().GetBaseException();
            Server.ClearError();
            //2. 로그 기록
            //2-1. 파일 로그

            /*
            Author : 개발자-장찬우G, 리뷰자-이정선G 
            Create Date : 2016.02.17 
            Desc : error시 detail한 로그포맷 생성
            */

            string errString = "errMESSAGE: " + serverEx.Message;
            errString += "\r\nSOURCE: " + serverEx.Source;
            errString += "\r\nHeaders[SM_USER]: " + System.Web.HttpContext.Current.Request.Headers["SM_USER"];
            errString += "\r\nCookies[SM_USER]: " + System.Web.HttpContext.Current.Request.Cookies["SM_USER"].Value;
            if (System.Web.HttpContext.Current.Session != null)
            {
                errString += "\r\nSession.IsNewSession: " + System.Web.HttpContext.Current.Session.IsNewSession.ToString();
                errString += "\r\nSession[UserID]: " + System.Web.HttpContext.Current.Session["UserID"].ToString();
            }
            errString += "\r\nRequest.Url: " + Request.Url.ToString();
            errString += "\r\nREMOTE_ADDR: " + Request.ServerVariables.Get("REMOTE_ADDR");
            errString += "\r\nTARGETSITE: " + serverEx.TargetSite.ToString();
            errString += "\r\nSTACKTRACE: " + serverEx.StackTrace.ToString();

            //Log4NetHelper.Error(serverEx.Message, serverEx);
            Log4NetHelper.Error(errString, serverEx);

            //2-2. DB 로그
            //DBLogHelper.WriteErrorLog("Exception", serverEx.Message, Server.MachineName, string.Empty, string.Empty, Request.Url.ToString(), GetUserIP(), Request.Browser.Capabilities[""].ToString());
            //DBLogHelper.ExceptionLogging(serverEx.Message, serverEx.Source, serverEx.StackTrace, serverEx.TargetSite.Name, GetUserID(), Request.Url.ToString(), GetUserIP(), Request.Browser.Capabilities[""].ToString());

            //3. 오류페이지로 리다이렉트           
            Dictionary<string, string> errObj = new Dictionary<string, string>();
            errObj.Add("err", serverEx.Message);
            errObj.Add("errAll", serverEx.ToString());
            //RedirectWithPost(Response, "/" + RootURL + "/Error.aspx", errObj);

            RedirectWithPost(Response, "/Error.aspx", errObj);     
        }

        /// <summary>특정 페이지로 리다이렉트, 변수를 POST방식으로 전달 가능</summary>
        public static void RedirectWithPost(System.Web.HttpResponse rsp, string url, Dictionary<string, string> parameters)
        {
            SendPostForm spf = new SendPostForm(url, "POST");
            if (parameters != null && parameters.Count > 0)
                foreach (KeyValuePair<string, string> param in parameters)
                    spf.Add(param.Key, param.Value);
            string rhtml = spf.MakeForm();

            // 에러 발생 시, 에러창으로 갈지 에러메세지를 띄울지 판단 작업 추가    Mostisoft 2015.08.21
            if (System.Configuration.ConfigurationManager.AppSettings["IsLocalErrorMessage"] != null &&
                System.Configuration.ConfigurationManager.AppSettings["IsLocalErrorMessage"] == "true")
            {
                foreach (KeyValuePair<string, string> param in parameters)
                {
                    rsp.Write(string.Format("key {0}<br/>msg {1}<br/>", param.Key, param.Value));
                    // spf.Add(param.Key, param.Value);
                }
                rsp.End();
            }
            else
            {
                rsp.Clear();
                rsp.Write(rhtml);
                rsp.End();
            }
        }

        class SendPostForm
        {
            private string msubmiturl;
            private string mMethod;
            private System.Collections.Hashtable mHash = new System.Collections.Hashtable();

            public string SubmitURL        // 프로퍼티 이동할 페이지 URL
            {
                get { return msubmiturl; }
                set { msubmiturl = value; }
            }

            public void Add(string key, string value)  // 포스트로 전송할 값 추가
            {
                mHash.Add(key, value);
            }

            public void Clear()
            {
                mHash.Clear();
            }

            public void Remove(string key)
            {
                mHash.Remove(key);
            }

            public int Count
            {
                get
                {
                    return mHash.Count;
                }
            }

            public SendPostForm()
            {
            }
            public SendPostForm(string rurl)
            {
                msubmiturl = rurl;
            }
            public SendPostForm(string rurl, string method)    //rurl - URL, method - POST, GET 등
            {
                msubmiturl = rurl;
                this.mMethod = method;
            }
            public string MakeForm()    // HTML을 생성 해서 string값으로 돌려줌.
            {
                string strForm = null;
                if (msubmiturl == null) return null;
                if (mMethod == null) mMethod = "POST";

                strForm = "<HTML><HEAD><title>Form</title><META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-\"></HEAD>";
                strForm += "<body onload=\"javascript:document.frm.submit()\"><form name=frm method=" + mMethod + " action=\"" + msubmiturl + "\">";
                foreach (System.Collections.DictionaryEntry de in mHash)    //-- Post나 GET으로 전송할 키와 값을 생성
                    strForm += "<input type=\"hidden\" name=\"" + (string)de.Key + "\" value=\"" + (string)de.Value + "\">";
                strForm += "</form></body></HTML>";
                return strForm;
            }
        }
        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        private string GetUserID()
        {
            string UserID = string.Empty;
            //HTTP Context
            //{
            //    UserID = System.Web.HttpContext.Current.User.Identity.Name;//string.Empty;
            //    if (UserID.Contains("\\\\"))
            //    {
            //        UserID = UserID.Split(new string[] { "\\\\" }, StringSplitOptions.None)[1];
            //    }
            //    else if (UserID.Contains("\\"))
            //    {
            //        UserID = UserID.Split(new string[] { "\\" }, StringSplitOptions.None)[1];
            //    }
            //    else { }
            //}
            ////SSO
            //{
            //    UserID = Request["SM_USER"];
            //}
            
            UserID = System.Web.HttpContext.Current.Request.Headers["SM_USER"];

            return UserID;
        }
    }
}