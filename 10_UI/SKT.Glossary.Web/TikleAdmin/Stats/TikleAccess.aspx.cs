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
    public partial class TikleAccess : System.Web.UI.Page
    {
        private string SearchSDate = string.Empty;
        private string SearchEDate = string.Empty;
        public const string XLSX_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (!IsPostBack)
            {
                //SearchSDate = (Request["txtsDate"] ?? DateTime.Now.AddDays(-7).ToString("yyyy.MM.dd")).ToString();
                //SearchEDate = (Request["txteDate"] ?? DateTime.Now.ToString("yyyy.MM.dd")).ToString();

                SearchSDate = (Request["txtsDate"] ?? DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString().PadLeft(2, '0') + ".01");
                SearchEDate = (Request["txteDate"] ?? DateTime.Now.ToString("yyyy.MM.dd")).ToString();

                //요일 정하기
                //GetDayOfWeek();               

                UserInfo u = new UserInfo(this.Page);
                if (u.isAdmin)
                {
                    AdminProcess();
                }

                BindSelect(SearchSDate, SearchEDate);
            }
        }

        protected void GetDayOfWeek()
        {
            var d = DateTime.Now.DayOfWeek;

            switch (d)
            {
                //case DayOfWeek.Sunday:
                //    SearchSDate = DateTime.Now.ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+5).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Monday:
                //    SearchSDate = DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+5).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Tuesday:
                //    SearchSDate = DateTime.Now.AddDays(-2).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+4).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Wednesday:
                //    SearchSDate = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+3).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Thursday:
                //    SearchSDate = DateTime.Now.AddDays(-4).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+2).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Friday:
                //    SearchSDate = DateTime.Now.AddDays(-5).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.AddDays(+1).ToString("yyyy.MM.dd").ToString();
                //    break;
                //case DayOfWeek.Saturday:
                //    SearchSDate = DateTime.Now.AddDays(-6).ToString("yyyy.MM.dd").ToString();
                //    SearchEDate = DateTime.Now.ToString("yyyy.MM.dd").ToString();
                //    break;
                case DayOfWeek.Sunday:
                    SearchSDate = DateTime.Now.AddDays(-7).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Monday:
                    SearchSDate = DateTime.Now.AddDays(-8).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-2).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Tuesday:
                    SearchSDate = DateTime.Now.AddDays(-9).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-3).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Wednesday:
                    SearchSDate = DateTime.Now.AddDays(-10).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-4).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Thursday:
                    SearchSDate = DateTime.Now.AddDays(-11).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-5).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Friday:
                    SearchSDate = DateTime.Now.AddDays(-12).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-6).ToString("yyyy.MM.dd").ToString();
                    break;
                case DayOfWeek.Saturday:
                    SearchSDate = DateTime.Now.AddDays(-13).ToString("yyyy.MM.dd").ToString();
                    SearchEDate = DateTime.Now.AddDays(-7).ToString("yyyy.MM.dd").ToString();
                    break;
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
            DataSet totalList = biz.TikleAdminAccess(searchStartDate, searchEndDate);

            //부서 인원수는 하드코딩처리 / 진현빈 대리 요청 2016-04-22
            //int[] deptcount = new int[20];

            //deptcount[0] = 54;
            //deptcount[1] = 18;
            //deptcount[2] = 38;
            //deptcount[3] = 47;
            //deptcount[4] = 61;
            //deptcount[5] = 43;
            //deptcount[6] = 820;
            //deptcount[7] = 400;
            //deptcount[8] = 1;
            //deptcount[9] = 45;
            //deptcount[10] = 1093;
            //deptcount[11] = 159;
            //deptcount[12] = 85;
            //deptcount[13] = 68;
            //deptcount[14] = 325;
            //deptcount[15] = 35;
            //deptcount[16] = 193;
            //deptcount[17] = 93;
            //deptcount[18] = 30;
            //deptcount[19] = 289;

            //for (int i = 0; i < deptcount.Length; i++)
            //{
            //    totalList.Tables[0].Rows[i]["UVCNT"] = deptcount[i];
            //}

            rptIn.DataSource = totalList.Tables[0];
            rptIn.DataBind();

            

            if (totalList.Tables[0].Rows.Count > 0)
            {
                //Author : 개발자-김성환D, 리뷰자-진현빈D
                //Create Date : 2016.07.28
                //Desc : 값 변경
                litUserViewCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(UVCNT)", null).ToString()));
                litAllCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(AllCNT)", null).ToString()));
                litAllTotal.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(AllCNT_Total)", null).ToString()));
                litGlossaryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null).ToString()));
                litGlossaryTotal.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(Glossary_Total)", null).ToString()));
                //litDirectoryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(DirectoryCNT)", null).ToString()));
                //litDirectoryTotal.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(Directory_Total)", null).ToString()));
                //litQnACNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QnACNT)", null).ToString()));
                //litQnATotal.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QnA_Total)", null).ToString()));
                //litPFCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(PFCNT)", null).ToString()));
                //litPFTotal.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(PF_Total)", null).ToString()));

                //litPFTotal.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(PFCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            }
            else
            {
                litUserViewCNT.Text = "0";
                litAllCNT.Text = "0";
                litAllTotal.Text = "0";
                litGlossaryCNT.Text = "0";
                litGlossaryTotal.Text = "0";
                //litDirectoryCNT.Text = "0";
                //litDirectoryTotal.Text = "0";
                //litQnACNT.Text = "0";
                //litQnATotal.Text = "0";
                //litPFCNT.Text = "0";
                //litPFTotal.Text = "0";
            }

            this.txtsDate.Text = syyMMdd;
            this.txteDate.Text = eyyMMdd;
        }

        static string DisplayPercentage(int top, int bottom)
        {
            double ratio = (double)top / bottom;
            return string.Format("{0:0%}", ratio);
        }

        protected void btnStatDeptToExcel_Click(object sender, ImageClickEventArgs e)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();
            string searchStartDate = txtsDate.Text.Replace(".", "");
            string searchEndDate = txteDate.Text.Replace(".", "");
            DataSet totalList = biz.TikleAdminAccess(searchStartDate, searchEndDate);


            DataTable dt = new DataTable();
            DataColumn cl = new DataColumn("부서명");
            dt.Columns.Add(cl);
            cl = new DataColumn("인원수");
            dt.Columns.Add(cl);
            cl = new DataColumn("끌.전체 방문자");
            dt.Columns.Add(cl);
            cl = new DataColumn("끌.전체 이용횟수");
            dt.Columns.Add(cl);
            cl = new DataColumn("끌.지식 방문자");
            dt.Columns.Add(cl);
            cl = new DataColumn("끌.지식 이용횟수");
            dt.Columns.Add(cl);

            DataRow dr;

            foreach (DataRow drTotal in totalList.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr[0] = drTotal["TODAY"].ToString();
                dr[1] = Convert.ToDouble(drTotal["UVCNT"]);
                dr[2] = Convert.ToDouble(drTotal["AllCNT"]);
                dr[3] = Convert.ToDouble(drTotal["AllCNT_Total"]);
                dr[4] = Convert.ToDouble(drTotal["GlossaryCNT"]);
                dr[5] = Convert.ToDouble(drTotal["Glossary_Total"]);
                dt.Rows.Add(dr);
            }
            if (totalList.Tables[0].Rows.Count > 0)
            {
                dr = dt.NewRow();
                dr[0] = "합계";
                dr[1] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(UVCNT)", null).ToString()));
                dr[2] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(AllCNT)", null).ToString()));
                dr[3] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(AllCNT_Total)", null).ToString()));
                dr[4] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null).ToString()));
                dr[5] = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(Glossary_Total)", null).ToString()));
                dt.Rows.Add(dr);
            }

            byte[] dataBytes = null;
            dataBytes = GetBytes(dt);

            DownloadExcel(dataBytes);

        }

        public void DownloadExcel(byte[] dataBytes)
        {
            string fileName = "TikleAccess_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

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
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("통계_방문현황");

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