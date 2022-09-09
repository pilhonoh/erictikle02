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

namespace SKT.Glossary.Web.TikleAdmin.Platform
{
    public partial class PlatQnaList : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;
        protected int iSuccessCount;
        protected int iUnSuccessCount;
        protected int iTotal;
        protected int iMyQnA;
        protected string iiSuccessCount;
        protected string iiUnSuccessCount;
        protected string iiTotal;
        protected string iiMyQnA;
        protected string SearchKeyword = string.Empty;
        protected string SearchSort = string.Empty;
        protected string SearchSortGubun = string.Empty;


        protected string ItemID = string.Empty;
        protected string UserName = string.Empty;
        protected string RootURL = string.Empty;
        protected string mode = string.Empty;
        protected string HistoryYN = string.Empty;
        protected string SearchType = "Total";
        protected static bool CountYN = true;

        protected string TutorialYN = string.Empty;
        protected string qnaMode = "List";

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            mode = (Request["mode"] ?? string.Empty).ToString();
            SearchSort = (Request["SearchSort"] ?? string.Empty).ToString();
            SearchSortGubun = (Request["SearchSortGubun"] ?? string.Empty).ToString();

            TutorialCheck();
            if (!IsPostBack)
            {
                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;
                BindSelect();
            }
        }

        private void TutorialCheck()
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryControlBiz biz = new GlossaryControlBiz();
            TutorialInfo data = biz.TutorialSelect(u.UserID);

            if (data.QNAYN == "N")
            {
                TutorialYN = "N";
            }
            else   //데이타가 없으면 무조건 보여줌.
            {
                TutorialYN = "Y";
            }

        }

        //글 목록
        private void BindSelect()
        {
            iTotalCount = 0;
            UserInfo u = new UserInfo(this.Page);
            UserName = u.Name;
            hidMenuType.Value = SearchType;
            GlossaryQnABiz biz = new GlossaryQnABiz();
            ArrayList list = biz.GlossaryQnAList(pager.CurrentIndex, pager.PageSize, qnaMode, out iTotalCount, out iTotal, out iSuccessCount, out iUnSuccessCount, SearchKeyword, SearchType, u.UserID, out iMyQnA, SearchSort, SearchSortGubun);
            //if (CountYN == true)
            {
                iiSuccessCount = String.Format("{0:#,#}", iSuccessCount);
                if (iiSuccessCount.Length == 0)
                {
                    iiSuccessCount = "0";
                }
                iiUnSuccessCount = String.Format("{0:#,#}", iUnSuccessCount);
                if (iiUnSuccessCount.Length == 0)
                {
                    iiUnSuccessCount = "0";
                }
                iiTotal = String.Format("{0:#,#}", iTotal);
                if (iiTotal.Length == 0)
                {
                    iiTotal = "0";
                }
                iiMyQnA = String.Format("{0:#,#}", iMyQnA);
                if (iiMyQnA.Length == 0)
                {
                    iiMyQnA = "0";
                }

                CountYN = false;
            }
            //if (SearchType =="Total")
            //    CountYN = true;

            pager.ItemCount = iTotalCount;

            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
        }
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            SearchType = hidMenuType.Value;
            BindSelect();
        }

        protected void rptIn_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            //GlossaryQnAType pp = new GlossaryQnAType();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string CommentHitscount = ((GlossaryQnAType)e.Item.DataItem).CommentHits;
                Literal Success = (Literal)e.Item.FindControl("lbSuccess");
                Literal Title = (Literal)e.Item.FindControl("litQnaTitle");
                // 2014-06-16 Mr.No
                Literal litUserInfo = (Literal)e.Item.FindControl("litUserInfo");
                Literal PlatformYN = (Literal)e.Item.FindControl("PlatformYN");

                GlossaryQnAType data = ((GlossaryQnAType)e.Item.DataItem);

                if (data.CommentHits == "0")
                {
                    Success.Text = "<span class=\"btn10\"><span><b>미답변</b></span></span>";
                }
                else if (data.BestReplyYN == "Y")
                {
                    Success.Text = "<span class=\"btn9\"><span>채택완료<b>(" + CommentHitscount + ")</b></span></span>";
                }
                else
                {
                    Success.Text = "<span class=\"btn8\"><span>답변완료<b>(" + CommentHitscount + ")</b></span></span>";
                }


                if (data.CommonID != "" && data.CommonID != null)
                {
                    Title.Text = "<a href=\"javascript:fnMyQnAView('"
                                    + ((GlossaryQnAType)e.Item.DataItem).ID
                                    + "')\"  class=\"Atag\">"
                                    + SecurityHelper.ReClear_XSS_CSRF(((GlossaryQnAType)e.Item.DataItem).Title)
                                    + "</a>";
                    Success.Text = "<span class=\"tikletag red\">티끌화 완료</span>";
                }
                else
                {
                    Title.Text = "<a href=\"javascript:fnMyQnAView('"
                                        + ((GlossaryQnAType)e.Item.DataItem).ID
                                        + "')\"  class=\"Atag\">"
                                        + SecurityHelper.ReClear_XSS_CSRF(((GlossaryQnAType)e.Item.DataItem).Title)
                                        + "</a>";
                }

                Literal Num = (Literal)e.Item.FindControl("Num");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }

                // 2014-06-16 Mr.No
                if (!data.ItemState.Equals("1"))
                {

                    litUserInfo.Text = "<a href='javascript:fnProfileView(\"" + data.UserID + "\");'  class=\"Atag\">";
                    litUserInfo.Text += data.UserName + "/" + data.DeptName + "</a>";
                    /*
                     * Rank 제외
                    string Rank = string.Empty;
                    if (data.Grade == 0) { Rank = "지존"; }
                    else if (data.Grade == 1) { Rank = "고수"; }
                    else if (data.Grade == 2) { Rank = "중수"; }
                    else { Rank = "초수"; }

                    litUserInfo.Text += "<img class='icon_img' width='19' height='19' title='" + Rank + "' src='";
                    litUserInfo.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + data.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "'/>";
                    */
                }
                else
                {
                    litUserInfo.Text = SecurityHelper.Clear_XSS_CSRF(data.UserName);
                }
                PlatformYN.Text = ((GlossaryQnAType)e.Item.DataItem).PlatformYN == "Y" ? "해당" : "미해당";
            }
        }

        protected void btn_Platform_update_Click(object sender, EventArgs e)
        {
            if (hdd_PlatformMoveID.Value.Length > 0)
            {
                GlossaryQnABiz biz = new GlossaryQnABiz();
                int result = biz.PlatformQnAUpdate(hdd_PlatformMoveID.Value);

                    Response.Redirect("/tikleAdmin/Platform/PlatQnaList.aspx");               
            }
        }
    }
}