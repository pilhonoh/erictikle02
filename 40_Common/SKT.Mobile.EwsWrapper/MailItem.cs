using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Mobile.EwsWrapper
{
    /// <summary>
    /// Author : 이충렬
    /// CreateDate : 2013.08.28
    /// Desc : Mobile Web 메일의 메일 정보 Class
    /// </summary>
    [Serializable]
    public class MailItemList
    {
        public MailItemList()
        {
            TotalCount = 0;
            WellknownFolderName = string.Empty;
            CurrentFolderID = string.Empty;
            CurrentFolderDisplayName = string.Empty;
            MailList = new List<MailItem>();

        }
        public int TotalCount { set; get; }
        public string WellknownFolderName { set; get; }
        public string CurrentFolderID { set; get; }
        public string CurrentFolderDisplayName { set; get; }
        public List<MailItem> MailList { set; get; }
    }

    [Serializable]
    public class MailItem
    {
        public MailItem()
        {
            Subject = string.Empty;
            Body = string.Empty;
            Sender = null;
            Pr_Search_Key = string.Empty;
            Pr_Icon_Index = null;
            IsRead = true;
            IsMeet = false;
            FolderID = string.Empty;
            Sign = new SignatureItem();
            Importance = ImportanceLevel.Normal;
            Attachments = new List<AttachmentItem>();
            ToRecipient = new List<EmailAddressItem>();
            CcRecipient = new List<EmailAddressItem>();
            BccRecipient = new List<EmailAddressItem>();
            MessageID = string.Empty;
            MeetLocation = string.Empty;
        }

        public string Subject { set; get; }
        public string Body { set; get; }
        public EmailAddressItem Sender { set; get; }
        public string Pr_Search_Key { set; get; }
        public int? Pr_Icon_Index { set; get; }
        public bool IsRead { set; get; }
        public bool IsMeet { set; get; }
        public string FolderID { set; get; }
        public SignatureItem Sign { set; get; }
        public bool HasAttachments { set; get; }
        public List<AttachmentItem> Attachments { get; set; }
        public List<EmailAddressItem> ToRecipient { set; get; }
        public List<EmailAddressItem> CcRecipient { set; get; }
        public List<EmailAddressItem> BccRecipient { set; get; }
        public DateTime DateTimeSent { set; get; }
        public String DateTimeSentByString { get { try { SKT.Mobile.Common.Util.FormatterDatetime df = new Common.Util.FormatterDatetime(this.DateTimeSent); return df.DateString; } catch { return String.Empty; } } }
        public DateTime DateTimeForList { set; get; }
        public ImportanceLevel Importance { set; get; }
        public String MessageID { get; set; }
        public DateTime MeetStart { set; get; }
        public DateTime MeetEnd { set; get; }
        public string MeetLocation { set; get; }
    }



    [Serializable]
    public class EmailAddressItem
    {
        public EmailAddressItem()
        {
            Address = string.Empty;
            Id = string.Empty;
            MailboxType = string.Empty;
            Name = string.Empty;
        }

        public string Address { set; get; }
        public string Id { set; get; }
        public string MailboxType { set; get; }
        public string Name { set; get; }


    }
    [Serializable]
    public enum ActionType { LIST = 0, VIEW, WRITE, FORWARD, REPLY, REPLYALL, NEW, MODIFY, RESPONSEMEETINGACCEPT, RESPONSEMEETINGTENTATIVE, RESPONSEMEETINGDECLINE};

    [Serializable]
    public enum ResponseMeetingType { Accept = 0, Tentative, Decline };

    [Serializable]
    public enum ResponseMeetingActionType { NotSendMail = 0, SendMail, SendCustomMail };

    [Serializable]
    public enum ImportanceLevel
    {
        Low, Normal, High
    }

    [Serializable]
    public class AttachmentItem
    {
        public AttachmentItem()
        {
            IsContactPicture = false;
            IsInline = false;
            ContentId = string.Empty;
            Name = string.Empty;
            ServerPath = string.Empty;
            ContentType = string.Empty;
            Size = 0;
            AttachId = string.Empty;
            MessageId = string.Empty;
        }
        public bool IsContactPicture { get; set; }
        public bool IsInline { get; set; }
        public string ContentId { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
        public string ServerPath { get; set; }
        public int Size { get; set; }
        public string AttachId { get; set; }
        public string MessageId { get; set; }

    }

    public class MailPathInfo
    {
        public MailPathInfo()
        {
            MailAttachFolderPath = string.Empty;
            MailInlineFolderPath = string.Empty;
            MailImageServicePage = string.Empty;
            MailDomainName = string.Empty;
        }

        public string MailAttachFolderPath { get; set; }
        public string MailInlineFolderPath { get; set; }
        public string MailImageServicePage { get; set; }
        public string MailDomainName { get; set; }
    }



}
