using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Transactions;
using SKT.Common;
using System.Configuration;

namespace SKT.Glossary.Biz
{
    public class GlossaryBiz
    {

        private string CutSummaryWithSearchWord(string Summary, string SearchKeyword)
        {
            string ret = string.Empty;

            if (Summary.Length > 500)
            {
                int WordStartindex = -1;
                if (SearchKeyword.Contains(" ") == true)
                {
                    string[] words = SearchKeyword.Split(' ');
                    foreach (string word in words)
                    {
                        int tempindex = Summary.IndexOf(word);
                        if (tempindex >= 0)
                        {
                            WordStartindex = tempindex;
                            break;  //제일 처음 걸리는 단어로 
                        }
                    }
                }
                else
                {
                    int tempindex = Summary.IndexOf(SearchKeyword);
                    if (tempindex >= 0)
                    {
                        WordStartindex = tempindex;
                    }
                }

                if (WordStartindex > 500)
                {
                    ret = "...";
                    if (Summary.Length - WordStartindex > 500)
                    {
                        ret = ret + Summary.Substring(WordStartindex, 500);
                        ret = ret + "...";
                    }
                    else
                    {
                        ret = Summary.Substring(WordStartindex, Summary.Length - WordStartindex);
                    }
                }
                else
                {
                    ret = Summary.Substring(0, 500);
                }



            }
            else  //길이가 안길면 그냥 리턴.
            {
                ret = Summary;
            }

            return ret;
        }

        public ArrayList GlossaryTitleList(int PageNum, int PageSize, out int TotalCount, string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryTitleList(PageNum, PageSize, SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.HistoryCount = dr["HistoryCount"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.Summary = CutSummaryWithSearchWord(Board.Summary, SearchKeyword);
                    //if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    //{
                    //    byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                    //    try
                    //    {
                    //        Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                    list.Add(Board);
                }
            }
            return list;
        }




        public ArrayList GlossaryContentsList(int PageNum, int PageSize, out int TotalCount, string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryContentsList(PageNum, PageSize, SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.HistoryCount = dr["HistoryCount"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");

                    Board.Summary = CutSummaryWithSearchWord(Board.Summary, SearchKeyword);
                    //if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    //{
                    //    byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                    //    try
                    //    {
                    //        Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                    list.Add(Board);
                }
            }
            return list;
        }


