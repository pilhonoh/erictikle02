using System;

namespace SKT.Glossary.Type
{
	/// <summary>
	/// 설명: Data entity class for tb_Weekly table.
	/// 작성일 : 2015-02-27
	/// 작성자 : miksystem.com
	/// </summary>
	public class WeeklyType
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WeeklyType class.
		/// </summary>
		public WeeklyType()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyType class.
		/// </summary>
        public WeeklyType(string contents, string textContents, int viewLevel, string teamChiefYN, string userID, string userName, string userEmail, string deptName, string deptCode, int year, int yearWeek, string tempYN, string memoWriterID, string memoContents, string memoSentYN, string LastUpdateWriterID, int hits = 0, int likeCount = 0, DateTime createDateTime = new DateTime(), DateTime updateDateTime = new DateTime(), DateTime date = new DateTime())
		{
            this.Contents = contents;
            this.TextContents = textContents;
            this.ViewLevel = viewLevel;
            this.TeamChiefYN = teamChiefYN;
			this.UserID = userID;
			this.UserName = userName;
            this.UserEmail = userEmail;
			this.DeptName = deptName;
			this.DeptCode = deptCode;
			this.Year = year;
			this.YearWeek = yearWeek;
            this.Date = date;
            this.TempYN = tempYN;
            this.MemoWriterID = memoWriterID;
            this.MemoContents = memoContents;
            this.MemoSentYN = memoSentYN;
            this.Hits = hits;
            this.LikeCount = likeCount;
            this.CreateDateTime = createDateTime;
            this.LastUpdateWriterID = LastUpdateWriterID;
            this.UpdateDateTime = updateDateTime;
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyType class.
		/// </summary>
        public WeeklyType(long weeklyID, string contents, string textContents, int viewLevel, string teamChiefYN, string userID, string userName, string userEmail, string deptName, string deptCode, int year, int yearWeek, string tempYN, string memoWriterID, string memoContents, string memoSentYN, string LastUpdateWriterID, int hits = 0, int likeCount = 0, DateTime createDateTime = new DateTime(), DateTime updateDateTime = new DateTime(), DateTime date = new DateTime())
		{
			this.WeeklyID = weeklyID;
            this.TextContents = textContents;
            this.ViewLevel = viewLevel;
			this.Contents = contents;
            this.TeamChiefYN = teamChiefYN;
			this.UserID = userID;
			this.UserName = userName;
            this.UserEmail = userEmail;
			this.DeptName = deptName;
			this.DeptCode = deptCode;
			this.Year = year;
			this.YearWeek = yearWeek;
            this.Date = date;
            this.TempYN = tempYN;
            this.MemoWriterID = memoWriterID;
            this.MemoContents = memoContents;
            this.MemoSentYN = memoSentYN;
            this.Hits = hits;
            this.LikeCount = likeCount;
            this.CreateDateTime = createDateTime;
            this.LastUpdateWriterID = LastUpdateWriterID;
            this.UpdateDateTime = updateDateTime;
		}

        //2015.07.13 zz17779
        public WeeklyType(string commonWeeklyFlag, long weeklyID, string contents, string textContents, int viewLevel, string teamChiefYN, string userID, string userName, string userEmail, string deptName, string deptCode, int year, int yearWeek, string tempYN, string memoWriterID, string memoContents, string memoSentYN, string LastUpdateWriterID, int hits = 0, int likeCount = 0, DateTime createDateTime = new DateTime(), DateTime updateDateTime = new DateTime(), DateTime date = new DateTime())
        {
            this.WeeklyID = weeklyID;
            this.TextContents = textContents;
            this.ViewLevel = viewLevel;
            this.Contents = contents;
            this.TeamChiefYN = teamChiefYN;
            this.UserID = userID;
            this.UserName = userName;
            this.UserEmail = userEmail;
            this.DeptName = deptName;
            this.DeptCode = deptCode;
            this.Year = year;
            this.YearWeek = yearWeek;
            this.Date = date;
            this.TempYN = tempYN;
            this.MemoWriterID = memoWriterID;
            this.MemoContents = memoContents;
            this.MemoSentYN = memoSentYN;
            this.Hits = hits;
            this.LikeCount = likeCount;
            this.CreateDateTime = createDateTime;
            this.LastUpdateWriterID = LastUpdateWriterID;
            this.UpdateDateTime = updateDateTime;
            this.CommonWeeklyFlag = commonWeeklyFlag;
        }


		#endregion

		#region Properties
		/// <summary>
        /// Gets or sets the WeeklyID value.
		/// </summary>
		public long WeeklyID { get; set; }

		/// <summary>
        /// Gets or sets the Contents value.
		/// </summary>
		public string Contents { get; set; }

        /// <summary>
        /// Gets or sets the TextContents value.
		/// </summary>
		public string TextContents { get; set; }

        /// <summary>
        /// Gets or sets the ViewLevel value.
        /// </summary>
        public int ViewLevel { get; set; }

		/// <summary>
		/// Gets or sets the Hits value.
		/// </summary>
		public int Hits { get; set; }

        ///// <summary>
        ///// Gets or sets the CommentCount value.
        ///// </summary>
        //public int? CommentCount { get; set; }

		/// <summary>
		/// Gets or sets the LikeCount value.
		/// </summary>
		public int LikeCount { get; set; }

		/// <summary>
        /// Gets or sets the TeamChiefYN value.
		/// </summary>
		public string TeamChiefYN { get; set; }

		/// <summary>
		/// Gets or sets the UserID value.
		/// </summary>
		public string UserID { get; set; }

		/// <summary>
		/// Gets or sets the UserName value.
		/// </summary>
		public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the UserEmail value.
        /// </summary>
        public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the DeptName value.
		/// </summary>
		public string DeptName { get; set; }

		/// <summary>
		/// Gets or sets the DeptCode value.
		/// </summary>
		public string DeptCode { get; set; }

		/// <summary>
		/// Gets or sets the Year value.
		/// </summary>
		public int Year { get; set; }

		/// <summary>
        /// Gets or sets the YearWeek value.
		/// </summary>
        public int YearWeek { get; set; }

        /// <summary>
        /// Gets or sets the Date value.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the TempYN value.
        /// </summary>
        public string TempYN { get; set; }

        /// <summary>
        /// Gets or sets the MemoWriterID value.
        /// </summary>
        public string MemoWriterID { get; set; }

        /// <summary>
        /// Gets or sets the MemoContents value.
        /// </summary>
        public string MemoContents { get; set; }

        /// <summary>
        /// Gets or sets the MemoSentYN value.
        /// </summary>
        public string MemoSentYN { get; set; }

        /// <summary>
        /// Gets or sets the PermissionYN value.
        /// </summary>
        public string PermissionYN { get; set; }

        /// <summary>
        /// Gets or sets the CreateDateTime value.
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the UpdateDateTime value.
        /// </summary>
        public DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdateWriterID value.
        /// </summary>
        public string LastUpdateWriterID { get; set; }

        // Mr.No
        public string Month { get; set; }
        public string Comments { get; set; }    // 2015-03-18 Mr.No
        public string photoURL { get; set; }
        public string DateString { get; set; }
        public string PositionName { get; set; }
        //public string PermissionYN { get; set; }
        public string PermissionsUserID { get; set; }
        public DateTime MemoCreateDateTime { get; set; }
        public DateTime MemoUpdateDateTime { get; set; }

        //2015.07.13 zz17779
        public string CommonWeeklyFlag { get; set; }

        //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련
        public string AbsenceMsg { get; set; }

        //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침  씨발 쫒도 꺼져라..믹 소프트 개새끼들아 개발 좀 똑바로 해라.병신같은 새끼들아 니들이 개발자냐.........
        public DateTime StartWeekDate { get; set; }
        public DateTime EndWeekDate { get; set; }



		#endregion
	}

