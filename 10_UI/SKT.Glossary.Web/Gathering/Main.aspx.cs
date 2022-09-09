using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Web.Services;
using System.Data;
using System.ServiceModel;

using SKT.Glossary.Biz;
using SKT.Glossary.Type;

using SKT.Common;
using SKT.Common.TikleDocManagerService;
using System.ServiceModel.Channels;
using SKT.Glossary.Web.Directory;
using SKT.Glossary.Web.Common.Controls;

namespace SKT.Glossary.Web.Gathering
{
    public partial class Main : System.Web.UI.Page
    {
        protected string UserID = string.Empty;
        protected string DivID = string.Empty;
        protected string DivType = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        public string bodyList = string.Empty;

        UserInfo u;

        private const string BuildingUserBoardViewAttachInfo = "BuildingUserBoardViewAttachInfo";

        internal const int GLOSSARY_ATTACH_ID = 100;

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

            //메뉴제거
            Response.Redirect("/TikleMain.aspx");
            Response.End();

            u = new UserInfo(this.Page);
            UserID = u.UserID;

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
        /// 모임 태그 게시글 목록 조회  
        /// </summary>
        [WebMethod]
        public static Dictionary<string, object> GetGatheringMainTagList(string GatheringID, string Board_Index, string Board_Count)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            DataSet ds = biz.GlossaryGatheringMainTag_Select(GatheringID, Board_Index, Board_Count);

            return Utility.ToJson(ds);
        }

        /// <summary>
        /// 모임 태그 목록 조회  
        /// </summary>
        [WebMethod]
        public static Dictionary<string, object> GetGatheringTagList(string GatheringID)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            DataSet ds = biz.GlossaryGatheringTag_List(GatheringID);

