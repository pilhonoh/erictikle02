/*******************************************************
* 프로그램명 : search.js   # 공통기능
* 설명       : 통합검색용  범용 코드 구현 js Class (CommonUtil)
* 작성일     : 2010.04.05
* 작성자     : 정민철
* 수정내역   :
*
* 2010.03.25 - 첨부파일미리보기 펑션수정
* 2010.03.24 - trim, replaceAll 추가
* 2010.03.23 - getValues 기능추가
* 2010.03.17 - getValue의 checkbox 리턴값 버그 수정
*****************************************************/

$(document).ready(function () {
    //callProcMyFeed();
    //callProcMSG();
    //callProcTop3();
});

function divShow(id) {
    $(id).removeClass('divFileInfoHidden');
}

function divHidden(id) {
    $(id).addClass('divFileInfoHidden');
}

function callProcMyFeed() {
    var targetUrl = "./common/module/proc.aspx";
    var obj = "#snb_feed";
    var uuid = $('#uuid').val();
    var args = {
        'uuid' : uuid,
        'part' : 'feed'
    };

    if ("" != uuid)
        ajaxCommon(targetUrl, obj, args);
}

function callProcMSG() {
    var targetUrl = "./common/module/proc.aspx";
    var obj = "#gnb_msg";
    var uuid = $('#uuid').val();
    var args = {
        'uuid': uuid,
        'part': 'msg'
    };

    if ("" != uuid)
        ajaxCommon(targetUrl, obj, args);
}

function callProcTop3() {
    var targetUrl = "./common/module/proc_top3.aspx";
    var obj = "#notice_board_con";
    
    ajaxCommonHtlm(targetUrl, obj);
}

function ajaxCommonHtlm(targetUrl, obj) {

    $.ajax({

        type: "GET",
        dataType: "html",
        url: targetUrl,
        success: function (data) {
            $(obj).html(data);
        },
        error: function (textStatus) {
            //alert(textStatus);
        }
    });
}

function ajaxCommon(targetUrl, obj, args) {

    $.ajax({

        type: "GET",
        dataType: "text",
        url: targetUrl,
        data: args,
        success: function (data) {
            if ("true" == $.trim(data))
                $(obj).html("<img src='images/bul_new.gif' alt='bul_new' />");

        },
        error: function (textStatus) {
            //alert(textStatus);
        }
    });
}

/**
* 특정 서브 카테고리로 이동 ( historyForm사용 )
* @ param	str			- 카테고리명
*
* @ return   void
**/

