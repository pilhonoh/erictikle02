using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Dac
{
   public class GlossaryProfileDac
    {
       private const string connectionStringName = "ConnGlossary";

       //사용자 정보 가져오기
       public DataSet UserSelect(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);

           DbCommand cmd = db.GetStoredProcCommand("up_UserInfo_Select");

           db.AddInParameter(cmd, "UserID", DbType.String, UserID);

           return db.ExecuteDataSet(cmd);
       }

       //사용자 정보 가져오기
       public DataSet UserSelectList(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);

           DbCommand cmd = db.GetStoredProcCommand("up_UserInfo_Select_List");

           db.AddInParameter(cmd, "UserID", DbType.String, UserID);

           return db.ExecuteDataSet(cmd);
       }

       //프로필 추가
       public DataSet GlossaryProfileInsert(GlossaryProfileType Board)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Profile_Insert");
           db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
           db.AddInParameter(cmd, "DeptCode", DbType.String, Board.DeptCode);
           db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
           db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
           db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
           return db.ExecuteDataSet(cmd);
       }

       //프로필 뷰
       public DataSet GlossaryProfileSelect(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Profile_Select");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }
              
       public string GetPicture(string UserID)
       {
           ////OrgChart에서 사진 정보 가져오기
           //string connectionStringName = "ConnOrgChart";
           //DbCommand cmd2 = db2.GetSqlStringCommand("SELECT [photoURL] FROM [OrgChart].[dbo].[tb_person] WHERE employeeID = '" + UserID + "'");
           Database db2 = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd2 = db2.GetStoredProcCommand("up_UserInfo_Photo");
           db2.AddInParameter(cmd2, "UserID", DbType.String, UserID);
           DataSet dsPhotoUrl = db2.ExecuteDataSet(cmd2);
           if (dsPhotoUrl.Tables.Count > 0 && dsPhotoUrl.Tables[0].Rows.Count > 0)
           {
               DataRow dr = dsPhotoUrl.Tables[0].Rows[0];
               return dr["photoURL"].ToString();
           }
           return "/common/images/user_none.png";
       }

       //부서프로필 뷰
       public DataSet GlossaryDeptProfileSelect(string DeptCode)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_DeptCodeProfile_Select");
           db.AddInParameter(cmd, "DeptCode", DbType.String, DeptCode);
           return db.ExecuteDataSet(cmd);
       }


       //메일로 사용자가져오기
       public DataSet GetProfileFromEmail(string Email)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Profile_FromEmailSelect");
           db.AddInParameter(cmd, "Email", DbType.String, Email);
           return db.ExecuteDataSet(cmd);
       }



       public DataSet GlossaryProfileList(int PageNum, int PageSize, string SearchKeyword)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Profile_Search");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "SearchWord", DbType.String, SearchKeyword);
           return db.ExecuteDataSet(cmd);
       }

       // 담당자
       public DataSet GlossaryJobDescriptionList(int PageNum, int PageSize, string SearchKeyword)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_JobDescription_Search");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "SearchWord", DbType.String, SearchKeyword);
           return db.ExecuteDataSet(cmd);
       }

       //사내케리어 초기화
       public DataSet GlossarySKCareerReset(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileSKCareer_Reset");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       //사외케리어 초기화
       public DataSet GlossaryNoSKCareerReset(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileNoSKCareer_Reset");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       //사내 케리어 리스트
       public DataSet GlossarySKCareerList(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileSKCareer_List");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       //사내 케리어 저장및 수정
       public DataSet GlossarySKCareerInsert(GlossaryProfileCareerAfterType Data)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileSKCareer_Insert");
           db.AddInParameter(cmd, "ID", DbType.Int64, Data.ID);
           db.AddInParameter(cmd, "UserID", DbType.String, Data.UserID);
           db.AddInParameter(cmd, "Date", DbType.String, Data.Date);
           db.AddInParameter(cmd, "Status", DbType.String, Data.Status);
           db.AddInParameter(cmd, "Depart", DbType.String, Data.Depart);
           db.AddInParameter(cmd, "Message", DbType.String, Data.Message);
           return db.ExecuteDataSet(cmd);
       }

       //사외 케리어 리스트
       public DataSet GlossaryNoSKCareerList(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileNoSKCareer_List");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }
       //사외 케리어 저장및 수정
       public DataSet GlossaryNoSKCareerInsert(GlossaryProfileCareerBeforeType Data)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileNoSKCareer_Insert");
           db.AddInParameter(cmd, "ID", DbType.Int64, Data.ID);
           db.AddInParameter(cmd, "UserID", DbType.String, Data.UserID);

           db.AddInParameter(cmd, "BeginDate", DbType.String, Data.BeginDate);
           db.AddInParameter(cmd, "EndDate", DbType.String, Data.EndDate);
           db.AddInParameter(cmd, "Company", DbType.String, Data.Company);
           db.AddInParameter(cmd, "Depart", DbType.String, Data.Depart);
           db.AddInParameter(cmd, "Position", DbType.String, Data.Position);
           db.AddInParameter(cmd, "Job", DbType.String, Data.Job);
           
           return db.ExecuteDataSet(cmd);
       }

        //사내 케리어 저장및 수정
       public DataSet GlossarySKCareerDelete(string ID,string Type)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileCareer_Delete");
           db.AddInParameter(cmd, "ID", DbType.Int64, ID);
           db.AddInParameter(cmd, "Type", DbType.String, Type);
           return db.ExecuteDataSet(cmd);
       }

       //사내 케리어 저장및 수정
       public DataSet GlossaryUserGlossaryList(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_ProfileUserGlossary_List");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }
    }
}
