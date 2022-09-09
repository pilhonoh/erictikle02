<html>
<head>
<meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>SK TELECOM</title>
<script src="https://code.jquery.com/jquery-2.2.4.min.js" integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
<script type="text/javascript" src="./common/JQuery/jquery.blockUI.js"></script>    

<%--조직도 팝업을 위한 스크립트--%>
<script type="text/javascript" src="/orgchart/common/OrgChart.js"></script>
<%--조직도 Dimmed버전 위한 스크립트--%>
<script type="text/javascript" src="./common/OrgChartDimmed.js"></script>    

<link href="./prettify.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="./prettify.js"></script>

<style type="text/css">
.content {
	margin:5px 5px 30px;
	line-height:16px !important;
}
address a {
	color:#006080;
	font-size:11px;
	font-weight:bold;
	text-decoration:none;
}
button {<a href="http://pnet.skplanet.com/GlobalEDMS/Form/Purchase.aspx">http://pnet.skplanet.com/GlobalEDMS/Form/Purchase.aspx</a>
	font-size:11px;
}
pre.prettyprint {
	background: #F8F8F8;
	border: 1px dotted #ccc !important;
	margin: 1em;
	padding:7px;
	margin-left:0;
	font-family:Consolas,"Lucida Console",GulimChe,"Courier New",Courier,monospace;
    display:block;
    overflow:auto;
    white-space:pre-wrap;    
    font-size:12px;
}
    .style2
    {
        font-family: "맑은 고딕";
        font-weight: bold;
        font-size: small;
    }
    .style3
    {
        font-family: "맑은 고딕";
        font-size: small;
    }
</style>


<script type="text/javascript">
    function orgchart_callback(data) {
        var orgchartData = document.getElementById("orgchartData1");
        var orgchartObj = null;
        if (typeof (data) == "string" && data.length > 0) {
            orgchartData.value = data;
            if (data.toLowerCase().indexOf("<?xml") > -1) {
                orgchartObj = parseXML(data);
            } else {
                orgchartObj = eval(data);
            }
        }
        if (orgchartObj) {
            var taData = document.getElementById("taData");
            var taResult = document.getElementById("taDiplayName");
            taData.value = "";
            taResult.value = "";

            taData.value = data;
            for (var i = 0; i < orgchartObj.length; i++) {
                taResult.value += orgchartObj[i].DisplayName + "\r\n";
            }
        }
    };

    function custom_orgchart_callback(data) {
        var names = ['one', 'two', 'three', 'four', 'five'];

        var orgchartData = document.getElementById("orgchartData2");
        var orgchartObj = {};
        if (typeof (data) == "string" && data.length > 0) {
            orgchartData.value = data;
            if (data.toLowerCase().indexOf("<?xml") > -1) {
                for (var k = 0; k < names.length; k++) {
                    orgchartObj[names[k]] = parseXML(data, names[k]);
                }
            } else {
                orgchartObj = eval(data);
            }
        }
        if (orgchartObj) {
            var taData1 = document.getElementById("taData1");
            var taResult1 = document.getElementById("taDiplayName1");
            var taResult2 = document.getElementById("taDiplayName2");
            var taResult3 = document.getElementById("taDiplayName3");
            var taResult4 = document.getElementById("taDiplayName4");
            var taResult5 = document.getElementById("taDiplayName5");
            taData1.value = "";
            taResult1.value = "";
            taResult2.value = "";
            taResult3.value = "";
            taResult4.value = "";
            taResult5.value = "";

            taData1.value = data;
            if (orgchartObj.one && orgchartObj.one.length > 0) {
                for (var i = 0; i < orgchartObj.one.length; i++) {
                    taResult1.value += orgchartObj.one[i].DisplayName + "\r\n";
                }
            }
            if (orgchartObj.two && orgchartObj.two.length > 0) {
                for (var i = 0; i < orgchartObj.two.length; i++) {
                    taResult2.value += orgchartObj.two[i].DisplayName + "\r\n";
                }
            }
            if (orgchartObj.three && orgchartObj.three.length > 0) {
                for (var i = 0; i < orgchartObj.three.length; i++) {
                    taResult3.value += orgchartObj.three[i].DisplayName + "\r\n";
                }
            }
            if (orgchartObj.four && orgchartObj.four.length > 0) {
                for (var i = 0; i < orgchartObj.four.length; i++) {
                    taResult4.value += orgchartObj.four[i].DisplayName + "\r\n";
                }
            }
            if (orgchartObj.five && orgchartObj.five.length > 0) {
                for (var i = 0; i < orgchartObj.five.length; i++) {
                    taResult5.value += orgchartObj.five[i].DisplayName + "\r\n";
                }
            }
        }
    };

	function mail_orgchart_callback(data) {

		try
		{
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
				var taData = document.getElementById("taData");
				taData.value = "";
				taData.value = data;
			}
		
        }
        catch (ex) {

            alert("11--" + ex.message);
        }

    };
	
