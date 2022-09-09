<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailUserList.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.MailUserList" %>
<script src="../Common/Js/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/OrgChart/common/orgchart.js"></script>
<%--<script type="text/javascript" src="/Common/js/jquery.word-break-keep-all.min.js"></script>--%>
<script type="text/javascript" language="javascript">

    var ShareSearchTemp = null;
    var m_ItemID = '<%= ItemID %>';
    var m_UserID = '<%= UserID %>';
    var m_selectID = "<%=ddlUserGroup.ClientID%>";  // Mr.No 2015-07-02 사용자 그룹선택 값

    // 최초 조직도 클릭시 페이지 나감 alert 창 제거
    onBeforeUnloadFired = true;

    $(document).ready(function () {

        // Mr.No 2015-07-02 Start
        // 사용자 그룹을 선택했을 경우 그룹 추가
        $("select#" + m_selectID).change(OnSelectMyGroup);
        // Mr.No 2015-07-02 End

        /*
        Author : 개발자-최현미C, 리뷰자-진현빈D
        Create Date : 2016.06.01 
        Desc : 메일 수신/참조/숨은참조 
        */
        //$('#tdToList').wordBreakKeepAll();
        //$('#tdCcList').wordBreakKeepAll();
        //$('#tdBccList').wordBreakKeepAll();
    });

    var obj = {};
    var objj = {};
    var objt = {};
    //공유 검색에서 사용자 배열 저장(자동완성목록)
    function pushToAry(name, val) {
        obj[name] = val;
    }

    //공유 검색에서 사용자 선택시 배열에 저장 
    function pushToArySave(name, val, utype) {
        //debugger;

        if (objj[name] == undefined) {
            var objcut = name.split('/');
            if (utype == "U") {
                //alert("TYPE U : " + name + " ★ " + val + " ★ " + utype);
                var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "/" + objcut[1] + "</a></li> ";
            } else {
                //alert("TYPE UX : " + name + " ★ " + val + " ★ " + utype);
                var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "</a></li> ";
            }

            $("#CommonUserList").prepend(NameSet);
            $("#CommonUserList").show();

            objj[name] = val;
            objt[name] = utype;
        }
        else {
            alert('이미 추가 되어 있습니다.');
        }
    }

    //선택된 공유 사용자 제거
    function fnDelete(obj, val) {
        delete objj[val];
        delete objt[val];

        $(obj).parent().remove();

        if ($("#CommonUserList li").length > 0) {
            $("#CommonUserList").show();
        }
        else {
            $("#CommonUserList").hide();
        }
    }

    //글작성 완료 버튼시 
    function fnShareSave() {
        var UserIDData = "";
        var UserTypeData = "";
        var UserNameData = "";

        //for (var i = 0; i < $("#CommonUserList li").length; i++) {
        //	var tempID = objj[$("#CommonUserList li")[i].innerText.replace('X', "")];
        //    var tempType = objt[$("#CommonUserList li")[i].innerText.replace('X', "")];

        //	if (tempID != "") {
        //		if (UserIDData == null) {
        //			UserIDData = tempID + "/";
        //			UserTypeData = tempType + "/";
        //			UserNameData = $("#CommonUserList li")[i].innerText + "&";
        //		} else {
        //			UserIDData += tempID + "/";
        //			UserTypeData += tempType + "/";
        //			UserNameData += $("#CommonUserList li")[i].innerText + "&";
        //		}
        //	}
        //}
        //// 공유사용자 ID, 이름
        //if (UserIDData != null) {
        //	saveUserItemID(UserIDData, UserNameData, UserTypeData);
        //}
        //else {
        //	//alert('공유할 사용자를 선택하세요');
        //}


        var objTmp = [];

        if (!isEmptyObj(objj)) {
            for (var key in objj) {
                if (objj.hasOwnProperty(key)) {
                    objTmp.push(key);
                }
            }
            //debugger;
            objTmp.sort();
            for (i in objTmp) {

                var key = objTmp[i];
                var value = objj[objTmp[i]];

                if (objj.hasOwnProperty(key)) {

                    //alert("key=" + key + ", value = " + value);
                    var nameCut = key.split("/");

                    UserIDData += value + "/";
                    UserNameData += nameCut[0] + "/" + nameCut[1] + "&";
                    if (value.length == 8) {
                        UserTypeData += "O/";
                    } else if (value.length == 5) {
                        UserTypeData += "G/";
                    } else {
                        if (!isEmptyObj(objt)) {
                            if (objt[objTmp[i]] == "M") {
                                UserTypeData += "M/";
                            } else {
                                UserTypeData += "U/";
                            }
                        }
                    }
                }
            }
            //alert("id : " + UserIDData + "\n" + "Name : " + UserNameData + "\n : " + "Type" + UserTypeData);


            // 공유사용자 ID, 이름
            if (UserIDData != "") {
                saveUserItemID(UserIDData, UserNameData, UserTypeData);
            }
            else {
                //alert('공유할 사용자를 선택하세요');
            }
        }
    }

    function isEmptyObj(obj) {
        for (var key in obj) return false; return true;
    }
    // hidden값 셋팅
    function saveUserItemID(UserIDData, UserNameData, UserTypeData) {
        $('#<%= this.hdUserItemID.ClientID %>').val("");
        $('#<%= this.hdUserItemID.ClientID %>').val(UserIDData);
        $('#<%= this.hdUserName.ClientID %>').val("");
        $('#<%= this.hdUserName.ClientID %>').val(UserNameData);
        $('#<%= this.hdUserType.ClientID %>').val("");
        $('#<%= this.hdUserType.ClientID %>').val(UserTypeData);
    }

    function getItemValue(ck) {
        var rtnItemValue = "";
        if (ck == "ID") {
            rtnItemValue = $('#<%= this.hdUserItemID.ClientID %>').val();
	    } else if (ck == "NM") {
	        rtnItemValue = $('#<%= this.hdUserName.ClientID %>').val();
        } else if (ck == "TY") {
            rtnItemValue = $('#<%= this.hdUserType.ClientID %>').val();
        }

    return rtnItemValue;
}

