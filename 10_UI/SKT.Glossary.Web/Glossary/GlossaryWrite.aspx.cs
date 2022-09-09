using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;


namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryWrite : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            string mode = (Request["mode"] ?? string.Empty).ToString();
            string ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            string CommonID = (Request["CommonID"] ?? string.Empty).ToString();
            string GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            string GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            Response.Redirect("/Glossary/GlossaryWriteNew.aspx?mode=" + mode + "&ItemID=" + ItemID + "&CommonID=" + CommonID + "&GatheringYN="+GatheringYN+"&GatheringID=", false);

        }
    }
}