</script>

</head>


<body >

<div class="content">
<b>설명</b><br>
조직도를 사용할 페이지에 다음과 같은 스크립트 파일을 포함시킵니다.<br>
** 조직도와 응용프로그램이 같은 사이트에 존재해야합니다.

다음 스크립트를 조직도 스크립트 윗줄에 추가로 포함합니다.<br>
<pre class="prettyprint lang-html" style="width:780px;height:68px;">
&lt;script type="text/javascript" src="./common/JQuery/jquery-1.4.1.min.js"&gt;&lt;/script&gt;
&lt;script type="text/javascript" src="./common/JQuery/jquery.blockUI.js"&gt;&lt;/script&gt;
</pre><br />

<pre class="prettyprint lang-html" style="width:780px;">
&lt;script type="text/javascript" src="./common/OrgChart.js"&gt;&lt;/script&gt;
</pre><br>
Dimmed 형태 조직도를 사용할 페이지에 다음과 같은 스크립트 파일을 포함시킵니다.<br>
<pre class="prettyprint lang-html" style="width:800px;">
&lt;script type="text/javascript" src="./common/OrgChartDimmed.js"&gt;&lt;/script&gt;
</pre><br />
<br/>

<br />
조직도에서 받을수 있는 데이터는 XML과 JSON 형태이며 다음과 같습니다.<br>
<font color="red">** XML은 IE에서만 지원됩니다. 모든 브라우저를 지원하려면 JSON 형태를 사용해야합니다.</font><br><br>
<TABLE style='border:1px solid #C0C0C0;' cellSpacing='0' cellPadding='0' border='0'>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">EntryType</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">0: 부서, 1: 그룹, 2:사용자</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">UserID</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">사용자 ID ( SKCC.0001 )</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">EmpID</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">사용자 사번</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">CompanyCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">회사코드</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">CompanyName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">회사이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">ChildCompanyCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">자회사코드 (있을경우)</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">ChildCompanyName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">자회사 이름 (있을경우)</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">DeptCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">부서코드</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">DeptName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">부서이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            GroupCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            그룹코드</TD>
	</TR>
<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">GroupName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">그룹이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            UserName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            사용자 이름 (단일)</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">DisplayName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">AD DisplayName</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            EmailAddress</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            이메일 주소</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">SIPUri</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">SIP </TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            TitCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            직위코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">TitName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">직위이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            RnkCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            직급코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">RnkName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">직급이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            JobCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            직책코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">JobName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">직책이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            DutCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            직무코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">DutName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">직무이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            LocCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            근무지역 코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">LocName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">근무지역 이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            EmpCode</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            사용자유형 코드</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">EmpName</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">사용자유형 이름</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            MobileTel</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            모바일 연락처</TD>
	</TR>	
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">OfficeTel</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">회사 연락처</TD>
	</TR>
	<TR style='HEIGHT: 25px'>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style2">
            OfficeTelExt</TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' 
            vAlign=middle class="style3">
            회사 연락처 2</TD>
	</TR>		
