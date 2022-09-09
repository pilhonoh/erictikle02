using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Security;
using System.DirectoryServices;
using System.IO;
using System.Web.SessionState;
using SKT.Mobile.Common.Util;
using System.Web;

namespace SKT.Mobile.EwsWrapper
{
    /// <summary>
    /// Author : 이충렬, 최규연
    /// CreateDate : 2013.08.28
    /// Desc : Exchange Web Service Managed API를 이용한 Wrapper Class
    /// </summary>
    public partial class ewsHelper
    {
        private ExchangeService service = null;
        private const string MAIL_UNREAD_FOLDER = "UnReadMailFolder";
        private const string MAIL_NONE_SUBJECT = "(제목없음)";
        private const string MAIL_UNREAD_FOLDER_NAME = "읽지 않은 메일";
        private MailPathInfo MailPathInfo;
        private string MAIL_FOLDER_DELETEDITEMS = string.Empty;
        private string MAIL_FOLDER_SENTITEMS = string.Empty;
        private string MAIL_FOLDER_JUNKEMAIL = string.Empty;
        private string MAIL_FOLDER_DRAFTS = string.Empty;
        private string MAIL_FOLDER_INBOX = string.Empty;
        private HttpSessionState Session = null;


        public ewsHelper(string mailAddress)
        {
            InitService(mailAddress);
        }

        public ewsHelper(string mailAddress, long exchVersion)
        {
            InitService(mailAddress, exchVersion);
        }

        public ewsHelper(string mailAddress, long exchVersion, HttpSessionState session)
        {
            InitService(mailAddress, exchVersion);
            Session = session;
        }

        public void SetMailPathInfo(MailPathInfo mailPathInfo)
        {
            MailPathInfo = mailPathInfo;
        }

        private void InitService(string emailAddress, long exchVersion)
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            string uri = string.Empty;
            string adminID = string.Empty;
            string adminPW = string.Empty;
            string adminDomain = string.Empty;


            string strExchageVer = string.Empty;
            ExchangeVersion exchangeVer;

            if (exchVersion == 0)
            {
                exchVersion = GetExchVersionByEmailAddress(emailAddress);
            }

            if (exchVersion == 4535486012416)
            {
                uri = ConfigurationManager.AppSettings["Exch2007"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2007"];
                adminID = ConfigurationManager.AppSettings["EwsAdminID2007"];
                adminPW = ConfigurationManager.AppSettings["EwsAdminPW2007"];
                adminDomain = ConfigurationManager.AppSettings["EwsAdminDomain2007"];
            }
            else if (exchVersion == 44220983382016)
            {
                uri = ConfigurationManager.AppSettings["Exch2010"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2010"];
                adminID = ConfigurationManager.AppSettings["EwsAdminID2010"];
                adminPW = ConfigurationManager.AppSettings["EwsAdminPW2010"];
                adminDomain = ConfigurationManager.AppSettings["EwsAdminDomain2010"];
            }
            else if (exchVersion == 88218628259840)
            {
                uri = ConfigurationManager.AppSettings["Exch2013"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2013"];
                adminID = ConfigurationManager.AppSettings["EwsAdminID2013"];
                adminPW = ConfigurationManager.AppSettings["EwsAdminPW2013"];
                adminDomain = ConfigurationManager.AppSettings["EwsAdminDomain2013"];
            }
            else if (exchVersion == -1)
            {
                throw new Exception("User[" + emailAddress + "] not found.");
            }
            else
            {
                throw new Exception("msExchVersion=" + exchVersion.ToString());
            }

            exchangeVer = (ExchangeVersion)Enum.Parse(typeof(ExchangeVersion), strExchageVer, true);

            service = new ExchangeService(exchangeVer);


            service.Credentials = new System.Net.NetworkCredential(adminID, adminPW, adminDomain);
            service.Url = new Uri(uri);
            //service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, emailAddress);

            this.CurrentEwsInfo = new EwsInfo();
            this.CurrentEwsInfo.AdminDomain = adminDomain;
            this.CurrentEwsInfo.AdminID = adminID;
            this.CurrentEwsInfo.AdminPW = adminPW;
            this.CurrentEwsInfo.EwsURL = uri;
            this.CurrentEwsInfo.CurrentEmailAddress = emailAddress;
        }

        private void InitService(string mailAddress)
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            string uri = string.Empty;
            string adminID = string.Empty;
            string adminPW = string.Empty;
            string adminDomain = string.Empty;
            string strExchageVer = string.Empty;
            ExchangeVersion exchangeVer;
            long exchVersion = GetExchVersionByEmailAddress(mailAddress);

            if (exchVersion == 4535486012416)
            {
                uri = ConfigurationManager.AppSettings["Exch2007"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2007"];
                adminID = ConfigurationManager.AppSettings["EwsAdminID2007"];
                adminPW = ConfigurationManager.AppSettings["EwsAdminPW2007"];
                adminDomain = ConfigurationManager.AppSettings["EwsAdminDomain2007"];
            }
            else if (exchVersion == 44220983382016)
            {
                uri = ConfigurationManager.AppSettings["Exch2010"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2010"];
            }
            else if (exchVersion == 88218628259840)
            {
                uri = ConfigurationManager.AppSettings["Exch2013"];
                strExchageVer = ConfigurationManager.AppSettings["ExchVer2013"];
                adminID = ConfigurationManager.AppSettings["EwsAdminID2013"];
                adminPW = ConfigurationManager.AppSettings["EwsAdminPW2013"];
                adminDomain = ConfigurationManager.AppSettings["EwsAdminDomain2013"];

            }
            else if (exchVersion == -1)
            {
                throw new Exception("User[" + mailAddress + "] not found.");
            }
            else
            {
                throw new Exception("msExchVersion=" + exchVersion.ToString());
            }

            exchangeVer = (ExchangeVersion)Enum.Parse(typeof(ExchangeVersion), strExchageVer, true);

            service = new ExchangeService(exchangeVer);
            service.Credentials = new System.Net.NetworkCredential(adminID, adminPW, adminDomain);
            service.Url = new Uri(uri);
            service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, mailAddress);

            this.CurrentEwsInfo = new EwsInfo();
            this.CurrentEwsInfo.AdminDomain = adminDomain;
            this.CurrentEwsInfo.AdminID = adminID;
            this.CurrentEwsInfo.AdminPW = adminPW;
            this.CurrentEwsInfo.EwsURL = uri;
            this.CurrentEwsInfo.CurrentEmailAddress = mailAddress;

        }
        long GetExchVersionByEmailAddress(string emailAddress)
        {
            string adminID = ConfigurationManager.AppSettings["AdAdminID"];
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

        public void SetWellKnownFolderList()
        {
            MAIL_FOLDER_DELETEDITEMS = GetWellKnownFolderID(WellKnownFolderName.DeletedItems);
            MAIL_FOLDER_SENTITEMS = GetWellKnownFolderID(WellKnownFolderName.SentItems);
            MAIL_FOLDER_JUNKEMAIL = GetWellKnownFolderID(WellKnownFolderName.JunkEmail);
            MAIL_FOLDER_DRAFTS = GetWellKnownFolderID(WellKnownFolderName.Drafts);
            MAIL_FOLDER_INBOX = GetWellKnownFolderID(WellKnownFolderName.Inbox);
        }

        public string GetWellKnownFolderID(string key, WellKnownFolderName wkf)
        {
            string retVal = string.Empty;
            try
            {
                retVal = GetFolderID(wkf);
                Session[key] = retVal;
            }
            catch
            {
                retVal = GetFolderID(wkf);

            }
            return retVal;
        }

        public string GetWellKnownFolderID(WellKnownFolderName wkf)
        {
            string retVal = string.Empty;
            switch (wkf)
            {
                case WellKnownFolderName.DeletedItems:
                    retVal = GetWellKnownFolderID("EWSHELPER_WELLKNOWNFOLDER_DELETEDITEMS", WellKnownFolderName.DeletedItems);
                    break;
                case WellKnownFolderName.SentItems:
                    retVal = GetWellKnownFolderID("EWSHELPER_WELLKNOWNFOLDER_SENTITMES", WellKnownFolderName.SentItems);
                    break;
                case WellKnownFolderName.Drafts:
                    retVal = GetWellKnownFolderID("EWSHELPER_WELLKNOWNFOLDER_DRAFTS", WellKnownFolderName.Drafts);
                    break;
                case WellKnownFolderName.Inbox:
                    retVal = GetWellKnownFolderID("EWSHELPER_WELLKNOWNFOLDER_INBOX", WellKnownFolderName.Inbox);
                    break;
                case WellKnownFolderName.JunkEmail:
                    retVal = GetWellKnownFolderID("EWSHELPER_WELLKNOWNFOLDER_JUNKEMAIL", WellKnownFolderName.JunkEmail);
                    break;
                default:
                    break;
            }

            return retVal;
        }

        public string GetWellKnownFolderName(string fid)
        {
            string retVal = string.Empty;
            if (fid == GetWellKnownFolderID(WellKnownFolderName.DeletedItems)) retVal = "DELETEDITEMS";
            else if (fid == GetWellKnownFolderID(WellKnownFolderName.SentItems)) retVal = "SENTITEMS";
            else if (fid == GetWellKnownFolderID(WellKnownFolderName.JunkEmail)) retVal = "JUNKEMAIL";
            else if (fid == GetWellKnownFolderID(WellKnownFolderName.Drafts)) retVal = "DRAFTS";
            else if (fid == GetWellKnownFolderID(WellKnownFolderName.Inbox)) retVal = "INBOX";

            return retVal;
        }

        public string GetFolderID(WellKnownFolderName wkf)
        {
            Folder folder = Folder.Bind(service, wkf);
            return folder.Id.UniqueId;
        }

        public string GetFolderName(string fid)
        {
            Folder folder = Folder.Bind(service, fid);
            return folder.DisplayName;
        }

        public Int32 GetUnreadMailCountQuickley()
        {
            int unreadCount = 0;

            FolderView fv = new FolderView(Int32.MaxValue) { Traversal = FolderTraversal.Deep };
            fv.PropertySet = new PropertySet(FolderSchema.Id);
            fv.PropertySet.Add(FolderSchema.UnreadCount);
            fv.PropertySet.Add(FolderSchema.FolderClass);

            FindFoldersResults findFoldersResults = service.FindFolders(WellKnownFolderName.Inbox, fv);

            unreadCount += Folder.Bind(service, WellKnownFolderName.Inbox).UnreadCount;

            foreach (Folder folder in findFoldersResults.Folders)
            {
                if (folder.FolderClass == null || folder.FolderClass.ToUpper() == "IPF.NOTE")
                {
                    unreadCount += folder.UnreadCount;
                }
            }
            return unreadCount;
        }

        public List<FolderItem> GetChildFolderList(string parentid)
        {
            List<FolderItem> listfolder = new List<FolderItem>();


            FolderView fv = new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep };
            fv.PropertySet = new PropertySet(FolderSchema.Id);
            fv.PropertySet.Add(FolderSchema.DisplayName);
            fv.PropertySet.Add(FolderSchema.ParentFolderId);
            fv.PropertySet.Add(FolderSchema.ChildFolderCount);
            fv.PropertySet.Add(FolderSchema.UnreadCount);
            fv.PropertySet.Add(FolderSchema.TotalCount);
            fv.PropertySet.Add(FolderSchema.FolderClass);

            FindFoldersResults findFoldersResults = service.FindFolders(new FolderId(parentid), fv);

            if (findFoldersResults.Folders.Count > 0)
            {
                foreach (Folder folder in findFoldersResults)
                {
                    if (folder.FolderClass == null || folder.FolderClass.ToUpper() == "IPF.NOTE")
                    {
                        FolderItem folderItem = new FolderItem();
                        folderItem.DisplayName = folder.DisplayName;
                        folderItem.UniqueId = folder.Id.UniqueId;
                        folderItem.ParentUniqueId = folder.ParentFolderId.UniqueId;
                        folderItem.UnReadMailCount = folder.UnreadCount;
                        folderItem.TotalMailCount = folder.TotalCount;
                        folderItem.ChildFolderCount = folder.ChildFolderCount;
                        folderItem.FolderClass = folder.FolderClass;
                        listfolder.Add(folderItem);

                    }
                }
            }

            return listfolder;
        }

        public List<FolderItem> GetFolderList()
        {
            Log4NetHelper.StartTime("GetFolderList - Start");

            ExchangeVersion exVer = service.RequestedServerVersion;

            List<FolderItem> listfolder = new List<FolderItem>();
            int UnReadTotalCount = 0;

            //if (string.IsNullOrEmpty(baseFolder))
            //{
            //    baseFolder = GetWellKnownFolderID(WellKnownFolderName.Inbox);
            //}

            FolderView fv = new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep };
            fv.PropertySet = new PropertySet(FolderSchema.Id);
            fv.PropertySet.Add(FolderSchema.DisplayName);
            fv.PropertySet.Add(FolderSchema.ParentFolderId);
            fv.PropertySet.Add(FolderSchema.ChildFolderCount);
            fv.PropertySet.Add(FolderSchema.UnreadCount);
            fv.PropertySet.Add(FolderSchema.TotalCount);
            fv.PropertySet.Add(FolderSchema.FolderClass);

            //if (exVer != ExchangeVersion.Exchange2007_SP1)
            //    fv.PropertySet.Add(FolderSchema.WellKnownFolderName);
            //0x36D9

            //2007일때 WellKnownFolder를 가져올수 없음, 따로 가져오는 방법
            //if (exVer == ExchangeVersion.Exchange2007_SP1)
            SetWellKnownFolderList();

            FindFoldersResults findFoldersResults = service.FindFolders(WellKnownFolderName.MsgFolderRoot, fv);

            List<string> needToRemoveFolder = new List<string>();
            //bool checkBase = false;
            foreach (Folder folder in findFoldersResults.Folders)
            {
                if (folder.FolderClass == null || folder.FolderClass.ToUpper() == "IPF.NOTE")
                {
                    //if (exVer != ExchangeVersion.Exchange2007_SP1)
                    //{
                    //    PropertySet ps = new PropertySet(FolderSchema.WellKnownFolderName);
                    //    folder.Load(ps);
                    //}

                    if (needToRemoveFolder.Contains(folder.ParentFolderId.ToString()))
                    {
                        needToRemoveFolder.Add(folder.Id.ToString());
                        continue;
                    }

                    FolderItem folderItem = new FolderItem();
                    folderItem.DisplayName = folder.DisplayName;
                    folderItem.UniqueId = folder.Id.UniqueId;
                    folderItem.ParentUniqueId = folder.ParentFolderId.UniqueId;
                    folderItem.UnReadMailCount = folder.UnreadCount;
                    folderItem.TotalMailCount = folder.TotalCount;
                    folderItem.ChildFolderCount = folder.ChildFolderCount;
                    folderItem.FolderClass = folder.FolderClass;
                    //if (exVer == ExchangeVersion.Exchange2007_SP1)
                    folderItem.WellKnownFolderName = GetWellKnownFolderName(folder.Id.UniqueId);
                    //else
                    //{
                    //    if (folder.WellKnownFolderName != null)
                    //        folderItem.WellKnownFolderName = folder.WellKnownFolderName.ToString();
                    //}
                    //if (folder.Id.UniqueId == baseFolder)
                    //{
                    //    folderItem.IsBase = true;
                    //    checkBase = true;
                    //}
                    listfolder.Add(folderItem);

                    UnReadTotalCount += folder.UnreadCount;
                }
                else
                {
                    if (folder.DisplayName.ToUpper().Contains("RSS") && folder.FolderClass.Equals("IPF.Note.OutlookHomepage"))
                        needToRemoveFolder.Add(folder.Id.ToString());
                }
            }


            //읽지 않은 메일함 추가

            FolderItem unReadFolder = new FolderItem();
            unReadFolder.DisplayName = MAIL_UNREAD_FOLDER_NAME;
            unReadFolder.UniqueId = MAIL_UNREAD_FOLDER;
            unReadFolder.UnReadMailCount = UnReadTotalCount;
            //if (baseFolder == MAIL_UNREAD_FOLDER)
            //{
            //    unReadFolder.IsBase = true;
            //    checkBase = true;
            //}

            //if (!checkBase)
            //{
            //    foreach (FolderItem fitem in listfolder)
            //    {
            //        if (fitem.UniqueId == GetWellKnownFolderID(WellKnownFolderName.Inbox))
            //        {
            //            fitem.IsBase = true;
            //            break;
            //        }
            //    }
            //}

            listfolder.Add(unReadFolder);

            return listfolder;
        }

        public List<FolderItem> GetFolderList(string baseFolder)
        {
            List<FolderItem> listfolder = new List<FolderItem>();
            int UnReadTotalCount = 0;

            if (string.IsNullOrEmpty(baseFolder))
            {
                baseFolder = GetWellKnownFolderID(WellKnownFolderName.Inbox);
            }

            FolderView fv = new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep };
            fv.PropertySet = new PropertySet(FolderSchema.Id);
            fv.PropertySet.Add(FolderSchema.DisplayName);
            fv.PropertySet.Add(FolderSchema.ParentFolderId);
            fv.PropertySet.Add(FolderSchema.ChildFolderCount);
            fv.PropertySet.Add(FolderSchema.UnreadCount);
            fv.PropertySet.Add(FolderSchema.TotalCount);
            fv.PropertySet.Add(FolderSchema.FolderClass);
            //SetWellKnownFolderList();

            FindFoldersResults findFoldersResults = service.FindFolders(WellKnownFolderName.MsgFolderRoot, fv);

            bool checkBase = false;
            foreach (Folder folder in findFoldersResults.Folders)
            {
                if (folder.FolderClass == null || folder.FolderClass.ToUpper() == "IPF.NOTE")
                {

                    FolderItem folderItem = new FolderItem();
                    folderItem.DisplayName = folder.DisplayName;
                    folderItem.UniqueId = folder.Id.UniqueId;
                    folderItem.ParentUniqueId = folder.ParentFolderId.UniqueId;
                    folderItem.UnReadMailCount = folder.UnreadCount;
                    folderItem.TotalMailCount = folder.TotalCount;
                    folderItem.ChildFolderCount = folder.ChildFolderCount;
                    folderItem.FolderClass = folder.FolderClass;
                    folderItem.WellKnownFolderName = GetWellKnownFolderName(folder.Id.UniqueId);
                    if (folder.Id.UniqueId == baseFolder)
                    {
                        folderItem.IsBase = true;
                        checkBase = true;
                    }
                    listfolder.Add(folderItem);

                    UnReadTotalCount += folder.UnreadCount;
                }
            }


            //읽지 않은 메일함 추가

            FolderItem unReadFolder = new FolderItem();
            unReadFolder.DisplayName = MAIL_UNREAD_FOLDER_NAME;
            unReadFolder.UniqueId = MAIL_UNREAD_FOLDER;
            unReadFolder.UnReadMailCount = UnReadTotalCount;
            if (baseFolder == MAIL_UNREAD_FOLDER)
            {
                unReadFolder.IsBase = true;
                checkBase = true;
            }

            if (!checkBase)
            {
                foreach (FolderItem fitem in listfolder)
                {
                    if (fitem.UniqueId == GetWellKnownFolderID(WellKnownFolderName.Inbox))
                    {
                        fitem.IsBase = true;
                        break;
                    }
                }
            }

            listfolder.Add(unReadFolder);

            return listfolder;
        }

