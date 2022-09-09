using System;
using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// 캐쉬 관련 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class CacheHelper
    {
        /// <summary>
        /// 서버 캐시로부터 값을 읽음. 값이 없으면 null 반환.
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <returns>object 타입으로 반환. 값이 없으면 null 반환.</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)HttpContext.Current.Cache[key];
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 캐시에 값 저장. 만료 시간 기본값 30분
        /// </summary>
        /// <param name="key">키. 키가 존재할 경우, 삭제하고 새로 생성.</param>
        /// <param name="value">저장할 값. null이라도 저장됨.</param>
        public static void Add(string key, object value)
        {
            Add<object>(key, value, 30, false);
        }

        /// <summary>
        /// 캐시에 값 저장
        /// </summary>
        /// <param name="key">키. 키가 존재할 경우, 삭제하고 새로 생성.</param>
        /// <param name="value">저장할 값. null이라도 저장됨.</param>
        /// <param name="expirationMinutes">만료 시간 (분). 현재 시각부터의 경과 시간. 0일 경우, 기본값 30분.</param>
        public static void Add(string key, object value, int expirationMinutes)
        {
            Add<object>(key, value, expirationMinutes, false);
        }

        /// <summary>
        /// 캐시에 값 저장
        /// </summary>
        /// <param name="key">키. 키가 존재할 경우, 삭제하고 새로 생성.</param>
        /// <param name="value">저장할 값. null이라도 저장됨.</param>
        /// <param name="expirationMinutes">만료 시간 (분). 현재 시각부터의 경과 시간. 0일 경우, 기본값 30분.</param>
        /// <param name="sliding">상대 만료 여부. 기본값 false.</param>
        public static void Add<T>(string key, T value, int expirationMinutes, bool sliding) where T : class
        {
            // 일단 삭제
            Remove(key);

            // 경과 시간 검증 : 기본값 30분
            if (expirationMinutes < 1) expirationMinutes = 30;

            // 만료 설정
            DateTime absoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
            TimeSpan slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            if (sliding)
            {
                slidingExpiration = new TimeSpan(0, expirationMinutes, 0);
            }
            else
            {
                absoluteExpiration = DateTime.Now.AddMinutes(expirationMinutes);
            }

            // 새로 생성
            HttpContext.Current.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 캐시에서 값 삭제
        /// </summary>
        /// <param name="key">캐시 키</param>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            // 캐시에서 삭제
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// 서버 캐시로부터 해당 키에 대한 존재 여부
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <returns>존재 여부</returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }
    }
}