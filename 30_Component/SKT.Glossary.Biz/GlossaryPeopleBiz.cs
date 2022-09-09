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
    public class GlossaryPeopleBiz
    {
        public void GlossaryScheduleInsert(GlossaryScheduleType Schedule)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            DataSet ds = Dac.GlossaryScheduleInsert(Schedule);
        }

        public void GlossaryScheduleUpdate(GlossaryScheduleType Schedule)
        {
            GlossaryScheduleDac Dac = new GlossaryScheduleDac();
            DataSet ds = Dac.GlossaryScheduleUpdate(Schedule);
        }
    }
}

