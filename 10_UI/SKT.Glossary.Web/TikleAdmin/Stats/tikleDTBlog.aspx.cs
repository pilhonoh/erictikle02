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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;


namespace SKT.Glossary.Web.TikleAdmin.Stats
{

    public partial class tikleDTBlog : System.Web.UI.Page
    {
        protected UserInfo u;
        public const string XLSX_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);

            if (!u.isAdmin)
            {
                string infomsg = "이화면을 보신분은 접속가능한 사용자가 아닙니다.";
                Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
            }

            if(!IsPostBack)
            {
                //this.txtsDate.Text = (Request["txtsDate"] ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy.MM.dd"));
                //this.txteDate.Text = (Request["txteDate"] ?? DateTime.Now.ToString("yyyy.MM.dd")).ToString();
            }
        }
        [WebMethod]
        public static Dictionary<string, object> GetList(string Gubun, string PageNumber, string PageSize, string SDate, string EDate)
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();

            int iGubun = Gubun.Equals(string.Empty) ? 0 : int.Parse(Gubun);
            int iPageNumber = PageNumber.Equals(string.Empty) ? 0 : int.Parse(PageNumber);
            int iPageSize = PageSize.Equals(string.Empty) ? 0 : int.Parse(PageSize);

            DataSet ds = biz.GlossaryAdminDTLogList(iGubun, iPageNumber, iPageSize, SDate.Replace(".", "-"), EDate.Replace(".", "-"));

            return Utility.ToJson(ds);

        }
       

        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {
            GlossaryAdminBiz biz = new GlossaryAdminBiz();
            DataSet ds = biz.GlossaryAdminDTLogExcel(this.txtsDate.Text.Replace(".","-"), this.txteDate.Text.Replace(".", "-"));

            DownloadExcel(ds);
        }

        /// <summary>
        /// xls 파일을 위한 ContentType
        /// </summary>
        public void DownloadExcel(byte[] dataBytes)
        {
            string fileName = "DTBlog_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

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

        /// <summary>
        /// Excel 파일을 다운로드 한다.
        /// </summary>
        /// <param name="response">HttpResponse</param>
        /// <param name="dt">데이타 테이블</param>
        /// <param name="fileName">파일명</param>
        /// <param name="worksheetTitle">워크시트명</param>
        public void DownloadExcel(DataSet ds)
        {
            byte[] dataBytes = null;
            dataBytes = GetBytes(ds);

            DownloadExcel(dataBytes);
        }

        /// <summary>
        /// Excel 데이타를 가져온다.
        /// </summary>
        /// <param name="dt">데이타 테이블</param>
        /// <param name="worksheetTitle">워크시트명</param>
        /// <returns>Excel 데이타</returns>
        public byte[] GetBytes(DataSet ds)
        {
            byte[] dataBytes = null;
            
            
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("1.접속자로그");
                
                ws.Cells["A1"].LoadFromDataTable(ds.Tables[0], true);

                using (ExcelRange rng = ws.Cells[1, 1, 1, ds.Tables[0].Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                ws.Cells.AutoFitColumns();


                ws = pck.Workbook.Worksheets.Add("2.게시글별접속자로그");
                ws.Cells["A1"].LoadFromDataTable(ds.Tables[1], true);

                using (ExcelRange rng = ws.Cells[1, 1, 1, ds.Tables[1].Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);
                }
                ws.Cells.AutoFitColumns();

                ws = pck.Workbook.Worksheets.Add("3.게시글별좋아요");
                ws.Cells["A1"].LoadFromDataTable(ds.Tables[2], true);

                using (ExcelRange rng = ws.Cells[1, 1, 1, ds.Tables[2].Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(Color.White);
                }
                ws.Cells.AutoFitColumns();

                dataBytes = pck.GetAsByteArray();
            }

            

            return dataBytes;
        }
        protected void rptmember_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

        }



        }


}