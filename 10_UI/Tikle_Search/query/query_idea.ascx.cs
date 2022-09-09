using System;
using System.Configuration;
using System.Text;
using com.konantech.search.module.SearchModule;
using com.konantech.search.data.ParameterVO;
using com.konantech.search.data.ResultVO;
using System.Web;

public partial class query_idea : System.Web.UI.UserControl
{
    //SearchModule objDOCRUZER = new SearchModule();
    SearchModule objWISENUT = new SearchModule();           //wisenut

    public string search_method = ConfigurationManager.AppSettings["SEARCH_METHOD"];

    public void excuteQuery(ResultVO rsb, ParameterVO srchParam, String scn, String category)
    {
        if (srchParam.Kwd != null && srchParam.Kwd.Length > 0)
        {

            string query_wisenut = string.Empty;

            string srchFd = "text_idx";
            string dateFd = "";
            string orderBy = "";
            string orderNm = "";
            string logInfo = "";
            string highlightKWd = srchParam.IncludeKwd;
            StringBuilder query = new StringBuilder();

            int rc = 0;
            int pageNum = srchParam.PageNum;
            int pageSize = srchParam.PageSize;

            //wisenut, search field저장
            srchParam.WisenutSearchField = srchFd;

            // 재검색, wisenut
            if (srchParam.ReSrchFlag && srchParam.PreKwds != null)
            {
                String[] preKwds = srchParam.PreKwds;
                for (int cnt = 0; cnt < preKwds.Length; cnt++)
                {
                    query_wisenut += preKwds[cnt] + " ";
                }
            }
            query_wisenut += srchParam.Kwd;
            System.Diagnostics.Debug.WriteLine("==============================================");
            System.Diagnostics.Debug.WriteLine("query_wisenut:" + query_wisenut);
            query.Append(query_wisenut);



            //// 기본 쿼리
            //query = DCUtil.makeQuery(srchFd, srchParam.getKwd(), search_method, query, "AND");

            //// IDEA 검색
            //query = DCUtil.makeQuery("gubuncode", "I", "", query, "AND");

            ////로그 정보 설정

            //logInfo = DCUtil.getLogInfo(srchParam.getSite(), srchParam.getCategory(), srchParam.getNickName(), srchParam.getKwd(),
            //            srchParam.getPageNum(), srchParam.getReSrchFlag(), "", srchParam.getRecKwd());

            try
            {
                //rc = objDOCRUZER.dcSubmitQuery(scn, query.ToString(), logInfo, srchParam.getKwd() + " " + highlightKWd, orderBy, srchParam.getPageNum(), srchParam.getPageSize(), true, rsb, 1, 1, srchParam);
                rc = objWISENUT.dcSubmitQuery(scn, query.ToString(), logInfo, srchParam.Kwd + " " + highlightKWd, orderBy, srchParam.PageNum, srchParam.PageSize, true, rsb, 1, 1, srchParam);
            }
            catch (Exception ex)
            {
                Response.Write(new Exception("Unexpected Error : " + ex.Message));
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}