<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="tikleDTBlog.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.Stats.tikleDTBlog" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
 <style>
     /*TAB CSS*/

        ul.tabs {
            margin: 0;
            padding: 0;
            float: left;
            list-style: none;
            height: 32px; /*--Set height of tabs--*/
            border-bottom: 1px solid #999;
            border-left: 1px solid #999;
            width: 100%;
            font-family:"Malgun Gothic";
            font-size: 0.8em;
        }
        ul.tabs li {
            float: left;
            margin: 0;
            padding: 0;
            height: 31px; /*--Subtract 1px from the height of the unordered list--*/
            line-height: 31px; /*--Vertically aligns the text within the tab--*/
            border: 1px solid #999;
            border-left: none;
            margin-bottom: -1px; /*--Pull the list item down 1px--*/
            overflow: hidden;
            position: relative;
            background: #e0e0e0;
        }
        ul.tabs li a {
            text-decoration: none;
            color: #000;
            display: block;
            font-size: 1.2em;
            padding: 0 20px;
            /*--Gives the bevel look with a 1px white border inside the list item--*/
            border: 1px solid #fff; 
            outline: none;
        }
        ul.tabs li a:hover {
            background: #ccc;
        }
        html ul.tabs li.active, html ul.tabs li.active a:hover  {
             /*--Makes sure that the active tab does not listen to the hover properties--*/
            background: #fff;
            /*--Makes the active tab look like it's connected with its content--*/
            border-bottom: 1px solid #fff; 
        }

        /*Tab Conent CSS*/
        .tab_container {
            border: 1px solid #999;
            border-top: none;
            overflow: hidden;
            clear: both;
            float: left; 
            width: 100%;
            background: #fff;
        }
        .tab_content {
            padding: 20px;
            font-size: 1.2em;
            height : 370px;
        }
       /* 페이징 */
        .pageing_c {	
	        text-align:center;
	        overflow:hidden;
	        margin-top:20px;
	        position:relative;
        }
        .pageing_c a {
	        display:inline-block;
	        width:24px;
	        line-height:24px;	
	        text-align:center;		
	        border:1px solid #fff;
	        font-size:12px;
	        margin-left:-2px;
	        color:#646464;
	        /*/border-radius:100%;*/
	        margin:0 1px;
            text-decoration : none;
        }
        .pageing_c a.selected { 
	        border:1px solid #515f79;
	        color:#515f79;
	        font-weight:bold;
        }
        .pageing_c a.leftMove { 
	        background:url("/Common/Images/btn/paging1.png") no-repeat 50% !important; 
	        margin-right:8px !important;
	        border:1px solid #a8abad;
        }
        .pageing_c a.leftEnd { 
	        background:url("/Common/Images/btn/paging2.png") no-repeat 50% !important;
	        border:1px solid #a8abad;
        }
        .pageing_c a.rightMove { 
	        background:url("/Common/Images/btn/paging3.png") no-repeat 50% !important; 
	        margin-left:8px !important;
	        border:1px solid #a8abad;
        }
        .pageing_c a.rightEnd { 
	        background:url("/Common/Images/btn/paging4.png") no-repeat 50% !important; 
	        border:1px solid #a8abad;
        }
        .pageing_c a:hover {
	        border:1px solid #515f79;
	        color:#515f79;
        }
 </style>
