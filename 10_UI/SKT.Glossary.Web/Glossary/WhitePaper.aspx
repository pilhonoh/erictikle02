<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="WhitePaper.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.WhitePaper" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $('#txt_SearchKeyword').val($("#<%= this.hidSearchText.ClientID %>").val());
    });
    function fnView(ItemID, HistoryYN, tType, Gubun) {
       
        var result = true;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/AjaxControl.aspx/GetGlossaryPermissions",
            data: "{commonID : '" + ItemID + "', userID : '<%=UserID%>'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d < 1) {
                        $("#pop_permission").show();
                        result = false;
                    }
                },
                error: function (result) {
                    //alert("Error DB Check");
                }
            });

        if (result) {
            var url = "";
            if (Gubun == "List")
                url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&PageNum=<%=pager.CurrentIndex%>&TType=" + tType + "&SchText=" + encodeURIComponent(encodeURIComponent($('#txt_SearchKeyword').val()));
            else
                url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&PageNum=<%=pager.CurrentIndex%>&TType=" + tType;
                location.href = url;
         }
    }

    function fnFileSearch() {

        $("#txt_SearchKeyword").val(strip_tag($("#txt_SearchKeyword").val().replace(/\'/gi, "").replace(/\"/gi, "").replace(/\\/gi, "")));
  
        $("#<%= this.hidSearchText.ClientID %>").val($('#txt_SearchKeyword').val());
        <%=Page.GetPostBackEventReference(btnSearch) %>;
 
    }
   
    function pop_permissionclose() {
        $(".pop_glossarypermission").hide();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
        var lnbDep1 = 1;		//LNB 1depth
</script>

<div id="contents">
	<div class="h2tag">

    </div>
    <div id="article" class="Glossary_list" style = "padding-top:30px;">
        <div id="list_area">
			<!--태그-->
            <!--// CHG610000074852 / 20181108 / T생활백서  -->
            <ul id="tabMenu" class="tabMenu" style="padding-bottom:20px">
				<li><a href="WhitePaper.aspx?TType=A" <% if (TType.Equals("A")) { Response.Write("class=\"on\""); } else { Response.Write(""); } %>>T생활 백서</a></li>
				<li><a href="WhitePaper.aspx?TType=B" <% if (TType.Equals("B")) { Response.Write("class=\"on\""); } else { Response.Write(""); } %>>T업무 백서</a></li>
                <li><a href="WhitePaper.aspx?TType=C" <% if (TType.Equals("C")) { Response.Write("class=\"on\""); } else { Response.Write(""); } %>>T식사 백서</a></li>
			</ul>
                <div id="SearchBar" runat="server" class="search_list_wrap">
				    <span>
					    <input class="ui-autocomplete-input search_list" id="txt_SearchKeyword" onkeypress="" onfocus="" type="text" >
					    <a href="javascript:fnFileSearch();" class="btn_search">검색</a>
				    </span>
			    </div>
                <table class="listTable"  >
					<colgroup><col width="6%" /><col width="*" /><col width="11%" /><col width="20%" /><col width="6%" /><col width="6%" /></colgroup>
					<thead>
					<tr>
						<th>No</th>
						<th>제목</th>
						<th>마지막 편집일</th>
						<th>마지막 편집자</th>
						<th>조회</th>
						<th>추천</th>
					</tr>
					</thead>
					<tbody>
                        <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:Literal runat="server" ID="Num"></asp:Literal></td>
                                    <div class="types" style="display: none">
                                        <asp:Literal runat="server" ID="ltWiki"></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span>
                                        </div>
                                    <td class="al">
                                        <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>','<%=TType%>', 'List')">
                                        <asp:Literal runat="server" ID="litPermission"></asp:Literal>
                                            <%# DataBinder.Eval(Container.DataItem, "Title")%>&nbsp;<asp:Literal runat="server" ID="litReply"></asp:Literal>
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
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
					</tbody>
				</table>
			<p class="pagination" style="position:relative;">
               <a href="/Glossary/GlossaryWriteNew.aspx?TType=<%=TType%>" class="btn1" style="position:absolute; top:0px; right:0;"><span style="padding:5px 20px 0;"><strong>지식올리기</strong></span></a>
                <Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" />
			</p>
        </div>
        <div class="raking_list">
				<dl>
					<dt>인기조회순</dt>
                    <asp:Repeater ID="rptHits" runat="server" >
                        <ItemTemplate>
                            <dd>
                                <b style="color:black"><%# DataBinder.Eval(Container.DataItem, "RowNum")%>.</b>                                            
                                <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>', '<%# DataBinder.Eval(Container.DataItem, "TWhiteFlag")%>', 'Hit')">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                </a>
                            </dd>
                        </ItemTemplate>
                    </asp:Repeater>
				</dl>
                <dl>
					<dt>인기추천수</dt>
                    <asp:Repeater ID="rptLike" runat="server">
                        <ItemTemplate>
                            <dd>
                                <b style="color:black"><%# DataBinder.Eval(Container.DataItem, "RowNum")%>.</b>                                            
                                <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>', '<%# DataBinder.Eval(Container.DataItem, "TWhiteFlag")%>', 'Hit')">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                </a>
                            </dd>
                        </ItemTemplate>
                    </asp:Repeater>
				</dl>
				
			</div>
    </div>
</div>




<div class="pop_glossarypermission" id="pop_permission" style="display:none;">
        <div class="pop_glossaryheader"><img style="padding-top:1px;cursor:pointer;" src="../Common/images/btn/btn_close.png" onclick="javascript:pop_permissionclose();" /></div>
        <div class="pop_glossarybody">이 지식은 조회권한이 부여된 분들만<br /> 보실 수 있습니다.
            <div onclick="javascript:pop_permissionclose();">확인 </div>
        </div>
 </div>
<asp:HiddenField ID="hidSearchText" runat="server" />
<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
</asp:Content>