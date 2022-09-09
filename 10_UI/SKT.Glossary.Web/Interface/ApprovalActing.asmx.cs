using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;
using System.Data.Common;

using System.IO;
using System.Xml;

namespace SKT.Glossary.Web.Interface
{
    /// <summary>
    /// ApprovalActing의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class ApprovalActing : System.Web.Services.WebService
    {
        [WebMethod]
        public string SetActingInfo(string T_FLAG, string APPCD, string EMPNO, string DEPT, string ACTING_EMPNO , string ACTING_DEPT, string STARTDAY, string ENDDAY)
        {
            string strResult = "false";

            WeeklyDelegateDuty _duty = new WeeklyDelegateDuty();
            WeeklyBiz biz = new WeeklyBiz();

            _duty.T_FLAG = T_FLAG;
            _duty.APPCD = APPCD;
            _duty.EMPNO = EMPNO;
            _duty.DEPT = DEPT;
            _duty.ACTING_EMPNO = ACTING_EMPNO;
            _duty.ACTING_DEPT = ACTING_DEPT;
            _duty.STARTDAY = STARTDAY;
            _duty.ENDDAY = ENDDAY;

            try
            {
                int iResult = biz.Weekly_Insert_UserAbsence_New(_duty);

                if (iResult > 0)
                    strResult = "true";
                else
                    strResult = "false";
            }
            catch(Exception ex)
            {
                strResult = "false";
            }

            return strResult;
        }
    }
}
