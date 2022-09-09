using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Common
{
    public class CBHInterface
    {
        public static string CBHMailSend(string ReceiverEmail, string SenderEmail, string Subject, string Content)
        {
            //CBHMSMQHelper helper = new CBHMSMQHelper();
            //CBHMailType data = new CBHMailType();

            //data.ReceiverEmail = ReceiverEmail;
            //data.SenderEmail = SenderEmail;
            //data.Subject = Subject;
            //data.Content = Content;

            //helper.SendMailToQueue(data);
            
            return "";

        }

        public static string CBHMailSend(CBHMailType data)
        {
            //CBHMSMQHelper helper = new CBHMSMQHelper();
            //helper.SendMailToQueue(data);

            return "";
        }

        public static string CBHNoteSend(string ReceiverEmail, string SendUserID, string SenderEmail, string Content)
        {
            CBHMSMQHelper helper = new CBHMSMQHelper();

            CBHNoteType dataNote = new CBHNoteType();
            dataNote.Content = Content;
            dataNote.Kind = "3"; //일반쪽지.
            //data.URL = NoteLink;

            if (SendUserID.Length == 0)
            {
                dataNote.SendUserName = "티끌이";
                dataNote.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
            }
            else
            {
                dataNote.SendUserID = SendUserID;
            }

            string userID = ReceiverEmail.Remove(ReceiverEmail.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            dataNote.TargetUser = userID;

            helper.SendNoteToQueue(dataNote);

            return "";

        }

        public static string CBHNoteSend(CBHNoteType data)
        {
            CBHMSMQHelper helper = new CBHMSMQHelper();
            helper.SendNoteToQueue(data);

            return "";
        }
    }
}
