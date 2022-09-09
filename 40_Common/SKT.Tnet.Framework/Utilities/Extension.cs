using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;
using System.Text.RegularExpressions;

namespace SKT.Tnet.Framework.Utilities
{
    public static partial class StringExtension
    {
        /// <summary>
        /// 숫자 여부
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        #region Validate

        #region Email

        /// <summary>
        /// 메일 형식 체크
        /// </summary>
        /// <param name="emailAddress">이메일 정보</param>
        /// <returns>true : 정상, false : 이상</returns>
        public static bool IsValidEmailAddress(this string emailAddress)
        {
            bool valid = true;
            bool isnotblank = false;

            if (string.IsNullOrEmpty(emailAddress) == true)
            {
                return false;
            }
            else
            {
                string email = emailAddress.Trim();
                if (email.Length > 0)
                {
                    isnotblank = true;
                    valid = Regex.IsMatch(email, @"\A([\w!#%&'""=`{}~\.\-\+\*\?\^\|\/\$])+@{1}\w+([-.]\w+)*\.\w+([-.]\w+)*\z", RegexOptions.IgnoreCase) &&
                        !email.StartsWith("-") &&
                        !email.StartsWith(".") &&
                        !email.EndsWith(".") &&
                        !email.Contains("..") &&
                        !email.Contains(".@") &&
                        !email.Contains("@.");
                }

                return (valid && isnotblank);
            }
        }

        /// <summary>
        /// Validates the string is an Email Address or a delimited string of email addresses...
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>bool</returns>
        public static bool IsValidEmailAddressDelimitedList(this string emailAddress, char delimiter = ';')
        {
            var valid = true;
            var isnotblank = false;

            string[] emails = emailAddress.Split(delimiter);

            foreach (string e in emails)
            {
                var email = e.Trim();
                if (email.Length > 0 && valid) // if valid == false, no reason to continue checking
                {
                    isnotblank = true;
                    if (!email.IsValidEmailAddress())
                    {
                        valid = false;
                    }
                }
            }
            return (valid && isnotblank);
        }

        #endregion

        #endregion

