using System;
using System.Collections;
using System.Configuration;
using com.konantech.search.data.ResultVO;
using com.konantech.search.data.ParameterVO;
using QUERYAPI530Lib;
using System.Collections.Generic;

namespace com.konantech.search.module.SearchModule
{

    /// <summary>
    /// Summary description for SearchModule
    /// </summary>
    public class SearchModule
    {

        public SearchModule()
        {
            //
            // TODO: Add constructor logic here
        }

        public string SERVER_IP = ConfigurationManager.AppSettings["WISENUT_SERVER_IP"];
        public int SERVER_PORT = int.Parse(ConfigurationManager.AppSettings["WISENUT_SERVER_PORT"]);
        //public string SERVER_ADDRESS = ConfigurationManager.AppSettings["WISENUT_SERVER_IP_PORT"];

        public int SERVER_TIMEOUT = 10 * 1000;
        public int m_total = 0;

        public int dcSubmitQuery(String scn, String query, String logInfo, String hilightTxt,
                                String orderBy, int pageNum, int pageSize, Boolean flag, ResultVO dcb,
                                int nLanguage, int nCharset, ParameterVO srchParam)
        {
            int rc = 0;


            object total = 0;
            object rows = 0;
            object cols = 0;

            object rowIds = null;
            object scores = null;
            object tmp_fdata = null;
            object fdataLength = null;

            int i, j;

            int startNum = (pageNum - 1) * pageSize;

            int ret;
            int QUERY_LOG = 1;
            int EXTEND_OR = 0;
            int PAGE_START = (pageNum - 1) * pageSize;
            int RESULT_COUNT = pageSize;
            String SORT_FIELD = "DATE/DESC";
            string FILTER_QUERY = string.Empty;

            //전체
            //String SEARCH_FIELD = "sub_name,title,name,content";

            string START_DATE = string.Empty;
            string END_DATE = string.Empty;

            int USE_HIGHLIGHT = 1;
            int USE_SNIPPET = 1;

            int USE_LA = 1;
            int IGNORE_CASE = 1;
            int USER_ORIGINAL = 1;
            int USER_SYNONYM = 1;

            int AND_OPERATOR = 1;



            System.Diagnostics.Debug.WriteLine("dcSubmitQuery함수 > query:" + query);
            String COLLECTION = scn;
            String QUERY = query;
            //QUERY = "sk";


            wisenut.common.WNCollection wncol = new wisenut.common.WNCollection();

            //String SEARCH_FIELD = wisenut.common.WNCollection.COLLECTION[COLLECTION]["SEARCH_FIELD"];

            //String DOCUMENT_FIELD = wisenut.common.WNCollection.COLLECTION[COLLECTION]["DOCUMENT_FIELD"];

            String SEARCH_FIELD = wncol.COLLECTION[COLLECTION]["SEARCH_FIELD"];

            String DOCUMENT_FIELD = wncol.COLLECTION[COLLECTION]["DOCUMENT_FIELD"];

            System.Diagnostics.Debug.WriteLine("COLLECTION:" + COLLECTION + "<");

            if (!string.IsNullOrEmpty(srchParam.WisenutSearchField))
            {
                if (srchParam.WisenutSearchField != "text_idx")
                {
                    SEARCH_FIELD = srchParam.WisenutSearchField;
                }
            }
            System.Diagnostics.Debug.WriteLine("SEARCH_FIELD:" + SEARCH_FIELD + "<");
            System.Diagnostics.Debug.WriteLine("DOCUMENT_FIELD:" + DOCUMENT_FIELD + "<");


            if ("d".Equals(srchParam.Sort, StringComparison.OrdinalIgnoreCase))
            {
                SORT_FIELD = "DATE/DESC";
            }
            else if ("m".Equals(srchParam.Sort, StringComparison.OrdinalIgnoreCase))
            {
                SORT_FIELD = "menu_id/DESC";
            }
            else
            {    //정확도순
                SORT_FIELD = "RANK/DESC";
            }

            Search search = new Search(); // 검색 객체 선언

            try
            {
                search.w3SetTraceLog(0);

                // setting common
                ret = search.w3SetCodePage("UTF-8");

                //접속로그추가
                ret = search.w3SetSessionInfo(srchParam.UserID, "", "");

                ret = search.w3SetQueryLog(QUERY_LOG);
                ret = search.w3SetCommonQuery(QUERY, EXTEND_OR);

                // setting collection
                ret = search.w3AddCollection(COLLECTION);
                ret = search.w3SetPageInfo(COLLECTION, PAGE_START, RESULT_COUNT);
                ret = search.w3SetSortField(COLLECTION, SORT_FIELD);
                ret = search.w3SetSearchField(COLLECTION, SEARCH_FIELD);
                //ret = search.w3SetDocumentField(COLLECTION, DOCUMENT_FIELD);

                //가중치 부분
                search.w3SetRanking(COLLECTION, "custom", "dweight=0.4;fweight=0.1;rweight=0.2;", 0); //최신순 0.4, 정확도 0.1, 가중치 0.2s

                search.w3AddSearchFieldScore(COLLECTION, "Title", 50);
                search.w3AddSearchFieldScore(COLLECTION, "Content", 40);
                search.w3AddSearchFieldScore(COLLECTION, "FileName", 10);
                search.w3AddSearchFieldScore(COLLECTION, "file_content", 10);

                String[] documentFields = DOCUMENT_FIELD.Split(',');
                foreach (string field in documentFields)
                {
                    // 요약된 결과 제공
                    // 해당 필드에 100글자의 요약을 제공하는 메소드
                    if (field.Equals("content"))
                        search.w3AddDocumentField(COLLECTION, field, 100);
                    else
                        search.w3AddDocumentField(COLLECTION, field, 0);
                }


                ret = search.w3SetHighlight(COLLECTION, USE_HIGHLIGHT, USE_SNIPPET);        //하이라이팅
                ret = search.w3SetQueryAnalyzer(COLLECTION, USE_LA, IGNORE_CASE, USER_ORIGINAL, USER_SYNONYM);


                //if (!string.IsNullOrEmpty(srchParam.getStartDate()) && !string.IsNullOrEmpty(srchParam.getEndDate()))
                //{
                //    START_DATE = DateTime.ParseExact(srchParam.getStartDate(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy'/'MM'/'dd");
                //    END_DATE = DateTime.ParseExact(srchParam.getEndDate(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy'/'MM'/'dd");
                //    ret = search.w3SetDateRange(COLLECTION, START_DATE, END_DATE);
                //}


                //ret = search.w3SetPrefixQuery(COLLECTION, PREFIX_CONDITION, AND_OPERATOR);

                if (!string.IsNullOrEmpty(srchParam.WisenutFilterQuery))
                {

                    FILTER_QUERY = srchParam.WisenutFilterQuery;
                    System.Diagnostics.Debug.WriteLine("filterQuery:" + FILTER_QUERY + "<");

                    //fastaccess=y가 선언되어있어야함.

                    ret = search.w3SetFilterQuery(COLLECTION, FILTER_QUERY);
                }

                // request
                ret = search.w3ConnectServer(SERVER_IP, SERVER_PORT, SERVER_TIMEOUT);
                ret = search.w3ReceiveSearchQueryResult(3);

               

                // check error
                if (search.w3GetError() != 0)
                {
                    //Response.Write("<b>" + search.w3GetErrorInfo() + "</b>");
                    //return;
                    throw new Exception(search.w3GetErrorInfo());
                }

                // get result information
                int totalCount = search.w3GetResultTotalCount(COLLECTION);
                int resultCount = search.w3GetResultCount(COLLECTION);

                List<Dictionary<String, String>> result = new List<Dictionary<String, String>>();
                for (int idx = 0; idx < search.w3GetResultCount(COLLECTION); idx++)
                {
                    Dictionary<String, String> row = new Dictionary<String, String>();
                    string[] str_split = DOCUMENT_FIELD.Split(new char[] { ',' });
                    //string[] str_split = DOCUMENT_FIELD.Split(',');

                    foreach (string field in str_split)
                    {
                        row.Add(field, search.w3GetField(COLLECTION, field, idx));
                    }

                    result.Add(row);
                }

                string[] DOCUMENT_FIELD_split = DOCUMENT_FIELD.Split(new char[] { ',' });
                object[,] fdata = new object[resultCount, DOCUMENT_FIELD_split.Length];
                for (i = 0; i < search.w3GetResultCount(COLLECTION); i++)
                {
                    for (j = 0; j < DOCUMENT_FIELD_split.Length; j++)
                    {
                        //fdata[i, j] = search.w3GetField(COLLECTION, DOCUMENT_FIELD_split[j], i);
                        //하이라이팅 추가
                        string data = search.w3GetField(COLLECTION, DOCUMENT_FIELD_split[j], i);
                        //data = data.Replace("<!HS>", "<b>");
                        //data = data.Replace("<!HE>", "</b>");

                        data = data.Replace("<!HS>", "<span style=\"color:#d25757;font-weight:bold;\">");
                        data = data.Replace("<!HE>", "</span>");
                        data = data.Replace("&nbsp;", "");
                        data = data.Replace("\r\n", "");
                        fdata[i, j] = data;

                    }
                }

                total = totalCount;
                rows = resultCount;
                cols = DOCUMENT_FIELD_split.Length;



                dcb.Total = (int)total;
                dcb.Rows = (int)rows;
                dcb.Cols = (int)cols;
                dcb.RowIds = (object[])rowIds;
                dcb.Scores = (object[])scores;
                dcb.Fdata = fdata;
                m_total = (int)total;
                search.w3CloseServer();
            }
            catch (Exception e)
            {
                throw (new Exception("Unexpected Error : " + e.Message));
            }
            finally
            {
                search.w3CloseServer();
            }

            return rc;
        }




    }
}
