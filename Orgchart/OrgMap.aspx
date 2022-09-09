<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgMap.aspx.cs" Inherits="Mik.OrgChart2.Web.OrgMap" %>

<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title></title>
  <script>
    AppConfig = {
      presenceEnable: <%=this.PresenceEnable ? "true" : "false" %>,
      lang: '<%=this.Lang %>',
      info: <%=this.MyInfo %>,
      addins: <%=this.GetJSON(this.AddinList) %>,
      section: '<%=this.Section %>',
    };

      function getRequests() {
          var s1 = location.search.substring(1, location.search.length).split('&'),
              r = {}, s2, i;
          for (i = 0; i < s1.length; i += 1) {
              s2 = s1[i].split('=');
              r[decodeURIComponent(s2[0]).toLowerCase()] = decodeURIComponent(s2[1]);
          }
          return r;
      };
      var params = getRequests();
      if (params['newdomain']) {
          window.document.domain = params['newdomain'];
      }

  </script>
  <script src="<%= GetClientFileMap("public/js/jquery.min.js") %>"></script>
  <% if (this.DevMap == true) { %>
  <link rel="stylesheet" href="<%= GetClientFileMap("public/css/main.css") %>">
  <script src="<%= GetClientFileMap("public/js/main.js") %>"></script>
  <% } else { %>
  <link rel="stylesheet" href="<%= GetClientFileMap("public/css/main.min.css") %>">
  <script src="<%= GetClientFileMap("public/js/main.min.js") %>"></script>
  <% } %>
  <script src="<%= GetClientFileMap("public/js/addin.config.js") %>"></script>
  <% if (this.Section == "MAIL") { %>
  <% if (string.IsNullOrEmpty(this.From)) { %>
  <script src="<%= GetClientFileMap("public/js/addin.mail.js") %>"></script>
  <% } else if (this.From == "outlook") { %>
  <script src="<%= GetClientFileMap("public/js/addin.outlook.js") %>"></script>
  <% } %>
  <% } else if (this.Section == "APP") { %>
  <script src="<%= GetClientFileMap("public/js/addin.app.js") %>"></script>
  <% } %>

  <% if (this.AddinList.Contains("MailGroup")) { %>
  <script src="<%= GetClientFileMap("public/js/addin.mailgroup.js") %>"></script>
  <% } %>
  <% foreach (var plugin in JsPluginList) { %>
  <script src="<%= GetClientFileMap("public/js/plugin/" + plugin) %>"></script>
  <% } %>
  <% foreach (var extension in ExtensionList) { %>
  <script src="<%= GetClientFileMap("extension/" + extension) %>"></script>
  <% } %>
  <script src="<%= GetClientFileMap("public/js/addin.lang.js") %>"></script>


