using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Common
{
    public static class EHRHelper
    {
        public static string EHRWorkStatus(string Empno)
        {
            try
            {
                //com.sktelecom.erp_prod.ZEHR_PT_LIST_VIEW client = new com.sktelecom.erp_prod.ZEHR_PT_LIST_VIEW();
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("RFCHRTE", "vlcwork");

                //com.sktelecom.erp_prod.ZehrPtListView pp = new com.sktelecom.erp_prod.ZehrPtListView();
                ////pp.IPernr = "1101511";
                //pp.IPernr = Empno;
                //com.sktelecom.erp_prod.ZehrPtListViewResponse rep = client.ZehrPtListView(pp);
                //return rep.EReturn;
                return "";
            }
            catch(Exception ex)
            {
                //return "EHR로 부터 정보를 가져오지못했습니다. 내용:"+ ex.Message;
                return "";
            }
                        
        }
    }
}
