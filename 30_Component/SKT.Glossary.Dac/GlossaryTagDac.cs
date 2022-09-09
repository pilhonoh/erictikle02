using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using SKT.Glossary.Type;
using SKT.Common;
using System.Collections;

namespace SKT.Glossary.Dac
{
    public class GlossaryTagDac
    {
        private const string connectionStringName = "ConnGlossary";

        private static GlossaryTagDac _instance = null;
        public static GlossaryTagDac Instance
        {
            get
            {
                GlossaryTagDac obj = _instance;
                if (obj == null)
                {
                    obj = new GlossaryTagDac();
                    _instance = obj;
                }
                return obj;
            }
        }

        private GlossaryTagDac() { }

        // 태그이름이 들어간 글로서리 조회
        public ArrayList TotalTagList(string TagTitle, int PageNum, int PageSize, out int TotalCount)  
        {
            ArrayList list = new ArrayList();

            GlossaryTagDac Dac = new GlossaryTagDac();

            DataSet ds = Dac.TotalTag(TagTitle, PageNum, PageSize);

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

                    list.Add(Board);
                    Rowindex++;
                }
            }
            return list;
        }


        // 태그를 조회해보자
        public DataSet TotalTag(string TagTitle, int PageNum, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);

            DbCommand cmd = db.GetStoredProcCommand("up_GlossaryTag_Select");

            db.AddInParameter(cmd, "TagTitle", DbType.String, TagTitle);
            db.AddInParameter(cmd, "PageNum", DbType.Int32, PageNum);
            db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize);
            
            return db.ExecuteDataSet(cmd);
        }

        public List<GlossaryTagToTalType> GlossaryTagSelect(int count)
        {
            List<GlossaryTagToTalType> listGlossaryCategoryType = new List<GlossaryTagToTalType>();
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_GlossaryTagColud_Select");

            db.AddInParameter(dbCommand, "count", DbType.Int16, count);

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GlossaryTagToTalType glossaryCategoryType = GetGlossaryTagTotalTypeMapData(dr);
                        listGlossaryCategoryType.Add(glossaryCategoryType);
                    }
                }
            }
            return listGlossaryCategoryType;
        }

        private GlossaryTagToTalType GetGlossaryTagTotalTypeMapData(DataRow dr)
        {
            GlossaryTagToTalType glossaryTagTotalType = new GlossaryTagToTalType();

            
                //glossaryTagTotalType.size = (dr["ShowCount"] == DBNull.Value) ? 0 : dr.Field<int>("ShowCount");
                glossaryTagTotalType.text = (dr["TagTitle"] == DBNull.Value) ? null : dr.Field<string>("TagTitle");
                glossaryTagTotalType.size = (dr["Percentages"] == DBNull.Value) ? 0 : dr.Field<int>("Percentages");

                return glossaryTagTotalType;
        }
    }
}
