<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DirectoryFileHistoryList.aspx.cs" Inherits="SKT.Glossary.Web.Directory.DirectoryFileHistoryList" %>

<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<title>T.끌, 소통과 협업의 플랫폼</title>
<link rel="stylesheet" href="/common/css/default.css" />
<link rel="stylesheet" href="/common/css/sub.css" />
<!--[if lt IE 9]>
<script src="/common/js/html5shiv.js"></script>
<![endif]-->
<%--<script src="/common/js/jquery-1.7.2.min.js"></script>
<script src="/common/js/design.js"></script>
<script src="/common/js/select.js"></script>--%>

    <script src="/common/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/common/js/design.js" type="text/javascript"></script>
    <script src="/common/js/select.js" type="text/javascript"></script>
    <script src="/common/js/jquery.filestyle.js" type="text/javascript"></script>
    <script src="/Common/js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>
    <script src="/common/js/lrscroll.js"></script>


    <script type="text/javascript">

        function fnMyConfrim(url) {

            //&tikle=1
            var fileurl = "FileOpenTransfer.aspx?file=" +  escape(url) + "&tikle=2";
            //var fileurl = "FileOpenTransfer.aspx?file=" + url + "&tikle=2";
            //alert(fileurl);
            var win = window.open(fileurl, "_blank", "left=10, top=10, width=10, height=10, toolbar=no, menubar=no, scrollbars=yes, resizable=no");

        }


    </script>

    <script type="text/javascript">
        var lnbDep1 = 1;		//LNB 1depth
    </script>


</head>
<body>
    <form id="form1" runat="server">
       
        	    <div id="group_list">
					<table class="listTable">
						<colgroup><col width="20%" /><col width="*" /></colgroup>
						<thead>
						<tr>
							<th>버전</th>
							<th>수정자</th>
                            <th>URL</th>
						</tr>
						</thead>
						<tbody>
						<tr id="NoData" runat="server">
							<td colspan="3">파일이 없습니다.</td>
						</tr>
                        <asp:Repeater ID="rptFile" runat="server" OnItemDataBound="rptFile_OnItemDataBound" >
                            <ItemTemplate>
                            <tr>
                                <td style="width:80px;"><%# DataBinder.Eval(Container.DataItem, "VERSION_NO")%></td>
                                <td style="width:160px;"><%# DataBinder.Eval(Container.DataItem, "EDITOR")%></td>
                               
                                <%-- <td style="width:80px;"><a href="javascript:fnMyConfrim('<%# DataBinder.Eval(Container.DataItem, "EDIT_URL")%>')" class="btn_s"><b>확인</b></a></td>--%>

                                <td style="width:80px;"><asp:Literal ID="ilConfrim" runat="server"></asp:Literal></td>

                            </tr> 
                            </ItemTemplate>
                        </asp:Repeater>
						</tbody>
					</table>
				</div>
    </form>
</body>

</html>