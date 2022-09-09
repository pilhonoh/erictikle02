using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// Base64 기반 인코딩/디코딩 관련 Helper 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 10일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 10일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public static class Base64
    {
        public static string Encoding(string value)
        {
            string returnValue = string.Empty;

            // 값 설정: Base64 인코딩 (한글, 특수문자 등 제거)
            if (string.IsNullOrEmpty(value))
            {
                returnValue = value;
            }
            else
            {
                byte[] valueBytes = System.Text.Encoding.Default.GetBytes(value);
                returnValue = System.Convert.ToBase64String(valueBytes);
            }

            return returnValue;
        }

        public static string Decoding(string value)
        {
            string returnValue = string.Empty;

            // 값 처리
            if (string.IsNullOrEmpty(value))
            {
                returnValue = value;   // string.empty일 경우의 처리
            }
            else
            {
                byte[] valueBytes = Convert.FromBase64String(value);
                returnValue = System.Text.Encoding.Default.GetString(valueBytes);
            }

            return returnValue;
        }
    }
}
