using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SKT.Glossary.Type;

namespace SKT.Glossary.Dac
{

    /// <summary>
    /// FloatingDac
    /// </summary>
    public class FloatingDac
    {
        private const string connectionStringName = "ConnGlossary";

        //검색결과 자동 완성
        public DataSet SearchAutoList(string Search, string Type)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_SearchAuto_List");
            db.AddInParameter(cmd, "SearchWord", DbType.String, Search);
            db.AddInParameter(cmd, "Type", DbType.String, Type);
            return db.ExecuteDataSet(cmd);
        }

        //용어 리스트
        public DataSet FloatingList(string SearchKeyword)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Floating_List");
            db.AddInParameter(cmd, "SearchKeyword", DbType.String, SearchKeyword);
            return db.ExecuteDataSet(cmd);
        }

        //용어 View
        public DataSet FloatingSelect(string ItemID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Floating_Select");
            db.AddInParameter(cmd, "ID", DbType.String, ItemID);
            return db.ExecuteDataSet(cmd);
        }


        //환경설정
        public DataSet FloatingSettingInsert(FloatingType Board)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_FloatingSetting_Insert");
            db.AddInParameter(cmd, "UserID", DbType.String, Board.UserID);
            db.AddInParameter(cmd, "DoubleClick", DbType.String, Board.DoubleClick);
            db.AddInParameter(cmd, "Drag", DbType.String, Board.Drag);
            db.AddInParameter(cmd, "Removal", DbType.String, Board.Removal);

            return db.ExecuteDataSet(cmd);
        }

        //환경설정 select
        public DataSet FloatingSettingSelect(string UserID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_FloatingSetting_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            return db.ExecuteDataSet(cmd);
        }

        //Glossary List
        public DataSet GlossaryQnAList()
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_FloatingQnA_List");
            return db.ExecuteDataSet(cmd);
        }
    }
}
