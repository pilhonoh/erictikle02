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
   public class GlossaryScrapDac
    {
       private const string connectionStringName = "ConnGlossary";

       //스크랩 리스트
       public DataSet GlossaryScrapList(int PageNum, int PageSize, string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Scrap_List");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       //스크랩 뷰
       public DataSet GlossaryScrapSelect(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Scrap_Select");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }

       //스크랩 추가
       public DataSet GlossaryScrapInsert(GlossaryScrapType Board)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Scrap_Insert");
           db.AddInParameter(cmd, "GlossaryID", DbType.String, Board.GlossaryID);
           db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
           db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
           db.AddInParameter(cmd, "YouUserID", DbType.String, Board.YouUserID);
           db.AddInParameter(cmd, "ScrapsYN", DbType.String, Board.ScrapsYN);
           db.AddInParameter(cmd, "NoteYN", DbType.String, Board.NoteYN);
           return db.ExecuteDataSet(cmd);
       }

       //스크랩 삭제
       public DataSet GlossaryScrapDelete(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_Scrap_Delete");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }
    }
}
