using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Configuration;
using SKT.Tnet.Framework.Configuration;

namespace SKT.Tnet.Framework.Security
{
    /// <Summary>
    /// 다른 사용자 인증(User.Identity)으로 가장 처리 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : @Shing208 <br/>
    /// # 작성일 : 2011년 07월 05일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2011년 07월 05일, @Shing208 최초작성 <br/>
    ///   - 2015년 02월 17일, impersonate 정보를 web.config참조하도록 변경처리 ,by JCW
    /// </Remarks>
    [ComVisible(false)]
    public class SafeUserToken : SafeHandle
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool CloseHandle(IntPtr handle);

        public SafeUserToken() : base(IntPtr.Zero, true) { }

        public override bool IsInvalid
        {
            get
            {
                return IntPtr.Zero.Equals(this.handle);
            }
        }

        protected override bool ReleaseHandle()
        {
            return CloseHandle(this.handle);
        }
    }

    public class Impersonation : IDisposable
    {
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_NETWORK = 3;
        public const int LOGON32_LOGON_BATCH = 4;
        public const int LOGON32_LOGON_SERVICE = 5;
        public const int LOGON32_LOGON_UNLOCK = 7;
        public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
        public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int LOGON32_PROVIDER_WINNT35 = 1;
        public const int LOGON32_PROVIDER_WINNT40 = 2;
        public const int LOGON32_PROVIDER_WINNT50 = 3;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref SafeUserToken phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(SafeUserToken hToken,
            int impersonationLevel,
            ref SafeUserToken hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        public void Dispose()
        {
            this.ImpersonationEnd();
        }

        SafeUserToken token = new SafeUserToken();
        SafeUserToken tokenDuplicate = new SafeUserToken();

        public String UserName { get; set; }
        public String Domain { get; set; }
        public String Password { get; set; }
        public Boolean Success { get; private set; }

        public Boolean ImpersonationStart()
        {
            string sDomain = string.Empty;
            string sUserID = string.Empty;
            string sUserPwd = string.Empty;

            //sDomain = Configuration.ConfigReader.GetString(SKT.Tnet.Framework.Common.CoreContants.DEFAULT_SECTION_NAME, SKT.Tnet.Framework.Common.CoreContants.DEFAULT_CATEGORY_NAME, "Domain");
            //sUserID = Configuration.ConfigReader.GetString(SKT.Tnet.Framework.Common.CoreContants.DEFAULT_SECTION_NAME, SKT.Tnet.Framework.Common.CoreContants.DEFAULT_CATEGORY_NAME, "CredID");
            //sUserPwd = Configuration.ConfigReader.GetString(SKT.Tnet.Framework.Common.CoreContants.DEFAULT_SECTION_NAME, SKT.Tnet.Framework.Common.CoreContants.DEFAULT_CATEGORY_NAME, "CredPWD");

            /*
            Author : 개발자-장찬우G, 리뷰자-이정선G 
            Create Date : 2016.02.17 
            Desc : tikle nas장비 impersonate 정보를 web.config에서 GET
            */
            sDomain = ConfigReader.GetString("ImpersonationDomain");
            sUserID = ConfigReader.GetString("ImpersonationID");
            sUserPwd = ConfigReader.GetString("ImpersonationPW");
            



            return ImpersonationStart(sDomain, sUserID, sUserPwd);
        }

        //2015-11-19 userid,userpw 변경
        //public Boolean ImpersonationStart()
        //{
        //    string sDomain = string.Empty;
        //    string sUserID = string.Empty;
        //    string sUserPwd = string.Empty;

        //    sDomain = ConfigurationManager.AppSettings["DomainName"];
        //    sUserID = ConfigurationManager.AppSettings["impersonationID"];
        //    sUserPwd = ConfigurationManager.AppSettings["impersonationPW"];

        //    return ImpersonationStart(sDomain, sUserID, sUserPwd);
        //}
        public Boolean ImpersonationStart(String domain, String userName, String password)
        {
            this.UserName = userName;
            this.Domain = domain;
            this.Password = password;
            this.Success = false;

            try{
                if (RevertToSelf())
                {
                    if (LogonUserA(UserName, Domain, Password, LOGON32_LOGON_INTERACTIVE,
                        LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            impersonationContext = WindowsIdentity.Impersonate(tokenDuplicate.DangerousGetHandle());
                            if (impersonationContext != null)
                            {
                                this.Success = true;
                            }
                        }
                    }
                }
            }
            catch(Exception e){
                throw e;
               
            }
            
            return this.Success;
        }

        public void ImpersonationEnd()
        {
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
                impersonationContext.Dispose();
                impersonationContext = null;

                if (token != null && !token.IsClosed)
                {
                    token.Close();
                    token.Dispose();
                }
                if (tokenDuplicate != null && !tokenDuplicate.IsClosed)
                {
                    tokenDuplicate.Close();
                    tokenDuplicate.Dispose();
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public Impersonation() { }

        public Impersonation(String domain, String userName, String password)
        {
            this.ImpersonationStart(domain, userName, password);
        }
        ~Impersonation()
        {
            this.ImpersonationEnd();
        }
    }
}
