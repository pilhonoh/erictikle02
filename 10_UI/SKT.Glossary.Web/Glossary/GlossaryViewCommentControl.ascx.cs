using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryViewCommentControl : System.Web.UI.UserControl
    {
        public string m_ItemID { get; set; }

        protected string m_UserID { get; set; }

        protected string m_UserName { get; set; }

        protected string RootURL = string.Empty;
        protected bool RootUserID = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            m_ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            string ajax = (Request["AJAX_METHOD"] ?? string.Empty).ToString();

            UserInfo u = new UserInfo(this.Page);

            //댓글 추가
            if (ajax == "INSERT")
            {
                GlossaryCommentType Comment = Newtonsoft.Json.JsonConvert.DeserializeObject<GlossaryCommentType>((Request["Comment"] ?? string.Empty).ToString());
                Comment.UserID = u.UserID;
                Comment.UserName = u.Name;
                Comment.CommonID = m_ItemID;
                Comment.UserEmail = u.EmailAddress;
                Comment.DeptName = u.DeptName;
                Comment.PhotoUrl = u.PhotoUrl;
                Comment.LikeCount = "0";
                Comment.UserIP = u.userIp;
                Comment.UserMachineName = u.userMachineName;

                if (Comment.PublicYN == "Y")
                {
                    Comment.UserID = "";
                    Comment.UserName = "비공개";
                    Comment.DeptName = "비공개부서";
                    Comment.UserEmail = "";
                    Comment.PhotoUrl = "/common/images/user_none.png";
                }
                Comment.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");

                //20140410 보안 조치
                Comment.Contents = MakeURLLink(SecurityHelper.Clear_XSS_CSRF(Comment.Contents).Trim()); // url 등 처리만들기.

                // 2014-06-20 Mr.No
                ScoreRankingType scoreRankingType = ScoreRankingDac.Instance.ScoreRankingSelect(u.UserID);
                Comment.Grade = scoreRankingType.Grade;

                GlossaryCommentBiz biz = new GlossaryCommentBiz();
                if (string.IsNullOrEmpty(Comment.ID))
                {
                    //추가
                    Comment = biz.GlossaryCommentInsert(Comment);
                }
                else
                {
                    //수정 업데이트
                    Comment.LastModifierID = u.UserID;
                    Comment.LastModifierIP = u.userIp;
                    Comment.LastModifierMachineName = u.userMachineName;

                    Comment = biz.GlossaryCommentUpdate(Comment);
                }
                //Comment.Contents = MakeLinkToURL(Comment.Contents); // DB 저장후 다시 링크를 원본으로 변신시켜 화면에 표시해준다.

                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Comment));
                Response.End();
                return;
            }
            if (ajax == "UPDATE")
            {
                GlossaryCommentType Comment = Newtonsoft.Json.JsonConvert.DeserializeObject<GlossaryCommentType>((Request["Comment"] ?? string.Empty).ToString());

                Comment.Contents = MakeURLLink(Comment.Contents); // url 등 처리만들기.

                GlossaryCommentBiz biz = new GlossaryCommentBiz();
                Comment = biz.GlossaryCommentUpdate(Comment);

                Comment.Contents = MakeLinkToURL(Comment.Contents); // DB 저장후 다시 링크를 원본으로 변신시켜 화면에 표시해준다.

                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Comment));
                Response.End();
                return;
            }
            if (ajax == "DELETE")
            {
                GlossaryCommentBiz biz = new GlossaryCommentBiz();
            }

            //좋아요 추가
            if (ajax == "Like")
            {
                GlossaryCommentType Comment = new GlossaryCommentType();
                GlossaryCommentBiz biz = new GlossaryCommentBiz();
                Comment.ID = m_ItemID;
                Comment.CommonID = (Request["CommentItemID"] ?? string.Empty).ToString();
                Comment.UserID = u.UserID;
                Comment.UserIP = u.userIp;
                Comment.UserMachineName = u.userMachineName;
                Comment = biz.GlossaryCommentLikeY(Comment);

                if (Comment.LikeY == "Y")
                {
                    LikeSendNote(m_ItemID, Comment.CommonID, u.Name);
                }
                Response.Write(Comment.LikeY);
                Response.End();
                return;
            }

            //수정 Select
            if (ajax == "SELECT")
            {
                string CommentItemID = (Request["CommentItemID"] ?? string.Empty).ToString();
                GlossaryCommentBiz biz = new GlossaryCommentBiz();
                GlossaryCommentType Board = new GlossaryCommentType();

                Board = biz.GlossaryCommentSelect(CommentItemID);
                Board.Contents = SecurityHelper.Add_XSS_CSRF(MakeLinkToURL(Board.Contents));
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Board));
                Response.End();
                return;
            }

            if (!IsPostBack)
            {
                List(m_ItemID);
            }

            if (u.isAdmin)
            {
                AdminProcess();
            }
        }

        protected void AdminProcess()
        {
        }

        //댓글 리스트
        private void List(string ItemID)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryBiz biz_ = new GlossaryBiz();
            GlossaryControlBiz bizCon = new GlossaryControlBiz();

            GlossaryControlType BoardCon = new GlossaryControlType();
            if (biz_.GlossarySelect(ItemID, "", "reply").UserID == u.UserID)
            {
                RootUserID = true;
            }

            GlossaryCommentBiz biz = new GlossaryCommentBiz();

            ArrayList list = biz.GlossaryCommentList(ItemID);
            for (int i = 0; i < list.Count; i++)
            {
                GlossaryCommentType data = (GlossaryCommentType)list[i];
                data.Contents = data.Contents.Replace("\n", "<br />");
            }
            rptList.DataSource = list;
            rptList.DataBind();
        }

        protected string MakeURLLink(string Contents)
        {
            string strContent = Contents;
            Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)",
                             RegexOptions.IgnoreCase | RegexOptions.Compiled);

            strContent = urlregex.Replace(strContent,
                         "<a href=\"$1\" target=\"_blank\">$1</a>");

            Regex emailregex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)",
                               RegexOptions.IgnoreCase | RegexOptions.Compiled);

            strContent = emailregex.Replace(strContent, "<a href=mailto:$1>$1</a>");

            strContent = strContent.Replace("\n", "<br />");

            return strContent;
        }

        protected string MakeLinkToURL(string Contents)
        {
            string strContent = Contents;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);
            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//a");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {
                    HtmlAgilityPack.HtmlNode textnode = htmlDoc.CreateTextNode(link.InnerText);
                    htmlDoc.DocumentNode.ReplaceChild(textnode, link);
                }
            }
            strContent = htmlDoc.DocumentNode.OuterHtml;
            strContent = strContent.Replace("<br>", "\n");

            return strContent;
        }

        //좋아요 쪽지 보내기
        protected void LikeSendNote(string ItemID, string CommonID, string likeUserName)
        {
            //글 정보
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryType qdata = biz.GlossarySelect(ItemID, "", "selectGlossary");

            //댓글 정보
            GlossaryCommentBiz biz_ = new GlossaryCommentBiz();
            GlossaryCommentType Board = new GlossaryCommentType();
            Board = biz_.GlossaryCommentSelect(CommonID);
            if (Board.UserName != "비공개")
            {
                string Title = qdata.Title;
                string Recipient = Board.UserEmail;

                string NoteTitle = "[T.끌]: '" + Title + "'에 댓글이 추천 되었습니다..";

                string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

                string NoteLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + ItemID;

                string NoteBody = "<html><body><font face=\"맑은고딕\" size=\"2\">안녕하세요, 티끌이입니다. ^^<br /><br />"
                                + Board.UserName + "님께서 <STRONG>&lt;" + Title + "&gt;</STRONG> 에 남겨주신<br />"
                                + "댓글에 "
                                + likeUserName
                                + "님이 감사의 마음을 전하였습니다.<br>"
                                + "<font face=\"맑은고딕\" size=\"2\">▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";

                CBHMSMQHelper helper = new CBHMSMQHelper();
                CBHNoteType data = new CBHNoteType();

                data.Content = NoteBody;
                data.Kind = "3"; //일반쪽지.
                data.URL = NoteLink;
                data.SendUserName = "티끌이";

                string userID = Recipient.Remove(Recipient.IndexOf('@')); //이메일 앞부분이 note id 값이다.
                data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                data.TargetUser = userID;

                //**helper.SendNoteToQueue(data);
            }
        }

        protected void rptList_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            TikleDynamicHtmlList likeTag = new TikleDynamicHtmlList();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btnDeleteComment = (LinkButton)e.Item.FindControl("btnDeleteComment");
                Literal NameDept = (Literal)e.Item.FindControl("itNameDept");
                Literal Modify = (Literal)e.Item.FindControl("itModify");
                Literal Like = (Literal)e.Item.FindControl("litLikeLink");
                
                // 2014-06-16 Mr.No
                GlossaryCommentType glossaryCommentType = (GlossaryCommentType)e.Item.DataItem;
                Literal litUserInfo = (Literal)e.Item.FindControl("litUserInfo");    

                if (RootUserID == true)
                {
                    Like.Visible = false;
                }
                //작성자 공개/비공개
                if (((GlossaryCommentType)e.Item.DataItem).PublicYN.ToString() == "N" || string.IsNullOrEmpty(((GlossaryCommentType)e.Item.DataItem).PublicYN.ToString()))
                {
                    NameDept.Text = "<a href=\"javascript:fnProfileCommontView('" + ((GlossaryCommentType)e.Item.DataItem).UserID + "');\">" + ((GlossaryCommentType)e.Item.DataItem).UserName + " / " + ((GlossaryCommentType)e.Item.DataItem).DeptName + "</a>";
                }
                else
                {
                    NameDept.Text = "비공개";
                }

                //수정버튼
                if (u.UserID == ((GlossaryCommentType)e.Item.DataItem).UserID.ToString())
                {
                    Modify.Text = "<a href=\"javascript:\" onclick=\"return fnModify(this, '" + ((GlossaryCommentType)e.Item.DataItem).ID + "');\" class=\"btn_s\"><b>편집</b></a>";
                    Like.Visible = false;
                }
                else
                {
                    Like.Visible = true;
                }
                Like.Text = likeTag.qnaCommentLikeLink(((GlossaryCommentType)e.Item.DataItem).ID, ((GlossaryCommentType)e.Item.DataItem).LikeCount, "N");

                if (u.isAdmin)
                {
                    btnDeleteComment.Visible = true;
                }
            }
        }

        public void btnDeleteComment_Click(object sender, CommandEventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryCommentType gcType = new GlossaryCommentType();
            GlossaryCommentBiz gcBiz = new GlossaryCommentBiz();
            gcType.ID = e.CommandArgument.ToString();
            gcType.LastModifierID = u.UserID;
            gcType.LastModifierIP = u.userIp;
            gcType.LastModifierMachineName = u.userMachineName;

            gcBiz.GlossaryCommentDelete(gcType);

            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }
    }
}