using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Common;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
   public class GlossaryTestQnABiz
    {
        //QnA 목록 리스트
       public ArrayList GlossaryTestQnAList(int PageNum, int PageSize, out int TotalCount, out int Total, out int SuccessCount, out int UnSuccessCount, string SearchKeyword, string SearchType, string UserID, out int MyQnA)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            Total = 0;
            SuccessCount = 0;
            UnSuccessCount = 0;
            MyQnA = 0;
            GlossaryTestQnADac dac = new GlossaryTestQnADac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryTestQnAList(PageNum, PageSize, SearchKeyword, SearchType, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                SuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "SuccessCount", 0);
                UnSuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "UnSuccessCount", 0);
                Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);
                MyQnA = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "MyQnA", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnAType Board = new GlossaryQnAType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Hits = dr["Hits"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
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

        //QnA Select
        public GlossaryQnAType GlossaryTestQnASelect(string ID)
        {
            GlossaryTestQnADac Dac = new GlossaryTestQnADac();
            DataSet ds = Dac.GlossaryTestQnASelect(ID);
            GlossaryQnAType Board = new GlossaryQnAType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                }
            }
            return Board;
        }

        //QnA 추가
        public GlossaryQnAType GlossaryTestQnAInsert(GlossaryQnAType Board)
        {
            GlossaryTestQnADac Dac = new GlossaryTestQnADac();
            DataSet ds = Dac.GlossaryTestQnAInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }

        public void GlossaryQnAUpdate(GlossaryQnAType Board, string userid, string usermachinename, string userip)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnAUpdate(Board, userid, usermachinename, userip);
        }

        //QnA 삭제
        public void GlossaryQnADelete(string ID, string UserID, string userIp, string userMachineName)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnADelete(ID, UserID, userIp, userMachineName);
        }

    }
}
