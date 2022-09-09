using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKT.Glossary.Type;


namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_EmailBookMark table.
    /// 작성일 : 2015-11-17
    /// 작성자 : miksystem.com
    /// </summary>
    public sealed class EmailBookMarkDac
    {
        private const string connectionStringName = "ConnGlossary";

        public EmailBookMarkDac() { }


        /// <summary>
        /// Inserts a record into the tb_EmailBookMark table.
        /// </summary>
        /// <returns></returns>
        public int EmailBookMarkInsert(EmailBookMarkType emailBookMarkType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_EmailBookMark_Insert");

            db.AddInParameter(dbCommand, "Idx", DbType.Int32, emailBookMarkType.Idx);
            db.AddInParameter(dbCommand, "BookmarkUserID", DbType.String, emailBookMarkType.BookmarkUserID);
            db.AddInParameter(dbCommand, "BookmarkAlias", DbType.String, emailBookMarkType.BookmarkAlias);
            db.AddInParameter(dbCommand, "AliasID", DbType.Guid, emailBookMarkType.AliasID);
            db.AddInParameter(dbCommand, "EmailType", DbType.String, emailBookMarkType.EmailType);
            db.AddInParameter(dbCommand, "EntryType", DbType.Int32, emailBookMarkType.EntryType);
            db.AddInParameter(dbCommand, "UserID", DbType.String, emailBookMarkType.UserID);
            db.AddInParameter(dbCommand, "EmpID", DbType.String, emailBookMarkType.EmpID);
            db.AddInParameter(dbCommand, "CompanyCode", DbType.String, emailBookMarkType.CompanyCode);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, emailBookMarkType.DeptCode);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, emailBookMarkType.DeptName);
            db.AddInParameter(dbCommand, "GroupCode", DbType.String, emailBookMarkType.GroupCode);
            db.AddInParameter(dbCommand, "GroupName", DbType.String, emailBookMarkType.GroupName);
            db.AddInParameter(dbCommand, "UserName", DbType.String, emailBookMarkType.UserName);
            db.AddInParameter(dbCommand, "DisplayName", DbType.String, emailBookMarkType.DisplayName);
            db.AddInParameter(dbCommand, "EmailAddress", DbType.String, emailBookMarkType.EmailAddress);
            db.AddInParameter(dbCommand, "TitCode", DbType.String, emailBookMarkType.TitCode);
            db.AddInParameter(dbCommand, "TitName", DbType.String, emailBookMarkType.TitName);
            db.AddInParameter(dbCommand, "JobName", DbType.String, emailBookMarkType.JobName);
            db.AddInParameter(dbCommand, "DutCode", DbType.String, emailBookMarkType.DutCode);
            db.AddInParameter(dbCommand, "DutName", DbType.String, emailBookMarkType.DutName);
            db.AddInParameter(dbCommand, "LocCode", DbType.String, emailBookMarkType.LocCode);
            db.AddInParameter(dbCommand, "LocName", DbType.String, emailBookMarkType.LocName);
            db.AddInParameter(dbCommand, "EmpCode", DbType.String, emailBookMarkType.EmpCode);
            db.AddInParameter(dbCommand, "EmpName", DbType.String, emailBookMarkType.EmpName);
            db.AddInParameter(dbCommand, "MobileTel", DbType.String, emailBookMarkType.MobileTel);
            db.AddInParameter(dbCommand, "OfficeTel", DbType.String, emailBookMarkType.OfficeTel);
            db.AddInParameter(dbCommand, "LyncOnnet", DbType.String, emailBookMarkType.LyncOnnet);
            db.AddInParameter(dbCommand, "OfficeTel2", DbType.String, emailBookMarkType.OfficeTel2);
            db.AddInParameter(dbCommand, "OfficeTelExt", DbType.String, emailBookMarkType.OfficeTelExt);
            db.AddInParameter(dbCommand, "CountryCode", DbType.String, emailBookMarkType.CountryCode);
            db.AddInParameter(dbCommand, "CountryName", DbType.String, emailBookMarkType.CountryName);
            db.AddInParameter(dbCommand, "OrgchartName", DbType.String, emailBookMarkType.OrgchartName);
            db.AddInParameter(dbCommand, "HasSubDept", DbType.String, emailBookMarkType.HasSubDept);

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }



        /// <summary>
        /// Updates a record in the tb_EmailBookMark table.
        /// </summary>
        public void EmailBookMarkUpdate(EmailBookMarkType emailBookMarkType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_EmailBookMark_Update");

            db.AddInParameter(dbCommand, "Idx", DbType.Int32, emailBookMarkType.Idx);
            db.AddInParameter(dbCommand, "BookmarkUserID", DbType.String, emailBookMarkType.BookmarkUserID);
            db.AddInParameter(dbCommand, "BookmarkAlias", DbType.String, emailBookMarkType.BookmarkAlias);
            db.AddInParameter(dbCommand, "EmailType", DbType.String, emailBookMarkType.EmailType);
            db.AddInParameter(dbCommand, "EntryType", DbType.Int32, emailBookMarkType.EntryType);
            db.AddInParameter(dbCommand, "UserID", DbType.String, emailBookMarkType.UserID);
            db.AddInParameter(dbCommand, "EmpID", DbType.String, emailBookMarkType.EmpID);
            db.AddInParameter(dbCommand, "CompanyCode", DbType.String, emailBookMarkType.CompanyCode);
            db.AddInParameter(dbCommand, "DeptCode", DbType.String, emailBookMarkType.DeptCode);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, emailBookMarkType.DeptName);
            db.AddInParameter(dbCommand, "GroupCode", DbType.String, emailBookMarkType.GroupCode);
            db.AddInParameter(dbCommand, "GroupName", DbType.String, emailBookMarkType.GroupName);
            db.AddInParameter(dbCommand, "UserName", DbType.String, emailBookMarkType.UserName);
            db.AddInParameter(dbCommand, "DisplayName", DbType.String, emailBookMarkType.DisplayName);
            db.AddInParameter(dbCommand, "EmailAddress", DbType.String, emailBookMarkType.EmailAddress);
            db.AddInParameter(dbCommand, "TitCode", DbType.String, emailBookMarkType.TitCode);
            db.AddInParameter(dbCommand, "TitName", DbType.String, emailBookMarkType.TitName);
            db.AddInParameter(dbCommand, "JobName", DbType.String, emailBookMarkType.JobName);
            db.AddInParameter(dbCommand, "DutCode", DbType.String, emailBookMarkType.DutCode);
            db.AddInParameter(dbCommand, "DutName", DbType.String, emailBookMarkType.DutName);
            db.AddInParameter(dbCommand, "LocCode", DbType.String, emailBookMarkType.LocCode);
            db.AddInParameter(dbCommand, "LocName", DbType.String, emailBookMarkType.LocName);
            db.AddInParameter(dbCommand, "EmpCode", DbType.String, emailBookMarkType.EmpCode);
            db.AddInParameter(dbCommand, "EmpName", DbType.String, emailBookMarkType.EmpName);
            db.AddInParameter(dbCommand, "MobileTel", DbType.String, emailBookMarkType.MobileTel);
            db.AddInParameter(dbCommand, "OfficeTel", DbType.String, emailBookMarkType.OfficeTel);
            db.AddInParameter(dbCommand, "LyncOnnet", DbType.String, emailBookMarkType.LyncOnnet);
            db.AddInParameter(dbCommand, "OfficeTel2", DbType.String, emailBookMarkType.OfficeTel2);
            db.AddInParameter(dbCommand, "OfficeTelExt", DbType.String, emailBookMarkType.OfficeTelExt);
            db.AddInParameter(dbCommand, "CountryCode", DbType.String, emailBookMarkType.CountryCode);
            db.AddInParameter(dbCommand, "CountryName", DbType.String, emailBookMarkType.CountryName);
            db.AddInParameter(dbCommand, "OrgchartName", DbType.String, emailBookMarkType.OrgchartName);
            db.AddInParameter(dbCommand, "HasSubDept", DbType.String, emailBookMarkType.HasSubDept);
            db.AddInParameter(dbCommand, "CreateDate", DbType.DateTime, emailBookMarkType.CreateDate);
            db.AddInParameter(dbCommand, "UpdateDate", DbType.DateTime, emailBookMarkType.UpdateDate);

            db.ExecuteNonQuery(dbCommand);
        }




        public DataSet EmailBookMarkSelect(string bookmarkUserID, Guid aliasid )
        {
            List<EmailBookMarkType> listEmailBookMarkType = new List<EmailBookMarkType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand dbCommand = db.GetStoredProcCommand("up_EmailBookMark_Select");
            db.AddInParameter(dbCommand, "BookmarkUserID", DbType.String, bookmarkUserID);
            db.AddInParameter(dbCommand, "AliasID", DbType.Guid, aliasid);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;

        }


        public DataSet EmailBookMarkAliasSelect(string bookmarkUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand dbCommand = db.GetStoredProcCommand("up_EmailBookMark_Alias_Select");
            db.AddInParameter(dbCommand, "BookmarkUserID", DbType.String, bookmarkUserID);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;

        }



        public void EmailBookMarkDelete(int idx, string bookmarkUserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_EmailBookMark_Delete");

            db.AddInParameter(dbCommand, "Idx", DbType.Int32, idx);
            db.AddInParameter(dbCommand, "BookmarkUserID", DbType.String, bookmarkUserID);

            db.ExecuteNonQuery(dbCommand);
        }

    }


}