var CommonUtil = {

    /**
    * URL을 받아서 해당 결과를 String으로 리턴해줌
    * @ param   url		- 읽어올 페이지의 주소
    * 
    * @ return   str		-  url에서 보여지는 페이지 결과의 string
    *
    **/
    UtltoHtml: function (url) {
        var str = "";

        var xmlhttp = null;

        if (window.XMLHttpRequest) {
            // FF 로 객체선언
            xmlhttp = new XMLHttpRequest();
        } else {
            // IE 경우 객체선언
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        if (xmlhttp) {//비동기로 전송
            xmlhttp.open('GET', url, false);
            xmlhttp.send(null);
            str = xmlhttp.responseText;
        }
        return str;
    },

    /**
    * form 의 특정 name에 값을 세팅해줌 (라디오버튼, input,hidden, 셀렉트 박스) 알아서 처리해줌
    * @ param   frmobj		- 폼오브젝트
    * @ param	name			- 해당 데이터의 name
    * @ param	value			- 세팅될 값
    *
    * @ return   void
    * 
    * 주의사항
    * name이 복수개일경우 첫번째에 값을 세팅해줌
    **/
    setValue: function (frmobj, name, value) {
        if (typeof (frmobj) == "object" && typeof (frmobj.length) == "number");
        {
            for (var i = 0; i < frmobj.length; i++) {
                if (frmobj[i].name == name) {
                    if (frmobj[i].type == "text" || frmobj[i].type == "hidden") {// hidden , text
                        frmobj[i].value = value;
                        break;
                    } //--end: hidden, text
                    else if (frmobj[i].type == "radio" && frmobj[i].value == value) {// radio 버튼
                        frmobj[i].checked = true;
                        break;
                    } //--end:radio
                    else if (frmobj[i].type == "checkbox") {//checkbox박스
                        if (value == true)
                            frmobj[i].checked = true;
                        else
                            frmobj[i].checked = false;

                        break;
                    } //--end:checkbox
                    else if (frmobj[i].type == "select-one" && typeof (frmobj[i].options) == "object" && typeof (frmobj[i].length) == "number") {//select박스
                        var selectidx = 0;
                        for (var j = 0; j < frmobj[i].length; j++) {
                            if (value == frmobj[i].options[j].value) {
                                selectidx = j;
                                break;
                            }
                        }
                        frmobj[i].selectedIndex = selectidx;
                    } //--end:select

                }

            }
        }
    },

    /**
    * form 의 특정 name에 값을 가져옴 (라디오버튼, input,hidden, 셀렉트 박스 알아서 처리됨  )
    * @ param   frmobj		- 폼오브젝트
    * @ param	name			- 해당 데이터의 name
    *
    * @ return   해당 frmobj의 특정 name에 있는 값(value)
    * 
    * 주의사항
    * name이 복수개일경우 첫번째에 값을 리턴
    **/
    getValue: function (frmobj, name) {
        var result = null;

        if (typeof (frmobj) == "object" && typeof (frmobj.length) == "number");
        {
            for (var i = 0; i < frmobj.length; i++) {
                if (frmobj[i].name == name) {
                    if (frmobj[i].type == "text" || frmobj[i].type == "hidden") {// hidden , text
                        result = frmobj[i].value;
                        break;
                    } //--end: hidden, text
                    else if (frmobj[i].type == "radio" && frmobj[i].checked == true) {// radio 버튼
                        result = frmobj[i].value;
                        break;
                    } //--end:radio
                    else if (frmobj[i].type == "checkbox") {//checkbox박스
                        result = frmobj[i].checked;
                        break;
                    } //--end:checkbox
                    else if (frmobj[i].type == "select-one" && typeof (frmobj[i].options) == "object" && typeof (frmobj[i].length) == "number") {//select박스
                        var idx = frmobj[i].selectedIndex;
                        result = frmobj[idx].value;
                        break;
                    }
                }
            }
        }
        return result;
    },

    /**
    * form 의 특정 name에 값을 가져옴(라디오버튼, input,hidden, 셀렉트 박스 알아서 처리됨  )
    *
    * @ param   frmobj		- 폼오브젝트
    * @ param	name			- 해당 데이터의 name
    *
    * @ return   해당 frmobj의 특정 name에 있는 값(value)
    * 
    * 주의사항
    * name이 복수개일경우 공백(space)을 넣어 나열된 값을 리턴
    **/
    getValues: function (frmobj, name) {
        var result = "";

        if (typeof (frmobj) == "object" && typeof (frmobj.length) == "number");
        {
            for (var i = 0; i < frmobj.length; i++) {
                if (frmobj[i].name == name) {
                    if (frmobj[i].type == "text" || frmobj[i].type == "hidden") {// hidden , text
                        result += frmobj[i].value;
                    } //--end: hidden, text
                    else if (frmobj[i].type == "radio" && frmobj[i].checked == true) {// radio 버튼
                        result += frmobj[i].value;
                    } //--end:radio
                    else if (frmobj[i].type == "checkbox") {//checkbox박스
                        result += frmobj[i].checked;
                    } //--end:checkbox
                    else if (frmobj[i].type == "select-one" && typeof (frmobj[i].options) == "object" && typeof (frmobj[i].length) == "number") {//select박스
                        var idx = frmobj[i].selectedIndex;
                        result += frmobj[idx].value;
                    }

                    result += " ";
                }
            }
        }
        return result;
    },

    /**
    * YYYYMMDD를 DATE() 형으로 변환
    *
    * @ param   str			- YYYYMMDD형 스트링 형태의 날짜값
    *
    * @ return   			- Date() 형 날짜값
    **/
    string2date: function (str) {
        var year = "";
        var month = "";
        var day = "";

        if (typeof (str) == "string") {
            if (str.length < 8) {
                alert("[error - search.js] : string2date() 8자리 날짜가 아닙니다");
                return null;
            }
            year = parseInt(str.substring(0, 4));
            month = parseInt(str.substring(4, 6));
            day = parseInt(str.substring(6, 8));

            return Date(year, month - 1, day);

        }
    },

    /**
    * DATE() 형을 YYYYMMDD String형으로 리턴
    *
    * @ param   date			- Date()형 값
    *
    * @ return   			- "YYYYMMDD" string형 날짜데이터
    **/
    date2string: function (date) {
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;

        return year + "" + month + "" + day;
    },

    /**
    * 오늘 날짜 리턴 
    *
    * @ param   
    *
    * @ return   			- YYYYMMDD 오늘날짜
    **/
    getToday: function () {
        if (typeof (this.todaystr) == "undefined") {
            this.todaystr = this.date2string(new Date());

        }
        return this.todaystr;
    },

    /**
    * 날짜계산 (일단위)
    *
    * @ param   p_strdate		- YYYYMMDD 
    * @ param   p_agoday		- 0 : 오늘 ,    음수 : 과거 ,   양수: 미래       (일(Day)단위)
    *
    * @ return   			- YYYYMMDD 에서 p_agoday일 전후 
    **/
    calcDateDay: function (p_strdate, p_agoday) {
        var result = "";
        var year, month, day;
        var tmp_strdate = "" + p_strdate; //string형으로 변환

        if (typeof (tmp_strdate) == "string") {
            if (tmp_strdate.length == 8) {
                year = parseInt(tmp_strdate.substring(0, 4));
                month = parseInt(tmp_strdate.substring(4, 6));
                day = parseInt(tmp_strdate.substring(6, 8));

                result = new Date(year, month - 1, day + p_agoday);
            }
        }
        return this.date2string(result);
    },

    /**
    * 날짜계산 (주단위)
    *
    * @ param   p_strdate		- YYYYMMDD 
    * @ param   p_agoweek		- 0 : 오늘 ,    음수 : 과거 ,   양수: 미래       (주(Week)단위)
    *
    * @ return   			- YYYYMMDD 에서 p_agoweek 주 전후 
    **/
    calcDateWeek: function (p_strdate, p_agoweek) {
        var agoDay = p_agoweek * 7;

        return this.calcDateDay(p_strdate, agoDay);
    },

    /**
    * 날짜계산 (월단위)
    *
    * @ param   p_strdate		- YYYYMMDD 
    * @ param   agoMonth		- 0 : 오늘 ,    음수 : 과거 ,   양수: 미래       (월(Month)단위)
    *
    * @ return   			- YYYYMMDD 에서 agoMonth 월 전후 
    **/
    calcDateMonth: function (p_strdate, agoMonth) {
        var result = "";
        var year, month, day;
        var tmp_strdate = "" + p_strdate; //string형으로 변환

        if (typeof (tmp_strdate) == "string") {
            if (tmp_strdate.length == 8) {
                year = parseInt(tmp_strdate.substring(0, 4));
                month = parseInt(tmp_strdate.substring(4, 6));
                day = parseInt(tmp_strdate.substring(6, 8));

                result = new Date(year, month - 1 + agoMonth, day);
            }
        }
        return this.date2string(result);
    },

    /**
    * 날짜계산 (년단위)
    *
    * @ param   p_strdate		- YYYYMMDD 
    * @ param   agoYear		- 0 : 오늘 ,    음수 : 과거 ,   양수: 미래       (년(Year)단위)
    *
    * @ return   			- YYYYMMDD 에서 agoYear 년 전후 
    **/
    calcDateYear: function (p_strdate, agoYear) {
        var result = "";
        var agoMonth = (agoYear + 0) * 12
        var tmp_strdate = "" + p_strdate; //string형으로 변환

        result = this.calcDateMonth(p_strdate, agoMonth);

        return result;
    },

    /**
    * 문자열 치환
    *
    * @ param   target		- 원본 text
    * @ param   oldstr		- 변경 대상 string
    * @ param   newstr	- 변경될 string
    *
    * @ return   		- 치환된 text
    **/
    replaceAll: function (target, oldstr, newstr) {
        var result = target;
        if (target != null) {
            result = target.split(oldstr).join(newstr);
        }
        return result;
    },

    /**
    * white Space제거
    *
    * @ param   str		- 문자열
    *
    * @ return   		- 제거된 문자열
    **/
    trim: function (str) {
        var result = str;
        if (str != null) {
            result = result.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        }
        return result;
    }

}

/*+------------------------------------------------------------------------------------------------------+
    | 검색용 자바스크립트
    |
    | 만든이 : 정민철 (minchul.jeong@konantech.com)
    | --------------------------------------------------------------------------------------------------------
    | HISTORY   
    |  2010.03.25 - 첨부파일미리보기 펑션수정
    |  2010.03.24 - trim, replaceAll 추가
    |  2010.03.23 - getValues 기능추가
    |  2010.03.17 - getValue의 checkbox 리턴값 버그 수정
   +-------------------------------------------------------------------------------------------------------+*/


/*****************************************
* gnb폼 객체 리턴
******************************************/
function getGnbForm()
{
	return document.forms["searchForm"];
}

/**
* 특수 문자 체크
* @ param str -  검색어
*
* @ return   str - 유효여부 true, false
**/

function inputcheck(str) {
    for (var i = 0; i < str.length; i++) {
        ch_char = str.charAt(i);
        ch = ch_char.charCodeAt();
        if ((ch >= 33 && ch <= 47) || (ch >= 58 && ch <= 64) || (ch >= 91 && ch <= 96) || (ch >= 123 && ch <= 126)) {
            alert("특수문자를 사용할 수 없습니다");
            return false;
        }
    }
    return true;
}

/**
* 검색어 체크 
* @ param	frm			- form Object
*
* @ return   true / false 		- 키워드 있음(true) , 없음(false)
**/
function strip_tag(str) {
    return str.replace(/(<([^>]+)>)/ig, "");
}
function seachKwd() {
    
    var frm = getGnbForm();
    var SearchTextValue = "찾고싶은 모든 것을 검색해 보세요.";
    var kwd = CommonUtil.getValue(frm, "kwd");

    kwd = strip_tag(kwd.replace(/\'/gi, "").replace(/\"/gi, "").replace(/\\/gi, ""));
    kwd = strip_tag(kwd);

    //alert($("#search-input").val());
    if (kwd == SearchTextValue || kwd.replace(/^\s+|\s+$/g, '') == "") {
        alert(SearchTextValue);
        return false;
    }
    else {

        if (kwd.length < 2) {
            alert('검색어는 2글자 이상 입력해 주세요');
            return false;
        }
    }

    goKwd(kwd);

    //CommonUtil.setValue(frm, "pageNum", "1");
    //goKwd(kwd);
}

// 키워드 서브밋
function goKwd(str)
{
    var frm = getGnbForm();
	
	CommonUtil.setValue(frm,"kwd",str);
	CommonUtil.setValue(frm,"pageNum","1");
	CommonUtil.setValue(frm,"reSrchFlag",false);
	
	frm.submit();
}

function goKwdNew(str, sort) {

    var frm = getGnbForm();

    CommonUtil.setValue(frm, "kwd", str);
    CommonUtil.setValue(frm, "pageNum", "1");
    CommonUtil.setValue(frm, "reSrchFlag", false);
    CommonUtil.setValue(frm, "sort", sort);

    frm.submit();
}

//페이지이동
function gotoPage(str, kwd, sort) {

    var frm = getGnbForm();

    CommonUtil.setValue(frm, "sort", sort);
    CommonUtil.setValue(frm, "kwd", kwd);
	CommonUtil.setValue(frm,"pageNum",str);
	frm.submit();
}

//시작일 id, 종료일id, 범위 (1, 7, 365, 0
function choiceDatebutton(startname, endname, range) {
    var startDate = "";
    var endDate = "";

    var frm = getGnbForm();

    if (range == 1) {
        startDate = CommonUtil.getToday();
        endDate = CommonUtil.getToday();
    } 
    else if (range == 7) {
        startDate = CommonUtil.calcDateDay(CommonUtil.getToday(), -7);
        endDate = CommonUtil.getToday();
    }
    else if (range == 30) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -1);
        endDate = CommonUtil.getToday();
    }
    else if (range == 90) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -3);
        endDate = CommonUtil.getToday();
    }
    else if (range == 180) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -6);
        endDate = CommonUtil.getToday();
    }
    else if (range == 365) {
        startDate = CommonUtil.calcDateYear(CommonUtil.getToday(), -1);
        endDate = CommonUtil.getToday();
    }
    else if (range == 1095) {
        startDate = CommonUtil.calcDateYear(CommonUtil.getToday(), -3);
        endDate = CommonUtil.getToday();
    }

    CommonUtil.setValue(frm, startname, formatDateStr(startDate, "-"));
    CommonUtil.setValue(frm, endname, formatDateStr(endDate, "-"));
}


