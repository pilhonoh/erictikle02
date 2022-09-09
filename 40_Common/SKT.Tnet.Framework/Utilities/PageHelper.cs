using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Diagnostics;
using SKT.Tnet.Framework.Security;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// 페이지 관련 Utility 처리 클래스
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 네오플러스, 정재혁 <br/>
    /// # 작성일 : 2015년 04월 01일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 01일, 네오플러스, 정재혁 최초작성 <br/>
    /// </Remarks>
    public class PageHelper
    {
        #region [ ##. Field & Const .## ]

        /// <summary>
        /// 램덤 객체
        /// </summary>
        private Random m_Random;

        /// <summary>
        /// 페이지 객체
        /// </summary>
        private Page m_Page;

        #endregion [ ##. Field & Const .## ]

        #region [ ##. Constructor .## ]

        /// <summary>
        /// 생성자.
        /// </summary>
        /// <param name="page"></param>
        public PageHelper(Page page)
        {
            m_Page = page;
            m_Random = new Random(unchecked((int)DateTime.Now.Ticks));
        }

        #endregion [ ##. Constructor .## ]

        /// <summary>
        /// 현재 페이지 객체의 모든 자원을 해제한다.
        /// </summary>
        public void Dispose()
        {
            if (m_Page != null)
            { 
                m_Page.Dispose();
            }
        }

        #region [  ##. DropDownList Method .## ]

        /// <summary>
        /// 드롭다운리스트에 검색된 데이타를 바인딩한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체</param>
        /// <param name="dsReceive">검색결과를 담은 DataSet 객체</param>
        /// <param name="text">바인딩할 Text 문자열</param>
        /// <param name="value">바인딩할 Value 문자열</param>
        /// <returns>드롭다운리스트에 바인딩된 레코드개수</returns>
        public int DataBindDropDownList(DropDownList ddlReceive, DataSet dsReceive, string text, string value)
        {
            return this.DataBindDropDownList(ddlReceive, dsReceive, text, value, null);
        }

        /// <summary>
        /// 드롭다운리스트에 검색된 데이타를 바인딩한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체</param>
        /// <param name="dsReceive">검색결과를 담은 DataTable 객체</param>
        /// <param name="text">바인딩할 Text 문자열</param>
        /// <param name="value">바인딩할 Value 문자열</param>
        /// <returns>드롭다운리스트에 바인딩된 레코드개수</returns>
        public int DataBindDropDownList(DropDownList ddlReceive, DataTable dtReceive, string text, string value)
        {
            return this.DataBindDropDownList(ddlReceive, dtReceive, text, value, null);
        }

        /// <summary>
        /// 드롭다운리스트에 검색된 데이타를 바인딩한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체</param>
        /// <param name="dsReceive">검색결과를 담은 DataSet 객체</param>
        /// <param name="text">바인딩할 Text 문자열</param>
        /// <param name="value">바인딩할 Value 문자열</param>
        /// <param name="defaultItem">드롭다운리스트의 첫번째 아이템에 출력할 문자열</param>
        /// <returns>드롭다운리스트에 바인딩된 레코드개수</returns>
        public int DataBindDropDownList(DropDownList ddlReceive, DataSet dsReceive, string text, string value, string defaultItem)
        {
            int iReturnCount = 0;
            if (ddlReceive != null && dsReceive != null)
            {
                //DB에서 검색된 데이타를 ddlReceive 컨트롤에 바인딩합니다.
                //DropDownList 컨트롤에 사용될 데이타소스입니다.
                ddlReceive.DataSource = dsReceive;
                //DropDownList 컨트롤에 출력될 Text 입니다.
                ddlReceive.DataTextField = text;
                //DropDownList 컨트롤에 출력될 Value 입니다.
                ddlReceive.DataValueField = value;
                //DropDownList 컨트롤에 바인딩합니다.
                ddlReceive.DataBind();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    //ddlReceive 맨 위에 ddl 디폴트 아이템을 추가합니다.
                    ddlReceive.Items.Insert(0, defaultItem);
                }

                //드롭다운리스트에 바인딩된 레코드개수
                iReturnCount = ddlReceive.Items.Count;

                //객체를 해제합니다
                dsReceive.Dispose();
                ddlReceive.Dispose();
                dsReceive = null;
                ddlReceive = null;
            }
            return iReturnCount;
        }

        /// <summary>
        /// 드롭다운리스트에 검색된 데이타를 바인딩한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체</param>
        /// <param name="dtReceive">검색결과를 담은 DataTable 객체</param>
        /// <param name="text">바인딩할 Text 문자열</param>
        /// <param name="value">바인딩할 Value 문자열</param>
        /// <param name="defaultItem">드롭다운리스트의 첫번째 아이템에 출력할 문자열</param>
        /// <returns>드롭다운리스트에 바인딩된 레코드개수</returns>
        public int DataBindDropDownList(DropDownList ddlReceive, DataTable dtReceive, string text, string value, string defaultItem)
        {
            int iReturnCount = 0;
            if (ddlReceive != null && dtReceive != null)
            {
                //DB에서 검색된 데이타를 ddlReceive 컨트롤에 바인딩합니다.
                //DropDownList 컨트롤에 사용될 데이타소스입니다.
                ddlReceive.DataSource = dtReceive;
                //DropDownList 컨트롤에 출력될 Text 입니다.
                ddlReceive.DataTextField = text;
                //DropDownList 컨트롤에 출력될 Value 입니다.
                ddlReceive.DataValueField = value;
                //DropDownList 컨트롤에 바인딩합니다.
                ddlReceive.DataBind();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    //ddlReceive 맨 위에 ddl 디폴트 아이템을 추가합니다.
                    ddlReceive.Items.Insert(0, defaultItem);
                }

                //드롭다운리스트에 바인딩된 레코드개수
                iReturnCount = ddlReceive.Items.Count;

                //객체를 해제합니다
                dtReceive.Dispose();
                ddlReceive.Dispose();
                dtReceive = null;
                ddlReceive = null;
            }
            return iReturnCount;
        }

        /// <summary>
        /// string 타입의 Value 값을 디롭다운리스트의 디폴트아이템으로 출력한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체</param>
        /// <param name="value">Value 값</param>
        public void FindByValueDropDownList(DropDownList ddlReceive, string value)
        {
            if (ddlReceive != null)
            {
                ddlReceive.SelectedIndex = ddlReceive.Items.IndexOf(ddlReceive.Items.FindByValue(value));
                ddlReceive.Dispose();
                ddlReceive = null;
            }
        }

        /// <summary>
        /// string 타입의 Text 값을 디롭다운리스트의 디폴트아이템으로 출력한다.
        /// </summary>
        /// <param name="ddlReceive">출력할 DropDownList 객체입니다.</param>
        /// <param name="text">Text 값입니다.</param>
        public void FindByTextDropDownList(DropDownList ddlReceive, string text)
        {
            if (ddlReceive != null)
            {
                ddlReceive.SelectedIndex = ddlReceive.Items.IndexOf(ddlReceive.Items.FindByText(text));
                ddlReceive.Dispose();
                ddlReceive = null;
            }
        }

        #endregion [  ##. DropDownList Method .## ]

        #region [ ##. ClientScript Execute Method .## ]

        /// <summary>
        /// 페이지 로딩 후 자바스크립트 Alert 메세지를 출력한다.
        /// </summary>
        /// <param name="message">Alert에 출력할 메시지</param>
        public void AlertMessage(string message)
        {
            this.AlertMessage(message, false, false, "");
        }

        /// <summary>
        /// 자바스크립트 Alert 메세지를 출력한다.
        /// </summary>
        /// <param name="message">Alert에 출력할 메시지</param>
        /// <param name="bIsBlank">Alert 창 백그라운드에 공백을 출력할지 여부</param>
        public void AlertMessage(string message, bool bIsBlank)
        {
            this.AlertMessage(message, bIsBlank, false, "");
        }

        /// <summary>
        /// 자바스크립트 Alert 메세지출력, 창닫기유무, Alert 창 백그라운드에 공백출력유무등의 기능을 제공한다.
        /// </summary>
        /// <param name="message">Alert에 출력할 메시지</param>
        /// <param name="bIsBlank">Alert 창 백그라운드에 공백을 출력할지 여부</param>
        /// <param name="bIsClose">메시지 출력후 창닫기 여부</param>
        public void AlertMessage(string message, bool bIsBlank, bool bIsClose)
        {
            this.AlertMessage(message, bIsBlank, bIsClose, "");
        }

        /// <summary>
        /// 자바스크립트 Alert 메세지를 출력후 페이지 이동한다.
        /// </summary>
        /// <param name="message">Alert에 출력할 메시지</param>
        /// <param name="bIsBlank">Alert 창 백그라운드에 공백을 출력할지 여부</param>
        /// <param name="redirectUrl">메시지 출력후 페이지 이동할 Url</param>
        public void AlertMessage(string message, bool bIsBlank, string redirectUrl)
        {
            string sScriptCommand = "document.location.href = '" + redirectUrl + "';";
            this.AlertMessage(message, bIsBlank, false, sScriptCommand);
        }

        /// <summary>
        /// 자바스크립트 Alert 메세지출력, 창닫기유무, Alert 창 백그라운드에 공백출력유무,
        /// 기타 자바스크립트 소스추가 유뮤를 자유롭게 선택한다.
        /// </summary>
        /// <param name="message">Alert에 출력할 메시지</param>
        /// <param name="bIsBlank">Alert 창 백그라운드에 공백을 출력할지의 여부</param>
        /// <param name="bIsClose">메시지 출력후 창닫기 여부</param>
        /// <param name="commandScript">메시지 출력후 추가 실행할 자바스크립트 소스</param>
        public void AlertMessage(string message, bool bIsBlank, bool bIsClose, string commandScript)
        {
            string msMSG = "";
            msMSG += "alert('" + message.Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\r") + "');";
            msMSG += commandScript;
            msMSG += bIsClose ? "self.close();" : "";

            //자바스크립트 실행
            this.ExecuteScript(msMSG, bIsBlank);
        }

        /// <summary>
        /// 자바스크립트로 페이지 이동합니다.
        /// </summary>
        /// <param name="redirectUrl">이동할 페이지 Url</param>
        public void MovePage(string redirectUrl)
        {
            string sScript = "";
            sScript += "document.location.href = '" + redirectUrl + "';";
            this.ExecuteScript(sScript, true);
        }

        /// <summary>
        /// 자바스크립트로 현재 페이지를 Refresh 한다.
        /// </summary>
        public void RefreshPage()
        {
            string sScript = "";
            sScript += "document.location.href = document.location.href;";
            this.ExecuteScript(sScript, true);
        }

        /// <summary>
        /// 페이지 출력후 자바스크립트 명령문을 처리한다.
        /// </summary>
        /// <param name="script">실행할 클라이언트 스크립트 문자열</param>
        public void ExecuteScript(string script)
        {
            script = "<script>" + script + "</script>";
            //현재시각과 랜덤값 더한 것을 sriptID 의 key 로 설정합니다.
            string sScriptID = DateTime.Now.ToString("yyyyMMddhhmmssfff") + m_Random.Next().ToString();
            if (!m_Page.ClientScript.IsClientScriptBlockRegistered(sScriptID))
                m_Page.ClientScript.RegisterStartupScript(this.GetType(), sScriptID, script);
        }

        /// <summary>
        /// 자바스크립트 명령문을 처리한다.
        /// </summary>
        /// <param name="script">실행할 클라이언트 스크립트 문자열</param>
        /// <param name="bIsRunBeforePageLoad">페이지출력전에 실행 여부 (출력 전 실행하려면 true로 설정)</param>
        public void ExecuteScript(string script, bool bIsRunBeforePageLoad)
        {
            script = "<script>" + script + "</script>";
            //페이지출력 전에 수행합니다.
            if (bIsRunBeforePageLoad)
            {
                HttpContext.Current.Response.Write(script);
            }
            //페이지출력후에 수행합니다.
            else
            {
                //현재시각과 랜덤값 더한 것을 sriptID 의 key 로 설정합니다.
                string sScriptID = DateTime.Now.ToString("yyyyMMddhhmmssfff") + m_Random.Next().ToString();
                if (!m_Page.ClientScript.IsClientScriptBlockRegistered(sScriptID))
                    m_Page.ClientScript.RegisterStartupScript(this.GetType(), sScriptID, script);
            }
        }

        #endregion [ ##. ClientScript Execute Method .## ]

        #region [ ##. Iframe Method .## ]

        /// <summary>
        /// C#(서버단)으로 iframe 을 호출(실행)합니다.
        /// </summary>
        /// <param name="iFrameID">iFrame Name</param>
        /// <param name="url">iFrame 에 넘겨줄 URL</param>
        public void ExecuteFrame(string iFrameID, string url)
        {
            //iFrame 호출
            string sScript = iFrameID + ".document.location.href='" + url + "';";

            if (m_Page != null)
            {
                //클라이언트 스크립드 실행
                this.ExecuteScript(sScript);
            }
        }

        #endregion [ ##. Iframe Method .## ]

        #region [ ##. Modal Popup Method .## ]

        /// <summary>
        /// Modal Popup Dialog에 리턴값을 넘겨주고 창을 닫는다.
        /// </summary>
        /// <param name="rtnValue">Modal Popup Dialog에 넘겨줄 리턴값</param>
        /// <param name="bIsClose">Modal Popup Dialog Close 여부</param>
        public virtual void PopupCloseWithResult(string rtnValue, bool bIsClose)
        {
            string script = string.Format("parent.window.returnValue='{0}';", rtnValue);
            if (bIsClose) script += "parent.window.close();";

            ExecuteScript(script);
        }

        #endregion [ ##. Modal Popup Method .## ]

        #region [##. HttpFileCollection Methods .##]
        /// <summary>
        /// 파일 다중 업로드
        /// </summary>
        /// <param name="fileList">파일 정보</param>
        /// <param name="UploadPath">업로드 경로</param>
        /// <returns></returns>
        public static bool MultiUpload(HttpFileCollection fileList, string UploadPath)
        {
            bool bChk = true;

            try
            {
                if (string.IsNullOrEmpty(UploadPath) == true) UploadPath = @"\\";

                Impersonation im = new Impersonation();

                im.ImpersonationStart();

                for (int i = 0; i < fileList.Count; i++)
                {
                    HttpPostedFile PostedFile = fileList[i];

                    if (PostedFile.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(PostedFile.FileName);

                        if (System.IO.Directory.Exists(UploadPath) == false)
                        {
                            System.IO.Directory.CreateDirectory(UploadPath);
                        }

                        DateTime dt = DateTime.Now;

                        PostedFile.SaveAs(UploadPath + dt.ToFileTime().ToString() + "_" + FileName);
                    }
                }

                im.ImpersonationEnd();
            }
            catch (Exception ex)
            {
                bChk = false;
                throw ex;
            }

            return bChk;
        }
        #endregion

        #region Display Error Page

        #region Popup Mode

        public void DisplayErrorPageInPopup()
        {
            try
            {
                Exception ex = this.m_Page.Server.GetLastError();
                LogData ErrorLog = LogManager.WriteLog(LogSourceType.ClassLibrary, "PageHelper", ex);
                this.m_Page.Server.ClearError();

                this.DisplayErrorPageInPopup(ErrorLog);
            }
            catch { }
        }

        /// <summary>
        /// 에러 페이지 표시 (Popup 창으로 표시)
        /// </summary>
        /// <param name="ErrorLog">로그 데이터 개체</param>
        public void DisplayErrorPageInPopup(LogData ErrorLog)
        {
			string strErrorHtml = GetErrorPageHtml(ErrorLog);
			if (!string.IsNullOrEmpty(strErrorHtml))
			{
				m_Page.Response.Write(strErrorHtml);
			}
        }

        /// <summary>
        /// 에러 페이지 표시 (Popup 창으로 표시)
        /// </summary>
        /// <param name="sourceType">로그 원본 타입</param>
        /// <param name="BizType">비즈니스 타입</param>
        public void DisplayErrorPageInPopup(LogSourceType sourceType, string BizType)
        {
            HttpContext ctx = HttpContext.Current;
            Exception ex = ctx.Server.GetLastError();

            this.DisplayErrorPageInPopup(ex, sourceType, BizType, ctx);
        }

        /// <summary>
        /// 에러 페이지 표시 (Popup 창으로 표시)
        /// </summary>
        /// <param name="ex">Exception 정보</param>
        /// <param name="sourceType">로그 원본 타입</param>
        /// <param name="BizType">비즈니스 타입</param>
        /// <param name="ctx"></param>
        public void DisplayErrorPageInPopup(Exception ex, LogSourceType sourceType, string BizType, HttpContext ctx)
        {
            try
            {
                LogData ErrorLog = LogManager.WriteLog(sourceType, BizType, ex);

                this.DisplayErrorPageInPopup(ErrorLog, ctx);
            }
            catch { }
        }

        /// <summary>
        /// 에러 페이지 표시 (Popup 창으로 표시)
        /// </summary>
        /// <param name="ErrorLog">로그 데이터 개체</param>
        /// <param name="ctx"></param>
        public void DisplayErrorPageInPopup(LogData ErrorLog, HttpContext ctx)
        {
            try
            {
                if (ctx != null) ctx.Server.ClearError();

                this.DisplayErrorPageInPopup(ErrorLog);
            }
            catch { }
        }


		/// <summary>
		/// 에러 페이지 표시 (Popup 창으로 표시)
		/// </summary>
		/// <param name="ErrorLog">로그 데이터 개체</param>
		/// <param name="ctx"></param>
		public void DisplayErrorPageInPopup(Exception ex, string bizType)
		{
			try
			{
				LogData ErrorLog = LogManager.WriteLog(LogSourceType.WebPage, bizType, ex);
				this.DisplayErrorPageInPopup(ErrorLog); 
			}
			catch { }
		}
	



		/// <summary>
		/// Error Log Popup Html을 가져온다. 
		/// </summary>
		/// <param name="ErrorLog"></param>
		/// <returns></returns>
		protected string GetErrorPageHtml(LogData ErrorLog)
		{
			string CustomerErrorMode = ConfigReader.GetString(CoreContants.ERROR_MODE);
			string strPopup = string.Empty;



			if (CustomerErrorMode == "Y")
			{
				// 에러 페이지 예시 폼
                string CustomerErrorHtml = string.Empty;
                string IsErrorDisplay = ConfigReader.GetString("SKTSection", "DefaultSettings", "IsErrorDIsplay");

                string strPath = string.Empty;

                if (IsErrorDisplay == "Y")
                {
                    CustomerErrorHtml = ConfigReader.GetString(CoreContants.ERROR_HTML_FILE);
                    
                }
                else
                {
                    CustomerErrorHtml = ConfigReader.GetString("CustomerErrorHtml2");
                }

                strPath = HttpContext.Current.Server.MapPath(CustomerErrorHtml);
			


				if (System.IO.File.Exists(strPath) == true)
				{
					string strTemp = System.IO.File.ReadAllText(strPath);
					string strData = string.Empty;

					if (string.IsNullOrEmpty(strTemp) == false)
					{

                        if (IsErrorDisplay == "Y")
                        {
                            strData = string.Format(strTemp,
                                HttpContext.Current.Request.Url.Authority,
                                ErrorLog.ID,
                                ErrorLog.EntryType.ToString(),
                                ErrorLog.SourceType.ToString(),
                                ErrorLog.OccurTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                ErrorLog.BizType,
                                ErrorLog.Message,
                                ErrorLog.Description.Replace("\r\n", "<br>")
                                );
                        }
                        else
                        {
                            strData = string.Format(strTemp,
                                HttpContext.Current.Request.Url.Authority,
                                ErrorLog.ID
                                );
                        }
					}

					string CustomerErrorPopup = ConfigReader.GetString("CustomerErrorPopup");
					strPath = HttpContext.Current.Server.MapPath(CustomerErrorPopup);

					if (System.IO.File.Exists(strPath) == true)
					{
						strTemp = System.IO.File.ReadAllText(strPath);

						if (string.IsNullOrEmpty(strTemp) == false)
						{
							strPopup = string.Format(strTemp, HttpContext.Current.Request.Url.Authority, Guid.NewGuid().ToString(), HttpUtility.JavaScriptStringEncode(strData));
						}
					}
				}
			}

			return strPopup; 
		}





        #endregion

        #region Page Redirect Mode

        public void DisplayErrorPageInRedirect()
        {
            try
            {
                Exception ex = this.m_Page.Server.GetLastError();
                LogData ErrorLog = LogManager.WriteLog(LogSourceType.ClassLibrary, "PageHelper", ex);
                this.m_Page.Server.ClearError();

                this.DisplayErrorPageInRedirect(ErrorLog);
            }
            catch { }
        }

        /// <summary>
        /// 에러 페이지 표시 (페이지 창 이동으로 표시)
        /// </summary>
        /// <param name="ErrorLog">로그 데이터 개체</param>
        public void DisplayErrorPageInRedirect(LogData ErrorLog)
        {
            try
            {
                // 에러 메시지 표시 여부
                string CustomerErrorMode = ConfigReader.GetString(CoreContants.ERROR_MODE);

                if (CustomerErrorMode == "Y")
                {
                    // 에러 메시지 표시 페이지
                    string CustomerErrorFile = ConfigReader.GetString(CoreContants.ERROR_TRANSFER_FILE);

                    if (string.IsNullOrEmpty(CustomerErrorFile) == false)
                    {
                        string strUrl = string.Format(CustomerErrorFile, m_Page.Request.Url.Authority, m_Page.Request.Url.PathAndQuery);

                        string CustomerErrorRedirect = ConfigReader.GetString("CustomerErrorRedirect");
                        string strPath = HttpContext.Current.Server.MapPath(CustomerErrorRedirect);

                        if (System.IO.File.Exists(strPath) == true)
                        {
                            string strTemp = System.IO.File.ReadAllText(strPath);
                            string strRedirect = string.Empty;

                            if (string.IsNullOrEmpty(strTemp) == false)
                            {
                                strRedirect = string.Format(strTemp,
                                    strUrl,
                                    HttpUtility.UrlEncode(ErrorLog.EntryType.ToString()),
                                    HttpUtility.UrlEncode(ErrorLog.SourceType.ToString()),
                                    HttpUtility.UrlEncode(ErrorLog.OccurTime.ToString("yyyy-MM-dd HH:mm:ss")),
                                    HttpUtility.UrlEncode(ErrorLog.BizType),
                                    HttpUtility.UrlEncode(ErrorLog.Message),
                                    HttpUtility.UrlEncode(ErrorLog.Description.Replace("\r\n", "<br>"))
                                    );

                                m_Page.Response.Write(strRedirect);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 에러 페이지 표시 (페이지 창 이동으로 표시)
        /// </summary>
        /// <param name="sourceType">로그 원본 타입</param>
        /// <param name="BizType">비즈니스 타입</param>
        public void DisplayErrorPageInRedirect(LogSourceType sourceType, string BizType)
        {
            HttpContext ctx = HttpContext.Current;
            Exception ex = ctx.Server.GetLastError();

            this.DisplayErrorPageInRedirect(ex, sourceType, BizType, ctx);
        }

        /// <summary>
        /// 에러 페이지 표시 (페이지 창 이동으로 표시)
        /// </summary>
        /// <param name="ex">Exception 정보</param>
        /// <param name="sourceType">로그 원본 타입</param>
        /// <param name="BizType">비즈니스 타입</param>
        /// <param name="ctx"></param>
        public void DisplayErrorPageInRedirect(Exception ex, LogSourceType sourceType, string BizType, HttpContext ctx)
        {
            try
            {
                LogData ErrorLog = LogManager.WriteLog(LogSourceType.WebPage, "PageBase", ex);

                this.DisplayErrorPageInRedirect(ErrorLog, ctx);
            }
            catch { }
        }

        /// <summary>
        /// 에러 페이지 표시 (페이지 창 이동으로 표시)
        /// </summary>
        /// <param name="ErrorLog">로그 데이터 개체</param>
        /// <param name="ctx"></param>
        public void DisplayErrorPageInRedirect(LogData ErrorLog, HttpContext ctx)
        {
            try
            {
                if (ctx != null) ctx.Server.ClearError();

                this.DisplayErrorPageInRedirect(ErrorLog);
            }
            catch { }
        }
        #endregion

        public void DisplayErrorMode(string ErrorDisplayMode = "1", LogData ErrorLog = null)
        {
            try
            {
                if (ErrorLog == null)
                {
                    Exception ex = this.m_Page.Server.GetLastError();
                    ErrorLog = LogManager.WriteLog(LogSourceType.ClassLibrary, "PageHelper", ex);
                }

                this.m_Page.Server.ClearError();

                switch(ErrorDisplayMode)
                {
                    case "2":
                        this.DisplayErrorPageInRedirect(ErrorLog);
                        break;
                    default:
                        this.DisplayErrorPageInPopup(ErrorLog);
                        break;
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}