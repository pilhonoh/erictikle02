<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SKT.Glossary.Web.Error" %>
<html lang="ko">
<head>
	<meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>티끌</title>
</head>
<body style="background-image:none">  
<form runat="server">
    <div id="container" style=" position:absolute;top:45%;left:50%;width:800px;height:450px;margin-top:-205px;margin-left:-345px;">
	    <!--CONTENTS-->
	    <div id="contents" >
            <table>
                <tr>
                    <td>
                        <img src="/Common/Images/etc/error_page.png" style="height:300px;width:300px" />
                    </td>
                    <td>
                        <img alt="티:끌" src="/common/images/etc/logo.png" /><br /><br />
                        <dt style='padding-bottom:0px;font-family: "맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;color: rgb(51, 51, 51);font-size:18px'>
                            <asp:Literal ID="litPreUrl" runat="server" Visible="false"></asp:Literal>
                            <asp:Literal ID="litErrorDetail" runat="server" Visible="false"></asp:Literal>
                            <asp:Literal ID="litUserDetail" runat="server"></asp:Literal>
                            <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
                        <dt>
                        <% if( ErrCode != "100") { %>
                        <dd style='font-family: "맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;color: rgb(51, 51, 51);font-size:12px;'><br />현재 요청하신 페이지를 정상적으로 보여줄 수 없습니다.</dd>
                        <dd style='font-family: "맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;color: rgb(51, 51, 51);font-size:12px;'>입력하신 주소가 잘못 되었거나, 서버의 내부 오류일 수 있습니다.</dd>
                        <% } %>
                        <dd style='font-family: "맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;color: rgb(51, 51, 51);font-size:12px'>
                            <br />
                            <a href="/TikleMain.aspx" >[홈으로가기]</a>
                        </dd>
                    </td>
                </tr>
            </table>
	        <!--검색결과 없음-->
	        <div id="no_data">
		        <dl>
		        </dl>
	        </div>
        </div>
    </div>
</form>
</body>
</html>
