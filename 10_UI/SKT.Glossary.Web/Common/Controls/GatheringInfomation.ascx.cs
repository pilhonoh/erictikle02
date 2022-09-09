using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class GatheringInfomation : System.Web.UI.UserControl
    {
        protected string UserID = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GI_GatheringYN;
        protected string GI_GatheringID;
        protected string GI_GatheringName;
        protected string GI_GatheringAuthor = string.Empty;
        protected string GI_GatheringAuthorDeptName = string.Empty;
        protected string GI_GatheringCreationDate = string.Empty;
        protected string GI_MenuType = string.Empty;

        protected string m_Glossary = string.Empty;
        protected string m_Directory = string.Empty;
        protected string m_Schedule = string.Empty;

        protected string AuthorYN = string.Empty;
        protected string AuthorID = string.Empty;

        protected GatheringPermission UDList;

        // 프로퍼티 생성
        private string gatheringYN;
        private string gatheringID;

        public string GatheringYN{get;set;}
        public string GatheringID{get;set;}

        protected void Page_Load(object sender, EventArgs e)
        {
            // 끌.모임 설정
            GI_GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GI_GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            // 프로퍼티 추가
            if (string.IsNullOrEmpty(GI_GatheringYN))
            {
                GI_GatheringYN = GatheringYN;
                GI_GatheringID = GatheringID;
            }

            GI_MenuType = (Request["MenuType"] ?? string.Empty).ToString();

            //UDList = this.uControl;

            UserInfo u = new UserInfo(this.Page);

            UserID = u.UserID;

            if (GI_GatheringYN == "Y")
            {
                dataBind("pub", u.UserID);
            }

            GI_MenuType = string.IsNullOrEmpty(GI_MenuType) ? "Glossary" : GI_MenuType;

            if (GI_MenuType == "Glossary")
            {
                m_Glossary = "class=\"on\"";
                m_Directory = "";
                m_Schedule = "";
            }
            else if (GI_MenuType == "Directory")
            {
                m_Glossary = "";
                m_Directory = "class=\"on\""; 
                m_Schedule = "";
            }
            else
            {
                m_Glossary = "";
                m_Directory = "";
                m_Schedule = "class=\"on\""; 
            }
        }

        protected void dataBind(string mode, string userid)
        {
            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();
            DataSet ds = biz.GlossaryGathering_List_Simple(mode, userid);
            DataTable dt = null;

            if (ds != null && ds.Tables.Count > 0)
            {
                //GI_GatheringDDL.Items.Clear();
                pop_GatheringList.Items.Clear();

                dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ddlUserFile.Items.Insert(i, new ListItem(dt.Rows[i]["DIR_NM"].ToString(), dt.Rows[i]["DIR_ID"].ToString())); 
                    //GI_GatheringDDL.Items.Insert(i, new ListItem(dt.Rows[i]["GatheringName"].ToString(), dt.Rows[i]["GatheringID"].ToString()));


                    pop_GatheringList.Items.Insert(i, new ListItem(SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(dt.Rows[i]["GatheringName"].ToString())), dt.Rows[i]["GatheringID"].ToString()));

                    if (GI_GatheringID == dt.Rows[i]["GatheringID"].ToString())
                    {
                        GI_GatheringAuthor = dt.Rows[i]["KOREANNAME"].ToString();
                        GI_GatheringAuthorDeptName = dt.Rows[i]["SOSOK"].ToString();
                        /*
                            Author : 개발자-김성환D, 리뷰자-진현빈G
                            Create Date : 2016.08.04 
                            Desc : 특수문자 " ' \ 처리
                        */
                        //GI_GatheringName = dt.Rows[i]["GatheringName"].ToString();
                        GI_GatheringName = SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(dt.Rows[i]["GatheringName"].ToString())).Trim();
                        GI_GatheringCreationDate = Convert.ToDateTime(dt.Rows[i]["CreationDate"]).ToShortDateString();
                        AuthorID = dt.Rows[i]["Author"].ToString();

                        if (userid == dt.Rows[i]["Author"].ToString())
                        {
                            AuthorYN = "Y";
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(GI_GatheringID) && GI_GatheringDDL.Items.Count > 0)
                if (!string.IsNullOrEmpty(GI_GatheringID) && pop_GatheringList.Items.Count > 0)
                {
                    //GI_GatheringDDL.SelectedValue = GI_GatheringID;
                    pop_GatheringList.SelectedValue = GI_GatheringID;
                }
                else
                {
                    //GI_GatheringDDL.SelectedIndex = 0;
                    pop_GatheringList.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 모임 변경 이벤트  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GI_GatheringDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strURL = "/Glossary/Glossary.aspx";
            //GI_GatheringID = GI_GatheringDDL.SelectedValue;

            //Response.Redirect(strURL + "?GatheringYN=Y" + "&GatheringID=" + GI_GatheringID);
        }
    }
}