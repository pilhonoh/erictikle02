using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryIframe_View : System.Web.UI.Page
    {
        protected string ItemID = string.Empty;     //해당 정보 ID
        protected string UserID = string.Empty;     //User ID
        protected string Mode = string.Empty;       //Mode
        protected string ItemHtml = string.Empty;   //바인딩 될 HTML
        // A태그에 _blank 넣기 위한 변수
        protected string _attrib = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //데이터를 조회할 때 사용할 파라미터들을 받아옴
            ItemID = Request.Params["ItemID"] ?? string.Empty;
            UserID = Request.Params["UserID"] ?? string.Empty;
            
            //2016-01-13 ksh Iframe 으로 해주면 카운트가 2번 안된다.
            //Mode = Request.Params["Mode"] ?? string.Empty;
            Mode = "Iframe";

            GlossaryType Board = new GlossaryType();
            GlossaryBiz biz = new GlossaryBiz();

            //데이터 조회
            Board = biz.GlossarySelect(ItemID, UserID, Mode);

            //Html 바인딩
            ItemHtml = SKT.Common.CommonActiveSquareEditor.ChangeCutSummaryBox(SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(Board.ContentsModify)));

            
            //IE가 아닐경우
            //<object type="application/x-shockwave-flash" data="http://mobile.skacademy.com/skgp/web/tikled/SKT_VOD.swf" width="720" height="480">
            //<param name="movie" value="http://mobile.skacademy.com/skgp/web/tikled/SKT_VOD.swf" />
            //IE일 경우
            //<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="720" height="480" id="SKT_VOD2151922" align="middle">
            //<param name="movie" value="http://mobile.skacademy.com/skgp/web/tikled/SKT_VOD.swf" />

            if (ItemHtml.IndexOf("flashContent") > -1)
            {
                if (ItemHtml.IndexOf("classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"") > -1)
                {
                    ItemHtml = ItemHtml.Replace("classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"", "classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" type=\"application/x-shockwave-flash\" ");
                }
            }
        }

      


    }
}