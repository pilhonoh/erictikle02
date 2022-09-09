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
    public class GlossaryDirectoryAuthDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static GlossaryDirectoryAuthDac _instance = null;
        public static GlossaryDirectoryAuthDac Instance
        {
            get
            {
                GlossaryDirectoryAuthDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossaryDirectoryAuthDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        public GlossaryDirectoryAuthDac() { }


        //공유 추가
		public DataSet GlossaryDirectoryAuthInsert(CommonAuthType Board, string mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryAuth_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "SeqNO", DbType.String, Board.SeqNO);
			db.AddInParameter(cmd, "DirID", DbType.String, Board.ItemID);
            db.AddInParameter(cmd, "AuthType", DbType.String, Board.AuthType);
            db.AddInParameter(cmd, "AuthID", DbType.String, Board.AuthID);
			db.AddInParameter(cmd, "AuthCL", DbType.String, Board.AuthRWX);
            db.AddInParameter(cmd, "AuditID", DbType.String, Board.AuditID);
            db.AddInParameter(cmd, "AuditDTM", DbType.String, Board.AuditDTM);

            return db.ExecuteDataSet(cmd);
        }

        //공유 삭제
        public DataSet GlossaryDirectoryAuthDelete(string ID, string Type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryAuth_Delete");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "Type", DbType.String, Type);
            return db.ExecuteDataSet(cmd);
        }


		public List<CommonAuthType> GlossaryDirectoryAuthSelect(string ID)
        {
			List<CommonAuthType> listGlossaryAuthType = new List<CommonAuthType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryAuth_Select");

            db.AddInParameter(cmd, "DirID", DbType.String, ID);

            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

						CommonAuthType GlossaryAuthType = new CommonAuthType();

                        GlossaryAuthType.SeqNO = (dr["SEQ_NO"] == DBNull.Value) ? null : Convert.ToString(dr["SEQ_NO"]);   
                        GlossaryAuthType.ItemID = (dr["DIR_ID"] == DBNull.Value) ? null : Convert.ToString(dr["DIR_ID"]);   
                        GlossaryAuthType.AuthType = (dr["AUTH_TYPE"] == DBNull.Value) ? null : dr.Field<string>("AUTH_TYPE");
                        GlossaryAuthType.AuthID = (dr["AUTH_ID"] == DBNull.Value) ? null : dr.Field<string>("AUTH_ID");
						GlossaryAuthType.AuthRWX = (dr["AUTH_CL"] == DBNull.Value) ? null : dr.Field<string>("AUTH_CL");
                        GlossaryAuthType.AuditID = (dr["AUDIT_ID"] == DBNull.Value) ? null : dr.Field<string>("AUDIT_ID");
                        

                        GlossaryAuthType.TeamName = (dr["DNAME"] == DBNull.Value) ? null : dr.Field<string>("DNAME");
                        GlossaryAuthType.DeptName = (dr["SNAME"] == DBNull.Value) ? null : dr.Field<string>("SNAME");

                        GlossaryAuthType.RegID = (dr["REG_ID"] == DBNull.Value) ? null : dr.Field<string>("REG_ID");

                        GlossaryAuthType.RegDTM = (dr["REG_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("REG_DTM");

                        listGlossaryAuthType.Add(GlossaryAuthType);
                    }
                }
            }
            return listGlossaryAuthType;
        }

    



    }



}
