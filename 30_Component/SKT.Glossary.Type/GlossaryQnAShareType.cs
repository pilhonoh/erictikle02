using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryQnAShareType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the QnAShareType class.
        /// </summary>
        public GlossaryQnAShareType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the QnAShareType class.
        /// </summary>
        public GlossaryQnAShareType(int qnAID, string title, string fromUserID, string toUserID, string BoardUserID, string userName, string deptName, string userEmail, string readYN, DateTime createDate, string myDeleteYN, string youDeleteYN)
        {
            this.QnAID = qnAID;
            this.Title = title;
            this.FromUserID = fromUserID;
            this.ToUserID = toUserID;
            this.BoardUserID = BoardUserID;
            this.UserName = userName;
            this.DeptName = deptName;
            this.UserEmail = userEmail;
            this.ReadYN = readYN;
            this.CreateDate = createDate;
            this.MyDeleteYN = myDeleteYN;
            this.YouDeleteYN = youDeleteYN;
        }

        /// <summary>
        /// Initializes a new instance of the QnAShareType class.
        /// </summary>
        public GlossaryQnAShareType(long iD, int qnAID, string title, string fromUserID, string toUserID, string BoardUserID, string userName, string deptName, string userEmail, string readYN, DateTime createDate, string myDeleteYN, string youDeleteYN)
        {
            this.ID = iD;
            this.QnAID = qnAID;
            this.Title = title;
            this.FromUserID = fromUserID;
            this.ToUserID = toUserID;
            this.BoardUserID = BoardUserID;
            this.UserName = userName;
            this.DeptName = deptName;
            this.UserEmail = userEmail;
            this.ReadYN = readYN;
            this.CreateDate = createDate;
            this.MyDeleteYN = myDeleteYN;
            this.YouDeleteYN = youDeleteYN;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the QnAID value.
        /// </summary>
        public int QnAID { get; set; }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the FromUserID value.
        /// </summary>
        public string FromUserID { get; set; }

        /// <summary>
        /// Gets or sets the ToUserID value.
        /// </summary>
        public string ToUserID { get; set; }

        /// <summary>
        /// Gets or sets the BoardUserId value.
        /// </summary>
        public string BoardUserID { get; set; }

        /// <summary>
        /// Gets or sets the UserName value.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the DeptName value.
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// Gets or sets the UserEmail value.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the ReadYN value.
        /// </summary>
        public string ReadYN { get; set; }

        /// <summary>
        /// Gets or sets the CreateDate value.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the MyDeleteYN value.
        /// </summary>
        public string MyDeleteYN { get; set; }

        /// <summary>
        /// Gets or sets the YouDeleteYN value.
        /// </summary>
        public string YouDeleteYN { get; set; }

        #endregion
    }
}
