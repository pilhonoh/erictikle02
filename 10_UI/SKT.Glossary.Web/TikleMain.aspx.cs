using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Collections;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;

namespace SKT.Glossary.Web
{
    public class AjaxActivityInfoNew
    {
        public string user;
        public string user_id;
        public string action;
        public string ItemState;
        public string RowNum;
        public string Title;
        public string CommonID;
        public string CreateTime;
    }


    public class BannerInfo
    {
        public string BannerCss { get; set; }
        public string Banner1Title { get; set; }
        public string Banner1imgFile { get; set; }
        public string Banner1Link { get; set; }
        public string Banner2Title { get; set; }
        public string Banner2imgFile { get; set; }
        public string Banner2Link { get; set; }
        public string Banner3Title { get; set; }
        public string Banner3imgFile { get; set; }
        public string Banner3Link { get; set; }
        public string Banner4Title { get; set; }
        public string Banner4imgFile { get; set; }
        public string Banner4Link { get; set; }
    }

    public partial class Main : System.Web.UI.Page
    {
        protected string RootURL = string.Empty;
        protected string DeptCode = string.Empty;
        protected string FirstTime = string.Empty;
        protected string UserID = string.Empty;
        protected string ViewLevel = string.Empty;
        protected UserInfo u;
        public BannerInfo banner;

        protected string selectedCategoryId;
        protected string SearchKeyword = string.Empty;

        protected string SCHEDULE_YYYYMM = string.Empty;
        protected string SCHEDULE_DAY = string.Empty;
        protected string SCHEDULE_WEEKNAME = string.Empty;

        //손님모드
        protected string GuestMode = "N";

        //위클리 데모용
        protected string WeeklyFlag = string.Empty;

        //메인화면 팝업 여부
        protected string popupFlag = "N";
        
        // Mr.No 2015-06-23
        // 15,19 사번 중에 접속 가능한 사번 체크
        protected string ExceptionUserID = string.Empty;

        // Mr.Kim 2015-10-23 임원 체크
        protected string isOfficer = "N";

        // Mr.Kim 2015-10-21 마케팅 접속 가능자 체크
        protected string isMarketer = "N";

        //먼슬리 사용권한
        protected string isMonthlyAuthority = "N";

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);
            UserID = u.UserID;
            ViewLevel = u.ViewLevel;
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            SearchKeyword = SearchKeyword.Replace("'", "\\\'").Replace("\"", "\\\"");

            if (u.IsGlossaryPermission == false && u.IsDirectoryPermission == false)
            {
                string str = "이화면을 보신분은 접속가능한 사용자가 아닙니다.";
                base.Response.Redirect("/Error.aspx?InfoMessage=" + str, false);
                Response.End();
            }

