using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SKT.Glossary.Type;
using SKT.Glossary.Biz;

using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml;
using System.IO;

namespace SKT.Glossary.Web.Common.Controls
{
    public class CommonSearch
    {
        public static string GetGlossarySelect(string sSearchKeyword, string sSearchSort, int iCurrPage, int iPerPage)
        {
            GlossarySearchType.SolrParams solrParams = new GlossarySearchType.SolrParams();
            solrParams.sort = sSearchSort + " desc";
            solrParams.indent = "true";
            solrParams.start = Convert.ToString(((iCurrPage - 1) * iPerPage));
            solrParams.rows = iPerPage.ToString();

            solrParams.q = "Title : \"" + sSearchKeyword + "\" ~1000"
            + " OR Content : \"" + sSearchKeyword + "\" ~1000"
            + " OR TagTitle : \"" + sSearchKeyword + "\" ~1000";

           // solrParams.q = "Title : \"" + sSearchKeyword + "\""
           //+ " OR Content : \"" + sSearchKeyword + "\""
           //+ " OR TagTitle : \"" + sSearchKeyword + "\"";

            solrParams.hl = "true";
            solrParams.hl_simple_pre = "<b class=\"point_blue\">";
            solrParams.hl_simple_post = "</b>";
            solrParams.hl_fl = "Title : \"" + sSearchKeyword + "\", Content : \"" + sSearchKeyword + "\"";
            solrParams.wt = "json";

            string uri = GlossarySearchBiz.GetSolrUrl(ConfigurationManager.AppSettings["SolrUrl"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchGlossary"].ToString(), solrParams);
            WebClient wc = new WebClient();
            var stm = wc.OpenRead(uri);
            string responseJSON = new StreamReader(stm).ReadToEnd();
            return responseJSON;
  
        }

        #region HttpPost
        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="URI"></param>
        /// <param name="solrParams"></param>
        /// <returns></returns>
        /// string URI = ConfigurationManager.AppSettings["SolrUrl"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchGlossary"].ToString();
        /// string URI = ConfigurationManager.AppSettings["SolrUrl"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchPeople"].ToString();
        /// string URI = ConfigurationManager.AppSettings["SolrUrl"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchQnA"].ToString();
        /// return HttpPost(URI, solrParams);
        public static string HttpPost(string URI, GlossarySearchType.SolrParams solrParams)
        {
            string Parameters = string.Empty;
            Parameters += "q=" + HttpUtility.UrlEncode(solrParams.q);
            Parameters += "&sort=" + HttpUtility.UrlEncode(solrParams.sort);
            Parameters += "&start=" + HttpUtility.UrlEncode(solrParams.start);
            Parameters += "&rows=" + HttpUtility.UrlEncode(solrParams.rows);
            Parameters += "&wt=" + HttpUtility.UrlEncode(solrParams.wt);
            Parameters += "&indent=" + HttpUtility.UrlEncode(solrParams.indent);
            Parameters += "&hl=" + HttpUtility.UrlEncode(solrParams.hl);
            Parameters += "&hl.fl=" + HttpUtility.UrlEncode(solrParams.hl_fl);
            Parameters += "&hl.simple.pre=" + HttpUtility.UrlEncode(solrParams.hl_simple_pre);
            Parameters += "&hl.simple.post=" + HttpUtility.UrlEncode(solrParams.hl_simple_post);

            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
        
        #endregion

        public static string GetPeopleSelect(string sSearchKeyword, string sSearchSort, int iCurrPage, int iPerPage)
        {
            GlossarySearchType.SolrParams solrParams = new GlossarySearchType.SolrParams();
            if (sSearchSort == "KoreanName")
            {
                solrParams.sort = sSearchSort + " asc";
            }
            else
            {
                solrParams.sort = sSearchSort + " desc";
            }
            solrParams.indent = "true";
            solrParams.start = Convert.ToString(((iCurrPage - 1) * iPerPage));
            solrParams.rows = iPerPage.ToString();
            solrParams.q = "KoreanName : \"" + sSearchKeyword + "\""
                + " OR Sosok : \"" + sSearchKeyword + "\""
                + " OR Mobile : \"" + sSearchKeyword + "\""
                + " OR TelephoneNumber : \"" + sSearchKeyword + "\""
                + " OR JobDescription01 : \"" + sSearchKeyword + "\""
                + " OR JobDescription02 : \"" + sSearchKeyword + "\""
                + " OR JobDescription03 : \"" + sSearchKeyword + "\""
                + " OR TaskNm : \"" + sSearchKeyword + "\""
                + " OR TaskTx1 : \"" + sSearchKeyword + "\""
                + " OR TaskTx2 : \"" + sSearchKeyword + "\""
                + " OR ThisYearTaskTx1 : \"" + sSearchKeyword + "\""
                + " OR ThisYearTaskTx2 : \"" + sSearchKeyword + "\""
                + " OR NOTICE_TITLE : \"" + sSearchKeyword + "\""
                + " OR NOTICE_CONTENTS : \"" + sSearchKeyword + "\""
                + " OR EFC_TITLE : \"" + sSearchKeyword + "\"";
            solrParams.hl = "true";
            solrParams.hl_simple_pre = "<b class=\"point_blue\">";
            solrParams.hl_simple_post = "</b>";
            solrParams.hl_fl = "KoreanName : \"" + sSearchKeyword + "\""
                + ", Sosok : \"" + sSearchKeyword + "\""
                + ", Mobile : \"" + sSearchKeyword + "\""
                + ", TelephoneNumber : \"" + sSearchKeyword + "\""
                + ", JobDescription01 : \"" + sSearchKeyword + "\""
                + ", JobDescription02 : \"" + sSearchKeyword + "\""
                + ", JobDescription03 : \"" + sSearchKeyword + "\""
                + ", TaskNm : \"" + sSearchKeyword + "\""
                + ", TaskTx1 : \"" + sSearchKeyword + "\""
                + ", TaskTx2 : \"" + sSearchKeyword + "\""
                + ", ThisYearTaskTx1 : \"" + sSearchKeyword + "\""
                + ", ThisYearTaskTx2 : \"" + sSearchKeyword + "\""
                + ", NOTICE_TITLE : \"" + sSearchKeyword + "\""
                + ", NOTICE_CONTENTS : \"" + sSearchKeyword + "\""
                + ", EFC_TITLE : \"" + sSearchKeyword + "\"";
            solrParams.wt = "json";

            string uri = GlossarySearchBiz.GetSolrUrl(ConfigurationManager.AppSettings["SolrUrl"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchPeople"].ToString(), solrParams);
            WebClient wc = new WebClient();
            var stm = wc.OpenRead(uri);
            string responseJSON = new StreamReader(stm).ReadToEnd();
            return responseJSON;

        }

        public static string GetQnASelect(string sSearchKeyword, string sSearchSort, int iCurrPage, int iPerPage)
        {
            GlossarySearchType.SolrParams solrParams = new GlossarySearchType.SolrParams();
            solrParams.sort = sSearchSort + " desc";
            solrParams.indent = "true";
            solrParams.start = Convert.ToString(((iCurrPage - 1) * iPerPage));
            solrParams.rows = iPerPage.ToString();
            solrParams.q = "Title : \"" + sSearchKeyword + "\""
            + " OR Content : \"" + sSearchKeyword + "\""
            + " OR TagTitle : \"" + sSearchKeyword + "\"";
            solrParams.hl = "true";
            solrParams.hl_simple_pre = "<b class=\"point_blue\">";
            solrParams.hl_simple_post = "</b>";
            solrParams.hl_fl = "Title : \"" + sSearchKeyword + "\", Content : \"" + sSearchKeyword + "\"";
            solrParams.wt = "json";

            string uri = GlossarySearchBiz.GetSolrUrl(ConfigurationManager.AppSettings["SolrUrl"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchQnA"].ToString(), solrParams);
            WebClient wc = new WebClient();
            var stm = wc.OpenRead(uri);
            string responseJSON = new StreamReader(stm).ReadToEnd();
            return responseJSON;

            //string result = string.Empty;
            //result += "q=" + HttpUtility.UrlEncode(solrParams.q);
            //result += "&sort=" + HttpUtility.UrlEncode(solrParams.sort);
            //result += "&start=" + HttpUtility.UrlEncode(solrParams.start);
            //result += "&rows=" + HttpUtility.UrlEncode(solrParams.rows);
            //result += "&wt=" + HttpUtility.UrlEncode(solrParams.wt);
            //result += "&indent=" + HttpUtility.UrlEncode(solrParams.indent);
            //result += "&hl=" + HttpUtility.UrlEncode(solrParams.hl);
            //result += "&hl.fl=" + HttpUtility.UrlEncode(solrParams.hl_fl);
            //result += "&hl.simple.pre=" + HttpUtility.UrlEncode(solrParams.hl_simple_pre);
            //result += "&hl.simple.post=" + HttpUtility.UrlEncode(solrParams.hl_simple_post);

            //string URI = ConfigurationManager.AppSettings["SolrUrl"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SolrType_SearchQnA"].ToString();
            //return HttpPost(URI, solrParams);
        }
    }
}