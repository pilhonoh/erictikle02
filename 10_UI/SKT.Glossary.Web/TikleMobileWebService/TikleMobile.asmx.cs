using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;

using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;
using SKT.Glossary.Web.Common.Controls;

using System.Web.Script.Serialization;
using System.Text;
using System.Net.Mail;

using System.Xml;
using System.Configuration;

namespace SKT.Glossary.Web.TikleMobileWebService
{
    /// <summary>
    /// TikleMobile의 요약 설명입니다.
    /// </summary>
    ///  //20140513 .Replace("%u001D", "") 필요한 부분들
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class TikleMobile : System.Web.Services.WebService
    {
        /***************  TIKLE ***************/  
      
        #region TikleCount - 사용안함
        /// <summary>
        /// 방문자수 - 카운트 관련 내용은 다 여기서 하기로
        /// </summary>
        /// <param name="LoginUserId"></param>
        /// <param name="Type"></param>
        [WebMethod]
        public void TikleCount(string LoginUserId, string Type)
        {
            TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            biz.TikleCount(LoginUserId, Type);
        }

        #endregion

        #region TikleSearch (모바일 최신지식 / 인기지식 검색)
        /// <summary> 
        /// 티끌 검색
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SearchKeyword"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleList TikleSearch(string LoginUserID, int PageIndex, int PageSize, string SearchKeyword, string SearchSort)
        //public ResultTikleList TikleSearch(int PageIndex, int PageSize, string SearchKeyword)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                // CHG610000076956 / 20181206 / 끌지식권한체크
                // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
                if (SetRoleGlossary(LoginUserID))
                {
                    int TotalCount = 0;
                    TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                    ArrayList list = biz.SearchGlossary(LoginUserID, PageIndex, PageSize, out TotalCount, SearchKeyword, SearchSort);

                    for (int i = 0; i < list.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)list[i];

                        TikleType temp = new TikleType();
                        temp.CommonID = data.CommonID;
                        temp.Title = HttpUtility.HtmlDecode(data.Title).Replace("&#39;", "'");
                        //temp.Contents = data.Contents;
                        temp.CreateDate = data.CreateDate;
                        temp.DeptName = data.DeptName;
                        temp.FirstDeptName = data.FirstDeptName;
                        temp.FirstUserID = data.FirstUserID;
                        temp.FirstUserName = data.FirstUserName;
                        temp.LikeCount = data.LikeCount;
                        //temp.ModifyCount = 
                        //temp.ScrapCount
                        //temp.ScrapYN
                        //temp.Summary = data.Summary;
                        //temp.TagList 
                        temp.UserID = data.UserID;
                        temp.UserName = data.UserName;
                        temp.UserGrade = data.UserGrade;
                        temp.Hits = data.Hits;

                        ret.TikleList.Add(temp);
                    }
                    ret.ListTotalCount = TotalCount;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.ListTotalCount = 0;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            
            return ret;
        }


        #endregion

        #region TikleList (모바일 최신지식)
        /// <summary>
        /// 최근 티끌 목록
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleList TikleList(string LoginUserID, int PageIndex, int PageSize)
        //public ResultTikleList TikleList(int PageIndex, int PageSize)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                // CHG610000076956 / 20181206 / 끌지식권한체크
                // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
                if (SetRoleGlossary(LoginUserID))
                {
                    int TotalCount;
                    TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                    ArrayList list = biz.NewList(LoginUserID, "New", "0", PageSize, out TotalCount, PageIndex);

                    for (int i = 0; i < list.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)list[i];

                        TikleType temp = new TikleType();
                        temp.CommonID = data.CommonID;
                        temp.Title = HttpUtility.HtmlDecode(data.Title).Replace("&#39;", "'");
                        //temp.Contents = data.Contents;
                        temp.CreateDate = data.CreateDate;
                        temp.DeptName = data.DeptName;
                        temp.FirstDeptName = data.FirstDeptName;
                        temp.FirstUserID = data.FirstUserID;
                        temp.FirstUserName = data.FirstUserName;
                        temp.LikeCount = data.LikeCount;
                        //temp.ModifyCount = 
                        //temp.ScrapCount
                        //temp.ScrapYN
                        //temp.Summary = data.Summary;
                        //temp.TagList 
                        temp.UserID = data.UserID;
                        temp.UserName = data.UserName;
                        temp.UserGrade = data.UserGrade;
                        temp.Hits = data.Hits;

                        ret.TikleList.Add(temp);
                    }
                    ret.ListTotalCount = TotalCount;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.ListTotalCount = 0;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TikleView
        /// <summary>
        /// 티끌 상세
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CommonID"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleView TikleView(string LoginUserID, string CommonID, string Mode)
        {
            ResultTikleView ret = new ResultTikleView();

            TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            // 20140528 권한체크
            if (!Convert.ToBoolean(biz.Permissions_Check(CommonID, LoginUserID)))
            {
                //throw new Exception("권한이 없습니다.");
                ret.IsSuccess = "false";
                ret.ErrorCode = "9998";
                ret.ErrorMessage = "권한이 없습니다";
            }
            else
            {
                if (String.IsNullOrEmpty(Mode) == true)
                {
                    Mode = "Histroy";
                }

                try
                {
                    GlossaryType data = biz.GlossarySelect(CommonID, LoginUserID, Mode);

                    ret.TikleViewData.CommonID = data.CommonID;
                    ret.TikleViewData.Title = data.Title;
                    ret.TikleViewData.Contents = data.Contents.Replace("%u001D", "");


                    // Mr.No 2016-01-04 Tnet Mobile 에서 동영상 재생이 가능하도록 변경
                    // html 중에 flashContent 가 있다면 변경작업 수행
                    if (ret.TikleViewData.Contents.Trim().IndexOf("flashContent") != -1)
                    {
                        String strXml = ret.TikleViewData.Contents;

                        // <div id="flashContent> 추출 포인트 셋팅
                        int firstEndPoint = Convert.ToInt32(strXml.IndexOf("&lt;div id=&quot;flashContent"));
                        int secondStartPoint = Convert.ToInt32(strXml.IndexOf("&lt;!--&lt;![endif]--&gt;&lt;/div&gt;")) + 37;

                        // falshContent 부분을 제외한 부분 추출
                        string frontStr = strXml.Substring(0, firstEndPoint);
                        string endStr = strXml.Substring(secondStartPoint, strXml.Length - secondStartPoint);

                        // filename 부분 추출
                        int filenameFront = Convert.ToInt32(strXml.IndexOf("filename=")) + 9;
                        string filename = strXml.Substring(filenameFront, strXml.IndexOf(".mp4") - filenameFront);
                        filename = Regex.Replace(filename, @"\D", "");
                        
                        // Mobile 에서 동영상 재생을 위해 HLS(Http Live Streaming) 방식 적용한 video tag 형식 가공 (전달 방식에 MA 와 TC 가 있음)
                        string replaceStr = string.Empty;
                        if (strXml.Substring(strXml.IndexOf("service_code=") + 13, 2) == "MA")
                        {
                            replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls></video>";
                            //replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls autoplay ></video>";
                        }
                        else if (strXml.Substring(strXml.IndexOf("service_code=") + 13, 2) == "TC")
                        {
                            //replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls autoplay ></video>";
                        }
                        else
                        {
                            // MA / TC 외에 전달 받거나 테스트시 발견된 사항이 없음
                        }

                        ret.TikleViewData.Contents = frontStr + HttpUtility.HtmlEncode(replaceStr) + endStr;
                    }

                    ret.TikleViewData.CreateDate = data.CreateDate;
                    ret.TikleViewData.DeptName = data.DeptName;
                    ret.TikleViewData.FirstDeptName = data.FirstDeptName;
                    ret.TikleViewData.FirstUserID = data.FirstUserID;
                    ret.TikleViewData.FirstUserName = data.FirstUserName;
                    ret.TikleViewData.FirstCreateDate = data.FirstCreateDate;
                    ret.TikleViewData.UserID = data.UserID;
                    ret.TikleViewData.UserName = data.UserName;
                    //ret.TikleViewData.Summary = data.Summary;  혹시필요하면 넣기.

                    ret.TikleViewData.UserGrade = data.UserGrade; // 편집자 랭킹
                    ret.TikleViewData.FirstUserGrade = data.FirstUserGrade; // 최초 작성자 랭킹

                    GlossaryControlType cdata = biz.GlossaryViewMenuSelect(LoginUserID, data.CommonID);
                    ret.TikleViewData.LikeCount = cdata.LikeCount;
                    ret.TikleViewData.ModifyCount = cdata.Historycount;
                    ret.TikleViewData.ScrapCount = cdata.ScrapCount;
                    ret.TikleViewData.ScrapYN = cdata.ScrapsYN;
                    ret.TikleViewData.LikeYN = cdata.LikeY;
                    ret.TikleViewData.NoteYN = cdata.NoteYN;

                    ret.TikleViewData.DTBlogFlag = cdata.DTBlogFlag;
                    ret.TikleViewData.TWhiteFlag = cdata.TWhiteFlag;

                    ArrayList taglist = biz.GlossaryTagList(data.CommonID, data.Title);

                    for (int i = 0; i < taglist.Count; i++)
                    {
                        GlossaryControlType temp = (GlossaryControlType)taglist[i];

                        TikleTagType addtemp = new TikleTagType();  //리턴id 가 DB commonid 이어야한다 SP 에서 틀리다면 바꾸어줄것.
                        addtemp.ID = temp.ID;
                        addtemp.Title = temp.Title;
                        ret.TikleViewData.TagList.Add(addtemp);
                    }

                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                catch (Exception ex)
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = ex.ToString();
                }
            }
            return ret;
        }

        #endregion

