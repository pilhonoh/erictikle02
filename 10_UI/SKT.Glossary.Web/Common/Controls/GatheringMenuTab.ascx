<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatheringMenuTab.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.GatheringMenuTab" %>

<ul id="tabMenu">
    <li><a href="/Gathering/GatheringMain.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=<%=GI_GatheringID%>" <%=m_Glossary%>>
        <img src="/common/images/btn/Gathering_tab1_1_2.png" alt="모임.게시판" title="모임.게시판" /></a></li>
    <li><a href="/Directory/DirectoryListNew.aspx?GatheringYN=Y&GatheringID=<%=GI_GatheringID%>&MenuType=Directory" <%=m_Directory%>>
        <img src="/common/images/btn/Gathering_tab1_2.png" alt="모임.문서" title="모임.문서" /></a></li>
    <li><a href="/Schedule/ScheduleNew.aspx?GatheringYN=Y&GatheringID=<%=GI_GatheringID%>&MenuType=Schedule" <%=m_Schedule%>>
        <img src="/common/images/btn/Gathering_tab1_3.png" alt="모임.일정" title="모임.일정" /></a></li>
</ul>