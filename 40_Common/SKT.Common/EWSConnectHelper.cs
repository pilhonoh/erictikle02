using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Exchange.WebServices.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Security;
using System.Configuration;
using System.DirectoryServices;


/*
 * Author : 개발자- 백충기G, 리뷰자-이정선G
 * CreateDate :  2016.02.24
 *   수정일        수정자           수정내용
 *  -------       --------    ---------------------------
 *   2016.02.02    백충기       최초작성 : 이메일 발송시 EWS연동하여 그룹에 지정된 사용자들을 가져오기 위한 연동 소스 
 */
namespace SKT.Common
{

    public partial class EWSConnectHelper
    {
        private static readonly string ServerCertificateValidationKey = "X-ExCompId"; 
        private static readonly string ServerCertificateValidationValue = "OWA_IgnoreCertErrors";
        private ExchangeService exwSvc = null;
        private Mailbox mailbox = null;


        public EWSConnectHelper(string emailAddress)
        {
            this.exwSvc = CreateExchangeService(emailAddress);
            this.mailbox = new Mailbox(emailAddress);
        }

        public static ExchangeService CreateExchangeService(string emailaddress)
        {
            /*
                Author : 개발자-최현미C, 리뷰자-진현빈D
                Create Date : 2016.10.27
                Desc : 버전 체크 안함 - Exchange 2013으로만 사용
            */
            //URL 구해오기......
            //long ldapExchVersion = GetExchVersionByEmailAddress(emailaddress);

            string ewsUrl = ConfigurationManager.AppSettings["Exch2013"];
            ExchangeVersion vt = ExchangeVersion.Exchange2013;
            string exwAdmin = ConfigurationManager.AppSettings["AdAdminId2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcct");
            string exwPwd = ConfigurationManager.AppSettings["AdAdminPW2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcctPwd");
            string exwDomin = ConfigurationManager.AppSettings["ExchangeAdminDomain"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminDomain");

            //switch (ldapExchVersion)
            //{
            //    case (88218628259840):
            //        vt = ExchangeVersion.Exchange2013;
            //        ewsUrl = ConfigurationManager.AppSettings["Exch2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeWebSvcURL");
            //        exwAdmin = ConfigurationManager.AppSettings["AdAdminId2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcct");
            //        exwPwd = ConfigurationManager.AppSettings["AdAdminPW2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcctPwd");
            //        exwDomin = ConfigurationManager.AppSettings["ExchangeAdminDomain"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminDomain");

            //        break;
            //    case (44220983382016):
            //        vt = ExchangeVersion.Exchange2010_SP2;
            //        ewsUrl = ConfigurationManager.AppSettings["Exch2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeWebSvcURL");
            //        exwAdmin = ConfigurationManager.AppSettings["AdAdminId2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcct");
            //        exwPwd = ConfigurationManager.AppSettings["AdAdminPW2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcctPwd");
            //        exwDomin = ConfigurationManager.AppSettings["ExchangeAdminDomain"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminDomain");

            //        break;
            //    case (4535486012416):
            //        vt = ExchangeVersion.Exchange2007_SP1;
            //        ewsUrl = ConfigurationManager.AppSettings["Exch2007"]; //Skcc.Configuration.SkccFxConfigManager.GetString("Exchange2007WebSvcURL");
            //        exwAdmin = ConfigurationManager.AppSettings["AdAdminId2007"]; //Skcc.Configuration.SkccFxConfigManager.GetString("Exchange2007AdminAcct");
            //        exwPwd = ConfigurationManager.AppSettings["AdAdminPW2007"]; //Skcc.Configuration.SkccFxConfigManager.GetString("Exchange2007AdminAcctPwd");
            //        exwDomin = ConfigurationManager.AppSettings["ExchangeAdminDomain"]; //Skcc.Configuration.SkccFxConfigManager.GetString("Exchange2007AdminDomain");

            //        break;
            //    default:
            //        ewsUrl = ConfigurationManager.AppSettings["Exch2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeWebSvcURL");
            //        exwAdmin = ConfigurationManager.AppSettings["AdAdminId2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcct");
            //        exwPwd = ConfigurationManager.AppSettings["AdAdminPW2013"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminAcctPwd");
            //        exwDomin = ConfigurationManager.AppSettings["ExchangeAdminDomain"]; //Skcc.Configuration.SkccFxConfigManager.GetString("ExchangeAdminDomain");

            //        break;
            //}

            ExchangeService exwSvc = new ExchangeService(vt);
            exwSvc.UseDefaultCredentials = false;

            string exwUrl = ewsUrl;
            if (string.IsNullOrEmpty(exwUrl))
            {
                throw new Exception("Exchange Service Url이 지정되지 않았습니다. (configuration/SkccFrameworkConfiguration/SkccFrameworkData/ExchangeWebSvcURL)");
            }
            exwSvc.Url = new Uri(exwUrl);
            exwSvc.HttpHeaders[ServerCertificateValidationKey] = ServerCertificateValidationValue;



            if (string.IsNullOrEmpty(exwAdmin) || string.IsNullOrEmpty(exwPwd) || string.IsNullOrEmpty(exwDomin))
            {
                throw new Exception("Exchange Service 계정이 지정되지 않았습니다. (configuration/SkccFrameworkConfiguration/SkccFrameworkData/ExchangeAdminAcct,ExchangeAdminAcctPwd,ExchangeAdminDomain)");
            }
            exwSvc.Credentials = new NetworkCredential(exwAdmin, exwPwd, exwDomin);

            return exwSvc;
        }


        public static long GetExchVersionByEmailAddress(string emailAddress)
        {
            string adminID = ConfigurationManager.AppSettings["AdAdminId"];
            string adminPW = ConfigurationManager.AppSettings["AdAdminPW"];
            string ldap = ConfigurationManager.AppSettings["Ldap"];

            long exchVersion = -1;
            using (DirectoryEntry ouEntry = new DirectoryEntry(ldap, adminID, adminPW))
            using (DirectorySearcher sch = new DirectorySearcher(ouEntry))
            {
                sch.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                sch.PageSize = 0x100;
                sch.ServerTimeLimit = new TimeSpan(0, 0, 30);
                sch.PropertiesToLoad.Clear();
                sch.PropertiesToLoad.AddRange(new string[] { "msExchVersion" });
                sch.Filter = "(&(objectCategory=person)(objectClass=user)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(proxyAddresses=smtp:" + EscapeRFC2254SpecialChars(emailAddress) + "))";

                SearchResult found = sch.FindOne();
                if (found != null)
                {
                    exchVersion = long.Parse(found.Properties["msExchVersion"][0].ToString());
                }
            }
            return exchVersion;

        }


        public List<EwsGroupUserItem> FindContactGroup(String groupName)
        {
            //그룹정보
            //List<EwsGroupUserItem> orgItems = new List<EwsGroupUserItem>();
            //NameResolutionCollection nameResolutions = exwSvc.ResolveName("IT혁신팀", ResolveNameSearchLocation.DirectoryOnly, true);

            //foreach (NameResolution nameResolution in nameResolutions)
            //{
            //    ExpandGroupResults groupResults = exwSvc.ExpandGroup(nameResolution.Mailbox.Address);
            //    foreach (EmailAddress member in groupResults.Members)
            //    {
            //        var aaa = member.Name + " <" + member.Address;
            //    }
            //}
            //return orgItems;

            List<EwsGroupUserItem> orgItems = new List<EwsGroupUserItem>();

            ItemView ItemView = new ItemView(int.MaxValue);

            //SearchFilter filter = new SearchFilter.ContainsSubstring(ContactSchema.DisplayName, groupName, ContainmentMode.Substring, ComparisonMode.IgnoreCaseAndNonSpacingCharacters);

            SearchFilter filter = new SearchFilter.IsEqualTo(ContactSchema.DisplayName, groupName);

            FolderId ContactFolder = new FolderId(WellKnownFolderName.Contacts, this.mailbox);
            
            FindItemsResults<Microsoft.Exchange.WebServices.Data.Item> fiCntResults = exwSvc.FindItems(ContactFolder, filter, ItemView);
            if (fiCntResults.Items.Count == 1)
            {
                ContactGroup contactGroup = (ContactGroup)fiCntResults.Items[0];

                contactGroup.Load();

                List<EwsGroupUserItem> listGroupEmails = new List<EwsGroupUserItem>();

                if (contactGroup != null && contactGroup.Members.Count > 0)
                {
                    ExpandGroupResults myGroupMembers = exwSvc.ExpandGroup(contactGroup.Id);

                    foreach (EmailAddress address in myGroupMembers.Members)
                    {

                        EwsGroupUserItem groupemail = new EwsGroupUserItem();

                        groupemail.EmailAddress = address.Address;
                        groupemail.DisplayName = address.Name;

                        orgItems.Add(groupemail);

                    }
                }

            }


            return orgItems;
        }




        [SecurityCritical]
        static string EscapeRFC2254SpecialChars(string s)
        {
            StringBuilder builder = new StringBuilder(s.Length);
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '(':
                        builder.Append(@"\28");
                        break;

                    case ')':
                        builder.Append(@"\29");
                        break;

                    case '*':
                        builder.Append(@"\2a");
                        break;

                    case '\\':
                        builder.Append(@"\5c");
                        break;

                    default:
                        builder.Append(ch.ToString());
                        break;
                }
            }
            return builder.ToString();
        }