        /// <summary>
        /// HTML 태그를 지운다
        /// </summary>
        /// <param name="tagHTML">Body Tag 안의 Tag를 제거한다.</param>
        /// <returns>HTML 이 제거된 Tag</returns>
        public static string GetHTMLText(this string tagHTML)
        {
            string strBody = string.Empty;

            Regex r;
            Match m;

            r = new Regex(@"<body[^>]*>[\w|\t|\r|\W]*</body>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            for (m = r.Match(tagHTML); m.Success; m = m.NextMatch())
            {
                strBody += m.Value;
            }

            return Regex.Replace(strBody, "<[^>]+>", "");
        }

	//	/// <summary>
	//	/// HTML 내용의 태그 정보 삭제 처리 함수
	//	/// </summary>
	//	/// <param name="source"></param>
	//	/// <returns></returns>
	//	public static string StripHTML(this string source)
	//	{
	//		try
	//		{
	//			string result;
                
	//			result = source.Replace("\r", " ");

	//			result = result.Replace("\n", " ");
	//			result = result.Replace("\t", string.Empty);
                
	//			result = Regex.Replace(result, @"<!-- * -->", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"( )+", " ");

	//			//result = Regex.Replace(result, @"<( )*a([^>])*>.*</a>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			//result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, "(<( )*head([^>])*>).*(<( )*(/)( )*head( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			//result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);


	//			result = Regex.Replace(result, @"(<( )*script([^>])*>).*(<( )*(/)( )*script( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, @"<( )*table([^>])*>.*</table>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, @"<( )*th([^>])*>.*</th>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			//result = Regex.Replace(result, @"<( )*br( )*>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"<( )*li( )*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, @"<( )*div([^>])*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"<( )*tr([^>])*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"<( )*p([^>])*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"<( )*br( )*>", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"(</p>)", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, @"<[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, @"</[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, @" ", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                
	//			//result = Regex.Replace(result, "(\r)( )+(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\t)( )+(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\t)( )+(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)( )+(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)(\t)+(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)(\t)+", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

	//			result = Regex.Replace(result, "(\r)", "<br/>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			result = Regex.Replace(result, "(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\t)( )+(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)( )+(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)(\t)+(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	//			//result = Regex.Replace(result, "(\r)(\t)+", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);


	//			result = source.Replace("\r", " ");

	//			result = result.Replace("\n", " ");
	//			result = result.Replace("\t", string.Empty);


	//			result = Regex.Replace(result, @"(#Line )+", " ");


	//			//StringBuilder sb = new StringBuilder(result);


	//			//sb.Replace("#Line #Line #Line #Line #Line #Line #Line #Line #Line #Line", "#Line").Replace("#Line #Line #Line #Line", "#Line").Replace("#Line #Line #Line", "#Line").Replace("#Line #Line", "#Line").Replace("#Line", "<br/>");

	//			return result; 
	//		}
	//		catch
	//		{
	//			return source;
	//		}
	//	}
	//}



	       /// <summary>
        /// HTML 내용의 태그 정보 삭제 처리 함수
        /// </summary>
        /// <param name="source"></param>
        /// <param name="IsLineUse">개행 처리 여부</param>
        /// <returns></returns>
        public static string StripHTML(this string source, bool IsLineUse = true)
        {
            try
            {
                string result = source;
               
				result = Regex.Replace(result, "(<( )*head([^>])*>).*(<( )*(/)( )*head( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"(<( )*script([^>])*>).*(<( )*(/)( )*script( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, "(<( )*style([^>])*>).*(<( )*(/)( )*style( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"<( )*table([^>])*>.*</table>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"<( )*br( )*>", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"(</p>)", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"<[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"</[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, "(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, "(\n)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, "(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (IsLineUse == true)
                {
                    result = Regex.Replace(result, @"(#Line(&nbsp;)*)+", "<br/>");
                }
                else
                {
                    result = result.Replace("#Line", "");
                }

				// 아래 둘다 필요 Ascii 코드 변경
				result = Regex.Replace(result, @"( )+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				result = Regex.Replace(result, @"( )+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				//result = Regex.Replace(result, @"&nbsp;", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				return result; 
            }
            catch
            {
                return source;
            }
        }
    }





	public static class DateTimeExtension
	{
		public static string DayToDisplyText(this DateTime Target)
		{
			string returnValue = string.Empty;

			string TargetTimeStr = Target.ToString("yyyyMMdd");
			if (string.Compare(TargetTimeStr, DateTime.Now.AddMonths(-2).ToString("yyyyMMdd")) <= 0) returnValue = "오래된항목";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddMonths(-1).ToString("yyyyMMdd")) <= 0) returnValue = "한달전";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddDays(-21).ToString("yyyyMMdd")) <= 0) returnValue = "3주전";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddDays(-14).ToString("yyyyMMdd")) <= 0) returnValue = "2주전";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddDays(-7).ToString("yyyyMMdd")) <= 0) returnValue = "1주전";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddDays(-1).ToString("yyyyMMdd")) < 0)
			{
				switch (Target.DayOfWeek)
				{
					case DayOfWeek.Monday: returnValue = "월요일"; break;
					case DayOfWeek.Tuesday: returnValue = "화요일"; break;
					case DayOfWeek.Wednesday: returnValue = "수요일"; break;
					case DayOfWeek.Thursday: returnValue = "목요일"; break;
					case DayOfWeek.Friday: returnValue = "금요일"; break;
					case DayOfWeek.Saturday: returnValue = "토요일"; break;
					case DayOfWeek.Sunday: returnValue = "일요일"; break;
				}
			}
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddDays(-1).ToString("yyyyMMdd")) == 0) returnValue = "어제";
			else returnValue = "오늘";
 

			return returnValue; 
		}



		public static string DateTimeToDisplyText(this DateTime Target)
		{
			string returnValue = string.Empty;
			string TargetDayStr		= Target.ToString("yyyyMMdd");
			string TargetTimeStr	= Target.ToString("yyyyMMddHH");
			string TargetMinuteStr	= Target.ToString("yyyyMMddHHmm");

			if (string.Compare(TargetDayStr, DateTime.Now.AddDays(-1).ToString("yyyyMMdd")) == 0) returnValue = Target.ToString("yyyy-MM-dd"); 
			else if (string.Compare(TargetDayStr, DateTime.Now.AddDays(-1).ToString("yyyyMMdd")) == 0) returnValue = "어제";
			else if (string.Compare(TargetTimeStr, DateTime.Now.AddHours(-1).ToString("yyyyMMddHH")) < 0)
			{
				TimeSpan ts = Target - DateTime.Now;
				returnValue = (ts.Hours * -1).ToString() + "시간전";
			}
			else if (string.Compare(TargetMinuteStr, DateTime.Now.AddMinutes(-5).ToString("yyyyMMddHHmm")) < 0)
			{
				TimeSpan ts = Target - DateTime.Now;
				returnValue = (ts.Minutes * -1).ToString() + "분전";
			}
			else
			{
				returnValue = "조금전";
			}

			return returnValue;

		}

	}


    public static partial class GenericEx
    {
        public static T ConvertTypeEx<T>(object data, T defaultValue)
        {
            T oRtn;

            try
            {
                oRtn = (T)System.Convert.ChangeType(data, typeof(T));
            }
            catch
            {
                oRtn = defaultValue;
            }

            return oRtn;
        }
    }
}

