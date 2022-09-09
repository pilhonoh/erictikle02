using System;

namespace SKT.Glossary.Type
{
	/// <summary>
	/// 설명: Data entity class for tb_WeeklyComment table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public class MonthlyCommentType
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentType class.
		/// </summary>
		public MonthlyCommentType()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentType class.
		/// </summary>
		public MonthlyCommentType(long sUP_ID, long weeklyID, string contents, string userID, string userName, string dutyName, string deptName, DateTime createDateTime = new DateTime(), DateTime updateDateTime = new DateTime())
		{
			this.SUP_ID = sUP_ID;
			this.WeeklyID = weeklyID;
			this.Contents = contents;
			this.UserID = userID;
			this.UserName = userName;
			this.DutyName = dutyName;
			this.DeptName = deptName;
            this.CreateDateTime = createDateTime;
            this.UpdateDateTime = updateDateTime;
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyCommentType class.
		/// </summary>
        public MonthlyCommentType(long weeklyCommentID, long sUP_ID, long weeklyID, string contents, string userID, string userName, string dutyName, string deptName, DateTime createDateTime = new DateTime(), DateTime updateDateTime = new DateTime())
		{
			this.WeeklyCommentID = weeklyCommentID;
			this.SUP_ID = sUP_ID;
			this.WeeklyID = weeklyID;
			this.Contents = contents;
			this.UserID = userID;
			this.UserName = userName;
			this.DutyName = dutyName;
			this.DeptName = deptName;
            this.CreateDateTime = createDateTime;
            this.UpdateDateTime = updateDateTime;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public long WeeklyCommentID { get; set; }

		/// <summary>
		/// Gets or sets the SUP_ID value.
		/// </summary>
		public long SUP_ID { get; set; }

		/// <summary>
		/// Gets or sets the WeeklyID value.
		/// </summary>
		public long WeeklyID { get; set; }

		/// <summary>
		/// Gets or sets the Contents value.
		/// </summary>
		public string Contents { get; set; }

		/// <summary>
		/// Gets or sets the UserID value.
		/// </summary>
		public string UserID { get; set; }

		/// <summary>
		/// Gets or sets the UserName value.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
        /// Gets or sets the DutyName value.
		/// </summary>
		public string DutyName { get; set; }

		/// <summary>
		/// Gets or sets the DeptName value.
		/// </summary>
		public string DeptName { get; set; }

		/// <summary>
		/// Gets or sets the CreateDateTime value.
		/// </summary>
		public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the UpdateDateTime value.
        /// </summary>
        public DateTime UpdateDateTime { get; set; }

        // Mr.No 추가
        // 2015-03-09
        public string DisplayYN { get; set; }

		#endregion
	}
}
