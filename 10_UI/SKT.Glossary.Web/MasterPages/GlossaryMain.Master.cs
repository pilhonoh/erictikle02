using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using System.Web;

namespace SKT.Glossary.Web.MasterPages
{
    public partial class GlossaryMain : System.Web.UI.MasterPage
    {
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string UserID = string.Empty;
        protected string MyMenuUserName = string.Empty;
        public bool isAdminValue = false;
        public bool isAdminMenu = false;
        protected string selectedCategoryId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            this.SearchKeyword = (base.Request["SearchKeyword"] ?? string.Empty).ToString();
            this.SearchKeyword = this.SearchKeyword.Replace("'", "\\'").Replace("\"", "\\\"");
            UserInfo u = new UserInfo(this.Page);

            MyMenuUserName = "<b>" + u.Name + "</b><span>" + u.DeptName + "</span>";
            UserID = u.UserID;

            if (u.isAdmin == true || u.isTiklei == true)
            {
                this.isAdminMenu = true;
            }

            //if (!u.isIdOnlyNum)
            //{
            //    if (u.isEuser == false && u.isAdmin == false && u.isTiklei == false)  //접속 불가능한 ID 예외처리
            //    {
            //        string str = "이화면을 보신분은 접속가능한 사용자가 아닙니다.";
            //        base.Response.Redirect("/Error.aspx?InfoMessage=" + str, false);
            //        Response.End();
            //    }
            //}

            string iscbt = ConfigurationManager.AppSettings["IsCBTUserCheck"] ?? string.Empty;
            if (iscbt == "Y")
            {
                GlossaryBiz biz = new GlossaryBiz();
                ArrayList list = biz.GetCBTUserList(UserID);

                //string infomsg = "CBT 기간에 등록된 사용자가 아닙니다 이것은 에러메세지가 아닙니다";
                string infomsg = "이것은 에러메세지가 아닙니다 CBT 기간이 종료 되었습니다 조금더 멋진 티끌이 되어 돌아오겠습니다 ^^";

                if (list.Count == 0)
                {
                    Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
                    Response.End();
                }
            }
            string IsEventadd = ConfigurationManager.AppSettings["IsEventadd"] ?? string.Empty;
            if (IsEventadd == "Y")
            {

                GlossaryPageRequestType gprt = new GlossaryPageRequestType();

                gprt.UserID = string.Empty;
                gprt.Name = string.Empty;
                gprt.SessionID = string.Empty;
                gprt.UrlBefore = string.Empty;
                gprt.UrlCurrent = string.Empty;
                gprt.PathCurrent = string.Empty;

                try
                {
                    gprt.UserID = u.UserID;
                    gprt.Name = u.Name;
                    gprt.SessionID = Session.SessionID.ToString();
                    gprt.UrlBefore = Request.UrlReferrer.ToString();
                    gprt.UrlCurrent = Request.Url.ToString();
                    gprt.PathCurrent = Request.Url.LocalPath.ToString();
                }
                catch { }

                GlossaryDac dac = new GlossaryDac();
                dac.InsertEventAttendance(gprt);

            }
            SelectedId();

            this.serverip.InnerText = base.Server.MachineName + ":" + base.Request.ServerVariables["LOCAL_ADDR"];
        }

        protected void SelectedId()
        {
            List<GlossaryCategoryType> listGlossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategorySelectAllAdmin();

            foreach (GlossaryCategoryType item in listGlossaryCategoryType)
            {
                if (item.UseYN == "Y")
                    selectedCategoryId = item.ID.ToString();
            }

            if (string.IsNullOrEmpty(selectedCategoryId))
                selectedCategoryId = Convert.ToString(listGlossaryCategoryType[0].ID);
        }
    }
}
