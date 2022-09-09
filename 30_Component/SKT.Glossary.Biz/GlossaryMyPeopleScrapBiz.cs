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
    public class GlossaryMyPeopleScrapBiz
    {
        public DataSet MyPeopleScrapList(int PageNum, int PageSize, out int TotalCount, string UserID)
        {
            GlossaryMyPeopleScrapDac dac = new GlossaryMyPeopleScrapDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryMyPeopleScrapList(PageNum, PageSize, UserID);

            TotalCount = 0;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
            }

            return ds;
        }
    }
}
