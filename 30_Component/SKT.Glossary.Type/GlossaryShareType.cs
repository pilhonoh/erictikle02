using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryShareType
    {
        public string ID { get; set; }
        public string RowNum { get; set; }
        public string Summary { get; set; }
        public string DeptName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string GlossaryID { get; set; }
        public string Title { get; set; }
        public string BoardUserID { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public string ReadYN { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string Type { get; set; }
        // 조회 수, 댓글 수, 추천 수
        public string Hits { get; set; }
        public string CommentCount { get; set; }
        public bool NewCommentFlag { get; set; }
        public string LikeCount { get; set; }
        // 권한
        public string Permissions { get; set; }
        public int UserGrade { get; set; }  // 2014-06-16 Mr.No
        public string Rank { get; set; }

        public string PrivateYN { get; set; }
    }

    public class GlossaryShareCountsType
    {
        public int ShareTotalCount { get; set; }
        public int ShareGetCount { get; set; }
        public int ShareSendCount { get; set; }
    }
}
