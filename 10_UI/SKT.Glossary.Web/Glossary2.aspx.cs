using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using System.Configuration;

namespace SKT.Glossary.Web
{
    public partial class Glossary2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Web.config BackUser 일때만 보이도록
            if (!CheckBackUser())
            {
                string infomsg = "해당 페이지를 볼 수 없습니다.";
                Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
            }
        }

        private bool CheckBackUser()
        {
            bool chkReturn = false;
            string BackUserID = ConfigurationManager.AppSettings["BackUser"].ToLower();

            if (BackUserID.Equals("*"))
            {
                chkReturn = true;
            }
            else
            {
                String UserID = System.Web.HttpContext.Current.Request.Headers["SM_USER"].ToLower();
                if (String.IsNullOrEmpty(UserID))
                {
                    if (System.Web.HttpContext.Current.Request.Cookies["SM_USER"] != null)
                    {
                        UserID = System.Web.HttpContext.Current.Request.Cookies["SM_USER"].Value;
                    }
                }

                if (BackUserID.IndexOf(',') > -1)
                {
                    string[] arrBackUserID = BackUserID.Split(',');
                    for (int i = 0; i < arrBackUserID.Length; i++)
                    {
                        if (arrBackUserID[i].Equals(UserID.ToLower()))
                        {
                            chkReturn = true;
                        }
                    }
                }
                else
                {
                    if (BackUserID.Equals(UserID.ToLower()))
                    {
                        chkReturn = true;
                    }
                }
            }
            return chkReturn;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            u.UserID = this.txtID.Text;

            Response.Redirect("/");
        }

        //protected void btnQueue_Click(object sender, EventArgs e)
        //{
        //    //메세지큐 서비스를 멈춘다.
        //    SKT.Common.CBHMSMQHelper aa = new CBHMSMQHelper();
        //    aa.DeleteQueue();
        //}
    }

}