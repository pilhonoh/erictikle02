using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Services;
using System.IO;
using System.Text;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryNoViewWrite : System.Web.UI.Page
    {

        protected string mode = string.Empty;
        protected string ItemID = string.Empty;
        protected string CommonID = string.Empty;
        protected string Recipient = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;

        protected string HtmlBody = string.Empty;
        


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Type = string.Empty;
                string ID = string.Empty;
                string SubJect = string.Empty;
                string Contents = string.Empty;

                //post 로 값이 오면 해당값으로 페이지를 보여준다. 원격에서 작성화면을 띠우게됨.. 그떄 파라미터.
                if (Request.HttpMethod == "POST")
                {
                    StreamReader readStream = new StreamReader(Request.InputStream, Encoding.UTF8);
                    string page = readStream.ReadToEnd();

                    int index = page.IndexOf("contents=");
                    if (index > 0)
                    {
                        string param = page;//.Substring(0, index);

                        int startuserkey = param.IndexOf("userkey=") + 8;
                        int enduserkey = param.IndexOf("&", startuserkey);
                        ID = param.Substring(startuserkey, enduserkey - startuserkey);

                        int starttype = param.IndexOf("type=") + 5;
                        int endtype = param.IndexOf("&", starttype);
                        Type = param.Substring(starttype, endtype - starttype);

                        int startsubject = param.IndexOf("subject=") + 8;
                        int endsubject = param.IndexOf("&", startsubject);
                        SubJect = param.Substring(startsubject, endsubject - startsubject);

                        if (string.IsNullOrEmpty(Type) == false && string.IsNullOrEmpty(SubJect) == false)
                        {

                            //mode = "RemoteUser";

                            int start = page.IndexOf("<BODY");
                            page = page.Remove(0, start);
                            int end = page.IndexOf("</BODY>");
                            page = page.Remove(end + 7, page.Length - (end + 7));

                            Contents = page;
                            //Contents = SecurityHelper.Clear_XSS_CSRF(Contents);
                            //Contents= Contents.Replace("&nbsp;", " ");


                            string UserID = GetUserIDFromEmail(ID);
                            Save(UserID, SubJect, Contents, Type);
                            Response.Write("ok!");
                        }
                    }
                    else
                    {
                        Response.Write("fail!");
                    }
                }



                if (!IsPostBack)
                {
                    //GlossaryDac Dac = new GlossaryDac();
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        private string GetUserIDFromEmail(string Email)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            ImpersonUserinfo data =biz.GetProfileFromEmail(Email);

            return data.UserID;
        }

        //저장
        
        protected void Save(string UserID, string Title, string Content,string Type)
        {
            
            UserInfo u = new UserInfo(this.Page);
            string IsTestServer = ConfigurationManager.AppSettings["IsTestServer"];
            if (IsTestServer == "Y" && UserID != "")
            {
                u.UserID = UserID;
            }
            
            GlossaryType Board = new GlossaryType();
            GlossaryBiz biz = new GlossaryBiz();
            Board.ID = ItemID;
            Board.UserID = u.UserID;    //작성자 ID
            Board.UserName = u.Name;    //작성자 이름
            Board.DeptName = u.DeptName;
            Board.UserEmail = u.EmailAddress;   //작성자 이메일
            Board.Title = SecurityHelper.Clear_XSS_CSRF(Title).Trim(); //제목
            Board.Contents = SecurityHelper.Clear_XSS_CSRF(Content).Trim();
            Board.ContentsModify = SecurityHelper.Clear_XSS_CSRF(Content).Trim();  //html 내용
            Board.Summary = Utility.RemoveHtmlTag(Content);    //text 내용
            Board.Description = "최초입력";
            Board.PrivateYN = "N";

            //if (btnPrivate.Checked == true)
            //    Board.PrivateYN = "Y";
            
            mode = "";
            Board.HistoryYN = "N";
            Board = biz.GlossaryInsert(Board, mode);
            
            //Response.Write(
            //Response.Redirect("/Glossary/GlossaryView.aspx?ItemID=" + Board.ID);

        }

        
        ////View
        //private void BoardSelect()
        //{
        //    UserInfo u = new UserInfo(this.Page);
        //    GlossaryBiz biz = new GlossaryBiz();
        //    GlossaryType Board = biz.GlossarySelect(ItemID, u.UserID, mode);
        //    hdTitle.Value = Board.Title;
        //    this.hdNamoContent.Value = Board.Contents;
        //    Editor.Contents = Board.Contents.Replace("\r\n", string.Empty);
        //    CommonID = Board.CommonID;
        //    HtmlBody = Board.Contents;
        //    Recipient = Board.UserEmail;
            
        //}
        
        ////임시문서 View
        //private void MyTempSelect()
        //{
        //    UserInfo u = new UserInfo(this.Page);
        //    GlossaryTempBiz biz = new GlossaryTempBiz();
        //    GlossaryTempType Board = biz.GlossaryTempSelect(ItemID);
        //    hdTitle.Value = Board.Title;
        //    this.hdNamoContent.Value = Board.Contents;
        //    Editor.Contents = Board.Contents.Replace("\r\n", string.Empty);
        //    CommonID = Board.CommonID;
        //    HtmlBody = Board.Contents;          
        //}

        ////목록 - ModifyYN 수정
        //protected void btnList_Click(object sender, EventArgs e)
        //{
        //    GlossaryDac Dac = new GlossaryDac();
        //    Dac.GlossaryModifyYNUpdate(ItemID, "List");
        //    //Response.Redirect("/" + RootURL + "/Glossary/GlossaryList.aspx");
        //    Response.Redirect("/Glossary/GlossaryList.aspx");
        //}

        ////임시저장
        //protected void btnGlossary_Click(object sender, EventArgs e)
        //{
        //    GlossaryTempBiz biz = new GlossaryTempBiz();
        //    GlossaryTempType Board = new GlossaryTempType();
        //    UserInfo u = new UserInfo(this.Page);
        //    Board.CommonID = (Request["CommonID"] ?? string.Empty).ToString();
        //    Board.Title = SecurityHelper.Clear_XSS_CSRF(hdTitle.Value).Trim(); //제목
        //    Board.Contents = SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
        //    Board.ContentsModify = SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용
        //    Board.Summary = Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
        //    Board.PrivateYN = "N";
        //    if (btnPrivate.Checked == true)
        //        Board.PrivateYN = "Y";
        //    Board.Description = txtReason.Value;
        //    Board.DocumentKind = hidRadioState.Value;
        //    Board.UserID = u.UserID;
        //    biz.GlossaryTempInsert(Board);
        //    Response.Redirect("/GlossaryMyPages/MyTempList.aspx");
        //}

        //protected string GetSearchKeyword()
        //{
        //    return Server.UrlEncode(SearchKeyword);
        //}
        //protected string GetTitle()
        //{
        //    return Server.UrlEncode(Title);
        //}

       

    }
}