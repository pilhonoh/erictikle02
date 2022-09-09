using System;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// Type 형태를 관리한다.
    /// </Summary>
    /// <Remarks>
    /// - 작  성  자 : 네오플러스, 정재혁<br/>
    /// - 최초작성일 : 2015년 04월 01일<br/>
    /// - 주요변경로그<br/>
    ///   * 2015년 04월 01일 정재혁 최초작성<br/>
    /// </Remarks>
    public class TypeUtility
    {
        /// <summary>
        /// 숫자인지 체크한다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="numberStyle"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value, System.Globalization.NumberStyles numberStyle)
        {
            bool bRtn = false;
            if (string.IsNullOrEmpty(value) == false)
            {
                Double result;
                bRtn = Double.TryParse(value, numberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
            }

            return bRtn;
        }
    }
}