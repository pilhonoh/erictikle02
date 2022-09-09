<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="MyDocumentsList.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyDocumentsList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">
        var ThisTemp = null;
        var m_ReaderUserID = '<%= ReaderUserID %>';
    	var m_UserID = '<%= UserID %>';

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
    				    location.href = "/Glossary/GlossaryWriteNew.aspx?mode=MyTemp&ItemID=" + ItemID + "&CommonID=" + CommonID;
    				}
    			}
    		});
    	}

        //뷰화면 보기 
        function fnMyDocumentView(ItemID, HistoryYN) {
            //            if(HistoryYN == "Y"){
            //                location.href = "/Glossary/GlossaryView.aspx?mode=MyDocHistory&ItemID=" + ItemID;
            //            }
            //            else{
            //                
            //                location.href = "/Glossary/GlossaryView.aspx?mode=MyDoc&ItemID=" + ItemID;
            //            }
            //            location.href = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;

            // 2014-06-17 Mr.No
            var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
            url += ("&PrevListUrl=" + encodeURIComponent(location.pathname + "?PageNum=" + '<%= pager.CurrentIndex%>'));
            location.href = url;
        }

        //        //알람 팝업창 숨기기
        //        $(document).ready(function () {
        //            $(".list-alarm-outer").hide();

        //        });

        //알람 팝업창 오픈
        function fnAlarmOpen(thisTag, CommonID) {
            if (m_ReaderUserID == "") {
                var AlarmYN = "N"
                if (thisTag.className == "alarm-icon on") {
                    thisTag.className = "alarm-icon off";
                    AlarmYN = "N"

                } else {
                    thisTag.className = "alarm-icon on";
                    AlarmYN = "Y"
                }
                fnAlarmSave(AlarmYN, CommonID);

                //20131122 수정
                //            if(ThisTemp != thisTag){
                //$(".list-alarm-outer").hide();
                //                if($(thisTag)[0].nextSibling.nextSibling.style.display == "block")
                //                {
                //                    $(thisTag)[0].nextSibling.nextSibling.style.display = "none";
                //                }else{
                //                    $(thisTag)[0].nextSibling.nextSibling.style.display = "block";
                //                }
                //                ThisTemp = thisTag;      
                //            } 
            } else {
                alert("다른 사용자의 티끌 모음에서는 알림을 설정 할 수 없습니다.");
            }
        }


        //20131122 수정
        //알람 선택 후 저장
        function fnAlarmSave(AlarmYN, CommonID) {
            //            var MailSet = "N";
            //var NoteSet = "N";

            //            //이메일
            //            if ($(thisTag)[0].parentNode.children[0].children[0].children[0].checked == true)
            //                MailSet = "Y";

            //쪽지
            //            if ($(thisTag)[0].parentNode.children[0].children[1].children[0].checked ==true)
            //                NoteSet = "Y";     


            $.ajax({
                type: "POST",
                url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryAlarm",
                data: "{CommonID : '" + CommonID + "', UserID : '" + m_UserID + "', MailSet : 'N', NoteSet : '" + AlarmYN + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (success) {
                    if (AlarmYN == "Y") {
                        alert("쪽지 알림이 설정 되었습니다.");
                    } else {
                        alert("쪽지 알림이 해제 되었습니다.");
                    }

                    //                     $(".list-alarm-outer").hide();
                    //                    
                    //                    if(MailSet=="Y" || NoteSet=="Y")
                    //                    {
                    //                        $(thisTag)[0].parentNode.parentNode.parentNode.children[0].className = "alarm-icon on";

                    //                    }else {
                    //                        $(thisTag)[0].parentNode.parentNode.parentNode.children[0].className = "alarm-icon off";
                    //                    }


                }
            });
        }

        //탭 메뉴
        function TebMenu(TebMenu) {
            $('#<%= this.hidMenuType.ClientID %>').val(TebMenu);
            var PostBackStr = "<%=Page.GetPostBackEventReference(this.btnTebMenu)%>";
            eval(PostBackStr);
        }

        //프로필 화면 이동
        function fnMyProfileView(UserID) {
            location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
        }

        function FnGoMyTemp() {
            alert("개발중입니다");
            //location.href = "/GlossaryMyPages/MyTempList.aspx";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 0;		//LNB 1depth
</script>
<div id="container" class="Mypage">
	<!--CONTENTS-->
	<div id="contents">
        <% if(bOwn) { %>
		<h2><img src="/common/images/text/Mypage.png" alt="마이페이지" /></h2>
        <% } else { %>
        <h2><img src="/common/images/text/Mypage_.png" alt="인물정보" /></h2>
        <% } %>
		<!--article-->
		<div id="article">
			<ul id="tabMenu">
                <% if(bOwn) { %>
				<li><a href="/GlossaryMyPages/MyProfile.aspx?UserID=<%= ReaderUserID %>"><img src="/common/images/btn/Mypage_tab1.png" alt="my 프로필" /></a></li>
				<li><a href="/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=<%= ReaderUserID %>"  class="on"><img src="/common/images/btn/Mypage_tab2.png" alt="my 지식 스크랩" /></a></li>
                <li><a href="/GlossaryMyPages/MyScrapList.aspx"><img src="/common/images/btn/Mypage_tab3.png" alt="my 지식 스크랩" /></a></li>
				<%--<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx"><img src="/common/images/btn/Mypage_tab4.png" alt="my 담당자 스크랩" /></a></li>--%>
                <%--<li><a href="/GlossaryMyPages/MyUseGroup.aspx"><img src="/common/images/btn/Mypage_tab5.png" alt="my 그룹 " /></a></li>--%>
                <% } else { %>
					<li><a href="/GlossaryMyPages/MyProfile.aspx?UserID=<%= ReaderUserID %>"><img src="/common/images/btn/Mypage_tab1_.png" alt="프로필" /></a></li>
				<% } %>
			</ul>
            <p class="btns_top" style="display:none">
				<a runat="server" id="MyModify" href="javascript:TebMenu('MyModify');" class="btn_icon icon1"><span>총 티끌<b><%= DisplayATikleCount %></b></span></a>
				<a runat="server" id="MyWrite" href="javascript:TebMenu('MyWrite');" class="btn_icon icon4"><span>작성한 티끌<b><%= DisplayWTikleCount %></b></span></a>
				<a runat="server" id="MyTemp" href="javascript:FnGoMyTemp();" class="btn_icon icon3"><span>임시 저장 티끌<b><%= DisplayTTikleCount%></b></span></a>
			</p>
			<table class="listTable">
				<colgroup><col width="6%" /><col width="*" /><!--col width="9%" /--><col width="9%" /><col width="15%" /><col width="9%" /><col width="9%" /></colgroup>
				<thead>
				<tr>
					<th>No</th>
					<th>제목</th>
					<th>임시 저장일</th>
					<!--th>최초 작성일</th>
					<th>마지막 편집일</th>
					<th>마지막 편집자</th>
					<th>조회</th>
					<th>추천</th-->
				</tr>
				</thead>
				<tbody>
                    <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound" >
                        <ItemTemplate>
                            <tr>
							    <td><asp:Literal runat="server" ID="Num" ></asp:Literal></td>
                                <div class="types" style="display:none"><asp:Literal runat="server" ID="ltWiki" ></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span></div>
							    <td class="al">
                                    <asp:Literal runat="server" ID="litPermission"></asp:Literal>
                                    <a href="javascript:fnMyTempWrite('<%# DataBinder.Eval(Container.DataItem, "ID")%>', '<%# DataBinder.Eval(Container.DataItem, "CommonID")%>')">
                                                        <%# DataBinder.Eval(Container.DataItem, "Title")%><asp:Literal runat="server" ID="litReply"></asp:Literal></a>

							    </td>
								 <td><%# DataBinder.Eval(Container.DataItem, "FirstCreateDate")%></td>
								<asp:Literal runat="server" ID="litName"></asp:Literal>
                                    <asp:Literal runat="server" ID="liticon"></asp:Literal>
							   <%-- <!--td><%# DataBinder.Eval(Container.DataItem, "FirstCreateDate")%></td-->
							    <td><%# DataBinder.Eval(Container.DataItem, "LastCreateDate")%></td>
							    <td><asp:Literal runat="server" ID="litName"></asp:Literal>
                                    <asp:Literal runat="server" ID="liticon"></asp:Literal>
							    </td>
							    <td><%# DataBinder.Eval(Container.DataItem, "Hits")%></td>
							    <td><%# DataBinder.Eval(Container.DataItem, "LikeCount")%></td>
							    <td style="display:none">
                                    <div class="alarms">
                                        <asp:Literal ID="litAlarmYN" runat="server"></asp:Literal>
				                        <div id="alarm-view-10-layer" class="list-alarm-outer" style="display:none;">
					                        <div class="list-alarm-inner">
						                        <ul>
							                        <li><asp:Literal ID="litEmail" runat="server"></asp:Literal><label for="email">이메일 알림 받기</label></li>
                                                    <li><asp:Literal ID="litNote" runat="server"></asp:Literal><label for="nateon">네이트온 알림 받기</label></li>
						                        </ul>
						                        <a href="javascript:" class="_btn_b_s" onclick="fnAlarmSave(this, '<%# DataBinder.Eval(Container.DataItem, "CommonID")%>');"><span>확인</span></a>
					                        </div>
				                        </div>
                                    </div>
							    </td>--%>
                            <tr>
                        </ItemTemplate>
                    </asp:Repeater>	 
				</tbody>
			</table>
			<p class="pagination">
				<Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
			</p>
		</div>
		<!--/article-->
	</div>
	<!--/CONTENTS-->
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
        <div style="display:none;">
            <asp:HiddenField ID="hidMenuType" runat="server" />     
            <asp:Button ID="btnTebMenu" runat="server" OnClick="btnTebMenu_Click"  />
            <asp:HiddenField ID="hdIsLoginUser" runat="server" />
        </div>
</asp:Content>
