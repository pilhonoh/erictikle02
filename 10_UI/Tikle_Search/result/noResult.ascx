<%@ Control Language="C#" AutoEventWireup="true" CodeFile="noResult.ascx.cs" Inherits="result_noResult" %>
<%
    if  (srchParam.CountTotal == 0){
%>
<div id="no_data">
		<dl>
			<dt>검색결과가 없습니다.</dt>
			<dd>- 단어의 철자가 정확한지 확인해 보세요.</dd>
			<dd>- 검색어의 단어 수를 줄이거나, 보다 일반적인 검색어로 다시 검색해 보세요.</dd>
		</dl>
	</div>

<%} %>