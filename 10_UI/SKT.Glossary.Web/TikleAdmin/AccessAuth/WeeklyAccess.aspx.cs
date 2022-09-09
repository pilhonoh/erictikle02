using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SKT.Common;
using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Controls;
using System.Security.Cryptography;

namespace SKT.Glossary.Web.TikleAdmin.AccessAuth
{
    public partial class WeeklyAccess : System.Web.UI.Page
    {
        protected UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);
        }
    }
}