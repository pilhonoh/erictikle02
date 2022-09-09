using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using System.Collections;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Data;
using System.Configuration;

namespace SKT.Glossary.Web.TikleAdmin.Platform
{
    public partial class PlatGlossaryList : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;

        protected string DisplayTotalCount = string.Empty;
        protected string MainType = string.Empty;
        protected string CategoryID = string.Empty;
        protected string RootURL = string.Empty;
        protected string Mode = string.Empty;
        protected string PageTitle = string.Empty;
        protected string UserID = string.Empty;
        protected string TagTitle = string.Empty;
        protected string SearchSort = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;
        protected string GatheringName;
        protected string GatheringAuthor = string.Empty;
        protected string GatheringCreationDate = string.Empty;

        protected List<CommonAuthType> glossaryAuthlist = new List<CommonAuthType>();

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            // 사용안함
            MainType = (Request["MainType"] ?? string.Empty).ToString();
            // 카테고리 별 조회 구분용
            CategoryID = (Request["CategoryID"] ?? string.Empty).ToString();
            TagTitle = ((Request["TagTitle"] == null || Request["TagTitle"] == string.Empty) ? string.Empty : HttpUtility.UrlDecode(Request["TagTitle"])).ToString();

            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            //Mode = (Request["Mode"] ?? string.Empty).ToString();

            // 권한처리(모임멤버검사)
            if (GatheringYN == "Y")
            {

                GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
                DataSet ds = gBiz.GlossaryGathering_MemberList(GatheringID);

                bool CheckResult = false;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (u.UserID == dr["EMPNO"].ToString())
                        {
                            CheckResult = true;
                        }
                    }
                }

                if (!CheckResult)
                {
                    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 페이지는 모임 멤버 분들만 보실 수 있습니다 ^^;");
                }
            }

            if (!IsPostBack)
            {
                //제목 페이지
                //pager.CurrentIndex = 1;
                // 2014-06-17 Mr.No   
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;

                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                PageTitle = "최근 티끌";
                BindSelect();
            }
        }


        // 통합 검색 리스트
        private void BindSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();

            // 카테고리 별 조회 및 전체 티끌 조회 구분
            if (!string.IsNullOrEmpty(CategoryID))
            {
                Titlelist = biz.TotalActivity(u.UserID, pager.PageSize, "Category", out iTotalCount, CategoryID, TagTitle, SearchSort, pager.CurrentIndex, GatheringYN, GatheringID);
            }
            else
            {
                Titlelist = biz.TotalActivity(u.UserID, pager.PageSize, "New", out iTotalCount, null, TagTitle, SearchSort, pager.CurrentIndex, GatheringYN, GatheringID);
            }

            for (int i = 0; i < Titlelist.Count; i++)
            {
                GlossaryType data = (GlossaryType)Titlelist[i];
                data.TagsInHtml = TagHtmlString(data.CommonID);
            }

            pager.ItemCount = iTotalCount;

            DisplayTotalCount = String.Format("{0:#,#}", iTotalCount);
            if (DisplayTotalCount.Length == 0)
            {
                DisplayTotalCount = "0";
            }

            rptInGeneral.DataSource = Titlelist;
            rptInGeneral.DataBind();

            //this.lbCount.InnerHtml = PagerTitle.ItemCount.ToString();
            //this.lbltitle.InnerHtml = PagerTitle.ItemCount.ToString();
        }

        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal WikiClass = (Literal)e.Item.FindControl("ltWiki");
                Literal Num = (Literal)e.Item.FindControl("Num");
                Literal litReply = (Literal)e.Item.FindControl("litReply");
                Literal litPermission = (Literal)e.Item.FindControl("litPermission");

                // 2014-06-16 Mr.No
                GlossaryType glossaryType = (GlossaryType)e.Item.DataItem;
                Literal litUserInfo = (Literal)e.Item.FindControl("litUserInfo");

                Literal PlatformYN = (Literal)e.Item.FindControl("PlatformYN");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }




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

                // 댓글
                if (!((GlossaryType)e.Item.DataItem).CommentCount.Equals("0"))
                {
                    if (((GlossaryType)e.Item.DataItem).NewCommentFlag)
                    {
                        litReply.Text = "<span>[" + ((GlossaryType)e.Item.DataItem).CommentCount + "]</span>";
                    }
                    else
                    {
                        litReply.Text = "<span>[" + ((GlossaryType)e.Item.DataItem).CommentCount + "]</span>";
                    }
                }

                // 권한 아이콘
                if (!((GlossaryType)e.Item.DataItem).Permissions.Equals("FullPublic"))
                {
                    if (GatheringYN != "Y")
                    {
                        litPermission.Text = "<img src=\"/common/images/icon/icn_closed.png\" alt=\"비밀글\" alt=\"비밀글\" />";
                    }

                }

                // 2014-06-16 Mr.No
                if (((GlossaryType)e.Item.DataItem).PrivateYN.Equals("N"))
                {
                    litUserInfo.Text = "<a href='javascript:fnProfileView(\"" + glossaryType.UserID + "\");' class='Atag'>";
                    litUserInfo.Text += glossaryType.UserName + "/" + glossaryType.DeptName + "</a>";
                    /*
                     * 2014-08-12 Rank제외
                    litUserInfo.Text += "<img class=\"icon_img\" title=\"" + glossaryType.Rank + "\" width=\"19\" height=\"19\" src=\"";
                    litUserInfo.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + glossaryType.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "\"/>";
                    */

                }
                else
                {
                    litUserInfo.Text = SecurityHelper.Clear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(glossaryType.UserName));
                }

                PlatformYN.Text = ((GlossaryType)e.Item.DataItem).PlatformYN == "Y" ? "해당" : "미해당";
            }
        }
        protected string GetTitle()
        {
            return Server.UrlEncode(Title);
        }

        protected string TagHtmlString(string CommonID)
        {
            //<a href="#">가나다</a>, <a href="#">라라라</a>

            string ret = string.Empty;

            GlossaryBiz biz = new GlossaryBiz();
            ArrayList list = biz.GetTagList(CommonID);

            for (int i = 0; i < list.Count; i++)
            {
                GlossaryTagType data = (GlossaryTagType)list[i];
                if (i == 0)
                {
                    ret = "<span class=\"tag\">태그 :<a href=\"javascript:fnGoView('" + data.CommonID + "')  \">" + data.TagTitle + "</a>";
                }
                else
                {
                    ret = ret + ",<a href=\"javascript:fnGoView('" + data.CommonID + "')  \">" + data.TagTitle + "</a>";
                }
            }
            if (list.Count > 0)
            {
                ret = ret + "</span>";
            }

            return ret;
        }
        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            BindSelect();
        }

        protected void btn_Platform_update_Click(object sender, EventArgs e)
        {
            if (hdd_PlatformMoveID.Value.Length > 0)
            {
                GlossaryBiz biz = new GlossaryBiz();
                int result = biz.Platformupdate(hdd_PlatformMoveID.Value);
                Response.Redirect("/tikleAdmin/Platform/PlatGlossaryList.aspx");
            }
        }
    }
}