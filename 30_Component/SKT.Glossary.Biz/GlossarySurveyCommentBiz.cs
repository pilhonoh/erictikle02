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
    public class GlossarySurveyCommentBiz
    {
        //댓글 베스트 목록 리스트
        public ArrayList GlossarySurveyBestCommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossarySurveyCommentDac dac = new GlossarySurveyCommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossarySurveyBestCommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossarySurveyCommentType Board = new GlossarySurveyCommentType();
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

        //댓글 목록 리스트(엑셀다운로드) 
        public DataSet  GlossarySurveyCommentExcel(string CommonID)
        {
            GlossarySurveyCommentDac dac = new GlossarySurveyCommentDac();
            DataSet ds = dac.GlossarySurveyCommentList(CommonID);
            return ds; 

        }

        //댓글 목록 리스트
        public ArrayList GlossarySurveyCommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossarySurveyCommentDac dac = new GlossarySurveyCommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossarySurveyCommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossarySurveyCommentType Board = new GlossarySurveyCommentType();

                    Board.ID= dr["CommentID"].ToString();
                    Board.CommentID = dr["CommentID"].ToString();
                    Board.QstID = dr["QST_ID"].ToString(); 

                    Board.Contents = dr["Contents"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();

                    Board.PhotoUrl = dr["PhotoUrl"].ToString();
                    if (string.IsNullOrEmpty(dr["PhotoUrl"].ToString()))
                        Board.PhotoUrl = "/Common/images/user_none.png";

                    //Board.UserIP = dr["UserIP"].ToString();
                    //Board.UserMarchineName = dr["UserMarchineName"].ToString();

                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();


                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    
                    //Board.LastModifyDate =  dr["LastModifyDate"] == null  ?   ""   : Convert.ToDateTime(dr["LastModifyDate"]).ToString("yyyy-MM-dd");


                    //DB필드오타로 추정 
                    Board.BestReplYN = "N"; //  dr["BestReplyYN"].ToString();
            
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

        //댓글 Select
        public GlossarySurveyCommentType GlossarySurveyCommentSelect(string ID)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyCommentSelect(ID);
            GlossarySurveyCommentType Board = new GlossarySurveyCommentType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["QST_ID"].ToString();
                    Board.CommonID = dr["CommentID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                }
            }
            return Board;
        }

        //댓글  추가
        public GlossarySurveyCommentType GlossarySurveyCommentInsert(GlossarySurveyCommentType Board)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyCommentInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();          
            return Board;
        }

        // 댓글  삭제
        public void GlossarySurveyCommentDelete(string ID)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyCommentDelete(ID);
        }

        // 댓글  업데이트
        public GlossarySurveyCommentType GlossarySurveyCommentUpdate(GlossarySurveyCommentType Board)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyCommentUpdate(Board);

            return Board;
        }

        //댓글  추가
        public GlossarySurveyCommentType GlossarySurveyCommentLikeY(GlossarySurveyCommentType Board)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyCommentLikeY(Board);
            Board.LikeY = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }


        // 베스트 댓글  추가
        public GlossarySurveyCommentType GlossarySurveyBestSuccessComment(GlossarySurveyCommentType Board)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            DataSet ds = Dac.GlossarySurveyBestSuccessComment(Board);
            return Board;
        }

        // 2014-07-09 Mr.No
        public string CommentBest_Check(string CommonID)
        {
            GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
            string BestReplyYN = Dac.CommentBest_Check(CommonID);
            return BestReplyYN;
        }
        
    }
}
 
