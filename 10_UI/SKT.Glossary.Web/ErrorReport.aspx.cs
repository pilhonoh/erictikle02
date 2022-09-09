using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;
using SKT.Glossary.Biz;
using System.Data;


namespace SKT.Glossary.Web
{
    public partial class ErrorReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlossaryBiz _biz = new GlossaryBiz();

                DataSet ds = _biz.GetSpecialUserChargeSelect("C");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblChargeName.Attributes.Add("style", "font-size:14px; font-weight:bold;");
                    lblChargeName.Text = ds.Tables[0].Rows[0]["DEPT_NAME"].ToString() + " " + ds.Tables[0].Rows[0]["USER_NAME"].ToString();
                }
            }
        }

        /// <summary>
        /// 2018-02-05 / 최현미 / 문의/오류신고 발송
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string IsTestServer = ConfigurationManager.AppSettings["IsTestServer"];
            GlossaryBiz _biz = new GlossaryBiz();

            CBHHelper _helper = new CBHHelper();

            UserInfo u = new UserInfo(this.Page);

            //제목
            //string subject = "[T.끌 문의/오류신고]" + StringCut(this.txtContent.Value);

            string str = this.txtContent.Value;
            str = Regex.Replace(str, @"<(.|\n)*?>", string.Empty);
            str = str.Replace("\r\n", " ");

            if(str.Length > 60)
                str = str.Substring(0, 60) + "...";

            string subject = "[T.끌 문의/오류신고]" + str;

            //내용
            string mailContent = "<span style=\"font-family;NanumGothic; font-size:10pt;\">";

            string contentText = this.txtContent.Value;
            contentText = SecurityHelper.Clear_XSS_CSRF(contentText);
            contentText = Utility.BREncode(contentText);

            mailContent += "<b>작성자</b> : " + u.Name + "/" + u.DeptName + "<br />";
            mailContent += "<b>작성일</b> : " + System.DateTime.Now.ToString() + "<br /><br />";
            mailContent += "<b>내용</b> : <br /><br />";
            mailContent += contentText;
            mailContent += "</span>";

            //string[] arrUser = hidUserList.Value.Split('/');
            DataSet ds = _biz.GetSpecialUserChargeSelect("M");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    string recieverMail = dr["EMAIL_ALIAS"].ToString();

                    if (IsTestServer.Equals("Y"))
                    {
                        recieverMail = ConfigurationManager.AppSettings["IsTestEmail"];
                    }
                    _helper.SendMail(u.EmailAddress, recieverMail, subject, mailContent);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Close", "EndMessage();", true);
            }
        }
    }
}

/*
메일 제목 : [시스템명 문의/오류신고] 작성 내용 20자(40byte)...

메일 내용 :

작성자 : 작성자 정보 
작성일 : 작성 일시 

내용 :
작성 내용*/
