using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossarySurveyCommentType
    {

        public string ID { get; set; }
        public string CommentID { get; set; }
        public string QstID { get; set; }

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
        public string LastModifyDate { get; set; }


        public string PublicYN { get; set; }
        public string Type { get; set; }
        public string BestReplYN { get; set; }
        public int Grade { get; set; }   
        public string Rank { get; set; }
        public string BestCheck { get; set; }


        public string TotalCnt { get; set; }
        public string TotalVot { get; set; }
        public string DlyCnt { get; set; } 
    }
}
