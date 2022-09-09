<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="ArraRegist.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.DigitalTrans.ArraRegist" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
 <style>
      .btn1{
	display:inline-block;
	height:32px;overflow:hidden;
	background:url(../../Common/images/btn/bg.png) no-repeat 0 -46px;
	padding-left:2px;
    cursor : pointer
}
.btn1 span{
	display:inline-block;
	height:27px;padding:5px 10px 0;
	background:url(../../Common/images/btn/bg.png) no-repeat right -46px;
    cursor : pointer
}
</style>

<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>ARRA 간행물</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>

<div class="TableStyleBox" style="margin-top:10px;">
	<table cellpadding="0" cellspacing="0" width="100%" class="TableStyle">
        <tr>
			<th class="tac" style="width:100px;"><strong>SKT 발간</strong></th>
			<td class="tal">
                <select id="selGubun" name="selGubun" runat="server" onchange="selGubunChange(this);">
                    <option value="">========= 선택 =========</option>
                    <option value="AT">ARRA Today(해외일일동향)</option>
                    <option value="WB">Weekly Brief</option>
                    <option value="GB">해외저널 Brief(Global Biz)</option>
                    <option value="AF">ARRA Focus</option>
                </select>
                &nbsp;&nbsp;&nbsp;<span><a href="#" onclick="selLink()">[ARRA 링크]</a></span>
			</td>
		</tr>
		<tr>
			<th class="tac" style="width:100px;"><strong>제목</strong></th>
			<td class="tal">
				<asp:TextBox ID="txtTitle" runat="server" Value="" MaxLength="800" Width="800" />
			</td>
		</tr>
		<tr>
			<th width="100" class="tac"><strong>Link</strong></th>
			<td class="tal">
				<asp:TextBox ID="txtLink" runat="server" Value="" MaxLength="1000" Width="1000" />
			</td>
		</tr>
	</table>
</div><br />
<div style="float:right; padding-top:10px; padding-bottom:10px;">
    
    <% if (System.Configuration.ConfigurationManager.AppSettings["DataTransfomationUser"].ToString().Contains(UserID)) {  %>
    <a class="btn1" style="top: 0px; right: 0px;" onclick="return RegistForm('R');"><span style="padding: 5px 20px 0px;"><strong>등록</strong></span></a>
    <a class="btn1" style="top: 0px; right: 0px;" onclick="return RegistForm('M');"><span style="padding: 5px 20px 0px;"><strong>수정</strong></span></a>
    <a class="btn1" style="top: 0px; right: 0px;" onclick="return RegistForm('D');"><span style="padding: 5px 20px 0px;"><strong>삭제</strong></span></a>
    <% } %>
</div>

 <table class="listTable">
	<colgroup><col width="4%" /><col width="4%" /><col width="*" /><col width="20%" /><col width="15%" /><col width="10%" /></colgroup>
	<thead>
	<tr>
        <th>선택</th>
		<th>No</th>
		<th>제목</th>
        <th>SKT발간</th>
        <th>등록자</th>
        <th>등록일</th>
	</tr>
	</thead>
	<tbody>
        <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound">
            <ItemTemplate>
                <tr >
                    <td><input type="radio" name="rdSel" style="border: none;" onclick="seletField('<%# DataBinder.Eval(Container.DataItem, "ID")%>  ');" /></td>
                    <td><asp:Literal runat="server" ID="Num"></asp:Literal></td>
                    <td style="text-align:left;"><a href="#" onclick="selArraDataLink('<%# DataBinder.Eval(Container.DataItem, "Url")%>');"><%# DataBinder.Eval(Container.DataItem, "Title")%></a>
                        <input type="hidden" value="<%# DataBinder.Eval(Container.DataItem, "Title")%>" id="hidTitle<%# DataBinder.Eval(Container.DataItem, "ID")%>" />
                        <input type="hidden" value="<%# DataBinder.Eval(Container.DataItem, "Url")%>" id="hidUrl<%# DataBinder.Eval(Container.DataItem, "ID")%>" />
                    </td>
                    <td style="text-align:left; height:30px;"><%# DataBinder.Eval(Container.DataItem, "GUBUN_NM")%>
                        <input type="hidden" value="<%# DataBinder.Eval(Container.DataItem, "Gubun")%>" id="hidGubun<%# DataBinder.Eval(Container.DataItem, "ID")%>" />
                    </td>
                    <td><%# DataBinder.Eval(Container.DataItem, "USER_NM")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "CreateDate")%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
	</tbody>
