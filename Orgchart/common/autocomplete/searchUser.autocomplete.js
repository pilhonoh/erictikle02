
/**
* AutoComplete.js
* 조직도의 서버 데이터 호출 페이지를 공유하여 autocomplete를 실행하기 위함
* Copyright(c) Miksystem, Inc. (www.miksystem.com)
*/

//var g_SearchUserServiceUrl = '/OrgChart/server.aspx?action=getMemberList&mode=searchUser';
var g_SearchUserServiceUrl = 'http://portal.miksystem.com/OrgChart/server.aspx?action=getMemberList&mode=searchUser';

var g_SearchUserAutoComplete = g_SearchUserAutoComplete || {
    serviceUrl: g_SearchUserServiceUrl,
    autoCompleteCount: 20,  // 순간검색 최대 Count
    options: {},
    coordsProperty: {
        elemID: '',
        initOuterHeight: 0,
        initLeft: 0,
        initTop: 0,
        changedLeft: 0,
        changedTop: 0
    },
    selectionCoords: [],
    callBackElementProperty: {
        elemID: '', // 작성되는 실제 Element ID
        callBackElement: null, // 데이터 보관 히든 input 지정
        maxCompleteUserCount: null, // 사용자 최대 인원수   --> null이면 무제한
        callBackAppendUserFunction: null  // 사용자 입력하는 함수
    },
    callBackElementList: [],
    init: function (companyCode) {
        var that = this;
        that.setOptions();
    },
    setOptions: function () {
        var that = this;

        var options = {
            serviceUrl: that.serviceUrl// + that.addUrlParam()
               , minChars: 1
               , width: 200
               , orientation: 'auto'
               , onSelect: function (suggest) {

                   var elemID = $(this).attr('id');
                   var callBackElementProperty = that.getCallBackElementProperty(elemID);
                   var data = '[]';
                   var callBackElement = null
                   var maxCompleteUserCount = null;
                   
                   if (callBackElementProperty && callBackElementProperty.callBackElement) {
                       callBackElement = callBackElementProperty.callBackElement;

                       if (callBackElementProperty.maxCompleteUserCount) {
                           maxCompleteUserCount = callBackElementProperty.maxCompleteUserCount;
                       }
                   }

                   if (callBackElement) {
                       data = $(callBackElement).val();
                   }

                   if (data == '') data = '[]';

                   var orgChartData = eval(data);

                   var isDuplicate = false;
                   for (var i = 0; i < orgChartData.length; i++) {
                       if (orgChartData[i].EmailAddress == suggest.data.EmailAddress) {
                           isDuplicate = true;
                       }
                   }

                   if (!isDuplicate) {

                       orgChartData.push(suggest.data);
                       
                       if (callBackElement) {

                           if (maxCompleteUserCount) {
                               orgChartData.splice(maxCompleteUserCount);
                           }
                           $(callBackElementProperty.callBackElement).val(JSON.stringify(orgChartData));
                       }
                   } else {

                       alert("중복된 사용자는 제외됩니다.");
                   }

                   that.clearCompleteUser(this);

                   if (callBackElementProperty.callBackAppendUserFunction) {
                       callBackElementProperty.callBackAppendUserFunction(this, orgChartData, maxCompleteUserCount);
                   } else {
                       that.appendCompleteUser(this, orgChartData, maxCompleteUserCount);
                   }

                   that.setCaretAtEnd(this);
               }
        };

        that.options = options;
    },
    addUrlParam: function (companyCode, autoCompleteCount) {
        var that = this;

        var addParam = '?';
        if (that.serviceUrl.indexOf('?') > -1) {
            addParam = '&';
        }

        if (autoCompleteCount) {
            addParam += 'autoCompleteCount=' + autoCompleteCount;
        } else {
            addParam += 'autoCompleteCount=' + that.autoCompleteCount;
        }
        

        if (companyCode) {
            addParam += '&companyCode=' + companyCode;
        }

        return addParam;
    },
    addCompanyCodeParam: function () {
        var that = this;
        if (!that.defaultCompanyCode) { return '' };

        var addParam = '?';
        if (that.serviceUrl.indexOf('?') > -1) {
            addParam = '&';
        }

        if (that.defaultCompanyCode) {
            addParam += 'companyCode=' + that.defaultCompanyCode;
        }

        return addParam;
    },
    registerAutocomplete: function (param) {
        var that = this;

        param = $.extend({}, {
            elementObject: null,
            companyCode: null,
            elementOrgDataInput: null,
            maxCompleteUserCount: null,
            callBackAppendUserFunction: null,
            autoCompleteCount: null,
            options: null,
            width: '300px'
        }, param);
            
        var thistag = param.elementObject,
            elemID = $(thistag).attr('id'),
            companyCode = param.companyCode,
            options = param.options,
            width = param.width;

        var callBackElementProperty = jQuery.extend({}, that.callBackElementProperty);
        callBackElementProperty.elemID = elemID;
        callBackElementProperty.callBackElement = param.elementOrgDataInput;
        callBackElementProperty.maxCompleteUserCount = param.maxCompleteUserCount;
        callBackElementProperty.callBackAppendUserFunction = param.callBackAppendUserFunction;
        that.callBackElementList.push(callBackElementProperty);

        $(thistag).width(width);

        //if (!options || options == {}) { // 파라미터로 옵션이 없다면 기본 설정 적용
        options = jQuery.extend({}, that.options, options);
        options.callBackGetCaretText = that.getCaretText;
        options.callBackGetPositionProperty = that.getPositionProperty;
        //}

        options.serviceUrl = that.serviceUrl + that.addUrlParam(param.companyCode, param.autoCompleteCount);

        var initCoords = true;

        var selectCoords = function (e) {

            var offset = $(this).offset();
            var caretOffset = that.getSelectionCoords();

            if (initCoords) {
                var coordsProperty = jQuery.extend({}, that.coordsProperty);

                coordsProperty.elemID = elemID;
                coordsProperty.initOuterHeight = $(this).outerHeight();
                //coordsProperty.initLeft = caretOffset.x;
                //coordsProperty.initTop = caretOffset.y;
                //coordsProperty.changedLeft = caretOffset.x;
                //coordsProperty.changedTop = caretOffset.y;

                that.selectionCoords.push(coordsProperty);
                initCoords = false;
            } else {
                for (var i = 0; i < that.selectionCoords.length; i++) {
                    var coordsProperty = that.selectionCoords[i];
                    if (coordsProperty.elemID == elemID) {
                        //coordsProperty.changedLeft = caretOffset.x;
                        //coordsProperty.changedTop = caretOffset.y;
                        break;
                    }
                }
            }

        };


        // Div에 디자인 적용
        that.setDesign(thistag);

        $(thistag).autocomplete(options);
        $(thistag).on("mousedown mouseup keydown keyup", selectCoords);
        //$(thistag).focus().mousedown();
        //$(thistag).focusout();
        $(thistag).keypress(function (e) { return e.which != 13; });
        $(thistag).empty();

        var data = '[]';
        var callBackElement = null;
        if (callBackElementProperty.callBackElement) {
            callBackElement = callBackElementProperty.callBackElement;
        }

        

        if (callBackElement) {
            data = $(callBackElement).val();
        }
        if (data == '') data = '[]';

        $(thistag).bind('drop', function () {
            return false;
        });

        if (callBackElementProperty.callBackAppendUserFunction) {
            callBackElementProperty.callBackAppendUserFunction(thistag, eval(data));
        } else {
            that.appendCompleteUser(thistag, eval(data));
        }

    },
    setDesign: function (thistag) {
        $(thistag).attr('contentEditable', 'true');
        $(thistag).addClass('divAutoComplete');
    },
    getCaretText: function (editableDiv, isReturnObj) {
        var that = g_SearchUserAutoComplete;
        var carsetTextObj = {
            prevText: '',
            currentText: '',
            nextText: ''
        },
          sel, range;
        if (window.getSelection) {
            sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0);
                if (range.commonAncestorContainer.parentNode == editableDiv) {
                    var text = range.commonAncestorContainer.data || '';

                    carsetTextObj = that.getCaretTextSubstring(text, range);
                }
            }
        } else if (document.selection && document.selection.createRange) {
            range = document.selection.createRange();
            if (range.parentElement() == editableDiv) {
                var tempEl = document.createElement("span");
                editableDiv.insertBefore(tempEl, editableDiv.firstChild);
                var tempRange = range.duplicate();
                tempRange.moveToElementText(tempEl);
                tempRange.setEndPoint("EndToEnd", range);
                var text = tempRange.text;

                carsetTextObj = that.getCaretTextSubstring(text, range).currentText;
                //caretText = tempRange.text;
            }
        }

        if (!isReturnObj) {
            return $.trim(carsetTextObj.currentText);
        } else {
            return carsetTextObj;
        }
    },
    getCaretTextSubstring: function (text, range) {
        var carsetTextObj = {
            prevText: '',
            currentText: '',
            nextText: ''
        };
        if (text.indexOf(';') == -1) {
            carsetTextObj.currentText = text;
        } else {
            var caretPos = range.endOffset;
            var startIndex = text.indexOf(';');
            var lastIndex = text.lastIndexOf(';');
            if (caretPos <= startIndex) {
                carsetTextObj.currentText = text.substring(0, startIndex);
                carsetTextObj.nextText = text.substring(startIndex, text.length);

            } else if (caretPos >= lastIndex + 1) {
                carsetTextObj.prevText = text.substring(0, lastIndex + 1);
                carsetTextObj.currentText = text.substring(lastIndex + 1, text.length);

            } else {
                startIndex = text.substring(0, caretPos).lastIndexOf(';');

                carsetTextObj.prevText = text.substring(0, startIndex);
                var textSplit = text.substring(startIndex + 1, text.length).split(';');
                carsetTextObj.currentText = textSplit[0];
                carsetTextObj.nextText = textSplit.length <= 1 ? '' : textSplit[1];
            }
        }

        return carsetTextObj;
    },
    getSelectionCoords: function (win) {

        var sel = document.selection, range, rect;
        var x = 0, y = 0;
        if (sel) {
            if (sel.type != "Control") {
                range = sel.createRange();
                range.collapse(true);
                x = range.boundingLeft;
                y = range.boundingTop;
            }
        } else if (window.getSelection) {
            sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0).cloneRange();
                if (range.getClientRects) {
                    range.collapse(true);
                    if (range.getClientRects().length > 0) {
                        rect = range.getClientRects()[0];
                        x = rect.left;
                        y = rect.top;
                    }
                }
                // Fall back to inserting a temporary element
                if (x == 0 && y == 0) {
                    var span = document.createElement("span");
                    if (span.getClientRects) {
                        // Ensure span has dimensions and position by
                        // adding a zero-width space character
                        span.appendChild(document.createTextNode("\u200b"));
                        range.insertNode(span);
                        rect = span.getClientRects()[0];
                        x = rect.left;
                        y = rect.top;
                        var spanParent = span.parentNode;
                        spanParent.removeChild(span);

                        // Glue any broken text nodes back together
                        spanParent.normalize();
                    }
                }
            }
        }
        return { x: x, y: y };
    },
    getPositionProperty: function (elemID) {
        var that = g_SearchUserAutoComplete;
        var coordsProperty = jQuery.extend({}, that.coordsProperty);
        for (var i = 0; i < that.selectionCoords.length; i++) {
            var coordsProperty = that.selectionCoords[i];
            if (coordsProperty.elemID == elemID) {
                coordsProperty = jQuery.extend({}, coordsProperty);
                break;
            }
        }
        return coordsProperty;
    },
    getCaretPosition: function (editableDiv) {
        var caretPos = 0,
          sel, range;
        if (window.getSelection) {
            sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0);
                if (range.commonAncestorContainer.parentNode == editableDiv) {
                    caretPos = range.endOffset;
                }
            }
        } else if (document.selection && document.selection.createRange) {
            range = document.selection.createRange();
            if (range.parentElement() == editableDiv) {
                var tempEl = document.createElement("span");
                editableDiv.insertBefore(tempEl, editableDiv.firstChild);
                var tempRange = range.duplicate();
                tempRange.moveToElementText(tempEl);
                tempRange.setEndPoint("EndToEnd", range);
                caretPos = tempRange.text.length;
            }
        }
        return caretPos;
    },
    selectText: function (elem) {
        var that = this;
        var doc = document;
        var element = elem;
        //console.log(this, element);
        if (doc.body.createTextRange) {
            var range = document.body.createTextRange();
            range.moveToElementText(element);
            range.select();
        } else if (window.getSelection) {
            var selection = window.getSelection();
            var range = document.createRange();
            range.selectNodeContents(element);
            selection.removeAllRanges();
            selection.addRange(range);
        }
    },
    getTextNodesIn: function (node) {
        var that = this;
        var textNodes = [];
        if (node.nodeType == 3) {
            textNodes.push(node);
        } else {
            var children = node.childNodes;
            for (var i = 0, len = children.length; i < len; ++i) {
                textNodes.push.apply(textNodes, that.getTextNodesIn(children[i]));
            }
        }
        return textNodes;
    },
    setCaretAtEnd: function (elem) {

        elem.focus();
        if (typeof window.getSelection != "undefined"
                && typeof document.createRange != "undefined") {
            var range = document.createRange();
            range.selectNodeContents(elem);
            range.collapse(false);
            var sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        } else if (typeof document.body.createTextRange != "undefined") {
            var textRange = document.body.createTextRange();
            textRange.moveToElementText(elem);
            textRange.collapse(false);
            textRange.select();
        }
    },
    getCallBackElementProperty: function (elemID) {
        var that = this;
        var callBackElementProperty = null;
        for (var i = 0; i < that.callBackElementList.length; i++) {
            var callBackElementProperty = that.callBackElementList[i];
            if (callBackElementProperty.elemID == elemID) {
                callBackElementProperty = callBackElementProperty;
                break;
            }
        }

        return callBackElementProperty;
    },
    clearCompleteUser: function(thistag){
        $(thistag).empty();
    },
    appendCompleteUser: function (thistag, data, maxCompleteUserCount) {
        
        var that = this;

        var elemID = $(thistag).attr('id');

        for (var i = 0; i < data.length; i++) {
            
            var compObj = $('<span name="completeSearchUser" class="completeUser"></span>');
            if (that.getIEVersion() > 0) {
                compObj.attr('contenteditable', 'true');
            } else {
                compObj.attr('contenteditable', 'false');
            }
            compObj.text(data[i].DisplayName);
            compObj.data('email', data[i].EmailAddress);

            $(thistag).append(compObj);

            // 최대 인원수라면 ;를 제거
            if (!maxCompleteUserCount || maxCompleteUserCount > i + 1) {
                $(thistag).append(';');
            }

            $(compObj).bind("DOMNodeRemoved", function () {
                that.removeCallBackElementData(this, elemID);
                //that.removeSpace(thistag);
                //alert("Removed: " + e.target.nodeName);
            });
        }
    },
    removeCallBackElementData: function (thistag, elemID) {

        var that = this;
        var EmailAddress = $(thistag).data('email');
        var callBackElementProperty = that.getCallBackElementProperty(elemID);
        var data = '[]';


        if (callBackElementProperty && callBackElementProperty.callBackElement) {
            data = $(callBackElementProperty.callBackElement).val();
        }

        if (data == '') data = '[]';

        var orgChartData = eval(data);

        var isDuplicate = false;
        for (var i = 0; i < orgChartData.length; i++) {
            if (orgChartData[i].EmailAddress == EmailAddress) {
                orgChartData.splice(i, 1);
                break;
            }
        }
        
        if (callBackElementProperty && callBackElementProperty.callBackElement) {
            $(callBackElementProperty.callBackElement).val(JSON.stringify(orgChartData));
        }
    },
    getIEVersion: function () {
        var sAgent = window.navigator.userAgent;
        var Idx = sAgent.indexOf("MSIE");

        // If IE, return version number.
        if (Idx > 0)
            return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

            // If IE 11 then look for Updated user agent string.
        else if (!!navigator.userAgent.match(/Trident\/7\./))
            return 11;

        else
            return 0; //It is not IE
    }//,
    //removeSpace: function (thistag) {

    //    var childNodes = $(thistag)[0].childNodes;
    //    for (var i = 0; i < childNodes.length; i++) {
    //        $(childNodes[i]).text().replace(/ /g, '');
    //    }

    //}
}

g_SearchUserAutoComplete.init();