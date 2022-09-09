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

namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyProfileIframe_View : System.Web.UI.Page
    {
        protected string UserID = string.Empty;     //User ID
        protected string ItemHtml = string.Empty;   //바인딩 될 HTML
        
        /// <summary>
        /// Cache 삭제0
        /// </summary>
        bool _noCache = true;
        protected override void OnPreRender(EventArgs e)
        {
            if (this._noCache)
            {
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetValidUntilExpires(false);
            }
            base.OnPreRender(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //데이터를 조회할 때 사용할 파라미터들을 받아옴
            UserID = Request.Params["UserID"] ?? string.Empty;

            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            GlossaryProfileType data = new GlossaryProfileType();

            //데이터 조회
            data = biz.GlossaryProfileSelect(UserID);

            //Html 바인딩
            //ItemHtml = data.Contents;

            if (data.Contents != null)
            {
                ItemHtml = SKT.Common.CommonActiveSquareEditor.ChangeCutSummaryBox(SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(data.Contents)));

                ItemHtml = ItemHtml.Replace("<p>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");
                ItemHtml = ItemHtml.Replace("<P>", "<P style=\"MARGIN-BOTTOM: 2px; MARGIN-TOP: 5px\">");
            }

        }

        
    }
}