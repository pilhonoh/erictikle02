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
   public class GlossaryShareDac
    {
       private const string connectionStringName = "ConnGlossary";

       //공유 리스트
       public DataSet GlossaryShareList(int PageNum, int PageSize, string UserID, string TebType)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Share_List");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           db.AddInParameter(cmd, "TebType", DbType.String, TebType);
           return db.ExecuteDataSet(cmd);
       }

       //20140205 리스트와 통합되어 있던 기능을 분리, 공유 카운트
       public DataSet GlossaryShareCounts(string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("UP_SHARE_COUNTS");
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);

           return db.ExecuteDataSet(cmd);
       }

       //공유 뷰
       public DataSet GlossaryShareSelect(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Share_Select");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }

       //공유 추가
       public DataSet GlossaryShareInsert(GlossaryShareType Board)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Share_Insert");
           db.AddInParameter(cmd, "ItemID", DbType.String, Board.GlossaryID);
           db.AddInParameter(cmd, "FromUserID", DbType.String, Board.FromUserID);
           db.AddInParameter(cmd, "ToUserID", DbType.String, Board.ToUserID);
           db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
           db.AddInParameter(cmd, "UserName", DbType.String, Board.UserName);
           db.AddInParameter(cmd, "DeptName", DbType.String, Board.DeptName);
           db.AddInParameter(cmd, "BoardUserID", DbType.String, Board.BoardUserID);
           return db.ExecuteDataSet(cmd);
       }

       //공유 삭제
       public DataSet GlossaryShareDelete(string ID, string Type)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Share_Delete");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           db.AddInParameter(cmd, "Type", DbType.String, Type);
           return db.ExecuteDataSet(cmd);
       }

       /// <summary>
       /// 2014-05-15 Mr.No
       /// 공유되있던 리스트 모두 삭제
       /// </summary>
       /// <param name="GlossaryID"></param>
       public void GlossaryShareDelete_GlossaryID(int GlossaryID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Share_Delete_GlossaryID");
           db.AddInParameter(cmd, "GlossaryID", DbType.Int32, GlossaryID);
           
           db.ExecuteDataSet(cmd);
       }
    }
}
