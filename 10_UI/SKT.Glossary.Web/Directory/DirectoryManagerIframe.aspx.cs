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

namespace SKT.Glossary.Web.Directory
{
    public partial class DirectoryManagerIframe : System.Web.UI.Page
    {
        // <summary>
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
			}
        }

        /// <summary>
        /// 폴더사용자 관리 저장  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            string DivID = string.Empty;
            string WorkType = "M";
            string tkType = "Directory"; 
            string errMsg = string.Empty;
            DivID = hdDirectoryID.Value;
            bool alertCheck = false;

            DirectoryMgrType MGR = new DirectoryMgrType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;

            MGR.CommonID = hdDirectoryID.Value;
            MGR.ManagerID = UDList.AuthID;
            MGR.ManagerName = UDList.AuthName;
            MGR.ManagerType = UDList.AuthCL;
            MGR.AUTH_ID = u.UserID;
            MGR.AUTH_NM = u.Name;


            biz.DirectoryManagerDelete(DivID, tkType);
            biz.DirectoryManagerInsert(MGR, tkType);


            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds1 = Dac.GetDirectoryManagerCheck(DivID);
            
            GlossaryDirectoryAuthBiz DirBiz = new GlossaryDirectoryAuthBiz();
            string[] arrMgrID = UDList.AuthID.Split('/');
            string tmpMgrID = string.Empty;
            string tmpMgrType = string.Empty;
            DataRow[] drr;

            foreach (string authid in arrMgrID)
            {
                if (authid != "") { 
                    drr = ds1.Tables[0].Select(" EMPNO IN ('"+authid+"')");
                    if(drr.Length < 1){
                        tmpMgrID = tmpMgrID+ authid + "/";
                        tmpMgrType = tmpMgrType+"U/";
                    }
                }
            }

            if (tmpMgrID != string.Empty) {
                alertCheck = true;
                
                DirBiz.GlossaryDirectoryAuthInsert(DivID, u.UserID, tmpMgrID, tmpMgrType, "RW", mode);
            

                MGR.ManagerID = UDList.AuthID;
                MGR.ManagerName = UDList.AuthName;


                DataSet ds = biz.GetDirectoryAuth(DivID);
                string addManagerID = string.Empty;
                string addManagerName = string.Empty;
                string addManagerType = string.Empty;
                string tmpName = string.Empty;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["SName"] == null || dr["SName"].ToString() == "")
                        {
                            tmpName = "[그룹]";
                        }
                        else
                        {
                            tmpName = dr["SName"].ToString();
                        }
                        addManagerID = addManagerID + dr["AUTH_ID"].ToString() + "/";
                        addManagerName = addManagerName + tmpName + "&";
                        addManagerType = addManagerType + dr["AUTH_TYPE"].ToString() + "/";
                    }

                    MGR.ManagerID = MGR.ManagerID + addManagerID;
                    MGR.ManagerName = MGR.ManagerName + addManagerName;
                    MGR.ManagerType = MGR.ManagerType + addManagerType;
                }

                List<T_Authority> authorityList = new List<T_Authority>();
                string[] ToUser = MGR.ManagerID.Split('/');
                string[] ToUserType = MGR.ManagerType.Split('/');


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
            }

            //Author : 개발자-김성환D, 리뷰자-진현빈D
            //  Create Date : 2016.12.02 
            //  Desc : 멤버에 포함되지 않은 구성원 추가시 alert 추가
            if (alertCheck)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>alert('문서함 멤버가 아닌 경우, 문서함으로 초대됩니다.'); window.parent.refreshMe('{0}','{1}')</script>", DivID, DivType));
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>window.parent.refreshMe('{0}','{1}')</script>", DivID, DivType));
            }
			

        }
    }
}