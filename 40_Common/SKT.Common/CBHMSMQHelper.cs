using System.Messaging;
using System;
using SKT.Common;

namespace SKT.Common
{
    public class CBHMSMQHelper : IDisposable
    {
        MessageQueue SKPMailQueue;
        MessageQueue SKPNoteQueue;
        MessageQueue SKPSMSQueue;
        private const string connectionStringName = "ConnPIN";

        public CBHMSMQHelper()
        {
            {//메일
                //1. MSMQ 경로 얻기
                string SKPMailQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKTMailQueuePath"];
                if (string.IsNullOrEmpty(SKPMailQueuePath))
                    SKPMailQueuePath = @".\private$\SKT.Mail";//@"FormatName:DIRECT=OS:.\private$\SKP.Mail";
                //2. MSMQ 서비스에 경로가 없으면 생성
                if (!MessageQueue.Exists(SKPMailQueuePath))
                    MessageQueue.Create(SKPMailQueuePath);
                //3. MSMQ객체 생성
                SKPMailQueue = new MessageQueue(SKPMailQueuePath);
                SKPMailQueue.Formatter = new BinaryMessageFormatter();
            }
            {//쪽지
                string SKPNoteQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKTNoteQueuePath"];
                if (string.IsNullOrEmpty(SKPNoteQueuePath))
                    SKPNoteQueuePath = @".\private$\SKT.Note";//@"FormatName:DIRECT=OS:.\private$\SKP.Note";

                if (!MessageQueue.Exists(SKPNoteQueuePath))
                    MessageQueue.Create(SKPNoteQueuePath);

                SKPNoteQueue = new MessageQueue(SKPNoteQueuePath);
                SKPNoteQueue.Formatter = new BinaryMessageFormatter();
            }
            {//SMS
                string SKPSMSQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKTSMSQueuePath"];
                if (string.IsNullOrEmpty(SKPSMSQueuePath))
                    SKPSMSQueuePath = @".\private$\SKT.SMS";//@"FormatName:DIRECT=OS:.\private$\SKP.SMS";

                if (!MessageQueue.Exists(SKPSMSQueuePath))
                    MessageQueue.Create(SKPSMSQueuePath);

                SKPSMSQueue = new MessageQueue(SKPSMSQueuePath);
                SKPSMSQueue.Formatter = new BinaryMessageFormatter();
            }
        }
        public void SendMailToQueue(CBHMailType data)
        {
            Message msg = new Message(data, new BinaryMessageFormatter());
            msg.Recoverable = false;
            msg.AttachSenderId = false;

            string IsTestServer = System.Web.Configuration.WebConfigurationManager.AppSettings["IsTestServer"];
            
            //20140219 특별한 예외 처리로 하드코딩
            //if (IsTestServer.ToUpper() == "Y" && data.ReceiverEmail.ToUpper() != "SKT.P033028@PARTNER.SK.COM" && data.ReceiverEmail.ToUpper() != "SKT.P029840@PARTNER.SK.COM")
            if (IsTestServer.ToUpper() == "Y")
            {
                string IsWeeklyNoteTestEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["IsTestEmail"];
                //data.ReceiverEmail = "tikle@sk.com";
                data.ReceiverEmail = IsWeeklyNoteTestEmail;
            }
            
            //label 표시길이는 255 사이즈 이므로 내용은 100까지만 보여준다.. 이길이가 길어지면 Error 
            string label = string.Format("{0}, {1}, {2}, {3}", data.SenderEmail, data.ReceiverEmail, data.Subject, data.Content);
            if (label.Length > 100)
            {
                label = label.Substring(0, 98) + "..";
            }
            SKPMailQueue.Send(msg, label);
        }
        public void SendNoteToQueue(CBHNoteType data)
        {
            Message msg = new Message(data, new BinaryMessageFormatter());
            msg.Recoverable = false;
            msg.AttachSenderId = false;

            string IsTestServer = System.Web.Configuration.WebConfigurationManager.AppSettings["IsTestServer"];

            //20140219 특별한 예외 처리요청으로 하드코딩
            //if (IsTestServer.ToUpper() == "Y" && data.TargetUser.ToUpper() != "SKT.P033028" && data.TargetUser.ToUpper() != "SKT.P029840")
            if (IsTestServer.ToUpper() == "Y")
            {
                string IsWeeklyNoteTest = System.Web.Configuration.WebConfigurationManager.AppSettings["IsTestEmail"];
                //data.TargetUser = "SKT.P033028";
                IsWeeklyNoteTest = IsWeeklyNoteTest.Remove(IsWeeklyNoteTest.IndexOf('@'));
                data.TargetUser = IsWeeklyNoteTest.ToUpper();
            }

            //label 표시길이는 255 사이즈 이므로 내용은 100까지만 보여준다.. 이길이가 길어지면 Error 
            string label = string.Format("{0}, {1}, {2}, {4}, {5}", data.SendUserID, data.SendUserName, data.TargetUser, data.URL, data.Kind, data.Content);
            if (label.Length > 100)
            {
                label = label.Substring(0, 98) + "..";
            }
            SKPNoteQueue.Send(msg, label);
        }
        public void SendSMSToQueue(CBHSMSType data)
        {
            string IsTestServer = System.Web.Configuration.WebConfigurationManager.AppSettings["IsTestServer"];

            if (!IsTestServer.ToUpper().Equals("Y"))
            {
                Message msg = new Message(data, new BinaryMessageFormatter());
                msg.Recoverable = false;
                msg.AttachSenderId = false;

                //label 표시길이는 255 사이즈 이므로 내용은 100까지만 보여준다.. 이길이가 길어지면 Error 
                string label = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", data.ConsumerID, data.RplyPhoneNum, data.Phone, data.URL, data.Start_DT_HMS, data.End_DT_HMS, data.Title);
                if (label.Length > 100)
                {
                    label = label.Substring(0, 98) + "..";
                }
                SKPSMSQueue.Send(msg, label);
            }
        }


