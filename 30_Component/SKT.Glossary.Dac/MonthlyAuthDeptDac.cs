using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_WeeklyAuthDept table.
    /// 작성일 : 2015-03-04
    /// 작성자 : miksystem.com
    /// </summary>
    public class MonthlyAuthDeptDac
	{
		private const string connectionStringName = "ConnGlossary";

        public MonthlyAuthDeptDac() { }

        /// <summary>
        /// Deletes a record from the tb_WeeklyAuthDept table by a composite primary key.
        /// </summary>
        public void WeeklyAuthDeptDelete(string deptName, string deptCode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyAuthDept_Delete");

            db.AddInParameter(dbCommand, "DeptName", DbType.String, deptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);

            db.ExecuteNonQuery(dbCommand);
        }

		/// <summary>
        /// Inserts a record into the tb_WeeklyAuthDept table.
		/// </summary>
		/// <returns></returns>
        public int WeeklyLikeInsert(WeeklyAuthDeptType weeklyAuthDeptType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyAuthDept_Insert");

            db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyAuthDeptType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyAuthDeptType.DeptCode);
            db.AddInParameter(dbCommand, "AuthDeptCode", DbType.String, weeklyAuthDeptType.AuthDeptCode);
            db.AddInParameter(dbCommand, "TopAuthDeptCode", DbType.String, weeklyAuthDeptType.TopAuthDeptCode);
			
			// Execute the query and return the new identity value
			int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

			return returnValue;
		}

        /// <summary>
        /// Selects single string from the tb_WeeklyAuthDept table.
        /// </summary>
        public string WeeklyAuthDeptSelect(string deptName, string deptCode, string chiefYN)
        {
            string returnString = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyAuthDept_Select");

            db.AddInParameter(dbCommand, "DeptName", DbType.String, deptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "ChiefYN", DbType.String, chiefYN);

            returnString = db.ExecuteScalar(dbCommand).ToString();

            return returnString;
        }

		/// <summary>
        /// Selects all records from the tb_WeeklyAuthDept table.
		/// </summary>
		public List<WeeklyAuthDeptType> WeeklyAuthDeptSelectAll(string deptName, string deptCode)
		{
            List<WeeklyAuthDeptType> listWeeklyAuthDeptType = new List<WeeklyAuthDeptType>();
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyAuthDept_SelectAll");

            db.AddInParameter(dbCommand, "DeptName", DbType.String, deptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);

			using (DataSet ds = db.ExecuteDataSet(dbCommand))
			{
			    if (ds != null && ds.Tables.Count > 0)
			    {
			        foreach (DataRow dr in ds.Tables[0].Rows)
			        {
                        WeeklyAuthDeptType weeklyAuthDeptType = GetWeeklyAuthDeptTypeMapData(dr);
                        listWeeklyAuthDeptType.Add(weeklyAuthDeptType);
			        }
			    }
			}
            return listWeeklyAuthDeptType;
		}

        /// <summary>
        /// Updates a record in the tb_WeeklyAuthDept table.
        /// </summary>
        public void WeeklyAuthDeptUpdate(WeeklyAuthDeptType weeklyAuthDeptType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyAuthDept_Update");

            db.AddInParameter(dbCommand, "WeeklyAuthDeptID", DbType.Int64, weeklyAuthDeptType.WeeklyAuthDeptID);
            db.AddInParameter(dbCommand, "DeptName", DbType.Int64, weeklyAuthDeptType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyAuthDeptType.DeptCode);
            db.AddInParameter(dbCommand, "AuthDeptCode", DbType.String, weeklyAuthDeptType.AuthDeptCode);
            db.AddInParameter(dbCommand, "TopAuthDeptCode", DbType.String, weeklyAuthDeptType.TopAuthDeptCode);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 임원일 경우 상위 권한이 있는지 체크합니다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public List<WeeklyAuthDeptType> Weekly_Select_AuthDept_DirectOrg(string deptCode)
        {
            //Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            //DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_AuthDept_DirectOrg");

            //db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);

            //return db.ExecuteDataSet(dbCommand);

            List<WeeklyAuthDeptType> listWeeklyAuthDeptType = new List<WeeklyAuthDeptType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_AuthDept_DirectOrg");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyAuthDeptType weeklyAuthDeptType = GetWeeklyAuthDeptTypeMapData(dr);
                        listWeeklyAuthDeptType.Add(weeklyAuthDeptType);
                    }
                }
            }
            return listWeeklyAuthDeptType;
        }

		/// <summary>
        /// Creates a new instance of the tb_WeeklyAuthDeptType class and populates it with data from the specified DataRow.
		/// </summary>
        private WeeklyAuthDeptType GetWeeklyAuthDeptTypeMapData(DataRow dr)
		{
            WeeklyAuthDeptType weeklyAuthDeptType = new WeeklyAuthDeptType();
            weeklyAuthDeptType.WeeklyAuthDeptID = (dr["WeeklyAuthDeptID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyAuthDeptID");
            weeklyAuthDeptType.DeptName = (dr["DeptName"] == DBNull.Value) ? string.Empty : dr.Field<string>("DeptName");
            weeklyAuthDeptType.DeptCode = (dr["DeptCode"] == DBNull.Value) ? string.Empty : dr.Field<string>("DeptCode");
            weeklyAuthDeptType.AuthDeptCode = (dr["AuthDeptCode"] == DBNull.Value) ? string.Empty : dr.Field<string>("AuthDeptCode");
            if (dr.Table.Columns.Contains("TopAuthDeptCode"))
            {
                weeklyAuthDeptType.TopAuthDeptCode = (dr["TopAuthDeptCode"] == DBNull.Value) ? string.Empty : dr.Field<string>("TopAuthDeptCode");
            }            

            return weeklyAuthDeptType;
		}
	}
}
