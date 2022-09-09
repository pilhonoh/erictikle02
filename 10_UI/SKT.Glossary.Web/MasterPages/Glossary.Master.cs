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
    public partial class Glossary : System.Web.UI.MasterPage
    {
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string UserID = string.Empty;
        protected string MyMenuUserName = string.Empty;
        public bool isAdminValue = false;

        protected string selectedCategoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();

            SearchKeyword = SearchKeyword.Replace("'", "\\\'").Replace("\"", "\\\"");

            UserInfo u = new UserInfo(this.Page);

            MyMenuUserName = u.Name + " / " + u.DeptName;
            //this.UserName.InnerText = u.Name + " / " + u.DeptName;
            //this.imgFace.ImageUrl = u.PhotoUrl;
            UserID = u.UserID;

            string tempuserid = u.UserID;

            if (u.isIdOnlyNum == false) //숫자 사번이 아니면 팅김.
            {
                if (u.isEuser == false && u.isAdmin == false && u.isTiklei == false)  //접속 불가능한 ID 예외처리.
                {
                    string infomsg = "이것은 에러메세지가 아닙니다 이화면을 보신분은 접속가능한 사용자가 아닙니다.";
                    Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
                }
            }

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

            // 카테고리 & 부문
            //CategoryListBind(u.UserID);

            if (!(u.isManager || u.isAdmin || u.isTiklei || u.isEuser))
            {
                string str = "<script language=javaScript>RemoveManagerMenu();</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", str, false);
            }
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

        /*
        /// <summary>
        /// 사용자에 따른 카테고리 & 부문 정보
        /// </summary>
        public void CategoryListBind(string UserID)
        {
            
            // 사용자에 따른 카테고리 & 부문 정보
            GlossaryCategoryBiz biz = new GlossaryCategoryBiz();
            List<GlossaryCategoryType> glossaryCategoryType = biz.GlossaryCategory_Main_User_List(UserID.ToString());

            rptCategoryGNB.DataSource = glossaryCategoryType;

            rptCategoryGNB.DataBind();
        }
        */

        /*
        /// <summary>
        /// 카테고리&부문 ItemDataBound
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptCategoryGNB_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GlossaryCategoryType glossaryCategoryType = (GlossaryCategoryType)e.Item.DataItem;
                Literal litCategory = (Literal)e.Item.FindControl("litCategory");

                int index = e.Item.ItemIndex + 1;
                litCategory.Text = "<li><a href=\"/Glossary/GlossaryNewsList.aspx?CategoryID=" + glossaryCategoryType.ID + "&PrevListUrl=" + HttpUtility.UrlEncode("/Glossary/GlossaryNewsList.aspx?CategoryID=" + glossaryCategoryType.ID);
                //litCategory.Text = "<li><a href=\"javascript:fnGoNewsList('" + glossaryCategoryType.ID + "')";
                if (index < 10)
                {
                    litCategory.Text += "\" class=\"tM0" + index;
                }
                else if (index >= 13)
                {
                    litCategory.Text += "\" class=\"tM13";
                }
                else
                {
                    litCategory.Text += "\" class=\"tM" + index;
                }
                litCategory.Text += "\">" + glossaryCategoryType.CategoryTitle + "</a></li>";
                
                
            }
        }
        */
    }
}