using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
	/// <summary>
	/// 설명: Data access class for tb_Weekly table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public class MonthlyDac
	{
		private const string connectionStringName = "ConnGlossary";

        public MonthlyDac() { }

        /// <summary>
        /// Deletes a record from the tb_Weekly table by a composite primary key.
        /// </summary>
        public void MonthlyDelete(string weeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Delete");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.String, weeklyID);

            db.ExecuteNonQuery(dbCommand);
        }

		/// <summary>
		/// Inserts a record into the tb_Weekly table.
		/// </summary>
		/// <returns></returns>
        public string MonthlyInsert(MonthlyType weeklyType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Insert");

            db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyType.Contents);
            db.AddInParameter(dbCommand, "TextContents", DbType.String, weeklyType.TextContents);
            db.AddInParameter(dbCommand, "ViewLevel", DbType.Int32, weeklyType.ViewLevel);
            db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyType.UserID);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, weeklyType.Date);
            db.AddInParameter(dbCommand, "TempYN", DbType.String, weeklyType.TempYN);

            db.AddInParameter(dbCommand, "MemoWriterID", DbType.String, weeklyType.MemoWriterID);
            db.AddInParameter(dbCommand, "MemoContents", DbType.String, weeklyType.MemoContents);
            db.AddInParameter(dbCommand, "MemoSentYN", DbType.String, weeklyType.MemoSentYN);
            db.AddInParameter(dbCommand, "PermissionYN", DbType.String, weeklyType.PermissionYN);

            //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련    
            db.AddInParameter(dbCommand, "AbsenceMsg", DbType.String, weeklyType.AbsenceMsg);


            //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침  씨발 쫒도 꺼져라..믹 소프트 개새끼들아 개발 좀 똑바로 해라.병신같은 새끼들아 니들이 개발자냐.........
            db.AddInParameter(dbCommand, "Year", DbType.Int32, weeklyType.Year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, weeklyType.YearWeek);
            db.AddInParameter(dbCommand, "StartWeekDate", DbType.DateTime, weeklyType.StartWeekDate);
            db.AddInParameter(dbCommand, "EndWeekDate", DbType.DateTime, weeklyType.EndWeekDate);

            // 2016-03-04
            // Tnet Mobile 운영자 노창현
            // Monthly 기능 추가로 인한 컬럼 추가
            db.AddInParameter(dbCommand, "MonthlyYYYY", DbType.String, weeklyType.MonthlyYYYY);
            db.AddInParameter(dbCommand, "MonthlyMM", DbType.String, weeklyType.MonthlyMM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM", DbType.String, weeklyType.MonthlyYYYYMM);            

			// Execute the query and return the new identity value
            return db.ExecuteScalar(dbCommand).ToString();
		}





        /// <summary>
        /// 사용자 ID로 겸직 정보를 Select
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet MonthlyAdditionalJob(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_AdditionalJob");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public DataSet MonthlySelect(string userID, int yearWeek, string deptCode, DateTime startWeekDate, DateTime endWeekDate, string WeeklyID, string startYYYYMM, string endYYYYMM, string MonthlyYYYYMM)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, yearWeek);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "StartWeekDate", DbType.DateTime, startWeekDate);
            db.AddInParameter(dbCommand, "EndWeekDate", DbType.DateTime, startWeekDate);


            if (!String.IsNullOrEmpty(WeeklyID))
            {
                db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, Convert.ToInt64(WeeklyID));
            }

            db.AddInParameter(dbCommand, "StartYYYYMM", DbType.String, startYYYYMM);
            db.AddInParameter(dbCommand, "EndYYYYMM", DbType.String, endYYYYMM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM", DbType.String, MonthlyYYYYMM);


            return db.ExecuteDataSet(dbCommand);
        }

     


        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public DataSet MonthlySelectWeeklyID(string weeklyID, string userID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_WeeklyID");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.String, weeklyID);
            if (userID != null)
            {
                db.AddInParameter(dbCommand, "userID", DbType.String, userID);
            }

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public DataSet MonthlySelectWeeklyIDFromUserID(string weeklyID, string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_WeeklyIDFromUserID");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.String, weeklyID);
            db.AddInParameter(dbCommand, "userID", DbType.String, userID);
            

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlySelectDeptCodeMyTeam(string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_MyTeam");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (!(EndDate == new DateTime(0)))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);


            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlySelectDeptCodeOfficer(string deptCode, DateTime weekDateTime, DateTime EndDate, string PositionName, string UserID, int year, int week, string YYYY, string MM, string YYYYMM, string YYYYMM_end)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_Officer");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            if (!String.IsNullOrEmpty(PositionName))
            {
                db.AddInParameter(dbCommand, "Position_Name", DbType.String, PositionName);
            }

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            //2015.12.18 위클리 년도,주 조회조건변경
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, week);


            db.AddInParameter(dbCommand, "MonthlyYYYY", DbType.String, YYYY);
            db.AddInParameter(dbCommand, "MonthlyMM", DbType.String, MM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM", DbType.String, YYYYMM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM_End", DbType.String, YYYYMM_end);

            return db.ExecuteDataSet(dbCommand);
        }



        /// <summary>
        /// CEO전용 임원 개별 출력
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlyCeoEachPrint(string TargetUserID, string UserID, string WeeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_CEO_print");

            db.AddInParameter(dbCommand, "TargetUserID", DbType.String, TargetUserID);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            db.AddInParameter(dbCommand, "WeeklyID", DbType.String, WeeklyID);

            return db.ExecuteDataSet(dbCommand);
        }
        /// <summary>
        /// 이형희 총괄 예외
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlySelectDeptCodeMno(string actiontype, string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID, int year, int week)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_General");

            db.AddInParameter(dbCommand, "type", DbType.String, actiontype);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);



            //2015.12.18 위클리 년도,주 조회조건변경
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, week);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 임원일 경우에 직속조직만 출력 SP
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <param name="EndDate"></param>
        /// <param name="PositionName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet MonthlySelectDeptCode_DirectOrg(string deptCode, DateTime weekDateTime, DateTime EndDate, string PositionName, string UserID, int year, int week)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_DirectOrg");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            if (!String.IsNullOrEmpty(PositionName))
            {
                db.AddInParameter(dbCommand, "Position_Name", DbType.String, PositionName);
            }

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            //2015.12.18 위클리 년도,주 조회조건변경
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, week);

            return db.ExecuteDataSet(dbCommand);
        }

        public DataSet MonthlySelectDeptCodeCEO(DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_CEO");

            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);
        }

        public DataSet MonthlySelectDeptCodeMNO(DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_MNO");

            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);
        }



        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlySelectDeptCodeUpper(string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID, string YYYY, string MM, string YYYYMM, string YYYYMM_End)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_Upper");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);

            if(EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            // 
            db.AddInParameter(dbCommand, "MonthlyYYYY", DbType.String, YYYY);
            db.AddInParameter(dbCommand, "MonthlyMM", DbType.String, MM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM", DbType.String, YYYYMM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM_End", DbType.String, YYYYMM_End);


            return db.ExecuteDataSet(dbCommand);
        }




        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet MonthlySelectExceptionViewUser(string deptCode, DateTime weekDateTime, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_Exception_User");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
           
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);
        }


        /// <summary>
        /// Mr.No
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public DataSet Monthly_Select_DeptCode_OrgChart(string deptCode, DateTime weekDateTime, string Kind, DateTime EndDate, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptCode_OrgChart");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            db.AddInParameter(dbCommand, "Kind", DbType.String, Kind);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public DataSet MonthlySelectNewestDown(string deptCode, DateTime weekDateTime, DateTime EndDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_Newest_Down");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            if (!(EndDate == new DateTime(0)))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public DataSet MonthlySelectNewestMyTeam(string deptCode, DateTime weekDateTime, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_Newest_MyTeam");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public DataSet MonthlySelectNewestUpper(string deptCode, DateTime weekDateTime, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_Newest_Upper");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(dbCommand);
        }

		/// <summary>
		/// Selects all records from the tb_Weekly table.
		/// </summary>
        public DataSet MonthlySelectUser(string userID, int year, DateTime StartDate, DateTime EndDate)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_User");
			
			db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
			db.AddInParameter(dbCommand, "Year", DbType.Int32, year);

            if (StartDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            }
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            return db.ExecuteDataSet(dbCommand);
		}

        /// <summary>
        /// 작성자별 보기 전용
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataSet MonthlySelectUser_User(string userID, int year, DateTime StartDate, DateTime EndDate, string MyUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_User");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);

            if (StartDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            }
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            db.AddInParameter(dbCommand, "MyUserID", DbType.String, MyUserID);

            return db.ExecuteDataSet(dbCommand);
        }



        //2015.06.01 KSM 추가. 팀장 부서이동 오류수정
        public DataSet MonthlySelectUser_Dept(string UserID, int year, DateTime StartDate, DateTime EndDate, string MyUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_UserByDept");

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);

            if (StartDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            }
            if (EndDate != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            }
            db.AddInParameter(dbCommand, "MyUserID", DbType.String, MyUserID);

            return db.ExecuteDataSet(dbCommand);
        }
        //2015.06.01 //
        public DataSet MonthlySelectMyTeamUser(string deptCode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_MyTeamUser");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);

            return db.ExecuteDataSet(dbCommand);
        }

		/// <summary>
		/// Updates a record in the tb_Weekly table.
		/// </summary>
        public void MonthlyUpdate(MonthlyType weeklyType)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Update");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyType.WeeklyID);
            db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyType.Contents);
            db.AddInParameter(dbCommand, "TextContents", DbType.String, weeklyType.TextContents);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(dbCommand, "TempYN", DbType.String, weeklyType.TempYN);
            db.AddInParameter(dbCommand, "PermissionYN", DbType.String, weeklyType.PermissionYN);
            db.AddInParameter(dbCommand, "LastUpdateWriterID", DbType.String, weeklyType.UserID);

            //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련
            db.AddInParameter(dbCommand, "AbsenceMsg", DbType.String, weeklyType.AbsenceMsg);

			db.ExecuteNonQuery(dbCommand);
		}

       

        /// <summary>
        /// 부서 내 사용자 정보를 가져온다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataSet MonthlySelectDeptUserList(string deptCode, int viewLevel)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptUserList");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            if (viewLevel != 0)
            {
                db.AddInParameter(dbCommand, "ViewLevel", DbType.Int32, viewLevel);
            }

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 부서의 상위 부서장 정보를 가져온다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataSet MonthlySelectDeptUpperList(string deptCode, int viewLevel)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_DeptUpperList");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "ViewLevel", DbType.Int32, viewLevel);

            return db.ExecuteDataSet(dbCommand);
        }

        public void Monthly_Memo_Update(int WeeklyID, string MemoWriterID, string MemoContents, DateTime MemoCreateDateTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Memo_Update");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, WeeklyID);
            db.AddInParameter(dbCommand, "MemoWriterID", DbType.String, MemoWriterID);
            db.AddInParameter(dbCommand, "MemoContents", DbType.String, MemoContents);
            if (MemoCreateDateTime != new DateTime(0))
            {
                db.AddInParameter(dbCommand, "MemoCreateDateTime", DbType.DateTime, MemoCreateDateTime);
            }

            db.ExecuteNonQuery(dbCommand);
        }

        public int Monthly_Memo_Select(int WeeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Memo_Select");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, WeeklyID);

            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// 자회사 대표 Weekly만 가져오기
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public DataSet Monthly_Select_ExternalRepresent(DateTime weekDateTime, int year, int week)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_ExternalRepresent");

            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);


            //2015.12.18 위클리 년도,주 조회조건변경
            db.AddInParameter(dbCommand, "Year", DbType.Int32, year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, week);

            return db.ExecuteDataSet(dbCommand);
        }



        /// <summary>
        /// UserInfo
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet UserInfo_Select(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_UserInfo_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// Mr.No
        /// 15,19 사번중 접속 가능한 사번 체크
        /// </summary>
        /// <returns></returns>
        public string Monthly_Select_AddExceptionUser(string UserID)
        {
            string userID = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_AddExceptionUser");

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    userID = (dr["UserID"] == DBNull.Value) ? string.Empty : dr.Field<string>("UserID");
                }
            }
            return userID;
        }

        /// <summary>
        /// Mr.No 2015-07-03
        /// 그룹ID를 받아서 EMail 발송형식에 맞춰 return
        /// </summary>
        /// <param name="MYGRP_ID"></param>
        /// <returns></returns>
        public string GroupUser_EMail(string MYGRP_ID, string UserID )
        {
            string GroupEMailString = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_MyGroupList_User");

            db.AddInParameter(dbCommand, "MYGRP_ID", DbType.String, MYGRP_ID);
            db.AddInParameter(dbCommand, "AUTH_ID", DbType.String, UserID);     // 생성자 ID

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GroupEMailString += (dr["GroupEMailString"] == DBNull.Value) ? string.Empty : dr.Field<string>("GroupEMailString");
                    }
                }
            }
            return GroupEMailString;
        }


        #region 2015.07.10 zz17779 : 공통 위클리 관련 메서드 --------------------------------------


        /// <summary>
        /// 2015.07.13 zz17779 : 공통 위클리 조회
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="yearWeek"></param>
        /// <param name="deptCode"></param>
        /// <param name="WeeklyID"></param>
        /// <returns></returns>
        public DataSet MonthlyCommonSelect(string userID, int yearWeek, string deptCode, DateTime startWeekDate, DateTime endWeekDate, string WeeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Common_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, yearWeek);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);


            db.AddInParameter(dbCommand, "StartWeekDate", DbType.DateTime, startWeekDate);
            db.AddInParameter(dbCommand, "EndWeekDate", DbType.DateTime, endWeekDate);



            if (!String.IsNullOrEmpty(WeeklyID))
            {
                db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, Convert.ToInt64(WeeklyID));
            }

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 2015.07.13 ZZ17779 
        /// </summary>
        /// <param name="weeklyType"></param>
        /// <returns></returns>
        public string MonthlyCommonInsert(MonthlyType weeklyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Common_Insert");

            db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyType.Contents);
            db.AddInParameter(dbCommand, "TextContents", DbType.String, weeklyType.TextContents);
            db.AddInParameter(dbCommand, "ViewLevel", DbType.Int32, weeklyType.ViewLevel);
            db.AddInParameter(dbCommand, "UserID", DbType.String, weeklyType.UserID);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, weeklyType.Date);
            db.AddInParameter(dbCommand, "TempYN", DbType.String, weeklyType.TempYN);

            db.AddInParameter(dbCommand, "MemoWriterID", DbType.String, weeklyType.MemoWriterID);
            db.AddInParameter(dbCommand, "MemoContents", DbType.String, weeklyType.MemoContents);
            db.AddInParameter(dbCommand, "MemoSentYN", DbType.String, weeklyType.MemoSentYN);
            db.AddInParameter(dbCommand, "PermissionYN", DbType.String, weeklyType.PermissionYN);
            db.AddInParameter(dbCommand, "CommonDeptFlag", DbType.String, weeklyType.CommonWeeklyFlag);



            //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침  씨발 쫒도 꺼져라..믹 소프트 개새끼들아 개발 좀 똑바로 해라.병신같은 새끼들아 니들이 개발자냐.........
            db.AddInParameter(dbCommand, "Year", DbType.Int32, weeklyType.Year);
            db.AddInParameter(dbCommand, "YearWeek", DbType.Int32, weeklyType.YearWeek);
            db.AddInParameter(dbCommand, "StartWeekDate", DbType.DateTime, weeklyType.StartWeekDate);
            db.AddInParameter(dbCommand, "EndWeekDate", DbType.DateTime, weeklyType.EndWeekDate);


            // Execute the query and return the new identity value
            return db.ExecuteScalar(dbCommand).ToString();
        }


        /// <summary>
        /// 2015.07.13 zz17779
        /// </summary>
        /// <param name="weeklyType"></param>
        public void MonthlyCommonUpdate(MonthlyType weeklyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Common_Update");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyType.WeeklyID);
            db.AddInParameter(dbCommand, "Contents", DbType.String, weeklyType.Contents);
            db.AddInParameter(dbCommand, "TextContents", DbType.String, weeklyType.TextContents);

            db.AddInParameter(dbCommand, "DeptName", DbType.String, weeklyType.DeptName);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(dbCommand, "TempYN", DbType.String, weeklyType.TempYN);
            db.AddInParameter(dbCommand, "PermissionYN", DbType.String, weeklyType.PermissionYN);

            db.AddInParameter(dbCommand, "LastUpdateWriterID", DbType.String, weeklyType.UserID);

            db.ExecuteNonQuery(dbCommand);

        }


        //2015.07.13 ZZ17779 : 공통위클리 관련 
        public DataSet Monthly_Common_Select_DeptCode_Upper(string deptCode, DateTime weekDateTime, DateTime endDateTime, string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Common_Select_DeptCode_Upper");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDateTime);

            //if (EndDate != new DateTime(0))
            //{
            //    db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            //}
            //db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);
        }


        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public DataSet MonthlyCommonSelectWeeklyID(string weeklyID, string userID = null, string cmmWeeklyFlag = "")
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Common_Select_WeeklyID");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.String, weeklyID);
            if (userID != null)
            {
                db.AddInParameter(dbCommand, "userID", DbType.String, userID);
            }

            return db.ExecuteDataSet(dbCommand);
        }


        #endregion //2015.07.10 공통 위클리 관련 메서드 -------------------------------------------




        #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 ----------------------------

        public DataSet Monthly_Select_Detail_UserAbsence(string idx)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_Detail_UserAbsence");

            db.AddInParameter(dbCommand, "Idx", DbType.String, idx);

            return db.ExecuteDataSet(dbCommand);
        }

        public DataSet Monthly_Select_UserAbsence_MyList(string absence_UserID, DateTime weekDateTime, string startDate, string endDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_UserAbsence_MyList");

            db.AddInParameter(dbCommand, "Absence_UserID", DbType.String, absence_UserID);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDateTime);
            db.AddInParameter(dbCommand, "StartDate", DbType.String, startDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.String, endDate);

            return db.ExecuteDataSet(dbCommand);
        }


        //up_Weekly_UserAbsence_Select @UserID
        public DataSet Monthly_Select_UserAbsence(string userid)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_UserAbsence");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userid);

            return db.ExecuteDataSet(dbCommand);
        }
        
        // [dbo].[up_Weekly_Update_UserAbsence] (
        public void Monthly_Update_UserAbsence(MonthlyAbsenceType AbsenceType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Update_UserAbsence");

            db.AddInParameter(dbCommand, "Idx", DbType.Int32, AbsenceType.Idx);
            db.AddInParameter(dbCommand, "UserID", DbType.String, AbsenceType.UserID);
            db.AddInParameter(dbCommand, "Absence_UserID", DbType.String, AbsenceType.Absence_UserID);
            db.AddInParameter(dbCommand, "Absence_Comment", DbType.String, AbsenceType.Absence_Comment);
            db.AddInParameter(dbCommand, "Absence_StartDt", DbType.String, AbsenceType.Absence_StartDt);
            db.AddInParameter(dbCommand, "Absence_EndDt", DbType.String, AbsenceType.Absence_EndDt);
            db.AddInParameter(dbCommand, "Absence_Flag", DbType.String, AbsenceType.Absence_Flag);

            db.ExecuteNonQuery(dbCommand);

        }

        //위임 수정 : up_Weekly_Insert_UserAbsence
        public void Monthly_Insert_UserAbsence(MonthlyAbsenceType AbsenceType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Insert_UserAbsence");

            db.AddInParameter(dbCommand, "UserID", DbType.String, AbsenceType.UserID);
            db.AddInParameter(dbCommand, "Absence_UserID", DbType.String, AbsenceType.Absence_UserID);
            db.AddInParameter(dbCommand, "Absence_Comment", DbType.String, AbsenceType.Absence_Comment);
            db.AddInParameter(dbCommand, "Absence_StartDt", DbType.String, AbsenceType.Absence_StartDt);
            db.AddInParameter(dbCommand, "Absence_EndDt", DbType.String, AbsenceType.Absence_EndDt);
            db.AddInParameter(dbCommand, "Absence_Flag", DbType.String, AbsenceType.Absence_Flag);

            db.ExecuteNonQuery(dbCommand);
        }

        //위임 해제 up_Weekly_Delete_UserAbsence
        public void Monthly_Delete_UserAbsence(int absenceIdx)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Delete_UserAbsence");

            db.AddInParameter(dbCommand, "Idx", DbType.Int32, absenceIdx);

            db.ExecuteNonQuery(dbCommand);

        }


        /// <summary>
        /// 공통코드 조회
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public DataSet CommonCode_Select(string pcode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Select_CommonCode");

            db.AddInParameter(dbCommand, "P_CODE_ID", DbType.String, pcode);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 위임중복체크
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet Monthly_Select_Duplicate_UserAbsence(MonthlyAbsenceType AbsenceType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Duplicate_UserAbsence");

            db.AddInParameter(dbCommand, "UserID", DbType.String, AbsenceType.UserID);
            db.AddInParameter(dbCommand, "Absence_UserID", DbType.String, AbsenceType.Absence_UserID);
            db.AddInParameter(dbCommand, "Absence_StartDt", DbType.String, AbsenceType.Absence_StartDt);
            db.AddInParameter(dbCommand, "Absence_EndDt", DbType.String, AbsenceType.Absence_EndDt);

            return db.ExecuteDataSet(dbCommand);
        }

        #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 -----------------------

        public void MonthlyInsertByMail(string email, string htmlContents, string textContents, DateTime currentDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Insert_By_Outlook");

            db.AddInParameter(cmd, "UserEmail", DbType.String, email);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, htmlContents);
            db.AddInParameter(cmd, "TextContents", DbType.String, textContents);
            db.AddInParameter(cmd, "CurrentDate", DbType.DateTime, currentDate);

            db.ExecuteNonQuery(cmd);

        }




        #region  전년도 위클리 데이터 조회

        //전년도 부서 위클리 조회
        public DataSet Monthly_OldYearWeekly_Select(string userid, string deptcode, DateTime StartDate, DateTime EndDate, string PrintType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_OldYearWeekly_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userid);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptcode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            db.AddInParameter(dbCommand, "PrintType", DbType.String, PrintType);

            return db.ExecuteDataSet(dbCommand);
        }


        #endregion //	- 전년도 위클리 데이터 조회


        /// <summary>
        /// 2016.01.29 백충기 : 위클리 관리자 통계화면에서 조직 select 조회.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet MonthlyddlDeptCodeSelect()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_ddlDeptCode");

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 먼슬리 사용권한 조회
        /// </summary>
        /// <returns></returns>
        public DataSet MonthlyUserAuthoritySelect(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_UserAuthority_Select");

            db.AddInParameter(cmd, "UserID", DbType.String, userID);

            return db.ExecuteDataSet(cmd);
        }







    } //end weeklyDac class

    /// <summary>
    /// 설명: Data access class for TB_DEPARTMENT_ORGCHART table.
    /// 작성일 : 2015-04-13
    /// 작성자 : miksystem.com
    /// </summary>
    public sealed class DEPARTMENT_ORGCHARTDac_Monthly
    {
        private const string connectionStringName = "ConnGlossary";

        public DEPARTMENT_ORGCHARTDac_Monthly() { }

        /// <summary>
        /// Selects a single record from the TB_DEPARTMENT_ORGCHART table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DEPARTMENT_ORGCHARTType_Monthly DEPARTMENT_ORGCHARTSelect(string departmentNumber)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("[up_MonthlyDept_Select]");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, departmentNumber);

            DEPARTMENT_ORGCHARTType_Monthly dEPARTMENT_ORGCHARTType = null;
            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dEPARTMENT_ORGCHARTType = GetDEPARTMENT_ORGCHARTTypeMapData(ds.Tables[0].Rows[0]);
                }
            }
            return dEPARTMENT_ORGCHARTType;
        }

        /// <summary>
        /// Creates a new instance of the DEPARTMENT_ORGCHARTType class and populates it with data from the specified DataRow.
        /// </summary>
        private DEPARTMENT_ORGCHARTType_Monthly GetDEPARTMENT_ORGCHARTTypeMapData(DataRow dr)
        {
            DEPARTMENT_ORGCHARTType_Monthly dEPARTMENT_ORGCHARTType = new DEPARTMENT_ORGCHARTType_Monthly();
            dEPARTMENT_ORGCHARTType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<int>("ID");
            dEPARTMENT_ORGCHARTType.CREATEDATE = (dr["CREATEDATE"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CREATEDATE");
            dEPARTMENT_ORGCHARTType.LASTMODIFIEDDATE = (dr["LASTMODIFIEDDATE"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("LASTMODIFIEDDATE");
            dEPARTMENT_ORGCHARTType.DepartmentNumber = (dr["departmentNumber"] == DBNull.Value) ? null : dr.Field<string>("departmentNumber");
            dEPARTMENT_ORGCHARTType.Department = (dr["department"] == DBNull.Value) ? null : dr.Field<string>("department");
            dEPARTMENT_ORGCHARTType.DisplayName = (dr["displayName"] == DBNull.Value) ? null : dr.Field<string>("displayName");
            dEPARTMENT_ORGCHARTType.DisplayLevel = (dr["displayLevel"] == DBNull.Value) ? null : dr.Field<string>("displayLevel");
            dEPARTMENT_ORGCHARTType.DisplayOrder = (dr["displayOrder"] == DBNull.Value) ? null : dr.Field<string>("displayOrder");
            dEPARTMENT_ORGCHARTType.HasChild = (dr["hasChild"] == DBNull.Value) ? false : dr.Field<bool>("hasChild");
            dEPARTMENT_ORGCHARTType.LocationCode = (dr["locationCode"] == DBNull.Value) ? null : dr.Field<string>("locationCode");
            dEPARTMENT_ORGCHARTType.ManagerEmployeeID = (dr["managerEmployeeID"] == DBNull.Value) ? null : dr.Field<string>("managerEmployeeID");
            dEPARTMENT_ORGCHARTType.UpperDepartmentNumber = (dr["upperDepartmentNumber"] == DBNull.Value) ? null : dr.Field<string>("upperDepartmentNumber");
            dEPARTMENT_ORGCHARTType.DisplayYN = (dr["displayYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("displayYN");
            dEPARTMENT_ORGCHARTType.Mail = (dr["mail"] == DBNull.Value) ? null : dr.Field<string>("mail");
            dEPARTMENT_ORGCHARTType.PATH = (dr["PATH"] == DBNull.Value) ? null : dr.Field<string>("PATH");

            return dEPARTMENT_ORGCHARTType;
        }




    }

    //2015-04-26 메일 읽어 오는데 필요함.
    /// <summary>
    /// 설명: Data access class for tb_WeeklyMail table.
    /// 작성일 : 2015-04-26
    /// 작성자 : miksystem.com
    /// </summary>
    public sealed class MonthlyMailDac
    {
        private const string connectionStringName = "ConnGlossary";

        public MonthlyMailDac() { }

        /// <summary>
        /// Inserts a record into the tb_WeeklyMail table.
        /// </summary>
        /// <returns></returns>
        public void WeeklyMailInsert(WeeklyMailType weeklyMailType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyMail_Insert");

            db.AddInParameter(dbCommand, "WeeklyID", DbType.Int64, weeklyMailType.WeeklyID);
            db.AddInParameter(dbCommand, "MessageID", DbType.String, weeklyMailType.MessageID);
            db.AddInParameter(dbCommand, "DateTimeSent", DbType.DateTime, weeklyMailType.DateTimeSent);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Max(DateTimeSent)를 얻는다.
        /// </summary>
        public DateTime WeeklyMailSelectMax()
        {
            DateTime maxSent = new DateTime();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MonthlyMail_SelectMax");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    maxSent = (dr["DateTimeSent"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("DateTimeSent");
                }
            }
            return maxSent;
        }
        public string WeeklyInsertByMail(string email, string htmlContents, string textContents, DateTime currentDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Insert_By_Outlook");

            db.AddInParameter(cmd, "UserEmail", DbType.String, email);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, htmlContents);
            db.AddInParameter(cmd, "TextContents", DbType.String, textContents);
            db.AddInParameter(cmd, "CurrentDate", DbType.DateTime, currentDate);
            //db.ExecuteNonQuery(cmd);
            return db.ExecuteScalar(cmd).ToString();

        }

        //2015-06-24 김성환 
        //public int WeeklyEmailSendLog_Insert(string fromMail, string pTo, string pCC, string pBcc, string pTitle, string pContent)
        public int WeeklyMailSendLog_Insert(string fromMail, string pTo, string pCC, string pBcc, string pTitle, string pContent,string inserttype)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_MonthlyEmailSendLog_Insert");

            db.AddInParameter(cmd, "FromEmail", DbType.String, fromMail);
            db.AddInParameter(cmd, "ToEmail", DbType.String, pTo);
            db.AddInParameter(cmd, "CcEmail", DbType.String, pCC);
            db.AddInParameter(cmd, "BccEmail", DbType.String, pBcc);
            db.AddInParameter(cmd, "MailTitle", DbType.String, pTitle);
            db.AddInParameter(cmd, "Contents", DbType.String, pContent);
            db.AddInParameter(cmd, "InsertType", DbType.String, inserttype);
            
            return db.ExecuteNonQuery(cmd);

        }




    }
}
