using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Configuration;
using System;
using System.Diagnostics;

namespace SKT.Tnet.Framework.Diagnostics
{
    /// <Summary>
    /// 시스템 진단을 위한 로그를 관리한다.
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class LogManager
    {
        #region Property
        private static LogMode m_LogMode = LogMode.File;
        private static LogData m_LogData = null;

        /// <summary>
        /// 현재 로그 데이터 반환
        /// </summary>
        public LogData CurrentLogData
        {
            get
            {
                return m_LogData;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// #. 생성자
        /// </summary>
        private LogManager()
        {
            try
            {
                m_LogMode = (LogMode)ConfigReader.GetInteger("LogMode");
            }
            catch { }
        }

        /// <summary>
        /// #. 생성자
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="sourceType"></param>
        public LogManager(LogEntryType entryType, LogSourceType sourceType)
            : this()
        {
            m_LogData = new LogData();
            m_LogData.EntryType = entryType;
            m_LogData.SourceType = sourceType;
        }

        /// <summary>
        /// #. 생성자
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="ex"></param>
        public LogManager(LogSourceType sourceType, Exception ex)
            : this()
        {
            m_LogData = new LogData(ex);
            m_LogData.SourceType = sourceType;
        }
        #endregion

        /// <summary>
        /// 타이머 종료.
        /// </summary>
        public void StopTimer()
        {
            if (m_LogData != null)
                m_LogData.StopTimer();
        }

        /// <summary>
        /// 로그 작성
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="bizType"></param>
        /// <param name="ex"></param>
        public static LogData WriteLog(LogSourceType sourceType, string bizType, Exception ex)
        {
            LogManager logManager = new LogManager(sourceType, ex);
            return logManager.WriteLog(bizType, "", "");
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
        public static LogData WriteLog(LogEntryType entryType, LogSourceType sourceType, string bizType, string message, string description)
        {
            LogManager logManager = new LogManager(entryType, sourceType);
            return logManager.WriteLog(bizType, message, description);
        }

        /// <summary>
        /// 로그 작성.
        /// </summary>
        protected LogData WriteLog(string bizType, string message, string description)
        {
            if (CalledBySelf()) return null;

            if (!string.IsNullOrEmpty(bizType))
            { 
                m_LogData.BizType = bizType;
            }

            if (m_LogData.EntryType == LogEntryType.Exception)
            {
                m_LogData.Message = (string.IsNullOrEmpty(message) ? "" : "[[" + message + "]]") + m_LogData.Message;
                m_LogData.Description = (string.IsNullOrEmpty(description) ? "" : "[[" + description + "]]") + m_LogData.Description;
            }
            else
            {
                if (!string.IsNullOrEmpty(message))
                {
                    m_LogData.Message = message;
                }


                if (!string.IsNullOrEmpty(description))
                {
                    m_LogData.Description = description;
                }
            }

            Logging();

            return m_LogData;
        }

        /// <summary>
        /// LogManager 원본 소스에 대한 로그인지 체크.
        /// </summary>
        /// <returns></returns>
        private static bool CalledBySelf()
        {
            // true: LogManager를 원본 소스로 인식하여, 로깅을 중단한다.
            bool rtnValue = true;

            // LogManager에서 호출한 공통 모듈에서 오류가 발생해서 다시 LogManager로 돌아온 경우에는 로깅을 스킵한다.
            try
            {
                // 호출 스택에서 역순으로 찾는다.
                // 1. LogManager 클래스가 포함된 프레임은 일단 제외한다.
                // 2. LogManager 클래스가 아닌 클래스가 나온 이후에 다시 LogManager 클래스가 나온다면 로깅 스킵.

                StackTrace st = new StackTrace();

                System.Type targetType = null;
                System.Type currentType = st.GetFrame(0).GetMethod().DeclaringType;

                // 0번 인덱스는 현재 메소드이다.
                int i = 1;

                // 처음 프레임부터 다른 클래스가 나올 때까지 일단 스킵 : 내부에서 메소드 호출 시의 처리
                for (; i < st.FrameCount; i++)
                {
                    targetType = st.GetFrame(i).GetMethod().DeclaringType;

                    // 1. LogManager 클래스가 포함된 프레임은 일단 제외한다.
                    if (currentType.Equals(targetType)) continue;

                    // 다른 클래스가 나왔다면 일단 중지
                    break;
                }

                // 다른 네임 스페이스가 나온 이후부터 체크
                for (; i < st.FrameCount; i++)
                {
                    targetType = st.GetFrame(i).GetMethod().DeclaringType;

                    // 2. LogManager 클래스가 아닌 클래스가 나온 이후에 다시 LogManager 클래스가 나온다면 로깅 스킵.
                    if (currentType.Equals(targetType)) return true;

                    // 다른 클래스일 경우, LogManager에서 호출한 내용이 아님.
                    rtnValue = false;
                }

                // 여기서 false로 설정하지 않음. 첫번째 루프에서 끝까지 간 경우, 반환값은 true여야 함.
            }
            catch { }

            return rtnValue;
        }

        /// <summary>
        /// 로그 모드에 따른 로그 작성.
        /// </summary>
        private static void Logging()
        {
            switch (m_LogMode)
            {
                case LogMode.File:
                    FileLogHelper.WriteLog(m_LogData);
                    break;


                default: return;
            }
        }
    }
}