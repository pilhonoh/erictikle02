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
   public class GlossaryTestQnADac
    {
        private const string connectionStringName = "ConnGlossary";

        //QnA 리스트
        public DataSet GlossaryTestQnAList(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnA_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 뷰
        public DataSet  GlossaryTestQnASelect(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnA_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 추가
        public DataSet  GlossaryTestQnAInsert(GlossaryQnAType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnA_Insert");
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
            db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            return db.ExecuteDataSet(cmd);
        }
        //QnA 추가
        public DataSet GlossaryTestQnAUpdate(GlossaryQnAType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnA_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
            db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 삭제
        public DataSet  GlossaryTestQnADelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TestQnA_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }
    }
}

