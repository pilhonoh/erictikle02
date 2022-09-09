<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="tikleMenu.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Stats.tikleMenu" %>
<asp:Content ID="Content0" ContentPlaceHolderID="MainContent" runat="server">
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>메뉴별통계</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
				
<div class="adminGuideBox">
	<dl>
		<dt><strong>Guide</strong></dt>
		<dd>날짜를 선택하시면 해당 기간의 통계를 조회하실수 있습니다<a href="javascript:DelCookie();" style="text-decoration:none;color:black;font-weight:normal;">.</a><br/>
		</dd>
	</dl>
</div>

<script type="text/javascript">
    function sarchDate(gbn) {
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
        
        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.07.28
        //Desc : 한달에서 1년으로 기간 변경
        if (betweenDay > 365) {
            alert("검색기간을 1년 이내로 지정하세요.");
            return false;
        }

        //조회일경우
        if(gbn == "S")
            __doPostBack('<%=SearchBtn.UniqueID %>', '');

        //엑셀일경우
        if (gbn == "E")
            return true;
    }
</script>
<div style="margin-top:30px;">
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
						<button type="button" onclick="return sarchDate('S');" >조회</button>
					</span>

					<asp:Button ID="SearchBtn" OnClick="SearchBtn_Click" runat="server" Text="조회" style="display:none"/>
					        
                    <asp:ImageButton ID="btnStatDeptToExcel" class="btnExcel" ImageUrl="~/Common/images/btn/excel.png" runat="server" OnClientClick="return sarchDate('E');" OnClick="btnStatDeptToExcel_Click" />			

				</td>
			</tr>
		</table>
	</div>
    <br />
	<div class="TableStyleBox">
	<table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
        <colgroup>
            <col style="width:200px;"/>
            <col style="width:400px;"/>
        </colgroup>
		<tr>
			<th class="tac"><strong>부서명</strong></th>
			<th class="tac"><strong>끌.지식</strong></th>
		</tr>
       
        <asp:Repeater ID="rptIn" runat="server" >
        <ItemTemplate>
        <tr>
	        <td class="tac"><%# DataBinder.Eval(Container.DataItem, "TODAY")%></td>
	        <td class="tac"><%# DataBinder.Eval(Container.DataItem, "GlossaryCNT")%></td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        <tr style="background-color:#fafafa;">
		    <td class="tac">합계</td>
		    <td class="tac"><asp:Literal ID="litGlossaryCNT" runat="server"></asp:Literal></td>
	    </tr>
	</table>
	</div>
	
</div>
</asp:Content>
