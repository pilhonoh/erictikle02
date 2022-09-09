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
    /// 설명: Data access class for tb_Permissions table.
    /// 작성일 : 2014-05-13
    /// 작성자 : miksystem.com
    /// </summary>
    public class GlossaryPermissionsDac
    {
        private const string connectionStringName = "ConnGlossary";

        public GlossaryPermissionsDac() { }

        /// <summary>
        /// Inserts a record into the tb_Permissions table.
        /// </summary>
        /// <returns></returns>
        public void PermissionsInsert(PermissionsType permissionsType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Insert");

            db.AddInParameter(dbCommand, "GlossaryID", DbType.Int32, permissionsType.GlossaryID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.String, permissionsType.ToUserID);
            db.AddInParameter(dbCommand, "ToUserName", DbType.String, permissionsType.ToUserName);

            // Execute the query and return the new identity value
            db.ExecuteDataSet(dbCommand);

            //return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_Permissions table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<PermissionsType> PermissionsSelect(Int64 iD)
        {
            List<PermissionsType> listPermissionsType = new List<PermissionsType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Select");

            db.AddInParameter(dbCommand, "GlossaryID", DbType.Int64, iD);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PermissionsType permissionsType = new PermissionsType();
                        permissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
                        permissionsType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");
                        listPermissionsType.Add(permissionsType);
                    }
                }
            }
            return listPermissionsType;
        }

        /// <summary>
        /// Selects all records from the tb_Permissions table.
        /// </summary>
        public List<PermissionsType> PermissionsSelectAll()
        {
            List<PermissionsType> listPermissionsType = new List<PermissionsType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_SelectAll");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PermissionsType permissionsType = GetPermissionsTypeMapData(dr);
                        listPermissionsType.Add(permissionsType);
                    }
                }
            }
            return listPermissionsType;
        }

        /// <summary>
        /// Updates a record in the tb_Permissions table.
        /// </summary>
        public void PermissionsUpdate(PermissionsType permissionsType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Update");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, permissionsType.ID);
            db.AddInParameter(dbCommand, "GlossaryID", DbType.Int32, permissionsType.GlossaryID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.String, permissionsType.ToUserID);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a record from the tb_Permissions table by a composite primary key.
        /// </summary>
        public void PermissionsDelete(Int64 iD)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Delete");

            db.AddInParameter(dbCommand, "GlossaryID", DbType.Int64, iD);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// ViewPage Permisstion Check
        /// </summary>
        public int Permissions_Check(string ItemID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Permissions_Check");

            db.AddInParameter(dbCommand, "ItemID", DbType.String, ItemID);
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// Creates a new instance of the PermissionsType class and populates it with data from the specified DataRow.
        /// </summary>
        private PermissionsType GetPermissionsTypeMapData(DataRow dr)
        {
            PermissionsType permissionsType = new PermissionsType();
            permissionsType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
            permissionsType.GlossaryID = (dr["GlossaryID"] == DBNull.Value) ? 0 : dr.Field<int>("GlossaryID");
            permissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
            permissionsType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");

            return permissionsType;
        }
    }
}
