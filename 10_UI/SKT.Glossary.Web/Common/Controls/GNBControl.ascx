<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GNBControl.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.GNBControl" %>
   
	<h2 class="alt-txt">메뉴</h2>
	<ul class="none" runat="server" id="LnbClick" >
		<li class="user"><a href="/GlossaryMyPages/MyProfile.aspx"><span class="user">내 프로필</span><span class="desc"><i>내 프로필 정보를 볼 수 있습니다.</i></span></a></li>
		<li class="archive"><a href="/GlossaryMyPages/MyDocumentsList.aspx"><span class="archive">내 티끌 모음</span><span class="desc"><i>내가 쓰고 편집한 티끌 목록을 볼 수 있습니다.</i></span></a></li>
		<li class="scrap"><a href="/GlossaryMyPages/MyScrapList.aspx"><span class="scrap">스크랩</span><span class="desc"><i>스크랩한 티끌들의 목록을 볼 수 있습니다.</i></span></a></li>
		<li class="share"><a href="/GlossaryMyPages/MyShareList.aspx"><span class="share">공유 티끌 모음</span><span class="desc"><i>다른 사용자로부터 공유 받은 티끌, 내가 공유 보낸 티끌을 볼 수 있습니다.</i></span></a></li>
		<li class="follow"><a href="/GlossaryMyPages/MyFollowList.aspx"><span class="follow">티끌 받아보기</span><span class="desc"><i>다른 사용자가 작성한 티끌을 업데이트 받아볼 수 있습니다.</i></span></a></li>
        <li class="qna"><a href="/QnA/QnAList.aspx"><span class="qna">질문/답변 게시판</span><span class="desc"><i>티끌에 없는 정보를 질문/답변할 수 있는 공간입니다.</i></span></a></li>
<%--		<li class="statics"><a href="javascript:alert('준비중입니다.');"><span class="statics">통계</span></a></li>--%>
<%--        <li class="qna"><a href="/TestBug/TestQnAList.aspx"><span class="qna">버그 게시판</span></a></li>--%>
<%--        <li class="setup"><a href="/GlossaryControl/GlossarySetupPage.aspx"><span class="setup">설정</span></a></li>--%>
        <li class="admin" id="gnbAdmin" runat="server"><a href="/GlossaryAdmin/GlossaryAdminStatTotal.aspx"><span class="admin">관리자</span><span class="desc"><i>관리자 전용 페이지</i></span></a></li>
    </ul>

