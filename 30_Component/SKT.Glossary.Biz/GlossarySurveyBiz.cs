using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

namespace SKT.Glossary.Biz
{
    public class GlossarySurveyBiz
    {

        public List<GlossarySurveyType> GlossarySurvey_List(string USER_ID)
        {
            List<GlossarySurveyType> listGlossarySurveyType = GlossarySurveyDac.Instance.GlossarySurveySelect(USER_ID);
            return listGlossarySurveyType;
        }

        /// <summary>
        /// 투표자 조회  
        /// </summary>
        /// <param name="USER_ID"></param>
        /// <returns></returns>
        public ArrayList GlossarySurvey_VoteList(string SV_ID, string USER_ID)
        {

            ArrayList list = new ArrayList();
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            DataSet ds = Dac.GlossarySurvey_VoteList(SV_ID, USER_ID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossarySurveyCommentType SurveyCommentType = new GlossarySurveyCommentType();

                    SurveyCommentType.CommentID = (dr["CommentID"] == DBNull.Value) ? null : dr.Field<string>("CommentID");
                    SurveyCommentType.QstID = (dr["QST_ID"] == DBNull.Value) ? null : dr.Field<string>("QST_ID");
                    SurveyCommentType.Contents = (dr["Contents"] == DBNull.Value) ? null : dr.Field<string>("Contents");
                    SurveyCommentType.LikeCount = (dr["LikeCount"] == DBNull.Value) ? null : dr.Field<string>("LikeCount");
                    SurveyCommentType.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
                    SurveyCommentType.UserName = (dr["UserName"] == DBNull.Value) ? null : dr.Field<string>("UserName");

                    list.Add(SurveyCommentType);
                }
            }

            return list;

        }

        /// <summary>
        /// 총건/총인원/참여인원  조회  
        /// </summary>
        /// <param name="USER_ID"></param>
        /// <returns></returns>
        public ArrayList GlossarySurvey_CommentList(string QstID, string USER_ID)
        {

            ArrayList list = new ArrayList();
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            DataSet ds = Dac.GlossarySurvey_CommentList(QstID, USER_ID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossarySurveyCommentType SurveyCommentType = new GlossarySurveyCommentType();

                    SurveyCommentType.TotalCnt = (dr["TotalCnt"] == DBNull.Value) ? null : dr.Field<string>("TotalCnt");
                    SurveyCommentType.TotalVot = (dr["TotalVot"] == DBNull.Value) ? null : dr.Field<string>("TotalVot");
                    SurveyCommentType.DlyCnt = (dr["DlyCnt"] == DBNull.Value) ? null : dr.Field<string>("DlyCnt");
                    

                    list.Add(SurveyCommentType);
                }
            }

