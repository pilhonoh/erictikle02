using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Web.Services;
using System.Data;
using System.ServiceModel;
using System.Text.RegularExpressions;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Common.TikleDocManagerService;
using System.ServiceModel.Channels;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Directory
{
    public partial class DirectoryListNew : System.Web.UI.Page
    {
        //static DataTable _dtDirList = null;
        //static DataTable _dtFileList = null;


        protected string UserID = string.Empty;
        protected string DivID = string.Empty;
        protected string DivType = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        //리스트에서 파일 카운트  
        //private int itemCount = 4;
        public string bodyList = string.Empty;

        // 끌.모임 설정
        protected string GatheringYN;
        protected string GatheringID;
        protected string GatheringName;
        protected string GatheringAuthor = string.Empty;
        protected string GatheringCreationDate = string.Empty;

        UserInfo u;

        private const string BuildingUserBoardViewAttachInfo = "BuildingUserBoardViewAttachInfo";

        internal const int GLOSSARY_ATTACH_ID = 100;

        protected string dirMgrUser = string.Empty;

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

            // 권한처리(모임멤버검사)
            if (GatheringYN.Equals("Y"))
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

            if (!IsPostBack)
            {
                hdItemGuid.Value = Guid.NewGuid().ToString();
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
        }

        /// <summary>
        /// 디렉토리 목록 조회  
        /// </summary>
        [WebMethod]
        public static Dictionary<string, object> DirectoryListSelectWeb(string mode, string userID, int pageNum, int pageSize, string GatheringYN, string GatheringID)
        {

            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            GlossaryControlBiz commBiz = new GlossaryControlBiz();

            DataSet ds = biz.GlossaryDirectory_List(mode, userID, pageNum, pageSize, GatheringYN, GatheringID);
            //dirMgrUser = 
            //string DIR_ID = string.Empty;
            //int UserCnt = 0;

            //if (ds != null && ds.Tables.Count > 0)
            //{
            //	foreach (DataRow dr in ds.Tables[0].Rows)
            //	{
            //		DIR_ID = dr["DIR_ID"].ToString();
            //		UserCnt = Int32.Parse(dr["AUTHUSERCNT"].ToString());

            //		if (UserCnt > 1)
            //		{
            //			UserCnt--;
            //			dr["AUTHUSERCNTSTR"] = dr["FIRSTNAME"].ToString() + "님 외 " + UserCnt + "명";
            //		}
            //		else
            //		{
            //			dr["AUTHUSERCNTSTR"] = dr["FIRSTNAME"].ToString();
            //		}

            //		//dr["FILECNT"] = DirectoryCommon.GetFileCount(DIR_ID);
            //		//dr["FILELIST"] = GetFileListWeb(DIR_ID, 5);
            //	}

            //	ds.Tables[0].AcceptChanges();
            //}

            return Utility.ToJson(ds);
        }

        /// <summary>
        /// 디렉토리 목록 조회  
        /// </summary>
        [WebMethod]
        public static String DirectoryListSelectFileName(string dirid)
        {
            T_FileInfo[] glossaryFilelist = DirectoryCommon.GetFileList(dirid, -1);
            String rtnVal = Newtonsoft.Json.JsonConvert.SerializeObject(glossaryFilelist);
            return rtnVal;
        }

        /// <summary>
        /// 폴더별 파일갯수 조회  
        /// </summary>
        [WebMethod]
        public static String GetFileCountWeb(string DIR_ID)
        {
            return DirectoryCommon.GetFileCount(DIR_ID);
        }

        /// <summary>
        /// 폴더별 파일리스트 조회  
        /// </summary>
        [WebMethod]
        public static String GetFileListWeb(string DIR_ID, int Top_Count = 5)
        {
            T_FileInfo[] fileList;
            String rtnVal = "";

            fileList = DirectoryCommon.GetFileList(DIR_ID, Top_Count);

            foreach (T_FileInfo fi in fileList)
            {
                fi.EDIT_DATE = Convert.ToDateTime(fi.EDIT_DATE).ToShortDateString();
            }

            rtnVal = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);


            return rtnVal;
        }

        /// <summary>
        /// 파일업로드 변경 이벤트  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFileSave_Click(object sender, EventArgs e)
        {

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

            if (rdFileUp == "upload")
            {
                //New 위치변경
                GetSaveFilesInfo(hdDirectoryID.Value); 

                foreach (Dictionary<string, object> file in fileCtrl.Save())
                {
                    txtFileName = file["FileName"].ToString().Substring(0, file["FileName"].ToString().LastIndexOf('.'));
                    //txtFileName = file["FileName"].ToString().Split('.')[0];

                    //******************//
                    //파일 업로드
                    //******************//
                    dirCommon.FileUpload(dirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
                }
            }
            else
            {
                txtFileName = hidFileDefaultName.Value;
                
                //******************//
                //파일 업로드
                //******************//
                dirCommon.FileUpload(dirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
            }

            //페이지 리플레쉬 
            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        //폴더 권한 변경
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string AuthType = string.Empty;
            string errMsg = string.Empty;
            string mode = "Update";

            DirectoryCommon dirCommon = new DirectoryCommon();

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;


            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();

            Board.DirID = this.hdDirectoryID.Value;

            Board.DirNM = SKT.Common.SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.Path = ConfigurationManager.AppSettings["AttachFilePath"].ToString();
            Board.UserID = u.UserID;
            Board.RegID = u.UserID;
            Board.RegNM = u.Name;

            //******************//
            //Directory 저장  
            //******************//
            Board = biz.DirectoryInsert(Board, mode);
            hdCommonID.Value = Board.DirID;


            //******************//
            //조직도 저장  
            //******************//
            string ToUser = this.UserControl.AuthID;
            string ToUserType = this.UserControl.AuthCL;

            //dirCommon.SaveUserList(Board.DirID, u.UserID, this.UserControl, mode);
            dirCommon.SaveUserList(Board.DirID, u.UserID, ToUser, ToUserType, mode);

            Response.Redirect("DirectoryListNew.aspx?DivType=" + DivType + "&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID + "&MenuType=Directory");

        }

        //문서함 삭제처리
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            //string rtn;

            Board.DirID = this.hdDirectoryID.Value;
            Board.UserID = u.UserID;
            Board.RegID = u.UserID;
            Board.RegNM = u.Name;

            //******************//
            //Directory 삭제
            //******************//
            biz.DirectoryDelete(Board);

            // 모임정보 삭제
            if (GatheringYN == "Y")
            {
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                gatheringBiz.GatheringMenuAuth_Delete(Board.DirID, "Dir", GatheringID);
            }

            Response.Redirect("DirectoryListNew.aspx?DivType=" + DivType + "&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID + "&MenuType=Directory");
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
                attach.ItemID = commonid; // Board.WeeklyID.ToString()위클리아이디
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
    }
}