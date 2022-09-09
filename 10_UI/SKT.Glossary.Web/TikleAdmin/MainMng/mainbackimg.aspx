<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="mainbackimg.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.MainMng.mainbackimg" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>메인이미지 관리</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
<br />
<div class="adminGuideBox">
	<dl>
		<dt><strong>* 메인배경이미지 업로드</strong></dt>
		<dd>
            <table>
                <tr>
                    <td>
                        <asp:FileUpload id="fileMainimg" runat="server" Height="24px"  />
                    </td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" Text="업로드" OnClick="btnSubmit_Click"  Height="24px"  /> 
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;<b>파일명 : bg.gif (파일 size : 1900 * 385)</b>
                    </td>
                </tr>
            </table>
		</dd>
	</dl>
</div>
<br />
<div class="adminGuideBox" >
	<dl>
		<dt><strong>* 예정 배경화면</strong>
            <br />
            <font style="color:red; font-weight:bold;">[적용하면 운영 메인 배경화면으로 적용 됩니다.]</font><asp:Button ID="btnMove" runat="server" Text="적용하기" OnClick="btnMove_Click"  Height="24px" />
            
		</dt>
		<dd>
            <img src="/SKT_MultiUploadedFiles/Glossary/tiklemain/bg_after.gif" border="0" width="1600" height="385" alt=""/>
		</dd>
	</dl>
</div><br />
<div class="adminGuideBox" >
	<dl>
		<dt><strong>* 운영 배경화면</strong></dt>
		<dd>
            <img src="/SKT_MultiUploadedFiles/Glossary/tiklemain/bg.gif" border="0" width="1600" height="385" alt=""/>
		</dd>
	</dl>
</div>
              
</asp:Content>