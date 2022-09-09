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
    public class GlossaryScheduleDac
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
            db.AddInParameter(cmd, "HOUR", DbType.String, Schedule.HOUR);
            db.AddInParameter(cmd, "MIN", DbType.String, Schedule.MIN);
            db.AddInParameter(cmd, "TITLE", DbType.String, Schedule.TITLE);
            db.AddInParameter(cmd, "CONTENTS", DbType.String, Schedule.CONTENTS);
            db.AddInParameter(cmd, "URL", DbType.String, Schedule.URL);
            db.AddInParameter(cmd, "USERID", DbType.String, Schedule.USERID);
            db.AddInParameter(cmd, "USERNAME", DbType.String, Schedule.USERNAME);
            db.AddInParameter(cmd, "AuthYN", DbType.String, Schedule.AuthYN);
            db.AddInParameter(cmd, "CREATEDATE", DbType.String, Schedule.CREATEDATE);
            db.AddInParameter(cmd, "AUDIT_ID", DbType.String, Schedule.AUDIT_ID);
            db.AddInParameter(cmd, "AUDIT_DTM", DbType.String, Schedule.AUDIT_DTM);

            //종료시간(모임용)
            db.AddInParameter(cmd, "EndHOUR", DbType.String, Schedule.EndHOUR);
            db.AddInParameter(cmd, "EndMIN", DbType.String, Schedule.EndMIN);

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
            db.AddInParameter(cmd, "HOUR", DbType.String, Schedule.HOUR);
            db.AddInParameter(cmd, "MIN", DbType.String, Schedule.MIN);
            db.AddInParameter(cmd, "TITLE", DbType.String, Schedule.TITLE);
            db.AddInParameter(cmd, "CONTENTS", DbType.String, Schedule.CONTENTS);
            db.AddInParameter(cmd, "URL", DbType.String, Schedule.URL);
            db.AddInParameter(cmd, "USERID", DbType.String, Schedule.USERID);
            db.AddInParameter(cmd, "USERNAME", DbType.String, Schedule.USERNAME);
            db.AddInParameter(cmd, "AuthYN", DbType.String, Schedule.AuthYN);
            db.AddInParameter(cmd, "CREATEDATE", DbType.String, Schedule.CREATEDATE);
            db.AddInParameter(cmd, "AUDIT_ID", DbType.String, Schedule.AUDIT_ID);
            db.AddInParameter(cmd, "AUDIT_DTM", DbType.String, Schedule.AUDIT_DTM);
            //종료시간(모임용)
            db.AddInParameter(cmd, "EndHOUR", DbType.String, Schedule.EndHOUR);
            db.AddInParameter(cmd, "EndMIN", DbType.String, Schedule.EndMIN);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryScheduleDelete(GlossaryScheduleType Schedule)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySchedule_Delete");
            db.AddInParameter(cmd, "SCID", DbType.String, Schedule.SCID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryScheduleMonthDataSelect(string Schedule_Gubun, string Schedule_DataGubun, string Schedule_YYYYMM, string UserID, string GatheringYN, string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleMonthData_Select");
            db.AddInParameter(cmd, "SCHEDULE_GUBUN", DbType.String, Schedule_Gubun);
            db.AddInParameter(cmd, "SCHEDULE_DATAGUBUN", DbType.String, Schedule_DataGubun);
            db.AddInParameter(cmd, "SCHEDULE_YYYYMM", DbType.String, Schedule_YYYYMM);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);

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

        public DataSet GlossaryScheduleItemData(string SCID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleItemData_Select");
            db.AddInParameter(cmd, "SCID", DbType.String, SCID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        //공유 삭제
        public DataSet GlossaryScheduleAuthDelete(string SCID, string Type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleAuth_Delete");
            db.AddInParameter(cmd, "SCID", DbType.String, SCID);
            db.AddInParameter(cmd, "Type", DbType.String, Type);
            return db.ExecuteDataSet(cmd);
        }

        //공유 추가
        public DataSet GlossaryScheduleAuthInsert(CommonAuthType Board, string mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleAuth_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "SeqNO", DbType.String, Board.SeqNO);
            db.AddInParameter(cmd, "SCID", DbType.String, Board.ItemID);
            db.AddInParameter(cmd, "AuthType", DbType.String, Board.AuthType);
            db.AddInParameter(cmd, "AuthID", DbType.String, Board.AuthID);
            db.AddInParameter(cmd, "AuthCL", DbType.String, Board.AuthRWX);
            db.AddInParameter(cmd, "AuditID", DbType.String, Board.AuditID);
            db.AddInParameter(cmd, "AuditDTM", DbType.String, Board.AuditDTM);

            return db.ExecuteDataSet(cmd);
        }

        public List<CommonAuthType> GlossaryScheduleAuthSelect(string SCID)
        {
            List<CommonAuthType> listGlossaryAuthType = new List<CommonAuthType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryScheduleAuth_Select");

            db.AddInParameter(cmd, "SCID", DbType.String, SCID);

            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        CommonAuthType GlossaryAuthType = new CommonAuthType();

                        GlossaryAuthType.SeqNO = (dr["SEQ_NO"] == DBNull.Value) ? null : Convert.ToString(dr["SEQ_NO"]);
                        GlossaryAuthType.ItemID = (dr["DIR_ID"] == DBNull.Value) ? null : Convert.ToString(dr["DIR_ID"]);
                        GlossaryAuthType.AuthType = (dr["AUTH_TYPE"] == DBNull.Value) ? null : dr.Field<string>("AUTH_TYPE");
                        GlossaryAuthType.AuthID = (dr["AUTH_ID"] == DBNull.Value) ? null : dr.Field<string>("AUTH_ID");
                        GlossaryAuthType.AuthRWX = (dr["AUTH_CL"] == DBNull.Value) ? null : dr.Field<string>("AUTH_CL");
                        GlossaryAuthType.AuditID = (dr["AUDIT_ID"] == DBNull.Value) ? null : dr.Field<string>("AUDIT_ID");


                        GlossaryAuthType.TeamName = (dr["DNAME"] == DBNull.Value) ? null : dr.Field<string>("DNAME");
                        GlossaryAuthType.DeptName = (dr["SNAME"] == DBNull.Value) ? null : dr.Field<string>("SNAME");

                        GlossaryAuthType.RegID = (dr["REG_ID"] == DBNull.Value) ? null : dr.Field<string>("REG_ID");

                        GlossaryAuthType.RegDTM = (dr["REG_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("REG_DTM");

                        listGlossaryAuthType.Add(GlossaryAuthType);
                    }
                }
            }
            return listGlossaryAuthType;
        }
    }
}

