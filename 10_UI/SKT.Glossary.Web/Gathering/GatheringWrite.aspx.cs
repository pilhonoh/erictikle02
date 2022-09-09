using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Web.Common.Controls;
using SKT.Glossary.Web.Directory;

namespace SKT.Glossary.Web.Gathering
{
    public partial class GatheringWrite : System.Web.UI.Page
    {

        //aspx 변수 
        protected string mode = string.Empty;
        protected string ItemID = string.Empty;
        protected string CommonID = string.Empty;

        //입력구분자 
        protected string DivType = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string GatheringID = string.Empty;

        protected string UserID = string.Empty;
        protected string UserNameDept = string.Empty;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            string ajax = (Request["AJAX_METHOD"] ?? string.Empty).ToString();
            DivType = (Request["DivType"] ?? string.Empty).ToString();

            u = new UserInfo(this.Page);
            UserID = u.UserID;
            UserNameDept = u.DeptName;

            //등록금지
            Response.Redirect("/TikleMain.aspx");
            Response.End();

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
            string errMsg = string.Empty;

            GlossaryGatheringType item = new GlossaryGatheringType();
            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            item.GatheringName = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            item.Author = u.UserID;
            item.Editor = u.UserID;
            item.UseYN = "Y";

            //******************//
            // 1. 끌.모임 정보 저장
            //******************//

            try
            {
                item = biz.GlossaryGathering_Insert(item, mode);
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            GatheringID = item.GatheringID;
            hdCommonID.Value = item.GatheringID;


            //******************//
            // 2. 권한정보 저장
            //******************//
            UserAndDepartmentList UDList = this.UserControl;
            
            //삭제후 인서트   
            try
            {
                // 1. 기존 권한 정보 제거   
                biz.GlossaryGatheringAuth_Delete(GatheringID);

                // 2. 새 권한 정보 등록(UDList.AuthCL은 실제로 AuthType 값을 갖음. U / O / G)
                biz.GlossaryGatheringAuth_Insert(GatheringID, u.UserID, UDList.AuthID, UDList.AuthCL, "RW", mode);

            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //******************//
            // 3. 쪽지발송
            //******************//
            
            List<string> Guest = new List<string>();

            GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            DataSet ds = gBiz.GlossaryGathering_MemberList(GatheringID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dr["EMPNO"].ToString().Equals(u.UserID))
                    {
                        SendNote(dr["EMPNO"].ToString(), item.GatheringName);
                    }
                }
            }


            //******************//
            // 4. 문서함 자동생성
            //******************//
            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz dirBiz = new GlossaryDirectoryBiz();
            DirectoryCommon dirCommon = new DirectoryCommon();

            string tempDirNM = item.GatheringName;
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
                Board = dirBiz.DirectoryInsert(Board, mode);
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            string DirID = Board.DirID;
            //hdCommonID.Value = Board.DirID;

            // 모임.문서 정보저장

            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Dir", DirID);

            string ToUser = this.UserControl.AuthID;
            string ToUserType = this.UserControl.AuthCL;

            //if (GatheringYN == "Y")
            //{
            //    GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            //    gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Dir", DirID);

            //    // 모임.문서에서는 모임의 권한처리를 상속
            //    DataSet memberDS = gatheringBiz.GlossaryGatheringAuth_Select(this.GatheringID);

            //    if (memberDS.Tables.Count > 0 && memberDS.Tables[0].Rows.Count > 0)
            //    {
            //        ToUser = string.Empty;
            //        ToUserType = string.Empty;

            //        foreach (DataRow dr in memberDS.Tables[0].Rows)
            //        {
            //            ToUser += dr["AuthID"].ToString() + "/";
            //            ToUserType += dr["AuthType"].ToString() + "/";
            //        }
            //    }
            //}

            //******************//
            //조직도 저장  
            //******************//
            //dirCommon.SaveUserList(DirID, u.UserID, this.UserControl, mode);
            dirCommon.SaveUserList(DirID, u.UserID, ToUser, ToUserType, mode);

            //새로고침
            Response.Redirect("Main.aspx");

        }

        private void SendNote(string uid, string gName)
        {
            GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
            ImpersonUserinfo ui = biz_.UserSelect(uid);

            string Recipient = ui.EmailAddress;

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            string NoteLink = BaseURL + "Gathering/Main.aspx";

            string NoteBody = "<html><body><font face='맑은고딕' size='2'>안녕하세요, 티끌이입니다. ^^<br /><br />"
                            + u.Name + "님께서 <STRONG>&lt;" + gName + "&gt;</STRONG> 모임에 초대하였습니다.<br /><br />"
                            + "<font face='맑은고딕' size='2'><a href='http://tikle.sktelecom.com/Gathering/Main.aspx'>▶ 끌.모임 바로가기</a></font></body></html>";

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
            CBHInterface.CBHMailSend(Recipient, u.EmailAddress, "T.끌 알림 메일입니다.", NoteBody);
        }
    }
}