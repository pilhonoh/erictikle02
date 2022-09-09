using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SKT.Glossary.Dac
{
    public class GlossaryMainDac
    {
        private const string connectionStringName = "ConnGlossary";

        public DataSet BasicInfoSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_BasicInfo_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        //오늘 등록된 문서count 와 전체 갯수를 보여준다.
        public DataSet CountTodayTotalSelect()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_CountTodayTotalSelect");

            return db.ExecuteDataSet(cmd);
        }

        //hits 수가 높은순으로 가져온다
        public DataSet TopHitsList(int Count)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TopViewsList");
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);

            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다.
        public DataSet TotalActivity(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 끌모임 게시글 검색 기능 추가
        public DataSet TotalActivity_GathringSearch(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchText, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_GathringSearch");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "SearchText", DbType.String, SearchText);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (플랫폼만)
        public DataSet TotalActivity_Platform(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_Platform");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (마케팅만)
        public DataSet TotalActivity_Marketing(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_Marketing");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (기술트렌드만)
        public DataSet TotalActivity_TechTrend(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_TechTrend");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (DT블로그)
        public DataSet TotalActivity_DT(string wType, string UserID, int PageNum, int Count, string Mode, string SearchSort, string SearchText)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_DT");
            db.AddInParameter(cmd, "WType", DbType.String, wType);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "SearchText", DbType.String, SearchText);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (T생활백서)
        public DataSet TotalActivity_TW(string tType, string UserID, int PageNum, int Count, string Mode, string SearchSort, string SearchText)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_TW");
            db.AddInParameter(cmd, "TType", DbType.String, tType);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "SearchText", DbType.String, SearchText);
            return db.ExecuteDataSet(cmd);
        }

        //모든 활동을 가져온다. (Tnet에서 링크타고 넘어온 경우만)
        public DataSet TotalActivity_Tnet(string UserID, int PageNum, int Count, string Mode, string CategoryID, string TagTitle, string SearchSort, string GatheringYN = "N", string GatheringID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity_Tnet");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            //db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            //db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            //db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            //db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            //db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            //db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            //db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetDeptList(int randindex)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Profile_DeptList");
            db.AddInParameter(cmd, "RandIndex", DbType.Int32, randindex);
            return db.ExecuteDataSet(cmd);
        }

        //날짜별로 그날의 부서 정보 인덱스 를 가져온다.
        public DataSet GetDeptIndex(int randindex)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Main_RandDeptIndex");
            db.AddInParameter(cmd, "RandIndex", DbType.Int32, randindex);
            return db.ExecuteDataSet(cmd);
        }

        //명예의 전당. 리스트 가져오기.
        public DataSet GlossaryHallofFameList(int PageNum, int PageSize, string UserID, string searchType, string CategoryID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_HallofFame_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "searchType", DbType.String, searchType);

            // 2014-06-24 Mr.No
            if (CategoryID != string.Empty)
            {
                db.AddInParameter(cmd, "CategoryID", DbType.Int32, CategoryID);
            }

            return db.ExecuteDataSet(cmd);
        }

        //2015-10-21 마케팅 유저 확인
        public DataSet MarketingUserCheck(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("UP_MAIN_MARKETING_USER_CHECK");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        //2015-10-23 임원 확인
        public DataSet OfficerCheck(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("UP_Main_Officer_Check");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        //메인
        public DataSet GlossaryMainNotice(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Main_Notice");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            //db.AddInParameter(cmd, "Count", DbType.Int32, PageCount);

            return db.ExecuteDataSet(cmd);
        }

        // 티넷 포틀릿 - DT블로그
        public DataSet GlossaryInterfaceTnet()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Interface_Tnet");

            return db.ExecuteDataSet(cmd);
        }
    }
}
