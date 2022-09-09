<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Glossary.Master" AutoEventWireup="true" CodeBehind="MyTempList.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyTempList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">
        //리스트 클릭 시 ROOT 글이 수정이면 수정이 안됨
        function fnMyTempWrite(ItemID, CommonID) {
            $.ajax({
                type: "POST",
                url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryModifyYN",
                data: "{ItemID : '" + CommonID + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (ModifyYNTableCnt) {
                    if (ModifyYNTableCnt.d == "0") {
                        alert("수정 중인 문서 입니다.");
                        return false;
                    } else {
                        location.href = "/Glossary/GlossaryWrite.aspx?mode=MyTemp&ItemID=" + ItemID + "&CommonID=" + CommonID;
                    }
                }
            });
        }
        $(document).ready(function () {
            if ('<%= mode %>' == "TempSave")
                alert("60분간 내용 입력이 없어 편집이 종료 되었습니다. 계속 편집을 원하시면 문서를 선택해 주세요.");

        });

            function FnGoMyTemp() {
                location.href = "/GlossaryMyPages/MyTempList.aspx";
            }

            function TebMenu(TebMenu) {
                location.href = "/GlossaryMyPages/MyDocumentsList.aspx?Tabmenu=" + TebMenu;
            }

            function fnSelectAll(oObj) {
                var _bVal = document.getElementById('<%= this.checkbox.ClientID%>').checked;

            if (typeof (oObj) == "undefined" || oObj == null) {
                alert("선택 가능 항목이 존재하지 않습니다.");
                return;
            }

            if (typeof (oObj.length) == "undefined")			//1개존재시
            {
                //if (oObj.disabled == false)
                oObj.checked = _bVal;
                //else
                //oObj.checked = false;
            }
            else {
                for (var i = 0; i < oObj.length; i++) {
                    //if (oObj[i].checked == false)
                    oObj[i].checked = _bVal;
                    //else
                    //    oObj[i].checked = false;
                }
            }

            _bVal = !(_bVal);
        }

        function fnDeleteconfirm() {
            var oObj = document.all.checkJob;
            var ischeked = false;
            if (typeof (oObj) != "undefined" || oObj != null) {
                for (var i = 0; i < oObj.length; i++) {
                    if (oObj[i].checked == true) {
                        ischeked = true;
                    }
                }

            }
            if (ischeked == false) {
                alert('항목을 먼저 선택해주십시오');
                return false;
            }
            if (confirm('선택된 티끌을 모두 삭제합니다 진행하시겠습니까?'))
                return true;

            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 0;		//LNB 1depth
</script>
	<!--CONTENTS-->
	<div id="contents">
		<!--article-->
		<div class="article" id="mypage">
			<ul id="tabMenu">
				<li><a href="/GlossaryMyPages/MyProfile.aspx">프로필</a></li>
				<li><a href="/GlossaryMyPages/MyDocumentsList.aspx">지식모음</a></li>
                
				<li><a href="/GlossaryMyPages/MyScrapList.aspx">스크랩한 지식</a></li>
                <li><a href="/GlossaryMyPages/MyTempList.aspx" class="on">임시저장 지식</a></li>
				<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx">스크랩한 담당자</a></li>
                <li><a href="/GlossaryMyPages/MyUseGroup.aspx">자주 사용하는 그룹</a></li>
				<!--<li><a href="/GlossaryMyPages/MyProfile.aspx">자주 사용하는 그룹</a></li>-->
			</ul>
			<div class="box_shadow">
				<h3 class="icon_title blue2"><%= UserName%>님의 티끌 모음</h3>
				<p class="btns_top">
					<a runat="server" id="MyModify" href="javascript:TebMenu('MyModify');" class="btn_icon icon1"><span>총 티끌<b><%= DisplayATikleCount %></b></span></a>
					<a runat="server" id="MyWrite" href="javascript:TebMenu('MyWrite');" class="btn_icon icon4"><span>작성한 티끌<b><%= DisplayWTikleCount %></b></span></a>
					<a runat="server" id="MyTemp" href="javascript:FnGoMyTemp();" class="btn_icon icon3"><span>임시 저장 티끌<b><%= DisplayTTikleCount%></b></span></a>
				</p>
				<div class="box_ct">
					<table class="listTable">
						<colgroup><col width="6%" /><col width="6%" /><col width="*" /><col width="9%" /></colgroup>
						<thead>
						<tr>
                            <th><input id="checkbox" onclick="return fnSelectAll(document.all.checkJob)" type="checkbox" value="checkbox" name="checkbox" onfocus="this.blur()" runat = "server"/></th>
							<th>No</th>
							<th>제목</th>
							<th>임시 저장일</th>
						</tr>
						</thead>
						<tbody>
                            <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound" >
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Literal id="itDelete" runat="server"></asp:Literal></td>
						                <td><asp:Literal runat="server" ID="Num" ></asp:Literal></Td>
						                <td style="display:none"><asp:Literal runat="server" ID="ltWiki" ></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span></div>
						                <td class="al">
                                        <a href="javascript:fnMyTempWrite('<%# DataBinder.Eval(Container.DataItem, "ID")%>', '<%# DataBinder.Eval(Container.DataItem, "CommonID")%>')">
                                          <%# DataBinder.Eval(Container.DataItem, "Title")%></a></td>
						                <td><%# DataBinder.Eval(Container.DataItem, "CreateDate")%></td>
                                    <tr>
                                </ItemTemplate>
                            </asp:Repeater>	 
						</tbody>
					</table>
					<p class="btn_r">
                        <asp:LinkButton ID="btnListDelete" CssClass="btn_box btn7" runat="server" OnClick="btnListDelete_Click" OnClientClick="return fnDeleteconfirm();">
                            <b>선택삭제</b>
                        </asp:LinkButton>
					</p>
					<p class="pagination">
						<Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
					</p>
				</div>
			</div>
		</div>
		<!--/article-->
	</div>
	<!--/CONTENTS-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
</asp:Content>


