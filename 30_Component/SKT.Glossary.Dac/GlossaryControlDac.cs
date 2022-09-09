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
    public class GlossaryControlDac
    {
        private const string connectionStringName = "ConnGlossary";


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

        //좋아요 카운트
        public DataSet GlossaryLikeSelect(string GlossaryID, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Like_Select");
            db.AddInParameter(cmd, "GlossaryID", DbType.String, GlossaryID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            return db.ExecuteDataSet(cmd);
        }

        ////프로필 리스트
        //public DataSet GlossaryProfileList(GlossaryControlType Board)
        //{
        //    Database db = DatabaseFactory.CreateDatabase(connectionStringName);
        //    DbCommand cmd = db.GetStoredProcCommand("up_Profile_List");
        //    db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
        //    return db.ExecuteDataSet(cmd);
        //}


        //히스토리 check in, check out
        public DataSet GlossaryHistoryModifyYN(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_HistoryModifyYN_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //수정 check in, check out
        public DataSet GlossaryModifyYN(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryModifyYN_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

       

        public DataSet GlossaryViewMenuSelect(string UserID, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryViewMenu_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GlossaryID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //사용자 이름으로 사용자정보 가져오기
        public DataSet UserNameList(string UserName)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_UserInfo_SearchName");

            db.AddInParameter(cmd, "UserName", DbType.String, UserName);

            return db.ExecuteDataSet(cmd);
        }


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

        //알람 신청한 사람 가져오기
        public DataSet GlossaryAlarmSelect(string CommonID, string AlarmType, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Alarm_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "AlarmType", DbType.String, AlarmType);
            return db.ExecuteDataSet(cmd);
        }


        //태그 추가
        public DataSet GlossaryTagInsert(GlossaryControlType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Insert");
            db.AddInParameter(cmd, "TagTitle", DbType.String, Board.TagTitle);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryTagTotal()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTagTotal");
            return db.ExecuteDataSet(cmd);
        }

        

        //태그 삭제
        public DataSet GlossaryTagDelete(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Delete");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //태그 리스트
        public DataSet GlossaryTagSelect(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryTagList(string CommonID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_List");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryLikeNote(string UserID, string GlossaryID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_LikeNote_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GlossaryID", DbType.String, GlossaryID);
            return db.ExecuteDataSet(cmd);
        }

        //유저별 튜토리얼 설정값을가져온다 
        public DataSet TutorialSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tutorial_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        //유저별 튜토리얼 설정값을 설정한다. 
        public DataSet TutorialInsert(TutorialInfo data)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tutorial_Insert");
            db.AddInParameter(cmd, "UserID", DbType.String, data.UserID);
            db.AddInParameter(cmd, "ResultYN", DbType.String, data.ResultYN);
            db.AddInParameter(cmd, "ProfileYN", DbType.String, data.ProfileYN);
            db.AddInParameter(cmd, "FirstWrite", DbType.String, data.FirstWrite);
            db.AddInParameter(cmd, "QNAYN", DbType.String, data.QNAYN);
            return db.ExecuteDataSet(cmd);
        }

        //제목으로 값이 있는지 확인한다.
        public DataSet ExistTitle(string Title,string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTitle_Select");
            db.AddInParameter(cmd, "Title", DbType.String, Title);
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //제목으로 값이 있는지 확인한다.
        public DataSet ExistTitle(string Title, string ID, string GatheringYN, string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTitle_Select");
            db.AddInParameter(cmd, "Title", DbType.String, Title);
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Mr.No 2014-06-03
        /// View 화면에서 태그 [수정/삭제] 기능을 추가하면서
        /// 글 [작성/편집]시에만 insert/delete 가 아닌 Update 를 추가합니다. 
        /// </summary>
        /// <param name="glossaryTagType"></param>
        public void GlossaryTagUpdate(GlossaryTagType glossaryTagType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Tag_Update");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, glossaryTagType.ID);
            db.AddInParameter(dbCommand, "CommonID", DbType.Int32, glossaryTagType.CommonID);
            db.AddInParameter(dbCommand, "Title", DbType.String, glossaryTagType.Title);
            db.AddInParameter(dbCommand, "TagTitle", DbType.String, glossaryTagType.TagTitle);
            db.AddInParameter(dbCommand, "UserID", DbType.String, glossaryTagType.UserID);
            db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, glossaryTagType.CreateDate);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Mr.No 2014-06-03
        /// View 화면에서 태그 하나만 삭제하는 로직
        /// </summary>
        /// <param name="CommonID"></param>
        /// <returns></returns>
        public DataSet GlossaryTagDelete_One(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Delete_One");
            db.AddInParameter(cmd, "ID", DbType.Int32, Convert.ToInt32(ID));
            return db.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 태그 하나 추가
        /// </summary>
        /// <param name="Board"></param>
        /// <returns></returns>
        public GlossaryControlType GlossaryTagInsert_One(GlossaryControlType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Insert_One");
            
            db.AddInParameter(cmd, "TagTitle", DbType.String, Board.TagTitle);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "Title", DbType.String, Board.Title);

            GlossaryControlType glossaryControlType = new GlossaryControlType();
            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        glossaryControlType.ID = (dr["ID"] == DBNull.Value) ? null : Convert.ToString(dr["ID"]);
                        glossaryControlType.TagTitle = (dr["TagTitle"] == DBNull.Value) ? null : Convert.ToString(dr["TagTitle"]);
                    }
                }
            }

            return glossaryControlType;           
        }

        /// <summary>
        /// 연관단어 중복검색
        /// </summary>
        /// <param name="Board"></param>
        /// <returns></returns>
        public GlossaryControlType GlossaryTagRedundancy_Check(string TagTitle, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Tag_Redundancy_Check");

            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "CommonID", DbType.Int32, Convert.ToInt32(CommonID));

            GlossaryControlType glossaryControlType = new GlossaryControlType();
            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        glossaryControlType.ID = (dr["ID"] == DBNull.Value) ? "0" : Convert.ToString(dr["ID"]);
                        //glossaryControlType.TagTitle = (dr["TagTitle"] == DBNull.Value) ? null : Convert.ToString(dr["TagTitle"]);
                    }
                }
            }

            return glossaryControlType;
        }



        public DataSet CommAuthCountUpdate(string idx, string type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("UP_COMMAUTHCOUNTUPDATE");
            db.AddInParameter(cmd, "idx", DbType.String, idx);
            db.AddInParameter(cmd, "type", DbType.String, type);
            return db.ExecuteDataSet(cmd);

        }
        /*
         *댓글 목록 가져오기
         */
        public DataSet CommCommentListSelect(string commType, string commIdx, string userID, int pageNum, int pageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_List");
            db.AddInParameter(cmd, "commType", DbType.String, commType);
            db.AddInParameter(cmd, "commIdx", DbType.String, commIdx);
            db.AddInParameter(cmd, "userID", DbType.String, userID);
            db.AddInParameter(cmd, "pageNum", DbType.Int32, pageNum);
            db.AddInParameter(cmd, "pageSize", DbType.Int32, pageSize);
            return db.ExecuteDataSet(cmd);
        }

        /*
         *댓글 내용 가져오기
         */
        public DataSet CommCommentSelect(string commType, string commIdx, string userID, string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Select");
            db.AddInParameter(cmd, "commType", DbType.String, commType);
            db.AddInParameter(cmd, "commIdx", DbType.String, commIdx);
            db.AddInParameter(cmd, "ID", DbType.Int32, Int32.Parse(ID));
            return db.ExecuteDataSet(cmd);
        }


        //댓글 추가
        public DataSet CommCommentInsert(CommCommentType Board)
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

         //댓글 답글추가
        public DataSet CommCommentSupInsert(CommCommentType Board)
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

        

        //댓글 수정
        public DataSet CommCommentUpdate(CommCommentType Board)
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



        //댓글 삭제
        public DataSet CommCommentDelete(CommCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "LASTMODIFIERID", DbType.String, Board.LASTMODIFIERID);
            db.AddInParameter(cmd, "LASTMODIFIERIP", DbType.String, Board.LASTMODIFIERID);
            db.AddInParameter(cmd, "LASTMODIFIERMACHINENAME", DbType.String, Board.LASTMODIFIERMACHINENAME);
            return db.ExecuteDataSet(cmd);
        }

        /*
         *댓글 내용 가져오기
         */
        public DataSet CommCommentCountSelect(string commType, string commIdx)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Count");
            db.AddInParameter(cmd, "commType", DbType.String, commType);
            db.AddInParameter(cmd, "commIdx", DbType.String, commIdx);
            return db.ExecuteDataSet(cmd);
        }

        

        //추천 추가
        public DataSet CommCommentCountLike(CommCommentType Comment)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_commComment_Like");
            db.AddInParameter(cmd, "ID", DbType.String, Comment.ID);
            db.AddInParameter(cmd, "UserID", DbType.String, Comment.USERID);
            db.AddInParameter(cmd, "userIP", DbType.String, Comment.USERIP); //접속IP
            db.AddInParameter(cmd, "userMachineName", DbType.String, Comment.USERMACHINENAME); //접속머신
            db.AddInParameter(cmd, "LikeY", DbType.String, Comment.LIKECOUNT);
            return db.ExecuteDataSet(cmd);
        }

        
        //추천 추가
        public DataSet CommCommentCountBest(CommCommentType Comment)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_commComment_Best");
            db.AddInParameter(cmd, "ID", DbType.String, Comment.ID);
            db.AddInParameter(cmd, "UserID", DbType.String, Comment.USERID);
            db.AddInParameter(cmd, "userIP", DbType.String, Comment.USERIP); //접속IP
            db.AddInParameter(cmd, "userMachineName", DbType.String, Comment.USERMACHINENAME); //접속머신
            db.AddInParameter(cmd, "BESTREPLYYN", DbType.String, Comment.BESTREPLYYN);
            return db.ExecuteDataSet(cmd);
        }

        /*
        //베스트 댓글
        public DataSet GlossarySurveyBestSuccessComment(GlossarySurveyCommentType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentBest_Insert");
            db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
            db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
            return db.ExecuteDataSet(cmd);
        }

        //베스트 댓글 select 
        public DataSet GlossarySurveyCommentSuccessSelect(string ItemID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentSuccess_Select");
            db.AddInParameter(cmd, "ItemID", DbType.String, ItemID);
            return db.ExecuteDataSet(cmd);
        }

        //댓글 쪽지 YN select 
        public DataSet NoteQnaYNSelect(string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyNoteQnaYNSelect_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);
            return db.ExecuteDataSet(cmd);
        }


        public string CommentBest_Check(string CommonID)
        {
            string BestReplyYN = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SurveyCommentBest_Check");
            db.AddInParameter(cmd, "CommonID", DbType.String, CommonID);

            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BestReplyYN = (dr["BestReplyYN"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["BestReplyYN"]);
                    }
                }
            }

            return BestReplyYN;
        }
        */

        /*
    *댓글 내용 가져오기
    */
        public DataSet CommCommentListExcel(string commType, string commIdx)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CommComment_Excel");
            db.AddInParameter(cmd, "commType", DbType.String, commType);
            db.AddInParameter(cmd, "commIdx", DbType.String, commIdx);
            return db.ExecuteDataSet(cmd);
        }

		//제목으로 값이 있는지 확인한다.(MyGroup, 문서함)
		public DataSet ExistTitleEtc(string Title, string itemID, string UserID, string mode)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_ExistTitleEtc_Select");
			db.AddInParameter(cmd, "Title", DbType.String, Title);
			db.AddInParameter(cmd, "itemID", DbType.String, itemID);
			db.AddInParameter(cmd, "UserID", DbType.String, UserID);
			db.AddInParameter(cmd, "mode", DbType.String, mode);
			return db.ExecuteDataSet(cmd);
		}

        /*
        Author : 개발자-김성환D, 리뷰자-이정선G
        Create Date : 2016.03.02
        Desc : 끌지식 댓글 작성시 동일인(최초/최종작성자) 체크 후 쪽지 발송
        */
        public DataSet UserGlossaryInfo(string GlossaryID, string commentWriterID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryUser_Select");
            db.AddInParameter(cmd, "CommonID", DbType.String, GlossaryID);
            db.AddInParameter(cmd, "UserID", DbType.String, commentWriterID);
            return db.ExecuteDataSet(cmd);
        }
    }
}
