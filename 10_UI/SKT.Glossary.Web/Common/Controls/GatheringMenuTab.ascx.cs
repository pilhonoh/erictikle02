using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class GatheringMenuTab : System.Web.UI.UserControl
    {
        protected string UserID = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)        
        protected string GI_GatheringID;
        protected string GI_GatheringName;

        protected string m_Glossary = string.Empty;
        protected string m_Directory = string.Empty;
        protected string m_Schedule = string.Empty;

        public string GatheringYN { get; set; }
        public string GatheringID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            GI_GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            // 프로퍼티 추가
            if (string.IsNullOrEmpty(GI_GatheringID))
            {
                GI_GatheringID = GatheringID;
            }

            string MenuType = (Request["MenuType"] ?? string.Empty).ToString();

            MenuType = string.IsNullOrEmpty(MenuType) ? "Glossary" : MenuType;

            if (MenuType == "Glossary")
            {
                m_Glossary = "class=\"on\"";
                m_Directory = "";
                m_Schedule = "";
            }
            else if (MenuType == "Directory")
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
    }
}