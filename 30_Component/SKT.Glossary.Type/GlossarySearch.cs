using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Web;

namespace SKT.Glossary.Type
{
    public class GlossarySearch
    {
        /*
        public static string SolrType_SearchGlossary = "/solr/core3/select";
        public static string SolrType_SearchPeople = "/solr/core1/select";
        public static string SolrType_SearchSchedule = "/solr/core2/select";
        public static string SolrType_SearchQnA = "/solr/core0/select";


        public static string SolrType_SearchGlossaryUpdate = "/solr/core3/update/json";
        public static string SolrType_SearchPeopleUpdate = "/solr/core1/update/json";
        public static string SolrType_SearchScheduleUpdate = "/solr/core2/update/json";
        public static string SolrType_SearchQnAUpdate = "/solr/core0/update/json";
        */

        public class SolrParams
        {
            public string sort { get; set; }
            public string indent { get; set; }
            public string start { get; set; }
            public string rows { get; set; }
            public string q { get; set; }
            public string hl { get; set; }
            public string hl_simple_pre { get; set; }
            public string hl_simple_post { get; set; }
            public string hl_fl { get; set; }
            public string wt { get; set; }
            /*
      "sort": "Hits Desc",
      "indent": "true",
      "start": "1",
      "q": "Title : \"법인카드\" OR Content : \"범인카드\"",
      "_": "1408259590114",
      "hl.simple.pre": "",
      "hl.simple.post": "",
      "hl.fl": "Title : \"법인카드\", Content : \"법인카드\"",
      "wt": "json",
      "hl": "true",
      "rows": "3"
            http://150.19.41.145:20000/solr/core0/select?
            q=Title+%3A+%22%EB%B2%95%EC%9D%B8%EC%B9%B4%EB%93%9C%22+OR+Content+%3A+%22%EB%B2%94%EC%9D%B8%EC%B9%B4%EB%93%9C%22
&sort=Hits+Desc
&start=1
&rows=3
&wt=json
&indent=true
&hl=true
&hl.fl=Title+%3A+%22%EB%B2%95%EC%9D%B8%EC%B9%B4%EB%93%9C%22%2C+Content+%3A+%22%EB%B2%95%EC%9D%B8%EC%B9%B4%EB%93%9C%22
&hl.simple.pre=%3Cb+class%3D%22point_orange%22%3E
&hl.simple.post=%3C%2Fb%3E
            */
        }

        public class SearchResponseHeader
        {
            public string numFound { get; set; }
        }


        #region SearchGlossary
        public class SearchGlossaryDocs
        {
            public string CommentsHits { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string CreateDate { get; set; }
            public string Title { get; set; }
            public string ModifyDate { get; set; }
            public string CommonID { get; set; }
            public string Hits { get; set; }
            public string BoardType { get; set; }
            public string DeptName { get; set; }
            public string TagTitle { get; set; }
            public string ID { get; set; }
            public string Content { get; set; }
            public string _version_ { get; set; }
            public string score { get; set; }
        }

        public class SearchGlossaryUpdateDocs
        {
            public string CommentsHits { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string CreateDate { get; set; }
            public string Title { get; set; }
            public string ModifyDate { get; set; }
            public string CommonID { get; set; }
            public string Hits { get; set; }
            public string BoardType { get; set; }
            public string DeptName { get; set; }
            public string TagTitle { get; set; }
            public string ID { get; set; }
            public string Content { get; set; }
        }

        public class SearchGlossaryHlightingItem
        {
            public List<string> Title { get; set; }
            public List<string> Content { get; set; }
        }

        public class SearchGlossaryUpdate
        {
            public SearchGlossaryUpdateDetail add { get; set; }
            //public string boost { get; set; }
            //public string overwrite { get; set; }
            //public string commitWithin { get; set; }
        }
        public class SearchGlossaryUpdateDetail
        {
            public int boost { get; set; }
            public bool overwrite { get; set; }
            public int commitWithin { get; set; }
            public SearchGlossaryUpdateDocs doc { get; set; }
        }

        public class SearchGlossaryDelete
        {
            public string delete { get; set; }
            public string boost { get; set; }
            public string overwrite { get; set; }
            public string commitWithin { get; set; }
        }
        #endregion 

