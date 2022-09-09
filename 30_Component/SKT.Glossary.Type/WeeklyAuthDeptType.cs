using System;

namespace SKT.Glossary.Type
{
    /// <summary>
	/// 설명: Data entity class for tb_WeeklyAuthDept table.
	/// 작성일 : 2015-03-04
	/// 작성자 : miksystem.com
	/// </summary>
    public class WeeklyAuthDeptType
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WeeklyLikeType class.
		/// </summary>
		public WeeklyAuthDeptType()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyLikeType class.
		/// </summary>
		public WeeklyAuthDeptType(string deptName, string deptCode, string authDeptCode, string topAuthDeptCode)
		{
            this.DeptName = deptName;
            this.DeptCode = deptCode;
            this.AuthDeptCode = authDeptCode;
            this.TopAuthDeptCode = topAuthDeptCode;
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyLikeType class.
		/// </summary>
        public WeeklyAuthDeptType(long weeklyAuthDeptID, string deptName, string deptCode, string authDeptCode, string topAuthDeptCode)
		{
            this.WeeklyAuthDeptID = weeklyAuthDeptID;
            this.DeptName = deptName;
            this.DeptCode = deptCode;
            this.AuthDeptCode = authDeptCode;
            this.TopAuthDeptCode = topAuthDeptCode;
		}

		#endregion

		#region Properties
		/// <summary>
        /// Gets or sets the WeeklyAuthDeptID value.
		/// </summary>
        public long WeeklyAuthDeptID { get; set; }

		/// <summary>
        /// Gets or sets the DeptName value.
		/// </summary>
		public string DeptName { get; set; }

		/// <summary>
        /// Gets or sets the DeptCode value.
		/// </summary>
		public string DeptCode { get; set; }

		/// <summary>
        /// Gets or sets the AuthDeptCode value.
		/// </summary>
		public string AuthDeptCode { get; set; }

		/// <summary>
        /// Gets or sets the TopAuthDeptCode value.
		/// </summary>
        public string TopAuthDeptCode { get; set; }

		#endregion
	}
}
