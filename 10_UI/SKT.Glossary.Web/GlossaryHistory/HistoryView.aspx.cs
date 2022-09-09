using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using System.Configuration;
using System.Data;
using SKT.Common;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace SKT.Glossary.Web.GlossaryControl
{
    public class Position
    {
        public int DiffStartIndex;
        public int DiffEndIndex;

        public int DiffHtmlStartIndex;
        public int DiffHtmlEndIndex;

        public string DiffString;

        public List<int> SummarySameStringIndexlist; // 오리지날 스트링같은 것이 잇으면 해당 인덱스가 무엇인지 저장.
        public List<int> HtmlSameStringIndexlist; // 오리지날 스트링같은 것이 잇으면 해당 인덱스가 무엇인지 저장.

        public void MakeSameStringCurrentIndex()
        {
            for (int i = 0; i < SummarySameStringIndexlist.Count; i++)
            {
                if (SummarySameStringIndexlist[i] == DiffStartIndex)
                {
                    if (HtmlSameStringIndexlist.Count > 0)
                    {
                        DiffHtmlStartIndex = HtmlSameStringIndexlist[i];
                        DiffHtmlEndIndex = DiffHtmlStartIndex + DiffString.Length;
                    }

                }
            }
        }


    }

    public partial class HistoryView : System.Web.UI.Page
    {
        protected string ItemID = string.Empty;
        protected string Title = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string CommonID = string.Empty;
        protected string mode = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;

        protected int PageNumList = 1;
        protected string TagTitle = string.Empty;
        protected string SearchSort = string.Empty;

        protected string WType = string.Empty;
        protected string TType = string.Empty;
        protected string SchText = string.Empty;
        internal const int GLOSSARY_ATTACH_ID = 314;

        protected void Page_Load(object sender, EventArgs e)
        {

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            mode = (Request["mode"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            CommonID = (Request["CommonID"] ?? string.Empty).ToString();

            PageNumList = Convert.ToInt16((Request["PageNumList"] ?? "1"));
            TagTitle = ((Request["TagTitle"] == null || Request["TagTitle"] == string.Empty) ? string.Empty : HttpUtility.UrlDecode(Request["TagTitle"])).ToString();
            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();

            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            WType = (Request["WType"] ?? string.Empty).ToString();
            TType = (Request["TType"] ?? string.Empty).ToString();

            //DT블로그홈 / T생활백서 검색어 유지
            if (!string.IsNullOrEmpty(WType) || !string.IsNullOrEmpty(TType))
            {
                SchText = (string.IsNullOrEmpty(Request["SchText"]) ? string.Empty : Request["SchText"]).ToString();
            }

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            Select();
        }

        private void Select()
        {
            //UserInfo u = new UserInfo(this.Page);
            //GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            //int rtnValue = permissionsBiz.Permissions_Check(CommonID, u.UserID.ToString());
            //if (rtnValue != 3) { Response.Redirect("../Error.aspx?Message=" + "이 글에 대한 조회 권한이 부여되지 않았습니다."); }  // 글 권한 체크

            GlossaryHistoryBiz biz = new GlossaryHistoryBiz();
            GlossaryHistoryType Board = new GlossaryHistoryType();
            Board = biz.GlossaryHistorySelect(ItemID, CommonID);
            this.lbTitle.Text = Board.RootTitle;
            if (mode == "MyPage")
            {
                txtBody.Text = SecurityHelper.Add_XSS_CSRF(Board.ContentsModify);
            }
            else
            {
                Board.RootContentsModify = SecurityHelper.Add_XSS_CSRF(Board.RootContentsModify);
                Board.RootSummary = SecurityHelper.Add_XSS_CSRF(Board.RootSummary);
                Board.ContentsModify = SecurityHelper.Add_XSS_CSRF(Board.ContentsModify);
                Board.Summary = SecurityHelper.Add_XSS_CSRF(Board.Summary);

                //this.txtBody.Text = Board.RootContentsModify.Replace("&nbsp;", "");
                this.txtBody.Text = SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(Board.RootContentsModify));
                //SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(
                lbOldTitle.Text = Board.Title;
                //this.txtOldBody.Text = Board.ContentsModify.Replace("&nbsp;", "");
                this.txtOldBody.Text = SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(Board.ContentsModify));

                this.litTitleList.Text = Utility.GlossaryViewH1List(txtBody.Text); //목차
                this.litOldTitleList.Text = Utility.GlossaryViewH1List(txtOldBody.Text); //목차

                if (Board.PrivateYN == "Y")
                {
                    this.litWriter.Text = "비공개님이 작성";
                }
                else
                {
                    //2016-01-13 P033028
                    //this.litWriter.Text = "<a href=\"javascript:fnProfileView('" + Board.RootBoardUserID + "');\"><b>" + Board.RootBoardUserName + "/" + Board.RootBoardDeptName + "</b></a>님이 <b class='point_red'>"+Board.LastCreateDate+"</b>에 작성";
                    this.litWriter.Text = "<b>" + Board.RootBoardUserName + "/" + Board.RootBoardDeptName + "</b>님이 <b class='point_red'>" + Board.LastCreateDate + "</b>에 작성";
                }

                if (Board.PrivateYN == "Y")
                {
                    this.litOldWriter.Text = "비공개님이 작성";
                }
                else
                {
                    //2016-01-13 P033028
                    //this.litOldWriter.Text = "<a href=\"javascript:fnProfileView('" + Board.UserID + "');\"><b>" + Board.UserName + "/" + Board.DeptName + "</b></a>님이 <b class='point_red'>" + Board.CreateDate + "</b>에 작성";
                    this.litOldWriter.Text = "<b>" + Board.UserName + "/" + Board.DeptName + "</b>님이 <b class='point_red'>" + Board.CreateDate + "</b>에 작성";
                }

            }
        }


        protected void btnRevert_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryDac Dac = new GlossaryDac();
            if (string.IsNullOrEmpty(DescriptionTemp.Value.Trim()))
                DescriptionTemp.Value = "변경내용 미 작성";
            //DataSet ds = Dac.GlossaryRevert(ItemID, DescriptionTemp.Value);
            //string ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            Dac.GlossaryRevert(ItemID, DescriptionTemp.Value);

            #region CHG610000073115 / 2018-11-15 / 최현미 / ContentFeeds
            if (ConfigurationManager.AppSettings["TnetContentFeeds"].ToString().Equals("Y"))
            {
                GlossaryBiz biz = new GlossaryBiz();
                GlossaryType Board = biz.GlossarySelect(CommonID, u.UserID, "History");

                OpinionBoardFeed bsObj = new OpinionBoardFeed();
                string BaseUrl = ConfigurationManager.AppSettings["BaseURL"].ToString();

                bsObj.sbmId = "TKE@100@" + Board.CommonID;
                bsObj.pushTime = DateTime.Now.ToString("yyyyMMddHHmm");
                bsObj.writeProfile = u.UserID;
                bsObj.contentTitle = TitleCut(Board.Title, 100).Trim();
                //bsObj.contentCont = TitleCut(Board.Summary, 500).Trim();
                bsObj.contentCont = Board.Summary.Trim();

                //CHG610000081447 / 내용이 없을 경우 제목 대체 / 2019-03-07  
                if (string.IsNullOrWhiteSpace(bsObj.contentCont))
                {
                    bsObj.contentCont = bsObj.contentTitle;
                }

                bsObj.linkUrl = BaseUrl + "Glossary/GlossaryView.aspx?ItemID=" + Board.CommonID;
                List<ReadRole> readRoleList = new List<ReadRole>();

                if (Board.Permissions.Equals("SomePublic"))
                {
                    GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
                    List<PermissionsType> info = permissionsBiz.PermissionsSelect(Convert.ToInt32(ItemID));

                    string userlist = string.Empty;
                    string deptlist = string.Empty;

                    foreach (PermissionsType pt in info)
                    {
                        //부서
                        if (pt.ToUserID.Trim().Length == 8)
                        {
                            deptlist += pt.ToUserID + ",";
                        }
                        //사용자
                        else
                        {
                            userlist += pt.ToUserID + ",";
                        }
                    }

                    ReadRole readRole = new ReadRole();
                    if (userlist.Length > 0)
                    {
                        readRole = new ReadRole();
                        readRole.type = "user";
                        readRole.code = userlist.Substring(0, userlist.Length - 1);
                        readRoleList.Add(readRole);
                    }
                    if (deptlist.Length > 0)
                    {
                        readRole = new ReadRole();
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

                if (string.IsNullOrEmpty(Board.UserID))
                    bsObj.nickName = Board.UserName;
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

                        feedFileInfoList.Add(tmpfeedFileInfo);
                    }
                }
                bsObj.feedFileInfo = feedFileInfoList;
                ContentFeeds.SendFeeds(bsObj, "PUT");
            }
            #endregion

            if (!string.IsNullOrEmpty(WType))
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&WType={3}&SchText={4}", CommonID, SearchKeyword, PageNumList, WType, SchText), false);
            else if (!string.IsNullOrEmpty(TType))
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&TType={3}&SchText={4}", CommonID, SearchKeyword, PageNumList, TType, SchText), false);
            else 
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&TagTitle={3}", CommonID, SearchKeyword, PageNumList, TagTitle), false);

        }

        protected string GetSearchKeyword()
        {
            return Server.UrlEncode(SearchKeyword);
        }
        protected string GetTitle()
        {
            return Server.UrlEncode(Title);
        }

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