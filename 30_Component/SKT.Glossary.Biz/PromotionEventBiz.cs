using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Biz
{
	public class PromotionEventBiz
	{
		/*
		 * 이벤트 데이터 셋팅 및 조회
		 */
		public DataSet PromotionEventSelect(string UserID, string EVT_Type)
		{
			string connectionStringName = "ConnGlossary";
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_OpenEvent20_Select");

			db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
			db.AddInParameter(dbCommand, "EVT_Type", DbType.String, EVT_Type);

			return db.ExecuteDataSet(dbCommand);
		}

		/*
		 * 개인프로필 변경 데이터 셋팅 및 조회
		 */
		public DataSet PromotionEvent_ProfileUpdate(string UserID, string EVT_Type)
		{
			string connectionStringName = "ConnGlossary";
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_OpenEvent20_ProfileUpdate");

			db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
			db.AddInParameter(dbCommand, "EVT_Type", DbType.String, EVT_Type);

			return db.ExecuteDataSet(dbCommand);
		}

	}
}
