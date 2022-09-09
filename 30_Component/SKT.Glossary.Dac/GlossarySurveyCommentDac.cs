using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
    public class GlossarySurveyCommentDac
    {

        private const string connectionStringName = "ConnGlossary";

        //베스트 댓글 리스트
        public DataSet GlossarySurveyBestCommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyBestComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 리스트
        public DataSet GlossarySurveyCommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 뷰
        public DataSet GlossarySurveyCommentSelect(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyComment_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 추가
        public DataSet GlossarySurveyCommentInsert(GlossarySurveyCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyComment_Insert");
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "PhotoUrl", DbType.String, Board.PhotoUrl);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "LikeCount", DbType.String, Board.LikeCount);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "PublicYN", DbType.String, Board.PublicYN);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 수정
        public DataSet GlossarySurveyCommentUpdate(GlossarySurveyCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyComment_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            //db.AddInParameter(cmd, "PhotoUrl", DbType.String, Board.PhotoUrl);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            //db.AddInParameter(cmd, "PublicYN", DbType.String, Board.PublicYN);
            //db.AddInParameter(cmd, "LikeCount", DbType.String, Board.LikeCount);
            //db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            //db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            //db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            //db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            return db.ExecuteDataSet(cmd);
        }


        //댓글 삭제
        public DataSet GlossarySurveyCommentDelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //추천 추가
        public DataSet GlossarySurveyCommentLikeY(GlossarySurveyCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentLike_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "LikeY", DbType.String, "Y");
            return db.ExecuteDataSet(cmd);
        }

        //베스트 댓글
        public DataSet GlossarySurveyBestSuccessComment(GlossarySurveyCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentBest_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //베스트 댓글 select 
        public DataSet GlossarySurveyCommentSuccessSelect(string ItemID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentSuccess_Select");
            db.AddInParameter(cmd, "ItemID", DbType.String, ItemID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 쪽지 YN select 
        public DataSet NoteQnaYNSelect(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyNoteQnaYNSelect_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }


        public string CommentBest_Check(string CommonID)
        {
            string BestReplyYN = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentBest_Check");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);

            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BestReplyYN = (dr["BestReplyYN"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["BestReplyYN"]);
                    }
                }
            }

            return BestReplyYN;
        }
    }
}
