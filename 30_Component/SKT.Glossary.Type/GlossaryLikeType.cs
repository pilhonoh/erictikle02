using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryLikeType
    {
        public string ID { get; set; }
        public string GlossaryID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string LatestUserID { get; set; }
        public string LatestUserName { get; set; }
        public string LikeY { get; set; }
        public string CreateDate { get; set; }
        public string TotalCount { get; set; }

    }
}