<script type="text/javascript" src="/Common/Js/PagingController.js"></script>
<script type="text/javascript">

    $(document).ready(function() {

        $(".tab_content").hide(); 
        $("ul.tabs li").removeClass("active");

        $("#tabs1").addClass("active").show(); 
        $("#tab1").show(); 
        
        PagingController.Id = "div_pagingAreaComment1";
        PagingController.FunctionName = "fnGetCommentList1";
        PagingController.AlwaysButton = false;
        PagingController.PageSize = 10;
        PagingController.BlockSize = 10;
        PagingController.PageNumber = 1;
        PagingController.UseActiveLink = true;
       
        PagingController1.Id = "div_pagingAreaComment2";
        PagingController1.FunctionName = "fnGetCommentList2";
        PagingController1.AlwaysButton = false;
        PagingController1.PageSize = 10;
        PagingController1.BlockSize = 10;
        PagingController1.PageNumber = 1;
        PagingController1.UseActiveLink = true;
       
        PagingController2.Id = "div_pagingAreaComment3";
        PagingController2.FunctionName = "fnGetCommentList3";
        PagingController2.AlwaysButton = false;
        PagingController2.PageSize = 10;
        PagingController2.BlockSize = 10;
        PagingController2.PageNumber = 1;
        PagingController2.UseActiveLink = true;

        //$("#<%=txtsDate.ClientID%>>").val("<%=new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy.MM.dd")%>");
        $("#<%=txtsDate.ClientID%>").val("<%=DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd")%>");
        $("#<%=txteDate.ClientID%>").val("<%=DateTime.Now.ToString("yyyy.MM.dd")%>");

        SearchClick();

    });
    function ShowClick(gbn) {

        $(".tab_content").hide(); //Hide all content
        $("ul.tabs li").removeClass("active");

        $("#<%=hidGubun.ClientID%>>").val(gbn);

        if (gbn == 1) {
            $("#tabs1").addClass("active").show(); 
            $("#tab1").show(); 
        }
        if (gbn == 2) {
            $("#tabs2").addClass("active").show(); 
            $("#tab2").show(); 
        }
        if (gbn == 3) {
            $("#tabs3").addClass("active").show(); 
            $("#tab3").show(); 
        }
    }

    function SearchExcelClick() {
         var ar1 = $("#<%=txtsDate.ClientID%>").val();
        var ar2 = $("#<%=txteDate.ClientID%>").val();

        var date1 = new Date(ar1.substr(0, 4), ar1.substr(5, 2), ar1.substr(8, 2));
        var date2 = new Date(ar2.substr(0, 4), ar2.substr(5, 2), ar2.substr(8, 2));

        var interval = date2 - date1;
        var day = 1000 * 60 * 60 * 24;
        var month = day * 30;

        var days = parseInt(interval / day);

        if (days > 90) {
            alert("날짜 선택 기간은 3개월 까지 가능합니다.");
            return false;
        }
        else {
            return true;
        }
    }
    function SearchClick() {

        var ar1 = $("#<%=txtsDate.ClientID%>").val();
        var ar2 = $("#<%=txteDate.ClientID%>").val();

        var date1 = new Date(ar1.substr(0, 4), ar1.substr(5, 2), ar1.substr(8, 2));
        var date2 = new Date(ar2.substr(0, 4), ar2.substr(5, 2), ar2.substr(8, 2));

        var interval = date2 - date1;
        var day = 1000 * 60 * 60 * 24;
        var month = day * 30;

        var days = parseInt(interval / day);

        if (days > 90) {
            alert("날짜 선택 기간은 3개월 까지 가능합니다.");
            return;
        }

        fnGetCommentList1(1);
        fnGetCommentList2(1);
        fnGetCommentList3(1);
    }
    function fnGetCommentList1(pageNumber) {

        if (pageNumber == 0) pageNumber = 1;

        try {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/tikleAdmin/stats/tikleDTBlog.aspx/GetList",
                data: "{ Gubun: '1', PageNumber: '" + pageNumber + "', PageSize: '" + PagingController.PageSize + "', SDate: '" + $("#<%=txtsDate.ClientID%>").val() + "', EDate: '" + $("#<%=txteDate.ClientID%>").val() + "'}",
                dataType: "json",
                success: function (data, textStatus, jqXHR) {

                    var Table = data.d.Table;

                    $("#tblList1 tr:not(:first)").remove();
                    var dHtml = "";
                    
                    for (var i = 0; i < Table.length; i++) {
                        dHtml += "<tr>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].RowNum + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].CreateDate + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNo + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptCd + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].WType + "</td>";
                        dHtml += "</tr>";
                    }
                    
                    $("#tblList1").append(dHtml);

                    PagingController.PageNumber = pageNumber;
                    if (Table.length > 0) {
                        PagingController.Append(Table[0].TotalCount);
                    }
                    else {
                        PagingController.Append(0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(jqXHR + ':' + textStatus + ':' + errorThrown);
                    alert("오류가 발생하였습니다. 관리자에게 문의바랍니다.");
                }
            });
        }
        catch (e) {
            alert(e);
        }

        return false;
    }
    function fnGetCommentList2(pageNumber) {

        if (pageNumber == 0) pageNumber = 1;

        try {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/tikleAdmin/stats/tikleDTBlog.aspx/GetList",
                data: "{ Gubun: '2', PageNumber: '" + pageNumber + "', PageSize: '" + PagingController.PageSize + "', SDate: '" + $("#<%=txtsDate.ClientID%>").val() + "', EDate: '" + $("#<%=txteDate.ClientID%>").val() + "'}",
                dataType: "json",
                success: function (data, textStatus, jqXHR) {

                    var Table = data.d.Table;

                    $("#tblList2 tr:not(:first)").remove();
                    var dHtml = "";
                    
                    for (var i = 0; i < Table.length; i++) {
                        dHtml += "<tr>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].RowNum + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].CreateDate + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNo + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptCd + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].WType + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].CommonID + "</td>";
                        dHtml += "<td align=\"left\" style=\"padding :0; height:26px;\">" + Table[i].Title + "</td>";
                        dHtml += "</tr>";
                    }
                    
                    $("#tblList2").append(dHtml);
                    
                    PagingController1.PageNumber = pageNumber;
                    if (Table.length > 0) {
                        PagingController1.Append(Table[0].TotalCount);
                    }
                    else {
                        PagingController1.Append(0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(jqXHR + ':' + textStatus + ':' + errorThrown);
                    alert("오류가 발생하였습니다. 관리자에게 문의바랍니다.");
                }
            });
        }
        catch (e) {
            alert(e);
        }

        return false;
    }
    function fnGetCommentList3(pageNumber) {

        if (pageNumber == 0) pageNumber = 1;

        try {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/tikleAdmin/stats/tikleDTBlog.aspx/GetList",
                data: "{ Gubun: '3', PageNumber: '" + pageNumber + "', PageSize: '" + PagingController.PageSize + "', SDate: '" + $("#<%=txtsDate.ClientID%>").val() + "', EDate: '" + $("#<%=txteDate.ClientID%>").val() + "'}",
                dataType: "json",
                success: function (data, textStatus, jqXHR) {

                    var Table = data.d.Table;

                    $("#tblList3 tr:not(:first)").remove();
                    var dHtml = "";
                    
                    for (var i = 0; i < Table.length; i++) {
                        dHtml += "<tr>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].RowNum + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].CreateDate + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNo + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].EmpNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptCd + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].DeptNm + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].WType + "</td>";
                        dHtml += "<td align=\"center\" style=\"padding :0; height:26px;\">" + Table[i].CommonID + "</td>";
                        dHtml += "<td align=\"left\" style=\"padding :0; height:26px;\">" + Table[i].Title + "</td>";
                        dHtml += "</tr>";
                    }
                    
                    $("#tblList3").append(dHtml);

                    PagingController2.PageNumber = pageNumber;
                    if (Table.length > 0) {
                        PagingController2.Append(Table[0].TotalCount);
                    }
                    else {
                        PagingController2.Append(0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(jqXHR + ':' + textStatus + ':' + errorThrown);
                    alert("오류가 발생하였습니다. 관리자에게 문의바랍니다.");
                }
            });
        }
        catch (e) {
            alert(e);
        }

        return false;
    }