    public class WeeklyUserType
    {
        public WeeklyUserType()
        {
        }

        public WeeklyUserType(string userID, string userName, string positionName, long weeklyID, string contents, string textContents, string tempYN, DateTime updateDateTime)
        {
            this.UserID = userID;
            this.UserName = UserName;
            this.PositionName = positionName;
            this.WeeklyID = weeklyID;
            this.Contents = contents;
            this.TextContents = textContents;
            this.TempYN = tempYN;
            this.UpdateDateTime = updateDateTime;
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PositionName { get; set; }
        public long WeeklyID{get;set;}
        public string Contents{get;set;}
        public string TextContents{get;set;}
        public string TempYN{get;set;}
        public DateTime UpdateDateTime{get;set;}
    }

    //<!--2015.03 수정 -->
    public class DeptWeeklyType
    {
        public ImpersonUserinfo DeptMemberInfo { get; set; }
        public long WeeklyID { get; set; }
        public string WeeklyDeptCode { get; set; } // 겸직용 부서코드
        public string WeeklyDeptName { get; set; } // 겸직용 부서명
        public DateTime WeeklyCreateDateTime { get; set; }
        public string TempFG { get; set; }
        public string PermissionYN { get; set; }
        public string PermissionsUserID { get; set; }
        
    }
    //<!-- 수정 끝 -->

