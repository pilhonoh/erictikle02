using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Utilities;
using SKT.Tnet.Framework.Security;
using SKT.Tnet;

namespace SKT.Tnet.Controls
{
    public class WebEditorData
    {
        private string _HtmlBody = string.Empty;
        private string[] _ImageFiles = null;

        public string HtmlBody
        {
            get { return this._HtmlBody; }
            set { this._HtmlBody = value; }
        }

        public string[] ImageFiles
        {
            get { return this._ImageFiles; }
            set { this._ImageFiles = value; }
        }
    }


    [DefaultProperty("ID"), ToolboxData("<{0}:WebEditor runat=server></{0}:WebEditor>")]
    public class WebEditor : System.Web.UI.WebControls.WebControl
    {
        #region Property
        private WebEditorData _weData = null;
        private string _imageFolder = string.Empty;
        private string _value = "";
        private string[] _imageFile = null;
        private string _type = "";
        private string _userlang = string.Empty;
        private string _defaultFont = string.Empty;
        private string _defaultFontSize = string.Empty;

        [Browsable(true), Category("WebEditor 기본 설정"), Description("Editor 내용")]
        public virtual string Value
        {
            get
            {
				if (_weData == null)
				{
                    if (HttpContext.Current != null && HttpContext.Current.Request.Form[this.HFWriteID] != null)
                    {
                        this._value = HttpContext.Current.Request.Form[this.HFWriteID].ToString();
                    }

					if (string.IsNullOrEmpty(this._value) == false)
					{
						_weData = this.GetDecodeMIME(this._value, this.ImageFolder);
					}
				}

                if (_weData != null)
                {
                    this._value = _weData.HtmlBody;

                    // script 태그 및 태그 내에 javascript 치환 작업
                    this._value = Regex.Replace(this._value, @"<( )*script([^>])*>", "<java_script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    this._value = Regex.Replace(this._value, @"(<( )*(/)( )*script( )*>)", "</java_script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    this._value = Regex.Replace(this._value, @"href=javascript", "href=java_script", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    this._value = Regex.Replace(this._value, @"href='javascript", "href='java_script", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    this._value = Regex.Replace(this._value, @"href=""javascript", @"href=""java_script", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    this._value = Regex.Replace(this._value, @"onclick=", @"xonclick=", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }
                else
                {
                    this._value = string.Empty;
                }

                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [Browsable(true), Category("WebEditor 기본 설정"), Description("Editor 이미지 파일들")]
        public string[] ImageFiles
        {
            get
            {
                if (_weData == null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request.Form[this.HFWriteID] != null)
                    {
                        this._value = HttpContext.Current.Request.Form[this.HFWriteID].ToString();
                    }

					if (string.IsNullOrEmpty(this._value) == false)
					{
						_weData = this.GetDecodeMIME(this._value, this.ImageFolder);
					}

                }

                if (_weData != null)
                {
                    if (_weData.ImageFiles != null)
                    {
                        this._imageFile = _weData.ImageFiles;
                    }
                }
                else
                {
                    this._imageFile = null;
                }

                return this._imageFile;
            }
        }

        [Browsable(true), Category("WebEditor 기본 설정"), Description("Web Editor 메뉴 설정")]
        public string Type
        {
            get
            {
                if (string.IsNullOrEmpty(this._type))
                    return "editor";
                else
                    return string.Format("editor_{0}", this._type);
            }
            set { this._type = value; }
        }

        [Browsable(true), Category("WebEditor 기본 설정"), Description("본문 이미지 업로드 폴더")]
        public string ImageFolder
        {
            get
            {
                if (string.IsNullOrEmpty(this._imageFolder) == true)
                {
                    this._imageFolder = "Default";
                }

                return this._imageFolder;
            }
            set { this._imageFolder = value; }
        }

        [Browsable(true), Category("WebEditor 기본 설정"), DefaultValue("kor"), Description("Web Editor의 언어 설정")]
        public string UserLang
        {
            get
            {
                if (string.IsNullOrEmpty(this._userlang) == true || string.IsNullOrWhiteSpace(this._userlang) == true)
                {
                    return "kor";
                }
                else
                {
                    return this._userlang;
                }
            }
            set { this._userlang = value.ToLower(); }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("hfWriter"), Description("등록용 Hidden Field 명칭")]
        public string HFWriteID
        {
            get
            {
                return this.ClientID + "_hfWriter";
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("100%"), Description("Web Editor의 폭 설정")]
        private string EditorWidth
        {
            get
            {
                if (string.IsNullOrEmpty(base.Width.ToString()) == true || string.IsNullOrWhiteSpace(base.Width.ToString()) == true)
                {
                    return "100%";
                }
                else
                {
                    return base.Width.ToString();
                }
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("400"), Description("Web Editor의 높이 설정")]
        private string EditorHeight
        {
            get
            {
                if (string.IsNullOrEmpty(base.Height.ToString()) == true || string.IsNullOrWhiteSpace(base.Height.ToString()) == true)
                {
                    return "400";
                }
                else
                {
                    return base.Height.ToString();
                }
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("/WebEditor"), Description("Namo Web Editor의 Cab 파일 경로")]
        private string EditorFolder
        {
            get
            {
                string strRtn = string.Empty;

                try
                {
                    strRtn = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "NamoCabFileUrl");
                }
                catch
                {
                    strRtn = "/WebEditor";
                }

                return strRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("400"), Description("Thumbnail Image Size (폭)")]
        private int ThumbnailSizeWidth
        {
            get
            {
                int iRtn = 0;

                try
                {
                    iRtn = ConfigReader.GetInteger(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "ThumbnailSizeWidth");
                }
                catch
                {
                    iRtn = 400;
                }

                return iRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), DefaultValue("400"), Description("Thumbnail Image Size (높이)")]
        private int ThumbnailSizeHeight
        {
            get
            {
                int iRtn = 0;

                try
                {
                    iRtn = ConfigReader.GetInteger(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "ThumbnailSizeHeight");
                }
                catch
                {
                    iRtn = 300;
                }

                return iRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("기본 CSS 파일")]
        public string BasicCSSFile
        {
            get
            {
                string strRtn = string.Empty;

                try
                {
                    strRtn = string.Format("{0}{1}",
                        ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "TnetHostName"), 
                        ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "BaseCSS")
                        );
                }
                catch { }

                return strRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("해더 CSS 파일")]
        public string HeadCSSFile
        {
            get
            {
                string strRtn = string.Empty;

                try
                {
                    strRtn = string.Format("{0}{1}",
                        ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, CoreContants.DEFAULT_CATEGORY_NAME, "TnetHostName"),
                        ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "HeadCSS"));
                }
                catch { }

                return strRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("이미지 파일 저장 경로")]
        private string FileRootFolder
        {
            get
            {
                string strRtn = string.Empty;
                try
                {
                    strRtn = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "FileRootKOR");
                }
                catch
                {
                    strRtn = "/FileRootKOR";
                }

                return strRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("이미지 파일 임시 경로")]
        private string NamoTempPath
        {
            get
            {
                string strRtn = string.Empty;

                try
                {
                    strRtn = ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "NamoTempPath");
                }
                catch
                {
                    strRtn = @"C:\Temp";
                }

                return strRtn;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("기본 폰트 설정")]
        public string DefaultFont
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultFont) == true)
                {
                    _defaultFont = "맑은 고딕";
                }

                return _defaultFont;
            }

            set
            {
                _defaultFont = value;
            }
        }

        [Browsable(false), Category("WebEditor 기본 설정"), Description("기본 폰트 사이즈 설정")]
        public string DefaultFontSize
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultFontSize) == true)
                {
                    _defaultFontSize = "12";
                }

                return _defaultFontSize;
            }

            set
            {
                _defaultFontSize = value;
            }
        }
        #endregion