</TABLE>
<br>
<b>XML 예시</b>
<pre class="prettyprint lang-xml" style="width:780px;">&lt;?xml version="1.0"?&gt;
&lt;person&gt;
    &lt;To&gt;
        &lt;EntryType&gt;&lt;![CDATA[2]]&gt;&lt;/EntryType&gt;
        &lt;UserID&gt;&lt;![CDATA[sksh.00750]]&gt;&lt;/UserID&gt;
        &lt;EmpID&gt;&lt;![CDATA[00750]]&gt;&lt;/EmpID&gt;
        &lt;CompanyCode&gt;&lt;![CDATA[SKSH]]&gt;&lt;/CompanyCode&gt;
        &lt;CompanyName&gt;&lt;![CDATA[SK 해운]]&gt;&lt;/CompanyName&gt;
        &lt;ChildCompanyCode&gt;&lt;![CDATA[]]&gt;&lt;/ChildCompanyCode&gt;
        &lt;ChildCompanyName&gt;&lt;![CDATA[]]&gt;&lt;/ChildCompanyName&gt;
        &lt;DeptCode&gt;&lt;![CDATA[SKSH.00601]]&gt;&lt;/DeptCode&gt;
        &lt;DeptName&gt;&lt;![CDATA[경영관리팀]]&gt;&lt;/DeptName&gt;
        &lt;GroupCode&gt;&lt;![CDATA[]]&gt;&lt;/GroupCode&gt;
        &lt;GroupName&gt;&lt;![CDATA[]]&gt;&lt;/GroupName&gt;
        &lt;UserName&gt;&lt;![CDATA[강사원]]&gt;&lt;/UserName&gt;
        &lt;DisplayName&gt;&lt;![CDATA[강사원(Mr.Kang)/경영관리팀/SKSH]]&gt;&lt;/DisplayName&gt;
        &lt;EmailAddress&gt;&lt;![CDATA[mrkang@sk.com]]&gt;&lt;/EmailAddress&gt;
        &lt;SIPUri&gt;&lt;![CDATA[]]&gt;&lt;/SIPUri&gt;
        &lt;TitCode&gt;&lt;![CDATA[038]]&gt;&lt;/TitCode&gt;
        &lt;TitName&gt;&lt;![CDATA[대리]]&gt;&lt;/TitName&gt;
        &lt;RnkCode&gt;&lt;![CDATA[017]]&gt;&lt;/RnkCode&gt;
        &lt;RnkName&gt;&lt;![CDATA[6급]]&gt;&lt;/RnkName&gt;
        &lt;JobCode&gt;&lt;![CDATA[030400]]&gt;&lt;/JobCode&gt;
        &lt;JobName&gt;&lt;![CDATA[경영기획/관리]]&gt;&lt;/JobName&gt;
        &lt;DutCode&gt;&lt;![CDATA[]]&gt;&lt;/DutCode&gt;
        &lt;DutName&gt;&lt;![CDATA[]]&gt;&lt;/DutName&gt;
        &lt;LocCode&gt;&lt;![CDATA[]]&gt;&lt;/LocCode&gt;
        &lt;LocName&gt;&lt;![CDATA[]]&gt;&lt;/LocName&gt;
        &lt;EmpCode&gt;&lt;![CDATA[]]&gt;&lt;/EmpCode&gt;
        &lt;EmpName&gt;&lt;![CDATA[]]&gt;&lt;/EmpName&gt;
        &lt;MobileTel&gt;&lt;![CDATA[010 62362073]]&gt;&lt;/MobileTel&gt;
        &lt;OfficeTel&gt;&lt;![CDATA[02  37888458]]&gt;&lt;/OfficeTel&gt;
        &lt;OfficeTelExt&gt;&lt;![CDATA[]]&gt;&lt;/OfficeTelExt&gt;
    &lt;/To&gt;
&lt;/person&gt;
</pre><br>
<b>JSON 예시</b>
<pre class="prettyprint lang-js" style="width:780px; height: 143px;">
[{"EntryType":2,"UserID":"sksh.00750","EmpID":"00750","CompanyCode":"SKSH","CompanyName":"SK 해운",
    "ChildCompanyCode":"","ChildCompanyName":"","DeptCode":"SKSH.00601","DeptName":"경영관리팀",
    "GroupCode":"","GroupName":"","UserName":"강사원","DisplayName":"강사원(Mr.Kang)/경영관리팀/SKSH",
    "EmailAddress":"mrkang@sk.com","SIPUri":"","TitCode":"038","TitName":"대리","RnkCode":"017","RnkName":"6급",
    "JobCode":"030400","JobName":"경영기획/관리","DutCode":"","DutName":"","LocCode":"","LocName":"",
    "EmpCode":"","EmpName":"","MobileTel":"010 62362073","OfficeTel":"02  37888458","OfficeTelExt":""}]
