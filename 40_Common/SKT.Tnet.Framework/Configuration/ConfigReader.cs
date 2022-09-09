using SKT.Tnet.Framework.Common;
using System;
using System.Collections.Generic;

namespace SKT.Tnet.Framework.Configuration
{
    /// <Summary>
    /// 시스템 구성 내에 지정된 섹션 및 구성 값 조회를 위한 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class ConfigReader
    {
        private static object m_Lock;
        private static Dictionary<string, ConfigSection> m_Sections;

        /// <summary>
        /// 생성자
        /// </summary>
        static ConfigReader()
        {
            m_Lock = new object();
            m_Sections = new Dictionary<string, ConfigSection>();
        }

        /// <summary>
        /// 섹션 객체를 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명</param>
        /// <returns>섹션 객체</returns>
        public static ConfigSection GetSection(string sectionName)
        {
            try
            {
                lock (m_Lock)
                {
                    if (!m_Sections.ContainsKey(sectionName))
                    {
                        ConfigSection section = System.Configuration.ConfigurationManager.GetSection(sectionName) as ConfigSection;

                        // null이라도 일단 등록. 한번만 실제로 컨피그를 읽기 위함.
                        m_Sections.Add(sectionName, section);

                        if (section == null)
                        {
                            // 로그를 남기기 위해 예외 발생
                            throw new Exception(string.Format(Properties.Resources.NotFoundConfigSection, sectionName));
                        }
                    }
                }

                return m_Sections[sectionName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Properties.Resources.NotFoundConfigSection, sectionName), ex.InnerException);
            }
        }

        /// <summary>
        ///  해당 섹션의 카테고리를 검색하여 해당 키의 값의 존재 여부를 확인
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistValue(string sectionName, string category, string key)
        {
            if (string.IsNullOrEmpty(sectionName)) sectionName = CoreContants.DEFAULT_SECTION_NAME;
            if (string.IsNullOrEmpty(category)) category = CoreContants.DEFAULT_CATEGORY_NAME;

            ConfigSection section = GetSection(sectionName);
            if (section == null)
            {
                return false;
            }
            else
            {
                return section.ExistValue(category, key);
            }
        }

        #region GetValue

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값을 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명(Default지정:"")</param>
        /// <param name="category">카테고리명(Default지정:"")</param>
        /// <param name="key">키명</param>
        /// <returns>키 값</returns>
        public static string GetValue(string sectionName, string category, string key)
        {
            if (string.IsNullOrEmpty(sectionName)) sectionName = CoreContants.DEFAULT_SECTION_NAME;
            if (string.IsNullOrEmpty(category)) category = CoreContants.DEFAULT_CATEGORY_NAME;

            ConfigSection section = GetSection(sectionName);
            if (section == null)
            {
                return null;
            }
            else
            {
                return section.GetValue(category, key);
            }
        }

        public static string GetValue(string key)
        {
            return GetValue("", "", key);
        }

        #endregion GetValue

        #region GetString

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값을 String으로 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명(Default지정:"")</param>
        /// <param name="category">카테고리명(Default지정:"")</param>
        /// <param name="key">키명</param>
        /// <returns>키 값</returns>
        public static string GetString(string sectionName, string category, string key)
        {
            return GetValue(sectionName, category, key);
        }

        public static string GetString(string key)
        {
            return GetValue("", "", key);
        }

        #endregion GetString

        #region GetInteger

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값의 Integer로 변환한 값을 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명(Default지정:"")</param>
        /// <param name="category">카테고리명(Default지정:"")</param>
        /// <param name="key">키명</param>
        /// <returns>키 값(변환 실패 시, -1 리턴)</returns>
        public static int GetInteger(string sectionName, string category, string key)
        {
            int result = -1;
            if (int.TryParse(GetValue(sectionName, category, key), out result))
                return result;

            return -1;
        }

        public static int GetInteger(string key)
        {
            return GetInteger("", "", key);
        }

        #endregion GetInteger

        #region GetDouble

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값의 Double로 변환한 값을 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명(Default지정:"")</param>
        /// <param name="category">카테고리명(Default지정:"")</param>
        /// <param name="key">키명</param>
        /// <returns>키 값(변환 실패 시, -1 리턴)</returns>
        public static double GetDouble(string sectionName, string category, string key)
        {
            double result = -1;
            if (double.TryParse(GetValue(sectionName, category, key), out result))
                return result;

            return -1;
        }

        public static double GetDouble(string key)
        {
            return GetDouble("", "", key);
        }

        #endregion GetDouble

        #region GetBoolean

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값을 Boolean으로 변환한 값을 가져온다.
        /// </summary>
        /// <param name="sectionName">섹션명(Default지정:"")</param>
        /// <param name="category">카테고리명(Default지정:"")</param>
        /// <param name="key">키명</param>
        /// <returns>키 값(변환 실패 시, 무조건 false 리턴)</returns>
        public static bool GetBoolean(string sectionName, string category, string key)
        {
            bool result = false;
            if (bool.TryParse(GetValue(sectionName, category, key), out result))
                return result;

            return false;
        }

        public static bool GetBoolean(string key)
        {
            return GetBoolean("", "", key);
        }

        #endregion GetBoolean

        #region GetLong

        /// <summary>
        /// 해당 섹션의 카테고리를 검색하여 해당 키의 값을 Long으로 변환한 값을 가져온다.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetLong(string sectionName, string category, string key)
        {
            long result = -1;
            if (long.TryParse(GetValue(sectionName, category, key), out result))
                return result;

            return -1;
        }

        public static long GetLong(string key)
        {
            return GetLong("", "", key);
        }

        #endregion GetLong
    }
}