function fnfocusMove() {
    $("#user").focus();
    $("#user").val("");
}

//공유 닫기
function fnShareClose() {
    $(".view-func-share-outer").hide();
    ShareOpen = true;
}

// 편집화면에서 일부공개 일 경우 list 
function DefaultSetting(str) {
    var JsonData = JSON.parse(str);
    for (var i = 0; i < JsonData.length; i++) {

        // 대상자 목록에 추가
        pushToArySave(JsonData[i].ToUserName + "/" + JsonData[i].ToUserID, JsonData[i].ToUserID, JsonData[i].ToUserType);
    }
    fnShareSave();
}

//사원값 초기화
function DefaultClear() {
    $("#CommonUserList").html("");
    obj = {};
    objj = {};
    objt = {};

    $("#CommonUserList").hide();
}

function fnCheckValue() {
    var rtn = false;
    if ($('#<%= this.hdUserItemID.ClientID %>').val() == "") {
        rtn = true;
    }
    return rtn;
}

//// 사원찾기(조직도)
function fn_SearchUser() {
    // alert('22');
    function callback(data) {
        var orgchartObj = {};
        if (typeof (data) == "string" && data.length > 0) {
            orgchartObj = eval(data);

            if (orgchartObj) {
                for (var i = 0; i < orgchartObj.length; i++) {
                    if (orgchartObj[i].EmpID) {
                        // 사원
                        OrgChartUser(orgchartObj[i].UserName, orgchartObj[i].DeptName, orgchartObj[i].EmpID);
                    } else {
                        //부서
                        OrgChartDept(orgchartObj[i].DeptName, orgchartObj[i].DeptCode);
                    }
                }
            }
        }
    }
    om_OpenOrgChart({
        callback: callback,
        app: 'mail',
        oneSelect: false,
        modal: false,
        returnType: 'json'
    });
}


// OrgChart 로 사원입력
function OrgChartUser(UserName, DeptName, EmpID) {
    ShareSearchTemp = $('#user').val();

    // 대상자 목록에 추가 김성환20150121
    pushToArySave(UserName + "/" + DeptName + "/" + EmpID, EmpID, 'U');
}

// OrgChar로 부서입력
function OrgChartDept(DeptName, DeptCode) {
    ShareSearchTemp = $('#user').val();

    // 대상자 목록에 추가
    pushToArySave(DeptName, DeptCode, 'O');
}

