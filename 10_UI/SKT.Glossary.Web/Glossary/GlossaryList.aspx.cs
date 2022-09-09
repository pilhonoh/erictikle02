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
using System.IO;
using System.Text;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryList : System.Web.UI.Page
    {

        int currentPageIndx;
        protected int iTotalCount;
        protected int iSuccessCount;
        protected int iUnSuccessCount;
        protected int iTotal;
        protected int iMyQnA;
        protected string SearchKeyword = string.Empty;
        protected string DisplaySearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string Mode = string.Empty;
        protected string qnaMode = "Search";

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            Mode = (Request["Mode"] ?? string.Empty).ToString();
            DisplaySearchKeyword = SearchKeyword;
                        
            if (!IsPostBack)
            {
                //제목 페이지
                PagerTitle.CurrentIndex = 1;
                PagerTitle.PageSize = 5;// int.Parse(this.ddlpageSize.SelectedValue);
                
                //내용 페이지
                PagerContent.CurrentIndex = 1;
                PagerContent.PageSize = 5;// int.Parse(this.ddlpageSize.SelectedValue);


                //QnA 페이지
                PagerQnA.CurrentIndex = 1;
                PagerQnA.PageSize = 5;// int.Parse(this.ddlpageSize.SelectedValue);


                //Profile 페이지
                PagerProfile.CurrentIndex = 1;
                PagerProfile.PageSize = 7;// int.Parse(this.ddlpageSize.SelectedValue);

                BindSelect();
            }

        }

        // 통합 검색 리스트
        private void BindSelect()
        {
            UserInfo u = new UserInfo(this);
            iTotalCount = 0;
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryQnABiz biz_ = new GlossaryQnABiz();
            GlossaryProfileBiz biz__ = new GlossaryProfileBiz();

            GlossaryAdminBiz abiz = new GlossaryAdminBiz();

            ArrayList Titlelist = new ArrayList();
            ArrayList Contentslist = new ArrayList();
            ArrayList QnAlist = new ArrayList();
            ArrayList Profilelist = new ArrayList();
            SearchKeyword = SearchKeyword.Replace("[", "@[").Replace("]", "@]").Replace("'", "&#39;").Replace("\"", "&quot;");

            switch (Mode)
            {
                //제목 검색
                case "Titles":
                    BodyContent.Visible = false;
                    TitleMore.Visible = false;
                    ContentMore.Visible = false;
                    TitlePage.Visible = true;
                    BodyQnA.Visible = false;
                    ProfileMore.Visible = false;
                    QnAMore.Visible = false;
                    PagerTitle.PageSize = 10;
                    //제목만 검색
                    Titlelist = biz.GlossaryTitleList(PagerTitle.CurrentIndex, PagerTitle.PageSize, out iTotalCount, SearchKeyword);
                    abiz.GlossaryAdminSearchKeywordsInsert("Title", SearchKeyword, u.UserID); 

                    for (int i = 0; i < Titlelist.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)Titlelist[i];
                        data.TagsInHtml = TagHtmlString(data.CommonID);
                    }
                    PagerTitle.ItemCount = iTotalCount;

                    rptInGeneral.DataSource = Titlelist;
                    rptInGeneral.DataBind();
                    this.lbCount.InnerHtml = PagerTitle.ItemCount.ToString();
                    this.lbltitle.InnerHtml = PagerTitle.ItemCount.ToString();
                    break;

                //내용 검색
                case "Contents":
                    PagerContent.PageSize = 10;
                    BodyTitle.Visible = false;
                    TitleMore.Visible = false;
                    ContentMore.Visible = false;
                    ContentPage.Visible = true;
                    BodyQnA.Visible = false;
                    ProfileMore.Visible = false;
                    QnAMore.Visible = false;

                    //내용만 검색
                    Contentslist = biz.GlossaryContentsList(PagerContent.CurrentIndex, PagerContent.PageSize, out iTotalCount, SearchKeyword);
                    abiz.GlossaryAdminSearchKeywordsInsert("Content", SearchKeyword, u.UserID);

                    for (int i = 0; i < Titlelist.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)Titlelist[i];
                        data.TagsInHtml = TagHtmlString(data.CommonID);
                    }
                    PagerContent.ItemCount = iTotalCount;

                    rptInGeneral_.DataSource = Contentslist;
                    rptInGeneral_.DataBind();
                    this.lbCount.InnerHtml = PagerContent.ItemCount.ToString();
                    this.lblContents.InnerHtml = PagerContent.ItemCount.ToString();
                    break;

                //QnA 검색
                case "QnA":
                    BodyContent.Visible = false;
                    TitleMore.Visible = false;
                    ContentMore.Visible = false;
                    TitlePage.Visible = false;
                    BodyTitle.Visible = false;
                    TitleMore.Visible = false;
                    ProfileMore.Visible = false;
                    ContentMore.Visible = false;
                    QnAPage.Visible = true;
                    QnAMore.Visible = false;

                    //QnA 검색
                    QnAlist = biz_.GlossaryQnAList(PagerQnA.CurrentIndex, PagerQnA.PageSize, qnaMode, out iTotalCount, out iTotal, out iSuccessCount, out iSuccessCount, SearchKeyword, "Total", "", out iMyQnA, "", "");
                    abiz.GlossaryAdminSearchKeywordsInsert("QnA", SearchKeyword, u.UserID);
                    PagerQnA.ItemCount = iTotalCount;

                    rptInGeneralQnA.DataSource = QnAlist;
                    rptInGeneralQnA.DataBind();
                    this.lbCount.InnerHtml = PagerQnA.ItemCount.ToString();
                    this.lblQnA.InnerHtml = PagerQnA.ItemCount.ToString();
                    break;

                //Profile 검색
                case "Profiles":
                    PagerProfile.PageSize = 10;
                    BodyContent.Visible = false;
                    TitleMore.Visible = false;
                    ContentMore.Visible = false;
                    TitlePage.Visible = false;
                    BodyTitle.Visible = false;
                    TitleMore.Visible = false;
                    ContentMore.Visible = false;
                    QnAPage.Visible = false;
                    QnAMore.Visible = false;
                    ProfileMore.Visible = false;
                    ProfilePage.Visible = true;

                    PagerProfile.PageSize = 14;

                    //Profile 검색
                    int iprofilecount;
                    Profilelist = biz__.GlossaryJobDescriptionList(PagerProfile.CurrentIndex, PagerProfile.PageSize, out iprofilecount, SearchKeyword);
                    abiz.GlossaryAdminSearchKeywordsInsert("Profile", SearchKeyword, u.UserID);
                    PagerProfile.ItemCount = iprofilecount;

                    this.lblPerson.InnerHtml = iprofilecount.ToString();

                    rptInProfile.DataSource = Profilelist;
                    rptInProfile.DataBind();
                    this.lbCount.InnerHtml = PagerProfile.ItemCount.ToString();
                    break;

                //제목 / 내용 / QnA 검색
                default:
                    abiz.GlossaryAdminSearchKeywordsInsert("Total", SearchKeyword, u.UserID);
                    //제목 검색
                    Titlelist = biz.GlossaryTitleList(PagerTitle.CurrentIndex, PagerTitle.PageSize, out iTotalCount, SearchKeyword);
                    for (int i = 0; i < Titlelist.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)Titlelist[i];
                        data.TagsInHtml = TagHtmlString(data.CommonID);
                    }
                    PagerTitle.ItemCount = iTotalCount;
                    if (iTotalCount <=5)
                    {
                        TitleMore.Visible = false;
                    }

                    rptInGeneral.DataSource = Titlelist;
                    rptInGeneral.DataBind();

                    //내용만 검색
                    Contentslist = biz.GlossaryContentsList(PagerContent.CurrentIndex, PagerContent.PageSize, out iTotalCount, SearchKeyword);
                    for (int i = 0; i < Contentslist.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)Contentslist[i];
                        data.TagsInHtml = TagHtmlString(data.CommonID);
                    }
                    PagerContent.ItemCount = iTotalCount;
                    if (iTotalCount <= 5)
                    {
                        ContentMore.Visible = false;
                    }
                    rptInGeneral_.DataSource = Contentslist;
                    rptInGeneral_.DataBind();


                    //QnA 검색
                    QnAlist = biz_.GlossaryQnAList(1, 5, qnaMode, out iTotalCount, out iTotal, out iSuccessCount, out iSuccessCount, SearchKeyword, "Total", "", out iMyQnA, "", "");
                    if (iMyQnA <= 5)
                    {
                        QnAMore.Visible = false;
                    }
                    rptInGeneralQnA.DataSource = QnAlist;
                    rptInGeneralQnA.DataBind();

                    //Profile
                    //int iprofilecount;
                    Profilelist = biz__.GlossaryJobDescriptionList(PagerProfile.CurrentIndex, PagerProfile.PageSize, out iprofilecount, SearchKeyword);
                    if (iprofilecount <= 7)
                    {
                        ProfileMore.Visible = false;
                    }
                    rptInProfile.DataSource = Profilelist;
                    rptInProfile.DataBind();

                    //총 검색 갯수
                    this.lbCount.InnerHtml = (PagerTitle.ItemCount + PagerContent.ItemCount + QnAlist.Count + iprofilecount).ToString();
                    this.lbltitle.InnerHtml = PagerTitle.ItemCount.ToString();
                    this.lblContents.InnerHtml = PagerContent.ItemCount.ToString();
                    this.lblQnA.InnerHtml = QnAlist.Count.ToString();
                    this.lblPerson.InnerHtml = iprofilecount.ToString();
                    break;         
            }
        
            //검색결과가 없을시 표시
            if (PagerTitle.ItemCount + PagerContent.ItemCount + QnAlist.Count + Profilelist.Count == 0)
            {
                Result.Visible = false;
                //SearchResult.Text = "<div class=\"inner\">"
                //                + "<!-- 검색결과 없는 경우 -->"
                //                + "<div class=\"none_result\">"
                //                + "<p><em>'" + SearchKeyword + "'</em>에 대한 검색결과가 없습니다.</p>"
                //                + "<ul>"
                //                + "<li>‘" + SearchKeyword + "’에 대해 알고있다면, 구성원에게 도움이 될 수 있도록<br />새 정보를 등록해 주세요.</li>"
                //                + "</ul>"
                //                + "</br>"
                //                + "<a href=javascript:fnGlossaryWrite(); class =\"_btn_s_s\"><span>용어 작성하기</span></a>"
                //                +"</br>"
                //                + "</br>"
                //                + "<a href=javascript:fnQnAWrite(); class =\"_btn_s_s\"><span>질문하기</span></a>"
                //                //+ "<a href='/" + RootURL + "/Glossary/GlossaryWrite.aspx?mode=UnRegister&SearchKeyword=" + Server.UrlEncode(SearchKeyword) + "' class=\"btn_type4\"><span class=\"btxt\">정보등록</span></a>"
                //                + "</div>"
                //                + "<!--// 검색결과 없는 경우 -->"
                //                + "</div>";


                SearchResult.Text = "<!--DASH -->"
                         + "<div class=\"main-dash\">"
                         + "<div class=\"main-dash-left\">"
                         + "        <h3 class=\"search-result-none-icon\">'<span>" + DisplaySearchKeyword + "</span>'에 대한 검색 결과가 없습니다.</h3>"
                         + "    </div>"
                         + "</div>"
                         + "<!-- result -->"
                         + "<div>"
                             + "<div class=\"search-result-none\">"
                                 + "<dl>"
                                     + "<dt>앗, 아쉽게도 검색 결과가 없습니다.</dt>"
                                         + "<dd>1. 검색 결과에 대해 모르시나요? 질문 게시판에 등록해 보세요!</dd>"
                                         + "<dd>2. 검색 결과에 대해 알고 있다면, 새 티끌을 등록해 주세요!</dd>"
                                 + "</dl>"
                             + "</div>"
                             + "<ul class=\"search-result-none-btn\">"
                             + "    <li><a href=javascript:fnGlossaryWrite(); class=\"new-article\">새 티끌 등록하기</a></li>"
                             + "    <li><a href=javascript:fnQnAWrite(); class=\"new-question\">질문 등록하기</a></li>"
                             + "</ul>"
                         + "</div>";

            }

            //제목 검색이 0건일 경우 숨기기
            if (Convert.ToInt32(PagerTitle.ItemCount) <= 0)
            {
                BodyTitle.Visible = false;
            }

            //내용 검색이 0건일 경우 숨기기
            if (Convert.ToInt32(PagerContent.ItemCount) <= 0)
            {
                BodyContent.Visible = false;
            }

            //QNA 검색이 0건일 경우 숨기기
            if (Convert.ToInt32(QnAlist.Count) <= 0)
            {
                BodyQnA.Visible = false;
            }

            //프로필이 0건일 경우 숨기기
            if (Convert.ToInt32(Profilelist.Count) <= 0)
            {
                BodyProfile.Visible = false;
            }

            //제목 검색이 1개일 경우 뷰페이지로 이동
            if (PagerTitle.ItemCount == 1 && (PagerContent.ItemCount + QnAlist.Count + Profilelist.Count) == 0)
            {
                Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + ((SKT.Glossary.Type.GlossaryType)(Titlelist[0])).CommonID + "&SearchKeyword=" + ((SKT.Glossary.Type.GlossaryType)(Titlelist[0])).Title.Replace("\n", ""));
            }

           //Gnb 검색 창에서 사용자를 검색 시 프로필로 이동하여 검색이 됨
           if (PagerTitle.ItemCount + PagerContent.ItemCount + QnAlist.Count == 0 && Profilelist.Count == 1)
           {
               GlossaryDac dac = new GlossaryDac();
               DataSet ds = new DataSet();
               ds = dac.GlossaryProfileSearch(SearchKeyword);
               if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
               {
                   Response.Redirect("/GlossaryMyPages/MyProfile.aspx?UserID=" + ds.Tables[0].Rows[0].ItemArray[0].ToString() + "&SearchKeyword=" + Server.UrlEncode(SearchKeyword));
               }
           }
           //내용 검색이 1개일 경우 뷰페이지로 이동
            //if (PagerContent.ItemCount == 1 && currentPageIndx == 0)
            //{
            //    Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + ((SKT.Glossary.Type.GlossaryType)(list_[0])).ID + "&SearchKeyword=" + ((SKT.Glossary.Type.GlossaryType)(list_[0])).Title.Replace("\n", ""));
            //}
        }

        //제목 리스트 검색어랑 일치할경우 빨간색
        protected void rptInTitle_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string TitleSet = null;
            GlossaryType Board = new GlossaryType();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litTitle = (Literal)e.Item.FindControl("litTitle");
                if (DisplaySearchKeyword == "")
                {
                    TitleSet = ((GlossaryType)e.Item.DataItem).Title.ToString();
                }
                else
                {
                    TitleSet = ((GlossaryType)e.Item.DataItem).Title.ToString();

                    if (DisplaySearchKeyword.Contains(" ") == true)
                    {
                        string[] words = DisplaySearchKeyword.Split(' ');
                         foreach (string word in words)
                         {
                             if (word.Length > 0)
                             {
                                 TitleSet = TitleSet.Replace(word.ToLower(), "<span>" + word.ToLower() + "</span>");
                                 TitleSet = TitleSet.Replace(word.ToUpper(), "<span>" + word.ToUpper() + "</span>");
                             }
                         }
                    }
                    else
                    {
                        TitleSet = ((GlossaryType)e.Item.DataItem).Title.ToString().Replace(DisplaySearchKeyword.ToLower(), "<span>" + DisplaySearchKeyword.ToLower() + "</span>");
                        TitleSet = ((GlossaryType)e.Item.DataItem).Title.ToString().Replace(DisplaySearchKeyword.ToUpper(), "<span>" + DisplaySearchKeyword.ToUpper() + "</span>");
                    }
                }
                litTitle.Text = "<a class=\"head\"  href=\"javascript:fnGoView('" + ((GlossaryType)e.Item.DataItem).CommonID + "');\">" + TitleSet + "</a>";
            }
        }

        //내용 리스트 검색어랑 일치할경우 빨간색
        protected void rptInContent_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string ContentSet = null;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litContent = (Literal)e.Item.FindControl("litContent");
                if (DisplaySearchKeyword == "")
                {
                    ContentSet = ((GlossaryType)e.Item.DataItem).Summary.ToString();
                }
                else
                {
                    ContentSet = ((GlossaryType)e.Item.DataItem).Summary.ToString();
                    if (DisplaySearchKeyword.Contains(" ") == true)
                    {
                        string[] words = DisplaySearchKeyword.Split(' ');
                        foreach (string word in words)
                        {
                            if (word.Length > 0)
                            {
                                ContentSet = ContentSet.Replace(word.ToLower(), "<span>" + word.ToLower() + "</span>");
                                ContentSet = ContentSet.Replace(word.ToUpper(), "<span>" + word.ToUpper() + "</span>");
                            }
                        }
                    }
                    else
                    {
                        ContentSet = ((GlossaryType)e.Item.DataItem).Summary.ToString().Replace(DisplaySearchKeyword.ToLower(), "<span>" + DisplaySearchKeyword.ToLower() + "</span>");
                        ContentSet = ((GlossaryType)e.Item.DataItem).Summary.ToString().Replace(DisplaySearchKeyword.ToUpper(), "<span>" + DisplaySearchKeyword.ToUpper() + "</span>");
                    }
                    
                }
                litContent.Text = ContentSet;
            }
        }

        //QnA 리스트 검색어랑 일치할경우 빨간색
        protected void rptInGeneralQnA_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string litQnATitleSet = null;
            string litQnABodySet = null;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litQnATitle = (Literal)e.Item.FindControl("litQnATitle");
                Literal litQnBody = (Literal)e.Item.FindControl("litQnABody");
                if (DisplaySearchKeyword == "")
                {
                    litQnATitleSet = ((GlossaryQnAType)e.Item.DataItem).Title.ToString();
                    litQnABodySet = ((GlossaryQnAType)e.Item.DataItem).Summary.ToString();
                }
                else
                {
                    litQnATitleSet = ((GlossaryQnAType)e.Item.DataItem).Title.ToString();
                    litQnABodySet = ((GlossaryQnAType)e.Item.DataItem).Summary.ToString();

                    if (DisplaySearchKeyword.Contains(" ") == true)
                    {
                        string[] words = DisplaySearchKeyword.Split(' ');
                        foreach (string word in words)
                        {
                            if (word.Length > 0)
                            {
                                litQnATitleSet = litQnATitleSet.Replace(word.ToLower(), "<span>" + word.ToLower() + "</span>");
                                litQnABodySet = litQnABodySet.Replace(word.ToLower(), "<span>" + word.ToLower() + "</span>");

                                litQnATitleSet = litQnATitleSet.Replace(word.ToUpper(), "<span>" + word.ToUpper() + "</span>");
                                litQnABodySet = litQnABodySet.Replace(word.ToUpper(), "<span>" + word.ToUpper() + "</span>");
                            }
                        }
                    }
                    else
                    {
                        litQnATitleSet = ((GlossaryQnAType)e.Item.DataItem).Title.ToString().Replace(DisplaySearchKeyword.ToLower(), "<span>" + DisplaySearchKeyword.ToLower() + "</span>");
                        litQnABodySet = ((GlossaryQnAType)e.Item.DataItem).Summary.ToString().Replace(DisplaySearchKeyword.ToLower(), "<span>" + DisplaySearchKeyword.ToLower() + "</span>");

                        litQnATitleSet = ((GlossaryQnAType)e.Item.DataItem).Title.ToString().Replace(DisplaySearchKeyword.ToUpper(), "<span>" + DisplaySearchKeyword.ToUpper() + "</span>");
                        litQnABodySet = ((GlossaryQnAType)e.Item.DataItem).Summary.ToString().Replace(DisplaySearchKeyword.ToUpper(), "<span>" + DisplaySearchKeyword.ToUpper() + "</span>");
                    }
                }
                litQnATitle.Text = "<a class=\"head\"  href=\"javascript:fnQnAGoView('" + ((GlossaryQnAType)e.Item.DataItem).ID + "');\">" + litQnATitleSet + "</a>";
                litQnBody.Text = litQnABodySet;
            }
        }

        //사용자 이름 검색어랑 일치할경우 빨간색
        protected void rptInProfile_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal UserImg = (Literal)e.Item.FindControl("UserImg");
                ImpersonUserinfo data = biz.UserSelect(((GlossaryProfileType)e.Item.DataItem).UserID);
                UserImg.Text = "<img src=" + data.PhotoUrl + " align=\"" + data.Name + "/" + data .DeptName+ "\" />";
            }
        }

        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            pager_CommandCommon(e, "Title");
        }

        //내용 페이지 처리
        public void pager_Command1(object sender, CommandEventArgs e)
        {
            pager_CommandCommon(e, "Content");
        }

        //QnA 페이지 처리
        public void pager_Command3(object sender, CommandEventArgs e)
        {
            pager_CommandCommon(e, "Qna");
        }

        //Profile 페이지 처리
        public void pager_Command4(object sender, CommandEventArgs e)
        {
            pager_CommandCommon(e, "Profile");
        }

        protected void pager_CommandCommon(CommandEventArgs e, string category)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            
            if(category == "Title")
            {
                PagerTitle.CurrentIndex = currentPageIndx;
                
            }else if(category =="Content")
            {
                PagerContent.CurrentIndex = currentPageIndx;
            }else if(category == "Qna")
            {
                PagerQnA.CurrentIndex = currentPageIndx;
            }else if(category == "Profile")
            {
                PagerProfile.CurrentIndex = currentPageIndx;
            }
            this.hdMore.Value = this.hdMore.Value;

            BindSelect();
        }
        
        //검색 결과
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //SearchKeyword = this.question.Value;
            PagerTitle.CurrentIndex = 1;
            PagerContent.CurrentIndex = 1;
            BindSelect();
        }

        ////제목 더보기
        //protected void btnTitleMore_Click(object sender, EventArgs e)
        //{
        //    this.hdMore.Value = "Title";
        //    BindSelect();
        //}

        ////내용 더보기
        //protected void btnContentMore_Click(object sender, EventArgs e)
        //{
        //    this.hdMore.Value = "Content";
        //    BindSelect();
        //}

        //검색어 인코딩
        protected string GetSearchKeyword()
        {
            return Server.UrlEncode(SearchKeyword);
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
                GlossaryTagType  data = (GlossaryTagType)list[i];
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
    }
}