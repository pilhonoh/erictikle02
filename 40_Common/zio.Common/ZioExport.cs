using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Collections;
using Zio.Type;


namespace Zio.Common
{
    public class ZioExport
    {
        /*예제
            GlossaryAdminBiz biz = new GlossaryAdminBiz();
            ZioExport zex = new ZioExport();
            ZioExcelTableType zett = new ZioExcelTableType();

            zett.headTrTdTag =
                    "<tr><td colspan='13'>"+ddlSearchYear.SelectedValue+"년 "+ddlSearchWeeks.SelectedItem.Text+"</td></tr>"
                    +"<tr>"
                        +"<td rowspan='2'>구분</td><td colspan='6'>Vistiors<td colspan='6'>Contents</td>"            
                    + "</tr>"
                    +"<tr>"
                        + "<td>월</td><td>화</td><td>수</td><td>목</td><td>금</td><td>일평균</td>"
                        + "<td>작성</td><td>편집</td><td>질문</td><td>답변</td><td>합계</td><td>일평균</td>"                       
                    + "</tr>";
            zett.bodyRecords = biz.GlossaryAdminStatDivList("00", ddlSearchWeeks.SelectedValue);
            zett.tableStyleAttributes =
                "border='1' bgColor='#ffffff' "
                + "borderColor='#000000' cellSpacing='0' cellPadding='0' "
                + "style='font-size:10.0pt; font-family:Gulim; background:white;'";
            zett.excelFilename = "TikleStaticDivisionWeeks_";

            zex.ExcelTable(zett);
         */
        public void ExcelTable(ZioExcelTableType zett)
        {
            DateTime dtime = DateTime.Now;
            zett.excelFilename = zett.excelFilename + dtime.ToString("yyyyMMdd") + dtime.Hour.ToString() + dtime.Minute.ToString() + ".xls";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "file/unknown";
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content='text/html; charset=utf-8'>");
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename={0}", zett.excelFilename));
            HttpContext.Current.Response.AddHeader("Content-Description", "Servlet Generated Data");
            HttpContext.Current.Response.AddHeader("Pragma","no-cache");
            HttpContext.Current.Response.AddHeader("Cache-Control", "private");
            HttpContext.Current.Response.AddHeader("Expires", "0");
            //HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //System.Text.Encoding.GetEncoding("utf-8");

            HttpContext.Current.Response.Write("<Table " + zett.tableStyleAttributes + " >");
            HttpContext.Current.Response.Write(zett.headTrTdTag);
            /* td 직접 입력으로 변경
            foreach (string column in columnnameList) //arraylist
            {
                HttpContext.Current.Response.Write("<td>" + column + "</td>");
            }*/          
            foreach (DataRow row in zett.bodyRecords.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < zett.bodyRecords.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write(zett.footTrTdTag);
            HttpContext.Current.Response.Write("</Table>");
            
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }

}
