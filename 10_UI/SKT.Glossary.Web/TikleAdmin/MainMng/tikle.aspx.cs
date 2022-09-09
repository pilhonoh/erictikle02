using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.TikleAdmin.MainMng
{
	public partial class tikle : System.Web.UI.Page
	{
        string strGubun = string.Empty;
        public string strTitleName = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			ClientScript.GetPostBackEventReference(this, string.Empty);
            strGubun = (Request["Gubun"] ?? "HNN").ToString();
            if (strGubun == "HNN") strTitleName = "메인 Hot & New";
            if (strGubun == "DT") strTitleName = "메인 DT"; 

			if (!IsPostBack) {
				//rdoGubun.SelectedValue = strReqGubun;

				databind();
			}
		}

        //protected void rdoGubun_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    databind();
        //}

		protected void databind()
		{
			//string strGubun = string.Empty;
			//strGubun = rdoGubun.SelectedValue;

			DataSet ds = null;
			TikleAdadminBiz biz = new TikleAdadminBiz();
			List<MainNoticeType> itemList = new List<MainNoticeType>();

			ds = biz.TikleAdminMainNoticeSelect(strGubun);

            
            //else
            //{
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainNoticeType item = new MainNoticeType();

                item.NotID = Int64.Parse(dr["NotID"].ToString());
                item.Gubun = dr["Gubun"].ToString();
                item.Title = dr["Title"].ToString();
                item.Content = dr["Content"].ToString();
                item.URL = dr["URL"].ToString();
                item.SeqNo = Int32.Parse(dr["SeqNo"].ToString());
                item.Itemid = dr["ItemID"].ToString();
                item.UseYn = dr["UseYn"].ToString();

                itemList.Add(item);
            }
            //}

            //if (strGubun == "HN")
            //{
            rptGlossary.Visible = false;
            rptQA.Visible = false;
            rptHnN.Visible = true;


            for (int i = 0; i < 5 - ds.Tables[0].Rows.Count; i++)
            {
                MainNoticeType item = new MainNoticeType();
                itemList.Add(item);
            }

            rptHnN.DataSource = itemList;
            rptHnN.DataBind();
            //}
            //else if (strGubun == "QA")
            //{
            //    rptGlossary.Visible = false;
            //    rptHnN.Visible = false;
            //    rptQA.Visible = true;

            //    rptQA.DataSource = itemList;
            //    rptQA.DataBind();
            //}
            //else
            //{
            //    rptHnN.Visible = false;
            //    rptQA.Visible = false;
            //    rptGlossary.Visible = true;

            //    rptGlossary.DataSource = itemList;
            //    rptGlossary.DataBind();

            //    hdnListSeqNo.Value = itemList.Count.ToString();
            //}
		}

		protected void rptHnN_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MainNoticeType item = (MainNoticeType)e.Item.DataItem;

				TextBox txtHnNTitle = (TextBox)e.Item.FindControl("txtHnNTitle");
				//TextBox txtHnNContent = (TextBox)e.Item.FindControl("txtHnNContent");
				TextBox txtHnNURL = (TextBox)e.Item.FindControl("txtHnNURL");
				HiddenField hdnNotID = (HiddenField)e.Item.FindControl("hdnNotID");

				txtHnNTitle.Text = item.Title;
				//txtHnNContent.Text = item.Content;
				txtHnNURL.Text = item.URL;
				hdnNotID.Value = item.NotID.ToString();
			}
		}

        protected void rptQA_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MainNoticeType item = (MainNoticeType)e.Item.DataItem;

                TextBox txtQATitle = (TextBox)e.Item.FindControl("txtQATitle");
                TextBox txtQAContent = (TextBox)e.Item.FindControl("txtQAContent");
                TextBox txtQAURL = (TextBox)e.Item.FindControl("txtQAURL");
                HiddenField hdnQANotID = (HiddenField)e.Item.FindControl("hdnQANotID");

                txtQATitle.Text = item.Title;
                txtQAContent.Text = item.Content;
                txtQAURL.Text = item.URL;
                hdnQANotID.Value = item.NotID.ToString();
            }
        }

		protected void rptGlossary_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MainNoticeType item = (MainNoticeType)e.Item.DataItem;

				CheckBox chkNotID = (CheckBox)e.Item.FindControl("chkNotID");
				Literal litItemID = (Literal)e.Item.FindControl("litItemID");
				HyperLink lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
				//HiddenField hdnNotID = (HiddenField)e.Item.FindControl("hdnNotID");

				chkNotID.Text = "";
				chkNotID.Attributes.Add("Value", item.NotID.ToString());

				litItemID.Text = item.Itemid.ToString();

				lnkTitle.Text = item.Title;
				lnkTitle.NavigateUrl = "#";
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			MainNoticeType data = new MainNoticeType();
			UserInfo u = new UserInfo(this.Page);

			//string strGubun = string.Empty;

			//strGubun = rdoGubun.SelectedValue;

			// Hot & New일경우 별도 처리
            //if (strGubun == "HN")
            //{
            // 기존데이터 삭제
            biz.TikleAdminMainNoticeDelete(strGubun);

            bool chkVali = false;
            foreach (RepeaterItem item in rptHnN.Items)
            {
                
                string strHnNTitle = ((TextBox)item.FindControl("txtHnNTitle")).Text;
                //string strHnNContent = ((TextBox)item.FindControl("txtHnNContent")).Text;
                string strHnNURL = ((TextBox)item.FindControl("txtHnNURL")).Text;

                if (strHnNTitle.Length > 0 && strHnNURL.Length > 0)
                {
                    string strDomain = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/";

                    if (strHnNURL.ToLower().IndexOf("http://") > -1)
                        strDomain = "";
                    
                    var uri = new Uri(strDomain + strHnNURL);

                    var query = HttpUtility.ParseQueryString(uri.Query);
                    var iValue = query.Get("ItemID");

                    if (String.IsNullOrEmpty(iValue))
                    {
                        chkVali = true;
                        this.ClientScript.RegisterClientScriptBlock(GetType(), "Alert", "alert('잘못된 지식 URL 정보입니다.');history.back();", true);
                        return;
                    }
                    
                    data.Gubun = strGubun;
                    data.Title = strHnNTitle;
                    //data.Content = strHnNContent;
                    data.URL = strHnNURL;
                    data.Itemid = (iValue == null ? "" : iValue.Trim());
                    data.SeqNo = item.ItemIndex + 1;
                    data.NotID = 0;
                    data.UseYn = "Y";
                    data.UserID = u.UserID;

                    
                    biz.TikleAdminMainNoticeInsert(data);
                }
            }
            //}
            //else if (strGubun == "QA")
            //{
            //    // 기존데이터 삭제
            //    biz.TikleAdminMainNoticeDelete(strGubun);

            //    foreach (RepeaterItem item in rptQA.Items)
            //    {
            //        string strQATitle = ((TextBox)item.FindControl("txtQATitle")).Text;
            //        string strQAContent = ((TextBox)item.FindControl("txtQAContent")).Text;
            //        string strQAURL = ((TextBox)item.FindControl("txtQAURL")).Text;

            //        data.Gubun = strGubun;
            //        data.Title = strQATitle;
            //        data.Content = strQAContent;
            //        data.URL = strQAURL;
            //        data.SeqNo = item.ItemIndex + 1;
            //        data.NotID = 0;
            //        data.UseYn = "Y";
            //        data.UserID = u.UserID;

            //        biz.TikleAdminMainNoticeInsert(data);
            //    }
            //}
            //else
            //{
            //    data.Gubun = strGubun;
            //    data.Title = hdnItemTitle.Value;
            //    data.Content = "";
            //    data.URL = "";
            //    data.SeqNo = (Int32.Parse(hdnListSeqNo.Value)+1);
            //    data.NotID = 0;
            //    data.Itemid = hdnItemID.Value;
            //    data.UseYn = "Y";
            //    data.UserID = u.UserID;

            //    biz.TikleAdminMainNoticeInsert(data);
            //}

            if (!chkVali)
            {
                this.ClientScript.RegisterClientScriptBlock(GetType(), string.Empty, "alert('저장되었습니다.'); location.href='tikle.aspx?Gubun=" + strGubun + "'", true);
            }


			//databind();
			//Response.Redirect("tikle.aspx?Gubun=" + strGubun);
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			MainNoticeType data = new MainNoticeType();
			string strGubun = string.Empty;

			//strGubun = rdoGubun.SelectedValue;

			foreach (RepeaterItem item in rptGlossary.Items)
			{
				CheckBox chkNotID = (CheckBox)item.FindControl("chkNotID");

				if (chkNotID.Checked) {
					biz.TikleAdminMainNoticeDelete(strGubun, chkNotID.Attributes["Value"]);
				}
			}

			Response.Redirect("tikle.aspx?Gubun=" + strGubun);
		}
	}
}