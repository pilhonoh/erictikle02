using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Controls;
using System.Security.Cryptography;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryWriteNew : System.Web.UI.Page
    {
        protected string mode = string.Empty;
        protected string ItemID = string.Empty;
        protected string CommonID = string.Empty;
        protected string Recipient = string.Empty;
        protected string RootURL = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string HtmlBody = string.Empty;
        protected string TutorialYN = string.Empty;
        protected string UserID = string.Empty; // 2014-05-12 Mr.No
        protected string UserNameDept = string.Empty; // 2014-06-09 Mr.No

        protected string AttachInfo = "[]";
        internal const int GLOSSARY_ATTACH_ID = 314;

        // 끌.모임 설정
        protected string GatheringYN;
        protected string GatheringID;

        protected string QnaID = "0";
        protected string TechTrendYN = "N";
        protected string TrendWriteYN = "N";
        protected int PageNum = 1;
        protected string TagTitle = string.Empty;
        protected string SearchSort = string.Empty;
        
        protected string WType = string.Empty;  //DT 블로그홈
        protected string TType = string.Empty;  //T생활백서
        protected string SchText = string.Empty; //DT 블로그홈 T생활백서 검색어

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            //2016-12-15 SSO 예외처리
            if (String.IsNullOrEmpty(u.UserID))
            {
                Response.Redirect("/Error.aspx?ErrCode=99999", false);
                Response.End();
            }

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;

            //수정하기 P097010
            mode = (Request["mode"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            CommonID = (Request["CommonID"] ?? string.Empty).ToString();
            
            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();
            PageNum = Convert.ToInt16((Request["PageNum"] ?? "1"));
            TagTitle = ((Request["TagTitle"] == null || Request["TagTitle"] == string.Empty) ? string.Empty : HttpUtility.UrlDecode(Request["TagTitle"])).ToString();
            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();
            
            WType = (Request["WType"] ?? string.Empty).ToString();
            TType = (Request["TType"] ?? string.Empty).ToString();

            //DT블로그홈 / T생활백서 검색어 유지
            if (!string.IsNullOrEmpty(WType) || !string.IsNullOrEmpty(TType))
            {
                SchText = (string.IsNullOrEmpty(Request["SchText"]) ? string.Empty : Request["SchText"]).ToString();
            }

            string Type = string.Empty;
            string ID = string.Empty;
            string SubJect = string.Empty;
            string Contents = string.Empty;

            // 권한 지정 시 그룹선택 허용 = true
            this.UserControl.UserGroupVisible = false;

            // 2014-05-12 Mr.No
            //ItemID = u.;
            UserID = u.UserID;

            if (!IsPostBack)
            {
                hdType.Value = "wiki";
                hdCategoryID.Value = "129";
                this.hidFileDeleteKey.Value = "";
                hdItemGuid.Value = Guid.NewGuid().ToString();

                //Author : 개발자-김성환D, 리뷰자-진현빈D
                //Create Date : 2016.05.18 
                //Desc : 기본 태그 리스트 가져오기
                DefaultTagList();

                GlossaryDac Dac = new GlossaryDac();
                //예전 임시저장했던 데이터 불러오기
                if (mode.Equals("MyTemp"))
                {
                    btnTempDelete.Visible = true;
                    MyTempSelect();
                    if (CommonID != "0")
                    {
                        Dac.GlossaryModifyYNUpdate(CommonID, "Write");
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(ItemID))
                    {
                        if (mode == "History")
                        {
                            Dac.GlossaryModifyYNUpdate(ItemID, "Write");
                            Reason.Visible = true;
                            BoardSelect();

                            GetAttachInfo(CommonID);
                            hdItemGuid.Value = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            //등록금지
                            if (GatheringYN.Equals("Y"))
                            {
                                Response.Redirect("/TikleMain.aspx");
                                Response.End();
                            }
                        }
                    }
                    else
                    {
                        //등록금지
                        if (GatheringYN.Equals("Y"))
                        {
                            Response.Redirect("/TikleMain.aspx");
                            Response.End();
                        }

                        this.hddActiveBody.Value = "<P style='MARGIN-TOP: 5px; MARGIN-BOTTOM: 2px'>&nbsp;</P>";
                    }
                }
            }
        }
        #endregion

        #region MyTempSelect()
        //임시문서 View
        private void MyTempSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryTempBiz biz = new GlossaryTempBiz();
            GlossaryTempType Board = biz.GlossaryTempSelect(ItemID);
            hdTitle.Value = SKT.Common.SecurityHelper.Add_XSS_CSRF(Board.Title);

            Board.Contents = Board.Contents.Replace("\r\n", "\n");
            this.hddActiveBody.Value = HttpUtility.HtmlDecode(Board.Contents);

            //액티브스퀘어 디자인에 맞추어 변경함
            this.hddActiveBody.Value = this.hddActiveBody.Value.Replace("<p>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");
            this.hddActiveBody.Value = this.hddActiveBody.Value.Replace("<P>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");

            hdCategoryID.Value = Board.CategoryID.ToString(); //2014-05-08 Mr.No        
            // 2014-06-13
            
            //임시저장 권한관리 테이블 없음
            //hdPermissions.Value = Board.Permissions;

            //if (Board.Permissions.Equals("SomePublic"))
            //{
            //    UserControl.Visible = true;
            //    GlossaryPermissionsTempBiz permissionsBiz = new GlossaryPermissionsTempBiz();
            //    List<PermissionsTempType> info = permissionsBiz.PermissionsTempSelect(Convert.ToInt32(Board.ID));
            //    hdPermissionsString.Value = JsonConvert.SerializeObject(info);
            //}

            //if (!String.IsNullOrEmpty(CommonID))
            //{
            //    GlossaryControlDac Dac = new GlossaryControlDac();
            //    DataSet ds = new DataSet();
            //    ds = Dac.GlossaryTagSelect(CommonID);
            //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in ds.Tables[0].Rows)
            //        {
            //            txtTag.Value += SKT.Common.SecurityHelper.Clear_XSS_CSRF(dr["TagTitle"].ToString()) + ",";
            //        }

            //        txtTag.Value = txtTag.Value.Remove(txtTag.Value.LastIndexOf(","));
            //    }
            //}
        }
        #endregion

        #region BoardSelect
        private void BoardSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryType Board = biz.GlossarySelect(CommonID, u.UserID, mode);

            // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
            if (Board.DTBlogFlag.Equals("D"))
            {
                if (u.IsDTPermission == false)
                {
                    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 게시글에 대한 권한이 없습니다.", false);
                    Response.End();
                }
            }
            

            hdTitle.Value = SKT.Common.SecurityHelper.Add_XSS_CSRF(Board.Title);

            //SKT.Tnet.Controls.WebEditorData wed = new WebEditorData();
            //wed = GetDecodeMIME(hddActiveBody.Value, "TempImage");

            //Board.ContentsModify = ChangeCutSummaryBox(SKT.Common.SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(Board.ContentsModify)));
            Board.Contents = Board.Contents.Replace("\r\n", "\n");
            this.hddActiveBody.Value = HttpUtility.HtmlDecode(Board.Contents);

            //액티브스퀘어 디자인에 맞추어 변경함
            this.hddActiveBody.Value = this.hddActiveBody.Value.Replace("<p>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");
            this.hddActiveBody.Value = this.hddActiveBody.Value.Replace("<P>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");
            
            //old
            //this.hdNamoContent.Value = Board.Contents;
            ////Editor 변수명 변경과 디코딩된 문자열로 변환 Mostisoft 2015.08.21
            //NamoEditor.Contents = SKT.Common.SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(Board.Contents.Replace("\r\n", string.Empty)));
            ////Editor.Contents = SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(Board.Contents.Replace("\r\n", string.Empty)));

            CommonID = Board.CommonID;
            HtmlBody = Board.Contents;
            Recipient = Board.UserEmail;
            QnaID = Board.fromQnaID;

            GlossaryMainBiz mainbiz = new GlossaryMainBiz();
            DataSet ds1 = mainbiz.OfficerCheck(u.UserID);
            if (ds1.Tables[1].Rows.Count > 0)
            {
                TrendWriteYN = ds1.Tables[1].Rows[0]["WriteYN"].ToString();
            }
            //2015-11-10 ksh 테크 트렌드면 폼이 보이도록 한다.
            TechTrendYN = Board.TechTrendYN;

            if (Board.JustOfficerYN == "Y")
            {
                rd_all.Checked = false;
                rd_officer.Checked = true;
            }

            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = new DataSet();
            ds = Dac.GlossaryTagSelect(CommonID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtTag.Value += SKT.Common.SecurityHelper.Clear_XSS_CSRF(dr["TagTitle"].ToString()) + ",";
                }

                txtTag.Value = txtTag.Value.Remove(txtTag.Value.LastIndexOf(","));

                
            }
            // 2014-04-29 Mr.No 추가
            hdCategoryID.Value = Board.CategoryID.ToString();
            // 2014-05-15 Mr.No 추가
            hdPermissions.Value = Board.Permissions;
            if (Board.Permissions.Equals("SomePublic"))
            {
                UserControl.Visible = true;
                GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
                List<PermissionsType> info = permissionsBiz.PermissionsSelect(Convert.ToInt32(CommonID));

                // 모임 권한 추가
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                List<PermissionsType> GatheringInfo = gatheringBiz.GatheringMenuAuth_Select(Convert.ToInt32(CommonID), "Knowledge");

                foreach (PermissionsType pt in GatheringInfo)
                {
                    info.Add(pt);
                }

                hdPermissionsString.Value = JsonConvert.SerializeObject(info);
            }

            hdDTBlog.Value = Board.DTBlogFlag;
            hdTWhite.Value = Board.TWhiteFlag;

            //if (!String.IsNullOrEmpty(Request["CommonID"])) { ItemID = Request["CommonID"]; }
            //// 2014-05-23 Mr.Mo
            //// DEXTupload 
            //List<Attach> attachList = AttachmentHelper.Select(Convert.ToInt64(ItemID), GLOSSARY_ATTACH_ID); //이거 저장되어 있는 첨부파일 갯수만큼 불러오기
            //this.AttachInfo = Newtonsoft.Json.JsonConvert.SerializeObject(attachList);

            //// 2014-05-26
            //GlossaryPermissionsBiz permissionsBizTwo = new GlossaryPermissionsBiz();
            //if (permissionsBizTwo.Permissions_Check(ItemID, u.UserID.ToString()) == 3) { hdPermisstionCheck.Value = "show"; }
            //else { hdPermisstionCheck.Value = "hide"; }

        }
        #endregion

        #region DefaultTagList
        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 기본 태그 리스트 가져오기
        protected void DefaultTagList()
        {
            GlossaryDac Dac = new GlossaryDac();
            DataTable dt = Dac.GetBasicTagList().Tables[0];

            string li_msg = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //li_msg += "<li onclick='txtTagSelectEvent('" + dt.Rows[i]["TagTitle"].ToString() + "');'>" + dt.Rows[i]["TagTitle"].ToString() + "</li>";
                li_msg += "<li onclick='txtTagSelectEvent(\"" + dt.Rows[i]["TagTitle"].ToString() + "\")' >" + dt.Rows[i]["TagTitle"].ToString() + "</li>";
            }

            ulDefaultTag.InnerHtml = li_msg;
        }
        #endregion

        #region btnSave_Click
        //저장
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            //작성중인 지식은 신규글로 저장된다.
            if (mode.Equals("MyTemp"))
            {
                GlossaryTempDac Dac = new GlossaryTempDac();
                Dac.GlossaryTempDelect(ItemID);

                mode = string.Empty; 
                CommonID = string.Empty;
                ItemID = string.Empty;
            }

            GlossaryType Board = new GlossaryType();
            GlossaryBiz biz = new GlossaryBiz();
            Board.ID = CommonID;
            Board.CommonID = CommonID;
            Board.UserID = u.UserID;    //작성자 ID
            Board.UserName = u.Name;    //작성자 이름
            Board.DeptName = u.DeptName;
            Board.UserEmail = u.EmailAddress;   //작성자 이메일
            Board.Title = SKT.Common.SecurityHelper.Clear_XSS_CSRF(hdTitle.Value).Trim(); //제목

            //new 
            SKT.Tnet.Controls.WebEditorData wed = new WebEditorData();
            wed = SKT.Common.CommonActiveSquareEditor.GetDecodeMIME(hddActiveBody.Value, "TempImage");

            string strContents = SKT.Common.CommonActiveSquareEditor.ConvertHtmlBlank(wed.HtmlBody);

            Board.Contents = Utility.WeeklyRemoveBackgroundColor(SKT.Common.SecurityHelper.Clear_XSS_CSRF(strContents).Trim());
            Board.ContentsModify = Board.Contents;

            string sumary = hddActiveBodyText.Value.Replace("\r", "");
            sumary = sumary.Replace("\n", "");
            sumary = sumary.Replace("\t", "");

            //Board.Summary = Utility.RemoveHtmlTag(strContents.Replace("</div>", "</div>\r\n").Replace("</p>", "</p>\r\n")).Trim();
            Board.Summary = sumary.Trim();

            //old
            //hdNamoContent.Value = hdNamoContent.Value.InsertNBSP();
            //Board.Contents = SKT.Common.SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            //Board.ContentsModify = SKT.Common.SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용
            //Board.Summary = Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용

            if (string.IsNullOrEmpty(ItemID))
                Board.Description = "처음 작성된 글입니다";
            else
            {
                Board.Description = SKT.Common.SecurityHelper.Clear_XSS_CSRF(txtReason.Value).Trim();
                if (string.IsNullOrEmpty(txtReason.Value))
                    Board.Description = "변경내용 미 작성";
            }

            Board.PrivateYN = "N";
            Board.HistoryYN = "N";
            Board.ItemState = hidItemState.Value;
            Board.CategoryID = Convert.ToInt32(hdCategoryID.Value);
            Board.Type = hdType.Value;

            Board.PlatformYN = hddPlatformYN.Value;
            Board.MarketingYN = hddMarketingYN.Value;
            Board.TechTrendYN = hddTechTrendYN.Value;

            //Tech Trend 일경우 권한 부분
            Board.JustOfficerYN = rd_all.Checked == true ? "N" : "Y";

            // CHG610000070396/ 2018-09-13 / 최현미 / DT블로그홈
            if (!string.IsNullOrEmpty(this.WType)) 
                Board.DTBlogFlag = this.WType;
            if (!string.IsNullOrEmpty(hdDTBlog.Value))
                Board.DTBlogFlag = hdDTBlog.Value;

            // CHG610000074852/ 2018-11-08 / 최현미 / T생활백서
            if (!string.IsNullOrEmpty(this.TType))
                Board.TWhiteFlag = this.TType;
            if (!string.IsNullOrEmpty(hdTWhite.Value))
                Board.TWhiteFlag = hdTWhite.Value;

            
            if (GatheringYN == "Y")
            {
                hdPermissions.Value = "GatheringPublic";
            }
            Board.Permissions = hdPermissions.Value;

            #region 닉네임설정 [끌지식/끌모임]
            if (HaveNick.Checked == true)
            {
                Board.PrivateYN = "Y";
                //Board.UserID = "";    //작성자 ID
                //Board.UserName = "비공개";    //작성자 이름
                Board.UserName = hdUserNikName.Value;
                Board.DeptName = "비공개부서";

                if (GatheringYN != "Y")
                {
                    // 작성자 비공개이면 무조건 전체공개
                    Board.Permissions = "FullPublic";
                }
            }
            #endregion

            Board = biz.GlossaryInsert(Board, mode);

            if (string.IsNullOrEmpty(ItemID))
            {
                Board.CommonID = Board.ID;
            }

            //저장후 내가작성한것은  알람을 설정.  새로운저장이면 commonid 가 id 와 같으므로 아이디를넘겨줌
            GlossaryAlarm(Board.CommonID, u.UserID, "N", "N");

            #region 태그 저장
            GlossaryControlDac dac = new GlossaryControlDac();
            dac.GlossaryTagDelete((Request["CommonID"] ?? Board.ID).ToString());
            if (!string.IsNullOrEmpty(hdTag.Value.Trim()))
            {
                GlossaryControlType ConBoard = new GlossaryControlType();
                ConBoard.CommonID = Board.CommonID;
                ConBoard.Title = Board.Title;
                ConBoard.UserID = u.UserID;

                if (!(hdTag.Value.IndexOf(',') == -1))
                {
                    string[] Tag = hdTag.Value.Split(',');
                    for (int i = 0; i < Tag.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Tag[i].Trim()))
                        {
                            //ConBoard.TagTitle = Tag[i].Remove(Tag[i].LastIndexOf(",")).Trim();
                            ConBoard.TagTitle = Tag[i];
                            dac.GlossaryTagInsert(ConBoard);
                        }
                    }
                }
                else
                {
                    ConBoard.TagTitle = hdTag.Value.Trim();
                    dac.GlossaryTagInsert(ConBoard);
                }

                dac.GlossaryTagTotal();
            }
            #endregion

            #region 조회권한설정
            
            //조회권한설정
            GlossaryShareBiz glossaryShareBiz = new GlossaryShareBiz();
            glossaryShareBiz.GlossaryShareDelete_GlossaryID(Convert.ToInt32(Board.CommonID));

            GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            permissionsBiz.PermissionsDelete(Convert.ToInt32(Board.CommonID));

            //끌모임 권한
            if (hdPermissions.Value == "GatheringPublic")
            {
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Knowledge", Board.CommonID);
            }

            //끌지식 일부공개
            if (hdPermissions.Value == "SomePublic")
            {
                SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;
                new GlossaryPermissionsBiz().PermissionsInsert(Board.CommonID, UDList.AuthID, UDList.AuthName, UDList.AuthCL, Board.CommonID);
            }
            
            #endregion

            #region 첨부파일 
            Guid itemGuid = Guid.Empty;
            GuidTryParse(hdItemGuid.Value, out itemGuid);

            if (string.IsNullOrEmpty(ItemID))
            {
                AttachmentHelper.UpdateByItemGuid(itemGuid, Convert.ToInt32(Board.ID), GlossaryWriteNew.GLOSSARY_ATTACH_ID);
            }

            //첨부파일 저장
            this.GetSaveFilesInfo(Board.CommonID.ToString());

            //첨부파일 삭제
            if (this.hidFileDeleteKey.Value != "")
            {
                string[] fileDeletes = this.hidFileDeleteKey.Value.Split('$');

                foreach (string attachid in fileDeletes)
                {
                    //3-3.DB에서 삭제 표시한다
                    AttachmentHelper.Delete(attachid);
                }

            }
            #endregion

            #region 검색서버전송
            //검색서버전송 - 신규저장일경우만 
            //if (ConfigurationManager.AppSettings["IsTestServer"].ToString() == "N")
            //{
            //    if (string.IsNullOrEmpty(ItemID))
            //    {
            //        GlossarySearchBiz bizGlossarySearch = new GlossarySearchBiz();
            //        bizGlossarySearch.SetSearchGlossarySyncDataUpdate("Glossary", Board.CommonID);
            //    }
            //}
            #endregion

            #region 끌모임 쪽지발송
            ////P097010 BACKUP2
            ////Author : 개발자-최현미, 리뷰자-윤자영
            ////Create Date : 2017.04.06 
            ////Desc : 끌.모임 신규 작성 알림설정한 멤버들에게 쪽지를 발송한다 
            //if (GatheringYN.Equals("Y") && string.IsNullOrEmpty(ItemID))
            //{
            //    SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(GatheringID, Board.CommonID, "0", u.UserID, "", "WRITE");
            //}
            #endregion

            #region CHG610000073115 / 2018-10-11 / 최현미 / ContentFeeds
            if (ConfigurationManager.AppSettings["TnetContentFeeds"].ToString().Equals("Y"))
            {
                OpinionBoardFeed bsObj = new OpinionBoardFeed();
                string BaseUrl = ConfigurationManager.AppSettings["BaseURL"].ToString();

                bsObj.sbmId = "TKE@100@" + Board.CommonID;
                //DateTime myDate = DateTime.Parse(Board.FirstCreateDate);
                bsObj.pushTime = DateTime.Now.ToString("yyyyMMddHHmm");
                bsObj.writeProfile = u.UserID;
                bsObj.contentTitle = TitleCut(Board.Title, 100).Trim();

                //CHG610000078167 / 본문내용 제한 없음 / 2019-01-10 / 최현미
                //bsObj.contentCont = TitleCut(sumary, 500).Trim();
                bsObj.contentCont = Board.Summary;

                //CHG610000081447 / 내용이 없을 경우 제목 대체 / 2019-03-07  
                if (string.IsNullOrWhiteSpace(bsObj.contentCont))
                {
                    bsObj.contentCont = bsObj.contentTitle;
                }

                bsObj.linkUrl = BaseUrl + "Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID;
                List<ReadRole> readRoleList = new List<ReadRole>();

                if (this.hdPermissions.Value == "SomePublic")
                {
                    SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;

                    string[] recvType = UDList.AuthCL.Split('/');
                    string[] recvID = UDList.AuthID.Split('/');

                    string userlist = string.Empty;
                    string deptlist = string.Empty;

                    for (int i = 0; i < recvType.Length; i++)
                    {
                        if (recvType[i].Trim().Equals("U"))
                            userlist = userlist + recvID[i].ToString() + ",";
                        if (recvType[i].Trim().Equals("O"))
                            deptlist = deptlist + recvID[i].ToString() + ",";
                    }
                    if (userlist.Length > 0)
                    {
                        ReadRole readRole = new ReadRole();
                        readRole.type = "user";
                        readRole.code = userlist.Substring(0, userlist.Length - 1);
                        readRoleList.Add(readRole);
                    }
                    if (deptlist.Length > 0)
                    {
                        ReadRole readRole = new ReadRole();
                        readRole.type = "dept";
                        readRole.code = deptlist.Substring(0, deptlist.Length - 1);
                        readRoleList.Add(readRole);
                    }
                }
                else 
                {
                    if (this.WType.Equals("D"))
                    {
                        ReadRole readRole1 = new ReadRole();
                        readRole1.type = "COMMON";
                        readRole1.code = "EXECUTIVE,COMMON";
                        readRoleList.Add(readRole1);
                    }
                    else
                    {
                        //EXECUTIVE	임원(107사번 사용자 및 임원 직책자) COMMON 정직원(11사번사용자(팀장포함)) CONTRACT 계약직(15사번) 
                        ReadRole readRole = new ReadRole();
                        readRole.type = "COMMON";
                        readRole.code = "EXECUTIVE,COMMON,CONTRACT";
                        readRoleList.Add(readRole);

                        readRole = new ReadRole();
                        readRole.type = "user";
                        readRole.code = "1901080,1901081,1901082,1901083";
                        readRoleList.Add(readRole);
                    }
                }
                bsObj.readRole = readRoleList;
                bsObj.replyYn = "Y";
                bsObj.replyType = "400"; //기명 or 익명

                LikeYN likeYn = new LikeYN();
                likeYn.ADD = "N";
                likeYn.CANCEL = "N";
                bsObj.likeYn = likeYn;

                if (this.HaveNick.Checked)
                    bsObj.nickName = this.hdUserNikName.Value;
                else
                    bsObj.nickName = string.Empty;

                List<FeedFileInfo> feedFileInfoList = new List<FeedFileInfo>();
                DataSet ds = AttachmentHelper.SelectAttach(Convert.ToInt32(Board.CommonID), 0, GLOSSARY_ATTACH_ID);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        FeedFileInfo tmpfeedFileInfo = new FeedFileInfo();
                        int length = Convert.ToInt32(row["FileSize"].ToString());
                        if (length >= (1 << 10))
                            tmpfeedFileInfo.Size = string.Format("{0}Kb", length >> 10);

                        tmpfeedFileInfo.fileType = Path.GetExtension(row["FileName"].ToString()).Substring(1);
                        tmpfeedFileInfo.Name = row["FileName"].ToString();

                        string fileName = row["FileName"].ToString();
                        string fileFolder = "/SKT_MultiUploadedFiles/" + row["ServerFileName"].ToString().Replace("\\", "/");
                        tmpfeedFileInfo.url = BaseUrl + "Common/Controls/FileDownload.aspx?FileName=" + Server.UrlEncode(fileName) + "&FilePath=" + Server.UrlEncode(fileFolder);

                        //tmpfeedFileInfo.url = BaseUrl + "SKT_MultiUploadedFiles/" + row["ServerFileName"].ToString().Replace("\\", "/");
                        feedFileInfoList.Add(tmpfeedFileInfo);
                    }
                }
                bsObj.feedFileInfo = feedFileInfoList;
                if (string.IsNullOrEmpty(this.ItemID))
                    ContentFeeds.SendFeeds(bsObj, "POST");
                else
                    ContentFeeds.SendFeeds(bsObj, "PUT");
            }
            #endregion

            //최신지식의 경우는 항상 최신글로 되므로 PageNum을 1로 설정함.
            //Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID + "&SearchSort="+ SearchSort+"&TagTitle=" + HttpUtility.UrlEncode(TagTitle) + "&PageNum=1&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID+"&WType="+WType);

            if (Request["WType"] != null)
                Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID + "&SearchSort=" + SearchSort + "&TagTitle=" + HttpUtility.UrlEncode(TagTitle) + "&PageNum=1&WType=" + WType + "&SchText="+ SchText);
            else if (Request["TType"] != null)
                Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID + "&SearchSort=" + SearchSort + "&TagTitle=" + HttpUtility.UrlEncode(TagTitle) + "&PageNum=1&TType=" + TType + "&SchText=" + SchText);
            else
                Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID + "&SearchSort=" + SearchSort + "&TagTitle=" + HttpUtility.UrlEncode(TagTitle) + "&PageNum=1");

        }
        
        #endregion

        #region btnList_Click
        //목록 - ModifyYN /수정
        protected void btnList_Click(object sender, EventArgs e)
        {
            GlossaryDac Dac = new GlossaryDac();
            Dac.GlossaryModifyYNUpdate(ItemID, "List");

            Response.Redirect("/Glossary/GlossaryList.aspx");
        }
        #endregion

        #region GlossaryAlarm
        //알람설정.
        //AjaxControl.aspx 에도 동일한기능존재한다 차후 수정이있으면 같이 수정해야함. 주ㅡ의!
        public static string GlossaryAlarm(string CommonID, string UserID, string MailSet, string NoteSet)
        {
            GlossaryControlType Board = new GlossaryControlType();
            GlossaryControlDac Dac = new GlossaryControlDac();
            Board.UserID = UserID;
            Board.MailYN = MailSet;
            Board.NoteYN = NoteSet;
            Board.CommonID = CommonID;
            Dac.GlossaryAlarmInsert(Board);
            DataSet ds = new DataSet();
            return "";
        }
        #endregion

        #region GuidTryParse
        // 2014-05-21 Mr.No
        // Guid.TryParse 를 닷넷 4.5 부터 지원하여 새로 만듬
        public static bool GuidTryParse(string s, out Guid result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = Guid.Empty;
                return false;
            }
            else
            {
                Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
                Match match = format.Match(s);
                if (match.Success)
                {
                    result = new Guid(s);
                    return true;
                }
                else
                {
                    result = Guid.Empty;
                    return false;
                }
            }
        }
        #endregion

        #region GetSaveFilesInfo
        /// <summary>
        /// 저장 파일의 정보를 가져온다.
        /// 해당 Item과 함께 저장할 파일 정보를 가져온다. 
        /// 저장시 하나의 파일에 대하여 "[FileKey]￥[FileType]￥[FileName]￥[FileExt]￥[FileSize]￥[FilePath]¶" 형식으로 하나의 string로 취합
        /// DB에서 일과 저장 하여 준다. 
        /// FileType : 파일의 유형으로   A: 첨부 파일,  B:WebEdit의 인라인 이미지 로 구분
        /// </summary>
        /// <returns></returns>
        public void GetSaveFilesInfo(string GlossaryID)
        {
            string filePathGuid = string.Empty;

            filePathGuid = string.IsNullOrEmpty(hidFilePathGuid.Value) ? Guid.NewGuid().ToString() : hidFilePathGuid.Value;
            GlossaryfileCtrl.SavePath = "Glossary/" + filePathGuid;

            //첨부파일 저장
            foreach (Dictionary<string, object> file in GlossaryfileCtrl.Save())
            {

                Guid itemGuid = Guid.Empty;
                GuidTryParse(filePathGuid, out itemGuid);

                Attach attach = new Attach();
                attach.AttachID = string.Empty;
                attach.BoardID = "314"; //위클리는 315
                attach.ItemID = GlossaryID; // Board.WeeklyID.ToString()위클리아이디
                attach.AttachType = AttachType.AttachmentFile;

                attach.FileName = file["FileName"].ToString();
                attach.ServerFileName = "Glossary\\" + filePathGuid + "\\" + file["FileName"].ToString();
                attach.Extension = "." + file["FileExt"].ToString().ToLower();
                attach.Folder = "Glossary\\" + filePathGuid;
                attach.FileSize = Convert.ToInt64(file["FileSize"].ToString());
                attach.ItemGuid = itemGuid;
                attach.DeleteYN = "N";


                ////1-3. DB에 기록한다
                attach.AttachID = AttachmentHelper.InsertWithItemGuid(attach).ToString();
            }
        }

        #endregion

        #region DeleteFiles
        /// <summary>
        /// 삭제 대상 파일을 실제 저장 경로에서 삭제 하여 준다.
        /// </summary>
        /// <param name="dt"></param>
        public void DeleteFile(DataTable dt)
        {
            //사용자 가장
            Impersonation im = new Impersonation();
            im.ImpersonationStart();

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if (File.Exists(Server.MapPath(dr["FilePath"].ToString())))
                    {
                        File.Delete(Server.MapPath(dr["FilePath"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogSourceType.WebPage, "TnetBuzzEdit", ex);
                }

            }

            im.ImpersonationEnd();
        }
        #endregion

        #region GetAttachInfo
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
                    dr["Filepath"] = "/SKT_MultiUploadedFiles/" + attachList.Tables[0].Rows[i]["ServerFileName"].ToString().Replace(@"\", "/"); ;
                    dr["RegDt"] = DateTime.Now;

                    fileFolder = attachList.Tables[0].Rows[i]["Folder"].ToString();
                    fileItemID = attachList.Tables[0].Rows[i]["ItemID"].ToString();

                    dt.Rows.Add(dr);

                    hidFilePathGuid.Value = attachList.Tables[0].Rows[i]["ItemGuid"].ToString();
                }
            }

            if (dt.Rows.Count > 0)
            {
                // 첨부 파일 바인딩
                GlossaryfileCtrl.FileDataBind(dt);
            }
        }
        #endregion

        #region btnTempDelete_Click
        //임시저장 삭제
        protected void btnTempDelete_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryTempDac Dac = new GlossaryTempDac();
            Dac.GlossaryTempDelect(ItemID);
            //Response.Redirect("/GlossaryMyPages/MyTempList.aspx");
            Response.Redirect("/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=" + u.UserID);
        }
        #endregion

        private static string TitleCut(string str, int len)
        {
            str = str.Replace("&nbsp;", "");
            str = str.Replace("\r\n", "");
            int byteCount = Encoding.Default.GetByteCount(str);
            if (byteCount > len)
            {
                int index = str.Length - 1;
                while (byteCount > len)
                {
                    if ((int)str[index] > (int)sbyte.MaxValue)
                        byteCount -= 2;
                    else
                        --byteCount;
                    --index;
                }
                str = str.Substring(0, index + 2);
                byteCount = Encoding.Default.GetByteCount(str);
            }
            str = str.PadRight(str.Length + len - byteCount);
            return str;
        }

    }
}