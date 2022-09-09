using System;
using System.Web;
using System.Data;
using System.Text;
using System.Configuration;
using com.konantech.search.util;
using com.konantech.search.data.ParameterVO;

public partial class common_setParameter : System.Web.UI.UserControl
{

    public ParameterVO paramSetting()
    {
        ParameterVO srchParam = new ParameterVO();

        //Cookie 설정
        if (null != HttpContext.Current.Request.Cookies["SU"])
        {
            /* user id */
            srchParam.Uuid = HttpContext.Current.Request.Cookies["SU"]["cUUID"].ToString();

            /* user 필명 */
            srchParam.NickName = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["SU"]["cUSER_NICK_NAME"].ToString());

            /* 관리자 YN (Y : 관리자) */
            srchParam.Supex_code = HttpContext.Current.Request.Cookies["SU"]["cSUPEX_CODE"].ToString();
        }
        else {
            srchParam.Uuid = "";
            srchParam.NickName = "";
            srchParam.Supex_code = "";
        }

        /*사이트 이름*/
        srchParam.Site = "Tikle";

        /*키워드*/
        srchParam.Kwd = CommonUtil.null2Str(Request["kwd"], "");

        /*검색 카테고리*/
        srchParam.Category = CommonUtil.null2Str(Request["category"], "idea");

        /*재검색유무*/
        srchParam.ReSrchFlag = CommonUtil.null2Bool(Request["reSrchFlag"], false);

        /*이전키워드 배열목록*/
        if (Request.Params.GetValues("preKwds") != null)
        {
            srchParam.PreKwds = Request.Params.GetValues("preKwds");
        }

        /*페이지번호*/
        srchParam.PageNum = CommonUtil.null2Int(Request["pageNum"], 1);

        /*페이지크기*/
        if ("TOTAL".Equals(srchParam.Category))
        {
            srchParam.PageSize =Int32.Parse(ConfigurationManager.AppSettings["PAGESIZE_TOTAL"]);
        }
        else
        {
            srchParam.PageSize =Int32.Parse(ConfigurationManager.AppSettings["PAGESIZE"]);
        }

        //날짜 검색 - 기간
        srchParam.Date = CommonUtil.null2Str(Request["date"], "all");

        //날짜 검색 - 시작일
        srchParam.SDate = CommonUtil.null2Str(Request["sDate"], "");

        //날짜 검색 - 종료일
        srchParam.EDate = CommonUtil.null2Str(Request["eDate"], "");

        // 검색 대상 필드
        srchParam.SrchFd = CommonUtil.null2Str(Request["srchFd"], "all");

        // 정렬
        srchParam.Sort = CommonUtil.null2Str(Request["sort"], "");

        return srchParam;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
