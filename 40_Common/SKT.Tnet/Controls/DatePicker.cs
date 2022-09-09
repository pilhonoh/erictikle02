using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SKT.Tnet.Controls
{
    /// <Summary>
    /// DataPicker Class
    /// </Summary>
    /// <Remarks>
    /// # 작성자 : 최진석 <br/>
    /// # 작성일 : 2015년 04월 13일 <br/>
    /// # 히스토리 로그 <br/>
    ///   - 2015년 04월 13일, 최진석 최초작성 <br/>
    /// </Remarks>
    public class DatePicker : CompositeControl
    {
        #region [Variable]
        /// <summary>
        /// DatePicker: 날짜 표시할 TextBox
        /// </summary>
        private TextBox TextDatePicker = null;

        /// <summary>
        /// Text: calendar 날짜
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (TextDatePicker != null)
                {
                    return TextDatePicker.Text;
                }
                else return string.Empty;
            }
            set
            {
                TextDatePicker.Text = value;
            }
        }
        #endregion

        #region [Constructors]
		/// <summary>
		/// Calendar: 생성자
		/// </summary>
        public DatePicker()
		{
            TextDatePicker = new TextBox();
		}
		#endregion

        #region [OnInit]
        /// <summary>
        /// OnInit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            TextDatePicker.ID = "txtCalendar";
            //yyyy-MM-dd
            //TxtCalendar.Text = DateTime.Now.ToShortDateString();

            //TxtCalendar.Style.Add("padding-bottom", "2px");
            //TxtCalendar.Style.Add("margin-right", "3px");
            //TxtCalendar.Style.Add("vertical-align", "top");

            ///TextDatePicker.Font.Size = FontUnit.Parse("11px");
            //TextDatePicker.Width = this.Width.IsEmpty ? Unit.Pixel(70) : this.Width;
			TextDatePicker.CssClass = "form_txt inp_date";
            base.OnInit(e);
        }
        #endregion

        #region [CreateChildControls]
        /// <summary>
        /// CreateChildControls
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.Controls.Add(this.TextDatePicker);
            //base.CreateChildControls();
        }
        #endregion

        #region [RenderContents]
        /// <summary>
        /// RenderContents
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<script>");
            writer.Write("$(function () { $(\"#" + this.TextDatePicker.ClientID + "\").datepicker({showAnim:''})});");
            
            //writer.Write("var orgFunc = EndRequestHandler;");
            //writer.Write("EndRequestHandler = new Function('$(\"#" + this.TxtCalendar.ClientID + "\").datepicker({ });orgFunc();')");
            // 2015-04-13: Sys Error 발생으로 제외
            //writer.Write("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);");
			//writer.Write("function EndRequestHandler(sender, args) {");
			//writer.Write("$(\"#" + this.TextDatePicker.ClientID + "\").datepicker({showAnim:'', onClose: function () { $('#ui-datepicker-div').css('display', 'none') }});");
			//writer.Write("}");
            //writer.Write("$('body').append('<iframe id=\"datepickerFrm\" name =\"TOP\" width=\"400px\" height=\"400px\" style=\"z-index:1; position:absolute;\"></iframe>');");
            //Datepicker ActiveX 아래에 묻히는 현상 수정(상대 2015-04-08)
            writer.WriteLine("if ($('#datepickerFrm').length == 0) {");
            writer.WriteLine("$('body').append(\"<iframe id='datepickerFrm' style='position:absolute;display:none;'></iframe>\");");
            writer.WriteLine("};");
            writer.WriteLine("var datepickerLeft;var datepickerTop;var datepickerWidth;var datepickerHeight;");
            writer.WriteLine("$(function () {");
            writer.WriteLine("function datepickerFrmSetting() {");
            writer.WriteLine("if ($('#ui-datepicker-div').css('display') == 'none') {");
            writer.WriteLine("$('#datepickerFrm').css('display', 'none');");
            writer.WriteLine("}");
            writer.WriteLine("else {");
            writer.WriteLine("datepickerLeft = $('#ui-datepicker-div').offset().left;");
            writer.WriteLine("datepickerTop = $('#ui-datepicker-div').offset().top;");
            writer.WriteLine("datepickerWidth = $('#ui-datepicker-div').outerWidth();");
            writer.WriteLine("datepickerHeight = $('#ui-datepicker-div').outerHeight();");
            writer.WriteLine("$('#datepickerFrm').css('left', datepickerLeft).css('top', datepickerTop).css('width', datepickerWidth).css('height', datepickerHeight);");
            writer.WriteLine("$('#datepickerFrm').css('z-index', 999);");
            writer.WriteLine("$('#ui-datepicker-div').css('z-index','1000')");
            writer.WriteLine("$('#datepickerFrm').css('display', '');}}");
            writer.WriteLine("$(document).click(function () {");
            writer.WriteLine("datepickerFrmSetting();");
            writer.WriteLine("if($('.ui-datepicker-year').length > 0){");
            writer.WriteLine("$('.ui-datepicker-year, .ui-datepicker-month').on('change', function () {");
            writer.WriteLine("datepickerFrmSetting();");
            writer.WriteLine("});");
            writer.WriteLine("}");
            writer.WriteLine("});");
            writer.WriteLine("$('.hasDatepicker').change(function () {");
            writer.WriteLine("$('#datepickerFrm').css('display', 'none');});");
            writer.WriteLine("$('.ui-datepicker-trigger').click(function () {");
            writer.WriteLine("datepickerFrmSetting();});});");

            writer.WriteLine("</script>");
            
            //writer.Write(string.Format("<script>Calender_Init('{0}'); </script>", this.TxtCalendar.ClientID)); 
            TextDatePicker.RenderControl(writer);
            //base.RenderContents(writer);
        }
        #endregion
    }
}
