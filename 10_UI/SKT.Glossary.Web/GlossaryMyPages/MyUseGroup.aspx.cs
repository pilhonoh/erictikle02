using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Common.TikleDocManagerService;
using System.Collections;

using Newtonsoft.Json;

using System.Web.Services;
using System.ServiceModel.Channels;
using System.ServiceModel; 


namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyUseGroup : System.Web.UI.Page
    {
        protected string UserID = string.Empty;
        protected string RootURL = string.Empty;
        protected string UserName = string.Empty;
        protected string UserTeamCode = string.Empty;
        protected string UserTeam = string.Empty;
        protected string UserEmail = string.Empty;

        protected string ReaderUserID = string.Empty;
        protected bool bOwn = true;
        protected string mode = string.Empty;

        UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {

            ClientScript.GetPostBackEventReference(this, string.Empty);

			u = new UserInfo(this.Page);
			UserID = u.UserID;
            
            mode = string.IsNullOrEmpty(hidMode.Value) ? "Select" : hidMode.Value;

			this.UserControl.UserGroupVisible = false;  
        }


        /// <summary>
        /// 조회  
        /// </summary>
		[WebMethod]
		public static ArrayList GetMyGroupWeb(string UserID)
        {

            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();

            //GlossaryGroupListType list = biz.GlossaryMyGroupSelect(u.UserID);
			ArrayList list = biz.GlossaryMyGroupSelect(UserID);

			//if (list.Count > 0)
			//{
			//	NoData.Visible = false;
			//}

            //rptGroup.DataSource = list;
            //rptGroup.DataBind();

			return list;
        }


        /// <summary>
        /// 그리드 바인딩  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptGroup_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Literal GroupName = (Literal)e.Item.FindControl("GrpNm");
                Literal GroupDelete = (Literal)e.Item.FindControl("GrpDelete");

                GlossaryGroupListType data = ((GlossaryGroupListType)e.Item.DataItem);

                GroupName.Text = "<a href=\"javascript:fnSelect('"
                                + ((GlossaryGroupListType)e.Item.DataItem).MyGrpID
                                + "', '" + data.MyGrpNM + "'); \" class=\"al\" >" + data.MyGrpNM + "</a>";


                GroupDelete.Text = "<a href=\"javascript:fnMyGrpDelete('"
                                  + ((GlossaryGroupListType)e.Item.DataItem).MyGrpID
                                  + "')\" class=\"btn_s\" ><b>삭제</b></a>";
            }
        }

        /// <summary>
        /// 소속회원 조회 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnGruopList_Click(object sender, EventArgs e)
        //{
        //    u = new UserInfo(this.Page);
        //    UserName = u.Name;
        //    UserID = u.UserID;
        //    UserControl.Visible = true;
        //    GlossaryMyGroupBiz permissionsBiz = new GlossaryMyGroupBiz();
        //    List<GlossaryGroupAuthType> info = permissionsBiz.MyGroupListSelect(u.UserID, hdGrpID.Value, "MyGroup");
        //    hdGroupString.Value = JsonConvert.SerializeObject(info);
        //}

        /// <summary>
        /// 저장  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            
            GlossaryGroupListType Board = new GlossaryGroupListType();
            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();

            Board.MyGrpID = mode.Equals("Insert") ? string.Empty : txtGrpID.Value; 
            Board.MyGrpNM = SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.RegID = u.UserID;
            Board.RegNM = u.Name;
            Board.AudidID = u.UserID;

            //biz.GlossaryMyGroupInsert(Board);

            Board = biz.GlossaryMyGroupInsert(Board, mode);
            hdGrpID.Value = mode.Equals("Insert") ? Board.MyGrpID : txtGrpID.Value; 

            //******************//
            //조직도 저장  
            //******************//

            GlossaryGroupListType DirBoard = new GlossaryGroupListType();

            try
            {
                //인서트
                SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;

                if (mode.Equals("Insert"))
                {
                    //biz.GlossaryMyGroupListInsert(hdGrpID.Value, u.UserID, hdUserItemID.Value, "Insert");
                    biz.GlossaryMyGroupListInsert(hdGrpID.Value, u.UserID, UDList.AuthID, UDList.AuthCL, mode);
                }
                else
                {
                    //delete 
                    biz.GlossaryMyGroupListInsert(hdGrpID.Value, u.UserID, UDList.AuthID, UDList.AuthCL, "Delete");


                    //insert 
                    biz.GlossaryMyGroupListInsert(hdGrpID.Value, u.UserID, UDList.AuthID, UDList.AuthCL, "Insert");
                } 


            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            ////************************//
            ////저장후 Share Point 동기화  
            ////***********************//
            //List<T_Authority> authorityList = null;
            //SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = null;
            //DataSet dsDir = null;
            //DataSet dsUser  = null; 

            //try
            //{

            //    //1. 디렉토리 조회 
            //    dsDir = biz.GlossaryMyGroupChangeSelect("Select", hdGrpID.Value);
            //    if (dsDir != null & dsDir.Tables.Count > 0 && dsDir.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dsDir.Tables[0].Rows)
            //        {

            //            //Share Point에 변경할 유저리스트 조회  
            //            dsUser = biz.GlossaryMyGroupUserChangeSelect("Select", dr["DIR_ID"].ToString());
            //            if (dsUser != null & dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
            //            {
            //                //배열생성  
            //                authorityList = new List<T_Authority>();

            //                //DIR_ID를 폴더별로 변경유저 수집  
            //                foreach (DataRow drUser in dsUser.Tables[0].Rows)
            //                {
            //                    //AUTH_TYPE
            //                    if (drUser["AUTH_TYPE"].ToString().Equals("U"))
            //                    {
            //                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + drUser["AUTH_ID"].ToString(), TYPE = "P" }); //개인
            //                    }
            //                    else
            //                    {
            //                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + drUser["AUTH_ID"].ToString(), TYPE = "G" });   //그룹
            //                    }

            //                }

            //                //********************//
            //                //Share Point 로 날린다 
            //                //********************//

            //                //웹서비스  객체 생성  
            //                proxy = new DocManagerServiceClient();

            //                using (new OperationContextScope(proxy.InnerChannel))
            //                {
            //                    // Add a HTTP Header to an outgoing request
            //                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //                    requestMessage.Headers["tikle"] = "31163105310731083101";
            //                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

            //                    Result result = proxy.CreateFolder(dr["DIR_ID"].ToString(), "skt\\" + u.UserID, "M", authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>());
            //                }

            //            }
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    errMsg = ex.Message;
            //}
            //finally
            //{
            //    proxy = null;
            //    //dsDir = null;
            //    //dsUser = null; 
            //    //dsDir.Dispose();
            //    //dsUser.Dispose(); 

            //} 

            //Response.Redirect("MyUseGroup.aspx");
            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// 삭제  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;

            GlossaryGroupListType Board = new GlossaryGroupListType();
            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();

            try
            {
                //userid 도 같이 넘기도록 수정
                biz.GlossaryMyGroupDelete(hdGrpID.Value); 
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //********************************//
            //그룹 삭제후 Share Point 연동 작업  
            //********************************//

            //List<T_Authority> authorityList = null;
            //SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = null;
            //DataSet dsDir = null;
            //DataSet dsUser = null;

            //try
            //{

            //    //1. 디렉토리 조회 
            //    dsDir = biz.GlossaryMyGroupChangeSelect("Select", hdGrpID.Value);
            //    if (dsDir != null & dsDir.Tables.Count > 0 && dsDir.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dsDir.Tables[0].Rows)
            //        {

            //            //Share Point에 변경할 유저리스트 조회  
            //            dsUser = biz.GlossaryMyGroupUserChangeSelect("Select", dr["DIR_ID"].ToString());
            //            if (dsUser != null & dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
            //            {
            //                //배열생성  
            //                authorityList = new List<T_Authority>();

            //                //DIR_ID를 폴더별로 변경유저 수집  
            //                foreach (DataRow drUser in dsUser.Tables[0].Rows)
            //                {
            //                    //AUTH_TYPE
            //                    if (drUser["AUTH_TYPE"].ToString().Equals("U"))
            //                    {
            //                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + drUser["AUTH_ID"].ToString(), TYPE = "P" }); //개인
            //                    }
            //                    else
            //                    {
            //                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + drUser["AUTH_ID"].ToString(), TYPE = "G" });   //그룹
            //                    }

            //                }

            //                //********************//
            //                //Share Point 로 날린다 
            //                //********************//

            //                //웹서비스  객체 생성  
            //                proxy = new DocManagerServiceClient();

            //                using (new OperationContextScope(proxy.InnerChannel))
            //                {
            //                    // Add a HTTP Header to an outgoing request
            //                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //                    requestMessage.Headers["tikle"] = "31163105310731083101";
            //                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

            //                    Result result = proxy.CreateFolder(dr["DIR_ID"].ToString(), "skt\\" + u.UserID, "M", authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>());
            //                }

            //            }
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    errMsg = ex.Message;
            //}
            //finally
            //{
            //    proxy = null;
            //    //dsDir = null;
            //    //dsUser = null; 
            //    //dsDir.Dispose();
            //    //dsUser.Dispose(); 
            //} 

            //Reflash 
            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        //탭 메뉴
        protected void btnTebMenu_Click(object sender, EventArgs e)
        {
            //SearchType = hidMenuType.Value;
            //SearchKeyword = "";

            //if (!string.IsNullOrEmpty(ReaderUserID))
            //{
            //    OtherUserBindSelect(ReaderUserID);
            //}
            //else
            //{
            //    BindSelect();
            //}
        }

        //탭 메뉴 색
        private void TebMenuColor()
        {
            if (hidMenuType.Value == "MyWrite")
            {
                //MyWrite.Style["Color"] = "red";
                //MyModify.Style.Clear();
            }
            else
            {
                //MyModify.Style["Color"] = "red";
                //MyWrite.Style.Clear();

            }
        }

    }
}