        #region Render
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = null;


            HiddenField hfValue = new HiddenField();

            try
            {

                if (Page.IsPostBack == true)
                {
                    hfValue.Value = this.Value;
                }
                else
                {
                    hfValue.Value = this._value; //??
                }
                
				hfValue.ID = this.ClientID + "_HIDVALUE"; 
                hfValue.RenderControl(writer);


                sb = new StringBuilder();
				sb.AppendFormat("<input type='hidden' name='{0}' id='{0}' />", this.HFWriteID); 
                sb.Append("<script type=\"text/javascript\">");
                if (this.Type == "editor_mini")
                {
                    sb.AppendFormat("LoadWebEdit(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\');", ClientID, this.EditorWidth, this.EditorHeight, this.EditorFolder, this.UserLang, this.EditorFolder + "/As7Init_mini5.xml");
                }
                else if(this.Type == "editor_tstory")
                {
                    sb.AppendFormat("LoadWebEdit(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\');", ClientID, this.EditorWidth, this.EditorHeight, this.EditorFolder, this.UserLang, this.EditorFolder + "/As7Init_tstory6.xml");
                }
                else
                {
                    if (this.Type.Contains("_") == true)
                    {
                        string strType = this.Type.Substring(this.Type.IndexOf("_") + 1, this.Type.Length - this.Type.IndexOf("_") - 1);
                        sb.AppendFormat("LoadWebEdit(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\');", ClientID, this.EditorWidth, this.EditorHeight, this.EditorFolder, this.UserLang, this.EditorFolder + "/As7Init_" + strType + ".xml");
                    }
                    else
                    {
                        sb.AppendFormat("LoadWebEdit(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\');", ClientID, this.EditorWidth, this.EditorHeight, this.EditorFolder, this.UserLang, this.EditorFolder + "/As7Init5.xml");
                    }
                    
                }
                sb.Append("</script>");
                writer.Write(sb.ToString());
                sb.Clear();

                sb.Append("<script type=\"text/javascript\" for=\"" + base.ClientID + "\" event=\"OnInitCompleted()\">");
                sb.Append("var wec = $('#" + this.ClientID + "');");

                if (Page.IsPostBack == true)
                {
                    if (string.IsNullOrEmpty(this.Value) == false) //??
                    {
                        sb.Append("wec.val($('#" + hfValue.ClientID + "').val());");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(this._value) == false)
                    {
                        sb.Append("wec.val($('#" + hfValue.ClientID + "').val());");
                    }
                }

                sb.Append("wec.get(0).HeadValue = \"<style type='text/css' src='" + this.HeadCSSFile + "'></style>\";");
                //sb.Append("wec.get(0).SetBasicCSSFile(\"" + this.BasicCSSFile + "\");");
                sb.Append("wec.get(0).SetDefaultFont(\"" + this.DefaultFont + "\");");
                sb.Append("wec.get(0).SetDefaultFontSize(\"" + this.DefaultFontSize + "\");");
                sb.Append("</script> ");



                if (sb.Length > 0) writer.Write(sb.ToString());
            }
            catch { }
        }
        #endregion

