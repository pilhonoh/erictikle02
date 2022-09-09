using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SKT.Common
{
    public class DatabaseMethod
    {
        // SqlDataSet datarow에서 데이터 꺼내기
        public static object GetDataRow(DataRow dr, string column, object returnDefualt)
        {
            try
            {
                object rtVal = dr[column];
                if (rtVal == DBNull.Value) return returnDefualt;
                return rtVal;
            }
            catch { return returnDefualt; }
        }

        public static bool CompareDataToString(DataRow dr, string column, string cmp)
        {
            try { return Convert.ToString(dr[column]).Equals(cmp); }
            catch { return false; }
        }

        public static object GetDBNullProcessedString(string CheckString)
        {
            if (string.IsNullOrEmpty(CheckString))
            {
                return DBNull.Value;
            }
            return CheckString;
        }
    }
}