</script>
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<h2 class="Title">
		<strong>DT블로그 접속통계</strong>
		</h2>
		</td>
	</tr>
</table>
<div class="adminGuideBox">
	<dl>
		<dt><strong>Guide</strong></dt>
		<dd>날짜선택하고 데이터를 조회한 후 엑셀이모티콘 클릭 시 데이터의 엑셀 다운로드가 가능합니다.<br>
		</dd>
	</dl>
</div>
<div style="margin-top:30px;">
	<dl>
		<dt>
			<div class="TableStyleBox">
				<table cellpadding="0" cellspacing="0" width="100%" class="TableStyle">
					<tr>
						<th width="100px" class="tac"><strong>날짜선택</strong></th>
						<td>
                            <div style="float:left;">
							<asp:TextBox ID="txtsDate" runat="server" MaxLength="10" onKeyDown="return false;" Width="85px"></asp:TextBox>
                            <em class="from">~</em>
                            <asp:TextBox ID="txteDate" runat="server" MaxLength="10" onKeyDown="return false;" Width="85px"></asp:TextBox>
   
                            <a onclick="ViewCalendar(this, document.getElementById('<%=txtsDate.ClientID%>') ,document.getElementById('<%=txteDate.ClientID%>'),'yyyy.mm.dd','S',0,0);" style="cursor:pointer;">
				                <img src="/common/calendar/images/ico_date.png" alt="날짜검색" style="vertical-align:middle;"/>
			                </a>
                            </div>

                            <span class="button" style="cursor:pointer;float:left; margin:1px 10px 0">
						        <button type="button" onclick="SearchClick();" >조회</button>
					        </span>

                            <asp:HiddenField ID="hidTabID" value ="" runat="server" />
                            <asp:ImageButton ID="btnExcel" class="btnExcel" ImageUrl="~/Common/images/btn/excel.png" runat="server" OnClientClick="return SearchExcelClick();" OnClick="btnExcel_Click"  />			

						</td>
					</tr>
				</table>
			</div>
		</dt>
	</dl>