        public ArrayList GlossaryMyDocumentsList(int PageNum, int PageSize, out int TotalCount, out int WTikleCount, out int MTikleCount, out int TTikleCount, string UserID, string SearchType)
        {
            ArrayList list = new ArrayList();

            TotalCount = 0;
            WTikleCount = 0;
            MTikleCount = 0;
            TTikleCount = 0;

            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryMyDocumentsList(PageNum, PageSize, UserID, SearchType);
            WTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GW", 0);
            MTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GM", 0);
            TTikleCount = (int)DatabaseMethod.GetDataRow(ds.Tables[1].Rows[0], "V_GT", 0);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                    }
                    Board.HistoryYN = dr["HistoryYN"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    //Board.UserName = dr["UserName"].ToString();
                    //Board.DeptName = dr["DeptName"].ToString();
                    if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    {
                        byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                        try
                        {
                            Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 350) + "...";
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
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }

                    list.Add(Board);
                }
            }
            return list;
        }

        public GlossaryType GlossarySelect(string ItemID, string Mode)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossarySelect(ItemID, "", Mode);
            GlossaryType Board = new GlossaryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.FirstUserID = dr["UserID"].ToString();
                    Board.FirstDeptName = dr["DeptName"].ToString();
                    Board.FirstUserName = dr["UserName"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.ModifyDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                }
            }
            return Board;
        }

        public GlossaryType GlossarySelect(string ItemID, string UserID, string Mode)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossarySelect(ItemID, UserID, Mode);
            GlossaryType Board = new GlossaryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.FirstUserID = dr["UserID"].ToString();
                    Board.FirstDeptName = dr["DeptName"].ToString();
                    Board.FirstUserName = dr["UserName"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.ModifyDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    if (dr.Table.Columns.Contains("FirstCreateDate"))
                    {
                        Board.FirstCreateDate = (dr["FirstCreateDate"] == DBNull.Value) ? Board.CreateDate : (dr.Field<DateTime>("FirstCreateDate")).ToString("yyyy-MM-dd");
                        Board.FirstUserID = (dr["FirstUserID"] == DBNull.Value) ? Board.UserID : dr.Field<string>("FirstUserID");
                        Board.FirstDeptName = (dr["FirstDeptName"] == DBNull.Value) ? Board.DeptName : dr.Field<string>("FirstDeptName");
                        Board.FirstUserName = (dr["FirstUserName"] == DBNull.Value) ? Board.UserName : dr.Field<string>("FirstUserName");
                    }
                    if (Board.ID == Board.CommonID) //아이디와 보드아이디가 같으면 최초값이다.
                    {
                        Board.FirstPrivateYN = Board.PrivateYN;
                    }
                    else
                    {
                        Board.FirstPrivateYN = dr["FirstPrivateYN"].ToString();
                    }

                    Board.ModifyYN = dr["ModifyYN"].ToString();
                    if (!(dr.Table.Columns.Contains("NoteYN")))
                    {
                        Board.NoteYN = "N";
                    }
                    else
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }
                    Board.fromQnaID = dr["fromQnaID"].ToString();
                    Board.HistoryYN = dr["historyyn"].ToString();
                    if (dr.Table.Columns.Contains("HallOfFameYN"))
                    {
                        Board.HallOfFameYN = (dr["HallOfFameYN"] == DBNull.Value) ? "0" : Convert.ToString(dr["HallOfFameYN"]);
                    }
                    // 2014-04-29 Mr.No 추가
                    Board.CategoryID = (dr["CategoryID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["CategoryID"]);
                    // 2014-05-15 Mr.Mo 추가
                    Board.Permissions = (dr["Permissions"] == DBNull.Value) ? null : Convert.ToString(dr["Permissions"]);
                    // 2014-05-16 Mr.No 
                    // LearningWorld Info
                    Board.Summary = (dr["Summary"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["Summary"]);
                    Board.Hits = (dr["Hits"] == DBNull.Value) ? string.Empty : (Convert.ToString(dr["Hits"]));
                    Board.UserEmail = (dr["UserEmail"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["UserEmail"]);
                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("LastGrade"))
                    {
                        Board.LastGrade = (dr["LastGrade"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["LastGrade"]);
                        Board.LastGradeUrl = RankingUrl(Board.LastGrade);
                    }
                    if (dr.Table.Columns.Contains("FirstGrade"))
                    {
                        Board.FirstGrade = (dr["FirstGrade"] == DBNull.Value) ? Board.LastGrade : Convert.ToInt32(dr["FirstGrade"]);
                        Board.FirstGradeUrl = RankingUrl(Board.FirstGrade);
                    }
                    Board.CommentCount = (dr["commentCount"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["commentCount"]);
                    Board.HistoryCount = (dr["Historycount"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["Historycount"]);

                    // 2015-09-23 ksh
                    Board.PlatformYN = (dr["PlatformYN"] == DBNull.Value) ? "N" : Convert.ToString(dr["PlatformYN"]);
                    // 2015-10-12 ksh
                    Board.MarketingYN = (dr["MarketingYN"] == DBNull.Value) ? "N" : Convert.ToString(dr["MarketingYN"]);
                    // 2015-10-27 ksh
                    Board.TechTrendYN = (dr["TechTrendYN"] == DBNull.Value) ? "N" : Convert.ToString(dr["TechTrendYN"]);
                    // 2015-11-10 ksh
                    Board.JustOfficerYN = (dr["JustOfficerYN"] == DBNull.Value) ? "N" : Convert.ToString(dr["JustOfficerYN"]);

                    //프로퍼티용
                    if (dr.Table.Columns.Contains("GatheringID"))
                    {
                        Board.GatheringID = (dr["GatheringID"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["GatheringID"]);
                        Board.PublicYN = (dr["PublicYN"] == DBNull.Value) ? string.Empty : Convert.ToString(dr["PublicYN"]);
                    }

                    Board.DTBlogFlag = (dr["DTBlogFlag"] == DBNull.Value) ? "" : Convert.ToString(dr["DTBlogFlag"]);
                    Board.TWhiteFlag = (dr["TWhiteFlag"] == DBNull.Value) ? "" : Convert.ToString(dr["TWhiteFlag"]);
                }
            }
            return Board;
        }

        //질문 티끌화 등,state 확인 용으로 사용
        public GlossaryType GlossarySelectState(string ItemID, string UserID, string Mode)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossarySelect(ItemID, UserID, Mode);
            GlossaryType Board = new GlossaryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Board.ID = ds.Tables[0].Rows[0]["ID"].ToString();
                Board.CommonID = ds.Tables[0].Rows[0]["CommonID"].ToString();
                Board.HistoryYN = ds.Tables[0].Rows[0]["historyyn"].ToString();
            }
            return Board;
        }

        public GlossaryType GlossaryMyDocumentSelect(string ItemID, string HistoryYN)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryMyDocumentSelect(ItemID, HistoryYN);
            GlossaryType Board = new GlossaryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                }
            }
            return Board;
        }

        public GlossaryType GlossaryInsert(GlossaryType Board, string Mode)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryInsert(Board, Mode);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            if (Mode == "" || Mode == "qnaToTikle")
                GlossaryCommentIDUpate(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            return Board;
        }

        //20131219 , 티클 삭제
        public void TikleDelete(string userid, string commonid, string userip, string usermachinename)
        {
            GlossaryDac Dac = new GlossaryDac();
            Dac.TikleDelete(userid, commonid, userip, usermachinename);
        }

        //20140102 , QnA 삭제
        public void QnaDelete(string id, string userid, string userip, string usermachinename)
        {
            GlossaryDac Dac = new GlossaryDac();
            Dac.QnaDelete(id, userid, userip, usermachinename);
        }


        public void GlossaryCommentIDUpate(String CommonID)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryCommonIDUpate(CommonID);
        }


        public ArrayList GetTagList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetTagList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //    //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryTagType data = new GlossaryTagType();
                    data.ID = dr["ID"].ToString();
                    data.Title = dr["Title"].ToString();
                    data.CommonID = dr["CommonID"].ToString();
                    data.TagTitle = dr["TagTitle"].ToString();
                    data.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    list.Add(data);
                }
            }

            return list;

        }
        public ArrayList GetCBTUserList(string UserID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetCBTUserList(UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //    //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //GlossaryTagType data = new GlossaryTagType();
                    string data = dr["UserID"].ToString();

                    list.Add(data);
                }
            }

            return list;
        }

        //오늘 등록된 문서count 와 전체 갯수를 보여준다.
        public void GetEventData(string UserID, out string MyRank, out string WriteCount, out string AnswerCount, out string AttendanceCount, out string AnswerRank)
        {
            MyRank = "0";
            WriteCount = "0";
            AnswerCount = "0";
            AttendanceCount = "0";
            AnswerRank = "0";

            GlossaryDac dac = new GlossaryDac();
            DataSet ds = dac.GetEventData(UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MyRank = dr["MyRank"].ToString();
                    WriteCount = dr["WriteCount"].ToString();
                    AnswerRank = dr["AnswerRank"].ToString();
                    AnswerCount = dr["AnswerCount"].ToString();
                    AttendanceCount = dr["AttendanceCount"].ToString();
                }
            }
        }

        public ArrayList GetEventRankList()
        {
            ArrayList list = new ArrayList();

            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetEventRankList();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //    //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryEventType data = new GlossaryEventType();
                    //string data = dr["UserID"].ToString();
                    data.WriteCount = dr["WriteCount"].ToString();
                    data.ID = dr["RowNum"].ToString();
                    data.Name = dr["UserName"].ToString();
                    list.Add(data);
                }
            }

            return list;
        }

        public ArrayList GetEventReplyRankList()
        {
            ArrayList list = new ArrayList();

            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetEventReplyRankList();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //    //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryEventType data = new GlossaryEventType();
                    //string data = dr["UserID"].ToString();
                    data.WriteCount = dr["commentwrite"].ToString();
                    data.ID = dr["RowNum"].ToString();
                    data.Name = dr["UserName"].ToString();
                    list.Add(data);
                }
            }

            return list;
        }

        //public ArrayList GetDocHistoryUsers(string CommonID, out string FirstWriter, out string LastModifyer, out string FirstWriterName, out string LastModifyerName)
        //{
        //    ArrayList list = new ArrayList();
        //    FirstWriter = "0";
        //    LastModifyer = "0";
        //    FirstWriterName = "";
        //    LastModifyerName = "";

        //2013 11 26  GetDocHistoryUsers biz 는 사용하지않음.
        //GlossaryDac dac = new GlossaryDac();
        //DataSet ds = dac.GetDocHistoryUsers(CommonID);

        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        FirstWriter = dr["FirstWriter"].ToString();
        //        LastModifyer = dr["LastModifyer"].ToString();

        //        FirstWriterName = dr["FirstWriterName"].ToString();
        //        LastModifyerName = dr["LastModifyerName"].ToString();
        //    }
        //    foreach (DataRow dr in ds.Tables[1].Rows)
        //    {
        //        ImpersonUserinfo tuser = new ImpersonUserinfo();
        //        tuser.UserID = dr["UserID"].ToString();
        //        tuser.EmailAddress = dr["UserEmail"].ToString();
        //        tuser.Name = dr["UserName"].ToString();
        //        list.Add(tuser);
        //    }
        //}
        //return list;
        //}

        public ArrayList GlossaryHallofFameMainList(string Mode, string UserID)
        {
            ArrayList list = new ArrayList();
            GlossaryAdminDac Dac = new GlossaryAdminDac();
            DataSet ds = Dac.GlossaryAdminHallofFameList(Mode, 0, 0, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryHallOfFameType data = new GlossaryHallOfFameType();

                    data.RowNum = dr["RowNum"].ToString();
                    data.ID = dr["ID"].ToString();
                    data.GlossaryID = dr["GlossaryID"].ToString();
                    data.GlossaryFrom = dr["GlossaryFrom"].ToString();

                    data.Title = (dr["Title"] == DBNull.Value) ? "" : dr["Title"].ToString();
                    data.LastWriteUserID = (dr["LastWriteUserID"] == DBNull.Value) ? "" : dr["LastWriteUserID"].ToString();
                    data.LastWriteUserName = (dr["LastWriteUserName"] == DBNull.Value) ? "" : dr["LastWriteUserName"].ToString();
                    data.LastWriteUserDeptName = (dr["LastWriteUserDeptName"] == DBNull.Value) ? "" : dr["LastWriteUserDeptName"].ToString();

                    data.LikeCount = (dr["LikeCount"] == DBNull.Value) ? "" : dr["LikeCount"].ToString();
                    data.CssTitleBox = (dr["CssTitleBox"] == DBNull.Value) ? "" : dr["CssTitleBox"].ToString();

                    // 2014-06-17 Mr.No
                    data.photoURL = (dr["photoURL"] == DBNull.Value) ? "/common/images/user_none.png" : dr.Field<string>("photoURL");


                    list.Add(data);
                }
            }
            return list;
        }
        public void GlossaryHallOfFameInsert(string Mode, GlossaryHallOfFameType data)
        {
            GlossaryDac dac = new GlossaryDac();
            dac.GlossaryHallOfFameInsert(Mode, data);
        }


        /// <summary>
        /// 2014-06-16 Mr.No
        /// </summary>
        /// <param name="Grade"></param>
        /// <returns></returns>
        protected string RankingUrl(int Grade)
        {
            string ImageUrl = string.Empty;

            if (Grade != 0)
            {
                string Rank = string.Empty;
                if (Grade == 0) { Rank = "지존"; }
                else if (Grade == 1) { Rank = "고수"; }
                else if (Grade == 2) { Rank = "중수"; }
                else { Rank = "초수"; }

                ImageUrl = "<img class=\"icon_img\" width='19' height='19' title='" + Rank + "' src='" + ConfigurationManager.AppSettings["FrontImageUrl"] + Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "'/>";
            }
            return ImageUrl;
        }
        public DataSet GlossaryMainInfoSelect(string UserID)
        {
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryMainInfoSelect(UserID);

            return ds;
        }
        public DataSet GlossaryMainInfoSelect(string GatheringYN, string GatheringID, string UserID)
        {
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryMainInfoSelect(GatheringYN, GatheringID, UserID);

            return ds;
        }

        public DataSet GetGlossaryMainTagBoardSelect(string Board_Index, string Board_Count, string Board_RowCount, string UserID)
        {
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetGlossaryMainTagBoardSelect(Board_Index, Board_Count, Board_RowCount, UserID);

            return ds;
        }
        public DataSet GetGlossaryMainTagSelect(string Tag_Index, string Tag_Count, string GatheringYN, string GatheringID, string UserID)
        {
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            ds = dac.GetGlossaryMainTagSelect(Tag_Index, Tag_Count, GatheringYN, GatheringID, UserID);

            return ds;
        }

        //2015-09-10 Platform 업데이트
        public int Platformupdate(string commonid)
        {
            GlossaryDac Dac = new GlossaryDac();
            int result = Dac.GlossaryPlatformUpdate(commonid);
            return result;
        }

        public string GlossaryCreateWriteYN(string CommonID)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryCreateWriteYN(CommonID);
            return ds.Tables[0].Rows[0]["NoteSendYN"].ToString();
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.07.13
        Desc : 삭제하기 버튼 체크 로직 추가
        */
        public string GlossaryDeleteCheck(string CommonID,string UserId)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryDeleteCheck(CommonID,UserId);
            return ds.Tables[0].Rows[0]["UserCNT"].ToString();
        }

        public DataSet GlossaryGatheringItemCheck(string CommonID)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GlossaryGatheringItemCheck(CommonID);
            return ds;
        }

        /// <summary>
        /// CHG610000059179 / 2018-03-07 / 최현미 / 문의요청 담당자 및 메일링 발송자 리스트
        /// </summary>
        /// <param name="gubun">문의요청='C' / 메일발송 = 'M' / DT블로그 = 'D' </param>
        /// <returns></returns>
        public DataSet GetSpecialUserChargeSelect(string gubun)
        {
            GlossaryDac Dac = new GlossaryDac();
            DataSet ds = Dac.GetSpecialUserChargeSelect(gubun);
            return ds;
        }

        /// <summary>
        /// Tnet ContentFeeds 저장
        /// </summary>
        /// <param name="sbmid"></param>
        /// <param name="status"></param>
        /// <param name="errormessage"></param>
        public void SetTnetContentFeedsLog(string sbmid, string method, string status, string errormessage, string jsondata)
        {
            GlossaryDac dac = new GlossaryDac();
            dac.SetTnetContentFeedsLog(sbmid, method, status, errormessage, jsondata);
        }
    }

}
