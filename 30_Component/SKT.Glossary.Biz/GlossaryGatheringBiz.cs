using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SKT.Glossary.Biz
{
    public class GlossaryGatheringBiz
    {
        #region 끌.모임 관리

        /// <summary>
        /// 끌 모임 태그 게시글 목록 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringMainTag_Select(string GatheringID, string Board_Index, string Board_Count)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringMainTag_Select(GatheringID, Board_Index, Board_Count);

            return ds;
        }

        /// <summary>
        /// 끌 모임 태그 목록 삭제
        /// </summary>
        public DataSet GlossaryGatheringTag_Delete(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringTag_Delete(GatheringID);

            return ds;
        }
        /// <summary>
        /// 끌 모임 태그 목록 저장
        /// </summary>
        public DataSet GlossaryGatheringTag_Insert(string GatheringID, string TagTitle, string TagSort, string UserID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringTag_Insert(GatheringID, TagTitle, TagSort, UserID);

            return ds;
        }

        /// <summary>
        /// 끌 모임 정렬 목록 삭제
        /// </summary>
        public DataSet GlossaryGatheringSort_Delete(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringSort_Delete(GatheringID);

            return ds;
        }
        /// <summary>
        /// 끌 모임 정렬 목록 저장
        /// </summary>
        public DataSet GlossaryGatheringSort_Insert(string UserID, string GatheringID, int Sort)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringSort_Insert(UserID, GatheringID, Sort);

            return ds;
        }

        /// <summary>
        /// 끌 모임 태그 목록 조회
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringTag_List(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringTag_List(GatheringID);

            return ds;
        }

        public DataSet GlossaryGatheringPush(string GatheringID, string CommonID, string SupID, string TargetUserID, string SenderUserID, string Mode)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringPush(GatheringID, CommonID, SupID, TargetUserID, SenderUserID, Mode);

            return ds;
        }

        /// <summary>
        /// 끌.모임 알림 인원 조회
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="CommonID"></param>
        /// <param name="SupID"></param>
        /// <param name="UserID"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringMember_Check(string GatheringID, string CommonID, string SupID, string UserID, string Mode, string Gbn)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringMember_Check(GatheringID, CommonID, SupID, UserID, Mode, Gbn);

            return ds;
        }

        /// <summary>
        /// 끌.모임 알림 목록 조회
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringNoti_List(string UserID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringNoti_List(UserID);

            return ds;
        }

        /// <summary>
        /// 끌.모임 알림 저장
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="GatheringID"></param>
        /// <param name="NotiYN"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringNoti_Insert(string UserID, string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringNoti_Insert(UserID, GatheringID);

            return ds;
        }

        /// <summary>
        /// 끌.모임 알림 삭제
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="GatheringID"></param>
        /// <param name="NotiYN"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringNoti_Delete(string UserID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGatheringNoti_Delete(UserID);

            return ds;
        }

        /// <summary>
        /// 모임 목록 조회(현재 사용자 기준)
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_List(string Mode, string UserID, int PageNum, int PageSize)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            //Public
            DataSet ds = Dac.GlossaryGathering_List(Mode, PageNum, PageSize, UserID);

            return ds;
        }

        /// <summary>
        /// 모임 목록 조회(단순형)
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_List_Simple(string Mode, string UserID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.GlossaryGathering_List_Simple(Mode, UserID);

            return ds;
        }

        /// <summary>
        /// 모임 생성
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public GlossaryGatheringType GlossaryGathering_Insert(GlossaryGatheringType item, string Mode)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();
            DataSet ds = Dac.GlossaryGathering_Insert(item, Mode);

            item.GatheringID = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            return item;
        }

        /// <summary>
        /// 모임 삭제
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="UserID"></param>
        public void GlossaryGathering_Delete(string GatheringID, string UserID, string UserIP, string usermachinename)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();
            Dac.GlossaryGathering_Delete(GatheringID, UserID, UserIP,usermachinename);
        }

        /// <summary>
        /// 모임 멤버 조회
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_MemberList(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.GlossaryGathering_MemberList(GatheringID);

            return ds;
        }


        /// <summary>
        /// 모임 메뉴 조회
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringMenu_List(string GatheringID, string GatheringMenu)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.GlossaryGatheringMenu_List(GatheringID, GatheringMenu);

            return ds;
        }

        #endregion

        #region 권한 설정

        /// <summary>
        /// 권한 제거
        /// </summary>
        /// <param name="ID"></param>
        public void GlossaryGatheringAuth_Delete(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();
            Dac.GlossaryGatheringAuth_Delete(GatheringID);
        }

        /// <summary>
        /// 권한 등록
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Title"></param>
        public void GlossaryGatheringAuth_Insert(string ItemID, string UserID, string ToUserID, string AuthCL, string AuthRWX, string Mode)
        {
            string[] ToUser = ToUserID.Split('/');
            string[] ToUserType = AuthCL.Split('/');

            CommonAuthType Board = new CommonAuthType();
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

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

            // 모임 만든이
            Board.AuthID = UserID;
            Board.AuthType = "U"; 
            Board.RegID = UserID;
                
            Dac.GlossaryGatheringAuth_Insert(Board, Mode);

            // 멤버
            for (int i = 0; i < ToUser.Length - 1; i++)
            {
                Board.AuthID = ToUser[i];
                Board.AuthType = ToUserType[i];

                Dac.GlossaryGatheringAuth_Insert(Board, Mode);
            }
        }


        /// <summary>
        /// 권한 조회
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringAuth_Select(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.GlossaryGatheringAuth_Select(GatheringID);

            return ds;
        }

        /// <summary>
        /// 권한 조회 2016-04-20 주석
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet DeptToUser_Select(string deptcode)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.DeptToUser_Select(deptcode);

            return ds;
        }
        #endregion

        #region 끌.모임 - 지식, 일정

        /// <summary>
        /// 메뉴아이템 권한 등록
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="GatheringMenu"></param>
        /// <param name="CommonID"></param>
        public void GatheringMenuAuth_Insert(string GatheringID, string GatheringMenu, string CommonID)
        {
            GlossaryGatheringDac dac = new GlossaryGatheringDac();

            dac.GatheringMenuAuth_Insert(GatheringID, GatheringMenu, CommonID);
        }

        /// <summary>
        /// 메뉴아이템 권한 등록 - 전체공개
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="GatheringMenu"></param>
        /// <param name="CommonID"></param>
        public void GatheringMenuAuth_Insert(string GatheringID, string GatheringMenu, string CommonID, string PublicYN)
        {
            GlossaryGatheringDac dac = new GlossaryGatheringDac();

            dac.GatheringMenuAuth_Insert(GatheringID, GatheringMenu, CommonID, PublicYN);
        }

        /// <summary>
        /// 메뉴아이템 권한 조회
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        public List<PermissionsType> GatheringMenuAuth_Select(Int64 iD, string GatheringMenu)
        {
            GlossaryGatheringDac dac = new GlossaryGatheringDac();

            DataSet ds = dac.GatheringMenuAuth_Select(iD, GatheringMenu);

            List<PermissionsType> listPermissionsType = new List<PermissionsType>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PermissionsType permissionsType = new PermissionsType();
                    permissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");
                    permissionsType.ToUserName = (dr["ToUserName"] == DBNull.Value) ? null : dr.Field<string>("ToUserName");
                    permissionsType.ToUserType = (dr["ToUserType"] == DBNull.Value) ? null : dr.Field<string>("ToUserType");
                    listPermissionsType.Add(permissionsType);
                }
            }

            return listPermissionsType;
        }

        public DataSet GlossaryGatheringAuth_List(string GatheringID)
        {
            GlossaryGatheringDac Dac = new GlossaryGatheringDac();

            DataSet ds = Dac.GlossaryGatheringAuth_List(GatheringID);

            return ds;
        }
        /// <summary>
        /// 메뉴아이템 권한 제거
        /// </summary>
        /// <param name="CommonID"></param>
        public void GatheringMenuAuth_Delete(string CommonID, string GatheringMenu, string GatheringID)
        {
            GlossaryGatheringDac dac = new GlossaryGatheringDac();

            dac.GatheringMenuAuth_Delete(CommonID, GatheringMenu, GatheringID);
        }

        #endregion
    }
}
