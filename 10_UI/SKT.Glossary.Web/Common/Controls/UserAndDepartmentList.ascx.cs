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
	public partial class UserAndDepartmentList : System.Web.UI.UserControl
	{
		protected string RootURL = string.Empty;
		public string ItemID = string.Empty;
		protected string Title = string.Empty;
		protected string UserID = string.Empty;
		
		public bool bGrp = true; /* 그룹리스트를 보여줄지 여부, 그룹관리 화면에서는 그룹리스트를 보여주지 않는다. */

        protected string viewType = "N";

        private bool _boolCheckSelf = false;
        public bool boolCheckSelf
        {
            get { return _boolCheckSelf; }
            set { _boolCheckSelf = value; }
        }


		public string AuthID
		{
			get
			{
				return hdUserItemID.Value;
			}
		}

		public string AuthName
		{
			get
			{
				return hdUserName.Value;
			}
		}

		public string AuthCL
		{
			get
			{
				return hdUserType.Value;
			}
		}

        public bool UserGroupVisible
        {
            set
            {
                bGrp = value;
            }
        }


        public string ViewType
        {
            set
            {
                viewType = value; 
            }
        } 
    

		protected void Page_Load(object sender, EventArgs e)
		{   
			RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
			ItemID = (Request["ItemID"] ?? string.Empty).ToString();
			Title = (Request["SearchKeyword"] ?? string.Empty).ToString();
			UserInfo u = new UserInfo(this.Page);
			UserID = u.UserID;

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
				ddlUserGroup.Items.Insert(0, new ListItem("그룹을 선택하세요.", "0"));

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					//스크립트 오류로 잠시 주석처리  

                    /*
                       Author : 개발자-김성환D, 리뷰자-진현빈G
                       Create Date : 2016.08.04 
                       Desc : 특수문자 " ' \ 처리
                   */
                    //ddlUserGroup.Items.Insert(i + 1, new ListItem("[그룹]" + ds.Tables[0].Rows[i]["MYGRP_NM"].ToString(), ds.Tables[0].Rows[i]["MYGRP_ID"].ToString()));
                    ddlUserGroup.Items.Insert(i + 1, new ListItem("[그룹]" + HttpUtility.HtmlDecode(ds.Tables[0].Rows[i]["MYGRP_NM"].ToString()), ds.Tables[0].Rows[i]["MYGRP_ID"].ToString()));
				}

				ddlUserGroup.SelectedIndex = 0;
			}
			else
			{
				ddlUserGroup.Items.Insert(0, new ListItem("사용자정의 그룹이 없습니다.", "0"));
			}

		}
	}
}