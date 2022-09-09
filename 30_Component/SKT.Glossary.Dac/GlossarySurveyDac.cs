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
    public class GlossarySurveyDac
    {

        private const string connectionStringName = "ConnGlossary";

        private static GlossarySurveyDac _instance = null;
        public static GlossarySurveyDac Instance
        {
            get
            {
                GlossarySurveyDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossarySurveyDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        public GlossarySurveyDac() { }

     
        /// <summary>
        /// Selects a single record from the tb_GlossaryCategory table.
        /// </summary>
        /// <returns>DataSet</returns>
        public List<GlossarySurveyType> GlossarySurveySelect(string USER_ID)
        {
            List<GlossarySurveyType> listGlossarySurveyType = new List<GlossarySurveyType>();


            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossarySurvey_Select");

            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossarySurveyType glossarySurveyType = new GlossarySurveyType();

                        glossarySurveyType.SvID = (dr["SV_ID"] == DBNull.Value) ? null : dr.Field<string>("SV_ID");
                        glossarySurveyType.SvNM = (dr["SV_NM"] == DBNull.Value) ? null : dr.Field<string>("SV_NM");
                        glossarySurveyType.SvSummary= (dr["SV_SUMMARY"] == DBNull.Value) ? null : dr.Field<string>("SV_SUMMARY");

                        //부서정보 더 필요

                        glossarySurveyType.StaDT = (dr["STA_DT"] == DBNull.Value) ? null : dr.Field<string>("STA_DT");
                        glossarySurveyType.EndDT = (dr["END_DT"] == DBNull.Value) ? null : dr.Field<string>("END_DT");
                        glossarySurveyType.Status = (dr["STATUS"] == DBNull.Value) ? null : dr.Field<string>("STATUS");
                        glossarySurveyType.TopImg = (dr["TOP_IMG"] == DBNull.Value) ? null : dr.Field<string>("TOP_IMG");
                        glossarySurveyType.RegID = (dr["REG_ID"] == DBNull.Value) ? null : dr.Field<string>("REG_ID");
                        glossarySurveyType.RegNM = (dr["REG_NM"] == DBNull.Value) ? null : dr.Field<string>("REG_NM");
                        glossarySurveyType.RegDTM = (dr["REG_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("REG_DTM");
                        glossarySurveyType.UseYN = (dr["USEYN"] == DBNull.Value) ? null : dr.Field<string>("USEYN");
                        glossarySurveyType.AuditID = (dr["AUDIT_ID"] == DBNull.Value) ? null : dr.Field<string>("AUDIT_ID");
                        glossarySurveyType.AuditDTM = (dr["AUDIT_DTM"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("AUDIT_DTM");

                        listGlossarySurveyType.Add(glossarySurveyType);
                    }

                }
            }
            return listGlossarySurveyType;     
        }


        public DataSet GlossarySurvey_List(string Mode, int PageNum, int PageSize, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySurvey_Select");
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }



        /// <summary>
        /// 의견 상세보기  
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataSet GlossarySurvey_View(string ItemID, string UserID, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySurvey_View");

            db.AddInParameter(cmd, "ID", DbType.String, ItemID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 의견 등록/수정 
        /// </summary>
        /// <param name="Survey"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataSet SurveyInsert(GlossarySurveyType Survey, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySurvey_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "SvID", DbType.String, Survey.SvID); 
            db.AddInParameter(cmd, "SvNM", DbType.String, Survey.SvNM);
            db.AddInParameter(cmd, "SvSummary", DbType.String, Survey.SvSummary);
            db.AddInParameter(cmd, "StaDT", DbType.String, Survey.StaDT);
            db.AddInParameter(cmd, "EndDT", DbType.String, Survey.EndDT);
            //등록자 
            db.AddInParameter(cmd, "UserID", DbType.String, Survey.UserID);
            db.AddInParameter(cmd, "RegID", DbType.String, Survey.RegID);
            db.AddInParameter(cmd, "RegNM", DbType.String, Survey.RegNM);


            return db.ExecuteDataSet(cmd);

        }


        /// <summary>
        /// 항목/문항 인서트  
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet SurveyQstInsert(string SurveyID, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySurveyQst_Insert");

            db.AddInParameter(cmd, "SvID", DbType.String, SurveyID);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd); 

        }



		////용어 보기
		//public DataSet GlossarySelect(string ItemID, string UserID, string Mode)
		//{
		//	Database db = DatabaseFactory.CreateDatabase(connectionStringName);
		//	DbCommand cmd = db.GetStoredProcCommand("up_Glossary_Select");
		//	db.AddInParameter(cmd, "ID", DbType.String, ItemID);
		//	db.AddInParameter(cmd, "Mode", DbType.String, Mode);
		//	db.AddInParameter(cmd, "UserID", DbType.String, UserID);
		//	return db.ExecuteDataSet(cmd);
		//}


        //뷰
        public DataSet GlossarySurveySelect(string ID, int count)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Survey_View_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ID);
            db.AddInParameter(cmd, "Count", DbType.Int32, count);
            return db.ExecuteDataSet(cmd);
        }


        //사용안함  
        public void SurveyHit(int ID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_SurveyHit");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, ID);

            db.ExecuteDataSet(dbCommand);
        }


        /// <summary>
        /// 투표자 조회  
        /// </summary>
        /// <param name="SV_ID"></param>
        /// <param name="USER_ID"></param>
        /// <returns></returns>
        public DataSet GlossarySurvey_VoteList(string SV_ID, string USER_ID)
        {

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_SurveyVote_Select");

            db.AddInParameter(dbCommand, "SV_ID", DbType.String, SV_ID);
            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            return db.ExecuteDataSet(dbCommand); 

        }

        /// <summary>
        /// 총인원 조회  
        /// </summary>
        /// <param name="SV_ID"></param>
        /// <param name="USER_ID"></param>
        /// <returns></returns>
        public DataSet GlossarySurvey_CommentList(string QstID, string USER_ID)
        {

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_SurveyTotal_Select");

            db.AddInParameter(dbCommand, "QstID", DbType.String, QstID);
            db.AddInParameter(dbCommand, "USER_ID", DbType.String, USER_ID);

            return db.ExecuteDataSet(dbCommand);

        }

		//대상자 추가
		public DataSet GlossarySurveyAuthInsert(CommonAuthType Board, string mode)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_SurveyAuth_Insert");

			db.AddInParameter(cmd, "Mode", DbType.String, mode);
			db.AddInParameter(cmd, "SeqNO", DbType.String, Board.SeqNO);
			db.AddInParameter(cmd, "SvID", DbType.String, Board.ItemID);
			db.AddInParameter(cmd, "AuthType", DbType.String, Board.AuthType);
			db.AddInParameter(cmd, "AuthID", DbType.String, Board.AuthID);

			db.AddInParameter(cmd, "AuditID", DbType.String, Board.AuditID);
			db.AddInParameter(cmd, "AuditDTM", DbType.String, Board.AuditDTM);

			return db.ExecuteDataSet(cmd);
		}

		//대상자 삭제
		public DataSet GlossarySurveyAuthDelete(string ID, string Type)
		{
			Database db = DatabaseFactory.CreateDatabase(connectionStringName);
			DbCommand cmd = db.GetStoredProcCommand("up_SurveyAuth_Delete");
			db.AddInParameter(cmd, "ID", DbType.String, ID);
			db.AddInParameter(cmd, "Type", DbType.String, Type);
			return db.ExecuteDataSet(cmd);
		}


        public DataSet GlossarySurveyDelete(string idx, string UserID)
        {

            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossarySurvey_Delete");
            db.AddInParameter(cmd, "SVID", DbType.String, idx);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }



    }
}