//시작일 id, 종료일id, 범위 (1, 7, 365, 0
function choiceDatebutton2(startname, endname, range) {
    var startDate = "";
    var endDate = "";

    var frm = getDetailForm();

    if (range == 1) {
        startDate = CommonUtil.calcDateDay(CommonUtil.getToday(), -1);
        endDate = CommonUtil.getToday();
    }
    else if (range == 7) {
        startDate = CommonUtil.calcDateDay(CommonUtil.getToday(), -7);
        endDate = CommonUtil.getToday();
    }
    else if (range == 30) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -1);
        endDate = CommonUtil.getToday();
    }
    else if (range == 90) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -3);
        endDate = CommonUtil.getToday();
    }
    else if (range == 180) {
        startDate = CommonUtil.calcDateMonth(CommonUtil.getToday(), -6);
        endDate = CommonUtil.getToday();
    }
    else if (range == 365) {
        startDate = CommonUtil.calcDateYear(CommonUtil.getToday(), -1);
        endDate = CommonUtil.getToday();
    }
    else if (range == 1095) {
        startDate = CommonUtil.calcDateYear(CommonUtil.getToday(), -3);
        endDate = CommonUtil.getToday();
    }
    frm.sDate.value = startDate;
    frm.eDate.value = endDate;
    //CommonUtil.setValue(frm, startname, formatDateStr(startDate, "-"));
    //CommonUtil.setValue(frm, endname, formatDateStr(endDate, "-"));
    return false;
}

