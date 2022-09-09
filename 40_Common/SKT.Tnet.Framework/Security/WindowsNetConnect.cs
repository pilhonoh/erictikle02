using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Principal;

namespace SKT.Tnet.Framework.Security
{

    /*
    Author : 개발자-장찬우G, 리뷰자-이정선G 
    Create Date : 2016.02.17 
    Desc : 다른 사용자 인증(User.Identity)으로 가장 처리 클래스
    */
    public enum LogonType
    {

        LOGON32_LOGON_INTERACTIVE = 2,
        LOGON32_LOGON_NETWORK = 3,
        LOGON32_LOGON_BATCH = 4,
        LOGON32_LOGON_SERVICE = 5,
        LOGON32_LOGON_UNLOCK = 7,
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
        LOGON32_LOGON_NEW_CREDENTIALS = 9
    }

    /// <summary>
    /// LogonUser API에서 사용하는 로그온 프로바이더
    /// </summary>
    public enum LogonProvider
    {
        LOGON32_PROVIDER_DEFAULT = 0,
        LOGON32_PROVIDER_WINNT35 = 1,
        LOGON32_PROVIDER_WINNT40 = 2,
        LOGON32_PROVIDER_WINNT50 = 3
    }
        
    /// <summary>
    /// WIN32 Security 관련 API 들에 대한 모음
    /// </summary>
    public class WindowsNetConnect
    {
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        [DllImport("advapi32.dll", EntryPoint = "LogonUser", SetLastError = true)]
        private static extern bool _LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
        int dwLogonType, int dwLogonProvider, out int phToken);

        /// <summary>
        /// 주어진 사용자 ID로 로그온하여 액세스 토큰을 반환한다.
        /// </summary>
        /// <returns></returns>
        public static IntPtr LogonUser(string userName, string password, string domainName, LogonType logonType, LogonProvider logonProvider)
        {
            int token = 0;
            bool logonSuccess = _LogonUser(userName, domainName, password,
            (int)logonType, (int)logonProvider, out token);
            if (logonSuccess)
            {
                return new IntPtr(token);
            }
            int retval = Marshal.GetLastWin32Error();

            throw new Win32Exception(retval);

        }

        public static WindowsImpersonationContext BeginImpersonation(string userID, string password)
        {
            IntPtr token = WindowsNetConnect.LogonUser(userID, password, "."
                , LogonType.LOGON32_LOGON_NETWORK_CLEARTEXT
                , LogonProvider.LOGON32_PROVIDER_DEFAULT);

            WindowsImpersonationContext ctx = WindowsIdentity.Impersonate(token);

            return ctx;
        }

        public static WindowsImpersonationContext BeginImpersonation(string userID, string password, string domainName)
        {
            IntPtr token = WindowsNetConnect.LogonUser(userID, password, domainName
                , LogonType.LOGON32_LOGON_NETWORK_CLEARTEXT
                , LogonProvider.LOGON32_PROVIDER_DEFAULT);

            WindowsImpersonationContext ctx = WindowsIdentity.Impersonate(token);

            return ctx;
        }

        public static void EndImpersonation(WindowsImpersonationContext context)
        {
            context.Undo();
        }


    }
}
