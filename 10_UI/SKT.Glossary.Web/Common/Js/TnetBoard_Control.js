
//  File Upload Control 관련 스크립트 합수 
var nFileIdx = 0;
function FileCtrl_FileChange() {
    var obj = event.srcElement;


    if (obj.value != "") {
        var FileUpladObjID = $(obj).parent().attr("FileUploadObj")
        var FileContainer = $(obj).parents(".FileUploadCtrl:first"); 
        var FileDivFiles = $(obj).parent();
        var FileDataTable = $("#" + FileUpladObjID + "_dtFiles");


        var fileNameArray = obj.value.split('\\');
        var fileName = fileNameArray[fileNameArray.length - 1];
        var fileExt = fileName.substring(fileName.lastIndexOf('.') + 1).toUpperCase();
        var fileDeniedExt = FileContainer.attr("DeniedExt");


        //특수문자 체크
        if (!checkStringFormat(fileName)) {
            alert('파일 이름에 제한된 특수문자가 포함되어 있습니다.\r\n\r\n제한된 특수문자: ~ ! @ \\ # $ % < > ^ & * \( ) \" \’ \` \' ');
            $(obj).replaceWith($(obj).clone());
            return false;
        }

        //제한된 확장자 체크
        if (fileDeniedExt != undefined && ((',' + fileDeniedExt.toUpperCase() + ',').indexOf(',' + fileExt.toUpperCase() + ',') >= 0) ){
            alert('[' + fileExt + ']는 제한된 확장자 입니다.\r\n\r\n제한된 확장자:' + fileDeniedExt);
            $(obj).replaceWith($(obj).clone());
            return false; 
        }

        

        // 동일 파일 명을 체크 하여 준다.
        var fileNametdCells = FileDataTable.find("tr");
        var fileCheck = false;
        for (var i = 0 ; i < fileNametdCells.length; i++) {
            if ($(fileNametdCells[i]).attr("FileName") == fileName) {
                alert("동일한 파일명이 존재 합니다.");
                fileCheck = true;
                break;
            }
        }


        if (fileCheck) {
            $(obj).replaceWith($(obj).clone());
            return false;
        }

        var fileCtrls = FileDivFiles.find("input:file")
        fileCtrls.addClass("fileOff");

        //전역변수로 추가함
        //var oldFileIdx = FileUpladObjID + '_File_' + fileIdx;
        //var newFileIdx = FileUpladObjID + '_File_' + (fileIdx + 1);
        var oldFileIdx = FileUpladObjID + '_File_' + nFileIdx;
        var newFileIdx = FileUpladObjID + '_File_' + (nFileIdx + 1);
        nFileIdx++;

        var strDt = "<tr FileName='" + fileName + "'>";
        strDt += "<td class='fileIcon'><img class='fileUploadTypeIcon' src='/Images/ICON/IC" + fileExt + ".gif'/></td>";
        strDt += "<td><img src='/Images/ICON/New.gif'/> " + fileName + "</td>";
        strDt += "<td class='fileSize'></td>";
        strDt += "<td class='fileDel'><img src='/Images/ICON/DELETE.gif' class='fileUploadDelIcon' onclick='javascript:FileCtrl_NewFileDelete(\"" + oldFileIdx + "\")'></td>";
        strDt += "</tr>";

        FileDataTable.append(strDt);
        FileDivFiles.append("<input type='file' ID='" + newFileIdx + "' name='" + FileUpladObjID + "_Files' onChange='FileCtrl_FileChange()'/>")


        FileCtrl_FileIconEmpty();


        return false;
    }
}

//특수문자 체크
function checkStringFormat(string) {
    //var stringRegx=/^[0-9a-zA-Z가-힝]*$/; 
    //var stringRegx = /[~!@\#$%<>^&*\()\-=+_\’]/gi;
    var stringRegx = /[~!@\#$%<>^&*\()\’\`\']/gi;
    var isValid = true;
    if (stringRegx.test(string)) {
        isValid = false;
    }

    return isValid;
}

function FileCtrl_FileIconEmpty() {
    $(".FileUloadTable").find("img").bind("error", function () {
        $(this).attr("src", "/Images/ICON/BLANK.GIF");
    });
}



function FileCtrl_NewFileDelete(fileObjID) {
    $(event.srcElement).parents("tr:first").remove();
    $("#" + fileObjID).remove();
}


function FileCtrl_CurFileDelete(eventObj, fileObjID, jsonIdx, fileKey) {
    
    var hidFileDeleteKey = $('#ctl00_MainContent_hidFileDeleteKey').val();
    
    if (hidFileDeleteKey == "") {
        $('#ctl00_MainContent_hidFileDeleteKey').val(fileKey);
    } else {

        $('#ctl00_MainContent_hidFileDeleteKey').val(hidFileDeleteKey + "$" + fileKey);

    }

    //alert($('#ctl00_MainContent_hidFileDeleteKey').val());

    var hidCurrentDel = $("#" + fileObjID + "_DelFiles");

    var fileObj = jQuery.parseJSON($("#" + fileObjID).val());
    fileObj[jsonIdx].FileMode = "D";
    $("#" + fileObjID).val(JSON.stringify(fileObj));


    $(eventObj).parents("tr:first").remove();
}


function FileCtrl_FileDownload(fileName, filePath) {
    
    if ($("#fileDown").length == 0) {
        $("body").append("<iframe id='fileDown' style='display:none;' frameboard='0px'>");
    }


    $("#fileDown").attr("src", "/Common/Controls/FileDownload.aspx?FileName=" + encodeURIComponent(fileName) + "&FilePath=" + encodeURIComponent(filePath));

}




////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 일정 등록 파일 관련 스크립트 함수 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function fn_ScheduleFileCheck(result) {
    var selFile = $(event.srcElement).val();
    var fileExt = selFile.substr(selFile.lastIndexOf('.') + 1)

    if (fileExt.toUpperCase() != "ICS") {
        alert('일정파일 등록은 ICS 파일만 등록 가능 합니다.');
        //event.srcElement.select();
        //document.selection.clear();


        $(event.srcElement).replaceWith($(event.srcElement).clone())

    }
}




