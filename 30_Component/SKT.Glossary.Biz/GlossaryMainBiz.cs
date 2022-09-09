using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Dac;
using System.Collections;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryMainBiz
    {
        //화면 기존정보 조회(공지사항, 일정, 신입사원정보
        public DataSet BasicInfoSelect(string UserID)
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.BasicInfoSelect(UserID);

            return ds;
        }

        //오늘 등록된 문서count 와 전체 갯수를 보여준다.
        public void CountTodayTotalSelect(out string TodayCount, out string TotalCount, out string TodayModifyCount, out string DeptCount)
        {
            TodayCount = "0";
            TotalCount = "0";
            TodayModifyCount = "0";
            DeptCount = "0";
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.CountTodayTotalSelect();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TodayCount = dr["TodayCount"].ToString();
                    TotalCount = dr["TotalCount"].ToString();
                    TodayModifyCount = dr["TodayModifyCount"].ToString();
                    DeptCount = dr["DeptCount"].ToString();
                }
            }
        }

        //hits 수가 높은순으로 가져온다
        public ArrayList TopHitsList(int Count)
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TopHitsList(Count);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm");
                    list.Add(Board);
                }
            }
            return list;
        }

        //모든 활동을 가져온다.
        public ArrayList TotalActivity(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort, int PageNum = 1, string GatheringYN = "N", string GatheringID = null)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2015-09-09 Mr.Kim
                    if (dr.Table.Columns.Contains("PlatformYN") == true)
                    {
                        Board.PlatformYN = dr["PlatformYN"].ToString();
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }


        //모든 활동을 가져온다. (플랫폼페이지에서만)
        public ArrayList TotalActivity_Platform(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort, int PageNum = 1, string GatheringYN = "N", string GatheringID = null)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity_Platform(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }


        //모든 활동을 가져온다. (마케팅페이지에서만)
        public ArrayList TotalActivity_Marketing(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort, int PageNum = 1, string GatheringYN = "N", string GatheringID = null)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity_Marketing(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }

        //모든 활동을 가져온다. (마케팅페이지에서만)
        public ArrayList TotalActivity_TechTrend(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort, int PageNum = 1, string GatheringYN = "N", string GatheringID = null)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity_TechTrend(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }

        //CHG610000073120 / 2018-10-05 / DT블로그
        //CHG610000074852 / 20181108 / T생활백서
        public ArrayList TotalActivityNew(string gubun, string ParmaType, string UserID, int Count, string Mode, out int TotalCount, string SearchSort, string SearchText, int PageNum = 1)  
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = new DataSet();
            if(gubun.Equals("DT"))
                ds = Dac.TotalActivity_DT(ParmaType, UserID, PageNum, Count, Mode, SearchSort, SearchText);

            if (gubun.Equals("TW"))
                ds = Dac.TotalActivity_TW(ParmaType, UserID, PageNum, Count, Mode, SearchSort, SearchText);

            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }

                    if (gubun.Equals("DT") && Mode.Equals("HitsDT"))
                    {
                        Board.DTBlogFlag = dr["DTBlogFlag"] == null ? "" : Convert.ToString(dr["DTBlogFlag"]);
                    }

                    if (gubun.Equals("TW") && (Mode.Equals("HitsTW") || Mode.Equals("LikesTW")))
                    {
                        Board.TWhiteFlag = dr["TWhiteFlag"] == null ? "" : Convert.ToString(dr["TWhiteFlag"]);
                    }

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }

     

        //모든 활동을 가져온다. (티넷에서 링크타고 넘어온 경우)
        public ArrayList TotalActivity_Tnet(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort, int PageNum = 1, string GatheringYN = "N", string GatheringID = null)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity_Tnet(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    //Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.Gubun = dr["Gubun"].ToString();
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }


                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }


                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                 


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.05.18 
        //Desc : 끌모임 게시글 검색 기능 추가
        public ArrayList TotalActivity_GathringSearch(string UserID, int Count, string Mode, out int TotalCount, string CategoryID, string TagTitle, string SearchSort,string SearchText, int PageNum = 1, string GatheringYN = "Y", string GatheringID = null)
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.TotalActivity_GathringSearch(UserID, PageNum, Count, Mode, CategoryID, TagTitle, SearchText, SearchSort, GatheringYN, GatheringID);
            TotalCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                int Rowindex = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();

                    Board.RowNum = Rowindex.ToString();
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //if (Mode == "Total")
                    //{
                    //    Board.PrivateYN = dr["PrivateYN"].ToString();
                    //}
                    if (dr.Table.Columns.Contains("PrivateYN") == true)
                    {
                        Board.PrivateYN = dr["PrivateYN"].ToString();
                    }
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.Table.Columns.Contains("Type") == true)
                    {
                        Board.Type = dr["Type"].ToString();
                    }
                    if (dr.Table.Columns.Contains("MailYN") == true)
                    {
                        Board.MailYN = dr["MailYN"].ToString();
                    }
                    if (dr.Table.Columns.Contains("NoteYN") == true)
                    {
                        Board.NoteYN = dr["NoteYN"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LastCreateDate") == true)
                    {
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    if (dr.Table.Columns.Contains("FirstCreateDate") == true)
                    {
                        if (dr["FirstCreateDate"].ToString() != "")
                        {
                            Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        }
                    }

                    if (dr.Table.Columns.Contains("Summary") == true)
                    {
                        Board.Summary = dr["Summary"].ToString();
                        if (int.Parse(Board.Summary.Length.ToString()) > 200)
                        {
                            byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                            try
                            {
                                Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 550) + "...";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (dr.Table.Columns.Contains("HistoryCount") == true)
                    {
                        Board.HistoryCount = dr["HistoryCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        Board.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("HistoryYN") == true)
                    {
                        Board.HistoryYN = dr["HistoryYN"].ToString();
                    }
                    //Board.ItemState 이값이 상태를 표현한다.

                    if (dr.Table.Columns.Contains("LikeY") == true)
                    {
                        if (dr["LikeY"].ToString() == "Y")
                        {
                            Board.ItemState = "추천";
                        }
                        else if (dr["HistoryYN"].ToString() == "Y")
                        {
                            Board.ItemState = "편집";
                        }
                        else if (dr["HistoryYN"].ToString() == "N")
                        {
                            Board.ItemState = "등록";
                        }
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        Board.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        Board.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        Board.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // jmlee : 카테고리명
                    if (dr.Table.Columns.Contains("CategoryTitle") == true)
                    {
                        Board.CategoryName = dr["CategoryTitle"].ToString();
                    }

                    // 2014-06-16 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }

                    // 2015-09-09 Mr.Kim
                    if (dr.Table.Columns.Contains("PlatformYN") == true)
                    {
                        Board.PlatformYN = dr["PlatformYN"].ToString();
                    }

                    // 2014-06-24
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (Board.Grade == 0) { Board.Rank = "지존"; }
                        else if (Board.Grade == 1) { Board.Rank = "고수"; }
                        else if (Board.Grade == 2) { Board.Rank = "중수"; }
                        else { Board.Rank = "초수"; }
                    }


                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }

        //부서리스트를 가져온다.
        public ArrayList GetDeptList(int randindex)  //mode 는 4가지 Total,New,Modify,Hits,//2013-10-14 Best 추가
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.GetDeptList(randindex);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = ds.Tables[0].Rows.Count;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileType data = new GlossaryProfileType();
                    data.ID = dr["ID"].ToString();
                    data.UserID = dr["UserID"].ToString();
                    data.DeptCode = dr["DeptCode"].ToString();
                    data.DeptName = dr["DeptName"].ToString();
                    data.Contents = dr["Contents"].ToString();
                    data.ContentsModify = dr["ContentsModify"].ToString();
                    data.Summary = dr["Summary"].ToString();
                    data.CreateDate = dr["CreateDate"].ToString();
                    list.Add(data);
                }
            }
            return list;
        }

        public int GetDeptIndex(int randindex)
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.GetDeptIndex(randindex);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tempindex = dr["RandIndex"].ToString();

                    return int.Parse(tempindex);
                }
            }

            return 0;
        }

        //명예의 전당 리스트 가져오기.
        public ArrayList GlossaryHallofFameList(int PageNum, int PageSize, out int TotalCount, string UserID, string type, string CategoryID, out int TotalCount_All)
        {
            ArrayList list = new ArrayList();
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.GlossaryHallofFameList(PageNum, PageSize, UserID, type, CategoryID);
            TotalCount = 0;
            TotalCount_All = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount_All = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                TotalCount = ds.Tables.Count;   // 2014-06-24 Mr.No

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType data = new GlossaryType();
                    data.ID = dr["ID"].ToString();
                    data.CommonID = dr["CommonID"].ToString();
                    data.Title = dr["Title"].ToString();
                    data.UserID = dr["UserID"].ToString();
                    data.UserName = dr["UserName"].ToString();
                    data.DeptName = dr["DeptName"].ToString();
                    data.Summary = dr["Summary"].ToString();
                    data.Type = dr["Type"].ToString();
                    data.MailYN = dr["MailYN"].ToString();
                    data.NoteYN = dr["NoteYN"].ToString();

                    data.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm");

                    data.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    data.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        data.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                    }

                    // 1Do : 리스트 화면에 조회 수, 댓글 수, 추천 수 표시
                    if (dr.Table.Columns.Contains("LikeCount") == true)
                    {
                        data.LikeCount = dr["LikeCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        data.Hits = dr["Hits"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CommentCount") == true)
                    {
                        data.CommentCount = dr["CommentCount"].ToString();
                    }

                    if (dr.Table.Columns.Contains("NewCommentFlag") == true)
                    {
                        data.NewCommentFlag = Convert.ToBoolean(dr["NewCommentFlag"]);
                    }

                    // 1Do : 문서 권한 모드
                    if (dr.Table.Columns.Contains("Permissions") == true)
                    {
                        data.Permissions = Convert.ToString(dr["Permissions"]);
                    }

                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        data.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                    }
                    // 2014-06-24 Mr.No
                    if (dr.Table.Columns.Contains("Grade"))
                    {
                        if (data.Grade == 0) { data.Rank = "지존"; }
                        else if (data.Grade == 1) { data.Rank = "고수"; }
                        else if (data.Grade == 2) { data.Rank = "중수"; }
                        else { data.Rank = "초수"; }
                    }

                    data.PrivateYN = (dr["PrivateYN"] == DBNull.Value) ? null : dr.Field<string>("PrivateYN");

                    list.Add(data);
                }
            }
            return list;
        }

        /*
		 * 팝업 데이터 insert
		 */
        public DataSet PopupInsert(string UserID, string pop_Type)
        {
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("UP_POPUP_INSERT");

            db.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
            db.AddInParameter(dbCommand, "POP_Type", DbType.String, pop_Type);

            return db.ExecuteDataSet(dbCommand);
        }


        //2015-10-21 마케팅 유저 확인
        public DataSet MarketingUserCheck(string UserID)
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.MarketingUserCheck(UserID);

            return ds;
        }

        //2015-10-23 임원 확인
        public DataSet OfficerCheck(string UserID)
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.OfficerCheck(UserID);

            return ds;
        }

        //메인
        public DataSet GlossaryMainNotice(string UserID)
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.GlossaryMainNotice(UserID);

            return ds;
        }

        // 티넷 포틀릿 - DT블로그
        public DataSet GlossaryInterfaceTnet()
        {
            GlossaryMainDac Dac = new GlossaryMainDac();
            DataSet ds = Dac.GlossaryInterfaceTnet();

            return ds;
        }
    }
}
