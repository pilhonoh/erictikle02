using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class MonthlyRequestType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WeeklyRequestType class.
        /// </summary>
        public MonthlyRequestType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyRequestType class.
        /// </summary>
        public MonthlyRequestType(int year, int yearWeek, string sendUserName, string sendUserID, string sendUserPositionName, string receiveUserName, string receiveUserID, string receiveUserPositionName, string sendYN)
        {
            this.Year = year;
            this.YearWeek = yearWeek;
            this.SendUserName = sendUserName;
            this.SendUserID = sendUserID;
            this.SendUserPositionName = sendUserPositionName;
            this.ReceiveUserName = receiveUserName;
            this.ReceiveUserID = receiveUserID;
            this.ReceiveUserPositionName = receiveUserPositionName;
            this.SendYN = sendYN;
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyRequestType class.
        /// </summary>
        public MonthlyRequestType(int weeklyRequestID, int year, int yearWeek, string sendUserName, string sendUserID, string sendUserPositionName, string receiveUserName, string receiveUserID, string receiveUserPositionName, string sendYN)
        {
            this.WeeklyRequestID = weeklyRequestID;
            this.Year = year;
            this.YearWeek = yearWeek;
            this.SendUserName = sendUserName;
            this.SendUserID = sendUserID;
            this.SendUserPositionName = sendUserPositionName;
            this.ReceiveUserName = receiveUserName;
            this.ReceiveUserID = receiveUserID;
            this.ReceiveUserPositionName = receiveUserPositionName;
            this.SendYN = sendYN;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the WeeklyRequestID value.
        /// </summary>
        public int WeeklyRequestID { get; set; }

        /// <summary>
        /// Gets or sets the Year value.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the YearWeek value.
        /// </summary>
        public int YearWeek { get; set; }

        /// <summary>
        /// Gets or sets the SendUserName value.
        /// </summary>
        public string SendUserName { get; set; }

        /// <summary>
        /// Gets or sets the SendUserID value.
        /// </summary>
        public string SendUserID { get; set; }

        /// <summary>
        /// Gets or sets the SendUserPositionName value.
        /// </summary>
        public string SendUserPositionName { get; set; }

        /// <summary>
        /// Gets or sets the ReceiveUserName value.
        /// </summary>
        public string ReceiveUserName { get; set; }

        /// <summary>
        /// Gets or sets the ReceiveUserID value.
        /// </summary>
        public string ReceiveUserID { get; set; }

        /// <summary>
        /// Gets or sets the ReceiveUserPositionName value.
        /// </summary>
        public string ReceiveUserPositionName { get; set; }

        /// <summary>
        /// Gets or sets the SendYN value.
        /// </summary>
        public string SendYN { get; set; }

        public DateTime NowDateTime { get; set; }

        #endregion
    }
}
