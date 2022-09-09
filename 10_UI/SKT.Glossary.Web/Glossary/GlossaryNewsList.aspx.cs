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
    public partial class GlossaryNewsList : System.Web.UI.Page
    {

        protected int currentPageIndx = 1;
        protected int iTotalCount;
        public int iTotalCountGatheringList;
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

            //CHG610000063658 / 2018-05-17 / 최현미 / SSO 연동관련 오류 수정 요청
            if (TagTitle.IndexOf("-$") > -1)
                TagTitle = TagTitle.Replace("-$", "&");
            if (TagTitle.IndexOf("- ") > -1)
                TagTitle = TagTitle.Replace("- ", " ");
            
            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            UserInfo u = new UserInfo(this.Page);

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            UserID = u.UserID;

            //Mode = (Request["Mode"] ?? string.Empty).ToString();

            // 권한처리(모임멤버검사)
            //if (!String.IsNullOrEmpty(GatheringID))
            //{
            //    //GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            //    //DataSet ds = gBiz.GlossaryGathering_MemberList(GatheringID);

            //    //bool CheckResult = false;
            //    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    //{
            //    //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    //    {
            //    //        if (u.UserID.ToUpper() == dr["EMPNO"].ToString().ToUpper())
            //    //        {
            //    //            CheckResult = true;
            //    //        }
            //    //    }
            //    //}

            //    //if (!CheckResult)
            //    //{
            //    //    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 페이지는 모임 멤버 분들만 보실 수 있습니다 ^^;");
            //    //}
            //    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "모임 게시글은 관리자에게 문의 바랍니다.", true);
            //}

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

                //SearchKeyword = string.Empty;
               

                BindSelect();

                BindHitsSelect();

                BindLikeSelect();

                // 모임 정보 조회
                GatheringUserListBind();
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

            // 사용안함
            //if (MainType == "Best") //이기능은 다른페이지로 변경...ㅜㅜ
            //{
            //    PageTitle = "명예의 전당";
            //    Titlelist = biz.TotalActivity(pager.PageSize, "Best", out iTotalCount, pager.CurrentIndex);
            //}
            //else
            //{
            //    Titlelist = biz.TotalActivity(pager.PageSize, "New", out iTotalCount, pager.CurrentIndex);
            //}

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

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 끌모임 검색 기능 추가
        private void SearchBindSelect(string keyword)
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();

            Titlelist = biz.TotalActivity_GathringSearch(u.UserID, pager.PageSize, "New", out iTotalCount, null, TagTitle, SearchSort,keyword, pager.CurrentIndex, GatheringYN, GatheringID);

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

            iTotalCountGatheringList = Titlelist.Count;
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
                    if (GatheringYN != "Y")
                    {
                        litPermission.Text = "<img src=\"/common/images/icon/icn_closed.png\" alt=\"비밀글\" alt=\"비밀글\" />";
                    }

                }

                // 2014-06-16 Mr.No
                if (((GlossaryType)e.Item.DataItem).PrivateYN.Equals("N"))
                {
                    //litUserInfo.Text = "<a href='javascript:fnProfileView(\"" + glossaryType.UserID + "\");'>";
                    //litUserInfo.Text += glossaryType.UserName + "/" + glossaryType.DeptName + "</a>";

                    litUserInfo.Text = glossaryType.UserName + "/" + glossaryType.DeptName;

                    /*
                     * 2014-08-12 Rank제외
                    litUserInfo.Text += "<img class=\"icon_img\" title=\"" + glossaryType.Rank + "\" width=\"19\" height=\"19\" src=\"";
                    litUserInfo.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + glossaryType.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "\"/>";
                    */

                }
                else
                {
                    litUserInfo.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(SKT.Common.SecurityHelper.Add_XSS_CSRF(glossaryType.UserName));
                }
            }
        }

        // 인기조회순
        private void BindHitsSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();
            Titlelist = biz.TotalActivity(u.UserID, 10, "Hits", out iTotalCount, CategoryID, TagTitle, SearchSort, 1, GatheringYN, GatheringID);

            rptHits.DataSource = Titlelist;
            rptHits.DataBind();
        }

        protected void rptHits_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }

        // 인기추천수
        private void BindLikeSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            iTotalCount = 0;
            GlossaryMainBiz biz = new GlossaryMainBiz();

            ArrayList Titlelist = new ArrayList();
            Titlelist = biz.TotalActivity(u.UserID, 10, "Like", out iTotalCount, CategoryID, TagTitle, SearchSort, 1, GatheringYN, GatheringID);

            rptLike.DataSource = Titlelist;
            rptLike.DataBind();
        }

        protected void rptLike_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }


        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            this.hdMore.Value = this.hdMore.Value;

            if (string.IsNullOrEmpty(hid_SearchKeyword.Value))
            {
                BindSelect();
            }
            else
            {
                SearchBindSelect(hid_SearchKeyword.Value);
            }
        }



        //검색 결과
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //SearchKeyword = this.question.Value;

            
            pager.CurrentIndex = 1;
            BindSelect();
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

        /// <summary>
        /// 모임 사용자 조회  
        /// </summary>
        public void GatheringUserListBind()
        {


            if (GatheringYN == "Y")
            {
                GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();
                DataSet ds = new DataSet();

                string DirectoryType = "Gathering";

                ds = biz.MyGroupListSelect2(UserID, GatheringID, DirectoryType, GatheringID);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //result.Add(dr["Title"].ToString());
                        //result.Add("/Main.aspx");  //임시 코드 메인 코드로 넘김.
                        //result.Add(dr["linkurl"].ToString());

                        CommonAuthType temp = new CommonAuthType();
                        temp.AuthID = dr["ToUserID"].ToString();
                        temp.AuthName = dr["ToUserName"].ToString();
                        temp.AuthType = dr["ToUserType"].ToString();
                        temp.RegID = dr["REG_ID"].ToString();
                        temp.RegDTM = Convert.ToDateTime(dr["REG_DTM"]);
                        temp.DeptNumber = dr["DeptNumber"].ToString();

                        if (temp.AuthType == "G")
                        {
                            temp.AuthName = "[그룹]" + temp.AuthName;
                        }


                        glossaryAuthlist.Add(temp);
                    }
                }

                //2016-12-01 모임 매니저 버튼 보여짐
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        if (dr["ToUserID"].ToString() == UserID)
                        {
                            p_gatheringsetting.Visible = true;
                        }
                    }
                }

                rptGatheringUser.DataSource = glossaryAuthlist;
                rptGatheringUser.DataBind();
            }
        }

        /// <summary>
        /// 모임 사용자 정보
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptGatheringUser_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CommonAuthType glossaryAuthType = (CommonAuthType)e.Item.DataItem;
                Literal litGatheringUser = (Literal)e.Item.FindControl("litGatheringUser");

                litGatheringUser.Text = "";

                if (glossaryAuthType.AuthID == glossaryAuthType.RegID)
                {
                    lblCreater.Text = glossaryAuthType.AuthName;
                    lblCreateDate.Text = glossaryAuthType.RegDTM.ToLongDateString();

                    litGatheringUser.Text += "<li class=\"super\"><span>" + glossaryAuthType.AuthName + "</span></li>";
                }
                else
                {
                    litGatheringUser.Text += "<li>" + glossaryAuthType.AuthName + "</li>";
                }


                //if (glossaryAuthType.RegID == UserID)
                //{
                //    litGatheringUser.Text += "<p><a href=\"javascript:viewDivShow();\"><img src=\"/common/images/icon/setting.png\" alt=\"\" title=\"폴더사용자 관리 바로가기\" /></a></p>";
                //}

            }

        }
        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 끌모임 검색 기능 추가
        protected void btnSearchKeyword_Click(object sender, EventArgs e)
        {
            pager.CurrentIndex = 1;
            SearchBindSelect(hid_SearchKeyword.Value);
        }
    }
}