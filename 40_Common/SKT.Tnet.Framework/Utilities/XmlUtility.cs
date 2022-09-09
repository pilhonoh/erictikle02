using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// XML 관련 도구 모음 클래스
    /// </Summary>
    /// <Remarks>
    /// - 작  성  자 : 네오플러스, 정재혁<br/>
    /// - 최초작성일 : 2015년 04월 01일<br/>
    /// - 주요변경로그<br/>
    ///   * 2015년 04월 01일 정재혁 최초작성<br/>
    /// </Remarks>
    public class XmlUtility
    {
        /// <summary>
        /// Xml 데이터에 찾을 키에 대한 값이 일치하는지 여부를 체크한다.
        /// </summary>
        /// <param name="xmlData">Xml 데이터</param>
        /// <param name="attributeName">Xml Attribute명</param>
        /// <param name="attributeValue">Xml Attribute 값</param>
        /// <returns></returns>
        public static bool IsEqualValueFromAttributeKey(string xmlData, string attributeName, string attributeValue)
        {
            bool IsEqual = false;

            if (string.IsNullOrEmpty(xmlData) == false)
            {
                TextReader textReader = null;

                try
                {
                    textReader = new StringReader(xmlData);
                    using (XmlReader xmlReader = XmlReader.Create(textReader))
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element)
                            {
                                if (xmlReader.HasAttributes)
                                {
                                    string value = xmlReader.GetAttribute(attributeName);
                                    if (!string.IsNullOrEmpty(value) && value.Equals(attributeValue, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        IsEqual = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                catch { }
                finally
                {
                    if (textReader != null) textReader.Dispose(); 
                }

            }
            

            return IsEqual;
        }

        public static T XPathValue<T>(XElement data, string key, T defaultValue)
        {
            T oRtn;

            try
            {
                oRtn = (T)System.Convert.ChangeType(data.XPathSelectElement(key).Value, typeof(T));
            }
            catch
            {
                oRtn = defaultValue;
            }

            return oRtn;
        }
    }
}