////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Category 관련 스크립트 함수 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function fn_CategoryChange(changeObj) {

    var eventObj = $(changeObj);
    var categoryObj = $("#" + $(eventObj).attr("CategoryObjID"));
    var NextSelObj = $("#" + $(eventObj).attr("NextSelObj"));
    var defaultOption = eval($(eventObj).attr("CategoryObjID") + "_CategoryDefaultOption");
    var xmlObjID = eval($(eventObj).attr("CategoryObjID") + "_CategoryXML");

    var xml = XMLFromString(xmlObjID);
    categoryObj.val(fn_GetCategorySeclectValue($(eventObj).attr("CategoryObjID")));
    var xmlChilds = $(xml).find("[Path='" + categoryHtmlEncode($(eventObj).val()) + "']").children();
    if (xmlChilds.length > 0) {
        NextSelObj.removeAttr("disabled");
        var newOptions = "<option value=''>" + defaultOption + "</option>";
        xmlChilds.each(function () {
            newOptions += "<option value='" + $(this).attr("Path") + "'>" + $(this).attr("Title") + "</option>";
        });
        NextSelObj.html(newOptions);

        var cloneSelObj = NextSelObj.clone(); 
        cloneSelObj.removeClass('jqTransformHidden');
        NextSelObj.parents("div:first").replaceWith(cloneSelObj)
        cloneSelObj.jqTransSelect();

        fn_CategoryInit(cloneSelObj, defaultOption);
    } else {
        fn_CategoryInit($(eventObj), defaultOption)
    }
}



function fn_GetCategorySeclectValue(CategoryObjID) {
    var nextCategorySel;
    var returnValue = "";
    for (i = 0 ; i < 10 ; i++) {
        nextCategorySel = $("#" + CategoryObjID + "_CategorySel_" + i);
        if (nextCategorySel.length == 1 && nextCategorySel.val() != "") {
            returnValue = nextCategorySel.val();
        } else {
            break;
        }
    }

    return returnValue;
}




function fn_CategoryInit(nextSelObj, defaultOption) {

    if (nextSelObj.attr("NextSelObj") == undefined) return;

    var nextSelObj = $("#" + nextSelObj.attr("NextSelObj"));

    nextSelObj.attr("disabled", "disabled")
    nextSelObj.html("<option value=''>" + defaultOption + "</option>");
    var cloneSelObj = nextSelObj.clone();
    cloneSelObj.removeClass('jqTransformHidden');
    nextSelObj.parents("div:first").replaceWith(cloneSelObj)
    cloneSelObj.jqTransSelect();

    fn_CategoryInit(nextSelObj, defaultOption);
}


function XMLToString(oXML) {
    if (window.ActiveXObject) {
        return oXML.xml;
    } else {
        return (new XMLSerializer()).serializeToString(oXML);
    }
}

function XMLFromString(sXML) {
    if (window.ActiveXObject) {
        var oXML = new ActiveXObject("Microsoft.XMLDOM");
        oXML.loadXML(sXML);
        return oXML;
    } else {
        return (new DOMParser()).parseFromString(sXML, "text/xml");
    }
}

function categoryHtmlEncode(str) {
    return str
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
}







