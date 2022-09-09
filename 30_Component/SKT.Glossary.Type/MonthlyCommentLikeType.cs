using System;

namespace SKT.Glossary.Type
{
	/// <summary>
	/// 설명: Data entity class for tb_WeeklyCommentLike table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public class MonthlyCommentLikeType
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentLikeType class.
		/// </summary>
		public MonthlyCommentLikeType()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentLikeType class.
		/// </summary>
		public MonthlyCommentLikeType(long commentID, string userID, string userName, string userIP, string userMachineName, string likeY, DateTime createDateTime)
		{
			this.CommentID = commentID;
			this.UserID = userID;
			this.UserName = userName;
			this.UserIP = userIP;
			this.UserMachineName = userMachineName;
			this.LikeY = likeY;
			this.CreateDateTime = createDateTime;
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentLikeType class.
		/// </summary>
        public MonthlyCommentLikeType(long iD, long commentID, string userID, string userName, string userIP, string userMachineName, string likeY, DateTime createDateTime)
		{
			this.ID = iD;
			this.CommentID = commentID;
			this.UserID = userID;
			this.UserName = userName;
			this.UserIP = userIP;
			this.UserMachineName = userMachineName;
			this.LikeY = likeY;
			this.CreateDateTime = createDateTime;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public long ID { get; set; }

		/// <summary>
		/// Gets or sets the CommentID value.
		/// </summary>
		public long CommentID { get; set; }

		/// <summary>
		/// Gets or sets the UserID value.
		/// </summary>
		public string UserID { get; set; }

		/// <summary>
		/// Gets or sets the UserName value.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the UserIP value.
		/// </summary>
		public string UserIP { get; set; }

		/// <summary>
		/// Gets or sets the UserMachineName value.
		/// </summary>
		public string UserMachineName { get; set; }

		/// <summary>
		/// Gets or sets the LikeY value.
		/// </summary>
		public string LikeY { get; set; }

		/// <summary>
		/// Gets or sets the CreateDateTime value.
		/// </summary>
		public DateTime CreateDateTime { get; set; }

		#endregion
	}
}
