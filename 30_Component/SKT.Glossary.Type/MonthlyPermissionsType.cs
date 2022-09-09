using System;

namespace SKT.Glossary.Type
{
	/// <summary>
	/// 설명: Data entity class for tb_WeeklyPermissions table.
	/// 작성일 : 2015-04-12
	/// 작성자 : miksystem.com
	/// </summary>
	public class MonthlyPermissionsType
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WeeklyPermissionsType class.
		/// </summary>
		public MonthlyPermissionsType()
		{
		}

		/// <summary>
		/// Initializes a new instance of the WeeklyPermissionsType class.
		/// </summary>
        public MonthlyPermissionsType(long weeklyID, string toUserID, string toUserName)
		{
			this.WeeklyID = weeklyID;
			this.ToUserID = toUserID;
			this.ToUserName = toUserName;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the WeeklyID value.
		/// </summary>
		public long WeeklyID { get; set; }

		/// <summary>
		/// Gets or sets the ToUserID value.
		/// </summary>
		public string ToUserID { get; set; }

		/// <summary>
		/// Gets or sets the ToUserName value.
		/// </summary>
		public string ToUserName { get; set; }

		#endregion
	}
}