        #region Utility

        /// <summary>
        /// 나모 에디터 컨텐츠 디코딩
        /// </summary>
        /// <returns></returns>
        public WebEditorData GetDecodeMINE()
        {
            return this.GetDecodeMIME(this.Value, this.ImageFolder);
        }

        /// <summary>
        /// 나모 에디터 컨텐츠 디코딩
        /// </summary>
        /// <param name="folder">이미지 파일 업로드 경로</param>
        /// <returns></returns>
        public WebEditorData GetDecodeMIME(string folder)
        {
            return this.GetDecodeMIME(this.Value, folder);
        }

        /// <summary>
        /// 나모 에디터 컨텐츠 디코딩
        /// </summary>
        /// <param name="pMIMEContent">Editor 내용</param>
        /// <param name="folder">이미지 파일 업로드 경로</param>
        /// <returns></returns>
        public WebEditorData GetDecodeMIME(string pMIMEContent, string folder)
        {
            string bodyHtml = string.Empty;
            WebEditorData weData = null;

            try
            {
                if (string.IsNullOrEmpty(pMIMEContent) == false && string.IsNullOrWhiteSpace(pMIMEContent) == false)
                {
                    Impersonation im = new Impersonation();
                    im.ImpersonationStart();

                    #region 환경 변수 설정
                    weData = new WebEditorData();

                    string rootUrl = string.Format(this.FileRootFolder, folder);
                    string rootPath = HttpContext.Current.Server.MapPath(rootUrl);
                    string tempPath = string.Format(@"{0}\{1}", this.NamoTempPath, Guid.NewGuid().ToString("N"));
                    #endregion 환경 변수 설정

                    if (System.IO.Directory.Exists(tempPath) == false) System.IO.Directory.CreateDirectory(tempPath);

                    ////MIMEObjectClass mime = new MIMEObjectClass();
                    ////mime.Charset = "utf-8";
                    ////mime.OnlyHTMLBody = 1;
                    MIMESquareLib8.MIMEObject mime = new MIMESquareLib8.MIMEObject();
                    mime.Decode(pMIMEContent, tempPath);

                    #region 이미지 처리 - 로컬 이미지만 서버에 업로드

                    bodyHtml = this.GetBodyHtml(tempPath);
                    Stack<string[]> bodyImg = this.GetBodyImg(tempPath);

                    if (bodyImg != null && bodyImg.Count > 0)
                    {
                        if (System.IO.Directory.Exists(rootPath) == false) System.IO.Directory.CreateDirectory(rootPath);

                        foreach (string[] files in bodyImg)
                        {
                            foreach (string file in files)
                            {
                                System.IO.FileInfo fi = new FileInfo(file);

                                string GuidValue = Guid.NewGuid().ToString("N");
                                string fileName = string.Format("{0}{1}", GuidValue, fi.Extension);
                                string movePath = string.Format(@"{0}\{1}", rootPath, fileName);

                                fi.MoveTo(movePath);

                                bodyHtml = bodyHtml.Replace(file, string.Format("http://{0}{1}/{2}",
                                                                        HttpContext.Current.Request.Url.Authority,
                                                                        rootUrl,
                                                                        fileName));

                                string ThumbnailFile = string.Format("{0}_M{1}", GuidValue, fi.Extension);
                                string ThumbnailPath = string.Format(@"{0}\{1}", rootPath, ThumbnailFile);


                                if (this.ThumbnailSizeHeight > 0 && this.ThumbnailSizeWidth > 0)
                                {
                                    ImageHelper.ResizeSave(movePath, ThumbnailPath, this.ThumbnailSizeHeight, this.ThumbnailSizeWidth);
                                }
                                else if (this.ThumbnailSizeHeight > 0 && this.ThumbnailSizeWidth == 0)
                                {
                                    ImageHelper.ResizeSave(movePath, ThumbnailPath, this.ThumbnailSizeHeight, "Height");
                                }
                                else if (this.ThumbnailSizeHeight == 0 && this.ThumbnailSizeWidth > 0)
                                {
                                    ImageHelper.ResizeSave(movePath, ThumbnailPath, this.ThumbnailSizeWidth, "Width");
                                }
                                else
                                {
                                    System.IO.FileInfo moveFile = new FileInfo(movePath);
                                    moveFile.CopyTo(ThumbnailPath);
                                }
                            }
                        }
                    }

                    if (System.IO.Directory.Exists(tempPath) == true) System.IO.Directory.Delete(tempPath);

                    weData.HtmlBody = bodyHtml;

                    #endregion 이미지 처리 - 로컬 이미지만 서버에 업로드

                    #region HTML 내 이미지 파일 목록 취득
                    List<Uri> links = ImageHelper.GetImageURLS(bodyHtml);

                    if (links != null && links.Count > 0)
                    {
                        weData.ImageFiles = new string[links.Count];

                        int iCnt = 0;

                        foreach (Uri imageUrl in links)
                        {
                            if (imageUrl.ToString().Contains("http://" + HttpContext.Current.Request.Url.Authority) == true)
                            {
                                weData.ImageFiles[iCnt] = imageUrl.ToString().Replace("http://" + HttpContext.Current.Request.Url.Authority, "");

                                iCnt++;
                            }
                        }

                        if (iCnt == 0)
                        {
                            if (System.IO.Directory.Exists(rootPath) == false) System.IO.Directory.Delete(rootPath);
                        }
                    }
                    #endregion

                    im.ImpersonationEnd();
                }
            }
            catch
            {
                weData = null;
            }

            return weData;
        }

