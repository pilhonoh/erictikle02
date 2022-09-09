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
    public class GlossaryMyPeopleScrapDac
    {
        private const string connectionStringName = "ConnGlossary";

        //공유 리스트
        public DataSet GlossaryMyPeopleScrapList(int PageNum, int PageSize, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_MyPeopleScrap_List");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "PageNum", DbType.String, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.String, PageSize);

            return db.ExecuteDataSet(cmd);
        }

        //스크랩 삭제
        public DataSet GlossaryMyPeopleScrapDelete(string ScrapID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_MyPeopleScrap_Delete");
            db.AddInParameter(cmd, "ScrapID", DbType.String, ScrapID);
            return db.ExecuteDataSet(cmd);
        }
    }
}
