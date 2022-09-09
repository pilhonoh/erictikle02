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
    public class GlossaryDirectoryDac
    {


        private const string connectionStringName = "ConnGlossary";

        private static GlossaryDirectoryDac _instance = null;
        public static GlossaryDirectoryDac Instance
        {
            get
            {
                GlossaryDirectoryDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossaryDirectoryDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        public GlossaryDirectoryDac() { }


        /// <summary>
        /// Selects a single record from the tb_GlossaryCategory table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<GlossaryDirectoryType> GlossaryDirSelect(string USER_ID)
        {
            List<GlossaryDirectoryType> listGlossaryDirType = new List<GlossaryDirectoryType>();


            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryDirectory_Select");

            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryDirectoryType glossaryDirType = new GlossaryDirectoryType();

                        glossaryDirType.DirID = (dr["DIR_ID"] == DBNull.Value) ? null : Convert.ToString(dr["DIR_ID"]); //dr.Field<string>("DIR_ID");
                        glossaryDirType.DirNM = (dr["DIR_NM"] == DBNull.Value) ? null : dr.Field<string>("DIR_NM");
                        glossaryDirType.Path = (dr["PATH"] == DBNull.Value) ? null : dr.Field<string>("PATH");
                        glossaryDirType.SupDirID = (dr["SUP_DIR_ID"] == DBNull.Value) ? null : Convert.ToString(dr["SUP_DIR_ID"]);  //dr.Field<string>("SUP_DIR_ID");
                        glossaryDirType.RegID = (dr["REG_ID"] == DBNull.Value) ? null : dr.Field<string>("REG_ID");
                        glossaryDirType.RegNM = (dr["REG_NM"] == DBNull.Value) ? null : dr.Field<string>("REG_NM");
                        glossaryDirType.RegDTM = (dr["REG_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("REG_DTM");
                        glossaryDirType.AuditID = (dr["AUDIT_ID"] == DBNull.Value) ? null : dr.Field<string>("AUDIT_ID");
                        glossaryDirType.AuditDTM = (dr["AUDIT_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("AUDIT_DTM");

                        listGlossaryDirType.Add(glossaryDirType);
                    }

                }
            }
            return listGlossaryDirType;
        }


        /// <summary>
        /// 디렉토리 리스트 조회(그리드 바인딩) 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryDir_List(string Mode, int PageNum, int PageSize, string UserID, string GatheringYN, string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirectory_Select");

            //테스트
            //DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirectory_Select3");

            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        public List<GlossaryDirectoryFileType> GlossaryDirFile_List(string ID)
        {
            List<GlossaryDirectoryFileType> listGlossaryDirFileType = new List<GlossaryDirectoryFileType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryDirFile_Select");

            db.AddInParameter(dbCommand, "DirID", DbType.String, ID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        GlossaryDirectoryFileType glossaryDirFileType = new GlossaryDirectoryFileType();

                        glossaryDirFileType.DirID = (dr["DIR_ID"] == DBNull.Value) ? null : Convert.ToString(dr["DIR_ID"]);  //dr.Field<string>("DIR_ID");
                        glossaryDirFileType.FileID = (dr["FILE_ID"] == DBNull.Value) ? null : Convert.ToString(dr["FILE_ID"]);  //dr.Field<string>("DIR_NM");
                        glossaryDirFileType.FileNM = (dr["FILE_NM"] == DBNull.Value) ? null : dr.Field<string>("FILE_NM");
                        glossaryDirFileType.FileExt = (dr["FILE_EXT"] == DBNull.Value) ? null : dr.Field<string>("FILE_EXT");
                        glossaryDirFileType.RegID = (dr["REG_ID"] == DBNull.Value) ? null : dr.Field<string>("REG_ID");
                        glossaryDirFileType.RegNM = (dr["REG_NM"] == DBNull.Value) ? null : dr.Field<string>("REG_NM");
                        glossaryDirFileType.RegDTM = (dr["REG_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("REG_DTM");
                        glossaryDirFileType.AuditID = (dr["AUDIT_ID"] == DBNull.Value) ? null : dr.Field<string>("AUDIT_ID");
                        glossaryDirFileType.AuditDTM = (dr["AUDIT_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("AUDIT_DTM");


                        listGlossaryDirFileType.Add(glossaryDirFileType);
                    }
                }
            }
            return listGlossaryDirFileType;
        }



        /*public DataSet GlossaryDirFile_List(string Mode, int PageNum, int PageSize, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirFile_Select");
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        } */


        public DataSet DirectoryInsert(GlossaryDirectoryType dirType, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirectory_Insert");

            //등록 모드
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "DirID", DbType.String, dirType.DirID);
            db.AddInParameter(cmd, "SupDirID", DbType.String, dirType.SupDirID);
            db.AddInParameter(cmd, "DirNM", DbType.String, dirType.DirNM);

            db.AddInParameter(cmd, "Path", DbType.String, dirType.Path);

            //등록자 
            db.AddInParameter(cmd, "UserID", DbType.String, dirType.UserID);
            db.AddInParameter(cmd, "RegID", DbType.String, dirType.RegID);
            db.AddInParameter(cmd, "RegNM", DbType.String, dirType.RegNM);

            return db.ExecuteDataSet(cmd);
        }

        //2016-11-03 공동관리자 추가
        public int DirectoryManagerInsert(DirectoryMgrType mgrType,string tkType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryManagerAuth_Insert");

            db.AddInParameter(cmd, "CommonID", DbType.String, mgrType.CommonID);
            db.AddInParameter(cmd, "ManagerID", DbType.String, mgrType.ManagerID);
            db.AddInParameter(cmd, "ManagerName", DbType.String, mgrType.ManagerName);
            db.AddInParameter(cmd, "AUTH_ID", DbType.String, mgrType.AUTH_ID);
            db.AddInParameter(cmd, "AUTH_NM", DbType.String, mgrType.AUTH_NM);
            db.AddInParameter(cmd, "@TYPE", DbType.String, tkType);

            return db.ExecuteNonQuery(cmd);
        }

        //2016-11-03 공동관리자 모두 제거
        public int DirectoryManagerDelete(string dirID, string tktype)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryManagerAuth_Delete");

            db.AddInParameter(cmd, "CommonID", DbType.String, dirID);
            db.AddInParameter(cmd, "Type", DbType.String, tktype);

            return db.ExecuteNonQuery(cmd);
        }

        

        public DataSet DirectoryDelete(GlossaryDirectoryType dirType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirectory_Delete");

            //등록 모드
            db.AddInParameter(cmd, "DirID", DbType.String, dirType.DirID);
            db.AddInParameter(cmd, "UserID", DbType.String, dirType.UserID);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet DirectoryFileInsert(GlossaryDirectoryFileType dirFileType, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryDirFile_Insert");

            //등록 모드
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "DirID", DbType.String, dirFileType.DirID);
            db.AddInParameter(cmd, "FileID", DbType.String, dirFileType.FileID);
            db.AddInParameter(cmd, "FileNM", DbType.String, dirFileType.FileNM);
            db.AddInParameter(cmd, "FileSize", DbType.String, dirFileType.FileSize);
            db.AddInParameter(cmd, "FileExt", DbType.String, dirFileType.FileExt);

            //등록자 
            db.AddInParameter(cmd, "RegID", DbType.String, dirFileType.RegID);
            db.AddInParameter(cmd, "RegNM", DbType.String, dirFileType.RegNM);


            return db.ExecuteDataSet(cmd);

        }


        /// <summary>
        /// (콤보상자 바인딩)  
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string userID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyGroup_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, userID);


            return db.ExecuteDataSet(cmd);

        }


        /// <summary>
        /// 디렉토리 조회
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string mode, string userID, string GatheringYN, string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_ddlDirectory_Select2");
            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "UserID", DbType.String, userID);
            db.AddInParameter(cmd, "GatheringYN", DbType.String, GatheringYN);
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            return db.ExecuteDataSet(cmd);

        }

        /// <summary>
        /// 디렉토리 조회  
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <param name="divID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string mode, string userID, string divID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Directory_Select");

            //등록 모드
            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "DivID", DbType.String, divID);
            db.AddInParameter(cmd, "UserID", DbType.String, userID);


            return db.ExecuteDataSet(cmd);

        }

        public DataSet GetDirectoryAuth(string DirID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryAuth_Select");

            //등록 모드
            db.AddInParameter(cmd, "DirID", DbType.String, DirID);

            return db.ExecuteDataSet(cmd);
        }
        public DataSet GetGatheringAuth(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GatheringAuth_Select");

            //등록 모드
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);

            return db.ExecuteDataSet(cmd);
        }

        
    }

}