//카테고리 변경
function goCategory(str) {
    var frm = getGnbForm();

    CommonUtil.setValue(frm, "pageNum", "1");
    CommonUtil.setValue(frm, "category", str);
    frm.submit();
}

//정렬
function goSort(str) {
    var frm = getGnbForm();

    CommonUtil.setValue(frm, "sort", str);
    CommonUtil.setValue(frm, "pageNum", "1");

    frm.submit();
}

function goSrchFd(str) {
    var frm = getGnbForm();
    var dtFrm = getDetailForm();

    CommonUtil.setValue(frm, "srchFd", str);
    CommonUtil.setValue(frm, "pageNum", "1");
 
    frm.submit();
}

function goDetailDate() {

    var frm = getGnbForm();
    var dtFrm = getDetailForm();
    var dflFrm = getDetailFormLeft();
    var startDate = dflFrm.sDate.value;
    var endDate = dflFrm.eDate.value;

    CommonUtil.setValue(frm, "pageNum", "1");
    CommonUtil.setValue(frm, "sDate", startDate);
    CommonUtil.setValue(frm, "eDate", endDate);

    //시작 일자가 종료 일자 보다 클 경우 종료 일자를 시작 일자에 대입
    if (CommonUtil.replaceAll(startDate, "-", "") > CommonUtil.replaceAll(endDate, "-", ""))
        startDate = endDate;

    if (isVaildDate(startDate) == false || isVaildDate(endDate) == false) {
        alert("날짜 입력 오류 입니다.\n다시 입력해 주세요");
        return false;
    }

    CommonUtil.setValue(frm, "pageNum", "1");
    CommonUtil.setValue(frm, "sDate", startDate);
    CommonUtil.setValue(frm, "eDate", endDate);
    CommonUtil.setValue(frm, "dds", "ON");

    var searchFlag = "DSF";
    if (searchFlag == "DSF") {
        CommonUtil.setValue(frm, "includeKwd", dtFrm.includeKwd.value);
        CommonUtil.setValue(frm, "exactKwd", dtFrm.exactKwd.value);
        CommonUtil.setValue(frm, "searchFlag", "DSF");
    }

    frm.submit();
}

