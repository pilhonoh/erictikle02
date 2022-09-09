using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;

namespace SKT.Glossary.Biz
{
   public class GlossaryTempBiz
    {
        //임시저장 리스트
       public ArrayList GlossaryTempList(int PageNum, int PageSize, out int TotalCount, out int WTikleCount, out int MTikleCount, out int TTikleCount, string UserID)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;

            WTikleCount = 0;
            MTikleCount = 0;
            TTikleCount = 0;

            GlossaryTempDac dac = new GlossaryTempDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryTempList(PageNum, PageSize, UserID);
            WTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GW", 0);
            MTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GM", 0);
            TTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GT", 0);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryTempType Board = new GlossaryTempType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.DocumentKind = dr["DocumentKind"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.Description = dr["Description"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.LastCreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    if (dr["LastCreateDate"].ToString() != "")
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }
                    if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    {
                        byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                        try
                        {
                            Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 320) + "...";
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    list.Add(Board);
                }
            }
            return list;
        }

        //임시저장
        public GlossaryTempType GlossaryTempInsert(GlossaryTempType Board)
        {
            GlossaryTempDac Dac = new GlossaryTempDac();
            DataSet ds = Dac.GlossaryTempInsert(Board);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                }
            }
            return Board;
        }

        //임시저장 Select
        public GlossaryTempType GlossaryTempSelect(string ID)
        {
            GlossaryTempDac Dac = new GlossaryTempDac();
            DataSet ds = Dac.GlossaryTempSelect(ID);
            GlossaryTempType Board = new GlossaryTempType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.DocumentKind = dr["DocumentKind"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.Description = dr["Description"].ToString();
                    Board.CreateDate = dr["CreateDate"].ToString();
                    // 2014-05-28 Mr.No 추가
                    //Board.CategoryID = (dr["CategoryID"] == DBNull.Value) ? 0 : dr.Field<int>("CategoryID");
                    Board.CategoryID = (dr["CategoryID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["CategoryID"]);
                    Board.Permissions = (dr["Permissions"] == DBNull.Value) ? String.Empty : dr.Field<string>("Permissions");   // 2014-06-13
                }
            }
            return Board;
        }
    }
}
