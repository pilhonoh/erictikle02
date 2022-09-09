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
    public class GlossaryMyGroupDac
    {

        private const string connectionStringName = "ConnGlossary";


        //조회  
        public DataSet GlossaryMyGroupSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyGroup_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }


        public List<GlossaryGroupAuthType> MyGroupListSelect(string iD, string GrpID, string type)
        {
            List<GlossaryGroupAuthType> listGroupType = new List<GlossaryGroupAuthType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);


            //확인// 
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryMyGroupList_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, iD);
            db.AddInParameter(dbCommand, "MyGrpID", DbType.String, GrpID);
            db.AddInParameter(dbCommand, "Type", DbType.String, type);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryGroupAuthType listType = new GlossaryGroupAuthType();
                        listType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
                        listType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");
                        listGroupType.Add(listType);
                    }
                }
            }
            return listGroupType;
        }


        public DataSet MyGroupListSelect2(string iD, string GrpID, string type, string GatheringID = "0")
        {

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryMyGroupList_Select");

            db.AddInParameter(dbCommand, "UserID", DbType.String, iD);
            db.AddInParameter(dbCommand, "MyGrpID", DbType.String, GrpID);
            db.AddInParameter(dbCommand, "Type", DbType.String, type);
            db.AddInParameter(dbCommand, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));


            return db.ExecuteDataSet(dbCommand);


        }

        public DataSet DirectoryAllUserList(string GrpID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_DirectoryAuth_AllList");

            db.AddInParameter(dbCommand, "MyGrpID", DbType.String, GrpID);

            return db.ExecuteDataSet(dbCommand);
        }

        //2016-11-03 공동 관리자 불러오기
        public DataSet MyGroupListManagerSelect(string commonID,string tkType)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GatheringDirectoryAuth_Check");

            db.AddInParameter(dbCommand, "CommonID", DbType.String, commonID);
            db.AddInParameter(dbCommand, "TikleType", DbType.String, tkType);

            return db.ExecuteDataSet(dbCommand);
        }

        //2016-11-03 공동 관리자 불러오기
        public DataSet GetDirectoryManagerCheck(string dirID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_DirectoryManagerAuth_Check");

            db.AddInParameter(dbCommand, "DirID", DbType.String, dirID);

            return db.ExecuteDataSet(dbCommand);
        }

        public DataSet CommAuthCount(string iD, string type)
        {

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("UP_COMMAUTHCOUNT");

            db.AddInParameter(dbCommand, "IDX", DbType.String, iD);
            db.AddInParameter(dbCommand, "Type", DbType.String, type);


            return db.ExecuteDataSet(dbCommand);


        }



        //저장
        public DataSet GlossaryMyGroupInsert(GlossaryGroupListType Board, string mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyGroup_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "MyGrpID", DbType.String, Board.MyGrpID);
            db.AddInParameter(cmd, "MyGrpNM", DbType.String, Board.MyGrpNM);
            db.AddInParameter(cmd, "RegID", DbType.String, Board.RegID);
            db.AddInParameter(cmd, "RegNM", DbType.String, Board.RegNM);
            db.AddInParameter(cmd, "AudidID", DbType.String, Board.AudidID);


            return db.ExecuteDataSet(cmd);
        }


        //저장
        public void GlossaryMyGroupListInsert(GlossaryGroupListType Board, string mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyGroupList_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "ListNO", DbType.String, Board.ListNO);
            db.AddInParameter(cmd, "MyGrpID", DbType.String, Board.MyGrpID);
            db.AddInParameter(cmd, "AuthType", DbType.String, Board.AuthType);
            db.AddInParameter(cmd, "AuthID", DbType.String, Board.AuthID);
            //db.AddInParameter(cmd, "PRT_SEQ", DbType.String, Board.PrtSEQ);
            db.AddInParameter(cmd, "AudidID", DbType.String, Board.AudidID);


            db.ExecuteDataSet(cmd);
        }

        //댓글 삭제
        public DataSet GlossaryMyGroupDelete(string ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryMyGroup_Delete");
            db.AddInParameter(cmd, "GrpID", DbType.String, ID);
            return db.ExecuteDataSet(cmd);
        }

        //그룹변경 - 문서공유 연동  
        public DataSet GlossaryMyGroupChangeSelect(string mode, string GrpID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryMyGroupChange_Select");
            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "GrpID", DbType.String, GrpID);


            return db.ExecuteDataSet(cmd);
        }


        //그룹유저변경 - 문서공유 연동  
        public DataSet GlossaryMyGroupUserChangeSelect(string mode, string DirID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_DirectoryMyGroupUserChange_Select");

            db.AddInParameter(cmd, "Mode", DbType.String, mode);
            db.AddInParameter(cmd, "DirID", DbType.String, DirID);


            return db.ExecuteDataSet(cmd);
        }



    }



}
