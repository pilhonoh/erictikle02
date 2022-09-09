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
    public class GlossaryGatheringDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static GlossaryGatheringDac _instance = null;
        public static GlossaryGatheringDac Instance
        {
            get
            {
                GlossaryGatheringDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossaryGatheringDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        #region 끌.모임 관리

        public GlossaryGatheringDac() { }

        /// <summary>
        /// 끌.모임 태그게시글 목록 
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringMainTag_Select(string GatheringID, string Board_Index, string Board_Count)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMainTag_Select");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            db.AddInParameter(cmd, "Board_Index", DbType.Int32, Board_Index);
            db.AddInParameter(cmd, "Board_Count", DbType.Int32, Board_Count);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 정렬 삭제
        /// </summary>
        public DataSet GlossaryGatheringSort_Delete(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringSort_Delete");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 정렬 저장
        /// </summary>
        public DataSet GlossaryGatheringSort_Insert(string UserID, string GatheringID, int Sort)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringSort_Insert");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            db.AddInParameter(cmd, "Sort", DbType.Int16, Sort);
            
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 태그 목록 삭제
        /// </summary>
        public DataSet GlossaryGatheringTag_Delete(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringTag_Delete");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 태그 목록 저장
        /// </summary>
        public DataSet GlossaryGatheringTag_Insert(string GatheringID, string TagTitle, string TagSort, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringTag_Insert");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "TagSort", DbType.Int16, TagSort);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 태그 목록 조회
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringTag_List(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringTag_Select");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GlossaryGatheringPush(string GatheringID, string CommonID, string SupID, string TargetUserID, string SenderUserID, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringPush_Insert");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            db.AddInParameter(cmd, "CommonID", DbType.Int64, CommonID);
            db.AddInParameter(cmd, "SupID", DbType.Int64, SupID);
            db.AddInParameter(cmd, "TargetUserID", DbType.String, TargetUserID);
            db.AddInParameter(cmd, "SenderUserID", DbType.String, SenderUserID);
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);


            return db.ExecuteDataSet(cmd);
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
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            string strProc = string.Empty;

            if (Gbn.Equals("Noti"))
                strProc = "up_GlossaryGatheringNoti_Check";
            else
                strProc = "up_GlossaryGatheringMobileNoti_Check";

            DbCommand cmd = db.GetStoredProcCommand(strProc);

            db.AddInParameter(cmd, "@GatheringID", DbType.Int64, GatheringID);
            db.AddInParameter(cmd, "@CommonID", DbType.Int64, CommonID);
            db.AddInParameter(cmd, "@SupID", DbType.Int64, SupID);
            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "@Mode", DbType.String, Mode);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 알림 목록 조회
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringNoti_List(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringNoti_Select");

            db.AddInParameter(cmd, "@UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
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
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringNoti_Insert");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);
            

            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// 끌.모임 알림 삭제
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringNoti_Delete(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringNoti_Delete");

            db.AddInParameter(cmd, "UserID", DbType.String, UserID);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 목록 조회
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_List(string Mode, int PageNum, int PageSize, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGathering_List");
            
            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 목록 조회(단순형)
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_List_Simple(string Mode, string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGathering_List_Simple");

            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 끌.모임 정보 입력
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_Insert(GlossaryGatheringType item, string Mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGathering_Insert");

            db.AddInParameter(cmd, "Mode", DbType.String, Mode);
            db.AddInParameter(cmd, "GatheringID", DbType.String, item.GatheringID);
            db.AddInParameter(cmd, "GatheringName", DbType.String, item.GatheringName);
            db.AddInParameter(cmd, "UseYN", DbType.String, item.UseYN);
            db.AddInParameter(cmd, "Author", DbType.String, item.Author);
            db.AddInParameter(cmd, "Editor", DbType.String, item.Editor);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 모임 삭제
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="UserID"></param>
        public void GlossaryGathering_Delete(string GatheringID, string UserID, string userip, string usermacnhinename)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGathering_Delete");
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            db.AddInParameter(cmd, "Editor", DbType.String, UserID);
            db.AddInParameter(cmd, "WHOIPDEL", DbType.String, userip);
            db.AddInParameter(cmd, "WHOMACHINENAMEDEL", DbType.String, usermacnhinename);
            db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 모임 멤버 조회
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GlossaryGathering_MemberList(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMenuAuth_Check");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));

            return db.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 모임 메뉴 조회
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringMenu_List(string GatheringID, string GatheringMenu)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMenuItem_Select");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));
            db.AddInParameter(cmd, "GatheringMenu", DbType.String, GatheringMenu);

            return db.ExecuteDataSet(cmd);
        }

        #endregion

        #region 권한 설정

        /// <summary>
        /// 권한 제거
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public void GlossaryGatheringAuth_Delete(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringAuth_Delete");
            db.AddInParameter(cmd, "GatheringID", DbType.String, GatheringID);
            db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 권한 설정
        /// </summary>
        /// <param name="Board"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringAuth_Insert(CommonAuthType Board, string mode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringAuth_Insert");

            db.AddInParameter(cmd, "GatheringID", DbType.String, Board.ItemID);
            db.AddInParameter(cmd, "AuthID", DbType.String, Board.AuthID); 
            db.AddInParameter(cmd, "AuthType", DbType.String, Board.AuthType);
            db.AddInParameter(cmd, "Author", DbType.String, Board.RegID);

            return db.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 권한 조회
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringAuth_Select(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringAuth_Select");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 권한 리스트
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <returns></returns>
        public DataSet GlossaryGatheringAuth_List(string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringAuth_List");

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));

            return db.ExecuteDataSet(cmd);
        }      

        /// <summary>
        /// 권한 조회 2016-04-20 주석
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet DeptToUser_Select(string deptcode)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_Dept_To_User_Select");

            db.AddInParameter(cmd, "@DEPTCODE", DbType.String, deptcode);

            return db.ExecuteDataSet(cmd);
        }
 
        #endregion

        #region 끌.모임 - 지식

        /// <summary>
        /// 권한 등록
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="GatheringMenu"></param>
        /// <param name="CommonID"></param>
        public void GatheringMenuAuth_Insert(string GatheringID, string GatheringMenu, string CommonID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMenuAuth_Insert");
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));
            db.AddInParameter(cmd, "GatheringMenu", DbType.String, GatheringMenu);
            db.AddInParameter(cmd, "CommonID", DbType.Int32, Convert.ToInt32(CommonID));

            db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 권한 등록 - 전체공개
        /// </summary>
        /// <param name="GatheringID"></param>
        /// <param name="GatheringMenu"></param>
        /// <param name="CommonID"></param>
        public void GatheringMenuAuth_Insert(string GatheringID, string GatheringMenu, string CommonID, string PublicYN)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMenuAuth_Insert");
            db.AddInParameter(cmd, "GatheringID", DbType.Int64, Convert.ToInt64(GatheringID));
            db.AddInParameter(cmd, "GatheringMenu", DbType.String, GatheringMenu);
            db.AddInParameter(cmd, "CommonID", DbType.Int32, Convert.ToInt32(CommonID));
            db.AddInParameter(cmd, "PublicYN", DbType.String, PublicYN);

            db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 권한 조회
        /// </summary>
        /// <param name="CommonID"></param>
        /// <returns></returns>
        public DataSet GatheringMenuAuth_Select(Int64 CommonID, string GatheringMenu)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryGatheringMenuAuth_Select");

            db.AddInParameter(dbCommand, "CommonID", DbType.Int32, CommonID);

            db.AddInParameter(dbCommand, "GatheringMenu", DbType.String, GatheringMenu);

            return db.ExecuteDataSet(dbCommand);
        }

        public void GatheringMenuAuth_Delete(string CommonID, string GatheringMenu, string GatheringID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryGatheringMenuAuth_Delete");
            
            db.AddInParameter(cmd, "CommonID", DbType.Int32, CommonID);

            db.AddInParameter(cmd, "GatheringMenu", DbType.String, GatheringMenu);

            db.AddInParameter(cmd, "GatheringID", DbType.Int64, GatheringID);

            db.ExecuteScalar(cmd);
        }

        #endregion
    }
}
