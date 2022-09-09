using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SKT.Glossary.Dac
{
    public class GlossaryHistoryDac
    {
        private const string connectionStringName = "ConnGlossary";

        //히스토리 리스트
        public DataSet GlossaryHistoryList(int PageNum, int PageSize, string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_History_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //히스토리 뷰
        public DataSet GlossaryHistorySelect(string ID, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_History_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //히스토리 check in, check out
        public DataSet GlossaryHistoryModifyYN(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_HistoryModifyYN_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }
    }
}