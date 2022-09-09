using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class ScoreRankingType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScoreRankingType class.
        /// </summary>
        public ScoreRankingType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ScoreRankingType class.
        /// </summary>
        public ScoreRankingType(string uSER_ID, string uSER_NAME, int wrtten, int edit, int comment, int likeCount, int qnA, int qnAComment, int wrttenScore, int editScore, int commentsScore, int likeCountScore, int qnAScore, int qnACommentScore, int totalScore)
        {
            this.USER_ID = uSER_ID;
            this.USER_NAME = uSER_NAME;
            this.Wrtten = wrtten;
            this.Edit = edit;
            this.Comment = comment;
            this.LikeCount = likeCount;
            this.QnA = qnA;
            this.QnAComment = qnAComment;
            this.WrttenScore = wrttenScore;
            this.EditScore = editScore;
            this.CommentsScore = commentsScore;
            this.LikeCountScore = likeCountScore;
            this.QnAScore = qnAScore;
            this.QnACommentScore = qnACommentScore;
            this.TotalScore = totalScore;
        }

        /// <summary>
        /// Initializes a new instance of the ScoreRankingType class.
        /// </summary>
        public ScoreRankingType(int iD, string uSER_ID, string uSER_NAME, int wrtten, int edit, int comment, int likeCount, int qnA, int qnAComment, int wrttenScore, int editScore, int commentsScore, int likeCountScore, int qnAScore, int qnACommentScore, int totalScore)
        {
            this.ID = iD;
            this.USER_ID = uSER_ID;
            this.USER_NAME = uSER_NAME;
            this.Wrtten = wrtten;
            this.Edit = edit;
            this.Comment = comment;
            this.LikeCount = likeCount;
            this.QnA = qnA;
            this.QnAComment = qnAComment;
            this.WrttenScore = wrttenScore;
            this.EditScore = editScore;
            this.CommentsScore = commentsScore;
            this.LikeCountScore = likeCountScore;
            this.QnAScore = qnAScore;
            this.QnACommentScore = qnACommentScore;
            this.TotalScore = totalScore;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the USER_ID value.
        /// </summary>
        public string USER_ID { get; set; }

        /// <summary>
        /// Gets or sets the USER_NAME value.
        /// </summary>
        public string USER_NAME { get; set; }

        /// <summary>
        /// Gets or sets the Wrtten value.
        /// </summary>
        public int Wrtten { get; set; }

        /// <summary>
        /// Gets or sets the Edit value.
        /// </summary>
        public int Edit { get; set; }

        /// <summary>
        /// Gets or sets the Comment value.
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// Gets or sets the LikeCount value.
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Gets or sets the QnA value.
        /// </summary>
        public int QnA { get; set; }

        /// <summary>
        /// Gets or sets the QnAComment value.
        /// </summary>
        public int QnAComment { get; set; }

        /// <summary>
        /// Gets or sets the WrttenScore value.
        /// </summary>
        public int WrttenScore { get; set; }

        /// <summary>
        /// Gets or sets the EditScore value.
        /// </summary>
        public int EditScore { get; set; }

        /// <summary>
        /// Gets or sets the CommentsScore value.
        /// </summary>
        public int CommentsScore { get; set; }

        /// <summary>
        /// Gets or sets the LikeCountScore value.
        /// </summary>
        public int LikeCountScore { get; set; }

        /// <summary>
        /// Gets or sets the QnAScore value.
        /// </summary>
        public int QnAScore { get; set; }

        /// <summary>
        /// Gets or sets the QnACommentScore value.
        /// </summary>
        public int QnACommentScore { get; set; }

        /// <summary>
        /// Gets or sets the TotalScore value.
        /// </summary>
        public int TotalScore { get; set; }

        public int TotalCount { get; set; }

        public string Rank { get; set; }

        public string RankUrl { get; set; }

        public int Visits { get; set; }

        public int Grade { get; set; }

        public string DEPT_NAME { get; set; }

        public long RowNum { get; set; }

        #endregion
    }
}
