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
using SKT.Glossary.Web.Common.Controls;
using System.Text.RegularExpressions;
using System.Text;
using DSAPILib;

using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Security.Cryptography;  

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class DirectoryViewIframe : System.Web.UI.Page
    {
        /// <summary>
        /// 모드  
        /// </summary>
        protected string mode = string.Empty;

        protected string DivID = string.Empty;
		protected string DivNM = string.Empty;
        protected string DivType = string.Empty;
        protected string UserID = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;


        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;
        protected string CommonID = string.Empty;

        protected static byte[] pbyteKey;   //암호화 키  


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

			

            //대상자 추가 화면 제공 여부
            //UserControlNateOnBizPop.targetBtnYn = true;
			if (!IsPostBack)
			{
				u = new UserInfo(this.Page);

				RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
				DivID = (Request["DivID"] ?? string.Empty).ToString();
				DivNM = (Request["DivNM"] ?? string.Empty).ToString();
				DivType = (Request["DivType"] ?? string.Empty).ToString();
				SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();

				DivType = string.IsNullOrEmpty(DivType) ? "Pub" : DivType;

				UserID = u.UserID;


                /*
                   Author : 개발자-김성환D, 리뷰자-진현빈G
                   Create Date : 2016.08.04 
                   Desc : 특수문자 " ' \ 처리
               */
				txtTitle.Value = DivNM;
                txtTitle.Value = SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(txtTitle.Value)).Trim(); //제목
			}
        }

        /// <summary>
        /// 폴더사용자 관리 저장  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string AuthType = string.Empty;
			string DivID = string.Empty;
            string errMsg = string.Empty;
            string mode = "Update";
            string WorkType = "M";

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

			DivID = hdDirectoryID.Value;
			
            GlossaryDirectoryType Board = new GlossaryDirectoryType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();

			Board.DirID = DivID;
           
            Board.DirNM = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
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
			SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;
            GlossaryDirectoryAuthBiz DirBiz = new GlossaryDirectoryAuthBiz();

            //삭제후 인서트   
            try
            {
                //삭제   
				DirBiz.GlossaryDirectoryAuthDelete(DivID, "Auth");

                //인서트
                //UDList.AuthCL은 auth_type 이다  
				DirBiz.GlossaryDirectoryAuthInsert(DivID, u.UserID, UDList.AuthID, UDList.AuthCL, "RW", mode);

            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }


            //****************************//
            // Share Point 서버 저장   
            //***************************//

            List<T_Authority> authorityList = new List<T_Authority>();
            string[] ToUser = UDList.AuthID.Split('/');
            string[] ToUserType = UDList.AuthCL.Split('/');


            for (int i = 0; i < ToUser.Length; i++)
            {
               

                switch (ToUserType[i])
                {
                    case "U": // 사용자
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + ToUser[i].ToString(), TYPE = "P" }); //개인 
                        break;
                    case "O": // 조직
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + ToUser[i].ToString(), TYPE = "G" });   //그룹 
                        break;
                    case "G":  // 그룹
                        List<string> MyGrpList = AjaxPage.GetMyGroupList("MyGroup", u.UserID, ToUser[i].ToString());

                        if (MyGrpList.Count > 0)
                        {
                            foreach (String tmp in MyGrpList)
                            {
                                AjaxMyGroupList grpList = new JavaScriptSerializer().Deserialize<AjaxMyGroupList>(tmp);

                                switch (grpList.ToUserType)
                                {
                                    case "U": // 사용자
                                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + grpList.ToUserID.ToString(), TYPE = "P" }); //개인 
                                        break;
                                    case "O": // 조직
                                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + grpList.ToUserID.ToString(), TYPE = "G" });   //그룹 
                                        break;
                                }
                            }
                        }
                        break;
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

                    //폴더생성  
					Result result = proxy.CreateFolder(DivID, "skt\\" + u.UserID, WorkType, authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>());

                    if (result.STATUS == 0)
                    {
                        //사용자 카운트 update
                        GlossaryControlBiz commBiz = new GlossaryControlBiz();
						commBiz.commAuthUserCntUpdate(DivID, "Directory");

                        //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
						//ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>window.parent.refreshMe('{0},{1}')</script>", DivID, DivType));
                    }
                }

            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
			ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>window.parent.refreshMe('{0}','{1}')</script>", DivID, DivType));

        }
    }
}