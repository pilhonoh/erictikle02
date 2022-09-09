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
using System.Web;
using System.Globalization;



namespace SKT.Glossary.Biz
{
    public class WeeklyBiz
    {
        public void WeeklyDelete(string weeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.WeeklyDelete(weeklyID);
        }

        public int WeeklyInsert(WeeklyType weeklyType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            int returnValue = Convert.ToInt32(weeklyDac.WeeklyInsert(weeklyType));
            return returnValue;
        }


        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public WeeklyType WeeklySelect(string userID, int yearWeek, string deptCode, DateTime startWeekDate, DateTime endWeekDate, string WeeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            WeeklyType weeklyType = new WeeklyType();

            using (DataSet ds = weeklyDac.WeeklySelect(userID, yearWeek, deptCode, startWeekDate, endWeekDate, WeeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        weeklyType = GetWeeklyTypeMapData(dr);
                    }
                }
            }
            return weeklyType;
        }


        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public WeeklyType WeeklySelectWeeklyID(string weeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            WeeklyType weeklyType = new WeeklyType();

            using (DataSet ds = weeklyDac.WeeklySelectWeeklyID(weeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        weeklyType = GetWeeklyTypeMapData(dr);
                    }
                }
            }
            return weeklyType;
        }

        /// <summary>
        /// Selects Single records from the tb_Weekly table.
        /// </summary>
        public WeeklyType WeeklySelectWeeklyFromUserID(string weeklyID, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            WeeklyType weeklyType = new WeeklyType();

            using (DataSet ds = weeklyDac.WeeklySelectWeeklyIDFromUserID(weeklyID, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        weeklyType = GetWeeklyTypeMapData(dr);
                    }
                }
            }
            return weeklyType;
        }

        /// <summary>
        /// Mr.No
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <returns></returns>
        public DataSet WeeklySelect_WeeklyID(string weeklyID, string userID = null)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.WeeklySelectWeeklyID(weeklyID, userID);

            if (weeklyID != "0")
            {
                if (ds.Tables[0].Columns.Contains("TempYN"))
                {
                    ds.Tables[0].Rows[0]["TempYN"] = (ds.Tables[0].Rows[0]["TempYN"] == DBNull.Value) ? "N" : ds.Tables[0].Rows[0].Field<string>("TempYN").Trim();
                }
                if (ds.Tables[0].Columns.Contains("Contents"))
                {
                    ds.Tables[0].Rows[0]["Contents"] = (ds.Tables[0].Rows[0]["Contents"] == DBNull.Value) ? string.Empty : HttpUtility.HtmlDecode(ds.Tables[0].Rows[0].Field<string>("Contents")); //.Replace("\r\n", "<p>");
                }
                if (ds.Tables[0].Columns.Contains("MemoWriterID"))
                {
                    ds.Tables[0].Rows[0]["MemoWriterID"] = (ds.Tables[0].Rows[0]["MemoWriterID"] == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<string>("MemoWriterID");
                }
                if (ds.Tables[0].Columns.Contains("MemoContents"))
                {
                    ds.Tables[0].Rows[0]["MemoContents"] = (ds.Tables[0].Rows[0]["MemoContents"] == DBNull.Value) ? string.Empty : HttpUtility.HtmlDecode(ds.Tables[0].Rows[0].Field<string>("MemoContents"));
                }
                if (ds.Tables[0].Columns.Contains("Year") && ds.Tables[0].Columns.Contains("YearWeek"))
                {
                    ds.Tables[0].Rows[0]["Month"] = (ds.Tables[0].Rows[0]["Month"] == DBNull.Value) ? string.Empty : FirstDateOfWeekISO8601(Convert.ToInt32(ds.Tables[0].Rows[0]["Year"]), Convert.ToInt32(ds.Tables[0].Rows[0]["YearWeek"]));
                }

                //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련
                if (ds.Tables[0].Columns.Contains("AbsenceMsg"))
                    ds.Tables[0].Rows[0]["AbsenceMsg"] = (ds.Tables[0].Rows[0]["AbsenceMsg"] == DBNull.Value) ? "-" : ds.Tables[0].Rows[0].Field<string>("AbsenceMsg").Trim();

                //2016.01.25
                if (ds.Tables[0].Columns.Contains("PositionName"))
                {
                    ds.Tables[0].Rows[0]["PositionName"] = (ds.Tables[0].Rows[0]["PositionName"] == DBNull.Value) ? String.Empty : ds.Tables[0].Rows[0].Field<string>("PositionName");
                }
            }
            return ds;
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<WeeklyType> WeeklySelectDeptCodeMyTeam(string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeMyTeam(deptCode, weekDateTime, EndDate, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet WeeklySelectDeptCodeMyTeamByDataSet(string deptCode, DateTime weekDateTime, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            DataSet ds = weeklyDac.WeeklySelectDeptCodeMyTeam(deptCode, weekDateTime, new DateTime(0), UserID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr.Table.Columns.Contains("TempYN"))
                    dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
            }

            return ds;
        }


        //public DataSet WeeklySelectDeptCodeMyTeamByDataSet(string deptCode, DateTime weekDateTime, DateTime endDateTime, string UserID)
        //{
        //    WeeklyDac weeklyDac = new WeeklyDac();
        //    List<WeeklyType> listWeeklyType = new List<WeeklyType>();

        //    DataSet ds = weeklyDac.WeeklySelectDeptCodeMyTeam(deptCode, weekDateTime, endDateTime, UserID, "team");
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        if (dr.Table.Columns.Contains("TempYN"))
        //            dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
        //    }

        //    return ds;
        //}

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        //public List<WeeklyType> WeeklySelectDeptCodeOfficer(string deptCode, DateTime weekDateTime)
        //{
        //    WeeklyDac weeklyDac = new WeeklyDac();
        //    List<WeeklyType> listWeeklyType = new List<WeeklyType>();

        //    using (DataSet ds = weeklyDac.WeeklySelectDeptCodeOfficer(deptCode, weekDateTime, new DateTime(0), string.Empty))
        //    {
        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
        //                listWeeklyType.Add(weeklyType);
        //            }
        //        }
        //    }
        //    return listWeeklyType;
        //}

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet WeeklySelectDeptCodeOfficerByDataSet(string deptCode, DateTime weekDateTime, string UserID, int year, int week)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            DataSet ds = weeklyDac.WeeklySelectDeptCodeOfficer(deptCode, weekDateTime, new DateTime(0), string.Empty, UserID, year, week);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr.Table.Columns.Contains("TempYN"))
                    dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
            }
            return ds;
        }

        /// <summary>
        /// Mr.No
        /// 프린트 or 엑셀에서 필요해서 return 값 다르게 따로 만들었습니다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <param name="Kind"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_DeptCode_Officer(string deptCode, DateTime weekDateTime, DateTime EndDate, string PositionName, string UserID, int year, int week)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeOfficer(deptCode, weekDateTime, EndDate, PositionName, UserID, year, week))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }




        /// <summary>
        /// Mr.kim
        /// CEO 임원 weekly 개별 출력
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <param name="Kind"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_Ceo_Each_Print(string TargetUserID, string UserID, string WeeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklyCeoEachPrint(TargetUserID, UserID, WeeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Mr.Kim
        /// 프린트 or 엑셀에서 필요해서 return 값 다르게 따로 만들었습니다. (이형희 MNO 총괄 예외)
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <param name="Kind"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_DeptCode_Mno(string actiontype, string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID, int year, int week)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeMno(actiontype, deptCode, weekDateTime, EndDate, UserID, year, week))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }


        /// <summary>
        /// 임원일 경우에 직속조직만 출력 관련 SP
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <param name="EndDate"></param>
        /// <param name="PositionName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_DeptCode_DirectOrg(string deptCode, DateTime weekDateTime, DateTime EndDate, string PositionName, string UserID, int year, int week)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCode_DirectOrg(deptCode, weekDateTime, EndDate, PositionName, UserID, year, week))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// 자회사 Weekly만 가져오기
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_ExternalRepresent(DateTime weekDateTime, int year, int week)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.Weekly_Select_ExternalRepresent(weekDateTime, year, week))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }



        public List<WeeklyType> Weekly_Select_DeptCode_CEO(DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeCEO(weekDateTime, EndDate, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        weeklyType.TextContents = weeklyType.TextContents ?? string.Empty;
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        public List<WeeklyType> Weekly_Select_DeptCode_MNO(DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeMNO(weekDateTime, EndDate, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        weeklyType.TextContents = weeklyType.TextContents ?? string.Empty;
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<WeeklyType> WeeklySelectDeptCodeUpper(string deptCode, DateTime weekDateTime, DateTime EndDate, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptCodeUpper(deptCode, weekDateTime, EndDate, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Mr.No
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public List<WeeklyType> Weekly_Select_DeptCode_OrgChart(string deptCode, DateTime weekDateTime, string Kind, DateTime EndDate, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.Weekly_Select_DeptCode_OrgChart(deptCode, weekDateTime, Kind, EndDate, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects a single record from the tb_Weekly table.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet WeeklySelectDeptCodeUpperByDataSet(string deptCode, DateTime weekDateTime, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            DataSet ds = weeklyDac.WeeklySelectDeptCodeUpper(deptCode, weekDateTime, new DateTime(0), UserID);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr.Table.Columns.Contains("TempYN"))
                    dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
                if (dr.Table.Columns.Contains("Path") && dr.Table.Columns.Contains("upperDepartmentNumber"))
                {
                    string[] path = dr["Path"].ToString().Split('/');
                    if (dr["upperDepartmentNumber"].ToString() != path[path.Length - 2])
                        ds.Tables[0].Rows.Remove(dr);
                }
            }

            return ds;
        }

        /// <summary>
        /// 2015-06-13 김성환 추가 예외부서 view 허용
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet WeeklySelectExceptionViewUserByDataSet(string deptCode, DateTime weekDateTime, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            DataSet ds = weeklyDac.WeeklySelectExceptionViewUser(deptCode, weekDateTime, UserID);

            return ds;
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public List<WeeklyType> WeeklySelectNewestDown(string deptCode, DateTime weekDateTime, DateTime EndDate)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectNewestDown(deptCode, weekDateTime, EndDate))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public List<WeeklyType> WeeklySelectNewestMyTeam(string deptCode, DateTime weekDateTime, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectNewestMyTeam(deptCode, weekDateTime, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table by newest.
        /// </summary>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        public List<WeeklyType> WeeklySelectNewestUpper(string deptCode, DateTime weekDateTime, string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectNewestUpper(deptCode, weekDateTime, UserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// Selects all records from the tb_Weekly table.
        /// </summary>
        public List<WeeklyType> WeeklySelectUser(string userID, int year, DateTime StartDate, DateTime EndDate)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectUser(userID, year, StartDate, EndDate))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }

        /// <summary>
        /// 작성자별 보기에서는 자신의 UserID 값이 필요하여 따로 만듬
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<WeeklyType> WeeklySelectUser_User(string userID, int year, DateTime StartDate, DateTime EndDate, string MyUserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectUser_User(userID, year, StartDate, EndDate, MyUserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }
        //2015.06.01 KSM 추가. 팀장 부서이동 오류수정
        public List<WeeklyType> WeeklySelectUser_Dept(string UserID, int year, DateTime StartDate, DateTime EndDate, string MyUserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyType> listWeeklyType = new List<WeeklyType>();

            using (DataSet ds = weeklyDac.WeeklySelectUser_Dept(UserID, year, StartDate, EndDate, MyUserID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }
            return listWeeklyType;
        }
        //2015.06.01 //
        /// <summary>
        /// Updates a record in the tb_Weekly table.
        /// </summary>
        public void WeeklyUpdate(WeeklyType weeklyType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.WeeklyUpdate(weeklyType);
        }



        /// <summary>
        /// DeptCode(부서코드)에 해당하는 사용자 정보들을 불러온다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public List<WeeklyUserType> WeeklySelectDeptUserList(string deptCode)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            List<WeeklyUserType> listWeeklyUserType = new List<WeeklyUserType>();

            using (DataSet ds = weeklyDac.WeeklySelectDeptUserList(deptCode, 0))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyUserType weeklyUserType = GetWeeklyUserTypeMapData(dr);
                        listWeeklyUserType.Add(weeklyUserType);
                    }
                }
            }
            return listWeeklyUserType;
        }

        /// <summary>
        /// 메일 전송 로그를 쌓는다. 2015-06-24 김성환
        /// </summary>
        /// //WeeklyEmailSendLog_Insert(fromMail.ToString(),pTo,pCC,pBcc,pTitle,pContent)
        /// <returns></returns>
        public int WeeklyEmailSendLog_Insert(string fromMail, string pTo, string pCC, string pBcc, string pTitle, string pContent, string inserttype)
        {
            WeeklyMailDac weeklyDac = new WeeklyMailDac();

            return weeklyDac.WeeklyMailSendLog_Insert(fromMail, pTo, pCC, pBcc, pTitle, pContent, inserttype);
        }

        /// <summary>
        /// DeptCode(부서코드)에 해당하는 사용자 정보들을 불러온다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataSet WeeklySelectDeptUserListByDataSet(string deptCode, int viewLevel)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            return weeklyDac.WeeklySelectDeptUserList(deptCode, viewLevel);
        }

        /// <summary>
        /// DeptCode(부서코드)에 해당하는 사용자 정보들을 불러온다.
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataSet WeeklySelectDeptUpperListByDataSet(string deptCode, int viewLevel)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            return weeklyDac.WeeklySelectDeptUpperList(deptCode, viewLevel);
        }

        /// <summary>
        /// UserInfo
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet UserInfo_Select(string userID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            return weeklyDac.UserInfo_Select(userID);
        }

        public static WeeklyUserType GetWeeklyUserTypeMapData(DataRow dr)
        {
            WeeklyUserType weeklyUserType = new WeeklyUserType();
            if (dr.Table.Columns.Contains("UserID"))
                weeklyUserType.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
            if (dr.Table.Columns.Contains("UserName"))
                weeklyUserType.UserName = (dr["UserName"] == DBNull.Value) ? null : dr.Field<string>("UserName");
            if (dr.Table.Columns.Contains("PositionName"))
                weeklyUserType.PositionName = (dr["PositionName"] == DBNull.Value) ? null : dr.Field<string>("PositionName");
            if (dr.Table.Columns.Contains("WeeklyID"))
                weeklyUserType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            if (dr.Table.Columns.Contains("Contents"))
                weeklyUserType.Contents = (dr["Contents"] == DBNull.Value) ? null : HttpUtility.HtmlDecode(dr.Field<string>("Contents"));
            if (dr.Table.Columns.Contains("TextContents"))
                weeklyUserType.TextContents = (dr["TextContents"] == DBNull.Value) ? null : dr.Field<string>("TextContents");
            if (dr.Table.Columns.Contains("TempYN"))
                weeklyUserType.TempYN = (dr["TempYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TempYN").Trim();
            if (dr.Table.Columns.Contains("UpdateDateTime"))
                weeklyUserType.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("UpdateDateTime");

            return weeklyUserType;
        }

        /// <summary>
        /// Creates a new instance of the WeeklyType class and populates it with data from the specified DataRow.
        /// </summary>
        public static WeeklyType GetWeeklyTypeMapData(DataRow dr)
        {
            WeeklyType weeklyType = new WeeklyType();
            if (dr.Table.Columns.Contains("WeeklyID"))
            {
                weeklyType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            }
            if (dr.Table.Columns.Contains("Contents"))
            {
                weeklyType.Contents = (dr["Contents"] == DBNull.Value) ? null : HttpUtility.HtmlDecode(dr.Field<string>("Contents"));
            }
            if (dr.Table.Columns.Contains("TextContents"))
            {
                weeklyType.TextContents = (dr["TextContents"] == DBNull.Value) ? null : dr.Field<string>("TextContents");
            }
            //if (dr.Table.Columns.Contains("CommentCount"))
            //{
            //    weeklyType.CommentCount = (dr["CommentCount"] == DBNull.Value) ? 0 : dr.Field<int>("CommentCount");
            //}
            if (dr.Table.Columns.Contains("Hits"))
            {
                weeklyType.Hits = (dr["Hits"] == DBNull.Value) ? 0 : dr.Field<int>("Hits");
            }
            if (dr.Table.Columns.Contains("LikeCount"))
            {
                weeklyType.LikeCount = (dr["LikeCount"] == DBNull.Value) ? 0 : dr.Field<int>("LikeCount");
            }
            if (dr.Table.Columns.Contains("ViewLevel"))
            {
                weeklyType.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<int>("ViewLevel");
            }
            //if (dr.Table.Columns.Contains("TeamChiefYN"))
            //{
            //    weeklyType.TeamChiefYN = (dr["TeamChiefYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TeamChiefYN").Trim();
            //}
            if (dr.Table.Columns.Contains("UserID"))
            {
                weeklyType.UserID = (dr["UserID"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserID");
            }
            if (dr.Table.Columns.Contains("UserName"))
            {
                weeklyType.UserName = (dr["UserName"] == DBNull.Value) ? String.Empty : dr.Field<string>("UserName");
            }
            if (dr.Table.Columns.Contains("DeptName"))
            {
                weeklyType.DeptName = (dr["DeptName"] == DBNull.Value) ? String.Empty : dr.Field<string>("DeptName");
            }
            if (dr.Table.Columns.Contains("DeptCode"))
            {
                weeklyType.DeptCode = (dr["DeptCode"] == DBNull.Value) ? String.Empty : dr.Field<string>("DeptCode");
            }
            if (dr.Table.Columns.Contains("Year"))
            {
                weeklyType.Year = (dr["Year"] == DBNull.Value) ? 0 : dr.Field<int>("Year");
            }
            if (dr.Table.Columns.Contains("YearWeek"))
            {
                weeklyType.YearWeek = (dr["YearWeek"] == DBNull.Value) ? 0 : dr.Field<int>("YearWeek");
            }
            if (dr.Table.Columns.Contains("TempYN"))
            {
                weeklyType.TempYN = (dr["TempYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TempYN").Trim();
            }
            if (dr.Table.Columns.Contains("CreateDateTime"))
            {
                weeklyType.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreateDateTime");
            }
            if (dr.Table.Columns.Contains("UpdateDateTime"))
            {
                weeklyType.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? weeklyType.CreateDateTime : dr.Field<DateTime>("UpdateDateTime");
            }
            // Mr.No
            if (dr.Table.Columns.Contains("Year") && dr.Table.Columns.Contains("YearWeek"))
            {
                if ((dr["Year"] != DBNull.Value) && (dr["YearWeek"] != DBNull.Value))
                {
                    weeklyType.Month = FirstDateOfWeekISO8601(Convert.ToInt32(dr["Year"]), Convert.ToInt32(dr["YearWeek"]));
                }
            }
            if (dr.Table.Columns.Contains("Year") && dr.Table.Columns.Contains("YearWeek"))
            {
                if ((dr["Year"] != DBNull.Value) && (dr["YearWeek"] != DBNull.Value))
                {
                    weeklyType.DateString = DateOfWeekISO8601(Convert.ToInt32(dr["Year"]), Convert.ToInt32(dr["YearWeek"]));
                }
            }
            if (dr.Table.Columns.Contains("photoURL"))
            {
                weeklyType.photoURL = (dr["photoURL"] == DBNull.Value) ? "/common/images/user_none.png" : dr.Field<string>("photoURL");
            }
            if (dr.Table.Columns.Contains("PositionName"))
            {
                weeklyType.PositionName = (dr["PositionName"] == DBNull.Value) ? String.Empty : dr.Field<string>("PositionName");
            }
            if (dr.Table.Columns.Contains("PermissionYN"))
            {
                weeklyType.PermissionYN = (dr["PermissionYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("PermissionYN");
            }
            if (dr.Table.Columns.Contains("PermissionsUserID"))
            {
                weeklyType.PermissionsUserID = (dr["PermissionsUserID"] == DBNull.Value) ? String.Empty : dr.Field<string>("PermissionsUserID");
            }
            if (dr.Table.Columns.Contains("MemoCreateDateTime"))
            {
                weeklyType.MemoCreateDateTime = (dr["MemoCreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("MemoCreateDateTime");
            }
            if (dr.Table.Columns.Contains("MemoUpdateDateTime"))
            {
                weeklyType.MemoUpdateDateTime = (dr["MemoUpdateDateTime"] == DBNull.Value) ? weeklyType.CreateDateTime : dr.Field<DateTime>("MemoUpdateDateTime");
            }

            return weeklyType;
        }

        /// <summary>
        /// Mr.No
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public static string FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            //return Convert.ToString(result.AddDays(-3).ToString("MM").Replace("0",""));
            //return result.AddDays(-3).StringMonthWeek();
            return result.AddDays(-3).StringStartEndWeek();
        }

        public static string DateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            //return Convert.ToString(result.AddDays(-3).ToString("MM").Replace("0",""));
            //return result.AddDays(-3).StringMonthWeek();
            return Convert.ToString(result.AddDays(-3).Date);
        }
        public static DateTime FirstDateOfWeekISO8601Date(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public void Weekly_Memo_Update(int WeeklyID, string MemoWriterID, string MemoContents, DateTime MemoCreateDateTime)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.Weekly_Memo_Update(WeeklyID, MemoWriterID, MemoContents, MemoCreateDateTime);
        }
        public int Weekly_Memo_Select(int WeeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            int returnValue = weeklyDac.Weekly_Memo_Select(WeeklyID);
            return returnValue;
        }


        #region 2015.07.10 zz17779 : 공통 위클리 관련 메서드 --------------------------------------

        //2015.07.10 zz17779 
        public DataSet Weekly_Common_Select_DeptCode_Upper(string deptCode, DateTime weekDateTime, DateTime endWeekDate, string userID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.Weekly_Common_Select_DeptCode_Upper(deptCode, weekDateTime, endWeekDate, userID);

            return ds;

        }

        /// <summary>
        /// 2015.07.13 zz17779
        /// </summary>
        /// <param name="weeklyType"></param>
        /// <returns></returns>
        public int WeeklyCommonInsert(WeeklyType weeklyType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            int returnValue = Convert.ToInt32(weeklyDac.WeeklyCommonInsert(weeklyType));
            return returnValue;
        }




        //2015.07.13 ZZ17779
        /// <summary>
        /// 공통 위클리 조회
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="yearWeek"></param>
        /// <param name="deptCode"></param>
        /// <param name="WeeklyID"></param>
        /// <returns></returns>
        public WeeklyType WeeklyCommonSelect(string userID, int yearWeek, string deptCode, DateTime startWeekDate, DateTime endWeekDate, string WeeklyID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            WeeklyType weeklyType = new WeeklyType();

            using (DataSet ds = weeklyDac.WeeklyCommonSelect(userID, yearWeek, deptCode, startWeekDate, endWeekDate, WeeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        weeklyType = GetWeeklyTypeMapData(dr);
                    }
                }
            }
            return weeklyType;
        }




        /// <summary>
        /// 2015.07.14 ZZ17779 : 공용 위클리WeeklyCommonSelectWeeklyID
        /// </summary>
        /// <param name="weeklyType"></param>
        public void WeeklyCommonUpdate(WeeklyType weeklyType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.WeeklyCommonUpdate(weeklyType);
        }


        /// <summary>
        /// 2015.07.14 ZZ17779 : 공용 위클리WeeklyCommonSelectWeeklyID
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <returns></returns>
        public DataSet WeeklyCommonSelectWeeklyID(string weeklyID, string userID = null, string cmmWeeklyFlag = "")
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.WeeklyCommonSelectWeeklyID(weeklyID, userID);

            if (weeklyID != "0")
            {
                if (ds.Tables[0].Columns.Contains("TempYN"))
                {
                    ds.Tables[0].Rows[0]["TempYN"] = (ds.Tables[0].Rows[0]["TempYN"] == DBNull.Value) ? "N" : ds.Tables[0].Rows[0].Field<string>("TempYN").Trim();
                }
                if (ds.Tables[0].Columns.Contains("Contents"))
                {
                    ds.Tables[0].Rows[0]["Contents"] = (ds.Tables[0].Rows[0]["Contents"] == DBNull.Value) ? string.Empty : HttpUtility.HtmlDecode(ds.Tables[0].Rows[0].Field<string>("Contents")); //.Replace("\r\n", "<p>");
                }
                if (ds.Tables[0].Columns.Contains("MemoWriterID"))
                {
                    ds.Tables[0].Rows[0]["MemoWriterID"] = (ds.Tables[0].Rows[0]["MemoWriterID"] == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<string>("MemoWriterID");
                }
                if (ds.Tables[0].Columns.Contains("MemoContents"))
                {
                    ds.Tables[0].Rows[0]["MemoContents"] = (ds.Tables[0].Rows[0]["MemoContents"] == DBNull.Value) ? string.Empty : HttpUtility.HtmlDecode(ds.Tables[0].Rows[0].Field<string>("MemoContents"));
                }
                if (ds.Tables[0].Columns.Contains("Year") && ds.Tables[0].Columns.Contains("YearWeek"))
                {
                    ds.Tables[0].Rows[0]["Month"] = (ds.Tables[0].Rows[0]["Month"] == DBNull.Value) ? string.Empty : FirstDateOfWeekISO8601(Convert.ToInt32(ds.Tables[0].Rows[0]["Year"]), Convert.ToInt32(ds.Tables[0].Rows[0]["YearWeek"]));
                }



            }
            return ds;
        }


        #endregion //2015.07.10 zz17779 : 공통 위클리 관련 메서드 --------------------------------------


        #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 ----------------------------


        public DataSet Weekly_Select_Detail_UserAbsence(string idx)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.Weekly_Select_Detail_UserAbsence(idx);

            return ds;
        }

        //로그인 대상자에게 위임한 대상 조회
        public DataSet Weekly_Select_UserAbsence_MyList(string absence_UserID, DateTime weekDateTime, string startDate, string endDate)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.Weekly_Select_UserAbsence_MyList(absence_UserID, weekDateTime, startDate, endDate);

            return ds;
        }

        //위임한 대상 조회
        public DataSet Weekly_Select_UserAbsence(string userid)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.Weekly_Select_UserAbsence(userid);

            return ds;
        }

        //수정
        public void Weekly_Update_UserAbsence(WeeklyAbsenceType AbsenceType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.Weekly_Update_UserAbsence(AbsenceType);
        }

        //저장
        public void Weekly_Insert_UserAbsence(WeeklyAbsenceType AbsenceType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.Weekly_Insert_UserAbsence(AbsenceType);
        }

        //위임 해제
        public void Weekly_Delete_UserAbsence(int absenceIdx)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.Weekly_Delete_UserAbsence(absenceIdx);
        }

        //공통코드 조회
        public DataSet CommonCode_Select(string pcode)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.CommonCode_Select(pcode);

            return ds;
        }

        //위임 중복체크 Weekly_Select_Duplicate_UserAbsence
        public DataSet Weekly_Select_Duplicate_UserAbsence(WeeklyAbsenceType AbsenceType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.Weekly_Select_Duplicate_UserAbsence(AbsenceType);

            return ds;
        }

        /// <summary>
        /// 위임 IF P097010
        /// </summary>
        /// <param name="duty"></param>
        /// <returns></returns>
        public int Weekly_Insert_UserAbsence_New(WeeklyDelegateDuty duty)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            return weeklyDac.Weekly_Insert_UserAbsence_New(duty);
        }

        #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 메서드 -----------------------


        //사장단 아웃룩 애드인용 Weekly Insert
        public void WeeklyInsertByMail(string email, string htmlContents, string textContents, DateTime currentDate)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            weeklyDac.WeeklyInsertByMail(email, htmlContents, textContents, currentDate);
        }



        #region  전년도 위클리 데이터 조회

        ////년도 조회
        //public DataSet Weekly_OldYearWeekly_Year_Select()
        //{
        //    WeeklyDac weeklyDac = new WeeklyDac();

        //    return weeklyDac.Weekly_OldYearWeekly_Year_Select();
        //}

        //전년도 부서 코드 조회
        //public DataSet Weekly_OldYearWeekly_Year_Select(string userid, string year)
        //{

        //    WeeklyDac weeklyDac = new WeeklyDac();

        //    return weeklyDac.Weekly_OldYearWeekly_Year_Select(userid, year);
        //}

        //전년도 부서 위클리 조회
        public List<WeeklyType> Weekly_OldYearWeekly_Select(string userid, string deptcode, DateTime StartDate, DateTime EndDate, string PrintType)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
             List<WeeklyType> listWeeklyType = new List<WeeklyType>();

             using (DataSet ds = weeklyDac.Weekly_OldYearWeekly_Select(userid, deptcode, StartDate, EndDate, PrintType))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType weeklyType = GetWeeklyTypeMapData(dr);
                        listWeeklyType.Add(weeklyType);
                    }
                }
            }

             return listWeeklyType;
        }


   

        #endregion //	- 전년도 위클리 데이터 조회



        /// <summary>
        /// 2016.01.29 백충기 : 위클리 관리자 통계화면에서 조직 select 조회.
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet WeeklyddlDeptCodeSelect()
        {
            WeeklyDac weeklyDac = new WeeklyDac();

            DataSet ds = weeklyDac.WeeklyddlDeptCodeSelect();

            return ds;
        }



        /*
         Author : 개발자- 백충기G, 리뷰자-이정선G
         CreateDae :  2016.04.06
         Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정        
        */
        public DataSet WeeklyAdditionalJob(string userID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            DataSet ds = weeklyDac.WeeklyAdditionalJob(userID);

            return ds;
        }

        public DataSet ExistsDeptCode(string UserID)
        {
            WeeklyDac weeklyDac = new WeeklyDac();
            DataSet ds = weeklyDac.ExistsDeptCode(UserID);

            return ds;
        }


    }
}

