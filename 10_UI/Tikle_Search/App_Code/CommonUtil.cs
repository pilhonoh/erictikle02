using System;
using System.Text;
using com.konantech.search.data.ParameterVO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;



namespace com.konantech.search.util
{

    /// <summary>
    /// Summary description for CommonUtil
    /// </summary>
    public class CommonUtil
    {
        public CommonUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /** 입력받은 문자열특수문자를 html format으로 변환.
        *	@param str
        *	@return 변환된 문자열
        */
        public static string formatHtml(string str)
        {
            if (str.Length == 0) return "&nbsp;";

            string t = "";

            char[] arr = str.ToCharArray();
            foreach (char c in arr)
            {
                switch (c)
                {
                    case '<': t += "&lt;"; break;
                    case '>': t += "&gt;"; break;
                    case '&': t += "&amp;"; break;
                    case '\"': t += "&quot;"; break;
                    case '\'': t += "\\\'"; break;
                    case '\r': t += "<br>\n"; break;
                    case '\n': t += "<br>\n"; break;
                    default: t += c; break;
                }
            }
            return t;
        }

        /** YYYYMMDD 포멧의 문자열을 입력받아 정의한 구분자를 사용하여 YYYY.MM.DD 포멧으로 변환.
        *	@param s
        *	@param deli	
        *
        *	@return 변환된 문자열
        */

        public static string formatDateStr(string str, string deli)
        {
            string t = "";
            str = str.Trim();

            if (str != null || str.Length > 0 || !"".Equals(str))
            {

                if (str.Length >= 8)
                {
                    t = str.Substring(0, 4) + deli + str.Substring(4, 2) + deli + str.Substring(6, 2);
                }
                else
                {
                    t = str;
                }
            }

            return t;
        }

        public static string formatDateStr(string str)
        {
            string t = "";
            str = str.Trim();

            if (str != null || str.Length > 0 || !"".Equals(str))
            {

                if (str.Length >= 8)
                {
                    t = str.Substring(0, 4) + "년 " + str.Substring(4, 2) + "월 " + str.Substring(6, 2) + "일";
                }
                else
                {
                    t = str;
                }
            }

            return t;
        }


        /**
        * 문자열이 긴 경우에 입력받은 문자길이로 자른다.
        *	@param str 
        *	@param cutLen 
        *	@param tail
        *
        *	@return String
        */

        public static string getCutString(string str, int cutlen, string tail)
        {
            string temp = str;
            int tempLen = temp.Length;

            if (tempLen <= cutlen)
                return temp;

            temp = temp.Substring(0, cutlen) + tail;

            return temp;
        }

        /** 널이거나 빈 문자열을 원하는 스트링으로 변환한다<br>
        * 단, 좌우 공백이 있는 문자열은 trim 한다 <br>.
        *
        * @param org 입력문자열
        * @param converted 변환문자열
        *
        * @return 치환된 문자열
        */
        public static string null2Str(string org, string converted)
        {
            if (org == null || org.Trim().Length == 0)
            {
                return converted;
            }
            else
            {
                return org.Trim();
            }
        }


        /** 널이거나 빈 문자열(숫자형)을 integer로 변환한다.
        *
        * @param org 입력문자열
        * @param converted 변환숫자
        *
        * @return 치환된 Interger
        */
        public static int null2Int(string org, int converted)
        {
            int i = 0;


            if (org == null || org.Trim().Length == 0)
            {
                return converted;
            }
            else
            {
                try
                {
                    i = Int32.Parse(org);
                }
                catch (Exception e)
                {
                    throw (new Exception("null2Int Error : " + e.Message));
                }

                return i;
            }
        }

