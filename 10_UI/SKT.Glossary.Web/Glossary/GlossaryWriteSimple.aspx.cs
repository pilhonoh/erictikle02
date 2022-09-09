using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;


namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryWriteSimple : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            // 끌.모임 설정
            string GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            string GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();


            Response.Redirect("/Glossary/GlossaryWriteNew.aspx?GatheringYN="+GatheringYN+"&GatheringID="+GatheringID, false);
        }

    }
}