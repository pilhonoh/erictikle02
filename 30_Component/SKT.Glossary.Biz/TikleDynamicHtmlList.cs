using System;
using System.Text;
using System.Data;
using System.Collections;

using SKT.Glossary.Type;

using System.Configuration;

namespace SKT.Glossary.Biz
{
    public class TikleDynamicHtmlList
    {
        public string UsernameLink(DataRow dr, string mode)  //사용자 이름과 부서 태그 
        {
            string usernameLink = string.Empty;
            if(mode == "contents")
            {
                usernameLink = "<a href=\"javascript:fnProfileView('" + dr["UserID"].ToString() + "');\">" + dr["UserName"].ToString() + " / " + dr["DeptName"].ToString() + "</a>";
            }else if(mode == "comment")
            {
                usernameLink = "<a href=\"javascript:fnProfileCommontView('" + dr["UserID"].ToString() + "');\">" + dr["UserName"].ToString() + " / " + dr["DeptName"].ToString() + "</a>";
            }

            return usernameLink;
        }

        public string CBestReplyContents(DataRow dr, String Public) //채택된 베스트 답변
        {
            string rankingHTML = string.Empty;
            if (dr["PublicYN"].ToString() == "N")
            {
                rankingHTML = "<img class=\"icon_img\" width=\"19\" height=\"19\" src=\"" + ConfigurationManager.AppSettings["FrontImageUrl"] + dr["UserGrade"].ToString() + ConfigurationManager.AppSettings["AftermageUrl"] + "\" title=\"" + dr["Rank"].ToString() + "\"/>";
            }

            string cbestreplyContents = string.Empty;
            /*
            return cbestreplyContents ="<div class=\"selected-headline\">질문자 채택 답변</div>"
                                        + "<div class=\"selected-user\">"
                                        + "<table class=\"selected-user-info\">"
                                        + "<tr>"
                                        + "<td class=\"pic\"><img src=\"" + dr["PhotoUrl"].ToString() + "\" alt=" + dr["UserName"].ToString() + "/" + dr["DeptName"].ToString() + " /></td>"
                                        //+ "<img class=\"noh\" width=\"19\" height=\"19\" src=\"" + ConfigurationManager.AppSettings["FrontImageUrl"] + dr["UserGrade"].ToString() + ConfigurationManager.AppSettings["AftermageUrl"] + "\"/>"
                                        + "<td class=\"name\">" + Public
                                        + rankingHTML
                                        + "<br /><span>" + Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd") + "</span></td>"
                                        + "<td class=\"write\" style=\"visibility:hidden\"></td>"      //티끌로 등록하기는여기서 추가한다.
                                        + "</tr>"
                                        + "</table>"
                                        + "</div>"
                                        + "<div class=\"selected-cont\">"
                                        + dr["Contents"].ToString().Replace("\n", "<br />")
                                        + "</div>";
            */

            return cbestreplyContents =
"<h3 class=\"icon_title blue9\">질문자 채택 답변</h3>"
+ "<div class=\"box_ct\">"
+ "<p class=\"view_writer\">"
+ "<a href=\"\">" + Public + "</a> 님이 " + Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd") + "에 작성"
+ "</p>"
+ "<div class=\"view_ct\">"
+ dr["Contents"].ToString().Replace("\n", "<br />")
+ "</div>"
//+ "<p class=\"btn_r\">"
//+ "<a href=\"\" class=\"btn_box btn_write\"><b>편집하기</b></a>"
//+ "</p>"
+ "</div>";
        }

        public string BottomBestReplyContents(ArrayList alist)
        {
            //베스트 댓글
            string bottomBestReplyContents ="<div id=\"CommentBestAdd\"></div>"
                                            +"<div class=\"qna-comment-view\" style=\"margin-top:0px;\" >"
                                            +"<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"qna-comment-view-tb\">"
                                            +"<tbody>";

                for (int i=0; i < alist.Count; i++)
                {
                    GlossaryQnACommentType data = (GlossaryQnACommentType)alist[i];

                    bottomBestReplyContents += "<tr class=\"best\">"
                                            +"<td class=\"pic\"><img src=" + data.PhotoUrl
                                            +" alt=" + data.UserName +"/" + data.DeptName +" /></td>"
                                            +"<td class=\"user\">"
                                            +"<dl>"
                                            +"<dt>";

                    if (data.PublicYN.ToString() == "N" || string.IsNullOrEmpty(data.PublicYN.ToString()))
                    {
                        bottomBestReplyContents += "<a href=\"javascript:fnProfileView('" + data.UserID + "');\">" + data.UserName + " / " + data.DeptName + "</a>";
                    }
                    else
                    {
                        bottomBestReplyContents += "비공개";
                    }   
                    
                    bottomBestReplyContents +="</dt>"
                                            +"<dd>" +  data.CreateDate + "</dd>"
                                            +"</dl>"
					                        +"</td>"
					                        +"<td class=\"txt\"><span class=\"best\">BEST</span></span>" + data.Contents + "</td>"
					                        +"<td runat=\"server\" id=\"oribtnbest\" class=\"rating\">"
                                            +"<span class=\"rating\">추천" + data.LikeCount + "</label>개</span></td>"
                                            +"</tr>";                                         
                }

            bottomBestReplyContents +="</tbody>"
                                +"</table>"
                                +"</div>";
            return bottomBestReplyContents;
        }