        public static Boolean null2Bool(string inputBool, Boolean convertedBool)
        {
            if (inputBool == null || "off".Equals(inputBool.ToLower()) || "false".Equals(inputBool.ToLower()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /** Int형 숫자의 포맷을  ###,### 으로 변환하여 리턴함.
        * @param num 정수값
        *
        * @return 변환된 문자열
        */
        public static string formatMoney(int num)
        {
            string str = "";

            str = num.ToString("#,#;0;0");

            return str;

        }

        /** string형 숫자의 포맷을  ###,### 으로 변환하여 리턴함.
        * @param num 숫자형 문자
        *
        * @return 변환된 문자열
        */
        public static string formatMoney(string num)
        {
            string str = "";

            //str = num.ToString("#,#;0;0");

            str = int.Parse(num).ToString("#,#;0;0");

            return str;

        }

        public static string makeReturnValue(string target, string str, string returnVal)
        {
            if (target.Equals(str, StringComparison.OrdinalIgnoreCase))
            {
                return returnVal;
            }
            else
            {
                return "";
            }
        }

        public static string makeReturnValue(string target, string str, string trueVal, string falseVal)
        {
            if (target.Equals(str, StringComparison.OrdinalIgnoreCase))
            {
                return trueVal;
            }
            else
            {
                return falseVal;
            }
        }

        /** 이전검색어 히든 태그 생성 후 반환.
        * 
        * @param srchParam ParameterVO 오브젝트
        * @return 이전 검색어 태그 문자열
        */

        public static string makeHtmlForPreKwd(ParameterVO srchParam)
        {
            StringBuilder preKwdStr = new StringBuilder("");
            int tmpCnt = 0;

            preKwdStr.Append("<input type='hidden' name=\"preKwds\" value=\"" + srchParam.Kwd + "\">\n");

            if (srchParam.ReSrchFlag)
            {

                System.Diagnostics.Debug.WriteLine("srchParam.getPreKwds()-->" + srchParam.PreKwds);
                if (srchParam.PreKwds != null)
                {
                    int preKwdCnt = srchParam.PreKwds.Length;

                    tmpCnt = 0;
                    if (srchParam.PreKwds[0].Equals(srchParam.Kwd) && preKwdCnt > 1) tmpCnt = 1;	// 
                    for (; tmpCnt < preKwdCnt; tmpCnt++)
                    {
                        preKwdStr.Append("<input type=\"hidden\" name=\"preKwds\" value=\"").Append(srchParam.PreKwds[tmpCnt]).Append("\">\n");
                    }

                    /* 이전검색어 & 키워드가 존재하는 경우 / 첫페이지내 검색시만 생성 / 2개 키워드가 같지 않은경우*/
                    if (srchParam.Kwd.Length > 0 && srchParam.PageNum == 1
                        && !srchParam.Kwd.Equals(srchParam.PreKwds[0]))
                    {
                        srchParam.RecKwd = srchParam.PreKwds[0] + "||" + srchParam.Kwd;    // 추천검색어 구성용 단어 생성
                    }
                } // end if
            } // end if  

            return preKwdStr.ToString();
        }

        /** 첨부파일명에 따른  이미지 파일명을 리턴함. 
        * @param fileName 파일명 
        * @return 이미지 파일명
        */
        public static string getAttachFileImage(string fileName)
        {

            string fileExt = "";
            string imgFile = "";

            //파일 확장자명 추출
            fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);

            if ("doc".Equals(fileExt, StringComparison.OrdinalIgnoreCase) || "docx".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_doc.gif";
            }
            else if ("ppt".Equals(fileExt, StringComparison.OrdinalIgnoreCase) || "pptx".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_ppt.gif";
            }
            else if ("xls".Equals(fileExt, StringComparison.OrdinalIgnoreCase) || "xlsx".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_xls.gif";
            }
            else if ("hwp".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_hwp.gif";
            }
            else if ("zip".Equals(fileExt, StringComparison.OrdinalIgnoreCase) || "gzip".Equals(fileExt, StringComparison.OrdinalIgnoreCase)
                  || "tar".Equals(fileExt, StringComparison.OrdinalIgnoreCase) || "azip".Equals(fileExt, StringComparison.OrdinalIgnoreCase)
                  || "bzip".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_zip.gif";
            }
            else if ("pdf".Equals(fileExt, StringComparison.OrdinalIgnoreCase))
            {
                imgFile = "ico_pdf.gif";
            }
            else
            {
                imgFile = "ico_etc.gif";
            }

            return imgFile;
        }

        public static string[] SplitByString(string inStr, string delimeter)
        {
            int offset = 0;
            int index = 0;
            int[] offsets = new int[inStr.Length + 1];
            while (index < inStr.Length)
            {
                int indexOf = inStr.IndexOf(delimeter, index);
                if (indexOf != -1)
                {
                    offsets[offset++] = indexOf;
                    index = (indexOf + delimeter.Length);
                }
                else
                {
                    index = inStr.Length;
                }
            }
            string[] final = new string[offset + 1];
            if (offset == 0)
            {
                final[0] = inStr;
            }
            else
            {
                offset--;
                final[0] = inStr.Substring(0, offsets[0]);
                for (int i = 0; i < offset; i++)
                {
                    final[i + 1] = inStr.Substring(offsets[i] + delimeter.Length, offsets[i + 1] - offsets[i] - delimeter.Length);
                }
                final[offset + 1] = inStr.Substring(offsets[offset] + delimeter.Length);
            }
            return final;
        }

        public static string returnChkVal(Boolean inputBool)
        {
            if (inputBool == true)
            {
                return "checked";
            }
            else
            {
                return "";
            }
        }

       

        public static string formatDate(string s)
        {


            if (s.IndexOf("-") > -1)
            {
                return (s + "-01");
            }
            else
            {
                if (s != null && s.Length > 0)
                {
                    //System.Diagnostics.Debug.WriteLine(s);
                    int d = Int32.Parse(s);
                    int yy = d / 10000;
                    int mm = (d % 10000) / 100;
                    int dd = d % 100;

                    if (yy > 2100)
                    {
                        return "9999-00";
                    }
                    else if (yy < 1970)
                    {
                        return "9999-00";
                    }


                    try
                    {

                        if (dd == 0)
                        {
                            dd = 1;


                            DateTime dt = new DateTime(yy, mm, dd);
                            return dt.ToString("yyyy-MM");
                        }
                        else
                        {
                            DateTime dt = new DateTime(yy, mm, dd);
                            return dt.ToString("yyyy-MM-dd");
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return "9999-00";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public static int getStrByteLength(string str)
        {
            string returnstr = "";
            int charcode = 0, charlen = 0;
            foreach (char c in str)
            {
                charcode = (int)c;
                if (charcode > 128)
                {
                    charlen = charlen + 2;
                }
                else
                {
                    charlen++;
                }
                
            }
            //return returnstr + tail;
            return charlen;
        }

        //검색 출처 그리기
        public static string gridFindSearchList(Boolean title, Boolean con, Boolean attach_name, Boolean attach_con, Boolean reply, string file_names, string file_path, string cnt)
        {
            string temp = "";
            string[] fileNameArray = file_names.Split('|');
            string[] filePathArray = file_path.Split('|');

            if (title || con || attach_name || attach_con || reply)
            {
                temp  = "<div class='extrasrch'>";

                temp += "<img src='./images/btn_srch1.gif' alt='검색출처' class='srch' />";

                if (title)
                    temp += "&nbsp;<img src='./images/btn_srch2.gif' alt='제목' class='srch' />";

                if (con)
                    temp += "&nbsp;<img src='./images/btn_srch3.gif' alt='본문' class='srch' />";

                if (attach_name)
                    temp += "&nbsp;<img src='./images/btn_srch4.gif' alt='첨부파일명' class='srch' />";

                if (attach_con)
                    temp += "&nbsp;<img src='./images/btn_srch5.gif' alt='첨부파일내용' class='srch' />";

                if (reply)
                    temp += "&nbsp;<img src='./images/btn_srch6.gif' alt='댓글' class='srch' />";

                if (!"".Equals(file_names.Trim()))
                    temp += "&nbsp;<a href='#' class='ico_file2' onmouseover=\"divShow('#" + cnt + "_popup_file')\" >첨부파일<span class='filebold'>[" + fileNameArray.Length + "]</span></a>";

                temp += "</div>";

                //첨부파일명 노출
                if (!"".Equals(file_names.Trim()))
                {

                    temp += "<div class='popup_file divFileInfoHidden' id='" + cnt + "_popup_file' onmouseout=\"divHidden('#" + cnt + "_popup_file')\">";

                    for (int i = 0; i < fileNameArray.Length; i++)
                    {

                        if(!"".Equals(fileNameArray[i].Trim()))
                            temp += "<a href='" + filePathArray[i] + "' class='ico_file1'>" + fileNameArray[i] + "</a>";

                        if(i < fileNameArray.Length-1)
                            temp += "<br />";
                    }

                    temp += "</div>";
                }
                    
                
            }

            return temp;
        }


        //첨부파일 확장자 추출
        public static string getExtensionFile(string fileName) {
            string temp = "";
            string extensionFile = "";

            if ("".Equals(fileName.Trim()) || fileName == null || !fileName.Contains("."))
                return fileName;

            //확장자 추출
            extensionFile = fileName.Substring(fileName.LastIndexOf(".") + 1);

            //워드
            if ("doc".Equals(extensionFile) || "docx".Equals(extensionFile))
                temp = "ico_doc";
            //엑셀
            else if("xls".Equals(extensionFile) || "xlsx".Equals(extensionFile))
                temp = "ico_xls";
            //파워포인트
            else if("ppt".Equals(extensionFile) || "pptx".Equals(extensionFile))
                temp = "ico_ppt";
            //한글
            else if("hwp".Equals(extensionFile))
                temp = "ico_hwp";
            //텍스트
            else if("txt".Equals(extensionFile))
                temp = "ico_txt";
            //훈민정음
            else if ("gul".Equals(extensionFile))
                temp = "ico_gul";
            //pdf
            else if ("pdf".Equals(extensionFile))
                temp = "ico_pdf";
            //기타
            else
                temp = "ico_etc";

            return "<img src='./imgs/" + temp + ".gif' class='extension_icon'>" + fileName;
        }
    }
}