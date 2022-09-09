using System.Threading;
using System.Web.UI;
using SKT.Common;

namespace SKT.Common
{
    public class InitializeCultureHelper
    {
        /// <summary>
        /// Pnet에서 세팅한 Language에 따라서 Culture 정보를 세팅한다. 
        /// </summary>
        public static void Initialize()
        {
            //string UserLang = CookieHelper.GetLangCookie().ToUpper();
            string UserLang = new UserInfo(new Page()).UserLang;
            if (UserLang == "KOR" || UserLang == "KO")
                UserLang = "ko-kr";
            else if (UserLang == "ENG" || UserLang == "EN")
                UserLang = "en-US";
            else // 쿠키에 언어 값이 세팅되지 않은 경우 Default로 한국어로 세팅해 준다. 
                UserLang = "ko-kr";
            //현재 언어를 변경한다.

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(UserLang);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UserLang);
        }
    }
}