        public List<FolderItem> GetFolderUnReadCount(List<string> fidList)
        {
            List<FolderItem> folderList = new List<FolderItem>();

            foreach (string fid in fidList)
            {
                FolderItem folderItem = new FolderItem();
                Folder folder = Folder.Bind(service, fid);

                folderItem.UniqueId = folder.Id.UniqueId;
                folderItem.UnReadMailCount = folder.UnreadCount;

                folderList.Add(folderItem);

            }
            return folderList;
        }

        public MailItemList GetMailItemListByMain(FolderId fid)
        {
            int totalCount = 0;
            string wellKnownFolderName = string.Empty;
            MailItemList mailList = new MailItemList();
            List<MailItem> listitem = new List<MailItem>();
            FindItemsResults<Item> findResults = null;

            string sentItemsFolderId = GetWellKnownFolderID(WellKnownFolderName.SentItems);
            string draftsFolderId = GetWellKnownFolderID(WellKnownFolderName.Drafts);

            Folder folder = Folder.Bind(service, fid);

            // 여기는 전체카운트대신 읽지않은 메일수만 넘긴다.
            totalCount = folder.UnreadCount;

            wellKnownFolderName = GetWellKnownFolderName(folder.Id.UniqueId);

            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();

            ItemView iv = new ItemView(Int32.MaxValue);

            iv.PropertySet = new PropertySet();
            iv.PropertySet.Add(EmailMessageSchema.Subject);
            iv.PropertySet.Add(EmailMessageSchema.DateTimeSent);
            iv.PropertySet.Add(EmailMessageSchema.DateTimeReceived);
            iv.PropertySet.Add(EmailMessageSchema.From);
            iv.PropertySet.Add(EmailMessageSchema.IsRead);
            iv.PropertySet.Add(EmailMessageSchema.HasAttachments);
            iv.PropertySet.Add(EmailMessageSchema.Importance);
            iv.PropertySet.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
            iv.PropertySet.Add(new ExtendedPropertyDefinition(0x1080, MapiPropertyType.Integer));

            findResults = service.FindItems(fid, iv);

            foreach (Item item in findResults.Items)
            {
                if (item is EmailMessage)
                {
                    EmailMessage msg = (EmailMessage)item;
                    MailItem mi = new MailItem();
                    if (!string.IsNullOrEmpty(msg.Subject))
                        mi.Subject = msg.Subject;
                    else
                        mi.Subject = MAIL_NONE_SUBJECT;
                    if (msg.From != null)
                    {
                        mi.Sender = GetFrom(msg.From);
                    }
                    mi.IsRead = msg.IsRead;
                    mi.FolderID = fid.ToString();
                    mi.HasAttachments = msg.HasAttachments;
                    mi.DateTimeSent = msg.DateTimeSent;
                    if (wellKnownFolderName == "SENTITEMS" || wellKnownFolderName == "DRAFTS")
                        mi.DateTimeForList = msg.DateTimeSent;
                    else
                        mi.DateTimeForList = msg.DateTimeReceived;
                    mi.Importance = (ImportanceLevel)msg.Importance;
                    foreach (ExtendedProperty exprop in msg.ExtendedProperties)
                    {
                        if (exprop.PropertyDefinition.Tag == 0x300B)
                        {
                            byte[] bkeys = exprop.Value as byte[];
                            mi.Pr_Search_Key = Convert.ToBase64String(bkeys);
                        }
                        else if (exprop.PropertyDefinition.Tag == 0x1080)
                        {
                            int? value = exprop.Value as int?;
                            mi.Pr_Icon_Index = value;
                        }
                    }
                    listitem.Add(mi);
                }
            }
            mailList.MailList = listitem;
            mailList.TotalCount = totalCount;
            mailList.WellknownFolderName = wellKnownFolderName;
            mailList.CurrentFolderID = fid.ToString();

            return mailList;
        }

        public MailItemList GetMailItemList(string fid, int offset, int pageSize, string searchText, bool searchIsRead, DateTime searchTime)
        {
            MailItemList mailList = new MailItemList();
            List<MailItem> listitem = new List<MailItem>();

            bool isInbox = false;
            FolderId folderId = null;
            string wellKnownFolderName = string.Empty;
            int totalCount = 0;

            if (string.IsNullOrEmpty(fid))
            {
                isInbox = true;
                fid = GetWellKnownFolderID(WellKnownFolderName.Inbox);
            }
            else
            {
                //isInbox = false;
                folderId = new FolderId(fid);
            }

            FindItemsResults<Item> findResults = null;

            string sentItemsFolderId = GetWellKnownFolderID(WellKnownFolderName.SentItems);
            string draftsFolderId = GetWellKnownFolderID(WellKnownFolderName.Drafts);
            Folder folder = null;
            if (isInbox)
                folder = Folder.Bind(service, WellKnownFolderName.Inbox);
            else
                folder = Folder.Bind(service, fid);
            mailList.CurrentFolderDisplayName = folder.DisplayName;

            wellKnownFolderName = GetWellKnownFolderName(folder.Id.UniqueId);

            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<SearchFilter> searchOrFilterCollection = new List<SearchFilter>();
                ExtendedPropertyDefinition propSender = new ExtendedPropertyDefinition(0x0C1A, MapiPropertyType.String);
                ExtendedPropertyDefinition propBcc = new ExtendedPropertyDefinition(0x0E02, MapiPropertyType.String);
                ExtendedPropertyDefinition propCc = new ExtendedPropertyDefinition(0x0E03, MapiPropertyType.String);
                ExtendedPropertyDefinition propTo = new ExtendedPropertyDefinition(0x0E04, MapiPropertyType.String);

                searchOrFilterCollection.Add(new SearchFilter.ContainsSubstring(EmailMessageSchema.Subject, searchText));
                searchOrFilterCollection.Add(new SearchFilter.ContainsSubstring(propSender, searchText));
                searchOrFilterCollection.Add(new SearchFilter.ContainsSubstring(propBcc, searchText));
                searchOrFilterCollection.Add(new SearchFilter.ContainsSubstring(propCc, searchText));
                searchOrFilterCollection.Add(new SearchFilter.ContainsSubstring(propTo, searchText));

                searchFilterCollection.Add(new SearchFilter.SearchFilterCollection(LogicalOperator.Or, searchOrFilterCollection.ToArray()));
            }
            if (searchIsRead)
            {
                searchFilterCollection.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
            }
            //2015-04-24 이전이 아닌 이후 검색을 하고 싶다.
            //if (offset > 0)
            //{
                //if (wellKnownFolderName == "SENTITEMS" || wellKnownFolderName == "DRAFTS")
                //    searchFilterCollection.Add(new SearchFilter.IsLessThan(EmailMessageSchema.DateTimeSent, searchTime));
                //else
                //    searchFilterCollection.Add(new SearchFilter.IsLessThan(EmailMessageSchema.DateTimeReceived, searchTime));
            //}
            if (wellKnownFolderName == "SENTITEMS" || wellKnownFolderName == "DRAFTS")
                searchFilterCollection.Add(new SearchFilter.IsGreaterThan(EmailMessageSchema.DateTimeSent, searchTime));
            else
                searchFilterCollection.Add(new SearchFilter.IsGreaterThan(EmailMessageSchema.DateTimeReceived, searchTime));

