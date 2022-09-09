using System;

namespace SKT.Tnet.Framework.Diagnostics
{
    /// <summary>
    /// 로그 저장 방식
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// NONE
        /// </summary>
        None = 0,
        /// <summary>
        /// FILE
        /// </summary>
        File = 10,
        /// <summary>
        /// DB
        /// </summary>
        DB = 20,
        /// <summary>
        /// 이벤트 로그
        /// </summary>
        EventLog = 30,
    }

    /// <summary>
    /// 로그 수준 타입
    /// </summary>
    [Serializable]
    public enum LogEntryType
    {

        /// <summary>
        /// 미정의
        /// </summary>
        Undefined = 0,
        /// <summary>
        ///  Information
        /// </summary>
        Information = 10,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 20,
        Exception = 30,
        Performance = 40,
        Audit = 50,
        SSO = 60,
        Debug = 99
    }

    /// <summary>
    /// 로그 원본 타입
    /// </summary>
    public enum LogSourceType
    {
        Undefined = 0,              // 미정의
        WebPage = 10,               // 웹 페이지
        WebControl = 20,            // 웹 컨트롤
        SharePoint = 30,            // SharePoint 관련
        ClassLibrary = 40,          // Class Library ,관련
        WebService = 50,            // 웹 서비스
        WindowService = 60,         // 윈도우 서비스
        Console = 70,               // 콘솔 관련
        TimerJob = 80,              // 타이머 잡
        SPField = 90,
        EventReceiver = 100,        // 이벤트 리시버
        WebPart = 110               // 웹 파트
    }
}