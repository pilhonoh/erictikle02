using System;
using System.IO;
using System.Text;
using System.Threading;

using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Diagnostics.Utilities;
using SKT.Tnet.Framework.Security;

namespace SKT.Tnet.Framework.Diagnostics
{
    /// <Summary>
    /// 로그 파일 Helper 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class FileLogHelper
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();



        /// <summary>
        /// 로그 파일 경로
        /// </summary>
        private string m_LogFilePath = "";

        /// <summary>
        /// 로그 데이터
        /// </summary>
        private LogData m_LogData = null;

        /// <summary>
        /// #1. 생성자
        /// </summary>
        /// <param name="logEntryType"></param>
        public FileLogHelper(LogData logData)
        {
            m_LogData = logData;

            Initialize();
        }

        /// <summary>
        /// 로깅 카테고리에 따라 로그 패스를 설정.
        /// </summary>
        private void Initialize()
        {
            Impersonation im = new Impersonation();
            im.ImpersonationStart();

            string folderPath = ConfigReader.GetString("LogFileRootPath");
            folderPath += "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString();

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            m_LogFilePath = folderPath + string.Format("\\{0}_{1}.log", m_LogData.EntryType.ToString(), m_LogData.BizType);

            im.ImpersonationEnd();

            /*
            folderPath += "\\" + m_LogData.BizType;

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            m_LogFilePath = folderPath + string.Format("\\{0}_{1}.log", m_LogData.EntryType.ToString(), DateTime.Now.ToString("yyyyMMdd"));
            */
        }

        /// <summary>
        /// 파일 출력으로 로그 처리.
        /// </summary>
        private void Write()
        {
            System.Type type = this.GetType();
            try
            {
                Monitor.Enter(type);

                // 파일 출력 로그를 처리
                if (string.IsNullOrEmpty(this.m_LogFilePath) == false)
                {
                    using (FileStream fsLog = new FileStream(this.m_LogFilePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter swLog = new StreamWriter(fsLog))
                        {
                            StringBuilder sbMsg = LogUtility.GetLogMessage(m_LogData);
                            swLog.Write(sbMsg.ToString());
                        }
                    }
                }
            }
            catch { }
            finally
            {
                ////	Lock을 해제.
                Monitor.Exit(type);
            }
        }

        /// <summary>
        /// 로그 작성
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="bizType"></param>
        /// <param name="ex"></param>
        public static void WriteLog(LogSourceType sourceType, string bizType, Exception ex)
        {
            LogData logData = new LogData(ex);
            logData.BizType = bizType;
            logData.SourceType = sourceType;

            FileLogHelper fileLog = new FileLogHelper(logData);
            fileLog.Write();
        }

        /// <summary>
        /// 로그 작성
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="sourceType"></param>
        /// <param name="bizType"></param>
        /// <param name="message"></param>
        /// <param name="description"></param>
        /// <param name="logMode"></param>
        public static void WriteLog(LogEntryType entryType, LogSourceType sourceType, string bizType, string message, string description)
        {
            LogData logData = new LogData();
            logData.EntryType = entryType;
            logData.SourceType = sourceType;
            logData.BizType = bizType;
            logData.Message = message;
            logData.Description = description;

            FileLogHelper fileLog = new FileLogHelper(logData);
            fileLog.Write();
        }

        /// <summary>
        /// 로그 작성
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="sourceType"></param>
        /// <param name="bizType"></param>
        /// <param name="message"></param>
        /// <param name="description"></param>
        /// <param name="logMode"></param>
        public static void WriteLog(LogData logData)
        {
            FileLogHelper fileLog = new FileLogHelper(logData);
            fileLog.Write();
        }
    }
}