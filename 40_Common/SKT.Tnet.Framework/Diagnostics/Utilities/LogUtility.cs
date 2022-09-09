using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SKT.Tnet.Framework.Diagnostics.Utilities
{
    /// <Summary>
    /// 시스템 진단을 위한 로그 Utility 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>

    public class LogUtility
    {
        #region [ ##. Exception 관련 .## ]

        /// <summary>
        /// Exception.Data에 데이터 추가.
        /// 이미 키가 있을 경우, 덮어써짐.
        /// </summary>
        /// <param name="ex">Exception 개체</param>
        /// <param name="key">키</param>
        /// <param name="value">값</param>
        public static void AddExceptionData(ref Exception ex, string key, object value)
        {
            if (ex == null || string.IsNullOrEmpty(key)) return;

            try
            {
                // 기존 값 삭제 후 추가
                if (ex.Data.Contains(key))
                    ex.Data.Remove(key);

                ex.Data.Add(key, value);
            }
            catch
            {
                // SKIP
            }
        }

        /// <summary>
        /// Exception 개체에 대한 상세한 설명을 가져온다.
        /// </summary>
        /// <param name="buffer">Exceptiong 설명을 담을 StringBuilder 개체</param>
        /// <param name="ex">Exception 객체</param>
        /// <param name="level">InnerException 레벨 수준(메소드 호출시 level를 0으로 지정한다 - 내부 로직에 따라 레벨 수준 증가함)</param>
        public static void GatherExceptionDescription(ref StringBuilder buffer, Exception ex, int level)
        {
            if (ex == null) return;

            try
            {
                // Inner Exception Header
                if (level > 0)
                {
                    buffer.AppendLine("");
                    buffer.AppendLine(string.Format("[Inner Exception #{0}]", level));
                }

                // Exception Information
                buffer.AppendLine("[Source] " + ex.GetType().FullName + "\r\n");
                buffer.AppendLine("[Message] " + ex.Message + "\r\n");

                if (!string.IsNullOrEmpty(ex.StackTrace))
                    buffer.AppendLine("[Stack Trace] " + ex.StackTrace + "\r\n");

                // Exception Data
                if (ex.Data != null && ex.Data.Count > 0)
                {
                    buffer.AppendLine("[Data] ");
                    foreach (string key in ex.Data.Keys)
                    {
                        object data = ex.Data[key];
                        if (data == null)
                        {
                            data = "{null}";
                        }
                        else if (data == DBNull.Value)
                        {
                            data = "{DBNull}";
                        }
                        buffer.AppendLine(string.Format("> {0} = [{1}]", key, data));
                    }
                }

                // Inner Exception
                if (ex.InnerException != null)
                {
                    GatherExceptionDescription(ref buffer, ex.InnerException, level + 1);
                }
            }
            catch
            {
                // SKIP
            }
        }

        /// <summary>
        /// 로그 메시지 가져오기
        /// </summary>
        /// <param name="logData">로그데이터 개체</param>
        /// <returns>로그 메시지</returns>
        public static StringBuilder GetLogMessage(LogData logData)
        {
            StringBuilder sbMsg = new StringBuilder();

            sbMsg.AppendLine("제 목 : " + logData.Message + "\n\r");
            sbMsg.AppendLine("업무 타입 : " + logData.BizType.ToString() + "\n\r");
            sbMsg.AppendLine("원본 타입 : " + logData.SourceType.ToString() + "\n\r");
            sbMsg.AppendLine("발생 날짜 : " + logData.OccurTime.ToShortDateString() + " " + logData.OccurTime.ToLongTimeString() + "\n\r");
            //sbMsg.AppendLine("실행 시간 : " + logData.ExecuteTime.ToString() + "\n\r");

            if (!string.IsNullOrEmpty(logData.RequestUser))
                sbMsg.AppendLine("요청자 이름 : " + logData.RequestUser + "\n\r");

            if (!string.IsNullOrEmpty(logData.RequestIP))
                sbMsg.AppendLine("요청자 IP : " + logData.RequestIP + "\n\r");

            if (!string.IsNullOrEmpty(logData.RequestUrl))
                sbMsg.AppendLine("요청 URL : " + logData.RequestUrl + "\n\r");

            sbMsg.AppendLine("서버 이름 : " + logData.ServerMachine + "\n\r");
            sbMsg.AppendLine("상세 설명 : " + logData.Description + "\n\r");

            StringBuilder sbXml = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(logData.GetType());
            using (StringWriter writer = new StringWriter(sbXml))
            {
                serializer.Serialize(writer, logData);
                xmlDoc.LoadXml(sbXml.ToString());
            }

            sbMsg.AppendLine("로그 XML : " + xmlDoc.OuterXml);
            sbMsg.AppendLine();

            return sbMsg;
        }

        /// <summary>
        /// 메소드명 조회
        /// </summary>
        /// <returns>메소드명</returns>
        public static string GetMethodName()
        {
            StackFrame stackFrame = new StackFrame(2, false);
            MethodBase methodBase = stackFrame.GetMethod();
            return methodBase.DeclaringType.FullName + "->" + methodBase.Name + "()";
        }

        #endregion [ ##. Exception 관련 .## ]
    }
}