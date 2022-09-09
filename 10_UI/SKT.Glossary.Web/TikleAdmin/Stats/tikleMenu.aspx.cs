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

namespace SKT.Glossary.Web.TikleAdmin.Stats
{
    public partial class tikleMenu : System.Web.UI.Page
    {
        private string SearchSDate = string.Empty;
        private string SearchEDate = string.Empty;
        public const string XLSX_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (!IsPostBack)
            {
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
            DataSet totalList = biz.TikleAdminMenu(searchStartDate, searchEndDate);
            rptIn.DataSource = totalList.Tables[0];
            rptIn.DataBind();

            if (totalList.Tables[0].Rows.Count > 0)
            {
                litGlossaryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null).ToString()));
                //litPeopleCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(PeopleCNT)", null).ToString()));
                //litDirectoryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(DirectoryCNT)", null).ToString()));
                //litSurveyCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(SurveyCNT)", null).ToString()));
                //litScheduleCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(ScheduleCNT)", null).ToString()));
                //litQnACNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QnACNT)", null).ToString()));
            }
            else
            {
                litGlossaryCNT.Text = "0";
                //litPeopleCNT.Text = "0";
                //litDirectoryCNT.Text = "0";
                //litSurveyCNT.Text = "0";
                //litScheduleCNT.Text = "0";
                //litQnACNT.Text = "0";
            }

            this.txtsDate.Text = syyMMdd;
            this.txteDate.Text = eyyMMdd;
        }

        protected void btnStatDeptToExcel_Click(object sender, ImageClickEventArgs e)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();
            string searchStartDate = txtsDate.Text.Replace(".", "");
            string searchEndDate = txteDate.Text.Replace(".", "");
            DataSet totalList = biz.TikleAdminMenu(searchStartDate, searchEndDate);
            //DataTable dt = totalList.Tables[0];

            DataTable dt = new DataTable();
            DataColumn cl = new DataColumn("부서명");
            dt.Columns.Add(cl);
            cl = new DataColumn("끌.지식");
            dt.Columns.Add(cl);
            
            DataRow dr;

            foreach (DataRow drTotal in totalList.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr[0] = drTotal["TODAY"].ToString();
                dr[1] = Convert.ToDouble(drTotal["GlossaryCNT"]);
                dt.Rows.Add(dr);
            }
            if (totalList.Tables[0].Rows.Count > 0)
            {
               
                dr = dt.NewRow();
                dr[0] = "합계";
                dr[1] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null).ToString()));
                dt.Rows.Add(dr);
            }

            byte[] dataBytes = null;
            dataBytes = GetBytes(dt);

            DownloadExcel(dataBytes);

        }

        public void DownloadExcel(byte[] dataBytes)
        {
            string fileName = "TikleMenu_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

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
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("통계_메뉴");

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