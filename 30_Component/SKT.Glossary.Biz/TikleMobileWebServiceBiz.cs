using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using SKT.Glossary.Dac;
using SKT.Common;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
    public class DatabaseMethod
    {
        #region SqlDataSet datarow에서 데이터 꺼내기
        public static object GetDataRow(DataRow dr, string column, object returnDefualt)
        {
            try
            {
                object rtVal = dr[column];
                if (rtVal == DBNull.Value) return returnDefualt;
                return rtVal;
            }
            catch { return returnDefualt; }
        }

        public static bool CompareDataToString(DataRow dr, string column, string cmp)
        {
            try { return Convert.ToString(dr[column]).Equals(cmp); }
            catch { return false; }
        }

        public static object GetDBNullProcessedString(string CheckString)
        {
            if (string.IsNullOrEmpty(CheckString))
            {
                return DBNull.Value;
            }
            return CheckString;
        } 
        #endregion
    }

    public class TikleMobileWebServiceBiz
    {
        /***************  TIKLE ***************/  

        #region NewList
        //모든 활동을 가져온다.
        public ArrayList NewList(string LoginUserID, string Mode, string CategoryID, int Count, out int TotalCount, int PageNum = 1)  //mode : Total,New,Modify,Hits,Category...
        {
            //string Mode = "New";
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TotalActivity(LoginUserID, CategoryID, PageNum, Count, Mode);
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
                    if (Mode == "Total")
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
                        //if (int.Parse(Board.Summary.Length.ToString()) > 200)  ...으로 자르기 인데.. 그냥줌 웹서비스는 그냥줌.
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

                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    // 1Do
                    //if (dr.Table.Columns.Contains("Grade") == true)
                    //{
                    //    Board.UserGrade = dr["Grade"].ToString();
                    //}

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }
        #endregion

        #region NewCategoryList
        public ArrayList NewCategoryList(string LoginUserID, string Mode, string CategoryID, int Count, out int TotalCount, int PageNum = 1)  //mode : Total,New,Modify,Hits,Category...
        {
            //string Mode = "New";
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TotalActivityCategory(LoginUserID, CategoryID, PageNum, Count, Mode);
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
                    if (Mode == "Total")
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
                        //if (int.Parse(Board.Summary.Length.ToString()) > 200)  ...으로 자르기 인데.. 그냥줌 웹서비스는 그냥줌.
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

                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    // 1Do
                    //if (dr.Table.Columns.Contains("Grade") == true)
                    //{
                    //    Board.UserGrade = dr["Grade"].ToString();
                    //}

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        } 
        #endregion

        #region PlatformTechTrendList
        //2015-11-26 플랫폼,TechTrend 리스트를 가져온다
        public ArrayList PlatformTechTrendList(string LoginUserID, string Mode, int Count, out int TotalCount, int PageNum = 1)  //mode : Total,New,Modify,Hits,Category...
        {
            //string Mode = "New";
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.PlatformTechTrendSelect(LoginUserID, PageNum, Count, Mode);
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
                    if (Mode == "Total")
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
                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        } 
        #endregion

        #region PlatTrendUserPermission
        //2015-12-04 플랫폼,TechTrend 접근 권한 확인
        public string PlatTrendUserPermission(string LoginUserID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.PlatTrendUserPermission(LoginUserID);

            string UserPermission = ds.Tables[0].Rows[0]["UserPermission"].ToString();

            return UserPermission;
        } 
        #endregion

        #region GlossarySelect
        public GlossaryType GlossarySelect(string ItemID, string UserID, string Mode)
        {
            //string Mode = "Histroy";
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossarySelect(ItemID, UserID, Mode);
            GlossaryType Board = new GlossaryType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.PrivateYN = dr["PrivateYN"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.FirstUserID = dr["UserID"].ToString();
                    Board.FirstDeptName = dr["DeptName"].ToString();
                    Board.FirstUserName = dr["UserName"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.ModifyDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                        Board.FirstUserID = dr["FirstUserID"].ToString();
                        Board.FirstDeptName = dr["FirstDeptName"].ToString();
                        Board.FirstUserName = dr["FirstUserName"].ToString();
                    }
                    if (Board.ID == Board.CommonID) //아이디와 보드아이디가 같으면 최초값이다.
                    {
                        Board.FirstPrivateYN = Board.PrivateYN;
                    }

                    Board.GatheringID = dr["GatheringID"].ToString();
                    if (dr["DTBlogFlag"] != null)
                        Board.DTBlogFlag = dr["DTBlogFlag"].ToString();

                    if (dr["TWhiteFlag"] != null)
                        Board.TWhiteFlag = dr["TWhiteFlag"].ToString();

                    // 1Do
                    //Board.UserGrade = dr["LastGrade"].ToString(); // 편집자 랭킹
                    //Board.FirstUserGrade = dr["FirstGrade"].ToString() ?? dr["LastGrade"].ToString(); // 최초 작성자 랭킹
                }
            }
            return Board;
        } 
        #endregion

        #region GlossaryViewMenuSelect
        //뷰페이지 메뉴들 Select
        public GlossaryControlType GlossaryViewMenuSelect(string UserID, string CommonID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryViewMenuSelect(UserID, CommonID);
            GlossaryControlType Board = new GlossaryControlType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.ReadYN = dr["ReadYN"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.LikeY = dr["LikeYN"].ToString();
                    Board.Historycount = dr["Historycount"].ToString();
                    Board.ScrapCount = dr["ScrapCount"].ToString();
                }
            }
            return Board;
        } 
        #endregion

        #region GlossaryTagList
        //태그 리스트
        public ArrayList GlossaryTagList(string CommonID, string Title)
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryTagList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Title"].ToString() != Title)
                    {
                        GlossaryControlType Board = new GlossaryControlType();
                        Board.ID = dr["ID"].ToString();
                        Board.Title = dr["Title"].ToString();
                        list.Add(Board);
                    }
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryTagSelect
        //태그 리스트
        public string GlossaryTagSelect(string CommonID)
        {
            GlossaryDac dac = new GlossaryDac();
            DataSet ds = new DataSet();
            string result = "";
            ds = dac.GetTagList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result += dr["TagTitle"].ToString()+",";
                    
                }
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        #endregion

        #region GlossaryLikeInsert
        //좋아요 추가
        public void GlossaryLikeInsert(GlossaryControlType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryLikeInsert(Board);
        } 
        #endregion

        #region GlossaryScrapInsert
        //스크랩 추가
        public void GlossaryScrapInsert(GlossaryScrapType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryScrapInsert(Board);
        } 
        #endregion

        #region GlossaryQnAList
        //QnA 목록 리스트
        public ArrayList GlossaryQnAList(int PageNum, int PageSize, string SearchKeyword, string SearchType, string UserID, out int Total)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            Total = 0;
            //SuccessCount = 0;
            //UnSuccessCount = 0;
            //MyQnA = 0;
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnAList(PageNum, PageSize, SearchKeyword, SearchType, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                //SuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "SuccessCount", 0);
                //UnSuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "UnSuccessCount", 0);
                Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);
                //MyQnA = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "MyQnA", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnAType Board = new GlossaryQnAType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Hits = dr["Hits"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.BastReplyYN = dr["BestReplyYN"].ToString();

                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    //if (int.Parse(Board.Summary.Length.ToString()) > 200)
                    //{
                    //byte[] maByte = System.Text.Encoding.Default.GetBytes(Board.Summary);
                    //try
                    //{
                    //    Board.Summary = System.Text.Encoding.Default.GetString(maByte, 0, 320) + "...";
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //}

                    // 1Do
                    //if (dr.Table.Columns.Contains("Grade") == true)
                    //{
                    //    Board.UserGrade = dr["Grade"].ToString();
                    //}

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryQnASelect
        //QnA Select
        public GlossaryQnAType GlossaryQnASelect(string ID, int Count = 0)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnASelect(ID, Count);
            GlossaryQnAType Board = new GlossaryQnAType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Hits = dr["Hits"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    // 1Do
                    //Board.UserGrade = dr["UserGrade"].ToString();
                }
            }
            return Board;
        } 
        #endregion

        #region GlossaryQnACommentSuccessSelect
        //QnA 베스트 댓글 select 
        public GlossaryQnACommentType GlossaryQnACommentSuccessSelect(string ID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnACommentSuccessSelect(ID);

            GlossaryQnACommentType Board = new GlossaryQnACommentType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();

                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    //Board.UserEmail = dr["UserEmail"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    // 1Do                    
                    //Board.UserGrade = dr["UserGrade"].ToString();
                }
            }
            return Board;
        } 
        #endregion

        #region GlossaryQnABastCommentList
        //QnA 댓글 베스트 목록 리스트
        public ArrayList GlossaryQnABastCommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnABastCommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnACommentType Board = new GlossaryQnACommentType();
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.PhotoUrl = dr["PhotoUrl"].ToString();
                    if (string.IsNullOrEmpty(dr["PhotoUrl"].ToString()))
                        Board.PhotoUrl = "/Common/images/user_none.png";
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    // 1Do
                    //Board.UserGrade = dr["UserGrade"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryQnACommentList
        //QnA 댓글 목록 리스트
        public ArrayList GlossaryQnACommentList(string CommonID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnACommentList(CommonID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnACommentType Board = new GlossaryQnACommentType();
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.PhotoUrl = dr["PhotoUrl"].ToString();
                    if (string.IsNullOrEmpty(dr["PhotoUrl"].ToString()))
                        Board.PhotoUrl = "/Common/images/user_none.png";
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");

                    // 1Do
                    //Board.UserGrade = dr["Grade"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryQnABastSuccessComment
        //QnA  베스트 댓글  추가
        public GlossaryQnACommentType GlossaryQnABastSuccessComment(GlossaryQnACommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnABastSuccessComment(Board);
            return Board;
        } 
        #endregion

        #region UserSelect
        //사용자 사번으로 사용자정보를 가져온다.
        public ImpersonUserinfo UserSelect(string UserID)
        {
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            DataSet ds = new DataSet();
            ds = dac.UserSelect(UserID);

            ImpersonUserinfo Info = new ImpersonUserinfo();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Info.UserID = dr["EmpID"].ToString();
                Info.Name = dr["Name"].ToString();
                Info.DeptID = dr["DeptID"].ToString();
                Info.DeptName = dr["DeptName"].ToString();
                Info.EmailAddress = dr["EmailAddress"].ToString();
                Info.WorkArea = dr["WorkArea"].ToString();
                Info.Part = dr["Part"].ToString();

                Info.TEL = dr["TEL"].ToString();
                Info.Phone = dr["Phone"].ToString();

                Info.PhotoUrl = dac.GetPicture(UserID);
                Info.JobCode = dr["JobCode"].ToString();

                //<!--2015.03 수정 -->
                Info.ViewLevel = dr["ViewLevel"].ToString();
                Info.PositionName = dr["PositionName"].ToString();
                Info.ManagerEmployeeID = dr["ManagerEmployeeID"].ToString();
                // 수정 끝

                Info.Level = dr["Level"].ToString();
            }

            return Info;
        }
        
        #endregion

        #region GlossaryQnACommentInsert
        //QnA  댓글  추가
        public GlossaryQnACommentType GlossaryQnACommentInsert(GlossaryQnACommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnACommentInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        } 
        #endregion

        #region GlossaryQnACommentUpdate
        //QnA  댓글  업데이트
        public GlossaryQnACommentType GlossaryQnACommentUpdate(GlossaryQnACommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnACommentUpdate(Board);

            return Board;
        } 
        #endregion

        #region GlossaryQnACommentLikeY
        //QnA  댓글  추천
        public GlossaryQnACommentType GlossaryQnACommentLikeY(GlossaryQnACommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnACommentLikeY(Board);
            Board.LikeY = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        } 
        #endregion

        #region GlossaryScrapList
        //스크랩 목록 리스트
        public ArrayList GlossaryScrapList(int PageNum, int PageSize, out int TotalCount, string UserID)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryScrapList(PageNum, PageSize, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryScrapType Board = new GlossaryScrapType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.YouUserID = dr["YouUserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    if (dr["LastCreateDate"] != DBNull.Value)
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    if (dr["FirstCreateDate"] != DBNull.Value)
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                    }
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

                    // 1Do
                    //if (dr.Table.Columns.Contains("UserGrade") == true)
                    //{
                    //    Board.UserGrade = dr["UserGrade"].ToString();
                    //}

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryQnACommentSelect
        //QnA 댓글 Select
        public GlossaryQnACommentType GlossaryQnACommentSelect(string ID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryQnACommentSelect(ID);
            GlossaryQnACommentType Board = new GlossaryQnACommentType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.PublicYN = dr["PublicYN"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                }
            }
            return Board;
        } 
        #endregion

        #region SearchGlossary
        //티끌 검색
        public ArrayList SearchGlossary(string LoginUserID, int PageNum, int Count, out int TotalCount, string SearchKeyword, string SearchSort)  //mode 는 4가지 웹서비스에서는 최근만쓴다. Total,New,Modify,Hits,//
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryMobileSearch(LoginUserID, PageNum, Count, SearchKeyword, SearchSort);
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
                        //if (int.Parse(Board.Summary.Length.ToString()) > 200)  ...으로 자르기 인데.. 그냥줌 웹서비스는 그냥줌.
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
                    if (dr.Table.Columns.Contains("Hits") == true)
                    {
                        Board.Hits = dr["Hits"].ToString();
                    }

                    // 1Do
                    //if (dr.Table.Columns.Contains("UserGrade") == true)
                    //{
                    //    Board.UserGrade = dr["UserGrade"].ToString();
                    //}

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        } 
        #endregion

        #region SearchGlossary_PlatTrend
        //티끌 검색
        public ArrayList SearchGlossary_PlatTrend(string LoginUserID, int PageNum, int Count, string SearchKeyword, string Mode)  //mode 는 4가지 웹서비스에서는 최근만쓴다. Total,New,Modify,Hits,//
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GlossaryMobileSearch_PlatTrend(LoginUserID, PageNum, Count, SearchKeyword, Mode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
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

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }
        
        #endregion

        #region SearchScrap
        //스크랩 검색
        public ArrayList SearchScrap(int PageNum, int Count, string SearchKeyword, string UserID)
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.ScrapMobileSearch(PageNum, Count, SearchKeyword, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryScrapType Board = new GlossaryScrapType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.GlossaryID = dr["GlossaryID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Type = dr["Type"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.YouUserID = dr["YouUserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    if (dr["LastCreateDate"] != DBNull.Value)
                        Board.LastCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");
                    if (dr["LastCreateDate"] != DBNull.Value)
                        Board.FirstCreateDate = Convert.ToDateTime(dr["LastCreateDate"]).ToString("yyyy-MM-dd");

                    if (dr["FirstCreateDate"].ToString() != "")
                    {
                        Board.FirstCreateDate = Convert.ToDateTime(dr["FirstCreateDate"]).ToString("yyyy-MM-dd");
                    }
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

                    // 1Do
                    //if (dr.Table.Columns.Contains("UserGrade") == true)
                    //{
                    //    Board.UserGrade = dr["UserGrade"].ToString();
                    //}

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region TikleCount - 사용안함
        public void TikleCount(string userid, string type)
        {
            GlossaryPageRequestType gprt = new GlossaryPageRequestType();

            gprt.UserID = userid;
            gprt.Type = type;

            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            dac.InsertEventAttendance(gprt);


        } 
        #endregion

        #region Permissions_Check
        /// <summary>
        /// Permisstion Check
        /// </summary>
        public int Permissions_Check(string ItemID, string UserID)
        {
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            int returnValue = dac.Permissions_Check(ItemID, UserID);

            return returnValue;
        } 
        #endregion

        #region 카테고리 목록

        public ResultCategoryList CategoryList()
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.CategoryList();
            ResultCategoryList list = new ResultCategoryList();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CategoryType item = new CategoryType();
                    if (dr["ID"] != DBNull.Value)
                        item.ID = dr["ID"].ToString();
                    if (dr["CategoryTitle"] != DBNull.Value)
                        item.Title = dr["CategoryTitle"].ToString();
                    list.CategoryList.Add(item);
                }
            }
            return list;
        }

        #endregion

        #region 첨부파일 목록

        public ResultAttachmentList AttachmentList(int ItemID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.AttachmentList(ItemID);
            ResultAttachmentList list = new ResultAttachmentList();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AttachmentType item = new AttachmentType();
                    if (dr["FileName"] != DBNull.Value)
                        item.FileName = dr["FileName"].ToString().Replace("'", "//");
                    if (dr["Folder"] != DBNull.Value)
                        item.FilePath = dr["Folder"].ToString();
                    if (dr["FileSize"] != DBNull.Value)
                        item.FileSize = dr["FileSize"].ToString();
                    if (dr["itemGuid"] != DBNull.Value)
                        item.itemGuid = dr["itemGuid"].ToString();
                    list.AttachmentList.Add(item);
                }
            }
            return list;
        }

        //2016-07-27 추가
        public ResultAttachmentList AttachmentList_Full(int ItemID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.AttachmentList(ItemID);
            ResultAttachmentList list = new ResultAttachmentList();

            // Mr.No 2015-07-01
            // Mobile Download Full Path Setting
            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            string DownloadPath = System.Configuration.ConfigurationManager.AppSettings["DownloadControlServerHandlerUrl2"];
            // Folder=Glossary 고정4
            

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AttachmentType item = new AttachmentType();
                    if (dr["FileName"] != DBNull.Value)
                        item.FileName = dr["FileName"].ToString().Replace("'", "//");
                    if (dr["Folder"] != DBNull.Value)
                        item.FilePath = DownloadPath + "?" + "Folder=Glossary" + "&ItemGuid=" + dr["itemGuid"].ToString() + "&FileName=" + System.Web.HttpUtility.UrlEncode(dr["FileName"].ToString().Replace("'", "//"));  //;
                    if (dr["FileSize"] != DBNull.Value)
                        item.FileSize = dr["FileSize"].ToString();
                    if (dr["itemGuid"] != DBNull.Value)
                        item.itemGuid = dr["itemGuid"].ToString();
                    list.AttachmentList.Add(item);
                }
            }
            return list;
        }


        
        #endregion

        #region 랭킹 정보

        public RankingType RankingInfo(string UserID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.RankingInfo(UserID);
            RankingType retItem = new RankingType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Grade"] != DBNull.Value)
                        retItem.Grade = dr["Grade"].ToString();
                    if (dr["ID"] != DBNull.Value)
                        retItem.ID = dr["ID"].ToString();
                    if (dr["TotalScore"] != DBNull.Value)
                        retItem.TotalScore = dr["TotalScore"].ToString();
                }
            }
            return retItem;
        }

        #endregion

        /////////////////////////////////
        //2016-06 추가 Method
        /////////////////////////////////

        #region TikleCommentInsert
        //댓글 추가
        public DataSet TikleCommentInsert(CommCommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TikleCommentInsert(Board);
            //Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return ds;
        } 
        #endregion

        #region TikleReCommentInsert
        //댓글 덧글 추가
        public DataSet TikleReCommentInsert(CommCommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TikleReCommentInsert(Board);
            //Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return ds;
        }
        #endregion

        #region TikleCommentModify
        //댓글 수정
        public DataSet TikleCommentModify(CommCommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TikleCommentModify(Board);
            return ds;
        } 
        #endregion

        #region TikleCommentDelete
        //댓글 삭제
        public DataSet TikleCommentDelete(CommCommentType Board)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.TikleCommentDelete(Board);
            return ds;
        } 
        #endregion

        #region QNAInsert
        //질문 추가
        public DataSet QNAInsert(string LoginUserID, string title, string contents)
        {
            GlossaryQnAType Board = new GlossaryQnAType();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();

            ImpersonUserinfo info = UserSelect(LoginUserID);

            Board.UserID = info.UserID;    //작성자 ID
            Board.UserName = info.Name;    //작성자 이름
            Board.DeptName = info.DeptName;
            Board.UserEmail = info.EmailAddress;   //작성자 이메일
            Board.Title = title;//SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.Summary = contents;//Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
            //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
            Board.Contents = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            Board.ContentsModify = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용

            Board.PlatformYN = "N";
            Board.MarketingYN = "N";
            
            DataSet ds = Dac.QNAInsert(Board);
            return ds;
        }   
        #endregion

        #region QNAModify
        //질문 수정
        public DataSet QNAModify(string LoginUserID,string QNAID, string title, string contents)
        {
            GlossaryQnAType Board = new GlossaryQnAType();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();

            ImpersonUserinfo info = UserSelect(LoginUserID);

            Board.ID = QNAID;
            Board.UserID = info.UserID;    //작성자 ID
            Board.UserName = info.Name;    //작성자 이름
            Board.DeptName = info.DeptName;
            Board.UserEmail = info.EmailAddress;   //작성자 이메일
            Board.Title = title;//SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.Summary = contents;//Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
            //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
            Board.Contents = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            Board.ContentsModify = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용

            Board.PlatformYN = "N";
            Board.MarketingYN = "N";

            DataSet ds = Dac.QNAModify(Board);
            return ds;
        }
        #endregion

        #region QNAModify
        //질문 삭제
        public void QNADelete(string LoginUserID, string QNAID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            Dac.QNADelete(LoginUserID, QNAID);
        }
        #endregion

        #region GatheringList
        //끌 모임 목록 가져오기
        public ArrayList GatheringList(string LoginUserID, string Mode, int PageNum, int PageSize, out int TotalCount)
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = new DataSet();
            ds = Dac.GatheringList(LoginUserID, Mode, PageNum, PageSize);
            TotalCount = 0;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryGatheringListType Board = new GlossaryGatheringListType();
                    Board.GatheringID = dr["GatheringID"].ToString();
                    Board.GatheringName = HttpUtility.HtmlDecode(dr["GatheringName"].ToString());
                    //Board.UseYN = dr["UseYN"].ToString();
                    //Board.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    //Board.EditDate = Convert.ToDateTime(dr["EditDate"]);
                    Board.CreateUserID = dr["AuthID"].ToString();
                    Board.CreateUserName = dr["AuthorName"].ToString();
                    //Board.Editor = dr["Editor"].ToString();
                    Board.JoinCount = dr["AUTHUSERCNT"].ToString();
                    Board.MobileNotiYN = dr["MobileNotiYN"].ToString();
                    //Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");

                    list.Add(Board);
                }
            }
            return list;
        }
        #endregion

        #region GatheringNoticeList
        public ArrayList GatheringNoticeList(string LoginUserID, string GatheringID, int PageSize, out int TotalCount, int PageNum = 1)  //mode : Total,New,Modify,Hits,Category...
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.GatheringNoticeList(LoginUserID, GatheringID, PageNum, PageSize);
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
                    Board.PrivateYN = dr["PrivateYN"].ToString();
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
                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }
        #endregion

        #region GatheringNoticeInsert
        //모임 게시글 작성
        public DataSet GatheringNoticeInsert(string LoginUserID, string title, string contents, string GatheringID, string TagTitle)
        {
            GlossaryType Board = new GlossaryType();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            ImpersonUserinfo info = UserSelect(LoginUserID);
            GlossaryBiz Gbiz = new GlossaryBiz();

            Board.UserID = info.UserID;    //작성자 ID
            Board.UserName = info.Name;    //작성자 이름
            Board.DeptName = info.DeptName;
            Board.UserEmail = info.EmailAddress;   //작성자 이메일
            Board.Title = title;//SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.Summary = contents;//Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
            //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
            Board.Contents = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            Board.ContentsModify = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용
            Board.Description = "처음 작성된 글입니다";
            Board.PrivateYN = "N";
            Board.HistoryYN = "N";
            Board.Permissions = "GatheringPublic";
            Board.CategoryID = 129;
            Board.PlatformYN = "N";
            Board.MarketingYN = "N";
            Board.TechTrendYN = "N";
            Board.JustOfficerYN = "N";
            Board.Type = "wiki";

            // 1. 끌모임 게시글 작성
            DataSet ds = Dac.GatheringNoticeInsert(Board,"");
            
            // 2. 끌모임 게시글 commonID 업데이트
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            Gbiz.GlossaryCommentIDUpate(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            
            // 3. 끌모임 <-> 게시글 매핑
            gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Knowledge", Board.ID);

            // 4. 태그 저장
            GlossaryControlDac dac = new GlossaryControlDac();
            dac.GlossaryTagDelete((Board.ID).ToString());
            if (!string.IsNullOrEmpty(TagTitle.Trim()))
            {
                GlossaryControlType ConBoard = new GlossaryControlType();
                ConBoard.CommonID = (Board.ID).ToString();
                ConBoard.Title = Board.Title;
                ConBoard.UserID = LoginUserID;

                if (!(TagTitle.IndexOf(',') == -1))
                {
                    string[] Tag = TagTitle.Split(',');
                    for (int i = 0; i < Tag.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Tag[i].Trim()))
                        {
                            //ConBoard.TagTitle = Tag[i].Remove(Tag[i].LastIndexOf(",")).Trim();
                            ConBoard.TagTitle = Tag[i];
                            dac.GlossaryTagInsert(ConBoard);
                        }

                    }
                }
                else
                {
                    ConBoard.TagTitle = TagTitle.Trim();
                    dac.GlossaryTagInsert(ConBoard);
                }

                dac.GlossaryTagTotal();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].ToString() == "N")
            {
                GlossarySearchBiz bizGlossarySearch = new GlossarySearchBiz();
                bizGlossarySearch.SetSearchGlossarySyncDataUpdate("Glossary", ds.Tables[0].Rows[0][0].ToString());
            }

            return ds;
        }
        #endregion

        #region GatheringNoticeModify
        //모임 게시물 수정
        public DataSet GatheringNoticeModify(string LoginUserID, string CommonID,string title, string contents, string GatheringID, string TagTitle)
        {
            GlossaryType Board = new GlossaryType();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();
            ImpersonUserinfo info = UserSelect(LoginUserID);
            GlossaryBiz Gbiz = new GlossaryBiz();

            Board.ID = CommonID;
            Board.CommonID = CommonID;
            Board.UserID = info.UserID;    //작성자 ID
            Board.UserName = info.Name;    //작성자 이름
            Board.DeptName = info.DeptName;
            Board.UserEmail = info.EmailAddress;   //작성자 이메일
            Board.Title = title;//SecurityHelper.Clear_XSS_CSRF(txtTitle.Value).Trim(); //제목
            Board.Summary = contents;//Utility.RemoveHtmlTag(hdNamoContent.Value);    //text 내용
            //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
            Board.Contents = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();
            Board.ContentsModify = contents;//SecurityHelper.Clear_XSS_CSRF(hdNamoContent.Value).Trim();  //html 내용
            Board.Description = "변경내용 미 작성";
            Board.PrivateYN = "N";
            Board.HistoryYN = "N";
            Board.Permissions = "GatheringPublic";
            Board.CategoryID = 129;
            Board.Type = "wiki";
            Board.JustOfficerYN = "N";

            // 1. 끌모임 게시글 작성
            DataSet ds = Dac.GatheringNoticeInsert(Board,"History");
            
            // 2. 끌모임 게시글 commonID 업데이트
            //Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            //Gbiz.GlossaryCommentIDUpate(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            
            // 3. 끌모임 <-> 게시글 매핑
            gatheringBiz.GatheringMenuAuth_Insert(GatheringID, "Knowledge", Board.ID);

            // 4. 태그 저장
            GlossaryControlDac dac = new GlossaryControlDac();
            dac.GlossaryTagDelete((Board.ID).ToString());
            if (!string.IsNullOrEmpty(TagTitle.Trim()))
            {
                GlossaryControlType ConBoard = new GlossaryControlType();
                ConBoard.CommonID = (Board.ID).ToString();
                ConBoard.Title = Board.Title;
                ConBoard.UserID = LoginUserID;

                if (!(TagTitle.IndexOf(',') == -1))
                {
                    string[] Tag = TagTitle.Split(',');
                    for (int i = 0; i < Tag.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Tag[i].Trim()))
                        {
                            //ConBoard.TagTitle = Tag[i].Remove(Tag[i].LastIndexOf(",")).Trim();
                            ConBoard.TagTitle = Tag[i];
                            dac.GlossaryTagInsert(ConBoard);
                        }

                    }
                }
                else
                {
                    ConBoard.TagTitle = TagTitle.Trim();
                    dac.GlossaryTagInsert(ConBoard);
                }

                dac.GlossaryTagTotal();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["IsTestServer"].ToString() == "N")
            {
                GlossarySearchBiz bizGlossarySearch = new GlossarySearchBiz();
                bizGlossarySearch.SetSearchGlossarySyncDataUpdate("Glossary", Board.CommonID);
            }

            return ds;
        }
        #endregion

        #region GatheringNoticeDelete
        //모임 게시글 삭제
        public Boolean GatheringNoticeDelete(string LoginUserID, string CommonID)
        {
            GlossaryBiz GBiz = new GlossaryBiz();
            GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();

            string GatheringID = "9999";
            DataSet ds = new DataSet();

            try{
                GBiz.TikleDelete(LoginUserID, CommonID, "", "TikleMobile");
                gatheringBiz.GatheringMenuAuth_Delete(CommonID, "Knowledge", GatheringID);
                return true;
            }catch(Exception e){
                return false;
            }
        }
        #endregion

        #region MarketingTikleList
        public ArrayList MarketingTikleList(string LoginUserID, int PageSize, out int TotalCount, int PageNum = 1)  //mode : Total,New,Modify,Hits,Category...
        {
            ArrayList list = new ArrayList();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.MarketingTikleList(LoginUserID, PageNum, PageSize);
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
                    Board.PrivateYN = dr["PrivateYN"].ToString();
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
                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }
        #endregion

        #region MarketingQnaList
        public ArrayList MarketingQnaList(string LoginUserID, int PageIndex, int PageSize, out int TotalCount)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();

            DataSet ds = new DataSet();
            ds = dac.MarketingQnaList(LoginUserID, PageIndex, PageSize);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryQnAType Board = new GlossaryQnAType();
                    Board.ID = dr["ID"].ToString();
                    Board.RowNum = dr["RowNum"].ToString();
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Hits = dr["Hits"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.BastReplyYN = dr["BestReplyYN"].ToString();

                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");

                    list.Add(Board);
                }
            }
            return list;
        } 
        #endregion

        #region GlossaryComment
        public List<GlossaryCommentTypeM> GlossaryComment(string commonID)
        {
            //WeeklyCommentDac dac = new WeeklyCommentDac();
            //List<GlossaryCommentType> wctList = new List<GlossaryCommentType>();

            //using (DataSet ds = dac.WeeklyComment_List_New(commonID))
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        foreach (DataRow dr in ds.Tables[0].Rows)
            //        {
            //            WeeklyCommentType dwt = new WeeklyCommentType();
            //            dwt.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
            //            dwt.WeeklyCommentID = (dr["WeeklyCommentID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyCommentID");
            //            dwt.SUP_ID = (dr["SUP_ID"] == DBNull.Value) ? 0 : dr.Field<long>("SUP_ID");
            //            dwt.UserID = dr["UserID"] as String;
            //            dwt.UserName = dr["UserName"] as String;
            //            dwt.DutyName = dr["DutyName"] as String;
            //            dwt.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("CreateDateTime");
            //            dwt.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
            //            dwt.Contents = SecurityHelper.Add_XSS_CSRF(dr["Contents"] as String);
            //            wctList.Add(dwt);
            //        }
            //    }
            //}

            //return wctList;


            List<GlossaryCommentTypeM> GCList = new List<GlossaryCommentTypeM>();

            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();


            using (DataSet ds = Dac.GlossaryCommentSelect(commonID, "Glossary"))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryCommentTypeM gct = new GlossaryCommentTypeM();
                        gct.ID = dr["ID"].ToString();
                        gct.CommonID = dr["CommonID"].ToString();
                        gct.SUP_ID = dr["SUP_ID"].ToString();
                        gct.Contents = SecurityHelper.Add_XSS_CSRF(dr["Contents"] as String);
                        gct.UserID = dr["UserID"].ToString();
                        gct.UserName = dr["UserName"].ToString();
                        gct.DeptName = dr["DeptName"].ToString();
                        gct.PublicYN = dr["PublicYN"].ToString();
                        gct.CreateDate = dr["CreateDate"].ToString();
                        gct.LastModifyDate = dr["LastModifyDate"].ToString();
                        GCList.Add(gct);
                    }
                }
            }
            return GCList;
        }
        #endregion

        #region GlossaryTitleCheck
        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.08.04 
        //Desc : 제목 중복 웹서비스 추가
        public string GlossaryTitleCheck(string Title, string commonID, string GatheringYN, string GatheringID)
        {
            GlossaryControlBiz biz = new GlossaryControlBiz();
            string ds = biz.ExistTitle(Title, commonID, GatheringYN, GatheringID);

            return ds;
        }
        #endregion

        #region GatheringMobileNotiInsert
        public DataSet GatheringMobileNotiInsert(string LoginUserID, int GatheringID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();

            DataSet ds = Dac.GatheringMobileNotiInsert(LoginUserID, GatheringID);

            return ds;
        }
        #endregion

        #region GatheringMobileNotiDelete
        public DataSet GatheringMobileNotiDelete(string LoginUserID, int GatheringID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();

            DataSet ds = Dac.GatheringMobileNotiDelete(LoginUserID, GatheringID);

            return ds;
        }
        #endregion
        

        /***************  WEEKLY ***************/

        #region GetMyWeeklyByID
        //<!--2015.03 수정 -->
        public WeeklyType GetMyWeeklyByID(string weeklyID)
        {
            WeeklyBiz biz = new WeeklyBiz();
            return biz.WeeklySelectWeeklyID(weeklyID);
        }
        #endregion

        #region GetMyWeeklyByFromUserID - 사용안함
        //2015-06-11 KSH
        public WeeklyType GetMyWeeklyByFromUserID(string weeklyID,string userID)
        {
            WeeklyBiz biz = new WeeklyBiz();
            return biz.WeeklySelectWeeklyFromUserID(weeklyID,userID);
        }
        #endregion

        #region GetMyWeeklyByUserID
		//2015-06-11 KSH
        public WeeklyType GetMyWeeklyByUserID(string userID, string deptCode, DateTime weekDateTime)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();

            WeeklyType weeklyType = new WeeklyType();
            using (DataSet ds = weeklyDac.SelectWeekly(userID, deptCode, weekDateTime))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        weeklyType = WeeklyBiz.GetWeeklyTypeMapData(dr);
                    }
                }
            }
            return weeklyType;
        } 
	    #endregion

        #region GetDeptWeeklyList
		 //2015.06.02 KSM 임원모바일
        public List<DeptWeeklyType> GetDeptWeeklyList(string userID, string deptCode, DateTime weekDate, int pageNum, int pageSize, string TempFG)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            List<DeptWeeklyType> listWeeklyType = new List<DeptWeeklyType>();

            using (DataSet ds = weeklyDac.SelectWeeklyDeptCode(userID, deptCode, weekDate, pageNum, pageSize, TempFG))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DeptWeeklyType deptWeeklyType = new DeptWeeklyType();
                        deptWeeklyType.DeptMemberInfo = new ImpersonUserinfo();
                        deptWeeklyType.DeptMemberInfo.UserID = dr["UserID"] as String;
                        deptWeeklyType.DeptMemberInfo.Name = dr["UserName"] as String;
                        deptWeeklyType.DeptMemberInfo.DeptID = dr["DeptCode"] as String;
                        deptWeeklyType.DeptMemberInfo.DeptName = dr["DeptName"] as String;
                        deptWeeklyType.DeptMemberInfo.EmailAddress = "";// dr["EmailAddress"] as String;
                        deptWeeklyType.DeptMemberInfo.WorkArea = "";// dr["WorkArea"] as String;
                        deptWeeklyType.DeptMemberInfo.TEL = "";// dr["TEL"] as String;
                        deptWeeklyType.DeptMemberInfo.Phone = "";// dr["Phone"] as String;
                        deptWeeklyType.DeptMemberInfo.JobCode = dr["JobCode"] as String;
                        deptWeeklyType.DeptMemberInfo.JobCodeName = (dr["JobCodeName"] == DBNull.Value) ? "" : dr.Field<string>("JobCodeName").Trim(); 
                        deptWeeklyType.DeptMemberInfo.floorNumber = "";// dr["floorNumber"] as String;
                        //deptWeeklyType.DeptMemberInfo.PositionCode = dr["PositionCode"] as String;
                        deptWeeklyType.DeptMemberInfo.PositionName = dr["PositionName"] as String;
                        deptWeeklyType.DeptMemberInfo.ViewLevel = dr["ViewLevel"] as String;

                        deptWeeklyType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        deptWeeklyType.WeeklyDeptCode = dr["DeptCode"] as String;
                        deptWeeklyType.WeeklyDeptName = dr["DeptName"] as String;

                        if (dr.Table.Columns.Contains("TempYN"))
                        {
                            dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
                            deptWeeklyType.TempFG = dr["TempYN"] as String;
                        }

                        deptWeeklyType.PermissionYN = (dr["PermissionYN"] == DBNull.Value) ? "" : dr.Field<string>("PermissionYN").Trim();
                        deptWeeklyType.PermissionsUserID = (dr["PermissionsUserID"] == DBNull.Value) ? "" : dr.Field<string>("PermissionsUserID").Trim(); 

                        deptWeeklyType.WeeklyCreateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
                        listWeeklyType.Add(deptWeeklyType);

                        
                    }
                }
            }
            return listWeeklyType;
        }
        //2015.06.02 // 
	    #endregion

        #region GetWeeklyComment
        public List<WeeklyCommentType> GetWeeklyComment(long weeklyID)
        {
            WeeklyCommentDac dac = new WeeklyCommentDac();
            List<WeeklyCommentType> wctList = new List<WeeklyCommentType>();

            using (DataSet ds = dac.WeeklyComment_List_New(weeklyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyCommentType dwt = new WeeklyCommentType();
                        dwt.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        dwt.WeeklyCommentID = (dr["WeeklyCommentID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyCommentID");
                        dwt.SUP_ID = (dr["SUP_ID"] == DBNull.Value) ? 0 : dr.Field<long>("SUP_ID");
                        dwt.UserID = dr["UserID"] as String;
                        dwt.UserName = dr["UserName"] as String;
                        dwt.DutyName = dr["DutyName"] as String;
                        dwt.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("CreateDateTime");
                        dwt.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
                        dwt.Contents = SecurityHelper.Add_XSS_CSRF(dr["Contents"] as String);
                        wctList.Add(dwt);
                    }
                }
            }

            return wctList;
        }
        #endregion

        #region SaveMyWeekly
		public void SaveMyWeekly(WeeklyType weeklyType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            Dac.InsertWeeklyByMobile(weeklyType);
        } 
	    #endregion

        #region UpdateMyWeekly
		public void UpdateMyWeekly(WeeklyType weeklyType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            Dac.UpdateWeeklyByMobile(weeklyType);
        } 
	    #endregion

        #region DeleteMyWeekly
		public void DeleteMyWeekly(string weeklyID, string LoginUserID)
        {
            TikleMobileWebServiceDac biz = new TikleMobileWebServiceDac();
            biz.WeeklyDeleteByMobile(weeklyID, LoginUserID);
        } 
	    #endregion

        #region GetWeeklyTempUserList
		/* 2015.04 MYWEEKLY */
        public List<DeptWeeklyType> GetWeeklyTempUserList(string userID)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            List<DeptWeeklyType> listWeeklyType = new List<DeptWeeklyType>();

            using (DataSet ds = weeklyDac.WeeklyMobileTemp_Select(userID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DeptWeeklyType deptWeeklyType = new DeptWeeklyType();
                        deptWeeklyType.DeptMemberInfo = new ImpersonUserinfo();
                        deptWeeklyType.DeptMemberInfo.UserID = dr["TempYN"] as String;
                        
                        listWeeklyType.Add(deptWeeklyType);
                    }
                }
            }
            return listWeeklyType;
        }
        /* 2015.04 MYWEEKLY */ 
	    #endregion

        #region InsertMyWeeklyComment
        public WeeklyCommentType InsertMyWeeklyComment(WeeklyCommentType commentType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            DataSet ds = Dac.InsertWeeklyComment(commentType);
            commentType.WeeklyID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]);
            return commentType;
        } 
        #endregion

        #region UpdateMyWeeklyComment
        public WeeklyCommentType UpdateMyWeeklyComment(WeeklyCommentType commentType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            Dac.UpdateWeeklyComment(commentType);
            return commentType;
        } 
        #endregion

        #region DeleteMyWeeklyComment
        public void DeleteMyWeeklyComment(Int64 weeklycommentID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            Dac.DeleteWeeklyComment(weeklycommentID);
        } 
        #endregion

        #region SelectExtendUserInfo
        public ResultExtendUserInfo SelectExtendUserInfo(string userID)
        {
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            DataSet ds = new DataSet();
            ds = dac.UserSelect(userID);

            ResultExtendUserInfo reu = new ResultExtendUserInfo();

            ImpersonUserinfo Info = null;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Info = new ImpersonUserinfo();
                DataRow dr = ds.Tables[0].Rows[0];
                Info.UserID = dr["EmpID"].ToString();
                Info.Name = dr["Name"].ToString();
                Info.DeptID = dr["DeptID"].ToString();
                Info.DeptName = dr["DeptName"].ToString();
                Info.EmailAddress = dr["EmailAddress"].ToString();
                Info.WorkArea = dr["WorkArea"].ToString();
                Info.Part = dr["Part"].ToString();

                Info.TEL = dr["TEL"].ToString();
                Info.Phone = dr["Phone"].ToString();

                Info.PhotoUrl = dac.GetPicture(userID);
                Info.JobCode = dr["JobCode"].ToString();

                Info.ViewLevel = dr["ViewLevel"].ToString();
                Info.PositionName = dr["PositionName"].ToString();
                Info.ManagerEmployeeID = dr["ManagerEmployeeID"].ToString();

                reu.UserInfo = Info;
            }

            // 겸직 체크
            ds = dac.SelectAdditionalJob(userID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AdditionalDeptType adt = new AdditionalDeptType();
                    adt.DeptCode = row["AdditionalJobCode"].ToString();
                    adt.DeptName = row["AdditionalJobName"].ToString();
                    adt.AdditionalViewLevel = row["ViewLevel"].ToString();
                    reu.AdditionalJobDeptNumberList.Add(adt);
                }
            }
            return reu;
        } 
        #endregion

        #region GetWeeklyMobileFile
        /// <summary>
        ///  Mr.No 2015-07-07
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="RealFileInfo"></param>
        /// <returns></returns>
        public List<FILE> GetWeeklyMobileFile(Int64 ItemID, Int64 CommonID, int BoardID)
        {
            List<FILE> list = new List<FILE>();

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Select(ItemID, CommonID, BoardID);
                FILE attach;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        attach = new FILE();

                        attach.NAME = ReplaceFileName(dr["FileName"].ToString());

                        Guid ItemGuid = new Guid();
                        if (dr.Table.Columns.Contains("ItemGuid"))
                        {
                            ItemGuid = dr["ItemGuid"] == DBNull.Value ? Guid.Empty : dr.Field<Guid>("ItemGuid");
                        }

                        // Mr.No 2015-07-01
                        // Mobile Download Full Path Setting
                        string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                        string DownloadPath = System.Configuration.ConfigurationManager.AppSettings["DownloadControlServerHandlerUrl2"];
                        // Folder=Glossary 고정4
                        attach.URL = DownloadPath + "?" + "Folder=Glossary" + "&ItemGuid=" + ItemGuid + "&FileName=" + System.Web.HttpUtility.UrlEncode(attach.NAME);

                        list.Add(attach);
                    }
                }
            }
            return list;
        } 
        #endregion

        #region Outlook WebMethod
        public void SaveMyWeeklyByOutlook(string email, string htmlContents, string textContents)
        {
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            dac.WeeklyInsertByOutlook(email, htmlContents, textContents);
        }

        
        public void SaveMyWeeklyByNateon(string email, string htmlContents, string textContents)
        {
            TikleMobileWebServiceDac dac = new TikleMobileWebServiceDac();
            dac.WeeklyInsertByNateon(email, htmlContents, textContents);
        }

        public bool IsVisibleMyWeeklyOutlookAddIn(string employeeID)
        {
            bool isVisible = false;
            bool extendsionUser = false;
            ImpersonUserinfo userInfo = UserSelect(employeeID);

            if (!String.IsNullOrEmpty(userInfo.UserID)
                && !(userInfo.UserID.StartsWith("15") || Char.IsLetter(userInfo.UserID, 0))
                || extendsionUser)
                isVisible = true;
            return isVisible;
        }
        #endregion

        /////////////////////////////////
        //2016-06 추가 Method
        /////////////////////////////////

        #region GetMemberWeekly
        public ResultWeeklyType GetMemberWeekly(string UserID, string deptCode, string userFG, DateTime weekDateTime)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            ResultWeeklyType listWeeklyType = new ResultWeeklyType();

            using (DataSet ds = weeklyDac.GetMemberWeekly(UserID, deptCode, userFG, weekDateTime))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WeeklyType _type = new WeeklyType();
                        _type.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        _type.TextContents = dr["TextContents"] as String;
                        _type.Contents = SecurityHelper.Add_XSS_CSRF(dr["Contents"] as String);
                        _type.UserID = dr["UserID"] as String;
                        _type.UserName = dr["UserName"] as String;
                        _type.DeptCode = dr["DeptCode"] as String;
                        _type.DeptName = dr["DeptName"] as String;
                        _type.PositionName = dr["PositionName"] as String;
                        _type.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<int>("ViewLevel");
                        _type.TempYN = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN");

                        listWeeklyType.MyWeeklyList.Add(_type);
                    }
                }
            }

            return listWeeklyType;
        }
        #endregion

        #region GetDeptMonthlyList
        public List<DeptMonthlyType> GetDeptMonthlyList(string LoginUserID, string deptCode, string weekDate, int pagenum, int pagecount)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            List<DeptMonthlyType> listType = new List<DeptMonthlyType>();

            using (DataSet ds = weeklyDac.GetDeptMonthlyList(LoginUserID, deptCode, weekDate, pagenum, pagecount))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DeptMonthlyType deptType = new DeptMonthlyType();
                        deptType.DeptMemberInfo = new ImpersonUserinfo();
                        deptType.DeptMemberInfo.UserID = dr["UserID"] as String;
                        deptType.DeptMemberInfo.Name = dr["UserName"] as String;
                        deptType.DeptMemberInfo.DeptID = dr["DeptCode"] as String;
                        deptType.DeptMemberInfo.DeptName = dr["DeptName"] as String;
                        deptType.DeptMemberInfo.EmailAddress = "";// dr["EmailAddress"] as String;
                        deptType.DeptMemberInfo.WorkArea = "";// dr["WorkArea"] as String;
                        deptType.DeptMemberInfo.TEL = "";// dr["TEL"] as String;
                        deptType.DeptMemberInfo.Phone = "";// dr["Phone"] as String;
                        deptType.DeptMemberInfo.JobCode = "";// dr["JobCode"] as String;
                        deptType.DeptMemberInfo.JobCodeName = "";// dr["JobCodeName"] as String;
                        deptType.DeptMemberInfo.floorNumber = "";// dr["floorNumber"] as String;
                        deptType.DeptMemberInfo.PositionName = dr["PositionName"] as String;

                        deptType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        deptType.WeeklyDeptCode = dr["DeptCode"] as String;
                        deptType.WeeklyDeptName = dr["DeptName"] as String;
                        deptType.DisplayLevel = dr["DisplayLevel"] as String;

                        if (dr.Table.Columns.Contains("TempYN"))
                            dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();

                        deptType.WeeklyCreateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
                        listType.Add(deptType);


                    }
                }
            }

            return listType;
        }
        #endregion

        #region SaveMonthly
        public DataSet SaveMonthly(MonthlyType monthlyType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            return Dac.InsertMonthlyByMobile(monthlyType);
        }
        #endregion

        #region UpdateMonthly
        public DataSet UpdateMonthly(MonthlyType monthlyType)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            return Dac.UpdateMonthlyByMobile(monthlyType);
        }
        #endregion

        #region DeleteMonthly
        public DataSet DeleteMonthly(string monthlyID, string LoginUserID)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            return Dac.DeleteMonthlyByMobile(monthlyID, LoginUserID);
        }
        #endregion

        #region GetMonthlyByID
        public MonthlyType GetMonthlyByID(string monthlyID)
        {
            MonthlyBiz biz = new MonthlyBiz();
            return biz.MonthlySelectWeeklyID(monthlyID);
        }
        #endregion

        #region GetMonthlyByUserID
        public MonthlyType GetMonthlyByUserID(string userID, string deptCode, DateTime weekDate)
        {
            MonthlyBiz biz = new MonthlyBiz();
            MonthlyType monthly = new MonthlyType();
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();

            WeeklyType weeklyType = new WeeklyType();
            using (DataSet ds = Dac.SelectMonthly(userID, deptCode, weekDate))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        monthly = MonthlyBiz.GetMonthlyTypeMapData(dr);
                    }
                }
            }
            return monthly;
        }
        #endregion

        #region GetMonthlyComment
        public List<MonthlyCommentType> GetMonthlyComment(long monthlyID)
        {
            MonthlyCommentDac dac = new MonthlyCommentDac();
            List<MonthlyCommentType> wctList = new List<MonthlyCommentType>();

            using (DataSet ds = dac.WeeklyComment_List_New(monthlyID))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MonthlyCommentType dwt = new MonthlyCommentType();
                        dwt.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        dwt.WeeklyCommentID = (dr["WeeklyCommentID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyCommentID");
                        dwt.SUP_ID = (dr["SUP_ID"] == DBNull.Value) ? 0 : dr.Field<long>("SUP_ID");
                        dwt.UserID = dr["UserID"] as String;
                        dwt.UserName = dr["UserName"] as String;
                        dwt.DutyName = dr["DutyName"] as String;
                        dwt.CreateDateTime = (dr["CreateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("CreateDateTime");
                        dwt.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
                        dwt.Contents = SecurityHelper.Add_XSS_CSRF(dr["Contents"] as String);
                        wctList.Add(dwt);
                    }
                }
            }

            return wctList;
        }
        #endregion

        #region TikleAccessLog
        public int TikleAccessLog(string LoginID, string IP, string HostName)
        {
            TikleMobileWebServiceDac Dac = new TikleMobileWebServiceDac();
            return Dac.TikleAccessLog(LoginID, IP, HostName);
        }
        #endregion

        #region GetWeeklyExceptionList
        public List<ResultWeeklyExceptionType> GetWeeklyExceptionList(string LoginUserID,  DateTime weekDateTime)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            List<ResultWeeklyExceptionType> _list = new List<ResultWeeklyExceptionType>();

            using (DataSet ds = weeklyDac.GetWeeklyExceptionList(LoginUserID, weekDateTime))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ResultWeeklyExceptionType _type = new ResultWeeklyExceptionType();
                        _type.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        _type.TargetUserID = dr["TargetID"] as String;
                        _type.TargetDeptID = dr["TargetDeptCode"] as String;
                        _type.TargetDeptNM = dr["SOSOK"] as String;

                        _list.Add(_type);
                    }
                }
            }

            return _list;
        }
        #endregion

        #region GetWeeklyExecutiveList
        //임원일경우 목록 - 팀까지 나옴
        public List<ExecutiveWeeklyType> GetWeeklyExecutiveList(string userID, string deptCode, DateTime weekDate)
        {
            TikleMobileWebServiceDac weeklyDac = new TikleMobileWebServiceDac();
            List<ExecutiveWeeklyType> listWeeklyType = new List<ExecutiveWeeklyType>();

            using (DataSet ds = weeklyDac.SelectWeeklyExecutive(userID, deptCode, weekDate))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ExecutiveWeeklyType deptWeeklyType = new ExecutiveWeeklyType();
                        deptWeeklyType.WeeklyID = (dr["WeeklyID"] == DBNull.Value) ? 0 : dr.Field<long>("WeeklyID");
                        deptWeeklyType.TextContents = (dr["TextContents"] == DBNull.Value) ? "" : dr.Field<string>("TextContents").Trim();
                        if (dr.Table.Columns.Contains("TempYN"))
                            dr["TempYN"] = (dr["TempYN"] == DBNull.Value) ? "N" : dr.Field<string>("TempYN").Trim();
                        deptWeeklyType.UpdateDateTime = (dr["UpdateDateTime"] == DBNull.Value) ? new DateTime() : dr.Field<DateTime>("UpdateDateTime");
                        deptWeeklyType.UserID = dr["UserID"] as String;
                        deptWeeklyType.UserName = dr["UserName"] as String;
                        deptWeeklyType.DeptCode = dr["DeptCode"] as String;
                        deptWeeklyType.DeptName = dr["DeptName"] as String;
                        deptWeeklyType.PositionName = dr["PositionName"] as String;
                        deptWeeklyType.DisplayOrder = dr["DisplayOrder"] as String;
                        deptWeeklyType.DisplayLevel = dr["DisplayLevel"] as String;
                        deptWeeklyType.HasChild = dr["HasChild"] as String;
                        deptWeeklyType.UpperDepartmentNumber = dr["UpperDepartmentNumber"] as String;
                        //deptWeeklyType.Path = dr["Path"] as String;
                        deptWeeklyType.ViewLevel = (dr["ViewLevel"] == DBNull.Value) ? 0 : dr.Field<Int32>("ViewLevel");
                        deptWeeklyType.PermissionYN = (dr["PermissionYN"] == DBNull.Value) ? "N" : dr.Field<string>("PermissionYN").Trim();
                        deptWeeklyType.PermissionsUserID = (dr["PermissionsUserID"] == DBNull.Value) ? "" : dr.Field<string>("PermissionsUserID").Trim();
                        
                        listWeeklyType.Add(deptWeeklyType);
                    }
                }
            }
            return listWeeklyType;
        }
        //2015.06.02 // 
        #endregion

        /***************  COMMON ***************/ 

        #region ReplaceFileName
        public static string ReplaceFileName(string strFileName)
        {
            /*
             * 쉐어포인트에서 허용하지 않는 특수문자를 언더바로 치환한다.
             * \ / : * ? " < > | # { } % ~ & '
             */
            //string removeSimbol = strFileName.Replace("\\", "_");

            //removeSimbol = removeSimbol.Replace("/", "_");
            //removeSimbol = removeSimbol.Replace(":", "_");
            //removeSimbol = removeSimbol.Replace("\"", "_");
            string removeSimbol = strFileName.Replace("*", "_");
            removeSimbol = removeSimbol.Replace("?", "_");
            removeSimbol = removeSimbol.Replace("<", "_");
            removeSimbol = removeSimbol.Replace(">", "_");
            removeSimbol = removeSimbol.Replace("|", "_");
            removeSimbol = removeSimbol.Replace("#", "_");
            removeSimbol = removeSimbol.Replace("{", "_");
            removeSimbol = removeSimbol.Replace("}", "_");
            removeSimbol = removeSimbol.Replace("%", "_");
            removeSimbol = removeSimbol.Replace("~", "_");
            removeSimbol = removeSimbol.Replace("&", "_");
            removeSimbol = removeSimbol.Replace("'", "_");

            return removeSimbol;
        }
        #endregion

    }
}