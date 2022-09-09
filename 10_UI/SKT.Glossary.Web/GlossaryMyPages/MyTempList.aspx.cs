using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Collections;
using System.Configuration;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyTempList : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;

        //20131218 , F-003 count 표시 변경 요청
        protected int iWTikleCount;
        protected int iMTikleCount;
        protected int iTTikleCount;

        protected string DisplayATikleCount = string.Empty;
        protected string DisplayWTikleCount = string.Empty;
        protected string DisplayMTikleCount = string.Empty;
        protected string DisplayTTikleCount = string.Empty;

        protected string SearchKeyword = string.Empty;
        protected string ItemID = string.Empty;
        protected string UserName = string.Empty;
        protected string RootURL = string.Empty;
        protected string mode = string.Empty;
        protected string HistoryYN = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();


            if (!IsPostBack)
            {
                mode = (Request["mode"] ?? string.Empty).ToString();
                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;
            }
            BindSelect();

        }

        //리스트
        private void BindSelect()
        {
            iTotalCount = 0;

            //20131218 , F-003 count 표시 변경 요청
            iWTikleCount = 0;
            iMTikleCount = 0;
            iTTikleCount = 0;

            UserInfo u = new UserInfo(this.Page);
            UserName = u.Name;
            GlossaryTempBiz biz = new GlossaryTempBiz();
            ArrayList list = biz.GlossaryTempList(pager.CurrentIndex, pager.PageSize, out iTotalCount, out iWTikleCount, out iMTikleCount, out iTTikleCount, u.UserID);
            pager.ItemCount = iTotalCount;

            //20131218 , F-003 count 표시 변경 요청
            DisplayATikleCount = (iWTikleCount + iMTikleCount + iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount + iMTikleCount + iTTikleCount);
            DisplayWTikleCount = (iWTikleCount == 0) ? "0" : String.Format("{0:#,#}", iWTikleCount);
            DisplayMTikleCount = (iMTikleCount == 0) ? "0" : String.Format("{0:#,#}", iMTikleCount);
            DisplayTTikleCount = (iTTikleCount == 0) ? "0" : String.Format("{0:#,#}", iTTikleCount);

            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
        }

        //페이지
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            BindSelect();
        }

        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal WikiClass = (Literal)e.Item.FindControl("ltWiki");
                Literal Num = (Literal)e.Item.FindControl("Num");
                Literal itDelete = (Literal)e.Item.FindControl("itDelete");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }

                switch (((GlossaryTempType)e.Item.DataItem).Type)
                {
                    case "wiki":
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;

                    case "nateon":
                        WikiClass.Text = "<span class=\"nateon\">";
                        break;

                    case "email":
                        WikiClass.Text = "<span class=\"email\">";
                        break;
                    default:
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;
                }
                itDelete.Text = "<input type=\"checkbox\" id = \"Checkbox\"  value='" + ((GlossaryTempType)e.Item.DataItem).ID + "' name=\"checkJob\" onclick=\"event.cancelBubble = true;\">";

            }
        }

        protected void btnListDelete_Click(object sender, EventArgs e)
        {
            string[] strCeckBox = Request.Params["checkJob"].Split(',');

            GlossaryTempDac dac = new GlossaryTempDac();


            for (int i = 0; i < strCeckBox.Length; i++)
            {
                dac.GlossaryTempDelect(strCeckBox[i]);
            }

            Response.Redirect("/GlossaryMyPages/MyTempList.aspx");

        }
    }
}