            return list;

        }

        /*
         신규 DataSet으로 데이터 가져오기
         */
        public DataSet GlossarySurveyListNew(string mode, string userID, int pageNum, int pageSize)
        {
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            return Dac.GlossarySurvey_List(mode, pageNum, pageSize, userID);;
        }





       

        /// <summary>
        /// 의견 저장  
        /// </summary>
        /// <param name="Survey"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public GlossarySurveyType SurveyInsert(GlossarySurveyType Survey, string Mode)
        {
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            DataSet ds = Dac.SurveyInsert(Survey, Mode);


            if (Mode.Equals("Insert"))
            {
                Survey.SvID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                if (!string.IsNullOrEmpty(Survey.SvID))
                    Dac.SurveyQstInsert(ds.Tables[0].Rows[0].ItemArray[0].ToString(), Survey.UserID);
            }

            return Survey;
        }





        /// <summary>
        /// 뷰(조회) 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public GlossarySurveyType GlossarySurveySelect(string ID, int Count = 0)
        {
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            //DataSet ds = Dac.GlossaryQnASelect(ID, Count);

            DataSet ds = Dac.GlossarySurveySelect(ID, Count);
            GlossarySurveyType Board = new GlossarySurveyType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //Board.ID = dr["ID"].ToString();
                    Board.SvID = dr["SV_ID"].ToString();
                    Board.SvNM = dr["SV_NM"].ToString();
                    Board.SvSummary = dr["SV_SUMMARY"].ToString();
                    Board.StaDT = dr["Sta_DT"].ToString();
                    Board.UserID = dr["Reg_ID"].ToString();
                    Board.RegNM = dr["Reg_NM"].ToString();
                    Board.EndDT = dr["END_DT"].ToString();
                    Board.Status = dr["STATUS"].ToString();
                    Board.QstId = dr["QST_ID"].ToString();
                    Board.RegDTM = Convert.ToDateTime(dr["REG_DTM"]);
                    
                    Board.AUTHUSERCNT = dr["AUTHUSERCNT"].ToString();
                    Board.DISPLAYNAME = dr["DISPLAYNAME"].ToString();
                    Board.CommentCnt = dr["CommentCnt"].ToString();
                    Board.CommentUseCnt = dr["CommentUseCnt"].ToString();
                    Board.VoteType = dr["VoteType"].ToString();
                       
                }
            }
            return Board;
        }




        /// <summary>
        /// 조회수증가
        /// </summary>
        /// <param name="ID"></param>
        public void SurveyHit(int ID)
        {
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            Dac.SurveyHit(ID);
        }

		/////////////////////////////////////////////////////////////////////////////////////
		// 권한 / 대상자 처리

		// 대상자삭제
		public void GlossarySurveyAuthDelete(string ID, string Type)
		{
			GlossarySurveyDac Dac = new GlossarySurveyDac();
			DataSet ds = Dac.GlossarySurveyAuthDelete(ID, Type);
		}


		/// <summary>
		/// 대상자 등록
		/// </summary>
		/// <param name="ItemID"></param>
		/// <param name="UserID"></param>
		/// <param name="ToUserID"></param>
		/// <param name="Title"></param>
		public void GlossarySurveyAuthInsert(string ItemID, string UserID, string ToUserID, string AuthCL, string Mode)
		{
			string[] ToUser = ToUserID.Split('/');
			string[] ToUserType = AuthCL.Split('/');

			CommonAuthType Board = new CommonAuthType();
			GlossarySurveyDac Dac = new GlossarySurveyDac();

			Board.ItemID = ItemID;
			Board.AuthID = UserID;
			Board.AuditID = UserID;   //등록자  
			Board.AuthType = "U";

			//등록자 
			Dac.GlossarySurveyAuthInsert(Board, Mode);


			for (int i = 0; i < ToUser.Length - 1; i++)
			{

				//**************************************************// 
				//기존의 처리는  View_User 테이블에서 사용자를 조회하는데 
				//조직도와 매핑이 되지 않아 조직도를 조회하는 테이블로 변경
				//2014-09-02
				//사용자/조직/그룹 여부는 클라이언트에서 넘겨준 값을 받아 처리한다.
				//**************************************************// 

				#region
				//GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
				//ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);
				//if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
				//{
				//    Board.AuthType = "U";
				//    Board.AuthID = ToUser[i];
				//    Dac.GlossarySurveyAuthInsert(Board, Mode);
				//}
				//else
				//{
				//    Board.AuthType = "O";
				//    Board.AuthID = ToUser[i];
				//    Dac.GlossarySurveyAuthInsert(Board, Mode);
				//}
				#endregion


				Board.AuthID = ToUser[i];
				Board.AuthType = ToUserType[i];

				Dac.GlossarySurveyAuthInsert(Board, Mode);
			}
		}



        public string sendNateOn(string mail, string sendNote) {





            return "";
        }


        public string GlossarySurveyDelete(string idx, string UserID)
        {
            string rtn = string.Empty; ;
            GlossarySurveyDac Dac = new GlossarySurveyDac();
            DataSet ds = Dac.GlossarySurveyDelete(idx, UserID);


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rtn = dr["DBFLAG"].ToString();
                }
            }
            return rtn;
        }
       


    }

   


}
