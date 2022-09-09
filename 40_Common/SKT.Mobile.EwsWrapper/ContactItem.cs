using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Mobile.EwsWrapper
{
    [Serializable]
    public class ContactItem
    {
        public ContactItem()
        {
            //DisplayName = string.Empty;
            Surname = string.Empty;
            GivenName = string.Empty;
            //Department = string.Empty;
            //JobTitle = string.Empty;
            EmailAddresses = new List<EmailAddressItem>();
            //CompanyName = string.Empty;
            //CompanyMainPhone = string.Empty;
            //Attachments = new AttachmentItem();
            //HasPicture = false;
        }

        //public string DisplayName { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        //public string CompanyName { get; set; }
        //public string CompanyMainPhone { get; set; }
        //public string Department { get; set; }
        //public string JobTitle { get; set; }
        public List<EmailAddressItem> EmailAddresses { get; set; }
        //public AttachmentItem Attachments { get; set; }
        //public bool HasPicture { get; set; }
                    
    }
}
