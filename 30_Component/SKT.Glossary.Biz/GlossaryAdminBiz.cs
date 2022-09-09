using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Common;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
    public class GlossaryAdminBiz
    {

        public void GlossaryAdminSearchKeywordsInsert(string searchtype, string searchKeyword, string userid)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            dac.GlossaryAdminSearchKeywordsInsert(searchtype, searchKeyword, userid);            
        }


        //관리자 통계 조회
        public DataSet GlossaryAdminStatList(int PageNum, int PageSize, string sdate, string edate, string stime, string etime, int mode)
        {
            //ArrayList list = new ArrayList();

            GlossaryAdminDac dac = new GlossaryAdminDac();

            DataSet ds = new DataSet();

            ds = dac.GlossaryAdminStatList(PageNum, PageSize, sdate, edate, stime, etime, mode);
            /* 20140109 , 사용하지 않음
             if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                
                 foreach (DataRow dr in ds.Tables[0].Rows)
                 {
                     GlossaryAdminType Board = new GlossaryAdminType();
                     Board.Num = dr["NUM"].ToString();
                     Board.Date = dr["DATE"].ToString();
                     Board.Dow = dr["DOW"].ToString();
                     Board.Vtotal = dr["VTOTAL"].ToString();
                     Board.WtWiki = dr["WTWIKI"].ToString();
                     Board.WtNateon = dr["WTNATEON"].ToString();
                     Board.WtEmail = dr["WTEMAIL"].ToString();
                     Board.WtETotal = dr["WTETOTAL"].ToString();
                     Board.EdTotal = dr["EDTOTAL"].ToString();
                     Board.WtEdTotal = dr["WTEDTOTAL"].ToString();
                     Board.Question = dr["QUESTION"].ToString();
                     Board.Answer = dr["ANSWER"].ToString();
                     Board.Exe107 = dr["EXE107"].ToString();
                     Board.Request = dr["REQUEST"].ToString();

                     if (Board.Date != "TOTAL")
                     {
                         list.Add(Board);
                     }                    
                 }
             }
            */
            return ds;
        }

        public DataTable GlossaryAdminStatDivList(string mode, string syear)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            DataSet ds = dac.GlossaryAdminStatDivList(mode, syear);
            DataTable dt = ds.Tables[0];
            DataRow drEtc = dt.NewRow();
            DataRow drSum = dt.NewRow();


            for(int i=0; i <dt.Rows.Count; i++)
            {
                if(dt.Rows[i]["DIVISION"].ToString() == "기타부서")
                {
                    drEtc.ItemArray = dt.Rows[i].ItemArray;
                    dt.Rows.Remove(dt.Rows[i]);
                }

                if (dt.Rows[i]["DIVISION"].ToString() == "총계")
                {
                    drSum.ItemArray = dt.Rows[i].ItemArray;
                    dt.Rows.Remove(dt.Rows[i]);
                }
            }
            dt.Rows.Add(drEtc);
            dt.Rows.Add(drSum);

            return dt;
        }

        public DataSet GlossaryAdminExceptUserList(int curpage, int pagesize, string schText)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            DataSet ds = new DataSet();

            ds = dac.GlossaryAdminExceptUserList(curpage, pagesize, schText);

            return ds;
        }

        public DataSet GlossaryAdminExceptInsert(string userid, string level, string registerid)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            return dac.GlossaryAdminExceptInsert(userid, level, registerid);
        }

        public DataSet GlossaryAdminExceptDelete(string userid)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            return dac.GlossaryAdminExceptDelete(userid);
        }

        public void ExporttoExcel(DataTable table)
        {
            DateTime dtime = DateTime.Now;

            string filename = "TikleStatistics" + dtime.ToString("yyyyMMdd") + dtime.Hour.ToString() + dtime.Minute.ToString() + ".xls";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename={0}", filename));
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Gulim;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            HttpContext.Current.Response.Write("<Td>no</TD>");
            HttpContext.Current.Response.Write("<Td>날짜</TD>");
            HttpContext.Current.Response.Write("<Td>요일</TD>");
            HttpContext.Current.Response.Write("<Td>방문자</TD>");
            HttpContext.Current.Response.Write("<Td>Web작성</TD>");
            HttpContext.Current.Response.Write("<Td>NateOn작성</TD>");
            HttpContext.Current.Response.Write("<Td>Outlook작성</TD>");
            HttpContext.Current.Response.Write("<Td>작성종합</TD>");
            HttpContext.Current.Response.Write("<Td>편집종합</TD>");
            HttpContext.Current.Response.Write("<Td>작성+편집종합</TD>");
            HttpContext.Current.Response.Write("<Td>질문</TD>");
            HttpContext.Current.Response.Write("<Td>답변</TD>");
            HttpContext.Current.Response.Write("<Td>임원접속</TD>");
            HttpContext.Current.Response.Write("<Td>개선요청</TD>");
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public ArrayList GlossaryHallofFameAdminList(string Mode, int PageNum, int PageSize, out int TotalCount, out int ShowCount)
        {
            ArrayList list = new ArrayList();
            GlossaryAdminDac Dac = new GlossaryAdminDac();
            DataSet ds = Dac.GlossaryAdminHallofFameList(Mode, PageNum, PageSize, null);
            TotalCount = 0;
            ShowCount = 0;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);

                ShowCount = Convert.ToInt32(ds.Tables[2].Rows[0]["ShowCount"]);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryHallOfFameType data = new GlossaryHallOfFameType();

                    data.RowNum = dr["RowNum"].ToString();
                    data.ID = dr["ID"].ToString();
                    data.GlossaryID = dr["GlossaryID"].ToString();
                    data.GlossaryFrom = dr["GlossaryFrom"].ToString();

                    data.Title = (dr["Title"] == DBNull.Value) ? "" : dr["Title"].ToString();
                    data.FirstWriteDate =  (dr["FirstWriteDate"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["FirstWriteDate"]).ToString("yyyy-MM-dd HH:mm");
                    data.LastWriteUserID = (dr["LastWriteUserID"] == DBNull.Value) ? "" : dr["LastWriteUserID"].ToString();
                    data.LastWriteUserName = (dr["LastWriteUserName"] == DBNull.Value) ? "" : dr["LastWriteUserName"].ToString();
                    data.LastWriteUserDeptName = (dr["LastWriteUserDeptName"] == DBNull.Value) ? "" : dr["LastWriteUserDeptName"].ToString();
                    data.LastWriteDate = (dr["LastWriteDate"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["LastWriteDate"]).ToString("yyyy-MM-dd HH:mm");

                    data.CreateUserID = (dr["CreateUserID"] == DBNull.Value) ? "" : dr["CreateUserID"].ToString();
                    data.CreateUserIP = (dr["CreateUserIP"] == DBNull.Value) ? "" : dr["CreateUserIP"].ToString();
                    data.CreateUserMachineName = (dr["CreateUserMachineName"] == DBNull.Value) ? "" : dr["CreateUserMachineName"].ToString();
                    data.CreateDate = (dr["CreateDate"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm");
                    data.CreateAutoYN = (dr["CreateAutoYN"] == DBNull.Value) ? "" : dr["CreateAutoYN"].ToString();

                    data.LastModifiedUserID = (dr["LastModifiedUserID"] == DBNull.Value) ? "" : dr["LastModifiedUserID"].ToString();
                    data.LastModifiedUserIP = (dr["LastModifiedUserIP"] == DBNull.Value) ? "" : dr["LastModifiedUserIP"].ToString();
                    data.LastModifiedUserMachineName = (dr["LastModifiedUserMachineName"] == DBNull.Value) ? "" : dr["LastModifiedUserMachineName"].ToString();
                    data.LastModifiedDate = (dr["LastModifiedDate"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["LastModifiedDate"]).ToString("yyyy-MM-dd HH:mm");

                    data.DisplayYN = (dr["DisplayYN"] == DBNull.Value) ? 0 : (int)dr["DisplayYN"];
                    data.OrderDate = (dr["OrderDate"] == DBNull.Value) ? "" : dr["OrderDate"].ToString();
                    data.FixYN = (dr["FixYN"] == DBNull.Value) ? "" : dr["FixYN"].ToString();
                    data.CssTitleBox = (dr["CssTitleBox"] == DBNull.Value) ? "" : dr["CssTitleBox"].ToString();

                    list.Add(data);
                }
            }
            return list;
        }       

        public void GlossaryHallofFameAdminUpdate(string Mode, GlossaryHallOfFameType Board)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            dac.GlossaryAdminHallofFameUpdate(Mode, Board);
        }

        public DataSet GlossaryAdminDTLogExcel(string sDate, string eDate)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            DataSet ds = new DataSet();

            ds = dac.GlossaryAdminDTLogExcel(sDate, eDate);

            return ds;
        }

        public DataSet GlossaryAdminDTLogList(int iGubun, int iPageNum, int iPageSize, string sDate, string eDate)
        {
            GlossaryAdminDac dac = new GlossaryAdminDac();
            DataSet ds = new DataSet();

            ds = dac.GlossaryAdminDTLogList(iGubun, iPageNum, iPageSize, sDate, eDate);

            return ds;
        }
    }
}

