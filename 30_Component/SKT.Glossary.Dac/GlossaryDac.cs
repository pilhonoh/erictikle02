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
    public class GlossaryDac
    {

        private const string connectionStringName = "ConnGlossary";

        //제목 리스트
        public DataSet GlossaryTitleList(int PageNum, int PageSize, string SearchKeyword)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTitle_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            return db.ExecuteDataSet(cmd);
        }

        //내용 리스트
        public DataSet GlossaryContentsList(int PageNum, int PageSize, string SearchKeyword)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryContents_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            return db.ExecuteDataSet(cmd);
        }

        //용어 추가
        public DataSet GlossaryInsert(GlossaryType Board, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "Type", DbType.String, Board.Type);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
            db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
            db.AddInParameter(cmd, "Description", DbType.String, Board.Description);
            db.AddInParameter(cmd, "PrivateYN", DbType.String, Board.PrivateYN);
            db.AddInParameter(cmd, "HistoryYN", DbType.String, Board.HistoryYN);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "fromQnaID", DbType.String, Board.fromQnaID);
            // 2014-04-29 Mr.No 추가
            db.AddInParameter(cmd, "CategoryID", DbType.Int32, Board.CategoryID);
            // 2014-05-12 Mr.No 추가
            db.AddInParameter(cmd, "Permissions", DbType.String, Board.Permissions);
            // 2015-09-22 Kim 추가
            db.AddInParameter(cmd, "PlatformYN", DbType.String, Board.PlatformYN);
            // 2015-10-13 Kim 추가
            db.AddInParameter(cmd, "MarketingYN", DbType.String, Board.MarketingYN);
            // 2015-10-27 Kim 추가
            db.AddInParameter(cmd, "TechTrendYN", DbType.String, Board.TechTrendYN);
            // 2015-11-09 Kim 추가
            db.AddInParameter(cmd, "JustOfficerYN", DbType.String, Board.JustOfficerYN);
            // DT블로그홈
            if (!String.IsNullOrEmpty(Board.DTBlogFlag))
            {
                db.AddInParameter(cmd, "DTBlogFlag", DbType.String, Board.DTBlogFlag);
            }
            // CHG610000074852/ 2018-11-08 / 최현미 / T생활백서  
            if (!String.IsNullOrEmpty(Board.TWhiteFlag))
            {
                db.AddInParameter(cmd, "TWhiteFlag", DbType.String, Board.TWhiteFlag);
            }
            return db.ExecuteDataSet(cmd);

        }

        //용어 Common 추가 업데이트
        public DataSet GlossaryCommonIDUpate(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Update");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //용어 보기
        public DataSet GlossarySelect(string ItemID, string UserID, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ItemID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }


        //내문서 리스트
        public DataSet GlossaryMyDocumentsList(int PageNum, int PageSize, string UserID, string SearchType)
        {
            // 내가 작성한 지식 리스트를 임시저장 리스트로 변경

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            //DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyDocument_List");
            //db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            //db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            //db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            //db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            //return db.ExecuteDataSet(cmd);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTemp_List");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            return db.ExecuteDataSet(cmd);
        }

        //내문서 보기
        public DataSet GlossaryMyDocumentSelect(string ItemID, string HistoryYN)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyDocument_Select");
            db.AddInParameter(cmd, "ItemID", DbType.String, ItemID);
            db.AddInParameter(cmd, "HistoryYN", DbType.String, HistoryYN);
            return db.ExecuteDataSet(cmd);
        }

        //히스토리 되돌리기 설정
        public DataSet GlossaryRevert(string RevertItemID, string Description)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryRevert_Update");
            db.AddInParameter(cmd, "RevertItemID", DbType.String, RevertItemID);
            db.AddInParameter(cmd, "Description", DbType.String, Description);
            return db.ExecuteDataSet(cmd);

        }

        //검색결과 자동 완성
        public DataSet SearchAutoList(string Search, string Type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SearchAuto_List");
            db.AddInParameter(cmd, "SearchWord", DbType.String, Search);
            db.AddInParameter(cmd, "Type", DbType.String, Type);
            return db.ExecuteDataSet(cmd);
        }

        //목록보기 ModifyYN 수정
        public DataSet GlossaryModifyYNUpdate(string ID, string Type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryModifyYN_Update");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "Type", DbType.String, Type);
            return db.ExecuteDataSet(cmd);
        }

        //검색어가 사용자 일 경우
        public DataSet GlossaryProfileSearch(string SearchKeyword)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Profile_Search");
            db.AddInParameter(cmd, "@SearchWord", DbType.String, SearchKeyword);
            return db.ExecuteDataSet(cmd);
        }

        //테그리스트 가져오기.
        public DataSet GetTagList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Select");
            db.AddInParameter(cmd, "@CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //closebetauser 존재여부체크
        public DataSet GetCBTUserList(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CBTUser_List");
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetEventData(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventData_Select");
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetEventRankList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventRank_List");
            return db.ExecuteDataSet(cmd);
        }
        public DataSet GetEventReplyRankList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventReplyRank_List");
            return db.ExecuteDataSet(cmd);
        }

        public DataSet InsertEventAttendance(GlossaryPageRequestType gprt)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventAttendance_Insert");
            db.AddInParameter(cmd, "@UserID", DbType.String, gprt.UserID);
            db.AddInParameter(cmd, "@UserName", DbType.String, gprt.Name);
            db.AddInParameter(cmd, "@SessionID", DbType.String, gprt.SessionID);
            db.AddInParameter(cmd, "@UrlBefore", DbType.String, gprt.UrlBefore);
            db.AddInParameter(cmd, "@UrlCurrent", DbType.String, gprt.UrlCurrent);
            db.AddInParameter(cmd, "@PathCurrent", DbType.String, gprt.PathCurrent);
            return db.ExecuteDataSet(cmd);
        }

        // Mr.No 2016-01-26
        public DataSet InsertEventAttendance_New(GlossaryPageRequestType gprt, string SmUser, string ip)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_New_EventAttendance_Insert");
            db.AddInParameter(cmd, "@UserID", DbType.String, gprt.UserID);
            db.AddInParameter(cmd, "@UserName", DbType.String, gprt.Name);
            db.AddInParameter(cmd, "@SessionID", DbType.String, gprt.SessionID);
            db.AddInParameter(cmd, "@UrlBefore", DbType.String, gprt.UrlBefore);
            db.AddInParameter(cmd, "@UrlCurrent", DbType.String, gprt.UrlCurrent);
            db.AddInParameter(cmd, "@PathCurrent", DbType.String, gprt.PathCurrent);
            db.AddInParameter(cmd, "@SmUser", DbType.String, SmUser);
            db.AddInParameter(cmd, "@IpAddress", DbType.String, ip);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet Insert_LW_EventAttendance(GlossaryLoginType glt)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventAttendance_LW_Insert");
            db.AddInParameter(cmd, "@UserID", DbType.String, glt.UserID);
            db.AddInParameter(cmd, "@UserName", DbType.String, glt.Name);
            db.AddInParameter(cmd, "@SessionID", DbType.String, glt.SessionID);
            db.AddInParameter(cmd, "@UrlBefore", DbType.String, glt.UrlBefore);
            db.AddInParameter(cmd, "@UrlCurrent", DbType.String, glt.UrlCurrent);
            db.AddInParameter(cmd, "@PathCurrent", DbType.String, glt.PathCurrent);
            db.AddInParameter(cmd, "@LoginType", DbType.String, glt.LoginType);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetDocHistoryUsers(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_DocHistoryUsers");
            db.AddInParameter(cmd, "@CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        public void TikleDelete(string userid, string commonid, string userip, string usermacnhinename)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_DeleteAll");
            db.AddInParameter(cmd, "@WHOIDDEL", DbType.String, userid);
            db.AddInParameter(cmd, "@COMMONID", DbType.String, commonid);
            db.AddInParameter(cmd, "@WHOIPDEL", DbType.String, userip);
            db.AddInParameter(cmd, "@WHOMACHINENAMEDEL", DbType.String, usermacnhinename);
            db.ExecuteNonQuery(cmd);
        }

        //20140102 , qna 삭제
        public void QnaDelete(string id, string userid, string userip, string usermacnhinename)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Delete");
            db.AddInParameter(cmd, "@ID", DbType.String, id);
            db.AddInParameter(cmd, "@WHOIDDEL", DbType.String, userid);
            db.AddInParameter(cmd, "@WHOIPDEL", DbType.String, userip);
            db.AddInParameter(cmd, "@WHOMACHINENAMEDEL", DbType.String, usermacnhinename);
            db.ExecuteNonQuery(cmd);
        }

        public void GlossaryHallOfFameInsert(string Mode, GlossaryHallOfFameType data)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_HALLOFFAME_INSERT");
            db.AddInParameter(cmd, "@MODE", DbType.String, Mode);
            db.AddInParameter(cmd, "@GLOSSARYID", DbType.String, data.GlossaryID);
            db.AddInParameter(cmd, "@USERID", DbType.String, data.CreateUserID);
            db.AddInParameter(cmd, "@USERIP", DbType.String, data.CreateUserIP);
            db.AddInParameter(cmd, "@USERMACHINENAME", DbType.String, data.CreateUserMachineName);
            db.ExecuteNonQuery(cmd);

        }
        public DataSet GlossaryMainInfoSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMain_Select_New");

            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryMainInfoSelect(string GatheringYN, string GatheringID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMain_Select");

            db.AddInParameter(cmd, "@GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "@GatheringID", DbType.String, GatheringID);
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetGlossaryMainTagBoardSelect(string Board_Index, string Board_Count, string Board_RowCount, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMainTagBoard_Select");
            db.AddInParameter(cmd, "@Board_Index", DbType.String, Board_Index);
            db.AddInParameter(cmd, "@Board_Count", DbType.String, Board_Count);
            db.AddInParameter(cmd, "@Board_RowCount", DbType.String, Board_RowCount);
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetGlossaryMainTagSelect(string Tag_Index, string Tag_Count, string GatheringYN, string GatheringID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMainTag_Select");
            db.AddInParameter(cmd, "@Tag_Index", DbType.String, Tag_Index);
            db.AddInParameter(cmd, "@Tag_Count", DbType.String, Tag_Count);
            db.AddInParameter(cmd, "@GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "@GatheringID", DbType.String, GatheringID);
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }


        //용어 Common 추가 업데이트
        public int GlossaryPlatformUpdate(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Platform_Update");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteNonQuery(cmd);
        }

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 기본 태그 리스트 가져오기
        public DataSet GetBasicTagList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_basicTagList");
            return db.ExecuteDataSet(cmd);
        }

        

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.06.01 
        //Desc : 최초작성자 = 편집자 여부
        public DataSet GlossaryCreateWriteYN(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_Glossary_Comment_SendNoteYN");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.07.13
        Desc : 삭제하기 버튼 체크 로직 추가
        */
        public DataSet GlossaryDeleteCheck(string CommonID, string UserId)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_DeleteButton_Check");
            db.AddInParameter(cmd, "commonID", DbType.String, CommonID);
            db.AddInParameter(cmd, "userID", DbType.String, UserId);
            return db.ExecuteDataSet(cmd);
        }



        public DataSet GlossaryGatheringItemCheck(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringItem_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// CHG610000059179 / 2018-03-07 / 최현미 / 문의요청 담당자 및 메일링 발송자 리스트
        /// </summary>
        /// <param name="gubun">문의요청='C' / 메일발송 = 'M' / DT블로그 = 'D' </param>
        /// <returns></returns>
        public DataSet GetSpecialUserChargeSelect(string gubun)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SpecialUser_Role_Select");
            db.AddInParameter(cmd, "Gubun", DbType.String, gubun);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Tnet ContentFeeds 저장
        /// </summary>
        /// <param name="sbmid"></param>
        /// <param name="status"></param>
        /// <param name="errormessage"></param>
        public void SetTnetContentFeedsLog(string sbmid, string method, string status, string errormessage, string jsondata)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Log_ContentFeeds_Insert");
            db.AddInParameter(cmd, "@sbmid", DbType.String, sbmid);
            db.AddInParameter(cmd, "@method", DbType.String, method);
            db.AddInParameter(cmd, "@status", DbType.String, status);
            db.AddInParameter(cmd, "@errormessage", DbType.String, errormessage);
            db.AddInParameter(cmd, "@jsondata", DbType.String, jsondata);

            db.ExecuteNonQuery(cmd);
        }
    }
}
