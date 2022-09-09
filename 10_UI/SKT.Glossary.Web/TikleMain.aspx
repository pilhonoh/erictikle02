<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryMain.Master" AutoEventWireup="true" CodeBehind="TikleMain.aspx.cs" Inherits="SKT.Glossary.Web.Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>T.끌, 소통과 협업의 플랫폼</title>
    <script type="text/javascript">
        var setup = true;
        var MainSearchTemp = false;
        var AutoSearchTemp = false;
        var SearchValue = '<%= SearchKeyword %>';
        var SearchTextValue = "찾고싶은 모든 것을 검색해 보세요.";

        $(document).ready(function () {

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
            if ($("#akcKwd").val() == SearchTextValue && AutoSearchTemp == false) {
                $("#akcKwd").one("click", function () {
                    $("#akcKwd").val("");
                });
            }

            /////////////////////아래는 디자인 코드//////////////

            var nowVisible = '';

            //DB
            if ("<%=popupFlag%>" == "N") {
                $(".popSample, .window").show();
            }

        }); //ready end 

        var lnbDep1 = 0;		//LNB 1depth


        $('.window .close').click(function (e) {
            //링크 기본동작은 작동하지 않도록 한다.
            e.preventDefault();
            $('.popSample, .window').hide();
        });

        //다시 이창 안보기
        function fnDoNotSee(id) {
            hidePop(id);
            var UserID = "<%=UserID%>";

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.04.20 / 2016.10.13
        Desc : Editor 교체 안내 팝업 변경 -> Main_Editor -> Main_Mobile
        */
         $.ajax({
             type: "POST",
             contentType: "application/json; charset=utf-8",
             url: "/TikleMain.aspx/PopupInsert",
             data: "{'UserID':'" + UserID + "','pop_Type':'Main_Single'}",
             dataType: "json",
             success: function (data) {
                 var d = data.d;
             },
             error: function (result) {

             }
         });
     }

     // 레이어 창 닫기
     function hidePop(pid) {
         $("#" + pid).hide();
         $("div.popSample").hide();
     }

     function fnView(commonid, type, gubun) {
         var url = "/Glossary/GlossaryView.aspx?ItemID=" + commonid;

         if (gubun == 1) {
             if (type.length > 0 && type != 'Z')
                 url += "&TType=" + type;
         }

         if (gubun == 2) {
             if (type.length > 0 && type != 'Z')
                 url += "&WType=" + type;
         }

         location.href = url;
     }
     function CloseMessage()
	 {
         alert("안녕하세요.\n\n금년도 9월 시스템 노후화로 인해 T.끌 문서함 서비스가 종료됩니다.\n\n이에 문서 추가/수정, 멤버관리 기능이 먼저 차단됨을 공지드립니다.\n\n추후 구성원 간의 문서 공유 시 TEAMS를 활용해주시기 바랍니다.\n\n감사합니다."); 
         location.href = "/Directory/DirectoryListNew.aspx";
	 }
