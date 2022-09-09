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
namespace SKT.Glossary.Web.TikleAdmin.Stats
{
    public partial class tikleWeeklynote : System.Web.UI.Page
    {
        private string SearchSDate = string.Empty;
        private string SearchEDate = string.Empty;

        protected bool td_visible = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (!IsPostBack)
            {
                //SearchSDate = (Request["txtsDate"] ?? DateTime.Now.AddDays(-7).ToString("yyyy.MM.dd")).ToString();
                //SearchEDate = (Request["txteDate"] ?? DateTime.Now.ToString("yyyy.MM.dd")).ToString();

                //요일 정하기 및 부서 바운딩
                GetDayOfWeek(); 

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
                //    SearchEDate = DateTime.Now.AddDays(+6).ToString("yyyy.MM.dd").ToString();
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

            TikleAdadminBiz biz = new TikleAdadminBiz();
            DataTable dt = biz.TikleAdminTargetDept().Tables[0];
            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_dept.Items.Add(new ListItem(dt.Rows[i]["department"].ToString(), dt.Rows[i]["DEPARTMENTNUMBER"].ToString()));
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
            DataSet totalList = biz.TikleAdminWeeklyData(searchStartDate, searchEndDate,ddl_dept.SelectedValue);

            if (new[] {  "00001634", "00001060", "00001435", "00001699", "00003737", "00003761" }.Contains(ddl_dept.SelectedValue))
            {
                td_visible = false;
            }

            //if (totalList.Tables[0].Rows[0]["displayLevel"].ToString() == "04")
            //{
            //    totalList.Tables[0].Rows[0]["Weekly3"] = "";
            //    totalList.Tables[0].Rows[0]["Weekly4"] = "";
            //    totalList.Tables[0].Rows[0]["note3"] = "";
            //    totalList.Tables[0].Rows[0]["noteCNT"] = "";
            //}

            rptIn.DataSource = totalList.Tables[0];
            rptIn.DataBind();

            //if (totalList.Tables[0].Rows.Count > 0)
            //{
            //    litTVCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(TVCNT)", null).ToString()));
            //    litWNoteCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(WNoteCNT)", null).ToString()));
            //    litWNotePer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(WNoteCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(TVCNT)", null)));

            //    //litUserViewCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(UVCNT)", null).ToString()));
            //    //litAllCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(AllCNT)", null).ToString()));
            //    //litAllPer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(AllCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            //    //litGlossaryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null).ToString()));
            //    //litGlossaryPer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(GlossaryCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            //    //litDirectoryCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(DirectoryCNT)", null).ToString()));
            //    //litDirectoryPer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(DirectoryCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            //    //litQnACNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(QnACNT)", null).ToString()));
            //    //litQnAPer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(QnACNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            //    //litPFCNT.Text = string.Format("{0:#,0}", Int32.Parse(totalList.Tables[0].Compute("Sum(PFCNT)", null).ToString()));
            //    //litPFPer.Text = DisplayPercentage(Convert.ToInt32(totalList.Tables[0].Compute("Sum(PFCNT)", null)), Convert.ToInt32(totalList.Tables[0].Compute("Sum(UVCNT)", null)));
            //}
            //else
            //{
            //    litTVCNT.Text = "0";
            //    litWNoteCNT.Text = "0";
            //    litWNotePer.Text = "0%";
            //}

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
            DataSet totalList = biz.TikleAdminWeeklyData(searchStartDate, searchEndDate, ddl_dept.SelectedValue);

            //if (totalList.Tables[0].Rows[0]["displayLevel"].ToString() == "04")
            //{
            //    totalList.Tables[0].Rows[0]["Weekly3"] = "";
            //    totalList.Tables[0].Rows[0]["Weekly4"] = "";
            //    totalList.Tables[0].Rows[0]["note3"] = "";
            //    totalList.Tables[0].Rows[0]["noteCNT"] = "";
            //}

            DataTable dt = totalList.Tables[0];
            dt.Columns.Remove("displayLevel");
            dt.Columns.Remove("Note3");
            dt.Columns.Remove("Note4");
            dt.Columns.Remove("TeamUserCount");

            //string calcTrTd = string.Empty;

            ZioExport zex = new ZioExport();
            ZioExcelTableType zett = new ZioExcelTableType();

            if (new[] { "00001634", "00001060", "00001435", "00001699", "00003737", "00003761" }.Contains(ddl_dept.SelectedValue))
            {
                td_visible = false;
            }

            if (td_visible)
            {
                zett.headTrTdTag = @"<tr>
					        <th class='tac' colspan='3'><strong>부서명</strong></th>
                            <th class='tac'><strong>Weekly 작성건수(팀장본인)</strong></th>
                            <th class='tac'><strong>Weekly 작성팀원(팀원수/팀정원)</strong></th>
                            <th class='tac'><strong>Weekly Note 수신팀원(팀원수/팀정원)</strong></th>
				            </tr>";
            }
            else
            {
                zett.headTrTdTag = @"<tr>
					        <th class='tac' colspan='2'><strong>부서명</strong></th>
                            <th class='tac'><strong>Weekly 작성건수(팀장본인)</strong></th>
                            <th class='tac'><strong>Weekly 작성팀원(팀원수/팀정원)</strong></th>
                            <th class='tac'><strong>Weekly Note 수신팀원(팀원수/팀정원)</strong></th>
				            </tr>";
                dt.Columns.Remove("displayName1");
            }
            
            //"<tr>"
            //+ "<td>부서명</td><td>끌.지식</td><td>끌.담당자</td><td>끌.문서</td><td>끌.의견</td><td>끌.일정</td><td>끌.질문</td>"
            //+ "</tr>";
            zett.bodyRecords = dt;
            zett.tableStyleAttributes =
                "border='1' bgColor='#ffffff' "
                + "borderColor='#000000' cellSpacing='0' cellPadding='0' "
                + "style='font-size:10.0pt; font-family:Gulim; background:white;'";
            zett.excelFilename = "TikleWeeklyStats_";
            //zett.footTrTdTag = calcTrTd;
            zex.ExcelTable(zett);
        }
    }
}