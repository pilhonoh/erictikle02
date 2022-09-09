using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace SKT.Common
{
    public static class CookieHelper
    {
        /// <summary>
        /// 쿠키값을 설정합니다. 
        /// </summary>
        /// <param name="strCookieName">쿠키명</param>
        /// <param name="value">쿠키값</param>
        public static void SetCookie(string strCookieName, string value)
        {
            SetCookie(strCookieName, value, DateTime.MinValue);
        }

        /// <summary>
        /// 쿠키값을 설정합니다. 
        /// </summary>
        /// <param name="strCookieName">쿠키명</param>
        /// <param name="value">쿠키값</param>
        public static void SetCookie(string strCookieName, string value, DateTime expires)
        {
            try
            {
                // Base64 인코딩
                value = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(value));

                HttpCookie cookie = new HttpCookie(strCookieName);
                cookie.Value = value;
                cookie.Path = "/Common";

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AttachFilePath"]))
                {
                    cookie.Domain = ConfigurationManager.AppSettings["AttachFilePath"].ToString();
                }

                cookie.Expires = expires;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch { }
        }

        /// <summary>
        /// 쿠키에서 값을 찾고 없으면 ""를 리턴한다.
        /// </summary>
        /// <param name="strCookieName">쿠키명</param>
        public static string GetCookie(string strCookieName)
        {
            try
            {
                HttpRequest req = null;
                HttpCookie cookie;
                req = HttpContext.Current.Request;

                if (HttpContext.Current != null)
                {
                    req = HttpContext.Current.Request;
                    cookie = req.Cookies[strCookieName];
                    cookie.HttpOnly = true;

                    // 쿠키값을 찾는다.
                    if (cookie != null)
                    {
                        // Base64 디코딩
                        try
                        {
                            return System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(cookie.Value));
                        }
                        catch
                        {
                            return cookie.Value;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch { }
            return "";
        }

        

        /// <summary>
        /// 쿠키에서 값을 찾고 없으면 ""를 리턴한다.
        /// </summary>
        /// <param name="strCookieName">쿠키명</param>
        public static string GetLangCookie()
        {
            try
            {
                HttpRequest req = null;
                HttpCookie PnetCookie;
                req = HttpContext.Current.Request;
                if (HttpContext.Current != null)
                {
                    req = HttpContext.Current.Request;
                    PnetCookie = req.Cookies["EP"];//.Values["cUSER_LANGUAGE"];
                    PnetCookie.HttpOnly = true;
                    //GWPCookie = req.Cookies["GWP_LANGUAGE_ID"];

                    // Pnet에서 쿠키값을 찾는다.
                    if (PnetCookie != null)
                    {
                        // Base64 디코딩
                        try
                        {
                            return System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(PnetCookie.Values["cUSER_LANGUAGE"]));
                        }
                        catch
                        {
                            return PnetCookie.Values["cUSER_LANGUAGE"] ?? String.Empty; ;
                        }
                    }
                    //else if (GWPCookie != null)
                    //{
                    //    // Base64 디코딩
                    //    try
                    //    {
                    //        return System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(GWPCookie.Value));
                    //    }
                    //    catch
                    //    {
                    //        return GWPCookie.Value;
                    //    }
                    //}
                    else
                    {
                        return String.Empty;
                    }
                }
            }
            catch(Exception ex) {
                ex.ToString();
            }
            return String.Empty;
        }
                    

    }
}
