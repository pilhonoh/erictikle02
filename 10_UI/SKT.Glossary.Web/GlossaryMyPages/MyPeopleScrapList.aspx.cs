using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using System.Collections;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml;
using System.Web.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyPeopleScrapList : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;
        protected int iSuccessCount;
        protected int iUnSuccessCount;
        protected int iTotal;
        protected int iMyQnA;
        protected string UserName = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string SearchSort = string.Empty;
        protected string DisplaySearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string Mode = string.Empty;
        protected string qnaMode = "Search";

        protected JObject joPeople;

        protected void Page_Load(object sender, EventArgs e)
        {
            ////URL 타고 넘어오는거 구분
            //if (Session["WebcubeUseYN"] == null)
            //{
            //    Session.Add("WebcubereturnURL", Page.Request.Url.ToString());
            //    Response.Redirect("../Main.aspx");
            //}
            //else
            //{
            //    Session.Remove("WebcubereturnURL");
            //}


            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = "법인";
            //SearchSort = (Request["SearchSort"] ?? string.Empty).ToString();
            SearchSort = "score";
            //Mode = (Request["Mode"] ?? string.Empty).ToString();
            DisplaySearchKeyword = SearchKeyword;

            if (!IsPostBack)
            {
                pager.PageSize = 6;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;

                BindPeopleSelect();
            }
        }

        #region 담당자 검색
        // 담당자 검색 리스트
        private void BindPeopleSelect()
        {
            iTotalCount = 0;

            UserInfo u = new UserInfo(this.Page);
            UserName = u.Name;
            GlossaryMyPeopleScrapBiz biz = new GlossaryMyPeopleScrapBiz();
            DataSet dsMyPeopleScrap = biz.MyPeopleScrapList(pager.CurrentIndex, pager.PageSize, out iTotalCount, u.UserID);
            pager.ItemCount = iTotalCount;

            if (iTotalCount == 0)
            {
                btnListDelete.Visible = false;
            }

            rptPeople.DataSource = dsMyPeopleScrap.Tables[0];
            rptPeople.DataBind();
        }

        protected void rptPeople_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string HighlightKoreanName = string.Empty;
            string HighlightSosok = string.Empty;
            string HighlightJobDescription = string.Empty;
            string HighlightTelephoneNumber = string.Empty;
            string HighlightMobile = string.Empty;
            string HighlightMail = string.Empty;


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litKoreanName = (Literal)e.Item.FindControl("litKoreanName");
                //Literal litSosok = (Literal)e.Item.FindControl("litSosok");
                Literal litSosok2 = (Literal)e.Item.FindControl("litSosok2");
                Literal litJobDescription = (Literal)e.Item.FindControl("litJobDescription");
                Literal litTelephoneNumber = (Literal)e.Item.FindControl("litTelephoneNumber");
                Literal litMobile = (Literal)e.Item.FindControl("litMobile");
                Literal litMail = (Literal)e.Item.FindControl("litMail");
                Literal litPositionName = (Literal)e.Item.FindControl("litPositionName");


                DataRowView drv = (DataRowView)e.Item.DataItem;
                HighlightKoreanName = drv["KoreanName"].ToString();

                HighlightSosok = drv["Sosok"].ToString();

                HighlightJobDescription = drv["JobDescription01"].ToString();
                if (HighlightJobDescription.Length > 0 && drv["JobDescription02"].ToString().Length > 0) HighlightJobDescription += ", ";
                HighlightJobDescription += drv["JobDescription02"].ToString();
                if (HighlightJobDescription.Length > 0 && drv["JobDescription03"].ToString().Length > 0) HighlightJobDescription += ", ";
                HighlightJobDescription += drv["JobDescription03"].ToString();
              
                litPositionName.Text = drv["PositionName"].ToString();


                HighlightTelephoneNumber = drv["TelephoneNumber"].ToString();
                HighlightMobile = drv["Mobile"].ToString();
                HighlightMail = drv["Mail"].ToString();

                litKoreanName.Text = HighlightKoreanName;
                //litSosok.Text = HighlightSosok;
                litSosok2.Text = HighlightSosok;
                litJobDescription.Text = HighlightJobDescription;
                litTelephoneNumber.Text = HighlightTelephoneNumber;
                if (HighlightTelephoneNumber != null && HighlightTelephoneNumber.Length > 0 && HighlightMobile != null && HighlightMobile.Length > 0)
                {
                    litTelephoneNumber.Text = HighlightTelephoneNumber + ", ";
                }
                litMobile.Text = HighlightMobile;
                litMail.Text = HighlightMail;

                
            }

        }

        #endregion

        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            BindPeopleSelect();
        }

        protected void btnListDelete_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            string[] strCeckBox = Request.Params["checkJob"].Split(',');

            GlossaryMyPeopleScrapDac dac = new GlossaryMyPeopleScrapDac();

            for (int i = 0; i < strCeckBox.Length; i++)
            {
                dac.GlossaryMyPeopleScrapDelete(strCeckBox[i]);
            }

            Response.Redirect("/GlossaryMyPages/MyPeopleScrapList.aspx");

        }
    }
}