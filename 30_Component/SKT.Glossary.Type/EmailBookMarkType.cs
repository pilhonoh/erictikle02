using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// 설명: Data entity class for tb_EmailBookMark table.
    /// 작성일 : 2015-11-17
    /// 작성자 : miksystem.com
    /// </summary>
    public class EmailBookMarkType
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EmailBookMarkType class.
        /// </summary>
        public EmailBookMarkType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EmailBookMarkType class.
        /// </summary>
        public EmailBookMarkType(string bookmarkUserID, string bookmarkAlias, Guid aliasid, string emailType, int entryType, string userID, string empID, string companyCode, string deptCode, string deptName, string groupCode, string groupName, string userName, string displayName, string emailAddress, string titCode, string titName, string jobName, string dutCode, string dutName, string locCode, string locName, string empCode, string empName, string mobileTel, string officeTel, string lyncOnnet, string officeTel2, string officeTelExt, string countryCode, string countryName, string orgchartName, string hasSubDept, string createDate, string updateDate)
        {
            this.BookmarkUserID = bookmarkUserID;
            this.BookmarkAlias = bookmarkAlias;
            this.AliasID = aliasid;
            this.EmailType = emailType;
            this.EntryType = entryType;
            this.UserID = userID;
            this.EmpID = empID;
            this.CompanyCode = companyCode;
            this.DeptCode = deptCode;
            this.DeptName = deptName;
            this.GroupCode = groupCode;
            this.GroupName = groupName;
            this.UserName = userName;
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.TitCode = titCode;
            this.TitName = titName;
            this.JobName = jobName;
            this.DutCode = dutCode;
            this.DutName = dutName;
            this.LocCode = locCode;
            this.LocName = locName;
            this.EmpCode = empCode;
            this.EmpName = empName;
            this.MobileTel = mobileTel;
            this.OfficeTel = officeTel;
            this.LyncOnnet = lyncOnnet;
            this.OfficeTel2 = officeTel2;
            this.OfficeTelExt = officeTelExt;
            this.CountryCode = countryCode;
            this.CountryName = countryName;
            this.OrgchartName = orgchartName;
            this.HasSubDept = hasSubDept;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
        }

        /// <summary>
        /// Initializes a new instance of the EmailBookMarkType class.
        /// </summary>
        public EmailBookMarkType(int idx, string bookmarkUserID, string bookmarkAlias, Guid aliasid,  string emailType, int entryType, string userID, string empID, string companyCode, string deptCode, string deptName, string groupCode, string groupName, string userName, string displayName, string emailAddress, string titCode, string titName, string jobName, string dutCode, string dutName, string locCode, string locName, string empCode, string empName, string mobileTel, string officeTel, string lyncOnnet, string officeTel2, string officeTelExt, string countryCode, string countryName, string orgchartName, string hasSubDept, string createDate, string updateDate)
        {
            this.Idx = idx;
            this.BookmarkUserID = bookmarkUserID;
            this.BookmarkAlias = bookmarkAlias;
            this.AliasID = aliasid;
            this.EmailType = emailType;
            this.EntryType = entryType;
            this.UserID = userID;
            this.EmpID = empID;
            this.CompanyCode = companyCode;
            this.DeptCode = deptCode;
            this.DeptName = deptName;
            this.GroupCode = groupCode;
            this.GroupName = groupName;
            this.UserName = userName;
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.TitCode = titCode;
            this.TitName = titName;
            this.JobName = jobName;
            this.DutCode = dutCode;
            this.DutName = dutName;
            this.LocCode = locCode;
            this.LocName = locName;
            this.EmpCode = empCode;
            this.EmpName = empName;
            this.MobileTel = mobileTel;
            this.OfficeTel = officeTel;
            this.LyncOnnet = lyncOnnet;
            this.OfficeTel2 = officeTel2;
            this.OfficeTelExt = officeTelExt;
            this.CountryCode = countryCode;
            this.CountryName = countryName;
            this.OrgchartName = orgchartName;
            this.HasSubDept = hasSubDept;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Idx value.
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// Gets or sets the BookmarkUserID value.
        /// </summary>
        public string BookmarkUserID { get; set; }

        /// <summary>
        /// Gets or sets the BookmarkAlias value.
        /// </summary>
        public string BookmarkAlias { get; set; }

        public Guid AliasID { get; set; }

        /// <summary>
        /// Gets or sets the EmailType value.
        /// </summary>
        public string EmailType { get; set; }

        /// <summary>
        /// Gets or sets the EntryType value.
        /// </summary>
        public int EntryType { get; set; }

        /// <summary>
        /// Gets or sets the UserID value.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the EmpID value.
        /// </summary>
        public string EmpID { get; set; }

        /// <summary>
        /// Gets or sets the CompanyCode value.
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets the DeptCode value.
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// Gets or sets the DeptName value.
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// Gets or sets the GroupCode value.
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Gets or sets the GroupName value.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the UserName value.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the DisplayName value.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress value.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the TitCode value.
        /// </summary>
        public string TitCode { get; set; }

        /// <summary>
        /// Gets or sets the TitName value.
        /// </summary>
        public string TitName { get; set; }

        /// <summary>
        /// Gets or sets the JobName value.
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Gets or sets the DutCode value.
        /// </summary>
        public string DutCode { get; set; }

        /// <summary>
        /// Gets or sets the DutName value.
        /// </summary>
        public string DutName { get; set; }

        /// <summary>
        /// Gets or sets the LocCode value.
        /// </summary>
        public string LocCode { get; set; }

        /// <summary>
        /// Gets or sets the LocName value.
        /// </summary>
        public string LocName { get; set; }

        /// <summary>
        /// Gets or sets the EmpCode value.
        /// </summary>
        public string EmpCode { get; set; }

        /// <summary>
        /// Gets or sets the EmpName value.
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// Gets or sets the MobileTel value.
        /// </summary>
        public string MobileTel { get; set; }

        /// <summary>
        /// Gets or sets the OfficeTel value.
        /// </summary>
        public string OfficeTel { get; set; }

        /// <summary>
        /// Gets or sets the LyncOnnet value.
        /// </summary>
        public string LyncOnnet { get; set; }

        /// <summary>
        /// Gets or sets the OfficeTel2 value.
        /// </summary>
        public string OfficeTel2 { get; set; }

        /// <summary>
        /// Gets or sets the OfficeTelExt value.
        /// </summary>
        public string OfficeTelExt { get; set; }

        /// <summary>
        /// Gets or sets the CountryCode value.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the CountryName value.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the OrgchartName value.
        /// </summary>
        public string OrgchartName { get; set; }

        /// <summary>
        /// Gets or sets the HasSubDept value.
        /// </summary>
        public string HasSubDept { get; set; }

        /// <summary>
        /// Gets or sets the CreateDate value.
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the UpdateDate value.
        /// </summary>
        public string UpdateDate { get; set; }

        #endregion


    }




    public class EmailOutputJson
    {
        public List<EmailBookMarkType> To { get; set; }
        public List<EmailBookMarkType> Cc { get; set; }
        public List<EmailBookMarkType> Bcc { get; set; }

    }



}