//날짜 유효성 체크
function isVaildDate(date) {

    date = CommonUtil.replaceAll(date, "-", "");

    var yy = date.substring(0, 4);
    var mm = date.substring(4, 6);
    var dd = date.substring(6, 8);

    if (date.length != 8)
        return false;

    --mm;
    var dateVar = new Date(yy, mm, dd);
    //인수로 받은 년월일과 생성한 Date객체의 년월일이 일치하면 true
    return (dateVar.getFullYear() == yy && dateVar.getMonth() == mm && dateVar.getDate() == dd) ? true : false;
}

function goDate(str) {
    var frm = getGnbForm();
    var dtFrm = getDetailForm();
    var temp = "";

    if (str == "1")
        temp = "7";
    else if (str == "2")
        temp = "30";
    else if (str == "3")
        temp = "90";
    else if (str == "4")
        temp = "365";
    else if (str == "5")
        temp = "all";

    CommonUtil.setValue(frm, "date", temp);
    choiceDatebutton("sDate", "eDate", temp);

    var searchFlag = frm.searchFlag.value;
    if (searchFlag == "DSF") {
        CommonUtil.setValue(frm, "includeKwd", dtFrm.includeKwd.value);
        CommonUtil.setValue(frm, "exactKwd", dtFrm.exactKwd.value);
        CommonUtil.setValue(frm, "searchFlag", "DSF");
    }

    frm.submit();
}

