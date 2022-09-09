
// 페이지 이동 1,2, ... 하는 HTML 코드를 생성해서 돌려준다.
//	funcName : 실제 페이지 이동을 위한 함수이름 (예: gotoPage)
//	pageNum : 현재 페이지 번호
//	pageSize : 한 페이지당 결과 갯수
//	total : 전체 결과 갯수

function navAnchor(funcName, pageNo, anchorText, kwd, sort)
{
    var font_class = "<a style='cursor:pointer;' class='' href=\"javascript:{" + funcName + "(" + pageNo + ", '" + kwd + "', '" + sort + "')}\" class=\"pre\">" + anchorText + "</a>";
    //var font_class = "<a href=# onclick=\"javascript:{" + funcName + "(" + pageNo + ")}\">" + anchorText + "</a>";
	return font_class;
}

function subNavAnchor(funcName, pageNo, anchorText, kwd, sort) {

    var font_class = "<a style='cursor:pointer;' class='' href=\"javascript:{" + funcName + "(" + pageNo + ", '" + kwd + "', '" + sort + "')}\">" + anchorText + "</a>";
    //var font_class = "<a href=# onclick=\"javascript:{" + funcName + "(" + pageNo + ")}\">" + anchorText + "</a>";
    return font_class;
}

function pageNav(funcName, pageNum, pageSize, total, kwd, sort) {
    
    if (total < 1)
        return "";

    var ret = "<span>";
    var PAGEBLOCK = 10;
    var totalPages = Math.floor((total - 1) / pageSize) + 1;

    var firstPage = Math.floor((pageNum - 1) / PAGEBLOCK) * PAGEBLOCK + 1;
    if (firstPage <= 0) // ?
        firstPage = 1;

    var lastPage = firstPage - 1 + PAGEBLOCK;
    if (lastPage > totalPages)
        lastPage = totalPages;

    if (firstPage > PAGEBLOCK) {
        ret += navAnchor(funcName, 1, '<img src="/common/images/btn/paging1.png" alt="처음"  style="vertical-align:middle;"/>', kwd, sort);
        ret += navAnchor(funcName, firstPage - 1, '<img src="/common/images/btn/paging2.png" alt="이전" style="vertical-align:middle;" />', kwd, sort);
    }
    else {
        //ret += '<a href="#"><img src="./images/btn_paginate_first.gif" alt="처음" /></a>&nbsp;';
        //ret += '<a href="#"><img src="./images/btn_paginate_prev.gif" alt="이전" /></a>&nbsp;';
    }

    for (i = firstPage; i <= lastPage; i++) {
        if (pageNum == i)
        {
            //ret += "<strong>" + i + "</strong>";
            ret += "<a style='cursor:pointer;' class='on'>" + i + "</a>";
        }
        else
            ret += "" + subNavAnchor(funcName, i, i, kwd, sort);
    }

    if (lastPage < totalPages) {
        ret += navAnchor(funcName, lastPage + 1, '<img src="/common/images/btn/paging3.png"" alt="다음" style="vertical-align:middle;" />', kwd, sort);
        ret += "" + navAnchor(funcName, totalPages, '<img src="/common/images/btn/paging4.png"" alt="끝" style="vertical-align:middle;" />', kwd, sort);
    }
    else {
        //ret += '&nbsp;<a href="#"><img src="./images/btn_paginate_next.gif" alt="다음" /></a>';
        //ret += '&nbsp;<a href="#"><img src="./images/btn_paginate_last.gif" alt="끝" /></a>';
    }

    ret += "</span>";
    return ret;
}