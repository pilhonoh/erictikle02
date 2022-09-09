using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Controls;
using System.Security.Cryptography;

namespace SKT.Common
{
    public class CommonActiveSquareEditor
    {
        protected static string _attrib = string.Empty;

        #region 액티브스퀘어 저장용
        public static SKT.Tnet.Controls.WebEditorData GetDecodeMIME(string pMIMEContent, string folder)
        {
            string bodyHtml = string.Empty;
            WebEditorData weData = null;

            try
            {
                if (string.IsNullOrEmpty(pMIMEContent) == false && string.IsNullOrWhiteSpace(pMIMEContent) == false)
                {

                    #region 환경 변수 설정
                    weData = new WebEditorData();

                    string NamoTempPath = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "NamoTempPath");
                    string FileRootKOR = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "ImageRootKOR");
                    string rootUrl = string.Format(FileRootKOR, folder);
                    string rootPath = HttpContext.Current.Server.MapPath(rootUrl);
                    string tempPath = string.Format(@"{0}\{1}", NamoTempPath, Guid.NewGuid().ToString("N"));
                    #endregion 환경 변수 설정

                    if (System.IO.Directory.Exists(tempPath) == false) System.IO.Directory.CreateDirectory(tempPath);

                    NamoMIME.MimeObject mime = new NamoMIME.MimeObject();
                    mime.Decode(pMIMEContent, tempPath);

                    #region 이미지 처리 - 로컬 이미지만 서버에 업로드

                    bodyHtml = GetBodyHtml(tempPath);
                    Stack<string[]> bodyImg = GetBodyImg(tempPath);

                    /*
                        Author : 개발자-장찬우G, 리뷰자-진현빈D 
                        Create Date : 2016.06.29 
                        Desc : Impersonation
                    */
                    Impersonation im = new Impersonation();
                    im.ImpersonationStart();
                    try
                    {
                        if (bodyImg != null && bodyImg.Count > 0)
                        {
                            if (System.IO.Directory.Exists(rootPath) == false) System.IO.Directory.CreateDirectory(rootPath);

                            foreach (string[] files in bodyImg)
                            {
                                foreach (string file in files)
                                {
                                    System.IO.FileInfo fi = new FileInfo(file);

                                    string GuidValue = Guid.NewGuid().ToString("N");
                                    string fileName = string.Format("{0}{1}", GuidValue, fi.Extension);
                                    string movePath = string.Format(@"{0}\{1}", rootPath, fileName);


                                    try
                                    {
                                        //서버 UAC권한설정때문에 잘라내기는 실패해서, copy로 change
                                        //fi.MoveTo(movePath);

                                        fi.CopyTo(movePath, false);

                                        //서버 UAC권한설정때문에 access거부
                                        //fi.Delete();

                                    }
                                    catch (UnauthorizedAccessException ex1)
                                    {
                                        Log4NetHelper.Error(ex1.Message.ToString());
                                        throw ex1;
                                    }

                                    bodyHtml = bodyHtml.Replace(file, string.Format("http://{0}{1}/{2}",
                                                                            HttpContext.Current.Request.Url.Authority,
                                                                            rootUrl,
                                                                            fileName));

                                    //string ThumbnailFile = string.Format("{0}_M{1}", GuidValue, fi.Extension);
                                    //string ThumbnailPath = string.Format(@"{0}\{1}", rootPath, ThumbnailFile);

                                    System.IO.FileInfo moveFile = new FileInfo(movePath);
                                    //moveFile.CopyTo(ThumbnailPath);                                    

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        im.ImpersonationEnd();
                        throw ex;
                    }


                    //서버 UAC권한설정때문에 위에서 파일을 안지우기 때문에 폴더삭제 안함
                    //if (System.IO.Directory.Exists(tempPath) == true) System.IO.Directory.Delete(tempPath);

                    weData.HtmlBody = bodyHtml;

                    #endregion 이미지 처리 - 로컬 이미지만 서버에 업로드

                    #region HTML 내 이미지 파일 목록 취득
                    List<Uri> links = ImageHelper.GetImageURLS(bodyHtml);

                    try
                    {
                        if (links != null && links.Count > 0)
                        {
                            weData.ImageFiles = new string[links.Count];

                            int iCnt = 0;

                            foreach (Uri imageUrl in links)
                            {
                                if (imageUrl.ToString().Contains("http://" + HttpContext.Current.Request.Url.Authority) == true)
                                {
                                    weData.ImageFiles[iCnt] = imageUrl.ToString().Replace("http://" + HttpContext.Current.Request.Url.Authority, "");

                                    iCnt++;
                                }
                            }

                            if (iCnt == 0)
                            {
                                if (System.IO.Directory.Exists(rootPath) == false) System.IO.Directory.Delete(rootPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        im.ImpersonationEnd();
                        throw ex;
                    }
                    im.ImpersonationEnd();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                weData = null;
            }

            return weData;
        }

        /// <summary>
        /// Namo Web Editor의 HTML 파싱 처리 함수
        /// </summary>
        /// <param name="tempPath"></param>
        /// <returns></returns>
        private static string GetBodyHtml(string tempPath)
        {
            string strRtn = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(tempPath) == false && string.IsNullOrWhiteSpace(tempPath) == false)
                {
                    tempPath += "\\noname.htm";
                    if (System.IO.File.Exists(tempPath) == true)
                    {
                        using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
                        {
                            strRtn = sr.ReadToEnd();
                        }
                        System.IO.File.Delete(tempPath);
                    }
                }
            }
            catch
            {
                strRtn = string.Empty;
            }

            return strRtn;
        }

        /// <summary>
        /// Namo Web Editor의 파일 파싱 처리 함수
        /// </summary>
        /// <param name="tempPath"></param>
        /// <returns></returns>
        private static Stack<string[]> GetBodyImg(string tempPath)
        {
            Stack<string[]> oRtn = null;
            string[] sFileItem = null;
            System.IO.DirectoryInfo di = null;
            int i;

            try
            {
                if (string.IsNullOrEmpty(tempPath) == false && string.IsNullOrWhiteSpace(tempPath) == false)
                {
                    if (System.IO.Directory.Exists(tempPath) == true)
                    {
                        di = new DirectoryInfo(tempPath);

                        if (di.GetFiles().Length > 0)
                        {
                            sFileItem = new string[di.GetFiles().Length];

                            i = 0;
                            foreach (var Item in di.GetFiles())
                            {
                                sFileItem[i] = string.Format("{0}\\{1}", tempPath, Item.Name);
                                i++;
                            }

                            oRtn = new Stack<string[]>();
                            oRtn.Push(sFileItem);
                        }
                    }
                }
            }
            catch
            {
                oRtn = null;
            }

            return oRtn;
        }

        #endregion

        #region 액티브스퀘어 저장용-내용html만 추출
        public static string ConvertHtmlBlank(string strContents)
        {
            //대소문자 구분

            if (Regex.IsMatch(strContents, "<HTML>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<HTML>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<HEAD>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<HEAD>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<TITLE>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<TITLE>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "</TITLE>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "</TITLE>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<META content=\"text/html; charset=utf-8\" http-equiv=Content-Type>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<META content=\"text/html; charset=utf-8\" http-equiv=Content-Type>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<META content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\">", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<META content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\">", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<META content=IE=5 http-equiv=X-UA-Compatible>", RegexOptions.IgnoreCase) )
                strContents = Regex.Replace(strContents, "<META content=IE=5 http-equiv=X-UA-Compatible>", "", RegexOptions.IgnoreCase);

             

            if (Regex.IsMatch(strContents, "<META name=GENERATOR content=\"MSHTML", RegexOptions.IgnoreCase))
            {
                //<META name=GENERATOR content=\"MSHTML 11.00.9600.18427\">
                int iStart = -1;
                int iEnd = -1; 
                if (strContents.IndexOf("<META name=GENERATOR content=\"MSHTML 11") > -1)
                {
                    iStart = strContents.IndexOf("<META name=GENERATOR content=\"MSHTML");
                    iEnd = 55;
                }
                if (strContents.IndexOf("<meta name=GENERATOR content=\"MSHTML 11") > -1)
                {
                    iStart = strContents.IndexOf("<meta name=GENERATOR content=\"MSHTML");
                    iEnd = 55;
                }
                
                if (iStart > -1)
                {
                    string tmp = strContents.Substring(iStart, iEnd);
                    strContents = Regex.Replace(strContents, tmp, "", RegexOptions.IgnoreCase);
                }
            }

            if (Regex.IsMatch(strContents, "<META name=\"GENERATOR\" content=\"MSHTML", RegexOptions.IgnoreCase))
            {
                //<META name=GENERATOR content=\"MSHTML 9.00.8112.16684\">
                int iStart = -1;
                int iEnd = -1; 
                if (strContents.IndexOf("<META name=\"GENERATOR\" content=\"MSHTML 9") > -1)
                {
                    iStart = strContents.IndexOf("<META name=\"GENERATOR\" content=\"MSHTML");
                    iEnd = 56;
                }
                if (strContents.IndexOf("<meta name=\"GENERATOR\" content=\"MSHTML 9") > -1)
                {
                    iStart = strContents.IndexOf("<meta name=\"GENERATOR\" content=\"MSHTML");
                    iEnd = 56;
                }
                
                if (iStart > -1)
                {
                    string tmp = strContents.Substring(iStart, iEnd);
                    strContents = Regex.Replace(strContents, tmp, "", RegexOptions.IgnoreCase);
                }
            }

            if (Regex.IsMatch(strContents, "</HEAD>", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "</HEAD>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<BODY style=\"FONT-SIZE: 11pt; FONT-FAMILY: 맑은 고딕\">", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "<BODY style=\"FONT-SIZE: 11pt; FONT-FAMILY: 맑은 고딕\">", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<BODY style=\"FONT-FAMILY: 맑은 고딕; FONT-SIZE: 11pt\">", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "<BODY style=\"FONT-FAMILY: 맑은 고딕; FONT-SIZE: 11pt\">", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "<BODY style=\"FONT-FAMILY: 맑은 고딕; FONT-SIZE: 11pt;\">", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "<BODY style=\"FONT-FAMILY: 맑은 고딕; FONT-SIZE: 11pt;\">", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "</BODY>", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "</BODY>", "", RegexOptions.IgnoreCase);

            if (Regex.IsMatch(strContents, "</HTML>", RegexOptions.IgnoreCase))
                strContents = Regex.Replace(strContents, "</HTML>", "", RegexOptions.IgnoreCase);

            return strContents;

        }
        #endregion

        #region 액티브스퀘어 읽기용
        public static string ChangeCutSummaryBox(string Contents)
        {
            string strContent = Contents;

            Contents = AddAttribute(Contents, "a", "target=\"_blank\"");

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);

            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//a");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {
                    if (link.Attributes["href"] != null)
                    {
                        string hrefval = link.Attributes["href"].Value;
                        if (hrefval.IndexOf("#myLink") >= 0)
                        {
                            string s = CutSummary(link.InnerText, 20);
                            link.InnerHtml = s;
                        }
                    }
                }
                strContent = HtmlAgilityPackErrorHandling(htmlDoc.DocumentNode.OuterHtml);
            }
            return AddHtagLinkID(strContent);
        }

        //20140206  HtmlAgilityPack 태그 교정 문제로 생성 이후 관련 처리는 여기서 <하나 둘 셋 넷>
        protected static string HtmlAgilityPackErrorHandling(string content)
        {
            string strContent = string.Empty;
            if (content != null)
            {
                strContent = content.Replace("=\"\"", "");
            }
            return strContent;
        }

        protected static string AddHtagLinkID(string Contents)
        {
            string strContent = Contents;

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(Contents);

            HtmlAgilityPack.HtmlNodeCollection colllink = htmlDoc.DocumentNode.SelectNodes("//h2|//h3|//h4");

            if (colllink != null)
            {
                foreach (HtmlAgilityPack.HtmlNode link in colllink)
                {
                    if (link.Attributes["id"] == null)
                    {
                        //HtmlAgilityPack.HtmlAttribute pp = new HtmlAgilityPack.HtmlAttribute();
                        HtmlAgilityPack.HtmlAttribute addid = htmlDoc.CreateAttribute("id", Guid.NewGuid().ToString());
                        link.Attributes.Add(addid);
                    }
                }
                strContent = htmlDoc.DocumentNode.OuterHtml;
            }
            return strContent;
        }

        public static string CutSummary(string Summary, int maxlengh = 40)
        {
            if (Summary.Length > maxlengh)
            {
                return Summary.Substring(0, maxlengh - 3) + "...";
            }
            else
            {
                return Summary;
            }
        }

        //2015-09-16 ksh a태그에 blank 추가
        public static string AddAttribute(string source, string tagName, string attrib)
        {
            _attrib = attrib;
            string term = "<" + tagName + " [^>]+>";
            Regex r = new Regex(term, RegexOptions.IgnoreCase);
            MatchEvaluator myEvaluator = new MatchEvaluator(ProcessMatch);
            return r.Replace(source, myEvaluator);
        }

        public static string ProcessMatch(Match m)
        {
            string tag = m.Value;
            if (tag.IndexOf(_attrib) == -1)
            {
                tag = tag.Replace(">", " " + _attrib + ">");
            }
            return tag;
        }
        ////2015-09-16 ksh a태그에 blank 추가
        #endregion

        
    }
}