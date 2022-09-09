<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommUserAndDepartmentIframe.aspx.cs" Inherits="SKT.Glossary.Web.Common.Controls.CommUserAndDepartmentIframe" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/DTD/loose.dtd">
<html lang="ko">
    <head>
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <link rel="shortcut icon" href="/favi.ico" />
        <link rel="stylesheet" type="text/css"  href="/common/css/default.css" />
        <link rel="stylesheet" type="text/css"  href="/common/css/view.css" />
        <link rel="stylesheet" type="text/css"  href="/common/css/sub.css" />   
        <link rel="stylesheet" type="text/css"  href="/common/css/jquery-ui.css" />   

        <script src="/Common/js/jquery-1.11.1/jquery-1.11.1.min.js" type="text/javascript"></script>
        <link href="/Common/js/jquery-1.11.1/jquery-ui.css" rel="stylesheet" type="text/css" />
        <script src="/Common/js/jquery-1.11.1/jquery-ui.js" type="text/javascript"></script>

        <script src="/common/js/design.js" type="text/javascript"></script>
        <script src="/common/js/select.js" type="text/javascript"></script>
        <script src="/common/js/jquery.filestyle.js" type="text/javascript"></script>

        <script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>

	    <link href="/Common/Css/jquery.bxslider.css" rel="stylesheet" type="text/css" />

	    <script type="text/javascript" src="/Common/js/jquery.bxslider.min.js"></script>
	    <%--<script type="text/javascript" src="http://www.gmarwaha.com/jquery/jcarousellite/js/jcarousellite_1.0.1.min.js"></script>  --%>
        <script src="/common/js/jcarousellite_1.0.1.min.js" type="text/javascript"></script>
        <title>T.끌, 소통과 협업의 플랫폼</title>
        <script type="text/javascript">
            var lnbDep1 = 4;		//LNB 1depth




        </script>
    </head>
    <body >
        <form id="form1" runat="server">
 


        <div id="pop_dc_folder" class="layer_pop">
			
		

				<fieldset class="authority">
					<common:UserAndDepartment ID="UserAndDepartment1" runat="server" />
				</fieldset>
			</div>


           
        </form>
    </body>


</html>
