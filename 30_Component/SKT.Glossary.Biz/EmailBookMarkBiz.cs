using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Transactions;
using SKT.Common;
using System.Configuration;

namespace SKT.Glossary.Biz
{
    public class EmailBookMarkBiz
    {
        public int EmailBookMarkInsert(EmailBookMarkType bookmarkType)
        {
            EmailBookMarkDac bookmarkDac = new EmailBookMarkDac();
            int returnValue = Convert.ToInt32(bookmarkDac.EmailBookMarkInsert(bookmarkType));
            return returnValue;
        }

        public void EmailBookMarkInsert(List<EmailBookMarkType> bookmarkType)
        {
            EmailBookMarkDac bookmarkDac = new EmailBookMarkDac();

            foreach (EmailBookMarkType item in bookmarkType)
            {
                bookmarkDac.EmailBookMarkInsert(item);
            }        
            
        }


        public void EmailBookMarkUpdate(List<EmailBookMarkType> bookmarkType)
        {
            EmailBookMarkDac bookmarkDac = new EmailBookMarkDac();

            foreach (EmailBookMarkType item in bookmarkType)
            {
                bookmarkDac.EmailBookMarkUpdate(item);
            }

        }




        public List<EmailBookMarkType> EmailBookMarkSelect(string bookmarkUserID, Guid aliasid)
        {
            EmailBookMarkDac bookmarkDac = new EmailBookMarkDac();
            List<EmailBookMarkType> listEmailBookMarkType = new List<EmailBookMarkType>();
            EmailBookMarkType emailbookmarkType = new EmailBookMarkType();

            using (DataSet ds = bookmarkDac.EmailBookMarkSelect(bookmarkUserID, aliasid))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        emailbookmarkType = GetEmailBookMarkTypeMapData(dr);

                        listEmailBookMarkType.Add(emailbookmarkType);
                    }
                }
            }
            return listEmailBookMarkType;
        }

        public DataSet EmailBookMarkAliasSelect(string bookmarkUserID)
        {
            EmailBookMarkDac bookmarkDac = new EmailBookMarkDac();

            DataSet ds = bookmarkDac.EmailBookMarkAliasSelect(bookmarkUserID);
        
            return ds;
        }


        /// <summary>
        /// Creates a new instance of the EmailBookMarkType class and populates it with data from the specified DataRow.
        /// </summary>
        public static EmailBookMarkType GetEmailBookMarkTypeMapData(DataRow dr)
        {
            EmailBookMarkType emailBookMarkType = new EmailBookMarkType();
            emailBookMarkType.Idx = (dr["Idx"] == DBNull.Value) ? 0 : dr.Field<int>("Idx");
            emailBookMarkType.BookmarkUserID = (dr["BookmarkUserID"] == DBNull.Value) ? "" : dr.Field<string>("BookmarkUserID");
            emailBookMarkType.BookmarkAlias = (dr["BookmarkAlias"] == DBNull.Value) ? "" : dr.Field<string>("BookmarkAlias");
            emailBookMarkType.EmailType = (dr["EmailType"] == DBNull.Value) ? "" : dr.Field<string>("EmailType");
            emailBookMarkType.EntryType = (dr["EntryType"] == DBNull.Value) ? 0 : dr.Field<int>("EntryType");
            emailBookMarkType.UserID = (dr["UserID"] == DBNull.Value) ? "" : dr.Field<string>("UserID");
            emailBookMarkType.EmpID = (dr["EmpID"] == DBNull.Value) ? "" : dr.Field<string>("EmpID");
            emailBookMarkType.CompanyCode = (dr["CompanyCode"] == DBNull.Value) ? "" : dr.Field<string>("CompanyCode");
            emailBookMarkType.DeptCode = (dr["DeptCode"] == DBNull.Value) ? "" : dr.Field<string>("DeptCode");
            emailBookMarkType.DeptName = (dr["DeptName"] == DBNull.Value) ? "" : dr.Field<string>("DeptName");
            emailBookMarkType.GroupCode = (dr["GroupCode"] == DBNull.Value) ? "" : dr.Field<string>("GroupCode");
            emailBookMarkType.GroupName = (dr["GroupName"] == DBNull.Value) ? "" : dr.Field<string>("GroupName");
            emailBookMarkType.UserName = (dr["UserName"] == DBNull.Value) ? "" : dr.Field<string>("UserName");
            emailBookMarkType.DisplayName = (dr["DisplayName"] == DBNull.Value) ? "" : dr.Field<string>("DisplayName");
            emailBookMarkType.EmailAddress = (dr["EmailAddress"] == DBNull.Value) ? "" : dr.Field<string>("EmailAddress");
            emailBookMarkType.TitCode = (dr["TitCode"] == DBNull.Value) ? "" : dr.Field<string>("TitCode");
            emailBookMarkType.TitName = (dr["TitName"] == DBNull.Value) ? "" : dr.Field<string>("TitName");
            emailBookMarkType.JobName = (dr["JobName"] == DBNull.Value) ? "" : dr.Field<string>("JobName");
            emailBookMarkType.DutCode = (dr["DutCode"] == DBNull.Value) ? "" : dr.Field<string>("DutCode");
            emailBookMarkType.DutName = (dr["DutName"] == DBNull.Value) ? "" : dr.Field<string>("DutName");
            emailBookMarkType.LocCode = (dr["LocCode"] == DBNull.Value) ? "" : dr.Field<string>("LocCode");
            emailBookMarkType.LocName = (dr["LocName"] == DBNull.Value) ? "" : dr.Field<string>("LocName");
            emailBookMarkType.EmpCode = (dr["EmpCode"] == DBNull.Value) ? "" : dr.Field<string>("EmpCode");
            emailBookMarkType.EmpName = (dr["EmpName"] == DBNull.Value) ? "" : dr.Field<string>("EmpName");
            emailBookMarkType.MobileTel = (dr["MobileTel"] == DBNull.Value) ? "" : dr.Field<string>("MobileTel");
            emailBookMarkType.OfficeTel = (dr["OfficeTel"] == DBNull.Value) ? "" : dr.Field<string>("OfficeTel");
            emailBookMarkType.LyncOnnet = (dr["LyncOnnet"] == DBNull.Value) ? "" : dr.Field<string>("LyncOnnet");
            emailBookMarkType.OfficeTel2 = (dr["OfficeTel2"] == DBNull.Value) ? "" : dr.Field<string>("OfficeTel2");
            emailBookMarkType.OfficeTelExt = (dr["OfficeTelExt"] == DBNull.Value) ? "" : dr.Field<string>("OfficeTelExt");
            emailBookMarkType.CountryCode = (dr["CountryCode"] == DBNull.Value) ? "" : dr.Field<string>("CountryCode");
            emailBookMarkType.CountryName = (dr["CountryName"] == DBNull.Value) ? "" : dr.Field<string>("CountryName");
            emailBookMarkType.OrgchartName = (dr["OrgchartName"] == DBNull.Value) ? "" : dr.Field<string>("OrgchartName");
            emailBookMarkType.HasSubDept = (dr["HasSubDept"] == DBNull.Value) ? null : dr.Field<string>("HasSubDept");
            emailBookMarkType.CreateDate = (dr["CreateDate"] == DBNull.Value) ? "" : dr.Field<string>("CreateDate");
            emailBookMarkType.UpdateDate = (dr["UpdateDate"] == DBNull.Value) ? "" : dr.Field<string>("UpdateDate");

            return emailBookMarkType;
        }


    }
}
