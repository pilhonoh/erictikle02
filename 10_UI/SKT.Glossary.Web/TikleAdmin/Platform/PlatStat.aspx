<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="PlatStat.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Platform.PlatStat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
  <!--
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
            showAnim: "slideDown"
        };
        $.datepicker.setDefaults($.datepicker.regional["ko"]);
    });--%>
    //-->
<%--    function fn_DirExcelList() {
        document.getElementById("<%=btn_DirListExcel.ClientID%>").click();
     }
     function fn_SurveyExcelList() {
         document.getElementById("<%=btn_SurveyListExcel.ClientID%>").click();
     }--%>
  </script>   
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
				<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
					<tr>
						<td align="left">
						<!--// 페이지 타이틀 부분 -->
						<h2 class="Title">
						<strong>Platform 접속자 수</strong>
						</h2>
						<!-- 페이지 타이틀 부분 //-->
						</td>
					</tr>
				</table>
				<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->

				<!--[if !IE]> 가이드 부분 <![endif]-->
<%--<div class="adminGuideBox">
	<dl>
		<dt><strong>전체</strong></dt>
		<dd>
            지식개수 : <asp:Literal ID="litGTOTALCNT" runat="server"></asp:Literal>
            &nbsp;&nbsp;질문개수 : <asp:Literal ID="litQTOTALCNT" runat="server"></asp:Literal>
            &nbsp;&nbsp;문서함개수 : <asp:Literal ID="litDIRTOTALCNT" runat="server"></asp:Literal>
            &nbsp;&nbsp;의견함개수 : <asp:Literal ID="litSVTOTALCNT" runat="server"></asp:Literal>
		</dd>
	</dl>
</div>--%>
				<!--[if !IE]> 가이드 부분 <![endif]-->

<script type="text/javascript">
    function sarchDate() {
        __doPostBack('<%=SearchBtn.UniqueID %>', '');
    }
</script>
<div style="margin-top:30px;">
	<dl>
		<dt>
			<div class="TableStyleBox">
				<table cellpadding="0" cellspacing="0" width="100%" class="TableStyle">
					<tr>
						<th width="100px" class="tac"><strong>날짜선택</strong></th>
						<td>
							<div style="float:left;">
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
                            <asp:ImageButton ID="btnStatTotalToExcel" class="btnExcel" runat="server" ImageUrl="~/Common/images/btn/excel.png" OnClick="btnStatTotalToExcel_Click" />
                            
					        <%--<asp:Button ID="btn_DirListExcel" OnClick="btn_DirListExcel_Click" runat="server" style="display:none"/>--%>
                            <%--<asp:Button ID="btn_SurveyListExcel" OnClick="btn_SurveyListExcel_Click" runat="server" style="display:none"/>--%>
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
				<tr>
					<th width="40%" class="tac"><strong>날짜</strong></th>
					<th width="60%" class="tac"><strong>접속자수</strong></th>
				</tr>

<asp:Repeater ID="rptIn" runat="server" >
<ItemTemplate>
	<tr>
		<td class="tac"><%# DataBinder.Eval(Container.DataItem, "TODAY")%></td>
		<td class="tac"><%# DataBinder.Eval(Container.DataItem, "TOTALCNT")%></td>
	</tr>
    </ItemTemplate>

</asp:Repeater>
		<tr>
		    <td class="tac">합계</td>
		    <td class="tac"><asp:Literal ID="litTOTALCNT" runat="server"></asp:Literal></td>
	    </tr>
			</table>
			</div>
		</dd>
	</dl>
</div>
</asp:Content>
