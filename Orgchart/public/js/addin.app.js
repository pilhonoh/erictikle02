// change default title
AppConfig = AppConfig || {};
AppConfig.title = {
  ko: '조직도',
  en: 'Organization Chart',
  zh: '组织图'
};

var useXML = false;
function convertXMLToJSON(d) {
  var xmlDoc = $.parseXML(d);
  var toArray = $(xmlDoc).find('person To');

  var jsonArray = [];

  if (toArray.length > 0) {
    toArray.each(function() {
      var displayName = $(this).find('DisplayName');
      var userName = $(this).find('UserName');
      var emailAddress = $(this).find('EmailAddress');
      var userID = $(this).find('UserID');

      jsonArray.push({
        DisplayName: displayName.text(),
        UserName: userName.text(),
        EmailAddress: emailAddress.text(),
        UserID: userID.text()
      });
    });
  }

  return jsonArray;
}
function convertJSONToXML(d) {
  var xmlString = '<?xml version="1.0"?>';
  xmlString += '<person>';
  for (var i = 0; i < d.length; i++) {
    var obj = d[i];
    xmlString += '<To>';
    for (key in obj) {
      if (obj.hasOwnProperty(key)) {
        xmlString += '<' + key + '>';
        xmlString += '<![CDATA[' + obj[key] + ']]>';
        xmlString += '</' + key + '>';
      }
    }
    xmlString += '</To>';
  }
  xmlString += '</person>';
  return xmlString;
}

function getQuerystring(paramName){

	var _tempUrl = window.location.search.substring(1); //url에서 처음부터 '?'까지 삭제
	var _tempArray = _tempUrl.split('&'); // '&'을 기준으로 분리하기
	var retVal = ''
	
	for(var i = 0; _tempArray.length; i++) {
		var _keyValuePair = _tempArray[i].split('='); // '=' 을 기준으로 분리하기
		
		if(_keyValuePair[0] == paramName){ // _keyValuePair[0] : 파라미터 명
			// _keyValuePair[1] : 파라미터 값
			retVal = _keyValuePair[1];
			break;
		}
	}
	return retVal;
}


Org.onReady(function() {

  if (Org.config.onlyOneSelect === false) {
    if (Org.config.orgMapData != null && Org.config.orgMapData !== '') {
      if (Org.config.orgMapData[0] === '<') {
        useXML = true;
      }

      var item = [];
      if (!useXML) {
        try {
          item = JSON.parse(Org.config.orgMapData);
        }
        catch (e) {
          useXML = true;
        }
      }

      if (useXML) {
	try {
          item = convertXMLToJSON(Org.config.orgMapData);
	}
	catch (e) {
          item = null;
        }
      }

      if(item != null)
	Org.helper.addSelect(item);
    }
  }
//  var isSearchAll = getQuerystring('searchAll');
//  if (isSearchAll !== typeof undefined && getQuerystring('searchAll') === 'true') {
//	$('#toggle_search_option').show();
//  }
//  else
//  {
	//전사검색 숨김
	$('#toggle_search_option').hide();
//  }

  //다국어 숨김
  $('#btn-language-slide').hide()

  if (Org.config.onlyOneSelect === true) {
    $('#orgGrid').dblclick(function(e) {
      if (Org.config.callback !== null) {
        var result = Org.helper.getGridItems();

        if (Org.config.returnType === 'XML') {
          Org.config.callback(convertJSONToXML(result));
        }
        else {
          var json = JSON.stringify(result);
          Org.config.callback(json);
        }
      }
      window.close();
    });

    if (Org.config.appType === 'DEPT') {
      $('#orgTree').on('dblclick', 'a', function(e) {
        var item = AppManager.controls.tree.getSelectedItem();
        if (Org.config.callback !== null) {
          if (Org.config.returnType === 'XML') {
            Org.config.callback(convertJSONToXML([item.data]));
          }
          else {
            var json = JSON.stringify([item.data]);
            Org.config.callback(json);
          }
        }
        window.close();
      });
    }
  }

  $('#btn-confirm').click(function(e) {
    if (Org.config.callback !== null) {

      if (Org.config.onlyOneSelect === false) {
        var result = Org.helper.getSelectItems();

        if (Org.config.returnType === 'XML') {
          Org.config.callback(convertJSONToXML(result));
        }
        else {
          var json = JSON.stringify(result);
          Org.config.callback(json);
        }
      }
      else {
        if (Org.config.appType === 'DEPT') {
          var item = [Org.helper.getTreeItem()];

          if (Org.config.returnType === 'XML') {
            Org.config.callback(convertJSONToXML(item));
          }
          else {
            var json = JSON.stringify(item);
            Org.config.callback(json);
          }
        }
        else if (Org.config.appType === 'DEPTUSER') {
          var items = Org.helper.getGridItems();

          if (Org.config.returnType === 'XML') {
            Org.config.callback(convertJSONToXML(items));
          }
          else {
            var json = JSON.stringify(items);
            Org.config.callback(json);
          }
        }
        else if (Org.config.appType === 'USER') {
          var items = Org.helper.getGridItems();

          if (Org.config.returnType === 'XML') {
            Org.config.callback(convertJSONToXML(items));
          }
          else {
            var json = JSON.stringify(items);
            Org.config.callback(json);
          }
        }
      }

    }
    window.close();
  });
   $('#btn-cancel').click(function(e) {
	  try{
		  if (Org.config.callback !== null) {
			  //Org.config.callback('CANCEL');
		  }
	  }
	  catch(e){
	  }
    window.close();
  });
});
