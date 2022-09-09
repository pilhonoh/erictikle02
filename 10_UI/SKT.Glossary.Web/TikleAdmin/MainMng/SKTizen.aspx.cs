using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;

namespace SKT.Glossary.Web.TikleAdmin.MainMng
{
	public partial class SKTizen : System.Web.UI.Page
	{
		string strGubun = "SKTIZEN";

		protected void Page_Load(object sender, EventArgs e)
		{
			ClientScript.GetPostBackEventReference(this, string.Empty);

			if (!IsPostBack)
			{
				databind();
			}
		}

		protected void databind()
		{	
			DataSet ds = null;
			TikleAdadminBiz biz = new TikleAdadminBiz();
			List<MainNoticeType> itemList = new List<MainNoticeType>();

			ds = biz.TikleAdminMainNoticeSelect(strGubun);

			
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

			rptGlossary.DataSource = itemList;
			rptGlossary.DataBind();

			hdnListSeqNo.Value = itemList.Count.ToString();
		}

		protected void rptGlossary_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MainNoticeType item = (MainNoticeType)e.Item.DataItem;


				Literal litRptNo = (Literal)e.Item.FindControl("litRptNo");
				CheckBox chkNotID = (CheckBox)e.Item.FindControl("chkRptNotID");
				Literal litRptTitle = (Literal)e.Item.FindControl("litRptTitle");
				TextBox txtContent = (TextBox)e.Item.FindControl("txtRptContent");
				HiddenField hdnRptSeqno = (HiddenField)e.Item.FindControl("hdnRptSeqno");
				HiddenField hdnRptItemID = (HiddenField)e.Item.FindControl("hdnRptItemID");

				litRptNo.Text = (e.Item.ItemIndex+1).ToString();

				chkNotID.Text = "";
				chkNotID.Attributes.Add("Value", item.NotID.ToString());

				litRptTitle.Text = item.Title.ToString() + "(" + item.Itemid.ToString() +")";

				txtContent.Text = item.Content.ToString();

				hdnRptSeqno.Value = item.SeqNo.ToString();
				hdnRptItemID.Value = item.Itemid.ToString();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			MainNoticeType data = new MainNoticeType();
			UserInfo u = new UserInfo(this.Page);

			foreach (RepeaterItem item in rptGlossary.Items)
			{
				CheckBox chkNotID = (CheckBox)item.FindControl("chkRptNotID");
				TextBox txtContent = (TextBox)item.FindControl("txtRptContent");
				Literal litRptTitle = (Literal)item.FindControl("litRptTitle");
				HiddenField hdnRptSeqno = (HiddenField)item.FindControl("hdnRptSeqno");
				HiddenField hdnRptItemID = (HiddenField)item.FindControl("hdnRptItemID");

				data.Gubun = strGubun;
                data.Title = litRptTitle.Text.Substring(0,litRptTitle.Text.IndexOf('('));
				data.Content = txtContent.Text;
				data.URL = "";
				data.SeqNo = Int32.Parse(hdnRptSeqno.Value);
				data.NotID = Int64.Parse(chkNotID.Attributes["Value"]);
				data.Itemid = hdnRptItemID.Value;
				data.UseYn = "Y";
				data.UserID = u.UserID;

				biz.TikleAdminMainNoticeInsert(data);
			}

			//this.ClientScript.RegisterClientScriptBlock(GetType(), string.Empty, "alert('저장되었습니다.');", true);

			//databind();
			Response.Redirect("SKTizen.aspx");
		}

		protected void btnAddUser_Click(object sender, EventArgs e)
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			MainNoticeType data = new MainNoticeType();
			UserInfo u = new UserInfo(this.Page);


			data.Gubun = strGubun;
			data.Title = hdnItemTitle.Value;
			data.Content = "";
			data.URL = "";
			data.SeqNo = (Int32.Parse(hdnListSeqNo.Value) + 1);
			data.NotID = 0;
			data.Itemid = hdnItemID.Value;
			data.UseYn = "Y";
			data.UserID = u.UserID;

			biz.TikleAdminMainNoticeInsert(data);


			//this.ClientScript.RegisterClientScriptBlock(GetType(), string.Empty, "alert('저장되었습니다.');", true);

			//databind();
			Response.Redirect("SKTizen.aspx");
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			MainNoticeType data = new MainNoticeType();
			
			foreach (RepeaterItem item in rptGlossary.Items)
			{
				CheckBox chkNotID = (CheckBox)item.FindControl("chkRptNotID");

				if (chkNotID.Checked)
				{
					biz.TikleAdminMainNoticeDelete(strGubun, chkNotID.Attributes["Value"]);
				}
			}

			Response.Redirect("SKTizen.aspx");
		}
	}
}