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
   public class GlossaryAdminDac
    {
        private const string connectionStringName = "ConnGlossary";

        public int GlossaryAdminSearchKeywordsInsert(string searchtype, string searchKeyword, string userid)
       {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_SearchKeywords_Insert");
            db.AddInParameter(cmd, "SEARCHTYPE", DbType.String, searchtype);
            db.AddInParameter(cmd, "SEARCHKEYWORD", DbType.String, searchKeyword);
            db.AddInParameter(cmd, "USERID", DbType.String, userid);
            return db.ExecuteNonQuery(cmd);
       }

        //관리자 통계 조회 20140106 
        public DataSet GlossaryAdminStatList(int PageNum, int PageSize, string sdate, string edate, string stime, string etime, int mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_STAT_SELECT");
            db.AddInParameter(cmd, "mode", DbType.String, mode);
            db.AddInParameter(cmd, "pagenum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "pagesize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            db.AddInParameter(cmd, "stime", DbType.String, stime);
            db.AddInParameter(cmd, "edate", DbType.String, edate);
            db.AddInParameter(cmd, "etime", DbType.String, etime);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryAdminStatDivConditionList(string mode, string syear)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_STAT_CONDITION_SELECT");
            db.AddInParameter(cmd, "mode", DbType.String, mode );
            db.AddInParameter(cmd, "syear", DbType.String, syear);            
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryAdminStatDivList(string mode, string sdate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_STAT_WEEK_SELECT");
            db.AddInParameter(cmd, "mode", DbType.String, mode);
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            return db.ExecuteDataSet(cmd);
        }

      

       public DataSet WeeklyAdminUserList(int pagenum, int pagesize, int level, string userid, int mode, int total)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_SPECIALUSER_TempWeekly_SELECT");
           db.AddInParameter(cmd, "pagenum", DbType.Int32, pagenum);
           db.AddInParameter(cmd, "pagesize", DbType.Int32, pagesize);
           db.AddInParameter(cmd, "level", DbType.Int32, level);
           db.AddInParameter(cmd, "userid", DbType.String, userid);
           db.AddInParameter(cmd, "mode", DbType.Int32, mode);
           db.AddInParameter(cmd, "total", DbType.Int32, total);
           return db.ExecuteDataSet(cmd);
       }

       public DataSet GlossaryAdminHallofFameList(string Mode, int PageNum, int PageSize, string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_HALLOFFAME_SELECT");
           db.AddInParameter(cmd, "Mode", DbType.String, Mode);
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       public int GlossaryAdminHallofFameUpdate(string Mode, GlossaryHallOfFameType Board)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_HALLOFFAME_UPDATE");
           db.AddInParameter(cmd, "Mode", DbType.String, Mode);
           db.AddInParameter(cmd, "GLOSSARYID", DbType.String, Board.GlossaryID);
           db.AddInParameter(cmd, "USERID", DbType.String, Board.LastModifiedUserID);
           db.AddInParameter(cmd, "USERIP", DbType.String, Board.LastModifiedUserIP);
           db.AddInParameter(cmd, "USERMACHINENAME", DbType.String, Board.LastModifiedUserMachineName);
           if(Board.CssTitleBox != null)
           {
               db.AddInParameter(cmd, "CSSTITLEBOX", DbType.String, Board.CssTitleBox);            
           }
           
           return db.ExecuteNonQuery(cmd);
       }


       public DataSet GlossaryAdminExceptUserList(int curpage, int pagesize, string schText)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_SPECIALUSER_LIST");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, curpage);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, pagesize);
           db.AddInParameter(cmd, "SearchText", DbType.String, schText);
           return db.ExecuteDataSet(cmd);
       }

       public DataSet GlossaryAdminExceptInsert(string userid, string level, string registerid)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_SPECIALUSER_INSERT");
           db.AddInParameter(cmd, "USERID", DbType.String, userid); 
           db.AddInParameter(cmd, "LEVEL", DbType.String, level);
           db.AddInParameter(cmd, "REGISTERID", DbType.String, registerid);
           return db.ExecuteDataSet(cmd);
       }

       public DataSet GlossaryAdminExceptDelete(string userid)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_SPECIALUSER_DELETE");
           db.AddInParameter(cmd, "USERID", DbType.String, userid);
           return db.ExecuteDataSet(cmd);
       }

        public DataSet GlossaryAdminDTLogExcel(string sDate, string eDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_DTLog_Excel ");
            db.AddInParameter(cmd, "sDate", DbType.String, sDate);
            db.AddInParameter(cmd, "eDate", DbType.String, eDate);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryAdminDTLogList(int iGubun, int iPageNum, int iPageSize, string sDate, string eDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_DTLog_List");
            db.AddInParameter(cmd, "Gubun", DbType.Int32, iGubun);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, iPageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, iPageSize);
            db.AddInParameter(cmd, "sDate", DbType.String, sDate);
            db.AddInParameter(cmd, "eDate", DbType.String, eDate);
            return db.ExecuteDataSet(cmd);
        }
    }
}