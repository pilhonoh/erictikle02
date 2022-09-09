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
    public class GlossaryCommentBiz
    {
        //댓글 목록 리스트
        public ArrayList GlossaryCommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            GlossaryCommentDac dac = new GlossaryCommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryCommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryCommentType Board = new GlossaryCommentType();
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.PhotoUrl = dr["PhotoUrl"].ToString();
                    if (string.IsNullOrEmpty(dr["PhotoUrl"].ToString()))
                        Board.PhotoUrl = "/Common/images/user_none.png";
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");   // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }

                    list.Add(Board);
                }
            }
            return list;
        }

        //댓글 Select
        public GlossaryCommentType GlossaryCommentSelect(string ID)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            DataSet ds = Dac.GlossaryCommentSelect(ID);
            GlossaryCommentType Board = new GlossaryCommentType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                }
            }
            return Board;
        }

        //QnA Select
        public GlossaryQnAType GlossaryQnASelect(string ID, int Count = 0)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnASelect(ID, Count);
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
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.CommonID = dr["CommonID"].ToString();
                }
            }
            return Board;
        }

        //댓글  추가
        public GlossaryCommentType GlossaryCommentInsert(GlossaryCommentType Board)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            DataSet ds = Dac.GlossaryCommentInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();          
            return Board;
        }

        //댓글 likey
        public GlossaryCommentType GlossaryCommentLikeY(GlossaryCommentType Board)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            DataSet ds = Dac.GlossaryCommentLikeY(Board);
            Board.LikeY = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }

        //댓글  삭제
        /*
        public void GlossaryCommentDelete(string ID)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            DataSet ds = Dac.GlossaryCommentDelete(ID);
        }
        */
        public void GlossaryCommentDelete(GlossaryCommentType gcType)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            Dac.GlossaryCommentDelete(gcType);
        }

        //댓글  업데이트
        public GlossaryCommentType GlossaryCommentUpdate(GlossaryCommentType Board)
        {
            GlossaryCommentDac Dac = new GlossaryCommentDac();
            DataSet ds = Dac.GlossaryCommentUpdate(Board);

            return Board;
        }

    }
}

