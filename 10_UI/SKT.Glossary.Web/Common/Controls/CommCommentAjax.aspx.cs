using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SKT.Glossary.Biz;
using System.Collections;
using System.Data;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using SKT.Common;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class CommCommentAjax : System.Web.UI.Page
    {

        UserInfo u;
        
        static string UID = string.Empty;
        static string UIP = string.Empty;
        static string UMN = string.Empty;

        //static string UserName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);
           // UserName = u.Name;           
        }

        //댓글목록가져오기
        [WebMethod]
        public static Dictionary<string, object> CommCommentListSelectWeb(string commType, string commIdx, string userID, int pageNum, int pageSize)
        {
           
            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            ds = biz.commCommentListSelect(commType, commIdx, userID, pageNum, pageSize);

            if (ds != null && ds.Tables.Count > 0)
            {
                string changeTag = string.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    changeTag = dr["CONTENTS"].ToString();
                    changeTag = Utility.BREncode2(changeTag);
                    changeTag = MakeURLLink(SecurityHelper.Add_XSS_CSRF(changeTag));
                    dr["CONTENTS"] = changeTag;
                }
                ds.Tables[0].AcceptChanges();
            }
            return Utility.ToJson(ds);
        }

        //댓글등록하기
        [WebMethod(EnableSession = true)]
        public static string CommCommentSaveWeb(string commType, string commIdx, string userID, string contentText, string PublicYN, string idx)
        {
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.COMMENTTYPE = commType;
            Comment.COMMONID = commIdx;
            Comment.LIKECOUNT = "0";
            Comment.ID = idx;
            Comment.PUBLICYN = PublicYN;
            Comment.CONTENTS = contentText;
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();

            //보안 조치
            Comment.CONTENTS = SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS);
            GlossaryControlBiz biz = new GlossaryControlBiz();

            Comment = biz.commCommentInsert(Comment);

            return Comment.ID;
        }
        
        [WebMethod(EnableSession = true)]
        //P097010 BACKUP2
        public static string GlossaryCommCommentSaveWeb(string commType, string commIdx, string userID, string contentText, string PublicYN, string idx, string SendCheck, string GatheringYN, string GatheringID)
        //public static string GlossaryCommCommentSaveWeb(string commType, string commIdx, string userID, string contentText, string PublicYN, string idx, string SendCheck)
        {
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.COMMENTTYPE = commType;
            Comment.COMMONID = commIdx;
            Comment.LIKECOUNT = "0";
            Comment.ID = idx;
            Comment.PUBLICYN = PublicYN;
            Comment.CONTENTS = contentText;
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();

            //보안 조치
            Comment.CONTENTS = SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS);
            GlossaryControlBiz biz = new GlossaryControlBiz();

            Comment = biz.commCommentInsert(Comment);

            //P097010 BACKUP2
            //Author : 개발자-최현미, 리뷰자-윤자영
            //Create Date : 2017.04.06 
            //Desc : 끌.모임 댓글 알림설정한 멤버들에게 쪽지를 발송한다 
            if (GatheringYN.Equals("Y"))
            {
                SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(GatheringID, commIdx, "0", userID, PublicYN, "REPLY");
            }
            else
            {
                /*
                Author : 개발자-김성환D, 리뷰자-진현빈D
                Create Date : 2016.06.01
                Desc : 동일인 체크여부로 쪽지 발송
                */
                if (SendCheck == "Y")
                {
                    GlossaryCommentSendNote(commIdx, userID);
                }
            }

            //모임글에 답변 일 경우 작성자/편집자 모두에게 댓글을 알린다.
            //댓글의 답변에게 댓글을 알린다.

            return Comment.ID;
        }

        public static string GatheringSendMemberCheck(string GatheringID, string CommonID, string SupID, string UserID, string PublicYN, string Mode)
        {
            GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            DataSet dsc = gBiz.GlossaryGatheringMember_Check(GatheringID, CommonID, SupID, UserID, Mode, "Noti");

            if (dsc != null)
            {
                if (dsc.Tables.Count > 0)
                {
                    for (int i = 0; i < dsc.Tables[0].Rows.Count; i++)
                    {
                        GatheringSendNote(GatheringID, dsc.Tables[0].Rows[i]["GatheringName"].ToString(), CommonID, dsc.Tables[0].Rows[i]["Title"].ToString(), dsc.Tables[0].Rows[i]["Mail"].ToString(), dsc.Tables[0].Rows[i]["SenderMail"].ToString(), PublicYN, Mode);
                    }
                }
            }

            //모바일은 신규등록일경우만 발송함
            if (Mode.Equals("WRITE"))
            {
                dsc = gBiz.GlossaryGatheringMember_Check(GatheringID, CommonID, SupID, UserID, Mode, "MobileNoti");

                if (dsc != null)
                {
                    if (dsc.Tables.Count > 0)
                    {
                        for (int i = 0; i < dsc.Tables[0].Rows.Count; i++)
                        {
                            GatheringSendPush(GatheringID, CommonID, SupID, dsc.Tables[0].Rows[i]["UserID"].ToString(), UserID, Mode);
                        }
                    }
                }
            }

            return "";
        }

        public static string GatheringSendPush(string GatheringID, string GlossaryID, string SupID, string TargetUserID, string SenderUserID, string Mode)
        {
            GlossaryGatheringBiz gBiz = new GlossaryGatheringBiz();
            DataSet ds = gBiz.GlossaryGatheringPush(GatheringID, GlossaryID, SupID, TargetUserID, SenderUserID, Mode);
            return "";
        }
        public static string GatheringSendNote(string GatheringID, string GatheringName, string GlossaryID, string Title, string TargetID, string SenderID, string PublicYN, string Mode)
        {
            //CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHNoteType data = new CBHNoteType();
            DataSet ds = new DataSet();
            
            //문구 : [T.끌][모임명] 신규 게시글이 작성되었습니다.  
            //문구 : [T.끌][모임명] 게시글 제목....  에 댓글이 작성되었습니다. 
            //문구 : [T.끌][모임명] 게시글 제목....  에 작성한 댓글에 답변이 작성되었습니다. 

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            string NoteLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + GlossaryID + "&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID + "&PageNum=1&TagTitle=";

            string ContentsStr = string.Empty;
            ContentsStr += "<html><body><font face=\"맑은고딕\" size=\"2\">";
            if (Mode.Equals("WRITE"))
            {
                ContentsStr += "[T.끌][" + GatheringName + "] 신규 게시글이 작성되었습니다. <br /><br />  ";
                ContentsStr += "제목 : " + Title + " <br/><br/><br/>";
            }
            else if (Mode.Equals("REPLY"))
            {
                ContentsStr += "[T.끌][" + GatheringName + "] <br /><br />게시글 '<b>" + Title + "</b>'  에 댓글이 작성되었습니다. <br /><br />  ";
            }
            else if (Mode.Equals("REREPLY"))
            {
                ContentsStr += "[T.끌][" + GatheringName + "] <br /><br />게시글 '<b>" + Title + "</b>'  <br /><br />작성한 댓글에 답글이 작성되었습니다. <br /><br />  ";
            }
            ContentsStr += "<a href=\"" + NoteLink + " \">▶ 게시글 확인하기</a><br />";

            ContentsStr += "</font></body></html>";

            data.Content = ContentsStr;
            data.Kind = "3"; //일반쪽지.
            //data.SendUserName = "티끌이";
            //data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.                            

            string senderID = string.Empty;
            if (PublicYN.ToUpper().Equals("Y"))
            {
                data.SendUserID = "tikle";
                senderID = "tikle@sk.com";
            }
            else
            {
                string userID1 = SenderID.Remove(SenderID.IndexOf('@')); //이메일 앞부분이 note id 값이다.
                data.SendUserID = userID1;
                senderID = SenderID;
            }

            string userID = TargetID.Remove(TargetID.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            data.TargetUser = userID;
            //OK//helper.SendNoteToQueue(data);

            //쪽지 20170802
            CBHInterface.CBHNoteSend(data);

            //메일 20170802
            CBHInterface.CBHMailSend(TargetID, senderID, "T.끌 알림 메일입니다.", ContentsStr);

            return "";
        }

        //댓글수정하기
        [WebMethod(EnableSession = true)]
        public static Dictionary<string, object> CommCommentUpdateWeb(string userID, string contentText, string PublicYN, string idx)
        {
            DataSet ds = new DataSet();
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.ID = idx;
            Comment.PUBLICYN = PublicYN;
            //contentText = SecurityHelper.Clear_SQL_Injection(contentText);
            Comment.CONTENTS = contentText;
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();


            //보안 조치
            Comment.CONTENTS = SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS);


            GlossaryControlBiz biz = new GlossaryControlBiz();

            //수정 업데이트
            ds = biz.commCommentUpdate(Comment);
            if (ds != null && ds.Tables.Count > 0)
            {
                string changeTag = string.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    changeTag = dr["CONTENTS"].ToString();
                    changeTag = Utility.BREncode(changeTag);
                    changeTag = SecurityHelper.Add_XSS_CSRF(changeTag);

                    changeTag = MakeURLLink(changeTag); 


                    dr["CONTENTS"] = changeTag;
                }

                ds.Tables[0].AcceptChanges();
            }


            return Utility.ToJson(ds);
        }

        //댓글삭제하기
        [WebMethod(EnableSession = true)]
        public static string CommCommentDeleteWeb(string commType, string commIdx, string userID, string idx)
        {
            
            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.ID = idx;
            Comment.COMMENTTYPE = commType;
            Comment.COMMONID = commIdx;
            Comment.LASTMODIFIERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.LASTMODIFIERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.LASTMODIFIERMACHINENAME = System.Net.Dns.GetHostName();

            ds = biz.commCommentDelete(Comment);
            string DBFLAG = "-1";

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DBFLAG = dr["DBFLAG"].ToString();
                    
                }
            }
            return DBFLAG;
        }


        //댓글내용상세
        [WebMethod]
        public static Dictionary<string, object> CommCommentSelectWeb(string commType, string commIdx, string userID, string idx)
        {

            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            ds = biz.commCommentSelect(commType, commIdx, userID, idx);

            string changeTag = string.Empty;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                changeTag = dr["CONTENTS"].ToString();
                changeTag = Utility.BREncode(changeTag);
                changeTag = SecurityHelper.Add_XSS_CSRF(changeTag);


                changeTag = MakeLinkToURL(changeTag);


                dr["CONTENTS"] = changeTag;
            }

            ds.Tables[0].AcceptChanges();



            return Utility.ToJson(ds);
        }


        //댓글카운트
        [WebMethod]
        public static Dictionary<string, object> CommCommentCountWeb(string commType, string commIdx)
        {

            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            ds = biz.commCommentCountSelect(commType, commIdx);
            return Utility.ToJson(ds);
        }


        
        //댓글삭제하기
        [WebMethod(EnableSession = true)]
        public static Dictionary<string, object> CommCommentLikeWeb(string idx)
        {
            
            
            GlossaryControlBiz biz = new GlossaryControlBiz();
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.ID = idx;
            Comment.LIKECOUNT = "Y";
            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();


            DataSet ds = new DataSet();
            ds = biz.commCommentCountLike(Comment);
            return Utility.ToJson(ds);
        }


        //댓글삭제하기
        [WebMethod(EnableSession = true)]
        public static Dictionary<string, object> CommCommentBestWeb(string idx)
        {


            GlossaryControlBiz biz = new GlossaryControlBiz();
            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.ID = idx;
            Comment.BESTREPLYYN = "Y";
            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();


            DataSet ds = new DataSet();
            ds = biz.commCommentCountBest(Comment);
            return Utility.ToJson(ds);
        }

        //댓글등록하기
        [WebMethod(EnableSession = true)]
        //P097010 BACKUP2
        //public static Dictionary<string, object> CommCommentSupSaveWeb(string commType, string commIdx, string userID, string contentText, string PublicYN, string idx)
        public static Dictionary<string, object> CommCommentSupSaveWeb(string commType, string commIdx, string userID, string contentText, string PublicYN, string idx, string GatheringYN, string GatheringID)
        {

            CommCommentType Comment = new CommCommentType();

            Comment.USERID = HttpContext.Current.Session["UserID"].ToString();
            Comment.COMMENTTYPE = commType;
            Comment.COMMONID = commIdx;
            Comment.LIKECOUNT = "0";
            Comment.ID = idx;
            Comment.PUBLICYN = PublicYN;
            Comment.CONTENTS = SKT.Common.Utility.BREncode2(contentText);
            Comment.USERIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Comment.USERMACHINENAME = System.Net.Dns.GetHostName();

            //보안 조치
            Comment.CONTENTS = MakeURLLink(SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS));
            
            GlossaryControlBiz biz = new GlossaryControlBiz();

            DataSet ds = new DataSet();
            ds = biz.commCommentSupInsert(Comment);

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string changeTag = ds.Tables[0].Rows[i]["CONTENTS"].ToString();
                    changeTag = SKT.Common.Utility.BREncode2(changeTag);
                    changeTag = MakeURLLink(SKT.Common.SecurityHelper.Add_XSS_CSRF(changeTag));

                    ds.Tables[0].Rows[i]["CONTENTS"] = changeTag;
                }
                ds.AcceptChanges();
            }
            
            ////P097010 BACKUP2
            ////Author : 개발자-최현미, 리뷰자-윤자영
            ////Create Date : 2017.04.06 
            ////Desc : 끌.모임 댓글의 답글 알림설정한 멤버들에게 쪽지를 발송한다 
            //if (GatheringYN.Equals("Y"))
            //{
            //    SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(GatheringID, commIdx, idx, userID, PublicYN, "REREPLY");
            //}

            return Utility.ToJson(ds);
        }

        public static string MakeURLLink(string Contents)
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


        public static string MakeLinkToURL(string Contents)
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

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.06.01
        Desc : 댓글 쪽지 발송하기
        */
        public static string GlossaryCommentSendNote(string GlossaryID, string commentWriterID)
        {
            //CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHNoteType data = new CBHNoteType();
            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.UserGlossaryInfo(GlossaryID, commentWriterID);

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            string NoteLink = BaseURL + "Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + GlossaryID;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                    //                    + "<STRONG>&lt;" + ds.Tables[0].Rows[0]["Title"].ToString() + "&gt;</STRONG>"
                    //                    + " 티끌에 대해 "
                    //                    + dr["CommentWriter"].ToString()//UserName
                    //                    + "님이 댓글을 작성하셨습니다.<br />"
                    //                    + "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + dr["Title"].ToString() + "</a><br /></font></body></html>";

                    string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                                        + ds.Tables[0].Rows[0]["KOREANNAME"].ToString() + "이 작성하신 글에 대해 "
                                        + dr["CommentWriter"].ToString() + "이 댓글을 남기셨습니다.<br/><br/>"
                                        + "<a href=\"" + NoteLink + " \">▶ 댓글 확인하기</a><br /></font></body></html>";



                    data.Content = ContentsStr;
                    data.Kind = "3"; //일반쪽지.
                    data.SendUserName = "티끌이";
                    string userID = dr["MAIL"].ToString().Remove(dr["MAIL"].ToString().IndexOf('@')); //이메일 앞부분이 note id 값이다.
                    data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.                            
                    data.TargetUser = userID;
                    //OK//helper.SendNoteToQueue(data);

                    //쪽지 20170802
                    CBHInterface.CBHNoteSend(data);

                    //메일 20170802
                    CBHInterface.CBHMailSend(dr["MAIL"].ToString().Trim(), "tikle@sk.com", "T.끌 알림 메일입니다.", ContentsStr);
                }
            }
            return "";
        }
    }
}