        /// <summary>
        /// Namo Web Editor의 HTML 파싱 처리 함수
        /// </summary>
        /// <param name="tempPath"></param>
        /// <returns></returns>
        private string GetBodyHtml(string tempPath)
        {
            string strRtn = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(tempPath) == false && string.IsNullOrWhiteSpace(tempPath) == false)
                {
                    tempPath += "\\noname.htm";
                    if (System.IO.File.Exists(tempPath) == true)
                    {
                        using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
                        {
                            strRtn = sr.ReadToEnd();
                        }

						//strRtn = strRtn.Replace("\0", "");      // a Null character
						//strRtn = strRtn.Replace("\a", "");      // alert character
						//strRtn = strRtn.Replace("\b", "");      // backspace
						//strRtn = strRtn.Replace("\f", "");      // form feed
						//strRtn = strRtn.Replace("\r\n", "");
						//strRtn = strRtn.Replace("\n\r", "");
						//strRtn = strRtn.Replace("\n", "");      // New Line
						//strRtn = strRtn.Replace("\r", "");      // Carriage return
						//strRtn = strRtn.Replace("\t", "");          // horizontal tab
						//strRtn = strRtn.Replace("\v", "");      // veritical tab
                        //strRtn = strRtn.Replace("\'", "`");      // Single Quote
                        //strRtn = strRtn.Replace("\"", "");      // Double Quote
                        //strRtn = strRtn.Replace("\\", "");      // Backslash

                        System.IO.File.Delete(tempPath);
                    }
                }
            }
            catch
            {
                strRtn = string.Empty;
            }

            return strRtn;
        }

        /// <summary>
        /// Namo Web Editor의 파일 파싱 처리 함수
        /// </summary>
        /// <param name="tempPath"></param>
        /// <returns></returns>
        private Stack<string[]> GetBodyImg(string tempPath)
        {
            Stack<string[]> oRtn = null;
            string[] sFileItem = null;
            System.IO.DirectoryInfo di = null;
            int i;

            try
            {
                if (string.IsNullOrEmpty(tempPath) == false && string.IsNullOrWhiteSpace(tempPath) == false)
                {
                    if (System.IO.Directory.Exists(tempPath) == true)
                    {
                        di = new DirectoryInfo(tempPath);

                        if (di.GetFiles().Length > 0)
                        {
                            sFileItem = new string[di.GetFiles().Length];

                            i = 0;
                            foreach (var Item in di.GetFiles())
                            {
                                sFileItem[i] = string.Format("{0}\\{1}", tempPath, Item.Name);
                                i++;
                            }

                            oRtn = new Stack<string[]>();
                            oRtn.Push(sFileItem);
                        }
                    }
                }
            }
            catch
            {
                oRtn = null;
            }

            return oRtn;
        }

        #endregion
    }
}