using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class WeeklyTeamLeaderNotiCheckType
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WeeklyTeamLeaderNotiCheckType class.
        /// </summary>
        public WeeklyTeamLeaderNotiCheckType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyTeamLeaderNotiCheckType class.
        /// </summary>
        public WeeklyTeamLeaderNotiCheckType(string userID, string teamMemberYN, string teamMemberAllYN, string sendYN)
        {
            this.UserID = userID;
            this.TeamMemberYN = teamMemberYN;
            this.TeamMemberAllYN = teamMemberAllYN;
            this.SendYN = sendYN;
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyTeamLeaderNotiCheckType class.
        /// </summary>
        public WeeklyTeamLeaderNotiCheckType(int weeklyNotiID, string userID, string teamMemberYN, string teamMemberAllYN, string sendYN)
        {
            this.WeeklyNotiID = weeklyNotiID;
            this.UserID = userID;
            this.TeamMemberYN = teamMemberYN;
            this.TeamMemberAllYN = teamMemberAllYN;
            this.SendYN = sendYN;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the WeeklyNotiID value.
        /// </summary>
        public int WeeklyNotiID { get; set; }

        /// <summary>
        /// Gets or sets the UserID value.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the TeamMemberYN value.
        /// </summary>
        public string TeamMemberYN { get; set; }

        /// <summary>
        /// Gets or sets the TeamMemberAllYN value.
        /// </summary>
        public string TeamMemberAllYN { get; set; }

        /// <summary>
        /// Gets or sets the SendYN value.
        /// </summary>
        public string SendYN { get; set; }

        public string UserName { get; set; }

        public string EMAIL_ALIAS { get; set; }

        #endregion
    }
}
