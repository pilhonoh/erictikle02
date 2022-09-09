using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Text;
using System.Data;

using System.IO;

using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Configuration;

namespace SKT.Tnet.Framework.Utilities
{
    public class MailHelper : System.IDisposable
    {
        private static SmtpClient smtpClient;
        private static MailMessage mailMessage;

        protected bool dispostedValue = false;

        #region 생성자

        static MailHelper()
        {
            try
            {
                MailHelper.smtpClient = new SmtpClient(ConfigReader.GetString(CoreContants.DEFAULT_SMTP_SERVER));
                smtpClient.Timeout = 5000;
            }
            catch { }
        }

        public MailHelper()
        {
            try
            {
                MailHelper.smtpClient = new SmtpClient(ConfigReader.GetString(CoreContants.DEFAULT_SMTP_SERVER));
                smtpClient.Timeout = 5000;
            }
            catch { }
        }

        public MailHelper(MailAddress from, string mailSubject, string mailBody)
        {
            try
            {
                MailHelper.mailMessage = new MailMessage();
                MailHelper.mailMessage.From = from;

                MailHelper.mailMessage.Body = mailBody;
                MailHelper.mailMessage.IsBodyHtml = true;
                MailHelper.mailMessage.BodyEncoding = Encoding.UTF8;

                MailHelper.mailMessage.Subject = mailSubject;
                MailHelper.mailMessage.SubjectEncoding = Encoding.UTF8;

                MailHelper.smtpClient = new SmtpClient(ConfigReader.GetString(CoreContants.DEFAULT_SMTP_SERVER));
                smtpClient.Timeout = 5000;
            }
            catch { }
        }

        #endregion

        #region 소멸자
        ~MailHelper()
        {
            try
            {
                Dispose(true);

                if (mailMessage != null) mailMessage = null;
                if (smtpClient != null) smtpClient = null;
            }
            catch { }
        }
        #endregion

        #region Dispose
        protected void Dispose(bool disposing)
        {
            try
            {
                if (!this.dispostedValue)
                {
                    if (disposing)
                    {
                        if (mailMessage != null) mailMessage.Dispose();
                        if (smtpClient != null) smtpClient.Dispose();
                    }

                    this.dispostedValue = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        #endregion

        #region SendOne
        public void SendOne(string fromName, string fromAddress, string toName, string toAddress, string subject, string body)
        {
            SendOne(new MailAddress(fromAddress, fromName), new MailAddress(toAddress, toName), subject, body);
        }

        public void SendOne(MailAddress from, MailAddress tomail, string subject, string body)
        {
            try
            {
                if (smtpClient == null) throw new ApplicationException();

                // MailMessage is used to represent the e-mail being sent
                using (mailMessage = new MailMessage())
                {
                    mailMessage.From = from;
                    mailMessage.To.Add(tomail);
                    mailMessage.Subject = subject;
                    mailMessage.SubjectEncoding = Encoding.UTF8;

                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = Encoding.UTF8;

                    smtpClient.Send(mailMessage);
                }
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("메일 주소 형식이 옳바르지 않습니다.", ex);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("메일 전송 프로토콜 오류", ex);
            }
        }

        public void SendOne(string toAddress, string toName = "")
        {
            try
            {
                if (mailMessage == null) throw new ApplicationException();
                if (smtpClient == null) throw new ApplicationException();

                if (string.IsNullOrEmpty(toName) == true || string.IsNullOrWhiteSpace(toName) == true)
                {
                    SendOne(new MailAddress(toAddress));
                }
                else
                {
                    SendOne(new MailAddress(toAddress, toName));
                }
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("메일 주소 형식이 옳바르지 않습니다.", ex);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("메일 전송 프로토콜 오류", ex);
            }
        }

        public void SendOne(MailAddress toMail)
        {
            try
            {
                mailMessage.To.Add(toMail);

                smtpClient.Send(mailMessage);
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("메일 주소 형식이 옳바르지 않습니다.", ex);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("메일 전송 프로토콜 오류", ex);
            }
        }
        #endregion

        /// <summary>
        /// Exchange 버전 체크시 구분값
        /// </summary>
        public enum ExVerGubun
        {
            UserID,
            Email
        }

        /// <summary>
        /// Exchange 버전 체크 함수
        /// </summary>
        /// <param name="userID">사용자 정보</param>
        /// <param name="gubun">검색 구분값 (사번, 이메일)</param>
        /// <returns></returns>
        public static string GetExchageVersion(string userID, ExVerGubun gubun)
        {
            System.Net.HttpWebRequest request = null;
            System.Net.HttpWebResponse response = null;
            Stream swRequest = null;
            byte[] bytes = null;
            StreamReader srResponseReader = null;

            string getUrl = string.Empty;
            string postData = string.Empty; ;
            string strResponseData = string.Empty;


            try
            {
                getUrl = ConfigReader.GetString(CoreContants.INTERFACE_SECTION_NAME, CoreContants.WEBSERVICE_CATEGORY_NAME, "EWS_InformationUrl");
                
                switch(gubun)
                {
                    case ExVerGubun.UserID:
                        postData = String.Format("userid={0}", userID);
                        break;
                    default:
                        postData = String.Format("email={0}", userID);
                        break;
                }

                bytes = System.Text.Encoding.UTF8.GetBytes(postData);
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(getUrl);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language", "ko");

                using (swRequest = request.GetRequestStream())
                {
                    swRequest.Write(bytes, 0, bytes.Length);
                    swRequest.Close();
                }

                response = (System.Net.HttpWebResponse)request.GetResponse();

                using (srResponseReader = new StreamReader(response.GetResponseStream()))
                {
                    strResponseData = srResponseReader.ReadToEnd();
                    srResponseReader.Close();
                }
            }
            catch { }

            if (string.IsNullOrEmpty(strResponseData) == true)
            {
                return string.Empty;
            }
            else
            {
                string[] ewsInfo = strResponseData.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                if (ewsInfo.Length > 0)
                {
                    return ewsInfo[0];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
