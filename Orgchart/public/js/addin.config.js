Org.onLoad(function() {
if (window.AppConfig.section == 'MAIL')
{
//기본
Org.setting.gridData.dept = [
{prop: 'name', width: 100},
{prop: 'job', width: 80},
{prop: 'email', width: 0}
];
//전사검색 결과
Org.setting.gridData.totalSearch = [
{prop: 'name', width: 100}, 
{prop: 'job', width: 80},
{prop: 'dept', width: 0},
{prop: 'email', width: 0},
{prop: 'company', width: 0}
];
//사내 검색 결과
Org.setting.gridData.search = [
{prop: 'name', width: 100},
{prop: 'job', width: 80},
{prop: 'dept', width: 0},
{prop: 'email', width: 0}
];
//개인주소록
Org.setting.gridData.contact = [
{prop: 'name', width: 100}, 
{prop: 'job', width: 80},
{prop: 'dept', width: 0},
{prop: 'email', width: 0},
{prop: 'company', width: 0}
];
}
else
{
//기본
Org.setting.gridData.dept = [
{prop: 'name', width: 100},
{prop: 'job', width: 80},
{prop: 'officeTel', width: 0}
];
//전사검색 결과
Org.setting.gridData.totalSearch = [
{prop: 'name', width: 100},
{prop: 'job', width: 80},
{prop: 'dept', width: 0},
{prop: 'company', width: 0}
];
//사내 검색 결과
Org.setting.gridData.search = [
{prop: 'name', width: 100},
{prop: 'job', width: 80},
{prop: 'dept', width: 0},
{prop: 'officeTel', width: 0}
];
}


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

//dept, title(직위), name, mobileTel, officeTel, rank, job(직책), duty, email, company, nick

Org.onReady(function() {
  Org.setting.pageCount = 50;
  Org.config.warnContactGroup = true;

});


Org.onConfig(function(){
Org.config.oneClickToggle = true;
});
