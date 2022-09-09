using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using SKT.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

namespace SKT.Glossary.Biz
{
    public class GlossaryTagBiz
    {
        public void TotalTagList(string TagTitle, int PageNum, int PageSize, int TotalCount)
        {
            GlossaryTagDac.Instance.TotalTagList(TagTitle, PageNum, PageSize, out TotalCount);
        }
        public void CloudTagList(int count)
        {
            GlossaryTagDac.Instance.GlossaryTagSelect(count);
        }



        

    }
}