        #region TikleViewAll
        /// <summary>
        /// 티끌 상세
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CommonID"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleAllView TikleViewAll(string LoginUserID, string CommonID, string Mode)
        {
            ResultTikleAllView ret = new ResultTikleAllView();
            TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            GlossaryBiz gbiz = new GlossaryBiz();
            GlossaryType data = new GlossaryType();
            
            //data = gbiz.GlossarySelect(CommonID, LoginUserID, Mode);
            //2016-11-18 KSH 빈값이어야 조회수가 증가된다.
            data = gbiz.GlossarySelect(CommonID, LoginUserID, "");

            string GatheringP = data.Permissions;

            // 20140528 권한체크
            if (!Convert.ToBoolean(biz.Permissions_Check(CommonID, LoginUserID)) && GatheringP != "GatheringPublic")
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9998";
                ret.ErrorMessage = "권한이 없습니다";
            }
            else
            {
                if (String.IsNullOrEmpty(Mode) == true)
                {
                    Mode = "Histroy";
                }

                try
                {
                    // data = biz.GlossarySelect(CommonID, LoginUserID, Mode);

                    ret.TikleViewData.CommonID = data.CommonID;
                    ret.TikleViewData.Title = data.Title;
                    ret.TikleViewData.Contents = data.Contents.Replace("%u001D", "");
                    ret.TikleViewData.Summary = data.Summary.Replace("&nbsp;", "").Replace("<br>","\n");

                    #region 사용안함
                    //// Mr.No 2016-01-04 Tnet Mobile 에서 동영상 재생이 가능하도록 변경
                    //// html 중에 flashContent 가 있다면 변경작업 수행
                    //if (ret.TikleViewData.Contents.Trim().IndexOf("flashContent") != -1)
                    //{
                    //    String strXml = ret.TikleViewData.Contents;

                    //    // <div id="flashContent> 추출 포인트 셋팅
                    //    //int firstEndPoint = Convert.ToInt32(strXml.IndexOf("&lt;div id=&quot;flashContent"));
                    //    //int secondStartPoint = Convert.ToInt32(strXml.IndexOf("&lt;!--&lt;![endif]--&gt;&lt;/div&gt;")) + 37;

                    //    int firstEndPoint = Convert.ToInt32(strXml.IndexOf("flashContent"));
                    //    int secondStartPoint = Convert.ToInt32(strXml.IndexOf("/OBJECT&gt;&lt;!--&lt;![endif]--&gt;&lt;/DIV&gt;&lt;")) + 52;



                    //    // falshContent 부분을 제외한 부분 추출
                    //    string frontStr = strXml.Substring(0, firstEndPoint);
                    //    string endStr = strXml.Substring(secondStartPoint, strXml.Length - secondStartPoint);

                    //    // filename 부분 추출
                    //    int filenameFront = Convert.ToInt32(strXml.IndexOf("filename=")) + 9;
                    //    string filename = strXml.Substring(filenameFront, strXml.IndexOf(".mp4") - filenameFront);
                    //    filename = Regex.Replace(filename, @"\D", "");

                    //    // Mobile 에서 동영상 재생을 위해 HLS(Http Live Streaming) 방식 적용한 video tag 형식 가공 (전달 방식에 MA 와 TC 가 있음)
                    //    string replaceStr = string.Empty;
                    //    if (strXml.Substring(strXml.IndexOf("service_code=") + 13, 2) == "MA")
                    //    {
                    //        replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls></video>";
                    //        //replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls autoplay ></video>";
                    //    }
                    //    else if (strXml.Substring(strXml.IndexOf("service_code=") + 13, 2) == "TC")
                    //    {
                    //        //replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls autoplay ></video>";
                    //    }
                    //    else
                    //    {
                    //        // MA / TC 외에 전달 받거나 테스트시 발견된 사항이 없음
                    //    }

                    //    ret.TikleViewData.Contents = frontStr + HttpUtility.HtmlEncode(replaceStr) + endStr;
                    //}
                    #endregion

                    string tmpContents = SecurityHelper.ReClear_XSS_CSRF(HttpUtility.HtmlDecode(ret.TikleViewData.Contents));

                    if (tmpContents.ToLower().Trim().IndexOf("<div id=\"flashContent\">".ToLower()) > -1)
                    {
                        //tmpContents = tmpContents.Replace("classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"", "classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" type=\"application/x-shockwave-flash\" ");
                        tmpContents = tmpContents.Replace("[if !IE]&gt--</OBJECT><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("[if !IE]&gt--</object><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("[if !IE]&gt;--&gt;</OBJECT><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("[if !IE]&gt;--&gt;</object><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("[if !IE]>--></OBJECT><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("[if !IE]>--></object><!--<![endif]-->", "</object>");
                        tmpContents = tmpContents.Replace("</OBJECT>", "</object>");

                        //filename=1006581_NM.mp4
                        int filenameFront = Convert.ToInt32(tmpContents.IndexOf("filename=")) + 9;
                        string filename = tmpContents.Substring(filenameFront, tmpContents.IndexOf(".mp4") - filenameFront);
                        filename = Regex.Replace(filename, @"\D", "");

                        string replaceStr = string.Empty;
                        if (tmpContents.IndexOf("service_code=MA") > -1)
                        {
                            // sample 소스  http://cdn.learningmate.co.kr:9080/vod_skgp/102836/mp4:102836_MHQ.mp4/playlist.m3u8
                            replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/vod_skgp/" + filename + "/mp4:" + filename + "_MHQ.mp4/playlist.m3u8\" controls></video>";
                        }
                        if (tmpContents.IndexOf("service_code=TC") > -1)
                        {
                            // sample 소스  http://cdn.learningmate.co.kr:9080/hls-vod/live_learning/streams/102836/102836.mp4/playlist.m3u8
                            replaceStr = "<video width=\"300\" height=\"300\" src=\"http://cdn.learningmate.co.kr:9080/hls-vod/live_learning/streams/" + filename + "/mp4:" + filename + ".mp4/playlist.m3u8\" controls></video>";
                        }

                        if (tmpContents.IndexOf("</object>") > -1)
                        {
                            tmpContents = tmpContents.Replace("</object>", "</object>" + replaceStr);
                        }
                        ret.TikleViewData.Summary = ret.TikleViewData.Summary.Replace("[if !IE]>-->", "");
                    }

                    ret.TikleViewData.Contents = tmpContents;
                    

                    ret.TikleViewData.CreateDate = data.CreateDate;
                    ret.TikleViewData.DeptName = data.DeptName;
                    ret.TikleViewData.FirstDeptName = data.FirstDeptName;
                    ret.TikleViewData.FirstUserID = data.FirstUserID;
                    ret.TikleViewData.FirstUserName = data.FirstUserName;
                    ret.TikleViewData.FirstCreateDate = data.FirstCreateDate;
                    ret.TikleViewData.UserID = data.UserID;
                    ret.TikleViewData.UserName = data.UserName;
                    //ret.TikleViewData.Summary = data.Summary;  혹시필요하면 넣기.

                    ret.TikleViewData.UserGrade = data.UserGrade; // 편집자 랭킹
                    ret.TikleViewData.FirstUserGrade = data.FirstUserGrade; // 최초 작성자 랭킹
                    ret.TikleViewData.TWhiteFlag = data.TWhiteFlag;
                    ret.TikleViewData.DTBlogFlag = data.DTBlogFlag; 

                    GlossaryControlType cdata = biz.GlossaryViewMenuSelect(LoginUserID, data.CommonID);
                    ret.TikleViewData.LikeCount = cdata.LikeCount;
                    ret.TikleViewData.ModifyCount = cdata.Historycount;
                    ret.TikleViewData.ScrapCount = cdata.ScrapCount;
                    ret.TikleViewData.ScrapYN = cdata.ScrapsYN;
                    ret.TikleViewData.LikeYN = cdata.LikeY;
                    ret.TikleViewData.NoteYN = cdata.NoteYN;

                    ArrayList taglist = biz.GlossaryTagList(data.CommonID, data.Title);

                    for (int i = 0; i < taglist.Count; i++)
                    {
                        GlossaryControlType temp = (GlossaryControlType)taglist[i];

                        TikleTagType addtemp = new TikleTagType();  //리턴id 가 DB commonid 이어야한다 SP 에서 틀리다면 바꾸어줄것.
                        addtemp.ID = temp.ID;
                        addtemp.Title = temp.Title;
                        ret.TikleViewData.TagList.Add(addtemp);
                    }

                    //2016-06-17 ksh 댓글 추가
                    ret.GlossaryComment = biz.GlossaryComment(CommonID);

                    //2016-06-17 ksh 파일 추가
                    ret.GlossaryAttach = biz.AttachmentList_Full(Convert.ToInt32(CommonID));
                    ret.GlossaryAttach.ListTotalCount = ret.GlossaryAttach.AttachmentList.Count;

                    ret.glossaryTags = biz.GlossaryTagSelect(CommonID);

                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                catch (Exception ex)
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = ex.ToString();
                }
            }
            return ret;
        }

        #endregion

        #region  ScrapSearch
        /// <summary>
        ///  스크랩 검색
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SearchKeyword"></param>
        /// <param name="LoginUserID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultScrapList ScrapSearch(int PageIndex, int PageSize, string SearchKeyword, string LoginUserID)
        {
            ResultScrapList ret = new ResultScrapList();

            try
            {
                int TotalCount = 0;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.SearchScrap(PageIndex, PageSize, SearchKeyword, LoginUserID);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryScrapType data = (GlossaryScrapType)list[i];
                    data.Summary.Replace("%u001D", "");
                    ret.ScrapList.Add(data);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region ScrapList 
        /// <summary>
        /// 스크랩 티끌 목록
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginUserID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultScrapList ScrapList(int PageIndex, int PageSize, string LoginUserID)
        {
            ResultScrapList ret = new ResultScrapList();

            try
            {
                int TotalCount;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.GlossaryScrapList(PageIndex, PageSize, out TotalCount, LoginUserID);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryScrapType data = (GlossaryScrapType)list[i];
                    data.Summary.Replace("%u001D", "");
                    ret.ScrapList.Add(data);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TikleRecommendation 
        /// <summary>
        /// 티끌 추천
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CommonID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon TikleRecommendation(string LoginUserID, string CommonID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                GlossaryControlType Board = new GlossaryControlType();
                TikleMobileWebServiceBiz Biz = new TikleMobileWebServiceBiz();
                Board.GlossaryID = CommonID;
                Board.UserID = LoginUserID;
                Board.LikeY = "Y";
                Biz.GlossaryLikeInsert(Board);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TikleSetNotification 
        /// <summary>
        /// 티끌 변경알림
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CommonID"></param>
        /// <param name="NoteYN"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon TikleSetNotification(string LoginUserID, string CommonID, string NoteYN)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                GlossaryControlType Board = new GlossaryControlType();
                TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
                Board.UserID = LoginUserID;
                Board.MailYN = "N"; //메일 설정은 삭제. 되어서 항상N 혹시 정책이 바뀌면 이값을 수정해야함.
                Board.NoteYN = NoteYN;
                Board.CommonID = CommonID;
                Dac.GlossaryAlarmInsert(Board);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TikleSetScrap
        /// <summary>
        /// 스크랩 설정
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CommonID"></param>
        /// <param name="ScrapsYN"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon TikleSetScrap(string LoginUserID, string CommonID, string ScrapsYN)
        {
            string Title = "MobileScrap"; //멀넣어도 의미없음... 아무거나.
            string TargetUserID = LoginUserID; //이거도 대상항목 사용자 아이디엿으나 의미가 없어짐. 그냥 로그인사용자로 넣음.
            ResultCommon ret = new ResultCommon();
            try
            {
                GlossaryScrapType Board = new GlossaryScrapType();
                TikleMobileWebServiceBiz Biz = new TikleMobileWebServiceBiz();
                Board.GlossaryID = CommonID;
                Board.Title = Title;
                Board.UserID = LoginUserID;
                Board.YouUserID = TargetUserID;
                Board.ScrapsYN = ScrapsYN;
                Biz.GlossaryScrapInsert(Board);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNAList
        /// <summary>
        /// QNA 목록
        /// </summary>
        [WebMethod]
        public ResultQNAList QNAList(int PageIndex, int PageSize, string LoginUserID, string SearchKeyword)
        {
            ResultQNAList ret = new ResultQNAList();
            string SearchType = "Total";

            int TotalCount;
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.GlossaryQnAList(PageIndex, PageSize, SearchKeyword, SearchType, LoginUserID, out TotalCount);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryQnAType data = (GlossaryQnAType)list[i];
                    data.Contents.Replace("%u001D", "");
                    data.ContentsModify.Replace("%u001D", "");
                    ret.QNAList.Add(data);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNAView
        /// <summary>
        /// QNA 상세
        /// </summary>
        /// <param name="QNAID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultQNAView QNAView(string QNAID)
        {
            ResultQNAView ret = new ResultQNAView();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                GlossaryQnAType Board = new GlossaryQnAType();
                ret.QNAViewData = biz.GlossaryQnASelect(QNAID, 1); //QNA hit 올리려면 1 아니면 0
                ret.QNAViewData.Contents.Replace("%u001D", "");
                ret.QNAViewData.ContentsModify.Replace("%u001D", "");
                ret.QNASelectedBestCommentData = biz.GlossaryQnACommentSuccessSelect(QNAID);
                if (ret.QNASelectedBestCommentData.Contents != null)
                    ret.QNASelectedBestCommentData.Contents.Replace("%u001D", "");

                ArrayList bestlist = biz.GlossaryQnABastCommentList(QNAID);
                for (int i = 0; i < bestlist.Count; i++)
                {
                    GlossaryQnACommentType data = (GlossaryQnACommentType)bestlist[i];
                    data.Contents.Replace("%u001D", "");
                    ret.BestCommentList.Add(data);
                }
                ArrayList list = biz.GlossaryQnACommentList(QNAID);
                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryQnACommentType data = (GlossaryQnACommentType)list[i];
                    data.Contents.Replace("%u001D", "");
                    ret.CommentList.Add(data);
                }

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNACommentSetBestComment
        /// <summary>
        /// QNA 답변 채택
        /// </summary>
        /// <param name="QNAID"></param>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon QNACommentSetBestComment(string QNAID, string CommentID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                GlossaryQnACommentType Comment = new GlossaryQnACommentType();
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                Comment.ID = QNAID;
                Comment.CommonID = CommentID; //기존코드가 꼬였지만.. commonid 가 여기선 commentid 로 쓰이고있음..

                Comment = biz.GlossaryQnABastSuccessComment(Comment);
                Comment.Contents.Replace("%u001D", "");

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNACommentInsert
        /// <summary>
        ///  Qna답글 입력을 입력한다
        /// </summary>
        /// <param name="QNAID"></param>
        /// <param name="Contents"></param>
        /// <param name="LoginUserID"></param>
        /// <param name="HiddenYN"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon QNACommentInsert(string QNAID, string Contents, string LoginUserID, string HiddenYN)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                GlossaryQnACommentType Comment = new GlossaryQnACommentType();

                ImpersonUserinfo u = biz.UserSelect(LoginUserID);
                Comment.UserID = u.UserID;
                Comment.UserName = u.Name;
                Comment.CommonID = QNAID;
                Comment.UserEmail = u.EmailAddress;
                Comment.DeptName = u.DeptName;
                Comment.PhotoUrl = u.PhotoUrl;
                Comment.LikeCount = "0";

                //131120 수정
                if (HiddenYN == "Y")  //비공개
                {
                    Comment.UserID = "";
                    Comment.UserName = "비공개";
                    Comment.DeptName = "비공개부서";
                    Comment.UserEmail = "";
                    Comment.PhotoUrl = "/common/images/user_none.png";
                }
                Comment.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");

                Comment.Contents = MakeURLLink(Contents); // url 등 처리만들기.

                //추가
                Comment = biz.GlossaryQnACommentInsert(Comment);

                //모바일에서는 쪽지보내기 어떻게 처리해야할까.. 원래는여기서 쪽지를 보냄

                TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

                DataSet ds = new DataSet();
                ds = dac.NoteQnaYNSelect(Comment.CommonID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["BastReplyYN"].ToString() != "Y" && dr["UserName"].ToString() != "비공개")
                        {
                            //쪽지보내기
                            SendNoteQna(QNAID);
                        }
                    }
                }


                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNACommentModify
        /// <summary>
        /// Qna답글을 수정한다
        /// </summary>
        /// <param name="QNAID"></param>
        /// <param name="CommentID"></param>
        /// <param name="Contents"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon QNACommentModify(string QNAID, string CommentID, string Contents)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                GlossaryQnACommentType Comment = new GlossaryQnACommentType();

                Comment.ID = CommentID;

                Comment.CommonID = QNAID;

                Comment.Contents = MakeURLLink(Contents); // url 등 처리만들기.

                //추가
                biz.GlossaryQnACommentUpdate(Comment);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region QNACommentSetRecommendation
        /// <summary>
        /// QNA 답변 추천
        /// </summary>
        /// <param name="QNAID"></param>
        /// <param name="CommentID"></param>
        /// <param name="LoginUserID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultCommon QNACommentSetRecommendation(string QNAID, string CommentID, string LoginUserID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                GlossaryQnACommentType Comment = new GlossaryQnACommentType();
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                Comment.ID = CommentID;
                Comment.CommonID = CommentID;
                Comment.UserID = LoginUserID;
                Comment = biz.GlossaryQnACommentLikeY(Comment);

                if (Comment.LikeY == "N")
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "1";
                    ret.ErrorMessage = "이미 추천하였습니다";
                }
                else
                {
                    //쪽지보내기?
                    //모바일에서는 쪽지보내기 어떻게 처리해야할까.. 원래는여기서 쪽지를 보냄
                    LikeSendNoteQna(QNAID, Comment.CommonID);
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TikleCategoryList (모바일 인기지식)
        /// <summary>
        /// 카테고리 별 티끌 목록
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleList TikleCategoryList(string LoginUserID, int PageIndex, int PageSize, string CategoryID)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                // CHG610000076956 / 20181206 / 끌지식권한체크
                // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
                if (SetRoleGlossary(LoginUserID))
                {
                    int TotalCount;
                    TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                    ArrayList list = biz.NewCategoryList(LoginUserID, "Category", CategoryID, PageSize, out TotalCount, PageIndex);

                    for (int i = 0; i < list.Count; i++)
                    {
                        GlossaryType data = (GlossaryType)list[i];

                        TikleType temp = new TikleType();
                        temp.CommonID = data.CommonID;
                        temp.Title = HttpUtility.HtmlDecode(data.Title).Replace("&#39;", "'");
                        //temp.Contents = data.Contents;
                        temp.CreateDate = data.CreateDate;
                        temp.DeptName = data.DeptName;
                        temp.FirstDeptName = data.FirstDeptName;
                        temp.FirstUserID = data.FirstUserID;
                        temp.FirstUserName = data.FirstUserName;
                        temp.LikeCount = data.LikeCount;
                        //temp.ModifyCount = 
                        //temp.ScrapCount
                        //temp.ScrapYN
                        //temp.Summary = data.Summary;
                        //temp.TagList 
                        temp.UserID = data.UserID;
                        temp.UserName = data.UserName;
                        temp.UserGrade = data.UserGrade;
                        temp.Hits = data.Hits;

                        ret.TikleList.Add(temp);
                    }
                    ret.ListTotalCount = TotalCount;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.ListTotalCount = 0;
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region CategoryList 
        /// <summary>
        /// 카테고리 목록
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public ResultCategoryList CategoryList()
        {
            ResultCategoryList ret = new ResultCategoryList();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ret = biz.CategoryList();

                ret.ListTotalCount = ret.CategoryList.Count;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region AttachmentList
        /// <summary>
        /// 첨부파일 정보
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultAttachmentList AttachmentList(int ItemID)
        {
            ResultAttachmentList ret = new ResultAttachmentList();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ret = biz.AttachmentList(ItemID);

                ret.ListTotalCount = ret.AttachmentList.Count;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region RankingInfo 
        /// <summary>
        /// 랭킹 정보
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [WebMethod]
        public RankingType RankingInfo(string UserID)
        {
            RankingType ret = new RankingType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ret = biz.RankingInfo(UserID);


                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region TiklePlatTrendList
        /// <summary>
        ///  2015-11-26 Platfrom / techtrend 추가 ksh
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultTikleList TiklePlatTrendList(string LoginUserID, int PageIndex, int PageSize, string Mode)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {

                int TotalCount;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.PlatformTechTrendList(LoginUserID, Mode, PageSize, out TotalCount, PageIndex);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryType data = (GlossaryType)list[i];

                    TikleType temp = new TikleType();
                    temp.CommonID = data.CommonID;
                    temp.Title = data.Title;
                    temp.CreateDate = data.CreateDate;
                    temp.DeptName = data.DeptName;
                    temp.FirstDeptName = data.FirstDeptName;
                    temp.FirstUserID = data.FirstUserID;
                    temp.FirstUserName = data.FirstUserName;
                    temp.LikeCount = data.LikeCount;
                    temp.UserID = data.UserID;
                    temp.UserName = data.UserName;
                    temp.UserGrade = data.UserGrade;

                    ret.TikleList.Add(temp);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region TiklePlatTrendUserPermission
        [WebMethod]
        public ResultTikleUserPermission TiklePlatTrendUserPermission(string LoginUserID)
        {
            ResultTikleUserPermission rup = new ResultTikleUserPermission();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                string UserPermission = biz.PlatTrendUserPermission(LoginUserID);

                rup.IsSuccess = "true";
                rup.ErrorCode = "0";
                rup.UserPermission = UserPermission;
            }
            catch (Exception ex)
            {
                rup.IsSuccess = "false";
                rup.ErrorCode = "9999";
                rup.ErrorMessage = ex.ToString();
            }
            return rup;
        }
        #endregion

        #region TikleSearch_PlatTrend
        //2015-12-04 ksh 티끌 플랫폼트렌드 검색
        [WebMethod]
        public ResultTikleList TikleSearch_PlatTrend(string LoginUserID, int PageIndex, int PageSize, string SearchKeyword, string Mode)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                int TotalCount = 0;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.SearchGlossary_PlatTrend(LoginUserID, PageIndex, PageSize, SearchKeyword, Mode);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryType data = (GlossaryType)list[i];

                    TikleType temp = new TikleType();
                    temp.CommonID = data.CommonID;
                    temp.Title = data.Title;
                    temp.CreateDate = data.CreateDate;
                    temp.DeptName = data.DeptName;
                    temp.FirstDeptName = data.FirstDeptName;
                    temp.FirstUserID = data.FirstUserID;
                    temp.FirstUserName = data.FirstUserName;
                    temp.LikeCount = data.LikeCount;
                    temp.UserID = data.UserID;
                    temp.UserName = data.UserName;
                    temp.UserGrade = data.UserGrade;

                    ret.TikleList.Add(temp);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        /////////////////////////////////
        //2016-06 추가 Interface
        /////////////////////////////////

        #region TikleCommentInsert
        /// <summary>
        /// 티끌 문서 댓글을 입력한다
        /// </summary>
        [WebMethod]
        public ResultCommon TikleCommentInsert(string LoginUserID, string commType, string commIdx, string contentText, string PublicYN, string supIdx)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                DataSet ds = new DataSet();

                CommCommentType Comment = new CommCommentType();

                Comment.USERID = LoginUserID;
                Comment.COMMENTTYPE = commType;
                Comment.COMMONID = commIdx;
                Comment.LIKECOUNT = "0";
                Comment.ID = supIdx;
                Comment.PUBLICYN = (PublicYN.Length == 0 ? "N" : PublicYN);
                Comment.CONTENTS = contentText;
                Comment.CONTENTS = SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS);  //보안 조치
                Comment.USERIP = ""; //HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; //mobile
                Comment.USERMACHINENAME = "MobileApp"; //System.Net.Dns.GetHostName(); //mobile

                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                //P097010 BACKUP2
                GlossaryType Board = biz.GlossarySelect(commIdx, LoginUserID, "");

                if (supIdx.Length == 0)
                {
                    ds = biz.TikleCommentInsert(Comment);

                    //끌지식을 댓글이 없고 끌모임만 댓글 작성
                    //P097010 BACKUP2
                    if (!String.IsNullOrEmpty(Board.GatheringID))
                    {
                        //Author : 개발자-최현미, 리뷰자-윤자영
                        //Create Date : 2017.04.06 
                        //Desc : 끌.모임 댓글 알림설정한 멤버들에게 쪽지를 발송한다 
                        SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(Board.GatheringID, commIdx, "0", LoginUserID, PublicYN, "REPLY");
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                    string DBFLAG = "0";
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DBFLAG = dr["DBFLAG"].ToString();
                        }
                    }

                    if (int.Parse(DBFLAG) == 0)
                    {
                        ret.IsSuccess = "true";
                        ret.ErrorCode = "0";
                    }
                    else
                    {
                        ret.IsSuccess = "false";
                        ret.ErrorCode = "9999";
                        ret.ErrorMessage = "NOT EXISTS";
                    }
                }
                else
                {
                    ds = biz.TikleReCommentInsert(Comment);

                    //P097010 BACKUP2
                    //Author : 개발자-최현미, 리뷰자-윤자영
                    //Create Date : 2017.04.06 
                    //Desc : 끌.모임 댓글의 답글 알림설정한 멤버들에게 쪽지를 발송한다 

                    if (!String.IsNullOrEmpty(Board.GatheringID))
                    {
                        SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(Board.GatheringID, commIdx, supIdx, LoginUserID, PublicYN, "REREPLY");
                    }

                    string DBFLAG = "0";
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DBFLAG = dr["DBFLAG"].ToString();
                        }
                    }

                    if (int.Parse(DBFLAG) == 0)
                    {
                        ret.IsSuccess = "true";
                        ret.ErrorCode = "0";
                    }
                    else
                    {
                        ret.IsSuccess = "false";
                        ret.ErrorCode = "9999";
                        ret.ErrorMessage = "PARENT NOT EXISTS";
                    }
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region TikleCommentModify
        /// <summary>
        /// 티끌 문서 댓글을 수정한다.
        /// </summary>
        [WebMethod]
        public ResultCommon TikleCommentModify(string LoginUserID, string Idx, string contentText, string PublicYN)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                DataSet ds = new DataSet();

                CommCommentType Comment = new CommCommentType();

                Comment.USERID = LoginUserID;
                Comment.ID = Idx;
                Comment.LASTMODIFIERID = LoginUserID;
                Comment.CONTENTS = contentText;
                Comment.CONTENTS = SecurityHelper.Clear_XSS_CSRF(Comment.CONTENTS); //보안 조치
                Comment.PUBLICYN = (PublicYN.Length == 0 ? "N" : PublicYN);
                Comment.USERIP = ""; //HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; //mobile
                Comment.USERMACHINENAME = "MobileApp"; //System.Net.Dns.GetHostName(); //mobile
                
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                //수정 업데이트
                ds = biz.TikleCommentModify(Comment);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region TikleCommentDelete
        /// <summary>
        /// 티끌 문서 댓글을 삭제한다.
        /// </summary>
        [WebMethod]
        public ResultCommon TikleCommentDelete(string LoginUserID, string Idx)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                DataSet ds = new DataSet();
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                CommCommentType Comment = new CommCommentType();
                Comment.ID = Idx;
                Comment.LASTMODIFIERID = LoginUserID;
                Comment.USERIP = ""; //HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; //mobile
                Comment.USERMACHINENAME = "MobileApp"; //System.Net.Dns.GetHostName(); //mobile

                ds = biz.TikleCommentDelete(Comment);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region QNAInsert
        [WebMethod]
        public ResultCommon QNAInsert(string LoginUserID, string title, string contents)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = new DataSet();
                ds = biz.QNAInsert(LoginUserID, title, contents);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "작성 실패";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region QNAModify
        [WebMethod]
        public ResultCommon QNAModify(string LoginUserID,string QNAID, string title, string contents)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = new DataSet();
                ds = biz.QNAModify(LoginUserID, QNAID, title, contents);

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["Column1"]) > 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "수정 실패";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region QNADelete
        [WebMethod]
        public ResultCommon QNADelete(string LoginUserID, string QNAID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                biz.QNADelete(LoginUserID, QNAID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region GatheringList
        [WebMethod]
        public ResultGatheringList GatheringList(string LoginUserID, string Mode, int PageNum, int PageSize)
        {
            ResultGatheringList ret = new ResultGatheringList();
            try
            {
                int TotalCount;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.GatheringList(LoginUserID, Mode, PageNum, PageSize, out TotalCount);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryGatheringListType data = (GlossaryGatheringListType)list[i];
                    ret.GatheringList.Add(data);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GatheringNoticeList
        [WebMethod]
        public ResultTikleList GatheringNoticeList(string LoginUserID,string GatheringID, int PageIndex, int PageSize)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                int TotalCount;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.GatheringNoticeList(LoginUserID, GatheringID,  PageSize, out TotalCount, PageIndex);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryType data = (GlossaryType)list[i];

                    TikleType temp = new TikleType();
                    temp.CommonID = data.CommonID;
                    temp.Title = HttpUtility.HtmlDecode(data.Title).Replace("&#39;", "'");
                    temp.CreateDate = data.CreateDate;
                    temp.DeptName = data.DeptName;
                    temp.FirstDeptName = data.FirstDeptName;
                    temp.FirstUserID = data.FirstUserID;
                    temp.FirstUserName = data.FirstUserName;
                    temp.LikeCount = data.LikeCount;
                    temp.UserID = data.UserID;
                    temp.UserName = data.UserName;
                    temp.UserGrade = data.UserGrade;

                    ret.TikleList.Add(temp);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        
        #endregion

        #region GatheringNoticeInsert
        [WebMethod]
        public ResultCommon GatheringNoticeInsert(string LoginUserID, string title, string contents,string GatheringID,string TagTitle)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = new DataSet();

                title = Regex.Replace(title, @"<(.|\n)*?>", String.Empty);
                contents = SKT.Common.SecurityHelper.Clear_XSS_CSRF(contents);

                ds = biz.GatheringNoticeInsert(LoginUserID, title, contents, GatheringID, TagTitle);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //P097010 BACKUP2
                    //Author : 개발자-최현미, 리뷰자-윤자영
                    //Create Date : 2017.04.06 
                    //Desc : 끌.모임 신규 작성 알림설정한 멤버들에게 쪽지를 발송한다.

                    SKT.Glossary.Web.Common.Controls.CommCommentAjax.GatheringSendMemberCheck(GatheringID, ds.Tables[0].Rows[0]["ID"].ToString(), "0", LoginUserID, "", "WRITE");

                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "작성 실패";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GatheringNoticeModify
        [WebMethod]
        public ResultCommon GatheringNoticeModify(string LoginUserID,string CommonID, string title, string contents, string GatheringID, string TagTitle)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = new DataSet();

                title = Regex.Replace(title, @"<(.|\n)*?>", String.Empty);
                contents = SKT.Common.SecurityHelper.Clear_XSS_CSRF(contents);

                ds = biz.GatheringNoticeModify(LoginUserID, CommonID, title, contents, GatheringID, TagTitle);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "작성 실패";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GatheringNoticeDelete
        [WebMethod]
        public ResultCommon GatheringNoticeDelete(string LoginUserID, string CommonID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                
                Boolean result = biz.GatheringNoticeDelete(LoginUserID, CommonID);

                if (result)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "t 실패";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        #endregion

        #region MarketingTikleList
        [WebMethod]
        public ResultTikleList MarketingTikleList(string LoginUserID, int PageIndex, int PageSize)
        {
            ResultTikleList ret = new ResultTikleList();

            try
            {
                int TotalCount;
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.MarketingTikleList(LoginUserID, PageSize, out TotalCount, PageIndex);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryType data = (GlossaryType)list[i];

                    TikleType temp = new TikleType();
                    temp.CommonID = data.CommonID;
                    temp.Title = data.Title;
                    temp.CreateDate = data.CreateDate;
                    temp.DeptName = data.DeptName;
                    temp.FirstDeptName = data.FirstDeptName;
                    temp.FirstUserID = data.FirstUserID;
                    temp.FirstUserName = data.FirstUserName;
                    temp.LikeCount = data.LikeCount;
                    temp.UserID = data.UserID;
                    temp.UserName = data.UserName;
                    temp.UserGrade = data.UserGrade;

                    ret.TikleList.Add(temp);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region MarketingQnaList
        [WebMethod]
        public ResultQNAList MarketingQnaList(string LoginUserID, int PageIndex, int PageSize)
        {
            ResultQNAList ret = new ResultQNAList();

            int TotalCount;
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ArrayList list = biz.MarketingQnaList(LoginUserID, PageIndex, PageSize, out TotalCount);

                for (int i = 0; i < list.Count; i++)
                {
                    GlossaryQnAType data = (GlossaryQnAType)list[i];
                    data.Contents.Replace("%u001D", "");
                    data.ContentsModify.Replace("%u001D", "");
                    ret.QNAList.Add(data);
                }
                ret.ListTotalCount = TotalCount;
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GlossaryTitleCheck
        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.08.04 
        //Desc : 제목 중복 웹서비스 추가
        [WebMethod]
        public ResultCommon GlossaryTitleCheck(string Title, string commonID, string GatheringYN, string GatheringID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                string id = biz.GlossaryTitleCheck(Title, commonID, GatheringYN, GatheringID);

                if (!string.IsNullOrEmpty(id))
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "1";
                    ret.ErrorMessage = "동일한 제목이 존재합니다. 수정 후 저장해주세요.";
                }
                else
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GatheringMobileNotiInsert
        [WebMethod]
        public ResultCommon GatheringMobileNotiInsert(string LoginUserID, int GatheringID)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = biz.GatheringMobileNotiInsert(LoginUserID, GatheringID);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "Exists";
                }
               
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GatheringMobileNotiDelete
        [WebMethod]
        public ResultCommon GatheringMobileNotiDelete(string LoginUserID, int GatheringID)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = biz.GatheringMobileNotiDelete(LoginUserID, GatheringID);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                    ret.ErrorMessage = "NOT Exists";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        /***************  WEEKLY ***************/        

        #region GetMyWeeklyByID 
        /// <summary>
        /// GetMyWeeklyByID
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultWeeklyView GetMyWeeklyByID(string weeklyID)
        {
            ResultWeeklyView ret = new ResultWeeklyView();

            try
            {
                long longWeeklyID = Convert.ToInt64(weeklyID);
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                WeeklyType weeklyType = biz.GetMyWeeklyByID(weeklyID);

                if (weeklyType != null && weeklyType.WeeklyID != 0)
                {
                    weeklyType.Contents = weeklyType.Contents;
                    weeklyType.TextContents = HttpUtility.HtmlDecode(weeklyType.TextContents).Replace("<br>", "\n");
                    ret.MyWeekly = weeklyType;
                }

                ret.CommentList = biz.GetWeeklyComment(longWeeklyID);
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

                // Mr.No 2015-07-01
                // Mobile Download FullURL 추가
                int WEEKLY_BOARD_ID = 315;  // Weekly 고유 넘버(첨부파일DB 고유 번호)
                //ret.attach = AttachmentHelper.Select(Convert.ToInt32(weeklyID), 0, WEEKLY_BOARD_ID);
                // Mr.No 2015-07-07
                ret.ROOT = biz.GetWeeklyMobileFile(Convert.ToInt32(weeklyID), 0, WEEKLY_BOARD_ID);
                
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region SaveMyWeekly
        [WebMethod]
        public ResultCommon SaveMyWeekly(WeeklyType weeklyType)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                weeklyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                weeklyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침
                weeklyType.Year = weeklyType.Date.Year;
                weeklyType.YearWeek = weeklyType.Date.WeekOfYear();
                weeklyType.StartWeekDate = weeklyType.Date.StartWeekDate();
                weeklyType.EndWeekDate = weeklyType.Date.EndWeekDate();

                biz.SaveMyWeekly(weeklyType);

                //팀장이 설정해 놓은 알림설정으로 인한 노트 발송
                CheckSendNoteMyWeekly(MyWeeklySendNoteType.MyWeekly, weeklyType.UserID.ToString(), weeklyType.WeeklyID.ToString(), weeklyType.Date, string.Empty);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            return ret;
        }

        //[WebMethod]
        public ResultCommon SaveMyWeekly1(string LoginUserId, string deptCode, string contents, string textContents, DateTime weedDate)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                WeeklyType weeklyType = new WeeklyType();
                weeklyType.UserID = LoginUserId;
                weeklyType.DeptCode = deptCode;
                weeklyType.Contents = contents;
                weeklyType.TextContents = textContents;
                weeklyType.Date = weedDate;

                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                weeklyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                weeklyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침
                weeklyType.Year = weeklyType.Date.Year;
                weeklyType.YearWeek = weeklyType.Date.WeekOfYear();
                weeklyType.StartWeekDate = weeklyType.Date.StartWeekDate();
                weeklyType.EndWeekDate = weeklyType.Date.EndWeekDate();

                biz.SaveMyWeekly(weeklyType);

                CheckSendNoteMyWeekly(MyWeeklySendNoteType.MyWeekly, weeklyType.UserID.ToString(), weeklyType.WeeklyID.ToString(), weeklyType.Date, string.Empty);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            return ret;
        }
        #endregion

        #region UpdateMyWeekly
        [WebMethod]
        public ResultCommon UpdateMyWeekly(WeeklyType weeklyType)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                weeklyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                weeklyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(weeklyType.TextContents.Replace("<br>", "\n"))).TrimEnd();
                biz.UpdateMyWeekly(weeklyType);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            return ret;
        }
        #endregion

        #region DeleteMyWeekly
        [WebMethod]
        public ResultCommon DeleteMyWeekly(string weeklyID, string LoginUserID = "")
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                biz.DeleteMyWeekly(weeklyID, LoginUserID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            return ret;
        }
        #endregion

        #region GetMyWeeklyByUserID
        /// <summary>
        /// GetMyWeeklyByUserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="deptCode"></param>
        /// <param name="weekDateTime"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultWeeklyType GetMyWeeklyByUserID(string userID, string deptCode, string weekDateTime)
        {
            ResultWeeklyType ret = new ResultWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (String.IsNullOrEmpty(weekDateTime))
                {
                    weekDateTime = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDateTime).StartWeekDate();
                
                WeeklyType weeklyType = biz.GetMyWeeklyByUserID(userID, deptCode, startWeekDate);

                if (weeklyType != null && weeklyType.WeeklyID != 0)
                {
                    weeklyType.Contents = weeklyType.Contents;
                    weeklyType.TextContents = HttpUtility.HtmlDecode(weeklyType.TextContents).Replace("<br>", "\n");
                    ret.MyWeeklyList.Add(weeklyType);
                }

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion 

        #region GetDeptWeeklyList
        /// <summary>
        /// GetDeptWeeklyList
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="deptCode"></param>
        /// <param name="weekDate"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultDeptWeeklyType GetDeptWeeklyList(string userID, string deptCode, string weekDate, int pageNum, int pageSize)
        {
            ResultDeptWeeklyType ret = new ResultDeptWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate);

                string TempFG = "N";
                ret.DeptWeeklyList = biz.GetDeptWeeklyList(userID, deptCode, startWeekDate.StartWeekDate(), pageNum, pageSize, TempFG);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        [WebMethod]
        public ResultDeptWeeklyType GetDeptWeeklyListNew(string userID, string deptCode, string weekDate, int pageNum, int pageSize)
        {
            ResultDeptWeeklyType ret = new ResultDeptWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate);

                string TempFG = "Y";
                ret.DeptWeeklyList = biz.GetDeptWeeklyList(userID, deptCode, startWeekDate.StartWeekDate(), pageNum, pageSize, TempFG);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetWeeklyTempUserList
        /// <summary>
        /// GetWeeklyTempUserList
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// /* 2015.04 MYWEEKLY */
        [WebMethod]
        public ResultDeptWeeklyType GetWeeklyTempUserList(string userID)
        {
            ResultDeptWeeklyType ret = new ResultDeptWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                
                ret.DeptWeeklyList = biz.GetWeeklyTempUserList(userID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        /* 2015.04 MYWEEKLY */
        #endregion 

        #region GetMyWeeklyComment - 사용안함
        /// <summary>
        /// GetMyWeeklyComment
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultWeeklyView GetMyWeeklyComment(string weeklyID)
        {
            ResultWeeklyView ret = new ResultWeeklyView();

            try
            {
                long longWeeklyID = Convert.ToInt64(weeklyID);
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                ret.CommentList = biz.GetWeeklyComment(longWeeklyID);
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region InsertMyWeeklyComment
        [WebMethod]
        public ResultCommon InsertMyWeeklyComment(string weeklyID, string parentCommentID, string contents, string userID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                WeeklyCommentType wct = new WeeklyCommentType();

                ImpersonUserinfo u = biz.UserSelect(userID);
                wct.UserID = userID;
                wct.UserName = u.Name;
                long longWeeklyID = 0;
                long.TryParse(weeklyID, out longWeeklyID);
                wct.WeeklyID = longWeeklyID;
                //2015-06-04 매니저가 Manager로 들어감
                //wct.DutyName = String.IsNullOrEmpty(u.PositionName) ? "Manager" : u.PositionName;
                wct.DutyName = String.IsNullOrEmpty(u.PositionName) ? "매니저" : u.PositionName;
                wct.DeptName = u.DeptName;
                wct.Contents = SecurityHelper.Clear_XSS_CSRF(contents);
                long longSUPID = 0;
                long.TryParse(parentCommentID, out longSUPID);
                wct.SUP_ID = longSUPID;
                wct = biz.InsertMyWeeklyComment(wct);

                //2015-06-05 CEO가 댓글 작성 시 메일 발송 추가 //1070537
                if (userID == System.Configuration.ConfigurationManager.AppSettings["CEOID"].ToString())
                {
                    sendMailMyWeeklyComment(userID, weeklyID, contents, parentCommentID);
                }

                //댓글일 경우
                if (String.IsNullOrEmpty(parentCommentID))
                {
                    CheckSendNoteMyWeekly(MyWeeklySendNoteType.Comment, userID, weeklyID, DateTime.Today, parentCommentID);
                }
                //댓글의 답글일 경우
                else
                {
                    CheckSendNoteMyWeekly(MyWeeklySendNoteType.CommentReply, userID, weeklyID, DateTime.Today, parentCommentID);
                }

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region ModifyMyWeeklyComment
        [WebMethod]
        public ResultCommon ModifyMyWeeklyComment(string weeklycommentID, string contents)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                WeeklyCommentType comment = new WeeklyCommentType();

                comment.WeeklyCommentID = Convert.ToInt64(weeklycommentID);
                comment.Contents = SecurityHelper.Clear_XSS_CSRF(contents);

                biz.UpdateMyWeeklyComment(comment);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region DeleteMyWeeklyComment
        [WebMethod]
        public ResultCommon DeleteMyWeeklyComment(string weeklycommentID)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                biz.DeleteMyWeeklyComment(Convert.ToInt64(weeklycommentID));

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetExtendUserInfo
        /// <summary>
        /// 겸직 포함
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultExtendUserInfo GetExtendUserInfo(string UserID)
        {
            ResultExtendUserInfo ret = new ResultExtendUserInfo();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ResultExtendUserInfo userInfo = biz.SelectExtendUserInfo(UserID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
                ret.UserInfo = userInfo.UserInfo;
                ret.AdditionalJobDeptNumberList = userInfo.AdditionalJobDeptNumberList;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetViewLevel
        [WebMethod]
        public ResultWeeklyData GetViewLevel(string UserID)
        {
            ResultWeeklyData ret = new ResultWeeklyData();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ImpersonUserinfo userInfo = biz.UserSelect(UserID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
                ret.Data = userInfo.ViewLevel ?? string.Empty;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region CommandSend
        [WebMethod]
        // 7117  skt.P068691@partner.sk.com&노창현/인트라넷
        // 7117  skt.P033028@partner.sk.com&김성환B/인트라넷
        public void CommandSend(string pWeeklyID, string pFromUser, string pToUser, string pCcUser, string pBccUser)
        {
            //WeeklyBiz weekBiz = new WeeklyBiz();
            //MailAddress fromMailAddress = null;
            ////20150623 김성환 수정; -> &
            //string[] arrFrom = pFromUser.Split('&');

            //if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].Equals("Y"))
            //{
            //    //fromMailAddress = new MailAddress("skt.P033028@partner.sk.com", "김성환B/인트라넷", System.Text.Encoding.UTF8);
            //    string testEmail = System.Configuration.ConfigurationManager.AppSettings["IsTestEmail"];
            //    string testName = System.Configuration.ConfigurationManager.AppSettings["IsTestEmailUserName"];

            //    fromMailAddress = new MailAddress(testEmail, testName, System.Text.Encoding.UTF8);
            //}
            //else
            //{
            //    fromMailAddress = new MailAddress(arrFrom[0], arrFrom[1], System.Text.Encoding.UTF8);
            //}

            //try
            //{
            //    CBHMailType data = new CBHMailType();
            //    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //    WeeklyType week = new WeeklyType();

            //    DataSet ds = weekBiz.WeeklySelect_WeeklyID(pWeeklyID);


            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = ds.Tables[0].Rows[0];
            //        week.Contents = (dr["Contents"] == DBNull.Value) ? null : dr.Field<string>("Contents");
            //        week.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreateDateTime");
            //        week.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<int>("ViewLevel");
            //        week.Year = (dr["Year"] == DBNull.Value) ? 0 : dr.Field<int>("Year");
            //        week.YearWeek = (dr["YearWeek"] == DBNull.Value) ? 0 : dr.Field<int>("YearWeek");
            //        week.DeptName = (dr["DeptName"] == DBNull.Value) ? null : dr.Field<string>("DeptName");
            //        week.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            //        week.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");
            //    }

            //    string linkUrl = "";
            //    if (week.ViewLevel == 1 || week.ViewLevel == 2)
            //        linkUrl = BaseURL + "Weekly/WeeklyListOrgChart.aspx";
            //    else
            //        linkUrl = BaseURL + "Weekly/WeeklyListTeam.aspx";
            //    linkUrl += "?date=" + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).ToString("yyyy-MM-dd");

            //    string strTmpWeekID = AESEncrytDecry.EncryptStringAES(week.WeeklyID.ToString());
            //    linkUrl += "&WeeklyID=" + strTmpWeekID;
            //    //linkUrl += "&WeeklyID=" + week.WeeklyID.ToString();

            //    /*
            //       Author : 개발자- 최현미C, 리뷰자-진현빈D
            //       CreateDae :  2016.11.10
            //       Desc : 메일 제목 변경 - 팀원일 경우
            //    */
            //    TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            //    ImpersonUserinfo u = biz.UserSelect(week.UserID); //사용자정보 

            //    string strSubject = string.Empty;
            //    if (week.ViewLevel.ToString().Trim().Equals("4"))
            //        strSubject = u.Name + " " + (u.PositionName.Equals("Manager") ? "매니저" : u.PositionName) + "의 " + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).StringMonthWeek() + " Weekly를 보내 드립니다.";
            //    else
            //        strSubject = week.DeptName + " " + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).StringMonthWeek() + " Weekly를 보내 드립니다.";

            //    data.Subject = strSubject;
            //    data.Content = EmailContent("Weekly", linkUrl, week.Contents.Trim(), pToUser, pCcUser, "N");
            //    SmtpMailSend(fromMailAddress, pToUser, pCcUser, pBccUser, data.Subject, data.Content);
            //    //메일로그
            //    weekBiz.WeeklyEmailSendLog_Insert(fromMailAddress.ToString(), pToUser, pCcUser, pBccUser, data.Subject, data.Content, "M");

            //    data.Subject = "[본인 확인] " + strSubject;
            //    //data.Subject = "[본인 확인] " + week.DeptName + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).StringMonthWeek() + " Weekly를 보내 드립니다.";

            //    data.Content = EmailContent("Weekly", linkUrl, week.Contents.Trim(), pToUser, pCcUser, "Y");
            //    SmtpMailSend(fromMailAddress, fromMailAddress.Address + "&" + fromMailAddress.DisplayName, "", "", data.Subject, data.Content);

            //}
            //catch (Exception ex)
            //{
            //    weekBiz.WeeklyEmailSendLog_Insert(fromMailAddress.ToString(), pToUser, pCcUser, pBccUser, "[이메일 전송 오류]", ex.Message.ToString(), "M");
            //}
        }
        
       

        #endregion

        #region SaveMyWeeklyOutlookAddIn
        [WebMethod]
        public string SaveMyWeeklyOutlookAddIn(string email, string htmlContents, string textContents)
        {
            ResultCommon ret = new ResultCommon();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                htmlContents = SecurityHelper.Clear_XSS_CSRF(htmlContents).Trim();
                textContents = SecurityHelper.Clear_XSS_CSRF(textContents).Trim();
                biz.SaveMyWeeklyByOutlook(email, htmlContents, textContents);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

                CheckSendNoteMyWeekly(MyWeeklySendNoteType.MyWeekly, email, string.Empty, DateTime.Today, string.Empty);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            string jsonValue = new JavaScriptSerializer().Serialize(ret);
            string originContentType = Context.Response.ContentType;
            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Output.Write(jsonValue);
            Context.Response.End();
            Context.Response.ContentType = originContentType;
            return String.Empty;
        }

        #endregion

        #region IsVisibleMyWeeklyOutlookAddIn - 사용안함
        [WebMethod]
        public string IsVisibleMyWeeklyOutlookAddIn(string employeeID)
        {
            ResultOutlookWeeklyType ret = new ResultOutlookWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                bool isVisibleAddIn = biz.IsVisibleMyWeeklyOutlookAddIn(employeeID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
                ret.IsVisibleAddin = isVisibleAddIn;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }

            string jsonValue = new JavaScriptSerializer().Serialize(ret);
            string originContentType = Context.Response.ContentType;
            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Output.Write(jsonValue);
            Context.Response.End();
            Context.Response.ContentType = originContentType;
            return String.Empty;
        }


        #endregion

        #region GetMyWeeklyByFromUserID - 사용안함
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weeklyID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        //2015-06-11 KSH
        [WebMethod]
        public ResultWeeklyView GetMyWeeklyByFromUserID(string weeklyID, string UserID)
        {
            ResultWeeklyView ret = new ResultWeeklyView();

            try
            {
                long longWeeklyID = Convert.ToInt64(weeklyID);
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                WeeklyType weeklyType = biz.GetMyWeeklyByFromUserID(weeklyID, UserID);

                if (weeklyType != null && weeklyType.WeeklyID != 0)
                {
                    weeklyType.Contents = weeklyType.Contents;
                    weeklyType.TextContents = HttpUtility.HtmlDecode(weeklyType.TextContents).Replace("<br>", "\n");
                    ret.MyWeekly = weeklyType;
                }

                ret.CommentList = biz.GetWeeklyComment(longWeeklyID);
                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        //2015-06-11 KSH
        #endregion

        /////////////////////////////////
        //2016-06 추가 Interface
        /////////////////////////////////

        #region GetDeptMonthlyList
        [WebMethod]
        /// LoginUserID : 1070362 , deptCode : 3355 , weekDate : 2016-06-08
        public ResultDeptMonthlyType GetDeptMonthlyList(string LoginUserID, string deptCode, string weekDate)
        {

            ResultDeptMonthlyType ret = new ResultDeptMonthlyType();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate).StartWeekDate();

                ret.DeptMonthlyList = biz.GetDeptMonthlyList(LoginUserID, deptCode, weekDate, 1, 50);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetMonthlyByID
        /// 228 , 먼슬리는 댓글이 없다
        [WebMethod]
        public ResultMonthlyView GetMonthlyByID(string monthlyID)
        {
            ResultMonthlyView ret = new ResultMonthlyView();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                long longWeeklyID = Convert.ToInt64(monthlyID);
                MonthlyType _type = biz.GetMonthlyByID(monthlyID);

                if (_type != null && _type.WeeklyID != 0)
                {
                    _type.Contents = _type.Contents;
                    _type.TextContents = HttpUtility.HtmlDecode(_type.TextContents).Replace("<br>", "\n");
                    ret.monthlyType = _type;
                }

                // Mr.No 2015-07-01
                // Mobile Download FullURL 추가
                int WEEKLY_BOARD_ID = 316;  // Monthly 고유 넘버(첨부파일DB 고유 번호)
                //ret.attach = AttachmentHelper.Select(Convert.ToInt32(weeklyID), 0, WEEKLY_BOARD_ID);
                // Mr.No 2015-07-07
                ret.ROOT = biz.GetWeeklyMobileFile(longWeeklyID, 0, WEEKLY_BOARD_ID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetMonthlyByUserID
        //1070362 ,  먼슬리는 댓글이 없다
        [WebMethod]
        public ResultMonthlyView GetMonthlyByUserID(string LoginUserID, string deptCode, string weekDate)
        {

            ResultMonthlyView ret = new ResultMonthlyView();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate).StartWeekDate();

                MonthlyType _type = biz.GetMonthlyByUserID(LoginUserID, deptCode, startWeekDate);

                if (_type != null && _type.WeeklyID != 0)
                {
                    _type.Contents = _type.Contents;
                    _type.TextContents = HttpUtility.HtmlDecode(_type.TextContents).Replace("<br>", "\n");
                    ret.monthlyType = _type;
                }

                // Mr.No 2015-07-01
                // Mobile Download FullURL 추가
                int WEEKLY_BOARD_ID = 316;  // Monthly 고유 넘버(첨부파일DB 고유 번호)
                //ret.attach = AttachmentHelper.Select(Convert.ToInt32(weeklyID), 0, WEEKLY_BOARD_ID);
                // Mr.No 2015-07-07
                ret.ROOT = biz.GetWeeklyMobileFile(_type.WeeklyID, 0, WEEKLY_BOARD_ID);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region SaveMonthly 
        [WebMethod]
        public ResultCommon SaveMonthly(MonthlyType monthlyType)
        {

            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                monthlyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                monthlyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침
                monthlyType.Year = monthlyType.Date.Year;
                monthlyType.YearWeek = monthlyType.Date.WeekOfYear();
                monthlyType.StartWeekDate = monthlyType.Date.StartWeekDate();
                monthlyType.EndWeekDate = monthlyType.Date.EndWeekDate();

                DataSet ds = biz.SaveMonthly(monthlyType);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }

        //[WebMethod]
        public ResultCommon SaveMonthly1(string LoginUserId, string deptCode, string contents, string textContents, DateTime weedDate)
        {
            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();


                MonthlyType monthlyType = new MonthlyType();

                monthlyType.UserID = LoginUserId;
                monthlyType.DeptCode = deptCode;
                monthlyType.Contents = contents;
                monthlyType.TextContents = textContents;
                monthlyType.Date = weedDate;

                monthlyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                monthlyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                //2015.12.18 위클리 1년 사용으로 개발해놔서 운영에서 다 뜯어고침
                monthlyType.Year = monthlyType.Date.Year;
                monthlyType.YearWeek = monthlyType.Date.WeekOfYear();
                monthlyType.StartWeekDate = monthlyType.Date.StartWeekDate();
                monthlyType.EndWeekDate = monthlyType.Date.EndWeekDate();

                DataSet ds = biz.SaveMonthly(monthlyType);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }

            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region UpdateMonthly 
        [WebMethod]
        public ResultCommon UpdateMonthly(MonthlyType monthlyType)
        {

            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                monthlyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                monthlyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                DataSet ds = biz.UpdateMonthly(monthlyType);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        //[WebMethod]
        public ResultCommon UpdateMonthly1(string monthlyID, string LoginUserId, string deptCode, string contents, string textContents)
        {

            ResultCommon ret = new ResultCommon();
            try
            {
                MonthlyType monthlyType = new MonthlyType();

                monthlyType.WeeklyID = long.Parse(monthlyID);
                monthlyType.UserID = LoginUserId;
                monthlyType.DeptCode = deptCode;
                monthlyType.Contents = contents;
                monthlyType.TextContents = textContents;

                monthlyType.Contents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.Contents.Replace("<br>", "\n")).Replace("\n", "<br>"));
                monthlyType.TextContents = SecurityHelper.Clear_XSS_CSRF(HttpUtility.HtmlEncode(monthlyType.TextContents.Replace("<br>", "\n"))).TrimEnd();

                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = biz.UpdateMonthly(monthlyType);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region DeleteMonthly 
        [WebMethod]
        public ResultCommon DeleteMonthly(string monthlyID, string LoginUserID = "")
        {

            ResultCommon ret = new ResultCommon();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                DataSet ds = biz.DeleteMonthly(monthlyID, LoginUserID);

                string DBFLAG = "0";
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DBFLAG = dr["DBFLAG"].ToString();
                    }
                }

                if (int.Parse(DBFLAG) == 0)
                {
                    ret.IsSuccess = "true";
                    ret.ErrorCode = "0";
                }
                else
                {
                    ret.IsSuccess = "false";
                    ret.ErrorCode = "9999";
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region MonthlyCommandSend 
        // 216  skt.P068691@partner.sk.com&노창현/인트라넷
        // 216  skt.P033028@partner.sk.com&김성환B/인트라넷
        [WebMethod]
        public ResultCommon MonthlyCommandSend(string pMonthlyID, string pFromUser, string pToUser, string pCcUser, string pBccUser)
        {
            ResultCommon ret = new ResultCommon();
            //MonthlyBiz monthlyBiz = new MonthlyBiz();

            //MailAddress fromUser = null;
            //string[] arrFrom = pFromUser.Split('&');
            
            //if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].Equals("Y"))
            //{
            //    string testEmail = System.Configuration.ConfigurationManager.AppSettings["IsTestEmail"];
            //    string testName = System.Configuration.ConfigurationManager.AppSettings["IsTestEmailUserName"];
            //    fromUser = new MailAddress(testEmail, testName, System.Text.Encoding.UTF8);
            //}
            //else
            //{
            //    fromUser = new MailAddress(arrFrom[0], arrFrom[1], System.Text.Encoding.UTF8);
            //}

            //try
            //{
            //    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    CBHMailType data = new CBHMailType();
            //    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                
            //    MonthlyType week = new MonthlyType();

            //    DataSet ds = monthlyBiz.MonthlySelect_WeeklyID(pMonthlyID);

            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = ds.Tables[0].Rows[0];
            //        week.Contents = (dr["Contents"] == DBNull.Value) ? null : dr.Field<string>("Contents");
            //        week.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("CreateDateTime");
            //        week.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<int>("ViewLevel");
            //        week.Year = (dr["Year"] == DBNull.Value) ? 0 : dr.Field<int>("Year");
            //        week.YearWeek = (dr["YearWeek"] == DBNull.Value) ? 0 : dr.Field<int>("YearWeek");
            //        week.DeptName = (dr["DeptName"] == DBNull.Value) ? null : dr.Field<string>("DeptName");
            //        week.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            //        week.MonthlyYYYY = (dr["MonthlyYYYY"] == DBNull.Value) ? null : dr.Field<string>("MonthlyYYYY");
            //        week.MonthlyMM = (dr["MonthlyMM"] == DBNull.Value) ? null : dr.Field<string>("MonthlyMM");

            //    }

            //    string linkUrl = "";
            //    linkUrl = BaseURL + "Monthly/MonthlyListOrgChart.aspx";
            //    linkUrl += "?date=" + week.MonthlyYYYY + "-" + week.MonthlyMM + "-01";
            //    string strTmpWeekID = AESEncrytDecry.EncryptStringAES(week.WeeklyID.ToString());
            //    linkUrl += "&WeeklyID=" + strTmpWeekID;

            //    //메일발송
            //    data.Subject = week.DeptName + " " + int.Parse(week.MonthlyMM).ToString() + "월" + " Monthly를 보내 드립니다.";
            //    data.Content = EmailContent("Monthly", linkUrl, week.Contents.Trim(), "", "", "N");
            //    SmtpMailSend(fromUser, pToUser, pCcUser, pBccUser, data.Subject, data.Content);
            //    monthlyBiz.WeeklyEmailSendLog_Insert(fromUser.ToString(), pToUser, pCcUser, pBccUser, data.Subject, data.Content, "M");

            //    //본인확인용 메일발송
            //    data.Subject = "[본인 확인] " + week.DeptName + " " + int.Parse(week.MonthlyMM).ToString() + "월" + " Monthly를 보내 드립니다.";
            //    //data.Subject = "[본인 확인] " + week.DeptName + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).StringMonthWeek() + " Monthly 를 보내 드립니다.";
            //    data.Content = EmailContent("Monthly", linkUrl, week.Contents.Trim(), pToUser, pCcUser, "Y");
            //    SmtpMailSend(fromUser, fromUser.Address + "&" + fromUser.DisplayName, "", "", data.Subject, data.Content);
               
            //    ret.IsSuccess = "true";
            //    ret.ErrorCode = "0";
            //}
            //catch (Exception ex)
            //{
            //    ret.IsSuccess = "false";
            //    ret.ErrorCode = "9999";
            //    ret.ErrorMessage = ex.ToString();

            //    monthlyBiz.WeeklyEmailSendLog_Insert(fromUser.ToString(), pToUser, pCcUser, pBccUser, "[이메일 전송 오류]", ex.Message.ToString(), "M");
            //}
            return ret;
        }

      
       
        #endregion

        #region WeeklyNoteInsert
        /// <summary>
        /// 위클리노트 작성(팀장)
        /// </summary>
        /// 팀장 : 1102996 , 7113, mobiletest
        [WebMethod]
        public ResultCommon WeeklyNoteInsert(string LoginUserID, string WeeklyID, string MemoContents, string ChkMailFlag)
        {
            ResultCommon ret = new ResultCommon();
            //try
            //{
            //    TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            //    ImpersonUserinfo _user = biz.UserSelect(LoginUserID); //사용자정보

            //    //Step 1 : 위클리 저장
            //    WeeklyBiz Biz = new WeeklyBiz();
            //    int returnValue = Biz.Weekly_Memo_Select(Convert.ToInt32(WeeklyID));

            //    if (returnValue == 0) // 처음작성 0 
            //    {
            //        Biz.Weekly_Memo_Update(Convert.ToInt32(WeeklyID), LoginUserID, SecurityHelper.Clear_XSS_CSRF(MemoContents), DateTime.Now);
            //    }
            //    else // 업데이트 1
            //    {
            //        Biz.Weekly_Memo_Update(Convert.ToInt32(WeeklyID), LoginUserID, SecurityHelper.Clear_XSS_CSRF(MemoContents), new DateTime(0));
            //    }


            //    // Step 2: E-HR연동 or 메일발송 기능
            //    DataSet ds = Biz.WeeklySelect_WeeklyID(WeeklyID);
            //    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            //    string C_contents = string.Empty;
            //    WeeklyType week = new WeeklyType();
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = ds.Tables[0].Rows[0];
            //        week.UserID = (dr["UserID"] == DBNull.Value) ? null : dr.Field<string>("UserID");

            //        // Mr.No 2015-07-22 HTML 전달
            //        week.Contents = (dr["Contents"] == DBNull.Value) ? string.Empty : Convert.ToString(dr.Field<string>("Contents"));
            //        week.CreateDateTime = (dr["MemoCreateDateTime"] == DBNull.Value) ? new DateTime(0) : dr.Field<DateTime>("MemoCreateDateTime");
            //        week.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<int>("ViewLevel");
            //        week.DeptName = (dr["DeptName"] == DBNull.Value) ? null : dr.Field<string>("DeptName");
            //        week.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            //        week.Year = (dr["Year"] == DBNull.Value) ? 0 : dr.Field<int>("Year");
            //        week.YearWeek = (dr["YearWeek"] == DBNull.Value) ? 0 : dr.Field<int>("YearWeek");

            //        string linkUrl = "";
            //        if (week.ViewLevel == 1 || week.ViewLevel == 2)
            //            linkUrl = BaseURL + "/Weekly/WeeklyListOrgChart.aspx";
            //        else
            //            linkUrl = BaseURL + "/Weekly/WeeklyListTeam.aspx";
            //        linkUrl += "?date=" + WeeklyBiz.FirstDateOfWeekISO8601Date(week.Year, week.YearWeek).ToString("yyyy-MM-dd");

            //        string strTmpWeekID = AESEncrytDecry.EncryptStringAES(week.WeeklyID.ToString());
            //        linkUrl += "&WeeklyID=" + strTmpWeekID;
                    
            //        SAPConnectorHelper hr = new SAPConnectorHelper();
            //        hr.SendMemo(LoginUserID, week.UserID, week.CreateDateTime.ToString("yyyy-MM-dd"), linkUrl, week.Contents, MemoContents);

            //        if (ChkMailFlag.Equals("Y"))
            //        {
            //            WeeklyNoteEmailType noteType = new WeeklyNoteEmailType();

            //            noteType.FromUserName = _user.Name;
            //            noteType.FromDeptName = _user.DeptName;
            //            noteType.FromPositionName = _user.PositionName;
            //            noteType.FromEmailAddress = _user.EmailAddress;
            //            noteType.ToUserName = dr["UserName"].ToString();
            //            noteType.ToDeptName = dr["DeptName"].ToString();
            //            noteType.ToEmailAddress = dr["EMAIL_ALIAS"].ToString();
            //            noteType.Year = week.Year;
            //            noteType.YearWeek = week.YearWeek;
            //            noteType.MemoContents = MemoContents;
            //            noteType.LinkUrl = linkUrl;
            //            noteType.MyConfirmMailFlag = "N";

            //            EmailWeeklyNoteMemoSend(noteType);
            //        }
            //    }

            //    ret.IsSuccess = "true";
            //    ret.ErrorCode = "0";
            //}
            //catch (Exception ex)
            //{
            //    ret.IsSuccess = "false";
            //    ret.ErrorCode = "9999";
            //    ret.ErrorMessage = ex.ToString();
            //}
            return ret;
        }

        public static void EmailWeeklyNoteMemoSend(WeeklyNoteEmailType noteType)
        {
            string mailServerName = System.Configuration.ConfigurationManager.AppSettings["MailServer"];
            MailAddress fromUser = null;

            if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].Equals("Y"))
            {
                string testEmail = System.Configuration.ConfigurationManager.AppSettings["IsTestEmail"];
                string testName = System.Configuration.ConfigurationManager.AppSettings["IsTestEmailUserName"];

                fromUser = new MailAddress(testEmail, testName, System.Text.Encoding.UTF8);
            }
            else
            {

                fromUser = new MailAddress(noteType.FromEmailAddress, noteType.FromUserName + "/" + noteType.FromDeptName, System.Text.Encoding.UTF8);
            }

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = fromUser;
                    MailAddress toAddress = null;

                    if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].Equals("Y"))
                    {
                        string testToEmail = System.Configuration.ConfigurationManager.AppSettings["IsTestEmail"];
                        string testToName = System.Configuration.ConfigurationManager.AppSettings["IsTestEmailUserName"];

                        toAddress = new MailAddress(testToEmail, testToName, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        if (noteType.MyConfirmMailFlag.Equals("N"))
                            toAddress = new MailAddress(noteType.ToEmailAddress, noteType.ToUserName + "/" + noteType.ToDeptName, System.Text.Encoding.UTF8);
                        else //본인확인
                            toAddress = new MailAddress(noteType.FromEmailAddress, noteType.FromUserName + "/" + noteType.FromDeptName, System.Text.Encoding.UTF8);
                    }

                    message.To.Add(toAddress);

                    string pTitle = string.Empty;
                    if (noteType.MyConfirmMailFlag.Equals("N"))
                    {
                        //message.Subject = noteType.FromUserName + " " + noteType.FromPositionName + "님이 작성한 " + WeeklyBiz.FirstDateOfWeekISO8601Date(noteType.Year, noteType.YearWeek).StringMonthWeek() + " WeeklyNote를 보내 드립니다.";
                        pTitle = noteType.FromUserName + " " + noteType.FromPositionName + "님께서 Weekly Note로 작성하신 피드백을 공유하셨습니다.";
                    }
                    else //본인확인
                    {
                        pTitle = "[본인확인] " + noteType.ToUserName + " 매니저에게 작성한 WeeklyNote를 보내 드립니다.";
                    }
                    message.Subject = pTitle;
                    message.SubjectEncoding = Encoding.UTF8;

                    string emailContent = string.Empty;
                    string convertContents = noteType.MemoContents.Replace("TEXT-DECORATION: \"", "TEXT-DECORATION: ;\"").Replace("\n", "<br/>");

                    emailContent = "<html>";
                    emailContent += "<head><title>이메일 발송</title><style type=\"text/css\">";
                    emailContent += "</style></head>";
                    //old emailContent += "<body><table cellpadding='0' cellspacing='0' style=\"width:680px\"><tr><td>"; 
                    emailContent += "<body><table cellpadding='0' cellspacing='0' style=\"width:100%;table-layout:auto;word-wrap:break-word;word-break:break-all;border-collapse:collapse;\"><tr><td>";
                    emailContent += "<p style=\"font-size:11px; padding:8px 0; border:1px solid #eeeeee; text-align:left; background:#eeeeee;\">";
                    emailContent += "&nbsp;&nbsp;<font face=\"맑은 고딕\" size=\"2\">본 내용은 " + noteType.ToUserName + " 매니저님이 작성하신 " + WeeklyBiz.FirstDateOfWeekISO8601Date(noteType.Year, noteType.YearWeek).StringMonthWeek() + "차 Weekly에 대해서, " + noteType.FromUserName + " " + noteType.FromPositionName + "님이 Weekly Note<br/>";
                    emailContent += "&nbsp;&nbsp;(e-HR시스템과 연동된 팀장 전용메뉴)로 작성하신 내용을 이메일로 공유하신 사항입니다.</font>";
                    emailContent += "</p></td></tr>";

                    if (noteType.MyConfirmMailFlag.Equals("Y"))
                    {

                        emailContent += "<tr>";
                        emailContent += "<td style=\"font-size:11px; padding:8px 0;border:2px solid #efefef; text-align:left;\">";
                        emailContent += "<table>";
                        emailContent += "<tr>";
                        emailContent += "<td style=\"vertical-align: top;\">";
                        emailContent += "<span style=\"color:black; font-weight: bold; font-size:11px;\">&nbsp;&nbsp;To&nbsp;:&nbsp;</span>";
                        emailContent += "</td>";
                        emailContent += "<td style=\"font-size:small;\">";
                        emailContent += noteType.ToUserName + "/" + noteType.ToDeptName;
                        emailContent += "</td>";
                        emailContent += "</tr>";
                        emailContent += "</table></td></tr>";
                    }
                    emailContent += "</table><br/>";

                    emailContent += "<table style=\"width:320px;word-break: break-all; border:1;\">"; //width 사이즈 320이 적절
                    emailContent += "<tr>";
                    emailContent += "<td>";
                    emailContent += "<p style=\"font-size:14px; font-family:'맑은 고딕';\">" + convertContents + "</p>";
                    emailContent += "</td>";
                    emailContent += "</tr>";
                    emailContent += "</table>";
                    emailContent += "</body></html>";


                    message.Body = emailContent;
                    message.IsBodyHtml = true;
                    message.BodyEncoding = Encoding.UTF8;

                    SmtpClient mailClient = new SmtpClient(mailServerName);

                    mailClient.Send(message);

                    //2015-06-24 김성환 메일 로그 인서트
                    WeeklyBiz weekBiz = new WeeklyBiz();
                    weekBiz.WeeklyEmailSendLog_Insert(fromUser.ToString(), toAddress.ToString(), "", "", pTitle, emailContent, "M");

                }
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("메일 주소 형식이 옳바르지 않습니다.", ex);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("메일 전송 프로토콜 오류", ex);
            }


        }
        #endregion

        #region GetMemberWeekly
        /// <summary>
        /// 팀장일 경우 -> 팀원, 팀원일 경우 팀원
        /// </summary>
        /// 팀장 : 1102996 , 1090, T , 2016-05-29
        /// 팀원 : 1109271 , 1090, M
        [WebMethod]
        public ResultWeeklyType GetMemberWeekly(string LoginUserID, string deptCode, string UserFG, string weekDate)
        {
            ResultWeeklyType ret = new ResultWeeklyType();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (UserFG.Equals("M")) //멤버일경우
                {
                    ret = biz.GetMemberWeekly(LoginUserID, deptCode, UserFG, Convert.ToDateTime(weekDate).StartWeekDate());
                }
                else //팀장일경우
                {
                    ret = biz.GetMemberWeekly(LoginUserID, deptCode, UserFG, Convert.ToDateTime(weekDate).StartWeekDate());
                }

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region TikleAccessLog
        /// <summary>
        /// 모바일 접근 로그 
        /// </summary>
        [WebMethod]
        public ResultCommon TikleAccessLog(string LoginID, string AccessIP, string AccessHost)
        {
            ResultCommon ret = new ResultCommon();
           
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                biz.TikleAccessLog(LoginID, AccessIP, AccessHost);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetWeeklyExceptionList
        [WebMethod]
        /// LoginUserID : 1070362 , deptCode : 3355 , weekDate : 2016-06-08
        /// 타부서 정보 보기
        public ResultWeeklyExceptionList GetWeeklyExceptionList(string LoginUserID, string weekDate)
        {

            ResultWeeklyExceptionList ret = new ResultWeeklyExceptionList();
            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate);

                ret.WeeklyExceptionList = biz.GetWeeklyExceptionList(LoginUserID, startWeekDate);

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        #region GetWeeklyExecutiveList
        /// <summary>
        /// 임원일경우 목록 - 팀까지 나옴
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="deptCode"></param>
        /// <param name="weekDate"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [WebMethod]
        public ResultExecutiveWeeklyType GetWeeklyExecutiveList(string userID, string deptCode, string weekDate)
        {
            ResultExecutiveWeeklyType ret = new ResultExecutiveWeeklyType();

            try
            {
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                if (String.IsNullOrEmpty(weekDate))
                {
                    weekDate = DateTime.Today.ToString("yyyy.MM.dd");
                }

                //2015.12.18 위클리 등록시 년,주,주시작일,주종료일 저장 추가
                DateTime startWeekDate = Convert.ToDateTime(weekDate);

                ret.ExecutiveWeeklyList = biz.GetWeeklyExecutiveList(userID, deptCode, startWeekDate.StartWeekDate());

                ret.IsSuccess = "true";
                ret.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = "false";
                ret.ErrorCode = "9999";
                ret.ErrorMessage = ex.ToString();
            }
            return ret;
        }
        #endregion

        /***************  COMMON ***************/        

        #region sendMailMyWeeklyComment
        /// <summary>
        /// CEO 댓글 작성 시, Weekly 작성자에게 Mail로 댓글 내용 전송
        /// </summary>
        /// <param name="userID">댓글 작성자 ID</param>
        /// <param name="weeklyID">weeklyID</param>
        /// <param name="contents">댓글 내용</param>
        /// <param name="parentCommentID">상위 댓글 ID</param>
        public void sendMailMyWeeklyComment(string userID, string weeklyID, string contents, string parentCommentID)
        {
            try
            {
                //if (System.Configuration.ConfigurationManager.AppSettings["CBHService_Use_Flag"] == "N")
                //    return;

                //댓글 작성자 정보
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ImpersonUserinfo commentWriterInfo = biz.UserSelect(userID);

                //weekly 작성자 정보
                ResultWeeklyView rwv = GetMyWeeklyByID(weeklyID);
                ImpersonUserinfo targetUserInfo = biz.UserSelect(rwv.MyWeekly.UserID);

                // 댓글의 댓글에는 보내지 않는다.
                if (String.IsNullOrEmpty(parentCommentID))
                {
                    // weekly 작성자와 댓글 작성자가 동일할 때는 보내지 않음
                    if (commentWriterInfo.UserID != targetUserInfo.UserID)
                    {
                        string targetUser = targetUserInfo.Name + " " + targetUserInfo.PositionName;

                        string mailTitle = "장동현 사장님께서 " + targetUser + "님의 Weekly에 피드백을 남기셨습니다.";

                        StringBuilder mailBody = new StringBuilder();

                        mailBody.Append("<html><body>");
                        mailBody.AppendFormat("안녕하세요. {0}님<br>", targetUser);
                        mailBody.AppendFormat("{0}님이 {1}에 작성하신 Weekly에<br>", targetUser, rwv.MyWeekly.CreateDateTime.ToString("yyyy.mm.dd").ToString());
                        mailBody.AppendFormat("장동현 사장님께서 아래와 같은 피드백을 남기셨습니다. 감사합니다. <br>");
                        mailBody.AppendFormat("---------------------------------------------------------------<br>");
                        mailBody.AppendFormat(contents);
                        mailBody.AppendFormat("---------------------------------------------------------------<br>");
                        mailBody.AppendFormat("</body></html>");

                        //sendNoteMail(mailTitle, mailBody.ToString(), commentWriterInfo.EmailAddress.ToString(), targetUserInfo.EmailAddress.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(ex.ToString());
            }
        }
        #endregion

        #region SendNoteMyWeekly 
        public enum MyWeeklySendNoteType { MyWeekly = 0, AllMyWeekly, Comment, CommentReply };
        //public void SendNoteMyWeekly(MyWeeklySendNoteType type, ImpersonUserinfo sendUser, ImpersonUserinfo targetUser)
        public void SendNoteMyWeekly(MyWeeklySendNoteType type, string weeklyID, ImpersonUserinfo sendUser, ImpersonUserinfo targetUser)
        {
            //StringBuilder sendMsg = new StringBuilder();

            //switch (type)
            //{
            //    case MyWeeklySendNoteType.MyWeekly:
            //        sendMsg.AppendFormat("{0} {1}님이<br>", sendUser.Name, sendUser.PositionName);
            //        sendMsg.AppendLine("이번 주 Weekly를<br>");
            //        sendMsg.AppendLine("작성하셨습니다 :D<br>");
            //        break;
            //    case MyWeeklySendNoteType.AllMyWeekly:
            //        sendMsg.AppendLine("모든 팀원이<br>");
            //        sendMsg.AppendLine("이번 주 Weekly를<br>");
            //        sendMsg.AppendLine("작성하셨습니다 :D<br>");
            //        break;
            //    case MyWeeklySendNoteType.Comment:
            //        //2015-06-04 수정
            //        //sendMsg.AppendFormat("{0} {1}님이<br>", sendUser.Name, sendUser.PositionName);
            //        //sendMsg.AppendFormat("{0} {1}님의 Weekly에<br>", targetUser.Name, targetUser.PositionName);
            //        //sendMsg.AppendLine("댓글을 남기셨습니다. <br>");

            //        //weekly 댓글 있는 Page로 이동
            //        string weeklyURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString();

            //        //targetUser 기준이면 이래 바까라...
            //        //if (targetUser.ViewLevel == "3" || targetUser.ViewLevel == "4")
            //        if (sendUser.ViewLevel == "3" || sendUser.ViewLevel == "4")
            //        {
            //            weeklyURL += "Weekly/WeeklyListTeam.aspx?WeeklyID=" + AESEncrytDecry.EncryptStringAES(weeklyID);
            //        }
            //        else
            //        {
            //            weeklyURL += "Weekly/WeeklyListOrgChart.aspx?WeeklyID=" + AESEncrytDecry.EncryptStringAES(weeklyID);
            //        }

            //        sendMsg.AppendFormat("{0} {1}님이 {2} {3}님의 Weekly에 댓글을 남기셨습니다.<br>"
            //                                            , sendUser.Name
            //                                            , sendUser.PositionName
            //                                            , targetUser.Name
            //                                            , targetUser.PositionName);
            //        sendMsg.AppendLine("<html><body><br />");
            //        sendMsg.AppendFormat("<font face='맑은고딕' size='3'><a href={0}><br />▶ 댓글 내용 확인하기</a></font></body></html>"
            //                                               , weeklyURL);

            //        break;
            //    case MyWeeklySendNoteType.CommentReply:
            //        weeklyURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString();

            //        if (sendUser.ViewLevel == "3" || sendUser.ViewLevel == "4")
            //        {
            //            weeklyURL += "Weekly/WeeklyListTeam.aspx?WeeklyID=" + AESEncrytDecry.EncryptStringAES(weeklyID);
            //        }
            //        else
            //        {
            //            weeklyURL += "Weekly/WeeklyListOrgChart.aspx?WeeklyID=" + AESEncrytDecry.EncryptStringAES(weeklyID);
            //        }

            //        sendMsg.AppendFormat("{0} {1}님이 {2} {3}님의 Weekly에 댓글에 대해 답글을 남기셨습니다.<br>"
            //                                            , sendUser.Name
            //                                            , sendUser.PositionName
            //                                            , targetUser.Name
            //                                            , targetUser.PositionName);
            //        sendMsg.AppendLine("<html><body><br />");
            //        sendMsg.AppendFormat("<font face='맑은고딕' size='3'><a href={0}><br />▶ 답글 내용 확인하기</a></font></body></html>"
            //                                               , weeklyURL);

            //        break;
            //}

            ////CBHMSMQHelper helper = new CBHMSMQHelper();
            //CBHNoteType data = new CBHNoteType();

            //data.Content = sendMsg.ToString();
            //data.Kind = "3"; //일반쪽지.
            ////data.URL = NoteLink;
            //data.SendUserName = "티끌이";

            //string targetUserNoteID = targetUser.EmailAddress.Remove(targetUser.EmailAddress.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            //data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
            //data.TargetUser = targetUserNoteID;

            ////OK//helper.SendNoteToQueue(data);
            ////쪽지 20170802
            //CBHInterface.CBHNoteSend(data);

            ////메일 20170802
            //CBHInterface.CBHMailSend(targetUser.EmailAddress, "tikle@sk.com", "T.끌 알림 메일입니다.", sendMsg.ToString());

            //// 테스트용
            ////CBHHelper helper = new CBHHelper();
            //////string targetUserNoteID = targetUser.EmailAddress.Remove(targetUser.EmailAddress.IndexOf('@')); 
            ////string targetUserNoteID = "soyoung.jeon"; 
            ////string ret = helper.SendNote("tikle", "티끌이", targetUserNoteID, sendMsg.ToString(), "", "3");
        }

        public void CheckSendNoteMyWeekly(MyWeeklySendNoteType type, string userID, string weeklyID, DateTime insertDateTime, string parentCommentID)
        {
            try
            {
                //if (System.Configuration.ConfigurationManager.AppSettings["CBHService_Use_Flag"] == "N")
                //    return;

                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                ImpersonUserinfo userInfo = biz.UserSelect(userID);

                if (type == MyWeeklySendNoteType.MyWeekly)
                {
                    WeeklyTeamLeaderNotiCheckDac dac = new WeeklyTeamLeaderNotiCheckDac();
                    WeeklyTeamLeaderNotiCheckType notiChecktype = dac.WeeklyTeamLeaderNotiCheckSelect(userInfo.ManagerEmployeeID);

                    bool isSend = (insertDateTime.Year == DateTime.Today.Year && insertDateTime.WeekOfYear() == DateTime.Today.WeekOfYear());

                    // 새로 작성한 Weekly이면 0
                    if ((String.IsNullOrEmpty(weeklyID) || weeklyID == "0") && isSend)
                    {
                        ImpersonUserinfo targetUserInfo = biz.UserSelect(notiChecktype.UserID);

                        if (notiChecktype.TeamMemberYN.ToLower() == "y")
                        {
                            //2015-06-04 수정
                            //SendNoteMyWeekly(MyWeeklySendNoteType.MyWeekly, userInfo, targetUserInfo);
                            SendNoteMyWeekly(MyWeeklySendNoteType.MyWeekly, weeklyID, userInfo, targetUserInfo);
                        }

                        // 팀 전체 Weekly 작성시 쪽지보내기가 설정되어 있으면
                        // Weekly를 작성하지 않은 멤버 Count를 가져와 체크
                        if (notiChecktype.TeamMemberAllYN.ToLower() == "y")
                        {
                            DataSet ds = dac.WeeklyTeamLeaderNotiCheck_WeeklyCount(userInfo.DeptID, DateTime.Today);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string noWriteWeeklyCount = (dr["Count"] == DBNull.Value) ? string.Empty : dr["Count"].ToString();
                                    if (!String.IsNullOrEmpty(noWriteWeeklyCount) && noWriteWeeklyCount == "0")
                                    {
                                        SendNoteMyWeekly(MyWeeklySendNoteType.AllMyWeekly, weeklyID, userInfo, targetUserInfo);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (type == MyWeeklySendNoteType.Comment)
                {
                    ResultWeeklyView rwv = GetMyWeeklyByID(weeklyID);
                    ImpersonUserinfo targetUserInfo = biz.UserSelect(rwv.MyWeekly.UserID);
                    
                    // 자기 자신 댓글에는 보내지 않는다.
                    if (userInfo.UserID != targetUserInfo.UserID)
                    {
                        //2015-06-04 수정
                        //SendNoteMyWeekly(MyWeeklySendNoteType.Comment, userInfo, targetUserInfo);
                        SendNoteMyWeekly(type, weeklyID, userInfo, targetUserInfo);
                    }
                   
                }
                else if (type == MyWeeklySendNoteType.CommentReply)
                {
                    List<WeeklyCommentType> commtype = new TikleMobileWebServiceBiz().GetWeeklyComment(long.Parse(weeklyID));

                    string sTargetUserID = string.Empty;

                    foreach (WeeklyCommentType t in commtype)
                    {
                        if(t.WeeklyCommentID.Equals(long.Parse(parentCommentID)))
                        {
                            sTargetUserID = t.UserID;
                        }
                    }
                    ImpersonUserinfo targetUserInfo = biz.UserSelect(sTargetUserID);
                    //댓글쓴사람이 내가 아닌경우만 쪽지 발송
                    if (userInfo.UserID != targetUserInfo.UserID)
                    {
                        SendNoteMyWeekly(type, weeklyID, userInfo, targetUserInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                // 예외는 로그만 남김
                Log4NetHelper.Error(ex.ToString());
            }
        }
        #endregion

        #region sendNoteMail
        /// <summary>
        /// 메일 보내기
        /// </summary>
        /// <param name="title">메일 제목</param>
        /// <param name="body">메일 내용</param>
        /// <param name="senderEmailAddr">메일 발신자</param>
        /// <param name="receiverEmailAddr">메일 수신자</param>
        public void sendNoteMail(string title, string body, string senderEmailAddr, string receiverEmailAddr)
        {
            CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHMailType data = new CBHMailType();

            if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"] == "Y")
            {
                title = "[원래 수신자 : " + receiverEmailAddr + "]" + title;
                receiverEmailAddr = "tikle@sk.com";
            }

            data.Subject = title;
            data.Content = body;
            data.SenderEmail = senderEmailAddr;
            data.ReceiverEmail = receiverEmailAddr;

            helper.SendMailToQueue(data);
        }
        #endregion

        #region MakeURLLink
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
        #endregion 

        #region SendNoteQna
        ////쪽지보내기
        //protected void SendNoteQna(string Title, string ItemID, string Recipient)
        protected void SendNoteQna(string ItemID)
        {

            TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();

            GlossaryQnAType qdata = biz.GlossaryQnASelect(ItemID);

            string Title = qdata.Title;
            string Recipient = qdata.UserEmail;

            string NoteTitle = "[T.끌]: '" + Title + "'에 답변이 등록되었습니다.";

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            string NoteLink = BaseURL + "/QNA/QnaView.aspx?ItemID=" + ItemID;

            string NoteBody = "<font face=\"맑은고딕\" size=\"2\">안녕하세요, 티끌이입니다. ^^<br /><br />"
                            + "'T.끌'에 질문하셨던 '<a href=\"" + NoteLink + " \">" + Title + "</a>'에 답변이 등록되었습니다.<br /><br />"
                            + "확인 후, 원하시던 답변일 경우에는 꼭! 답변을 '채택'해주시기를 부탁드립니다. ^^<br /><br />"
                            + "오늘도 좋은 하루 되세요~! <br /><br /></font>"
                            + "<font face=\"맑은고딕\" size=\"2\">▶ 질문 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font>";

            //CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHNoteType data = new CBHNoteType();

            data.Content = NoteBody;
            data.Kind = "3"; //일반쪽지.
            data.URL = NoteLink;
            data.SendUserName = "티끌이";

            string userID = Recipient.Remove(Recipient.IndexOf('@')); //이메일 앞부분이 note id 값이다.
            data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
            data.TargetUser = userID;

            //OK//helper.SendNoteToQueue(data);

            //쪽지 20170802
            CBHInterface.CBHNoteSend(data);

            //메일 20170802
            CBHInterface.CBHMailSend(Recipient, "tikle@sk.com", "T.끌 알림 메일입니다.", NoteBody);

        }
        #endregion

        #region LikeSendNoteQna
        //좋아요 쪽지 보내기
        protected void LikeSendNoteQna(string ItemID, string CommonID)
        {

            //글 정보
            TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
            GlossaryQnAType qdata = biz.GlossaryQnASelect(ItemID);

            //댓글 정보
            TikleMobileWebServiceBiz biz_ = new TikleMobileWebServiceBiz();
            GlossaryQnACommentType Board = new GlossaryQnACommentType();
            Board = biz_.GlossaryQnACommentSelect(CommonID);
            if (Board.UserName != "비공개")
            {


                string Title = qdata.Title;
                string Recipient = Board.UserEmail;

                string NoteTitle = "[T.끌]: '" + Title + "'에 답글이 추천 되었습니다..";

                string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

                string NoteLink = BaseURL + "/QNA/QnaView.aspx?ItemID=" + ItemID;

                string NoteBody = "<font face=\"맑은고딕\" size=\"2\">안녕하세요, 티끌이입니다. ^^<br /><br />"
                                + Board.UserName + "님께서 질문 <STRONG>&lt;" + Title + "&gt;</STRONG> 에 남겨주신<br />"
                                + "답글이 추천받았습니다. 감사합니다.!"
                    //+ "'T.끌'에 질문하셨던 '<a href=\"" + NoteLink + " \">" + Title + "</a>'에 답변이 등록되었습니다.<br /><br />"
                    //+ "확인 후, 원하시던 답변일 경우에는 꼭! 답변을 '채택'해주시기를 부탁드립니다. ^^<br /><br />"
                    //+ "오늘도 좋은 하루 되세요~! <br /><br /></font>"
                                + "<font face=\"맑은고딕\" size=\"2\">▶ 질문 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font>";

                //CBHMSMQHelper helper = new CBHMSMQHelper();
                CBHNoteType data = new CBHNoteType();

                data.Content = NoteBody;
                data.Kind = "3"; //일반쪽지.
                data.URL = NoteLink;
                data.SendUserName = "티끌이";

                string userID = Recipient.Remove(Recipient.IndexOf('@')); //이메일 앞부분이 note id 값이다.
                data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                data.TargetUser = userID;

                //OK//helper.SendNoteToQueue(data);

                //쪽지 20170802
                CBHInterface.CBHNoteSend(data);

                //메일 20170802
                CBHInterface.CBHMailSend(Recipient, "tikle@sk.com", "T.끌 알림 메일입니다.", NoteBody);
            }

        }

        #endregion

        #region ConvertOutlookHTML
        public string ConvertOutlookHTML(string contents)
        {
            string resultStr = contents;

            //1. TEXT-DECORATION 닫기 태그 처리
            resultStr = resultStr.Replace("TEXT-DECORATION: \"", "TEXT-DECORATION: ;\"");

            return resultStr;

        } 
        #endregion

        #region EmailContent
        private string EmailContent(string gbn, string linkUrl, string Contents, string To, string Cc, string myFlag)
        {
            string emailContent = string.Empty;
            string convertContents = ConvertOutlookHTML(Contents);

            emailContent = "<html>";
            emailContent += "<head><title>이메일 발송</title>";
            emailContent += "<style type=\"text/css\">";
            emailContent += "</style>";
            emailContent += "<meta name='viewport' content='width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no'>";
            emailContent += "</head>";
            //emailContent += "<body><table style=\"width:680px\"><tr><td>"; //680px -> 730px로이동, font-size:12px -> 11px로이동.
            /*
              Author : 개발자-최현미C, 리뷰자-진현빈D
              Create Date : 2016.07.21 / 2016.07.28
              Desc : 메일 전달 시 폼 깨짐 / 메일사이즈 100% 설정
          */
            emailContent += "<body><table cellpadding='0' cellspacing='0' style=\"width:100%;table-layout:auto;word-wrap:break-word;word-break:break-all;border-collapse:collapse;\"><tr><td>";
            //emailContent += "<body><table cellpadding='0' cellspacing='0' style=\"width:680px\"><tr><td>"; 

            emailContent += "<p style=\"font-size:11px; padding:8px 0; border:1px solid #eeeeee; text-align:left; background:#eeeeee;\">";
            emailContent += "&nbsp;&nbsp;<font face=\"맑은 고딕\" size=\"2\">T.끌에서 작성된 " + gbn + "의 메일발송입니다.</font>";
            emailContent += "&nbsp;&nbsp;<a href='" + linkUrl + "'><font face=\"맑은 고딕\" size=\"2\">▶ "+ gbn +" 바로 가기</font></a>";
            emailContent += "<font color=\"#747474\"> (이메일을 통한 접속은 PC에서만 가능)</font>";
            emailContent += "</p></td></tr>";

            if (myFlag.Equals("Y"))
            {
                string strTo = string.Empty;
                string strCc = string.Empty;

                if (To.IndexOf(";") > -1)
                {
                    string[] arrTo = To.Split(';');

                    foreach (string too in arrTo)
                    {
                        if (too.Contains('&'))
                        {
                            string[] arrToTmp = too.Split('&');
                            strTo = arrToTmp[1];
                        }
                    }
                }
                else
                {
                    if (To.IndexOf("&") > -1)
                    {
                        string[] arrToTmp = To.Split('&');
                        strTo = arrToTmp[1];
                    }
                    else
                        strTo = To;
                }


                if (Cc.IndexOf(";") > -1)
                {
                    string[] arrCc = Cc.Split(';');

                    foreach (string ccc in arrCc)
                    {
                        if (ccc.Contains('&'))
                        {
                            string[] arrCcTmp = ccc.Split('&');
                            strCc += arrCcTmp[1] + ";";
                        }
                    }
                    if (strCc.IndexOf(";") > -1)
                        strCc = strCc.Substring(0, strCc.Length - 1);
                }
                else
                {
                    if (Cc.IndexOf("&") > -1)
                    {
                        string[] arrToTmp = Cc.Split('&');
                        strCc = arrToTmp[1];
                    }
                    else
                        strCc = Cc;
                }

                emailContent += "<tr>";
                emailContent += "<td style=\"font-size:11px; padding:8px 0;border:2px solid #efefef; text-align:left;\">";

                emailContent += "<table>";
                emailContent += "<tr>";
                emailContent += "<td style=\"vertical-align: top;\">";
                emailContent += "<span style=\"color:black; font-weight: bold; font-size:11px;\">&nbsp;&nbsp;To&nbsp;:&nbsp;</span>";
                emailContent += "</td>";
                emailContent += "<td style=\"font-size:small;\">";
                emailContent += strTo;
                emailContent += "</td>";
                emailContent += "</tr>";

                if (Cc.Length > 0)
                {
                    emailContent += "<tr>";
                    emailContent += "<td style=\"vertical-align: top;\">";
                    emailContent += "<span style=\"color:black; font-weight: bold; font-size:11px;\">&nbsp;&nbsp;Cc&nbsp;:&nbsp;</span>";
                    emailContent += "</td>";
                    emailContent += "<td style=\"font-size:small;\">";
                    emailContent += strCc;
                    emailContent += "</td>";
                    emailContent += "</tr>";

                }
                emailContent += "</table>";
                emailContent += "</td>";
                emailContent += "</tr>";
            }

            emailContent += "<tr><td style=\"padding-top:20px;\">";
            emailContent += convertContents;
            emailContent += "</td></tr></table></body></html>";

            return emailContent;
        }
        #endregion

        #region SmtpMailSend
        public void SmtpMailSend(MailAddress fromMail, string pTo, string pCC, string pBcc, string pTitle, string pContent)
        {
            string mailServerName = System.Configuration.ConfigurationManager.AppSettings["MailServer"];

            string[] arrTo = pTo.Split(';');
            string[] arrCc = pCC.Split(';');
            string[] arrBcc = pBcc.Split(';');

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = fromMail;

                    //받는사람
                    foreach (string to in arrTo)
                    {
                        if (!to.Equals(""))
                        {
                            string[] arrToTmp = to.Split('&');

                            MailAddress toAddress = new MailAddress(arrToTmp[0], arrToTmp[1], System.Text.Encoding.UTF8);
                            //MailAddress toAddress = new MailAddress("ckpj1@naver.com", "백충기", System.Text.Encoding.UTF8);
                            message.To.Add(toAddress);
                        }
                    }

                    //참조
                    foreach (string cc in arrCc)
                    {
                        if (!cc.Equals(""))
                        {
                            string[] arrCcTmp = cc.Split('&');
                            //MailAddress ccAddress = new MailAddress(cc);
                            MailAddress ccAddress = new MailAddress(arrCcTmp[0], arrCcTmp[1], System.Text.Encoding.UTF8);
                            message.CC.Add(ccAddress);
                        }
                    }

                    //숨은참조mm
                    foreach (string bcc in arrBcc)
                    {
                        if (!bcc.Equals(""))
                        {
                            string[] arrBccTmp = bcc.Split('&');
                            MailAddress bccAddress = new MailAddress(arrBccTmp[0], arrBccTmp[1], System.Text.Encoding.UTF8);
                            //MailAddress bccAddress = new MailAddress(bcc);
                            message.Bcc.Add(bccAddress);
                        }
                    }

                    //본인확인용으로 수신 추가
                    //message.To.Add(fromMail);

                    message.Subject = pTitle;
                    message.SubjectEncoding = Encoding.UTF8;

                    message.Body = pContent;
                    message.IsBodyHtml = true;
                    message.BodyEncoding = Encoding.UTF8;

                    SmtpClient mailClient = new SmtpClient(mailServerName);

                    mailClient.Send(message);
                }
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("메일 주소 형식이 옳바르지 않습니다.", ex);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("메일 전송 프로토콜 오류", ex);
            }
        } 
        #endregion

        #region SetRoleGlossary
        // CHG610000084398 / 20190502 / DT블로그 DT센터 15,19,P사번 권한부여
        public bool SetRoleGlossary(string UserID)
        {
            bool returnData = false;
            
            //1. 기본 권한 체크 (10/11/15)
            string RoleGlossary = ConfigurationManager.AppSettings["RoleGlossary"].ToString();

            string[] arr = RoleGlossary.Split(',');
            var target = UserID.Substring(0, 2);
            string match = Array.Find(arr, n => n.Contains(target));

            //2. 기본권한이 없으면 예외 사용자 체크 
            if (!string.IsNullOrEmpty(match))
            {
                returnData = true;
            }
            else 
            {
                //3.예외권한자확인
                TikleMobileWebServiceBiz biz = new TikleMobileWebServiceBiz();
                GlossaryQnACommentType Comment = new GlossaryQnACommentType();

                ImpersonUserinfo u = biz.UserSelect(UserID);

                string[] arrLevel = u.Level.Split(',');

                if (arr.Length == 0)
                {
                    switch (u.Level)
                    {
                        case "1": returnData = true; break; //티끌이
                        case "2": returnData = true; break; //관리자
                        case "G": returnData = true; break; //끌지식권한자
                    }
                }
                else
                {
                    string matchLevel = string.Empty;

                    matchLevel = Array.Find(arrLevel, n => n.Contains("1"));
                    if (!string.IsNullOrEmpty(matchLevel))
                        returnData = true;

                    matchLevel = Array.Find(arrLevel, n => n.Contains("2"));
                    if (!string.IsNullOrEmpty(matchLevel))
                        returnData = true;

                    matchLevel = Array.Find(arrLevel, n => n.Contains("G"));
                    if (!string.IsNullOrEmpty(matchLevel))
                        returnData = true;
                }
            }

            return returnData;
        }
        #endregion
    }
 
}

/*
// 웹서비스 시 프로시저 

 * 추가
UP_WEEKLY_SELECT_BY_MOBILE2
UP_LOGACCESS_INSERT_MOBLIE
TB_LOG_ACCESS_MOBILE
TB_LOG_WEEKLY (위클리 / 먼슬리 삭제 관리)

 * 변경
UP_COMMCOMMENT_UPDATE 
UP_COMMCOMMENT_DELETE
UP_WEEKLY_SELECT_BY_MOBILE
UP_MONTHLY_INSERT_BY_MOBILE
UP_MONTHLY_UPDATE_BY_MOBILE
UP_MONTHLY_SELECT_BY_MOBILE
UP_WEEKLY_SELECT_EXCEPTION_USER

UP_MONTHLY_SELECT_DEPTCODE_OFFICER (USER_NAME 추가)
UP_WEEKLY_SELECT_DEPTCODE_OFFICER (USER_NAME 추가)
UP_WEEKLY_SELECT_DEPTCODE_CEO (USER_NAME 추가)

UP_MONTHLY_SELECT_DEPTCODE
UP_WEEKLY_INSERT_BY_MOBILE 
UP_WEEKLY_UPDATE_BY_MOBILE
UP_MONTHLY_DELETE (TEST)
UP_WEEKLY_DELETE (TEST)
UP_WEEKLY_SELECT_DEPTCODE

*/