</pre>
<br>
<b>조직도 호출함수 파라미터</b><br>
조직도 호출함수의 파라미터는 Javascript의 Object로 구성하여 보내줍니다.<br><br>
<TABLE style='border:1px solid #C0C0C0;' cellSpacing='0' cellPadding='0' border='0'>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>title</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: string<br><br>조직도 제목<br><br>default-><br>한글: 조직도<br>영문: Organization Chart<br>중문: ???</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>callback</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>function<br><br>callback 함수, 필수</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>appType</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: string<br><br>'user': 유저만 선택<br>'dept': 부서만 선택<br>'deptuser': 유저, 부서 선택가능<br><br>default->'deptuser'</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>oneSelect</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: boolean<br><br>true: 단일선택<br>false: 다중선택<br><br>default->false</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>returnType</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: string<br><br>'xml': XML 데이터 (IE전용)<br>'json': JSON 데이터<br><br>default->'json'</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>selectCompany</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: boolean<br><br>true: 회사선택메뉴 보임<br>false: 회사선택메뉴 안보임<br><br>default->true</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>data</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: string<br><br>이전에 선택한 값이<br>호출시에 선택된 상태로...</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle><b>langCode</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA; BORDER-BOTTOM: #C0C0C0 1px solid;' vAlign=middle>type: string<br><br>'ko': 한글<br>'en': 영문<br>'zh': 중문<br><br>default->지정이 안되면 BasePage의 쿠키값을 참고합니다.</TD>
	</TR>
	<TR>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA;' vAlign=middle><b>custom</b></TD>
		<TD style='PADDING-RIGHT: 5px; PADDING-LEFT: 5px; BACKGROUND: #FAFAFA;' vAlign=middle>type: object<br><br>조직도의 선택박스를 설정해서 호출합니다.(2 이상)<br>Ex:<br><pre class="prettyprint lang-js">custom:{
 height: 580,
 items:[{
    name: 'To',
    title: '받는사람'
  },{
    name: 'Cc',
    title: '참조'
  },{
    name: 'Bcc',
    title: '숨은참조'
  }]
}</pre><br>height(Number): 조직도의 창 크기<br>items(Array): 선택박스 설정<br>name(string): xml이나 json으로 받을 데이터의 container 이름<br>title(string): 선택박스의 제목</TD>
	</TR>
</TABLE>
<br>
<b>예제</b><br>

















<input id="orgchartData1" type="hidden" value="" />
<textarea id="taData" rows="10" cols="40"></textarea>
<textarea id="taDiplayName" rows="10" cols="20"></textarea><br><br>


<button onclick="om_OpenOrgChart({callback:orgchart_callback,title:'App 조직도 예제',data:document.getElementById('orgchartData1').value}); return false;">다중선택, 유저+부서</button><br>
<button onclick="om_OpenOrgChart({callback:orgchart_callback,appType:'user',title:'App 조직도 예제',searchAll:true,data:document.getElementById('orgchartData1').value}); return false;">다중 유저선택</button><br><br>

<button onclick="om_OpenOrgChart({callback:orgchart_callback,oneSelect:true,data:document.getElementById('orgchartData1').value}); return false;">단일선택, 유저+부서</button><br>

<button onclick="om_OpenOrgChart({callback:orgchart_callback,appType:'user',oneSelect:true,data:document.getElementById('orgchartData1').value}); return false;">단일 유저선택</button><br><br>
<button onclick="om_OpenOrgChart({callback:orgchart_callback,appType:'dept',searchAll:true,data:document.getElementById('orgchartData1').value}); return false;">다중 부서선택</button><br>
<button onclick="om_OpenOrgChart({callback:orgchart_callback,appType:'dept',oneSelect:true,data:document.getElementById('orgchartData1').value}); return false;">단일 부서선택</button><br>

<button onclick="om_OpenOrgChart({callback:mail_orgchart_callback,app:'mail',appType:'user',devMap:true, oneSelect:false,modal: false,returnType:'json',data:document.getElementById('orgchartData1').value}); return false;">메일테스트</button><br>

<br><br>
<script type="text/javascript" src="./common/autocomplete/jquery-1.11.3.min.js"></script>
<script type="text/javascript" src="./common/autocomplete/jquery.autocomplete.js"></script>
<script type="text/javascript" src="./common/autocomplete/searchUser.autocomplete.js"></script>
<link type="text/css" href="./common/autocomplete/searchUser.autocomplete.css"  rel="stylesheet" />
    
<script type="text/javascript">
    g_SearchUserAutoComplete.registerAutocomplete({
        elementObject: $("#txtAutocomplete"),
        companyCode: 'MIK',
        elementOrgDataInput: $("#taAutocompleteView"),
        options: {
            minChars: 1
        },
        width: '300px'
    });
</script>
</body>
</html>
