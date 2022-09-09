using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryQnAType
    {
        public string ID { get; set; }
        public string CommonID { get; set; }
        public string RowNum { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentsModify { get; set; }
        public string Summary { get; set; }
        public string Hits { get; set; }
        public string CommentHits { get; set; }
        public string ItemState { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string UserEmail { get; set; }
        public string CreateDate { get; set; }
        public string SuccessCount { get; set; }
        public string UnSuccessCount { get; set; }
        public string Type { get; set; }
        public string BestReplyYN { get; set; }
        public string HallOfFameYN { get; set; }
        public int Grade { get; set; }
        public string PlatformYN { get; set; }
        public string MarketingYN { get; set; }

        // 모바일용으로 추가
        public string BastReplyYN { get; set; }

        //<!--2015.03 수정 -->
        public string UserGrade { get; set; }
        //<!-- 수정 끝 -->
    }
}
