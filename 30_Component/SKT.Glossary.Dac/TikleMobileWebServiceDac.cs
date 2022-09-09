using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

using SKT.Glossary.Type;


namespace SKT.Glossary.Dac
{
    public class TikleMobileWebServiceDac
    {
        private const string connectionStringName = "ConnGlossary";

        /***************  TIKLE ***************/  

        #region TotalActivity
        //모든 활동을 가져온다.
        public DataSet TotalActivity(string LoginUserID, string CategoryID, int PageNum, int Count, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Main_TotalActivity");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region TotalActivityCategory
        public DataSet TotalActivityCategory(string LoginUserID, string CategoryID, int PageNum, int Count, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleMobile_Main_TotalActivity");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "CategoryID", DbType.String, CategoryID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region PlatformTechTrendSelect
        //2015-11-26 플랫폼 테크 트렌드 List 추가
        public DataSet PlatformTechTrendSelect(string LoginUserID, int PageNum, int Count, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleMobile_PlatTrend_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "Count", DbType.Int32, Count);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region PlatTrendUserPermission
        //2015-12-04 플랫폼 테크 트렌드 권한 확인
        public DataSet PlatTrendUserPermission(string LoginUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleMobile_PlatTrend_List_Permission");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossarySelect
        public DataSet GlossarySelect(string ItemID, string UserID, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ItemID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryViewMenuSelect
        public DataSet GlossaryViewMenuSelect(string UserID, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryViewMenu_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GlossaryID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryTagList
        public DataSet GlossaryTagList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryLikeInsert
        //좋아요 추가
        public DataSet GlossaryLikeInsert(GlossaryControlType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Like_Insert");
            db.AddInParameter(cmd, "GlossaryID", DbType.String, Board.GlossaryID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "LikeY", DbType.String, Board.LikeY);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryAlarmInsert
        //알람 추가 
        public DataSet GlossaryAlarmInsert(GlossaryControlType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Alarm_Insert");
            db.AddInParameter(cmd, "NoteYN", DbType.String, Board.NoteYN);
            db.AddInParameter(cmd, "MailYN", DbType.String, Board.MailYN);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryScrapInsert
        //스크랩 추가
        public DataSet GlossaryScrapInsert(GlossaryScrapType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Scrap_Insert");
            db.AddInParameter(cmd, "GlossaryID", DbType.String, Board.GlossaryID);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "YouUserID", DbType.String, Board.YouUserID);
            db.AddInParameter(cmd, "ScrapsYN", DbType.String, Board.ScrapsYN);
            db.AddInParameter(cmd, "NoteYN", DbType.String, Board.NoteYN);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnAList
        //QnA 리스트
        public DataSet GlossaryQnAList(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnASelect
        //QnA 뷰
        public DataSet GlossaryQnASelect(string ID, int count)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "Count", DbType.Int32, count);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnACommentSuccessSelect
        //QnA 베스트 댓글 select 
        public DataSet GlossaryQnACommentSuccessSelect(string ItemID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommentSuccess_Select");
            db.AddInParameter(cmd, "ItemID", DbType.String, ItemID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnABastCommentList
        //QnA 베스트 댓글 리스트
        public DataSet GlossaryQnABastCommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnABestComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnACommentList
        //QnA 댓글 리스트
        public DataSet GlossaryQnACommentList(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnAComment_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnABastSuccessComment
        //QnA 베스트 댓글
        public DataSet GlossaryQnABastSuccessComment(GlossaryQnACommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommentBest_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region UserSelect
        //사용자 정보 가져오기
        public DataSet UserSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_UserInfo_Select");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GetPicture
        //사용자 사진 가져오기.
        public string GetPicture(string UserID)
        {

            Database db2 = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd2 = db2.GetStoredProcCommand("up_UserInfo_Photo");
            db2.AddInParameter(cmd2, "UserID", DbType.String, UserID);
            DataSet dsPhotoUrl = db2.ExecuteDataSet(cmd2);
            if (dsPhotoUrl.Tables.Count > 0 && dsPhotoUrl.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPhotoUrl.Tables[0].Rows[0];
                return dr["photoURL"].ToString();
            }
            return "/common/images/user_none.png";
        } 
        #endregion

        #region GlossaryQnACommentInsert
        //QnA 댓글 추가
        public DataSet GlossaryQnACommentInsert(GlossaryQnACommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnAComment_Insert");
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "PhotoUrl", DbType.String, Board.PhotoUrl);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "LikeCount", DbType.String, Board.LikeCount);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "PublicYN", DbType.String, Board.PublicYN);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnACommentUpdate
        //QnA 댓글 수정
        public DataSet GlossaryQnACommentUpdate(GlossaryQnACommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnAComment_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnACommentLikeY
        //QnA 추천 추가
        public DataSet GlossaryQnACommentLikeY(GlossaryQnACommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommentLike_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "LikeY", DbType.String, "Y");
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryScrapList
        //스크랩 리스트
        public DataSet GlossaryScrapList(int PageNum, int PageSize, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Scrap_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryQnACommentSelect
        //QnA 댓글 뷰
        public DataSet GlossaryQnACommentSelect(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnAComment_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region NoteQnaYNSelect
        //QnA 댓글 쪽지 YN select 
        public DataSet NoteQnaYNSelect(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_NoteQnaYNSelect_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryMobileSearch
        // 티끌 검색
        public DataSet GlossaryMobileSearch(string LoginUserID, int PageNum, int PageSize, string SearchKeyword, string SearchSort)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMobile_Search");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);

            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GlossaryMobileSearch_PlatTrend
        // 티끌 검색(Platform, TechTrend)
        public DataSet GlossaryMobileSearch_PlatTrend(string LoginUserID, int PageNum, int PageSize, string SearchKeyword, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMobile_Search_PlatTrend");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region ScrapMobileSearch
        //스크랩 검색
        public DataSet ScrapMobileSearch(int PageNum, int PageSize, string SearchKeyword, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_ScrapMobile_Search");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region InsertEventAttendance - 사용안함
        //count
        public void InsertEventAttendance(GlossaryPageRequestType gprt)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_EventAttendance_Insert");
            db.AddInParameter(cmd, "@UserID", DbType.String, gprt.UserID);
            db.AddInParameter(cmd, "@Type", DbType.String, gprt.Type);
            db.ExecuteNonQuery(cmd);
        } 
        #endregion

        #region Permissions_Check
        /// <summary>
        /// Permisstion Check
        /// </summary>
        public int Permissions_Check(string ItemID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Check");

            db.AddInParameter(dbCommand, "ItemID", DbType.String, ItemID);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        } 
        #endregion

        #region 카테고리 목록

        public DataSet CategoryList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            //DbCommand cmd = db.GetStoredProcCommand("up_GlossaryCategory_List");
            DbCommand cmd = db.GetStoredProcCommand("up_TikleMobile_GlossaryCategory_List");
            return db.ExecuteDataSet(cmd);
        }

        #endregion

        #region 첨부파일 목록

        public DataSet AttachmentList(int ItemID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Select");

            db.AddInParameter(cmd, "ItemID", DbType.Int64, ItemID); // 티끌 CommonID
            db.AddInParameter(cmd, "BoardID", DbType.Int32, 314); // BoardID는 314로 고정되어있음.. 티끌 유선 소스에..
            return db.ExecuteDataSet(cmd);
        }

        #endregion

        #region 랭킹 정보

        public DataSet RankingInfo(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_ScoreRanking_Select");

            db.AddInParameter(cmd, "User_ID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        #endregion

        /////////////////////////////////
        //2016-06 추가 Method
        /////////////////////////////////

        #region TikleCommentInsert
        //댓글 추가
        public DataSet TikleCommentInsert(CommCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Insert");
            db.AddInParameter(cmd, "commType", DbType.String, Board.COMMENTTYPE); //구분
            db.AddInParameter(cmd, "commIdx", DbType.String, Board.COMMONID); //해당글코드
            db.AddInParameter(cmd, "userID", DbType.String, Board.USERID); //등록한사용자
            db.AddInParameter(cmd, "contents", DbType.String, Board.CONTENTS); //내용
            db.AddInParameter(cmd, "publicYN", DbType.String, Board.PUBLICYN); //비공개여부
            db.AddInParameter(cmd, "userIP", DbType.String, Board.USERIP); //접속IP
            db.AddInParameter(cmd, "userMachineName", DbType.String, Board.USERMACHINENAME); //접속머신
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region TikleReCommentInsert
        //댓글 추가
        public DataSet TikleReCommentInsert(CommCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Insert_Sup");
            db.AddInParameter(cmd, "SUP_ID", DbType.String, Board.ID); //구분
            db.AddInParameter(cmd, "commType", DbType.String, Board.COMMENTTYPE); //구분
            db.AddInParameter(cmd, "commIdx", DbType.String, Board.COMMONID); //해당글코드
            db.AddInParameter(cmd, "userID", DbType.String, Board.USERID); //등록한사용자
            db.AddInParameter(cmd, "contents", DbType.String, Board.CONTENTS); //내용
            db.AddInParameter(cmd, "publicYN", DbType.String, Board.PUBLICYN); //비공개여부
            db.AddInParameter(cmd, "userIP", DbType.String, Board.USERIP); //접속IP
            db.AddInParameter(cmd, "userMachineName", DbType.String, Board.USERMACHINENAME); //접속머신
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region TikleCommentModify
        //댓글 수정
        public DataSet TikleCommentModify(CommCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.CONTENTS);
            db.AddInParameter(cmd, "PublicYN", DbType.String, Board.PUBLICYN);
            db.AddInParameter(cmd, "LASTMODIFIERID", DbType.String, Board.USERID);
            db.AddInParameter(cmd, "LASTMODIFIERIP", DbType.String, Board.USERIP);
            db.AddInParameter(cmd, "LASTMODIFIERMACHINENAME", DbType.String, Board.USERMACHINENAME);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region TikleCommentDelete
        //댓글 삭제
        public DataSet TikleCommentDelete(CommCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "LASTMODIFIERID", DbType.String, Board.LASTMODIFIERID);
            db.AddInParameter(cmd, "LASTMODIFIERIP", DbType.String, Board.LASTMODIFIERID);
            db.AddInParameter(cmd, "LASTMODIFIERMACHINENAME", DbType.String, Board.LASTMODIFIERMACHINENAME);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region QNAInsert
        //댓글 추가
        public DataSet QNAInsert(GlossaryQnAType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Insert");
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
            db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "ItemState", DbType.String, Board.ItemState);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "PlatformYN", DbType.String, Board.PlatformYN);
            db.AddInParameter(cmd, "MarketingYN", DbType.String, Board.MarketingYN);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region QNAModify
        //댓글 추가
        public DataSet QNAModify(GlossaryQnAType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Update");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
            db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
            db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
            db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
            db.AddInParameter(cmd, "UserEmail", DbType.String, Board.UserEmail);
            db.AddInParameter(cmd, "commonid", DbType.String, Board.CommonID);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region QNADelete
        public void QNADelete(string LoginUserID, string QNAID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Delete");
            db.AddInParameter(cmd, "@ID", DbType.String, QNAID);
            db.AddInParameter(cmd, "@WHOIDDEL", DbType.String, LoginUserID);
            db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region GatheringList
        public DataSet GatheringList(string LoginUserID,string Mode, int PageNum, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGathering_List_By_Mobile");

            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region GatheringNoticeList
        public DataSet GatheringNoticeList(string LoginUserID,string GatheringID, int PageNum, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringNoticeList_By_Mobile");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region GatheringNoticeInsert, GatheringNoticeModify 겸용 (구분 = 모드 ("","History"))
        public DataSet GatheringNoticeInsert(GlossaryType Board,string mode)
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
            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "HistoryYN", DbType.String, Board.HistoryYN);
            db.AddInParameter(cmd, "fromQnaID", DbType.String, Board.fromQnaID);
            db.AddInParameter(cmd, "CategoryID", DbType.Int32, Board.CategoryID);
            db.AddInParameter(cmd, "Permissions", DbType.String, Board.Permissions);
            db.AddInParameter(cmd, "PlatformYN", DbType.String, Board.PlatformYN);
            db.AddInParameter(cmd, "MarketingYN", DbType.String, Board.MarketingYN);
            db.AddInParameter(cmd, "TechTrendYN", DbType.String, Board.TechTrendYN);
            db.AddInParameter(cmd, "JustOfficerYN", DbType.String, Board.JustOfficerYN);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region MarketingTikleList
        public DataSet MarketingTikleList(string LoginUserID, int PageNum, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMarketingTikleList_By_Mobile");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region MarketingQnaList
        //QnA 리스트
        public DataSet MarketingQnaList(string LoginUserID, int PageIndex, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_List_Marketing");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageIndex);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region GlossaryCommentSelect
        public DataSet GlossaryCommentSelect(string CommonID, string GlossaryType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_TikleGlossaryComment_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            db.AddInParameter(cmd, "COMMENTTYPE", DbType.String, GlossaryType);
            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GatheringMobileNotiInsert
        public DataSet GatheringMobileNotiInsert(string LoginUserID, int GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMobileNoti_Insert");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region GatheringMobileNotiDelete
        public DataSet GatheringMobileNotiDelete(string LoginUserID, int GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMobileNoti_Delete");
            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        /***************  WEEKLY ***************/

        #region SelectWeekly
        //<!--2015.03 수정 -->
        public DataSet SelectWeekly(string userID, string deptCode, DateTime weekDateTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_By_Mobile");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, weekDateTime);

            return db.ExecuteDataSet(dbCommand);
        }
        #endregion

        #region InsertWeeklyByMobile
        public void InsertWeeklyByMobile(WeeklyType weeklyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Insert_By_Mobile");

            db.AddInParameter(cmd, "UserID", DbType.String, weeklyType.UserID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, weeklyType.Contents);
            db.AddInParameter(cmd, "TextContents", DbType.String, weeklyType.TextContents);
            db.AddInParameter(cmd, "Date", DbType.DateTime, weeklyType.Date);

            //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
            db.AddInParameter(cmd, "Year", DbType.Int32, weeklyType.Year);
            db.AddInParameter(cmd, "YearWeek", DbType.Int32, weeklyType.YearWeek);
            db.AddInParameter(cmd, "StartWeekDate", DbType.DateTime, weeklyType.StartWeekDate);
            db.AddInParameter(cmd, "EndWeekDate", DbType.DateTime, weeklyType.EndWeekDate);


            db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region UpdateWeeklyByMobile
        public void UpdateWeeklyByMobile(WeeklyType weeklyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Update_By_Mobile");

            db.AddInParameter(cmd, "WeeklyID", DbType.String, weeklyType.WeeklyID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, weeklyType.DeptCode);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, weeklyType.Contents);
            db.AddInParameter(cmd, "TextContents", DbType.String, weeklyType.TextContents);
            db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region SelectWeeklyAllComment (X)
		public DataSet SelectWeeklyAllComment(long weeklyID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_WeeklyComment_SelectAll_Weekly");

            db.AddInParameter(cmd, "weeklyID", DbType.Int64, weeklyID);

            return db.ExecuteDataSet(cmd);
        } 
	    #endregion

        #region InsertWeeklyComment
		public DataSet InsertWeeklyComment(WeeklyCommentType commentType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_WeeklyComment_Insert");
            db.AddInParameter(cmd, "SUP_ID", DbType.Int64, commentType.SUP_ID);
            db.AddInParameter(cmd, "WeeklyID", DbType.String, commentType.WeeklyID);
            db.AddInParameter(cmd, "Contents", DbType.String, commentType.Contents);
            db.AddInParameter(cmd, "UserID", DbType.String, commentType.UserID);
            db.AddInParameter(cmd, "UserName", DbType.String, commentType.UserName);
            db.AddInParameter(cmd, "DeptName", DbType.String, commentType.DeptName);
            db.AddInParameter(cmd, "DutyName", DbType.String, commentType.DutyName);
            return db.ExecuteDataSet(cmd);
        } 
	    #endregion

        #region UpdateWeeklyComment
		public void UpdateWeeklyComment(WeeklyCommentType commentType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_WeeklyComment_Update");
            db.AddInParameter(cmd, "WeeklyCommentID", DbType.Int64, commentType.WeeklyCommentID);
            db.AddInParameter(cmd, "Contents", DbType.String, commentType.Contents);
            db.ExecuteNonQuery(cmd);
        } 
	    #endregion

        #region DeleteWeeklyComment
		public void DeleteWeeklyComment(Int64 weeklycommentID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_WeeklyComment_Delete");
            db.AddInParameter(cmd, "WeeklyCommentID", DbType.Int64, weeklycommentID);
            db.ExecuteNonQuery(cmd);
        } 
	    #endregion

        #region SelectWeeklyDeptCode
        public DataSet SelectWeeklyDeptCode(string userID, string deptCode, DateTime weekDate, int pageNum, int pageSize, string TempFG)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_DeptCode");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, weekDate);
            db.AddInParameter(dbCommand, "PageNum", DbType.Int32, pageNum);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "TempFG", DbType.String, TempFG);
            return db.ExecuteDataSet(dbCommand);
        } 
	    #endregion

        #region SelectWeeklyExecutive
        //임원일경우 목록 - 팀까지 나옴
        public DataSet SelectWeeklyExecutive(string userID, string deptCode, DateTime weekDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_DeptCode_Officer");

            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "WeekDateTime", DbType.DateTime, weekDate);
            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            return db.ExecuteDataSet(dbCommand);
        }
        #endregion

        #region SelectAdditionalJob
        public DataSet SelectAdditionalJob(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Weekly_Select_AdditionalJob");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            return db.ExecuteDataSet(dbCommand);
        }
        #endregion

        #region WeeklyMobileTemp_Select
        /* 2015.04 MYWEEKLY */
        public DataSet WeeklyMobileTemp_Select(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_WeeklyMobileTemp_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            return db.ExecuteDataSet(dbCommand);
        }
        /* 2015.04 MYWEEKLY */
        #endregion

        #region Outlook WebMethod
        public void WeeklyInsertByOutlook(string email, string htmlContents, string textContents)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Insert_By_Outlook");

            db.AddInParameter(cmd, "UserEmail", DbType.String, email);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, htmlContents);
            db.AddInParameter(cmd, "TextContents", DbType.String, textContents);
            db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region Nateon WebMethod
        public void WeeklyInsertByNateon(string email, string htmlContents, string textContents)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Insert_By_Nateon");

            db.AddInParameter(cmd, "UserEmail", DbType.String, email);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, htmlContents);
            db.AddInParameter(cmd, "TextContents", DbType.String, textContents);
            db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region WeeklyDeleteByMobile
        public void WeeklyDeleteByMobile(string weeklyID, string LoginUserID = null)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Delete");

            db.AddInParameter(cmd, "WeeklyID", DbType.String, weeklyID);
            db.AddInParameter(cmd, "DeleteType", DbType.String, "M");
            db.AddInParameter(cmd, "DeleteUserID", DbType.String, LoginUserID);

            db.ExecuteNonQuery(cmd);
        }
        #endregion

        /////////////////////////////////
        //2016-06 추가 Method
        /////////////////////////////////
        #region GetMemberWeekly
        public DataSet GetMemberWeekly(string UserID, string deptCode, string userFG, DateTime weekDateTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Select_By_Mobile2");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(cmd, "UserFG", DbType.String, userFG);
            db.AddInParameter(cmd, "Date", DbType.DateTime, weekDateTime);

            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region GetDeptMonthlyList
        public DataSet GetDeptMonthlyList(string LoginUserID, string deptCode, string weekDate, int pagenum, int pagecount)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Select_DeptCode");

            DateTime tmpDate = Convert.ToDateTime(weekDate);
            string Monthly_YYYY = tmpDate.ToString("yyyyMM");
            string Monthly_MM = tmpDate.ToString("MM");
            string Monthly_YYYYMM = tmpDate.ToString("yyyyMM");

            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(cmd, "Date", DbType.DateTime, weekDate);

            db.AddInParameter(cmd, "PageNum", DbType.Int32, pagenum);
            db.AddInParameter(cmd, "Count", DbType.Int32, pagecount);

            db.AddInParameter(cmd, "MonthlyYYYY", DbType.String, Monthly_YYYY);
            db.AddInParameter(cmd, "MonthlyMM", DbType.String, Monthly_MM);
            db.AddInParameter(cmd, "MonthlyYYYYMM", DbType.String, Monthly_YYYYMM);

            return db.ExecuteDataSet(cmd);
        } 
        #endregion

        #region InsertMonthlyByMobile
        public DataSet InsertMonthlyByMobile(MonthlyType monthlyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Insert_By_Mobile");

            db.AddInParameter(cmd, "UserID", DbType.String, monthlyType.UserID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, monthlyType.DeptCode);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, monthlyType.Contents);
            db.AddInParameter(cmd, "TextContents", DbType.String, monthlyType.TextContents);
            db.AddInParameter(cmd, "Date", DbType.DateTime, monthlyType.Date);

            //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
            db.AddInParameter(cmd, "Year", DbType.Int32, monthlyType.Year);
            db.AddInParameter(cmd, "YearWeek", DbType.Int32, monthlyType.YearWeek);
            db.AddInParameter(cmd, "StartWeekDate", DbType.DateTime, monthlyType.StartWeekDate);
            db.AddInParameter(cmd, "EndWeekDate", DbType.DateTime, monthlyType.EndWeekDate);

            DateTime tmpDate = monthlyType.Date;
            string Monthly_YYYY = tmpDate.ToString("yyyyMM");
            string Monthly_MM = tmpDate.ToString("MM");
            string Monthly_YYYYMM = tmpDate.ToString("yyyyMM");

            db.AddInParameter(cmd, "MonthlyYYYY", DbType.String, Monthly_YYYY);
            db.AddInParameter(cmd, "MonthlyMM", DbType.String, Monthly_MM);
            db.AddInParameter(cmd, "MonthlyYYYYMM", DbType.String, Monthly_YYYYMM);    

            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region UpdateMonthlyByMobile
        public DataSet UpdateMonthlyByMobile(MonthlyType monthlyType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Update_By_Mobile");

            db.AddInParameter(cmd, "WeeklyID", DbType.String, monthlyType.WeeklyID);
            db.AddInParameter(cmd, "DeptCode", DbType.String, monthlyType.DeptCode);
            db.AddInParameter(cmd, "HtmlContents", DbType.String, monthlyType.Contents);
            db.AddInParameter(cmd, "TextContents", DbType.String, monthlyType.TextContents);
            
            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region DeleteMonthlyByMobile
        public DataSet DeleteMonthlyByMobile(string monthlyID, string LoginUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Monthly_Delete");

            db.AddInParameter(cmd, "WeeklyID", DbType.String, monthlyID);
            db.AddInParameter(cmd, "DeleteType", DbType.String, "M");
            db.AddInParameter(cmd, "DeleteUserID", DbType.String, LoginUserID);

            return db.ExecuteDataSet(cmd);
        }
        #endregion

        #region SelectMonthly
        public DataSet SelectMonthly(string userID, string deptCode, DateTime weekDate)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Monthly_Select_By_Mobile");

            DateTime tmpDate = Convert.ToDateTime(weekDate);
            string Monthly_YYYY = tmpDate.ToString("yyyyMM");
            string Monthly_MM = tmpDate.ToString("MM");
            string Monthly_YYYYMM = tmpDate.ToString("yyyyMM");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, weekDate);

            db.AddInParameter(dbCommand, "MonthlyYYYY", DbType.String, Monthly_YYYY);
            db.AddInParameter(dbCommand, "MonthlyMM", DbType.String, Monthly_MM);
            db.AddInParameter(dbCommand, "MonthlyYYYYMM", DbType.String, Monthly_YYYYMM);

            return db.ExecuteDataSet(dbCommand);
        }
        #endregion

        #region TikleAccessLog
        public int TikleAccessLog(string LoginID, string IP, string HostName)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_LogAccess_Insert_moblie");

            db.AddInParameter(cmd, "UserID", DbType.String, LoginID);
            db.AddInParameter(cmd, "AccessIP", DbType.String, IP);
            db.AddInParameter(cmd, "AccessHost", DbType.String, HostName);

            return db.ExecuteNonQuery(cmd);
        }
        #endregion

        #region GetWeeklyExceptionList
        public DataSet GetWeeklyExceptionList(string LoginUserID, DateTime weekDateTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Weekly_Select_Exception_User");

            db.AddInParameter(cmd, "UserID", DbType.String, LoginUserID);
            //db.AddInParameter(cmd, "DeptCode", DbType.String, deptCode);
            db.AddInParameter(cmd, "WeekDateTime", DbType.DateTime, weekDateTime);

            return db.ExecuteDataSet(cmd);
        }
        #endregion
    }
}