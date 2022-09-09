using SKT.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;
using System.Web;

namespace SKT.PUSHService
{
    public partial class PushService : ServiceBase
    {
        ScheduledTimer st;

        int sleeptime = 1000;  //1000 1초 250 0.25초

        public PushService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log4NetHelper.Info("PUSHService Start!!!");

            try
            {
                st = new ScheduledTimer(0, 60);  //기본은 10분마다한다..  60 1분 / 600 10분 / 300 5분

                TimeSpan StartTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second + 10);
                st.SetTime(StartTime, SendGatheringPush); 
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("OnStart ex", ex);
            }
        }

        //private string getWordByByte(string src, int byteCount)
        //{
        //    System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

        //    byte[] buf = myEncoding.GetBytes(src);

        //    string result = myEncoding.GetString(buf, 0, byteCount);

        //    if( byteCount != myEncoding.GetBytes(result).Length)
        //    {
        //        result = myEncoding.GetString(buf, 0, byteCount + 1);
        //    }

        //    return result;
        //}

        private string SubStringWidthPad(string str, int len)
        {
            // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
            int inCnt = Encoding.Default.GetByteCount(str);
            if (inCnt > len)
            {
                int i = 0;
                for (i = str.Length - 1; inCnt > len; i--)
                {
                    //ANSI 문자는 1, 이외의 문자는 2자리로 계산한다
                    if (str[i] > 0x7f)
                    {
                        inCnt -= 2;
                    }
                    else
                    {
                        inCnt -= 1;
                    }
                }

                // i는 마지막 문자 인덱스이고 substring 의 두번째 파라미터는 길이이기 때문에 + 1 한다.
                str = str.Substring(0, i + 1);
                // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
                inCnt = Encoding.Default.GetByteCount(str);

                //PadRight(len) 이 맞겠지만 유니코드 처리가 되기 때문에 멀티바이트 문자도 1로 
                //처리되어 길이가 일정하지 않기 때문에 아래와 같이 계산하여 Padding한다
                str = str.PadRight(str.Length + len - inCnt) +"...";
            }

            return str;
        }


        public void SendGatheringPush()
        {
            string PUSH_MESSAGE = System.Configuration.ConfigurationManager.AppSettings["PUSH_MESSAGE"];
            string PUSH_MESSAGE_RE = System.Configuration.ConfigurationManager.AppSettings["PUSH_MESSAGE_RE"];
            string PUSH_MESSAGE_RE_RE = System.Configuration.ConfigurationManager.AppSettings["PUSH_MESSAGE_RE_RE"];

            string ConnectionString = ConfigurationManager.ConnectionStrings["ConnGlossary"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            try
            {
                DataSet ds = PushSelect(Con);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            string SendMessage = string.Empty;

                            string strTitle = ds.Tables[0].Rows[i]["Title"].ToString();
                            //제목을 자른다.
                            strTitle = SubStringWidthPad(strTitle, 40);
                            //인코딩 처리
                            strTitle = strTitle.Replace("%", "%25");
                            strTitle = strTitle.Replace("&amp;", "%26");
                            strTitle = strTitle.Replace("+", "%2B");
                            strTitle = strTitle.Replace("&", "%26");
                            
                            string Mode = ds.Tables[0].Rows[i]["Mode"].ToString();
                            if (Mode.Equals("WRITE"))
                            {
                                //SendMessage = string.Format(PUSH_MESSAGE, ds.Tables[0].Rows[i]["GatheringName"].ToString(), ds.Tables[0].Rows[i]["Title"].ToString());
                                SendMessage = string.Format(PUSH_MESSAGE, strTitle);
                            }
                            else if (Mode.Equals("REPLY"))
                            {
                                //SendMessage = string.Format(PUSH_MESSAGE_RE, ds.Tables[0].Rows[i]["GatheringName"].ToString(), ds.Tables[0].Rows[i]["Title"].ToString());
                                SendMessage = string.Format(PUSH_MESSAGE_RE, strTitle);
                            }
                            else if (Mode.Equals("REREPLY"))
                            {
                                //SendMessage = string.Format(PUSH_MESSAGE_RE_RE, ds.Tables[0].Rows[i]["GatheringName"].ToString(), ds.Tables[0].Rows[i]["Title"].ToString());
                                SendMessage = string.Format(PUSH_MESSAGE_RE_RE, strTitle);
                            }

                            string GatheringID = ds.Tables[0].Rows[i]["GatheringID"].ToString();
                            string GatheringName = ds.Tables[0].Rows[i]["GatheringName"].ToString();
                            string CommonID = ds.Tables[0].Rows[i]["CommonID"].ToString();
                            string EmpNo = ds.Tables[0].Rows[i]["TargetUserID"].ToString();

                            //Log4NetHelper.Info(HttpUtility.HtmlEncode(GatheringName));

                            //string SendResult = CallPushServer(GatheringID, HttpUtility.UrlEncode(GatheringName), CommonID, EmpNo, HttpUtility.UrlEncode(SendMessage));
                            string SendResult = CallPushServer(GatheringID, GatheringName, CommonID, EmpNo, SendMessage);

                            PushUpdate(Con, Convert.ToInt64(ds.Tables[0].Rows[i]["ID"]), SendResult);

                            System.Threading.Thread.Sleep(sleeptime);

                        }
                    }
                }


            }
            catch(Exception ex)
            {
                Log4NetHelper.Error(ex.ToString());
            }
        }



        private string CallPushServer(string GatheringID, string GatheringName, string CommonID, string EmpNo, string SendMessage)
        {
            string strResult = string.Empty;

            try
            {
                string PUSH_URL = System.Configuration.ConfigurationManager.AppSettings["PUSH_URL"];

                // POST, GET 보낼 데이터 입력
                StringBuilder dataParams = new StringBuilder();
                dataParams.Append("primitive=SKT_TIKLE_NOTI");
                dataParams.Append("&companyCd=SKT");
                dataParams.Append("&msg=" + SendMessage);
                dataParams.Append("&GatheringID=" + GatheringID);
                dataParams.Append("&GatheringName=" + "[T.끌]" + GatheringName);
                dataParams.Append("&CommonID=" + CommonID);
                dataParams.Append("&sabun=" + EmpNo);
                // 요청 String -> 요청 Byte 변환
                byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(dataParams.ToString());

                Log4NetHelper.Info(dataParams.ToString());
                /////////////////////////////////////////////////////////////////////////////////////
                /* POST */
                // HttpWebRequest 객체 생성, 설정
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PUSH_URL);
                request.Method = "POST";    // 기본값 "GET"
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteDataParams.Length;

                // 요청 Byte -> 요청 Stream 변환
                Stream stDataParams = request.GetRequestStream();
                stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
                stDataParams.Close();

                // 요청, 응답 받기
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 응답 Stream 읽기
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.UTF8);

                // 응답 Stream -> 응답 String 변환
                string xmlString = srReadData.ReadToEnd();
                

                //strResult = xmlString;
                //string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>  <skmo><primitive><![CDATA[SKT_TIKLE_NOTI]]></primitive><result><![CDATA[1000]]></result><resultMessage><![CDATA[서비스 요청 성공]]></resultMessage></skmo>";

                using (StringReader stringReader = new StringReader(xmlString))
                using (XmlTextReader reader = new XmlTextReader(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "result":
                                    strResult += reader.ReadString();
                                    break;
                                case "resultMessage":
                                    strResult += "/" + reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log4NetHelper.Error(ex.ToString());
            }
            return strResult;
            //return "";

        }

        private DataSet PushSelect(SqlConnection Con)
        {
            DataSet ds = new DataSet();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = Con;
                Cmd.CommandText = "up_GlossaryGatheringPush_Select";
                Cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adp = new SqlDataAdapter(Cmd);

                adp.Fill(ds, "Table1");
            }
            return ds;

        }

        private DataSet PushUpdate(SqlConnection Con, Int64 ID, string SendResult)
        {
            DataSet ds = new DataSet();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = Con;
                Cmd.CommandText = "up_GlossaryGatheringPush_Update";
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                Cmd.Parameters.Add("@SendResult", SqlDbType.VarChar, 3000);

                Cmd.Parameters["@ID"].Value = ID;
                Cmd.Parameters["@SendResult"].Value = SendResult;

                SqlDataAdapter adp = new SqlDataAdapter(Cmd);

                adp.Fill(ds, "Table1");
            }
            return ds;

        }

        protected override void OnStop()
        {
            Log4NetHelper.Info("PUSHService Stoped!!!");
        }


        




    }
}



//backspace      %08
//      tab            %09
//      linefeed       %0A
//      creturn        %0D
//      space          %20
//      !              %21
//      "              %22
//      #              %23
//      $              %24
//      %              %25
//      &              %26
//      '              %27
//      (              %28
//      )              %29
//      *              %2A
//      +              %2B
//      ,              %2C
//      -              %2D
//      .              %2E
//      /              %2F
