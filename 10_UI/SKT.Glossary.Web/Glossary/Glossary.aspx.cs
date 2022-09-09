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
using SKT.Glossary.Web.Common.Controls;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Glossary
{
    public partial class Glossary : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;
        protected int iSuccessCount;
        protected int iUnSuccessCount;
        protected int iTotal;
        protected int iMyQnA;
        protected string SearchKeyword = string.Empty;
        protected string SearchSort = string.Empty;
        protected string DisplaySearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string Mode = string.Empty;
        protected string qnaMode = "Search";

        protected string UserID = string.Empty;
        protected string DisplayCareer = string.Empty;
        //public bool onluNumUser = false;

        protected JObject joPeople;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;
        protected string GatheringName;
        protected string GatheringAuthor = string.Empty;
        protected string GatheringCreationDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            SearchSort = (Request["SearchSort"] ?? string.Empty).ToString();
            Mode = (Request["Mode"] ?? string.Empty).ToString();
            DisplaySearchKeyword = SearchKeyword;

            UserInfo u = new UserInfo(this.Page);

            // CHG610000076956 / 20181206 / 끌지식권한체크
            if (u.IsGlossaryPermission == false)
            {
                //권한 없음 경고 및 페이지 이동
                new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
                Response.End();
            }
            //onluNumUser = u.isIdOnlyNum;

            UserID = u.UserID;

            // 끌.모임 설정
            //GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            //GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            if (!IsPostBack)
            {
                //BindPeopleSelect();
            }
        }

        #region 담당자 검색
        // 담당자 검색 리스트
        //private void BindPeopleSelect()
        //{
        //    UserInfo u = new UserInfo(this);
        //}

        //protected void rptPeople_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{
        //    string HighlightKoreanName = string.Empty;
        //    string HighlightSosok = string.Empty;
        //    string HighlightJobDescription = string.Empty;
        //    string HighlightTelephoneNumber = string.Empty;
        //    string HighlightMobile = string.Empty;
        //    string HighlightMail = string.Empty;
        //    string HighlightPOSITION_NAME = string.Empty;

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        GlossarySearchType.SearchPeopleDocs data = ((GlossarySearchType.SearchPeopleDocs)e.Item.DataItem);

        //        Literal litKoreanName = (Literal)e.Item.FindControl("litKoreanName");
        //        Literal litSosok = (Literal)e.Item.FindControl("litSosok");
        //        Literal litJobDescription = (Literal)e.Item.FindControl("litJobDescription");
        //        Literal litTelephoneNumber = (Literal)e.Item.FindControl("litTelephoneNumber");
        //        Literal litMobile = (Literal)e.Item.FindControl("litMobile");
        //        Literal litMail = (Literal)e.Item.FindControl("litMail");
        //        Literal litPOSITION_NAME = (Literal)e.Item.FindControl("litPOSITION_NAME");

        //        GlossarySearchType.SearchPeopleHlightingItem highlightingItem = JsonConvert.DeserializeObject<GlossarySearchType.SearchPeopleHlightingItem>(joPeople["highlighting"][data.EmployeeID].ToString());

        //        if (highlightingItem.KoreanName != null)
        //        {
        //            HighlightKoreanName = highlightingItem.KoreanName[0].ToString();
        //        }
        //        else
        //        {
        //            HighlightKoreanName = data.KoreanName;
        //        }

        //        if (highlightingItem.Sosok != null)
        //        {
        //            HighlightSosok = highlightingItem.Sosok[0].ToString();
        //        }
        //        else
        //        {
        //            HighlightSosok = data.Sosok;
        //        }

        //        if (highlightingItem.JobDescription01 != null)
        //        {
        //            HighlightJobDescription += highlightingItem.JobDescription01[0].ToString();
        //        }
        //        else
        //        {
        //            HighlightJobDescription += data.JobDescription01;
        //        }

        //        if (highlightingItem.JobDescription02 != null)
        //        {
        //            if (HighlightJobDescription.Length > 0 && highlightingItem.JobDescription02[0].ToString().Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += highlightingItem.JobDescription02[0].ToString();
        //        }
        //        else
        //        {
        //            if (HighlightJobDescription.Length > 0 && data.JobDescription02.Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += data.JobDescription02;
        //        }

        //        if (highlightingItem.JobDescription03 != null)
        //        {
        //            if (HighlightJobDescription.Length > 0 && highlightingItem.JobDescription03[0].ToString().Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += highlightingItem.JobDescription03[0].ToString();
        //        }
        //        else
        //        {
        //            if (HighlightJobDescription.Length > 0 && data.JobDescription03.Length > 0) HighlightJobDescription += ", ";
        //            HighlightJobDescription += data.JobDescription03;
        //        }

        //        if (highlightingItem.TelephoneNumber != null)
        //        {
        //            HighlightTelephoneNumber = highlightingItem.TelephoneNumber[0].ToString();
        //        }
        //        else
        //        {
        //            HighlightTelephoneNumber = data.TelephoneNumber;
        //        }

        //        if (highlightingItem.Mobile != null)
        //        {
        //            HighlightMobile = highlightingItem.Mobile[0].ToString();
        //        }
        //        else
        //        {
        //            HighlightMobile = data.Mobile;
        //        }

        //        HighlightMail = data.Mail;
        //        HighlightPOSITION_NAME = ((data.POSITION_NAME == null || data.POSITION_NAME == "") ? "Manager" : data.POSITION_NAME);

        //        litKoreanName.Text = HighlightKoreanName;
        //        litSosok.Text = HighlightSosok;
        //        litJobDescription.Text = HighlightJobDescription;
        //        litTelephoneNumber.Text = HighlightTelephoneNumber;
        //        if (HighlightTelephoneNumber != null && HighlightTelephoneNumber.Length > 0 && HighlightMobile != null && HighlightMobile.Length > 0)
        //        {
        //            litTelephoneNumber.Text = HighlightTelephoneNumber + ", ";
        //        }
        //        litMobile.Text = HighlightMobile;
        //        litMail.Text = HighlightMail;
        //        litPOSITION_NAME.Text = HighlightPOSITION_NAME;
        //    }

        //}

        #endregion

        [WebMethod]
        public static Dictionary<string, object> GetGlossaryMainInfoSelect(string UserID, string GatheringYN, string GatheringID)
        {
            DataSet ds = new DataSet();
            GlossaryBiz biz = new GlossaryBiz();
            return Utility.ToJson(new GlossaryBiz().GlossaryMainInfoSelect(UserID));

            return Utility.ToJson(ds);
        }

        [WebMethod]
        public static Dictionary<string, object> GetGlossaryMainTagBoardSelect(string Board_Index, string Board_Count, string Board_RowCount, string UserID)
        {
            DataSet ds = new DataSet();
            GlossaryBiz biz = new GlossaryBiz();
            ds = biz.GetGlossaryMainTagBoardSelect(Board_Index, Board_Count, Board_RowCount, UserID);

            return Utility.ToJson(ds);
        }

        [WebMethod]
        public static Dictionary<string, object> GetGlossaryMainTagSelect(string Tag_Index, string Tag_Count, string GatheringYN, string GatheringID, string UserID)
        {
            DataSet ds = new DataSet();
            GlossaryBiz biz = new GlossaryBiz();
            ds = biz.GetGlossaryMainTagSelect(Tag_Index, Tag_Count, GatheringYN, GatheringID, UserID);

            return Utility.ToJson(ds);
        }


    }
}