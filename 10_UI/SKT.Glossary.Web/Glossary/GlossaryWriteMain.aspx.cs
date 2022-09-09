using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Collections;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryWriteMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryInfoBinding();
            }
        }

        protected void CategoryInfoBinding()
        {
            // 2014-06-23 Mr.No
            UserInfo u = new UserInfo(this.Page);
            GlossaryCategoryBiz biz = new GlossaryCategoryBiz();
            List<GlossaryCategoryType> glossaryCategoryType = biz.GlossaryCategory_Main_User_List((u.UserID).ToString());
            rptCategory.DataSource = glossaryCategoryType;
            rptCategory.DataBind();
        }
    }
}