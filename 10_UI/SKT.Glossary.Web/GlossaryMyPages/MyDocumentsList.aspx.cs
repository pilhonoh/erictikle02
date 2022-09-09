using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Common;
using SKT.Glossary.Biz;
using System.Collections;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyDocumentsList : System.Web.UI.Page
    {
        int currentPageIndx;

        protected int iTotalCount;
        protected string DisplayTotalCount = string.Empty;

        //20131218 , F-003 count 표시 변경 요청
        protected int iWTikleCount;
        protected int iMTikleCount;
        protected int iTTikleCount;

        protected string DisplayATikleCount = string.Empty;
        protected string DisplayWTikleCount = string.Empty;
        protected string DisplayMTikleCount = string.Empty;
        protected string DisplayTTikleCount = string.Empty;

        protected string SearchKeyword = string.Empty;
        protected string ItemID = string.Empty;
        protected string UserName = string.Empty;
        protected string RootURL = string.Empty;
        protected string mode = string.Empty;
        protected string UserID = string.Empty;
        protected string HistoryYN = string.Empty;
        protected string SearchType = "MyWrite";
        protected string ReaderUserID = string.Empty;

        protected string WriteCount = string.Empty;
        protected string ModifyCount = string.Empty;
        protected string TempCount = string.Empty;
        protected bool bOwn = true;

        protected void Page_Load(object sender, EventArgs e)
        {

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            mode = (Request["mode"] ?? string.Empty).ToString();
            ReaderUserID = (Request["ReaderUserID"] ?? string.Empty).ToString();
            string Tabmenu = (Request["Tabmenu"] ?? string.Empty).ToString();

            UserInfo u = new UserInfo(this.Page);

            // 페이지 접근 계정 본인여부 체크
            if (ReaderUserID == string.Empty || u.UserID == ReaderUserID)
            {
                bOwn = true;
            }
            else
            {
                bOwn = false;
            }

            if (!IsPostBack)
            {
                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;


                if (string.IsNullOrEmpty(Tabmenu) == false) // 이값이 있으면 다른곳에서 온거임.
                {
                    hidMenuType.Value = Tabmenu;
                }
                //if (string.IsNullOrEmpty(ReaderUserID) == false) // 이값이 있으면 다른사용자를 본다는뜻이다.
                //{
                //    OtherUserBindSelect(ReaderUserID);
                //}
                //else
                //{
                    BindSelect();
                //}

            }
            TebMenuColor();
        }

        //다른 사용자 리스트
        private void OtherUserBindSelect(string UserID)
        {
            iTotalCount = 0;

            //20131218 , F-003 count 표시 변경 요청
            iWTikleCount = 0;
            iMTikleCount = 0;
            iTTikleCount = 0;

            GlossaryProfileBiz probiz = new GlossaryProfileBiz();
            ImpersonUserinfo u = probiz.UserSelect(UserID);
            UserName = u.Name;
            hidMenuType.Value = SearchType;

            MyTemp.Visible = false; //임시저장은 다른사람꺼 못봄.
            hdIsLoginUser.Value = "Y";

            GlossaryBiz biz = new GlossaryBiz();
            SearchType = "OtherMyWrite";    // 다른사람이 들어 갈때도 비공개로 작성한 것이 보여서 SP 쿼리 추가 후 타입명 추가 해 주었습니다.
            ArrayList list = biz.GlossaryMyDocumentsList(pager.CurrentIndex, pager.PageSize, out iTotalCount, out iWTikleCount, out iMTikleCount, out iTTikleCount, u.UserID, SearchType);
            pager.ItemCount = iTotalCount;

            //20131218 , F-003 count 표시 변경 요청
            DisplayTotalCount = (iTotalCount == 0) ? "0" : String.Format("{0:#,#}", iTotalCount);
            DisplayATikleCount = (iWTikleCount + iMTikleCount + iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount + iMTikleCount + iTTikleCount);
            DisplayWTikleCount = (iWTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount);
            DisplayMTikleCount = (iMTikleCount == 0) ? "0" : String.Format("{0:#,#}", iMTikleCount);
            DisplayTTikleCount = (iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iTTikleCount);

            //DisplayTotalCount = String.Format("{0:#,#}", iTotalCount);
            //if (DisplayTotalCount.Length == 0)
            //{
            //    DisplayTotalCount = "0";
            //}
            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();


        }

        //현제 사용자 리스트
        private void BindSelect()
        {
            iTotalCount = 0;

            //20131218 , F-003 count 표시 변경 요청
            iWTikleCount = 0;
            iMTikleCount = 0;
            iTTikleCount = 0;

            UserInfo u = new UserInfo(this.Page);
            hidMenuType.Value = SearchType;
            UserName = u.Name;
            UserID = u.UserID;
            GlossaryBiz biz = new GlossaryBiz();
            ArrayList list = biz.GlossaryMyDocumentsList(pager.CurrentIndex, pager.PageSize, out iTotalCount, out iWTikleCount, out iMTikleCount, out iTTikleCount, u.UserID, SearchType);
            pager.ItemCount = iTotalCount;

            //20131218 , F-003 count 표시 변경 요
            DisplayTotalCount = (iTotalCount == 0) ? "0" : String.Format("{0:#,#}", iTotalCount);
            DisplayATikleCount = (iWTikleCount + iMTikleCount + iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount + iMTikleCount + iTTikleCount);
            DisplayWTikleCount = (iWTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount);
            DisplayMTikleCount = (iMTikleCount == 0) ? "0" : String.Format("{0:#,#}", iMTikleCount);
            DisplayTTikleCount = (iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iTTikleCount);

            //if (DisplayTotalCount.Length == 0)
            //{
            //   DisplayTotalCount = "0";
            //}
            //if (SearchType == "MyWrite")
            //{
            WriteCount = iTotalCount.ToString();
            //}

            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
        }

        //페이지
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            SearchType = hidMenuType.Value;
            pager.CurrentIndex = currentPageIndx;
            if (!string.IsNullOrEmpty(ReaderUserID))
            {
                OtherUserBindSelect(ReaderUserID);
            }
            else
            {
                BindSelect();
            }
        }


        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal Num = (Literal)e.Item.FindControl("Num");
                Literal WikiClass = (Literal)e.Item.FindControl("ltWiki");
                Literal litPermission = (Literal)e.Item.FindControl("litPermission");
                // 2014-06-16 Mr.No
                GlossaryType glossaryType = (GlossaryType)e.Item.DataItem;
                Literal liticon = (Literal)e.Item.FindControl("liticon");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }

				//Literal litAlarmYN = (Literal)e.Item.FindControl("litAlarmYN");
				//Literal litEmail = (Literal)e.Item.FindControl("litEmail");
				//Literal litNote = (Literal)e.Item.FindControl("litNote");
				//Literal litName = (Literal)e.Item.FindControl("litName");
				//Literal litReply = (Literal)e.Item.FindControl("litReply");

				//if (hdIsLoginUser.Value == "Y")
				//{
				//	//litAlarmYN.Text = "비공개";
				//	litName.Text = SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(((GlossaryType)e.Item.DataItem).UserName));
				//}
				//else
				//{
				//	if (((GlossaryType)e.Item.DataItem).NoteYN == "Y")
				//	{
				//		litAlarmYN.Text = "<a id=\"alarm-view-10\" class=\"alarm-icon on\" onclick =\"fnAlarmOpen(this,'" + ((GlossaryType)e.Item.DataItem).CommonID + "');\">알림안함</a>";
				//	}
				//	else
				//	{
				//		litAlarmYN.Text = "<a id=\"alarm-view-10\" class=\"alarm-icon off\" onclick =\"fnAlarmOpen(this,'" + ((GlossaryType)e.Item.DataItem).CommonID + "');\">알림안함</a>";
				//	}
				//}

				//if (((GlossaryType)e.Item.DataItem).PrivateYN == "Y")
				//{
				//	//litName.Text = "비공개";
				//	litName.Text = SecurityHelper.Clear_XSS_CSRF(((GlossaryType)e.Item.DataItem).UserName);
				//}
				//else
				//{
				//	litName.Text = "<a href=\"javascript:fnMyProfileView('" + ((GlossaryType)e.Item.DataItem).UserID + "');\">" + ((GlossaryType)e.Item.DataItem).UserName + "/" + ((GlossaryType)e.Item.DataItem).DeptName + "</a>";
				//	// 2014-06-16 Mr.No
				//	//liticon.Text += "<img class=\"icon_img\" width='19' height='19' title='" + glossaryType.Rank + "' src='";
				//	//liticon.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + glossaryType.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "'/>";
				//}

                switch (((GlossaryType)e.Item.DataItem).Type)
                {
                    case "wiki":
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;

                    case "nateon":
                        WikiClass.Text = "<span class=\"nateon\">";
                        break;

                    case "email":
                        WikiClass.Text = "<span class=\"email\">";
                        break;
                    default:
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;
                }

				//if (!((GlossaryType)e.Item.DataItem).CommentCount.Equals("0"))
				//{
				//	if (((GlossaryType)e.Item.DataItem).NewCommentFlag)
				//	{
				//		litReply.Text = "<span class=\"reply new\">[" + ((GlossaryType)e.Item.DataItem).CommentCount + "]</span>";
				//	}
				//	else
				//	{
				//		litReply.Text = "<span class=\"reply\">[" + ((GlossaryType)e.Item.DataItem).CommentCount + "]</span>";
				//	}
				//}

                // 권한 아이콘
                if (!((GlossaryType)e.Item.DataItem).Permissions.Equals("FullPublic"))
                {
                    litPermission.Text = "<img src=\"/common/images/icon/icn_closed.png\" alt=\"비공개\" />";
                }
            }
        }

        //탭 메뉴
        protected void btnTebMenu_Click(object sender, EventArgs e)
        {
            SearchType = hidMenuType.Value;
            pager.CurrentIndex = 1;
            SearchKeyword = "";
            if (!string.IsNullOrEmpty(ReaderUserID))
            {
                OtherUserBindSelect(ReaderUserID);
            }
            else
            {
                BindSelect();
            }
        }

        //탭 메뉴 색
        private void TebMenuColor()
        {
            if (hidMenuType.Value == "MyWrite")
            {
                MyWrite.Style["Color"] = "red";
                MyModify.Style.Clear();
            }
            else
            {
                MyModify.Style["Color"] = "red";
                MyWrite.Style.Clear();

            }
        }
    }
}