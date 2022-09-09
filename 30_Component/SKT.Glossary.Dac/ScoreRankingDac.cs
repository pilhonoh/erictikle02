using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;
using System.Configuration;

namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_ScoreRanking table.
    /// 작성일 : 2014-06-11
    /// 작성자 : miksystem.com
    /// </summary>
    public class ScoreRankingDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static ScoreRankingDac _instance = null;
        public static ScoreRankingDac Instance
        {
            get
            {
                ScoreRankingDac obj = _instance;
                if (obj == null)
                {
                    obj = new ScoreRankingDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        private ScoreRankingDac() { }

        /// <summary>
        /// Inserts a record into the tb_ScoreRanking table.
        /// </summary>
        /// <returns></returns>
        public int ScoreRankingInsert(ScoreRankingType scoreRankingType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_Insert");

            db.AddInParameter(dbCommand, "USER_ID", DbType.String, scoreRankingType.USER_ID);
            db.AddInParameter(dbCommand, "USER_NAME", DbType.String, scoreRankingType.USER_NAME);
            db.AddInParameter(dbCommand, "Wrtten", DbType.Int32, scoreRankingType.Wrtten);
            db.AddInParameter(dbCommand, "Edit", DbType.Int32, scoreRankingType.Edit);
            db.AddInParameter(dbCommand, "Comment", DbType.Int32, scoreRankingType.Comment);
            db.AddInParameter(dbCommand, "LikeCount", DbType.Int32, scoreRankingType.LikeCount);
            db.AddInParameter(dbCommand, "QnA", DbType.Int32, scoreRankingType.QnA);
            db.AddInParameter(dbCommand, "QnAComment", DbType.Int32, scoreRankingType.QnAComment);
            db.AddInParameter(dbCommand, "WrttenScore", DbType.Int32, scoreRankingType.WrttenScore);
            db.AddInParameter(dbCommand, "EditScore", DbType.Int32, scoreRankingType.EditScore);
            db.AddInParameter(dbCommand, "CommentsScore", DbType.Int32, scoreRankingType.CommentsScore);
            db.AddInParameter(dbCommand, "LikeCountScore", DbType.Int32, scoreRankingType.LikeCountScore);
            db.AddInParameter(dbCommand, "QnAScore", DbType.Int32, scoreRankingType.QnAScore);
            db.AddInParameter(dbCommand, "QnACommentScore", DbType.Int32, scoreRankingType.QnACommentScore);
            db.AddInParameter(dbCommand, "TotalScore", DbType.Int32, scoreRankingType.TotalScore);

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_ScoreRanking table.
        /// </summary>
        /// <returns>DataSet</returns>
        public ScoreRankingType ScoreRankingSelect(string USER_ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_Select");

            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            ScoreRankingType scoreRankingType = null;
            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    scoreRankingType = GetScoreRankingTypeMapData(ds.Tables[0].Rows[0]);
                }
            }
            return scoreRankingType;
        }

        /// <summary>
        /// Selects all records from the tb_ScoreRanking table.
        /// </summary>
        public List<ScoreRankingType> ScoreRankingSelectAll()
        {
            List<ScoreRankingType> listScoreRankingType = new List<ScoreRankingType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_SelectAll");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ScoreRankingType scoreRankingType = GetScoreRankingTypeMapData(dr);
                        listScoreRankingType.Add(scoreRankingType);
                    }
                }
            }
            return listScoreRankingType;
        }

        /// <summary>
        /// Updates a record in the tb_ScoreRanking table.
        /// </summary>
        public void ScoreRankingUpdate(ScoreRankingType scoreRankingType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_Update");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, scoreRankingType.ID);
            db.AddInParameter(dbCommand, "USER_ID", DbType.String, scoreRankingType.USER_ID);
            db.AddInParameter(dbCommand, "USER_NAME", DbType.String, scoreRankingType.USER_NAME);
            db.AddInParameter(dbCommand, "Wrtten", DbType.Int32, scoreRankingType.Wrtten);
            db.AddInParameter(dbCommand, "Edit", DbType.Int32, scoreRankingType.Edit);
            db.AddInParameter(dbCommand, "Comment", DbType.Int32, scoreRankingType.Comment);
            db.AddInParameter(dbCommand, "LikeCount", DbType.Int32, scoreRankingType.LikeCount);
            db.AddInParameter(dbCommand, "QnA", DbType.Int32, scoreRankingType.QnA);
            db.AddInParameter(dbCommand, "QnAComment", DbType.Int32, scoreRankingType.QnAComment);
            db.AddInParameter(dbCommand, "WrttenScore", DbType.Int32, scoreRankingType.WrttenScore);
            db.AddInParameter(dbCommand, "EditScore", DbType.Int32, scoreRankingType.EditScore);
            db.AddInParameter(dbCommand, "CommentsScore", DbType.Int32, scoreRankingType.CommentsScore);
            db.AddInParameter(dbCommand, "LikeCountScore", DbType.Int32, scoreRankingType.LikeCountScore);
            db.AddInParameter(dbCommand, "QnAScore", DbType.Int32, scoreRankingType.QnAScore);
            db.AddInParameter(dbCommand, "QnACommentScore", DbType.Int32, scoreRankingType.QnACommentScore);
            db.AddInParameter(dbCommand, "TotalScore", DbType.Int32, scoreRankingType.TotalScore);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a record from the tb_ScoreRanking table by a composite primary key.
        /// </summary>
        public void ScoreRankingDelete(int iD)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_Delete");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, iD);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Creates a new instance of the ScoreRankingType class and populates it with data from the specified DataRow.
        /// </summary>
        private ScoreRankingType GetScoreRankingTypeMapData(DataRow dr)
        {
            ScoreRankingType scoreRankingType = new ScoreRankingType();
            if (dr.Table.Columns.Contains("RowNum"))
            {
                scoreRankingType.RowNum = (dr["RowNum"] == DBNull.Value) ? 0 : dr.Field<long>("RowNum");
            }
            scoreRankingType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<int>("ID");
            scoreRankingType.USER_ID = (dr["USER_ID"] == DBNull.Value) ? null : dr.Field<string>("USER_ID");
            scoreRankingType.USER_NAME = (dr["USER_NAME"] == DBNull.Value) ? null : dr.Field<string>("USER_NAME");
            scoreRankingType.Wrtten = (dr["Wrtten"] == DBNull.Value) ? 0 : dr.Field<int>("Wrtten");
            scoreRankingType.Edit = (dr["Edit"] == DBNull.Value) ? 0 : dr.Field<int>("Edit");
            scoreRankingType.Comment = (dr["Comment"] == DBNull.Value) ? 0 : dr.Field<int>("Comment");
            scoreRankingType.LikeCount = (dr["LikeCount"] == DBNull.Value) ? 0 : dr.Field<int>("LikeCount");
            scoreRankingType.QnA = (dr["QnA"] == DBNull.Value) ? 0 : dr.Field<int>("QnA");
            scoreRankingType.QnAComment = (dr["QnAComment"] == DBNull.Value) ? 0 : dr.Field<int>("QnAComment");
            scoreRankingType.WrttenScore = (dr["WrttenScore"] == DBNull.Value) ? 0 : dr.Field<int>("WrttenScore");
            scoreRankingType.EditScore = (dr["EditScore"] == DBNull.Value) ? 0 : dr.Field<int>("EditScore");
            scoreRankingType.CommentsScore = (dr["CommentsScore"] == DBNull.Value) ? 0 : dr.Field<int>("CommentsScore");
            scoreRankingType.LikeCountScore = (dr["LikeCountScore"] == DBNull.Value) ? 0 : dr.Field<int>("LikeCountScore");
            scoreRankingType.QnAScore = (dr["QnAScore"] == DBNull.Value) ? 0 : dr.Field<int>("QnAScore");
            scoreRankingType.QnACommentScore = (dr["QnACommentScore"] == DBNull.Value) ? 0 : dr.Field<int>("QnACommentScore");
            scoreRankingType.TotalScore = (dr["TotalScore"] == DBNull.Value) ? 0 : dr.Field<int>("TotalScore");
            if (dr.Table.Columns.Contains("TotalCount"))
            {
                scoreRankingType.TotalCount = (dr["TotalCount"] == DBNull.Value) ? 0 : dr.Field<int>("TotalCount");
            }
            scoreRankingType.Visits = (dr["Visits"] == DBNull.Value) ? 0 : dr.Field<int>("Visits");
            scoreRankingType.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
            scoreRankingType.RankUrl = RankingUrl(scoreRankingType.Grade);
            if (dr.Table.Columns.Contains("DEPT_NAME"))
            {
                scoreRankingType.DEPT_NAME = (dr["DEPT_NAME"] == DBNull.Value) ? string.Empty : dr.Field<string>("DEPT_NAME");
            }

            return scoreRankingType;
        }

        /// <summary>
        /// Paging 처리
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public List<ScoreRankingType> ScoreRanking_List(int PageNum)
        {
            List<ScoreRankingType> listScoreRankingType = new List<ScoreRankingType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_ScoreRanking_List");
            db.AddInParameter(dbCommand, "PageNum", DbType.Int32, PageNum);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ScoreRankingType scoreRankingType = GetScoreRankingTypeMapData(dr);
                        listScoreRankingType.Add(scoreRankingType);
                    }
                }
            }
            return listScoreRankingType;
        }

        /// <summary>
        /// 순위
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public List<ScoreRankingType> MainScore(string SortOrder)
        {
            List<ScoreRankingType> listScoreRankingType = new List<ScoreRankingType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MainScore");
            db.AddInParameter(dbCommand, "SortOrder", DbType.String, SortOrder);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ScoreRankingType scoreRankingType = GetScoreRankingTypeMapData(dr);
                        listScoreRankingType.Add(scoreRankingType);
                    }
                }
            }
            return listScoreRankingType;
        }

        protected string RankingUrl(int Grade)
        {
            string ImageUrl = string.Empty;

            
            string Rank = string.Empty;
            if (Grade == 0) { Rank = "지존"; }
            else if (Grade == 1) { Rank = "고수"; }
            else if (Grade == 2) { Rank = "중수"; }
            else { Rank = "초수"; }

            ImageUrl = "<img class=\"icon_img\" width='19' height='19' title='" + Rank + "' src='" + ConfigurationManager.AppSettings["FrontImageUrl"] + Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "'/>";

            return ImageUrl;
        }
        
    }
}
