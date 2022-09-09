

var browserType = ""; // 브라우져 종류
var scrollTop;

// Modal Popup Open
var NoneIE_Guide_Open = {
    Window: function () {
        // noneie guide 팝업
        //쿠키에서 오늘 하루 안뜨게 체크했으면 안뜨게 함 
        //alert( document.cookie )
        strCookieDate = getCookie("noneie9pop");
        scrollTop = $(window).scrollTop();

        var todayDate = new Date();
        var dd = todayDate.getDate();    //날짜
        if (dd < 10) {
            dd = "0" + dd;
        }

        var mm = (todayDate.getMonth() + 1);
        if (mm < 10) {
            mm = "0" + mm;
        }
        var yy = todayDate.getFullYear();
        var fulldate = yy + "-" + mm + "-" + dd;

        //alert("오늘날짜:" + fulldate + "////쿠키날자:"+ strCookieDate);
        //alert(fulldate > strCookieDate);
        if (fulldate > strCookieDate || strCookieDate == undefined) {
            var url = "/NoneIEGuide_v1.html";
            ChangeIframeUrl(url);
            layer_open_ieGuide("popup_guide", false);
            $(".pop").show();
            $(".pop_layer_content2").attr("style", "display:block;z-index: 99999; top: 0px; margin-top: 30px; max-height: 661px;");
            //$(".pop_layer_content2").show();
        }
    }
}
// ifram Url 변경
function ChangeIframeUrl(url) {
   
    document.getElementById("ifmViewGuide").src = url;
   
}

// Close Modal Window
function ModalClose() {
   
    $('#mw_guide .pop_btn_close').click();
}

// Modal Resize
$(window).resize(function () {

    layer_shape('ifmViewGuide');
});

//layer popup 위치 및 창 크기 조절(기본)
function layer_shape(iframeID) {
    var $content = $(".pop_layer_content");
    var $iFrame = $('#' + iframeID);

    fn_SetLayerShape($content, $iFrame);
}

