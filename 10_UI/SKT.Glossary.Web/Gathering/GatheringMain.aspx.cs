using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Web.Services;
using System.Data;
using System.ServiceModel;

using SKT.Glossary.Biz;
using SKT.Glossary.Type;

using SKT.Common;
using SKT.Common.TikleDocManagerService;
using System.ServiceModel.Channels;
using SKT.Glossary.Web.Directory;
using SKT.Glossary.Web.Common.Controls;

namespace SKT.Glossary.Web.Gathering
{
    public partial class GatheringMain : System.Web.UI.Page
    {
        protected string GatheringYN;
        protected string GatheringID;
        protected string SearchSort;
        protected string UserID = string.Empty;

        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        protected string DivID = string.Empty;
        protected string DivType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();
            SearchSort = (Request["SearchSort"] ?? string.Empty).ToString();
            DivID = (Request["DivID"] ?? string.Empty).ToString();
            DivType = (Request["DivType"] ?? string.Empty).ToString();

            //메뉴제거
            if (GatheringYN.Equals("Y") && GatheringID.Equals("221"))
            {
            }
            else
            {
                Response.Redirect("/TikleMain.aspx");
                Response.End();
            }

            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

            DivType = string.IsNullOrEmpty(DivType) ? "Pub" : DivType;

            if (DivType == "Pri")
            {
                m_pub = "";
                m_vis = "";
                m_pri = "class=\"on\"";
            }
            else if (DivType == "Vis")
            {
                m_pub = "";
                m_vis = "class=\"on\"";
                m_pri = "";
            }
            else
            {
                m_pub = "class=\"on\"";
                m_vis = "";
                m_pri = "";
            }

            // 권한처리(모임멤버검사)
            if (GatheringYN == "Y")
            {
                GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
                DataSet ds = gBiz.GlossaryGathering_MemberList(GatheringID);

                bool CheckResult = false;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (u.UserID.ToUpper().Equals(dr["EMPNO"].ToString().ToUpper()))
                        {
                            CheckResult = true;
                        }
                    }
                }

                if (!CheckResult)
                {
                    Response.Redirect("../Error.aspx?ErrCode=100&Message=" + "이 페이지는 모임 멤버 분들만 보실 수 있습니다 ^^;");
                }
            }

        }
    }
}