using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using System;
using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// 쿠키 관련 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class CookieHelper
    {
        public static readonly DateTime BrowserCookie = DateTime.MinValue;

        #region Add

        /// <summary>
        /// 브라우저 쿠키 저장
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        public static void Add(string cookieName, string value)
        {
            Add(null, cookieName, value, BrowserCookie);
        }

        /// <summary>
        /// 쿠키 저장
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        /// <param name="expirationMinutes">만료 시간(분). 현재 시각으로부터 경과 시간. 0이면 브라우저 쿠키로 저장. 기본값 0.</param>
        public static void Add(string cookieName, string value, int expirationMinutes)
        {
            Add(null, cookieName, value, expirationMinutes);
        }

        /// <summary>
        /// 쿠키 저장
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        /// <param name="expires">만료 일시. CookieUtil.BrowserCookie 값을 넘기면 브라우저 쿠키로 저장. 기본값 CookieUtil.BrowserCookie.</param>
        public static void Add(string cookieName, string value, DateTime expires)
        {
            Add(null, cookieName, value, expires);
        }

        /// <summary>
        /// 브라우저 쿠키 저장
        /// </summary>
        /// <param name="response">HttpResponse 개체. null이면 HttpContext.Current.Response 사용.</param>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        public static void Add(HttpResponse response, string cookieName, string value)
        {
            Add(response, cookieName, value, BrowserCookie);
        }

        /// <summary>
        /// 쿠키 저장
        /// </summary>
        /// <param name="response">HttpResponse 개체. null이면 HttpContext.Current.Response 사용.</param>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        /// <param name="expirationMinutes">만료 시간(분). 현재 시각으로부터 경과 시간. 0이면 브라우저 쿠키로 저장. 기본값 0.</param>
        public static void Add(HttpResponse response, string cookieName, string value, int expirationMinutes)
        {
            DateTime expires = BrowserCookie;
            if (expirationMinutes > 0)
            {
                expires = DateTime.Now.AddMinutes(expirationMinutes);
            }
            Add(response, cookieName, value, expires);
        }

        /// <summary>
        /// 쿠키 저장
        /// </summary>
        /// <param name="response">HttpResponse 개체. null이면 HttpContext.Current.Response 사용.</param>
        /// <param name="cookieName">쿠키 이름</param>
        /// <param name="value">쿠키 값. Base64 인코딩하여 저장된다.</param>
        /// <param name="expires">만료 일시. CookieUtil.BrowserCookie 값을 넘기면 브라우저 쿠키로 저장. 기본값 CookieUtil.BrowserCookie.</param>
        public static void Add(HttpResponse response, string cookieName, string value, DateTime expires)
        {
            // 쿠키 생성
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.HttpOnly = true;

            // 만료 설정
            if (expires == DateTime.MinValue)
            {
                // 브라우저 쿠키
            }
            else
            {
                cookie.Expires = expires;
            }

            // 값 설정
            cookie.Value = value;

            // 도메인 설정
            string domain = ConfigReader.GetString(CoreContants.COOKIE_DOMAIN);
            if (!String.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }

            // 응답 검증
            if (response == null)
            {
                response = HttpContext.Current.Response;
            }

            // 추가
            response.Cookies.Add(cookie);
        }

        #endregion Add

        #region Get

        /// <summary>
        /// 쿠키에서 값 읽음
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        /// <returns>쿠키 값. 없으면 null</returns>
        public static string Get(string cookieName)
        {
            return Get(null, cookieName);
        }

        /// <summary>
        /// 쿠키에서 값 읽음
        /// </summary>
        /// <param name="request">HttpRequest 개체. null이면 HttpContext.Current.Request 사용.</param>
        /// <param name="cookieName">쿠키 이름</param>
        /// <returns>쿠키 값. 없으면 null</returns>
        public static string Get(HttpRequest request, string cookieName)
        {
            string returnValue = null;

            try
            {
                if (request == null)
                {
                    request = HttpContext.Current.Request;
                }

                HttpCookie cookie = request.Cookies[cookieName];
                
                if (cookie != null)
                {
                    cookie.HttpOnly = true;
                    returnValue = cookie.Value;
                }
            }
            catch { }

            return returnValue;
        }

        #endregion Get

        #region Remove

        /// <summary>
        /// 응답 쿠키 제거
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        public static void Remove(string cookieName)
        {
            Add(cookieName, "", DateTime.Now.AddDays(-10));
        }

        #endregion Remove

        #region Exists

        public static bool Exists(string key)
        {
            if (string.IsNullOrEmpty(CookieHelper.Get(key)) == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion Exists
    }
}