<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageTransfer.aspx.cs" Inherits="SKT.Glossary.Web.Common.Controls.MessageTransfer" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>

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

         $(document).ready(function () {

             //var a = "Paper";
             //var a = "Mail";
             //var a = "SMS";

             var m_RdoType = '<%= RdoType %>';

             //라디오버튼 결정 
             setSelectRadioCheck('rBtnA', m_RdoType);

             TypeValCheck();

     
             var m_PageType = '<%= PageType %>';
             var m_UserID = '<%= UserID %>';
             var m_First = '<%= FirID %>';
             var m_Second = '<%= SecID %>'; 

       
             try {
                 $.ajax({
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
                     data: "{Type : '" + m_PageType + "', UserID : '" + m_UserID + "', MyGrpID : '" + m_First + "'}",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         if (data.d[0].length != 0) {

                             //alert("[" + data.d + "]");

                             DefaultClear();
                             DefaultSetting("[" + data.d + "]");
                         }

                     },
                     error: function (result) {
                         alert("Error DB Check");
                     }
                 });
             }
             catch (exception) {
                 alert('Exception error' + exception.toString());
             }

              
         });


         function setSelectRadioCheck(rdoName, rdoValue) {
             $('input:radio[name="' + rdoName + '"]').each(function () {
                 $(this).removeAttr("checked");
             });
             var selectedItem = $('input:radio[name="' + rdoName + '"][value="' + rdoValue + '"]');
             selectedItem.attr('checked', 'checked');
             selectedItem.prop('checked', true);
         } 

         function TypeValCheck() {

             var typeValue = $(":input:radio[name=rBtnA]:checked").val();

             //alert(typeValue); 
 
             // 쪽지  
             if (typeValue == "Paper") {
                 $('#<%= this.hdType.ClientID %>').val(typeValue);
             }
             else if (typeValue == "Mail") {
                 $('#<%= this.hdType.ClientID %>').val(typeValue);
             }
             else {
                 $('#<%= this.hdType.ClientID %>').val(typeValue);
             }

         }


         //저장  
         function fnSave() {

             //IsEditMode = false;

             //조직도 
             fnShareSave();

             //저장 
             __doPostBack('<%=btnSave.UniqueID %>', '');
         }



         //파일오픈 
         function fileOpen(EncryptUrl) {

             //alert(EncryptUrl);
             //잠시주석  
             //var url = "FileOpenTransfer.aspx?file=" + dir + "/" + file;

             var win = window.open(EncryptUrl, "_blank", "left=10, top=10, width=10, height=10, toolbar=no, menubar=no, scrollbars=yes, resizable=no");
         }



         //조직도에서 리턴되어 온값을 저장 삭제    
         

     </script>

    <script type="text/javascript">
        var lnbDep1 = 1;		//LNB 1depth
</script>


</head>
<body>
    <form id="form1" runat="server">

   <!--메시지보내기-->
<div class="popCt" id="pop_message">
	<table class="popTable">
		<colgroup><col width="20%" /><col width="*" /></colgroup>
		<tr>
			<th>선택</th>
			<td>
				<input type="radio" name="rBtnA" id="rdoPaper" value="Paper"  onclick="TypeValCheck()" /> <label for="">쪽지</label>
				<input type="radio" name="rBtnA" id="rdoMail"  value="Mail"  onclick="TypeValCheck()" /> <label for="">메일</label>
				<input type="radio" name="rBtnA" id="rdoSMS"  value="SMS"   onclick="TypeValCheck()" /> <label for="">SMS</label>
			</td>
		</tr>
		<tr>
			<th>제목</th>
	<%--		<td><input type="text" name=""  class="txt t1" /></td>--%>
            <td><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t1" /></td>
		</tr>
		<tr>
			<th>내용</th>
			<td><textarea id="msgContents" cols="4" rows="1" runat="server" class="txta1"></textarea></td>
		</tr>
		<tr>
			<th>메시지를<br />받을<br />구성원</th>
			<td>
				<div class="select_name">
                    <common:UserAndDepartment ID="UserControl" runat="server" />
				</div>
			</td>
		</tr>
	</table>
	<p class="btn_c"><a href="javascript:///" class="btn_orange2"  onclick="fnSave()"><b>저장</b></a></p>
	<input type="image" src="/common/images/pop/btn_close.png" title="닫기" class="close" />
</div>
<!--/메시지보내기-->

 <div style="display:none;">
        
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            <asp:HiddenField ID="hdDirectoryID" runat="server" />
            <asp:HiddenField ID="hdFileID" runat="server" />
            <asp:HiddenField ID="hdType" runat="server" />
<%--            <asp:HiddenField ID="hdUserItemID" runat="server" />
            <asp:HiddenField ID="hdUserName" runat="server" />--%>
    </div>

    </form>
</body>

    
</html>
