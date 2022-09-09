jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

$(document).ready(function () {
    var keywordInputId = "#akcKwd";
    var keyArrId = "";
    var listDivId = "#akc_div";
    var menuDivId = "";
    var akcAjaxUrl = "/Search/common/module/akc.aspx";
    var searchUrl = "/Search/search.aspx?kwd=";
    //var akcAjaxUrl = "./common/module/akc.aspx";
    //var searchUrl = "./Search.aspx?kwd=";
    var akc_switch = "#akc_switch";
    var akcCookie = getCookie("akc");

    var autoKeywordCompleteClass = new autoKeywordComplete(keywordInputId, listDivId, menuDivId, akcAjaxUrl, searchUrl, keyArrId, akc_switch, akcCookie);

    $('#akc_switch').click(function () {
        if (akcCookie == undefined) {
            akcCookie = getCookie("akc");
        }

        akcCookie = setAkcCookie(akcCookie);
        akc_switcClick(akcCookie);
    });
    $('#akc_switch').each(function () {
        if (akcCookie == undefined) {
            akcCookie = getCookie("akc");
        }
        akc_switcClick(akcCookie);
    });

    autoKeywordCompleteClass.menuBindEvent();
    autoKeywordCompleteClass.listBindEvent();
});



function konanSearch(url, kwd) {

    //document.charset = "utf-8";

    url += kwd;
    location.href(url);

    //document.charset = "euc-kr";
}

function akc_switcClick(akcCookie){

    akcCookie = getCookie("akc");

	if(akcCookie == null || akcCookie == 'on')
		$('#akc_text').html("기능끄기");
	else
		$('#akc_text').html("기능켜기");
}

function getCookie(name) {
    cookie = document.cookie;
    name = name + "=";
    idx = cookie.indexOf(name);
    if (cookie && idx >= 0) {

        tmp = cookie.substring(idx, cookie.length);
        deli = tmp.indexOf(";");
        if (deli > 0) {
            return tmp.substring(name.length, deli);
        } else {
            return tmp.substring(name.length);
        }
    }
}

