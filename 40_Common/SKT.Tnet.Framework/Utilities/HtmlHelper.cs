using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SKT.Tnet.Framework.Utilities
{
    public class HtmlHelper
    {
        /// <summary>
        /// HTML 내에 a 태그에서 target 속성이 없을 경우, 등록 처리
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SetAddTarget(string text)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                Regex _r = new Regex("<a (.+?)>", RegexOptions.IgnoreCase);

                if (_r.Matches(text).Count > 0)
                {
                    foreach (Match m in _r.Matches(text))
                    {
                        string Link = m.Groups[0].Value;

                        if (!Link.Contains("target"))
                        {
                            text = text.Replace(Link, string.Format("{0} target=\"_blank\">", Link.Substring(0, Link.Length - 1)));
                        }
                    }
                }
            }

            return text;
        }
    }
}