</table>
<div>
    <p class="pagination" style="">
    <Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" />
    </p>
</div>

     <script type="text/javascript">
         function Init()
         {
             $("#<%= this.selGubun.ClientID %>").val("").prop("selected", true);
             $("#<%= this.txtTitle.ClientID %>").val("");
             $("#<%= this.txtLink.ClientID %>").val("");
             $("#<%= this.hidID.ClientID %>").val("");
             $("#<%= this.hidMode.ClientID %>").val("");
         }
         function selGubunChange(obj)
         {
             selLink();
         }
         function selLink() {

             val = $('#<%= this.selGubun.ClientID %>').val();

             var Url = "";
             
             //ARRA Today(해외일일동향)
             if (val == "AT") {
                 Url = "http://arra.sktelecom.com/IndustrialTrend/Outside/DailyTrendList.aspx?MENU_ID=TR00001";
             
             }
             //Weekly Brief
             else if (val == "WB") {
                 Url = "http://arra.sktelecom.com/IndustrialTrend/Inside/WeeklyJournalIssueList.aspx?MENU_ID=TR00039";
             
             }
             //해외저널 Brief(Global Biz)
             else if (val == "GB") {
                 Url = "http://arra.sktelecom.com/IndustrialTrend/Outside/GlobalBizTrendList.aspx?MENU_ID=TR00005";
             
             }
             //ARRA Focus
             else if (val == "AF") {
                 Url = "http://arra.sktelecom.com/Focus/ArraFocusList.aspx?MENU_ID=BS00016";
             
             }
             else
             {
                 Url = "http://arra.sktelecom.com/IndustrialTrend/IndustrialTrendMain.aspx?MENU_ID=TR";
             }

             var win = window.open(Url, "ARRA", "left=500, top=100, width=1000, height=800, location=yes, scrollbars=yes, resizable=yes");
             win.focus();
             //var win = window.open(EncryptUrl, "ARRA", "left=100, top=10, width=10, height=10, toolbar=yes, menubar=yes, scrollbars=yes, resizable=no");
         }

         function selArraDataLink(Url)
         {
             var win = window.open(Url, "ARRA_URL");
             win.focus();
         }

         function seletField(id) {
             
             //alert(id);
             $('#<%= this.hidID.ClientID %>').val(id);
             $('#<%= this.selGubun.ClientID %>').val($("#hidGubun" + id).val()).prop("selected", true);
             $("#ctl00_MainContent_txtTitle").val($("#hidTitle" + id).val());
             $("#ctl00_MainContent_txtLink").val($("#hidUrl" + id).val());
         }

         function RegistForm(mode) {

             //alert(mode);
             if(mode == "M" || mode == "D")
             {
                 if ($('#<%= this.hidID.ClientID %>').val() == "")
                 {
                     alert("선택된 항목이 없습니다.")
                     return false;
                 }

                 if (mode == "D") {
                     if (!confirm("삭제하시겠습니까?"))
                         return false;
                 }
             }

             if(mode == "R" || mode == "M")
             {
                 if ($('#<%= this.selGubun.ClientID %>').val() == "")
                 {
                     alert("구분을 선택하여 주세요.");
                     $('#<%= this.selGubun.ClientID %>').focus();
                     return false;

                 }
                 if ($("#ctl00_MainContent_txtTitle").val() == "") {
                     alert("제목을 입력하여 주세요.");
                     $("#ctl00_MainContent_txtTitle").focus();
                     return false;

                 }
                 if ($("#ctl00_MainContent_txtLink").val() == "") {
                     alert("Link를 입력하여 주세요.");
                     $("#ctl00_MainContent_txtLink").focus();
                     return false;

                 }

             }
             
             $('#<%= this.hidMode.ClientID %>').val(mode);

             <%=Page.GetPostBackEventReference(btnSave) %>;

             //return true;
         }

</script> 
<div style="display:none;">
    <asp:HiddenField ID="hidMode" runat="server" />
    <asp:HiddenField ID="hidID" runat="server" />
    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />  
</div>
</asp:Content>