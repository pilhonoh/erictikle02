<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="HistoryView.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryControl.HistoryView" %>

<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
        <script language="javascript" type="text/javascript">
            var m_ItemID = '<%= ItemID %>';

            //되돌리기 버튼
            function fnRevert() {
                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/HistoryModifyYN",
                    data: "{ItemID : '" + m_ItemID + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (ModifyYNTableCnt) {
                        if (ModifyYNTableCnt.d == "0") {
                            alert("수정 중인 문서 입니다.");
                            return false;
                        } else {
                            var PostBackStr = "<%=Page.GetPostBackEventReference(this.btnRevert)%>";
                            eval(PostBackStr);
                        }
                    }
                });
            }

            //피로파일 페이지 이동
            function fnProfileView(UserID) {
                location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
            }


            $(document).ready(function () {
                $("#divpop").hide();
                $("#pop_alert").hide();
            });

            function fnClose() {
                $("#<%= this.DescriptionTemp.ClientID %>").val("");
                $("#divpop").hide()
                $("#pop_back").hide()
            }

            function fnRevetOpen() {

                $("#<%= this.DescriptionTemp.ClientID %>").val("");
                $("#divpop").show()
                $("#pop_back").show()
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
    <!-- roll back -->
<div id="divpop" class="pop">
	<div class="popBg"></div>
	<!--지식_되돌리기-->
	<div id="pop_back" class="layer_pop">
		<h3>되돌리기를 선택하셨습니다.<br />사유를 작성해주세요</h3>
		<div><input type="text" runat="server" id="DescriptionTemp" class="txt t6" /></div>
		<p class="btn_c">
			<a href="javascript:fnClose();" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnRevert();" class="btn3"><b>되돌리기</b></a> 
		</p>
		<input type="image" src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="fnClose(); return false;" />
	</div>
	<!--/지식_되돌리기-->
</div>

	<div id="contents">
		<h2>
            <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType))  { %>
                <img src="/common/images/text/Glossary.png" alt="끌.지식" />
            <% } %>
		</h2>
		<p class="btn_top">
            <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType))  { %>
            <a href="/Glossary/GlossaryWriteNew.aspx"><img src="/common/images/btn/write.png" alt="글쓰기" /></a>
            <% } %>
        </p>
		<div id="article">
			<!--편집 비교 상세-->
			<div id="edit_view">
				<!--상세보기-->
				<div id="edit_view_prev">
					<h3><asp:Literal ID="litTitleList" runat="server" Visible="false"></asp:Literal><asp:Label ID="lbTitle" runat="server"></asp:Label></h3>
					<p class="view_writer">
                        <asp:Literal ID="litWriter" runat="server"></asp:Literal>
                       <%-- <a href=""><b>송일섭/Values교육팀</b></a> 님이 <b class="point_red">2013.09.26 16:00</b>에 작성--%>
					</p>
					<div class="view_ct">
						<asp:Label ID="txtBody" runat="server"></asp:Label>
					</div>
				</div>
				<!--/상세보기-->
				<!--상세보기-->
				<div id="edit_view_next">
					<h3><asp:Literal ID="litOldTitleList" runat="server" Visible="false"></asp:Literal><asp:Label ID="lbOldTitle" runat="server"></asp:Label></h3>
					<p class="view_writer">
                        <asp:Literal ID="litOldWriter" runat="server"></asp:Literal>
                        <%--<a href=""><b>송일섭/Values교육팀</b></a> 님이 <b class="point_red">2013.09.26 16:00</b>에 작성--%>
					</p>
					<a href="javascript:fnRevetOpen();" class="btn_back"><img src="/common/images/btn/back.png" alt="" title="되돌리기" /></a>
					<div class="view_ct">
						<asp:Label ID="txtOldBody" runat="server"></asp:Label>
					</div>
				</div>
				<!--/상세보기-->
			</div>
			<!--/편집 비교 상세-->
		</div>
	</div>
	<!--/CONTENTS-->

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <div style ="display:none";>
        <asp:Button ID="btnRevert" runat="server" style="display:none" OnClick="btnRevert_Click" />
             <asp:HiddenField ID="hidRevertID" runat="server" />
    </div>
</asp:Content>