            return Utility.ToJson(ds);
        }

        /// <summary>
        /// 모임 태그 목록 조회  
        /// </summary>
        [WebMethod]
        public static void SaveGatheringTagList(string GatheringID, string TagTitle, string TagSort, string UserID)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            biz.GlossaryGatheringTag_Delete(GatheringID);

            string[] ArrTagTitle = TagTitle.Split('|');
            string[] ArrTagSort = TagSort.Split('|');

            for (int i = 0; i < ArrTagTitle.Length - 1; i++)
            {
                biz.GlossaryGatheringTag_Insert(GatheringID, ArrTagTitle[i], ArrTagSort[i], UserID);
            }
        }

        /// <summary>
        /// 모임 노티 목록 조회  
        /// </summary>
        [WebMethod]
        public static Dictionary<string, object> GetGatheringNotiList(string UserID)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            DataSet ds = biz.GlossaryGatheringNoti_List(UserID);

            return Utility.ToJson(ds);
        }

        /// <summary>
        /// 모임 노티 저장
        /// </summary>
        [WebMethod]
        public static void SaveGatheringNotiList(string GatheringNotiList, string UserID)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            biz.GlossaryGatheringNoti_Delete(UserID);

            string[] ArrNoti = GatheringNotiList.Split('|');

            for (int i = 0; i < ArrNoti.Length - 1; i++)
            {
                biz.GlossaryGatheringNoti_Insert(UserID, ArrNoti[i]);
            }
        }

        /// <summary>
        /// 모임 순서 저장
        /// </summary>
        [WebMethod]
        public static void SaveGatheringSortList(string GatheringSortList, string UserID)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            biz.GlossaryGatheringSort_Delete(UserID);

            string[] ArrSort = GatheringSortList.Split(',');

            int sort = ArrSort.Length;

            for (int i = 0; i < ArrSort.Length; i++)
            {
                biz.GlossaryGatheringSort_Insert(UserID, ArrSort[i], sort--);
            }
        }

        /// <summary>
        /// 모임 목록 조회  
        /// </summary>
        [WebMethod]
        public static Dictionary<string, object> GetGatheringList(string mode, string userID, int pageNum, int pageSize)
        {

            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();
            
            DataSet ds = biz.GlossaryGathering_List(mode, userID, pageNum, pageSize);

            return Utility.ToJson(ds);
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

            ////다른 폴더로 올릴수 있다 그래서 지역변수를 사용한다  
            //string dirID = string.Empty;
            //string errMsg = string.Empty;
            //string strGUID = hdItemGuid.Value;
            //string rdFileUp = (Request["rdFileUp"] ?? string.Empty).ToString();
            //string txtFileName = (Request["txtFileName"] ?? string.Empty).ToString();

            //DirectoryCommon dirCommon = new DirectoryCommon();

            //dirID = hdGatheringID.Value;

            ////******************//
            ////파일 업로드
            ////******************//
            //dirCommon.FileUpload(dirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName, "", 0);

            ////페이지 리플레쉬 
            //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        //모임 권한 변경
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string AuthType = string.Empty;
            string errMsg = string.Empty;
            string mode = "Update";

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;


            GlossaryGatheringType Board = new GlossaryGatheringType();
            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            Board.GatheringID = this.hdGatheringID.Value;
            Board.GatheringName = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목            
            Board.Author= u.UserID;
            Board.Editor = u.UserID;
            Board.UseYN = "Y";

            //******************//
            // 1. 끌.모임 정보 저장
            //******************//
            Board = biz.GlossaryGathering_Insert(Board, mode);
            hdCommonID.Value = Board.GatheringID;


            //******************//
            // 2. 권한정보 저장
            //******************//
            UserAndDepartmentList UDList = this.UserControl;

            //삭제후 인서트   
            try
            {
                // 1. 기존 권한 정보 제거   
                biz.GlossaryGatheringAuth_Delete(Board.GatheringID);

                // 2. 새 권한 정보 등록(UDList.AuthCL은 실제로 AuthType 값을 갖음. U / O / G)
                biz.GlossaryGatheringAuth_Insert(Board.GatheringID, u.UserID, UDList.AuthID, UDList.AuthCL, "RW", mode);

                // 3. 문서함 권한처리(+쉐어포인트)
                DirectoryCommon dirCommon = new DirectoryCommon();
                GlossaryGatheringBiz ggBiz = new GlossaryGatheringBiz();
                DataSet dirList = ggBiz.GlossaryGatheringMenu_List(Board.GatheringID, "Dir");

                if (dirList.Tables.Count > 0 && dirList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dirList.Tables[0].Rows)
                    {   
                        dirCommon.SaveUserList(dr["CommonID"].ToString(), u.UserID, UDList.AuthID, UDList.AuthCL, mode);   
                    }
                }
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //새로고침
            Response.Redirect("Main.aspx");

        }

        //문서함 삭제처리
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryGatheringBiz gbiz = new GlossaryGatheringBiz();

            string GatheringID = this.hdGatheringID.Value;
            string UserID = u.UserID;
            
            //Author : 개발자-김성환D, 리뷰자-진현빈D
            //Create Date : 2016.08.25 
            //Desc : 모임 삭제 로직 변경
            gbiz.GlossaryGathering_Delete(GatheringID, UserID,u.userIp,u.userMachineName);


            //문서삭제
            DataSet ds = new DataSet();
            ds = gbiz.GlossaryGatheringMenu_List(GatheringID, "Dir");

            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            Board.UserID = u.UserID;
            Board.RegID = u.UserID;
            Board.RegNM = u.Name;

            for(int i=0;i < ds.Tables[0].Rows.Count; i++){
                Board.DirID = ds.Tables[0].Rows[i]["CommonID"].ToString();

                //Directory 삭제
                biz.DirectoryDelete(Board);
            }
            
            //쉐어포인트 제거 보류
            //웹서비스  객체 생성  
            //SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();
            //string errMsg = string.Empty;
            //try
            //{
            //    using (new OperationContextScope(proxy.InnerChannel))
            //    {
            //        // Add a HTTP Header to an outgoing request
            //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //        requestMessage.Headers["tikle"] = "31163105310731083101";
            //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

            //        Result result = proxy.DeleteFolder(hdDirectoryID.Value, "skt\\" + u.UserID);
            //        Result result = proxy.DeleteFile(hdDirectoryID.Value, hdFileID.Value, "skt\\" + u.UserID, "Y");

            //        if (result.STATUS == 0) //성공 
            //        {
            //            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    errMsg = ex.Message;
            //}

            Response.Redirect("Main.aspx?DivType=" + DivType);
        }
    }
}