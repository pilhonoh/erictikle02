<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="TikleAccess.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.AccessAuth.TikleAccess" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript">

        function DelUser(obj)
        {
            var arrTemp = obj.id.split('_');
            if(arrTemp[2] == "S003331")
            {
                alert("티끌이는 삭제할 수 없습니다.");
            }
            else {

                if(confirm("삭제하시겠습니까?"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        function AddUser()
        {


        }

    </script>
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>Tikle 접속권한</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
<br />
<div style="margin-top:30px;">
	<dl>
        <dt>
            <div class="TableStyleBox" style="width:800px;">
            <table cellpadding="0" cellspacing="0" class="TableStyle" style="width:800px;">
                <tr>
	            <th style="width:60px;">아이디</th>
                <td><input type="text" id="UserID" maxlength="10" /></td>
	            <th style="width:60px;">이름</th>
                <td><input type="text" id="UserNM" /></td>
                <th style="width:60px;">레벨</th>
                <td><select id="selLevel">
                    <option value="3">관리자</option>
                    <option value="9">예외접속</option>
                    </select></td>
                <td align="center" style="width:70px;"><input type="button" id="btnSave" value="저장" style="font-size:10px; padding:0;" onclick="AddUser();"/></td>
                </tr>
            </table>
        </dt>
        <dt><br /><br /></dt>
		<dt>
			<div class="TableStyleBox" style="width:800px;">
                <table cellpadding="0" cellspacing="0" class="TableStyle" style="width:800px;">
	            <tr>
		            <th align="center"><strong>등록일</strong></th>
		            <th align="center"><strong>사번</strong></th>
		            <th align="center"><strong>이름</strong></th>
		            <th align="center"><strong>레벨</strong></th>
                    <th align="center">&nbsp;</th>
	            </tr>
                <asp:Repeater ID="rptmember" runat="server" >
                <ItemTemplate>
                    <tr>
		            <td align="center"><%# DataBinder.Eval(Container.DataItem, "CREATEDATE")%></td>
		            <td align="center"><%# DataBinder.Eval(Container.DataItem, "USERID")%></td>
		            <td align="center"><%# DataBinder.Eval(Container.DataItem, "NAME")%></td>
		            <td align="center"><%# DataBinder.Eval(Container.DataItem, "LEVEL_NAME")%></td>
                    <td align="center"><input type="button" id="btn_<%# DataBinder.Eval(Container.DataItem, "ID")%>_<%# DataBinder.Eval(Container.DataItem, "USERID")%>" value="삭제" style="font-size:10px; padding:0;" onclick="DelUser(this);"/></td>
	            </tr>
                </ItemTemplate>
                </asp:Repeater>
                </table>
            </div>
        </dt>
    </dl>
</div>
</asp:Content>