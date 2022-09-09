Org.addins.mailGroup = {
  select: null,
  setPlaceHolder: function(langCode) {
    var placeholder = '그룹선택';
    if (langCode === 'en') {
      placeholder = 'Select Group';
    }
    else if (langCode === 'cn' || langCode === 'zh') {
      placeholder = '选择组';
    }
    Org.addins.mailGroup.select.setPlaceHolder(placeholder);
  },
  fetchMailGroup: function(childCompanyCode, fn, fe) {
    $.ajax({
      type: 'GET',
      dataType: 'json',
      url: 'Rest.aspx?action=GetCustomMailGroup&childCompanyCode=' + childCompanyCode,
    })
    .done(fn)
    .fail(fe);
  },
  fetchMailGroupMember: function(groupCode, success, fail) {
    if (groupCode === '') {
      return;
    }

    Org.app.setViewMode('mailgroup');
    Org.controls.tree.unselect();
    var url = 'Rest.aspx?action=GetMembers&mode=custommailgroup&groupCode=' + groupCode + '&langCode=' + Org.app.language;
    url += '&limit=' + Org.app.paging.limit + '&page=' + Org.app.paging.currentPage;
    Org.controls.grid.fetchData(url, {
      method: 'GET',
      redirect: 'follow',
      mode: 'cors',
      credentials: 'same-origin',
    }, function() {
      // before
      Org.component.spinner.show();
    }, function(j) {
      // success
      Org.app.setPaging(j.CurrentPage, j.TotalPage);
      $('#grid-panel-total').html(Org.multiLang.get(Org.app.language, 'ui.userCount').replace('$0', j.Items.length));

      var colData = Org.app.getGridApplier('mailgroup');
      var items = [];
      for (var i = 0; i < j.Items.length; i++) {
        var item = Org.app.applyGridData(colData, j.Items[i]);
        items.push(item);
      }

      var headers = Org.app.applyGridHeader(colData);
      Org.controls.grid.setHeader(headers);

      Org.component.spinner.hide();

      if (success) {
        success();
      }

      return items;
    }, function(err) {
      // fail
      Org.component.spinner.close();

      if (fail) {
        fail();
      }
    });
  }
};

Org.onLoad(function() {
  if (window.AppConfig.info.Companies.Items.length === 1) {
    if (window.AppConfig.info.Companies.Items[0].HasChildCompany === false) {
      var isMailGroup = false;
      for (var i = 0; i < window.AppConfig.addins.length; i++) {
        if (window.AppConfig.addins[i] === 'MailGroup') {
          isMailGroup = true;
          break;
        }
      }
      if (isMailGroup === false) {
        Org.helper.hideToolbar();
      }
    }
  }
});

Org.onLoad(function() {
  Org.setting.gridData.mailgroup = [
    {prop: 'title', width: 60},
    {prop: 'name', width: 0},
    {prop: 'mobileTel', width: 110},
    {prop: 'officeTel', width: 110},
  ];
});

Org.onReady(function() {
  Org.addins.mailGroup.select = Org.component.StyledSelect.create('#pnl_toolbar', {
    direction: 'append',
    container: {
      style: 'width: 140px; display: inline-block',
    },
    select: {
      arrow: true,
    },
  });

  Org.on('viewChange', function(viewMode) {
    if (viewMode !== 'mailgroup') {
      Org.addins.mailGroup.select.reset();
    }
  });

  Org.on('companySelect', function(e) {
    if (e.value !== Org.app.userInfo.companyCode) {
      Org.addins.mailGroup.select.reset();
      Org.addins.mailGroup.select.$el.hide();
    }
    else {
      Org.addins.mailGroup.select.$el.css('display', 'inline-block');
    }
  });

  Org.on('languageChange', function(langCode) {
    Org.addins.mailGroup.setPlaceHolder(langCode);
    Org.addins.mailGroup.select.reset();
  });

  Org.addins.mailGroup.select.change(function(e) {
    Org.app.setPaging(1, 1);
    Org.addins.mailGroup.fetchMailGroupMember(e.value, function() {
      $('#grid-panel-header').text(e.text);
    }, function() { });

    if (e.value === '') {
      return;
    }
  });
  $('#btn-page-first').click(function(e) {
    e.preventDefault();
    if (Org.app.viewMode === 'mailgroup') {
      if (Org.app.paging.currentPage === 0 || Org.app.paging.currentPage === 1) return;
      if (Org.app.paging.totalPage === 0) return;
      Org.app.paging.currentPage = 1;
      Org.addins.mailGroup.fetchMailGroupMember(Org.addins.mailGroup.select.current);
    }
  });
  $('#btn-page-last').click(function(e) {
    e.preventDefault();
    if (Org.app.viewMode === 'mailgroup') {
      if (Org.app.paging.currentPage === Org.app.paging.totalPage) return;
      Org.app.paging.currentPage = Org.app.paging.totalPage;
      Org.addins.mailGroup.fetchMailGroupMember(Org.addins.mailGroup.select.current);
    }
  });
  $('#btn-page-prev').click(function(e) {
    e.preventDefault();
    if (Org.app.viewMode === 'mailgroup') {
      if (Org.app.paging.currentPage === 0 || Org.app.paging.currentPage === 1) return;
      if (Org.app.paging.totalPage === 0 || Org.app.paging.totalPage === 1) return;
      Org.app.paging.currentPage = Org.app.paging.currentPage - 1;
      Org.addins.mailGroup.fetchMailGroupMember(Org.addins.mailGroup.select.current);
    }
  });
  $('#btn-page-next').click(function(e) {
    e.preventDefault();
    if (Org.app.viewMode === 'mailgroup') {
      if (Org.app.paging.totalPage === 0 || Org.app.paging.totalPage === 1) return;
      if (Org.app.paging.currentPage === Org.app.paging.totalPage) return;
      Org.app.paging.currentPage = Org.app.paging.currentPage + 1;
      Org.addins.mailGroup.fetchMailGroupMember(Org.addins.mailGroup.select.current);
    }
  });
  $('#btn-page-refresh').click(function(e) {
    e.preventDefault();
    if (Org.app.viewMode === 'mailgroup') {
      Org.addins.mailGroup.fetchMailGroupMember(Org.addins.mailGroup.select.current);
    }
  });

  Org.addins.mailGroup.setPlaceHolder(Org.app.language);

  Org.addins.mailGroup.fetchMailGroup(Org.app.userInfo.childCompanyCode, function(j) {
    // success
    for (var i = 0; i < j.Items.length; i++) {
      var item = j.Items[i];
      Org.addins.mailGroup.select.addOption({
        text: item.DisplayName,
        value: item.GroupCode,
      });
    }
  }, function(err) {
    // fail
  });
});
