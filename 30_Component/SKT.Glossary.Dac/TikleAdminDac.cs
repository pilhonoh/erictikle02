using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKT.Glossary.Type;
using SKT.Common;

namespace SKT.Glossary.Dac
{
    public class TikleAdminDac
    {
        private const string connectionStringName = "ConnGlossary";

        public DataSet TikleAdminTotal(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_Total_Select");
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            db.AddInParameter(cmd, "edate", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }


        public DataSet TikleAdminDept(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_Dept_Select");
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            db.AddInParameter(cmd, "edate", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet TikleAdminMenu(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_Dept_SubMenu_Select");
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            db.AddInParameter(cmd, "edate", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.04.27 
        Desc : 통계화면 추가
        */     
        public DataSet TikleAdminAccess(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_ADMIN_WEEK_UVCNT");
            db.AddInParameter(cmd, "StartDT", DbType.String, sdate);
            db.AddInParameter(cmd, "EndDT", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet TikleAdminWeeklyNoteCount(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_ADMIN_WEEK_WEEKLYNOTE_CNT");
            db.AddInParameter(cmd, "StartDT", DbType.String, sdate);
            db.AddInParameter(cmd, "EndDT", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.05.11
        Desc : Weekly 통계 추가
        */ 
        public DataSet TikleAdminWeeklyData(string sdate, string edate, string deptcode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_Admin_Stats_WeeklyData");
            db.AddInParameter(cmd, "StartDT", DbType.String, sdate);
            db.AddInParameter(cmd, "EndDT", DbType.String, edate);
            db.AddInParameter(cmd, "deptcode", DbType.String, deptcode);
            return db.ExecuteDataSet(cmd);
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.05.11
        Desc : Weekly 통계 대상 부서 조회
        */ 
        public DataSet TikleAdminTargetDept()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_Admin_stats_WeeklyTeam");
            return db.ExecuteDataSet(cmd);
        }

		public DataSet TikleAdminBannerSelect()
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_MainNoticeBanner_Select");
			return db.ExecuteDataSet(cmd);
		}

		public DataSet TikleAdminBannerUpdate(string NotID, string Title, string ImgFile, string URL, string SeqNo, string USERID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_MainNoticeBanner_Update");
			db.AddInParameter(cmd, "NotID", DbType.String, NotID);
			db.AddInParameter(cmd, "Title", DbType.String, Title);
			db.AddInParameter(cmd, "ImgFile", DbType.String, ImgFile);
			db.AddInParameter(cmd, "URL", DbType.String, URL);
			db.AddInParameter(cmd, "SeqNo", DbType.String, SeqNo);
			db.AddInParameter(cmd, "USERID", DbType.String, USERID);
			return db.ExecuteDataSet(cmd);
		}

		public DataSet TikleAdminSiteConfigUpdate(string ATTR_NM, string ATTR_VAL, string ATTR_DESC, string USERID)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_SiteConfig_Update");
			db.AddInParameter(cmd, "ATTR_NM", DbType.String, ATTR_NM);
			db.AddInParameter(cmd, "ATTR_VAL", DbType.String, ATTR_VAL);
			db.AddInParameter(cmd, "ATTR_DESC", DbType.String, ATTR_DESC);
			db.AddInParameter(cmd, "USERID", DbType.String, USERID);
			return db.ExecuteDataSet(cmd);
		}

		public DataSet TikleAdminMainNoticeInsert(MainNoticeType data)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("UP_MainNotice_Insert");
			
			db.AddInParameter(cmd, "NotID", DbType.String, data.NotID);
			db.AddInParameter(cmd, "Gubun", DbType.String, data.Gubun);
			db.AddInParameter(cmd, "Title", DbType.String, data.Title);
			db.AddInParameter(cmd, "Content", DbType.String, data.Content);
			db.AddInParameter(cmd, "ImgFile", DbType.String, data.ImgFile);
			db.AddInParameter(cmd, "URL", DbType.String, data.URL);
			db.AddInParameter(cmd, "SeqNo", DbType.Int32, data.SeqNo);
			db.AddInParameter(cmd, "ItemID", DbType.String, data.Itemid);
			db.AddInParameter(cmd, "UseYn", DbType.String, data.UseYn);
			db.AddInParameter(cmd, "USERID", DbType.String, data.UserID);

			return db.ExecuteDataSet(cmd);
		}

		public DataSet TikleAdminMainNoticeSelect(string Gubun)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_MainNotice_Select");

			db.AddInParameter(cmd, "Gubun", DbType.String, Gubun);

			return db.ExecuteDataSet(cmd);
		}


		public DataSet TikleAdminMainNoticeDelete(string Gubun, string NotID = null)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_MainNotice_Delete");

			db.AddInParameter(cmd, "Gubun", DbType.String, Gubun);
			db.AddInParameter(cmd, "NotID", DbType.Int64, NotID);

			return db.ExecuteDataSet(cmd);
		}

        //관리자>통계>종합 문서함 개수 2014-11-24 김성환
        public DataSet TikleAdminTotal_DirExcel()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_Total_Dir_Excel");
            return db.ExecuteDataSet(cmd);
        }

        //관리자>통계>종합 의견함 개수 2014-11-24 김성환
        public DataSet TikleAdminTotal_SurveyExcel()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_Total_Survey_Excel");
            return db.ExecuteDataSet(cmd);
        }

        //Guest 스위칭 2015-05-012 김성환
        public DataSet TikleAdmin_GuestSwitch(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_Guest_Switch");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        //플랫폼 접속자수 2015-10-02 김성환
        public DataSet TikleAdminPlatStat(string sdate, string edate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleAdmin_PlatStat_Select");
            db.AddInParameter(cmd, "sdate", DbType.String, sdate);
            db.AddInParameter(cmd, "edate", DbType.String, edate);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet ArraTrendSelect(int PageNum, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_ArraTrend_Selct");
            db.AddInParameter(cmd, "PageNum", DbType.Int16, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int16, PageSize);
            return db.ExecuteDataSet(cmd);
        }
        public DataSet ArraTrendAction(string mode, int ID, string Gubun, string Title, string Url, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_ArraTrend_Action");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "ID", DbType.Int16, ID);
            db.AddInParameter(cmd, "Gubun", DbType.String, Gubun);
            db.AddInParameter(cmd, "Title", DbType.String, Title);
            db.AddInParameter(cmd, "Url", DbType.String, Url);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }


    }
}
