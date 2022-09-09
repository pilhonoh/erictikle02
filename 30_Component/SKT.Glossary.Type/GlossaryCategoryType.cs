using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// 설명: Data entity class for tb_GlossaryCategory table.
    /// 작성일 : 2014-04-28
    /// 작성자 : miksystem.com
    /// </summary>
    public class GlossaryCategoryType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GlossaryCategoryType class.
        /// </summary>
        public GlossaryCategoryType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GlossaryCategoryType class.
        /// </summary>
        public GlossaryCategoryType(string categoryTitle, string categoryContents, string NOTES, string userID, string userName, string deptName, string userEmail, string useYN, DateTime createdDT, DateTime updatedDT)
        {
            this.CategoryTitle = categoryTitle;
            this.CategoryContents = categoryContents;
            this.NOTES = NOTES;
            this.UserID = userID;
            this.UserName = userName;
            this.DeptName = deptName;
            this.UserEmail = userEmail;
            this.UseYN = useYN;
            this.CreatedDT = createdDT;
            this.UpdatedDT = updatedDT;
        }

        /// <summary>
        /// Initializes a new instance of the GlossaryCategoryType class.
        /// </summary>
        public GlossaryCategoryType(long rowNum, long iD, string categoryTitle, string categoryContents, string NOTES, string userID, string userName, string deptName, string userEmail, string useYN, DateTime createdDT, DateTime updatedDT)
        {
            this.RowNum = rowNum;
            this.ID = iD;
            this.CategoryTitle = categoryTitle;
            this.CategoryContents = categoryContents;
            this.NOTES = NOTES;
            this.UserID = userID;
            this.UserName = userName;
            this.DeptName = deptName;
            this.UserEmail = userEmail;
            this.UseYN = useYN;
            this.CreatedDT = createdDT;
            this.UpdatedDT = updatedDT;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the RowNum value.
        /// </summary>
        public long RowNum { get; set; }

        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the CategoryTitle value.
        /// </summary>
        public string CategoryTitle { get; set; }

        /// <summary>
        /// Gets or sets the CategoryContents value.
        /// </summary>
        public string CategoryContents { get; set; }

        /// <summary>
        /// Gets or sets the NOTES value.
        /// </summary>
        public string NOTES { get; set; }

        /// <summary>
        /// Gets or sets the UserID value.
        /// </summary>
        public string UserID { get; set; }

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
        /// Gets or sets the UseYN value.
        /// </summary>
        public string UseYN { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDT value.
        /// </summary>
        public DateTime CreatedDT { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedDT value.
        /// </summary>
        public DateTime UpdatedDT { get; set; }

        // 2014-06-23 Mr.No
        public string HIERACHY_CODE { get; set; }

        #endregion
    }
}
