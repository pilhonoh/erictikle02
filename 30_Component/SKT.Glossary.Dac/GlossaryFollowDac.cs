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
   public class GlossaryFollowDac
    {
       private const string connectionStringName = "ConnGlossary";

        //팔로우 리스트
       public DataSet GlossaryFollowList(string UserID, int PageNum, int PageSize, string ReaderUserID, string SearchType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Follow_List");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "ReaderUserID", DbType.String, ReaderUserID);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            return db.ExecuteDataSet(cmd);
        }

        //팔로우 뷰
        public DataSet GlossaryFollowSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_FollowUser_List");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        //팔로우 추가
        public DataSet GlossaryFollowInsert(string UserID, string ReaderUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Follow_Insert");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "ReaderUserID", DbType.String, ReaderUserID);
            return db.ExecuteDataSet(cmd);
        }

        //팔로우 삭제
        public DataSet GlossaryFollowDelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Follow_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }
    }
}
