using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using System.Collections;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace SKT.Glossary.Web.CommonPages
{
    public partial class GlossaryHistoryList : System.Web.UI.Page
    {

        int currentPageIndx;
        protected int iTotalCount;
        protected string SearchKeyword = string.Empty;
        protected string ItemID;
        protected string RootURL = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;

        protected int PageNumList = 1;
        protected string TagTitle = string.Empty;
        protected string SearchSort = string.Empty;

        protected string WType = string.Empty;  //DT 블로그홈
        protected string TType = string.Empty;  //T생활백서
        protected string SchText = string.Empty;

        internal const int GLOSSARY_ATTACH_ID = 314;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            PageNumList = Convert.ToInt16((Request["PageNumList"] ?? "1"));
            TagTitle = ((Request["TagTitle"] == null || Request["TagTitle"] == string.Empty) ? string.Empty : HttpUtility.UrlDecode(Request["TagTitle"])).ToString();
            SearchSort = (Request["SearchSort"] ?? "CreateDate").ToString();
            
            WType = (Request["WType"] ?? string.Empty).ToString();
            TType = (Request["TType"] ?? string.Empty).ToString();

            //DT블로그홈 / T생활백서 검색어 유지
            if (!string.IsNullOrEmpty(WType) || !string.IsNullOrEmpty(TType))
            {
                SchText = (string.IsNullOrEmpty(Request["SchText"]) ? string.Empty : Request["SchText"]).ToString();
            }

            if (!IsPostBack)
            {
                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;
                BindSelect();
            }

        }

        private void BindSelect()
        {
            //UserInfo u = new UserInfo(this.Page);
            //GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            //int rtnValue = permissionsBiz.Permissions_Check(ItemID, u.UserID.ToString());
            //if (rtnValue != 3) { Response.Redirect("../Error.aspx?Message=" + "이 글에 대한 조회 권한이 부여되지 않았습니다."); }  // 글 권한 체크

            iTotalCount = 0;
            //SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            //UserInfo u = new UserInfo(this.Page);
            GlossaryHistoryBiz biz = new GlossaryHistoryBiz();
            ArrayList list = biz.GlossaryHistoryList(pager.CurrentIndex, pager.PageSize, out iTotalCount, ItemID);
            pager.ItemCount = iTotalCount;

            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
        }


        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            Literal Num = (Literal)e.Item.FindControl("Num");
            Literal litClass = (Literal)e.Item.FindControl("litClass");
            Literal litUserInfo = (Literal)e.Item.FindControl("litUserInfo");
            GlossaryType glossaryType = ((GlossaryType)e.Item.DataItem);

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }

                if (glossaryType.PrivateYN == "N")
                {
                    //litUserInfo.Text = "<a href=\"javascript:fnProfileView('" + glossaryType.UserID + "');\">" + glossaryType.UserName + "/" + glossaryType.DeptName + "</a>";
                    litUserInfo.Text = glossaryType.UserName + "/" + glossaryType.DeptName;
                    
                }
                else
                {
                    litUserInfo.Text = SecurityHelper.Clear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(glossaryType.UserName));
                }



                //<a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>');"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a>
                //if (int.Parse(Num.Text) % 2 == 0)
                //{
                //    litClass.Text = "<tr class=\"odd\" >";
                //}
                //else
                //{
                //    litClass.Text = "<tr class=\"even\" >";
                //}
            }
        }
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            BindSelect();
        }

        protected void btnRevert_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            //변경내용 업데이트
            GlossaryDac Dac = new GlossaryDac();
            if (string.IsNullOrEmpty(DescriptionTemp.Value.Trim()))
                DescriptionTemp.Value = "변경내용 미 작성";
            Dac.GlossaryRevert(hidRevertID.Value, DescriptionTemp.Value);

            #region CHG610000075418 / 2018-11-15 / 최현미 / ContentFeeds
            if (ConfigurationManager.AppSettings["TnetContentFeeds"].ToString().Equals("Y"))
            {
                GlossaryBiz biz = new GlossaryBiz();
                GlossaryType Board = biz.GlossarySelect(ItemID, u.UserID, "History");

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
                        if(pt.ToUserID.Trim().Length == 8)
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
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&WType={3}&SchText={4}", ItemID, SearchKeyword, PageNumList, WType, SchText), false);
            else if (!string.IsNullOrEmpty(TType))
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&TType={3}&SchText={4}", ItemID, SearchKeyword, PageNumList, TType, SchText), false);
            else
                Response.Redirect(string.Format("/GlossaryHistory/HistoryList.aspx?mode=History&ItemID={0}&SearchKeyword={1}&PageNum={2}&TagTitle={3}", ItemID, SearchKeyword, PageNumList, TagTitle), false);

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