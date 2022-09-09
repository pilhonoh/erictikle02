using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace SKT.Common
{
    public static class SecurityHelper
    {
        public static string Clear_XSS_CSRF(string NoneSecureText, HtmlAllowScope Scope = HtmlAllowScope.None)
        {
            string SecureText = string.Empty;

            if (HtmlAllowScope.Full == Scope)            
            {
                //HTML 태그를 전부 허용 할 경우 인코딩 없이 리턴
                return NoneSecureText;
            }
            else if (HtmlAllowScope.Limited == Scope)
            {
                //HTML 태그를 부분 적으로 허용 할 경우
                SecureText = HttpUtility.HtmlEncode(NoneSecureText);
                SecureText = SecureText.Replace("&lt;p&gt;", "<p>");
                SecureText = SecureText.Replace("&lt;/p&gt;", "</p>");
                SecureText = SecureText.Replace("&lt;P&gt;", "<P>");
                SecureText = SecureText.Replace("&lt;/P&gt;", "</P>");
                SecureText = SecureText.Replace("&lt;br&gt;", "<br>");
                SecureText = SecureText.Replace("&lt;BR&gt;", "<BR>");

            }
            else //if (HtmlAllowScope.None == Scope)
            {
                //HTML 태그를 사용하지 못하게 할 경우
                SecureText = HttpUtility.HtmlEncode(NoneSecureText);
                //SecureText = SecureText.Replace("<", "&lt;");
                //SecureText = SecureText.Replace(">", "&gt;");
                SecureText = SecureText.Replace("&#63;", "");
                SecureText = SecureText.Replace("'", "&#39;");
            }

            return SecureText;
        }


        public static string Add_XSS_CSRF(string NoneSecureText)
        {
             string SecureText = string.Empty;
                 //HTML 태그를 사용하지 못하게 할 경우
			 if (String.IsNullOrEmpty(NoneSecureText))
			 {
				 SecureText = "";
			 }
			 else 
			 {
				 SecureText = HttpUtility.HtmlDecode(NoneSecureText);
				 SecureText = SecureText.Replace("&amp;", "&");
				 SecureText = SecureText.Replace("amp;", "");
				 //SecureText = SecureText.Replace("&nbsp;", " ");
				 SecureText = SecureText.Replace("&lt;", "<");
				 SecureText = SecureText.Replace("&gt;", ">");
				 SecureText = SecureText.Replace("&quot;", "'");
                 SecureText = SecureText.Replace("&nbsp;", " ");
			 }

            return SecureText;
        }

        public static string ReClear_XSS_CSRF(string NoneSecureText)
        {
            string SecureText = NoneSecureText;

            //20140410 보안 조치 관련
            SecureText = Regex.Replace(SecureText, "<script>", "&lt;script&gt;", RegexOptions.IgnoreCase);
            SecureText = Regex.Replace(SecureText, "</script>", "&lt;&#47script&gt;", RegexOptions.IgnoreCase);
            //SecureText = Regex.Replace(SecureText, "<iframe", "&lt;iframe", RegexOptions.IgnoreCase);
            //SecureText = Regex.Replace(SecureText, "</iframe>", "&lt;&#47iframe&gt;", RegexOptions.IgnoreCase);      
            
            return SecureText;
        }



       
        public enum HtmlAllowScope
        {
            None,
            Limited,
            Full
        }

        public static string Clear_SQL_Injection(string NoneSecureQuery)
        {
            string SecureQuery = string.Empty;

            //특수문자 제거 시작—
            NoneSecureQuery = NoneSecureQuery.Replace("'", "''");
            NoneSecureQuery = NoneSecureQuery.Replace(";", "");
            NoneSecureQuery = NoneSecureQuery.Replace("--", "");
            NoneSecureQuery = NoneSecureQuery.Replace("+", "");
            NoneSecureQuery = NoneSecureQuery.Replace("%", "");
            NoneSecureQuery = NoneSecureQuery.Replace("<", "&lt;");
            NoneSecureQuery = NoneSecureQuery.Replace(">", "&gt;");
            NoneSecureQuery = NoneSecureQuery.Replace("(", "&#40;");
            NoneSecureQuery = NoneSecureQuery.Replace(")", "&#41;");
            NoneSecureQuery = NoneSecureQuery.Replace("#", "&#35;");
            NoneSecureQuery = NoneSecureQuery.Replace("&", "&#38;");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("@@variable", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("@variable", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("print", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("set", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("or", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("union", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("and", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("insert", "");
            NoneSecureQuery = NoneSecureQuery.ToLower().Replace("openrowset", "");
            //특수문자 제거 종료

            SecureQuery = NoneSecureQuery;

            return SecureQuery;
        }
    }
}
