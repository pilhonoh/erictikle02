using System;
using System.Configuration; // 삭제예정
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Common.TikleDocManagerService;
using System.ServiceModel.Channels;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Directory
{
    public partial class DirectoryWrite : System.Web.UI.Page
    {
        //aspx 변수 
        protected string mode = string.Empty;
        protected string ItemID = string.Empty;
        protected string CommonID = string.Empty;

        //입력구분자 
        protected string DivType = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string DirID = string.Empty;

        protected string UserID = string.Empty;
        protected string UserNameDept = string.Empty;

        protected string AttachInfo = "[]";
        internal const int GLOSSARY_ATTACH_ID = 100;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        // 끌.모임 설정
        protected string GatheringYN;
        protected string GatheringID;
        protected string GatheringName;
        protected string GatheringAuthor = string.Empty;
        protected string GatheringCreationDate = string.Empty;

        UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            string ajax = (Request["AJAX_METHOD"] ?? string.Empty).ToString();
            DivType = (Request["DivType"] ?? string.Empty).ToString();

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
            UserNameDept = u.DeptName;

            if (GatheringYN.Equals("Y"))
            {
                Response.Redirect("/TikleMain.aspx");
                Response.End();
            }

            //모드 
            mode = "Insert";

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

            //더 할일을 추가  
            if (!IsPostBack)
            {
                hdItemGuid.Value = Guid.NewGuid().ToString();
            }

        }


        /// <summary>
        /// 저장 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string AuthType = string.Empty;
            string errMsg = string.Empty;
            string tempDirNM = string.Empty;
            //string rdFileUp = (Request["rdFileUp"] ?? string.Empty).ToString();
            //string txtFileName = (Request["txtFileName"] ?? string.Empty).ToString();
            //2015-12-23 ksh Tnet 업로드시 아래껄 사용함
            string rdFileUp = hidFileRadioCheck.Value;
            string txtFileName = string.Empty;

            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            DirectoryCommon dirCommon = new DirectoryCommon();

            /*
                Author : 개발자-김성환D, 리뷰자-진현빈G
                Create Date : 2016.08.04 
                Desc : 특수문자 " ' \ 처리
            */
            //tempDirNM = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim();
            tempDirNM = SKT.Common.SecurityHelper.ReClear_XSS_CSRF(System.Web.HttpUtility.HtmlDecode(txtTitle.Value)).Trim();

            tempDirNM = ((tempDirNM.Replace("&lt;", "<")).Replace("&gt;", ">")).Replace("&amp;", "&");
            //Board.DirNM = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.DirNM = tempDirNM;//제목


            Board.Path = ConfigurationManager.AppSettings["AttachFilePath"].ToString();
            Board.UserID = u.UserID;
            Board.RegID = u.UserID;
            Board.RegNM = u.Name;

            //******************//
            //Directory 저장  
            //******************//

            try
            {
                Board = biz.DirectoryInsert(Board, mode);
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            DirID = Board.DirID;
            hdCommonID.Value = Board.DirID;

            // 모임.문서 정보저장

            string ToUser = this.UserControl.AuthID;
            string ToUserType = this.UserControl.AuthCL;

            if (GatheringYN == "Y")
            {
                GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
                gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Dir", DirID);

                // 모임.문서에서는 모임의 권한처리를 상속
                DataSet memberDS = gatheringBiz.GlossaryGatheringAuth_Select(this.GatheringID);

                if (memberDS.Tables.Count > 0 && memberDS.Tables[0].Rows.Count > 0)
                {
                    ToUser = string.Empty;
                    ToUserType = string.Empty;

                    foreach (DataRow dr in memberDS.Tables[0].Rows)
                    {
                        ToUser += dr["AuthID"].ToString() + "/";
                        ToUserType += dr["AuthType"].ToString() + "/";
                    }
                }
            }

            //******************//
            //조직도 저장  
            //******************//
            //dirCommon.SaveUserList(DirID, u.UserID, this.UserControl, mode);
            dirCommon.SaveUserList(DirID, u.UserID, ToUser, ToUserType, mode);

            // 파일 업로드
            string strGUID = hdItemGuid.Value;

            //GetSaveFilesInfo(DirID);

            if (rdFileUp == "upload")
            {
                //New 위치변경
                GetSaveFilesInfo(DirID);

                foreach (Dictionary<string, object> file in fileCtrl.Save())
                {

                    //txtFileName = file["FileName"].ToString().Split('.')[0];
                    txtFileName = file["FileName"].ToString().Substring(0, file["FileName"].ToString().LastIndexOf('.'));



                    //******************//
                    //파일 업로드
                    //******************//
                    dirCommon.FileUpload(DirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
                }
            }
            else
            {
                txtFileName = hidFileDefaultName.Value;

                //******************//
                //파일 업로드
                //******************//
                dirCommon.FileUpload(DirID, rdFileUp, strGUID, u.UserID, u.Name, txtFileName);
            }

            //새로고침  
            Response.Redirect("DirectoryView.aspx?DivType=" + DivType + "&DivID=" + DirID + "&GatheringYN=" + GatheringYN + "&GatheringID=" + GatheringID + "&MenuType=Directory");

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
    }
}