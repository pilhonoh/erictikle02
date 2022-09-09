using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SKT.Glossary.Type;
using System.Data;

namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_FloatingMenuUserSetting table.
    /// 작성일 : 2013-09-11
    /// 작성자 : miksystem.com
    /// </summary>
    public sealed class FloatingMenuUserSettingDac
    {
        private const string connectionStringName = "ConnGlossary";

        public FloatingMenuUserSettingDac() { }

        /// <summary>
        /// Inserts a record into the tb_FloatingMenuUserSetting table.
        /// </summary>
        /// <returns></returns>
        public void FloatingMenuUserSettingInsert(FloatingMenuUserSettingType floatingMenuUserSettingType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_FloatingMenuUserSetting_Insert");

            db.AddInParameter(dbCommand, "UserID", DbType.String, floatingMenuUserSettingType.UserID);
            db.AddInParameter(dbCommand, "DisplayFloatingIconYN", DbType.String, floatingMenuUserSettingType.DisplayFloatingIconYN);
            db.AddInParameter(dbCommand, "DragSearchActivateYN", DbType.String, floatingMenuUserSettingType.DragSearchActivateYN);
            db.AddInParameter(dbCommand, "DoubleClickSearchActivateYN", DbType.String, floatingMenuUserSettingType.DoubleClickSearchActivateYN);
            db.AddInParameter(dbCommand, "PositionX", DbType.Int32, floatingMenuUserSettingType.PositionX);
            db.AddInParameter(dbCommand, "PositionY", DbType.Int32, floatingMenuUserSettingType.PositionY);
            db.AddInParameter(dbCommand, "CreatedDT", DbType.DateTime, floatingMenuUserSettingType.CreatedDT);
            db.AddInParameter(dbCommand, "UpdatedDT", DbType.DateTime, floatingMenuUserSettingType.UpdatedDT);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Selects a single record from the tb_FloatingMenuUserSetting table.
        /// </summary>
        /// <returns>DataSet</returns>
        public FloatingMenuUserSettingType FloatingMenuUserSettingSelect(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_FloatingMenuUserSetting_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            FloatingMenuUserSettingType floatingMenuUserSettingType = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                floatingMenuUserSettingType = GetFloatingMenuUserSettingTypeMapData(ds.Tables[0].Rows[0]);
            }
            return floatingMenuUserSettingType;
        }

        /// <summary>
        /// Selects all records from the tb_FloatingMenuUserSetting table.
        /// </summary>
        public List<FloatingMenuUserSettingType> FloatingMenuUserSettingSelectAll()
        {
            List<FloatingMenuUserSettingType> listFloatingMenuUserSettingType = new List<FloatingMenuUserSettingType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_FloatingMenuUserSetting_SelectAll");

            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FloatingMenuUserSettingType floatingMenuUserSettingType = GetFloatingMenuUserSettingTypeMapData(dr);
                    listFloatingMenuUserSettingType.Add(floatingMenuUserSettingType);
                }
            }
            return listFloatingMenuUserSettingType;
        }

        /// <summary>
        /// Updates a record in the tb_FloatingMenuUserSetting table.
        /// </summary>
        public void FloatingMenuUserSettingUpdate(FloatingMenuUserSettingType floatingMenuUserSettingType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_FloatingMenuUserSetting_Update");

            db.AddInParameter(dbCommand, "UserID", DbType.String, floatingMenuUserSettingType.UserID);
            db.AddInParameter(dbCommand, "DisplayFloatingIconYN", DbType.String, floatingMenuUserSettingType.DisplayFloatingIconYN);
            db.AddInParameter(dbCommand, "DragSearchActivateYN", DbType.String, floatingMenuUserSettingType.DragSearchActivateYN);
            db.AddInParameter(dbCommand, "DoubleClickSearchActivateYN", DbType.String, floatingMenuUserSettingType.DoubleClickSearchActivateYN);
            db.AddInParameter(dbCommand, "PositionX", DbType.Int32, floatingMenuUserSettingType.PositionX);
            db.AddInParameter(dbCommand, "PositionY", DbType.Int32, floatingMenuUserSettingType.PositionY);
            db.AddInParameter(dbCommand, "CreatedDT", DbType.DateTime, floatingMenuUserSettingType.CreatedDT);
            db.AddInParameter(dbCommand, "UpdatedDT", DbType.DateTime, floatingMenuUserSettingType.UpdatedDT);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a record from the tb_FloatingMenuUserSetting table by a composite primary key.
        /// </summary>
        public void FloatingMenuUserSettingDelete(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_FloatingMenuUserSetting_Delete");

            db.AddInParameter(dbCommand, "UserID", DbType.String, userID);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Creates a new instance of the FloatingMenuUserSettingType class and populates it with data from the specified DataRow.
        /// </summary>
        private FloatingMenuUserSettingType GetFloatingMenuUserSettingTypeMapData(DataRow dr)
        {
            FloatingMenuUserSettingType floatingMenuUserSettingType = new FloatingMenuUserSettingType();
            floatingMenuUserSettingType.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
            floatingMenuUserSettingType.DisplayFloatingIconYN = (dr["DisplayFloatingIconYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("DisplayFloatingIconYN");
            floatingMenuUserSettingType.DragSearchActivateYN = (dr["DragSearchActivateYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("DragSearchActivateYN");
            floatingMenuUserSettingType.DoubleClickSearchActivateYN = (dr["DoubleClickSearchActivateYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("DoubleClickSearchActivateYN");
            floatingMenuUserSettingType.TikleNoteYN = (dr["TikleNoteYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("TikleNoteYN");
            floatingMenuUserSettingType.PositionX = (dr["PositionX"] == DBNull.Value) ? 0 : dr.Field<int>("PositionX");
            floatingMenuUserSettingType.PositionY = (dr["PositionY"] == DBNull.Value) ? 0 : dr.Field<int>("PositionY");
            floatingMenuUserSettingType.CreatedDT = (dr["CreatedDT"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreatedDT");
            floatingMenuUserSettingType.UpdatedDT = (dr["UpdatedDT"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("UpdatedDT");

            return floatingMenuUserSettingType;
        }


        public void GlossaryFloatingSetInsert(string UserID, string FloatingSet)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MainSiteFloatingSet_Insert");
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            db.AddInParameter(dbCommand, "DisplayFloatingIconYN ", DbType.String, FloatingSet);
            db.ExecuteNonQuery(dbCommand);
        }

        public void GlossaryTikleNoteSetInsert(string UserID, string TikleNoteSet)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_MainSiteTikleNoteSet_Insert");
            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            db.AddInParameter(dbCommand, "TikleNoteYN ", DbType.String, TikleNoteSet);
            db.ExecuteNonQuery(dbCommand);
        }
    }
}
