using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Collections;
using System.Web.Services;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;

using SKT.Common.TikleDocManagerService;
using System.Text.RegularExpressions;
using System.Text;

//using DSAPILib;

using System.ServiceModel.Channels;
using System.ServiceModel;  


namespace SKT.Glossary.Web.Common.Controls
{
    public partial class MessageTransfer : System.Web.UI.Page
    {
        protected string RootURL = string.Empty;

        /// <summary>
        /// 라디오 버튼 구분자 
        /// </summary>
        protected string RdoType = string.Empty;

        /// <summary>
        /// 페이지 구분자 문서공유는 Directory
        /// 의견공유 Suvery 
        /// 사용자   User  
        /// </summary>
        protected string PageType = string.Empty;

        //첫번째는 조회시 필수 조건 
        /// <summary>
        /// 문서공유는 Dir_ID
        /// 의견공유는 Survey_ID 
        /// 사용자는 보내는 사람의 ID 
        /// </summary>
        protected string FirID = string.Empty;

        /// <summary>
        /// 두번째 매개변수(선택) 
        /// </summary>
        protected string SecID = string.Empty;


        protected string Title = string.Empty;

        protected string UserID = string.Empty;
    
        protected bool bDir = false;
 

        UserInfo u;


        protected void Page_Load(object sender, EventArgs e)
        {

            ClientScript.GetPostBackEventReference(this, string.Empty);

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;

            RdoType = (Request["rdoType"] ?? string.Empty).ToString();
            PageType = (Request["pageType"] ?? string.Empty).ToString();
            FirID = (Request["firID"] ?? string.Empty).ToString();
            SecID = (Request["secID"] ?? string.Empty).ToString();

            Title = (Request["SearchKeyword"] ?? string.Empty).ToString();

            if (PageType == "Directory")
            {
                hdDirectoryID.Value = FirID;
                hdFileID.Value = SecID;
                txtTitle.Value = SecID; 

                bDir = true;
            }
            else if (PageType == "Survey")
            {
                hdDirectoryID.Value = FirID;
                txtTitle.Value = SecID;  
            }


            u = new UserInfo(this.Page);
            UserID = u.UserID;
 

        }

        /// <summary>
        /// 쪽지/SMS/email발송 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
 
            string MailSenderEmail = string.Empty; 
            string Recipient = string.Empty;
            string NoteBody = string.Empty;
            string errMsg = string.Empty;

            string AuthID = string.Empty; 

            u = new UserInfo(this.Page);
			CommonAuthType DirBoard = new CommonAuthType();
            GlossaryDirectoryAuthBiz DirBiz = new GlossaryDirectoryAuthBiz();

            SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;

            ArrayList arryList = DirBiz.GlossaryInfo_Select(UDList.AuthCL, UDList.AuthID);
            if (hdType.Value.Equals("Paper"))   //쪽지  
            {
                for (int i = 0; i < arryList.Count; i++)
                {
                    GlossarySendType data = (GlossarySendType)arryList[i];

                    SendNote(txtTitle.Value, SecID, data.mail, Recipient, NoteBody, u.Name, hdDirectoryID.Value);
                }
            }
            else if (hdType.Value.Equals("Mail"))   //메일   
            {
                for (int i = 0; i < arryList.Count; i++)
                {
                    GlossarySendType data = (GlossarySendType)arryList[i];
                    SendMail(txtTitle.Value, SecID, data.mail, Recipient, NoteBody);
                }
            }
            else if (hdType.Value.Equals("SMS"))   //SMS
            {
                for (int i = 0; i < arryList.Count; i++)
                {
                    GlossarySendType data = (GlossarySendType)arryList[i];
                    SendSMS(txtTitle.Value, SecID, data.mail, Recipient, data.mobile);
                }
            }


            //***********************************************//
            //문서공유에서 보낸 경우 Share Point 와 연동작업을 한다 
            //**********************************************// 