        public string BottomReplyContents(ArrayList alist)
        {
            //일반 댓글
            string bottomReplyContents ="<div id=\"CommentAdd\"></div>"
                                            +"<div class=\"qna-comment-view\" style=\"margin-top:0px;\" >"
                                            +"<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"qna-comment-view-tb\">"
                                            +"<tbody>";

            for (int i=0; i < alist.Count; i++)
            {
                GlossaryQnACommentType data = (GlossaryQnACommentType)alist[i];

                bottomReplyContents += "<tr>"
                                        +"<td class=\"pic\"><img src=" + data.PhotoUrl
                                        +" alt=" + data.UserName +"/" + data.DeptName +" /></td>"
                                        +"<td class=\"user\">"
                                        +"<dl>"
                                        +"<dt>";

                if (data.PublicYN.ToString() == "N" || string.IsNullOrEmpty(data.PublicYN.ToString()))
                {
                    bottomReplyContents += "<a href=\"javascript:fnProfileView('" + data.UserID + "');\">" + data.UserName + " / " + data.DeptName + "</a>";
                    bottomReplyContents += "<img class=\"icon_img\" width=\"19\" height=\"19\" src=\"" + ConfigurationManager.AppSettings["FrontImageUrl"] + data.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "\" title=\"" + data.Rank + "\"/>";
                }
                else
                {
                    bottomReplyContents += "비공개";
                }

                bottomReplyContents += "</dt>"
                                        +"<dd>" +  data.CreateDate + "</dd>"
                                        +"</dl>"
					                    +"</td>"
					                    +"<td class=\"txt\"></span>" + data.Contents + "</td>"
					                    +"<td runat=\"server\" id=\"oribtnbest\" class=\"rating\">"
                                        +"<span class=\"rating\">추천" + data.LikeCount + "</label>개</span></td>"
                                        +"</tr>";                                         
            }

            bottomReplyContents += "</tbody>"
                                +"</table>"
                                +"</div>";
            return bottomReplyContents;
        }

        public string qnaToTikleLink(string itemid, string historyyn)
        {
            string link = string.Empty;
            string tag = string.Empty;

            if(historyyn == "Y")
            {
                link = "../Glossary/GlossaryView.aspx?mode=History&ItemID=" + itemid;
            }
            else
            {
                link = "../Glossary/GlossaryView.aspx?ItemID=" + itemid;
            }
            tag = "&nbsp;<font color=black><b>(티끌로 변경 됨:</b></font>"
                                +"<a href=" 
                                +link
                                +" target=\"_self\">티끌로 가기</a>"
                                +"<font color=black><b>)</b></font>";  
            return tag;
        }

        public string qnaCommentLikeLink(string id, string likecount, string bestReplyYN)
        {
            string tag = string.Empty;

            //if(bestReplyYN == "Y")
            //{
            //    tag = "<a href=\"javascript:\" class=\"btn_s\">"
            //            + "<b>추천" + likecount + "개</b><span class=\""
            //            + id
            //            + "\" style=\"display:none\">" + likecount + "</span>"
            //            +"</a>";
                
            //}
            //else
            //{
            //    tag = "<a href=\"javascript:\" onclick=\" return fnCommentLike(this,'"
            //            + id
            //            + "', 'Y')\" class=\"btn_s\">"
            //            + "<b>추천" + likecount + "개</b><span class=\""
            //            + id
            //            + "\" style=\"display:none\">" + likecount + "</span>"
            //            +"</a>";
            //}

            tag = "<span class=\"btn_share\"><a href=\"javascript:\" onclick=\" return fnCommentLike(this,'"
                        + id
                        + "', 'Y')\" >" + likecount + "</span>"; 
                     


            return tag;
        }
    }
}