        #region SearchQnA
        public class SearchQnADocs
        {
            public string CommentsHits { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string CreateDate { get; set; }
            public string Title { get; set; }
            public string ModifyDate { get; set; }
            public string CommonID { get; set; }
            public string Hits { get; set; }
            public string BoardType { get; set; }
            public string DeptName { get; set; }
            public string TagTitle { get; set; }
            public string ID { get; set; }
            public string Content { get; set; }
            public string _version_ { get; set; }
            public string score { get; set; }
        }

        public class SearchQnAUpdateDocs
        {
            public string CommentsHits { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string CreateDate { get; set; }
            public string Title { get; set; }
            public string ModifyDate { get; set; }
            public string CommonID { get; set; }
            public string Hits { get; set; }
            public string BoardType { get; set; }
            public string DeptName { get; set; }
            public string TagTitle { get; set; }
            public string ID { get; set; }
            public string Content { get; set; }
        }

        public class SearchQnAHlightingItem
        {
            public List<string> Title { get; set; }
            public List<string> Content { get; set; }
        }

        public class SearchQnAUpdate
        {
            public SearchQnAUpdateDetail add { get; set; }
            //public string boost { get; set; }
            //public string overwrite { get; set; }
            //public string commitWithin { get; set; }
        }
        public class SearchQnAUpdateDetail
        {
            public int boost { get; set; }
            public bool overwrite { get; set; }
            public int commitWithin { get; set; }
            public SearchQnAUpdateDocs doc { get; set; }
        }

        public class SearchQnADelete
        {
            public string delete { get; set; }
            public string boost { get; set; }
            public string overwrite { get; set; }
            public string commitWithin { get; set; }
        }
        #endregion

        #region SearchPeople
        public class SearchPeopleDocs
        {
            [JsonProperty(PropertyName = "_version_")]
            public string _version_ { get; set; }

            [JsonProperty(PropertyName = "EmployeeID")]
            public string EmployeeID { get; set; }

            [JsonProperty(PropertyName = "KoreanName")]
            public string KoreanName { get; set; }

            [JsonProperty(PropertyName = "EnglishName")]
            public string EnglishName { get; set; }

            [JsonProperty(PropertyName = "DisplayName")]
            public string DisplayName { get; set; }

            [JsonProperty(PropertyName = "Mail")]
            public string Mail { get; set; }

            [JsonProperty(PropertyName = "Mobile")]
            public string Mobile { get; set; }

            [JsonProperty(PropertyName = "TelephoneNumber")]
            public string TelephoneNumber { get; set; }

            [JsonProperty(PropertyName = "PhotoURL")]
            public string PhotoURL { get; set; }

            [JsonProperty(PropertyName = "Sosok")]
            public string Sosok { get; set; }

            [JsonProperty(PropertyName = "JobDescription01")]
            public string JobDescription01 { get; set; }

            [JsonProperty(PropertyName = "JobDescription02")]
            public string JobDescription02 { get; set; }

            [JsonProperty(PropertyName = "JobDescription03")]
            public string JobDescription03 { get; set; }

            [JsonProperty(PropertyName = "TaskNm")]
            public string TaskNm { get; set; }

            [JsonProperty(PropertyName = "TaskTx1")]
            public string TaskTx1 { get; set; }

            [JsonProperty(PropertyName = "TaskTx2")]
            public string TaskTx2 { get; set; }

            [JsonProperty(PropertyName = "POSITION_NAME")]
            public string POSITION_NAME { get; set; }

            [JsonProperty(PropertyName = "NOTICE_TITLE")]
            public string NOTICE_TITLE { get; set; }

            [JsonProperty(PropertyName = "NOTICE_CONTENTS")]
            public string NOTICE_CONTENTS { get; set; }

            [JsonProperty(PropertyName = "EFC_TITLE")]
            public string EFC_TITLE { get; set; }

            [JsonProperty(PropertyName = "ThisYearTaskTx1")]
            public string ThisYearTaskTx1 { get; set; }

            [JsonProperty(PropertyName = "ThisYearTaskTx2")]
            public string ThisYearTaskTx2 { get; set; }

            /*
                    "Mail":"smha@sk.com",
                    "EmployeeID":"1070155",
                    "PhotoURL":"http://150.2.2.95:1090/ContentServer/ContentServer.dll?get&pVersion=0046&contRep=Z1&docId=4DE2269E329E3C68E100000096020227&compId=data",
                    "KoreanName":"하성민",
                    "EnglishName":"Ha Sung Min",
                    "Sosok":"사장",
                    "TelephoneNumber":"",
                    "DisplayName":"하성민/사장",
                    "Mobile":"",
                    "_version_":1476191202554413056,
                    "score":1.0},
             */
        }

