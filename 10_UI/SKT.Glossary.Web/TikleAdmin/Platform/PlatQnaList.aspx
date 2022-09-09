<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="PlatQnaList.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Platform.PlatQnaList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script type="text/javascript">

        function fn_PlatformMove(obj) {
            var Result = confirm('확인을 누르시면 해당 끌 질문은 Platform 전용 페이지에 보여집니다.');
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

        //뷰화면 이동
        function fnMyQnAView(ItemID) {
            //location.href = "/QnA/QnAView.aspx?ItemID=" + ItemID;
            var url = "/QnA/QnAView.aspx?ItemID=" + ItemID;

            location.href = url;
        }

        //뷰화면 이동
        function fnMyQnAList(searchsort, searchsortgubun) {
            //location.href = "/QnA/QnAView.aspx?ItemID=" + ItemID;
            var url = "/Platform/PlatQnaList.aspx?SearchSort=" + searchsort + "&SearchSortGubun=" + searchsortgubun;

            location.href = url;
        }


        //20131120 수정
        //프로필 화면 이도 이동
        function fnProfileView(UserID) {
            if (UserID == "") {
                alert("작성자 비공개 글 입니다.");

            } else {
                location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
            }

        }
        var lnbDep1 = 5;		//LNB 1depth
    </script>
    
	<!--CONTENTS-->
		<div id="article" class="qna_list">
            <ul id="tabMenu">
		        <li><a href="PlatGlossaryList.aspx" ><img src="/common/images/btn/Platform_tab3.png" alt="끌.지식 업로드 리스트" /></a></li>
		        <li><a href="PlatQnaList.aspx" class="on"><img src="/common/images/btn/Platform_tab4.png" alt="끌.질문 업로드 리스트" /></a></li>
	        </ul>
			<table class="listTable">
				<colgroup><col width="6%"><col width="6%" /><col width="*" /><col width="9%" /><col width="15%" /><col width="18%" /><col width="7%" /><col width="6%"></colgroup>
				<thead>
				<tr>
                    <th>선택</th>
					<th>No</th>
					<%--<th><a href="javascript:fnMyQnAList('Title', '<%= (SearchSort == "Title" && SearchSortGubun == "DESC") ? "ASC" : "DESC" %>');" class="Atag">제목</a></th>
					<th><a href="javascript:fnMyQnAList('CreateDate', '<%= (SearchSort == "CreateDate" && SearchSortGubun == "DESC") ? "ASC" : "DESC" %>');" class="Atag">작성일</a></th>
					<th>작성자</th>
					<th><a href="javascript:fnMyQnAList('CommentHits', '<%= (SearchSort == "CommentHits" && SearchSortGubun == "ASC") ? "DESC" : "ASC" %>');" class="Atag">상태<%= (SearchSort == "CommentHits" && SearchSortGubun == "DESC") ? "▲" : "▼" %></a></th>
					<th><a href="javascript:fnMyQnAList('Hits', '<%= (SearchSort == "Hits" && SearchSortGubun == "DESC") ? "ASC" : "DESC" %>');" class="Atag">조회<%= (SearchSort == "Hits" && SearchSortGubun == "DESC") ? "▲" : "▼" %></a></th>--%>
                    <th>제목</th>
					<th>작성일</th>
					<th>작성자</th>
					<th>상태</th>
					<th>조회</th>
                    <th>Platform</th>
				</tr>
				</thead>
				<tbody>
                    <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptIn_OnItemDataBound" >
                        <ItemTemplate>		
                            <tr>
                                <td><input type="checkbox" name="checkbox1" onclick="fn_PlatformMove('<%# DataBinder.Eval(Container.DataItem, "ID")%>    ');"></td>
								<td><asp:Literal runat="server" ID="Num" ></asp:Literal></td>
								<td class="al"><asp:Literal ID="litQnaTitle" runat="server"  ></asp:Literal></td>
								<td><%# DataBinder.Eval(Container.DataItem, "CreateDate")%></td>
								<td><asp:Literal ID="litUserInfo" runat="server" ></asp:Literal></td>
                                <%--<td><a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>');"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a></td>--%>
								<td><asp:Literal ID="lbSuccess" runat="server"></asp:Literal></td>
								<td><%# DataBinder.Eval(Container.DataItem, "Hits")%></td>
                                <td><asp:Literal ID="PlatformYN" runat="server"></asp:Literal></td>
                            </tr>			
                        </ItemTemplate>
                    </asp:Repeater>		
				</tbody>
			</table>
			<p class="pagination" style="position:relative;">
				<Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
			</p>
		</div>

	<!--/CONTENTS-->
    <div style="display:none;">
        <asp:HiddenField ID="hidMenuType" runat="server" />     
        <asp:HiddenField ID="hdd_PlatformMoveID" runat="server" />
        <asp:Button ID="btn_Platform_update" runat="server" OnClick="btn_Platform_update_Click" />
    </div>
</asp:Content>
