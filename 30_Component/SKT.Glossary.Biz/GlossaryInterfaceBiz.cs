using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Dac;
using System.Collections;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryInterfaceBiz
    {
        public DataSet TnetInterface_GlossaryMainTopList()
        {

            GlossaryInterfaceDac Dac = new GlossaryInterfaceDac();
            DataSet dsResult = Dac.TnetTopTotalActivityList();
            
            return dsResult;
        }
        public DataSet TnetInterface_GlossaryMainTopList3()
        {

            GlossaryInterfaceDac Dac = new GlossaryInterfaceDac();
            DataSet dsResult = Dac.TnetTopTotalActivityList3();

            return dsResult;
        }

        /// <summary>
        /// Mr.No
        /// 2015-07-16 
        /// </summary>
        /// <returns></returns>
        public DataSet TnetInterface_GlossaryMainTopList4()
        {

            GlossaryInterfaceDac Dac = new GlossaryInterfaceDac();
            DataSet dsResult = Dac.TnetTopTotalActivityList4();

            return dsResult;
        }
    }
}
