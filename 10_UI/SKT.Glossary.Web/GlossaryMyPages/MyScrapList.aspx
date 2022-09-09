<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.master" AutoEventWireup="true" CodeBehind="MyScrapList.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyScrapList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">
        var ThisTemp = null;
        var m_UserID = '<%= UserID %>';

        //뷰화면
        function fnMyScrapView(ItemID) {
            //            location.href = "/Glossary/GlossaryView.aspx?mode=MyScrap&ItemID=" + ItemID;

            // 2014-06-17 Mr.No
            var url = "/Glossary/GlossaryView.aspx?mode=MyScrap&ItemID=" + ItemID;
            url += ("&PrevListUrl=" + encodeURIComponent(location.pathname + "?PageNum=" + '<%= pager.CurrentIndex%>'));
            location.href = url;
        }

        //프로필 화면으로이동
        function fnProfileView(UserID) {
            location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
        }


        //        //알람 히든
        //        $(document).ready(function () {
        //            $(".list-alarm-outer").hide();
        //        });


        //알람 팝업창 오픈
        function fnAlarmOpen(thisTag, CommonID) {
            var AlarmYN = "N"
            if (thisTag.className == "btn_alarm on") {
                thisTag.className = "btn_alarm off";
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

                /*
             var oObj = document.all.checkJob;
             var ischeked = false;
             if (typeof (oObj) != "undefined" || oObj != null) {

                 
                 for (var i = 0; i < oObj.length; i++) {
                     if (oObj[i].checked == true) {
                         ischeked = true;
                     }
                 }

             }
             */

             
             if ($("input:checkbox[name=checkJob]:checked").length == 0) {
                 alert('항목을 먼저 선택해주십시오');
                 return false;
             }

             if (confirm('선택한 지식스크랩을 모두 삭제합니다. 진행하시겠습니까?')) {
                 return true;
             }

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
        <h2><img src="/common/images/text/Mypage.png" alt="마이페이지" /></h2>
		<!--article-->
		<div id="article">
			<ul id="tabMenu">
				<li><a href="/GlossaryMyPages/MyProfile.aspx"><img src="/common/images/btn/Mypage_tab1.png" alt="my 프로필" /></a></li>
				<li><a href="/GlossaryMyPages/MyDocumentsList.aspx"><img src="/common/images/btn/Mypage_tab2.png" alt="my 지식 스크랩" /></a></li>
				<li><a href="/GlossaryMyPages/MyScrapList.aspx" class="on"><img src="/common/images/btn/Mypage_tab3.png" alt="my 지식 스크랩" /></a></li>
				<%--<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx"><img src="/common/images/btn/Mypage_tab4.png" alt="my 담당자 스크랩" /></a></li>--%>
                <%--<li><a href="/GlossaryMyPages/MyUseGroup.aspx"><img src="/common/images/btn/Mypage_tab5.png" alt="my 그룹 " /></a></li>--%>
			</ul>
			<table class="listTable">
				<colgroup><col width="6%" /><col width="6%" /><col width="*" /><!--col width="9%" /--><col width="9%" /><col width="15%" /><col width="9%" /><col width="9%" /></colgroup>
				<thead>
				<tr>
					<th><input id ="checkbox" onclick="return fnSelectAll(document.all.checkJob)" type="checkbox" value="checkbox" name="checkbox" onfocus="this.blur()" runat = "server"/></th>
					<th>No</th>
					<th>제목</th>
					<!--th>최초 작성일<th-->
					<th>마지막 편집일</th>
					<th>마지막 편집자</th>
					<th>조회</th>
					<th>추천</th>
				</tr>
				</thead>
				<tbody>
                <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound" >
                    <ItemTemplate>
                        <tr>
                            <td><asp:Literal id="itDelete" runat="server"></asp:Literal></td>
						    <td><asp:Literal runat="server" ID="Num" ></asp:Literal></td>
						    <div class="types" style="display:none"><asp:Literal runat="server" ID="ltWiki" ></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span></div>
						    <td class="al"><asp:Literal runat="server" ID="litPermission"></asp:Literal>
                            <a href="javascript:fnMyScrapView('<%# DataBinder.Eval(Container.DataItem, "GlossaryID")%>')">
                                                        <%# DataBinder.Eval(Container.DataItem, "Title")%><asp:Literal runat="server" ID="litReply"></asp:Literal></a></td>
						    <!--td><%# DataBinder.Eval(Container.DataItem, "FirstCreateDate")%></td-->
						    <td><%# DataBinder.Eval(Container.DataItem, "LastCreateDate")%></td>
						    <td>
                                <asp:Literal ID="UserInfo" runat="server"></asp:Literal>
                                <!--<a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "YouUserID")%>');"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a>-->
                            </td>
						    <td><%# DataBinder.Eval(Container.DataItem, "Hits")%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "LikeCount")%></td>

                        </tr>
                    </ItemTemplate>
                </asp:Repeater>				
				</tbody>
			</table>
			<!--<p class="btn_r"><a href="javascript:alert('개발중입니다');" class="btn2"><b>선택 삭제하기</b></a></p>-->
			<p class="btn_r">
				<asp:LinkButton ID="btnListDelete" CssClass="btn2" runat="server" OnClick="btnListDelete_Click" OnClientClick="return fnDeleteconfirm();">
					<b>선택 삭제하기</b>
				</asp:LinkButton>
			</p>
			<p class="pagination">
				<Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
			</p>
		</div>
		<!--/article-->
	</div>
	<!--/CONTENTS-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
</asp:Content>


