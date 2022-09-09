using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using System.Data;
using System.Collections;

namespace SKT.Glossary.Biz
{

    /// <summary>
    /// FloatingBiz
    /// </summary>
    public class FloatingBiz
    {
        //List
        public ArrayList FloatingList(string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            FloatingDac dac = new FloatingDac();
            DataSet ds = new DataSet();
            ds = dac.FloatingList(SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FloatingType Board = new FloatingType();
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Summary = dr["Summary"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        }

        //View
        public ArrayList FloatingSelect(string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            FloatingDac dac = new FloatingDac();
            DataSet ds = new DataSet();
            ds = dac.FloatingSelect(SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FloatingType Board = new FloatingType();
                    Board.ID = dr["ID"].ToString();
                    Board.Title = dr["Title"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.TotalCnt = dr["TotalCnt"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        }
    }
}