    //KSM 2015-04-13 displayLevel이 필요해서 추가함
    /// <summary>
    /// 설명: Data entity class for TB_DEPARTMENT_ORGCHART table.
    /// 작성일 : 2015-04-13
    /// 작성자 : miksystem.com
    /// </summary>
    public class DEPARTMENT_ORGCHARTType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DEPARTMENT_ORGCHARTType class.
        /// </summary>
        public DEPARTMENT_ORGCHARTType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DEPARTMENT_ORGCHARTType class.
        /// </summary>
        public DEPARTMENT_ORGCHARTType(int iD, DateTime cREATEDATE, DateTime lASTMODIFIEDDATE, string departmentNumber, string department, string displayName, string displayLevel, string displayOrder, bool hasChild, string locationCode, string managerEmployeeID, string upperDepartmentNumber, string displayYN, string mail, string pATH)
        {
            this.ID = iD;
            this.CREATEDATE = cREATEDATE;
            this.LASTMODIFIEDDATE = lASTMODIFIEDDATE;
            this.DepartmentNumber = departmentNumber;
            this.Department = department;
            this.DisplayName = displayName;
            this.DisplayLevel = displayLevel;
            this.DisplayOrder = displayOrder;
            this.HasChild = hasChild;
            this.LocationCode = locationCode;
            this.ManagerEmployeeID = managerEmployeeID;
            this.UpperDepartmentNumber = upperDepartmentNumber;
            this.DisplayYN = displayYN;
            this.Mail = mail;
            this.PATH = pATH;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the CREATEDATE value.
        /// </summary>
        public DateTime CREATEDATE { get; set; }

        /// <summary>
        /// Gets or sets the LASTMODIFIEDDATE value.
        /// </summary>
        public DateTime LASTMODIFIEDDATE { get; set; }

        /// <summary>
        /// Gets or sets the DepartmentNumber value.
        /// </summary>
        public string DepartmentNumber { get; set; }

        /// <summary>
        /// Gets or sets the Department value.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the DisplayName value.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the DisplayLevel value.
        /// </summary>
        public string DisplayLevel { get; set; }

        /// <summary>
        /// Gets or sets the DisplayOrder value.
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the HasChild value.
        /// </summary>
        public bool HasChild { get; set; }

        /// <summary>
        /// Gets or sets the LocationCode value.
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// Gets or sets the ManagerEmployeeID value.
        /// </summary>
        public string ManagerEmployeeID { get; set; }

        /// <summary>
        /// Gets or sets the UpperDepartmentNumber value.
        /// </summary>
        public string UpperDepartmentNumber { get; set; }

        /// <summary>
        /// Gets or sets the DisplayYN value.
        /// </summary>
        public string DisplayYN { get; set; }

        /// <summary>
        /// Gets or sets the Mail value.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets the PATH value.
        /// </summary>
        public string PATH { get; set; }

        #endregion
    }

    //2015-04-26 메일읽어 오는데 필요함.
    /// <summary>
    /// 설명: Data entity class for tb_WeeklyMail table.
    /// 작성일 : 2015-04-26
    /// 작성자 : miksystem.com
    /// </summary>
    public class WeeklyMailType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WeeklyMailType class.
        /// </summary>
        public WeeklyMailType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyMailType class.
        /// </summary>
        public WeeklyMailType(string weeklyID, string messageID, DateTime dateTimeSent)
        {
            this.WeeklyID = weeklyID;
            this.MessageID = messageID;
            this.DateTimeSent = dateTimeSent;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the WeeklyID value.
        /// </summary>
        public string WeeklyID { get; set; }

        /// <summary>
        /// Gets or sets the MessageID value.
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// Gets or sets the DateTimeSent value.
        /// </summary>
        public DateTime DateTimeSent { get; set; }

        #endregion
    }


    public class WeeklyDelegateDuty
    {
        public string T_FLAG { get; set; }
        public string APPCD { get; set; }
        public string EMPNO { get; set; }
        public string DEPT { get; set; }
        public string ACTING_EMPNO { get; set; }
        public string ACTING_DEPT { get; set; }
        public string STARTDAY { get; set; }
        public string ENDDAY { get; set; }
    }
    #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 ----------------------------

    public class WeeklyAbsenceType
    {
        public WeeklyAbsenceType(){}

        public WeeklyAbsenceType(int idx, string userID, string absence_UserID, string absence_UserNm, string absence_Comment, string absence_StartDt, string absence_EndDt, string absence_Flag, string additionJobCode)
        {
            this.Idx = idx;
            this.UserID = userID;
            this.Absence_UserID = absence_UserID;
            this.Absence_UserNm = absence_UserNm;
            this.Absence_Comment = absence_Comment;
            this.Absence_StartDt = absence_StartDt;
            this.Absence_EndDt = absence_EndDt;
            this.Absence_Flag = absence_Flag;
            this.AdditionJobCode = additionJobCode;


        }


        public int Idx { get; set; }
        public string UserID{get;set;}
        public string Absence_UserID { get; set; }
        public string Absence_UserNm { get; set; }
        public string Absence_Comment { get; set; }

        public string Absence_StartDt { get; set; }
        public string Absence_EndDt { get; set; }
        public string Absence_Flag { get; set; }

        /*
             Author : 개발자- 백충기G, 리뷰자-진현빈D
             CreateDae :  2016.04.20
             Desc : 임원 겸직의 위클리 모아보기&출력 기능 수정
        */
        public string AdditionJobCode { get; set; }
        public string AdditionJobName { get; set; }
        

    }

    #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 -----------------------


}
