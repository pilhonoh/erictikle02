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
using System.Web.Script.Serialization;

namespace SKT.Glossary.Web.Glossary
{
    public partial class GlossaryOutLookWrite : System.Web.UI.Page
    {
        protected string mode = string.Empty;
        protected string ItemID = string.Empty;
        protected string Title = string.Empty;
        protected string CommonID = string.Empty;
        protected string Recipient = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string RootURL = string.Empty;

        protected string HtmlBody = string.Empty;
        //protected string DocType = "wiki";
        protected string TutorialYN = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            mode = (Request["mode"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            Title = (Request["Title"] ?? string.Empty).ToString();
            CommonID = (Request["CommonID"] ?? string.Empty).ToString();
            string ajax = (Request["AJAX_METHOD"] ?? string.Empty).ToString();

            string Type = string.Empty;
            string ID = string.Empty;
            string SubJect = string.Empty;
            string Contents = string.Empty;

            //post 로 값이 오면 해당값으로 페이지를 보여준다. 원격에서 작성화면을 띠우게됨.. 그떄 파라미터.
            if (Request.HttpMethod == "POST" && !IsPostBack)
            {
                ID = Request.Form["userkey"];
                Type = Request.Form["type"];
                SubJect = Request.Form["subject"];
                Contents = Request.Form["contents"];

                mode = "RemoteUser";
                hdType.Value = "email"; //타입을 저장한다.

                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(Contents);

                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    // Do something with bodyNode 
                    Contents = bodyNode.InnerHtml;
                }

                //Contents = System.Security.SecurityElement.Escape(Contents);
                Contents = SecurityHelper.Clear_XSS_CSRF(Contents);

                if (mode == "RemoteUser")
                {
                    RemoteBoardSelect(Type, ID, SubJect, Contents);
                }
            }
        }

        //원격 사용자가 왔을떄 처리
        private void RemoteBoardSelect(string type, string UserID, string SubJect, string Contents)
        {
            //Type 은 3가지 nateonbiz ,email,wiki  소문자 string 으로 들어온다.
            //userID 값 이메일로 온다  
            //이메일을 통해 사용자를  가져온다 ..? 기능이 필요? 이미 sso 로 로그인 이뜰듯..
            try
            {
                GlossaryProfileBiz biz = new GlossaryProfileBiz();
                ImpersonUserinfo udata = biz.GetProfileFromEmail(UserID);

                if (udata.UserID.Length > 0) //사용자를 찿앗을경우
                {
                    UserInfo templogin = new UserInfo(this.Page);
                    templogin.UserID = udata.UserID;   //강제 로그인을 하도록한다.

                    //hdTitle.Value = SubJect; //제목은 저장하지않기로함.
                    this.hdNamoContent.Value = Contents;
                    Editor.Contents = Contents.Replace("\r\n", string.Empty);
                }
                else //사용자를 못찿으면 에러를보여주자.
                {
                    this.hdNamoContent.Value = "nateon 으로 부터 값을 가져왔으나  사용자 로그인을 하지못해 데이터를 가져올수없습니다";
                }
            }
            catch (Exception ex)
            {
                this.hdNamoContent.Value = ex.ToString();
            }
        }

        //저장
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryType Board = new GlossaryType();
            GlossaryBiz biz = new GlossaryBiz();
            Board.ID = ItemID;
            Board.UserID = u.UserID;    //작성자 ID
            Board.UserName = u.Name;    //작성자 이름
            Board.DeptName = u.DeptName;
            Board.UserEmail = u.EmailAddress;   //작성자 이메일
            Board.Title = SecurityHelper.Clear_XSS_CSRF(hdTitle.Value).Trim(); //제목
            Board.Contents = SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            Board.ContentsModify = SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용
            Board.Summary = Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
            Board.Description = "처음 작성된 글입니다";
            Board.PrivateYN = "N";
            if (btnPrivate.Checked == true)
                Board.PrivateYN = "Y";
            Board.Type = "email";

            Board.HistoryYN = "N";
            Board.ItemState = hidItemState.Value;
            Board = biz.GlossaryInsert(Board, string.Empty);

            //태그 저장
            GlossaryControlDac dac = new GlossaryControlDac();
            dac.GlossaryTagDelete((Request["CommonID"] ?? Board.ID).ToString());
            if (!string.IsNullOrEmpty(hdTag.Value.Trim()))
            {
                GlossaryControlType ConBoard = new GlossaryControlType();
                ConBoard.CommonID = (Request["CommonID"] ?? Board.ID).ToString();
                ConBoard.Title = Board.Title;
                ConBoard.UserID = u.UserID;

                if (!(hdTag.Value.IndexOf(',') == -1))
                {
                    string[] Tag = hdTag.Value.Split(',');
                    for (int i = 0; i < Tag.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Tag[i].Trim()))
                        {
                            ConBoard.TagTitle = Tag[i].Remove(Tag[i].LastIndexOf("X"));
                            dac.GlossaryTagInsert(ConBoard);
                        }

                    }
                }
                else
                {
                    ConBoard.TagTitle = hdTag.Value.Trim().Remove(hdTag.Value.LastIndexOf("X"));
                    dac.GlossaryTagInsert(ConBoard);
                }
            }
            StringBuilder script = new StringBuilder();
            script.AppendLine("alert('작성이 완료되었습니다.');");
            script.AppendLine("fnClose();");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CLOSE", script.ToString(), true);
        }

        //View
        private void BoardSelect()
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryType Board = biz.GlossarySelect(CommonID, u.UserID, mode);
            hdTitle.Value = SecurityHelper.Add_XSS_CSRF(Board.Title);
            this.hdNamoContent.Value = Board.Contents;
            Editor.Contents = Board.Contents.Replace("\r\n", string.Empty);
            CommonID = Board.CommonID;
            HtmlBody = Board.Contents;
            Recipient = Board.UserEmail;
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = new DataSet();
            ds = Dac.GlossaryTagSelect(CommonID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtTag.Value += dr["TagTitle"].ToString() + ", ";
                }
                txtTag.Value = txtTag.Value.Remove(txtTag.Value.LastIndexOf(","));
            }
        }
    }
}