</head>
<body>
  <div class="org_window orgchart-page-container" style="min-height: 540px;">
    <!-- HEAD START -->
    <div class="org_header">
      <div class="left_area">
        <div id="org_title" class="pop_title"><%= DefaultTitle %></div>
      </div>
      <div id="area-mode-selection" class="right_area">

        <% if (this.Section == "MAIL") { %>
        <div class="toggle_selection_wrap">
          <div id="select-view-mode" class="selection_box">
            <div data-value="dept" class="select-toggle arrow">조직도</div>
            <ul class="select-toggle-options">
              <li data-value="dept">조직도</li>
              <li data-value="contact">개인주소록</li>
            </ul>
          </div>
        </div>
        <% } %>
        <div id="toggle_search_option" class="toggle_selection_wrap">
          <div id="select-search-mode" class="selection_box">
            <div data-value="current" class="select-toggle arrow">자사 검색</div>
            <ul class="select-toggle-options">
              <li data-value="total">전사 검색</li>
              <li data-value="current">자사 검색</li>
            </ul>
          </div>
        </div>

        <div id="search-box" class="srch_wrap">
          <input id="input-search" type="text" name="searchquery" placeholder="검색어를 입력하세요." />
          <button id="btn-search" class="btn_org_srch">
            <span class="ico_popup ico_popup_org_srch"></span>
          </button>
        </div>
        <div class="header_lang" style="margin-left: 5px;">
          <button id="btn-language-slide" class="btn_lang btn_slidedown lang_anchor">KOR</button>
          <ul id="ul-language-slide" class="lang_list box_slidedown">
          </ul>
        </div>
      </div>
    </div>
    <!-- HEAD END -->

    <!-- TOOLBAR START -->
    <div id="pnl_toolbar" class="org_toolbar">
      <div id="sel-company-box" style="width: 140px; display: inline-block;">
        <div id="sel_company" class="select" style="width: 100%;">
          <div class="select-styled arrow"></div>
          <ul class="select-options" style="max-height: 300px; overflow: auto;">
          </ul>
        </div>
      </div>

      <div id="sel-child-company-box" style="width: 140px; display: none;">
        <div id="sel-child-company" class="select" style="width: 100%;">
          <div class="select-styled arrow"></div>
          <ul class="select-options" style="max-height: 300px; overflow: auto;">
          </ul>
        </div>
      </div>

    </div>
    <!-- TOOLBAR END -->

    <!-- CONTENT START -->
    <div id="pnl_content" class="org_content orgchart-panel-container">
      <!-- TREE START -->
      <div id="pnl_content_tree" class="con_vert_pnl orgchart-panel-left" style="width: 210px; min-width: 200px;">
        <div class="pnl_header">
          <span id="tree-panel-header" class="pnl_title" style="margin-top: 2px;">조직도</span>
        </div>
        <div id="pnl-dept-tree" class="pnl_body pnl_tree">

          <div id="dept-search-container" class="dept_search" style="width: 100%; height: 33px;">

            <div style="width: 100%; height: 3px;"></div>
            <div class="search-select-editable" style="">
              <div class="input-group">
                <ul id="select-dept-search" tabindex="1"></ul>
                <div class="input-wrap">
                  <input id="input-dept-search" type="text" value="" placeholder="부서를 입력하세요." />
                </div>
                <%--<div class="flow-btn-wrap">
                  <button id="btn-dept-search-cancel" class="btn-search-cancel">
                    <span class="ico_popup ico_cancel"></span>
                  </button>
                </div>--%>
              </div>

              <div class="btn-group">
                <button id="btn-dept-search" class="btn-search">
                  <span class="ico_popup ico_popup_org_srch"></span>
                </button>
              </div>
            </div>

          </div>
          <div id="orgTree" class="set_scroll dept_tree" style="width: 100%; height: calc(100% - 33px); overflow: auto; position:relative; "></div>
        </div>
      </div>
      <!-- TREE END -->

      <div id="resizer_tree" class="con_vert_sizer orgchart-splitter-left" style="width: 5px;"></div>
      <div class="orgchart-splitter-shadow-left" style="width: 3px;"></div>

      <!-- GRID START -->
      <div id="pnl_content_grid" class="con_vert_pnl orgchart-panel-grid" style="width: calc(100% - 420px); min-width: 300px;">
        <div class="pnl_header">
          <span id="grid-panel-header" class="pnl_title"></span>
          <span id="grid-panel-total" class="total">총 <b>0</b>명</span>
        </div>
        <div class="pnl_body">

          <div id="orgGrid" class="pnl_grid" style="overflow: hidden;"></div>

          <div class="pnl_paginate">
            <!-- PAGINATE START -->
            <div class="paginate" style="text-align: center;">
              <div class="right_area">
                <span class="paginate_txt"></span>
              </div>
              <div class="paginate_list">
                <a id="btn-page-first" href="#" class="paginate_btn paginate_btn_fst">
                  <span class="blind">처음으로</span>
                </a>
                <a id="btn-page-prev" href="#" class="paginate_btn paginate_btn_prev">
                  <span class="blind">이전으로</span>
                </a>
                <input id="input-current-page" type="text" class="paginate_num_input" value="1" />
                <span id="txt-total-page" class="paginate_total">/ 1</span>
                <a id="btn-page-next" href="#" class="paginate_btn paginate_btn_next">
                  <span class="blind">다음으로</span>
                </a>
                <a id="btn-page-last" href="#" class="paginate_btn paginate_btn_lst">
                  <span class="blind">마지막으로</span>
                </a>
                <button id="btn-page-refresh" class="btn_refresh">
                  <span class="ico_popup ico_popup_refresh"></span>
                </button>
              </div>
            </div>
            <!-- PAGINATE END -->
          </div>
        </div>
      </div>
      <!-- GRID END -->

      <div id="resizer_list" class="con_vert_sizer orgchart-splitter-right" style="width: 5px;"></div>
      <div class="orgchart-splitter-shadow-right" style="width: 3px;"></div>

      <!-- LIST START -->
      <div id="pnl_content_list" class="con_vert_pnl orgchart-panel-right" style="width: 200px; min-width: 200px;">
        <div class="pnl_header">
          <span id="detail-panel-header" class="pnl_title">상세정보</span>
        </div>
        <div class="pnl_body detail_list">

          <div id="pnl-list-to" class="list_body" style="height: 40%;">
            <div style="height: calc(100% - 36px); background-color: #ffffff;">
              <div id="orgToList" style="height: 100%;"></div>
            </div>
            <div class="list_footer">
              <div class="col_button" style="text-align: right;">
                <button id="btn-to-add" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-to-add" class="btn_txt">추가</span>
                </button>
                <button id="btn-to-remove" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-to-remove" class="btn_txt">제거</span>
                </button>
                <button id="btn-to-remove-all" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-to-remove-all" class="btn_txt">모두제거</span>
                </button>
              </div>
            </div>
          </div>

          <div id="pnl-list-cc" class="list_body" style="height: 33%;">
            <div style="height: calc(100% - 36px); background-color: #ffffff;">
              <div id="orgCcList" style="height: 100%;"></div>
            </div>
            <div class="list_footer">
              <div class="col_button" style="text-align: right;">
                <button id="btn-cc-add" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-cc-add" class="btn_txt">추가</span>
                </button>
                <button id="btn-cc-remove" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-cc-remove" class="btn_txt">제거</span>
                </button>
                <button id="btn-cc-remove-all" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-cc-remove-all" class="btn_txt">모두제거</span>
                </button>
              </div>
            </div>
          </div>

          <div id="pnl-list-bcc" class="list_body" style="height: 27%;">
            <div style="height: calc(100% - 36px); background-color: #ffffff;">
              <div id="orgBccList" style="height: 100%;"></div>
            </div>
            <div class="list_footer">
              <div class="col_button" style="text-align: right;">
                <button id="btn-bcc-add" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-bcc-add" class="btn_txt">추가</span>
                </button>
                <button id="btn-bcc-remove" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-bcc-remove" class="btn_txt">제거</span>
                </button>
                <button id="btn-bcc-remove-all" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-bcc-remove-all" class="btn_txt">모두제거</span>
                </button>
              </div>
            </div>
          </div>

          <div id="pnl-list-sel" class="list_body" style="height: 100%; display: none;">
            <div style="height: calc(100% - 36px); background-color: #ffffff;">
              <div id="orgSelectList" style="height: 100%;"></div>
            </div>
            <div class="list_footer">
              <div class="col_button" style="text-align: right;">
                <button id="btn-sel-add" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-sel-add" class="btn_txt">추가</span>
                </button>
                <button id="btn-sel-remove" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-sel-remove" class="btn_txt">제거</span>
                </button>
                <button id="btn-sel-remove-all" class="btn_type_f btn_color_03 btn_ff_compt">
                  <span id="btn-txt-sel-remove-all" class="btn_txt">모두제거</span>
                </button>
              </div>
            </div>
          </div>

        </div>
      </div>
      <!-- LIST END -->
    </div>
    <!-- CONTENT END -->

    <!-- FOOTER START -->
    <div class="org_footer">
      <div style="display: table; width: 100%; padding: 7px 0;">
        <div style="display: table-cell; text-align: center;">
          <button id="btn-confirm" class="btn_type_a btn_color_04">
            <span id="btn-txt-confirm" class="btn_txt">확인</span>
          </button>
          <button id="btn-cancel" class="btn_type_a btn_color_03">
            <span id="btn-txt-cancel" class="btn_txt">취소</span>
          </button>
        </div>
      </div>
    </div>
    <!-- FOOTER END -->

  </div>

  <div id="tooltip-drag" style="padding: 5px; position: fixed; display: none; background-color: white; border: 1px solid #cfcfd0; white-space: nowrap; vertical-align: middle; text-align: center;">
    <img id="tooltip-drag-img" src="" style="vertical-align: middle;" /><span id="tooltip-drag-txt" style="display: inline-block; line-height: normal; vertical-align: middle;"></span>
  </div>
  <div id="tooltip-data" style="padding: 5px; position: fixed; display: none; background-color: white; border: 1px solid #cfcfd0; white-space: nowrap; vertical-align: middle;">
    <span id="tooltip-data-txt" style="display: inline-block; line-height: normal; vertical-align: middle;"></span>
  </div>
  <div class="pop_wrap">
    <div class="bg"></div>
    <div id="popup-container" class="pop_container pop_alert">
      <button class="btn_close">
        <span class="ico_popup ico_popup_alert_close"></span>
      </button>
      <div class="pop_body">
        <div class="top_area">
          <span class="ico_popup ico_popup_alert_caution"></span>
        </div>
        <div class="bottom_area">
          <div id="popup-title" class="alert_title"></div>
          <div id="popup-txt" class="alert_desc"></div>
        </div>
      </div>
    </div>
    <div class="spinner">
      <div class="rect_wrap">
        <div class="rect rect1"></div>
        <div class="rect rect2"></div>
        <div class="rect rect3"></div>
        <div class="rect rect4"></div>
        <div class="rect rect5"></div>
      </div>
      <!--<div class="spinner_txt">Loading...</div>-->
    </div>
  </div>

  <script type="text/javascript">
    <%= BodyScriptBlock %>
  </script>
  <style>
	#orgGrid thead tr th:nth-of-type(2){
	text-align: center;
	}

	#orgGrid tbody tr td:nth-of-type(1) {
	text-align: left;
	}

	#orgSelectList thead tr th:nth-of-type(2){
	text-align: left;
	}

	#orgSelectList tbody tr td:nth-of-type(1){
	text-align: left;
	}


  </style>

</body>
</html>
