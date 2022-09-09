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
   public class GlossaryTempDac
    {
       private const string connectionStringName = "ConnGlossary";


       //임시저장 리스트
       public DataSet GlossaryTempList(int PageNum, int PageSize, string UserID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTemp_List");
           db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
           db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
           db.AddInParameter(cmd, "UserID", DbType.String, UserID);
           return db.ExecuteDataSet(cmd);
       }

       //임시저장 추가
       public DataSet GlossaryTempInsert(GlossaryTempType Board)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTemp_Insert");
           db.AddInParameter(cmd, "ID", DbType.String, Board.ID);
           db.AddInParameter(cmd, "Type", DbType.String, Board.Type);
           db.AddInParameter(cmd, "CommonID", DbType.String, Board.CommonID);
           db.AddInParameter(cmd, "Title", DbType.String, Board.Title);
           db.AddInParameter(cmd, "Contents", DbType.String, Board.Contents);
           db.AddInParameter(cmd, "ContentsModify", DbType.String, Board.ContentsModify);
           db.AddInParameter(cmd, "Summary", DbType.String, Board.Summary);
           db.AddInParameter(cmd, "DocumentKind", DbType.String, Board.DocumentKind);
           db.AddInParameter(cmd, "PrivateYN", DbType.String, Board.PrivateYN);
           db.AddInParameter(cmd, "Description", DbType.String, Board.Description);
           db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
           db.AddInParameter(cmd, "CategoryID", DbType.Int32, Board.CategoryID);    //2014-05-08 Mr.No
           db.AddInParameter(cmd, "Permissions", DbType.String, Board.Permissions);    //2014-05-08 Mr.No
           return db.ExecuteDataSet(cmd);
       }

       //임시저장 Select
       public DataSet GlossaryTempSelect(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTemp_Select");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }

       //임시저장 Delete
       public DataSet GlossaryTempDelect(string ID)
       {
           Database db = DatabaseFactory.CreateDatabase(connectionStringName);
           DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTemp_Delete");
           db.AddInParameter(cmd, "ID", DbType.String, ID);
           return db.ExecuteDataSet(cmd);
       }
    }
}
