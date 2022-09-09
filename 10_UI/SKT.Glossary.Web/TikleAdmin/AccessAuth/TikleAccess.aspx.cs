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

namespace SKT.Glossary.Web.TikleAdmin.AccessAuth
{
    public partial class TikleAccess : System.Web.UI.Page
    {
        protected UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);

            if (!u.isAdmin)
            {
                string infomsg = "이화면을 보신분은 접속가능한 사용자가 아닙니다.";
                Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
            }
            if (!IsPostBack)
            {
                GlossaryAdminBiz biz = new GlossaryAdminBiz();
                DataSet ds = biz.GlossaryAdminUserList(0, 0, 0, "", 0, 0);
                rptmember.DataSource = ds.Tables[0];
                rptmember.DataBind();
            }
        }
    }
}