</script>
<style>
    .searchWrap .search_btn span { position:absolute; display:inline-block; top:0; right:0; width:50px; height:42px; background:url(/common/images/ico_search.png) no-repeat 0px 0px; cursor:pointer;}
    .searchWrap .autoSearch { position:absolute; top:45px; left:0; display:block; width:635px; padding-top: 10px; background: #fff; border: 1px solid #8d8c8b; border-top:none;  z-index: 30}
    .searchWrap .autoSearch li {width: 100%; height: 24px; padding: 2px 5px; text-align:left; font-size: 13px; }
    .foot_autoSearch {padding: 7px 5px; border-top: 1px solid #e2e2e2; background-color: #f2f2f2; text-align: right; font-size: 12px; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<div id="main">
	<div class="mainContents">
		<div class="mainInner">
			<div id="main_header">
				<h1><span class="blind">T.끌 모두의 지식공유</span><span style="display:inline-block; margin:0 0 0 4px; color:#54383c; font-size:25px; font-weight:normal; vertical-align: top; position:absolute;left:400px">
                    <% if (System.Environment.MachineName.Equals("SKT-TNCDALL1")) { %>
                    DEV
                    <% }  else if (System.Environment.MachineName.Equals("SKT-TNCNPWEB1") || System.Environment.MachineName.Equals("SKT-TNCNPWEB2")) { %>
                    <% } else { %>
                    Local
                    <% } %>
                    </span></h1>
				<!--검색-->
                <form name="searchForm" id="searchForm" method="get" onsubmit="return seachKwd();" action="/search/search.aspx"  >
				<div class="searchWrap">
					<fieldset id="searchBox">				
                        <span><input type="text"  name="kwd" id="akcKwd"  onfocus="this.value=''"  autocomplete="off" onkeypress="javascript:if(event.keyCode==13){seachKwd(); }" /></span>
	                    <button type="submit" class="search_btn"><span></span></button>
					</fieldset>
                        <div class="autoSearch" id="akc_div" style="display:none;">
	                    <ul class='keyword_list'></ul>
	                    <div class="foot_autoSearch">
		                    <a href="#" id="akc_switch"><span id="akc_text">기능끄기</span></a>
	                    </div>
                    </div>
				</div>
                </form>
				<!--/검색-->
			</div>
			<div id="contents">
                <!-- CHG610000074852 / 20181108 / T생활백서 -->
				<div class="main_gnb">
					<ul id="main_gnb">
						<li class="tikleMenu01"><a href="/Glossary/Glossary.aspx" title="끌지식"><span class="icon"></span>끌지식</a></li>
						<!--<li class="tikleMenu02"><a href="javascript:CloseMessage();" title="끌문서"><span class="icon"></span>끌문서</a></li>-->
						<li class="tikleMenu03"><a href="/Glossary/WhitePaper.aspx" title="T생활백서"><span class="icon"></span>T생활백서</a></li>	
					</ul>
				</div>

				<div class="contsList">
					<h3>
						<span onclick="location.href='/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate'" style="cursor:pointer;" title="Hot &amp; New">Hot &amp; New</span>
						<a href="/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate" class="more" title="Hot &amp; New"><span class="blind">더보기</span></a>
					</h3>
					<ul>
                        <asp:Repeater ID="rptHN" runat="server" OnItemDataBound="rptHN_OnItemDataBound">
                        <ItemTemplate>
                        <li><a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>', '<%# DataBinder.Eval(Container.DataItem, "TType")%>', '1');" title="<%# DataBinder.Eval(Container.DataItem, "Title")%>"> <%# DataBinder.Eval(Container.DataItem, "Title")%> </a></li>
                        </ItemTemplate>
                        </asp:Repeater>
					</ul>
				</div>

                <!-- CHG610000073120 / 2018-10-05 / 최현미 / DT블로그홈 --> 
				<div class="contsList">
					<h3>
						<span onclick="location.href='/Glossary/DigitalTrans.aspx'" style="cursor:pointer;" title="DT 블로그">DT 블로그</span>
						<a href="/Glossary/DigitalTrans.aspx" class="more" title="DT 블로그"><span class="blind">더보기</span></a>
					</h3>
					<ul>
                        <asp:Repeater ID="rptDT" runat="server" OnItemDataBound="rptDT_OnItemDataBound">
                        <ItemTemplate>
                        <li><a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>', '<%# DataBinder.Eval(Container.DataItem, "WType")%>','2');" title="<%# DataBinder.Eval(Container.DataItem, "Title")%>"> <%# DataBinder.Eval(Container.DataItem, "Title")%> </a></li>
                        </ItemTemplate>
                        </asp:Repeater>
					</ul>
				</div>
			</div>

		</div>
	</div>
</div>
<div class="popSample">
    <div class="popBgSample"></div>     
    <div class="window" id="pop_event_platform" style="float:center; top: 88px;">
	   <%-- <table style="width:550px; height:660px; position:relative; top:0; padding:0; border:0; border-spacing:0;border-collapse:collapse;  float:center; margin: 0 auto">
	    <tr>
		    <td>
                <img src='/Common/images/main/single_popup.png' border='0' usemap="#imgmap1106" />
                <map id="imgmap1106" name="imgmap1106">
                <area shape="rect" alt="닫기" title="닫기" coords="473,13,533,70" href="javascript:hidePop('pop_event_platform');" target="_self" />
                <area shape="rect" alt="전사협업시스템모바일사용자가이드" title="전사협업시스템모바일사용자가이드" coords="56,471,290,491" href="./enterprise_collaboration_mobile_guide.pptx" target="_self" />
                <area shape="rect" alt="전사협업시스템사용자가이드" title="전사협업시스템사용자가이드" coords="300,471,494,491" href="./enterprise_collaboration_system_guide.pdf" target="_blank" />
                </map>
            </td>
        </tr>
        <tr>
            <td>
			    <div style="width:550px; background:#111; padding:5px 0; margin-left:0px;margin-top:0px;"><a href="javascript:fnDoNotSee('pop_event_platform');"><u style=" color:#fff; font-size:14px; text-decoration:none;">이 창을 다시 보지 않기</u></a></div>
		    </td>
	    </tr>
	    </table>      --%>
</div>
</div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
</asp:Content>
    