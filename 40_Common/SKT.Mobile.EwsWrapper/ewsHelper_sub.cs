using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SKT.Mobile.EwsWrapper
{
    public partial class ewsHelper
    {

        protected class EwsInfo
        {
            public String AdminID { get; set; }
            public String AdminPW { get; set; }
            public String AdminDomain { get; set; }
            public String EwsURL { get; set; }
            public String CurrentEmailAddress { get; set; }
        }
        protected EwsInfo CurrentEwsInfo { get; set; }


        /// <summary>
        /// Ews 타입의 ID를 EwsLegacy 타입의 ID로 변환한다.
        /// </summary>
        /// <param name="ewsid">Ews 타입의 ID</param>
        /// <param name="mailbox">메일박스(메일주소), 공백시 현 계정사용</param>
        /// <returns></returns>
        public String ConvertIdEwsToEwsLegacy(String ewsid, String mailbox = null)
        {
            return ConvertId(IdFormat.EwsId, IdFormat.EwsLegacyId, ewsid);
        }

        public String ConvertId(IdFormat originalType, IdFormat convertedType, String originalId, String mailbox = null, Boolean isarchive = false)
        {
            if (String.IsNullOrEmpty(mailbox)) mailbox = this.CurrentEwsInfo.CurrentEmailAddress;

            AlternateId original = new AlternateId();
            original.Format = originalType;
            original.UniqueId = originalId;
            original.Mailbox = mailbox;
            original.IsArchive = false;
            AlternateIdBase refined = service.ConvertId(original, convertedType);

            //SKT.Mobile.Common.Util.Log4NetHelper.Info(String.Format("첨부파일 ID 변환 {0} to {1} \r\nOriginal:{2}\r\nConverted:{3}\r\n", originalType, convertedType, originalId, (refined as AlternateId).UniqueId));

            return (refined as AlternateId).UniqueId;
        }

        public String GetConvertedAttachmentID(IdFormat originalType, IdFormat convertedType, String attachId, String attachName, String attachContId, String attachContType, String mailbox = null)
        {
            String convertedMessageID = ConvertId(originalType, convertedType, attachId, mailbox);

            StringBuilder sbContent = new StringBuilder();
            sbContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sbContent.Append("   <soapenv:Envelope	xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"	xmlns:m=\"http://schemas.microsoft.com/exchange/services/2006/messages\"	xmlns:t=\"http://schemas.microsoft.com/exchange/services/2006/types\">");
            sbContent.Append("      <soapenv:Body>");
            sbContent.Append("         <m:GetItem>");
            sbContent.Append("            <m:ItemShape>");
            sbContent.Append("               <t:BaseShape>AllProperties</t:BaseShape>                  ");
            sbContent.Append("                  <t:AdditionalProperties>");
            sbContent.Append("                     <t:FieldURI FieldURI=\"item:Attachments\"/>");
            sbContent.Append("                     <t:FieldURI FieldURI=\"item:HasAttachments\"/>");
            sbContent.Append("                  </t:AdditionalProperties>");
            sbContent.Append("               </m:ItemShape>");
            sbContent.Append("               <m:ItemIds>");
            //sbContent.Append("                  <t:ItemId Id=\"AQAQAGptY2hvdGVzdEBzay5jb20ARgAAA9F9kUKWSVFCgvh/7O12mmYHAEW5PtLGh75Cu3mj52QpvdQAV9DyxbEAAADxB57bGYNFSamFmpZtKe1JAAACP6UAAAA=\" />");
            sbContent.Append("                  <t:ItemId Id=\"" + convertedMessageID + "\" />");
            sbContent.Append("            </m:ItemIds>");
            sbContent.Append("         </m:GetItem>");
            sbContent.Append("      </soapenv:Body>");
            sbContent.Append("   </soapenv:Envelope>");
            byte[] bytesContent = Encoding.UTF8.GetBytes(sbContent.ToString());

            HttpWebRequest req = WebRequest.Create(this.CurrentEwsInfo.EwsURL) as HttpWebRequest;
            req.UseDefaultCredentials = false;
            req.Credentials = new NetworkCredential(this.CurrentEwsInfo.AdminID, this.CurrentEwsInfo.AdminPW, this.CurrentEwsInfo.AdminDomain);
            req.Method = "POST";
            req.ContentType = "text/xml; charset=utf-8";
            req.Accept = "text/xml";
            req.UserAgent = "MobileTnetMailAttachHandler";
            req.ContentLength = bytesContent.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bytesContent, 0, bytesContent.Length);
                reqStream.Close();
            }
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            //String message = String.Empty;
            XmlDocument xDoc = new XmlDocument();
            using (StreamReader sr = new StreamReader(res.GetResponseStream()))
            {
                //message = sr.ReadToEnd();
                xDoc.Load(sr.BaseStream);
                sr.BaseStream.Close();
                sr.Close();
            }

            SKT.Mobile.Common.Util.Log4NetHelper.Info(xDoc.OuterXml);

            String convertedFileAttachmentID = String.Empty;

            Boolean isUnder2007 = false;
            if (xDoc["soap:Envelope"] != null) isUnder2007 = true;

            String result = String.Empty;
            if (isUnder2007)
                result = xDoc["soap:Envelope"]["soap:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:ResponseCode"].InnerText;
            else
                result = xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:ResponseCode"].InnerText;
            if (result.ToLower() == "noerror")
            {
                XmlNodeList attaches = null;
                if (isUnder2007)
                {
                    if (xDoc["soap:Envelope"]["soap:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:Message"] != null)
                    {
                        attaches = xDoc["soap:Envelope"]["soap:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:Message"]["t:Attachments"].ChildNodes;
                    }
                    else
                    {
                        attaches = xDoc["soap:Envelope"]["soap:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:MeetingRequest"]["t:Attachments"].ChildNodes;
                    }
                }
                else
                {
                    if (xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:Message"] != null)
                    {
                        attaches = xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:Message"]["t:Attachments"].ChildNodes;
                    }
                    else
                    {
                        attaches = xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:MeetingRequest"]["t:Attachments"].ChildNodes;
                    }
                }

                foreach (XmlNode attach in attaches)
                {
                    XmlElement current = null;
                    String id = attach["t:AttachmentId"].Attributes["Id"].Value;
                    String name = String.Empty;
                    current = attach["t:Name"];
                    if (current != null) name = current.InnerText;
                    String contenttype = String.Empty;
                    current = attach["t:ContentType"];
                    if (current != null) contenttype = current.InnerText;
                    String contentid = String.Empty;
                    current = attach["t:ContentId"];
                    if (current != null) contentid = current.InnerText;

                    if (String.IsNullOrEmpty(attachContId) || attachContId.ToLower() == "null") attachContId = String.Empty;
                    if (String.IsNullOrEmpty(attachContType) || attachContType.ToLower() == "null") attachContType = String.Empty;
                    if (String.IsNullOrEmpty(attachName) || attachName.ToLower() == "null") attachName = String.Empty;

                    if (name == attachName && contenttype == attachContType && contentid == attachContId)
                    {
                        convertedFileAttachmentID = id;
                        break;
                    }
                }
            }
            return convertedFileAttachmentID;

        }


        public List<Dictionary<String, String>> GetConvertedAttachmentInfo(IdFormat originalType, IdFormat convertedType, String attachId, String attachName, String attachContId, String attachContType, String mailbox = null)
        {
            String convertedMessageID = ConvertId(originalType, convertedType, attachId, mailbox);

            StringBuilder sbContent = new StringBuilder();
            sbContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sbContent.Append("   <soapenv:Envelope	xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"	xmlns:m=\"http://schemas.microsoft.com/exchange/services/2006/messages\"	xmlns:t=\"http://schemas.microsoft.com/exchange/services/2006/types\">");
            sbContent.Append("      <soapenv:Body>");
            sbContent.Append("         <m:GetItem>");
            sbContent.Append("            <m:ItemShape>");
            sbContent.Append("               <t:BaseShape>AllProperties</t:BaseShape>                  ");
            sbContent.Append("                  <t:AdditionalProperties>");
            sbContent.Append("                     <t:FieldURI FieldURI=\"item:Attachments\"/>");
            sbContent.Append("                     <t:FieldURI FieldURI=\"item:HasAttachments\"/>");
            sbContent.Append("                  </t:AdditionalProperties>");
            sbContent.Append("               </m:ItemShape>");
            sbContent.Append("               <m:ItemIds>");
            //sbContent.Append("                  <t:ItemId Id=\"AQAQAGptY2hvdGVzdEBzay5jb20ARgAAA9F9kUKWSVFCgvh/7O12mmYHAEW5PtLGh75Cu3mj52QpvdQAV9DyxbEAAADxB57bGYNFSamFmpZtKe1JAAACP6UAAAA=\" />");
            sbContent.Append("                  <t:ItemId Id=\"" + convertedMessageID + "\" />");
            sbContent.Append("            </m:ItemIds>");
            sbContent.Append("         </m:GetItem>");
            sbContent.Append("      </soapenv:Body>");
            sbContent.Append("   </soapenv:Envelope>");
            byte[] bytesContent = Encoding.UTF8.GetBytes(sbContent.ToString());

            HttpWebRequest req = WebRequest.Create(this.CurrentEwsInfo.EwsURL) as HttpWebRequest;
            req.UseDefaultCredentials = false;
            req.Credentials = new NetworkCredential(this.CurrentEwsInfo.AdminID, this.CurrentEwsInfo.AdminPW, this.CurrentEwsInfo.AdminDomain);
            req.Method = "POST";
            req.ContentType = "text/xml; charset=utf-8";
            req.Accept = "text/xml";
            req.UserAgent = "MobileTnetMailAttachHandler";
            req.ContentLength = bytesContent.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bytesContent, 0, bytesContent.Length);
                reqStream.Close();
            }
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            //String message = String.Empty;
            XmlDocument xDoc = new XmlDocument();
            using (StreamReader sr = new StreamReader(res.GetResponseStream()))
            {
                //message = sr.ReadToEnd();
                xDoc.Load(sr.BaseStream);
                sr.BaseStream.Close();
                sr.Close();
            }

            SKT.Mobile.Common.Util.Log4NetHelper.Info(xDoc.OuterXml);



            List<Dictionary<String, String>> attachInfoList = new List<Dictionary<string, string>>();


            String result = xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:ResponseCode"].InnerText;
            if (result.ToLower() == "noerror")
            {
                XmlNodeList attaches = xDoc["s:Envelope"]["s:Body"]["m:GetItemResponse"]["m:ResponseMessages"]["m:GetItemResponseMessage"]["m:Items"]["t:Message"]["t:Attachments"].ChildNodes;

                foreach (XmlNode attach in attaches)
                {
                    XmlElement current = null;
                    String id = attach["t:AttachmentId"].Attributes["Id"].Value;
                    String name = String.Empty;
                    current = attach["t:Name"];
                    if (current != null) name = current.InnerText;
                    String contenttype = String.Empty;
                    current = attach["t:ContentType"];
                    if (current != null) contenttype = current.InnerText;
                    String contentid = String.Empty;
                    current = attach["t:ContentId"];
                    if (current != null) contentid = current.InnerText;

                    if (String.IsNullOrEmpty(attachContId)) attachContId = String.Empty;
                    if (String.IsNullOrEmpty(attachContType)) attachContType = String.Empty;
                    if (String.IsNullOrEmpty(attachName)) attachName = String.Empty;

                    Dictionary<String, String> attachInfo = new Dictionary<string, string>();
                    attachInfo.Add("ID", id);
                    attachInfo.Add("Name", name);
                    attachInfo.Add("ContentType", attachContType);
                    attachInfo.Add("ContentID", attachContId);
                    attachInfoList.Add(attachInfo);

                }
            }

            return attachInfoList;


        }



    }
}