            if (!IsPostBack)
            {
                Select();
            }
        }

        /// <summary>
        /// 최초실행
        /// </summary>
        public void Select()
        {
            // 공지사항, 배너, 일정정보, 신입사원정보 설정
            BasicInfoBind();

            MainNotice();
        }
        
        public void MainNotice()
        {
            DataSet ds = new DataSet();
            
            GlossaryMainBiz biz = new GlossaryMainBiz();
            ds = biz.GlossaryMainNotice(u.UserID);

            rptHN.DataSource = ds.Tables[0];
            rptHN.DataBind();
          
            rptDT.DataSource = ds.Tables[1];
            rptDT.DataBind();
        }

        protected void rptHN_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }

        }

        protected void rptDT_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }

        }

        public void BasicInfoBind()
        {
            DataSet ds = new DataSet();
            GlossaryMainBiz biz = new GlossaryMainBiz();
            ds = biz.BasicInfoSelect(u.UserID);

            // 마케팅 유저인지 구분
            isMarketer = Convert.ToInt32(biz.MarketingUserCheck(u.UserID).Tables[0].Rows[0]["CHECKCOUNT"].ToString()) > 0 ? "Y" : "N";
            
            // 임원인지 체크
            isOfficer = Convert.ToInt32(biz.OfficerCheck(u.UserID).Tables[0].Rows[0]["CHECKCOUNT"].ToString()) > 0 ? "Y" : "N";

            //손님모드
            GuestMode = ds.Tables[5].Rows[0]["Flag"].ToString().Trim();

            //팝업
            //if (isMonthlyAuthority == "Y") { 
                popupFlag = Convert.ToInt32(ds.Tables[6].Rows[0]["popupcount"]) > 0 ? "Y" : "N";
            //}
            //else
            //{
            //    popupFlag = "Y";
            // }
            //2015-11-09 메인 배너 중지
            //popupFlag = "Y";

            // 좌측 공지사항
            //2015-09-08 KSH platform으로 인한 hot & new repeater 주석
            //rptNotice.DataSource = ds.Tables[0];
            //rptNotice.DataBind();

            //// 좌측 공지사항
            //banner = new BannerInfo();
            //string BANNER_DOWNLOAD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["BaseURL"]
            //+ System.Configuration.ConfigurationManager.AppSettings["BANNER_DOWNLOAD_FILE_PATH"];

            //foreach (DataRow dr in ds.Tables[1].Rows)
            //{
            //    banner.BannerCss = dr["BannerCss"].ToString();

            //    if (dr["SeqNo"].ToString() == "1")
            //    {
            //        banner.Banner1Title = dr["Title"].ToString();
            //        banner.Banner1imgFile = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();
            //        banner.Banner1Link = dr["URL"].ToString();
            //    }
            //    else if (dr["SeqNo"].ToString() == "2")
            //    {
            //        banner.Banner2Title = dr["Title"].ToString();
            //        banner.Banner2imgFile = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();
            //        banner.Banner2Link = dr["URL"].ToString();
            //    }
            //    else if (dr["SeqNo"].ToString() == "3")
            //    {
            //        banner.Banner3Title = dr["Title"].ToString();
            //        banner.Banner3imgFile = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();
            //        banner.Banner3Link = dr["URL"].ToString();
            //    }
            //    else if (dr["SeqNo"].ToString() == "4")
            //    {
            //        banner.Banner4Title = dr["Title"].ToString();
            //        banner.Banner4imgFile = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();
            //        banner.Banner4Link = dr["URL"].ToString();
            //    }

            //}

            ////rptBanner.DataSource = ds.Tables[1];
            ////rptBanner.DataBind();

            //// 일정정보
            //SCHEDULE_YYYYMM = ds.Tables[2].Rows[0]["SCHEDULE_YYYYMM"].ToString();
            //SCHEDULE_DAY = ds.Tables[2].Rows[0]["SCHEDULE_DAY"].ToString();
            //SCHEDULE_WEEKNAME = ds.Tables[2].Rows[0]["SCHEDULE_WEEKNAME"].ToString();
            
            //rptSchedule.DataSource = ds.Tables[3];
            //rptSchedule.DataBind();


            // 신인사원
            // 2016.09.29 불필요한 Request제거 김성환D
            //rptEmp.DataSource = ds.Tables[4];
            //rptEmp.DataBind();
        }

        //2015-09-08 KSH platform으로 인한 hot & new repeater 주석
        //protected void rptNotice_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{rptNotice
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {

        //        Literal litTitle = (Literal)e.Item.FindControl("litTitle");
        //        Literal litContent = (Literal)e.Item.FindControl("litContent");
        //        HyperLink lnkNotice = (HyperLink)e.Item.FindControl("lnkNotice");

        //        DataRowView drv = (DataRowView)e.Item.DataItem;
        //        lnkNotice.NavigateUrl = drv["Url"].ToString();
        //        litTitle.Text = drv["Title"].ToString();
        //        litContent.Text = drv["Content"].ToString();
        //    }
        //}

       

        //protected void rptSchedule_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{
        //    if (rptSchedule.Items.Count < 1)
        //    {
        //        if (e.Item.ItemType == ListItemType.Footer)
        //        {
        //            Literal lblEmptyData = (Literal)e.Item.FindControl("lblEmptyData");
        //            lblEmptyData.Visible = true;
        //        }
        //    }

        //}

        #region rptEmp_OnItemDataBound 
        //protected void rptEmp_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{
        //    string HighlightJobDescription = string.Empty;
        //    string HighlightTelephoneNumber = string.Empty;
        //    string HighlightMobile = string.Empty;
        //    string HighlightMail = string.Empty;
        //    string HighlightMAINCOMMENT = string.Empty;

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        Literal litJobDescription = (Literal)e.Item.FindControl("litJobDescription");
        //        //Literal litTelephoneNumber = (Literal)e.Item.FindControl("litTelephoneNumber");
        //        Literal litMobile = (Literal)e.Item.FindControl("litMobile");
        //        Literal litMail = (Literal)e.Item.FindControl("litMail");


        //        Literal litMAINCOMMENT = (Literal)e.Item.FindControl("litMAINCOMMENT");

        //        DataRowView drv = (DataRowView)e.Item.DataItem;

        //        if (drv["JobDescription01"] != null)
        //        {
        //            HighlightJobDescription += drv["JobDescription01"].ToString();
        //        }

        //        if (drv["JobDescription02"] != null)
        //        {
        //            if (HighlightJobDescription.Length > 0 && drv["JobDescription02"].ToString().Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += drv["JobDescription02"].ToString();
        //        }

        //        if (drv["JobDescription03"] != null)
        //        {
        //            if (HighlightJobDescription.Length > 0 && drv["JobDescription03"].ToString().Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += drv["JobDescription03"].ToString();
        //        }

        //        if (drv["TelephoneNumber"] != null)
        //        {
        //            HighlightTelephoneNumber = drv["TelephoneNumber"].ToString();
        //        }

        //        if (drv["Mobile"] != null)
        //        {
        //            HighlightMobile = drv["Mobile"].ToString();
        //        }


        //        if (drv["MAINCOMMENT"] != null)
        //        {
        //            HighlightMAINCOMMENT = drv["MAINCOMMENT"].ToString();
        //        }

        //        HighlightMail = drv["Mail"].ToString();

        //        /*
        //        litTelephoneNumber.Text = HighlightTelephoneNumber;
                
        //        if (HighlightTelephoneNumber != null && HighlightTelephoneNumber.Length > 0 && HighlightMobile != null && HighlightMobile.Length > 0)
        //        {
        //            litTelephoneNumber.Text = HighlightTelephoneNumber + ", ";
        //        }
        //        */
        //        //litMobile.Text = HighlightMobile;
        //       // litMail.Text = HighlightMail;
        //        litMAINCOMMENT.Text = HighlightMAINCOMMENT;
        //    }
        //}
        #endregion

        /// <summary>
		/// 이벤트 현황 조회
		/// </summary>
		/// <param name="Summary"></param>
		/// <param name="maxlengh"></param>
		/// <returns></returns>
		[WebMethod]
		public static Dictionary<string,object> GetPromotionEventSelect(string UserID, string EVT_Type)
		{
			DataSet ds;
			DataTable dt;

			PromotionEventBiz biz = new PromotionEventBiz();
			ds = biz.PromotionEventSelect(UserID, EVT_Type);

			return Utility.ToJson(ds.Tables[0]);
		}

		[WebMethod]
		public static Dictionary<string, object> PromotionEvent_ProfileUpdate(string UserID, string EVT_Type)
		{
			DataSet ds;
			DataTable dt;

			PromotionEventBiz biz = new PromotionEventBiz();
			ds = biz.PromotionEvent_ProfileUpdate(UserID, EVT_Type);

			return Utility.ToJson(ds.Tables[0]);
		}

        [WebMethod]
        public static Dictionary<string, object> PopupInsert(string UserID, string pop_Type)
        {
            DataSet ds;

            GlossaryMainBiz biz = new GlossaryMainBiz();
            ds = biz.PopupInsert(UserID, pop_Type);

            return Utility.ToJson(ds.Tables[0]);
        }

        
        /// <summary>
        /// 글자 수 제한
        /// </summary>
        /// <param name="Summary"></param>
        /// <param name="maxlengh"></param>
        /// <returns></returns>
        public static string CutSummary(string Summary, int maxlengh = 40)
        {
            if (Summary.Length > maxlengh)
            {
                return Summary.Substring(0, maxlengh - 3) + "...";
            }
            else
            {
                return Summary;
            }
        }
    }
}