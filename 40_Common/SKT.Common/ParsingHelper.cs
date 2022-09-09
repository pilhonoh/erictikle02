using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

/*
 * Mostisoft
 * */

namespace SKT.Common
{
    public static class ParsingHelper
    {
        /// <summary>
        /// div나 p 와 같은 줄바꿈 문자열에 ' ' 나 혹은 아무것도 들어있지 않을 경우 &nbsp 추가.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string InsertNBSP(this string source)
        {
            string strResult = string.Empty;
            // Document Load
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(source);

            // 빈 div 태그 찾음
            HtmlNodeCollection divNodes = htmlDoc.DocumentNode.SelectNodes("//div[translate(., ' &#9;&#xA;&#xD;','') = '']");
            // 빈 div 태그 있을 때
            if (divNodes != null && divNodes.Count > 0)
            {
                // Text 노드가 있다면 &nbsp; 로 변경
                // 없다면 &nbsp; TextNode 추가.
                foreach (var item in divNodes)
                {
                    HtmlTextNode textNode = item.FirstChild as HtmlTextNode;
                    if (textNode != null)
                        textNode.Text = "&nbsp;";
                    else
                        item.AppendChild(htmlDoc.CreateTextNode("&nbsp;"));
                }
                // strResult 에 덮어씀.
                strResult = htmlDoc.DocumentNode.WriteTo();
            }
            else
            {
                strResult = source;
            }
            return strResult;
        }

        /// <summary>
        /// string 데이터를 Hash 데이터로 변환
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        //ToHashString 제거

        /// <summary>
        /// byte[] 데이터를 string으로 변환
        /// </summary>
        /// <param name="arrInput"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
