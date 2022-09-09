using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace SKT.Common
{
    public class Log4NetHelper
    {
        //private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Log4NetHelper));
        private static /*readonly*/ log4net.ILog logger;
        private static void init()
        {
            //1. 로거 생성
            if (logger == null)
            {
                //1-1. web.config에 특장값으로 되어있다면 그걸 사용
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["log4net"]))
                {
                    logger = log4net.LogManager.GetLogger(ConfigurationManager.AppSettings["log4net"]);
                }
                else//1-2. 아니면 스텍을 뒤져서 호출한 곳의 NameSpace + ClassName + MethodName으로 로그 생성
                {                    
                    StackTrace stackTrace = new StackTrace();           // get call stack
                    StackFrame[] stackFrames = stackTrace.GetFrames();
                    string MethodName = stackFrames[2].GetMethod().Name;
                    string ClassFullName = stackFrames[2].GetMethod().DeclaringType.FullName;
                    logger = log4net.LogManager.GetLogger(ClassFullName + "." + MethodName);
                    //logger = log4net.LogManager.GetLogger(typeof(Log4NetHelper));
                }
            }
            //2. 로깅 환경 설정
            if (!logger.Logger.Repository.Configured)
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4netconf.xml"));
            }
        }
        public static void SetLogger(object Class)
        {
            logger = log4net.LogManager.GetLogger(Class.GetType());
        }
        public static void Info(object message)
        {
            init();
            logger.Info(message);
        }
        public static void Warn(object message)
        {
            init();
            logger.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            init();
            logger.Warn(message, exception);
        }
        public static void Error(object message)
        {
            init();
            logger.Error(message);
        }
        public static void Error(object message, Exception exception)
        {
            init();
            logger.Error(message, exception);
        }
        public static void Fatal(object message)
        {
            init();
            logger.Fatal(message);
        }
        public static void Fatal(object message, Exception exception)
        {
            init();
            logger.Fatal(message, exception);
        }

    }
}