////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 공통게시판 분류 관련 함수  
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function fn_BoardCategoryChange(obj) {

    var nextSelobj = $("#" + $(obj).attr("nextselobj"));
    var defaultOption = eval($(obj).attr("BoardCategoryObjID") + "_DefaultOption");
    var jsonData = eval($(obj).attr("BoardCategoryObjID") + "_BoardCategoryJson");

    var newOptions = "<option value=''>" + defaultOption + "</option>";
    var nextSelObjEmpty = true;


    var BoardCategoryObjID = $(obj).attr("BoardCategoryObjID");
    var BoardCategoryObj = $("#" + $(obj).attr("BoardCategoryObjID")); 



    for (i = 0 ; i < jsonData.length ; i++) {
        if (jsonData[i].ParentID == $(obj).val()) {
            nextSelObjEmpty = false;
            nextSelobj.removeAttr("disabled");
            newOptions += "<option value='" + jsonData[i].CategoryID + "'>" + categoryHtmlEncode(jsonData[i].Category) + "</option>";
        }
    }

    if (nextSelObjEmpty) nextSelobj.attr("disabled", "disabled");

    nextSelobj.html(newOptions);


    var cloneSelObj = nextSelobj.clone();
    cloneSelObj.removeClass('jqTransformHidden');

    nextSelobj.parents("div:first").replaceWith(cloneSelObj)
    cloneSelObj.jqTransSelect();

    fn_BoardCategoryNextDDLInit(nextSelobj, defaultOption);


    // 분류의 선택되어진 값을 변경
    BoardCategoryObj.val(fn_GetBoardCategorySeclectValue(BoardCategoryObjID));


}



function fn_BoardCategoryNextDDLInit(nextSelObj, defaultOption) {

    if (nextSelObj.attr("NextSelObj") == undefined) return;
    var nextSelObj = $("#" + nextSelObj.attr("NextSelObj"));


    nextSelObj.attr("disabled", "disabled")
    nextSelObj.html("<option value=''>" + defaultOption + "</option>");


    var cloneSelObj = nextSelObj.clone();
    cloneSelObj.removeClass('jqTransformHidden');
    nextSelObj.parents("div:first").replaceWith(cloneSelObj)
    cloneSelObj.jqTransSelect();

    fn_BoardCategoryNextDDLInit(nextSelObj, defaultOption);
}


function fn_GetBoardCategorySeclectValue(BoardCategoryObjID) {
    var nextBoardCategorySel;
    var returnValue = "";
    for (i = 0 ; i < 10 ; i++) {
        nextBoardCategorySel = $("#" + BoardCategoryObjID + "_BoardCategorySel_" + i);
        if (nextBoardCategorySel.length == 1 && nextBoardCategorySel.val() != "") {
            returnValue = returnValue + "^" + nextBoardCategorySel.val();
        } else {
            break;
        }
    }

    return returnValue;
}




////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// WebEdit 관련 스크립트 함수 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// 문자 존재 여부 체크
String.isNullOrEmpty = function (str) {
    if (str == null) return true;
    if (str == undefined) return true;
    if (str.trim() == "") return true;

    return false;
}

// 문자의 좌, 우 공백 제거
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

// 문자의 좌 공백 제거
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/, "");
}

// 문자의 우 공백 제거
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/, "");
}

// 문장중에 문자나 문자열을 지정된 값으로 변환
String.prototype.replaceAll = function (oldstr, newstr) {
    re = eval("/" + oldstr + "/gi");
    return this.replace(re, newstr);
}

// 문자열의 byte 길이 반환  
String.prototype.byte = function () {
    var cnt = 0;
    for (var i = 0; i < this.length; i++) {
        if (this.charCodeAt(i) > 127)
            cnt += 2;
        else
            cnt++;
    }
    return cnt;
}

// ***************************************************
// 내용 : WebEdit Control의 내용 체크 함수
// 파라매터
//   - ContentID     : WebEdit Control의 ID 값
//   - HiddenFieldID : WebEdit Control의 HFWriteID 값
// ***************************************************
function WebEditCheck(ContentID) {
    var wec = document.getElementById(ContentID);

    var content = wec.TextValue;
    content = content.replace("\r", "");
    content = content.replace("\n", "");
    content = content.trim();

    if (content.length <= 0) {
        alert('내용을 입력해주세요.');
        wec.SetCaretPos(0);
        return false;
    }
    else {
        wec.DefaultCharSet = "utf-8";

        var hfWebEditor = $(document.getElementById(ContentID + "_hfWriter"));
        hfWebEditor.val(wec.MIMEValue);

        return true;
    }

}






function LoadWebEdit(ClientID, Width, Height, Folder, UserLang, XmlFile) {
    var script = "<OBJECT ID='" + ClientID + "' CLASSID='CLSID:18EF21C4-9E4A-42E5-AFC6-26D221E5B7EE' WIDTH='" + Width + "' HEIGHT='" + Height + "' CodeBase='" + Folder + "/NamoWec.cab#Version=7,0,3,22'>";
    script += "<PARAM NAME='UserLang' VALUE='" + UserLang + "'>";
    script += "<PARAM NAME='InitFileURL' VALUE='" + XmlFile + "'>";
    script += "<PARAM NAME='InitFileVer' VALUE='6.5'>";
    script += "<PARAM NAME='InitFileWaitTime' VALUE='3000'>";
    script += "<PARAM NAME='InstallSourceURL' VALUE='http://help.namo.co.kr/activesquare/techlist/help/AS7_update'>";
    script += "</OBJECT>";

    document.write(script);
}