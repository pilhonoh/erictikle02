using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Common;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services;
using SKT.Glossary.Biz;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class CommNateOnBizControl : System.Web.UI.UserControl
    {

        public Boolean targetBtnYn = true;
        public Boolean targetLIstYn = false;
        public string noteOnBizUserid = string.Empty;

        public Boolean targetBtnCheck
        {
            set
            {
                targetBtnYn = value;
            }
        }

        

        

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            noteOnBizUserid = u.UserID;
        }


       

    }
}
