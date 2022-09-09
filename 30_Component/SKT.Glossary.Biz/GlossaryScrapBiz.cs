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
   public class GlossaryScrapBiz
    {
        //스크랩 목록 리스트
        public ArrayList GlossaryScrapList(int PageNum, int PageSize, out int TotalCount, string UserID)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryScrapDac dac = new GlossaryScrapDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryScrapList(PageNum, PageSize, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryScrapType Board = new GlossaryScrapType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.YouUserID = dr["YouUserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    // 2014-05-08 Mr.No 수정
                    Board.LastCreateDate = (dr["LastCreateDate"] == DBNull.Value) ? string.Empty : dr.Field<DateTime>("LastCreateDate").ToString("yyyy-MM-dd");
                    Board.FirstCreateDate = (dr["FirstCreateDate"] == DBNull.Value) ? Board.LastCreateDate : dr.Field<DateTime>("FirstCreateDate").ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
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
                    if (dr.Table.Columns.Contains("UserGrade") == true)
                    {
                        Board.UserGrade = (dr["UserGrade"] == DBNull.Value) ? 0 : dr.Field<int>("UserGrade");
                    }
                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("UserGrade"))
                    {
                        if (Board.UserGrade == 0) { Board.Rank = "지존"; }
                        else if (Board.UserGrade == 1) { Board.Rank = "고수"; }
                        else if (Board.UserGrade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }

                    list.Add(Board);
                }
            }
            return list;
        }

        //스크랩 Select
        public GlossaryScrapType GlossaryScrapSelect(string ID)
        {
            GlossaryScrapDac Dac = new GlossaryScrapDac();
            DataSet ds = Dac.GlossaryScrapSelect(ID);
            GlossaryScrapType Board = new GlossaryScrapType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.CreateDate = dr["CreateDate"].ToString();
                }
            }
            return Board;
        }

        //스크랩 추가
        public void GlossaryScrapInsert(GlossaryScrapType Board)
        {
            GlossaryScrapDac Dac = new GlossaryScrapDac();
            DataSet ds = Dac.GlossaryScrapInsert(Board);
        }

        //스크랩 삭제
        public void GlossaryScrapDelete(string ID)
        {
            GlossaryScrapDac Dac = new GlossaryScrapDac();
            DataSet ds = Dac.GlossaryScrapDelete(ID);
        }

    }
}
