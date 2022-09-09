using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;


namespace SKT.Common
{
    public class Utility
    {
        ///<summary>
        /// 문자열을 길이 만큼 자른다.
        ///</summary>
        ///<param name="str">문자열</param>
        ///<param name="intLen">길이</param>
        ///<returns>string</returns>	
        public static string CutString(string str, int intLen)
        {
            int intCnt = 0;
            string strSplit = "";

            if (str.ToString() == "")
                return "";

            string strTemp = str.Trim().ToString();
            char[] arrChr = strTemp.ToCharArray();

            if (strTemp.Length != 0)
            {
                for (int i = 0; i < arrChr.Length; i++)
                {
                    int intTemp = Convert.ToInt32(arrChr[i]);
                    if (intTemp < 0 || intTemp >= 128)
                    {
                        intCnt += 2;
                    }
                    else
                    {
                        intCnt += 1;
                    }

                    if (intCnt <= intLen)
                    {
                        strSplit += strTemp.Substring(i, 1);
                    }
                    else
                    {
                        strSplit += "..";
                        break;
                    }
                }
            }

            return strSplit;
        }

        public static string formatdecimalNumber(decimal iNum, int iDecimal)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalDigits = iDecimal;

            return iNum.ToString("N", nfi);
        }

        public static string formatNumber(int iNum, int iDecimal)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalDigits = iDecimal;

