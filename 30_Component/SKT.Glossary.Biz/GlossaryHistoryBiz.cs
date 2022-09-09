using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using System.Data;
using SKT.Glossary.Dac;
using System.Collections;
using SKT.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryHistoryBiz
    {
        //히스토리 리스트
        public ArrayList GlossaryHistoryList(int PageNum, int PageSize, out int TotalCount, string ID)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryHistoryDac dac = new GlossaryHistoryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryHistoryList(PageNum, PageSize, ID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.UserID = dr["UserID"].ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Description = dr["Description"].ToString();
                    Board.HistoryYN = dr["HistoryYN"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.Grade = Convert.ToInt32(dr["Grade"]);
                    if (Board.Grade == 0) { Board.Rank = "지존"; }
                    else if (Board.Grade == 1) { Board.Rank = "고수"; }
                    else if (Board.Grade == 2) { Board.Rank = "중수"; }
                    else { Board.Rank = "초수"; }
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    list.Add(Board);
                }
            }
            return list;
        }

        //히스토리 보기
        public GlossaryHistoryType GlossaryHistorySelect(string ID, string CommonID)
        {
            GlossaryHistoryDac Dac = new GlossaryHistoryDac();
            DataSet ds = Dac.GlossaryHistorySelect(ID, CommonID);
            GlossaryHistoryType Board = new GlossaryHistoryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.RootTitle = dr["RootTitle"].ToString();
                    Board.RootPrivateYN = dr["RootPrivateYN"].ToString();
                    Board.RootSummary = dr["RootSummary"].ToString();
                    Board.RootContentsModify = dr["RootContentsModify"].ToString();
                    Board.RootBoardUserID = dr["RootBoardUserID"].ToString();
                    Board.RootBoardUserName = dr["RootBoardUserName"].ToString();
                    Board.RootBoardDeptName = dr["RootBoardDeptName"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");

                }
            }
            return Board;
        }
    }
}