<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="PlatGlossaryList.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Platform.PlatGlossaryList" %>

<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_PlatformMove(obj) {
            var Result = confirm('확인을 누르시면 해당 끌지식은 Platform 전용 페이지에 보여집니다.');
            if (Result) {
                document.getElementById("<%= hdd_PlatformMoveID.ClientID%>").value = obj;
                document.getElementById("<%= btn_Platform_update.ClientID%>").click();
                //alert("페이지이동!");
            } else {
                var pcode = document.getElementByTagNames("checkbox1");
                alert(pcode);
                for(var i = 0; i < pcode.length; i++){
                    pcode[i].checked=false;
                }
            }
        }

        //제목, 내용 뷰화면 가기
        function fnView(ItemID, HistoryYN) {
            if ('<%= CategoryID %>' == "") {
                var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&GatheringYN=<%=this.GatheringYN%>&GatheringID=<%=this.GatheringID%>";
                //url += ("&PrevListUrl=" + encodeURIComponent(location.pathname + "?PageNum=" + '<%= pager.CurrentIndex%>'));
                location.href = url;
            }
            else {
                var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&GatheringYN=<%=this.GatheringYN%>&GatheringID=<%=this.GatheringID%>";
                //url += ("&PrevListUrl=" + encodeURIComponent(location.pathname +"?CategoryID=" + '<%= CategoryID%>' + "&PageNum=" + '<%= pager.CurrentIndex%>'));
                location.href = url;
            }
        }
    </script>
    <ul id="tabMenu">
		<li><a href="PlatGlossaryList.aspx" class="on"><img src="/common/images/btn/Platform_tab3.png" alt="끌.지식 업로드 리스트" /></a></li>
		<li><a href="PlatQnaList.aspx"><img src="/common/images/btn/Platform_tab4.png" alt="끌.질문 업로드 리스트" /></a></li>
	</ul>
    <table class="listTable">
		<colgroup><col width="6%"><col width="6%" /><col width="*" /><col width="11%" /><col width="20%" /><col width="6%" /><col width="6%" /><col width="6%" /></colgroup>
		<thead>
		<tr>
            <th>선택</th>
			<th>No</th>
			<th>제목</th>
			<th>마지막 편집일</th>
			<th>마지막 편집자</th>
			<th>조회</th>
			<th>추천</th>
            <th>Platform</th>
		</tr>
		</thead>
		<tbody>
            <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td><input type="checkbox" name="checkbox1" onclick="fn_PlatformMove('<%# DataBinder.Eval(Container.DataItem, "commonID")%>');"></td>
                        <td><asp:Literal runat="server" ID="Num"></asp:Literal></td>
                        <div class="types" style="display: none">
                            <asp:Literal runat="server" ID="ltWiki"></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span>
                            </div>
                        <td class="al">
                            <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>')" class="Atag">
                            <asp:Literal runat="server" ID="litPermission"></asp:Literal>
                                <%# DataBinder.Eval(Container.DataItem, "Title")%><asp:Literal runat="server" ID="litReply"></asp:Literal>
                            </a>
                        </td>
                                   
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "LastCreateDate")%>
                        </td>
                        <td>
                            <asp:Literal ID="litUserInfo" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Hits")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "LikeCount")%>
                        </td>
                        <td>
                            <asp:Literal ID="PlatformYN" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
		</tbody>
	</table>
    <p class="pagination" style="position:relative;">
        <Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" />
	</p>
    <span style="display:none;">
        <asp:HiddenField ID="hdd_PlatformMoveID" runat="server" />
        <asp:Button ID="btn_Platform_update" runat="server" OnClick="btn_Platform_update_Click" />
    </span>
</asp:Content>
