using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Linq;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Collections;
using System.Web.Services;
using System.Data;
using System.Reflection;
using Zio.Common;
using Zio.Type;


namespace SKT.Glossary.Web.TikleAdmin.Platform
{
    public partial class PlatStat : System.Web.UI.Page
    {
        private string SearchSDate = string.Empty;
        private string SearchEDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (!IsPostBack)
            {

                SearchSDate = (Request["txtsDate"] ?? DateTime.Now.AddDays(-7).ToString("yyyy.MM.dd")).ToString();
                SearchEDate = (Request["txteDate"] ?? DateTime.Now.ToString("yyyy.MM.dd")).ToString();

                UserInfo u = new UserInfo(this.Page);
                if (u.isAdmin)
                {
                    AdminProcess();
                }

                BindSelect(SearchSDate, SearchEDate);
            }
        }

        protected void AdminProcess()
        {
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            BindSelect(txtsDate.Text, txteDate.Text);
        }



        private void BindSelect(string syyMMdd, string eyyMMdd)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();
            string searchStartDate = syyMMdd.Replace(".", "");
            string searchEndDate = eyyMMdd.Replace(".", "");
            DataSet totalList = biz.TikleAdminPlatStat(searchStartDate, searchEndDate);
            DataTable dt = totalList.Tables[0];
            rptIn.DataSource = dt;
            rptIn.DataBind();

            if (dt.Rows.Count > 0)
            {
                litTOTALCNT.Text = string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(TOTALCNT)", null).ToString()));
                //litTOTALAVG.Text = string.Format("{0:#,0.0}", dt.AsEnumerable().Average((row) => Convert.ToDouble(row["TOTALCNT"])));
            }
            else
            {
                litTOTALCNT.Text = "0";
                //litTOTALAVG.Text = "0";
            }

            this.txtsDate.Text = syyMMdd;
            this.txteDate.Text = eyyMMdd;

            //litGTOTALCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[1].Rows[0]["GTOTALCNT"].ToString()));
            //litQTOTALCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[1].Rows[0]["QTOTALCNT"].ToString()));
            //litDIRTOTALCNT.Text = "<a href='javascript:fn_DirExcelList();'>" + string.Format("{0:#,0}", Int32.Parse(totalList.Tables[1].Rows[0]["DIRTOTALCNT"].ToString())) + " </a > ";
            //litSVTOTALCNT.Text = "<a href='javascript:fn_SurveyExcelList();'>" + string.Format("{0:#,0}", Int32.Parse(totalList.Tables[1].Rows[0]["SVTOTALCNT"].ToString())) + " </a > ";
        }

        protected void btnStatTotalToExcel_Click(object sender, ImageClickEventArgs e)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();
            string searchStartDate = txtsDate.Text.Replace(".", "");
            string searchEndDate = txteDate.Text.Replace(".", "");
            DataSet totalList = biz.TikleAdminPlatStat(searchStartDate, searchEndDate);
            DataTable dt = totalList.Tables[0];

            string calcTrTd = string.Empty;
            if (dt.Rows.Count > 0)
            {
                calcTrTd =
                    "<tr>"
                    + "<td>합계</td>"
                        + "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(TOTALCNT)", null).ToString())) + "</td>"
                    + "</tr>";
            }

            ZioExport zex = new ZioExport();
            ZioExcelTableType zett = new ZioExcelTableType();

            zett.headTrTdTag =
                    "<tr>"
                    + "<td>날짜</td><td>접속자수</td>"
                    + "</tr>";
            zett.bodyRecords = dt;
            zett.tableStyleAttributes =
                "border='1' bgColor='#ffffff' "
                + "borderColor='#000000' cellSpacing='0' cellPadding='0' "
                + "style='font-size:10.0pt; font-family:Gulim; background:white;'";
            zett.excelFilename = "TiklePlatStats_";
            zett.footTrTdTag = calcTrTd;
            zex.ExcelTable(zett);
        }
    }
}