function setCookie(name, value, expiredays) {
    var today = new Date();
    today.setDate(today.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + today.toGMTString() + ";";
}

function setAkcCookie(akcCookie){

    akcCookie = getCookie("akc");

    var domain = document.domain.substring(document.domain.indexOf("."));

    //setCookie("isUseACK", isUse, date, "/", domain);

    if (akcCookie == null || akcCookie == 'on' || akcCookie == 'akc=on')
        akcCookie = setCookie("akc", "off", 365, "/", domain);
	
	else
        akcCookie = setCookie("akc", "on", 365, "/", domain);

	if(akcCookie != null)
		akcCookie = akcCookie.replace("akc=","");

	return akcCookie;
}

//검색창 외 클릭 시 자동완성 창 닫힘 이벤트 처리
function layer_blur(e) {

	var listDivId = "#akc_div";

	if(!e && window.event) {
		e = window.event;
	}

	if(e) {
		clickX = e.clientX;
		clickY = e.clientY;
	}

	if(e.srcElement) {
		akc_evtsrcid = e.srcElement.id;
		akc_evtsrcname = e.srcElement.name;
	} else if(e.target) {
		akc_evtsrcid = e.target.id;
		akc_evtsrcname = e.target.name;
	}

	if(akc_evtsrcid != "akc_arrow" && akc_evtsrcid != "header-search-text"){
		$(listDivId).css({'display':'none'});
	}
 }

function autoKeywordComplete(keywordInputId, listDivId, menuDivId, akcAjaxUrl, searchUrl, keyArrId, akc_switch, akcCookie) {

	this.keywordInputId = keywordInputId;
	this.keyArrId = keyArrId;
	this.listDivId = listDivId;
	this.menuDivId = menuDivId;
	this.menuLeftDivId = this.menuDivId;
	this.menuRightDivId = this.menuDivId + ' .autocomplete';
	this.akcAjaxUrl = akcAjaxUrl;
	this.searchUrl = searchUrl;
	this.akc_switch = akc_switch;
	this._listIdx = -1;
	this._hookevent;

	//검색창 외 클릭 시 자동완성 창 닫힘 (이벤트 처리)
	document.onclick=layer_blur;


    /*
	if ($.browser.mozilla == true || $.browser.opera == true || $.browser.safari == true) {
		this._hookevent = new hookEvent($(keywordInputId)[0]);
	}
	else {
		$(keywordInputId).css('ime-mode', 'active');
	}
    */

	if ($.browser.mozilla == true || $.browser.opera == true || $.browser.safari == true) {
	    if (navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) {
	        $(keywordInputId).css('ime-mode', 'active');
	    } else {
	        //this._hookevent = new hookEvent($(keywordInputId)[0]);
	        $(keywordInputId).css('ime-mode', 'active');
	    }
	}
	else {
	    $(keywordInputId).css('ime-mode', 'active');
	}

	this.menuBindEvent = function(){
		var _this = this;

		$(_this.keywordInputId).keyup( function(e){
			_this.keyStroke(e);
		});

		$(_this.keywordInputId).click(function()
		{
			if ($(listDivId).css('display') == 'none') {
				if ($(keywordInputId).val().length > 0) {
					_this.getCompleteKeyword();
					_this.viewCompleteList();
				}else
					_this.hiddenCompleteList();
			}
			else {
				_this.hiddenCompleteList();
			}
		});

		$(keyArrId).click(function(){
			if ($(listDivId).css('display') == 'none') {
				_this.getCompleteKeyword();
				_this.viewCompleteList();
			}
			else {
				_this.hiddenCompleteList();
			}
		});
	}

	this.listBindEvent = function () {
	    var _this = this;

	    $(_this.listDivId + " UL > LI").mouseover(function () {
	        $(this).addClass('select');
	        _this._listIdx = jQuery(_this.listDivId + " UL > LI").index(this);
	    });

	    $(_this.listDivId + " UL > LI").mouseout(function () {
	        $(this).removeClass('select');
	    });

	    $(_this.listDivId + " UL > LI").click(function () {
	        _this.hiddenCompleteList();
	        $(_this.keywordInputId).val($(this).text());
	        $('.serch FORM').submit();
	    });
	}

	//auto complete
	this.keyStroke = function(e){
		var keyCode = e.keyCode;
/*
		if(keyCode==255) { //dummy
			return;
		}
*/
		this.checkKeyCode(keyCode);
	}

	//auto complete
	this.checkKeyCode = function(keyCode){

		if(keyCode==38) { //up
			if(this._listIdx==-1) {
				return;
			}
            $($(this.listDivId + ' ul li')[this._listIdx]).removeClass('akcKwdOn');
			this._listIdx--;
			if (this._listIdx >= 0) {
			    $($(this.listDivId + ' ul li')[this._listIdx]).addClass('akcKwdOn');
				$(this.keywordInputId).val($($(this.listDivId + ' ul li')[this._listIdx]).text());
				this.viewCompleteList();
			}
			else {
				this.hiddenCompleteList();
			}
			if($.browser.mozilla == true || $.browser.opera == true || $.browser.safari == true) {
				this._hookevent._value = this._hookevent._o.value;
			}
		}
		else if(keyCode==40) { //down
			if(this._listIdx==$(this.listDivId + ' ul li').length-1) {
				return;
			}
			if (this._listIdx>=-1) {
			    $($(this.listDivId + ' ul li')[this._listIdx]).removeClass('akcKwdOn');
			}
			this._listIdx++;
			$($(this.listDivId + ' ul li')[this._listIdx]).addClass('akcKwdOn');
			$(this.keywordInputId).val($.trim($($(this.listDivId + ' ul li')[this._listIdx]).text()));
			this.viewCompleteList();
			if($.browser.mozilla == true || $.browser.opera == true || $.browser.safari == true) {
				this._hookevent._value = this._hookevent._o.value;
			}
		}
		// back space 입력 AND TEXT 키워드 없을 시 자동완성 창 닫음
		else if(keyCode==8 && $(keywordInputId).val().length == 0) { //back space
			this.hiddenCompleteList();
		}
		else if(keyCode==8) { //back space
			this.getCompleteKeyword();
		}
		else if(keyCode==255 && ($.browser.opera==true || $.browser.mozilla==true)) { //firefox, safari
			this.viewCompleteList();
			this.getCompleteKeyword();
		}
		else if(keyCode!=255 && $.browser.msie==true) { //ie
			this.viewCompleteList();
			this.getCompleteKeyword();
		}else{
			this.viewCompleteList();
			this.getCompleteKeyword();
		}
	}

	//auto complete
	this.getCompleteKeyword = function () {
	    var _this = this;

	    if ($(keywordInputId).val() == "") {
	        _this.helpMessage();
	        return false;
	    }

	    if (getCookie("akc") != 'off') {
	        var now = new Date();
	        var opt = this.getOption();
	        var args = {
	            'q': ($(keywordInputId).val()),
	            's': opt,
	            'time': now.getTime()
	        };

	        $.ajax({
	            type: "GET",
	            async: true,
	            timeout: 10000,
	            dataType: "json",
	            url: akcAjaxUrl,
	            data: args,

	            success: function (data) {
	                var akcHtml_F = "";
	                var akcHtml_E = "";
	                var akcHtml_N = "";
	                var akcResult = "";

	                //alert('success');


	                $.each(data.LIST, function () {

	                    var highlight = "";
	                    var kwd = $(keywordInputId).val();
	                    var kwdLen = $(keywordInputId).val().length; //키워드 길이
	                    var fword = this["KEYWORD"].substring(0, kwdLen);
	                    var eword = this["KEYWORD"].substring(this["KEYWORD"].length - kwdLen, this["KEYWORD"].length);

	                    var akcKwd = this["KEYWORD"];

	                    if (fword == kwd) {
	                        highlight = fword.replace(fword, "<span style=\"color:#d25757;font-weight:bold;\">" + fword + "</span>") + akcKwd.substring(kwdLen, akcKwd.length);
	                        akcHtml_F += "<li class=\"akclist\">";
	                        akcHtml_F += "	<a href=\"javascript:konanSearch('" + _this.searchUrl +"','" + (akcKwd) + "')\">" + highlight + "</a>";
	                        akcHtml_F += "</li>";
	                    } else if (eword == kwd) {
	                        highlight = akcKwd.substring(0, akcKwd.length - kwdLen) + eword.replace(eword, "<span style=\"color:#d25757;font-weight:bold;\">" + eword + "</span>");
	                        akcHtml_E += "<li class=\"akclist\">";
	                        akcHtml_E += "	<a href=\"javascript:konanSearch('" + _this.searchUrl + "','" + (akcKwd) + "')\">" + highlight + "</a>";
	                        akcHtml_E += "</li>";
	                    } else {
	                        akcHtml_N += "<li class=\"akclist\">";
	                        akcHtml_N += "	<a href=\"javascript:konanSearch('" + _this.searchUrl + "','" + (akcKwd) + "')\">" + akcKwd + "</a>";
	                        akcHtml_N += "</li>";
	                    }
	                });


	                akcResult += akcHtml_F + akcHtml_N + akcHtml_E;

	                if (data.LIST.length == 0)
	                    _this.onHelpMessage();
	                else
	                    $(_this.listDivId + " ul.keyword_list").html(akcResult);
	            },
	            error: function (XMLHttpRequest, textStatus, errorThrown) {
	                //alert("XMLHttpRequest="+XMLHttpRequest.responseText+"\ntextStatus="+textStatus+"\nerrorThrown="+errorThrown);
	            }
	        });
	    } else {
	        _this.offHelpMessage();
	    }
	}

	this.getOption = function() {
		var opt = 0;

		if($(this.menuLeftDivId).hasClass('rear')) opt = 0;
		else if($(this.menuLeftDivId).hasClass('front'))opt = 1;
		else opt = 2

		return opt;
	}

	this.toggleOption = function() {
		if($(this.menuLeftDivId).hasClass('front')) {
			$(this.menuLeftDivId).html('<a href=\"\" title=\"왼쪽</a>');
			$(this.menuLeftDivId).addClass('rear');
			$(this.menuLeftDivId).removeClass('front');
		}
		else {
			$(this.menuLeftDivId).html('<a href=\"\" title=\"오른쪽</a>');
			$(this.menuLeftDivId).addClass('front');
			$(this.menuLeftDivId).removeClass('rear');
		}
		this.listBindEvent();
		this.getCompleteKeyword();
	}

	this.toggleOnOff = function() {
		if($(this.menuRightDivId).hasClass('on')) {
			this.toggleOn();
			this.listBindEvent();
			$(this.keywordInputId).keyup( function(e){ this.keyStroke(e); });
			this.getCompleteKeyword();
		}
		else {
			this.toggleOff();
		}
	}

	this.toggleOn = function() {
		if(!$(this.listDivId + " > ul > li").hasClass('akclist')) {
			this.onHelpMessage();
		}
	}

	this.toggleOff = function() {
		var _this = this;
		this.offHelpMessage();
	}

	//auto complete list view
	this.viewCompleteList = function(){
		var _this = this;
		$(this.listDivId).css({
			'display':'block'
		});

		_this.toggleOn();
	}

	//auto complete list hidden
	this.hiddenCompleteList = function(){
		$(this.listDivId).css({
			'display':'none'
		});
	}

	//show help message
	this.helpMessage = function(){

		if($.cookie("akc") == 'off') {
			this.offHelpMessage();
		}
		else {
			this.onHelpMessage();
		}
	}

	//set auto complete help message
	this.offHelpMessage = function(){
	    $(this.listDivId + " ul.keyword_list").html('<li style="padding-left:5px;">자동완성 기능을 사용하고 있지 않습니다.</li>');
	}

	//set auto complete help message
	this.onHelpMessage = function () {

	    if("" != $(this.keywordInputId).val())
	        $(this.listDivId + " ul.keyword_list").html('<li style="padding-left:5px;">일치하는 단어가 없습니다.</li>');
        else
            $(this.listDivId + " ul.keyword_list").html('<li style="padding-left:5px;">자동완성 기능을 사용하고 있습니다.</li>');
	}
}
