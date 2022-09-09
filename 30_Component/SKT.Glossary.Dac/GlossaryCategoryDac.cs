using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKT.Glossary.Type;
using SKT.Common;

namespace SKT.Glossary.Dac
{
    /// <summary>
    /// 설명: Data access class for tb_GlossaryCategory table.
    /// 작성일 : 2014-04-28
    /// 작성자 : miksystem.com
    /// </summary>
    public class GlossaryCategoryDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static GlossaryCategoryDac _instance = null;
        public static GlossaryCategoryDac Instance
        {
            get
            {
                GlossaryCategoryDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossaryCategoryDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        private GlossaryCategoryDac() { }

        /// <summary>
        /// Inserts a record into the tb_GlossaryCategory table.
        /// </summary>
        /// <returns></returns>
        public int GlossaryCategoryInsert(GlossaryCategoryType glossaryCategoryType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Insert");

            db.AddInParameter(dbCommand, "CategoryTitle", DbType.String, glossaryCategoryType.CategoryTitle);
            db.AddInParameter(dbCommand, "CategoryContents", DbType.String, glossaryCategoryType.CategoryContents);
            db.AddInParameter(dbCommand, "NOTES", DbType.String, glossaryCategoryType.NOTES);
            db.AddInParameter(dbCommand, "UserID", DbType.String, glossaryCategoryType.UserID);
            db.AddInParameter(dbCommand, "UserName", DbType.String, glossaryCategoryType.UserName);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, glossaryCategoryType.DeptName);
            db.AddInParameter(dbCommand, "UserEmail", DbType.String, glossaryCategoryType.UserEmail);
            db.AddInParameter(dbCommand, "UseYN", DbType.String, glossaryCategoryType.UseYN);
            db.AddInParameter(dbCommand, "CreatedDT", DbType.DateTime, glossaryCategoryType.CreatedDT);
            db.AddInParameter(dbCommand, "UpdatedDT", DbType.DateTime, glossaryCategoryType.UpdatedDT);
            // 2014-06-23 Mr.No
            if (glossaryCategoryType.HIERACHY_CODE != null || glossaryCategoryType.HIERACHY_CODE != "")
            {
                db.AddInParameter(dbCommand, "HIERACHY_CODE", DbType.String, glossaryCategoryType.HIERACHY_CODE);
            }
            // Execute the query and return the new identity value
            int returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            return returnValue;
        }

        /// <summary>
        /// Selects a single record from the tb_GlossaryCategory table.
        /// </summary>
        /// <returns>DataSet</returns>
        public GlossaryCategoryType GlossaryCategorySelect(Int64 iD)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Select");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, iD);

            GlossaryCategoryType glossaryCategoryType = null;
            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    glossaryCategoryType = GetGlossaryCategoryTypeMapData(ds.Tables[0].Rows[0]);
                }
            }
            return glossaryCategoryType;
        }

        /// <summary>
        /// Selects all records from the tb_GlossaryCategory table.
        /// </summary>
        public List<GlossaryCategoryType> GlossaryCategorySelectAll()
        {
            List<GlossaryCategoryType> listGlossaryCategoryType = new List<GlossaryCategoryType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_List");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryCategoryType glossaryCategoryType = GetGlossaryCategoryTypeMapData(dr);
                        listGlossaryCategoryType.Add(glossaryCategoryType);
                    }
                }
            }
            return listGlossaryCategoryType;
        }

        /// <summary>
        /// Selects all records from the tb_GlossaryCategory table - Admin.
        /// </summary>
        public List<GlossaryCategoryType> GlossaryCategorySelectAllAdmin()
        {
            List<GlossaryCategoryType> listGlossaryCategoryType = new List<GlossaryCategoryType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_List_Admin");

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryCategoryType glossaryCategoryType = GetGlossaryCategoryTypeMapData(dr);
                        listGlossaryCategoryType.Add(glossaryCategoryType);
                    }
                }
            }
            return listGlossaryCategoryType;
        }

        /// <summary>
        /// Updates a record in the tb_GlossaryCategory table.
        /// </summary>
        public void GlossaryCategoryUpdate(GlossaryCategoryType glossaryCategoryType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Update");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, glossaryCategoryType.ID);
            db.AddInParameter(dbCommand, "CategoryTitle", DbType.String, glossaryCategoryType.CategoryTitle);
            db.AddInParameter(dbCommand, "CategoryContents", DbType.String, glossaryCategoryType.CategoryContents);
            //db.AddInParameter(dbCommand, "NOTES", DbType.String, glossaryCategoryType.NOTES);
            //db.AddInParameter(dbCommand, "UserID", DbType.String, glossaryCategoryType.UserID);
            //db.AddInParameter(dbCommand, "UserName", DbType.String, glossaryCategoryType.UserName);
            //db.AddInParameter(dbCommand, "DeptName", DbType.String, glossaryCategoryType.DeptName);
            //db.AddInParameter(dbCommand, "UserEmail", DbType.String, glossaryCategoryType.UserEmail);
            //db.AddInParameter(dbCommand, "UseYN", DbType.String, glossaryCategoryType.UseYN);
            //db.AddInParameter(dbCommand, "CreatedDT", DbType.DateTime, glossaryCategoryType.CreatedDT);
            db.AddInParameter(dbCommand, "UpdatedDT", DbType.DateTime, glossaryCategoryType.UpdatedDT);

            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Updates a record in the tb_GlossaryCategory table.
        /// </summary>
        public void GlossaryCategoryUpdateAdmin(string mode, GlossaryCategoryType glossaryCategoryType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Update_Admin");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, glossaryCategoryType.ID);
            db.AddInParameter(dbCommand, "MODE", DbType.String, mode);
            db.AddInParameter(dbCommand, "UseYN", DbType.String, glossaryCategoryType.UseYN);            

            db.AddInParameter(dbCommand, "UserID", DbType.String, glossaryCategoryType.UserID);
            db.AddInParameter(dbCommand, "UserName", DbType.String, glossaryCategoryType.UserName);
            db.AddInParameter(dbCommand, "DeptName", DbType.String, glossaryCategoryType.DeptName);
            db.AddInParameter(dbCommand, "UserEmail", DbType.String, glossaryCategoryType.UserEmail);

            if (glossaryCategoryType.CategoryTitle != null)
            {
                db.AddInParameter(dbCommand, "CategoryTitle", DbType.String, glossaryCategoryType.CategoryTitle);
            }
            if (glossaryCategoryType.CategoryContents != null)
            {
                db.AddInParameter(dbCommand, "CategoryContents", DbType.String, glossaryCategoryType.CategoryContents);
            }
            if (glossaryCategoryType.NOTES != null)
            {
                db.AddInParameter(dbCommand, "CategoryNotes", DbType.String, glossaryCategoryType.NOTES);
            }
            // 2014-06-23 Mr.No
            if (glossaryCategoryType.HIERACHY_CODE != null || glossaryCategoryType.HIERACHY_CODE != "")
            {
                db.AddInParameter(dbCommand, "HIERACHY_CODE", DbType.String, glossaryCategoryType.HIERACHY_CODE);
            }
            db.ExecuteNonQuery(dbCommand);
        }

        public void GlossaryCategoryMoveAdmin(int id, int categoryId)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Move_Admin");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            
            db.ExecuteNonQuery(dbCommand);
        }

        /// Updates a record in the tb_GlossaryCategory table.
        /// </summary>
        public void GlossaryCategoryDeleteAdmin(GlossaryCategoryType GlossaryCategoryType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Delete_Admin");

            db.AddInParameter(dbCommand, "ID", DbType.Int64, GlossaryCategoryType.ID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Creates a new instance of the GlossaryCategoryType class and populates it with data from the specified DataRow.
        /// </summary>
        private GlossaryCategoryType GetGlossaryCategoryTypeMapData(DataRow dr)
        {
            GlossaryCategoryType glossaryCategoryType = new GlossaryCategoryType();

            // 140519 ljm. 읽기가 안됨...RowNum이 없으면 넘어가는 조건 추가
            if (dr.Table.Columns.Contains("RowNum"))
            {
                glossaryCategoryType.RowNum = (dr["RowNum"] == DBNull.Value) ? 0 : dr.Field<long>("RowNum");
            }
            glossaryCategoryType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
            glossaryCategoryType.CategoryTitle = (dr["CategoryTitle"] == DBNull.Value) ? string.Empty : dr.Field<string>("CategoryTitle");
            glossaryCategoryType.CategoryContents = (dr["CategoryContents"] == DBNull.Value) ? string.Empty : dr.Field<string>("CategoryContents");
            if (glossaryCategoryType.CategoryContents != null)
            {
                SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(glossaryCategoryType.CategoryContents.Replace("\r\n", string.Empty)));
            }
            glossaryCategoryType.NOTES = (dr["NOTES"] == DBNull.Value) ? null : dr.Field<string>("NOTES");
            glossaryCategoryType.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
            glossaryCategoryType.UserName = (dr["UserName"] == DBNull.Value) ? null : dr.Field<string>("UserName");
            glossaryCategoryType.DeptName = (dr["DeptName"] == DBNull.Value) ? null : dr.Field<string>("DeptName");
            glossaryCategoryType.UserEmail = (dr["UserEmail"] == DBNull.Value) ? null : dr.Field<string>("UserEmail");
            glossaryCategoryType.UseYN = (dr["UseYN"] == DBNull.Value) ? String.Empty : dr.Field<string>("UseYN");
            glossaryCategoryType.CreatedDT = (dr["CreatedDT"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreatedDT");
            glossaryCategoryType.UpdatedDT = (dr["UpdatedDT"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("UpdatedDT");
            if (dr.Table.Columns.Contains("HIERACHY_CODE"))
            {
                glossaryCategoryType.HIERACHY_CODE = (dr["HIERACHY_CODE"] == DBNull.Value) ? String.Empty : dr.Field<string>("HIERACHY_CODE");
            }
            return glossaryCategoryType;
        }


        /// <summary>
        /// 메인 화면에서 사용자에 따른 카테고리와 해당 부문의 ID, CategoryName 을 리턴함
        /// </summary>
        /// <param name="USER_ID"></param>
        /// <returns></returns>
        public List<GlossaryCategoryType> GlossaryCategory_Main_User_List(string USER_ID)
        {
            List<GlossaryCategoryType> listGlossaryCategoryType = new List<GlossaryCategoryType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Main_User_List");

            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryCategoryType glossaryCategoryType = new GlossaryCategoryType();
                        glossaryCategoryType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
                        glossaryCategoryType.CategoryTitle = (dr["CategoryTitle"] == DBNull.Value) ? null : dr.Field<string>("CategoryTitle");
                        glossaryCategoryType.NOTES = (dr["NOTES"] == DBNull.Value) ? null : dr.Field<string>("NOTES");
                        listGlossaryCategoryType.Add(glossaryCategoryType);
                    }
                }
            }
            return listGlossaryCategoryType;
        }

        /// <summary>
        /// 메인 화면에서 뿌려줄 사용자에 따른 카테고리와 해당 부문의 최신 글 4개를 가져옴
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<GlossaryType> GlossaryCategory_Main_Category_List(int ID)
        {
            List<GlossaryType> listGlossaryType = new List<GlossaryType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Main_Category_List");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryType glossaryType = new GlossaryType();
                        glossaryType.ID = (dr["ID"] == DBNull.Value) ? string.Empty : dr.Field<long>("ID").ToString();
                        glossaryType.Title = (dr["Title"] == DBNull.Value) ? null : dr.Field<string>("Title");
                        glossaryType.CommonID = (dr["CommonID"] == DBNull.Value) ? string.Empty : dr.Field<int>("CommonID").ToString();
                        listGlossaryType.Add(glossaryType);
                    }
                }
            }
            return listGlossaryType;
        }


        public int GlossaryCategory_Check(int CategoryID)
        {
            int returnCount = 0;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryCategory_Check");

            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        returnCount = (dr["Count"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["Count"]);
                    }
                }
            }
            return returnCount;
        }


        public string CategoryTitle_Check(string CategoryTitle)
        {
            string returnCategoryTitle = string.Empty;
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_CategoryTitle_Check");

            db.AddInParameter(cmd, "CategoryTitle", DbType.String, CategoryTitle);
            
            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        returnCategoryTitle = (dr["CategoryTitle"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["CategoryTitle"]);
                    }
                }
            }

            return returnCategoryTitle;
        }


    }
}
