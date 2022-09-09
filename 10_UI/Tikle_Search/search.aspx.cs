using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using com.konantech.search.module.SearchModule;
using com.konantech.search.data.ResultVO;
using com.konantech.search.data.ParameterVO;
using com.konantech.search.util;
using System.Collections;
using SKT.Tnet.Framework.Utilities;
using SKT.Common;

public partial class main_search : System.Web.UI.Page
{
    public ParameterVO srchParam = new ParameterVO();

    protected void controlInitiate()
    {
        this.srchParam = common_setParameter.paramSetting();
    }

    protected void setParameter()
    {
        //common 파라미터 값 셋팅
        searchForm.srchParam = srchParam;

        //result 파라미터 값 셋팅
        result_idea.srchParam = srchParam;

        noResult.srchParam = srchParam;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo u = new UserInfo(this.Page);

        if (u.IsGlossaryPermission == false)
        {
            //권한 없음 경고 및 페이지 이동
            new PageHelper(this.Page).AlertMessage("해당 메뉴에 접근 권한이 없습니다.\nHome으로 이동합니다.\n관리자에게 문의하세요.", true, "/");
            Response.End();
        }

        this.common_setUserName.MyMenuUserName = "<b>" + u.Name + "</b><span>" + u.DeptName + "</span>";

        bool isAdminMenu = false;

        if (u.isAdmin || u.isManager || u.isTiklei)
        {
            isAdminMenu = true;
        }

        string tempuserid = u.UserID;

        controlInitiate();
        setParameter();

        String scn_all = ConfigurationManager.AppSettings["SCN_BRANDNET_REQUEST"];


        ////Total Search
        //if ("TOTAL".Equals(srchParam.getCategory()))
        //{
        //    //IDEA Search
        //    query_idea.excuteQuery(result_idea.rsbIdea, srchParam, scn_all, "idea");
        //}

        srchParam.UserID = u.UserID;
        srchParam.Category = "idea";
        srchParam.Date = "all";
        srchParam.SrchFd = "all";
        if(srchParam.PageNum == 0)
            srchParam.PageNum = 1;

        //개별검색
        if ("idea".Equals(srchParam.Category))
            query_idea.excuteQuery(result_idea.rsbIdea, srchParam, scn_all, "idea");

        //// Total Count
        srchParam.CountTotal = result_idea.rsbIdea.Total;

        
    }
}