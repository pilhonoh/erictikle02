<%@ Control Language="C#" AutoEventWireup="true" CodeFile="searchForm.ascx.cs" Inherits="common_search" %>
<%
    string kwd = srchParam.Kwd;

    //if ("".Equals(kwd.Trim()))
    //    kwd = "검색어를 입력하세요.";
%>

<script type="text/javascript">
 var SearchTextValue = "찾고싶은 모든 것을 검색해 보세요.";
$(document).ready(function () {
              
    if ($("#akcKwd").val() == "")
        $("#akcKwd").val(SearchTextValue);

    // GNB 창에 검색어가 없을 경우 초기화
    $("#akcKwd").focusout(function () {
        if ($("#akcKwd").val().replace(/^\s+|\s+$/g, '') == "") {
            $("#akcKwd").val(SearchTextValue);;
            $("#akcKwd").click(function () {
                if ($("#akcKwd").val() == SearchTextValue) {
                    $("#akcKwd").val("");
                }
            });
        }
    });


    // 검색 텍스트 박스 입력
    if ($("#akcKwd").val() == SearchTextValue) {
        $("#akcKwd").one("click", function () {
            $("#akcKwd").val("");
        });
    }
}); 
 </script>
 <style>
        /*#main_header fieldset:focus {}*/
     #header p a {padding-right:25px;}
     #header .searchTop { position:absolute; top:0; right:0; }
     #header .searchWrap { position:relative; display:inline-block; width:550px;}
     #header .searchWrap .search_btn { width:36px; height:42px; cursor:pointer; padding:0; border:0 none; }
     #header .searchWrap .search_btn span { display:block; width:36px; height:42px; background:url(/common/images/main/btn_search.png) no-repeat -3px -3px; }
     #header .searchWrap .autoSearch { position:absolute; top:53px; right:0; display:block; width:432px; padding-top: 10px; background: #fff; border: 1px solid #8d8c8b; border-top:none;  z-index: 30}
     #header .searchWrap .autoSearch li {width: 100%; height: 20px; padding: 2px 5px; text-align:left; }
     #header .foot_autoSearch {padding: 7px 5px; border-top: 1px solid #e2e2e2; background-color: #f2f2f2; text-align: right; font-size: 12px; font-family: '돋움', dotum}
</style>
<form name="form1" method="post" id="form1"></form>
<div class="searchTop">
    <form name="searchForm" id="searchForm"  method="get" onsubmit="return seachKwd();"  >
	    <div class="searchWrap">
            <fieldset id="searchBox">
		        <span><input type="text"  name="kwd" id="akcKwd"  onfocus="this.value=''"  value="<%=kwd%>" autocomplete="off" onkeypress="javascript:if(event.keyCode==13){return seachKwd(); }" /></span>
	                <button type="submit" class="search_btn"><span></span><!--img src="/common/images/main/btn_search.png"  alt="검색" /--></button>
	        </fieldset>
            <div class="autoSearch" id="akc_div" style="display:none;">
	            <ul class='keyword_list'></ul>
	            <div class="foot_autoSearch">
		            <a href="#" id="akc_switch"><span id="akc_text">기능끄기</span></a>
	            </div>
            </div>
        </div>
        <input type="hidden"  name="pageNum" id="pageNum" />
        <input type="hidden"  name="sort" id="sort" />
	</form>
</div>
   
