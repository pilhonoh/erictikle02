using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using SKT.Common;
using System.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
   public class GlossaryFollowBiz
    {
        //팔로우 목록 리스트
       public ArrayList GlossaryFollowList(string UserID, int PageNum, int PageSize, out int TotalCount, string ReaderUserID, string SearchType)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryFollowDac dac = new GlossaryFollowDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryFollowList(UserID, PageNum, PageSize, ReaderUserID, SearchType);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                    }
                    Board.HistoryYN = dr["HistoryYN"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    {
                        byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                        try
                        {
                            Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 250) + "...";
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits"))
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount"))
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag"))
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    if (dr.Table.Columns.Contains("LikeCount"))
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade") == true)
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }
                    Board.PrivateYN = (dr["PrivateYN"] == DBNull.Value) ? null : dr.Field<string>("PrivateYN");

                    list.Add(Board);
                }
            }
            return list;
        }

        //팔오우 구독자 Select
        public ArrayList GlossaryFollowSelect(string UserID)
        {
            ArrayList list = new ArrayList();
            GlossaryFollowDac Dac = new GlossaryFollowDac();
            DataSet ds = Dac.GlossaryFollowSelect(UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryFollowType Board = new GlossaryFollowType();
                    Board.ID = dr["ID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.ReaderUserID = dr["ReaderUserID"].ToString();
                    Board.ReaderUserName = dr["ReaderUserName"].ToString();
                    Board.ReaderUserPhoto = dr["ReaderUserPhoto"].ToString();
                    Board.ReaderDeptName = dr["ReaderDeptName"].ToString();
                      Board.CreateDate = dr["CreateDate"].ToString();
                    list.Add(Board);
                }
            }
            return list;
        }      

        //팔로우 추가
        //public void GlossaryFollowInsert(GlossaryFollowType Board)
        //{
        //    GlossaryFollowDac Dac = new GlossaryFollowDac();
        //    DataSet ds = Dac.GlossaryFollowInsert(Board);
        //}

        //팔로우 삭제
        public void GlossaryFollowDelete(string ID)
        {
            GlossaryFollowDac Dac = new GlossaryFollowDac();
            DataSet ds = Dac.GlossaryFollowDelete(ID);
        }

    }
}