        public void SendMailFromQueue()
        {
            string SendInfo = string.Empty;
            try
            {
                //1. Queue데이터 추출
                Message msg = SKPMailQueue.Receive();
                CBHMailType data = (CBHMailType)msg.Body;
                SendInfo = "" + msg.Label;

                //2. OUTPUT 설정
                string retValue = string.Empty;	 // 성공여부(정상:HTTP/1.1 200 OK  , 오류:SOAP Fault 메시지)

                //3. 서비스 호출
                using (CBHHelper cbhh = new CBHHelper())
                {
                    retValue = cbhh.SendMail(data.SenderEmail, data.ReceiverEmail, data.Subject, data.Content);
                }

                Log4NetHelper.Info("SendMail Success!! " + retValue + ", " + SendInfo);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(SendInfo, ex);
            }
        }
        public void SendNoteFromQueue()
        {
            string SendInfo = string.Empty;
            try
            {
                //1. Queue데이터 추출
                Message msg = SKPNoteQueue.Receive();
                CBHNoteType data = (CBHNoteType)msg.Body;
                SendInfo = msg.Label;

                //2. OUTPUT 설정
                string retValue = string.Empty;	 // 성공여부(정상:HTTP/1.1 200 OK  , 오류:SOAP Fault 메시지)

                //3. 서비스 호출
                using (CBHHelper cbhh = new CBHHelper())
                {
                    retValue = cbhh.SendNote(data.SendUserID, data.SendUserName, data.TargetUser, data.Content, data.URL, data.Kind);
                }

                Log4NetHelper.Info("SendNote Success!! " + retValue + ", " + SendInfo);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(SendInfo, ex);
            }
        }
        public void SendSMSFromQueue()
        {
            string SendInfo = string.Empty;
            try
            {
                //1. Queue데이터 추출
                Message msg = SKPSMSQueue.Receive();
                CBHSMSType data = (CBHSMSType)msg.Body;
                SendInfo = "" + msg.Label;

                //2. OUTPUT 설정
                string retValue = string.Empty;	 // 성공여부(정상:HTTP/1.1 200 OK  , 오류:SOAP Fault 메시지)

                //3. 서비스 호출
                using (CBHHelper cbhh = new CBHHelper())
                {
                    retValue = cbhh.SendSMS(data.ConsumerID, data.RplyPhoneNum, data.Title, data.Phone, data.URL, data.Start_DT_HMS, data.End_DT_HMS, out data.Uuid);
                }

                Log4NetHelper.Info("SendSMS Success!! " + retValue + ", " + data.Uuid + ", " + SendInfo);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(SendInfo, ex);
            }
        }

        public void DeleteQueue()
        {
            {//메일
                //1. MSMQ 경로 얻기
                string SKPMailQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKPMailQueuePath"];
                if (string.IsNullOrEmpty(SKPMailQueuePath))
                    SKPMailQueuePath = @".\private$\SKT.Mail";//@"FormatName:DIRECT=OS:.\private$\SKP.Mail";
                //2. MSMQ 서비스에 경로가 없으면 삭제
                if (MessageQueue.Exists(SKPMailQueuePath))
                    MessageQueue.Delete(SKPMailQueuePath);
            }
            {//쪽지
                string SKPNoteQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKPNoteQueuePath"];
                if (string.IsNullOrEmpty(SKPNoteQueuePath))
                    SKPNoteQueuePath = @".\private$\SKT.Note";//@"FormatName:DIRECT=OS:.\private$\SKP.Note";

                if (MessageQueue.Exists(SKPNoteQueuePath))
                    MessageQueue.Delete(SKPNoteQueuePath);
            }
            {//SMS
                string SKPSMSQueuePath = System.Configuration.ConfigurationManager.AppSettings["SKPSMSQueuePath"];
                if (string.IsNullOrEmpty(SKPSMSQueuePath))
                    SKPSMSQueuePath = @".\private$\SKT.SMS";//@"FormatName:DIRECT=OS:.\private$\SKP.SMS";

                if (MessageQueue.Exists(SKPSMSQueuePath))
                    MessageQueue.Delete(SKPSMSQueuePath);
            }
        }

        public void Dispose()
        {
        }
    }

    [Serializable()]
    public class CBHMailType
    {
        public string SenderEmail = string.Empty;
        public string ReceiverEmail = string.Empty;
        public string Subject = string.Empty;
        public string Content = string.Empty;
    }

    [Serializable()]
    public class CBHNoteType
    {
        public string SendUserID = string.Empty;
        public string SendUserName = string.Empty;
        public string TargetUser = string.Empty;
        public string Content = string.Empty;
        public string URL = string.Empty;
        public string Kind = string.Empty;
    }

    [Serializable()]
    public class CBHSMSType
    {
        public string ConsumerID = string.Empty;
        public string RplyPhoneNum = string.Empty;
        public string Title = string.Empty;
        public string Phone = string.Empty;
        public string URL = string.Empty;
        public string Start_DT_HMS = string.Empty;
        public string End_DT_HMS = string.Empty;
        public string Uuid = string.Empty;//out
    }
}
