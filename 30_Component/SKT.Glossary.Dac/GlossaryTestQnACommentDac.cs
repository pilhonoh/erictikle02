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
   public class GlossaryTestQnACommentDac
    {
        private const string connectionStringName = "ConnGlossary";

        //QnA 댓글 리스트
        public DataSet GlossaryTestQnACommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnAComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 댓글 뷰
        public DataSet GlossaryTestQnACommentSelect(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnAComment_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 댓글 추가
        public DataSet GlossaryTestQnACommentInsert(GlossaryQnACommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnAComment_Insert");
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "PhotoUrl", DbType.String, Board.PhotoUrl);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "LikeCount", DbType.String, Board.LikeCount);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 댓글 삭제
        public DataSet GlossaryTestQnACommentDelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnAComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }
    }
}
