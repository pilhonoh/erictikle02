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
    public class GlossaryTestQnACommentBiz
    {
        //QnA 댓글 목록 리스트
        public ArrayList GlossaryTestQnACommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryTestQnACommentDac dac = new GlossaryTestQnACommentDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryTestQnACommentList(CommonID);

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
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    if (Board.Grade == 0) { Board.Rank = "지존"; }
                    else if (Board.Grade == 1) { Board.Rank = "고수"; }
                    else if (Board.Grade == 2) { Board.Rank = "중수"; }
                    else { Board.Rank = "초수"; }
                    list.Add(Board);
                }
            }
            return list;
        }

        //QnA 댓글 Select
        public GlossaryQnACommentType GlossaryTestQnACommentSelect(string ID)
        {
            GlossaryTestQnACommentDac Dac = new GlossaryTestQnACommentDac();
            DataSet ds = Dac.GlossaryTestQnACommentSelect(ID);
            GlossaryQnACommentType Board = new GlossaryQnACommentType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                }
            }
            return Board;
        }

        //QnA  댓글  추가
        public GlossaryQnACommentType GlossaryTestQnACommentInsert(GlossaryQnACommentType Board)
        {
            GlossaryTestQnACommentDac Dac = new GlossaryTestQnACommentDac();
            DataSet ds = Dac.GlossaryTestQnACommentInsert(Board);
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
            DataSet ds = Dac.GlossaryQnACommentInsert(Board);

            return Board;
        }

        //QnA  잫아요  추가
        public GlossaryQnACommentType GlossaryQnACommentLikeY(GlossaryQnACommentType Board)
        {
            GlossaryQnACommentDac Dac = new GlossaryQnACommentDac();
            DataSet ds = Dac.GlossaryQnACommentLikeY(Board);
            Board.LikeY = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }

    }
}

