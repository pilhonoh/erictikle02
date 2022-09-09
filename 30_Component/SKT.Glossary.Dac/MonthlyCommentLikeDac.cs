using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
	/// <summary>
	/// 설명: Data access class for tb_WeeklyCommentLike table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public sealed class MonthlyCommentLikeDac
	{
		private const string connectionStringName = "ConnGlossary";

        public MonthlyCommentLikeDac() { }

		/// <summary>
		/// Inserts a record into the tb_WeeklyCommentLike table.
		/// </summary>
		/// <returns></returns>
		public int WeeklyCommentLikeInsert(MonthlyCommentLikeType weeklyCommentLikeType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyCommentLike_Insert");

			db.AddInParameter(dbCommand, "CommentID", DbType.Int64, weeklyCommentLikeType.CommentID);
			db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyCommentLikeType.UserID);
			db.AddInParameter(dbCommand, "UserName", DbType.String, weeklyCommentLikeType.UserName);
			db.AddInParameter(dbCommand, "UserIP", DbType.String, weeklyCommentLikeType.UserIP);
			db.AddInParameter(dbCommand, "UserMachineName", DbType.String, weeklyCommentLikeType.UserMachineName);
			db.AddInParameter(dbCommand, "LikeY", DbType.String, weeklyCommentLikeType.LikeY);
			db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, weeklyCommentLikeType.CreateDateTime);

			// Execute the query and return the new identity value
			int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

			return returnValue;
		}

		/// <summary>
		/// Selects a single record from the tb_WeeklyCommentLike table.
		/// </summary>
		/// <returns>DataSet</returns>
		public MonthlyCommentLikeType WeeklyCommentLikeSelect(Int64 iD)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyCommentLike_Select");

			db.AddInParameter(dbCommand, "ID", DbType.Int64, iD);

			MonthlyCommentLikeType weeklyCommentLikeType = null;
			using (DataSet ds = db.ExecuteDataSet(dbCommand))
			{
			    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			    {
			        weeklyCommentLikeType = GetWeeklyCommentLikeTypeMapData(ds.Tables[0].Rows[0]);
			    }
			}
            return weeklyCommentLikeType;
		}

		/// <summary>
		/// Selects all records from the tb_WeeklyCommentLike table.
		/// </summary>
		public List<MonthlyCommentLikeType> WeeklyCommentLikeSelectAll()
		{
            List<MonthlyCommentLikeType> listWeeklyCommentLikeType = new List<MonthlyCommentLikeType>();
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyCommentLike_SelectAll");

			using (DataSet ds = db.ExecuteDataSet(dbCommand))
			{
			    if (ds != null && ds.Tables.Count > 0)
			    {
			        foreach (DataRow dr in ds.Tables[0].Rows)
			        {
			            MonthlyCommentLikeType weeklyCommentLikeType = GetWeeklyCommentLikeTypeMapData(dr);
                        listWeeklyCommentLikeType.Add(weeklyCommentLikeType);
			        }
			    }
			}
			return listWeeklyCommentLikeType;
		}

		/// <summary>
		/// Updates a record in the tb_WeeklyCommentLike table.
		/// </summary>
		public void WeeklyCommentLikeUpdate(WeeklyCommentLikeType weeklyCommentLikeType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyCommentLike_Update");

			db.AddInParameter(dbCommand, "ID", DbType.Int64, weeklyCommentLikeType.ID);
			db.AddInParameter(dbCommand, "CommentID", DbType.Int64, weeklyCommentLikeType.CommentID);
			db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyCommentLikeType.UserID);
			db.AddInParameter(dbCommand, "UserName", DbType.String, weeklyCommentLikeType.UserName);
			db.AddInParameter(dbCommand, "UserIP", DbType.String, weeklyCommentLikeType.UserIP);
			db.AddInParameter(dbCommand, "UserMachineName", DbType.String, weeklyCommentLikeType.UserMachineName);
			db.AddInParameter(dbCommand, "LikeY", DbType.String, weeklyCommentLikeType.LikeY);
			db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, weeklyCommentLikeType.CreateDateTime);

			db.ExecuteNonQuery(dbCommand);
		}

		/// <summary>
		/// Deletes a record from the tb_WeeklyCommentLike table by a composite primary key.
		/// </summary>
		public void WeeklyCommentLikeDelete(Int64 iD)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyCommentLike_Delete");

			db.AddInParameter(dbCommand, "ID", DbType.Int64, iD);

			db.ExecuteNonQuery(dbCommand);
		}

		/// <summary>
		/// Creates a new instance of the WeeklyCommentLikeType class and populates it with data from the specified DataRow.
		/// </summary>
		private MonthlyCommentLikeType GetWeeklyCommentLikeTypeMapData(DataRow dr)
		{
            MonthlyCommentLikeType weeklyCommentLikeType = new MonthlyCommentLikeType();
			weeklyCommentLikeType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
			weeklyCommentLikeType.CommentID = (dr["CommentID"] == DBNull.Value) ? 0 : dr.Field<long>("CommentID");
			weeklyCommentLikeType.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
			weeklyCommentLikeType.UserName = (dr["UserName"] == DBNull.Value) ? null : dr.Field<string>("UserName");
			weeklyCommentLikeType.UserIP = (dr["UserIP"] == DBNull.Value) ? null : dr.Field<string>("UserIP");
			weeklyCommentLikeType.UserMachineName = (dr["UserMachineName"] == DBNull.Value) ? null : dr.Field<string>("UserMachineName");
			weeklyCommentLikeType.LikeY = (dr["LikeY"] == DBNull.Value) ? String.Empty : dr.Field<string>("LikeY");
			weeklyCommentLikeType.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreateDateTime");

			return weeklyCommentLikeType;
		}
	}
}
