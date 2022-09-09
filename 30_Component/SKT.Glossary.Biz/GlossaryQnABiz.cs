using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Common;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
   public class GlossaryQnABiz
    {
        //QnA 목록 리스트
       public ArrayList GlossaryQnAList(int PageNum, int PageSize, string QnaMode, out int TotalCount, out int Total, out int SuccessCount, out int UnSuccessCount, string SearchKeyword, string SearchType, string UserID, out int MyQnA, string SearchSort, string SearchSortGubun)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            Total = 0;
            SuccessCount = 0;
            UnSuccessCount = 0;
            MyQnA = 0;
            GlossaryQnADac dac = new GlossaryQnADac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryQnAList(PageNum, PageSize, SearchKeyword, SearchType, UserID, SearchSort, SearchSortGubun);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                SuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "SuccessCount", 0);
                UnSuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "UnSuccessCount", 0);
                Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);
                MyQnA = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "MyQnA", 0);
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
                    Board.BestReplyYN = dr["BestReplyYN"].ToString();
                    
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

                    //질문 글 티끌화, 검색 제외 관련 여기서 처리
                    if(QnaMode == "List")
                    {
                        list.Add(Board);
                        
                    }else if(QnaMode == "Search" && Board.CommonID == "")
                    {
                        list.Add(Board);
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
                }
            }
            return list;
        }

           //QnA 목록 리스트(platform)
           public ArrayList GlossaryQnAList_Platform(int PageNum, int PageSize, string QnaMode, out int TotalCount, out int Total, out int SuccessCount, out int UnSuccessCount, string SearchKeyword, string SearchType, string UserID, out int MyQnA, string SearchSort, string SearchSortGubun)
           {
               ArrayList list = new ArrayList();
               TotalCount = 0;
               Total = 0;
               SuccessCount = 0;
               UnSuccessCount = 0;
               MyQnA = 0;
               GlossaryQnADac dac = new GlossaryQnADac();

               DataSet ds = new DataSet();
               ds = dac.GlossaryQnAList_Platform(PageNum, PageSize, SearchKeyword, SearchType, UserID, SearchSort, SearchSortGubun);

               if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                   SuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "SuccessCount", 0);
                   UnSuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "UnSuccessCount", 0);
                   Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);
                   MyQnA = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "MyQnA", 0);
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
                       Board.BestReplyYN = dr["BestReplyYN"].ToString();

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

                       //질문 글 티끌화, 검색 제외 관련 여기서 처리
                       if (QnaMode == "List")
                       {
                           list.Add(Board);

                       }
                       else if (QnaMode == "Search" && Board.CommonID == "")
                       {
                           list.Add(Board);
                       }

                       // 2014-06-16 Mr.No
                       if (dr.Table.Columns.Contains("Grade"))
                       {
                           Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                       }

                   }
               }
               return list;
           }

           //QnA 목록 리스트(Marketing)
           public ArrayList GlossaryQnAList_Marketing(int PageNum, int PageSize, string QnaMode, out int TotalCount, out int Total, out int SuccessCount, out int UnSuccessCount, string SearchKeyword, string SearchType, string UserID, out int MyQnA, string SearchSort, string SearchSortGubun)
           {
               ArrayList list = new ArrayList();
               TotalCount = 0;
               Total = 0;
               SuccessCount = 0;
               UnSuccessCount = 0;
               MyQnA = 0;
               GlossaryQnADac dac = new GlossaryQnADac();

               DataSet ds = new DataSet();
               ds = dac.GlossaryQnAList_Marketing(PageNum, PageSize, SearchKeyword, SearchType, UserID, SearchSort, SearchSortGubun);

               if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                   SuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "SuccessCount", 0);
                   UnSuccessCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "UnSuccessCount", 0);
                   Total = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "Total", 0);
                   MyQnA = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "MyQnA", 0);
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
                       Board.BestReplyYN = dr["BestReplyYN"].ToString();

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

                       //질문 글 티끌화, 검색 제외 관련 여기서 처리
                       if (QnaMode == "List")
                       {
                           list.Add(Board);

                       }
                       else if (QnaMode == "Search" && Board.CommonID == "")
                       {
                           list.Add(Board);
                       }

                       // 2014-06-16 Mr.No
                       if (dr.Table.Columns.Contains("Grade"))
                       {
                           Board.Grade = (dr["Grade"] == DBNull.Value) ? 0 : dr.Field<int>("Grade");
                       }

                   }
               }
               return list;
           }
        //QnA Select
        public GlossaryQnAType GlossaryQnASelect(string ID,int Count=0)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnASelect(ID,Count);
            GlossaryQnAType Board = new GlossaryQnAType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.UserEmail = dr["UserEmail"].ToString();
                    Board.ItemState = dr["ItemState"].ToString();
                    Board.CommentHits = dr["CommentHits"].ToString();
                    Board.CreateDate = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd");
                    Board.CommonID = dr["CommonID"].ToString();
                    Board.BestReplyYN = dr["BestReplyYN"].ToString();
                    Board.HallOfFameYN = (dr["HallOfFameYN"] == DBNull.Value) ? "0" : dr["HallOfFameYN"].ToString();
                    Board.PlatformYN = (dr["PlatformYN"] == DBNull.Value) ? "N" : dr["PlatformYN"].ToString();
                }
            }
            return Board;
        }

        //QnA 추가
        public GlossaryQnAType GlossaryQnAInsert(GlossaryQnAType Board)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnAInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }

        public void GlossaryQnAUpdate(GlossaryQnAType Board, string userid, string usermachinename, string userip)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnAUpdate(Board, userid, usermachinename, userip);
        }

        //QnA 삭제
        public void GlossaryQnADelete(string ID, string UserID, string userIp, string userMachineName)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            DataSet ds = Dac.GlossaryQnADelete(ID, UserID, userIp, userMachineName);
        }

       /// <summary>
       /// 조회수증가
       /// </summary>
       /// <param name="ID"></param>
        public void QnAHit(int ID)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            Dac.QnAHit(ID);
        }



        public DataSet qnaBestComment(string ID)
        {
            GlossaryQnADac Dac = new GlossaryQnADac();
            return Dac.QnaBestComment(ID); ;
        }
        
       
       public string GlossaryQnAExistTitle(string ID, string title)
        {

            string rtn = string.Empty; ;
            GlossaryQnADac Dac = new GlossaryQnADac();

            DataSet ds;
            if (ID.Length == 0)
                ds = Dac.GlossaryQnAExistTitle(0, title);
            else
                ds = Dac.GlossaryQnAExistTitle(int.Parse(ID), title);


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rtn = dr["DBFLAG"].ToString();
                }
            }
            return rtn;

        }

       //2015-09-15 platform 업데이트
       public int PlatformQnAUpdate(string id)
       {
           GlossaryQnADac Dac = new GlossaryQnADac();
           return Dac.PlatformQnAUpdate(id);
       }

    }
}
