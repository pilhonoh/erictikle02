Org.onReady(function() {

  if (window.external) {
    if (window.external.GetRecipients) {
      var orgData = window.external.GetRecipients();
      if (orgData && typeof orgData === 'string') {
        var j = JSON.parse(orgData);

        if (typeof j.To !== typeof undefined) {
          // owa 인터페이스 변경시 같이 변경, 현재는 그냥 사용해도 됨
          // var to = orgData.To;
          // for (var i = 0; i < to.length; i++) {
          //   AppInterface.addTo({
          //     EmailAddress: to[i].EmailAddress,
          //     DisplayName: to[i].DisplayName,
          //   });
          // }
          Org.helper.addTo(j.To);
        }
        if (typeof j.Cc !== typeof undefined) {
          Org.helper.addCc(j.Cc);
        }
        if (typeof j.Bcc !== typeof undefined) {
          Org.helper.addBcc(j.Bcc);
        }
      }
    }
  }

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
    var jsonString = JSON.stringify(result);
    window.external.Ok(jsonString);
  });
  $('#btn-cancel').click(function(e) {
    window.external.Cancel();
  });
});
