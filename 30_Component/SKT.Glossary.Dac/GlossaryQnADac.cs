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
   public class GlossaryQnADac
    {
        private const string connectionStringName = "ConnGlossary";

        //QnA 리스트
        public DataSet GlossaryQnAList(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID, string SearchSort, string SearchSortGubun)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_List");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "SearchSortGubun", DbType.String, SearchSortGubun);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 리스트(platform)
        public DataSet GlossaryQnAList_Platform(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID, string SearchSort, string SearchSortGubun)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_List_Platform");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "SearchSortGubun", DbType.String, SearchSortGubun);
            return db.ExecuteDataSet(cmd);
        }


        //QnA 리스트(Marketing)
        public DataSet GlossaryQnAList_Marketing(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID, string SearchSort, string SearchSortGubun)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_List_Marketing");
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            db.AddInParameter(cmd, "SearchType", DbType.String, SearchType);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(cmd, "SearchSortGubun", DbType.String, SearchSortGubun);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 뷰
        public DataSet  GlossaryQnASelect(string ID,int count)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "Count", DbType.Int32, count);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 추가
        public DataSet  GlossaryQnAInsert(GlossaryQnAType Board)
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
        //QnA 추가
        public DataSet GlossaryQnAUpdate(GlossaryQnAType Board, string userid, string usermachinename, string userip)
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
            db.AddInParameter(cmd, "LastModifierUserid", DbType.String, userid);
            db.AddInParameter(cmd, "LastModifierUserMachinename", DbType.String, usermachinename);
            db.AddInParameter(cmd, "LastModifierUserip", DbType.String, userip);
            return db.ExecuteDataSet(cmd);
        }

        //QnA 삭제
        public DataSet GlossaryQnADelete(string ID, string UserID, string userIp, string userMachineName)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_QnA_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "UserID", DbType.String, ID);
            db.AddInParameter(cmd, "userIp", DbType.String, ID);
            db.AddInParameter(cmd, "userMachineName", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 2014-06-10 Mr.No
        /// Inserts a record into the tb_QnAShare table.
        /// </summary>
        /// <returns></returns>
        public int QnAShareInsert(GlossaryQnAShareType qnAShareType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_QnAShare_Insert");

            db.AddInParameter(dbCommand, "QnAID", DbType.Int32, qnAShareType.QnAID);
            db.AddInParameter(dbCommand, "Title", DbType.String, qnAShareType.Title);
            db.AddInParameter(dbCommand, "FromUserID", DbType.String, qnAShareType.FromUserID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.String, qnAShareType.ToUserID);
            db.AddInParameter(dbCommand, "BoardUserID", DbType.String, qnAShareType.BoardUserID);
            db.AddInParameter(dbCommand, "UserName", DbType.String, qnAShareType.UserName);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, qnAShareType.DeptName);
            db.AddInParameter(dbCommand, "UserEmail", DbType.String, qnAShareType.UserEmail);
            //db.AddInParameter(dbCommand, "ReadYN", DbType.String, qnAShareType.ReadYN);
            //db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, qnAShareType.CreateDate);
            //db.AddInParameter(dbCommand, "MyDeleteYN", DbType.String, qnAShareType.MyDeleteYN);
            //db.AddInParameter(dbCommand, "YouDeleteYN", DbType.String, qnAShareType.YouDeleteYN);

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="ID"></param>
        public void QnAHit(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_QnAHit");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);

            db.ExecuteDataSet(dbCommand);
        }


        public DataSet QnaBestComment(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_commCommentQnaBest_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryQnAExistTitle(int ID, string title)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryQnaTitle_Select");
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);
            db.AddInParameter(cmd, "Title", DbType.String, title);
            return db.ExecuteDataSet(cmd);
        }

        //Platform update
        public int PlatformQnAUpdate(string id)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Qna_Platform_Update");
            db.AddInParameter(cmd, "ID", DbType.String, id);
            return db.ExecuteNonQuery(cmd);
        }
    }
}

