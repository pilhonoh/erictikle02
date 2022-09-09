using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// 섹션 관련 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class SessionHelper
    {
        /// <summary>
        /// 섹션으로부터 값을 읽음. 값이 없으면 null 반환.
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <returns>object 타입으로 반환. 값이 없으면 null 반환.</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)HttpContext.Current.Session[key];
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 섹션에 값 저장.
        /// </summary>
        /// <param name="key">키. 키가 존재할 경우, 삭제하고 새로 생성.</param>
        /// <param name="value">저장할 값. null이라도 저장됨.</param>
        public static void Add<T>(string key, T value) where T : class
        {
            // 일단 삭제
            Remove(key);
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 섹션에서 값 삭제
        /// </summary>
        /// <param name="key">키</param>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        /// <summary>
        /// 섹션으로부터 해당 키에 대한 존재 여부
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <returns>존재 여부</returns>
        public static bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;

            if (HttpContext.Current.Session == null)
            {
                return false;
            }
            else
            {
                return HttpContext.Current.Session[key] != null;
            }
        }
    }
}