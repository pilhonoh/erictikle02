using SKT.Tnet.Framework.Diagnostics.Utilities;
using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Common;
using System;
using System.Text;
using System.Web;

namespace SKT.Tnet.Framework.Diagnostics
{
    /// <Summary>
    /// 로그 데이터 개체 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    [Serializable]
    public class LogData
    {
        #region Property
        public int ID { get; set; }

        /// <summary>
        /// 로그 수준 타입
        /// </summary>
        public LogEntryType EntryType { get; set; }

        /// <summary>
        /// 로그 원본 타입
        /// </summary>
        public LogSourceType SourceType { get; set; }

        /// <summary>
        /// 비즈니스 타입
        /// </summary>
        public string BizType { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public string Description { get; set; }

        public DateTime OccurTime { get; set; }

        private double m_ExecuteTime = 0.0;

        public double ExecuteTime
        {
            get { return m_ExecuteTime; }
            set { m_ExecuteTime = value; }
        }

        /// <summary>
        /// 클라이언트 IP
        /// </summary>
        public string RequestIP { get; set; }

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        public string RequestUser { get; set; }

        /// <summary>
        /// 에러 발생 웹 페이지 URL
        /// </summary>
        public string RequestUrl { get; set; }

        public string ServerMachine { get; set; }

        [NonSerializedAttribute]
        private Exception m_Exception = null;

        public Exception Exception { get { return m_Exception; } }
        #endregion

        #region Constructor
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public LogData()
        {
            this.StartTimer();

            this.BizType = "ETC";
            this.Message = "";
            this.Description = "";
            this.ServerMachine = System.Environment.MachineName;

            if (HttpContext.Current != null)
            {
                //this.RequestIP = HttpContext.Current.Request.UserHostAddress;
                this.RequestIP = ContextHelper.GetUserIpAddress();
                //this.RequestUser = HttpContext.Current.User.Identity.Name;
                this.RequestUser = CookieHelper.Get(CoreContants.LoginCookieUser);

                if (string.IsNullOrEmpty(this.RequestUser) == true)
                {
                    this.RequestUser = "";
                }


                this.RequestUrl = HttpContext.Current.Request.Url.ToString();
            }
        }

        /// <summary>
        /// 생성자 (Exception 로그 관련)
        /// </summary>
        /// <param name="ex"></param>
        public LogData(Exception ex)
            : this()
        {
            if (ex != null)
            {
                StringBuilder description = new StringBuilder();
                LogUtility.GatherExceptionDescription(ref description, ex, 0);

                this.EntryType = LogEntryType.Exception;
                this.Message = ex.Message;
                this.Description = description.ToString();

                this.m_Exception = ex;
            }
        }
        #endregion

        /// <summary>
        /// 타이머 동작 여부
        /// </summary>
        [NonSerializedAttribute]
        private bool m_IsTimerStopped = false;

        private bool IsTimerStopped
        {
            get { return m_IsTimerStopped; }
        }

        /// <summary>
        /// 타이머 시작.
        /// </summary>
        internal void StartTimer()
        {
            m_IsTimerStopped = false;
            this.OccurTime = DateTime.Now;
        }

        /// <summary>
        /// 타이머 종료.
        /// </summary>
        internal void StopTimer()
        {
            if (this.IsTimerStopped)
                return;

            DateTime dtEntTime = DateTime.Now;
            TimeSpan ts = new TimeSpan(dtEntTime.Ticks - this.OccurTime.Ticks);
            m_ExecuteTime = ts.TotalMilliseconds;
            m_IsTimerStopped = true;
        }
    }
}