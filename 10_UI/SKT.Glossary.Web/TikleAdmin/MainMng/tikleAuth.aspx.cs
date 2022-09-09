using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using SKT.Common;
using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Controls;
using System.Security.Cryptography;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.TikleAdmin.MainMng
{
    public partial class tikleAuth : System.Web.UI.Page
    {
        protected UserInfo u;
        protected int currentPageIndx = 1;
        protected int iTotalCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtEmpNo.Attributes.Add("readonly", "readonly");
            txtEmpNm.Attributes.Add("readonly", "readonly");

            u = new UserInfo(this.Page);

            if (!u.isAdmin)
            {
                string infomsg = "이화면을 보신분은 접속가능한 사용자가 아닙니다.";
                Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
            }
            if (!IsPostBack)
            {
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;

                pager.PageSize = 10;

                GetList();
            }
        }

        private void GetList()
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();
            DataSet ds = biz.GlossaryAdminExceptUserList(pager.CurrentIndex, pager.PageSize, "");
            rptmember.DataSource = ds.Tables[0];
            rptmember.DataBind();

            pager.ItemCount = 0;
            if (ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows.Count > 0)
                {
                    pager.ItemCount = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalCount"].ToString());
                }
            }
            
        }

        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;

            GetList();

        }

        protected void rptmember_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            Literal UserAuth = (Literal)e.Item.FindControl("litUserAuth");

            string strUserAuth = string.Empty;
            string strDefaultAuth = string.Empty;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;

                string[] arr = drv.Row["LEVEL"].ToString().Split(',');
                string match = string.Empty;

                //match = Array.Find(arr, n => n.Contains("1"));
                //if (!string.IsNullOrEmpty(match))
                //    strUserAuth += "티끌이,";

                //match = Array.Find(arr, n => n.Equals("2"));
                //if (!string.IsNullOrEmpty(match))
                //    strUserAuth += "관리자,";

                match = Array.Find(arr, n => n.Equals("G"));
                if (!string.IsNullOrEmpty(match))
                    strUserAuth += "끌지식(T생활백서/DT스토리(IoT))<br />";

                match = Array.Find(arr, n => n.Equals("DT"));
                if (!string.IsNullOrEmpty(match))
                    strUserAuth += "DT스토리(DT센터)<br />";

                match = Array.Find(arr, n => n.Equals("D"));
                if (!string.IsNullOrEmpty(match))
                    strUserAuth += "끌문서";

                //UserAuth.Text = strUserAuth.Substring(0, strUserAuth.Length -1);
                UserAuth.Text = strUserAuth;

                
            }
        }

        protected void hidSave_Click(object sender, EventArgs e)
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();

            string strLevel = string.Empty;

            if (cbAuth1.Checked)
                strLevel += "G,";
            if (cbAuth2.Checked)
                strLevel += "DT,";
            if (cbAuth3.Checked)
                strLevel += "D,";

            strLevel = strLevel.Substring(0, strLevel.Length - 1);

            DataSet ds = biz.GlossaryAdminExceptInsert(this.txtEmpNo.Text.ToUpper(), strLevel, this.u.UserID);

            if(ds != null)
            {
                if(ds.Tables[0].Rows[0]["MSG"].ToString().Equals("OK"))
                {
                    GetList();
                }
                else if (ds.Tables[0].Rows[0]["MSG"].ToString().Equals("NOTUSER"))
                {
                    Response.Write("<script language=\"javascript\">alert('구성원 정보가 없습니다.');history.back(-1);</script>");
                }
                else if(ds.Tables[0].Rows[0]["MSG"].ToString().Equals("ADMIN"))
                {
                    Response.Write("<script language=\"javascript\">alert('관리자 계정입니다.');history.back(-1);</script>");
                }
                else if (ds.Tables[0].Rows[0]["MSG"].ToString().Equals("EXISTS"))
                {
                    Response.Write("<script language=\"javascript\">alert('등록된 구성원입니다. 삭제 후 진행바랍니다.');history.back(-1);</script>");
                }
            }
            
        }

        protected void hidDelete_Click(object sender, EventArgs e)
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();
            biz.GlossaryAdminExceptDelete(this.hidDeleteEmpNo.Value);
            GetList();
        }

        protected void hidSearch_Click(object sender, EventArgs e)
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();
            DataSet ds = biz.GlossaryAdminExceptUserList(pager.CurrentIndex, pager.PageSize, this.txtSchText.Text);
            rptmember.DataSource = ds.Tables[0];
            rptmember.DataBind();

            pager.ItemCount = 0;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pager.ItemCount = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalCount"].ToString());
                }
            }
        }
    }
}