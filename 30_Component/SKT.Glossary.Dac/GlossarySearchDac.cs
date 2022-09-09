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
    public class GlossarySearchDac
    {
        private const string connectionStringName = "ConnGlossary";

        //공유 리스트
        public DataSet SearchGlossarySyncDataSelect(string BoardType, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SearchGlossarySyncData_Select");
            db.AddInParameter(cmd, "BoardType", DbType.String, BoardType);
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet SearchQnASyncDataSelect(string BoardType, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SearchQnASyncData_Select");
            db.AddInParameter(cmd, "BoardType", DbType.String, BoardType);
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);

            return db.ExecuteDataSet(cmd);
        }
        
    }
}
