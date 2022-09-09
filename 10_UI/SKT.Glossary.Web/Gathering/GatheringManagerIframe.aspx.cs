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

using SKT.Glossary.Web.Directory;

namespace SKT.Glossary.Web.Gathering
{
    public partial class GatheringManagerIframe : System.Web.UI.Page
    {
        // <summary>
        /// 모드  
        /// </summary>
        protected string mode = string.Empty;

        protected string GatheringID = string.Empty;
        protected string GatheringNM = string.Empty;
        protected string GatheringType = string.Empty;
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
                GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();
                GatheringNM = (Request["GatheringNM"] ?? string.Empty).ToString();
                GatheringType = (Request["GatheringType"] ?? string.Empty).ToString();
                SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();

                GatheringType = string.IsNullOrEmpty(GatheringType) ? "Pub" : GatheringType;

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

            string GatheringID = string.Empty;
            string WorkType = "M";
            string tkType = "Gathering";
            string errMsg = string.Empty;
            GatheringID = hdGatheringID.Value;
            bool alertCheck = false;

            DirectoryMgrType MGR = new DirectoryMgrType();
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            SKT.Glossary.Web.Common.Controls.UserAndDepartmentList UDList = this.UserControl;

            MGR.CommonID = hdGatheringID.Value;
            MGR.ManagerID = UDList.AuthID;
            MGR.ManagerName = UDList.AuthName;
            MGR.ManagerType = UDList.AuthCL;
            MGR.AUTH_ID = u.UserID;
            MGR.AUTH_NM = u.Name;

            //모임이지만 DirectoryManager는 공통으로 사용한다.
            biz.DirectoryManagerDelete(GatheringID, tkType);
            biz.DirectoryManagerInsert(MGR,tkType);

            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();

            GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            DataSet ds1 = gBiz.GlossaryGathering_MemberList(GatheringID);

            string[] arrMgrID = UDList.AuthID.Split('/');
            string tmpMgrID = string.Empty;
            string tmpMgrType = string.Empty;
            DataRow[] drr;

            foreach (string authid in arrMgrID)
            {
                if (authid != "")
                {
                    drr = ds1.Tables[0].Select(" EMPNO IN ('" + authid + "')");
                    if (drr.Length < 1)
                    {
                        tmpMgrID = tmpMgrID + authid + "/";
                        tmpMgrType = tmpMgrType + "U/";
                    }
                }
            }


            if (tmpMgrID != string.Empty)
            {
                alertCheck = true;

                // 2. 새 권한 정보 등록(UDList.AuthCL은 실제로 AuthType 값을 갖음. U / O / G)
                gBiz.GlossaryGatheringAuth_Insert(GatheringID, u.UserID, tmpMgrID, tmpMgrType, "RW", mode);
                //DirBiz.GlossaryDirectoryAuthInsert(GatheringID, u.UserID, tmpMgrID, tmpMgrType, "RW", mode);


                MGR.ManagerID = UDList.AuthID;
                MGR.ManagerName = UDList.AuthName;


                DataSet ds = biz.GetGatheringAuth(GatheringID);
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
                        addManagerID = addManagerID + dr["AuthID"].ToString() + "/";
                        addManagerName = addManagerName + tmpName + "&";
                        addManagerType = addManagerType + dr["AuthType"].ToString() + "/";
                    }

                    MGR.ManagerID = MGR.ManagerID + addManagerID;
                    MGR.ManagerName = MGR.ManagerName + addManagerName;
                    MGR.ManagerType = MGR.ManagerType + addManagerType;
                }

                //삭제후 인서트   
                try
                {
                    // 3. 문서함 권한처리(+쉐어포인트)
                    DirectoryCommon dirCommon = new DirectoryCommon();
                    DataSet dirList = gBiz.GlossaryGatheringMenu_List(GatheringID, "Dir");

                    if (dirList.Tables.Count > 0 && dirList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dirList.Tables[0].Rows)
                        {
                            dirCommon.SaveUserList(dr["CommonID"].ToString(), u.UserID, MGR.ManagerID, MGR.ManagerType, mode);
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
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>alert('모임 멤버가 아닌 경우, 모임으로 초대됩니다.'); window.parent.refreshMe('{0}','{1}')</script>", "", ""));
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", string.Format("<script>window.parent.refreshMe('{0}','{1}')</script>", "", ""));
            }

            

        }
    }
}