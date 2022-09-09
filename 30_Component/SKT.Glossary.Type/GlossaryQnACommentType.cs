using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryQnACommentType
    {
        public string ID { get; set; }
        public string CommonID { get; set; }
        public string PhotoUrl { get; set; }
        public string Contents { get; set; }
        public string LikeCount { get; set; }
        public string LikeY { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string UserEmail { get; set; }
        public string CreateDate { get; set; }
        public string PublicYN { get; set; }
        public string Type { get; set; }
        public string BestReplYN { get; set; }
        public int Grade { get; set; }  // 2014-06-16 Mr.No
        public string Rank { get; set; }
        public string BestCheck { get; set; }   // 2014-07-09 Mr.No
    }
}
