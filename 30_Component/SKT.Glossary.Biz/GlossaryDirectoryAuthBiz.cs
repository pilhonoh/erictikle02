using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryDirectoryAuthBiz
    {
        // 삭제
        public void GlossaryDirectoryAuthDelete(string ID, string Type)
        {
            GlossaryDirectoryAuthDac Dac = new GlossaryDirectoryAuthDac();
            DataSet ds = Dac.GlossaryDirectoryAuthDelete(ID, Type);
        }


        /// <summary>
        /// 최초 글 작성시 일부공개 일 경우 공유 추가되는 부분
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Title"></param>
		public void GlossaryDirectoryAuthInsert(string ItemID, string UserID, string ToUserID, string AuthCL, string AuthRWX, string Mode)
        {
			string[] ToUser = ToUserID.Split('/');
			string[] ToUserType = AuthCL.Split('/');

			CommonAuthType Board = new CommonAuthType();
            GlossaryDirectoryAuthDac Dac = new GlossaryDirectoryAuthDac();

			Board.ItemID = ItemID;

            #region 

            //대상구분(AUTH_TYPE)
            //U : 구성원
            //O : 조직
            //G : 그룹

            //권한구분(AUTH_CL)
            //R : 읽기
            //RW : 읽기쓰기

            #endregion 


			if (AuthRWX.Equals("RW"))
            {
                Board.AuthID = UserID;
                Board.AuditID = UserID;
                Board.AuthType = "U";
				Board.AuthRWX = "RW"; 

                //등록자 
                Dac.GlossaryDirectoryAuthInsert(Board, Mode); 
            }

            for (int i = 0; i < ToUser.Length-1; i++)
            {
                //**************************************************// 
                //기존의 처리는  View_User 테이블에서 사용자를 조회하는데 
                //조직도와 매핑이 되지 않아 조직도를 조회하는 테이블로 변경 
                //**************************************************// 

                //GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                //ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);
                //if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                //{
                //    Board.AuthType = "U";
                //    Board.AuthCL = AuthCL;
                //    Board.AuthID = ToUser[i];
                //    Dac.GlossaryDirectoryAuthInsert(Board, Mode);
                //}
                //else
                //{
                //    Board.AuthType = "G";
                //    Board.AuthCL = AuthCL;
                //    Board.AuthID = ToUser[i];
                //    Dac.GlossaryDirectoryAuthInsert(Board, Mode);
                //    //GlossaryDirectoryAuthInsert_Dept(ItemID, UserID, ToUser[i], Title, Mode);
                //}
				Board.AuthID = ToUser[i];
				Board.AuthType = ToUserType[i];
				Board.AuthRWX = AuthRWX;

				Dac.GlossaryDirectoryAuthInsert(Board, Mode);
            }
        }


		public List<CommonAuthType> GlossaryDirectoryAuthSelect(string ID)
        {
			List<CommonAuthType> listGlossaryAuthType = GlossaryDirectoryAuthDac.Instance.GlossaryDirectoryAuthSelect(ID);
            return listGlossaryAuthType;
        }

        /// <summary>
        /// 문서함 권한체크
        /// 2015-02-10 김성환
        /// </summary>
        public DataSet GlossaryDirectoryAuthSelect2(string DivID, string USERID)
        {
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_DirectoryAuth_Select2");

            db.AddInParameter(dbCommand, "DirID", DbType.String, DivID);
            db.AddInParameter(dbCommand, "USERID", DbType.String, USERID);

            return db.ExecuteDataSet(dbCommand);
        }

        /// <summary>
        /// 부서로 넘어온것은 재귀호출로 처리
		/// 현재는 사용안함
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Title"></param>
        /// <param name="Mode"></param>
		//public void GlossaryDirectoryAuthInsert_Dept(string ItemID, string UserID, string ToUserID, string Title, string Mode)
		//{
 
		//	string connectionStringName = "ConnGlossary";
		//	Database db = DatabaseFactory.CreateDatabase(connectionStringName);
		//	DbCommand dbCommand = db.GetStoredProcCommand("up_list_department_person");

		//	db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUserID.ToString());

		//	using (DataSet ds = db.ExecuteDataSet(dbCommand))
		//	{
		//		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//		{
		//			foreach (DataRow dr in ds.Tables[1].Rows)
		//			{
		//				string employeeID = (dr["employeeID"] == DBNull.Value) ? null : dr.Field<string>("employeeID");
		//				if (!String.IsNullOrEmpty(employeeID)) { GlossaryDirectoryAuthInsert(ItemID, UserID, employeeID, Title, Mode); }
		//			}
		//		}
		//	}
		//}


        /// <summary>
        /// 이메일,전화번호 정보 가져오기  
        /// </summary>
        /// <param name="ToUserType"></param>
        /// <param name="ToUserID"></param>
        /// <returns></returns>
        public ArrayList GlossaryInfo_Select(string ToUserType, string ToUserID)
        {
            ArrayList list = new ArrayList();

            DataSet ds = null; 
            string[] ToUser = ToUserID.Split('/');
            string[] ToType = ToUserType.Split('/');

            for (int i = 0; i < ToUser.Length - 1; i++)
            {

                if (ToType[i] =="U")
                { 
                    //개인
                    ds = GlossaryUserInfo_Select(ToUser[i]);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            GlossarySendType Board = new GlossarySendType();

                            Board.employeeID = dr["employeeID"].ToString();
                            Board.koreanName = dr["koreanName"].ToString();
                            Board.mail = dr["mail"].ToString();
                            Board.mobile = dr["mobile"].ToString();

                            list.Add(Board);
                        }
                    }


                }
                else
                {
                    //조직
                    GlossaryTeamInfo_Select(ToUser[i]);

                    ds = GlossaryTeamInfo_Select(ToUser[i]);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            GlossarySendType Board = new GlossarySendType();

                            Board.employeeID = dr["employeeID"].ToString();
                            Board.koreanName = dr["koreanName"].ToString();
                            Board.mail = dr["mail"].ToString();
                            Board.mobile = dr["mobile"].ToString();

                            list.Add(Board);
                        }
                    }
                } 
            }

            return list; 
          }


        //Eamil 쪽지 SMS 조회 
        public DataSet  GlossaryTeamInfo_Select(string DeptID)
        {

            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Directory_person1");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, DeptID);

            return db.ExecuteDataSet(dbCommand);

           
        }

        public DataSet GlossaryUserInfo_Select(string UserID)
        {

            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Directory_person2");

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(dbCommand);


        }


        //부서인지 조직인지 구분
        public string  DirectoryAuthTypeList(string ToUserID)
        {
            string AuthType = string.Empty;

            GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
            ImpersonUserinfo u = biz_.UserSelect(ToUserID);

            if (ToUserID != "" && !String.IsNullOrEmpty(u.UserID))
            {
                AuthType = "1";
            }
            else
            {
                AuthType = "2";
            }
   
            return AuthType;

        }





    }




}
