<%@ Control Language="C#" AutoEventWireup="true" CodeFile="result_idea.ascx.cs" Inherits="result_idea" %>

<script type="text/javascript">

    var vSort = "<%=srchParam.Sort%>";

    $(document).ready(function () {
        if (vSort == "d")
            $("#sort2").attr('checked', true);
        else 
            $("#sort1").attr('checked', true);
    });

    function fnGlossaryGoView(BoardType, ItemID) {
        location.href = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
    }

    function fnFileDownload(fileName, filePath)
    {
        filePath = "/SKT_MultiUploadedFiles/" + filePath + "/" + fileName;
        location.href = "http://tikle.sktelecom.com/Common/Controls/FileDownload.aspx?FileName=" + encodeURIComponent(fileName) + "&FilePath=" + encodeURIComponent(filePath);
    }

    function ActionForm(sort) {
        var kwd = $("#akcKwd").val();

        if (sort == "")
            goKwd(kwd);
        else
            goKwdNew(kwd, sort);
    }
   
</script>
<ul class="search_effect" style="border-top: 0;">
<% if (rsbIdea.Total > 0)   { %>
    <script type="text/javascript">
        $("#article").css("min-height:500px;");
    </script>
    <%--<h2>검색결과</h2>--%>
    <%
        if ("TOTAL".Equals(srchParam.Category))
        {
    %>
	   <%-- <div class="allview"><a href="javascript:goCategory('idea');">전체보기</a></div>--%>
    <%
        }
    %>
    <div style="padding-bottom:40px; ">
        <p class="num1">
            <input type="radio" id="sort1" value="" onclick="javascript: ActionForm('');"/>&nbsp;정확도순&nbsp;&nbsp;
            <input type="radio" id="sort2" value="" onclick="javascript: ActionForm('d');"/>&nbsp;최신순</p>
        <p class="num">검색결과 <b><%=string.Format("{0:N0}", rsbIdea.Total)%>건</b></p>
    </div>
    <ul class="effect_Glossary" style="border-top:1px solid #333;padding-top:30px;">
<%

    
    for (int i = 0; i < rsbIdea.Rows; i++)
    {
        /*
            DOCID     //0
            BoardType //1
            ID        //2
            CommonID  //3
            Title     //4
            Content   //5
            Hits      //6
            CommentsHits //7
            UserID    //8
            UserName  //9
            DeptName  //10
            CreateDate//11
            TagTitle  //12 
            ModifyDate//13 
            FileName//15 
            Folder//16 
        */


        string CommonID = rsbIdea.Fdata[i, 3].ToString();
        string Title = rsbIdea.Fdata[i, 4].ToString();
        //string Content = TitleCut(rsbIdea.getFdata()[i, 5].ToString());
        string Content = rsbIdea.Fdata[i, 5].ToString();
       
        string ModifyDate = rsbIdea.Fdata[i, 13].ToString();
        string UserName = rsbIdea.Fdata[i, 9].ToString();
        string DeptName = rsbIdea.Fdata[i, 10].ToString();
        string CommentsHits = rsbIdea.Fdata[i, 7].ToString();
        string Hits = rsbIdea.Fdata[i, 6].ToString();

        string FileName = rsbIdea.Fdata[i, 15].ToString();
        string Folder = rsbIdea.Fdata[i, 16].ToString().Replace(@"\", "/");
        
%>
        <li style="padding : 0 0 30px 0">
			<dl>
				<dt><a href="javascript:fnGlossaryGoView('Glossary','<%=CommonID%>');"><%=Title%></a><time><%=Convert.ToDateTime(ModifyDate).ToString("yyyy-MM-dd")%></time></dt>
                <dd>작성자 : <%=UserName%> / <%=DeptName%> <span class="line">답변수 : <%=CommentsHits%></span><span class="line">조회수 : <%=Hits%></span></dd>
				<dd class="ct"><%=Content%>&nbsp;</dd>
				
                <%--<dd>--%>
                    <%
                        //if (FileName.Length > 0)
                        //{
                        //    if (FileName.IndexOf('|') > -1)
                        //    {
                        //        string[] arrFileName = FileName.Split('|');
   
                        //        for (int j = 0; j < arrFileName.Length - 1; j++)
                        //        {
                        //            string FileExt = arrFileName[j].Substring(arrFileName[j].LastIndexOf('.')).ToLower();
                        //            string FileNameNew = "<a href=\"javascript:fnFileDownload('" + arrFileName[j] + "', '" + Folder + "');\" style=\"text-decoration:underline;font-weight:normal;\">" + arrFileName[j] + "</a>";
                        //            HttpContext.Current.Response.Write(FileExtImg(FileExt) + FileNameNew + "<br />");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        string FileExt = FileName.Substring(FileName.LastIndexOf('.')).ToLower();
                        //        string FileNameNew = "<a href=\"javascript:fnFileDownload('" + FileName + "', '" + Folder + "');\" style=\"text-decoration:underline;font-weight:normal;\">" + FileName + "</a>";
                        //        HttpContext.Current.Response.Write(FileExtImg(FileExt) + FileNameNew);
                        //    }
                        //}
                    %>
                <%--</dd>--%>
			</dl>
		</li>

	 <%--   <ul>
		    <li>
		        <div class="unit_list_list">
		            <div class="unit_list_cont">
		                <h2 class="ellipsis"><a href="javascript:popUpWindow('<%=linkUrl %>','790','720','');" ><%=title %></a></h2>
		                <p class="ellipsis"><%=contents %></p>
		                <div class="extra"><span class="fr"><%=regdate %></span></div>
		            </div>
		            <div class="unit_list_person">
		                <p><%=name%></p>
		            </div>

		        </div>
		    </li>
	    </ul>--%>
<%
    }
%>
    </ul>


<%
    if (!"TOTAL".Equals(srchParam.Category))
    {
%>
    <p class="pagination" style="position:relative; margin-top:0px;">
        <script>
            document.write(pageNav("gotoPage", "<%=srchParam.PageNum%>", "<%=srchParam.PageSize%>", "<%=rsbIdea.Total%>", "<%=srchParam.Kwd%>", "<%=srchParam.Sort%>"));
        </script>
    </p>
<%
        }
   }
   else 
   { 
   %>
<script type="text/javascript">
    $("#article").css("min-height:1px;");
    </script>
 <%
   
   }
%>
