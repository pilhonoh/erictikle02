using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using SKT.Common;
using SKT.Glossary.Biz;
using System.Data;


namespace SKT.Glossary.Web.Common.Controls
{
    public partial class AbsenceUserAndDepartmentList : System.Web.UI.UserControl
	{
		protected string RootURL = string.Empty;
		public string ItemID = string.Empty;
		protected string Title = string.Empty;
		protected string UserID = string.Empty;
        protected string UserDeptCode = string.Empty;
        protected string UserDeptName = string.Empty;
		
		public bool bGrp = true; /* 그룹리스트를 보여줄지 여부, 그룹관리 화면에서는 그룹리스트를 보여주지 않는다. */

		public string AuthID
		{
			get
			{
				return hidUserItemID.Value;
			}
		}

		public string AuthName
		{
			get
			{
				return hidUserName.Value;
			}
		}

		public string AuthCL
		{
			get
			{
				return hidUserType.Value;
			}
		}

        public bool UserGroupVisible
        {
            set
            {
                bGrp = value;
            }
        }
            
        


		protected void Page_Load(object sender, EventArgs e)
		{   
			RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
			ItemID = (Request["ItemID"] ?? string.Empty).ToString();
			Title = (Request["SearchKeyword"] ?? string.Empty).ToString();
			UserInfo u = new UserInfo(this.Page);
			UserID = u.UserID;
            UserDeptCode = u.DeptID;
            UserDeptName = u.DeptName;

			GetUserGroupList(); 
		}

		private void GetUserGroupList()
		{

			UserInfo u = new UserInfo(this.Page);
			UserID = u.UserID;

			DataSet ds = null;

			GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();

			ds = biz.ddlDirectorySelect(UserID);
			if (ds != null && ds.Tables.Count > 0)
			{
				ddlGroupUser.Items.Insert(0, new ListItem("그룹을 선택하세요.", "0"));

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					//스크립트 오류로 잠시 주석처리  
                    ddlGroupUser.Items.Insert(i + 1, new ListItem("[그룹]" + ds.Tables[0].Rows[i]["MYGRP_NM"].ToString(), ds.Tables[0].Rows[i]["MYGRP_ID"].ToString()));
				}

                ddlGroupUser.SelectedIndex = 0;
			}
			else
			{
                ddlGroupUser.Items.Insert(0, new ListItem("사용자정의 그룹이 없습니다.", "0"));
			}

		}
	}
}