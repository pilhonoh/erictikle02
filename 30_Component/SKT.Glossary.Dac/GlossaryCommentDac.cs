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
   public class GlossaryCommentDac
    {
        private const string connectionStringName = "ConnGlossary";
        
       //댓글 리스트
        public DataSet GlossaryCommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 뷰
        public DataSet GlossaryCommentSelect(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 추가
        public DataSet GlossaryCommentInsert(GlossaryCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_Insert");
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "PhotoUrl", DbType.String, Board.PhotoUrl);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "LikeCount", DbType.String, Board.LikeCount);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "UserIP", DbType.String, Board.UserIP);
            db.AddInParameter(cmd, "UserMachineName", DbType.String, Board.UserMachineName);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "PublicYN", DbType.String, Board.PublicYN);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 수정
        public DataSet GlossaryCommentUpdate(GlossaryCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "LastModifierID", DbType.String, Board.LastModifierID);
            db.AddInParameter(cmd, "LastModifierIP", DbType.String, Board.LastModifierIP);
            db.AddInParameter(cmd, "LastModifierMachineName", DbType.String, Board.LastModifierMachineName);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 삭제
       /*
        public DataSet GlossaryCommentDelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }
        */
        public void GlossaryCommentDelete(GlossaryCommentType gcType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, gcType.ID);
            db.AddInParameter(cmd, "LastModifierID", DbType.String, gcType.LastModifierID);
            db.AddInParameter(cmd, "LastModifierIP", DbType.String, gcType.LastModifierIP);
            db.AddInParameter(cmd, "LastModifierMachineName", DbType.String, gcType.LastModifierMachineName);
            db.ExecuteNonQuery(cmd);
        }


        //댓글 추천 추가
        public DataSet GlossaryCommentLikeY(GlossaryCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryCommentLike_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "UserIP", DbType.String, Board.UserIP);
            db.AddInParameter(cmd, "UserMachineName", DbType.String, Board.UserMachineName);
            db.AddInParameter(cmd, "LikeY", DbType.String, "Y");
            
            return db.ExecuteDataSet(cmd);
        }

    }
}