            if (isInbox)
            {
                if (searchFilterCollection.Count > 0)
                {
                    SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());
                    totalCount = service.FindItems(WellKnownFolderName.Inbox, searchFilter, new ItemView(int.MaxValue)).TotalCount;
                    findResults = service.FindItems(WellKnownFolderName.Inbox, searchFilter, new ItemView(pageSize, offset));
                }
                else
                {
                    totalCount = service.FindItems(WellKnownFolderName.Inbox, new ItemView(int.MaxValue)).TotalCount;
                    findResults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(pageSize, offset));
                }
            }
            else
            {
                if (searchFilterCollection.Count > 0)
                {
                    SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

                    totalCount = service.FindItems(fid, searchFilter, new ItemView(int.MaxValue)).TotalCount;
                    findResults = service.FindItems(fid, searchFilter, new ItemView(pageSize, offset));
                }
                else
                {
                    totalCount = service.FindItems(fid, new ItemView(int.MaxValue)).TotalCount;
                    findResults = service.FindItems(fid, new ItemView(pageSize, offset));
                }
            }

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet();
                ps.Add(EmailMessageSchema.Subject);
                ps.Add(EmailMessageSchema.DateTimeSent);
                ps.Add(EmailMessageSchema.DateTimeReceived);
                ps.Add(EmailMessageSchema.From);
                ps.Add(EmailMessageSchema.IsRead);
                ps.Add(EmailMessageSchema.HasAttachments);
                ps.Add(EmailMessageSchema.Importance);
                if (wellKnownFolderName == "SENTITEMS")
                    ps.Add(EmailMessageSchema.ToRecipients);
                ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                ps.Add(new ExtendedPropertyDefinition(0x1080, MapiPropertyType.Integer));
                if (service.RequestedServerVersion == ExchangeVersion.Exchange2013)
                    ps.Add(MeetingRequestSchema.IsMeeting);


                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Item item in findResults.Items)
            {
                MailItem mi = new MailItem();

                // 해당 메세지 아이템이 일정요청에 해당할때... 메일메세지와 중복가능함.

                //else if (item is MeetingResponse)
                //{
                //    MeetingResponse meet = item as MeetingResponse;
                //    if (service.RequestedServerVersion == ExchangeVersion.Exchange2007_SP1)
                //        meet.Load(new PropertySet(MeetingRequestSchema.IsMeeting));
                //    if (meet.ResponseType == MeetingResponseType.)
                //    {
                //        mi.IsMeet = true;
                //    }
                //}

                if (item is EmailMessage)
                {
                    EmailMessage msg = (EmailMessage)item;

                    if (!string.IsNullOrEmpty(msg.Subject))
                        mi.Subject = msg.Subject;
                    else
                        mi.Subject = MAIL_NONE_SUBJECT;
                    if (msg.From != null)
                    {
                        mi.Sender = GetFrom(msg.From);
                    }
                    mi.IsRead = msg.IsRead;
                    mi.FolderID = fid.ToString();
                    mi.HasAttachments = msg.HasAttachments;
                    mi.DateTimeSent = msg.DateTimeSent;
                    if (wellKnownFolderName == "SENTITEMS" || wellKnownFolderName == "DRAFTS")
                        mi.DateTimeForList = msg.DateTimeSent;
                    else
                        mi.DateTimeForList = msg.DateTimeReceived;
                    mi.Importance = (ImportanceLevel)msg.Importance;

                    if (wellKnownFolderName == "SENTITEMS")
                        mi.ToRecipient = GetRecipients(msg.ToRecipients);

                    foreach (ExtendedProperty exprop in msg.ExtendedProperties)
                    {
                        if (exprop.PropertyDefinition.Tag == 0x300B)
                        {
                            byte[] bkeys = exprop.Value as byte[];
                            mi.Pr_Search_Key = Convert.ToBase64String(bkeys);
                        }
                        else if (exprop.PropertyDefinition.Tag == 0x1080)
                        {
                            int? value = exprop.Value as int?;
                            mi.Pr_Icon_Index = value;
                        }
                    }
                    if (item is MeetingRequest)
                    {
                        MeetingRequest meet = item as MeetingRequest;
                        if (service.RequestedServerVersion == ExchangeVersion.Exchange2007_SP1)
                        {
                            PropertySet ps = new PropertySet(MeetingRequestSchema.IsMeeting);

                            meet.Load(ps);
                        }
                        if (meet.IsMeeting)
                        {
                            mi.IsMeet = true;
                        }
                    }
                    listitem.Add(mi);
                }


            }
            //listitem.Reverse();

            mailList.MailList = listitem;
            mailList.TotalCount = totalCount;
            mailList.WellknownFolderName = wellKnownFolderName;
            mailList.CurrentFolderID = fid.ToString();

            return mailList;
        }

        public MailItem GetResponseMailItem(FolderId fid, string prsearchkey, string type)
        {
            ActionType aType = (ActionType)Enum.Parse(typeof(ActionType), type, true);

            MailItem mi = new MailItem();
            FindItemsResults<Item> messages;

            ItemView view = new ItemView(1);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            messages = service.FindItems(fid, filter, view);

            foreach (Item item in messages)
            {
                if (item is EmailMessage)
                {
                    EmailMessage msg = (EmailMessage)item;

                    if (aType == ActionType.FORWARD)
                    {
                        ResponseMessage response = msg.CreateForward();
                        msg = response.Save(WellKnownFolderName.Drafts);
                    }
                    else if (aType == ActionType.REPLY)
                    {
                        ResponseMessage response = msg.CreateReply(false);
                        msg = response.Save(WellKnownFolderName.Drafts);
                    }
                    else if (aType == ActionType.REPLYALL)
                    {
                        ResponseMessage response = msg.CreateReply(true);
                        msg = response.Save(WellKnownFolderName.Drafts);
                    }

                    PropertySet ps = new PropertySet(EmailMessageSchema.Body);
                    ps.Add(EmailMessageSchema.Subject);
                    ps.Add(EmailMessageSchema.DateTimeSent);
                    ps.Add(EmailMessageSchema.From);
                    ps.Add(EmailMessageSchema.ToRecipients);
                    ps.Add(EmailMessageSchema.CcRecipients);
                    ps.Add(EmailMessageSchema.BccRecipients);
                    ps.Add(EmailMessageSchema.IsRead);
                    ps.Add(EmailMessageSchema.Attachments);
                    ps.Add(EmailMessageSchema.HasAttachments);
                    ps.Add(EmailMessageSchema.Importance);
                    ps.Add(EmailMessageSchema.InternetMessageId);
                    ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                    msg.Load(ps);

                    foreach (ExtendedProperty exprop in msg.ExtendedProperties)
                    {
                        if (exprop.PropertyDefinition.Tag == 0x300B)
                        {
                            byte[] bkeys = exprop.Value as byte[];
                            mi.Pr_Search_Key = Convert.ToBase64String(bkeys);
                            break;

                        }
                    }
                    if (string.IsNullOrEmpty(msg.Subject))
                        mi.Subject = MAIL_NONE_SUBJECT;
                    else
                        mi.Subject = msg.Subject;
                    mi.Body = msg.Body;
                    if (msg.From != null)
                    {
                        mi.Sender = GetFrom(msg.From);
                    }
                    mi.IsRead = msg.IsRead;
                    mi.FolderID = fid.ToString();
                    mi.Importance = (ImportanceLevel)msg.Importance;
                    mi.HasAttachments = msg.HasAttachments;
                    mi.DateTimeSent = msg.DateTimeSent;
                    mi.ToRecipient = GetRecipients(msg.ToRecipients);
                    mi.CcRecipient = GetRecipients(msg.CcRecipients);
                    mi.BccRecipient = GetRecipients(msg.BccRecipients);
                    mi.MessageID = msg.InternetMessageId;

                    if (!msg.IsRead)
                    {
                        msg.IsRead = true;
                        msg.Update(ConflictResolutionMode.AutoResolve);
                    }

                    if (aType == ActionType.FORWARD || aType == ActionType.REPLY || aType == ActionType.REPLYALL)
                    {
                        msg.Delete(DeleteMode.HardDelete);
                    }

                }
            }
            return mi;
        }

        public MailItem GetMailItem(FolderId fid, string prsearchkey, string type, string folderId, bool addAttach)
        {
            ActionType aType = (ActionType)Enum.Parse(typeof(ActionType), type, true);

            MailItem mi = new MailItem();
            FindItemsResults<Item> messages;

            ItemView view = new ItemView(1);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            messages = service.FindItems(fid, filter, view);

            foreach (Item item in messages)
            {
                // 해당 메세지 아이템이 일정요청에 해당할때... 메일메세지와 중복가능함.
                if (item is MeetingRequest)
                {
                    MeetingRequest meet = item as MeetingRequest;
                    if (service.RequestedServerVersion == ExchangeVersion.Exchange2007_SP1)
                    {
                        PropertySet ps = new PropertySet(MeetingRequestSchema.IsMeeting);
                        ps.Add(MeetingRequestSchema.Start);
                        ps.Add(MeetingRequestSchema.End);
                        ps.Add(MeetingRequestSchema.Location);

                        meet.Load(ps);
                    }
                    if (meet.IsMeeting)
                    {
                        mi.IsMeet = true;
                        mi.MeetStart = meet.Start;
                        mi.MeetEnd = meet.End;
                        mi.MeetLocation = meet.Location;
                    }
                }

                if (item is EmailMessage)
                {
                    EmailMessage msg = (EmailMessage)item;

                    PropertySet ps = new PropertySet(EmailMessageSchema.Body);
                    ps.Add(EmailMessageSchema.Subject);
                    ps.Add(EmailMessageSchema.DateTimeSent);
                    ps.Add(EmailMessageSchema.From);
                    ps.Add(EmailMessageSchema.ToRecipients);
                    ps.Add(EmailMessageSchema.CcRecipients);
                    ps.Add(EmailMessageSchema.BccRecipients);
                    ps.Add(EmailMessageSchema.IsRead);
                    ps.Add(EmailMessageSchema.Attachments);
                    ps.Add(EmailMessageSchema.HasAttachments);
                    ps.Add(EmailMessageSchema.Importance);
                    ps.Add(EmailMessageSchema.InternetMessageId);
                    ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                    msg.Load(ps);

                    foreach (ExtendedProperty exprop in msg.ExtendedProperties)
                    {
                        if (exprop.PropertyDefinition.Tag == 0x300B)
                        {
                            byte[] bkeys = exprop.Value as byte[];
                            mi.Pr_Search_Key = Convert.ToBase64String(bkeys);
                            break;

                        }
                    }
                    if (string.IsNullOrEmpty(msg.Subject))
                        mi.Subject = MAIL_NONE_SUBJECT;
                    else
                        mi.Subject = msg.Subject;
                    mi.Body = msg.Body;
                    if (msg.From != null)
                    {
                        mi.Sender = GetFrom(msg.From);
                    }
                    mi.IsRead = msg.IsRead;
                    mi.FolderID = fid.ToString();
                    mi.Importance = (ImportanceLevel)msg.Importance;
                    mi.HasAttachments = msg.HasAttachments;
                    mi.DateTimeSent = msg.DateTimeSent;
                    mi.ToRecipient = GetRecipients(msg.ToRecipients);
                    mi.CcRecipient = GetRecipients(msg.CcRecipients);
                    mi.BccRecipient = GetRecipients(msg.BccRecipients);
                    mi.MessageID = msg.InternetMessageId;

                    //첨부
                    if (msg.Attachments.Count > 0)
                    {
                        string inlineGuid = Guid.NewGuid().ToString();

                        List<AttachmentItem> attItemList = new List<AttachmentItem>();
                        foreach (Attachment att in msg.Attachments)
                        {
                            if (att is FileAttachment)
                            {

                                FileAttachment fatt = att as FileAttachment;

                                fatt.Load();
                                bool isInline = false;
                                string sType = string.Empty;
                                string oldString = string.Empty;
                                if (fatt.ContentType != null && fatt.ContentType.ToLower().Contains("image"))
                                {
                                    sType = fatt.ContentType.ToLower();
                                    sType = sType.Replace("image/", "");
                                    oldString = "cid:" + fatt.ContentId;
                                    if (mi.Body.Contains(oldString))
                                        isInline = true;
                                }
                                if (isInline)
                                {
                                    //2015-04-24 인라인 무시. 첨부 필요 없음.
                                    //string filePath = MailPathInfo.MailInlineFolderPath + "\\" + inlineGuid;
                                    //Directory.CreateDirectory(filePath);
                                    //fatt.Load(filePath + "\\" + fatt.Name);
                                    ////http://m3.tnet.sktelecom.com/CommonPages/InlineImgViewer.aspx?folder=UserFile/Mail/InlineImg&fileName=image003.jpg
                                    //string encodedName = HttpUtility.UrlEncode(fatt.Name);
                                    //string src = MailPathInfo.MailImageServicePage + "?folder=/Inline/" + inlineGuid + "&fileName=" + encodedName;
                                    //mi.Body = mi.Body.Replace(oldString, src);

                                    //AttachmentItem attItem = new AttachmentItem();

                                    //attItem.Name = fatt.Name;
                                    ////attItem.Size = fatt.Size;
                                    //attItem.ContentType = fatt.ContentType;
                                    //attItem.ContentId = fatt.ContentId;
                                    //attItem.ServerPath = src;
                                    //attItem.IsInline = true;
                                    //attItem.AttachId = fatt.Id;

                                    //attItemList.Add(attItem);
                                }//첨부
                                else
                                {
                                    if (addAttach)
                                    {
                                        if (aType == ActionType.FORWARD || aType == ActionType.VIEW || aType == ActionType.MODIFY)
                                        {
                                            AttachmentItem attItem = new AttachmentItem();
                                            attItem.Name = fatt.Name;

                                            attItem.ContentType = fatt.ContentType;
                                            attItem.ContentId = fatt.ContentId;
                                            if (String.IsNullOrEmpty(attItem.ContentType)) attItem.ContentType = String.Empty;
                                            if (String.IsNullOrEmpty(attItem.ContentId)) attItem.ContentId = String.Empty;

                                            attItem.IsInline = false;
                                            attItem.AttachId = fatt.Id;
                                            attItem.MessageId = mi.MessageID;

                                            string folderPath = MailPathInfo.MailAttachFolderPath + "\\" + folderId + "\\";
                                            string filePath = folderPath + fatt.Name;

                                            if (!Directory.Exists(folderPath))
                                                Directory.CreateDirectory(folderPath);

                                            fatt.Load(filePath);
                                            attItem.ServerPath = filePath;

                                            if (service.RequestedServerVersion == ExchangeVersion.Exchange2007_SP1)
                                            {
                                                FileInfo fInfo = new FileInfo(filePath);
                                                attItem.Size = (int)fInfo.Length;
                                            }
                                            else
                                                attItem.Size = fatt.Size;

                                            attItemList.Add(attItem);
                                        }
                                    }
                                }
                            }
                        }
                        mi.Attachments = attItemList;
                    }

                    if (!msg.IsRead)
                    {
                        msg.IsRead = true;
                        msg.Update(ConflictResolutionMode.AutoResolve);
                    }
                }
            }
            return mi;
        }

        public void ResponseMail(MailItem mi, bool isDraft, ActionType actionType)
        {
            FindItemsResults<Item> messages;

            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            ItemView view = new ItemView(1);
            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, mi.Pr_Search_Key);

            messages = service.FindItems(mi.FolderID, filter, view);

            foreach (Item item in messages)
            {
                if (item is EmailMessage)
                {
                    EmailMessage message = item as EmailMessage;

                    if (actionType == ActionType.FORWARD)
                    {
                        ResponseMessage response = message.CreateForward();

                        message = response.Save(WellKnownFolderName.Drafts);
                    }
                    else if (actionType == ActionType.REPLY)
                    {
                        ResponseMessage response = message.CreateReply(false);
                        message = response.Save(WellKnownFolderName.Drafts);
                    }
                    else if (actionType == ActionType.REPLYALL)
                    {
                        ResponseMessage response = message.CreateReply(true);
                        message = response.Save(WellKnownFolderName.Drafts);
                    }

                    for (int i = 0; i < mi.Attachments.Count; i++)
                    {
                        AttachmentItem attachItem = mi.Attachments[i];
                        if (!attachItem.IsInline)
                        {
                            if (File.Exists(attachItem.ServerPath))
                            {
                                byte[] theBytes = File.ReadAllBytes(attachItem.ServerPath);
                                FileAttachment addedAttachment = message.Attachments.AddFileAttachment(attachItem.ContentId, theBytes);
                                addedAttachment.ContentId = attachItem.ContentId;
                                //message.Attachments[i].IsInline = attachItem.IsInline;
                                addedAttachment.Name = attachItem.Name;
                                addedAttachment.ContentType = attachItem.ContentType;


                                message.Update(ConflictResolutionMode.AlwaysOverwrite);
                            }
                        }
                    }

                    PropertySet ps = new PropertySet(EmailMessageSchema.Body);
                    ps.Add(EmailMessageSchema.Id);
                    ps.Add(EmailMessageSchema.Subject);
                    ps.Add(EmailMessageSchema.ToRecipients);
                    ps.Add(EmailMessageSchema.CcRecipients);
                    ps.Add(EmailMessageSchema.BccRecipients);
                    ps.Add(EmailMessageSchema.Attachments);
                    message.Load(ps);

                    message.ToRecipients.Clear();
                    message.CcRecipients.Clear();
                    message.BccRecipients.Clear();

                    SetRecipient(mi.ToRecipient, message.ToRecipients);
                    SetRecipient(mi.CcRecipient, message.CcRecipients);
                    SetRecipient(mi.BccRecipient, message.BccRecipients);

                    message.Subject = mi.Subject;



                    string temp = message.Body;
                    int startIdx = temp.IndexOf("<span id=\"sktmobilemailbodystart\">");

                    if (startIdx > -1)
                    {
                        temp = temp.Substring(startIdx, temp.IndexOf("</span>", startIdx) + 7 - startIdx);
                        string body = message.Body.ToString();
                        message.Body = body.Replace(temp, mi.Body);
                    }
                    else
                    {
                        message.Body = mi.Body + message.Body;
                    }


                    message.Update(ConflictResolutionMode.AlwaysOverwrite);

                    //삭제
                    if (message.Attachments.Count > 0)
                    {
                        for (int i = message.Attachments.Count - 1; i > -1; i--)
                        {
                            FileAttachment attach = message.Attachments[i] as FileAttachment;

                            bool isNeedRemove = true;
                            foreach (AttachmentItem aItem in mi.Attachments)
                            {
                                if (aItem.IsInline && attach.ContentId == aItem.ContentId)
                                {
                                    isNeedRemove = false;
                                    break;
                                }
                                else if (!aItem.IsInline && attach.ContentId == aItem.ContentId)
                                {
                                    isNeedRemove = false;
                                    break;
                                }
                            }
                            if (isNeedRemove)
                            {
                                message.Attachments.RemoveAt(i);
                                message.Update(ConflictResolutionMode.AlwaysOverwrite);
                            }

                        }

                    }

                    if (!isDraft)
                        message.SendAndSaveCopy();
                }
            }

        }

        //일정 초대 답장 OR action (거절, 수락, 미정)
        public void ResponseMeeting(string fid, string prsearchkey, ResponseMeetingType responseType, ResponseMeetingActionType responseActionType)
        {
            FindItemsResults<Item> messages;

            ItemView view = new ItemView(1);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            messages = service.FindItems(fid, filter, view);

            foreach (Item item in messages)
            {
                if (item is MeetingRequest)
                {
                    MeetingRequest meet = item as MeetingRequest;
                    if (meet.IsMeeting)
                    {
                        if (responseType == ResponseMeetingType.Accept)
                        {
                            if (responseActionType == ResponseMeetingActionType.NotSendMail)
                            {
                                meet.Accept(false);
                            }
                            else if (responseActionType == ResponseMeetingActionType.SendMail)
                            {
                                meet.Accept(true);
                            }

                        }
                        else if (responseType == ResponseMeetingType.Tentative)
                        {
                            if (responseActionType == ResponseMeetingActionType.NotSendMail)
                            {
                                meet.AcceptTentatively(false);
                            }
                            else if (responseActionType == ResponseMeetingActionType.SendMail)
                            {
                                meet.AcceptTentatively(true);
                            }
                        }
                        else
                        {
                            if (responseActionType == ResponseMeetingActionType.NotSendMail)
                            {
                                meet.Decline(false);
                            }
                            else if (responseActionType == ResponseMeetingActionType.SendMail)
                            {
                                meet.Decline(true);
                            }
                        }

                    }
                }
            }
        }

        //일정 초대 custom 답장
        public void ResponseMeeting(MailItem mi, ActionType actionType)
        {
            FindItemsResults<Item> messages;

            ItemView view = new ItemView(1);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, mi.Pr_Search_Key);

            messages = service.FindItems(mi.FolderID, filter, view);

            foreach (Item item in messages)
            {
                if (item is MeetingRequest)
                {
                    MeetingRequest meet = item as MeetingRequest;
                    MeetingResponse message = null;
                    if (actionType == ActionType.RESPONSEMEETINGACCEPT)
                    {
                        AcceptMeetingInvitationMessage msg = meet.CreateAcceptMessage(false);
                        CalendarActionResults actionResult = msg.Save(WellKnownFolderName.Drafts);
                        message = actionResult.MeetingResponse;
                    }
                    else if (actionType == ActionType.RESPONSEMEETINGTENTATIVE)
                    {
                        AcceptMeetingInvitationMessage msg = meet.CreateAcceptMessage(true);
                        CalendarActionResults actionResult = msg.Save(WellKnownFolderName.Drafts);
                        message = actionResult.MeetingResponse;
                    }
                    else
                    {
                        DeclineMeetingInvitationMessage msg = meet.CreateDeclineMessage();
                        CalendarActionResults actionResult = msg.Save(WellKnownFolderName.Drafts);
                        message = actionResult.MeetingResponse;
                    }

                    for (int i = 0; i < mi.Attachments.Count; i++)
                    {
                        AttachmentItem attachItem = mi.Attachments[i];
                        if (!attachItem.IsInline)
                        {
                            if (File.Exists(attachItem.ServerPath))
                            {
                                byte[] theBytes = File.ReadAllBytes(attachItem.ServerPath);
                                FileAttachment addedAttachment = message.Attachments.AddFileAttachment(attachItem.ContentId, theBytes);
                                addedAttachment.ContentId = attachItem.ContentId;
                                //message.Attachments[i].IsInline = attachItem.IsInline;
                                addedAttachment.Name = attachItem.Name;
                                addedAttachment.ContentType = attachItem.ContentType;


                                message.Update(ConflictResolutionMode.AlwaysOverwrite);
                            }
                        }
                    }

                    PropertySet ps = new PropertySet(MeetingResponseSchema.Body);
                    ps.Add(MeetingResponseSchema.Id);
                    ps.Add(MeetingResponseSchema.Subject);
                    ps.Add(MeetingResponseSchema.ToRecipients);
                    ps.Add(MeetingResponseSchema.CcRecipients);
                    ps.Add(MeetingResponseSchema.BccRecipients);
                    ps.Add(MeetingResponseSchema.Attachments);
                    message.Load(ps);

                    message.ToRecipients.Clear();
                    message.CcRecipients.Clear();
                    message.BccRecipients.Clear();

                    SetRecipient(mi.ToRecipient, message.ToRecipients);
                    SetRecipient(mi.CcRecipient, message.CcRecipients);
                    SetRecipient(mi.CcRecipient, message.BccRecipients);

                    message.Subject = mi.Subject;
                    message.Body = mi.Body + message.Body;

                    message.Update(ConflictResolutionMode.AlwaysOverwrite);

                    //삭제
                    if (message.Attachments.Count > 0)
                    {
                        for (int i = message.Attachments.Count - 1; i > -1; i--)
                        {
                            FileAttachment attach = message.Attachments[i] as FileAttachment;

                            bool isNeedRemove = true;
                            foreach (AttachmentItem aItem in mi.Attachments)
                            {
                                if (aItem.IsInline && attach.ContentId == aItem.ContentId)
                                {
                                    isNeedRemove = false;
                                    break;
                                }
                                else if (!aItem.IsInline && attach.ContentId == aItem.ContentId)
                                {
                                    isNeedRemove = false;
                                    break;
                                }
                            }
                            if (isNeedRemove)
                            {
                                message.Attachments.RemoveAt(i);
                                message.Update(ConflictResolutionMode.AlwaysOverwrite);
                            }

                        }

                    }

                    message.SendAndSaveCopy();
                }

            }
        }

        public MailItem GetResponseMeeting(string fid, string prsearchkey, ActionType actionType)
        {
            FindItemsResults<Item> messages;

            ItemView view = new ItemView(1);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            messages = service.FindItems(fid, filter, view);

            MailItem mi = new MailItem();

            foreach (Item item in messages)
            {
                if (item is MeetingRequest)
                {
                    MeetingRequest meet = item as MeetingRequest;
                    if (meet.IsMeeting)
                    {
                        PropertySet ps = new PropertySet();
                        ps.Add(MeetingRequestSchema.Id);
                        ps.Add(MeetingRequestSchema.Subject);
                        ps.Add(MeetingRequestSchema.ToRecipients);
                        ps.Add(MeetingRequestSchema.CcRecipients);
                        ps.Add(MeetingRequestSchema.BccRecipients);
                        ps.Add(MeetingRequestSchema.Attachments);
                        ps.Add(MeetingRequestSchema.Sender);
                        meet.Load(ps);

                        if (string.IsNullOrEmpty(meet.Subject))
                            mi.Subject = MAIL_NONE_SUBJECT;
                        else

                            mi.FolderID = fid.ToString();
                        mi.ToRecipient = GetRecipients(meet.ToRecipients);
                        mi.CcRecipient = GetRecipients(meet.CcRecipients);
                        mi.BccRecipient = GetRecipients(meet.BccRecipients);

                        List<EmailAddressItem> toRecipient = new List<EmailAddressItem>();
                        toRecipient.Add(GetFrom(meet.Sender));
                        if (actionType == ActionType.RESPONSEMEETINGACCEPT)
                        {
                            mi.Subject = "Accepted: " + meet.Subject;
                        }
                        else if (actionType == ActionType.RESPONSEMEETINGTENTATIVE)
                        {
                            mi.Subject = "Tentative: " + meet.Subject;
                        }
                        else
                        {
                            mi.Subject = "Decline: " + meet.Subject;
                        }

                        mi.ToRecipient = toRecipient;
                    }
                }
            }
            return mi;
        }

        public void SendMail(MailItem mi, bool isDraft)
        {

            EmailMessage message = new EmailMessage(service);
            SetRecipient(mi.ToRecipient, message.ToRecipients);
            SetRecipient(mi.CcRecipient, message.CcRecipients);
            SetRecipient(mi.BccRecipient, message.BccRecipients);

            message.Subject = mi.Subject;
            message.Body = new MessageBody(BodyType.HTML, mi.Body);

            for (int i = 0; i < mi.Attachments.Count; i++)
            {
                AttachmentItem item = mi.Attachments[i];
                if (File.Exists(item.ServerPath))
                {
                    byte[] theBytes = File.ReadAllBytes(item.ServerPath);
                    message.Attachments.AddFileAttachment(item.ContentId, theBytes);
                    message.Attachments[i].ContentId = item.ContentId;
                    //message.Attachments[i].IsInline = item.IsInline;
                    message.Attachments[i].Name = item.Name;
                    message.Attachments[i].ContentType = item.ContentType;
                }

            }

            if (!isDraft)
                message.SendAndSaveCopy();
            else
                message.Save(WellKnownFolderName.Drafts);
        }

        public EmailAddressCollection SetRecipient(List<EmailAddressItem> recipients, EmailAddressCollection collection)
        {
            foreach (EmailAddressItem ri in recipients)
            {
                if (!string.IsNullOrEmpty(ri.Address))
                    collection.Add(ri.Address);
                else
                {
                    string smtpAddr = GetSmtpAddress(ri.Name);
                    if (!string.IsNullOrEmpty(smtpAddr))
                        collection.Add(smtpAddr);
                }
            }

            return collection;
        }

        public void DeleteMail(string fid, string pr_search_key, DeleteMode dm)
        {
            FolderId folderId = null;
            if (string.IsNullOrEmpty(fid))
                return;
            folderId = new FolderId(fid);

            ItemView view = new ItemView(int.MaxValue);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, pr_search_key);

            FindItemsResults<Item> messages = service.FindItems(fid, filter, view);

            foreach (Item item in messages)
            {
                item.Delete(dm);
            }

        }

        public void DeleteMailAll(FolderId fid, DeleteMode dm)
        {
            ItemView view = new ItemView(int.MaxValue);

            FindItemsResults<Item> deletedMessages = service.FindItems(fid, view);

            foreach (Item item in deletedMessages)
            {
                item.Delete(dm);
            }

            List<FolderItem> folderList = GetChildFolderList(fid.ToString());
            foreach (FolderItem folder in folderList)
            {
                FindItemsResults<Item> messages = service.FindItems(fid, view);

                foreach (Item item in messages)
                {
                    item.Delete(dm);
                }
            }

        }

        public void SetIsRead(FolderId fid, string pr_search_key, bool isread)
        {
            ItemView view = new ItemView(int.MaxValue);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, pr_search_key);

            FindItemsResults<Item> messages = service.FindItems(fid, filter, view);

            foreach (EmailMessage item in messages)
            {
                item.IsRead = isread;
                item.Update(ConflictResolutionMode.AutoResolve);
            }

        }

        public void SetImportance(FolderId fid, string prsearchkey, ImportanceLevel Importance)
        {
            ItemView view = new ItemView(int.MaxValue);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            FindItemsResults<Item> messages = service.FindItems(fid, filter, view);

            PropertySet ps = new PropertySet(EmailMessageSchema.Importance);
            service.LoadPropertiesForItems(messages, ps);

            foreach (Item item in messages.Items)
            {
                item.Importance = (Importance)Importance;
                item.Update(ConflictResolutionMode.AutoResolve);
            }


        }



        public void MoveMail(FolderId fid, FolderId targetFid, string prsearchkey)
        {
            ItemView view = new ItemView(int.MaxValue);
            ExtendedPropertyDefinition propDef = new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary);

            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, propDef);
            view.Traversal = ItemTraversal.Shallow;

            SearchFilter filter = new SearchFilter.IsEqualTo(propDef, prsearchkey);

            FindItemsResults<Item> messages = service.FindItems(fid, filter, view);

            foreach (Item item in messages)
            {
                item.Move(targetFid);
            }
        }

        //public SignatureItem GetSignature()
        //{
        //    SignatureItem sign = new SignatureItem();

        //    Folder Root = Folder.Bind(service, WellKnownFolderName.Root);
        //    UserConfiguration OWAConfig = UserConfiguration.Bind(service, "OWA.UserOptions", Root.ParentFolderId, UserConfigurationProperties.All);
        //    if (OWAConfig.Dictionary.ContainsKey("sktmobileautoaddsignature"))
        //    {
        //        sign.AutoAddSignature = (bool)OWAConfig.Dictionary["sktmobileautoaddsignature"];
        //    }
        //    else
        //    {
        //        sign.AutoAddSignature = false;
        //    }
        //    if (OWAConfig.Dictionary.ContainsKey("sktmobilesignaturetext"))
        //    {
        //        sign.SignatureText = (string)OWAConfig.Dictionary["sktmobilesignaturetext"];
        //    }

        //    return sign;

        //}
        //// 서명 만들기
        //public void SetSignature(bool autoAdd, string text)
        //{
        //    Folder Root = Folder.Bind(service, WellKnownFolderName.Root);
        //    UserConfiguration OWAConfig = UserConfiguration.Bind(service, "OWA.UserOptions", Root.ParentFolderId, UserConfigurationProperties.All);

        //    if (OWAConfig.Dictionary.ContainsKey("sktmobileautoaddsignature"))
        //    {
        //        OWAConfig.Dictionary["sktmobileautoaddsignature"] = autoAdd;
        //    }
        //    else
        //    {
        //        OWAConfig.Dictionary.Add("sktmobileautoaddsignature", autoAdd);
        //    }
        //    if (OWAConfig.Dictionary.ContainsKey("sktmobilesignaturetext"))
        //    {
        //        OWAConfig.Dictionary["sktmobilesignaturetext"] = text;
        //    }
        //    else
        //    {
        //        OWAConfig.Dictionary.Add("sktmobilesignaturetext", text);
        //    }
        //    OWAConfig.Update();
        //}

        //public string GetBaseFolder()
        //{
        //    string retVal = string.Empty;

        //    Folder Root = Folder.Bind(service, WellKnownFolderName.Root);
        //    UserConfiguration OWAConfig = UserConfiguration.Bind(service, "OWA.UserOptions", Root.ParentFolderId, UserConfigurationProperties.All);
        //    if (OWAConfig.Dictionary.ContainsKey("basefolder"))
        //    {
        //        retVal = (string)OWAConfig.Dictionary["basefolder"];
        //    }
        //    return retVal;
        //}

        //public void SetBaseFolder(string folderId)
        //{
        //    Folder Root = Folder.Bind(service, WellKnownFolderName.Root);
        //    UserConfiguration OWAConfig = UserConfiguration.Bind(service, "OWA.UserOptions", Root.ParentFolderId, UserConfigurationProperties.All);
        //    if (OWAConfig.Dictionary.ContainsKey("basefolder"))
        //    {
        //        OWAConfig.Dictionary["basefolder"] = folderId;
        //    }
        //    else
        //    {
        //        OWAConfig.Dictionary.Add("basefolder", folderId);
        //    }

        //    OWAConfig.Update();
        //}

        private List<EmailAddressItem> GetRecipients(EmailAddressCollection recipients)
        {
            List<EmailAddressItem> recipientList = new List<EmailAddressItem>();
            foreach (EmailAddress ea in recipients)
            {
                EmailAddressItem ri = new EmailAddressItem();
                ri.Address = ea.Address;
                ri.Name = ea.Name;
                //ri.Id = ea.Id.ToString();
                ri.MailboxType = ea.MailboxType.ToString();
                recipientList.Add(ri);
            }

            return recipientList;
        }

        private EmailAddressItem GetFrom(EmailAddress from)
        {
            EmailAddressItem eai = new EmailAddressItem();
            eai.Address = from.Address;
            eai.Name = from.Name;
            eai.MailboxType = from.MailboxType.ToString();

            return eai;
        }

        private List<EmailAddressItem> GetEmailAddress(EmailAddressDictionary emailAddreses)
        {
            List<EmailAddressItem> recipientList = new List<EmailAddressItem>();
            for (int i = 0; i < 3; i++)
            {
                if (emailAddreses.Contains((EmailAddressKey)i))
                {
                    EmailAddressItem eai = new EmailAddressItem();
                    eai.Address = emailAddreses[(EmailAddressKey)i].Address;
                    eai.Name = emailAddreses[(EmailAddressKey)i].Name;
                    //ri.Id = ea.Id.ToString();
                    eai.MailboxType = emailAddreses[(EmailAddressKey)i].MailboxType.ToString();
                    recipientList.Add(eai);
                }
            }
            return recipientList;
        }

        private string GetSmtpAddress(string Name)
        {
            string retVal = string.Empty;
            if (string.IsNullOrEmpty(Name))
                return retVal;
            NameResolutionCollection nrc = service.ResolveName(Name);
            foreach (NameResolution nr in nrc)
            {
                if (nr.Mailbox.Name == Name)
                {
                    retVal = nr.Mailbox.Address;
                    break;
                }
            }
            return retVal;
        }

        public List<FolderId> GetContactFolderItem()
        {
            List<FolderId> folderItemList = new List<FolderId>();
            FindFoldersResults findFoldersResults = service.FindFolders(WellKnownFolderName.Contacts, new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep });
            foreach (Folder folder in findFoldersResults.Folders)
            {
                folderItemList.Add(folder.Id);
            }

            Folder cfolder = Folder.Bind(service, WellKnownFolderName.Contacts);
            folderItemList.Add(cfolder.Id);

            return folderItemList;
        }

        public List<ContactItem> GetContactList(string searchText, int pageSize, int offset)
        {

            List<ContactItem> contactItemList = new List<ContactItem>();

            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.DisplayName, searchText));
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.Surname, searchText));
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.GivenName, searchText));
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.EmailAddress1, searchText));
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.EmailAddress2, searchText));
                searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ContactSchema.EmailAddress3, searchText));
            }
            FindItemsResults<Item> findResults = null;
            if (searchFilterCollection.Count > 0)
            {
                SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, searchFilterCollection.ToArray());
                findResults = service.FindItems(WellKnownFolderName.Contacts, searchFilter, new ItemView(pageSize, offset));
            }
            else
            {
                findResults = service.FindItems(WellKnownFolderName.Contacts, new ItemView(pageSize, offset));
            }

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet();
                ps.Add(ContactSchema.Surname);
                ps.Add(ContactSchema.GivenName);
                ps.Add(ContactSchema.EmailAddresses);
                ps.Add(ContactSchema.EmailAddress1);
                ps.Add(ContactSchema.EmailAddress2);
                ps.Add(ContactSchema.EmailAddress3);

                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Item item in findResults.Items)
            {
                if (item is Contact)
                {
                    Contact contact = item as Contact;
                    ContactItem contactItem = new ContactItem();
                    contactItem.Surname = contact.Surname;
                    contactItem.GivenName = contact.GivenName;
                    contactItem.EmailAddresses = GetEmailAddress(contact.EmailAddresses);

                    contactItemList.Add(contactItem);
                }

            }

            return contactItemList;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------- Calendar ----------------------------------------------------------------//
        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Author : 최규연
        /// CreateDate : 2013.08.28
        /// Desc : Exchange Web Service Managed API를 이용한 Wrapper Class - Calendar
        /// </summary>

        //ID Property
        private static string GetObjectIdStringFromUid(string id)
        {
            var buffer = new byte[id.Length / 2];
            for (int i = 0; i < id.Length / 2; i++)
            {
                var hexValue = byte.Parse(id.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                buffer[i] = hexValue;
            }
            return Convert.ToBase64String(buffer);
        }

        //ICalUid를 사용하여 Appointment 탐색
        private Appointment FindRelatedAppointment(ExchangeService service, string ICalUid)
        {
            var filter = new SearchFilter.IsEqualTo
            {
                PropertyDefinition = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Meeting, 0x03, MapiPropertyType.Binary)
                ,
                Value = GetObjectIdStringFromUid(ICalUid) //Hex value converted to byte and base64 encoded
            };

            var view = new ItemView(1) { PropertySet = new PropertySet(BasePropertySet.FirstClassProperties) };
            var am = service.FindItems(WellKnownFolderName.Calendar, filter, view);

            PropertySet ps = new PropertySet(AppointmentSchema.Body);
            ps.RequestedBodyType = BodyType.Text;
            ps.Add(AppointmentSchema.Subject);
            ps.Add(AppointmentSchema.DateTimeSent);
            ps.Add(AppointmentSchema.Start);
            ps.Add(AppointmentSchema.End);
            ps.Add(AppointmentSchema.Recurrence);
            //ps.Add(AppointmentSchema.ReminderDueBy);
            ps.Add(AppointmentSchema.ReminderMinutesBeforeStart);
            ps.Add(AppointmentSchema.IsAllDayEvent);
            ps.Add(AppointmentSchema.IsReminderSet);
            ps.Add(AppointmentSchema.Location);
            ps.Add(AppointmentSchema.Id);
            ps.Add(AppointmentSchema.ICalUid);
            ps.Add(AppointmentSchema.Organizer);
            ps.Add(AppointmentSchema.MyResponseType);
            ps.Add(AppointmentSchema.IsCancelled);
            ps.Add(AppointmentSchema.RequiredAttendees);
            ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
            service.LoadPropertiesForItems(am, ps);
            return am.Items[0] as Appointment;
        }
        //Appointment를 AppointmentItem으로 바인딩
        public AppointmentItem ConnectAppointment(Appointment apm)
        {
            AppointmentItem ai = new AppointmentItem();
            if (apm != null)
            {
                //RepeatItem ri = new RepeatItem();
                ai.Subject = apm.Subject;
                ai.Body = apm.Body.Text;
                if (!string.IsNullOrEmpty(ai.Body))
                {
                    ai.Body = ai.Body.Text.Replace("<br>", "\n");
                }
                ai.Start = apm.Start;
                ai.End = apm.End;
                ai.DateTimeSent = apm.DateTimeSent;
                if (apm.RequiredAttendees != null)
                {
                    for (int i = 0; i < apm.RequiredAttendees.Count; i++)
                    {
                        RequiredAttendeesItem rai = new RequiredAttendeesItem();
                        if (!string.IsNullOrEmpty(apm.RequiredAttendees[i].Address))
                        { rai.Address = apm.RequiredAttendees[i].Address; }
                        if (!string.IsNullOrEmpty(apm.RequiredAttendees[i].Name))
                        { rai.Name = apm.RequiredAttendees[i].Name; }
                        ai.RequiredAttendees.Add(rai);
                    }
                }
                else
                {
                    ai.RequiredAttendees = new List<RequiredAttendeesItem>();
                }
                //Recurrence.IntervalPattern pattern = (Recurrence.IntervalPattern)apm.Recurrence;
                int repeatValue = 0;
                if (apm.Recurrence == null)
                {
                    ai.Recurrence = null;
                }
                else
                {
                    if (apm.Recurrence.GetType().Equals(typeof(Microsoft.Exchange.WebServices.Data.Recurrence.DailyPattern)))
                    {
                        repeatValue = 1;
                    }
                    else if (apm.Recurrence.GetType().Equals(typeof(Microsoft.Exchange.WebServices.Data.Recurrence.WeeklyPattern)))
                    {
                        repeatValue = 2;
                    }
                    else if (apm.Recurrence.GetType().Equals(typeof(Microsoft.Exchange.WebServices.Data.Recurrence.MonthlyPattern)))
                    {
                        repeatValue = 3;
                    }
                    else
                    {
                        repeatValue = 0;
                    }
                    Recurrence recurrence = new Recurrence();
                    recurrence.EndDate = apm.Recurrence.EndDate;
                    recurrence.HasEnd = apm.Recurrence.HasEnd;
                    recurrence.NumberOfOccurrences = apm.Recurrence.NumberOfOccurrences;
                    recurrence.Pattern = repeatValue;
                    recurrence.StartDate = apm.Recurrence.StartDate;
                    ai.Recurrence = recurrence;
                }
                //if (pattern != null)
                //{
                //    ri.StartRepeat = apm.Recurrence.StartDate;
                //    ri.Pattern = apm.Recurrence.
                //    if (ri.Pattern != "NoPattern" && string.IsNullOrEmpty(apm.Recurrence.EndDate.ToString()))
                //    {
                //        ri.EndRepeat = apm.Recurrence.StartDate.AddDays(-1);
                //    }
                //    else
                //    {
                //        ri.EndRepeat = apm.Recurrence.EndDate.Value;
                //    }
                //}
                //else
                //{
                //    ri.StartRepeat = apm.Start;
                //    ri.EndRepeat = apm.End;
                //    ri.Pattern = "NoPattern";
                //}
                //ai.Recurrence = ri;
                //ai.ReminderDueBy = apm.ReminderDueBy;
                ai.ReminderMinutesBeforeStart = apm.ReminderMinutesBeforeStart;
                ai.IsReminderSet = apm.IsReminderSet;
                ai.IsAllDayEvent = apm.IsAllDayEvent;
                ai.Location = apm.Location;
                ai.Id = apm.Id.ToString();
                ai.ICalUid = apm.ICalUid;
                if (apm.Organizer != null)
                {
                    ai.Organizer = apm.Organizer.Address;
                }
                ai.MyResponseType = apm.MyResponseType.ToString();
                ai.IsCancelled = apm.IsCancelled;
            }
            return ai;
        }

        //AppointmentItem을 Appointment로 바인딩
        public Appointment ConnectAppointment(AppointmentItem ai)
        {
            // Set properties on the appointment.
            Appointment apm = null;
            if (string.IsNullOrEmpty(ai.Id))
            {
                apm = new Appointment(service);
            }
            else
            {
                apm = Appointment.Bind(service, new ItemId(ai.Id));
            }
            //일정 제목
            apm.Subject = ai.Subject;
            //일정 내용
            if (!string.IsNullOrEmpty(ai.Body))
            {
                ai.Body = ai.Body.Text.Replace("\n", "<br>");
            }
            apm.Body = ai.Body;
            //일정 시작 시간
            apm.Start = ai.Start;
            //일정 종료 시간
            apm.End = ai.End;
            //일정 알람
            apm.ReminderMinutesBeforeStart = ai.ReminderMinutesBeforeStart;
            //알람 유무
            apm.IsReminderSet = ai.IsReminderSet;
            //하루종일
            apm.IsAllDayEvent = ai.IsAllDayEvent;
            //일정 위치
            apm.Location = ai.Location;
            //중요 표시
            apm.ConferenceType = ai.ConferenceType;
            //반복 설정
            if (ai.Recurrence != null)
            {
                apm.Recurrence = RepeatAppointment(ai);
            }
            else
            {
                apm.Recurrence = null;
            }
            //일정 참석자
            apm.RequiredAttendees.Clear();
            for (int i = 0; i < ai.RequiredAttendees.Count; i++)
            {
                if (!string.IsNullOrEmpty(ai.RequiredAttendees[i].Address))
                {
                    Attendee att = new Attendee();
                    ai.RequiredAttendees[i].Address = ai.RequiredAttendees[i].Address.Trim();
                    att.Address = ai.RequiredAttendees[i].Address;
                    if (!string.IsNullOrEmpty(ai.RequiredAttendees[i].Name))
                    {
                        ai.RequiredAttendees[i].Name = ai.RequiredAttendees[i].Name.Trim();
                    }
                    att.Name = ai.RequiredAttendees[i].Name;
                    apm.RequiredAttendees.Add(att);
                }
            }
            return apm;
        }

        //AppointmentItem을 Appointment로 바인딩
        public Appointment ChangeAppointment(AppointmentItem ai, Appointment apm)
        {
            //일정 제목
            apm.Subject = ai.Subject;
            //일정 내용
            if (!string.IsNullOrEmpty(ai.Body))
            {
                ai.Body = ai.Body.Text.Replace("\n", "<br>");
            }
            apm.Body = ai.Body;
            //TimeZone
            apm.StartTimeZone = TimeZoneInfo.Local;
            //일정 시작 시간
            apm.Start = ai.Start;
            //일정 종료 시간
            apm.End = ai.End;
            //일정 알람
            apm.ReminderMinutesBeforeStart = ai.ReminderMinutesBeforeStart;
            //알람 유무
            apm.IsReminderSet = ai.IsReminderSet;
            //하루종일
            apm.IsAllDayEvent = ai.IsAllDayEvent;
            //일정 위치
            apm.Location = ai.Location;
            //중요 표시
            apm.ConferenceType = ai.ConferenceType;
            //반복 설정
            apm.Recurrence = RepeatAppointment(ai);// RepeatAppointment(ai);
            //일정 참석자
            apm.RequiredAttendees.Clear();
            for (int i = 0; i < ai.RequiredAttendees.Count; i++)
            {
                if (!string.IsNullOrEmpty(ai.RequiredAttendees[i].Address))
                {
                    Attendee att = new Attendee();
                    ai.RequiredAttendees[i].Address = ai.RequiredAttendees[i].Address.Trim();
                    att.Address = ai.RequiredAttendees[i].Address;
                    if (!string.IsNullOrEmpty(ai.RequiredAttendees[i].Name))
                    {
                        ai.RequiredAttendees[i].Name = ai.RequiredAttendees[i].Name.Trim();
                    }
                    att.Name = ai.RequiredAttendees[i].Name;
                    apm.RequiredAttendees.Add(att);
                }
            }
            return apm;
        }

        //월간 리스트 로드
        public List<AppointmentItem> GetAppointmentMonthList(DateTime StartDate, DateTime EndDate)
        {
            List<AppointmentItem> retList = new List<AppointmentItem>();
            CalendarView CalView = new CalendarView(StartDate, EndDate.AddMilliseconds(-1));
            FindItemsResults<Appointment> findResults = service.FindAppointments(WellKnownFolderName.Calendar, CalView);

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(AppointmentSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(AppointmentSchema.Subject);
                ps.Add(AppointmentSchema.DateTimeSent);
                ps.Add(AppointmentSchema.Start);
                ps.Add(AppointmentSchema.End);
                ps.Add(AppointmentSchema.Recurrence);
                //ps.Add(AppointmentSchema.ReminderDueBy);
                ps.Add(AppointmentSchema.ReminderMinutesBeforeStart);
                ps.Add(AppointmentSchema.IsAllDayEvent);
                ps.Add(AppointmentSchema.IsReminderSet);
                ps.Add(AppointmentSchema.Location);
                ps.Add(AppointmentSchema.Id);
                ps.Add(AppointmentSchema.ICalUid);
                ps.Add(AppointmentSchema.Organizer);
                ps.Add(AppointmentSchema.MyResponseType);
                ps.Add(AppointmentSchema.IsCancelled);
                ps.Add(AppointmentSchema.RequiredAttendees);
                ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Appointment apm in findResults.Items)
            {

                if (!String.IsNullOrEmpty(apm.Subject))
                    apm.Subject = apm.Subject.Replace("<", "&lt;").Replace(">", "&gt;");
                if (!String.IsNullOrEmpty(apm.Location))
                    apm.Location = apm.Location.Replace("<", "&lt;").Replace(">", "&gt;");
                if (apm.Body != null && !String.IsNullOrEmpty(apm.Body.Text))
                    apm.Body.Text = apm.Body.Text.Replace("<", "&lt;").Replace(">", "&gt;");

                retList.Add(ConnectAppointment(apm));

            }


            /*for (int i = 0; i < retList.Count; i++)
            {
                retList[i].Subject = retList[i].Subject.Replace("<", "&lt;").Replace(">", "&gt;");                
            }*/


            return retList;
        }
        //주간 리스트 로드
        public List<AppointmentItem> GetAppointmentWeekList(DateTime startDate, DateTime endDate)
        {
            List<AppointmentItem> retList = new List<AppointmentItem>();
            CalendarView CalView = new CalendarView(startDate, endDate.AddMilliseconds(-1));
            FindItemsResults<Appointment> findResults = service.FindAppointments(WellKnownFolderName.Calendar, CalView);

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(AppointmentSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(AppointmentSchema.Subject);
                ps.Add(AppointmentSchema.DateTimeSent);
                ps.Add(AppointmentSchema.Start);
                ps.Add(AppointmentSchema.End);
                ps.Add(AppointmentSchema.Recurrence);
                //ps.Add(AppointmentSchema.ReminderDueBy);
                ps.Add(AppointmentSchema.ReminderMinutesBeforeStart);
                ps.Add(AppointmentSchema.IsAllDayEvent);
                ps.Add(AppointmentSchema.IsReminderSet);
                ps.Add(AppointmentSchema.Location);
                ps.Add(AppointmentSchema.Id);
                ps.Add(AppointmentSchema.ICalUid);
                ps.Add(AppointmentSchema.Organizer);
                ps.Add(AppointmentSchema.MyResponseType);
                ps.Add(AppointmentSchema.IsCancelled);
                ps.Add(AppointmentSchema.RequiredAttendees);
                ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Appointment apm in findResults.Items)
            {

                if (!String.IsNullOrEmpty(apm.Subject))
                    apm.Subject = apm.Subject.Replace("<", "&lt;").Replace(">", "&gt;");
                if (!String.IsNullOrEmpty(apm.Location))
                    apm.Location = apm.Location.Replace("<", "&lt;").Replace(">", "&gt;");
                if (apm.Body != null && !String.IsNullOrEmpty(apm.Body.Text))
                    apm.Body.Text = apm.Body.Text.Replace("<", "&lt;").Replace(">", "&gt;");

                retList.Add(ConnectAppointment(apm));
            }
            return retList;
        }

        //일간 리스트 로드
        public List<AppointmentItem> GetAppointmentDayListByMain(DateTime date)
        {
            DateTime startDate = date;
            DateTime endDate = date.AddYears(2).AddDays(-1);
            List<AppointmentItem> retList = new List<AppointmentItem>();
            CalendarView CalView = new CalendarView(startDate, endDate, 3);
            FindItemsResults<Appointment> findResults = service.FindAppointments(WellKnownFolderName.Calendar, CalView);

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(AppointmentSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(AppointmentSchema.Subject);
                ps.Add(AppointmentSchema.DateTimeSent);
                ps.Add(AppointmentSchema.Start);
                ps.Add(AppointmentSchema.End);
                ps.Add(AppointmentSchema.Recurrence);
                //ps.Add(AppointmentSchema.ReminderDueBy);
                ps.Add(AppointmentSchema.ReminderMinutesBeforeStart);
                ps.Add(AppointmentSchema.IsAllDayEvent);
                ps.Add(AppointmentSchema.IsReminderSet);
                ps.Add(AppointmentSchema.Location);
                ps.Add(AppointmentSchema.Id);
                ps.Add(AppointmentSchema.ICalUid);
                ps.Add(AppointmentSchema.Organizer);
                ps.Add(AppointmentSchema.MyResponseType);
                ps.Add(AppointmentSchema.IsCancelled);
                ps.Add(AppointmentSchema.RequiredAttendees);
                ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Appointment apm in findResults.Items)
            {

                if (!String.IsNullOrEmpty(apm.Subject))
                    apm.Subject = apm.Subject.Replace("<", "&lt;").Replace(">", "&gt;");
                if (!String.IsNullOrEmpty(apm.Location))
                    apm.Location = apm.Location.Replace("<", "&lt;").Replace(">", "&gt;");
                if (apm.Body != null && !String.IsNullOrEmpty(apm.Body.Text))
                    apm.Body.Text = apm.Body.Text.Replace("<", "&lt;").Replace(">", "&gt;");

                retList.Add(ConnectAppointment(apm));
            }
            return retList;
        }
        //일간 리스트 로드
        public List<AppointmentItem> GetAppointmentDayList(DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);//.AddMilliseconds(1);
            DateTime endDate = startDate.AddDays(1).AddMilliseconds(-1);
            List<AppointmentItem> retList = new List<AppointmentItem>();
            CalendarView CalView = new CalendarView(startDate, endDate);
            FindItemsResults<Appointment> findResults = service.FindAppointments(WellKnownFolderName.Calendar, CalView);

            if (findResults.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(AppointmentSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(AppointmentSchema.Subject);
                ps.Add(AppointmentSchema.DateTimeSent);
                ps.Add(AppointmentSchema.Start);
                ps.Add(AppointmentSchema.End);
                ps.Add(AppointmentSchema.Recurrence);
                //ps.Add(AppointmentSchema.ReminderDueBy);
                ps.Add(AppointmentSchema.ReminderMinutesBeforeStart);
                ps.Add(AppointmentSchema.IsAllDayEvent);
                ps.Add(AppointmentSchema.IsReminderSet);
                ps.Add(AppointmentSchema.Location);
                ps.Add(AppointmentSchema.Id);
                ps.Add(AppointmentSchema.ICalUid);
                ps.Add(AppointmentSchema.Organizer);
                ps.Add(AppointmentSchema.MyResponseType);
                ps.Add(AppointmentSchema.IsCancelled);
                ps.Add(AppointmentSchema.RequiredAttendees);
                ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
                service.LoadPropertiesForItems(findResults, ps);
            }
            foreach (Appointment apm in findResults.Items)
            {

                if (!String.IsNullOrEmpty(apm.Subject))
                    apm.Subject = apm.Subject.Replace("<", "&lt;").Replace(">", "&gt;");
                if (!String.IsNullOrEmpty(apm.Location))
                    apm.Location = apm.Location.Replace("<", "&lt;").Replace(">", "&gt;");
                if (apm.Body != null && !String.IsNullOrEmpty(apm.Body.Text))
                    apm.Body.Text = apm.Body.Text.Replace("<", "&lt;").Replace(">", "&gt;");

                retList.Add(ConnectAppointment(apm));
            }
            return retList;
        }

        //일정 시간 이후의 일간 리스트 갯수
        public int GetDayListNumber(DateTime date)
        {
            DateTime startDate = date;
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1).AddMilliseconds(-1);
            List<AppointmentItem> retList = new List<AppointmentItem>();
            CalendarView CalView = new CalendarView(startDate, endDate);
            FindItemsResults<Appointment> findResults = service.FindAppointments(WellKnownFolderName.Calendar, CalView);

            return findResults.Items.Count;
        }

        /*
        public AppointmentItem GetDetailAppointment(string Id)
        {
            Appointment apm = Appointment.Bind(service, new ItemId(Id));
            return ConnectAppointment(apm);
        }
        */
        //ICalUid를 사용하여 해당 Appointment 로드
        public AppointmentItem GetDetailAppointmentKey(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            return ConnectAppointment(apm);
        }

        //ICalUid를 사용하여 해당 Appointment의 수락응답
        public AppointmentItem AcceptAppointment(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            Appointment ap = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            ap.Accept(true);
            return GetDetailAppointmentKey(ap.ICalUid);
        }

        //ICalUid를 사용하여 해당 Appointment 미정응답
        public AppointmentItem TentativelyAppointment(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            Appointment ap = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            ap.AcceptTentatively(true);
            return GetDetailAppointmentKey(ap.ICalUid);
        }
        //ICalUid를 사용하여 해당 Appointment 거부응답
        public void DeclineAppointment(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            Appointment ap = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            if (ap.IsCancelled)
            {
                ap.Delete(DeleteMode.MoveToDeletedItems);
            }
            else
            {
                ap.Decline(true);
            }
        }

        //ICalUid를 사용하여 해당 Appointment 삭제
        public void DeleteAppointment(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            Appointment appointment = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            appointment.Delete(DeleteMode.MoveToDeletedItems);
        }

        //ICalUid를 사용하여 해당 Appointment 취소(참석자가 있을 경우)
        public void CancelAppointment(string ICalUid)
        {
            Appointment apm = FindRelatedAppointment(service, ICalUid);
            Appointment appointment = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            appointment.CancelMeeting("This meeting has been canceled");
        }

        //AppointmentItem을 수정
        public void UpdateAppointment(AppointmentItem ai)
        {
            //해당 일정 찾기
            Appointment apm = FindRelatedAppointment(service, ai.ICalUid);
            //Bind
            Appointment appointment = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            //수정
            appointment = ChangeAppointment(ai, appointment);
            appointment.Update(ConflictResolutionMode.AlwaysOverwrite);
        }

        //AppointmentItem을 입력
        public string SetAppointment(AppointmentItem ai)
        {
            Appointment appointment = new Appointment(service);
            if (ai.RequiredAttendees.Count != 0 && !string.IsNullOrEmpty(ai.RequiredAttendees[0].Address))
            {
                // Send the meeting request to all attendees and save a copy in the Sent Items folder.
                appointment = ConnectAppointment(ai);
                appointment.Save(SendInvitationsMode.SendToAllAndSaveCopy);
            }
            else
            {
                // Save the appointment.
                appointment = ConnectAppointment(ai);
                appointment.Save(SendInvitationsMode.SendToNone);
            }

            //ICalUid를 얻기 위한 로직
            PropertyDefinitionBase AppointementIdPropertyDefinition = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "ICalUid", MapiPropertyType.String);
            PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties, AppointementIdPropertyDefinition);
            appointment.Load(propertySet);

            return appointment.ICalUid;
        }

        //되풀이 부분 설정
        public Microsoft.Exchange.WebServices.Data.Recurrence RepeatAppointment(AppointmentItem ai)
        {
            //RepeatItem ri = ai.Recurrence;
            Appointment apm = new Appointment(service);
            //일정 반복
            if (ai.Recurrence == null)
            //반복 없음
            {
                return null;
                //apm.Recurrence = new Recurrence.DailyPattern(ri.StartRepeat.Date, 0);
                //apm.Recurrence.StartDate = ri.StartRepeat.Date;
                //apm.Recurrence.EndDate = ri.StartRepeat.Date;
            }
            else if (ai.Recurrence.Pattern == 1)//매일
            {
                apm.Recurrence = new Microsoft.Exchange.WebServices.Data.Recurrence.DailyPattern(ai.Recurrence.StartDate.Date, 1);
                apm.Recurrence.StartDate = ai.Recurrence.StartDate.Date;
            }
            else if (ai.Recurrence.Pattern == 2)//매주
            {
                DayOfTheWeek[] days = new DayOfTheWeek[7];
                days[0] = DayOfTheWeek.Sunday;
                days[1] = DayOfTheWeek.Monday;
                days[2] = DayOfTheWeek.Tuesday;
                days[3] = DayOfTheWeek.Wednesday;
                days[4] = DayOfTheWeek.Thursday;
                days[5] = DayOfTheWeek.Friday;
                days[6] = DayOfTheWeek.Saturday;

                apm.Recurrence = new Microsoft.Exchange.WebServices.Data.Recurrence.WeeklyPattern(ai.Recurrence.StartDate.Date, 1, days[Convert.ToInt16(ai.Recurrence.StartDate.DayOfWeek)]);
                apm.Recurrence.StartDate = ai.Recurrence.StartDate.Date;
            }
            else if (ai.Recurrence.Pattern == 3)//매월
            {
                apm.Recurrence = new Microsoft.Exchange.WebServices.Data.Recurrence.MonthlyPattern(ai.Recurrence.StartDate.Date, 1, ai.Recurrence.StartDate.Day);
                apm.Recurrence.StartDate = ai.Recurrence.StartDate.Date;
            }
            //반복 종료 설정
            if (!ai.Recurrence.HasEnd)
            {
                apm.Recurrence.NeverEnds();
            }
            else
            {
                apm.Recurrence.EndDate = ai.Recurrence.EndDate;
            }
            return apm.Recurrence;
        }

        //인증서 검증
        private static bool CertificateValidationCallBack(
                                                            object sender,
                                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
        //URL 검증
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------- Task ------------------------------------------------------------------//
        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Author : 최규연
        /// CreateDate : 2013.08.28
        /// Desc : Exchange Web Service Managed API를 이용한 Wrapper Class - Task
        /// </summary>

        //Task를 TaskItem으로 바인딩
        public TaskItem ConnectTask(Microsoft.Exchange.WebServices.Data.Task task)
        {
            TaskItem taskItem = new TaskItem();
            taskItem.Subject = task.Subject;
            taskItem.Body = task.Body;
            taskItem.Body.BodyType = BodyType.Text;
            if (!string.IsNullOrEmpty(taskItem.Body.Text))
            {
                taskItem.Body.Text = taskItem.Body.Text.Replace("<br>", "\n");
            }
            if (task.StartDate == null && task.DueDate == null)
            {
                taskItem.Nodeadline = true;
            }
            else
            {
                taskItem.Nodeadline = false;
            }
            taskItem.StartDate = task.StartDate;
            taskItem.DueDate = task.DueDate;
            taskItem.Importance = Convert.ToInt16(task.Importance);
            taskItem.Status = task.Status;
            taskItem.UniqueId = task.Id.UniqueId;
            taskItem.ChangeKey = task.Id.ChangeKey;
            return taskItem;
        }

        //TaskItem을 Task로 바인딩
        public Microsoft.Exchange.WebServices.Data.Task ConnectTask(TaskItem taskItem)
        {
            Microsoft.Exchange.WebServices.Data.Task task = null;
            // Create the task item and set property values.
            if (string.IsNullOrEmpty(taskItem.UniqueId))
            {
                task = new Microsoft.Exchange.WebServices.Data.Task(service);
            }
            else
            {
                task = Microsoft.Exchange.WebServices.Data.Task.Bind(service, new ItemId(taskItem.UniqueId));
            }

            task.Subject = taskItem.Subject;
            if (!string.IsNullOrEmpty(taskItem.Body.Text))
            {
                taskItem.Body.BodyType = BodyType.Text;
                taskItem.Body.Text = taskItem.Body.Text.Replace("\n", "<br>");
            }
            task.Body = taskItem.Body;

            //deadline이 정해져 있지 않으면
            if (taskItem.Nodeadline)
            {
                task.StartDate = null;
                task.DueDate = null;
            }
            //현재보다 이른 데드라인이면
            else if (taskItem.DueDate != null && taskItem.StartDate != null)
            {
                if (taskItem.DueDate.Value.CompareTo(taskItem.StartDate.Value) < 0)
                {
                    task.StartDate = taskItem.DueDate;
                    task.DueDate = taskItem.DueDate;
                }
                else
                {
                    task.StartDate = taskItem.StartDate;
                    task.DueDate = taskItem.DueDate;
                }
            }
            else
            {
                task.StartDate = taskItem.StartDate;
                task.DueDate = taskItem.DueDate;
            }

            task.Status = taskItem.Status;
            Importance[] importance = new Importance[3];
            importance[0] = Importance.Low;
            importance[1] = Importance.Normal;
            importance[2] = Importance.High;
            task.Importance = importance[taskItem.Importance];
            return task;
        }

        //UniqueId를 사용하여 Task 탐색
        private Microsoft.Exchange.WebServices.Data.Task FindRelatedTask(ExchangeService service, string UniqueId)
        {
            SearchFilter relatedFilter = new SearchFilter.IsEqualTo(ItemSchema.Id, new ItemId(UniqueId));
            ItemView view = new ItemView(1) { PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Id) };
            FindItemsResults<Item> task = service.FindItems(WellKnownFolderName.Tasks, relatedFilter, view);

            PropertySet ps = new PropertySet(TaskSchema.Body);
            ps.RequestedBodyType = BodyType.Text;
            ps.Add(TaskSchema.Subject);
            ps.Add(TaskSchema.StartDate);
            ps.Add(TaskSchema.DueDate);
            ps.Add(TaskSchema.Importance);
            ps.Add(TaskSchema.Status);
            ps.Add(TaskSchema.Id);
            //ps.Add(new ExtendedPropertyDefinition(0x300B, MapiPropertyType.Binary));
            service.LoadPropertiesForItems(task, ps);
            return task.Items[0] as Microsoft.Exchange.WebServices.Data.Task;
        }

        //Task Status Update
        public void CompleteTask(string UniqueId)
        {
            Microsoft.Exchange.WebServices.Data.Task task = Microsoft.Exchange.WebServices.Data.Task.Bind(service, new ItemId(UniqueId));
            task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.Completed;
            task.PercentComplete = 100.0;
            //task.CompleteDate = DateTime.Now;
            task.Update(ConflictResolutionMode.AlwaysOverwrite);
        }

        //Task Delete
        public void DeleteTask(string UniqueId)
        {
            //Appointment apm = FindRelatedAppointment(service, ICalUid);
            //Appointment appointment = Appointment.Bind(service, new ItemId(apm.Id.UniqueId));
            //appointment.Delete(DeleteMode.MoveToDeletedItems);
            Microsoft.Exchange.WebServices.Data.Task.Bind(service, new ItemId(UniqueId)).Delete(DeleteMode.MoveToDeletedItems);
        }

        //DetailTask
        public TaskItem DetailTask(string UniqueId)
        {
            //Microsoft.Exchange.WebServices.Data.Task task = FindRelatedTask(service, UniqueId);
            //PropertyDefinitionBase TaskIdPropertyDefinition = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "Body", MapiPropertyType.String);
            //PropertySet PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, TaskIdPropertyDefinition);

            PropertySet ps = new PropertySet(TaskSchema.Body);
            ps.RequestedBodyType = BodyType.Text;
            ps.Add(TaskSchema.Subject);
            ps.Add(TaskSchema.StartDate);
            ps.Add(TaskSchema.DueDate);
            ps.Add(TaskSchema.Importance);
            ps.Add(TaskSchema.Status);
            ps.Add(TaskSchema.Id);

            Microsoft.Exchange.WebServices.Data.Task task = Microsoft.Exchange.WebServices.Data.Task.Bind(service, new ItemId(UniqueId), ps);

            return ConnectTask(task);
        }

        //Task Set
        public void SetTask(TaskItem taskItem)
        {
            Microsoft.Exchange.WebServices.Data.Task task = ConnectTask(taskItem);
            //task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.InProgress;
            task.Save();
        }

        //Task Update
        public void UpdateTask(TaskItem taskItem)
        {
            //taskItem.ChangeKey = FindRelatedTask(service, taskItem.UniqueId).Id.ChangeKey;
            //Microsoft.Exchange.WebServices.Data.Task task = ConnectTask(taskItem);
            //task.Update(ConflictResolutionMode.AlwaysOverwrite);
            ConnectTask(taskItem).Update(ConflictResolutionMode.AlwaysOverwrite);
        }

        //Search Task
        public List<TaskItem> SearchTask(string searchKey, int sortFilter)
        {
            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            //Subject 검사 필터
            SearchFilter.ContainsSubstring subjectFilter = new SearchFilter.ContainsSubstring(TaskSchema.Subject, searchKey);
            //Body 검사 필터
            SearchFilter.ContainsSubstring bodyFilter = new SearchFilter.ContainsSubstring(TaskSchema.Body, searchKey);

            searchFilterCollection.Add(subjectFilter);
            searchFilterCollection.Add(bodyFilter);

            SearchFilter searchTaskFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, searchFilterCollection.ToArray());

            ItemView itemView = new ItemView(int.MaxValue);
            //sortFilter == 1:시작, 2:기한, 3:우선순위, 4:상태, 5:제목

            if (sortFilter == 1)
            {
                ExtendedPropertyDefinition taskStartProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008104, MapiPropertyType.SystemTime);
                itemView.OrderBy.Add(taskStartProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (sortFilter == 2)
            {
                ExtendedPropertyDefinition taskDueDateProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008105, MapiPropertyType.SystemTime);
                itemView.OrderBy.Add(taskDueDateProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (sortFilter == 3)
            {
                itemView.OrderBy.Add(TaskSchema.Importance, Microsoft.Exchange.WebServices.Data.SortDirection.Descending);
            }
            else if (sortFilter == 4)
            {
                ExtendedPropertyDefinition taskStatusProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008101, MapiPropertyType.Integer);
                itemView.OrderBy.Add(taskStatusProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (sortFilter == 5)
            {
                itemView.OrderBy.Add(TaskSchema.Subject, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else
            {
                //정렬 없음
            }

            List<TaskItem> retList = new List<TaskItem>();

            FindItemsResults<Item> tasks = service.FindItems(WellKnownFolderName.Tasks, searchTaskFilter, itemView);

            if (tasks.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(TaskSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(TaskSchema.Subject);
                ps.Add(TaskSchema.StartDate);
                ps.Add(TaskSchema.DueDate);
                ps.Add(TaskSchema.Importance);
                ps.Add(TaskSchema.Status);
                ps.Add(TaskSchema.Id);
                service.LoadPropertiesForItems(tasks, ps);
            }
            foreach (Microsoft.Exchange.WebServices.Data.Task apm in tasks.Items)
            {
                retList.Add(ConnectTask(apm));
            }
            return retList;
        }
        ////메인에서 Task List 호출
        //public List<TaskItem> GetTaskList(int searchFilter, int taskSortFilter)
        //{
        //    List<TaskItem> retList = new List<TaskItem>();

        //    List<SearchFilter> searchFilterCollection = new List<SearchFilter>();

        //    //진행중
        //    ExtendedPropertyDefinition taskCompleteProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x0000811C, MapiPropertyType.Boolean);
        //    SearchFilter.IsNotEqualTo inProgress = new SearchFilter.IsNotEqualTo(taskCompleteProp, true);

        //    ItemView itemView = new ItemView(int.MaxValue);
        //    //sortFilter == 데드라인순
        //    ExtendedPropertyDefinition taskDelayProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008105, MapiPropertyType.SystemTime);
        //    itemView.OrderBy.Add(taskDelayProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);

        //    FindItemsResults<Item> tasks = service.FindItems(WellKnownFolderName.Tasks, inProgress, itemView);

        //    if (tasks.Items.Count > 0)
        //    {
        //        PropertySet ps = new PropertySet(TaskSchema.Body);
        //        ps.RequestedBodyType = BodyType.Text;
        //        ps.Add(TaskSchema.Subject);
        //        ps.Add(TaskSchema.StartDate);
        //        ps.Add(TaskSchema.DueDate);
        //        ps.Add(TaskSchema.Importance);
        //        ps.Add(TaskSchema.Status);
        //        ps.Add(TaskSchema.Id);
        //        service.LoadPropertiesForItems(tasks, ps);
        //    }
        //    foreach (Microsoft.Exchange.WebServices.Data.Task apm in tasks.Items)
        //    {
        //        retList.Add(ConnectTask(apm));
        //    }
        //    return retList;
        //}

        //Task List Load
        public List<TaskItem> GetTaskList(int taskSortFilter)
        {
            List<TaskItem> retList = new List<TaskItem>();

            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();

            ////searchFilter == 1:진행중, 2:지연, 3:완료, 4:없음
            //if (taskSearchFilter == 1)
            //{
            //    ExtendedPropertyDefinition taskCompleteProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x0000811C, MapiPropertyType.Boolean);
            //    SearchFilter.IsNotEqualTo inProgress = new SearchFilter.IsNotEqualTo(taskCompleteProp, true);
            //    searchFilterCollection.Add(inProgress);

            //}
            //else if (taskSearchFilter == 2)
            //{
            //    ExtendedPropertyDefinition taskDelayProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008105, MapiPropertyType.SystemTime);
            //    SearchFilter.IsLessThan inDelay = new SearchFilter.IsLessThan(taskDelayProp, DateTime.Now);
            //    searchFilterCollection.Add(inDelay);
            //    //SearchFilter.IsLessThan inDelay = new SearchFilter.IsLessThan(TaskSchema.DueDate, DateTime.Now);
            //    //searchFilterCollection.Add(inDelay);
            //    ExtendedPropertyDefinition taskCompleteProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x0000811C, MapiPropertyType.Boolean);
            //    SearchFilter.IsEqualTo notComplete = new SearchFilter.IsEqualTo(taskCompleteProp, false);
            //    searchFilterCollection.Add(notComplete);
            //}
            //else if (taskSearchFilter == 3)
            //{
            //    ExtendedPropertyDefinition taskCompleteProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x0000811C, MapiPropertyType.Boolean);
            //    SearchFilter.IsEqualTo inComplete = new SearchFilter.IsEqualTo(taskCompleteProp, true);
            //    searchFilterCollection.Add(inComplete);
            //}
            //else
            //{
            //    //조건 없음
            //}

            //SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

            ItemView itemView = new ItemView(int.MaxValue);
            //sortFilter == 1:시작, 2:기한, 3:우선순위, 4:상태, 5:제목

            if (taskSortFilter == 1)
            {
                ExtendedPropertyDefinition taskStartProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008104, MapiPropertyType.SystemTime);
                itemView.OrderBy.Add(taskStartProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (taskSortFilter == 2)
            {
                ExtendedPropertyDefinition taskDueDateProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008105, MapiPropertyType.SystemTime);
                itemView.OrderBy.Add(taskDueDateProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (taskSortFilter == 3)
            {
                itemView.OrderBy.Add(TaskSchema.Importance, Microsoft.Exchange.WebServices.Data.SortDirection.Descending);
            }
            else if (taskSortFilter == 4)
            {
                ExtendedPropertyDefinition taskStatusProp = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.Task, 0x00008101, MapiPropertyType.Integer);
                itemView.OrderBy.Add(taskStatusProp, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else if (taskSortFilter == 5)
            {
                itemView.OrderBy.Add(TaskSchema.Subject, Microsoft.Exchange.WebServices.Data.SortDirection.Ascending);
            }
            else
            {
                //정렬 없음
            }

            FindItemsResults<Item> tasks = service.FindItems(WellKnownFolderName.Tasks, itemView);

            if (tasks.Items.Count > 0)
            {
                PropertySet ps = new PropertySet(TaskSchema.Body);
                ps.RequestedBodyType = BodyType.Text;
                ps.Add(TaskSchema.Subject);
                ps.Add(TaskSchema.StartDate);
                ps.Add(TaskSchema.DueDate);
                ps.Add(TaskSchema.Importance);
                ps.Add(TaskSchema.Status);
                ps.Add(TaskSchema.Id);
                service.LoadPropertiesForItems(tasks, ps);
            }
            foreach (Microsoft.Exchange.WebServices.Data.Task apm in tasks.Items)
            {
                if (!String.IsNullOrEmpty(apm.Subject))
                    apm.Subject = apm.Subject.Replace("<", "&lt;").Replace(">", "&gt;");
                if (apm.Body != null && !String.IsNullOrEmpty(apm.Body.Text))
                    apm.Body.Text = apm.Body.Text.Replace("<", "&lt;").Replace(">", "&gt;");

                retList.Add(ConnectTask(apm));
            }
            return retList;
        }
    }
}
