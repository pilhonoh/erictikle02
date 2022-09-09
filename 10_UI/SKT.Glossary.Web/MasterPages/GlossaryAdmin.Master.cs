using System;
using System.Collections.Generic;
using System.Configuration;
using SKT.Common;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using System.Web.UI.WebControls;

namespace SKT.Glossary.Web.MasterPages
{
    public partial class GlossaryAdmin : System.Web.UI.MasterPage
    {
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;
        protected string UserID = string.Empty;
        public bool isAdminValue = false;

        public string leftMenuView = string.Empty;
        //public string[] leftMenu = { "통계|/tikleAdmin/stats/tikleTotal.aspx", "메인화면관리|/tikleAdmin/MainMng/tikle.aspx", "Platform 관리|/tikleAdmin/Platform/PlatGlossaryList.aspx", "권한관리|//tikleAdmin/AccessAuth/TikleAccess.aspx" }; //1Depth메뉴
        public string[] leftMenu;
        public string[][] leftMenuSub;

        protected UserInfo u;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);
            UserID = u.UserID;

            //RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;

            //leftMenu = new string[3];
            //leftMenu[0] = "통계|/tikleAdmin/stats/tikleTotal.aspx";
            //leftMenu[1] = "메인화면관리|/tikleAdmin/MainMng/tikle.aspx";
            ////leftMenu[2] = "Platform 관리|/tikleAdmin/Platform/PlatGlossaryList.aspx";
            //leftMenu[2] = "Data Transformation|//tikleAdmin/DigitalTrans/ArraRegist.aspx";

            //leftMenuSub = new string[3][]; //2Depth메뉴

            //leftMenuSub[0] = new string[] { "종합|/tikleAdmin/stats/tikleTotal.aspx", "부서|/tikleAdmin/stats/tikleDept.aspx", "메뉴|/tikleAdmin/stats/tikleMenu.aspx", "T.끌 방문 현황|/tikleAdmin/stats/tikleAccess.aspx", "Weekly 현황|/tikleAdmin/stats/tikleWeeklynote.aspx" };
            //leftMenuSub[1] = new string[] { "메인이미지|/tikleAdmin/MainMng/mainbackimg.aspx", "지식선택|/tikleAdmin/MainMng/tikle.aspx", "메인배너|/tikleAdmin/MainMng/banner.aspx", "SKTizen|/tikleAdmin/MainMng/sktizen.aspx" };
            //////leftMenuSub[1] = new string[] { "지식선택|/tikleAdmin/MainMng/tikle.aspx", "메인배너|/tikleAdmin/MainMng/banner.aspx", "SKTizen|/tikleAdmin/MainMng/sktizen.aspx" };
            ////leftMenuSub[2] = new string[] { "Platform 관리|/tikleAdmin/Platform/PlatGlossaryList.aspx", "Platform UV|/tikleAdmin/Platform/PlatStat.aspx" };

            //leftMenuSub[2] = new string[] { "ARRA 간행물|/tikleAdmin/DigitalTrans/ArraRegist.aspx" };
            ////leftMenuSub[3] = new string[] { "Tikle접속권한|/tikleAdmin/AccessAuth/TikleAccess.aspx", "Weekly접속권한|/tikleAdmin/AccessAuth/WeeklyAccess.aspx" };
           

            if (u.isAdmin || u.isManager || u.isTiklei)
            {
                //LeftMenuWrite(RootURL);
            }
            else
            {
                //20140103 , 관리자 페이지 접근 관련
                if (Request.Path.Split('/')[1].ToUpper() == "TIKLEADMIN")
                {
                    string infomsg = "잘못된 접근 입니다.";
                    Response.Redirect("/Error.aspx?InfoMessage=" + infomsg, false);
                }
            }

            string IsEventadd = ConfigurationManager.AppSettings["IsEventadd"] ?? string.Empty;
            if (IsEventadd == "Y")
            {
                GlossaryPageRequestType gprt = new GlossaryPageRequestType();
                gprt.UserID = string.Empty;
                gprt.Name = string.Empty;
                gprt.SessionID = string.Empty;
                gprt.UrlBefore = string.Empty;
                gprt.UrlCurrent = string.Empty;
                gprt.PathCurrent = string.Empty;
                
                try
                {
                    gprt.UserID = u.UserID;
                    gprt.Name = u.Name;
                    gprt.SessionID = Session.SessionID.ToString();
                    gprt.UrlBefore = Request.UrlReferrer.ToString();
                    gprt.UrlCurrent = Request.Url.ToString();
                    gprt.PathCurrent = Request.Url.LocalPath.ToString();
                }catch { }                              

                GlossaryDac dac = new GlossaryDac();
                dac.InsertEventAttendance(gprt);
            }


        }

        #region 사용안함
        //public string LeftMenuCheck(int index, int subindex,string gb) {
        //    string rtn = "false";
        //    string LeftMenuUrl = Request.Url.LocalPath;
        //    if (gb.Equals("1"))
        //    {
        //        if (LeftMenuUrl.ToUpper().IndexOf(leftMenu[index].ToUpper().Split('|')[1]) >= 0 )
        //        {
        //            rtn = "true";
        //        }
               
        //    }
        //    else {

        //        if (LeftMenuUrl.ToUpper().IndexOf(leftMenuSub[index][subindex].ToUpper().Split('|')[1]) >= 0)
        //        {
        //            rtn = "true";
        //        }
            
        //    }

        //    return rtn;            
        //}
       
        //public void LeftMenuWrite(string Url) {

         
        //    leftMenuView += "<div class='LeftMenuArea'>";
        //    leftMenuView += "   <ul class='LeftMenuBox'>";

        //    string left = string.Empty;
        //    string leftSub = string.Empty;
        //    for(int m = 0 ; m < leftMenu.Length; m++){
        //        left = leftMenu[m];
        //        string[] leftArr = left.Split('|');

        //         //1Depth
        //        leftMenuView += "   <li class='ListStyle'>";




        //        leftMenuView += "       <a href='" + leftArr[1] + "'><strong select='" + LeftMenuCheck(m,-1,"1") + "'>" + (m + 1) + ". " + leftArr[0] + "</strong></a>";

        //        //2Depth
        //        leftMenuView += "       <div class='Left2ListArea'>";
        //        leftMenuView += "           <table cellpadding='0' cellspacing='0'>";

        //        for (int x = 0; x < leftMenuSub[m].Length; x++)
        //        {
        //            leftSub = leftMenuSub[m][x];
        //            string[] leftSubArr = leftSub.Split('|');

        //            leftMenuView += "               <tr>";
        //            leftMenuView += "               	<td class='Left2List'>";
        //            leftMenuView += "               		<a href='" + leftSubArr[1] + "'><strong  select='" + LeftMenuCheck(m, x,"2") + "'> · " + leftSubArr[0] + "</strong></a>";
        //            leftMenuView += "               	</td>";
        //            leftMenuView += "               </tr>";
                   
        //        }
        //        leftMenuView += "           </table>";
        //        leftMenuView += "       </div>";
        //        leftMenuView += "   </li>";
        //    }

        //    leftMenuView += "   </ul>";
        //    leftMenuView += "</div>";

        //}
        #endregion
    }
}