using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Dac
{
    public class MonthlyTeamLeaderNotiCheckDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static MonthlyTeamLeaderNotiCheckDac _instance = null;
        public static MonthlyTeamLeaderNotiCheckDac Instance
        {
            get
            {
                MonthlyTeamLeaderNotiCheckDac obj = _instance;
                if (obj == null)
                {
                    obj = new MonthlyTeamLeaderNotiCheckDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        public MonthlyTeamLeaderNotiCheckDac() { }

        /// <summary>
        /// Inserts a record into the tb_WeeklyTeamLeaderNotiCheck table.
        /// </summary>
        /// <returns></returns>
        public int WeeklyTeamLeaderNotiCheckInsert(MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Insert");

            db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyTeamLeaderNotiCheckType.UserID);
            db.AddInParameter(dbCommand, "UserName", DbType.String, weeklyTeamLeaderNotiCheckType.UserName);
            db.AddInParameter(dbCommand, "TeamMemberYN", DbType.String, weeklyTeamLeaderNotiCheckType.TeamMemberYN);
            db.AddInParameter(dbCommand, "TeamMemberAllYN", DbType.String, weeklyTeamLeaderNotiCheckType.TeamMemberAllYN);
            //db.AddInParameter(dbCommand, "SendYN", DbType.String, weeklyTeamLeaderNotiCheckType.SendYN);

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_WeeklyTeamLeaderNotiCheck table.
        /// </summary>
        /// <returns>DataSet</returns>
        public MonthlyTeamLeaderNotiCheckType WeeklyTeamLeaderNotiCheckSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType = null;
            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    weeklyTeamLeaderNotiCheckType = GetWeeklyTeamLeaderNotiCheckTypeMapData(ds.Tables[0].Rows[0]);
                }
            }
            return weeklyTeamLeaderNotiCheckType;
        }

        /// <summary>
        /// Selects all records from the tb_WeeklyTeamLeaderNotiCheck table.
        /// </summary>
        public List<MonthlyTeamLeaderNotiCheckType> WeeklyTeamLeaderNotiCheckSelectAll()
        {
            List<MonthlyTeamLeaderNotiCheckType> listWeeklyTeamLeaderNotiCheckType = new List<MonthlyTeamLeaderNotiCheckType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_SelectAll");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType = GetWeeklyTeamLeaderNotiCheckTypeMapData(dr);
                        listWeeklyTeamLeaderNotiCheckType.Add(weeklyTeamLeaderNotiCheckType);
                    }
                }
            }
            return listWeeklyTeamLeaderNotiCheckType;
        }

        /// <summary>
        /// Updates a record in the tb_WeeklyTeamLeaderNotiCheck table.
        /// </summary>
        public void WeeklyTeamLeaderNotiCheckUpdate(MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Update");

            db.AddInParameter(dbCommand, "WeeklyNotiID", DbType.Int32, weeklyTeamLeaderNotiCheckType.WeeklyNotiID);
            db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyTeamLeaderNotiCheckType.UserID);
            db.AddInParameter(dbCommand, "TeamMemberYN", DbType.String, weeklyTeamLeaderNotiCheckType.TeamMemberYN);
            db.AddInParameter(dbCommand, "TeamMemberAllYN", DbType.String, weeklyTeamLeaderNotiCheckType.TeamMemberAllYN);
            db.AddInParameter(dbCommand, "SendYN", DbType.String, weeklyTeamLeaderNotiCheckType.SendYN);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a record from the tb_WeeklyTeamLeaderNotiCheck table by a composite primary key.
        /// </summary>
        public void WeeklyTeamLeaderNotiCheckDelete(int weeklyNotiID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Delete");

            db.AddInParameter(dbCommand, "WeeklyNotiID", DbType.Int32, weeklyNotiID);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 부서코드로 팀장 알림설정값 가져오기
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public DataSet WeeklyTeamLeaderNotiCheckSelect_Dept(string DeptCode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Select_Dept");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, DeptCode);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            
            return ds;
        }

        /// <summary>
        /// 부서코드로 팀장 알림설정값 가져오기
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public List<MonthlyTeamLeaderNotiCheckType> TeamLeaderNotiCheckSelect_Dept(string DeptCode)
        {
            List<MonthlyTeamLeaderNotiCheckType> listWeeklyTeamLeaderNotiCheckType = new List<MonthlyTeamLeaderNotiCheckType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_Select_Dept");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, DeptCode);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType = GetWeeklyTeamLeaderNotiCheckTypeMapData(dr);
                        listWeeklyTeamLeaderNotiCheckType.Add(weeklyTeamLeaderNotiCheckType);
                    }
                }
            }
            return listWeeklyTeamLeaderNotiCheckType;
        }

        public DataSet WeeklyTeamLeaderNotiCheck_WeeklyCount(string DeptCode, DateTime WeekDateTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_WeeklyCount");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, DeptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, WeekDateTime);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        /// <summary>
        /// 마지막 팀원이라면 1을 반환
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="WeekDateTime"></param>
        /// <returns></returns>
        public string TeamLeaderNotiCheck_WeeklyCount(string DeptCode, DateTime WeekDateTime)
        {
            List<MonthlyTeamLeaderNotiCheckType> listWeeklyTeamLeaderNotiCheckType = new List<MonthlyTeamLeaderNotiCheckType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyTeamLeaderNotiCheck_WeeklyCount");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, DeptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, WeekDateTime);

            string count = string.Empty;
            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        count = (dr["Count"] == DBNull.Value) ? String.Empty : dr.Field<string>("Count");
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Creates a new instance of the WeeklyTeamLeaderNotiCheckType class and populates it with data from the specified DataRow.
        /// </summary>
        private MonthlyTeamLeaderNotiCheckType GetWeeklyTeamLeaderNotiCheckTypeMapData(DataRow dr)
        {
            MonthlyTeamLeaderNotiCheckType weeklyTeamLeaderNotiCheckType = new MonthlyTeamLeaderNotiCheckType();
            weeklyTeamLeaderNotiCheckType.WeeklyNotiID = (dr["WeeklyNotiID"] == DBNull.Value) ? 0 : dr.Field<int>("WeeklyNotiID");
            weeklyTeamLeaderNotiCheckType.UserID = (dr["UserID"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserID");
            weeklyTeamLeaderNotiCheckType.UserName = (dr["UserName"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserName");
            weeklyTeamLeaderNotiCheckType.TeamMemberYN = (dr["TeamMemberYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TeamMemberYN");
            weeklyTeamLeaderNotiCheckType.TeamMemberAllYN = (dr["TeamMemberAllYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TeamMemberAllYN");
            weeklyTeamLeaderNotiCheckType.SendYN = (dr["SendYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("SendYN");
            if (dr.Table.Columns.Contains("EMAIL_ALIAS"))
            {
                weeklyTeamLeaderNotiCheckType.EMAIL_ALIAS = (dr["EMAIL_ALIAS"] == DBNull.Value) ? String.Empty : dr.Field<string>("EMAIL_ALIAS");
            }

            return weeklyTeamLeaderNotiCheckType;
        }
    }
}
