using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_PermissionsTemp table.
    /// 작성일 : 2014-06-13
    /// 작성자 : miksystem.com
    /// </summary>
    public sealed class GlossaryPermissionsTempDac
    {
        private const string connectionStringName = "ConnGlossary";

        public GlossaryPermissionsTempDac() { }

        /// <summary>
        /// Inserts a record into the tb_PermissionsTemp table.
        /// </summary>
        /// <returns></returns>
        public int PermissionsTempInsert(PermissionsTempType permissionsTempType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_PermissionsTemp_Insert");

            db.AddInParameter(dbCommand, "GlossaryTempID", DbType.Int32, permissionsTempType.GlossaryTempID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.String, permissionsTempType.ToUserID);
            db.AddInParameter(dbCommand, "ToUserName", DbType.String, permissionsTempType.ToUserName);

            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_Permissions table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<PermissionsTempType> PermissionsTempSelect(Int64 iD)
        {
            List<PermissionsTempType> listPermissionsType = new List<PermissionsTempType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_PermissionsTemp_Select");

            db.AddInParameter(dbCommand, "GlossaryTempID", DbType.Int64, iD);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PermissionsTempType permissionsType = new PermissionsTempType();
                        permissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
                        permissionsType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");
                        listPermissionsType.Add(permissionsType);
                    }
                }
            }
            return listPermissionsType;
        }

        /// <summary>
        /// Selects all records from the tb_PermissionsTemp table.
        /// </summary>
        public List<PermissionsTempType> PermissionsTempSelectAll()
        {
            List<PermissionsTempType> listPermissionsTempType = new List<PermissionsTempType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_PermissionsTemp_SelectAll");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PermissionsTempType permissionsTempType = GetPermissionsTempTypeMapData(dr);
                        listPermissionsTempType.Add(permissionsTempType);
                    }
                }
            }
            return listPermissionsTempType;
        }

        /// <summary>
        /// Updates a record in the tb_PermissionsTemp table.
        /// </summary>
        public void PermissionsTempUpdate(PermissionsTempType permissionsTempType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_PermissionsTemp_Update");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, permissionsTempType.ID);
            db.AddInParameter(dbCommand, "GlossaryTempID", DbType.Int32, permissionsTempType.GlossaryTempID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.String, permissionsTempType.ToUserID);
            db.AddInParameter(dbCommand, "ToUserName", DbType.String, permissionsTempType.ToUserName);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a record from the tb_PermissionsTemp table by a composite primary key.
        /// </summary>
        public void PermissionsTempDelete(Int64 iD)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_PermissionsTemp_Delete");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, iD);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Creates a new instance of the PermissionsTempType class and populates it with data from the specified DataRow.
        /// </summary>
        private PermissionsTempType GetPermissionsTempTypeMapData(DataRow dr)
        {
            PermissionsTempType permissionsTempType = new PermissionsTempType();
            permissionsTempType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
            permissionsTempType.GlossaryTempID = (dr["GlossaryTempID"] == DBNull.Value) ? 0 : dr.Field<int>("GlossaryTempID");
            permissionsTempType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
            permissionsTempType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");

            return permissionsTempType;
        }
    }
}
