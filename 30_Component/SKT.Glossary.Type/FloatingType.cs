using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// FloatingType
    /// </summary>
    public class FloatingType //Type
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string TotalCnt { get; set; }
        public string ItemState { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string CreateDate { get; set; }
        public string DeleteYN { get; set; }
        public string RowNum { get; set; }

        public string Count { get; set; }
        public string CommonID { get; set; }

        public string DoubleClick { get; set; }
        public string Drag { get; set; }
        public string Removal { get; set; }
    }
}
