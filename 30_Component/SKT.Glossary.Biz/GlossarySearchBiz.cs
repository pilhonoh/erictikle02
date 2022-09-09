using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Common;
using SKT.Glossary.Type;

using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace SKT.Glossary.Biz
{
    public class GlossarySearchBiz
    {
        public static string GetSolrUrl(string SolrUrl, string SolrType, GlossarySearchType.SolrParams solrParams)
        {
            string result = SolrUrl + SolrType;
            result += "?q=" + HttpUtility.UrlEncode(solrParams.q);
            result += "&sort=" + HttpUtility.UrlEncode(solrParams.sort);
            result += "&start=" + HttpUtility.UrlEncode(solrParams.start);
            result += "&rows=" + HttpUtility.UrlEncode(solrParams.rows);
            result += "&wt=" + HttpUtility.UrlEncode(solrParams.wt);
            result += "&indent=" + HttpUtility.UrlEncode(solrParams.indent);
            result += "&hl=" + HttpUtility.UrlEncode(solrParams.hl);
            result += "&hl.fl=" + HttpUtility.UrlEncode(solrParams.hl_fl);
            result += "&hl.simple.pre=" + HttpUtility.UrlEncode(solrParams.hl_simple_pre);
            result += "&hl.simple.post=" + HttpUtility.UrlEncode(solrParams.hl_simple_post);

            return result;
        }

        public DataSet GetSearchGlossarySyncDataSelect(string BoardType, string CommonID)
        {
            GlossarySearchDac Dac = new GlossarySearchDac();
            DataSet ds = Dac.SearchGlossarySyncDataSelect(BoardType, CommonID);

            return ds;
        }

        public void SetSearchGlossarySyncDataUpdate(string BoardType, string CommonID)
        {
            string url = ConfigurationManager.AppSettings["SolrUrl"].ToString() + ConfigurationManager.AppSettings["SolrType_SearchGlossaryUpdate"].ToString();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "Content-type:application/json";
            request.Method = "POST";

            DataSet ds = GetSearchGlossarySyncDataSelect(BoardType, CommonID);

            GlossarySearchType.SearchGlossaryUpdate GlossaryUpdate = new GlossarySearchType.SearchGlossaryUpdate();
            GlossarySearchType.SearchGlossaryUpdateDetail GlossaryUpdateDetail = new GlossarySearchType.SearchGlossaryUpdateDetail();
            

            DataRow dr =  ds.Tables[0].Rows[0];
            GlossarySearchType.SearchGlossaryUpdateDocs GlossaryDocs = new GlossarySearchType.SearchGlossaryUpdateDocs();
            GlossaryDocs.CommentsHits = dr["CommentsHits"].ToString();
            GlossaryDocs.UserID = dr["UserID"].ToString();
            GlossaryDocs.UserName = dr["UserName"].ToString();
            GlossaryDocs.CreateDate = dr["CreateDate"].ToString();
            GlossaryDocs.Title = dr["Title"].ToString();
            GlossaryDocs.ModifyDate = dr["ModifyDate"].ToString();
            GlossaryDocs.CommonID = dr["CommonID"].ToString();
            GlossaryDocs.Hits = dr["Hits"].ToString();
            GlossaryDocs.BoardType = dr["BoardType"].ToString();
            GlossaryDocs.DeptName = dr["DeptName"].ToString();
            GlossaryDocs.TagTitle = dr["TagTitle"].ToString();
            GlossaryDocs.ID = dr["ID"].ToString();
            GlossaryDocs.Content = dr["Content"].ToString();
            //GlossaryDocs._version_ = null;
            //GlossaryDocs.score = null;

            GlossaryUpdateDetail.boost = 1;
            GlossaryUpdateDetail.overwrite = true;
            GlossaryUpdateDetail.commitWithin = 1000;

            GlossaryUpdateDetail.doc = GlossaryDocs;
            GlossaryUpdate.add = GlossaryUpdateDetail;
            /*
            GlossaryUpdate.boost = "1.0";
            GlossaryUpdate.overwrite = "true";
            GlossaryUpdate.commitWithin = "1000";
            */
            //string json = new JavaScriptSerializer().Serialize(GlossaryUpdate);

            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(GlossaryUpdate);

                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            
        }

        public DataSet GetSearchQnASyncDataSelect(string BoardType, string CommonID)
        {
            GlossarySearchDac Dac = new GlossarySearchDac();
            DataSet ds = Dac.SearchQnASyncDataSelect(BoardType, CommonID);

            return ds;
        }

        public void SetSearchQnASyncDataUpdate(string BoardType, string CommonID)
        {
            //string url = ConfigurationManager.AppSettings["SolrUrl"].ToString() + GlossarySearch.SolrType_SearchGlossary;
            string url = ConfigurationManager.AppSettings["SolrUrl"].ToString() + ConfigurationManager.AppSettings["SolrType_SearchQnAUpdate"].ToString();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "Content-type:application/json";
            request.Method = "POST";

            DataSet ds = GetSearchQnASyncDataSelect(BoardType, CommonID);

            GlossarySearchType.SearchGlossaryUpdate GlossaryUpdate = new GlossarySearchType.SearchGlossaryUpdate();
            GlossarySearchType.SearchGlossaryUpdateDetail GlossaryUpdateDetail = new GlossarySearchType.SearchGlossaryUpdateDetail();


            DataRow dr = ds.Tables[0].Rows[0];
            GlossarySearchType.SearchGlossaryUpdateDocs GlossaryDocs = new GlossarySearchType.SearchGlossaryUpdateDocs();
            GlossaryDocs.CommentsHits = dr["CommentsHits"].ToString();
            GlossaryDocs.UserID = dr["UserID"].ToString();
            GlossaryDocs.UserName = dr["UserName"].ToString();
            GlossaryDocs.CreateDate = dr["CreateDate"].ToString();
            GlossaryDocs.Title = dr["Title"].ToString();
            GlossaryDocs.ModifyDate = dr["ModifyDate"].ToString();
            GlossaryDocs.CommonID = dr["CommonID"].ToString();
            GlossaryDocs.Hits = dr["Hits"].ToString();
            GlossaryDocs.BoardType = dr["BoardType"].ToString();
            GlossaryDocs.DeptName = dr["DeptName"].ToString();
            GlossaryDocs.TagTitle = dr["TagTitle"].ToString();
            GlossaryDocs.ID = dr["ID"].ToString();
            GlossaryDocs.Content = dr["Content"].ToString();
            //GlossaryDocs._version_ = null;
            //GlossaryDocs.score = null;

            GlossaryUpdateDetail.boost = 1;
            GlossaryUpdateDetail.overwrite = true;
            GlossaryUpdateDetail.commitWithin = 1000;

            GlossaryUpdateDetail.doc = GlossaryDocs;
            GlossaryUpdate.add = GlossaryUpdateDetail;
            /*
            GlossaryUpdate.boost = "1.0";
            GlossaryUpdate.overwrite = "true";
            GlossaryUpdate.commitWithin = "1000";
            */
            //string json = new JavaScriptSerializer().Serialize(GlossaryUpdate);


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(GlossaryUpdate);

                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

        }
    }
}
