<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="tikleWeeklynote.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Stats.tikleWeeklynote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        // 달력 한글
       <%-- $(function () {
            $("#<%= txtsDate.ClientID %>").datepicker({ dateFormat: 'yy.mm.dd' });
            $("#<%= txteDate.ClientID %>").datepicker({ dateFormat: 'yy.mm.dd' });

            $.datepicker.regional['ko'] = {
                prevText: '이전달',
                nextText: '다음달',
                monthNames: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
                monthNamesShort: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
                dayNames: ['일', '월', '화', '수', '목', '금', '토'],
                dayNamesShort: ['일', '월', '화', '수', '목', '금', '토'],
                dayNamesMin: ['일', '월', '화', '수', '목', '금', '토'],
                firstDay: 0,
                yearSuffix: '',
                showAnim: "slideDown"//,
                //onSelect: function (dateText, inst){
                //    getWeekday(dateText);
                //}
            };
            $.datepicker.setDefaults($.datepicker.regional["ko"]);
        });--%>

        function getWeekday(sDate) {
            var yy = parseInt(sDate.substr(0, 4), 10);
            var mm = parseInt(sDate.substr(5, 2), 10);
            var dd = parseInt(sDate.substr(8), 10);
            var sd = new Date(yy, mm - 1, dd);
            var ed = new Date(yy, mm - 1, dd);

            switch (sd.getDay()) {
                case 0:
                    ed.setDate(ed.getDate() + 6);
                    break;
                case 1:
                    sd.setDate(sd.getDate() - 1);
                    ed.setDate(ed.getDate() + 5);
                    break;
                case 2:
                    sd.setDate(sd.getDate() - 2);
                    ed.setDate(ed.getDate() + 4);
                    break;
                case 3:
                    sd.setDate(sd.getDate() - 3);
                    ed.setDate(ed.getDate() + 3);
                    break;
                case 4:
                    sd.setDate(sd.getDate() - 4);
                    ed.setDate(ed.getDate() + 2);
                    break;
                case 5:
                    sd.setDate(sd.getDate() - 5);
                    ed.setDate(ed.getDate() + 1);
                    break;
                case 6:
                    sd.setDate(sd.getDate() - 6);
                    break;
            }

            var sdyy = sd.getFullYear();
            var sdmm = sd.getMonth() + 1;
            var sddd = sd.getDate();

            var edyy = ed.getFullYear();
            var edmm = ed.getMonth() + 1;
            var eddd = ed.getDate();

            if (sdmm < 10) { sdmm = "0" + sdmm; }
            if (sddd < 10) { sddd = "0" + sddd; }
            if (edmm < 10) { edmm = "0" + edmm; }
            if (eddd < 10) { eddd = "0" + eddd; }

            $("#<%= txtsDate.ClientID %>").val(sdyy + "." + sdmm + "." + sddd);
            $("#<%= txteDate.ClientID %>").val(edyy + "." + edmm + "." + eddd);
        }

        //function setCookie(name, value, expiredays) {
        //    var todayDate = new Date();
        //    todayDate.setDate(todayDate.getDate() + expiredays);
        //    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
        //}

        //function DelCookie() {
        //    setCookie("pop_doc_cookie2", "N", 365);
        //    setCookie("pop_doc_notice", "N", 365);
        //    setCookie("pop_qna_notice", "N", 365);
        //    setCookie("pop_doc_cookie2_GatheringOpen", "N", 365);
        //    setCookie("pop_doc_cookie2_WeeklyOpen", "N", 365);
        //    setCookie("pop_event_cookie_NamoEditor_2", "N", 365);
        //}

    </script>
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
				<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
					<tr>
						<td align="left">
						<!--// 페이지 타이틀 부분 -->
						<h2 class="Title">
						<strong>Weekly 작성 현황</strong>
						</h2>
						<!-- 페이지 타이틀 부분 //-->
						</td>
					</tr>
				</table>
				<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->

				<!--[if !IE]> 가이드 부분 <![endif]-->
<div class="adminGuideBox">
	<dl>
		<dt><strong>Guide</strong></dt>
		<dd>팀과 날짜를 선택하시면 해당 기간의 통계를 조회하실수 있습니다<a href="javascript:DelCookie();" style="text-decoration:none;color:black;font-weight:normal;">.</a><br/>
		</dd>
	</dl>
</div>
				<!--[if !IE]> 가이드 부분 <![endif]-->