            if (bDir)
            {
                //조직도 변경으로 함수변경 
                //DirBiz.GlossaryDirectoryAuthInsert(hdDirectoryID.Value, u.UserID, AuthID, "R", "Insert");
                //UDList.AuthCL은 auth_type 
                DirBiz.GlossaryDirectoryAuthInsert(hdDirectoryID.Value, u.UserID, UDList.AuthID, UDList.AuthCL, "R", "Insert");

                //****************************//
                // Share Point 서버 저장   
                //***************************//

                string AuthType = string.Empty;
                string DirName = FirID; 
                string FileName = SecID;

                List<T_Authority> authorityList = new List<T_Authority>();
                string[] ToUser = AuthID.Split('/');
                for (int i = 0; i < ToUser.Length - 1; i++)
                {
                    AuthType = DirBiz.DirectoryAuthTypeList(ToUser[i]);
                    if (AuthType.Equals("1"))
                    {
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + ToUser[i].ToString(), TYPE = "P" }); //개인
                    }
                    else if (AuthType.Equals("2"))
                    {
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + ToUser[i].ToString(), TYPE = "G" });   //그룹
                    }
                }

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

                        Result result = proxy.SetFileReadPermission(hdDirectoryID.Value, @FileName, authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>(), "skt\\" + u.UserID);
                        if (result.STATUS != 0)
                        {
                            //Response.Write("<script>alert('성공');</script>"); 
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    errMsg = ex.Message;
                }

            } 
        }

        /// <summary>
        /// 쪽지 
        /// </summary>
        /// <param name="NoteBody"></param>
        /// <param name="NoteLink"></param>
        /// <param name="SendUserEmail"></param>
        protected void SendNoteQueue(string NoteBody, string NoteLink, string SendUserEmail)
        {

            if (SendUserEmail.Length == 0)
            {
                return;
            }
            CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHNoteType data = new CBHNoteType();
            data.Content = NoteBody;
            data.Kind = "3"; //일반쪽지.
            data.URL = NoteLink;
            data.SendUserName = "티끌이";


            string userID = SendUserEmail.Remove(SendUserEmail.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
            data.TargetUser = userID;

            helper.SendNoteToQueue(data);
        }


        /// <summary>
        /// 쪽지 보내기 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="CommonID"></param>
        /// <param name="MailSenderEmail"></param>
        /// <param name="Recipient"></param>
        /// <param name="NoteBody"></param>
        protected void SendNote(string Title, string CommonID, string MailSenderEmail, string Recipient, string NoteBody , string Kname, string DirId)
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["CBHService_Use_Flag"] == "N") return;

            string NoteTitle = string.Empty;
            string BaseURL = string.Empty;
            string NoteLink = string.Empty;

            string bottomStr = string.Empty;

            //테스트 
            //BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            if (PageType.Equals("Directory"))
            {
                // data.Add("file", fileUrl);
                // data.Add("tikle", "31163105310731083101");

                string fileLink = DirId + "/" + Title;

                string EncryptfileLink = SKT.Common.CryptoHelper.AESEncryptString(fileLink, "sktelecom_tikle2");

                //NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + HttpUtility.UrlEncode(fileLink) + "&tikle=31163105310731083101";
                //NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + Server.UrlEncode(fileLink) + "&tikle=31163105310731083101";
                NoteLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + EncryptfileLink + "&tikle=1"; 


                NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">" + Kname + "님께서  공유하신 문서가 전달되었습니다.<br/></font>"; //보낼 메세지 만들기.
                bottomStr = "<font face=\"맑은고딕\" size=\"2\">▶ 끌.문서 바로가기: ＇<a href=\"" + NoteLink + "\" target=\"_docs\" >" + Title + "</a><br /></font></body></html>";
            }
            else
            {
                NoteTitle = "[T.끌]: '" + Title + "'이 편집되었습니다.";
                
                NoteLink = BaseURL + "Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + CommonID;
                NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">" + MailSenderEmail + "님께서 작성하신 " + Title + " 이 편집되었습니다.<br/></font>"; //보낼 메세지 만들기.
                bottomStr = "<font face=\"맑은고딕\" size=\"2\">▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";
            }

            #region 
            /* 
            //작성자 / 알람설정한 사용자에게 쪽지 보내기
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GetDocHistoryUsers(CommonID);

            for (int i = 0; i < ds.Tables.Count; i++)
                foreach (DataRow dr in ds.Tables[i].Rows)
                {
                    if (i == 0)
                    {
                        //최초글에 최초 편집
                        if (dr["HistoryCount"].ToString() == "1")
                        {
                            NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">" + dr["UserName"].ToString() + "님께서 작성하신 " + Title + " 이 편집되었습니다.<br/></font>"; //보낼 메세지 만들기.
                            Recipient = dr["UserEmail"].ToString();
                            NoteBody = NoteBody + bottomStr;  //보낼 메세지 만들기.
                            SendNoteQueue(NoteBody, NoteLink, Recipient);
                            SendEmailAddress += dr["UserEmail"].ToString() + ", ";
                        }
                        // 편집한 사람
                        else
                        {
                            if (SendEmailAddress.IndexOf(dr["UserEmail"].ToString()) == -1) //이미 보낸사람이 있는지 체크해서 안보냄.. 위에 작성자가 편집시 여기서 걸러질듯.
                            {
                                NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">" + dr["UserName"].ToString() + "님께서 편집하신 " + Title + " 이 편집되었습니다.<br/></font>";  //보낼 메세지 만들기.
                                Recipient = dr["UserEmail"].ToString();
                                SendNoteQueue(NoteBody, NoteLink, Recipient);
                                NoteBody = NoteBody + bottomStr;  //보낼 메세지 만들기.
                                SendEmailAddress += dr["UserEmail"].ToString() + ", ";
                            }
                        }

                    }
                    else
                    {
                        if (SendEmailAddress.IndexOf(dr["UserEmail"].ToString()) == -1) //이미보내지않은사람이면
                        {
                            //알람 설정한 사람
                            NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">알림 설정하신 " + Title + " 이 편집되었습니다.<br/></font>";  //보낼 메세지 만들기.
                            Recipient = dr["UserEmail"].ToString();
                            NoteBody = NoteBody + bottomStr;  //보낼 메세지 만들기.
                            SendNoteQueue(NoteBody, NoteLink, Recipient);
                        }
                    }

                    //NoteBody = NoteBody + bottomStr;  //보낼 메세지 만들기.
                    //SendNoteQueue(NoteBody, NoteLink, Recipient);

                }
             */
            #endregion 


            Recipient = MailSenderEmail;
            NoteBody = NoteBody + bottomStr;  //보낼 메세지 만들기.
            
            SendNoteQueue(NoteBody, NoteLink, Recipient);

            //테스트  
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "fileOpen", "fileOpen('" + NoteLink + "')", true);
      


        }

        /// <summary>
        /// 메일 보내기  
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="CommonID"></param>
        /// <param name="MailSenderEmail"></param>
        /// <param name="Recipient"></param>
        /// <param name="MailBody"></param>
        protected void SendMail(string Title, string CommonID, string MailSenderEmail, string Recipient, string MailBody = "")
        {


            string MailTitle = "[T.끌]: '" + Title + "'이 편집되었습니다.";

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //string MailLink = BaseURL +"/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
            string MailLink = BaseURL + "Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + CommonID;

            if (MailBody == "")
            {
                MailBody = "<html><body>안녕하세요, 티끌이입니다. ^^<br /><br />"
                                    + "'T.끌'에 등록하신 '<a href=\"" + MailLink + " \">" + Title + "</a>'이 편집되었으니 확인 부탁드립니다!.<br /><br />"
                                    + "오늘도 좋은 하루 되세요~! <br /><br /></body></html>";
            }
            string bottomStr = "▶신규 글 제목 : ＇<a href=\"" + MailLink + " \">" + Title + "</a><br /><br><br>";
            //MailBody = MailBody + bottomStr;

            CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHMailType data = new CBHMailType();

            Recipient = MailSenderEmail;
            data.Subject = MailTitle;
            data.Content = MailBody;

            data.SenderEmail = "tikle@sk.com";
            data.ReceiverEmail = Recipient;

            helper.SendMailToQueue(data);

            /*DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.GlossaryAlarmSelect(CommonID, "Write", "");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["MailYN"].ToString() == "Y" && dr["TikleNoteYN"].ToString() == "Y" || string.IsNullOrEmpty(dr["TikleNoteYN"].ToString()))
                    {
                        Recipient = dr["UserEmail"].ToString();
                        data.Subject = MailTitle;
                        data.Content = MailBody;

                        data.SenderEmail = "tikle@sk.com";
                        data.ReceiverEmail = Recipient;
                        helper.SendMailToQueue(data);
                    }
                }
            } */


        }


        /// <summary>
        /// SMS
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="CommonID"></param>
        /// <param name="MailSenderEmail"></param>
        /// <param name="Recipient"></param>
        /// <param name="MailBody"></param>
        protected void SendSMS(string Title, string CommonID, string MailSenderEmail, string Recipient, string mobile)
        {
            
            string MailBody = ""; 
            string MailTitle = "[T.끌]: '" + Title + "'이 편집되었습니다.";

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //string MailLink = BaseURL +"/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
            string MailLink = BaseURL + "Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + CommonID;

            if (MailBody == "")
            {
                MailBody = "<html><body>안녕하세요, 티끌이입니다. ^^<br /><br />"
                                    + "'T.끌'에 등록하신 '<a href=\"" + MailLink + " \">" + Title + "</a>'이 편집되었으니 확인 부탁드립니다!.<br /><br />"
                                    + "오늘도 좋은 하루 되세요~! <br /><br /></body></html>";
            }
            string bottomStr = "▶신규 글 제목 : ＇<a href=\"" + MailLink + " \">" + Title + "</a><br /><br><br>";
            //MailBody = MailBody + bottomStr;

            CBHMSMQHelper helper = new CBHMSMQHelper();

            //CBHMailType data = new CBHMailType();
            CBHSMSType data = new CBHSMSType();

            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.GlossaryAlarmSelect(CommonID, "Write", "");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["MailYN"].ToString() == "Y" && dr["TikleNoteYN"].ToString() == "Y" || string.IsNullOrEmpty(dr["TikleNoteYN"].ToString()))
                    {
                        Recipient = dr["UserEmail"].ToString();


                        data.ConsumerID = MailTitle;
                        data.Title = MailTitle;
                        data.Start_DT_HMS = DateTime.Now.ToString();
                        data.End_DT_HMS = DateTime.Now.ToString();

                        data.Phone = mobile;
                        data.RplyPhoneNum = mobile;
                  
                        helper.SendSMSToQueue(data);
                    }
                }
            }
        }




    }
}