// 사용자 그룹 선택
function OnSelectMyGroup() {
    var obj = $("select#" + m_selectID + " option");

    var grpNM = obj.filter(":selected").text();
    var grpID = obj.filter(":selected").val();

    if (grpID != "0" && grpID != null && grpID != undefined) {
        pushToArySave(grpNM, grpID, 'G');

        obj.eq(0).attr('selected', 'selected');
    }
}

// 게시글 조회/수정 시 권한 목록 조회
function fnSelectAuth(stype, id, uid) {

    try {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
            data: "{Type : '" + stype + "', UserID : '" + uid + "', MyGrpID : '" + id + "'}",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.d[0] != null && data.d[0].length != 0) {
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
}

function fnEmail_init() {
    var toUser = $('#<%= hidToEmail.ClientID%>'); //기본수신자
        var ccUser = $('#<%=hidCcEmail.ClientID%>');  //참조자
        var bccUser = $('#<%=hidBccEmail.ClientID%>'); //숨은참조자
        var orgchartdata = $("#orgchartData1");

        //초기화
        toUser.val("");
        ccUser.val("");
        bccUser.val("");
        orgchartdata.val("");

        $("#tdToList").html("");
        $("#tdCcList").html("");
        $("#tdBccList").html("");

    }

    // 사원찾기
    function fn_SearchUser_mailemail() {

        //var gg = '[{"To":[{"EntryType":2,"UserID":"1109972","EmpID":"1109972","CompanyCode":"SKT","CompanyName":"SK 텔레콤","DeptCode":"00001448","DeptName":"Biz. Application팀","GroupCode":"","GroupName":"","UserName":"김준호","DisplayName":"김준호/Biz. Application팀","EmailAddress":"skt.junho.kim@sk.com","TitCode":"00","TitName":"","JobName":"","DutCode":"","DutName":"","LocCode":"SK01","LocName":"본사_SK T-타워","EmpCode":"","EmpName":"","MobileTel":"010-3538-5948","OfficeTel":"02-6100-2957","LyncOnnet":"","OfficeTel2":"","OfficeTelExt":"80-2957","Fax":"","CountryCode":"","CountryName":"","OrgchartName":"김준호 (1109972)","HasSubDept":null}],"Cc":[],"Bcc":[]}]';
        //var jsoninfo = JSON.parse(JSON.stringify(gg));
        //$("#orgchartData1").val(jsoninfo);

        function callback(data) {

            orgchart_callback(data);
        }
        om_OpenOrgChart({
            callback: callback,
            app: 'mail',
            oneSelect: false,
            modal: false,
            returnType: 'json',
            appType: 'USER',
            data: $("#orgchartData1").val()
        });


    }

    function orgchart_callback(data) {

        var toUser = $('#<%= hidToEmail.ClientID%>'); //기본수신자
        var ccUser = $('#<%=hidCcEmail.ClientID%>');  //참조자
        var bccUser = $('#<%=hidBccEmail.ClientID%>'); //숨은참조자
        var orgchartdata = $("#orgchartData1");

        //초기화
        toUser.val("");
        ccUser.val("");
        bccUser.val("");
        orgchartdata.val("");

        var orgchartData = data;
        //$("#orgchartData1" + Number(Id)).val(data);
        //alert("modal의 값 ==>" + orgchartData);

        if (orgchartData == "null") {
            return false;
        }

        try {

            var orgchartData = $("#orgchartData1");

            if (typeof (data) == "string" && data.length > 0) {
                orgchartData.val(data);
                if (data.toLowerCase().indexOf("<?xml") > -1) {
                    orgchartObj = parseXML(data);
                } else {
                    //orgchartObj = eval(data);
                    orgchartObj = JSON.parse(data);
                }
            }

            if (orgchartObj) {
                var strToList = "";
                var strCcList = "";
                var strBccList = "";

                $("#tdToList").html("");
                $("#tdCcList").html("");
                $("#tdBccList").html("");
                for (var i = 0; i < orgchartObj.To.length; i++) {

                    if (toUser.val() == "")
                        orgTmp = orgchartObj.To[i].EmailAddress + "&" + orgchartObj.To[i].DisplayName;
                    else
                        orgTmp += ";" + orgchartObj.To[i].EmailAddress + "&" + orgchartObj.To[i].DisplayName;
                    toUser.val(orgTmp);

                    /*
                    Author : 개발자- 최현미C, 리뷰자-진현빈D
                    CreateDae :  2016.06.01
                    Desc : 수신/참조/숨은참조 추가
                    */
                    var ToName = orgchartObj.To[i].DisplayName.replace(/ /g, '');
                    strToList += "●" + ToName + "; ";
                }

                for (var i = 0; i < orgchartObj.Cc.length; i++) {
                    //taResult.value += 'CC' + orgchartObj.Cc[i].DisplayName + "\r\n";
                    if (ccUser.val() == "")
                        orgTmp = orgchartObj.Cc[i].EmailAddress + "&" + orgchartObj.Cc[i].DisplayName;
                    else
                        orgTmp += ";" + orgchartObj.Cc[i].EmailAddress + "&" + orgchartObj.Cc[i].DisplayName;

                    ccUser.val(orgTmp);

                    /*
                   Author : 개발자- 최현미C, 리뷰자-진현빈D
                   CreateDae :  2016.06.01
                   Desc : 수신/참조/숨은참조 추가
                   */
                    var CcName = orgchartObj.Cc[i].DisplayName.replace(/ /g, '');
                    strCcList += "●" + CcName + "; ";

                }

                for (var i = 0; i < orgchartObj.Bcc.length; i++) {
                    //taResult.value += 'BCC' + orgchartObj.Bcc[i].DisplayName + "\r\n";
                    if (bccUser.val() == "")
                        orgTmp = orgchartObj.Bcc[i].EmailAddress + "&" + orgchartObj.Bcc[i].DisplayName;
                    else
                        orgTmp += ";" + orgchartObj.Bcc[i].EmailAddress + "&" + orgchartObj.Bcc[i].DisplayName;

                    bccUser.val(orgTmp);

                    /*
                   Author : 개발자- 최현미C, 리뷰자-진현빈D
                   CreateDae :  2016.06.01
                   Desc : 수신/참조/숨은참조 추가
                   */
                    var BccName = orgchartObj.Bcc[i].DisplayName.replace(/ /g, '');
                    strBccList += "●" + BccName + "; ";
                }


                $("#tdToList").html(strToList);
                $("#tdCcList").html(strCcList);
                $("#tdBccList").html(strBccList);
            }

        }
        catch (ex) {

            alert("11--" + ex.message);
        }

    };

    // Mr.No 2015-07-02
    // 사용자 그룹 선택
    function OnSelectMyGroup() {
        var obj = $("select#" + m_selectID + " option");

        var grpNM = obj.filter(":selected").text();
        var grpID = obj.filter(":selected").val();

        if (grpID != "0" && grpID != null && grpID != undefined) {
            pushToArySave(grpNM, grpID, 'G');

            obj.eq(0).attr('selected', 'selected');
        }
    }
    //공유 검색에서 사용자 선택시 배열에 저장 
    function pushToArySave(name, val, utype) {
        //debugger;

        if (objj[name] == undefined) {
            var objcut = name.split('/');
            if (utype == "U") {
                //alert("TYPE U : " + name + " ★ " + val + " ★ " + utype);
                var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDeleteGroup(this,'" + name + "');\">" + objcut[0] + "/" + objcut[1] + "</a></li> ";
            } else {
                //alert("TYPE UX : " + name + " ★ " + val + " ★ " + utype + "objcut[0] : " + objcut[0]);
                var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDeleteGroup(this,'" + name + "');\">" + objcut[0] + "</a></li> ";

            }

            $("#CommonUserList2").prepend(NameSet);
            $("#CommonUserList2").show();

            objj[name] = val;
            objt[name] = utype;
            //alert(objj[name]);
        }
        else {
            alert('이미 추가 되어 있습니다.');
        }
    }
    // 메일보내기 클릭시 그룹 값 셋팅
    function fnGroupSet() {
        var UserIDData = "";
        var UserTypeData = "";
        var UserNameData = "";


        var objTmp = [];

        if (!isEmptyObj(objj)) {
            for (var key in objj) {
                if (objj.hasOwnProperty(key)) {
                    objTmp.push(key);
                }
            }
            //debugger;
            objTmp.sort();
            for (i in objTmp) {

                var key = objTmp[i];
                var value = objj[objTmp[i]];

                if (objj.hasOwnProperty(key)) {

                    //alert("key=" + key + ", value = " + value);
                    var nameCut = key.split("/");

                    UserIDData += value + "/";
                    UserNameData += nameCut[0] + "/" + nameCut[1] + "&";
                    if (value.length == 8) {
                        UserTypeData += "O/";
                    } else if (value.length == 5) {
                        UserTypeData += "G/";
                    } else {
                        if (!isEmptyObj(objt)) {
                            if (objt[objTmp[i]] == "M") {
                                UserTypeData += "M/";
                            } else {
                                UserTypeData += "U/";
                            }
                        }
                    }
                }
            }
            //alert("id : " + UserIDData + "\n" + "Name : " + UserNameData + "\n : " + "Type" + UserTypeData);


            // 공유사용자 ID, 이름
            if (UserIDData != "") {
                //alert(UserIDData + "\n" + UserNameData + "\n" + UserTypeData);
                //saveUserItemID(UserIDData, UserNameData, UserTypeData);
                $('#<%= this.hdGroupUser.ClientID %>').val("");
                $('#<%= this.hdGroupUser.ClientID %>').val(UserIDData);
            }
            else {
                //alert('공유할 사용자를 선택하세요');
            }
        }
    }
    //선택된 그룹 제거
    function fnDeleteGroup(obj, val) {
        delete objj[val];
        delete objt[val];

        $(obj).parent().remove();

        if ($("#CommonUserList2 li").length > 0) {
            $("#CommonUserList2").show();
        }
        else {
            $("#CommonUserList2").hide();
        }
    }
</script>
<div id="UserAndDepartment" style="overflow-x:hidden;overflow-y:hidden;">
    <% if (bGrp) { %>
            <p id="SelectUserGroup" style="display:none;">
                <asp:DropDownList ID="ddlUserGroup" Width="266px" CssClass="select" runat="server"  />
            </p>
    <% } %>

	<p>
        <input type="text" id="hidFromEmail" name="hidFromEmail" value="" runat="server" style="display:none;" /> 
        <input type="text" id="hidToEmail" name="hidToEmail" value="" runat="server" style="display:none;"/> 
        <input type="text" id="hidCcEmail" name="hidCcEmail" value="" runat="server" style="display:none;"/>
        <input type="text" id="hidBccEmail" name="hidBccEmail" value="" runat="server" style="display:none;"/>
        <input type="text" id="orgchartData1" name="orgchartData1" value="" style="display:none;" /> 
        <input type="text" id="hdGroupUser" name="hdGroupUser" value="" runat="server" style="display:none;"/><%--Mr.No 2015-07-02 그룹선택--%>        
        <!--<input type="text" id="user"  name="" class="txt t3" value="  이름으로 검색11sss1"  onclick="fnfocusMove();"/>         -->
		<a href="javascript:fn_SearchUser_mailemail();" class="btn15"><b style="line-height:1.1;padding-top:12px;">조직도에서 선택하기</b>
		</a>
	</p>
	<ul id="CommonUserList2" style="display:none;"></ul>
    <%--<div id="CommonUserList2" style="display:none;"></div>--%>
</div>
<br />
<table class="email-table">
      <tr>
          <th style="background:#2f8d12; ">받는사람</th>
          <td ><div id="tdToList" style="overflow-y:scroll; max-height:60px; width:100%; word-wrap:break-word; word-break:keep-all;"></div></td>
      </tr>
</table><br />
<table class="email-table">
        <tr>
          <th style="background:#777; ">참조</th>
          <td><div id="tdCcList" style="overflow-y:scroll; max-height:60px; width:100%; word-wrap:break-word;word-break:keep-all;"></div></td>
      </tr>
</table><br />
<table class="email-table">

     <tr>
          <th style="background:#777;">숨은참조</th>
          <td><div id="tdBccList" style="overflow-y:scroll; max-height:60px; width:100%; word-wrap:break-word; word-break:keep-all;"></div></td>
      </tr>
</table>


<!--/지식_조회 권한 설정하기-->
<asp:HiddenField ID="hdUserItemID" runat="server" />
<asp:HiddenField ID="hdUserName" runat="server" />
<asp:HiddenField ID="hdUserType" runat="server" />