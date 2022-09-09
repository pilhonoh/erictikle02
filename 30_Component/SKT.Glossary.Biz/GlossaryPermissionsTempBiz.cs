using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryPermissionsTempBiz
    {
        /// <summary>
        /// Inserts a record into the tb_Permissions table.
        /// </summary>
        /// <returns></returns>
        public void PermissionsTempInsert(string GlossaryTempID, string ToUserID, string ToUserName)
        {
            GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
            PermissionsTempType PermissionsTempType = new PermissionsTempType();
            DataSet ds = new DataSet();

            string[] ToUser = ToUserID.Split('/');
            string[] ToName = ToUserName.Split('&');
            for (int i = 0; i < ToUser.Length; i++)
            {
                // User 인지 부서인지 Check
                GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                {
                    PermissionsTempType.GlossaryTempID = Convert.ToInt32(GlossaryTempID);
                    PermissionsTempType.ToUserID = ToUser[i];
                    PermissionsTempType.ToUserName = ToName[i];
                    dac.PermissionsTempInsert(PermissionsTempType);
                }
                else
                {
                    PermissionsInsert_Dept(GlossaryTempID, ToUser[i], ToName[i]);
                }
            }
        }

        /// <summary>
        /// 부서는 재귀 호출로 처리
        /// </summary>
        /// <param name="GlossaryTempID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="ToUserName"></param>
        public void PermissionsInsert_Dept(string GlossaryTempID, string ToUserID, string ToUserName)
        {
            //string connectionStringName = "ConnOrgChart";
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_list_department_person");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUserID.ToString());

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        string employeeID = (dr["employeeID"] == DBNull.Value) ? null : dr.Field<string>("employeeID");
                        string koreanName = (dr["koreanName"] == DBNull.Value) ? null : dr.Field<string>("koreanName");
                        if (!String.IsNullOrEmpty(employeeID)) { PermissionsTempInsert(GlossaryTempID, employeeID, (koreanName + "/" + ToUserName).ToString()); }
                    }
                }
            }

        }

        /// <summary>
        /// GlossaryTempID 값을 주면 그에 해당하는 Name과 사번을 반환하도록 변경함
        /// </summary>
        /// <returns>DataSet</returns>
        public List<PermissionsTempType> PermissionsTempSelect(Int64 iD)
        {
            GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
            DataSet ds = new DataSet();
            List<PermissionsTempType> listPermissionsTempType = dac.PermissionsTempSelect(iD);

            return listPermissionsTempType;
        }

        /// <summary>
        /// Selects all records from the tb_Permissions table.
        /// </summary>
        public List<PermissionsTempType> PermissionsSelectAll()
        {
            GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
            DataSet ds = new DataSet();
            List<PermissionsTempType> listPermissionsTempType = dac.PermissionsTempSelectAll();

            return listPermissionsTempType;
        }

        /// <summary>
        /// Updates a record in the tb_Permissions table.
        /// </summary>
        public void PermissionsUpdate(PermissionsTempType PermissionsTempType)
        {
            GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
            DataSet ds = new DataSet();
            //dac.PermissionsTempUpdate(PermissionsTempType);

            return;
        }

        /// <summary>
        /// Deletes a record from the tb_Permissions table by a composite primary key.
        /// </summary>
        public void PermissionsDelete(Int64 iD)
        {
            GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
            DataSet ds = new DataSet();
            //dac.PermissionsTempDelete(iD);

            return;
        }

        /// <summary>
        /// ViewPage Permisstion Check
        /// </summary>
        //public int Permissions_Check(string ItemID, string UserID)
        //{
        //    GlossaryPermissionsTempDac dac = new GlossaryPermissionsTempDac();
        //    DataSet ds = new DataSet();

        //    int returnValue = dac.PermissionsTemp_Check(ItemID, UserID);

        //    return returnValue;
        //}

        /// <summary>
        /// Creates a new instance of the PermissionsTempType class and populates it with data from the specified DataRow.
        /// </summary>
        private PermissionsTempType GetPermissionsTempTypeMapData(DataRow dr)
        {
            PermissionsTempType PermissionsTempType = new PermissionsTempType();
            PermissionsTempType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
            PermissionsTempType.GlossaryTempID = (dr["GlossaryTempID"] == DBNull.Value) ? 0 : dr.Field<int>("GlossaryTempID");
            PermissionsTempType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");

            return PermissionsTempType;
        }
    }
}
