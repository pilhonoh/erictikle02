<%@ Page  Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="MyPeopleScrapList.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyPeopleScrapList" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">
        var ThisTemp = null;
        var m_UserID = "";

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

            if (confirm('선택한 담당자스크랩을 모두 삭제합니다. 진행하시겠습니까?')){
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
				<li><a href="/GlossaryMyPages/MyScrapList.aspx"><img src="/common/images/btn/Mypage_tab3.png" alt="my 지식 스크랩" /></a></li>
				<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx" class="on"><img src="/common/images/btn/Mypage_tab4.png" alt="my 담당자 스크랩" /></a></li>
                <li><a href="/GlossaryMyPages/MyUseGroup.aspx"><img src="/common/images/btn/Mypage_tab5.png" alt="my 그룹 " /></a></li>
			</ul>
			<ul id="pm_list">
			    <asp:Repeater ID="rptPeople" runat="server" OnItemDataBound="rptPeople_OnItemDataBound" >
				    <ItemTemplate>
						<li>
                            <div id="my_profile" style="padding-right:0; ">
                                <!--
                                Author : 개발자-김성환D, 리뷰자-이정선G
                                Create Date : 2016.02.17 
                                Desc : 스크랩 범위 넘어가는 부분처리
                                -->
							    <dl  style="overflow-y:scroll; height:230px; position:relative;">
                                    <dt class="scrap"><asp:Literal runat="server" ID="litKoreanName" ></asp:Literal> / <asp:Literal runat="server" ID="litPositionName" ></asp:Literal></dt>
                                    <dd><span style="left:0;">소속</span>: <asp:Literal runat="server" ID="litSosok2" ></asp:Literal></dd>
								    <dd><span style="left:0;">연락</span>: <asp:Literal runat="server" ID="litTelephoneNumber" ></asp:Literal><asp:Literal runat="server" ID="litMobile" ></asp:Literal></dd>
								    <dd><span style="left:0;">이메일</span>: <asp:Literal runat="server" ID="litMail" ></asp:Literal></dd>
								    <dd><span style="left:0;">담당업무</span>: <asp:Literal runat="server" ID="litJobDescription" ></asp:Literal></dd>
							    </dl>
							    <p><img src="<%# DataBinder.Eval(Container.DataItem, "PhotoURL")%>" alt="" /></p>
                                <input type="checkbox" name="checkJob" title="선택" value="<%# DataBinder.Eval(Container.DataItem, "ID")%>" />
							    <!--a href="javascript:alert('개발중');" class="btn1 btn_pop"><span>쪽지보내기</span></a-->
                            </div>
						</li>
                    </ItemTemplate>
			    </asp:Repeater>
			</ul>
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



