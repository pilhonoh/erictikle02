using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
	/// <summary>
	/// 설명: Data access class for tb_WeeklyComment table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public sealed class MonthlyCommentDac
	{
		private const string connectionStringName = "ConnGlossary";

        public MonthlyCommentDac() { }

        /// <summary>
        /// Deletes a record from the tb_WeeklyComment table by a composite primary key.
        /// </summary>
        public string WeeklyCommentDelete(Int64 weeklyCommentID, Int64? weeklyID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_Delete");

            db.AddInParameter(dbCommand, "WeeklyCommentID", DbType.Int64, weeklyCommentID);
            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

            //db.ExecuteNonQuery(dbCommand);
            return Convert.ToString(db.ExecuteScalar(dbCommand));
        }

		/// <summary>
		/// Inserts a record into the tb_WeeklyComment table.
		/// </summary>
		/// <returns></returns>
		public string WeeklyCommentInsert(WeeklyCommentType weeklyCommentType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_Insert");

			db.AddInParameter(dbCommand, "SUP_ID", DbType.Int64, weeklyCommentType.SUP_ID);
			db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyCommentType.WeeklyID);
			db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyCommentType.Contents);
			db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyCommentType.UserID);
			db.AddInParameter(dbCommand, "UserName", DbType.String, weeklyCommentType.UserName);
            db.AddInParameter(dbCommand, "DutyName", DbType.String, weeklyCommentType.DutyName);
			db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyCommentType.DeptName);

			// Execute the query and return the new identity value
            return db.ExecuteScalar(dbCommand).ToString();
		}

		/// <summary>
		/// Selects a single record from the tb_WeeklyComment table.
		/// </summary>
		/// <returns>DataSet</returns>
		public DataSet WeeklyCommentSelect(Int64 weeklyCommentID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_Select");

            db.AddInParameter(dbCommand, "WeeklyCommentID", DbType.Int64, weeklyCommentID);

			return db.ExecuteDataSet(dbCommand);
		}

		/// <summary>
		/// Selects all records from the tb_WeeklyComment table.
		/// </summary>
		public DataSet WeeklyCommentSelectAll(Int64 weeklyID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_SelectAll_Weekly");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

			return db.ExecuteDataSet(dbCommand);
		}

        public DataSet WeeklyComment_List_New(Int64 weeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_List_New");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyID);

            return db.ExecuteDataSet(dbCommand);
        }

		/// <summary>
		/// Updates a record in the tb_WeeklyComment table.
		/// </summary>
		public void WeeklyCommentUpdate(WeeklyCommentType weeklyCommentType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyComment_Update");

            db.AddInParameter(dbCommand, "WeeklyCommentID", DbType.Int64, weeklyCommentType.WeeklyCommentID);
			
            //db.AddInParameter(dbCommand, "SUP_ID", DbType.Int64, weeklyCommentType.SUP_ID);
            //db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyCommentType.WeeklyID);

			db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyCommentType.Contents);
			
            //db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyCommentType.UserID);
            //db.AddInParameter(dbCommand, "UserName", DbType.String, weeklyCommentType.UserName);
            //db.AddInParameter(dbCommand, "DutyName", DbType.String, weeklyCommentType.DutyName);
            //db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyCommentType.DeptName);

            //db.AddInParameter(dbCommand, "CreateDateTime", DbType.DateTime, weeklyCommentType.CreateDateTime);
            //db.AddInParameter(dbCommand, "UpdateDateTime", DbType.DateTime, weeklyCommentType.UpdateDateTime);

			db.ExecuteNonQuery(dbCommand);
		}
	}
}
