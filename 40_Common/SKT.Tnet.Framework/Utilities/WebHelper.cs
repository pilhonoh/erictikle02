using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// HTTP 관련 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class WebHelper
    {
        /// <summary>
        ///		시스템 구성 요소의 값들을 이용 NetworkCredential 을 생성 하여 준다.
        /// </summary>
        /// <returns></returns>
        public static NetworkCredential GetNetworkCredential()
        {
            string sCredDomain = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "Domain");
            string sCredID = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "CredID");
            string sCredPWD = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "CredPWD");

            return new NetworkCredential(sCredID, sCredPWD, sCredDomain);
        }

        /// <summary>
        ///		NetworkCredential 을 생성 하여 준다.
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="UserID"></param>
        /// <param name="UserPass"></param>
        /// <returns></returns>
        public static NetworkCredential GetNetworkCredential(string Domain, string UserID, string UserPass)
        {
            return new NetworkCredential(UserID, UserPass, Domain);
        }

        /// <summary>
        /// 타 시스템에 HTTP Request를 보내어 Request를 처리 하는 함수
        /// </summary>
        /// <param name="url">Request 대상 URL</param>
        /// <param name="lstPostData">키 쌍으로 구성된 Post할 Data</param>
        /// <param name="postEncode">euc-kr, utf-8등 Post data의 Encoding</param>
        /// <param name="responseEncode">euc-kr, utf-8등 Response의 Encoding</param>
        /// <returns></returns>
        public static string GetHttpRequestByPost(string url, Dictionary<string, string> lstPostData, string postEncode, string responseEncode, NetworkCredential networkCredential)
        {
            string sTimeOut = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "HttpRequestByPost_Timeout");

            // Web Request 생성
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.590; .NET CLR 3.5.20706; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

            // 자격증명을 지정
            if (networkCredential != null)
            {
                request.Credentials = networkCredential;
            }

            //request.UseDefaultCredentials = true;
            request.KeepAlive = true;
            request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            // TimeOut 설정
            int nResult = 3;
            if (!string.IsNullOrEmpty(sTimeOut))
            {
                if (!int.TryParse(sTimeOut, out nResult))
                {
                    nResult = 3;
                }
            }

            request.Timeout = nResult * 1000;

            // Post할 Data가 있는 경우 처리
            if (lstPostData.Count != 0)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                StringBuilder sbData = new StringBuilder();
                foreach (string postDataName in lstPostData.Keys)
                {
                    if (sbData.Length > 0)
                    {
                        sbData.Append("&");
                    }

                    sbData.Append(postDataName + "=" + HttpUtility.UrlEncode(lstPostData[postDataName], Encoding.GetEncoding(postEncode)));
                }

                byte[] buffer = UTF8Encoding.UTF8.GetBytes(sbData.ToString());

                request.ContentLength = buffer.Length;

                // 데이터 작성
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }

            string result = string.Empty;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(responseEncode)))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// SharePoint의 웹서비스에 HTTP Request를 보내어 Request를 처리 하는 함수
        /// SharePoint가 다중 인증일 경우, Windows 인증으로 처리할 수 있음. (폼인증 또는 윈도우 + 폼인증 일 경우)
        /// </summary>
        /// <param name="url">Request 대상 URL</param>
        /// <param name="contentType">HttpWebRequest ContentType (ex:text/xml; charset=utf-8)</param>
        /// <param name="SOAPActionUrl">호출할 대상 웹서비스의 SOAPActionUrl (ex:http://tempuri.org/AddSite) </param>
        /// <param name="postData">호출할 대상 웹버시스의 Parameter XML Data (SOAP 1.1)</param>
        /// <returns>return XML</returns>
        public static string GetHttpRequestByPostInMoss(string url, string contentType, string SOAPActionUrl, string postData)
        {
            string result = string.Empty;

            string sCredDomain = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "Domain");
            string sCredID = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "CredID");
            string sCredPWD = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "CredPWD");
            string sTimeOut = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "HttpRequestByPost_Timeout");

            try
            {
                // Web Request 생성
                HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = contentType;
                request.Headers["SOAPAction"] = SOAPActionUrl;
                request.Headers["X-FORMS_BASED_AUTH_ACCEPTED"] = "f";       //SharePoint의 윈도우 인증을 뚫기 위한 Header
                request.UseDefaultCredentials = false;
                request.Credentials = new NetworkCredential(sCredID, sCredPWD, sCredDomain);

                // TimeOut 설정
                int nResult = 3;
                if (!string.IsNullOrEmpty(sTimeOut))
                {
                    if (!int.TryParse(sTimeOut, out nResult))
                    {
                        nResult = 3;
                    }
                }

                //SharePoint의 작업을 하기 위해, Session Timeout을 최대한 길게 가져간다.
                request.Timeout = nResult * 1000000;

                //Parameter Data
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(postData.ToString());
                request.ContentLength = buffer.Length;

                // 데이터 작성
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}