            return iNum.ToString("N", nfi);
        }


        /// <summary>
        /// 입력된 문자열이 null 또는 빈 문자열인지 여부를 반환합니다.
        /// </summary>
        /// <param name="strValue">확인할 문자열</param>
        /// <returns>null 여부</returns>
        public static bool IsNull(string strValue)
        {
            if (strValue == null || strValue == "")
                return true;

            return false;
        }

        /// <summary>
        /// 입력된 문자열이 null 또는 빈 문자열인지 여부를 반환합니다.
        /// </summary>
        /// <param name="strValue">확인할 문자열</param>
        /// <param name="Message">알림 메시지</param>
        /// <returns>null 여부</returns>
        public static bool IsNull(string strValue, string Message)
        {
            bool isNull = false;

            if (strValue == null || strValue == "undefined" || strValue == "")
            {
                isNull = true;
                //JavaScript.Alert(Message);
            }

            return isNull;
        }

        /// <summary>
        /// 대소문자 구별없이 oldValue를 newValue로 바꿈
        /// </summary>
        /// <param name="strRaw">원본 문자열</param>
        /// <param name="oldValue">바꿀 문자열</param>
        /// <param name="newValue">바뀔 문자열</param>
        /// <returns>string</returns>
        public static string Replace(string strValue, string oldValue, string newValue)
        {
            if (IsNull(strValue)) return "";

            return Regex.Replace(strValue, oldValue, newValue, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 개행문자를 인코딩.
        /// </summary>
        /// <param name="encodeString"></param>
        /// <returns></returns>
        public static string BREncode(string encodeString)
        {
            string strBR = encodeString;

            if (strBR.Length != 0)
            {
                if (!strBR.ToString().Substring(0, 1).Equals("<"))
                {
                    strBR = Replace(strBR, "\n", "<br>");
                    strBR = Replace(strBR, " ", "&nbsp;");
                }
            }

            return strBR;
        }

        /// <summary>
        /// 개행문자를 인코딩. (댓글 전용 메소드)
        /// </summary>
        /// <param name="encodeString"></param>
        /// <returns></returns>
        public static string BREncode2(string encodeString)
        {
            string strBR = encodeString;

            if (strBR.Length != 0)
            {
                if (!strBR.ToString().Substring(0, 1).Equals("<"))
                {
                    strBR = Replace(strBR, "\n", " <br>");
                    strBR = Replace(strBR, " ", "&nbsp;");
                }
            }

            return strBR;
        }

        /// <summary>
        /// 개행문자를 디코딩.
        /// </summary>
        /// <param name="decodeString"></param>
        /// <returns></returns>
        public static string BRDecode(string decodeString)
        {
            string strBR = decodeString;
            //return html.Replace("&","&amp;").Replace(">","&gt;").Replace("<","&lt;");
            strBR = Replace(strBR, "<br>", "\r\n");
            strBR = Replace(strBR, "&lt;br&gt;", "\r\n");
            strBR = Replace(strBR, "&nbsp;", " ");
            strBR = Replace(strBR, "&amp;nbsp;", " ");

            return strBR;
        }


        #region Object형의 NULL을 선처리한 후 문자열로 반환
        /// <summary>
        /// Object형의 NULL을 선처리한 후 문자열로 반환한다.
        /// </summary>
        /// <param name="poObj">Object</param>
        /// <returns>String형 문자열</returns>
        public static string objToStr(object poObj)
        {
            return (poObj == null ? "" : poObj.ToString());
        }
        #endregion


        #region 문자열 분리 Spilt Function
        /// <summary>
        ///  문자열 분리 Spilt Function
        /// </summary>
        /// <param name="psBuffer">대상문자열</param>
        /// <param name="psSeparator">분리 구분문자열</param>
        /// <returns>분리된 배열</returns>
        public static string[] Split(string psBuffer, string psSeparator)
        {
            int ii, jj, kk;

            for (ii = 0, jj = 0; jj < psBuffer.Length; ii++)
            {
                kk = psBuffer.IndexOf(psSeparator, jj);
                kk = kk < 0 ? psBuffer.Length : kk;
                jj = (kk + psSeparator.Length);
            }
            string[] msArray = new string[ii];

            for (ii = 0, jj = 0; jj < psBuffer.Length; ii++)
            {
                kk = psBuffer.IndexOf(psSeparator, jj);
                kk = kk < 0 ? psBuffer.Length : kk;
                msArray[ii] = psBuffer.Substring(jj, kk - jj);
                jj = (kk + psSeparator.Length);
            }
            return msArray;
        }
        #endregion

        #region 문자열 길이반환
        /// <summary>
        /// 문자열 길이반환
        /// </summary>
        /// <param name="psString">대상문자열</param>
        /// <returns>문자열길이</returns>
        public static int strLength(string psString)
        {
            byte[] maByte = System.Text.Encoding.Default.GetBytes(psString);

            return maByte.Length;
        }
        #endregion

        #region 문자열 Copy
        /// <summary>
        /// 한글을 2Byte로 인식하여 Copy한다.
        /// </summary>
        /// <param name="psString">해당문자열</param>
        /// <param name="piLength">Copy할 길이</param>
        /// <returns>해당길이 만큼의 문자열</returns>
        public static string strCopy(string psString, int piLength)
        {
            byte[] maByte = System.Text.Encoding.Default.GetBytes(psString);
            int miLength = maByte.Length < piLength ? maByte.Length : piLength;
            string msString = System.Text.Encoding.Default.GetString(maByte, 0, miLength); ;

            return msString;
        }
        #endregion

        #region Quot문자 변환처리
        /// <summary>
        ///  Quot문자 변환처리
        /// </summary>
        /// <param name="psString">해당문자열</param>
        public static string quotString(string psString)
        {
            int ii;
            string msString = "", lsCHAR;
            for (ii = 0; ii < psString.Length; ii++)
            {
                lsCHAR = psString.Substring(ii, 1);
                msString += ((lsCHAR == "\'" || lsCHAR == "\"") ? ("\\" + lsCHAR) : lsCHAR);
            }
            return msString;
        }
        #endregion

        /// <summary>
        /// HTML의 모든 테그를 제거한다.
        /// </summary>
        /// <param name="s">HTML원본 내용</param>
        /// <returns>테그가 제거된 내용</returns>
        public static string RemoveHtmlTag(string s)
        {
            /*
            int NORMAL_STATE = 0;
            int TAG_STATE = 1;
            int START_TAG_STATE = 2;
            int END_TAG_STATE = 3;
            int SINGLE_QUOT_STATE = 4;
            int DOUBLE_QUOT_STATE = 5;
            */
            int state = 0;
            int oldState = 0;

            //char[] chars = s.toCharArray();
            //StringBuffer sb = new StringBuffer();
            string sb = "";
            for (int i = 0; i < s.Length; i++)
            {
                char a = s[i];
                switch (state)
                {
                    case 0:
                        if (a == '<')
                            state = 1;
                        else
                            sb += a;
                        break;
                    case 1:
                        if (a == '>')
                            state = 0;
                        else if (a == '\"')
                        {
                            oldState = state;
                            state = 5;
                        }
                        else if (a == '\'')
                        {
                            oldState = state;
                            state = 4;
                        }
                        else if (a == '/')
                            state = 3;
                        else if (a != ' ' && a != '\t' && a != '\n' && a != '\r' && a != '\f')
                            state = 2;
                        break;
                    case 2:
                    case 3:
                        if (a == '>')
                            state = 0;
                        else if (a == '\"')
                        {
                            oldState = state;
                            state = 5;
                        }
                        else if (a == '\'')
                        {
                            oldState = state;
                            state = 4;
                        }
                        else if (a == '\"')
                            state = 5;
                        else if (a == '\'')
                            state = 4;
                        break;
                    case 5:
                        if (a == '\"')
                            state = oldState;
                        break;
                    case 4:
                        if (a == '\'')
                            state = oldState;
                        break;
                }
            }
            return sb;
        }


        //Glossary 파싱
        public static string GlossaryHtml(string s)
        {
            string txt = s;

            string pattern = "(<p.*>)(.*)(<\\/p>)";
            MatchCollection ms = Regex.Matches(txt, pattern);
            string Tag = null;

            for (int i = 0; i < ms.Count; i++)
            {
                if (ms[i].Value.Contains("h1"))
                {
                    Tag = "<h1>" + ms[i].Value.ToString().Replace("h1", "") + "</h1>";
                    s = s.Replace(ms[i].Value.ToString(), Tag);
                }
                if (ms[i].Value.Contains("*"))
                {
                    Tag = "<ul><li>" + ms[i].Value.ToString().Replace("*", "") + "</li></ul>";
                    s = s.Replace(ms[i].Value.ToString(), Tag);
                }

                if (ms[i].Value.Contains("#"))
                {
                    try
                    {
                        if (ms[i - 1].Value.Contains("#"))
                        {
                            string TagTemp = Tag;
                            Tag = Tag.Replace("</ol>", "");
                            s = s.Replace(TagTemp, Tag);
                            Tag = "<li>" + ms[i].Value.ToString().Replace("#", "") + "</li></ol>";
                            s = s.Replace(ms[i].Value.ToString(), Tag);
                        }
                        else
                        {

                            Tag = "<ol><li>" + ms[i].Value.ToString().Replace("#", "") + "</li></ol>";
                            s = s.Replace(ms[i].Value.ToString(), Tag);
                        }
                    }
                    catch (Exception ex)
                    {
                        Tag = "<ol><li>" + ms[i].Value.ToString().Replace("#", "") + "</li></ol>";
                        s = s.Replace(ms[i].Value.ToString(), Tag);
                    }
                }
            }
            return s;
        }

        //Glossary 파싱
        public static string GlossaryText(string s)
        {
            return s.Replace("h1", "").Replace("*", "").Replace("#", "");
        }

        public static string GetAttubuteIDVal(string Contents)
        {
            string strContent = "";

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);

            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//h2|//h3|//h4");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {

                    if (link.Attributes["id"] != null)
                    {
                        // HtmlAgilityPack.HtmlAttribute addid = htmlDoc.CreateAttribute("id", "#" + Guid.NewGuid().ToString());
                        //link.Attributes.Add(addid);
                        //return link.Attributes["id"].Value;
                        strContent = link.Attributes["id"].Value;
                    }
                }
                //strContent = htmlDoc.DocumentNode.OuterHtml;
            }
            return strContent;
        }

        public static string GlossaryViewH1List(string s)
        {
            string txt = s;
            //string pattern = "<[hH][1-6] .*?>.*?</[hH][1-6]>";
            //string pattern = "(<[Hh]\\d>(.*))(.*)(</[Hh]\\d>)";
            //string pattern = "((<h2.*>)(.*)(<\\/h2>))";
            //string pattern = "<[hH][1-6]>.*?</[hH][1-6]>";
            string pattern = "<[hH][1-6].*?>.*?</[hH][1-6]>";
            MatchCollection ms = Regex.Matches(txt, pattern);
            string Tag = null;
            string RemoveTag = null;
            string HtagID = null;
            try
            {
                for (int i = 0; i < ms.Count; i++)
                {
                    //HtagID = ms[i].Value.Remove(ms[i].Value.IndexOf(">")).Remove(0, (ms[i].Value.IndexOf("=") + 1));
                    //if (HtagID.Substring(0, 1) != "H")
                    //    HtagID = "";
                    HtagID = GetAttubuteIDVal(ms[i].Value);
                    

                    if (ms[i].Value.Contains("H2") || ms[i].Value.Contains("h2"))
                    {
                        //HtagID = HtagID.Remove(0, (HtagID.IndexOf("=") + 1));


                        RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                        if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                            Tag += "<li><a href=\"#" + HtagID + "\" class=\"upper\">" + RemoveTag + "</a></li>";

                        // RemoveTag.ToString().Replace("<H2>", "<li><a href=\"#\" class=\"upper\">").Replace("</H2>", "</a></li>");
                    }
                    else if (ms[i].Value.Contains("H3") || ms[i].Value.Contains("h3"))
                    {
                        if (i == 0)
                        {

                            RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                            if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                Tag += "<ol class=\"lower\"><li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                        }
                        else
                        {

                            if (ms[i - 1].Value.Contains("H3") || ms[i - 1].Value.Contains("h4") || ms[i - 1].Value.Contains("h3") || ms[i - 1].Value.Contains("H4"))
                            {
                                Tag = Tag.Remove(Tag.LastIndexOf("</ol>"));
                                if (ms[i - 1].Value.Contains("H3") || ms[i - 1].Value.Contains("h3"))
                                {
                                    RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                    if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                        Tag += "<li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                                }
                                else
                                {
                                    RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                    if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                        Tag += "</ol><li><a href=\"" + HtagID + "\">" + RemoveTag + "</H3>";
                                }
                            }
                            else
                            {
                                RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                    Tag += "<ol class=\"lower\"><li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                            }
                        }

                    }
                    else if (ms[i].Value.Contains("H4") || ms[i].Value.Contains("h4"))
                    {
                        if (i == 0)
                        {
                            RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                            if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                Tag += "<ol class=\"lowest\"><li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                        }
                        else {
                            if (ms[i - 1].Value.Contains("H4") || ms[i - 1].Value.Contains("H3") || ms[i - 1].Value.Contains("h4") || ms[i - 1].Value.Contains("h3"))
                            {
                                Tag = Tag.Remove(Tag.LastIndexOf("</ol>"));
                                if (ms[i - 1].Value.Contains("H4") || ms[i - 1].Value.Contains("h4"))
                                {
                                    RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                    if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                        Tag += "<li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                                }
                                else
                                {
                                    RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                    if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                        Tag += "<ol class=\"lowest\"><li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                                }

                            }
                            else
                            {
                                RemoveTag = StripHTMLTags(ms[i].Value).Replace("&nbsp;", "");
                                if (!string.IsNullOrEmpty(RemoveTag.Trim()))
                                    Tag += "<ol class=\"lowest\"><li><a href=\"#" + HtagID + "\">" + RemoveTag + "</a></li></ol>";
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Tag = "";
            }
            
            return Tag;
        }

        public static string StripHTMLTags(string str)
        {
            str = str.Replace("</p>", "\r\n").Replace("</ul>", "\r\n").ToString();
            return System.Text.RegularExpressions.Regex.Replace(str, @"<(.|)*?>", String.Empty).Replace("&#63;", "");;
        }


        #region Json 데이터 처리
        public  static List<Dictionary<string, object>> RowsToDictionary(DataTable table)
        {
            List<Dictionary<string, object>> objs =
                new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    drow.Add(table.Columns[i].ColumnName, dr[i]);
                }
                objs.Add(drow);
            }

            return objs;
        }

        public static Dictionary<string, object> ToJson(DataTable table)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add(table.TableName, RowsToDictionary(table));
            return d;
        }

        public static Dictionary<string, object> ToJson(DataSet data)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            foreach (DataTable table in data.Tables)
            {
                d.Add(table.TableName, RowsToDictionary(table));
            }
            return d;
        }
        #endregion


        #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 ===================================
        public static string WeeklyRemoveBackgroundColor(string str)
        {
            string tmpHtml = str.Replace("color:#000000;", "")
                                .Replace("background-color:#FFFFFF;", "")
                                .Replace("color: #000000;", "")
                                .Replace("background-color: #ffffff;", "")
                                .Replace("background: white;", "")
                                .Replace("background:white;","");

            return tmpHtml;

        }


        #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 ==============================


        #region EWS 연동 --------------------------------------------------------------------------

        /*
	         Author : 개발자- 백충기G, 리뷰자-이정선G
	         CreateDae :  2016.02.24
	         Desc :이메일 발송시 EWS연동하여 그룹에 지정된 사용자들을 가져오기 위한 연동 소스      
	    */
        /// <summary>
        /// 2016.02.02 백충기 : 이메일 형식인지 체크
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool isEmailCheck(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        #endregion //EWS 연동 ---------------------------------------------------------------------

        #region
        /// <summary>
        /// 2016-03-04
        /// Tnet Mobile 운영자 노창현
        /// Monthly Table 에 추가되는 컬럼 공통 set
        /// </summary>
        /// <param name="Delimiters"></param>
        /// <returns></returns>
        public static string monthlyDateReturn(string Delimiters)
        {
            string returnStr = string.Empty;
            
            switch (Delimiters.ToUpper())
            {
                case "YYYY":
                    returnStr = DateTime.Now.ToString("yyyy");
                    break;
                case "MM":
                    returnStr = DateTime.Now.ToString("MM").Replace("0","");    // 01, 02, 03 일 경우 0을 제외함
                    break;
                case "YYYYMM":
                    returnStr = DateTime.Now.ToString("yyyyMM");
                    break;
                default:
                    break;
            }

            return returnStr;
        }


        #endregion

    }

    #region Week of Year by DateTime
    public static class ExtensionMethods
    {
        /// <summary>
        /// Offsets to move the day of the year on a week, allowing
        /// for the current year Jan 1st day of week, and the Sun/Mon 
        /// week start difference between ISO 8601 and Microsoft
        /// </summary>
        private static int[] moveByDays = { 6, 7, 8, 9, 10, 4, 5 };
        /// <summary>
        /// Get the Week number of the year
        /// (In the range 1..53)
        /// This conforms to ISO 8601 specification for week number.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Week of year</returns>
        public static int WeekOfYear(this DateTime date)
        {
            DateTime startOfYear = new DateTime(date.Year, 1, 1);
            DateTime endOfYear = new DateTime(date.Year, 12, 31);
            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // This means that Jan 1st could be in week 51, 52, or 53 of the previous year...
            int numberDays = date.Subtract(startOfYear).Days +
                            moveByDays[(int)startOfYear.DayOfWeek];
            int weekNumber = numberDays % 7 == 6 ? (numberDays / 7) + 1 : numberDays / 7;
            switch (weekNumber)
            {
                case 0:
                    // Before start of first week of this year - in last week of previous year
                    weekNumber = WeekOfYear(startOfYear.AddDays(-1));
                    break;
                case 53:
                    // In first week of next year.
                    if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
                    {
                        weekNumber = 1;
                    }
                    break;
            }
            return weekNumber;
        }

        public static string StringMonthWeek(this DateTime date)
        {
            //기존
            //SKT Week 룰: 그 주의 일요일을 기준으로 한다.
            //DateTime thisSunday = date.AddDays(-(int)date.DayOfWeek);
            //var tmpStringDate = string.Format("{0}월 {1}주", thisSunday.Month, thisSunday.Day / 7 + 1);

            //if (tmpStringDate == System.Configuration.ConfigurationManager.AppSettings["basicWeek"])
            //{
            //    tmpStringDate = System.Configuration.ConfigurationManager.AppSettings["modifyWeek"];
            //}

            /*
             Author : 개발자- 최현미, 리뷰자-진현빈
             CreateDae : 2016.08.10
             Desc : 1. 1일이 월요일인 경우는 시작하는 월의 첫주로 보고   
                    2. 1일이 화요일인 경는 전월의 마지막주로 봅니다. 
             끌지식 http://tikle.sktelecom.com/Glossary/GlossaryView.aspx?ItemID=21387
            */

            //월요일이 주차 월의 기준으로 표현
            DateTime thisMonday = Convert.ToDateTime(date).AddDays(1 - (int)Convert.ToDateTime(date).DayOfWeek);
            //주차리턴받음
            int iWeek = GetMonthWeekly(thisMonday);
            var tmpStringDate = string.Format("{0}월 {1}주", thisMonday.Month, iWeek);

            return tmpStringDate;
        }
        //주차리턴
        public static int GetMonthWeekly(DateTime Date)
        {
            int i_Days = Date.Day;
            DateTime DateTemp = new DateTime((int)Date.Year, (int)Date.Month, 1);

            int count = 0;
            for (int i = 1; i <= i_Days; i++)  //보낸 일자 까지만
            {
                //월욜일 경우만 카운트를 한다
                if (DateTemp.DayOfWeek == DayOfWeek.Monday)
                {
                    count++;
                }
                DateTemp = DateTemp.AddDays(1);
            }

            return count;
        }



        public static string StringStartEndWeek(this DateTime date)
        {
            //int minusDay = (int)date.DayOfWeek - 1;
            //DateTime startDay = date.AddDays(-minusDay);
            //DateTime endDay = date.AddDays(4 - minusDay);
            //일~토 표시로 변경
            DateTime startDay = date.AddDays(-(int)date.DayOfWeek);
            DateTime endDay = date.AddDays(6 - (int)date.DayOfWeek);

            var stringMonthDate = StringMonthWeek(date);
            //if (stringMonthDate == System.Configuration.ConfigurationManager.AppSettings["basicWeek"])
            //{
            //    stringMonthDate = System.Configuration.ConfigurationManager.AppSettings["modifyWeek"];
            //}

            //return stringMonthDate + " " + string.Format("({0}.{1}~{2}.{3})", startDay.Month, startDay.Day, endDay.Month, endDay.Day);

            // My Weekly 2015년, 2016년 'year' 기간 반영 
            if (startDay.Year.Equals(endDay.Year))
                return stringMonthDate + " " + string.Format("('{0}.{1}.{2}~{3}.{4})", startDay.Year.ToString().Substring(2,2) , startDay.Month, startDay.Day, endDay.Month, endDay.Day);
            else
                return stringMonthDate + " " + string.Format("('{0}.{1}.{2}~'{3}.{4}.{5})", startDay.Year.ToString().Substring(2, 2), startDay.Month, startDay.Day, endDay.Year.ToString().Substring(2, 2), endDay.Month, endDay.Day);
        }

        //2016-03-04 김성환 스트링 Month 버전 추가
        public static string StringStartEndMonth(this DateTime date)
        {
            //일~토 표시로 변경
            DateTime startDay = date.AddDays(-(int)date.Month);
            DateTime endDay = date.AddDays(startDay.AddMonths(1).AddDays(-1).Day - (int)date.Month);
            var stringMonthDate = StringMonth(date);
            return stringMonthDate;
        }

        //2016-03-04 김성환 스트링 Month 버전 추가
        public static string StringMonth(this DateTime date)
        {
            //SKT Week 룰: 그 주의 일요일을 기준으로 한다.
            //DateTime thisSunday = date.AddDays(-(int)date.DayOfWeek);
            return string.Format("{0}년 {1}월",date.Year ,date.Month);
        }

        #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 ===================================
        public static string StartDay(this DateTime date)
        {

            DateTime startDay = date.AddDays(-(int)date.DayOfWeek);

            return startDay.ToString("yyyy-MM-dd");

        }

        public static string EndDay(this DateTime date)
        {

            DateTime endDay = date.AddDays(6 - (int)date.DayOfWeek);

            return endDay.ToString("yyyy-MM-dd");

        }

        #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 ==============================


        public static DateTime StartWeekDate(this DateTime date)
        {
            DateTime startDay = date.AddDays(-(int)date.DayOfWeek);

            return startDay;
        }


        public static DateTime EndWeekDate(this DateTime date)
        {

            DateTime endDay = date.AddDays(6 - (int)date.DayOfWeek);

            return endDay;
        }










    }
    #endregion
}