        public class SearchPeopleHlightingItem
        {
            [JsonProperty(PropertyName = "KoreanName")]
            public List<string> KoreanName { get; set; }

            [JsonProperty(PropertyName = "Sosok")]
            public List<string> Sosok { get; set; }

            [JsonProperty(PropertyName = "Mobile")]
            public List<string> Mobile { get; set; }

            [JsonProperty(PropertyName = "TelephoneNumber")]
            public List<string> TelephoneNumber { get; set; }

            [JsonProperty(PropertyName = "JobDescription01")]
            public List<string> JobDescription01 { get; set; }

            [JsonProperty(PropertyName = "JobDescription02")]
            public List<string> JobDescription02 { get; set; }

            [JsonProperty(PropertyName = "JobDescription03")]
            public List<string> JobDescription03 { get; set; }

            [JsonProperty(PropertyName = "TaskNm")]
            public List<string> TaskNm { get; set; }

            [JsonProperty(PropertyName = "TaskTx1")]
            public List<string> TaskTx1 { get; set; }

            [JsonProperty(PropertyName = "TaskTx2")]
            public List<string> TaskTx2 { get; set; }

            [JsonProperty(PropertyName = "NOTICE_TITLE")]
            public List<string> NOTICE_TITLE { get; set; }

            [JsonProperty(PropertyName = "NOTICE_CONTENTS")]
            public List<string> NOTICE_CONTENTS { get; set; }

            [JsonProperty(PropertyName = "EFC_TITLE")]
            public List<string> EFC_TITLE { get; set; }

            [JsonProperty(PropertyName = "ThisYearTaskTx1")]
            public List<string> ThisYearTaskTx1 { get; set; }

            [JsonProperty(PropertyName = "ThisYearTaskTx2")]
            public List<string> ThisYearTaskTx2 { get; set; }
        }
        #endregion

        #region SearchSchedule
        public class SearchScheduleDocs
        {
            [JsonProperty(PropertyName = "_version_")]
            public string _version_ { get; set; }

            [JsonProperty(PropertyName = "SCID")]
            public string SCID { get; set; }

            [JsonProperty(PropertyName = "YYMMDD")]
            public string YYMMDD { get; set; }

            [JsonProperty(PropertyName = "YEAR")]
            public string YEAR { get; set; }

            [JsonProperty(PropertyName = "MON")]
            public string MON { get; set; }

            [JsonProperty(PropertyName = "DAY")]
            public string DAY { get; set; }

            [JsonProperty(PropertyName = "Title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "Contents")]
            public string Contents { get; set; }

            [JsonProperty(PropertyName = "UserID")]
            public string UserID { get; set; }

            [JsonProperty(PropertyName = "UserName")]
            public string UserName { get; set; }


            /*
                    "Mail":"smha@sk.com",
                    "EmployeeID":"1070155",
                    "PhotoURL":"http://150.2.2.95:1090/ContentServer/ContentServer.dll?get&pVersion=0046&contRep=Z1&docId=4DE2269E329E3C68E100000096020227&compId=data",
                    "KoreanName":"하성민",
                    "EnglishName":"Ha Sung Min",
                    "Sosok":"사장",
                    "TelephoneNumber":"",
                    "DisplayName":"하성민/사장",
                    "Mobile":"",
                    "_version_":1476191202554413056,
                    "score":1.0},
             */
        }

        public class SearchScheduleHlightingItem
        {
            [JsonProperty(PropertyName = "_version_")]
            public List<string> _version_ { get; set; }

            [JsonProperty(PropertyName = "SCID")]
            public List<string> SCID { get; set; }

            [JsonProperty(PropertyName = "YYMMDD")]
            public List<string> YYMMDD { get; set; }

            [JsonProperty(PropertyName = "YEAR")]
            public List<string> YEAR { get; set; }

            [JsonProperty(PropertyName = "MON")]
            public List<string> MON { get; set; }

            [JsonProperty(PropertyName = "DAY")]
            public List<string> DAY { get; set; }

            [JsonProperty(PropertyName = "Title")]
            public List<string> Title { get; set; }

            [JsonProperty(PropertyName = "Contents")]
            public List<string> Contents { get; set; }

            [JsonProperty(PropertyName = "UserID")]
            public List<string> UserID { get; set; }

            [JsonProperty(PropertyName = "UserName")]
            public List<string> UserName { get; set; }
        }
        #endregion

    }
}
