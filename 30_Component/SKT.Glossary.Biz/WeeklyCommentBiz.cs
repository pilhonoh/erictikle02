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
    public class WeeklyCommentBiz
    {
        /// <summary>
        /// Deletes a record from the tb_WeeklyComment table by a composite primary key.
        /// </summary>
        public string WeeklyCommentDelete(Int64 weeklyCommentID, Int64? weeklyID = null)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();
            string rtnValue = weeklyCommentDac.WeeklyCommentDelete(weeklyCommentID, weeklyID);
            
            return rtnValue;
        }

        /// <summary>
        /// Inserts a record into the tb_WeeklyComment table.
        /// </summary>
        /// <returns></returns>
        public int WeeklyCommentInsert(WeeklyCommentType weeklyCommentType)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(weeklyCommentDac.WeeklyCommentInsert(weeklyCommentType));

            return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_WeeklyComment table.
        /// </summary>
        /// <returns>DataSet</returns>
        public WeeklyCommentType WeeklyCommentSelect(Int64 weeklyCommentID)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();
            WeeklyCommentType weeklyCommentType = new WeeklyCommentType();

            using (DataSet ds = weeklyCommentDac.WeeklyCommentSelect(weeklyCommentID))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    weeklyCommentType = GetWeeklyCommentTypeMapData(ds.Tables[0].Rows[0]);
                }
            }
            return weeklyCommentType;
        }

        /// <summary>
        /// Selects all records from the tb_WeeklyComment table.
        /// </summary>
        public List<WeeklyCommentType> WeeklyCommentSelectAll(Int64 weeklyID)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();
            List<WeeklyCommentType> listWeeklyCommentType = new List<WeeklyCommentType>();
            
            using (DataSet ds = weeklyCommentDac.WeeklyCommentSelectAll(weeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyCommentType weeklyCommentType = GetWeeklyCommentTypeMapData(dr);
                        listWeeklyCommentType.Add(weeklyCommentType);
                    }
                }
            }
            return listWeeklyCommentType;
        }

        /// <summary>
        /// Updates a record in the tb_WeeklyComment table.
        /// </summary>
        public void WeeklyCommentUpdate(WeeklyCommentType weeklyCommentType)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();
            weeklyCommentDac.WeeklyCommentUpdate(weeklyCommentType);
        }

        /// <summary>
        /// Mr.No
        /// WeeklyListNew List
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <returns></returns>
        public DataSet WeeklyComment_List_New(Int64 weeklyID)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();

            DataSet ds = weeklyCommentDac.WeeklyComment_List_New(weeklyID);

            return ds;
        }

        /// <summary>
        /// Mr.No
        /// </summary>
        /// <param name="weeklyCommentID"></param>
        /// <returns></returns>
        public DataSet WeeklyCommentSelect_New(Int64 weeklyCommentID)
        {
            WeeklyCommentDac weeklyCommentDac = new WeeklyCommentDac();

            DataSet ds = weeklyCommentDac.WeeklyCommentSelect(weeklyCommentID);

            return ds;
        }

        /// <summary>
        /// Creates a new instance of the WeeklyCommentType class and populates it with data from the specified DataRow.
        /// </summary>
        private WeeklyCommentType GetWeeklyCommentTypeMapData(DataRow dr)
        {
            WeeklyCommentType weeklyCommentType = new WeeklyCommentType();
            weeklyCommentType.WeeklyCommentID = (dr["WeeklyCommentID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyCommentID");
            weeklyCommentType.SUP_ID = (dr["SUP_ID"] == DBNull.Value) ? 0 : dr.Field<long>("SUP_ID");
            weeklyCommentType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            weeklyCommentType.Contents = (dr["Contents"] == DBNull.Value) ? String.Empty : dr.Field<string>("Contents");
            weeklyCommentType.UserID = (dr["UserID"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserID");
            weeklyCommentType.UserName = (dr["UserName"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserName");
            weeklyCommentType.DutyName = (dr["DutyName"] == DBNull.Value) ? String.Empty : dr.Field<string>("DutyName");
            weeklyCommentType.DeptName = (dr["DeptName"] == DBNull.Value) ? String.Empty : dr.Field<string>("DeptName");
            weeklyCommentType.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreateDateTime");
            weeklyCommentType.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("UpdateDateTime");

            return weeklyCommentType;
        }
    }
}
