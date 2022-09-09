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
    public class GlossaryPeopleDac
    {
        private const string connectionStringName = "ConnGlossary";

        //공유 리스트
        public DataSet GlossaryScheduleInsert(GlossaryScheduleType Schedule)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySchedule_Insert");
            //db.AddInParameter(cmd, "SCID", DbType.String, Schedule.SCID);
            db.AddInParameter(cmd, "YYMMDD", DbType.String, Schedule.YYMMDD);
            db.AddInParameter(cmd, "YEAR", DbType.String, Schedule.YEAR);
            db.AddInParameter(cmd, "MON", DbType.String, Schedule.MON);
            db.AddInParameter(cmd, "DAY", DbType.String, Schedule.DAY);
            db.AddInParameter(cmd, "WEEK", DbType.String, Schedule.WEEK);
            db.AddInParameter(cmd, "TITLE", DbType.String, Schedule.TITLE);
            db.AddInParameter(cmd, "CONTENTS", DbType.String, Schedule.CONTENTS);
            db.AddInParameter(cmd, "USERID", DbType.String, Schedule.USERID);
            db.AddInParameter(cmd, "USERNAME", DbType.String, Schedule.USERNAME);
            db.AddInParameter(cmd, "CREATEDATE", DbType.String, Schedule.CREATEDATE);
            db.AddInParameter(cmd, "AUDIT_ID", DbType.String, Schedule.AUDIT_ID);
            db.AddInParameter(cmd, "AUDIT_DTM", DbType.String, Schedule.AUDIT_DTM);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryScheduleUpdate(GlossaryScheduleType Schedule)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySchedule_Update");
            db.AddInParameter(cmd, "SCID", DbType.String, Schedule.SCID);
            db.AddInParameter(cmd, "YYMMDD", DbType.String, Schedule.YYMMDD);
            db.AddInParameter(cmd, "YEAR", DbType.String, Schedule.YEAR);
            db.AddInParameter(cmd, "MON", DbType.String, Schedule.MON);
            db.AddInParameter(cmd, "DAY", DbType.String, Schedule.DAY);
            db.AddInParameter(cmd, "WEEK", DbType.String, Schedule.WEEK);
            db.AddInParameter(cmd, "TITLE", DbType.String, Schedule.TITLE);
            db.AddInParameter(cmd, "CONTENTS", DbType.String, Schedule.CONTENTS);
            db.AddInParameter(cmd, "USERID", DbType.String, Schedule.USERID);
            db.AddInParameter(cmd, "USERNAME", DbType.String, Schedule.USERNAME);
            db.AddInParameter(cmd, "CREATEDATE", DbType.String, Schedule.CREATEDATE);
            db.AddInParameter(cmd, "AUDIT_ID", DbType.String, Schedule.AUDIT_ID);
            db.AddInParameter(cmd, "AUDIT_DTM", DbType.String, Schedule.AUDIT_DTM);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryScheduleMonthDataSelect(string Schedule_Gubun, string Schedule_YYYYMM, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleMonthData_Select");
            db.AddInParameter(cmd, "SCHEDULE_GUBUN", DbType.String, Schedule_Gubun);
            db.AddInParameter(cmd, "SCHEDULE_YYYYMM", DbType.String, Schedule_YYYYMM);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryScheduleItemAlramSet(string SCID, string Alram_Flag, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleItemAlram_Update");
            db.AddInParameter(cmd, "SCID", DbType.String, SCID);
            db.AddInParameter(cmd, "ALRAM_FLAG", DbType.String, Alram_Flag);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryPeopleUserInfoSelect(string UserID, string SearchUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_PeopleUserInfo_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchUserID", DbType.String, SearchUserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryPeopleScrapUpdate(string UserID, string ScrapUserID, string ScrapsYN)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_PeopleScrap_Update");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "ScrapUserID", DbType.String, ScrapUserID);
            db.AddInParameter(cmd, "ScrapsYN", DbType.String, ScrapsYN);

            return db.ExecuteDataSet(cmd);
        }

        
    }
}