using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Runtime.InteropServices;


namespace SKT.Tnet.Framework.Configuration
{
    /// <Summary>
    /// 시스템 구성 내 별도 섹션 관리를 위한 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    [ComVisible(false)]
    public class ConfigSection : ConfigurationSection
    {
        private string m_SectionName = string.Empty;
        private Dictionary<string, Dictionary<string, string>> m_ConfigValues;

        /// <summary>
        /// 섹션명
        /// </summary>
        public string SectionName
        {
            get { return m_SectionName; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public ConfigSection()
        {
            m_ConfigValues = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        /// 카테고리를 검색하여 해당 키의 값을 가져온다.
        /// </summary>
        /// <param name="category">카테고리명</param>
        /// <param name="key">키</param>
        /// <returns>키값</returns>
        public string GetValue(string category, string key)
        {
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(key)
                && m_ConfigValues.ContainsKey(category) && m_ConfigValues[category].ContainsKey(key))
            {
                return m_ConfigValues[category][key];
            }
            else
            {
                throw new Exception(string.Format(SKT.Tnet.Framework.Properties.Resources.NotFoundCategoryOrKeyInConfigSection, this.SectionName, category, key));
            }
        }

        /// <summary>
        /// 카테고리를 검색하여 해당 키의 존재 여부를 확인 하여 준다.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistValue(string category, string key)
        {
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(key) && m_ConfigValues.ContainsKey(category) && m_ConfigValues[category].ContainsKey(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 해당 섹션 XML 정보를 읽어온다.
        /// </summary>
        /// <param name="reader">구성 파일 Xml</param>
        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            try
            {
                reader.MoveToElement();

                if (!reader.IsEmptyElement)
                {
                    Dictionary<string, string> subSectionValues = null;

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (string.IsNullOrEmpty(m_SectionName))
                            {
                                // 섹션 시작
                                m_SectionName = reader.Name;
                            }
                            else if (subSectionValues == null)
                            {
                                // 새로운 서브 섹션 시작
                                subSectionValues = new Dictionary<string, string>();
                                m_ConfigValues.Add(reader.Name, subSectionValues);
                            }
                            else
                            {
                                // add 엘리먼트 시작
                                if (reader.AttributeCount > 0)
                                {
                                    string key = null;
                                    string value = null;
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "key")
                                        {
                                            key = reader.Value;
                                        }
                                        else if (reader.Name == "value")
                                        {
                                            value = reader.Value;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                                    {
                                        subSectionValues.Add(key, value);
                                    }
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            subSectionValues = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(SKT.Tnet.Framework.Properties.Resources.ConfigDeserializeException, ex.InnerException);
            }
        }
    }
}