//layer popup 위치 및 창 크기 조절(공통)
function fn_SetLayerShape($content, $iFrame) {
    //창 높이 조절
    try {
        var $height = $iFrame.contents().find("body").height();

        if ($height) $(".pop_con_fix").height($height);
        else $(".pop_con_fix").height('600px');
    }
    catch (e) { $(".pop_con_fix").height('600px'); }

    //배경 조절
    $(".pop_bg").css("height", window.innerHeight + scrollTop);

    //창 최대높이 조절
    $content.css('top', scrollTop).css('margin-top', 30).css("max-height", window.innerHeight - 60);
    $(".pop_con_fix").css("max-height", window.innerHeight - 120);
    $iFrame.css("max-height", window.innerHeight - 120);
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function windowClose() {

    
    if (funBrowserCheck() == true) {

        var ie7 = navigator.userAgent.toLowerCase().indexOf('msie 7') != -1;
        var ie8 = navigator.userAgent.toLowerCase().indexOf('msie 8') != -1;
        var ie9 = navigator.userAgent.toLowerCase().indexOf('msie 9') != -1;


        if (ie7) {
            window.open('about:blank', '_self').close();
        } else if (ie8) {
            window.opener = "nothing";
            window.open('', '_parent', '');
            window.close();
        } else if (ie9) {
            window.opener = "nothing";
            window.open('', '_parent', '');
            window.close();
        } else {
            opener = self;
            opener.close();
        }
    }
    else {
	//alert(  navigator.userAgent + "---" + browserType.toLowerCase() ) ;
        if (browserType.toLowerCase() == "firefox") {
            window.open('about:blank', '_self', '').close();
        }
        else if (browserType.toLowerCase() == "chrome") { 

	    var locStr = location.href ; 
	    location.href = "http://"+ locStr.split("/")[2] ; 

            /*alert('크롬 창닫기') ; 
	    var win = window.open("", "_self" ) ; 
	    win.close() ; */


        }
        else if (browserType.toLowerCase() == "safari") {
            window.open('', '_self').close();
        }
        else {
            window.open('', '_self').close();
        }

    }
}


//브라우저 종류 및 버전확인  
function funBrowserCheck() {

    //alert('fnBrowserCheck');
    var browserName = navigator.appName;
    var browserAgent = navigator.userAgent;
    var browserPlatform = navigator.platform;


    //alert('navigator.userAgent :' + browserAgent);

    var inform = "Browser CodeName: " + navigator.appCodeName +"\n" ;
    inform += "/Browser Name: " + navigator.appName + "\n";
    inform += "/Browser Version: " + navigator.appVersion + "\n";
    inform += "/Cookies Enabled: " + navigator.cookieEnabled + "\n";
    inform += "/Browser Language: " + navigator.language + "\n";
    inform += "/Browser Online: " + navigator.onLine + "\n";
    inform += "/Platform: " + navigator.platform + "\n";
    inform += "User-agent header: " + navigator.userAgent + "\n";

    var browserVer = 0; // 브라우저  버전정보    
    
    // 브라우져 종류 설정.
    if (navigator.appVersion.indexOf('Trident/') > -1) {// Begin stupid IE crap
        var IE = navigator.appVersion;
        IE = IE.slice(IE.indexOf('Trident/') + 8);
        IE = IE.slice(0, IE.indexOf(';'));
        IE = Number(IE);
    }

    if ((browserName.charAt(0) == "M" && navigator.appVersion.indexOf("MSIE 9") != -1) || (browserName == 'Netscape' && IE == 7)) {
        browserType = "MSIE";
    } else if (browserName.charAt(0) == "N") {
        if (browserAgent.indexOf("Chrome") != -1) {
            browserType = "Chrome";
        } else if (browserAgent.indexOf("Firefox") != -1) {
            browserType = "Firefox";
        } else if (browserAgent.indexOf("Mobile Safari") != -1) {
            browserType = "Mobile Safari";
        } else if (browserAgent.indexOf("Safari") != -1) {
            browserType = "Safari";
        }
    } else {
        browserVer = "??";
    }

    //alert(browserType);
    // 브라우져 버젼 가져오기
    if (browserType != "") {
        browserVer = funGetInternetVersion(browserType);
       // alert('browserVer:' + browserVer); 
    }
    
    //IE9 & 32bit 일때만 
    //if (browserType == "MSIE" && browserVer == "9" && browserPlatform == "Win32") {
    if (browserType == "MSIE"){
        return true;
    }
    else {
        return false;
    }
}

/*브라우져 체크 2013-11-13 */
// 브라우져 버젼
function funGetInternetVersion(ver) {
    var rv = -1; // Return value assumes failure.      
    var ua = navigator.userAgent;
    var re = null;
    if (ver == "MSIE") {
        re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    } else {
        re = new RegExp(ver + "/([0-9]{1,}[\.0-9]{0,})");
    }
    if (re.exec(ua) != null) {
        rv = parseFloat(RegExp.$1);
    }
    return rv;
}
function setCookie(name, value, expiredays) {
    var todayDate = new Date();
    todayDate.setDate(todayDate.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
}

function DelCookie() {
    setCookie("noneie9pop", "2016-07-01", -1);
}

function layer_open_ieGuide(mw_name, IsRefresh) {

    var mw_id = $('#' + mw_name);
    var bg = mw_id.parents().find('.pop_bg');

    //현재 화면 Scroll 값 저장
    scrollTop = $(window).scrollTop();

    //팝업 시 속성 부여
    $("html").css("position", "fixed").css("width", "100%").css("top", -scrollTop);
    $(".pop_bg").css("height", window.innerHeight + scrollTop);


    //top 0 margin-top 30    //창 최대높이 조절
    mw_id.css("top", scrollTop).css("margin-top", 30).css("max-height", window.innerHeight - 120);

    // var ConnectionIP = myip_addr.substr(0, myip_addr.lastIndexOf('.'));
    // ConnectionIP = ConnectionIP.substr(0, ConnectionIP.lastIndexOf('.'));   

    $(".pop_con_fix").css("height", window.innerHeight - 850).css("max-height", window.innerHeight - 850);

    if (bg) {
        $('#mw_guide').show();
        mw_id.show();
    } else {
        mw_id.show();
    };

    mw_id.css('display', 'block');

    mw_id.find('.pop_btn_close').on("click", function (e) {
        //부여한 속성 제거
        $("html").css("position", "").css("width", "").css("top", "");
        $(window).scrollTop(scrollTop);

        // 현재 창에 LayerPopup Close 함수가 재정의 되었을 경우  함수가 있을 경우 해당 함수를 호출
        if (typeof fn_CloseLayerPopup == 'function') {
            fn_CloseLayerPopup();
        } else {

            //기본 Layer 창 닫기 기본 함수 
            //layer_default_Close(mw_name)
            layer_default_Close_ieGuide(mw_name);

            // 2015-06-09: 최진석top replace 함수 추가
            if (IsRefresh == "Y") parent.fn_ModalCloseRefresh();
        }

        e.preventDefault();
        mw_id.find('.pop_btn_close').off("click");
    });
}

// 2015-11-24 none IE가 아닐때는 안내문구 레이어 닫기
function layer_default_Close_ieGuide(mw_name) {

    var mw_id = $('#' + mw_name);
    var bg = mw_id.parents().find('.pop_bg');

    if (bg) {
        $('#mw_guide').hide();
        mw_id.hide();
    } else {
        mw_id.hide();
    }

    if ($("#ifmViewGuide").length > 0) {
        $("#ifmViewGuide").attr("src", 'about:blank');
    }

    //부여한 속성 제거
    $("html").css("position", "").css("width", "").css("top", "");
    $(window).scrollTop(scrollTop);


    $('.pop').hide();
    mw_id.find('.pop_btn_close').off("click");
}
// ***********************************************************
// ******************* none IE가 아닐때는 안내문구 띄우기 END ****************************************

