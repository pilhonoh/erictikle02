using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Common;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services;
using SKT.Glossary.Biz;

namespace SKT.Glossary.Web.Common.Controls
{
    public partial class CommCommentControl : System.Web.UI.UserControl
    {

        public string UserID = string.Empty; //사번
        public string commType = string.Empty;//댓글타입(
        public string commIdx = string.Empty;//게시글코드(상위부모게시글)
        public string commBest = "N";//채택버튼 표시여부
        public string commWrite = "Y";//댓글작성여부
        
        public string totalCount = string.Empty;//전체게시글수
        public string totalAuthCount = string.Empty;//참여대상자수
        public string authCount = string.Empty;//참석자수


        //P097010 BACKUP2
        public string GatheringID { get; set; }
        public string GatheringYN { get; set; }

        public string commTypeCk
        {
            set
            {
                commType = value;
            }
        }
        public string commIdxCK
        {
            set
            {
                commIdx = value;
            }
        }


        public string commBestCk
        {
            set
            {
                commBest = value;
            }
        }

        public string commWriteCk
        {
            set
            {
                commWrite = value;
            }
        }

        private string _SendYN = string.Empty;

        public string SendYN
        {
            get { return _SendYN; }
            set { _SendYN = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            UserID = u.UserID;

        }


       

    }
}
