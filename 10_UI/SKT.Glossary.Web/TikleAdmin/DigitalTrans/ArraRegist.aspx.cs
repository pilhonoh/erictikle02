using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

namespace SKT.Glossary.Web.TikleAdmin.DigitalTrans
{
    public partial class ArraRegist : System.Web.UI.Page
    {
        protected int currentPageIndx = 1;
        protected int iTotalCount;
        UserInfo u;
        protected string UserID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new UserInfo(this.Page);
            UserID = u.UserID;

            if (!IsPostBack)
            {
                pager.PageSize = 10;
                pager.CurrentIndex = 1;
                BindSelect();
            }
        }

        //제목 페이지 처리
        public void pager_Command(object sender, CommandEventArgs e)
        {
            currentPageIndx = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = currentPageIndx;

            BindSelect();

        }

        private void BindSelect()
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();

            DataSet ds = biz.ArraTrendSelect(pager.CurrentIndex, pager.PageSize);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                pager.ItemCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString());
                iTotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString());

                rptInGeneral.DataSource = ds.Tables[0];
                rptInGeneral.DataBind();
            }
        }

        protected void rptInGeneral_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal Num = (Literal)e.Item.FindControl("Num");

                if (pager.CurrentIndex != 1)
                {
                    Num.Text = Convert.ToInt16((iTotalCount--) - (pager.CurrentIndex * 10) + 10).ToString();
                }
                else
                {
                    Num.Text = Convert.ToInt16(iTotalCount--).ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TikleAdadminBiz biz = new TikleAdadminBiz();

            int Idx = 0;

            if (!String.IsNullOrEmpty(this.hidID.Value))
                Idx = int.Parse(this.hidID.Value);

            DataSet ds = biz.ArraTrendAction(this.hidMode.Value, Idx, this.selGubun.Value, this.txtTitle.Text, this.txtLink.Text, u.UserID);

            this.selGubun.SelectedIndex = 0;
            this.txtTitle.Text = "";
            this.txtLink.Text = "";
            this.hidID.Value = "";
            this.hidMode.Value = "";

            BindSelect();
        }
      
    }
}