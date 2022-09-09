using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// Data (DataSet, DataTable) 관련 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class DataHelper
    {
        #region DataSet 변경관련 Utility

        /// <summary>
        ///   DataSet에서 원하는 테이블을 반환
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable ToTable(this DataSet ds, int TableIndex)
        {
            DataTable returnDt = null;

            try
            {
                if (ds == null || ds.Tables.Count <= 0) throw new ArgumentNullException();

                if (ds.Tables.Count > TableIndex)
                {
                    returnDt = ds.Tables[TableIndex];
                }
            }
            catch
            {
                ds = null;
            }

            return returnDt;
        }

        /// <summary>
        ///  DataSet에서 첫번재 테이블 반환
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable ToTable(this DataSet ds)
        {
            return ToTable(ds, 0);
        }

        #endregion DataSet 변경관련 Utility

        #region DataTable 관련 Utility
        /// <summary>
        /// Get data type of column
        /// </summary>
        public static Type GetColumnDataType(this DataTable tbl, string ColumnName)
        {
            try
            {
                return tbl.Columns[ColumnName].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception("OS_GetColumnDataType: \n" + ex.Message);
            }
        }

        /// <summary>
        /// Get data type of column
        /// </summary>
        public static Type GetColumnDataType(this DataTable tbl, int ColumnIndex)
        {
            try
            {
                return tbl.Columns[ColumnIndex].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception("OS_GetColumnDataType: \n" + ex.Message);
            }
        }

        /// <summary>
        /// Get table element with given column and row indices
        /// </summary>
        public static object GetElementValue(this DataTable tbl, int ColInd, int RowInd)
        {
            try
            {
                return tbl.Rows[RowInd][ColInd];
            }
            catch (Exception ex)
            {
                throw new Exception("OS_GetElementValue: \n" + ex.Message);
            }
        }

        /// <summary>
        /// Get table element with given column names and row indices
        /// </summary>
        public static object GetElementValue(this DataTable tbl, string ColumnName, int RowInd)
        {
            try
            {
                return tbl.Rows[RowInd][ColumnName];
            }
            catch (Exception ex)
            {
                throw new Exception("OS_GetElementValue: \n" + ex.Message);
            }
        }

        /// <summary>Assign value to a table element with given column and row indices
        /// </summary>
        public static void SetElementValue(this DataTable tbl, string ColumnName, int RowInd, object ElementValue)
        {
            try
            {
                tbl.Rows[RowInd][ColumnName] = ElementValue;
            }
            catch (Exception ex)
            {
                throw new Exception("OS_SetElementValue: \n" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// DataSet의 테이블을 HashTable 로 변환 한다.
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="value">값</param>
        /// <param name="ds">데이타셋</param>
        /// <returns>해쉬테이블</returns>
        public static Hashtable ConvertToHashTable(this DataSet ds, string key, string value)
        {
            Hashtable ht = new Hashtable(10000);

            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains(key) && dt.Columns.Contains(value))
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr[key] is string && dr[value] is string)
                                    {
                                        if (!ht.ContainsKey(dr[key].ToString()))
                                            ht.Add(dr[key] as string, dr[value] as string);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch { }

            return ht;
        }
    }
}