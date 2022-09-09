using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using SKT.Common;
using System.Messaging;
using System.Configuration;
using System.Data.SqlClient;

namespace SKT.CBHService
{
    public partial class CBHService : ServiceBase
    {
        Thread MailWorkingThread;
        Thread NoteWorkingThread;
        Thread SMSWorkingThread;
        ScheduledTimer st;

        int sleeptime = 250;

        public CBHService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log4NetHelper.Info("CBHService OnStart!!!");
            string RemoveQueue = System.Configuration.ConfigurationManager.AppSettings["RemoveQueue"];
            Log4NetHelper.Info("RemoveQueue Config : " + RemoveQueue.ToLower());
            if (RemoveQueue.ToLower().Equals("true"))
            {
                using (CBHMSMQHelper cbhmqh = new CBHMSMQHelper())
                {
                    cbhmqh.DeleteQueue();
                }
                Log4NetHelper.Info("CBHService RemoveQueue!!!");
                this.Stop();
                return;
            }
            this.MailWorkingThread = new Thread(new ThreadStart(MailWorkingMethod));
            this.MailWorkingThread.Start();
            this.NoteWorkingThread = new Thread(new ThreadStart(NoteWorkingMethod));
            this.NoteWorkingThread.Start();
            this.SMSWorkingThread = new Thread(new ThreadStart(SMSWorkingMethod));
            this.SMSWorkingThread.Start();
            Log4NetHelper.Info("CBHService Started!!!");


            //10분마다 HistoryYNCheck
            try
            {
                st = new ScheduledTimer(0,600);  //기본은 10분마다한다..
                
                TimeSpan StartTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second+10); // 시작 시간
                st.SetTime(StartTime, HistoryYNCheck); // SetTime(예약시간, 실행메소드)
                
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("OnStart ex", ex);
            }

        }

        protected override void OnStop()
        {
            Log4NetHelper.Info("CBHService OnStop!!!");
            this.MailWorkingThread.Abort();
            this.NoteWorkingThread.Abort();
            this.SMSWorkingThread.Abort();
            Log4NetHelper.Info("CBHService Stoped!!!");
        }

        public void MailWorkingMethod()
        {
            while (true)
            {
                //Log4NetHelper.Info("CBHService!!!");
                using (CBHMSMQHelper cbhmqh = new CBHMSMQHelper())
                {
                    cbhmqh.SendMailFromQueue();
                }
                System.Threading.Thread.Sleep(sleeptime);
            }
        }
        public void NoteWorkingMethod()
        {
            while (true)
            {
                //Log4NetHelper.Info("CBHService!!!");
                using (CBHMSMQHelper cbhmqh = new CBHMSMQHelper())
                {
                    cbhmqh.SendNoteFromQueue();
                }
                System.Threading.Thread.Sleep(sleeptime);
            }
        }
        public void SMSWorkingMethod()
        {
            while (true)
            {
                //Log4NetHelper.Info("CBHService!!!");
                using (CBHMSMQHelper cbhmqh = new CBHMSMQHelper())
                {
                    cbhmqh.SendSMSFromQueue();
                }
                System.Threading.Thread.Sleep(sleeptime);
            }
        }

        public void HistoryYNCheck()
        {
            //Log4NetHelper.Info("DailyMailCheck Start");

            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["ConnGlossary"].ConnectionString;

                //1. db connect
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                //2. instruction 
                String sql = "[Glossary].[dbo].[up_GlossaryModifyYN_Timer]";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("DailyMailCheckError", ex);
            }
        }
    }
}
