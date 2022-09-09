using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using System.Web.Services;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SKT.Common.TikleDocManagerService;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Directory
{
    public partial class DirectoryView : System.Web.UI.Page
    {
        /// <summary>
        /// 모드  
        /// </summary>
        protected string mode = string.Empty;

        protected string DivID = string.Empty;
        protected string DivType = string.Empty;
        protected string UserID = string.Empty;
        protected static string SUserID = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        protected string DIR_CREATOR_ID = string.Empty;
        protected List<CommonAuthType> glossaryAuthlist = new List<CommonAuthType>();

        // 끌.모임 설정
        protected string GatheringYN;
        protected string GatheringID;
        protected string GatheringName;
        protected string GatheringAuthor = string.Empty;
        protected string GatheringCreationDate = string.Empty;

        public UserInfo u;

        protected void Page_Init(object sender, EventArgs e)
        {
            Debug.Write("시작시간 : " + System.DateTime.Now.ToString() + "<br />");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ClientScript.GetPostBackEventReference(this, string.Empty);

            //대상자 추가 화면 제공 여부
            //UserControlNateOnBizPop.targetBtnYn = true;

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            DivID = (Request["DivID"] ?? string.Empty).ToString();
            DivType = (Request["DivType"] ?? string.Empty).ToString();
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            u = new UserInfo(this.Page);

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsDirectoryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }

            UserID = u.UserID;
            SUserID = UserID;

            if (!IsPostBack)
            {
                //문서함 권한 체크
                if (GatheringYN != "Y")
                {
                    if (DirectoryUserAuth() == false)
                    {
                        Response.Redirect("~/Directory/DirectoryListNew.aspx");
                    }
                }
                else
                {
                    Response.Redirect("/TikleMain.aspx");
                    Response.End();

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

                hdItemGuid.Value = Guid.NewGuid().ToString();

                //폴더명 조회  
                dataBind(string.IsNullOrEmpty(DivType) ? "Pub" : DivType, u.UserID);

                //폴더사용자 조회  
                DirectoryUserListBind();

                //Share Point 데이터 조회  
                DirectoryFileListBind();

                //폴더전체사용자조회
                DirectoryAllUserListBind();


            }

            DivType = string.IsNullOrEmpty(DivType) ? "Pub" : DivType;
            if (DivType == "Pri")
            {
                m_pub = "";
                m_vis = "";
                m_pri = "class=\"on\"";
            }
            else if (DivType == "Vis")
            {
                m_pub = "";
                m_vis = "class=\"on\"";
                m_pri = "";
            }
            else
            {
                m_pub = "class=\"on\"";
                m_vis = "";
                m_pri = "";
            }

            //대상자 추가 버튼 제공
            //UserControlNateOnBizPop.targetBtnYn = false;

        }

        /// <summary>
        /// Share Point 데이터 조회  
        /// </summary>
        public void DirectoryFileListBind()
        {
            //Share Point 애서 조회 
            T_FileInfo[] glossaryFilelist = DirectoryCommon.GetFileList(DivID, -1);

            for (int i = 0; i < glossaryFilelist.Length; i++)
            {
                hdFileList.Value += glossaryFilelist[i].FILE_NAME + ",";
            }

            rptDirectory.DataSource = glossaryFilelist;
            rptDirectory.DataBind();
        }

        /// <summary>
        /// 바인딩  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptDirectory_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            GlossaryDirectoryBiz DirBiz = new GlossaryDirectoryBiz();

            if (rptDirectory.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Literal lblFooter = (Literal)e.Item.FindControl("lblEmptyData");
                    lblFooter.Visible = true;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                T_FileInfo glossaryFileType = (T_FileInfo)e.Item.DataItem;
                Literal litDirectory = (Literal)e.Item.FindControl("litDirectory");

                litDirectory.Text = "<li>";

                string DirID = DivID;
                string FileNM = glossaryFileType.FILE_NAME;
                string fileExt = "";
                string confirmUser = "";

                List<string> result = new List<string>();

                string[] extArr = FileNM.Split('.');

                fileExt = extArr[extArr.Length - 1];


                if (fileExt == "pptx" || fileExt == "ppt")
                {
                    fileExt = "ms_ppt.png";
                }
                else if (fileExt == "docx" || fileExt == "doc")
                {
                    fileExt = "ms_word.png";
                }
                else if (fileExt == "xlsx" || fileExt == "xls")
                {
                    fileExt = "ms_excel.png";

                    /*
                    Author : 개발자-최현미C, 리뷰자-진현빈D
                    Create Date : 2016.07.21
                    Desc : 사용안함 (엑셀파일일 경우 현재 사용자 조회)
                    */
                    //result = DirBiz.GetExcelConfirmData(DirID, FileNM);

                    //if (result[0] != "-1")
                    //{
                    //    confirmUser = result[1]; // + "(" + result[3] + ")";
                    //}
                }
                else if (fileExt == "one")
                {
                    fileExt = "ms_onenote.png";
                }
                else if (fileExt == "pdf")
                {
                    fileExt = "ms_pdf.png";
                }
                else
                {
                    fileExt = "ms_pc.png";
                }

                litDirectory.Text += " <img src=\"/common/images/icon/" + fileExt + "\" alt=\"\" onclick=\"fileDownload('" + DirID + "','" + FileNM + "')\" style='cursor:pointer; width:50px;height:50px;' />";
                litDirectory.Text += " <a href=\"javascript:fileVerCheck('" + DirID + "','" + FileNM + "')\"  class=\"editbtn\">편집이력보기</a>";

                litDirectory.Text += " <dl>";

                //파일명  
                litDirectory.Text += " <dt><a href=\"javascript:fileDownload('" + DirID + "','" + FileNM + "') \">" + FileNM + "</a></dt>";

                //문서유형 없애기  
                //litDirectory.Text += " <dd>최종수정일 : <span>" + glossaryFileType.AuditDTM + " </span> 유형 : <span>" + fileSyle + "</span>   최종수정자 : " + glossaryFileType.AuditNM + "</dd>";

                litDirectory.Text += " <dd>최종수정일 : <span class='pr'>" + glossaryFileType.EDIT_DATE + " </span>";

                if (confirmUser == "")
                {
                    litDirectory.Text += "최종수정자 : <span>" + glossaryFileType.EDITOR + "</span>";
                }
                else
                {
                    litDirectory.Text += "<span class=\"point_red\">현재사용자</span> : " + confirmUser;
                }

                //편집이력보기
                //litDirectory.Text += " <a href=\"javascript:fileVerCheck('" + DirID + "','" + FileNM + "') \"> [편집이력보기]</a>";
                litDirectory.Text += "</dd>";


                //litDirectory.Text += "   <input type=\"hidden\" name=\"txtFile\" id=\"txtFile\" runat=\"server\"  value=\"" + glossaryFileType.FileID + "\">";
                litDirectory.Text += " </dl> ";

                //버튼
                litDirectory.Text += " <span class=\"btns\">";


                //-----------------------------------------------
                //관리자만 삭제와 쪽지 가능
                //수정 : 파일삭제는 관리자와 파일생성자가 삭제가능 
                //    : 파일을 수정한 이력이 있으면 삭제를 못하게 
                //-----------------------------------------------
                //1.Share Point 에서 파일이력을 조회해서 이력이 있으면 삭제불가  
                //if (glossaryFileType.HAS_EDITED_VERSION == "N")
                //{
                //    //폴더생성자와 파일생성자 
                    if (DIR_CREATOR_ID == u.UserID || glossaryFileType.WRITE_ID.Replace(@"SKT\", "") == u.UserID)
                    {
                        //삭제 
                        litDirectory.Text += "   <a href=\"javascript:fnDeleteFile('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>삭제하기</span></a>";
                    }
                

                /*
                    Author : 개발자-최현미C, 리뷰자-진현빈D
                    Create Date : 2016.07.21
                    Desc : 문서함 자체가 권한 설정으로 접근 되므로 별도 체크 하지 않는다.
                */
                //////20141020 전소영 메니저 요청으로 초대된 사람들도 모두 쪽지 보내기가 가능하도록                                
                //DirectoryCommon dirCommon = new DirectoryCommon();
                //bool isBelongToGrp = dirCommon.checkUserInGroup(glossaryAuthlist, u);

                //if (isBelongToGrp)
                //{
                string fileLink = DirID + "/" + FileNM;

                string EncryptfileLink = SKT.Common.CryptoHelper.AESEncryptString(fileLink, "sktelecom_tikle2");
                litDirectory.Text += "   <a href=\"javascript:fnMeno('" + DirID + "','" + FileNM + "','" + EncryptfileLink + "')\"  class=\"btn1\"><span>쪽지보내기</span></a>";
                //}

                //if (DIR_CREATOR_ID == u.UserID)
                //if (glossaryFileType.RegID.Replace(@"SKT\", "")  == u.UserID)
                //{

                //    string fileLink = DirID + "/" + FileNM;

                //    string EncryptfileLink = SKT.Common.CryptoHelper.AESEncryptString(fileLink, "sktelecom_tikle2");

                //NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + HttpUtility.UrlEncode(fileLink) + "&tikle=31163105310731083101";
                //NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + Server.UrlEncode(fileLink) + "&tikle=31163105310731083101";
                //NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + EncryptfileLink + "&tikle=1";

                //    litDirectory.Text += "   <a href=\"javascript:fnMeno('" + DirID + "','" + FileNM + "','" + EncryptfileLink + "')\"  class=\"btn1\"><span>쪽지보내기</span></a>";
                //}


                //열기버튼 없애기 
                //litDirectory.Text += "   <a href=\"javascript:fileOpen('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><b>열기</b></a>"; 
                //엑셀확인버튼 추가  
                //litDirectory.Text += "   <a href=\"javascript:fileExcelConfirm('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>사용자확인</span></a>";
                //버전체크
                //litDirectory.Text += "   <a href=\"javascript:fileVerCheck('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>편집이력보기</span></a>"; 
                //다운로드
                litDirectory.Text += "   <a href=\"javascript:fileDownload('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>다운로드</span></a>";

                litDirectory.Text += " </span>";
                litDirectory.Text += "</li> ";
            }

        }

        /// <summary>
        /// 폴더사용자 조회해서 권한 체크 by.김성환  
        /// </summary>
        public Boolean DirectoryUserAuth()
        {
            GlossaryDirectoryAuthBiz biz = new GlossaryDirectoryAuthBiz();
            DataSet ds = new DataSet();
            ds = biz.GlossaryDirectoryAuthSelect2(DivID, u.UserID);

            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 폴더사용자 상세조회
        /// </summary>
        public void DirectoryAllUserListBind()
        {
            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();
            DataSet ds = new DataSet();
            ds = biz.DirectoryAllUserList(DivID);

            litTargetAll.Items.Clear();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string userID = row["MAIL"].ToString().Remove(row["MAIL"].ToString().IndexOf('@'));
                litTargetAll.Items.Add(new ListItem(row["USER_NAME"].ToString(), userID));
                                                    
            }
        }

        /// <summary>
        /// 폴더사용자 조회  
        /// </summary>
        public void DirectoryUserListBind()
        {
            if (ddlUserDirectory.Items.Count == 0) return;

            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();
            DataSet ds = new DataSet();

            string DirectoryType = "Directory";

            string dir_CreaterID = DIR_CREATOR_ID;
            string dir_CreaterName = "";

            if (GatheringYN == "Y")
            {
                DirectoryType = "GatheringDirectory";

                ds = biz.MyGroupListSelect2(u.UserID, ddlUserDirectory.SelectedValue, DirectoryType, GatheringID);

                //GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                //List<PermissionsType> GatheringInfo = gatheringBiz.GatheringMenuAuth_Select(Convert.ToInt32(DivID), "Dir");
                //hdPermissionsString.Value = JsonConvert.SerializeObject(GatheringInfo);
            }
            else
            {
                ds = biz.MyGroupListSelect2(u.UserID, ddlUserDirectory.SelectedValue, DirectoryType);
            }

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

                    if (dr["ToUserID"].ToString() == DIR_CREATOR_ID)
                    {
                        dir_CreaterName = dr["ToUserName"].ToString();
                    }
                    glossaryAuthlist.Add(temp);
                }
            }

            rptDirectoryUser.DataSource = glossaryAuthlist;
            rptDirectoryUser.DataBind();

            if (GatheringYN != "Y")
            {
                //glossaryAuthlist.Clear();

                //CommonAuthType temp1 = new CommonAuthType();
                //temp1.AuthID = dir_CreaterID;
                //temp1.AuthName = dir_CreaterName;
                //glossaryAuthlist.Add(temp1);

                //if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[1].Rows)
                //    {
                //        temp1 = new CommonAuthType();
                //        temp1.AuthID = dr["ToUserID"].ToString();
                //        temp1.AuthName = dr["ToUserName"].ToString();
                //        glossaryAuthlist.Add(temp1);
                //    }
                //}

                //rptDirectoryAdminUser.DataSource = glossaryAuthlist;
                //rptDirectoryAdminUser.DataBind();

                //if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[1].Rows)
                //    {
                //        if (dr["ToUserID"].ToString() == u.UserID)
                //        {
                //            p_dirsetting.Visible = true;
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// 폴더 사용자  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptDirectoryUser_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CommonAuthType glossaryAuthType = (CommonAuthType)e.Item.DataItem;
                Literal litDirectoryUser = (Literal)e.Item.FindControl("litDirectoryUser");

                litDirectoryUser.Text = "";

                if (glossaryAuthType.AuthID == glossaryAuthType.RegID)
                {
                    lblCreater.Text = glossaryAuthType.AuthName;
                    lblCreateDate.Text = glossaryAuthType.RegDTM.ToLongDateString();

                    litDirectoryUser.Text += "<li class=\"super\"><span>" + glossaryAuthType.AuthName + "</span></li>";
                }
                else
                {
                    litDirectoryUser.Text += "<li>" + glossaryAuthType.AuthName + "</li>";
                }

                
                //    litDirectoryUser.Text += "<p><a href=\"javascript:viewDivShow();\"><img src=\"/common/images/icon/setting.png\" alt=\"\" title=\"폴더사용자 관리 바로가기\" /></a></p>";
            }
        }

        /// <summary>
        /// 폴더 사용자  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        //protected void rptDirectoryAdminUser_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        CommonAuthType glossaryAuthType = (CommonAuthType)e.Item.DataItem;
        //        Literal litDirectoryAdminUser = (Literal)e.Item.FindControl("litDirectoryAdminUser");


        //        if (glossaryAuthType.AuthID == DIR_CREATOR_ID)
        //        {
        //            litDirectoryAdminUser.Text += "<li class=\"super\"><span>" + glossaryAuthType.AuthName + "</span></li>";
        //        }
        //        else
        //        {
        //            litDirectoryAdminUser.Text += "<li>" + glossaryAuthType.AuthName + "</li>";
        //        }

        //        if (glossaryAuthType.AuthID == UserID)
        //        {
        //            //litDirectoryAdminManager.Text += "<a id=\"admindivshow\" href=\"javascript:viewDivManagerShow();\"><img src=\"/common/images/icon/setting.png\" alt=\"\" title=\"폴더관리자 바로가기\" /></a>";
        //            p_dirsetting.Visible = true;
        //            p_mgrsetting.Visible = true;
        //        }
        //    }
        //}

        /// <summary>
        /// 파일 삭제  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string AuthType = string.Empty;
            string errMsg = string.Empty;

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            //***********************//
            //1. 로컬DB에서 파일 삭제 
            //***********************//

            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            GlossaryDirectoryFileType FileType = null;
            string fileName = string.Empty;

            try
            {
                FileType = new GlossaryDirectoryFileType();
                FileType.DirID = hdDirectoryID.Value;
                FileType.FileID = "0";      //(Share Point 연동문제로 이렇게 쓴다)      
                FileType.FileNM = hdFileID.Value;  //로컬DB의 파일id (Share Point 연동문제로 이렇게 쓴다) 
                FileType.FileNM = hdFileName.Value;  //로컬DB의 파일id 


                biz.DirectoryFileInsert(FileType, "Delete");
            }
            catch (System.Exception exp)
            {
                errMsg = exp.Message;
            }

            //****************************//
            //2. Share Point 파일 삭제
            //****************************// 

            //웹서비스  객체 생성  
            SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

            try
            {
                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    Result result = proxy.DeleteFile(hdDirectoryID.Value, hdFileID.Value, "skt\\" + u.UserID, "Y");

                    if (result.STATUS == 0) //성공 
                    {
                        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
                    }
                }
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri); 


        }


        /// <summary>
        /// 디렉토리 변경이벤트  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strURL = "/Directory/DirectoryView.aspx";
            string dirID = ddlUserDirectory.SelectedValue;

            Response.Redirect(strURL + "?DivType=" + DivType + "&DivID=" + dirID + "&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID + "&MenuType=Directory");
        }

        /// <summary>
        /// 폴더명 드롭다운 리스트 조회  
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userid"></param>
        protected void dataBind(string mode, string userid)
        {
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            DataSet ds = biz.ddlDirectorySelect(mode, userid, GatheringYN, GatheringID);
            DataTable dt = null;

            if (ds != null && ds.Tables.Count > 0)
            {
                ddlUserDirectory.Items.Clear();

                dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ddlUserFile.Items.Insert(i, new ListItem(dt.Rows[i]["DIR_NM"].ToString(), dt.Rows[i]["DIR_ID"].ToString())); 

                    /*
                        Author : 개발자-김성환D, 리뷰자-진현빈G
                        Create Date : 2016.08.04 
                        Desc : 특수문자 " ' \ 처리
                    */
                    //dt.Rows[i]["DIR_NM"].ToString().Replace("&quot;", "\"")).Replace("&#39;", "\'")
                    ddlUserDirectory.Items.Insert(i, new ListItem((SKT.Common.SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(dt.Rows[i]["DIR_NM"].ToString()))), dt.Rows[i]["DIR_ID"].ToString()));

                    // 문서함 생성자 저장
                    if (DivID == dt.Rows[i]["DIR_ID"].ToString())
                    {
                        DIR_CREATOR_ID = dt.Rows[i]["REG_ID"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(DivID) && ddlUserDirectory.Items.Count > 0)
                {
                    ddlUserDirectory.SelectedValue = DivID;
                }
                else
                {
                    ddlUserDirectory.SelectedIndex = 0;
                }
            }
        }

        //파일 올리기 저장  
        protected void btnFileSave_Click(object sender, EventArgs e)
        {
            //2016-05-25
            //T_FileInfo[] glossaryFilelist = DirectoryCommon.GetFileList(DivID, -1);

            //다른 폴더로 올릴수 있다 그래서 지역변수를 사용한다  
            string dirID = string.Empty;
            string errMsg = string.Empty;
            string strGUID = hdItemGuid.Value;
            //string rdFileUp = (Request["rdFileUp"] ?? string.Empty).ToString();
            //string txtFileName = (Request["txtFileName"] ?? string.Empty).ToString();
            string rdFileUp = hidFileRadioCheck.Value;
            string txtFileName = string.Empty;

            //GetSaveFilesInfo(hdDirectoryID.Value);

            DirectoryCommon dirCommon = new DirectoryCommon();

            dirID = hdDirectoryID.Value;

            bool filenamecheck = false;

            if (rdFileUp == "upload")
            {
                //New 위치변경
                GetSaveFilesInfo(hdDirectoryID.Value);

                foreach (Dictionary<string, object> file in fileCtrl.Save())
                {

                    //txtFileName = file["FileName"].ToString().Split('.')[0];
                    txtFileName = file["FileName"].ToString().Substring(0, file["FileName"].ToString().LastIndexOf('.'));

                    //for (int i = 0; i < glossaryFilelist.Length; i++)
                    //{
                    //    if (file["FileName"].ToString() == glossaryFilelist[i].FILE_NAME)
                    //    {
                    //        filenamecheck = true;
                    //    }
                    //}

                    //******************//
                    //파일 업로드
                    //******************//
                    dirCommon.FileUpload(dirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
                }
            }
            else
            {
                txtFileName = hidFileDefaultName.Value;

                //string ext = "";
                //switch (rdFileUp)
                //{
                //    case "excel" : ext = ".xlsx"; break;
                //    case "word" : ext = ".docx"; break;
                //    case "ppt" : ext = ".pptx"; break;
                //}

                //for (int i = 0; i < glossaryFilelist.Length; i++)
                //{
                //    if ((txtFileName + ext) == glossaryFilelist[i].FILE_NAME)
                //    {
                //        filenamecheck = true;
                //    }
                //}

                //******************//
                //파일 업로드
                //******************//
                dirCommon.FileUpload(dirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
            }

            if (filenamecheck == true)
            {
                string msg = "등록하신 파일명과 동일한 문서가 존재합니다. 기존 문서의 새로운 버전으로 관리됩니다";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "GuideMsg", "alert('" + msg + "');", true);

            }

            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// 파일 편집이력 조회 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [WebMethod]
        public static List<Dictionary<string, string>> FileEditHistoryWeb(string DirID, string FileID)
        {
            T_FileInfo[] resultFiles;
            List<Dictionary<string, string>> fList = new List<Dictionary<string, string>>();
            Dictionary<string, string> sfile;

            SKT.Common.TikleDocManagerService.DocManagerServiceClient client = new SKT.Common.TikleDocManagerService.DocManagerServiceClient();

            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                resultFiles = client.GetFileHistory(DirID, FileID);

                for (int i = 0; i < resultFiles.Length; i++)
                {
                    //2016-11-16 관리자는 가져오지 않도록 설정
                    if (!resultFiles[i].EDITOR.Equals("시스템 계정"))
                    {
                        sfile = new Dictionary<string, string>();

                        sfile.Add("IS_CURRENT_VERSION", resultFiles[i].IS_CURRENT_VERSION);
                        sfile.Add("EDITOR_ID", resultFiles[i].EDITOR_ID);
                        sfile.Add("EDITOR", resultFiles[i].EDITOR);
                        sfile.Add("EDIT_DATE", Convert.ToDateTime(resultFiles[i].EDIT_DATE).ToShortDateString());
                        sfile.Add("EDIT_TIME", Convert.ToDateTime(resultFiles[i].EDIT_DATE).ToShortTimeString());
                        sfile.Add("EDIT_URL", resultFiles[i].EDIT_URL);
                        sfile.Add("VERSION_NO", resultFiles[i].VERSION_NO);

                        fList.Add(sfile);
                    }

                    ////if (resultFiles[i].IS_CURRENT_VERSION == "N")
                    ////{
                    //DataRow drAdd = _dtFileList.NewRow();

                    //drAdd["IS_CURRENT_VERSION"] = resultFiles[i].IS_CURRENT_VERSION;
                    //drAdd["EDITOR_ID"] = resultFiles[i].EDITOR_ID;
                    //drAdd["EDITOR"] = resultFiles[i].EDITOR;
                    //drAdd["EDIT_DATE"] = resultFiles[i].EDIT_DATE;
                    //drAdd["EDIT_URL"] = resultFiles[i].EDIT_URL;
                    //drAdd["VERSION_NO"] = resultFiles[i].VERSION_NO;

                    //_dtFileList.Rows.Add(drAdd);
                    ////}

                }

                //rtn.Add("result", Newtonsoft.Json.JsonConvert.SerializeObject(resultFiles));
            }

            return fList;
        }

        /// <summary>
        /// 삭제자 체크 조회 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [WebMethod]
        public static string FileDeleteHistoryCheck(string DirID, string FileID)
        {
            T_FileInfo[] resultFiles;
            List<Dictionary<string, string>> fList = new List<Dictionary<string, string>>();

            SKT.Common.TikleDocManagerService.DocManagerServiceClient client = new SKT.Common.TikleDocManagerService.DocManagerServiceClient();

            string DeleteCheckYN = "Y";

            //UserInfo info = new UserInfo();

            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                resultFiles = client.GetFileHistory(DirID, FileID);

                for (int i = 0; i < resultFiles.Length; i++)
                {
                    if (!SUserID.Equals(resultFiles[i].EDITOR_ID.Replace("SKT\\", "")))
                    {
                        DeleteCheckYN = "N";
                    }
                }
            }
            return DeleteCheckYN;
        }

        /// <summary>
        /// 다운로드  
        /// </summary>DSEecFileDAC
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string strFileName = hdFileID.Value;

            byte[] FileStream = null;

            DirectoryCommon dirCommon = new DirectoryCommon();

            FileStream = dirCommon.FileDownload(hdDirectoryID.Value, hdFileID.Value);

            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", Server.UrlEncode(strFileName).Replace("+", "%20")));
            Response.BinaryWrite(FileStream);
            Response.End();
        }

        /// <summary>
        /// 버전확인 
        /// </summary>
        /// <param name="dirID"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        [WebMethod]
        public static List<string> GetVersionCheck(string dirID, string fileID)
        {
            List<string> result = new List<string>();

            SKT.Common.TikleDocManagerService.DocManagerServiceClient client = new SKT.Common.TikleDocManagerService.DocManagerServiceClient();
            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                SKT.Common.TikleDocManagerService.T_FileInfo[] resultFiles = client.GetFileHistory(dirID, fileID);

                result.Add(resultFiles[0].IS_CURRENT_VERSION);
                result.Add(resultFiles[0].EDITOR_ID);
                result.Add(resultFiles[0].EDITOR);
                result.Add(resultFiles[0].EDIT_DATE);
                result.Add(resultFiles[0].EDIT_URL);
                result.Add(resultFiles[0].VERSION_NO);

            }

            return result;
        }

        #region >> GetSaveFilesInfo : 저장 파일의 정보를 가져온다.
        /// <summary>
        /// 해당 Item과 함께 저장할 파일 정보를 가져온다. 
        /// 저장시 하나의 파일에 대하여 "[FileKey]￥[FileType]￥[FileName]￥[FileExt]￥[FileSize]￥[FilePath]¶" 형식으로 하나의 string로 취합
        /// DB에서 일과 저장 하여 준다. 
        /// FileType : 파일의 유형으로   A: 첨부 파일,  B:WebEdit의 인라인 이미지 로 구분
        /// </summary>
        /// <returns></returns>
        public void GetSaveFilesInfo(string commonid)
        {
            string filePathGuid = string.Empty;

            filePathGuid = string.IsNullOrEmpty(hidFilePathGuid.Value) ? Guid.NewGuid().ToString() : hidFilePathGuid.Value;
            fileCtrl.SavePath = "ShareFiles/" + filePathGuid;

            //첨부파일 저장
            foreach (Dictionary<string, object> file in fileCtrl.Save())
            {

                Guid itemGuid = Guid.Empty;
                GuidTryParse(filePathGuid, out itemGuid);

                Attach attach = new Attach();
                attach.AttachID = string.Empty;
                attach.BoardID = "100"; //디렉토리는 100
                attach.ItemID = commonid; // 디렉토리 번호
                attach.AttachType = AttachType.AttachmentFile;

                attach.FileName = file["FileName"].ToString();
                attach.ServerFileName = "ShareFiles\\" + filePathGuid + "\\" + file["FileName"].ToString();
                attach.Extension = "." + file["FileExt"].ToString().ToLower();
                attach.Folder = "ShareFiles\\" + filePathGuid;
                attach.FileSize = Convert.ToInt64(file["FileSize"].ToString());
                attach.ItemGuid = itemGuid;
                attach.DeleteYN = "N";


                ////1-3. DB에 기록한다
                attach.AttachID = AttachmentHelper.InsertWithItemGuid(attach).ToString();
            }
        }
        #endregion

        // 2014-05-23 Mr.No
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


        //protected override void OnUnload(EventArgs e)
        //{
        //    //8단계
        //    base.OnUnload(e);
        //    Response.Write("종료시간 : " + System.DateTime.Now.ToString());
        //}

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            Debug.Write("종료시간 : " + System.DateTime.Now.ToString() + "<br />");
        }

        // CHG610000060738 / 2018-06-21 / 최현미 / 끌문서 쪽지보내기 모임인원만 발송  
        [WebMethod]
        public static void SendNoteDirectory(string DirectoryEncFileName, string DirectoryFileName, string TargetUserList, string SendUserID, string SendUserNM, string DivNoteTextArea, string DivID)
        {
            string dirLink = string.Empty;
            string NoteBody = "<html><body>"; //보낼 메세지 만들기.
            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //dirLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + HttpUtility.UrlEncode(DirectoryEncFileName) + "&tikle=1";
            dirLink = BaseURL + "/Directory/DirectoryView.aspx?DivType=Pub&DivID="+ DivID +"& MenuType=Directory";
            NoteBody += "<font face=\"맑은고딕\" size=\"2\">" + DivNoteTextArea + "<br/><br/><br/>▶ 끌.문서 바로가기: ＇<a href=\"" + dirLink + "\" target=\"_docs\" >" + DirectoryFileName + "</a><br /></font>";

            NoteBody += "</body></html>";

            SendUserID = SendUserID.ToString().Remove(SendUserID.IndexOf('@'));
           
            string[] strTargetList = TargetUserList.Split(';');
            for (int i = 0; i < strTargetList.Length - 1; i++)
            {
                CBHNoteType sendData = new CBHNoteType();
                sendData.Content = NoteBody;
                sendData.Kind = "3"; //일반쪽지.
                sendData.URL = null;
                sendData.TargetUser = strTargetList[i].ToString();
                sendData.SendUserName = SendUserNM;
                sendData.SendUserID = SendUserID;
                CBHInterface.CBHNoteSend(sendData);

            }
        }
    }
}