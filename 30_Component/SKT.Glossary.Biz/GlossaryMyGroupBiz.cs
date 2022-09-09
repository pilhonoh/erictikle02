using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;


namespace SKT.Glossary.Biz
{
    public class GlossaryMyGroupBiz
    {


        //그룹 조회
        public ArrayList GlossaryMyGroupSelect(string UserID)
        {
            ArrayList list = new ArrayList();

            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = Dac.GlossaryMyGroupSelect(UserID);


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryGroupListType Board = new GlossaryGroupListType();

                    Board.MyGrpID = dr["MYGRP_ID"].ToString();
                    Board.MyGrpNM = dr["MYGRP_NM"].ToString();
                    //필요하면 추가 
                    //Board.ListNO = dr["LIST_NO"].ToString();
                    //Board.AuthID = dr["AUTH_ID"].ToString();
                    //Board.AuthType = dr["AUTH_TYPE"].ToString();
                    //Board.PrtSEQ = dr["PRT_SEQ"].ToString();
                    //Board.AudidID = dr["AUDID_ID"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        }


        //그룹 조회
        public List<GlossaryGroupAuthType> MyGroupListSelect(string UserID, string GrpID, string Type)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = new DataSet();
            List<GlossaryGroupAuthType> Board = Dac.MyGroupListSelect(UserID, GrpID, Type);

            return Board;
        }


        //그룹 조회
        public DataSet MyGroupListSelect2(string UserID, string GrpID, string Type, string GatheringID = "0")
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = new DataSet();
            ds = Dac.MyGroupListSelect2(UserID, GrpID, Type, GatheringID);

            return ds;
        }

        //그룹 조회
        public DataSet DirectoryAllUserList(string GrpID)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = new DataSet();
            ds = Dac.DirectoryAllUserList(GrpID);

            return ds;
        }


        //추가
        //public void GlossaryMyGroupInsert(GlossaryGroupListType Board)
        //{
        //    GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
        //    DataSet ds = Dac.GlossaryMyGroupInsert(Board);
        //}


        //추가  
        public GlossaryGroupListType GlossaryMyGroupInsert(GlossaryGroupListType Board, string mode)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = Dac.GlossaryMyGroupInsert(Board, mode);

            Board.MyGrpID = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            //if (!string.IsNullOrEmpty(dirType.SvID))
            //    Dac.SurveyQstInsert(ds.Tables[0].Rows[0].ItemArray[0].ToString(), dirType.UserID);

            return Board;

        }

        /// <summary>
        /// my그룹 조직도 저장  
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Mode"></param>
        public void GlossaryMyGroupListInsert(string ItemID, string UserID, string ToUserID, string Mode)
        {
            string[] ToUser = ToUserID.Split('/');

            GlossaryGroupListType Board = new GlossaryGroupListType();
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();

            Board.MyGrpID = ItemID;


            Board.AuthID = UserID;
            Board.AudidID = UserID;
            Board.AuthType = "U";
            //등록자 
            Dac.GlossaryMyGroupListInsert(Board, Mode);

            for (int i = 0; i < ToUser.Length - 1; i++)
            {
                //그룹인지 사용자인지 구분 
                if (!GroupOrUserCheck(ToUser[i]))
                {

                    Board.AudidID = UserID;
                    Board.AuthID = ToUser[i];
                    Board.AuthType = "U";     //개인 

                    Dac.GlossaryMyGroupListInsert(Board, Mode);
                }
                else
                {
                    Board.AudidID = UserID;
                    Board.AuthType = "O";   //팀  
                    Board.AuthID = ToUser[i];

                    Dac.GlossaryMyGroupListInsert(Board, Mode);
                }
            }
        }


        //조직도 변경으로 추가  
        //biz.GlossaryMyGroupListInsert(hdGrpID.Value, u.UserID, UDList.AuthID, UDList.AuthCL, "Insert"); 
        public void GlossaryMyGroupListInsert(string ItemID, string UserID, string ToUserID, string ToUserType, string Mode)
        {
            string[] ToUser = ToUserID.Split('/');
            string[] ToType = ToUserType.Split('/');


            GlossaryGroupListType Board = new GlossaryGroupListType();
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();

            //등록자 
            Board.MyGrpID = ItemID;
            //Board.AuthID = UserID;
            //Board.AudidID = UserID;
            //Board.AuthType = "U";
            //Dac.GlossaryMyGroupListInsert(Board, Mode);

            //for (int i = 0; i < ToUser.Length - 1; i++)
            //{
            //    //그룹인지 사용자인지 구분 
            //    if (!GroupOrUserCheck(ToUser[i]))
            //    {
            //        Board.AudidID = UserID;
            //        Board.AuthID = ToUser[i];
            //        Board.AuthType = "U";     //개인 
            //        Dac.GlossaryMyGroupListInsert(Board, Mode);
            //    }
            //    else
            //    {
            //        Board.AudidID = UserID;
            //        Board.AuthType = "O";   //팀  
            //        Board.AuthID = ToUser[i];
            //        Dac.GlossaryMyGroupListInsert(Board, Mode);
            //    }
            //}


            for (int i = 0; i < ToUser.Length - 1; i++)
            {
                Board.AudidID = UserID;
                Board.AuthType = ToType[i];
                Board.AuthID = ToUser[i];

                Dac.GlossaryMyGroupListInsert(Board, Mode);
            }
        }


        //조직인지 개인인지 구분  
        public bool GroupOrUserCheck(string ToUserID)
        {

            bool retValue = false;

            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_Survey_GroupOrUserCheck");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUserID.ToString());

            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                retValue = true;
            }

            return retValue;
        }

        //GlossaryMyGroupDelete 

        // 댓글  삭제
        public void GlossaryMyGroupDelete(string ID)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            DataSet ds = Dac.GlossaryMyGroupDelete(ID);
        }

        //그룹변경 - 문서공유 연동  
        public DataSet GlossaryMyGroupChangeSelect(string mode, string GrpID)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();

            DataSet ds = new DataSet();
            ds = Dac.GlossaryMyGroupChangeSelect(mode, GrpID);

            return ds;
        }


        public DataSet GlossaryMyGroupUserChangeSelect(string mode, string DirID)
        {
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();

            DataSet ds = new DataSet();
            ds = Dac.GlossaryMyGroupUserChangeSelect(mode, DirID);

            return ds;
        }





    }



}
