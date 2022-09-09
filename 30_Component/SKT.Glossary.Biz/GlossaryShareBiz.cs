using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;


namespace SKT.Glossary.Biz
{
   public class GlossaryShareBiz
    {
        //공유 목록 리스트
       public ArrayList GlossaryShareList(int PageNum, int PageSize, out int TotalCount, out int Total, string UserID, string TebType)
        {
            ArrayList list = new ArrayList();

            TotalCount = 0;
            Total = 0;
            GlossaryShareDac dac = new GlossaryShareDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryShareList(PageNum, PageSize, UserID, TebType);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {             
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryShareType Board = new GlossaryShareType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.FromUserID = dr["FromUserID"].ToString();
                    Board.ToUserID = dr["ToUserID"].ToString();
                    Board.BoardUserID = dr["BoardUserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ReadYN = dr["ReadYN"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    {
                        byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                        try
                        {
                            Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 320) + "...";
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits"))
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount"))
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag"))
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    if (dr.Table.Columns.Contains("LikeCount"))
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("UserGrade") == true)
                    {
                        Board.UserGrade = (dr["UserGrade"] == DBNull.Value) ? 0 : dr.Field<int>("UserGrade");
                    }
                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("UserGrade"))
                    {
                        if (Board.UserGrade == 0) { Board.Rank = "지존"; }
                        else if (Board.UserGrade == 1) { Board.Rank = "고수"; }
                        else if (Board.UserGrade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }

                    if (dr.Table.Columns.Contains("PrivateYN"))
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }

                    list.Add(Board);
                }
            }
            return list;
        }

       //공유 목록 카운트
       public GlossaryShareCountsType GlossaryShareCounts(string UserID)
       {
           GlossaryShareDac dac = new GlossaryShareDac();
           DataSet ds = new DataSet();
           ds = dac.GlossaryShareCounts(UserID);

           GlossaryShareCountsType gsCounts = new GlossaryShareCountsType();
           gsCounts.ShareGetCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ShareGetCount"]);
           gsCounts.ShareSendCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ShareSendCount"]);
           gsCounts.ShareTotalCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ShareTotalCount"]);

           return gsCounts;        
       }


        //공유 Select
        public GlossaryShareType GlossaryShareSelect(string ID)
        {
            GlossaryShareDac Dac = new GlossaryShareDac();
            DataSet ds = Dac.GlossaryShareSelect(ID);
            GlossaryShareType Board = new GlossaryShareType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.FromUserID = dr["FromUserID"].ToString();
                    Board.ToUserID = dr["ToUserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ReadYN = dr["ReadYN"].ToString();
                    Board.CreateDate = dr["CreateDate"].ToString();
                }
            }
            return Board;
        }

        //공유 추가
        public void GlossaryShareInsert(GlossaryShareType Board)
        {
            GlossaryShareDac Dac = new GlossaryShareDac();
            DataSet ds = Dac.GlossaryShareInsert(Board);
        }

        //공유 삭제
        //public void GlossaryShareDelete(string ID)
        //{
        //    GlossaryShareDac Dac = new GlossaryShareDac();
        //    DataSet ds = Dac.GlossaryShareDelete(ID);
        //}


        /// <summary>
        /// 2014-05-14 Mr.No
        /// 최초 글 작성시 일부공개 일 경우 공유 추가되는 부분
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="UserID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="Title"></param>
        public void GlossaryShareInsert_SomePublic(string ItemID, string UserID, string ToUserID, string Title, string Mode)
        {
            string[] ToUser = ToUserID.Split('/');
            GlossaryShareType Board = new GlossaryShareType();
            GlossaryShareDac Dac = new GlossaryShareDac();
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryType Board_ = biz.GlossarySelect(ItemID, "", Mode);
            Board.FromUserID = UserID;
            Board.GlossaryID = Board_.CommonID;  //common 아이디를 넘겨줘야하므로 넘겨주자.
            Board.Title = Title;

            Board.UserName = Board_.UserName;
            Board.DeptName = Board_.DeptName;
            Board.BoardUserID = Board_.UserID;
            for (int i = 0; i < ToUser.Length; i++)
            {
                GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                // 단순 유저인 경우
                if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                {

                    Board.ToUserID = ToUser[i];
                    if (Mode != "History")
                    {
                        //CBHMSMQHelper helper = new CBHMSMQHelper();
                        CBHNoteType data = new CBHNoteType();

                        string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                        string NoteLink = BaseURL + "GlossaryMyPages/MyShareList.aspx";

                        string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                            //+ "[공유자] 님께서 [공유받은자] 님께 [티끌제목]을 공유 하셨습니다.";
                        + biz_.UserSelect(UserID).Name + "님께서 " + u.Name + " 님께 <STRONG>&lt;" + Title + "&gt;</STRONG>을 공유 하셨습니다."
                            //+ " 티끌이 많은 도움이 되었습니다.<br/> 감사합니다. ^^<br /><br />"
                        + "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";

                        data.Content = ContentsStr;
                        data.Kind = "3"; //일반쪽지.
                        data.SendUserName = "티끌이";
                        //if (!String.IsNullOrEmpty(u.EmailAddress))
                        //{
                        string userID = u.EmailAddress.Remove(u.EmailAddress.ToString().IndexOf('@')); //이메일 앞부분이 note id 값이다.
                        data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                        data.TargetUser = userID;

                        //OK//helper.SendNoteToQueue(data);
                        ////}

                        //쪽지 20170802
                        CBHInterface.CBHNoteSend(data);

                        //메일 20170802
                        CBHInterface.CBHMailSend(u.EmailAddress, "tikle@sk.com", "T.끌 알림 메일입니다.", ContentsStr);
                    }

                    Dac.GlossaryShareInsert(Board);
                }
                    // 부서인 경우
                else
                {
                    GlossaryShareInsert_SomePublic_Dept(ItemID, UserID, ToUser[i], Title, Mode);
                }
            }
            //return "";
        }

       /// <summary>
       /// 부서로 넘어온것은 재귀호출로 처리
       /// </summary>
       /// <param name="ItemID"></param>
       /// <param name="UserID"></param>
       /// <param name="ToUserID"></param>
       /// <param name="Title"></param>
       /// <param name="Mode"></param>
        public void GlossaryShareInsert_SomePublic_Dept(string ItemID, string UserID, string ToUserID, string Title, string Mode)
        {
            //string connectionStringName = "ConnOrgChart";
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_list_department_person");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUserID.ToString());

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        string employeeID = (dr["employeeID"] == DBNull.Value) ? null : dr.Field<string>("employeeID");
                        if (!String.IsNullOrEmpty(employeeID)) { GlossaryShareInsert_SomePublic(ItemID, UserID, employeeID, Title, Mode); }
                    }
                }
            }
        }

       /// <summary>
       /// 2014-05-15 Mr.No 
       /// </summary>
       /// <param name="GlossaryID"></param>
        public void GlossaryShareDelete_GlossaryID(int GlossaryID)
        {
            GlossaryShareDac Dac = new GlossaryShareDac();
            Dac.GlossaryShareDelete_GlossaryID(GlossaryID);
        }
    }
}
