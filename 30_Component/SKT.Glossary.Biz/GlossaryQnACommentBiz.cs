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
    public class GlossaryQnACommentBiz
    {
        //QnA 댓글 베스트 목록 리스트
        public ArrayList GlossaryQnABestCommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryQnACommentDac dac = new GlossaryQnACommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnABestCommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnACommentType Board = new GlossaryQnACommentType();
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
                    Board.BestReplYN = dr["BestReplyYN"].ToString();
                    list.Add(Board);
                }
            }
            return list;
        }

        //QnA 댓글 목록 리스트
        public ArrayList GlossaryQnACommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryQnACommentDac dac = new GlossaryQnACommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnACommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnACommentType Board = new GlossaryQnACommentType();
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
                    Board.BestReplYN = dr["BestReplyYN"].ToString();
                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["Grade"]);
                    }

                    if (dr.Table.Columns.Contains("Rank"))
                    {
                        Board.Rank = (dr["Rank"] == DBNull.Value) ? string.Empty : dr["Rank"].ToString();
                    }
                    list.Add(Board);
                }
            }
            return list;
        }

        //QnA 댓글 Select
        public GlossaryQnACommentType GlossaryQnACommentSelect(string ID)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentSelect(ID);
            GlossaryQnACommentType Board = new GlossaryQnACommentType();

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

        //QnA  댓글  추가
        public GlossaryQnACommentType GlossaryQnACommentInsert(GlossaryQnACommentType Board)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();          
            return Board;
        }

        //QnA  댓글  삭제
        public void GlossaryQnACommentDelete(string ID)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentDelete(ID);
        }

        //QnA  댓글  업데이트
        public GlossaryQnACommentType GlossaryQnACommentUpdate(GlossaryQnACommentType Board)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentUpdate(Board);

            return Board;
        }

        //QnA  댓글  추가
        public GlossaryQnACommentType GlossaryQnACommentLikeY(GlossaryQnACommentType Board)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentLikeY(Board);
            Board.LikeY = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }


        //QnA  베스트 댓글  추가
        public GlossaryQnACommentType GlossaryQnABestSuccessComment(GlossaryQnACommentType Board)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnABestSuccessComment(Board);
            return Board;
        }

        // 2014-07-09 Mr.No
        public string CommentBest_Check(string CommonID)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            string BestReplyYN = Dac.CommentBest_Check(CommonID);
            return BestReplyYN;
        }
        
    }
}

