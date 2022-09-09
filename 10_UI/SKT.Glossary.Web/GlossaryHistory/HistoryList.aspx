<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" validateRequest="false" AutoEventWireup="true" CodeBehind="HistoryList.aspx.cs" Inherits="SKT.Glossary.Web.CommonPages.GlossaryHistoryList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
<script type="text/javascript">
    var RevertID = null;
    var DescriptionTemp = null;
    //되돌리기 버튼
    function fnRevert() {
        $('#<%= this.hidRevertID.ClientID %>').val(RevertID);
        var PostBackStr = "<%=Page.GetPostBackEventReference(this.btnRevert)%>";
        eval(PostBackStr);
    }

    //뷰 보기
    function fnGoView(ItemID, HistoryYN, CommonID) {
        if (HistoryYN == 'N') {
           
            <% if (!string.IsNullOrEmpty(this.WType)) { %>
                location.href = "/Glossary/GlossaryView.aspx?ItemID=" + CommonID + "&SearchSort=<%=this.SearchSort%>&PageNum=<%=PageNumList%>&WType=<%=WType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
            <% } else if (!string.IsNullOrEmpty(this.TType)) { %>
                location.href = "/Glossary/GlossaryView.aspx?ItemID=" + CommonID + "&SearchSort=<%=this.SearchSort%>&PageNum=<%=PageNumList%>&TType=<%=TType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
            <% } else  { %>
                location.href = "/Glossary/GlossaryView.aspx?ItemID=" + CommonID + "&SearchSort=<%=this.SearchSort%>&PageNum=<%=PageNumList%>&TagTitle=" + encodeURIComponent(encodeURIComponent("<%=TagTitle%>"));
            <% }  %>
            
        } else {
            <% if (!string.IsNullOrEmpty(this.WType)) { %>
                location.href = "/GlossaryHistory/HistoryView.aspx?ItemID=" + ItemID + "&CommonID=" + CommonID + "&SearchKeyword=" + encodeURIComponent(encodeURIComponent("<%=SearchKeyword%>"))+"&WType=<%=WType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
            <% } else if (!string.IsNullOrEmpty(this.TType)) { %>
                location.href = "/GlossaryHistory/HistoryView.aspx?ItemID=" + ItemID + "&CommonID=" + CommonID + "&SearchKeyword=" + encodeURIComponent(encodeURIComponent("<%=SearchKeyword%>"))+"&TType=<%=TType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
            <% } else  { %>
                location.href = "/GlossaryHistory/HistoryView.aspx?ItemID=" + ItemID + "&CommonID=" + CommonID + "&SearchKeyword=" + encodeURIComponent(encodeURIComponent("<%=SearchKeyword%>"))+"&SearchSort=<%=this.SearchSort%>&PageNum=<%=PageNumList%>&TagTitle=" + encodeURIComponent(encodeURIComponent("<%=TagTitle%>"));
            <% }  %>
        }

    }

    //프로파일 이동
    function fnProfileView(UserID) {
        if (UserID == "") {
            alert("작성자 비공개 글 입니다.");
        }
        else {
            location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
        }

    }


    $(document).ready(function () {
        $(".history-global-blackout").hide()
        $(".history-cloud-wrap").hide()

        if ("<%=GatheringYN%>" == "Y") {
            $("#container").attr('class', 'Gathering');
        }
    });

    function fnClose() {
        $(".history-global-blackout").hide()
        $(".history-cloud-wrap").hide()
    }

    function fnRevetOpen(ItemID, HistoryYN) {
        if (HistoryYN == "N") {
            alert("현재 글 입니다. '되돌리기' 할 수 없습니다.");
        } else {
            RevertID = ItemID;

            $("#<%= this.DescriptionTemp.ClientID %>").val("");
            $("#divpop").show()
            $("#pop_back").show()
        }
    }
    function pop_back_close() {
        $("#<%= this.DescriptionTemp.ClientID %>").val("");
        $("#divpop").hide()
        $("#pop_back").hide()
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>

<asp:Content id="Content4" ContentPlaceholderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 0;		//LNB 1depth
</script>
	<!-- roll back -->
<div id="divpop" class="pop">
	<div class="popBg"></div>
	<!--지식_되돌리기-->
	<div id="pop_back" class="layer_pop">
		<h3>되돌리기를 선택하셨습니다.<br />사유를 작성해주세요</h3>
		<div><input type="text" runat="server" id="DescriptionTemp" class="txt t6" /></div>
		<p class="btn_c">
			<a href="javascript:pop_back_close();" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnRevert();" class="btn3"><b>되돌리기</b></a> 
		</p>
		<input type="image" src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="pop_back_close(); return false;" />
	</div>
	<!--/지식_되돌리기-->
</div>


	<!--CONTENTS-->
    <div id="contents">
		<div class="h2tag">
            <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType))  { %>
            <img src="/common/images/text/Glossary.png" alt="끌.지식" />
            <% } %>
           
		</div>
        	<p class="btn_top">
                 <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType))  { %>
                 <a href="/Glossary/GlossaryWriteNew.aspx"><img src="/common/images/btn/write.png" alt="글쓰기" /></a>
                 <% } %>
        	</p>
		<div id="article" class="edit_list">
			<div class="box_shadow">
				<h3 class="view_title"><b>편집 내역 보기 :</b><%= SearchKeyword %></h3>
                <p class="view_num">편집횟수 <b><%= pager.ItemCount%></b></p>
				<table class="listTable">
					<colgroup><col width="8%" /><col width="15%" /><col width="10%" /><col width="*" /><col width="10%" /></colgroup>
					<thead>
						<tr>
							<th>No</th>
							<th>마지막 편집자</th>
							<th>편집일</th>
							<th>수정 사유/내용</th>
							<th>되돌리기</th>
						</tr>
					</thead>
					<tbody>
                        <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound" >
                            <ItemTemplate>
					            <tr>
						            <td><asp:Literal runat="server" ID="Num" ></asp:Literal></td>
						            <td>
                                        <%--<a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>');"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a>--%>
						                <asp:Literal ID="litUserInfo" runat="server" />
                                    </td>
						            <td><%# DataBinder.Eval(Container.DataItem, "CreateDate")%></td>
						            <td><a href="javascript:fnGoView('<%# DataBinder.Eval(Container.DataItem, "ID")%>', '<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>', '<%# DataBinder.Eval(Container.DataItem, "CommonID")%>')" class="history"><%# DataBinder.Eval(Container.DataItem, "Description")%></a></td>
						            <td><a href="javascript:fnRevetOpen('<%# DataBinder.Eval(Container.DataItem, "ID")%>', '<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>', '<%# DataBinder.Eval(Container.DataItem, "CommonID")%>');" class="btn_back" ><img src="/common/images/btn/back.png" alt="" title="되돌리기" /></a></td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
					</tbody>
				</table>
				<p class="pagination">
					<Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
				</p>
			</div>
		</div>
	</div>
	<!--/CONTENTS-->

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
     <asp:Button ID="btnRevert" runat="server" style="display:none" OnClick="btnRevert_Click" />
     <asp:HiddenField ID="hidRevertID" runat="server" />
</asp:Content>

