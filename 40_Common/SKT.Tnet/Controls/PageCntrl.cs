using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI.WebControls;

namespace SKT.Tnet.Controls
{
    /// <summary>
    /// 페이징 클래스
    /// </summary>
    /// <history>
    /// </history>

    /// <summary>
    /// 인덱스 체인지 이벤트 인자
    /// </summary>
    #region ----- PageIndexEventArgs Class -----
    public class PageIndexEventArgs : EventArgs
    {
        // Methods
        public PageIndexEventArgs(int pageIndex)
        {
            this._newPageIndex = pageIndex;
        }

        // Properties
        public int NewPageIndex
        {
            get
            {
                return this._newPageIndex;
            }
            set
            {
                this._newPageIndex = value;
            }
        }

        // Fields
        private int _newPageIndex;
    }
    #endregion

    /// <summary>
    ///  페이지컨트롤 클래스
    /// </summary>
    #region ----- PageCntrl -----
    [DefaultProperty("ID"), ToolboxData("<{0}:PageCntrl runat=server></{0}:PageCntrl>")]
    public class PageCntrl : WebControl, IPostBackEventHandler
    {
        // Events
        public event PageIndexChangeEventHandler PageIndexChanged;

        // Methods
        public PageCntrl()
        {
            this._npageSize = 10;
            this._npageWidth = 10;
            this._ncurrentPage = 1;
            this._nTotalCount = 100;
            this._nMovePage = 10;

            this._nPagingFormatValue = 1;
            this._sFirst = "처음으로";
            this._sPrevPage = "이전으로";
            this._sLast = "마지막으로";
            this._sNextPage = "다음으로";
            this._sFirstActivateImageUrl = "nav_paging_btn nav_paging_btn_fst";
            this._sFirstDeActivateImageUrl = "nav_paging_btn_DeAct nav_paging_btn_fst_DeAct";
            this._sPrevActivateImageUrl = "nav_paging_btn nav_paging_btn_prev";
            this._sPrevDeActivateImageUrl = "nav_paging_btn_DeAct nav_paging_btn_prev_DeAct";
            this._sLastActivateImageUrl = "nav_paging_btn nav_paging_btn_lst";
            this._sLastDeActivateImageUrl = "nav_paging_btn_DeAct nav_paging_btn_lst_DeAct";
            this._sNextActivateImageUrl = "nav_paging_btn nav_paging_btn_next";
            this._sNextDeActivateImageUrl = "nav_paging_btn_DeAct nav_paging_btn_next_DeAct";
            this._ntotalPage = 0;
            this._sTotalCaption = "Total:";
            this._sHtmlActivateImageFormat = "<a href=\"{1}\" class=\"{0}\"><span class=\"blind\">{2}</span></a>";
            this._sHtmlDeActivateImageFormat = "<a href=\"{1}\" class=\"{0}\"><span class=\"blind\">{2}</span></a>";
            this._sDeActivateTextFormat = "<a href=\"#\" class=\"{0}\"><span class=\"blind\">{1}</span></a>";
            this._bPageVisible = false;
            this._CtrlAlign = "center";
            this._ClickMethods = "";

        }

        #region ViewState
        /// <summary>
        ///  뷰스테이트 로드..
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            this.PageSize = (int)this.ViewState["PageSize"];
            this.PageWidth = (int)this.ViewState["PageWidth"];
            this.CurrentPage = (int)this.ViewState["CurrentPage"];
            this.TotalCount = (int)this.ViewState["TotalCount"];
        }

