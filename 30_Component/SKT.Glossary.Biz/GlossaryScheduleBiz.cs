using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Transactions;
using SKT.Common;
using System.Configuration;

namespace SKT.Glossary.Biz
{
    public class GlossaryScheduleBiz
    {
        public DataSet GlossaryScheduleInsert(GlossaryScheduleType Schedule)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            return Dac.GlossaryScheduleInsert(Schedule);
        }

        public void GlossaryScheduleUpdate(GlossaryScheduleType Schedule)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            DataSet ds = Dac.GlossaryScheduleUpdate(Schedule);
        }

        public void GlossaryScheduleDelete(GlossaryScheduleType Schedule)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            DataSet ds = Dac.GlossaryScheduleDelete(Schedule);
        }

        

        public void GlossaryScheduleAuthDelete(string SCID, string Type)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            DataSet ds = Dac.GlossaryScheduleAuthDelete(SCID, Type);
        }

        /// <summary>
        /// 최초 글 작성시 일부공개 일 경우 공유 추가되는 부분
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Title"></param>
        public void GlossaryScheduleAuthInsert(string ItemID, string UserID, string ToUserID, string AuthCL, string AuthRWX, string Mode)
        {
            string[] ToUser = ToUserID.Split('/');
            string[] ToUserType = AuthCL.Split('/');

            CommonAuthType Auth = new CommonAuthType();
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();

            Auth.ItemID = ItemID;

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
                Auth.AuthID = UserID;
                Auth.AuditID = UserID;
                Auth.AuthType = "U";
                Auth.AuthRWX = "RW";

                //등록자 
                Dac.GlossaryScheduleAuthInsert(Auth, Mode);
            }

            for (int i = 0; i < ToUser.Length - 1; i++)
            {
                //**************************************************// 
                //기존의 처리는  View_User 테이블에서 사용자를 조회하는데 
                //조직도와 매핑이 되지 않아 조직도를 조회하는 테이블로 변경 
                //**************************************************// 

                Auth.AuthID = ToUser[i];
                Auth.AuthType = ToUserType[i];
                Auth.AuthRWX = AuthRWX;

                Dac.GlossaryScheduleAuthInsert(Auth, Mode);
            }
        }

        public List<CommonAuthType> GlossaryScheduleAuthSelect(string ID)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();

            List<CommonAuthType> listGlossaryAuthType = Dac.GlossaryScheduleAuthSelect(ID);
            return listGlossaryAuthType;
        }
    }
}

