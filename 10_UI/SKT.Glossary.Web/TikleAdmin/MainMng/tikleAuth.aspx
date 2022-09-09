<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="tikleAuth.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.MainMng.tikleAuth" %>
<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" src="/OrgChart/common/orgchart.js"></script>
<script type="text/javascript">
    $(document).ready(function () {

        $("#<%= this.txtEmpNo.ClientID %>").val("");
        $("#<%= this.txtEmpNm.ClientID %>").val("");
        $("#<%= this.hidDeleteEmpNo.ClientID %>").val("");

        $("#<%= this.cbAuth1.ClientID %>").prop("checked", false);
        $("#<%= this.cbAuth2.ClientID %>").prop("checked", false);
        $("#<%= this.cbAuth3.ClientID %>").prop("checked", false);
    });

    function fn_SearchUser() {
        function callback(data) {
            var orgchartObj = {};
            if (typeof (data) == "string" && data.length > 0) {
                orgchartObj = eval(data);

                if (orgchartObj) {
                    for (var i = 0; i < orgchartObj.length; i++) {
                        if (orgchartObj[i].EmpID) {
                            // 사원
                            OrgChartUser(orgchartObj[i].UserName, orgchartObj[i].DeptName, orgchartObj[i].EmpID);
                        } 
                    }
                }
            }
        }
        om_OpenOrgChart({
            callback: callback,
            selectGroup:true,modal:false,oneSelect:true,selectCompany:true,appType:'user'
        });
    }
    function OrgChartUser(UserName, DeptName, EmpID) {

        if (EmpID.substring(0, 2) == "10" || EmpID.substring(0, 2) == "11")
        {
            alert("정규사번은 전체권한을 가지고 있습니다.");
            return false;
        }

        $("#<%= this.cbAuth1.ClientID %>").prop("checked", false);
        $("#<%= this.cbAuth2.ClientID %>").prop("checked", false);
        $("#<%= this.cbAuth3.ClientID %>").prop("checked", false);

        var regType1 = /^[A-Za-z+]*$/;
        if (EmpID.substring(0, 2) == "15")
        {
            $("#<%= this.cbAuth1.ClientID %>").attr("style", "visibility:hidden;");
            $("#<%= this.cbAuth2.ClientID %>").attr("style", "visibility:none;");
            $("#<%= this.cbAuth3.ClientID %>").attr("style", "visibility:hidden;");
        }
        else if (EmpID.substring(0, 2) == "19")
        {
            $("#<%= this.cbAuth1.ClientID %>").attr("style", "visibility:none;");
            $("#<%= this.cbAuth2.ClientID %>").attr("style", "visibility:none;");
            $("#<%= this.cbAuth3.ClientID %>").attr("style", "visibility:hidden;");
        }
        else if (!regType1.test(EmpID.substring(0, 1)))
        {
            $("#<%= this.cbAuth1.ClientID %>").attr("style", "visibility:none;");
            $("#<%= this.cbAuth2.ClientID %>").attr("style", "visibility:none;");
            $("#<%= this.cbAuth3.ClientID %>").attr("style", "visibility:none;");
        }
        $("#<%= this.txtEmpNo.ClientID %>").val(EmpID);
        $("#<%= this.txtEmpNm.ClientID %>").val(UserName);
    }

    function AddUser()
    {
        var empno = $("#<%= this.txtEmpNo.ClientID %>").val();

        if(empno == "")
        {
            alert("구성원을 선택하여 주세요.");
            return false;
        }
        else if (empno.substring(0, 2) == "15" && $("#<%= this.cbAuth2.ClientID %>").prop("checked") == false)
        {
           alert("추가권한을 선택하여 주세요.");
           return false;
        }
        else if (empno.substring(0, 2) == "19" && $("#<%= this.cbAuth1.ClientID %>").prop("checked") == false && $("#<%= this.cbAuth2.ClientID %>").prop("checked") == false)
        {
            alert("추가권한을 선택하여 주세요.");
            return false;
        }
        else if(
            $("#<%= this.cbAuth1.ClientID %>").prop("checked") == false && 
            $("#<%= this.cbAuth2.ClientID %>").prop("checked") == false &&
            $("#<%= this.cbAuth3.ClientID %>").prop("checked") == false)
        {
            alert("추가권한을 선택하여 주세요.");
            return false;
        }
        <%=Page.GetPostBackEventReference(hidSave) %>;
    }
    
    function DelUser(userid)
    {
        if(confirm("삭제하시겠습니까?"))
        {
            $("#<%= this.hidDeleteEmpNo.ClientID %>").val(userid);
            <%=Page.GetPostBackEventReference(hidDelete) %>;
        }
    }
    function chkAuth1()
    {
        var empno = $("#<%= this.txtEmpNo.ClientID %>").val();

        if (empno.substring(0, 2) != "15")
        {
            if($("#<%= this.cbAuth1.ClientID %>").prop("checked") == false)
            {
                $("#<%= this.cbAuth2.ClientID %>").prop("checked", false);
            }
        }
    }
    function chkAuth2()
    {
        var empno = $("#<%= this.txtEmpNo.ClientID %>").val();

        if (empno.substring(0, 2) != "15")
        {
            if($("#<%= this.cbAuth2.ClientID %>").prop("checked") == true)
            {
                $("#<%= this.cbAuth1.ClientID %>").prop("checked", true);
            }
        }
    }
    function SearchUser()
    {
        <%=Page.GetPostBackEventReference(hidSearch) %>;
    }
    
