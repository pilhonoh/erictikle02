using System;
using System.Collections.Generic;
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
using System.Reflection;
using System.Linq;
using Zio.Common;
using Zio.Type;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace SKT.Glossary.Web.TikleAdmin
{
    public partial class TikleDept : System.Web.UI.Page
    {

        private string SearchSDate = string.Empty;
        private string SearchEDate = string.Empty;
        public const string XLSX_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (!IsPostBack) {

                SearchSDate = (Request["txtsDate"] ?? DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString().PadLeft(2, '0') + ".01");
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
            DataSet totalList = biz.TikleAdminDept(searchStartDate, searchEndDate);
            rptIn.DataSource = totalList.Tables[0];
            rptIn.DataBind();

            
            if (totalList.Tables[0].Rows.Count > 0)
            {
                litTOTALCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(TOTALCNT)",null).ToString()));
                litEXE107CNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(EXE107CNT)", null).ToString()));
                litGCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GCNT)", null).ToString()));
                litGECNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GECNT)", null).ToString()));
                //litDIRCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(DIRCNT)", null).ToString()));
                //litSVCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(SVCNT)", null).ToString()));
                //litQCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QCNT)", null).ToString()));
                //litQCCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QCCNT)", null).ToString()));
            }
            else
            {
                litTOTALCNT.Text = "0";
                litEXE107CNT.Text = "0";
                litGCNT.Text = "0";
                litGECNT.Text = "0";
                //litDIRCNT.Text = "0";
                //litSVCNT.Text = "0";
                //litQCNT.Text = "0";
                //litQCCNT.Text = "0";
            }

            this.txtsDate.Text = syyMMdd;
            this.txteDate.Text = eyyMMdd; 

           
        }

        protected void btnStatDeptToExcel_Click(object sender, ImageClickEventArgs e)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();
            string searchStartDate = txtsDate.Text.Replace(".", "");
            string searchEndDate = txteDate.Text.Replace(".", "");
            DataSet totalList = biz.TikleAdminDept(searchStartDate, searchEndDate);

            DataTable dt = new DataTable();
            DataColumn cl = new DataColumn("부서명");
            dt.Columns.Add(cl);
            cl = new DataColumn("접속수");
            dt.Columns.Add(cl);
            cl = new DataColumn("임원접속수");
            dt.Columns.Add(cl);
            cl = new DataColumn("지식등록수");
            dt.Columns.Add(cl);
            cl = new DataColumn("지식편집수");
            dt.Columns.Add(cl);

            DataRow dr;

            foreach (DataRow drTotal in totalList.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr[0] = drTotal["TODAY"].ToString();
                dr[1] = Convert.ToDouble(drTotal["TOTALCNT"]);
                dr[2] = Convert.ToDouble(drTotal["EXE107CNT"]);
                dr[3] = Convert.ToDouble(drTotal["GCNT"]);
                dr[4] = Convert.ToDouble(drTotal["GECNT"]);
                dt.Rows.Add(dr);
            }
            if (totalList.Tables[0].Rows.Count > 0)
            {
                dr = dt.NewRow();
                dr[0] = "합계";
                dr[1] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(TOTALCNT)", null).ToString()));
                dr[2] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(EXE107CNT)", null).ToString()));
                dr[3] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GCNT)", null).ToString()));
                dr[4] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GECNT)", null).ToString()));
                dt.Rows.Add(dr);
            }

            byte[] dataBytes = null;
            dataBytes = GetBytes(dt);

            DownloadExcel(dataBytes);

            //DataTable dt = totalList.Tables[0];

            //string calcTrTd = string.Empty;
            //if (dt.Rows.Count > 0)
            //{
            //    calcTrTd =                    
            //        "<tr>"
            //        + "<td>합계</td>"
            //            + "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(TOTALCNT)", null).ToString())) + "</td>"
            //            + "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(EXE107CNT)", null).ToString())) + "</td>"
            //            + "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(GCNT)", null).ToString())) + "</td>"
            //            + "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(GECNT)", null).ToString())) + "</td>"
            //            //+ "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(DIRCNT)", null).ToString())) + "</td>"
            //            //+ "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(SVCNT)", null).ToString())) + "</td>"
            //            //+ "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(QCNT)", null).ToString())) + "</td>"
            //            //+ "<td>" + string.Format("{0:#,0}", Int32.Parse(dt.Compute("Sum(QCCNT)", null).ToString())) + "</td>"
            //        + "</tr>";
            //}

            //ZioExport zex = new ZioExport();
            //ZioExcelTableType zett = new ZioExcelTableType();

            //zett.headTrTdTag =
            //        "<tr>"
            //        + "<td>부서명</td><td>접속수</td><td>임원접속수</td><td>지식등록수</td><td>지식편집수</td>"
            //        + "</tr>";
            //zett.bodyRecords = dt;
            //zett.tableStyleAttributes =
            //    "border='1' bgColor='#ffffff' "
            //    + "borderColor='#000000' cellSpacing='0' cellPadding='0' "
            //    + "style='font-size:10.0pt; font-family:Gulim; background:white;'";
            //zett.excelFilename = "TikleDeptStats_";
            //zett.footTrTdTag = calcTrTd;
            //zex.ExcelTable(zett);
        }

        public void DownloadExcel(byte[] dataBytes)
        {
            string fileName = "TikleDept_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            string extension = Path.GetExtension(fileName);
            // EPPlus 라이브러리가 xlsx만 지원
            if (string.IsNullOrEmpty(extension) || extension.ToLower() != ".xlsx")
            {
                throw new ArgumentException("Invalid Filename");
            }
            fileName = HttpUtility.UrlPathEncode(fileName);

            Response.ContentType = XLSX_CONTENT_TYPE;
            Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", fileName));
            Response.BinaryWrite(dataBytes);
            Response.Flush();
            Response.End();
        }

        public byte[] GetBytes(DataTable dt)
        {
            byte[] dataBytes = null;


            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("통계_부서");

                ws.Cells["A1"].LoadFromDataTable(dt, true);

                using (ExcelRange rng = ws.Cells[1, 1, 1, dt.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                ws.Cells.AutoFitColumns();

                dataBytes = pck.GetAsByteArray();
            }

            return dataBytes;
        }
    }
}