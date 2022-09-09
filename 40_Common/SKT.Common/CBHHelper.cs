using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

using System.Data.SqlClient;
using System.Data;

namespace SKT.Common
{
    public class CBHHelper : IDisposable
    {
        public string SendMail(string SenderEmail, string ReceiverEmail, string Subject, string Content)
        {
            string resultMsg = string.Empty;

            try
            {
                CBH_Mail.MailSenderService Service = new CBH_Mail.MailSenderService();

                //20170413 배포시 추가 - 경로변경
                string Url = ConfigurationManager.AppSettings["MailNoticeWSDLUrl"].ToString();
                //Log4NetHelper.Info("SendNote Url -> " + Url);
                Service.Url = Url;

                Service.Credentials = GetCredential(Service.Url);
                Service.PreAuthenticate = true;
                //타임아웃설정 3초
                Service.Timeout = 3000;

                ReceiverEmail = ReceiverEmail.Replace(";", "").Replace(",","");
                resultMsg = Service.send(SenderEmail, ReceiverEmail, Subject, Content);
                //Insert_CSP_SEND_LOG(SenderEmail, ReceiverEmail, Content, resultMsg);

            }
            catch (Exception ex)
            {
                resultMsg = ex.Message.ToString();
                //Insert_CSP_SEND_LOG(SenderEmail, ReceiverEmail, Content, resultMsg);
            }

            return resultMsg;
        }

        //public void Insert_CSP_SEND_LOG(string SenderEmail, string ReceiverEmail, string Content, string resultMsg)
        //{
        //    try
        //    {
        //        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnGlossary"].ConnectionString;

        //        using (SqlConnection dbcon = new SqlConnection(ConnectionString))
        //        {
        //            dbcon.Open();

        //            SqlCommand cmd = new SqlCommand("up_CSP_Log_Insert", dbcon);

        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@SENDER", SenderEmail);
        //            cmd.Parameters.AddWithValue("@RECEIVER", ReceiverEmail);
        //            cmd.Parameters.AddWithValue("@CSP_MSG", Content);
        //            cmd.Parameters.AddWithValue("@CSP_TYPE", "mail");
        //            cmd.Parameters.AddWithValue("@CSP_RETURN_MSG", resultMsg);

        //            cmd.ExecuteNonQuery();

        //            dbcon.Close();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Log4NetHelper.Info("up_CSP_Log_Insert -> " + ex.Message.ToString());
        //    }
        //}

        public string SendNote(string SendUserID, string SendUserName, string TargetUser, string Content, string URL, string Kind)
        {
            CBH_Note.NoteServiceService Service = new CBH_Note.NoteServiceService();

            //20170413 배포시 추가 - 경로변경
            string Url = ConfigurationManager.AppSettings["NoteNoticeWSDLUrl"].ToString();
            Log4NetHelper.Info("SendNote Url -> " + Url);
            Service.Url = Url;

            Service.Credentials = GetCredential(Service.Url);
            Service.PreAuthenticate = true;
            //타임아웃설정 3초
            Service.Timeout = 3000;

            return Service.sendNote(SendUserID,
                SendUserName,
                TargetUser,
                Content,
                URL,
                Kind);

        }

        public string SendSMS(string ConsumerID, string RplyPhoneNum, string Title, string Phone, string URL, string Start_DT_HMS, string End_DT_HMS, out string Uuid)
        {
            CBH_SMS.SMSSenderService Service = new CBH_SMS.SMSSenderService();

            //20170413 배포시 추가 - 경로변경
            string Url = ConfigurationManager.AppSettings["SMSNoticeWSDLUrl"].ToString();
            //Log4NetHelper.Info("SendNote Url -> " + Url);
            Service.Url = Url;

            Service.Credentials = GetCredential(Service.Url);
            Service.PreAuthenticate = true;
            //타임아웃설정 3초
            Service.Timeout = 3000;

            return Service.send(ConsumerID,
                RplyPhoneNum,
                Title,
                Phone,
                URL,
                Start_DT_HMS,
                End_DT_HMS,
                out Uuid);
        }

        internal CredentialCache GetCredential(string Url)
        {
            string CBHUserName = ConfigurationManager.AppSettings["CBHUserName"].ToString();//soatest
            string CBHPassword = ConfigurationManager.AppSettings["CBHPassword"].ToString();//soatest1

            NetworkCredential myCred = new System.Net.NetworkCredential(CBHUserName, CBHPassword);

            CredentialCache myCache = new CredentialCache();
            myCache.Add(new Uri(Url), "Basic", myCred);

            return myCache;

            //return new System.Net.NetworkCredential(CBHUserName, CBHPassword);
        }

        // I/F Methods
        public void Dispose()
        {
        }
    }
}
