<%@ Control Language="C#" AutoEventWireup="true" CodeFile="setUserName.ascx.cs" Inherits="common_setUserName" %>
 <div class="userInfo">
<p><a href="#myMenu"><%=MyMenuUserName%></a></p>
<ul id="myMenu">
	<li><a href="/GlossaryMyPages/MyProfile.aspx" class="my1">My 프로필</a></li>
	<li><a href="/GlossaryMyPages/MyDocumentsList.aspx" class="my2">작성 중인 지식</a></li>
	<li><a href="/GlossaryMyPages/MyScrapList.aspx" class="my3">지식스크랩</a></li>
	<%--<li><a href="/GlossaryMyPages/MyUseGroup.aspx" class="my5">My 그룹</a></li>--%>
	<li><a href="/TikleAdmin/Stats/tikleTotal.aspx" class="my6">관리자모드</a></li>
</ul>
</div>