</div>
<br />
<asp:HiddenField ID="hidGubun" runat="server" Value="1" />
<div id="wrapper">    
    <!--탭 메뉴 영역 -->
    <ul class="tabs">
        <li id="tabs1" style="width:120px;" onclick="ShowClick(1);"><a href="#tab1">1. 접속자로그</a></li>
        <li id="tabs2" style="width:150px;" onclick="ShowClick(2);"><a href="#tab2">2. 게시글별 접속자로그</a></li>
        <li id="tabs3" style="width:150px;" onclick="ShowClick(3);"><a href="#tab3">3. 게시글별좋아요</a></li>
    </ul>

    <!--탭 콘텐츠 영역 -->
    <div class="tab_container">
        <div id="tab1" class="tab_content">
            
            <div class="TableStyleBox" style="width:80%;">
                <table cellpadding="0" cellspacing="0" class="TableStyle"  id="tblList1">
                    <colgroup>
                        <col width="5%" />
                        <col width="*" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="20%" />
                        <col width="10%" />
                    </colgroup>
	            <tr>
                    <th align="center"><strong>No</strong></th>
		            <th align="center"><strong>날짜</strong></th>
		            <th align="center"><strong>사번</strong></th>
		            <th align="center"><strong>이름</strong></th>
		            <th align="center"><strong>부서코드</strong></th>
                    <th align="center"><strong>부서명</strong></th>
                    <th align="center"><strong>DT타입</strong></th>
	            </tr>
               
                </table>
                <p  style="position:relative; margin-top:10px;">
                    <p class="pageing_c" id="div_pagingAreaComment1" ></p>    
                </p>
             </div>
           

        </div>

        <div id="tab2" class="tab_content" style="display:none;">
            <div class="TableStyleBox" style="width:90%;">
                <table cellpadding="0" cellspacing="0" class="TableStyle"  id="tblList2">
                    <colgroup>
                        <col width="5%" />
                        <col width="15%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="15%" />
                        <col width="5%" />
                        <col width="10%" />
                        <col width="*" />
                    </colgroup>
	            <tr>
                    <th align="center"><strong>No</strong></th>
		            <th align="center"><strong>날짜</strong></th>
		            <th align="center"><strong>사번</strong></th>
		            <th align="center"><strong>이름</strong></th>
		            <th align="center"><strong>부서코드</strong></th>
                    <th align="center"><strong>부서명</strong></th>
                    <th align="center"><strong>DT타입</strong></th>
                    <th align="center"><strong>게시글</strong></th>
                    <th align="center"><strong>게시글제목</strong></th>
	            </tr>
                </table>
                <p  style="position:relative; margin-top:10px;">
                    <p class="pageing_c" id="div_pagingAreaComment2" ></p>    
                </p>
             </div>
        </div>

         <div id="tab3" class="tab_content"  style="display:none;">
             <div class="TableStyleBox" style="width:90%;">
                <table cellpadding="0" cellspacing="0" class="TableStyle"  id="tblList3">
                    <colgroup>
                        <col width="5%" />
                        <col width="15%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="15%" />
                        <col width="5%" />
                        <col width="10%" />
                        <col width="*" />
                    </colgroup>
	            <tr>
                    <th align="center"><strong>No</strong></th>
		            <th align="center"><strong>날짜</strong></th>
		            <th align="center"><strong>사번</strong></th>
		            <th align="center"><strong>이름</strong></th>
		            <th align="center"><strong>부서코드</strong></th>
                    <th align="center"><strong>부서명</strong></th>
                    <th align="center"><strong>DT타입</strong></th>
                    <th align="center"><strong>게시글</strong></th>
                    <th align="center"><strong>게시글제목</strong></th>
	            </tr>
                </table>
                <p  style="position:relative; margin-top:10px;">
                    <p class="pageing_c" id="div_pagingAreaComment3" ></p>    
                </p>
             </div>
        </div>
    </div>

</div>
</asp:Content>