</script>
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>예외 접속권한</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
<br />
<div class="adminGuideBox" style="width:980px;">
	<dl>
		<dt><strong>검색</strong>&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtSchText" runat="server" Width="100px" ></asp:TextBox> 
            <input type="button" id="btnSearch" value="조 회" style="font-size:13px; font-weight:bold;  width:60px; padding : 0 0 0 15px; height:23px;" onclick="SearchUser();"/>
            (* 사번이나 이름을 넣어주세요.)
		</dt>
	</dl>
</div>
<br />
<div>
	<dl>
       <dt>
            <div class="TableStyleBox" style="width:1000px;">
            <table cellpadding="0" cellspacing="0" class="TableStyle" style="width:1000px;">
                <tr>
                    <th style="width:50px;">구성원</th>
                    <td><asp:TextBox ID="txtEmpNo" runat="server" maxlength="7" Width="100px" ></asp:TextBox> / 
                        <asp:TextBox ID="txtEmpNm" runat="server" Width="100px" ></asp:TextBox> 
                        <a href="#" onclick="fn_SearchUser();"><img src="/common/images/ico_search.png" border="0" alt="" style="vertical-align:middle;" width="25" height="25" /></a>
                        <br />(정규사번이외에 <b>15/19/알파벳사번</b>에 대한 예외권한추가만 가능합니다.)
                    </td>
                    <th style="width:70px;">추가권한</th>
                    <td style="width:300px;">
                    <input type="checkbox" id="cbAuth1" runat="server" class="checkbox" onclick="chkAuth1();" /><span id="lblAuth1">&nbsp;&nbsp;끌지식(T생활백서/DT스토리(IoT))</span><br />
                    <input type="checkbox" id="cbAuth2" runat="server" class="checkbox" onclick="chkAuth2();" /><span id="lblAuth2">&nbsp;&nbsp;DT스토리(DT센터)</span><br />
                    <input type="checkbox" id="cbAuth3" runat="server" class="checkbox" /><span id="lblAuth3">&nbsp;&nbsp;끌문서</span></td>
                    <th><input type="button" id="btnSave2" value="저 장" style="font-size:13px; font-weight:bold;  width:60px; padding : 0 0 0 15px; height:23px;" onclick="AddUser();"/></th>
                </tr>
            </table>
            </div>
        </dt><br />
        * 기존 데이터는 삭제 후 신규로 저장바랍니다.<br />
		<dt>
			<div class="TableStyleBox" style="width:1000px;">
                <table cellpadding="0" cellspacing="0" class="TableStyle" style="width:1000px;">
                    <colgroup>
                        <col width="5%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="20%" />
                        <col width="*" />
                        <col width="5%" />

                    </colgroup>
	            <tr>
                    <th align="center"><strong>No</strong></th>
		            <th align="center"><strong>수정날짜</strong></th>
		            <th align="center"><strong>사번</strong></th>
		            <th align="center"><strong>이름</strong></th>
		            <th align="center"><strong>부서</strong></th>
                    <th align="center">추가권한</th>
                    <th align="center">&nbsp;</th>
	            </tr>
                <asp:Repeater ID="rptmember" runat="server" OnItemDataBound="rptmember_OnItemDataBound" >
                <ItemTemplate>
                    <tr>
                    <td align="center" style="padding :0; height:26px;" ><%# DataBinder.Eval(Container.DataItem, "ROWNUM")%></td>
		            <td align="center" style="padding :0; height:26px;" ><%# DataBinder.Eval(Container.DataItem, "CREATEDATE")%></td>
		            <td align="center" style="padding :0; height:26px;"><%# DataBinder.Eval(Container.DataItem, "USERID")%></td>
		            <td align="center" style="padding :0; height:26px;"><%# DataBinder.Eval(Container.DataItem, "HNAME")%></td>
                    <td align="center" style="padding :0; height:26px;"><%# DataBinder.Eval(Container.DataItem, "DEPTNM")%></td>
                    <td align="left" style="padding :0 0 0 10px;height:26px;"><asp:Literal ID="litUserAuth" runat="server" /></td>
                    <td align="center" style="padding :0; height:26px;">
                        <a href="#" onclick="DelUser('<%# DataBinder.Eval(Container.DataItem, "USERID")%>');""><img src="/common/images/btn/del.png" id="btn_<%# DataBinder.Eval(Container.DataItem, "ID")%>_<%# DataBinder.Eval(Container.DataItem, "USERID")%>" /></a>
                    </td>
	            </tr>
                </ItemTemplate>
                </asp:Repeater>
                </table>
                <p class="pagination" style="position:relative; margin-top:10px;">
                <Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" />
                </p>
             </div>
        </dt>
       
    </dl>
</div>
    <span style="display:none;">
    <asp:Button ID="hidSave" runat="server" Text="" OnClick="hidSave_Click"  />
    <asp:Button ID="hidSearch" runat="server" Text="" OnClick="hidSearch_Click"  />
    <asp:Button ID="hidDelete" runat="server" Text="" OnClick="hidDelete_Click" />
    <asp:HiddenField ID="hidDeleteEmpNo" runat="server" />
    </span>
</asp:Content>
