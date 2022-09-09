// change default title
AppConfig = AppConfig || {};
AppConfig.title = {
  ko: '메일수신처 지정',
  en: 'Specifying addressee',
  zh: '收件人指定'
};


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
  // oPar.orgmapData {
  // To: [{
  //    DisplayName: string,
  //    EmailAddress: string,
  //  }],
  // Cc: [{
  //    DisplayName: string,
  //    EmailAddress: string,
  //  }],
  // Bcc: [{
  //    DisplayName: string,
  //    EmailAddress: string,
  //  }],
  // }

  // IE
  // if (window.dialogArguments) {
	  // alert('test1');
    // if (typeof window.dialogArguments.oPar !== typeof undefined) {
		// alert('test2');
      // if (typeof window.dialogArguments.oPar.orgmapData !== typeof undefined) {
		  // alert('test3');
        // var orgData = window.dialogArguments.oPar.orgmapData;

        // if (typeof orgData.To !== typeof undefined) {
          // // owa 인터페이스 변경시 같이 변경, 현재는 그냥 사용해도 됨
          // // var to = orgData.To;
          // // for (var i = 0; i < to.length; i++) {
          // //   AppInterface.addTo({
          // //     EmailAddress: to[i].EmailAddress,
          // //     DisplayName: to[i].DisplayName,
          // //   });
          // // }
          // Org.helper.addTo(orgData.To);
        // }
        // if (typeof orgData.Cc !== typeof undefined) {
          // Org.helper.addCc(orgData.Cc);
        // }
        // if (typeof orgData.Bcc !== typeof undefined) {
          // Org.helper.addBcc(orgData.Bcc);
        // }
      // }
    // }
  // }
// // Chrome, etc
  // if (window.top.opener && window.top.opener.OrgArgument){ 
    // if (window.top.opener.OrgArgument.orgmapData) {
      
      // var orgData = window.top.opener.OrgArgument.orgmapData;
      
      // if (typeof orgData.To !== typeof undefined) {
        // Org.helper.addTo(orgData.To);
      // }
      
      // if (typeof orgData.Cc !== typeof undefined) {
        // Org.helper.addCc(orgData.Cc);
      // }
      
      // if (typeof orgData.Bcc !== typeof undefined) {
        // Org.helper.addBcc(orgData.Bcc);
      // }
    // }
  // }

  if(Org != null && Org.config != null && Org.config.orgMapData != null && Org.config.orgMapData !== '') {
 
	var orgData;
	try
	{
		orgData = JSON.parse(Org.config.orgMapData);
	}
	catch(error)
	{
		//alert(Org.config.orgMapData);
		orgData = null;
	}
	
	if(orgData != null && orgData != '')
	{
		if (typeof orgData.To !== typeof undefined ) {
		  // owa 인터페이스 변경시 같이 변경, 현재는 그냥 사용해도 됨
		  // var to = orgData.To;
		  // for (var i = 0; i < to.length; i++) {
		  //   AppInterface.addTo({
		  //     EmailAddress: to[i].EmailAddress,
		  //     DisplayName: to[i].DisplayName,
		  //   });
		  // }
		  Org.helper.addTo(orgData.To);
		}
		if (typeof orgData.Cc !== typeof undefined) {
		  Org.helper.addCc(orgData.Cc);
		}
		if (typeof orgData.Bcc !== typeof undefined) {
		  Org.helper.addBcc(orgData.Bcc);
		}
	}
  }
	
  //티끌은 내부 사용자만 사용
  //if (getQuerystring('searchAll') !== 'true') {
	$('#toggle_search_option').hide();
  //}

  //다국어 숨김
  $('#btn-language-slide').hide();


    $('#btn-confirm').click(function(e) {
    if (Org.config.callback !== null) {


      
      if (Org.config.onlyOneSelect === false) {
        //var result = Org.helper.getSelectItems();
	    var result = {
	      To: Org.helper.getTo(),
	      Cc: Org.helper.getCc(),
	      Bcc: Org.helper.getBcc()
    		};

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

/*
  $('#btn-confirm').click(function(e) {
    // result: {
    //  To: [{
    //    EntryType: number,
    //    DisplayName: string,
    //    EmailAddress: string,
    //  }],
    //  Cc: [{
    //    EntryType: number,
    //    DisplayName: string,
    //    EmailAddress: string,
    //  }],
    //  Bcc: [{
    //    EntryType: number,
    //    DisplayName: string,
    //    EmailAddress: string,
    //  }],
    // }
    var result = {
      To: Org.helper.getTo(),
      Cc: Org.helper.getCc(),
      Bcc: Org.helper.getBcc(),
    };

    if (Org.helper.isIE()) {
      window.returnValue = result;
    }
    else {
      window.opener.returnValue = result;
    }
    window.close();
  });

*/
  $('#btn-cancel').click(function(e) {
    if (Org.helper.isIE()) {
      window.returnValue = null;
    }
    else {
      window.opener.returnValue = null;
    }
    window.close();
  });
});
