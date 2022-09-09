using SKT.Tnet.Framework.Configuration;

namespace SKT.Tnet.Framework.Common
{
    /// <Summary>
    /// 공통 상수 정의 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class CoreContants
    {
        // ConfigureSection 및 Category 관련 정의
        public const string DEFAULT_SECTION_NAME = "SKTSection";
        public const string DEFAULT_CATEGORY_NAME = "DefaultSettings";

        // SKT.Tnet.Framework 기본 DataBase 속성
        public const string DEFAULT_CONNECTIONSTRING_NAME = "SKTDefaultDB";

        // SKT.Tnet.Framework 기본 Message 파일 변수 설정
        public const string DEFAULT_MESSAGEFILE = "MessageXMLFile";

        // SKT.Tnet.Framework 기본 Performance 체크 여부
        public const string DEFAULT_PERFORMANCE_CHK = "PerformanceCheck";

        // SKT.Tnet.Framework 조직도 관련
        public const string DEFAULT_CATEGORY_ORGCHART = "OrgChart";

        // SKT.Tnet.Framework 조직도 관련 CompanyCodeName
        public const string DEFAULT_COMPANYCODE = "CompanyCode";

        // SKT.Tnet.Framework 기본 메일 서버
        public const string DEFAULT_SMTP_SERVER = "SMTPServerAddress";

        // 동기화 관련
        public const string DEFAULT_SYNC_NAME = "HRSyncSource";

        public const string PREFIX_PROFILE = "SESSION_PROFILE_";
        public const string SESSION_LOGIN_USERID = "SESSION_LOGIN_USERID";

        #region Interface 관련
        // Interface 관련 ConfigureSection
        public const string INTERFACE_SECTION_NAME = "SKTIFSection";

        // Web Service 관련 Category 정의
        public const string WEBSERVICE_CATEGORY_NAME = "WebServiceData";

        // 알림 I/F
        public const string NOTICE_CATEGORY_NAME = "NoticeData";
        #endregion

        /// <summary>
        /// 에러 페이지 표시 여부 (Y, N)
        /// </summary>
        public const string ERROR_MODE = "CustomerErrorMode";

        /// <summary>
        /// 공통 에러 페이지의 URL 정보
        /// </summary>
        public const string ERROR_TRANSFER_FILE = "CustomerErrorFile";
        /// <summary>
        /// 공통 에러 페이지의 URL 정보
        /// </summary>
        public const string ERROR_HTML_FILE = "CustomerErrorHtml";

        /// <summary>
        /// 쿠키 정보 이용시 도메인 정보
        /// </summary>
        public const string COOKIE_DOMAIN = "Cookie_Domain";

        /// <summary>
        /// 로그인시 쿠기 정보 키
        /// </summary>
        //public const string LoginCookieUser = "SM_USER";
        public static string LoginCookieUser
        {
            get
            {
                string Value = "SM_USER";

                try
                {
                    Value = ConfigReader.GetString(DEFAULT_SECTION_NAME, DEFAULT_CATEGORY_NAME, "Cookie_Name");
                }
                catch { }

                return Value;
            }
        }

        /// <summary>
        /// QueryString 관련 사용자 사번
        /// </summary>
        //public const string QueryStringUser = "SM_USER";
        public static string QueryStringUser
        {
            get
            {
                string Value = "SM_USER";

                try
                {
                    Value = ConfigReader.GetString(DEFAULT_SECTION_NAME, DEFAULT_CATEGORY_NAME, "QueryString_Name");
                }
                catch { }

                return Value;
            }
        }

        /// <summary>
        /// 로그인 페이지 페이지 
        /// </summary>
        public const string LoginUrl = "LoginUrl";
    }
}