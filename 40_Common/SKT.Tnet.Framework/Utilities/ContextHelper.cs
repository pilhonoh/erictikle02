using System;
using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// HttpContext 내용을 읽거나 내용을 추가합니다.
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class ContextHelper
    {
        /// <summary>
        /// 해당 키값에 대한 HttpContext Item Value 값을 가져온다.
        /// </summary>
        /// <param name="strKey">키값</param>
        /// <returns>Null이면 공백으로 리턴</returns>
        public static string GetHttpContext(string strKey)
        {
            string value = HttpContext.Current.Items[strKey] as string;
            if (!string.IsNullOrEmpty(value))
                return value;

            return "";
        }

        /// <summary>
        /// HttpContext를 세팅
        /// </summary>
        /// <param name="strKey">키값</param>
        /// <param name="strValue">값</param>
        public static void SetHttpContext(string strKey, string strValue)
        {
            string id = (string)HttpContext.Current.Items[strKey];
            if (string.IsNullOrEmpty(id))
            {
                HttpContext.Current.Items[strKey] = strValue;
            }
        }

        /// <summary>
        /// 클라이언트 IP 정보 취득
        /// </summary>
        /// <returns></returns>
        public static string GetUserIpAddress()
        {
            string ip = string.Empty;

            try
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip) == false)
                {
                    string[] forwardedIps = ip.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    ip = forwardedIps[forwardedIps.Length - 1];
                }
                else
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    if (ip == "::1") ip = "127.0.0.1"; // localhost
                }
            }
            catch { }

            return ip;
        }

        /// <summary>
        /// 현재 웹 페이지 정보 취득
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }
    }
}