// 달력 text 입력 값 숫자 체크, 달력 형식
function checkNum(obj) {

    var num = (obj).value.split("-").join("");
    var e = window.event;

    if (isNaN(num)) {
        alert("숫자만 입력하실 수 있습니다.");
        eval(obj).value = "";
        eval(obj).focus();
    }

    //날짜 형식에 맞게  " - " 추가 , back space 처리
    if ((obj.value.length == 4 || obj.value.length == 7) && e.keyCode != 8)
        eval(obj).value = obj.value + "-";
}

/*****************************************
* 자바스크립트 기능구현시 범용 코드 구현
*
* 만든이 : 정민철
* history : 2010.03.24
******************************************/	
	
var CommonUtil = {
	
	// URL을 받아서 해당 결과를 String으로 리턴해줌
	UtltoHtml : function (url) {
		var str = "";

		var xmlhttp = null;

		if(window.XMLHttpRequest) {
		   // FF 로 객체선언
		   xmlhttp = new XMLHttpRequest();
		} else {
		   // IE 경우 객체선언
		   xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
		}
		if ( xmlhttp ) 
		{//비동기로 전송
			xmlhttp.open('GET', url, false);
			xmlhttp.send(null);
			str = xmlhttp.responseText;
		}
		return str;
	},
	
	//form 의 특정 name에 값을 세팅해줌 (라디오버튼, input,hidden, 셀렉트 박스 알아서 처리해줌
	setValue : function (frmobj, name, value) {
		if ( typeof(frmobj) == "object" && typeof(frmobj.length) == "number");
		{
			for (var i=0; i< frmobj.length; i++)
			{
				if (frmobj[i].name == name)
				{
					if (frmobj[i].type=="text" || frmobj[i].type=="hidden" )
					{// hidden , text
						frmobj[i].value = value;
						break;
					}//--end: hidden, text
					else if (frmobj[i].type=="radio" && frmobj[i].value == value )
					{// radio 버튼
						 frmobj[i].checked = true;
						 break;
					}//--end:radio
					else if (frmobj[i].type=="checkbox")
					{//checkbox박스
						if (value == true)
							frmobj[i].checked = true;
						else
							frmobj[i].checked = false;
						
						break;
					}//--end:checkbox
					else if (frmobj[i].type=="select-one" && typeof(frmobj[i].options) == "object" && typeof(frmobj[i].length) == "number")
					{//select박스
						var selectidx = 0;
						for(var j=0; j<frmobj[i].length; j++)
						{
							if (value == frmobj[i].options[j].value)
							{
								selectidx = j;
								break;
							}
						}
						frmobj[i].selectedIndex = selectidx;
					}//--end:select
					
				}
				
			}
		}
	},
	
	getValue : function (frmobj, name)	{
		var result = null;

		if ( typeof(frmobj) == "object" && typeof(frmobj.length) == "number");
		{
			for (var i=0; i< frmobj.length; i++)
			{
				if (frmobj[i].name == name)
				{
					if (frmobj[i].type=="text" || frmobj[i].type=="hidden" )
					{// hidden , text
						result = frmobj[i].value;
						break;
					}//--end: hidden, text
					else if (frmobj[i].type=="radio" && frmobj[i].checked == true)
					{// radio 버튼
						 result = frmobj[i].value;
						 break;
					}//--end:radio
					else if (frmobj[i].type=="checkbox")
					{//checkbox박스
						result = frmobj[i].checked;
						break;
					}//--end:checkbox
					else if (frmobj[i].type=="select-one" && typeof(frmobj[i].options) == "object" && typeof(frmobj[i].length) == "number")
					{//select박스
						var idx = frmobj[i].selectedIndex;
						result = frmobj[idx].value;
						break;
					}
				}
			}
		}
		return result;
	},
	
	//같은 복수의 Name에 대한 값들을 공백으로 묶어서 리턴해줌
	getValues : function (frmobj, name)	{
		var result = "";

		if ( typeof(frmobj) == "object" && typeof(frmobj.length) == "number");
		{
			for (var i=0; i< frmobj.length; i++)
			{
				if (frmobj[i].name == name)
				{
					if (frmobj[i].type=="text" || frmobj[i].type=="hidden" )
					{// hidden , text
						result += frmobj[i].value;
					}//--end: hidden, text
					else if (frmobj[i].type=="radio" && frmobj[i].checked == true)
					{// radio 버튼
						 result += frmobj[i].value;
					}//--end:radio
					else if (frmobj[i].type=="checkbox")
					{//checkbox박스
						result += frmobj[i].checked;
					}//--end:checkbox
					else if (frmobj[i].type=="select-one" && typeof(frmobj[i].options) == "object" && typeof(frmobj[i].length) == "number")
					{//select박스
						var idx = frmobj[i].selectedIndex;
						result += frmobj[idx].value;
					}
					
					result += " ";
				}
			}
		}
		return result;
	},
	
	//YYYYMMDD를 DATE() 형으로 변환
	string2date : function (str)
	{
		var year = "";
        var month = "";
        var day = "";

        if (typeof (str) == "string") {
            if (str.length < 8)
			{
				alert("[error - search.js] : string2date() 8자리 날짜가 아닙니다");
                return null;
			}
            year = parseInt(str.substring(0, 4), 10);
            month = parseInt(str.substring(4, 6), 10);
            day = parseInt(str.substring(6, 8), 10);

            return Date(year, month - 1, day);

        }
	},
	
	//DATE()형을 YYYYMMDD 로 변경
	date2string : function (date)
	{
		var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;

        return year + "" + month + "" + day;
	},
	//오늘 날짜를 리턴받음 YYYYMMDD
	getToday : function () {
			if (typeof(this.todaystr) == "undefined")
			{
				this.todaystr = this.date2string(new Date());
				
			}
			return this.todaystr;
	},
	// 일계산 (YYYYMMDD, 1) -- 1일후 YYYYMMDD는?
	calcDateDay : function (p_strdate, p_agoday) {
		var result = "";
		var year,month,day;
		var tmp_strdate = ""+p_strdate;	//string형으로 변환
		
        if (typeof (tmp_strdate) == "string") {
            if (tmp_strdate.length == 8)
			{
			    year = parseInt(tmp_strdate.substring(0, 4), 10);
			    month = parseInt(tmp_strdate.substring(4, 6), 10);
			    day = parseInt(tmp_strdate.substring(6, 8), 10);
				
				result = new Date(year, month-1, day + p_agoday);
			}	
		}
		return this.date2string(result);
	},
	// 주계산  (YYYYMMDD , -2)  -- 2주전 YYYYMMDD는?
	calcDateWeek : function (p_strdate, p_agoweek) {
		var agoDay = p_agoweek * 7;
		
		return this.calcDateDay(p_strdate, agoDay );
	},
	//월계산 (YYYYMMDD , -1) -- 1달전 YYYYMMDD는?
	calcDateMonth : function (p_strdate, agoMonth) {
		var result = "";
		var year,month,day;
		var tmp_strdate = ""+p_strdate;	//string형으로 변환
		
        if (typeof (tmp_strdate) == "string") {
            if (tmp_strdate.length == 8)
			{
			    year = parseInt(tmp_strdate.substring(0, 4), 10);
			    month = parseInt(tmp_strdate.substring(4, 6), 10);
			    day = parseInt(tmp_strdate.substring(6, 8), 10);
				
				result = new Date(year, month-1+agoMonth, day);
			}	
		}
		return this.date2string(result);
	},
	//년계산 (YYYYMMDD , -1) -- 1년전 YYYYMMDD는?
	calcDateYear : function (p_strdate, agoYear) {
		var result = "";
		var agoMonth = (agoYear + 0) * 12
		var tmp_strdate = ""+p_strdate;	//string형으로 변환
		
        result = this.calcDateMonth(p_strdate,agoMonth);
		
		return result;
	},
	//자바스크립트에서 replace를 쓰면 앞쪽 하나만 바뀌는 문제가 있으므로 replaceAll구현
	replaceAll : function (target, oldstr, newstr)
	{
		var result = target;
		if (target != null)
		{
			result = target.split(oldstr).join(newstr);
		}
		return result;
	},
	trim : function (str)
	{
		var result = str;
		if (str != null)
		{
			result = result.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
		}
		return result;
	}

}

function reSearchCheck(){

    var f = document.forms["searchForm"];
    var reKwd = CommonUtil.getValues(f, "preKwds");
    
    f.kwd.value = "";
    f.kwd.focus();
    
    if (reKwd.length == 1){
        alert("이전 검색어가 없습니다.");
        f.reSrchFlag.checked = false;
        f.kwd.focus();
        return;
   }
}

/********************************************************
* 윈도우 오픈											*
 ********************************************************/
function popUpWindow(URL,nWidth,nHeight,WindowName)
{
	var windowOption = "toolbar=no,resizable=yes,location=no,menubar=no,scrollbars=yes,width="+nWidth+",height="+nHeight;
	window.open(URL,WindowName,windowOption);
}

function escapeString(str){

    str = str.replaceAll("\'","");
    str = str.replaceAll("\"","");
    str = str.replaceAll("<","");
    str = str.replaceAll(">","");
    
    return str;
}
