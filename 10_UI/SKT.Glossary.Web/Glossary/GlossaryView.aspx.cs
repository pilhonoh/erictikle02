using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

using System.IO;
using Ionic.Zip;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryView : System.Web.UI.Page
    {
        protected string ItemID;

        protected string mode = string.Empty;

        protected string ReplyAdd = string.Empty;

        protected string CommonID = string.Empty;
        protected string UserID = string.Empty;
        protected string UserName = string.Empty;
        protected string YouUserID = string.Empty;
        protected string Popularity = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string Title = string.Empty;
        protected string RootURL = string.Empty;
        protected int Number = 0;
        protected string TutorialYN = string.Empty;
        protected string backUrl;
        protected string AlarmYN = string.Empty;
        protected int HallOfFameYN;

        public string ScrapOn = string.Empty;

        // 2014-04-29 Mr.No 추가
        protected string CategoryTitle = string.Empty;

        protected string CategoryID = string.Empty;

        // 2014-05-22 Mr.No
        internal const int GLOSSARY_ATTACH_ID = 314;

        private const string BuildingUserBoardViewAttachInfo = "BuildingUserBoardViewAttachInfo";

        // 2015-02-25 Mr.Kim 추가
        public string Liked = "N";

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;
        protected string TagTitle;

        protected string AuthorYN = "N";

        // Platform Flag
        protected string PlatformYN = "N";

        // Marketing Flag
        protected string MarketingYN = "N";

        // 기술 트렌드 Flag
        protected string TechTrendYN = "N";

        protected int PageNum = 1;
        protected string SearchSort = string.Empty;

        protected string WType = string.Empty;  //DT 블로그홈
        protected bool WTypeWrite = false;
        protected string TType = string.Empty;  //T생활백서
        protected string SchText = string.Empty; //DT 블로그홈 T생활백서 검색어


        /// <summary>
        /// 첨부파일 Javascript Serialize 데이터
        /// </summary>
        protected string AttachInfo
        {
            get
            {
                if (ViewState[BuildingUserBoardViewAttachInfo] == null)
                {
                    return "[]";
                }
                else
                {
                    return (string)ViewState[BuildingUserBoardViewAttachInfo];
                }
            }
            set
            {
                ViewState[BuildingUserBoardViewAttachInfo] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;
            UserName = u.Name;

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            CommonID = (Request["ItemID"] ?? string.Empty).ToString();
            mode = (Request["mode"] ?? string.Empty).ToString();
            Popularity = (Request["Popularity"] ?? string.Empty).ToString();
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ReplyAdd = (Request["reply"] ?? string.Empty).ToString();

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            PageNum = Convert.ToInt16((Request["PageNum"] ?? "1"));

            TagTitle = ((Request["TagTitle"] == null || Request["TagTitle"] == string.Empty) ? string.Empty : HttpUtility.UrlDecode(Request["TagTitle"])).ToString();

            //CHG610000063658 / 2018-05-17 / 최현미 / SSO 연동관련 오류 수정 요청
            if (TagTitle.IndexOf("-$") > -1)
                TagTitle = TagTitle.Replace("-$", "&");
            if (TagTitle.IndexOf("- ") > -1)
                TagTitle = TagTitle.Replace("- ", " ");

            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();

            WType = (Request["WType"] ?? string.Empty).ToString();
            TType = (Request["TType"] ?? string.Empty).ToString();

            //DT블로그홈 / T생활백서 검색어 유지
            if (!string.IsNullOrEmpty(WType) || !string.IsNullOrEmpty(TType))
            {
                SchText = (string.IsNullOrEmpty(Request["SchText"]) ? string.Empty : Request["SchText"]).ToString();
            }

            // 권한 지정 시 그룹선택 허용 = true
            this.UserControl.UserGroupVisible = false;

            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    ViewState["PreviousPage"] = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                else
                {
                    ViewState["PreviousPage"] = "";
                }
            }

            // reply param이 Y이면 mode를 reply로 바꿔 Hits 이 증가하지 않도록 한다
            if (ReplyAdd == "Y")
            mode = "reply";

            //조회
            Select();

            foreach (DataRow row in new GlossaryBiz().GetSpecialUserChargeSelect("D").Tables[0].Rows)
            {
                if (this.UserID.Equals(row["USERID"].ToString()) && this.WType.Equals(row["DTBlog"].ToString()))
                    this.WTypeWrite = true;
            }
            if (u.isAdmin)
            {
                this.WTypeWrite = true;
            }

        }


        //리스트
        private void Select()
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryType Board = new GlossaryType();
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryControlBiz bizCon = new GlossaryControlBiz();
            GlossaryControlType BoardCon = new GlossaryControlType();

            Board = biz.GlossarySelect(ItemID, u.UserID, mode);

            //목록 쿼리는 넘 복잡해서 못 건드리게땅.. 간단한거 위주로 하자고..
            //up_Glossary_Select
            //성환대리 위에서 쿼리 가져올때 이 부분을 OUTER로 걸어서 GatheringID를 가져오명 될것 같아
            //LEFT OUTER JOIN tb_Glossary_Gathering_ItemType B WITH(NOLOCK)  ON tb_Glossary.CommonID = B.CommonID AND B.GatheringMenu = 'Knowledge'
            ////검색 시 삭제된 데이터 처리 부분
            if (Board.ID == null)
            {
                Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 게시글은 삭제된 데이터 입니다.", false);
                Response.End();
            }

            // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
            if (Board.DTBlogFlag.Equals("D"))
            {
                if (u.IsDTPermission == false)
                {
                    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 게시글에 대한 권한이 없습니다.", false);
                    Response.End();
                }
            }

            // CHG610000074852 / 20181108 / T생활백서 
            //T생활백서를 통해 접근 시 ThanksLetter버튼 보임
            LetterAdd.Visible = false;
            if (TType.Equals("A") || TType.Equals("B") || TType.Equals("C"))
            {
                LetterAdd.Visible = true;
            }


            bool CheckResult = false;
            //20171201 메뉴제거시 모임글 체크안함
            GatheringYN = string.Empty;
            GatheringID = string.Empty;
            //if (!String.IsNullOrEmpty(Board.GatheringID))
            //{
            //    GatheringInfomation1.GatheringYN = "Y";
            //    GatheringInfomation1.GatheringID = Board.GatheringID;
            //    GatheringMenuTabCtrl.GatheringID = Board.GatheringID;
            //    GatheringYN = "Y";
            //    GatheringID = Board.GatheringID;

            //    GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            //    DataSet GatheringMemberDS = gBiz.GlossaryGathering_MemberList(GatheringID);

            //    if (GatheringMemberDS.Tables.Count > 0 && GatheringMemberDS.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in GatheringMemberDS.Tables[0].Rows)
            //        {
            //            if (u.UserID.ToUpper().Equals(dr["EMPNO"].ToString().ToUpper()))
            //            {
            //                CheckResult = true;
            //            }
            //        }
            //    }
            //}

            //전사지식공유 
            if (GatheringYN.Equals("Y") && Board.PublicYN.Equals("Y") && CheckResult == false)
            {
                //GatheringInfomation1.GatheringYN = string.Empty;
                //GatheringInfomation1.GatheringID = string.Empty;

                GatheringYN = string.Empty;
                GatheringID = string.Empty;
            }


            ////모임정보가 비어있다면 모임글인지 한번 더 체크
            //if (GatheringYN == string.Empty)
            //{
            //    DataSet gds = new DataSet();
            //    gds = biz.GlossaryGatheringItemCheck(ItemID);

            //    if (gds.Tables[0].Rows.Count > 0)
            //    {
            //        GatheringYN = "Y";
            //        GatheringID = gds.Tables[0].Rows[0]["GatheringID"].ToString();
            //        //GatheringInfomation에 프로퍼티로 넣어줘야 함
            //    }
            //}

            // 2014-06-03
            // 권한 체크 후 에러 페이지로 이동
            GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            int rtnValue = permissionsBiz.Permissions_Check(Board.CommonID, u.UserID.ToString());
            //if (rtnValue == 0) { Response.Redirect("../Error.aspx?Message=" + "이 글에 대한 조회 권한이 부여되지 않았습니다."); }  // 글 권한 체크
            if (rtnValue == 0)
            {
                // 권한처리(모임멤버검사 : 모임글이면 한번 더 검사해서 해당 모임 멤버이면 통과)
                if (GatheringYN == "Y")
                {
                    if (!CheckResult)
                    {
                        Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 지식은 끌.모임의 멤버 권한이 부여된 분들만 보실 수 있습니다 ^^;", false);
                        Response.End();
                    }
                }
                else
                {
                    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 지식은 조회 권한이 부여된 분들만 보실 수 있습니다 ^^;", false);
                    Response.End();
                }
            }
            else
            {
                //일반공유 
                if (GatheringYN == "Y" && CheckResult == false)
                {
                    //GatheringInfomation1.GatheringYN = string.Empty;
                    //GatheringInfomation1.GatheringID = string.Empty;

                    GatheringYN = string.Empty;
                    GatheringID = string.Empty;
                }
            }


            YouUserID = Board.UserID;
            AlarmYN = Board.NoteYN ?? string.Empty;
            HallOfFameYN = Convert.ToInt16(Board.HallOfFameYN);


            PlatformYN = Board.PlatformYN;
            MarketingYN = Board.MarketingYN;
            TechTrendYN = Board.TechTrendYN;



            btnTikleDelete.Visible = false;
            this.lbTitle.Text = Board.Title;

            Title = Board.Title;
            SearchKeyword = Board.Title;    // 제목 넣기
            //this.txtBody.Text = ChangeCutSummaryBox(SecurityHelper.Add_XSS_CSRF(Board.ContentsModify));  //내용 넣기
            //this.txtBody.Text = ChangeCutSummaryBox(SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(Board.ContentsModify)));  //내용 넣기
            //Iframe 생성 Mostisoft 2015.08.21
            this.txtBody.Text = "<p class='text' data-ItemID=" + ItemID + "><iframe class='StandaloneView" + ItemID + "' src='GlossaryIframe_View.aspx?ItemID=" + ItemID + "&UserID=" + u.UserID + "&Mode=" + mode + "'  scrolling='no' frameborder='0' style='width:100%'><div class='view_ct_area' style='padding-top:10px;'><div class='view_ct' style='margin:0;'>" + Board.ContentsModify + "</div></div></iframe></p>";

            CommonID = Board.CommonID;
            //this.litTitleList.Text = Utility.GlossaryViewH1List(txtBody.Text); //목차
            //lbDate.Text = Board.ModifyDate; //이 티끌은?? - 최초작성

            /*
            Author : 개발자-김성환D, 리뷰자-진현빈D
            Create Date : 2016.06.01
            Desc : 댓글등록하기 끌지식 쪽지 보내기 체크
            */
            if (u.UserID == Board.UserID)
            {
                CommCommentControl.SendYN = "N";
            }
            else
            {
                if (GatheringYN != "Y")
                {
                    CommCommentControl.SendYN = biz.GlossaryCreateWriteYN(ItemID);
                }
            }

            // 이 글이 최초 작성자 이후로 편집된 기록이 없고
            if ((Board.ID == Board.CommonID))
            {
                // 비공개로 작성되지 않았을 경우
                if ((Board.PrivateYN == "N"))
                {
                    // 2014-06-11 Mr.No
                    //litFirstUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.FirstUserID) + "\"><a href=\"javascript:fnProfileView('" + Board.FirstUserID + "');\"><b>" + Board.FirstUserName + "/" + Board.FirstDeptName;
                    litFirstUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.FirstUserID) + "\"><b>" + Board.FirstUserName + "/" + Board.FirstDeptName;
                    litFirstUser.Text += "</b></abbr>이 " + Board.FirstCreateDate + "에 최초 작성";

                    //litLastUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.UserID) + "\"><a href=\"javascript:fnProfileView('" + Board.UserID + "');\"><b>" + Board.UserName + "/" + Board.DeptName;
                    litLastUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.UserID) + "\"><b>" + Board.UserName + "/" + Board.DeptName;
                    litLastUser.Text += "</b></abbr>이 " + Board.CreateDate + "에 마지막 수정";
                }
                // 최초 작성자 이후로 편집된 기록이 없으나 비공개로 작성되었을 경우
                if ((Board.PrivateYN == "Y"))
                {
                    litFirstUser.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(Board.FirstUserName) + "이 " + Board.FirstCreateDate + "에 최초 작성";
                    litLastUser.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(Board.UserName) + "이 " + Board.CreateDate + "에 마지막 수정";
                }
            }

            // 편집 내역이 있는 글일 경우
            else
            {
                // 최초 글이 비공개로 작성되지 않았을 경우
                if (Board.FirstPrivateYN == "N")
                {
                    //litFirstUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.FirstUserID) + "\"><a href=\"javascript:fnProfileView('" + Board.FirstUserID + "');\"><b>" + Board.FirstUserName + "/" + Board.FirstDeptName;
                    //litFirstUser.Text += "</b></a></abbr>님이 " + Board.FirstCreateDate + "에 최초 작성";

                    litFirstUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.FirstUserID) + "\"><b>" + Board.FirstUserName + "/" + Board.FirstDeptName;
                    litFirstUser.Text += "</b></abbr>이 " + Board.FirstCreateDate + "에 최초 작성";
                }
                else
                {
                    litFirstUser.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(Board.FirstUserName) + "이 " + Board.FirstCreateDate + "에 최초 작성";
                }

                // 현재 글이 비공개로 작성되지 않았을 경우
                if (Board.PrivateYN == "N")
                {
                    //litLastUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.UserID) + "\"><a href=\"javascript:fnProfileView('" + Board.UserID + "');\"><b>" + Board.UserName + "/" + Board.DeptName;
                    //litLastUser.Text += "</b></a></abbr>님이 " + Board.CreateDate + "에 마지막 수정";

                    litLastUser.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(Board.UserID) + "\"><b>" + Board.UserName + "/" + Board.DeptName;
                    litLastUser.Text += "</b></abbr>이 " + Board.CreateDate + "에 마지막 수정";
                }
                else
                {
                    litLastUser.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(Board.UserName) + "이 " + Board.CreateDate + "에 마지막 수정"; // 2014-06-09 Mr.No
                }
            }
            if (Board.fromQnaID != "" && Convert.ToInt16(Board.fromQnaID) != 0)
            {
                litFromQna.Text = "<font color=black><b>(질문/답변 게시판에서 올라온 티끌)</b></font>";
            }

            //뷰 메뉴들 세팅 우측 상단(인쇄, pdf저장 스크랩 등..)
            BoardCon = bizCon.GlossaryViewMenuSelect(u.UserID, CommonID);
            if (BoardCon.ScrapsYN == "Y")
            {
                ScrapOn = " on";
                litScrap.Text = "스크랩해제";
            }
            else
            {
                ScrapOn = "";
                litScrap.Text = "스크랩하기";
            }

            // 이미 추천을 했으면
            if (BoardCon.LikeY != "")
            {
                //LikeAdd.Visible = false;
                //LikeCount.Visible = true;
                Liked = "Y";
            }
            else
            {
                //LikeAdd.Visible = true;
                //LikeCount.Visible = false;
            }

            //if (BoardCon.LikeCount == "")
            //{
            //    lbLikeCounts.Text = "0";
            //}
            //else
            //{
            //    lbLikeCounts.Text = BoardCon.LikeCount;
            //}
            /*
            if (BoardCon.MailYN == "Y")
                email.Checked = true;
            */
            /*
            if (BoardCon.NoteYN == "Y")
                nateon.Checked = true;
            */
            /*
            if (string.IsNullOrEmpty(this.litTitleList.Text))
            {
                BodyList.Visible = false;
            }
            */

            /*Author : 개발자-김성환D, 리뷰자-진현빈D
            Create Date : 2016.08.11
            Desc : 조회수, 추천수 보이도록 처리            */
            lbHitCount.Text = Board.Hits;
            //lbLikeCount.Text = BoardCon.LikeCount;

            lbHistorycount.Text = BoardCon.Historycount;    //이 티끌은?? - 총 폅집횟수
            lbScrapCount.Text = BoardCon.ScrapCount;    //이 티끌은?? - 총 스크랩 횟수
            //lbLikeCounts.Text = BoardCon.LikeCount;    //이 티끌은?? - 감사표시 횟수
            //if (string.IsNullOrEmpty(BoardCon.LikeCount.Trim()))
            //    lbLikeCount.Text = "0";

            GlossaryControlBiz cBiz = new GlossaryControlBiz();
            //태그 리스트
            /*
            ArrayList list = new ArrayList();
            
            list = cBiz.GlossaryTagList(Board.CommonID, Title, UserID);
            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
            if (list.Count == 0)
                TagList.Visible = false;
            */

            /*
            //20140212 , 추천 기능 변경
            GlossaryLikeType lType = new GlossaryLikeType();
            lType = cBiz.GlossaryLikeSelect(CommonID, "Info");

            // CHG610000060541 / 2018-03-27 / 최현미 / 님제거
            //lblLikeName.Text = lType.LatestUserName;
            //lblLikeNum.Text = lType.TotalCount;
            
            if (lType.TotalCount == "0")
            {
                LikeToolTip = lType.LatestUserName + "이 감사의 마음을 남기셨습니다";
                LikeAdd.Attributes.Add("title", LikeToolTip);
                //lblLikeText01.Text = "이 감사의 마음을 남기셨습니다";
                //lblLikeNum.Text = string.Empty;
                //lblLikeNum.Visible = true;
                //lblLikeText02.Text = string.Empty;
                //lblLikeText02.Visible = true;
            }
            else if (Convert.ToInt32(lType.TotalCount) > 0)
            {
                LikeToolTip = lType.LatestUserName + " 외 " + lType.TotalCount + "명이 감사의 마음을 남기셨습니다.";
                LikeAdd.Attributes.Add("title", LikeToolTip);

                //lblLikeText01.Text = " 외 ";
                //lblLikeText02.Text = "명이 감사의 마음을 남기셨습니다.";
            }
            else
            {
                //lblLikeName.Visible = false;
                //lblLikeText01.Text = string.Empty;
                //lblLikeText01.Visible = true;
                //lblLikeNum.Visible = false;
                //lblLikeText02.Visible = false;
            }
             * */

            /*
            Author : 개발자-김성환D, 리뷰자-진현빈D
            Create Date : 2016.07.13
            Desc : 삭제하기 버튼 체크 로직 추가 (0일때만 보여지도록 return 값 처리)
            */
            string deleteCheck = biz.GlossaryDeleteCheck(CommonID, u.UserID);
            if (Convert.ToInt32(deleteCheck) == 0)
            {
                btnTikleDelete.Visible = true;
            }

            //20131219 , 삭제 버튼 관련
            if (Board.UserID == u.UserID && Board.HistoryCount == "0" && Board.CommentCount == "0")
            {
                btnTikleDelete.Visible = true;
            }

            //20160415 티끌이한테는 글 삭제 버튼을 보여준다.
            if (u.UserID == "S003331")
            {
                btnTikleDelete.Visible = true;
            }

            if (Board.UserID == u.UserID)
            {
                AuthorYN = "Y";
            }

            // 2014-04-29 Mr.No 추가
            if (Board.CategoryID != 0)
            {
                GlossaryCategoryType glossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategorySelect(Board.CategoryID);
                if (glossaryCategoryType != null)
                {
                    CategoryTitle = glossaryCategoryType.CategoryTitle;
                }
                else
                {
                    CategoryTitle = string.Empty;
                }
                CategoryID = Convert.ToString(Board.CategoryID);
            }

            if (GatheringYN == "Y")
            {
                //UserControl.Visible = true;
                //2016-04-15 P033028 주석처리 
                //List<PermissionsType> info = permissionsBiz.PermissionsSelect(Convert.ToInt32(CommonID));

                // 모임 권한 추가
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                List<PermissionsType> GatheringInfo = gatheringBiz.GatheringMenuAuth_Select(Convert.ToInt32(CommonID), "Knowledge");

                //2016-04-15 P033028 주석처리 
                //foreach (PermissionsType pt in GatheringInfo)
                //{
                //    info.Add(pt);
                //}

                //2016-04-15 P033028 변경
                //hdPermissionsString.Value = JsonConvert.SerializeObject(info);
                hdPermissionsString.Value = JsonConvert.SerializeObject(GatheringInfo);
            }

            // 2014-05-22 Mr.No
            //DextUpload
            List<Attach> attachList = AttachmentHelper.Select(Convert.ToInt32(ItemID), Convert.ToInt32(CommonID), GLOSSARY_ATTACH_ID);
            this.AttachInfo = Newtonsoft.Json.JsonConvert.SerializeObject(attachList);

            GetAttachInfo(CommonID);

            // 2014-05-26
            // 전체공개(FullPublic) 경우 공유 기능 show
            if (Board.Permissions == "FullPublic") { hdPermissionRtnValue.Value = "show"; }

            // 2014-06-03
            // Tag List 들어갑니다.
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = new DataSet();
            ds = Dac.GlossaryTagSelect(Board.CommonID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtTag.Value += SKT.Common.SecurityHelper.Clear_XSS_CSRF(dr["TagTitle"].ToString()) + ",";
                    hdTagValue.Value += dr["ID"].ToString() + ", ";
                }
                //txtTag.Value = txtTag.Value.Remove(txtTag.Value.LastIndexOf(","));
            }
            CommCommentControl.commTypeCk = "Glossary";
            CommCommentControl.commIdxCK = Board.CommonID;
            //P097010 BACKUP2
            CommCommentControl.GatheringID = GatheringID;
            CommCommentControl.GatheringYN = GatheringYN;

            if (GatheringYN.Equals("Y"))
            {
                CommCommentControl.commWriteCk = "N";
            }


        }


        //20131219 , 삭제 버튼 관련
        protected void btnTikleDelete_Click(object sender, EventArgs e)
        {
            GlossaryBiz GBiz = new GlossaryBiz();
            UserInfo u = new UserInfo(this.Page);

            GBiz.TikleDelete(u.UserID, CommonID, u.userIp, u.userMachineName);

            // 모임정보 삭제
            if (GatheringYN == "Y")
            {
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                gatheringBiz.GatheringMenuAuth_Delete(CommonID, "Knowledge", GatheringID);
            }

            string BaseURL = ConfigurationManager.AppSettings["BaseURL"];

            Uri url = new Uri("http://tikle.sktelecom.com");
            string absolutePath = string.Empty;
            if (ViewState["PreviousPage"].ToString() != "")
            {
                absolutePath = new Uri(ViewState["PreviousPage"].ToString()).AbsolutePath;

            }
            #region CHG610000073115 / 2018-10-11 / 최현미 / ContentFeeds
            if (ConfigurationManager.AppSettings["TnetContentFeeds"].ToString().Equals("Y"))
            {
                OpinionBoardFeed bsObj = new OpinionBoardFeed();
                bsObj.sbmId = "TKE@100@" + this.CommonID;
                ContentFeeds.SendFeeds(bsObj, "DELETE");
            }
            #endregion

            if (!string.IsNullOrEmpty(this.WType))
                this.Response.Redirect(BaseURL + "/Glossary/DigitalTrans.aspx?WType=" + this.WType);
            else if (!string.IsNullOrEmpty(this.TType))
                this.Response.Redirect(BaseURL + "/Glossary/WhitePaper.aspx?TType=" + this.TType);
            else if (absolutePath == "/Glossary/GlossaryWriteNew.aspx" || this.ViewState["PreviousPage"].ToString() == "")
                this.Response.Redirect(BaseURL + "Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&WType=" + this.WType);
            else
                this.Response.Redirect(this.ViewState["PreviousPage"].ToString());
        }

        //
        protected void btnTikleGoToHall_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryHallOfFameType board = new GlossaryHallOfFameType();
            board.GlossaryID = CommonID;
            board.CreateUserID = u.UserID;
            board.CreateUserIP = u.userIp;
            board.CreateUserMachineName = u.userMachineName;

            try
            {
                biz.GlossaryHallOfFameInsert("TIKLE", board);
            }
            catch (Exception error)
            {
            }
            finally
            {
                Response.Redirect("GlossaryView.aspx?ItemID=" + ItemID);
            }
        }

        //pdf 저장 예전에 사용했으나 없어짐..
        protected void btnPdfFile_Click(object sender, EventArgs e)
        {
            //string pdffilename = NowPageToPDF(Request.Url.ToString());

            //string m_Directory = ConfigurationManager.AppSettings["PdfPath"] ?? string.Empty;

            //string FilePath = m_Directory + "\\" + pdffilename;

            //if (!File.Exists(FilePath))
            //{
            //    throw new Exception("파일이 존재하지않습니다.");
            //    //return;
            //}
            //var fileInfo = new System.IO.FileInfo(FilePath);
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", FilePath));
            //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            //Response.WriteFile(FilePath);
            //Response.End();
        }

        //pdf 이름 저장
        //public string GetNewName()
        //{
        //    UserInfo u = new UserInfo(this.Page);
        //    string UserID = u.UserID;

        //    string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

        //    return UserID + date + ".pdf";
        //}

        //public string NowPageToPDF(string sRawUrl)
        //{
        //    string sFileName = GetNewName();

        //    string m_Directory = ConfigurationManager.AppSettings["PdfPath"] ?? string.Empty;

        //    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

        //    System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo();

        //    startinfo.FileName = m_Directory + "\\wkhtmltopdf-0.8.3.exe";
        //    string args = String.Format("{0}\"{1}\"{2}\"", "-q ", sRawUrl + "\" ", sFileName);
        //    startinfo.Arguments = args;
        //    startinfo.UseShellExecute = false;
        //    startinfo.CreateNoWindow = true;
        //    startinfo.WorkingDirectory = m_Directory;
        //    pProcess = System.Diagnostics.Process.Start(startinfo);
        //    pProcess.WaitForExit();

        //    pProcess.Close();
        //    pProcess.Dispose();

        //    return sFileName;
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

        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal TagList = (Literal)e.Item.FindControl("litTitleList");
                //((GlossaryProfileType)e.Item.DataItem).UserID;
                TagList.Text = "<li><a href=\"javascript:TagView(" + ((GlossaryControlType)e.Item.DataItem).ID + ")\">" + ((GlossaryControlType)e.Item.DataItem).Title + "</a></li>";
                //<span>(15)</span>
                Number += 1;
            }
        }

        protected string ChangeCutSummaryBox(string Contents)
        {
            string strContent = Contents;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);

            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//a");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {
                    if (link.Attributes["href"] != null)
                    {
                        string hrefval = link.Attributes["href"].Value;
                        if (hrefval.IndexOf("#myLink") >= 0)
                        {
                            string s = CutSummary(link.InnerText, 20);
                            link.InnerHtml = s;
                        }
                    }
                }
                strContent = HtmlAgilityPackErrorHandling(htmlDoc.DocumentNode.OuterHtml);
            }
            return AddHtagLinkID(strContent);
        }

        //20140206  HtmlAgilityPack 태그 교정 문제로 생성 이후 관련 처리는 여기서 <하나 둘 셋 넷>
        protected string HtmlAgilityPackErrorHandling(string content)
        {
            string strContent = string.Empty;
            if (content != null)
            {
                strContent = content.Replace("=\"\"", "");
            }
            return strContent;
        }

        protected string AddHtagLinkID(string Contents)
        {
            string strContent = Contents;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);

            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//h2|//h3|//h4");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {
                    if (link.Attributes["id"] == null)
                    {
                        //HtmlAgilityPack.HtmlAttribute pp = new HtmlAgilityPack.HtmlAttribute();
                        HtmlAgilityPack.HtmlAttribute addid = htmlDoc.CreateAttribute("id", Guid.NewGuid().ToString());
                        link.Attributes.Add(addid);
                    }
                }
                strContent = htmlDoc.DocumentNode.OuterHtml;
            }
            return strContent;
        }

        public string CutSummary(string Summary, int maxlengh = 40)
        {
            if (Summary.Length > maxlengh)
            {
                return Summary.Substring(0, maxlengh - 3) + "...";
            }
            else
            {
                return Summary;
            }
        }

        //저장
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            permissionsBiz.PermissionsDelete(Convert.ToInt32(ItemID));

            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();

            if ((hdPermissions.Value).Equals("FullPublic"))
            {
                gatheringBiz.GatheringMenuAuth_Delete(CommonID, "Knowledge", GatheringID);

                gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Knowledge", CommonID, "Y");
            }

            if ((hdPermissions.Value).Equals("SomePublic"))
            {
                gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Knowledge", CommonID);
                SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;
                permissionsBiz.PermissionsInsert(ItemID, UDList.AuthID, UDList.AuthName, UDList.AuthCL, CommonID);

                //2015-08-10 KSH 끌모임> 특정사용자 공유하기 쪽지 전송
                GatheringSendNote(UDList);
            }

            ////20180327 solr 중지
            //if (ConfigurationManager.AppSettings["IsTestServer"] != "Y")
            //{
            //    GlossarySearchBiz bizGlossarySearch = new GlossarySearchBiz();
            //    bizGlossarySearch.SetSearchGlossarySyncDataUpdate("Glossary", CommonID);
            //}

            string url = string.Format("/Glossary/GlossaryView.aspx?ItemID={0}&GatheringYN={1}&GatheringID={2}", ItemID, GatheringYN, GatheringID);
            Response.Write("<script>location.href = '" + url + "';</script>");
        }

        //2015-08-10 KSH 끌모임 > 공유하기 쪽지 보내기
        protected void GatheringSendNote(SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            DataSet ds = null;
            string[] recvType = null;
            string[] recvID = null;

            if (UDList != null)
            {

                //UDList.AuthCL = "M/U/"
                //UDList.AuthID = "79/P060000/"
                recvType = UDList.AuthCL.Split('/');
                recvID = UDList.AuthID.Split('/');

                List<string> userid = new List<string> { };


                for (int i = 0; i < recvType.Length; i++)
                {
                    if (recvType[i] == "U")
                    {
                        userid.Add(recvID[i].ToUpper());
                    }
                    else if (recvType[i] == "O")
                    {
                        ds = gatheringBiz.DeptToUser_Select(recvID[i]);


                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            userid.Add(ds.Tables[0].Rows[j]["EMPNO"].ToString().ToUpper());
                        }
                    }
                    else if (recvType[i] == "M")
                    {
                        ds = gatheringBiz.GlossaryGatheringAuth_Select(recvID[i]);
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            userid.Add(ds.Tables[0].Rows[j]["AuthID"].ToString().ToUpper());
                        }
                    }
                }

                IEnumerable<string> disuserid = userid.Distinct();
                string userids = "";
                foreach (string dr in disuserid)
                {
                    userids += "'" + dr + "',";
                }

                userids = userids.Substring(0, userids.Length - 1);
                SendNote_Gathering(userids, "GG");
            }
        }



        //끌지식 > 쪽지 보내기
        protected void btnNotiSend_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;
            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            DataSet ds = new DataSet();
            string[] recvType = null;
            string[] recvID = null;

            WeeklyBiz biz = new WeeklyBiz();

            if (UDList != null)
            {
                recvType = UDList.AuthCL.Split('/');
                recvID = UDList.AuthID.Split('/');

                List<string> userid = new List<string> { };

                for (int i = 0; i < recvType.Length; i++)
                {
                    if (recvType[i] == "U")
                    {
                        userid.Add(recvID[i].ToUpper());
                    }
                    else if (recvType[i] == "O")
                    {
                        ds = gatheringBiz.DeptToUser_Select(recvID[i]);


                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            userid.Add(ds.Tables[0].Rows[j]["EMPNO"].ToString().ToUpper());
                        }
                    }
                }

                IEnumerable<string> disuserid = userid.Distinct();
                string userids = "";
                foreach (string dr in disuserid)
                {
                    userids += "'" + dr + "',";
                }

                userids = userids.Substring(0, userids.Length - 1);
                SendNote_Gathering(userids, "G");
            }
        }

        #region 사용안함
        //끌지식 > 쪽지보내기
        private void SendNote(string uid)
        {
            //UserInfo u = new UserInfo(this.Page);
            //GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
            //ImpersonUserinfo ui = biz_.UserSelect(uid);

            //string Recipient = ui.EmailAddress;

            //string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //string NoteLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&GatheringYN=&GatheringID=";

            //string NoteBody = "<html><body><font face='맑은고딕' size='2'>안녕하세요, 티끌이입니다. ^^<br /><br />"
            //                + u.Name + "님께서 끌.지식을 공유하였습니다.<br /><br />"
            //    //+ "<font face='맑은고딕' size='2'><a href='" + NoteLink + "'>" + Title + "</a></font></body></html>";
            //                + "<font face='맑은고딕' size='2'><a href='" + NoteLink + "'>▶ 끌.지식 바로가기</a></font></body></html>";

            //CBHMSMQHelper helper = new CBHMSMQHelper();
            //CBHNoteType data = new CBHNoteType();

            //data.Content = NoteBody;
            //data.Kind = "3"; //일반쪽지.
            //data.URL = NoteLink;
            //data.SendUserName = "티끌이";

            //string userID = Recipient.Remove(Recipient.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            //data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
            //data.TargetUser = userID;

            //**helper.SendNoteToQueue(data);
        }
        #endregion

        //2015-08-10 KSH 끌모임 > 공유하기 쪽지보내기
        private void SendNote_Gathering(string uid, string type)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
            List<ImpersonUserinfo> tu = biz_.UserSelectList(uid);

            foreach (ImpersonUserinfo ui in tu)
            {
                if (ui.EmailAddress != null)
                {
                    string Recipient = ui.EmailAddress;

                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string NoteLink = "";
                    string NoteBody = "";

                    string senderJobName = u.JobCodeName;   //보내는 사람 직책
                    string targetJobName = ui.JobCodeName;  //받는 사람 직책
                    if (u.JobCodeName == null)
                    {
                        senderJobName = "";
                    }

                    if (ui.JobCodeName == null)
                    {
                        targetJobName = "";
                    }

                    if (type == "GG")
                    {
                        NoteLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID;
                        NoteBody = "<html><body><font face='맑은고딕' size='2'>" + u.Name + " " + senderJobName + "이 " + ui.Name + " " + targetJobName + "께<br />"
                                    + "끌.모임 \"" + this.lbTitle.Text + "\"의 게시물을 보시도록 공유하기를 하셨습니다.<br />"
                                    + "<font face='맑은고딕' size='2'><a href='" + NoteLink + "'>공유된 내용확인 바로가기</a></font></body></html>";
                    }
                    else
                    {
                        NoteLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + ItemID;
                        NoteBody = "<html><body><font face='맑은고딕' size='2'>안녕하세요. 티끌이입니다. ^^<br /><br />"
                                    + u.Name + " 께서 끌.지식을 공유하였습니다.<br /><br />"
                                    + "<font face='맑은고딕' size='2'><a href='" + NoteLink + "'>▶ 끌.지식 바로가기</a></font></body></html>";
                    }



                    //CBHMSMQHelper helper = new CBHMSMQHelper();
                    CBHNoteType data = new CBHNoteType();
                    data.Content = NoteBody;
                    data.Kind = "3"; //일반쪽지.
                    data.URL = NoteLink;
                    data.SendUserName = "티끌이";

                    string userID = Recipient.Remove(Recipient.IndexOf('@')); //이메일 앞부분이 note id 값이다.
                    data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                    data.TargetUser = userID;
                    //OK//helper.SendNoteToQueue(data);

                    //쪽지 20170802
                    CBHInterface.CBHNoteSend(data);

                    //메일 20170802
                    CBHInterface.CBHMailSend(Recipient, "tikle@sk.com", "T.끌 알림 메일입니다.", NoteBody);
                }
            }
        }


        private String fileFolder = string.Empty;
        private String fileItemID = string.Empty;

        //저장된 파일정보를 가져와 바인딩 처리
        private void GetAttachInfo(string GlossaryID)
        {
            //DextUpload
            DataSet attachList = AttachmentHelper.SelectAttach(Convert.ToInt32(GlossaryID), 0, GLOSSARY_ATTACH_ID);

            DataTable dt = new DataTable();

            dt.Columns.Add("FileKey", typeof(string)); //attachid
            dt.Columns.Add("BoardID", typeof(string)); //boardid
            dt.Columns.Add("ContentID", typeof(int)); //weeklyid
            dt.Columns.Add("FileType", typeof(string)); //
            dt.Columns.Add("FileName", typeof(string)); //파일명
            dt.Columns.Add("FileExt", typeof(string)); //확장자
            dt.Columns.Add("FileSize", typeof(int)); //사이즈
            dt.Columns.Add("Filepath", typeof(string)); //경로
            dt.Columns.Add("RegDt", typeof(DateTime));//등록일자
            int multifilesize = 0;

            if (attachList != null)
            {
                for (int i = 0; i < attachList.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["FileKey"] = attachList.Tables[0].Rows[i]["AttachID"];

                    dr["BoardID"] = attachList.Tables[0].Rows[i]["BoardID"];
                    dr["ContentID"] = attachList.Tables[0].Rows[i]["ItemID"];
                    dr["FileType"] = "A";
                    dr["FileName"] = attachList.Tables[0].Rows[i]["FileName"];
                    dr["FileExt"] = attachList.Tables[0].Rows[i]["Extension"].ToString().Replace(".", "").ToUpper();
                    dr["FileSize"] = attachList.Tables[0].Rows[i]["FileSize"];

                    multifilesize = Convert.ToInt32(dr["FileSize"]);

                    dr["Filepath"] = "/SKT_MultiUploadedFiles/" + attachList.Tables[0].Rows[i]["ServerFileName"].ToString().Replace(@"\", "/"); ;
                    dr["RegDt"] = DateTime.Now;


                    fileFolder = attachList.Tables[0].Rows[i]["Folder"].ToString();
                    fileItemID = attachList.Tables[0].Rows[i]["ItemID"].ToString();

                    dt.Rows.Add(dr);


                    hidFilePathGuid.Value = attachList.Tables[0].Rows[i]["ItemGuid"].ToString();
                }

                //this.multi_filecount.Text = attachList.Tables[0].Rows.Count.ToString();
                //this.multi_filesize.Text = multifilesize.ToString();

            }


            if (dt.Rows.Count > 0)
            {

                // 첨부 파일 바인딩
                //GlossaryfileCtrl.FileDataBind(dt);
            }
        }

        //첨부 목록
        [System.Web.Services.WebMethod]
        public static string GetAttachInfo2(string GlossaryID)
        {
            //DextUpload
            List<Attach> attachList = AttachmentHelper.Select(Convert.ToInt32(GlossaryID), 0, GlossaryView.GLOSSARY_ATTACH_ID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(attachList);
        }

        //2015.10.26 zz17779 : 파일업로드 변경관련 추가
        //2016.06.29, nas 변경
        protected void btnMulti_Click_back(object sender, EventArgs e)
        {
            string glossaryID = this.hidGlossaryID.Value;
            DataSet attachList = AttachmentHelper.SelectAttach(Convert.ToInt32(glossaryID), 0, 314);


            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                if (attachList != null)
                {
                    byte[] b = null;
                    string entry = null;
                    for (int i = 0; i < attachList.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = attachList.Tables[0].Rows[i];

                        if (hidChkAllFlag.Value == "Y")
                        {


                            string filePath = @"D:\SKT_MultiUploadedFiles\" + row["ServerFileName"].ToString();


                            //// 시스템의 기본 인코딩 타입으로 읽어서
                            b = System.Text.Encoding.Default.GetBytes(row["FileName"].ToString());
                            // IBM437로 변환해 준다.
                            entry = System.Text.Encoding.GetEncoding("IBM437").GetString(b);


                            zip.AddEntry(entry, File.ReadAllBytes(filePath));



                            //zip.AddEntry(entry, "D:\\SKT_MultiUploadedFiles\\" + row["ServerFileName"].ToString(), UTF8Encoding.UTF8);
                        }
                        else
                        {
                            string[] attachid = this.hidFileChkAttachID.Value.Split('$');
                            for (int j = 0; j < attachid.Length; j++)
                            {
                                if (row["AttachID"].ToString() == attachid[j].ToString())
                                {

                                    string filePath = @"D:\SKT_MultiUploadedFiles\" + row["ServerFileName"].ToString();


                                    // 시스템의 기본 인코딩 타입으로 읽어서
                                    b = System.Text.Encoding.Default.GetBytes(row["FileName"].ToString());
                                    // IBM437로 변환해 준다.
                                    entry = System.Text.Encoding.GetEncoding("IBM437").GetString(b);


                                    zip.AddEntry(entry, File.ReadAllBytes(filePath));


                                    //zip.AddEntry(entry, "D:\\SKT_MultiUploadedFiles\\" + row["ServerFileName"].ToString(), UTF8Encoding.UTF8);
                                }
                            }
                        }
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Glossary_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);

                zip.Save(Response.OutputStream);
                Response.End();
            }
        }

        /*
            Author : 개발자-장찬우G, 리뷰자-진현빈D 
            Create Date : 2016.06.29 
            Desc : nas장비로 변경
        */
        protected void btnMulti_Click(object sender, EventArgs e)
        {
            string glossaryID = this.hidGlossaryID.Value;
            DataSet attachList = AttachmentHelper.SelectAttach(Convert.ToInt32(glossaryID), 0, 314);

            string NAS_VirtualDirectory = ConfigReader.GetString("NAS_VirtualDirectory");
            string NAS_PhysicalPath = Server.MapPath("/" + NAS_VirtualDirectory);
            // attachList테이블 row값에 SKT_MultiUploadedFiles 이름이 중복이 되서 replace시킴
            NAS_PhysicalPath = NAS_PhysicalPath.Replace(NAS_VirtualDirectory, "");

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                if (attachList != null)
                {
                    byte[] b = null;
                    string entry = null;
                    for (int i = 0; i < attachList.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = attachList.Tables[0].Rows[i];

                        if (hidChkAllFlag.Value == "Y")
                        {

                            /*
                            Author : 개발자-장찬우G, 리뷰자-진현빈D 
                            Create Date : 2016.06.29 
                            Desc : 하드코딩제거후, nas장비로 변경
                            */
                            //string filePath = @"D:\SKT_MultiUploadedFiles\" + row["ServerFileName"].ToString();
                            string filePath = string.Format(@"{0}SKT_MultiUploadedFiles\{1}", NAS_PhysicalPath, row["ServerFileName"].ToString());

                            //// 시스템의 기본 인코딩 타입으로 읽어서
                            b = System.Text.Encoding.Default.GetBytes(row["FileName"].ToString());
                            // IBM437로 변환해 준다.
                            entry = System.Text.Encoding.GetEncoding("IBM437").GetString(b);

                            /*
                            Author : 개발자-장찬우G, 리뷰자-진현빈D 
                            Create Date : 2016.06.29 
                            Desc : Impersonation
                            */
                            //zip.AddEntry(entry, File.ReadAllBytes(filePath));
                            Impersonation im = new Impersonation();
                            im.ImpersonationStart();
                            try
                            {
                                zip.AddEntry(entry, File.ReadAllBytes(filePath));
                            }
                            catch (Exception ex)
                            {
                                im.ImpersonationEnd();
                                throw ex;
                            }
                            im.ImpersonationEnd();


                            //zip.AddEntry(entry, "D:\\SKT_MultiUploadedFiles\\" + row["ServerFileName"].ToString(), UTF8Encoding.UTF8);
                        }
                        else
                        {
                            string[] attachid = this.hidFileChkAttachID.Value.Split('$');
                            for (int j = 0; j < attachid.Length; j++)
                            {
                                if (row["AttachID"].ToString() == attachid[j].ToString())
                                {

                                    /*
                                    Author : 개발자-장찬우G, 리뷰자-진현빈D 
                                    Create Date : 2016.06.29 
                                    Desc : 하드코딩제거후, nas장비로 변경
                                    */
                                    //string filePath = @"D:\SKT_MultiUploadedFiles\" + row["ServerFileName"].ToString();
                                    string filePath = string.Format(@"{0}SKT_MultiUploadedFiles\{1}", NAS_PhysicalPath, row["ServerFileName"].ToString());

                                    // 시스템의 기본 인코딩 타입으로 읽어서
                                    b = System.Text.Encoding.Default.GetBytes(row["FileName"].ToString());
                                    // IBM437로 변환해 준다.
                                    entry = System.Text.Encoding.GetEncoding("IBM437").GetString(b);

                                    /*
                                    Author : 개발자-장찬우G, 리뷰자-진현빈D 
                                    Create Date : 2016.06.29 
                                    Desc : Impersonation
                                    */
                                    //zip.AddEntry(entry, File.ReadAllBytes(filePath));
                                    Impersonation im = new Impersonation();
                                    im.ImpersonationStart();
                                    try
                                    {
                                        zip.AddEntry(entry, File.ReadAllBytes(filePath));
                                    }
                                    catch (Exception ex)
                                    {
                                        im.ImpersonationEnd();
                                        throw ex;
                                    }
                                    im.ImpersonationEnd();

                                    //zip.AddEntry(entry, "D:\\SKT_MultiUploadedFiles\\" + row["ServerFileName"].ToString(), UTF8Encoding.UTF8);
                                }
                            }
                        }
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Glossary_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);

                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }
}