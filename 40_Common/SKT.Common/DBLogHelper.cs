using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Common
{
    public class DBLogHelper
    {
        static public void WriteErrorLog(string Title)
        {
            WriteErrorLog(Title, string.Empty);
        }
        static public void WriteErrorLog(string Title, string Message)
        {
            WriteErrorLog(Title, Message, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
        static public void WriteErrorLog(string Title, string Message, string SystemName, string BoardID
            , string UserID, string URL, string ClientIP, string ClientApp)
        {
            Logging(LogLevel.Error, Title, Message, SystemName, BoardID, UserID, URL, ClientIP, ClientApp);
        }

        static public void Logging(LogLevel ExcptionLvl, string Title, string Message, string SystemName, string BoardID,
            string UserID, string URL, string ClientIP, string ClientApp)
        {
            //비즈 호출
            using (ExceptionLogBiz biz = new ExceptionLogBiz())
            {
                //DataSet ds = biz.Logging(ExcptionLvl.ToString(), Title, Message, SystemName, BoardID, UserID, ClientIP, ClientApp, URL, DateTime.Now);
            }
        }

        static public void ExceptionLogging(string Message, string Source, string StackTrace, string TargetSite
            , string UserID, string URL, string ClientIP, string ClientBrowser)
        {
            using (ExceptionLogBiz biz = new ExceptionLogBiz())
            {
                DataSet ds = biz.ExceptionLogging(Message, Source, StackTrace, TargetSite, UserID, URL, ClientIP, ClientBrowser, DateTime.Now);
            }
        }
    }

    public enum LogLevel
    { 
        Critical,
        Error, 
        Warning, 
        Informational, 
        Verbose
    }

    public class ExceptionLogBiz : IDisposable
    {
        // Methods
        public void Dispose()
        {
        }

        //public DataSet Logging(string LogLevel, string Title, string Message, string SystemName, string BoardID
        //    , string UserID, string ClientIP, string ClientApp, string URL, DateTime LogTime)
        //{
        //    //using (ExceptionLogDac dac = new ExceptionLogDac())
        //    //{
        //    //    return dac.Logging(LogLevel, Title,Message, SystemName, BoardID, UserID, ClientIP, ClientApp, URL, LogTime);
        //    //}
        //}

        public DataSet ExceptionLogging(string Message, string Source, string StackTrace, string TargetSite
            , string UserID, string URL, string ClientIP, string ClientBrowser, DateTime LogTime)
        {
            using (ExceptionLogDac dac = new ExceptionLogDac())
            {
                return dac.ExceptionLogging(Message, Source, StackTrace, TargetSite, UserID, URL, ClientIP, ClientBrowser, LogTime);
            }
        }
    }
    public class ExceptionLogDac : IDisposable
    {
        private const string connectionStringName = "ConnGlossary";

        //public DataSet Logging(string LogLevel, string Title, string Message, string SystemName, string BoardID
        //    , string UserID, string ClientIP, string ClientApp, string URL, DateTime LogTime)
        //{
        //    Database db = DatabaseFactory.CreateDatabase(connectionStringName);
        //    DbCommand cmd = db.GetStoredProcCommand("up_Log_Insert");
        //    db.AddInParameter(cmd, "LogLevel", DbType.String, LogLevel);
        //    db.AddInParameter(cmd, "Title", DbType.String, string.IsNullOrEmpty(Title) ? (object)DBNull.Value : Title);
        //    db.AddInParameter(cmd, "Message", DbType.String, string.IsNullOrEmpty(Message) ? (object)DBNull.Value : Message);
        //    db.AddInParameter(cmd, "SystemName", DbType.String, string.IsNullOrEmpty(SystemName) ? (object)DBNull.Value : SystemName);
        //    db.AddInParameter(cmd, "BoardID", DbType.Int16, string.IsNullOrEmpty(BoardID) ? (object)DBNull.Value : BoardID);
        //    db.AddInParameter(cmd, "UserID", DbType.String, string.IsNullOrEmpty(UserID) ? (object)DBNull.Value : UserID);
        //    db.AddInParameter(cmd, "ClientIP", DbType.String, string.IsNullOrEmpty(ClientIP) ? (object)DBNull.Value : ClientIP);
        //    db.AddInParameter(cmd, "ClientApp", DbType.String, string.IsNullOrEmpty(ClientApp) ? (object)DBNull.Value : ClientApp);
        //    db.AddInParameter(cmd, "URL", DbType.String, string.IsNullOrEmpty(URL) ? (object)DBNull.Value : URL);
        //    db.AddInParameter(cmd, "LogTime", DbType.DateTime, LogTime);
        //    return db.ExecuteDataSet(cmd);
        //}
        public DataSet ExceptionLogging(string Message, string Source, string StackTrace, string TargetSite
            , string UserID, string URL, string ClientIP, string ClientBrowser, DateTime LogTime)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Exception_Insert");
            db.AddInParameter(cmd, "Message", DbType.String, string.IsNullOrEmpty(Message) ? (object)DBNull.Value : Message);
            db.AddInParameter(cmd, "Source", DbType.String, string.IsNullOrEmpty(Source) ? (object)DBNull.Value : Source);
            db.AddInParameter(cmd, "StackTrace", DbType.String, string.IsNullOrEmpty(StackTrace) ? (object)DBNull.Value : StackTrace);
            db.AddInParameter(cmd, "TargetSite", DbType.String, string.IsNullOrEmpty(TargetSite) ? (object)DBNull.Value : TargetSite);
            db.AddInParameter(cmd, "UserID", DbType.String, string.IsNullOrEmpty(UserID) ? (object)DBNull.Value : UserID);
            db.AddInParameter(cmd, "URL", DbType.String, string.IsNullOrEmpty(URL) ? (object)DBNull.Value : URL);
            db.AddInParameter(cmd, "ClientIP", DbType.String, string.IsNullOrEmpty(ClientIP) ? (object)DBNull.Value : ClientIP);
            db.AddInParameter(cmd, "ClientBrowser", DbType.String, string.IsNullOrEmpty(ClientBrowser) ? (object)DBNull.Value : ClientBrowser);
            db.AddInParameter(cmd, "LogTime", DbType.DateTime, LogTime);
            return db.ExecuteDataSet(cmd);
        }
        public void Dispose()
        {
            ;
        }
    }
}
