using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Common
{
    public static class StringHelper
    {
        //요약필드에서 길이 줄이는데 사용 2바이트문자와 1바이트 문자를 고려하여 처리
        private static string ShortString(string str, int length)
        {
            char c;
            string _return;
            for (int i = 0; i < length; i++)
            {
                if (i < str.Length)
                {
                    c = str[i];
                    if (char.GetUnicodeCategory(c).ToString() == "OtherLetter") //c가 영어가 아니라면(한글이라면)
                    {
                        length -= 1;
                    }
                }
                else
                {
                    break;
                }
            }
            if (str.Length <= length)
            {
                _return = str;
            }
            else
            {
                str = str.Substring(0, length) + "..."; ;
                _return = str;
            }
            return _return;
        }

        //웹에디터를 통한 본문을 제외한 다른 입력 값에서 HTML테그를 제거하고 평문 문자열만 뽑아낼때 사용
        private static string Strip(string HTMLText)
        {
            string removeNbsp = System.Text.RegularExpressions.Regex.Replace(HTMLText, @"&nbsp;", string.Empty);
            string removeStyle = System.Text.RegularExpressions.Regex.Replace(removeNbsp, @"<STYLE(.|\n)*?</STYLE>", string.Empty);
            string removeTag = System.Text.RegularExpressions.Regex.Replace(removeStyle, @"<(.|\n)*?>", string.Empty);
            string removeNewline = System.Text.RegularExpressions.Regex.Replace(removeTag, @"\n", string.Empty);
            string removeCarriagereturn = System.Text.RegularExpressions.Regex.Replace(removeNewline, @"\r", string.Empty);

            return removeCarriagereturn;
        }
    }
}
