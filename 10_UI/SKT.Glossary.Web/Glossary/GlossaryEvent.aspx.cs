using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Collections;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;

namespace SKT.Glossary.Web
{
    public partial class GlossaryEvent : System.Web.UI.Page
    {
        protected string RootURL = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            Select();
        }

        //최초실행
        public void Select()
        {
            UserInfo u = new UserInfo(this.Page);

            string temprank;
            string tempWriteCount;
            string tempAnswerCount;
            string AnswerRank;
            string AttendanceCount;

            GlossaryBiz biz = new GlossaryBiz();
            biz.GetEventData(u.UserID, out temprank, out tempWriteCount, out tempAnswerCount, out AttendanceCount, out AnswerRank);

            //"<span></span>";
            string Rankspan = string.Empty;
            for (int i = 0; i < temprank.Length; i++)
            {
                Rankspan = Rankspan + "<span>" + temprank[i] + "</span>";
            }

            if (temprank.Length == 0)
            {
                Rankspan = "<span>-</span>";
            }

            Rank.Text = Rankspan;


            string AnswerRankspan = string.Empty;
            for (int i = 0; i < AnswerRank.Length; i++)
            {
                AnswerRankspan = AnswerRankspan + "<span>" + AnswerRank[i] + "</span>";
            }

            if (AnswerRank.Length == 0)
            {
                AnswerRankspan = "<span>-</span>";
            }

            this.AnswerRank.Text = AnswerRankspan;


            string Writespan = string.Empty;
            for (int i = 0; i < tempWriteCount.Length; i++)
            {
                Writespan = Writespan + "<span>" + tempWriteCount[i] + "</span>";
            }
            WriteCount.Text = Writespan;

            string Answerspan = string.Empty;
            for (int i = 0; i < tempAnswerCount.Length; i++)
            {
                Answerspan = Answerspan + "<span>" + tempAnswerCount[i] + "</span>";
            }
            AnswerCount.Text = Answerspan;


            //this.AttendanceCount.Text = AttendanceCount;
            string stamptext1 = string.Empty;
            string stamptext2 = string.Empty;

            int AttendanceCountnum=int.Parse(AttendanceCount);
            //AttendanceCountnum;
            for (int i = 1; i <= 10; i++)
            {
                if (i <= 5)
                {
                    if (i <= AttendanceCountnum)
                    {
                        stamptext1 = stamptext1 + "<td><img src=\"/common/images/event1_mark.png\" /></td>";
                    }
                    else
                    {
                        stamptext1 = stamptext1 + "<td></td>";
                    }
                }
                else if(i<=10)
                {
                    if (i <= AttendanceCountnum)
                    {
                        stamptext2 = stamptext2 + "<td><img src=\"/common/images/event1_mark.png\" /></td>";
                    }
                    else
                    {
                        stamptext2 = stamptext2 + "<td></td>";
                    }
                }
            }

            this.Stamp1.Text = stamptext1;
            this.Stamp2.Text = stamptext2;

            WriteRankSelect();
            AnswerRankSelect();
        }

        //티끌랭킹 가져오기
        protected void WriteRankSelect()
        {
            ArrayList list = new ArrayList();

            GlossaryBiz biz = new GlossaryBiz();
            list = biz.GetEventRankList();
            //GlossaryEventType test = new GlossaryEventType();
            //test.Name = "홍길동";
            //list.Add(test);

            rptInRanking.DataSource = list;
            rptInRanking.DataBind();
            
        }

        //답변랭킹 가져오기
        protected void AnswerRankSelect()
        {
            ArrayList list = new ArrayList();

            //GlossaryEventType test = new GlossaryEventType();
            //test.Name = "홍길동2";
            //list.Add(test);
            GlossaryBiz biz = new GlossaryBiz();
            list = biz.GetEventReplyRankList();

            rptInWriteRanking.DataSource = list;
            rptInWriteRanking.DataBind();
        }
      
    }
}