        /// <summary>
        ///  뷰스테이트 저장
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            this.ViewState["PageSize"] = this.PageSize;
            this.ViewState["PageWidth"] = this.PageWidth;
            this.ViewState["CurrentPage"] = this.CurrentPage;
            this.ViewState["TotalCount"] = this.TotalCount;
            return base.SaveViewState();
        }
        #endregion

        #region PagingButtonGen
        /// <summary>
        ///  버튼생성
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sPrevActivateImg"></param>
        /// <param name="sPrevDeActivateImg"></param>
        /// <returns></returns>
        private string PagingButtonGen(string RenderHtml, string item, string sPrevActivateImg, string sPrevDeActivateImg, string linkurl, string txt)
        {
            return this.PagingButtonGen(RenderHtml, item, 1, sPrevActivateImg, sPrevDeActivateImg, linkurl, txt);
        }

        /// <summary>
        ///  버튼생성 오버라이드
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nGubn">1 : 버튼 활성화, 2 : 버튼 비활성화</param>
        /// <param name="sPrevActivateImg">버튼 활성시 CSS 정보</param>
        /// <param name="sPrevDeActivateImg">버튼 비활성시 CSS 정보</param>
        /// <param name="linkurl">링크 URL 정보</param>
        /// <returns></returns>
        private string PagingButtonGen(string RenderHtml, string item, int nGubn, string sPrevActivateImg, string sPrevDeActivateImg, string linkurl, string txt)
        {
            if (this.PagingType == PageCntrl.PagingFormat.Text)
            {
                if (nGubn == 2)
                {
                    return string.Format(this._sDeActivateTextFormat, RenderHtml, item);
                }
                //return item;
            }

            if (nGubn == 2)
            {
                item = sPrevDeActivateImg;
            }
            else
            {
                item = sPrevActivateImg;
            }
            return string.Format(RenderHtml, item, linkurl, txt);
        }
        #endregion

        #region Render
        /// <summary>
        ///  렌더링시 이벤트 
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder sb = new StringBuilder();

            if (this.TotalCount != 0)
            {
                if ((this.TotalCount % this.PageSize) == 0)
                {
                    this._ntotalPage = this.TotalCount / this.PageSize;
                }
                else
                {
                    this._ntotalPage = (this.TotalCount / this.PageSize) + 1;
                }

                if (this.CurrentPage < 1) this.CurrentPage = 1;
                if (this.CurrentPage > this._ntotalPage) this.CurrentPage = this._ntotalPage;
            }
            else
            {
                this._ntotalPage = 0;
            }

            if (this.TotalCount > 0)
            {
                sb.Append("<div class=\"nav_paging\">");

                if (this.CurrentPage <= 1)
                {
                    sb.Append(this.PagingButtonGen(this._sHtmlDeActivateImageFormat, this.First, 2, this.FirstActivateImageUrl, this.FirstDeActivateImageUrl, this.PagingLinkUrl(), this.First));
                    sb.Append(" ");
                    sb.Append(this.PagingButtonGen(this._sHtmlDeActivateImageFormat, this.PrevPage, 2, this.PrevActivateImageUrl, this.PrevDeActivateImageUrl, this.PagingLinkUrl(), this.PrevPage));
                }
                else
                {
                    sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.First, this.FirstActivateImageUrl, this.FirstDeActivateImageUrl, this.PagingLinkUrl("1"), this.First));
                    sb.Append(" ");

                    if ((this._ncurrentPage - this._nMovePage) < 1)
                    {
                        sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.PrevPage, this.PrevActivateImageUrl, this.PrevDeActivateImageUrl, this.PagingLinkUrl("1"), this.PrevPage));
                    }
                    else
                    {
                        //sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.PrevPage, this.PrevActivateImageUrl, this.PrevDeActivateImageUrl, this.PagingLinkUrl((this._ncurrentPage - this._nMovePage).ToString()), this.PrevPage));
                        if ((this._ncurrentPage - this._nMovePage).ToString().Length == 1)
                        {
                            sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.PrevPage, this.PrevActivateImageUrl, this.PrevDeActivateImageUrl, this.PagingLinkUrl("10"), this.PrevPage));
                        }
                        else
                        {
                            int iCnt1 = 0;
                            string strCnt1 = (this._ncurrentPage - this._nMovePage).ToString();

                            if (strCnt1.Length > 1)
                            {
                                if (strCnt1.Substring(strCnt1.Length - 1, 1) == "0")
                                {
                                    strCnt1 = strCnt1.Substring(0, strCnt1.Length - 1);
                                    iCnt1 = System.Convert.ToInt32(strCnt1) * this.MovePage;
                                }
                                else
                                {
                                    strCnt1 = strCnt1.Substring(0, strCnt1.Length - 1);
                                    iCnt1 = (System.Convert.ToInt32(strCnt1) + 1) * this.MovePage;
                                }
                            }
                            else
                            {
                                iCnt1 = 1;
                            }

                            sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.PrevPage, this.PrevActivateImageUrl, this.PrevDeActivateImageUrl, this.PagingLinkUrl(iCnt1.ToString()), this.PrevPage));
                        }



                    }
                }

                sb.Append(" ");

                if (this._ntotalPage > 0)
                {
                    int iPageBlock = 0;

                    if ((this.CurrentPage % this.PageWidth) == 0)
                    {
                        iPageBlock = ((this.CurrentPage / this.PageWidth) * this.PageWidth) - this.PageWidth;
                    }
                    else
                    {
                        iPageBlock = (this.CurrentPage / this.PageWidth) * this.PageWidth;
                    }

                    for (int iPageNum = (iPageBlock + 1); iPageNum < ((iPageBlock + 1) + this.PageWidth); iPageNum++)
                    {
                        if (iPageNum == this.CurrentPage)
                        {
                            sb.Append("<a href=\"#\" class=\"nav_paging_num selected\">" + iPageNum + "</a>");
                        }
                        else
                        {
                            sb.Append("<a href=\"" + this.PagingLinkUrl(iPageNum.ToString()) + "\" class=\"nav_paging_num\">" + iPageNum + "</a>");
                        }

                        if (iPageNum == this._ntotalPage) break;
                    }
                }

                sb.Append(" ");

                if (this.CurrentPage >= this._ntotalPage)
                {
                    sb.Append(this.PagingButtonGen(this._sHtmlDeActivateImageFormat, this.NextPage, 2, this.NextActivateImageUrl, this.NextDeActivateImageUrl, this.PagingLinkUrl(), this.NextPage));
                    sb.Append(" ");
                    sb.Append(this.PagingButtonGen(this._sHtmlDeActivateImageFormat, this.Last, 2, this.LastActivateImageUrl, this.LastDeActivateImageUrl, this.PagingLinkUrl(), this.Last));
                }
                else
                {
                    if ((this._ncurrentPage + this._nMovePage) > this._ntotalPage)
                    {
                        //this.CurrentPage = this._ntotalPage;
                        //sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.NextPage, this.NextActivateImageUrl, this.NextDeActivateImageUrl, this.PagingLinkUrl(this._ntotalPage.ToString()), this.NextPage));

                        string strCnt2 = this._ncurrentPage.ToString();
                        int iCnt2 = 0;

                        if (strCnt2.Length > 1)
                        {
                            if (strCnt2.Substring(strCnt2.Length - 1, 1) == "0")
                            {
                                strCnt2 = strCnt2.Substring(0, strCnt2.Length - 1);
                                iCnt2 = (System.Convert.ToInt32(strCnt2) * this.MovePage) + 1;
                            }
                            else
                            {
                                strCnt2 = strCnt2.Substring(0, strCnt2.Length - 1);
                                iCnt2 = ((System.Convert.ToInt32(strCnt2) + 1) * this.MovePage) + 1;
                            }
                        }
                        else
                        {
                            iCnt2 = this.MovePage + 1;
                        }

                        if (iCnt2 >= this.TotalPage)
                        {
                            if ((this.CurrentPage + this.MovePage) <= this.TotalPage)
                            {
                                strCnt2 = strCnt2.Substring(0, strCnt2.Length - 1);
                                iCnt2 = (System.Convert.ToInt32(strCnt2) * this.MovePage) + 1;
                            }
                            else
                            {
                                iCnt2 = this.TotalPage;
                            }
                        }

                        sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.NextPage, this.NextActivateImageUrl, this.NextDeActivateImageUrl, this.PagingLinkUrl(iCnt2.ToString()), this.NextPage));
                    }
                    else
                    {
                        //sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.NextPage, this.NextActivateImageUrl, this.NextDeActivateImageUrl, this.PagingLinkUrl((this._ncurrentPage + this._nMovePage).ToString()), this.NextPage));
                        string strCnt2 = this._ncurrentPage.ToString();
                        int iCnt2 = 0;

                        if (strCnt2.Length > 1)
                        {
                            if (strCnt2.Substring(strCnt2.Length - 1, 1) == "0")
                            {
                                strCnt2 = strCnt2.Substring(0, strCnt2.Length - 1);
                                iCnt2 = (System.Convert.ToInt32(strCnt2) * this.MovePage) + 1;
                            }
                            else
                            {
                                strCnt2 = strCnt2.Substring(0, strCnt2.Length - 1);
                                iCnt2 = ((System.Convert.ToInt32(strCnt2) + 1) * this.MovePage) + 1;
                            }
                        }
                        else
                        {
                            iCnt2 = this.MovePage + 1;
                        }

                        sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.NextPage, this.NextActivateImageUrl, this.NextDeActivateImageUrl, this.PagingLinkUrl(iCnt2.ToString()), this.NextPage));
                    }
                    sb.Append(" ");
                    sb.Append(this.PagingButtonGen(this._sHtmlActivateImageFormat, this.Last, this.LastActivateImageUrl, this.LastDeActivateImageUrl, this.PagingLinkUrl(this._ntotalPage.ToString()), this.Last));
                }

                sb.Append("</div>");
            }

            if (sb.Length > 0)
            {
                output.Write(sb.ToString());
            }
        }
        #endregion

        #region Proverty
        // Properties
        /// <summary>
        ///  현재의 페이지를 설정한다. 
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("현재 Page의 Index를 설정하거나 얻어 오는 속성")]
        public int CurrentPage
        {
            get
            {
                if (this._nTotalCount > 0)
                {
                    return this._ncurrentPage;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                this._ncurrentPage = value;
            }
        }

        /// <summary>
        /// 현제페이지의 데이타 인덱스
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("현재 페이징 되어 있는 데이타의 위치를 얻어오는 속성")]
        public int CurrentPageDataIndex
        {
            get
            {
                return ((this._ncurrentPage - 1) * this._npageSize);
            }
        }

        /// <summary>
        /// 페이지 이동 블록
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), DefaultValue(1), Description("페이지 이동시 이동 페이지 갯수 설정 속성")]
        public int MovePage
        {
            get { return this._nMovePage; }
            set { this._nMovePage = value; }
        }

        /// <summary>
        ///  최초의 문자
        /// </summary>
        [Browsable(true), Category("Paging 텍스트")]
        public string First
        {
            get
            {
                return this._sFirst;
            }
            set
            {
                this._sFirst = value;
            }
        }

        /// <summary>
        ///  최초의 이미지경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("first 활성화 이미지( 맨 처음으로 이동 )"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string FirstActivateImageUrl
        {
            get
            {
                return this._sFirstActivateImageUrl;
            }
            set
            {
                this._sFirstActivateImageUrl = value;
            }
        }

        /// <summary>
        /// 최초의 비활성화 이미지경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("first 비활성화 이미지"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string FirstDeActivateImageUrl
        {
            get
            {
                return this._sFirstDeActivateImageUrl;
            }
            set
            {
                this._sFirstDeActivateImageUrl = value;
            }
        }

        /// <summary>
        ///  최종인데스
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("Insert시에 라스트 페이지를 가지고 오는 속성")]
        public int InsertLastPageIndex
        {
            get
            {
                if (this._nTotalCount > 0)
                {
                    return ((this._nTotalCount / this._npageSize) + 1);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///  최후 글자
        /// </summary>
        [Browsable(true), Category("Paging 텍스트")]
        public string Last
        {
            get
            {
                return this._sLast;
            }
            set
            {
                this._sLast = value;
            }
        }

        /// <summary>
        /// 최후 활성화이미지 경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("Last 활성화 이미지( 맨 마지막으로 이동 )"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string LastActivateImageUrl
        {
            get
            {
                return this._sLastActivateImageUrl;
            }
            set
            {
                this._sLastActivateImageUrl = value;
            }
        }

        /// <summary>
        /// 최후 비활성화 이미지 경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("Last 비활성화 이미지"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string LastDeActivateImageUrl
        {
            get
            {
                return this._sLastDeActivateImageUrl;
            }
            set
            {
                this._sLastDeActivateImageUrl = value;
            }
        }

        /// <summary>
        /// 다음 활성화 이미지 경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("다음 리스트 활성화 이미지"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string NextActivateImageUrl
        {
            get
            {
                return this._sNextActivateImageUrl;
            }
            set
            {
                this._sNextActivateImageUrl = value;
            }
        }

        /// <summary>
        ///  다음 비활성화 이미지 경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("다음 리스트 비활성화 이미지"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string NextDeActivateImageUrl
        {
            get
            {
                return this._sNextDeActivateImageUrl;
            }
            set
            {
                this._sNextDeActivateImageUrl = value;
            }
        }

        /// <summary>
        ///  다음 문자
        /// </summary>
        [Browsable(true), Category("Paging 텍스트")]
        public string NextPage
        {
            get
            {
                return this._sNextPage;
            }
            set
            {
                this._sNextPage = value;
            }
        }

        /// <summary>
        ///  페이지 크기 
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("페이징 사이즈를 조절하는 속성")]
        public int PageSize
        {
            get
            {
                return this._npageSize;
            }
            set
            {
                this._npageSize = value;
            }
        }

        /// <summary>
        /// 페이지 넓이 
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("페이징 사이즈를 조절하는 속성")]
        public int PageWidth
        {
            get
            {
                return this._npageWidth;
            }
            set
            {
                this._npageWidth = value;
            }
        }

        /// <summary>
        ///  페이지 정보 활성화 여부 
        /// </summary>
        [Bindable(true), Category("Paging 기본 설정"), Description("페이지의 위치, 총 데이타 건수를 보여 줄지 여부를 설정하는 속성"), DefaultValue(false)]
        public bool PageVisible
        {
            get
            {
                return this._bPageVisible;
            }
            set
            {
                this._bPageVisible = value;
            }
        }

        /// <summary>
        ///  페이징 유형
        /// </summary>
        [Bindable(true), Category("Paging 기본 설정"), Description("처음페이지, 이전페이지, 다음페이지, 마지막페이지를 텍스트 또는 이미지로 설정하는 속성"), DefaultValue(0)]
        public PagingFormat PagingType
        {
            get
            {
                if (this._nPagingFormatValue == 0)
                {
                    return PageCntrl.PagingFormat.Text;
                }
                return PageCntrl.PagingFormat.Image;
            }
            set
            {
                if (value == PageCntrl.PagingFormat.Text)
                {
                    this._nPagingFormatValue = 0;
                }
                else
                {
                    this._nPagingFormatValue = 1;
                }
            }
        }

        /// <summary>
        ///  이전 활성화 이미지경로
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), Description("이전 리스트 활성화 이미지"), DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string PrevActivateImageUrl
        {
            get
            {
                return this._sPrevActivateImageUrl;
            }
            set
            {
                this._sPrevActivateImageUrl = value;
            }
        }

        /// <summary>
        ///  이전 비활성화 이미지경로 
        /// </summary>
        [Bindable(true), Category("Paging 이미지"), DefaultValue("이전 리스트 비활성화 이미지"), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor)), Description("이전 리스트 비활성화 이미지")]
        public string PrevDeActivateImageUrl
        {
            get
            {
                return this._sPrevDeActivateImageUrl;
            }
            set
            {
                this._sPrevDeActivateImageUrl = value;
            }
        }

        /// <summary>
        ///  이전페이지 문자.
        /// </summary>
        [Browsable(true), Category("Paging 텍스트")]
        public string PrevPage
        {
            get
            {
                return this._sPrevPage;
            }
            set
            {
                this._sPrevPage = value;
            }
        }

        /// <summary>
        ///  전체문자
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("총 데이타 건수의 Caption을 설정하는 속성")]
        public string TotalCaption
        {
            get
            {
                return this._sTotalCaption;
            }
            set
            {
                this._sTotalCaption = value;
            }
        }

        /// <summary>
        ///  전체 카운트
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("총 데이타 건수를 설정하거나 얻어 오는 속성")]
        public int TotalCount
        {
            get
            {
                return this._nTotalCount;
            }
            set
            {
                this._nTotalCount = value;
            }
        }

        /// <summary>
        ///  전체 페이지 
        /// </summary>
        [Browsable(true), Category("Paging 기본 설정"), Description("Total Page를 얻어오는 속성")]
        public int TotalPage
        {
            get
            {
                return this._ntotalPage;
            }
        }


        /// <summary>
        /// 컨트롤 정렬 (left, center, right)
        /// </summary>
        [Browsable(true), Category("Control Align"), Description("Control Align")]
        public string CntrAlign
        {
            get
            {
                return this._CtrlAlign;
            }
            set
            {
                this._CtrlAlign = value;
            }
        }

        /// <summary>
        /// Paging 번호 클릭시 호출 자바스크립트 함수 명칭
        /// </summary>
        [Browsable(true), Category("Click Methods"), Description("Click Methods")]
        public string ClickMethods
        {
            get
            {
                return this._ClickMethods;
            }

            set
            {
                this._ClickMethods = value;
            }
        }
        #endregion

        #region Fields
        // Fields
        private bool _bPageVisible;
        private int _ncurrentPage;
        private int _npageSize;
        private int _npageWidth;
        private int _nPagingFormatValue;
        private int _nTotalCount;
        private int _ntotalPage;
        private int _nMovePage;
        private string _sDeActivateTextFormat;
        private string _sFirst;
        private string _sFirstActivateImageUrl;
        private string _sFirstDeActivateImageUrl;
        private string _sHtmlActivateImageFormat;
        private string _sHtmlDeActivateImageFormat;
        private string _sLast;
        private string _sLastActivateImageUrl;
        private string _sLastDeActivateImageUrl;
        private string _sNextActivateImageUrl;
        private string _sNextDeActivateImageUrl;
        private string _sNextPage;
        private string _sPrevActivateImageUrl;
        private string _sPrevDeActivateImageUrl;
        private string _sPrevPage;
        private string _sTotalCaption;
        private string _CtrlAlign;
        private string _ClickMethods;

        //private PageIndexChangeEventHandler PageIndexChanged;
        #endregion

        // Nested Types
        public enum ePageVisible
        {
            // Fields
            False = 1,
            True = 0
        }

        public delegate void PageIndexChangeEventHandler(object sender, PageIndexEventArgs e);

        public enum PagingFormat
        {
            // Fields
            Image = 1,
            Text = 0
        }

        #region IPostBackEventHandler 멤버

        public void RaisePostBackEvent(string eventArgument)
        {
            this.CurrentPage = Convert.ToInt32(eventArgument);
            if (this.PageIndexChanged != null)
            {
                PageIndexEventArgs args1 = new PageIndexEventArgs(this.CurrentPage);
                this.PageIndexChanged(this, args1);
            }
        }

        #endregion

        #region Utilitys

        /// <summary>
        /// Paging Number 클릭시, 호출 Javascript 함수 취득
        /// </summary>
        /// <param name="PagingNumber">페이지 번호</param>
        /// <returns></returns>
        private string PagingLinkUrl(string PagingNumber = null)
        {
            string strRtn = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(PagingNumber) == true || PagingNumber == "0")
                {
                    strRtn = "javascript:void(0);";
                }
                else
                {
                    if (string.IsNullOrEmpty(ClickMethods) == true || string.IsNullOrWhiteSpace(ClickMethods) == true)
                    {
                        strRtn = this.Page.ClientScript.GetPostBackClientHyperlink(this, PagingNumber);
                    }
                    else
                    {
                        strRtn = string.Format("javascript:{0}('{1}');", ClickMethods, PagingNumber);
                    }
                }
            }
            catch
            {
                strRtn = "javascript:void(0);";
            }

            return strRtn;
        }

        #endregion
    }
    #endregion
}
