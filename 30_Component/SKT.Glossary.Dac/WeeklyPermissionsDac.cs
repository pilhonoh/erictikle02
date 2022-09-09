using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
	/// <summary>
	/// 설명: Data access class for tb_WeeklyPermissions table.
	/// 작성일 : 2015-04-12
	/// 작성자 : miksystem.com
	/// </summary>
	public sealed class WeeklyPermissionsDac
	{
        private const string connectionStringName = "ConnGlossary";

		public WeeklyPermissionsDac() {}

		/// <summary>
		/// Inserts a record into the tb_WeeklyPermissions table.
		/// </summary>
		/// <returns></returns>
		public void WeeklyPermissionsInsert(WeeklyPermissionsType weeklyPermissionsType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyPermissions_Insert");

			db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyPermissionsType.WeeklyID);
			db.AddInParameter(dbCommand, "ToUserID", DbType.String, weeklyPermissionsType.ToUserID);
			db.AddInParameter(dbCommand, "ToUserName", DbType.String, weeklyPermissionsType.ToUserName);

			db.ExecuteNonQuery(dbCommand);
		}

		/// <summary>
		/// Selects a single record from the tb_WeeklyPermissions table.
		/// </summary>
		/// <returns>DataSet</returns>
		public WeeklyPermissionsType WeeklyPermissionsSelect(Int64 weeklyID, string toUserID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyPermissions_Select");

			db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);
			db.AddInParameter(dbCommand, "ToUserID", DbType.String, toUserID);

			WeeklyPermissionsType weeklyPermissionsType = null;
			using (DataSet ds = db.ExecuteDataSet(dbCommand))
			{
			    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			    {
			        weeklyPermissionsType = GetWeeklyPermissionsTypeMapData(ds.Tables[0].Rows[0]);
			    }
			}
			return weeklyPermissionsType;
		}

		/// <summary>
		/// Selects all records from the tb_WeeklyPermissions table.
		/// </summary>
		public List<WeeklyPermissionsType> WeeklyPermissionsList(Int64 weeklyID)
		{
			List<WeeklyPermissionsType> listWeeklyPermissionsType = new List<WeeklyPermissionsType>();
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyPermissions_List");
            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

			using (DataSet ds = db.ExecuteDataSet(dbCommand))
			{
			    if (ds != null && ds.Tables.Count > 0)
			    {
			        foreach (DataRow dr in ds.Tables[0].Rows)
			        {
			            WeeklyPermissionsType weeklyPermissionsType = GetWeeklyPermissionsTypeMapData(dr);
			            listWeeklyPermissionsType.Add(weeklyPermissionsType);
			        }
			    }
			}
			return listWeeklyPermissionsType;
		}

        /// <summary>
        /// Selects all records from the tb_WeeklyPermissions table.
        /// </summary>
        public DataSet WeeklyPermissionsView(Int64 weeklyID)
        {
            List<WeeklyPermissionsType> listWeeklyPermissionsType = new List<WeeklyPermissionsType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyPermissions_List");
            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            return ds;
        }

		/// <summary>
		/// Deletes a record from the tb_WeeklyPermissions table by a composite primary key.
		/// </summary>
		public void WeeklyPermissionsDeleteAll(Int64 weeklyID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyPermissions_DeleteAll");

			db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

			db.ExecuteNonQuery(dbCommand);
		}

		/// <summary>
		/// Creates a new instance of the WeeklyPermissionsType class and populates it with data from the specified DataRow.
		/// </summary>
		private WeeklyPermissionsType GetWeeklyPermissionsTypeMapData(DataRow dr)
		{
			WeeklyPermissionsType weeklyPermissionsType = new WeeklyPermissionsType();
			weeklyPermissionsType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
			weeklyPermissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
			weeklyPermissionsType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");

			return weeklyPermissionsType;
		}
	}
}
