using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SKT.Glossary.Dac
{
    public class GlossaryInterfaceDac
    {
        private const string connectionStringName = "ConnGlossary";

        //히스토리 리스트
        public DataSet TnetTopTotalActivityList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Interface_TnetTopTotalActivity_List");
            return db.ExecuteDataSet(cmd);
        }

        //히스토리 리스트
        public DataSet TnetTopTotalActivityList3()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Interface_TnetTopTotalActivity_List2");
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Mr.No
        /// [PHOTOURL] 추가
        /// </summary>
        /// <returns></returns>
        public DataSet TnetTopTotalActivityList4()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Interface_TnetTopTotalActivity_List4");
            return db.ExecuteDataSet(cmd);
        }
    }
}
