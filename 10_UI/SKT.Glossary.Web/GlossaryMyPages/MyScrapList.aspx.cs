using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Common;
using SKT.Glossary.Biz;
using System.Collections;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

namespace SKT.Glossary.Web.GlossaryMyPages
{
    public partial class MyScrapList : System.Web.UI.Page
    {
        int currentPageIndx;
        protected int iTotalCount;
        protected string DisplayTotalCount = string.Empty;
        protected string SearchKeyword = string.Empty;
        protected string ItemID = string.Empty;
        protected string UserName = string.Empty;
        protected string RootURL = string.Empty;
        protected string mode = string.Empty;
        protected string HistoryYN = string.Empty;
        protected string UserID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            SearchKeyword = (Request["SearchKeyword"] ?? string.Empty).ToString();
            ItemID = (Request["ItemID"] ?? string.Empty).ToString();
            mode = (Request["mode"] ?? string.Empty).ToString();



            if (!IsPostBack)
            {
                pager.PageSize = 10;// int.Parse(this.ddlpageSize.SelectedValue);
                int PageNum;
                int.TryParse((Request["PageNum"] ?? string.Empty).ToString(), out PageNum);
                pager.CurrentIndex = (PageNum == 0) ? 1 : PageNum;
                BindSelect();

            }

        }

        //글 목록
        private void BindSelect()
        {
            iTotalCount = 0;
            UserInfo u = new UserInfo(this.Page);
            UserName = u.Name;
            UserID = u.UserID;
            GlossaryScrapBiz biz = new GlossaryScrapBiz();
            ArrayList list = biz.GlossaryScrapList(pager.CurrentIndex, pager.PageSize, out iTotalCount, u.UserID);
            pager.ItemCount = iTotalCount;
            DisplayTotalCount = String.Format("{0:#,#}", iTotalCount);
            if (DisplayTotalCount.Length == 0)
            {
                DisplayTotalCount = "0";
            }
            rptInGeneral.DataSource = list;
            rptInGeneral.DataBind();
        }

        //페이지
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;
            BindSelect();
        }

        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal WikiClass = (Literal)e.Item.FindControl("ltWiki");
                Literal Num = (Literal)e.Item.FindControl("Num");
                // 2014-06-16 Mr.No
                GlossaryScrapType glossaryScrapType = (GlossaryScrapType)e.Item.DataItem;
                Literal UserInfo = (Literal)e.Item.FindControl("UserInfo");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }

                //Literal litAlarmYN = (Literal)e.Item.FindControl("litAlarmYN");
                //Literal litEmail = (Literal)e.Item.FindControl("litEmail");
                //Literal litNote = (Literal)e.Item.FindControl("litNote");
                Literal itDelete = (Literal)e.Item.FindControl("itDelete");
                Literal litReply = (Literal)e.Item.FindControl("litReply");
                Literal litPermission = (Literal)e.Item.FindControl("litPermission");

                /*
                if (((GlossaryScrapType)e.Item.DataItem).NoteYN == "Y")
                {
                    litAlarmYN.Text = "<a id=\"alarm-view-3\" class=\"btn_alarm on\" onclick =\"fnAlarmOpen(this,'" + ((GlossaryScrapType)e.Item.DataItem).GlossaryID + "');\">알림안함</a>";
                }
                else
                {
                    litAlarmYN.Text = "<a id=\"alarm-view-3\" class=\"btn_alarm off\"  onclick =\"fnAlarmOpen(this,'" + ((GlossaryScrapType)e.Item.DataItem).GlossaryID + "');\">알림안함</a>";
                }
                */

                //if (((GlossaryScrapType)e.Item.DataItem).MailYN == "Y")
                //{
                //    litEmail.Text = "<input type=\"checkbox\" value=\"\" name=\"\" id=\"email\" checked=\"checked\" />";
                //}
                //else
                //{
                //    litEmail.Text = "<input type=\"checkbox\" value=\"\" name=\"\" id=\"email\" />";
                //}

                //if (((GlossaryScrapType)e.Item.DataItem).NoteYN == "Y")
                //{
                //    litNote.Text = "<input type=\"checkbox\" value=\"\" name=\"\" id=\"nateon\" checked=\"checked\" />";
                //}
                //else
                //{
                //    litNote.Text = "<input type=\"checkbox\" value=\"\" name=\"\" id=\"nateon\" />";
                //}


                switch (((GlossaryScrapType)e.Item.DataItem).Type)
                {
                    case "wiki":
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;

                    case "nateon":
                        WikiClass.Text = "<span class=\"nateon\">";
                        break;

                    case "email":
                        WikiClass.Text = "<span class=\"email\">";
                        break;
                    default:
                        WikiClass.Text = "<span class=\"wiki\">";
                        break;
                }

                itDelete.Text = "<input type=\"checkbox\" id = \"Checkbox\"  value='" + ((GlossaryScrapType)e.Item.DataItem).ID + "' name=\"checkJob\" onclick=\"event.cancelBubble = true;\">";

                if (!((GlossaryScrapType)e.Item.DataItem).CommentCount.Equals("0"))
                {
                    if (((GlossaryScrapType)e.Item.DataItem).NewCommentFlag)
                    {
                        litReply.Text = "<span class=\"reply new\">[" + ((GlossaryScrapType)e.Item.DataItem).CommentCount + "]</span>";
                    }
                    else
                    {
                        litReply.Text = "<span class=\"reply\">[" + ((GlossaryScrapType)e.Item.DataItem).CommentCount + "]</span>";
                    }
                }

                // 권한 아이콘
                if (!((GlossaryScrapType)e.Item.DataItem).Permissions.Equals("FullPublic"))
                {
                    litPermission.Text = "<img src=\"/common/images/icon/icn_closed.png\" alt=\"비공개\" />";
                }

                // 2014-06-16 Mr.No
                if (glossaryScrapType.PrivateYN == "Y")
                {
                    UserInfo.Text = SecurityHelper.Clear_XSS_CSRF(glossaryScrapType.UserName);
                }
                else
                {
                    //UserInfo.Text = "<a href='javascript:fnProfileView(\"" + glossaryScrapType.YouUserID + "\");'>";
                    UserInfo.Text += glossaryScrapType.UserName + "/" + glossaryScrapType.DeptName;
                    //UserInfo.Text += "<img class=\"icon_img\" width=\"19\" height=\"19\" title=\"" + glossaryScrapType.Rank + "\" src=\"";
                    //UserInfo.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + glossaryScrapType.UserGrade + ConfigurationManager.AppSettings["AftermageUrl"] + "\"/>";
                }

            }

        }

        protected void btnListDelete_Click(object sender, EventArgs e)
        {
            string[] strCeckBox = Request.Params["checkJob"].Split(',');

            GlossaryScrapDac dac = new GlossaryScrapDac();

            for (int i = 0; i < strCeckBox.Length; i++)
            {
                //    dac.GlossaryTempDelect(strCeckBox[i]);
                dac.GlossaryScrapDelete(strCeckBox[i]);
            }

            Response.Redirect("/GlossaryMyPages/MyScrapList.aspx");

        }
    }
}