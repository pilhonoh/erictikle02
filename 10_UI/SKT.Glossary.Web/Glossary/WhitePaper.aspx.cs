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
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Glossary
{
    public partial class WhitePaper : System.Web.UI.Page
    {
        protected int currentPageIndx = 1;
        protected int iTotalCount;

        protected UserInfo u;
        protected string DisplayTotalCount = string.Empty;
        protected string MainType = string.Empty;
        protected string CategoryID = string.Empty;
        protected string RootURL = string.Empty;
        protected string Mode = string.Empty;
        protected string PageTitle = string.Empty;
        protected string UserID = string.Empty;
        protected string TagTitle = string.Empty;
        protected string SearchSort = string.Empty;
        protected string TType = string.Empty;
        protected string SchText = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            TType = (Request["TType"] ?? string.Empty).ToString();
            SchText = (string.IsNullOrEmpty(Request["SchText"]) ? string.Empty : HttpUtility.UrlDecode(Request["SchText"])).ToString();

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            // 타입이 없을 경우 기본값
            if (string.IsNullOrEmpty(TType))
            {
                TType = "A";
            }

            if (!IsPostBack)
            {
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;

                pager.PageSize = 10;
                PageTitle = "티끌";

                if (!string.IsNullOrEmpty(SchText))
                {
                    this.hidSearchText.Value = SchText;
                }

                BindSelect();
                BindHitsSelect();
            }
        }

        // CHG610000074852 / 20181108 / T생활백서
        private void BindSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();

            Titlelist = biz.TotalActivityNew("TW", this.TType, u.UserID, pager.PageSize, "NewTW", out iTotalCount, this.SearchSort, this.hidSearchText.Value, pager.CurrentIndex);

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

        }
        protected void rptInGeneral1_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal Num = (Literal)e.Item.FindControl("Num");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }
            }
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
                    litPermission.Text = "<img src=\"/common/images/icon/icn_closed.png\" alt=\"비밀글\" alt=\"비밀글\" />";
                }

                // 2014-06-16 Mr.No
                if (((GlossaryType)e.Item.DataItem).PrivateYN.Equals("N"))
                {
                    //litUserInfo.Text = "<a href='javascript:fnProfileView(\"" + glossaryType.UserID + "\");'>";
                    //litUserInfo.Text += glossaryType.UserName + "/" + glossaryType.DeptName + "</a>";
                    litUserInfo.Text += glossaryType.UserName + "/" + glossaryType.DeptName;
                }
                else
                {
                    litUserInfo.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(SKT.Common.SecurityHelper.Add_XSS_CSRF(glossaryType.UserName));
                }
            }
        }

        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;

            BindSelect();

        }

        private void BindHitsSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();
            Titlelist = biz.TotalActivityNew("TW", "", u.UserID, 20, "HitsTW", out iTotalCount, this.SearchSort, "", 1);

            rptHits.DataSource = Titlelist;
            rptHits.DataBind();

            Titlelist = new ArrayList();
            Titlelist = biz.TotalActivityNew("TW", "", u.UserID, 20, "LikesTW", out iTotalCount, this.SearchSort, "", 1);
            rptLike.DataSource = Titlelist;
            rptLike.DataBind();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pager.CurrentIndex = 1;
            this.BindSelect();
        }
    }
}