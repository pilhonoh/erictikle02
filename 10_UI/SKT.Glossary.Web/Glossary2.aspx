<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Glossary2.aspx.cs" Inherits="SKT.Glossary.Web.Glossary2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge" charset="utf-8"/>
    <title></title>
    <script src="/Common/js/jquery-1.11.1/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //벨리데이션체크
        function fn_validation() {
            if ($("#<%=txtID.ClientID%>").val().trim() == "") {
                alert("사번을 입력하세요.");
                return false;
            }

            return true;
        }

        $(function () {
            //텍스트 박스 이벤트 바인딩
            $("#<%=txtID.ClientID%>").keyup(function () {
                if (event.keyCode == 13) {
                    if (fn_validation()) {
                        location.href = $("#<%=btnLogin.ClientID%>").attr("href");
                        return false;
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <% if (System.Environment.MachineName.Equals("SKT-TNCDALL1")) { %>
        <h2>T.끌 DEV – Test로그인</h2>
        <% }  else if (System.Environment.MachineName.Equals("SKT-TNCNPWEB1") || System.Environment.MachineName.Equals("SKT-TNCNPWEB2")) { %>
        <h2>T.끌 – Test로그인</h2>
        <% } else { %>
        <h2>T.끌 Local – Test로그인</h2>
        <% } %>
        <asp:TextBox runat="server" ID="txtID"></asp:TextBox>
        <asp:Button runat="server" ID="btnLogin" Text="로그인" OnClientClick="return fn_validation();" OnClick="btnLogin_Click" /><br />
    </div>
    </form>
</body>
</html>

