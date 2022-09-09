using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.konantech.search.data.ParameterVO;
using com.konantech.search.data.ResultVO;
using com.konantech.search.util;
using System.Text.RegularExpressions;
using System.Text;

public partial class result_idea : System.Web.UI.UserControl
{
    public ParameterVO srchParam = new ParameterVO();

    public ResultVO rsbIdea = new ResultVO();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public string formatDateStr(string value, string deli)
    {
        return CommonUtil.formatDateStr(value, deli);
    }

    /*  하이라이팅 태그 체크
     *  @return val : true, false
     */
    public static Boolean findHighlightTag(String str)
    {

        string highlightTag = "<span class=sk_USearchTextBold>";
        
        return str.Contains(highlightTag);
    }

    public static String gridFindSearchList(Boolean title, Boolean con, Boolean attach_name, Boolean attach_con, Boolean reply, string file_names, string file_path, int cnt)
    {
        return CommonUtil.gridFindSearchList(title, con, attach_name, attach_con, reply, file_names, file_path, "q_" + cnt.ToString());
    }

    public static string TitleCut(string str)
    {
        //str = Regex.Replace(str, @"<(.|\n)*?>", string.Empty);

        str = str.Replace("&nbsp;", "");
        str = str.Replace("\r\n", "");

        //// 한글 2byte 갯수를 계산
        //int hanLength = (System.Text.Encoding.Default.GetByteCount(title) - title.Length) / 2;

        //if (System.Text.Encoding.Default.GetByteCount(title) > 690)
        //    return title.Substring(0, 690 - hanLength) + "..";
        //else
        //    return title;


        int len = 690;

        // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
        int inCnt = Encoding.Default.GetByteCount(str);
        if (inCnt > len)
        {
            int i = 0;
            for (i = str.Length - 1; inCnt > len; i--)
            {
                //ANSI 문자는 1, 이외의 문자는 2자리로 계산한다
                if (str[i] > 0x7f)
                {
                    inCnt -= 2;
                }
                else
                {
                    inCnt -= 1;
                }
            }

            // i는 마지막 문자 인덱스이고 substring 의 두번째 파라미터는 길이이기 때문에 + 1 한다.
            str = str.Substring(0, i + 2);
            // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
            inCnt = Encoding.Default.GetByteCount(str);
        }
        //PadRight(len) 이 맞겠지만 유니코드 처리가 되기 때문에 멀티바이트 문자도 1로 
        //처리되어 길이가 일정하지 않기 때문에 아래와 같이 계산하여 Padding한다
        str = str.PadRight(str.Length + len - inCnt);
        return str;
    }

    public static string FileExtImg(string FileExt)
    {
        string FileExtImg = string.Empty;

        if (FileExt == ".pptx" || FileExt == ".ppt")
        {
            FileExtImg = "ms_ppt.png";
        }
        else if (FileExt == ".docx" || FileExt == ".doc")
        {
            FileExtImg = "ms_word.png";
        }
        else if (FileExt == ".xlsx" || FileExt == ".xls")
        {
            FileExtImg = "ms_excel.png";
        }
        else if (FileExt == ".one")
        {
            FileExtImg = "ms_onenote.png";
        }
        else if (FileExt == ".pdf")
        {
            FileExtImg = "ms_pdf.png";
        }
        else
        {
            FileExtImg = "ms_pc.png";
        }

        FileExtImg = "<img src='/common/images/icon/" + FileExtImg + "' alt='' width=15 height=15 style=\"vertical-align:middle;\" />&nbsp;&nbsp;";
        
        return FileExtImg;
    }
}