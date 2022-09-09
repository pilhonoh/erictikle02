using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;

namespace SKT.Glossary.Web.TikleAdmin.MainMng
{
    public partial class Banner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

			if (!IsPostBack)
			{
				Select();
			}
        }

		private void Select()
		{
			TikleAdadminBiz biz = new TikleAdadminBiz();
			DataSet dsBanner = biz.TikleAdminBannerSelect();

            string BANNER_DOWNLOAD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["BaseURL"]
            + System.Configuration.ConfigurationManager.AppSettings["BANNER_DOWNLOAD_FILE_PATH"];


			rdoBanner.SelectedValue = dsBanner.Tables[0].Rows[0]["ATTR_VAL"].ToString();

            //SetBannerCss();

			foreach (DataRow dr in dsBanner.Tables[1].Rows)
			{
				if (dr["SeqNo"].ToString() == "1")
				{
					hdnBanner1NotID.Value = dr["NotID"].ToString();
					txtBanner1Title.Value = dr["Title"].ToString();
                    if (dr["ImgFile"] != null && dr["ImgFile"].ToString() != string.Empty)
                    {
                        hdnBanner1imgFile.Value = dr["ImgFile"].ToString();
                        fileBanner1.Style["display"] = "none";
                        imgBanner1.Src = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();

                        btnBanner1Edit.Style.Remove("display");
                        btnBanner1Cancel.Style["display"] = "none";
                    }
                    else
                    {
                        fileBanner1.Style["display"] = "";
                    }
					txtBanner1Link.Value = dr["URL"].ToString();
				}
				else if (dr["SeqNo"].ToString() == "2")
				{
					hdnBanner2NotID.Value = dr["NotID"].ToString();
					txtBanner2Title.Value = dr["Title"].ToString();
                    if (dr["ImgFile"] != null && dr["ImgFile"].ToString() != string.Empty)
                    {
                        hdnBanner2imgFile.Value = dr["ImgFile"].ToString();
                        fileBanner2.Style["display"] = "none";
                        imgBanner2.Src = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();

                        btnBanner2Edit.Style.Remove("display");
                        btnBanner2Cancel.Style["display"] = "none";
                    }
                    else
                    {
                        fileBanner2.Style["display"] = "";
                    }
					txtBanner2Link.Value = dr["URL"].ToString();
				}
				else if (dr["SeqNo"].ToString() == "3")
				{
					hdnBanner3NotID.Value = dr["NotID"].ToString();
					txtBanner3Title.Value = dr["Title"].ToString();
                    if (dr["ImgFile"] != null && dr["ImgFile"].ToString() != string.Empty)
                    {
                        hdnBanner3imgFile.Value = dr["ImgFile"].ToString();
                        fileBanner3.Style["display"] = "none";
                        imgBanner3.Src = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();

                        btnBanner3Edit.Style.Remove("display");
                        btnBanner3Cancel.Style["display"] = "none";
                    }
                    else
                    {
                        fileBanner3.Style["display"] = "";
                    }
					txtBanner3Link.Value = dr["URL"].ToString();
				}
				else if (dr["SeqNo"].ToString() == "4")
				{
					hdnBanner4NotID.Value = dr["NotID"].ToString();
					txtBanner4Title.Value = dr["Title"].ToString();
                    if (dr["ImgFile"] != null && dr["ImgFile"].ToString() != string.Empty)
                    {
                        hdnBanner4imgFile.Value = dr["ImgFile"].ToString();
                        fileBanner4.Style["display"] = "none";
                        imgBanner4.Src = BANNER_DOWNLOAD_FILE_PATH + dr["ImgFile"].ToString();

                        btnBanner4Edit.Style.Remove("display");
                        btnBanner4Cancel.Style["display"] = "none";
                    }
                    else
                    {
                        fileBanner4.Style["display"] = "";
                    }
					txtBanner4Link.Value = dr["URL"].ToString();
				}
			}
		}

        private void SetBannerCss()
        {
            string selBannerCss = rdoBanner.SelectedValue;

            if (selBannerCss == "style1")
            {

                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Add("display", "none");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "450px");
                imgBanner1.Style.Add("height", "135px");
                imgBanner2.Style.Add("width", "450px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "0px");
                imgBanner3.Style.Add("height", "0px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style2")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Remove("display");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "220px");
                imgBanner1.Style.Add("height", "135px");
                imgBanner2.Style.Add("width", "220px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "450px");
                imgBanner3.Style.Add("height", "135px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style3")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Remove("display");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "450px");
                imgBanner1.Style.Add("height", "135px");
                imgBanner2.Style.Add("width", "220px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "220px");
                imgBanner3.Style.Add("height", "135px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style4")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Remove("display");
                dlBanner4.Style.Remove("display");
                imgBanner1.Style.Add("width", "220px");
                imgBanner1.Style.Add("height", "135px");
                imgBanner2.Style.Add("width", "450px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "450px");
                imgBanner3.Style.Add("height", "135px");
                imgBanner4.Style.Add("width", "450px");
                imgBanner4.Style.Add("height", "135px");
            }
            else if (selBannerCss == "style5")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Remove("display");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "220px");
                imgBanner1.Style.Add("height", "135px");
                imgBanner2.Style.Add("width", "220px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "220px");
                imgBanner3.Style.Add("height", "280px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style6")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Remove("display");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "220px");
                imgBanner1.Style.Add("height", "280px");
                imgBanner2.Style.Add("width", "220px");
                imgBanner2.Style.Add("height", "135px");
                imgBanner3.Style.Add("width", "220px");
                imgBanner3.Style.Add("height", "135px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style7")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Remove("display");
                dlBanner3.Style.Add("display", "none");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "220px");
                imgBanner1.Style.Add("height", "280px");
                imgBanner2.Style.Add("width", "220px");
                imgBanner2.Style.Add("height", "280px");
                imgBanner3.Style.Add("width", "0px");
                imgBanner3.Style.Add("height", "0px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
            else if (selBannerCss == "style8")
            {
                dlBanner1.Style.Remove("display");
                dlBanner2.Style.Add("display", "none");
                dlBanner3.Style.Add("display", "none");
                dlBanner4.Style.Add("display", "none");
                imgBanner1.Style.Add("width", "450px");
                imgBanner1.Style.Add("height", "280px");
                imgBanner2.Style.Add("width", "0px");
                imgBanner2.Style.Add("height", "0px");
                imgBanner3.Style.Add("width", "0px");
                imgBanner3.Style.Add("height", "0px");
                imgBanner4.Style.Add("width", "0px");
                imgBanner4.Style.Add("height", "0px");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
			UserInfo u = new UserInfo(this.Page);


			string BANNER_UPLOAD_FILE_PATH =  System.Configuration.ConfigurationManager.AppSettings["BANNER_UPLOAD_FILE_PATH"];

            if (fileBanner1.HasFile)
            {
				fileBanner1.SaveAs(BANNER_UPLOAD_FILE_PATH + "\\" + fileBanner1.FileName);
				//lblBanner1.Text = "File Uploaded: " + @"ETC\MainBanner\" + fileBanner1.FileName;
            }
            else
            {
                //lblBanner1.Text = "No File Uploaded.";
            }

            if (fileBanner2.HasFile)
            {
				fileBanner2.SaveAs(BANNER_UPLOAD_FILE_PATH + "\\" + fileBanner2.FileName);
				//lblBanner2.Text = "File Uploaded: " + @"ETC\MainBanner\" + fileBanner2.FileName;
            }
            else
            {
                //lblBanner2.Text = "No File Uploaded.";
            }

            if (fileBanner3.HasFile)
            {
				fileBanner3.SaveAs(BANNER_UPLOAD_FILE_PATH + "\\" + fileBanner3.FileName);
				//lblBanner3.Text = "File Uploaded: " + @"ETC\MainBanner\" + fileBanner3.FileName;
            }
            else
            {
                //lblBanner3.Text = "No File Uploaded.";
            }

            if (fileBanner4.HasFile)
            {
				fileBanner4.SaveAs(BANNER_UPLOAD_FILE_PATH + "\\" + fileBanner4.FileName);
				//lblBanner4.Text = "File Uploaded: " + @"ETC\MainBanner\" + fileBanner4.FileName;
            }
            else
            {
                //lblBanner4.Text = "No File Uploaded.";
            }

			TikleAdadminBiz biz = new TikleAdadminBiz();

			biz.TikleAdminSiteConfigUpdate("MainBanner", rdoBanner.SelectedValue, "메인화면 베너 스타일 설정", u.UserID);

			biz.TikleAdminBannerUpdate(
				hdnBanner1NotID.Value
				, txtBanner1Title.Value
                , ((fileBanner1.HasFile) ? @"ETC\MainBanner\" + fileBanner1.FileName : "")
				, txtBanner1Link.Value
				, "1"
				, u.UserID
			);

			biz.TikleAdminBannerUpdate(
				hdnBanner2NotID.Value
				, txtBanner2Title.Value
				, ((fileBanner2.HasFile)?@"ETC\MainBanner\" + fileBanner2.FileName : "")
				, txtBanner2Link.Value
				, "2"
				, u.UserID
			);

			biz.TikleAdminBannerUpdate(
				hdnBanner3NotID.Value
				, txtBanner3Title.Value
                , ((fileBanner3.HasFile) ? @"ETC\MainBanner\" + fileBanner3.FileName : "")
				, txtBanner3Link.Value
				, "3"
				, u.UserID
			);

			biz.TikleAdminBannerUpdate(
				hdnBanner4NotID.Value
				, txtBanner4Title.Value
                , ((fileBanner4.HasFile) ? @"ETC\MainBanner\" + fileBanner4.FileName : "")
				, txtBanner4Link.Value
				, "4"
				, u.UserID
			);

			Select();
        }

        protected void btn_Guestmode_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            TikleAdadminBiz biz = new TikleAdadminBiz();
            DataSet ds = new DataSet();
            ds = biz.TikleAdmin_GuestSwitch(u.UserID);

            string msg = string.Empty;
            if(ds.Tables[0].Rows[0]["Flag"].ToString().Trim().Equals("Y")){
                msg = "alert(' Daytime 서버점검으로 전환됩니다. ');";
            }
            else
            {
                msg = "alert('Daytime 서버점검이 완료되었습니다. ');";
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Guest_ALERT", msg,true);
        }
    }
}