<script type="text/javascript">
    function sarchDate() {
        var sd = $("#<%= txtsDate.ClientID %>").val();
        var ed = $("#<%= txteDate.ClientID %>").val();

        var dateArray = sd.split(".");
        var dateObj = new Date(dateArray[0], Number(dateArray[1]) - 1, dateArray[2]);

        var dateArray1 = ed.split(".");
        var dateObj1 = new Date(dateArray1[0], Number(dateArray1[1]) - 1, dateArray1[2]);

        var betweenDay = (dateObj1.getTime() - dateObj.getTime()) / 1000 / 60 / 60 / 24;
        betweenDay = Math.floor(betweenDay);

        if (betweenDay < 0) {
            alert("시작일과 종료일을 확인하세요.");
            return;
        }

        if (betweenDay > 365) {
            alert("검색기간을 365일내로 지정하세요.");
            return;
        }
        __doPostBack('<%=SearchBtn.UniqueID %>', '');
    }
</script>
<div style="margin-top:30px;">
	<dl>
		<dt>
			<div class="TableStyleBox">
				<table cellpadding="0" cellspacing="0" width="100%" class="TableStyle">
					<tr>
						<th width="100px" class="tac"><strong>팀 및 날짜선택</strong></th>
						<td>
                            <div style="float:left;">
                                <asp:DropDownList runat="server" ID="ddl_dept"></asp:DropDownList>
							    <asp:TextBox ID="txtsDate" runat="server" MaxLength="10" onKeyDown="return false;" Width="85px"></asp:TextBox>
                                <em class="from">~</em>
                                <asp:TextBox ID="txteDate" runat="server" MaxLength="10" onKeyDown="return false;" Width="85px"></asp:TextBox>
                           
                                <%--
                                    Author : 개발자-최현미, 리뷰자-윤자영
                                    Create Date : 2017.03.09 
                                    Desc : 공통달력적용
                                --%>

                                <a onclick="ViewCalendar(this, document.getElementById('<%=txtsDate.ClientID%>') ,document.getElementById('<%=txteDate.ClientID%>'),'yyyy.mm.dd','S',0,0);" style="cursor:pointer;">
				                    <img src="/common/calendar/images/ico_date.png" alt="날짜검색"/>
			                    </a>
                            </div>
                            <span class="button" style="cursor:pointer;float:left; margin:1px 10px 0">
						        <button type="button" onclick="sarchDate();" >조회</button>
					        </span>

						    <asp:Button ID="SearchBtn" OnClick="SearchBtn_Click" runat="server" Text="조회" style="display:none"/>
					        
                            <asp:ImageButton ID="btnStatDeptToExcel" class="btnExcel" ImageUrl="~/Common/images/btn/excel.png" runat="server" OnClick="btnStatDeptToExcel_Click" style="float:left; top:0; margin:0;" />			

						</td>
					</tr>
				</table>
			
			</div>
		</dt>
	</dl>
	<dl style="margin-top:30px;">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
			    <table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
                    <colgroup>
                        <% if(td_visible){ %>
                            <col width="10%" />
                        <% } %>
                        <col width="10%" />
                        <col width="10%" />
                        <col width="25%" />
                        <col width="25%" />
                        <col width="25%" />
                    </colgroup>
				    <tr>
                        <% if(td_visible){ %>
					        <th class="tac" colspan="3"><strong>부서명</strong></th>
                        <%} else{ %>
                            <th class="tac" colspan="2"><strong>부서명</strong></th>
                        <%} %>
                        <th class="tac"><strong>Weekly 작성건수(팀장본인)</strong></th>
                        <th class="tac"><strong>Weekly 작성팀원(팀원수/팀정원)</strong></th>
                        <th class="tac"><strong>Weekly Note 수신팀원(팀원수/팀정원)</strong></th>
				    </tr>
                    <asp:Repeater ID="rptIn" runat="server" >
                        <ItemTemplate>
	                        <tr>
                                <% if(td_visible){ %>
		                            <td class="tac"><%# DataBinder.Eval(Container.DataItem, "displayName1")%></td>
                                <%} %>
                                <td class="tac"><%# DataBinder.Eval(Container.DataItem, "displayName2")%></td>
                                <td class="tac"><%# DataBinder.Eval(Container.DataItem, "displayName3")%></td>
		                        <td class="tac"><%# DataBinder.Eval(Container.DataItem, "Weekly3")%></td>
                                <td class="tac"><%# DataBinder.Eval(Container.DataItem, "Weekly4")%></td>
                                <td class="tac"><%# DataBinder.Eval(Container.DataItem, "noteCNT")%></td>
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
			    </table>
			</div>
		</dd>
	</dl>
</div>
</asp:Content>