        /// <summary>        
        /// 작성일 : 2016-02-02
        /// 작성자 : 백충기
        ///    설명: 개인 주소록의 그룹에 담긴 맴버들을 저장할 클래스
        /// </summary>
        [Serializable]
        public class EwsGroupUserItem
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the EmailBookMarkType class.
            /// </summary>
            public EwsGroupUserItem()
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the UserID value.
            /// </summary>
            public string UserID { get; set; }

            /// <summary>
            /// Gets or sets the EmpID value.
            /// </summary>
            public string EmpID { get; set; }

            /// <summary>
            /// Gets or sets the CompanyCode value.
            /// </summary>
            public string CompanyCode { get; set; }

            /// <summary>
            /// Gets or sets the DeptCode value.
            /// </summary>
            public string DeptCode { get; set; }

            /// <summary>
            /// Gets or sets the DeptName value.
            /// </summary>
            public string DeptName { get; set; }

            /// <summary>
            /// Gets or sets the GroupCode value.
            /// </summary>
            public string GroupCode { get; set; }

            /// <summary>
            /// Gets or sets the GroupName value.
            /// </summary>
            public string GroupName { get; set; }

            /// <summary>
            /// Gets or sets the UserName value.
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// Gets or sets the DisplayName value.
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            /// Gets or sets the EmailAddress value.
            /// </summary>
            public string EmailAddress { get; set; }


            #endregion
        }



    }
}
