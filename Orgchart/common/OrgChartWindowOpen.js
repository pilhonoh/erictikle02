
/**
* OrgChartDimmed.js
* Dimmed 처리후 iframe 에서 조직도를 호출하기 위함
* Copyright(c) Miksystem, Inc. (www.miksystem.com)
*/




// OrgChart URL
// !! 아래 주소는 조직도 호출시 필요한 주소입니다. 임의로수정하지 마세요
var g_orgchartWindowOpen_url = '/orgchart/orgmap.aspx?section=app';


//2013-12-06 부모창 포커스 최대한 막기위한 전역변수 추가
var popupWindow = null;

// OrgChart Data 전역변수
var g_orgLoadConfig = null;

// crossdomain
var g_newDomain = '';

var timeoutLoading;


var sizeDivW = 0;
var sizeDivH = 0;

// iframe 에 해당 조직도 띄우기
function om_OpenOrgChart(config) {
    if (!config && !config.callback) {
        alert('조직도 호출 설정이 잘못되었습니다.');

    }
    else {
        // 브라우저 체크하기
        var browserType = navigator.sayswho.toUpperCase();

        var d_height = 540;
        var d_width = 900; //540, 280

        if (browserType.indexOf("SAFARI") > -1 || browserType.indexOf("FIREFOX") > -1) {
            d_width = 785;
        } else if (browserType.indexOf("CHROME") > -1) {
            d_width = 795;
        }
        
         d_width = 900; //540, 280
         
//        // 익스플로러 브라우져이면 iframe으로 띄우기
//        if (navigator.userAgent.match(/msie /i)) {
//            g_orgchartWindowOpen_url = '/OrgChart/orgmapiframe.aspx?section=app';
//        }

        var sURL = g_orgchartWindowOpen_url;
        //edge:{ sunken | raised }, The default is raised
        //var sFeatures = "center:yes;dialogHide:yes;resizable:no;status:no;unadorned:yes;scroll:no;edge:sunken;";
        var vArguments = new Array();

        vArguments['CallBack'] = config.callback;
        vArguments['CustomApp'] = config.custom || null;

        var appType = config.appType || "deptuser";
        appType = appType.toUpperCase();
        var returnType = config.returnType || "json";
        returnType = returnType.toUpperCase();

        // 조직도 app 에서 회사 선택 여부.. winform에서 호출시 값 정해주지 않으면 기본 true로 처리하도록
        //var IsSelectCompany = true; // 기본값 false로 변경
        var IsSelectCompany = false;
        //if (config.selectCompany != null && config.selectCompany == false)
        ////IsSelectCompany = config.selectCompany === true ? true : false;    
        //    IsSelectCompany = false;        

        // 2013-11-27 전사 유저 또는 부서 검색이 가능하도록 수정 searchAll == true이면 검색시 자동 전체 검색
        if (config.selectCompany != null)
            IsSelectCompany = config.selectCompany === true ? true : false;
        vArguments['SelectCompany'] = IsSelectCompany;

        var IsSearchAll = false;
        if (config.searchAll != null) {
            IsSearchAll = config.searchAll === true ? true : false;
        }
        vArguments['SearchAll'] = IsSearchAll;

        var isChildSelectCompany = false;
        if (IsSelectCompany == false && config.childSelectCompany != null) {
            isChildSelectCompany = config.childSelectCompany === true ? true : false;
        }
        vArguments['ChildSelectCompany'] = isChildSelectCompany;

        var isOneSelect = config.oneSelect === true ? true : false;
		
        var showToolbar = false;
        if (config.showToolbar != null) {
            showToolbar = config.showToolbar === true ? true : false;
        }
	vArguments['showToolbar'] = showToolbar;

        if (config.title) vArguments['OrgMapTitle'] = config.title;
        vArguments['ReturnType'] = returnType;

        if (appType === "DEPTUSER") {
            if (isOneSelect) d_width = d_width - 240;
        }
        else if (appType === "USER") {
            if (isOneSelect) d_width = d_width - 240;
            else d_width = d_width - 60;
        }
        else if (appType === "DEPT") {
            if (isOneSelect == true) {
                if (isChildSelectCompany == true) d_width = d_width - 240;
                else { d_width = d_width - 460; }
            } else { d_width = d_width - 240; }
        }
        else if (appType === "GROUP") {
            d_width = d_width - 460;

        }
        else { }
        if (config.custom) {
            if (config.custom.height) {
                d_height = config.custom.height;
            }
            else {
                alert("Custom 조직도는 height를 지정해줘야 합니다."); return false;
            }
        }
        //스크린의 크기
//        var sw = screen.availWidth;
        var sw = (document.body.clientWidth / 2) - (d_width / 2); //브라우저 중앙위치

        var sh = screen.availHeight;

        //열 창의 포지션
//        var px = (sw - d_width) / 2;
        var px = sw + window.screenLeft  //현재브라우저 x위치
        var py = (sh - d_height) / 2;

//        var sFeatures = "height=" + d_height + "px,width=" + d_width + "px,resizable=no";
        var sFeatures = "height=" + d_height + "px,width=" + d_width + "px,resizable=yes" + ",left=" + px + ",top=" + py;

        vArguments['OnlyOneSelect'] = isOneSelect;
        vArguments['AppType'] = appType;

        sURL += "&AppType=" + appType;

        vArguments['SelectGroup'] = config.selectGroup == true ? true : false;

        var data = null;
        if (!isOneSelect) { data = config.data; }
        vArguments['OrgMapData'] = data;

        var selectDeptCode = "";
        selectDeptCode = (typeof (config.selectDeptCode) == "string" && config.selectDeptCode.length > 0) ? config.selectDeptCode : "";
        vArguments['SelectDeptCode'] = selectDeptCode;
        if (selectDeptCode != "")
            sURL += "&SelectDeptCode=" + selectDeptCode;

        var CompanyCodeForGroup = "";
        CompanyCodeForGroup = (typeof (config.CompanyCodeForGroup) == "string" && config.CompanyCodeForGroup.length > 0) ? config.CompanyCodeForGroup : "";
        vArguments['CompanyCodeForGroup'] = CompanyCodeForGroup;
        if (selectDeptCode != "")
            sURL += "&CompanyCodeForGroup=" + CompanyCodeForGroup;

        //if (config.langCode) {
            //sURL += "&langCode=" + config.langCode;
        //}
        
        // 하드코딩
		if ($.cookie) {
			var lc = $.cookie('MIK_LANGUAGE_CODE');
			if (lc === 'zh') lc = 'cn';
			sURL += '&langCode='+ lc;
		}


        try { sURL += "&ACCOUNT=" + g_OrgChart_UserID; } catch (e) { }

        // 크로스도메인
        if (g_newDomain != '') sURL += "&newDomain=" + encodeURI(g_newDomain);

        var date = new Date();
        sURL += "&_dc=" + date.getTime();
        config.Modal = config.Modal || false;

        g_orgLoadConfig = vArguments;

        // 2013-12-06 익스플로러는 모달로 그냥 띄워보기
        if (navigator.userAgent.match(/msie /i)) {
            sFeatures = "center:yes;dialogHide:yes;resizable:no;status:no;unadorned:yes;scroll:no;edge:sunken;";
            sFeatures += "dialogHeight:" + d_height + "px;dialogWidth:" + d_width + "px;";

            if (config.Modal == true) {
                window.showModalDialog(sURL, vArguments, sFeatures);
            }
            else {
                window.showModelessDialog(sURL, vArguments, sFeatures);
            }
        } // 2013-12-06 익스플로러 제외하고는 팝업으로 띄우기
        else {
            //alert("T@");
            //sURL += "&IsNewWindow=true";
            //popupWindow = window.open(sURL, "orgchart", sFeatures);
            if(popupWindow != null && !popupWindow.closed)
            {
                popupWindow.focus();
                return;
            }
			popupWindow = window.open(sURL, "_blank", sFeatures);
            popupWindow.focus();
//            document.onmousedown = focusPopup;
//            document.onkeyup = focusPopup;
//            document.onmousemove = focusPopup;
        }
    }
};

function focusPopup() {
    if (popupWindow && !popupWindow.closed)
        popupWindow.focus();
}

function CallBackFromNewWindow(returnVal) {
    alert(returnVal);
}
function SetDomain(newDomain) {

    g_newDomain = '';
    return;
}

navigator.sayswho = (function () {
    var ua = navigator.userAgent, tem,
    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];
        return 'IE ' + (tem[1] || '');
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
    return M.join(' ');
})();