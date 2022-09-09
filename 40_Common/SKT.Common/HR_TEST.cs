//using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace SKT.Common
{
    public class HR_TEST
    {
        //private static RfcDestination destination;
        //private static InMemoryDestinationConfiguration myDestinationConfiguration = new InMemoryDestinationConfiguration();

        //private void SetDestination()
        //{

        //    if (myDestinationConfiguration.Count == 0)
        //    {
        //        #region ---- 개발 서버 연동시 ----
        //        string name = Properties.Settings.Default.name;
        //        string user = Properties.Settings.Default.user;
        //        string passwd = Properties.Settings.Default.passwd;
        //        string client = Properties.Settings.Default.client;
        //        string lang = Properties.Settings.Default.lang;
        //        string ashost = Properties.Settings.Default.ashost;
        //        string sysnr = Properties.Settings.Default.sysnr;
        //        int max_pool_size = Properties.Settings.Default.max_pool_size;
        //        string sysid = Properties.Settings.Default.sysid;
        //        string gwserv = Properties.Settings.Default.gwservice;


        //        myDestinationConfiguration.AddOrEditDestination(name, max_pool_size, user, passwd, lang, client, ashost, sysnr, sysid, gwserv);
        //        #endregion

        //        //운영 서버 연동시 

        //        //string name = Properties.Settings.Default.name;
        //        //string user = Properties.Settings.Default.user;
        //        //string passwd = Properties.Settings.Default.passwd;
        //        //string client = Properties.Settings.Default.client;
        //        //string lang = Properties.Settings.Default.lang;
        //        //string mshost = Properties.Settings.Default.mshost;
        //        //int max_pool_size = Properties.Settings.Default.max_pool_size;
        //        //string sysid = Properties.Settings.Default.sysid;
        //        //string logongroup = Properties.Settings.Default.group;

        //        //myDestinationConfiguration.AddOrEditDestination(name, max_pool_size, user, passwd, lang, client, mshost, sysid, logongroup);

        //    }
        //}

        //public HR_TEST()
        //{

        //    SetDestination();

        //    RfcDestinationManager.RegisterDestinationConfiguration(myDestinationConfiguration);

        //    RfcConfigParameters a = new RfcConfigParameters();


        //    if (destination == null)
        //        destination = RfcDestinationManager.GetDestination("SKT");   // 개발은 SKT_DEV 로

        //    //RfcDestinationManager.UnregisterDestinationConfiguration(myDestinationConfiguration);
        //}

        //public HR_TEST(string dest)
        //{
        //    if (myDestinationConfiguration.Count == 0)
        //        SetDestination();

        //    RfcDestinationManager.RegisterDestinationConfiguration(myDestinationConfiguration);

        //    if (destination == null)
        //        destination = RfcDestinationManager.GetDestination(dest);

        //    //RfcDestinationManager.UnregisterDestinationConfiguration(myDestinationConfiguration);
        //}

        //public Dictionary<string, string> GetHRApprovalCount(string empid)
        //{
        //    Dictionary<string, string> hr = new Dictionary<string, string>();


        //    IRfcFunction function = destination.Repository.CreateFunction("ZEHR_SS_INBOX_LIST_1");

        //    function.SetValue("USER_ID", empid);

        //    function.Invoke(destination);

        //    hr.Add("HR결재", function.GetValue("SUM_NUM") as string);
        //    hr.Add("근무승인", function.GetValue("DUTY_NUM") as string);
        //    hr.Add("출장/교육", function.GetValue("EXPENSE_NUM") as string);
        //    hr.Add("Refresh계획", function.GetValue("REFRESH_NUM") as string);
        //    hr.Add("기타HR승인", function.GetValue("APPROVE_NUM") as string);

        //    //  RfcDestinationManager.UnregisterDestinationConfiguration(myDestinationConfiguration);

        //    return hr;
        //}


        //public Dictionary<string, string> SendMemo(string cheifEmpID, string empID, string createDate, string contents)
        //{
        //    Dictionary<string, string> hr = new Dictionary<string, string>();
        //    IRfcFunction function = destination.Repository.CreateFunction("Z_AP2_MS_031_100M_03");


        //    // HR 에 450 바이트씩 잘라서 전송
        //    const int transferBytes = 450;
        //    byte[] buf = Encoding.UTF8.GetBytes(contents);
        //    int maxCount = buf.Length / transferBytes;
        //    int remainder = buf.Length % transferBytes;

        //    if (remainder > 0)
        //    {
        //        maxCount += 1;
        //    }

        //    //테이블정보 T_RESULT2 LIKE ZSHR_RECORD2 관찰기록(인터페이스)
        //    RfcStructureMetadata metaData = destination.Repository.GetStructureMetadata("ZSHR_RECORD2");
        //    IRfcStructure structItems = metaData.CreateStructure();

        //    foreach (IRfcParameter data in function)
        //    {
        //        if (data.Active && data.Metadata.Direction == RfcDirection.TABLES)
        //        {
        //            IRfcTable table = data.GetTable();
        //            for (int i = 0; i < maxCount; i++)
        //            {
        //                string transferContents = GetWordByByte(contents, i * transferBytes, transferBytes);
        //                structItems.SetValue("PERNR", empID);
        //                structItems.SetValue("FILLD", createDate);
        //                structItems.SetValue("SEQ_NO", i);
        //                structItems.SetValue("VDATA", transferContents);
        //                structItems.SetValue("OPERA", "");
        //                structItems.SetValue("JIKWI", "");
        //                structItems.SetValue("FILLP", cheifEmpID);
        //                table.Append(structItems);
        //            }
        //            function.SetValue("T_RESULT2", table);
        //        }
        //    }

        //    function.Invoke(destination);

        //    hr.Add("ErrorCode", function.GetValue("E_ERROR") as string);
        //    hr.Add("ErrorMsg", function.GetValue("E_TEXT") as string);

        //    return hr;
        //}

        ///// <summary>
        ///// 450 BYTE씩 잘라서 반환
        ///// </summary>
        ///// <param name="src"></param>
        ///// <param name="startIndex"></param>
        ///// <param name="byteCount"></param>
        ///// <returns></returns>
        //private string GetWordByByte(string src, int startIndex, int byteCount)
        //{
        //    byte[] buf = Encoding.UTF8.GetBytes(src);
        //    if (startIndex > buf.Length)
        //        return string.Empty;

        //    if (startIndex + byteCount > buf.Length)
        //        byteCount = buf.Length - startIndex;

        //    return Encoding.UTF8.GetString(buf, startIndex, byteCount);
        //}
    }
}
