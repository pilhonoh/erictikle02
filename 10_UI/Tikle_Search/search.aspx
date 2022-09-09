<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="search.aspx.cs" Inherits="main_search" MasterPageFile="search.master"%>
<%@ Register Src="./common/setParameter.ascx"   TagName="common_setParameter"   TagPrefix="header1" %>
<%@ Register Src="./common/searchForm.ascx"     TagName="common_searchForm"     TagPrefix="header2" %>
<%@ Register Src="./common/setUserName.ascx"   TagName="common_setUserName"   TagPrefix="header3" %>
<%-- Query 등록--%>
<%@ Register Src="./query/query_idea.ascx"       TagName="query_idea"       TagPrefix="query1" %>
<%-- Result 등록--%>
<%@ Register Src="./result/result_idea.ascx"       TagName="result_idea"       TagPrefix="result1" %>
<%@ Register Src="./result/noResult.ascx"          TagName="noResult"          TagPrefix="result6" %>
<%-- GNB 영역 시작--%>
<asp:Content ID="Content1" ContentPlaceHolderID="header1" Runat="Server">
    <header1:common_setParameter    ID="common_setParameter"    runat="server" />
  	<header3:common_setUserName     ID="common_setUserName"    runat="server" />
    <header2:common_searchForm      ID="searchForm"             runat="server" />
    <query1:query_idea              ID="query_idea"             runat="server" />
</asp:Content>
<%-- GNB 영역 끝--%>
<asp:Content ID="Content3" ContentPlaceHolderID="result" Runat="Server">
    <result1:result_idea      ID="result_idea"       runat="server" />
    <result6:noResult         ID="noResult"          runat="server" />
</asp:Content>

