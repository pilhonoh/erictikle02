using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryTempType
    {
        public string ID { get; set; }
        public string RowNum { get; set; }
        public string UserID { get; set; }
        public string Contents { get; set; }
        public string ContentsModify { get; set; }
        public string Summary  { get; set; }
        public string Title { get; set; }
        public string CommonID { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string DocumentKind { get; set; }
        public string PrivateYN { get; set; }
        public string Description { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string Type { get; set; }
        public int CategoryID { get; set; }     // 2014-05-08 Mr.No
        public string Permissions { get; set; } // 2014-06-13 Mr.No
    }
}
