using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryType
    {
        public string ID { get; set; }
        public string CommonID { get; set; }
        public string GlossaryID { get; set; }
        public string RowNum { get; set; }
        public string Title { get; set; }    
        public string Contents { get; set; }
        public string ContentsModify { get; set; }
        public string Summary { get; set; }
        public string Hits { get; set; }
        public string PrivateYN { get; set; }
        public string ItemState { get; set; }
        public string HistoryYN { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string UserEmail { get; set; }
        public string CreateDate { get; set; }
        public string ModifyYN { get; set; }
        public string MailYN { get; set; }
        public string NoteYN { get; set; }
        public string ModifyDate { get; set; }
        public string HistoryY { get; set; }
        public string FirstPrivateYN { get; set; }
        public string FirstUserID { get; set; }
        public string FirstUserName { get; set; }
        public string FirstDeptName { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string LikeCount { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string HistoryCount { get; set; }
        public string ModifyCount { get; set; } //편집 count 를 적는다.
        public string TagsInHtml { get; set; } //테그리스트를 표현한다.
        public string fromQnaID { get; set; } //질문글 티끌화 
        public string HallOfFameYN { get; set; }
        // 2014-04-29 Mr.No 추가
        public int CategoryID { get; set; }
       // 2014-05-12 Mr.No 추가
        public string Permissions { get; set; }
        // 2014-05-19 1Do : 댓글 수, 신규댓글여부
        public string CommentCount { get; set; }
        public bool NewCommentFlag { get; set; }
       // 2014-05-23 jmlee : 카테고리명
        public string CategoryName { get; set; }
        // 2014-06-16 Mr.No
        public int FirstGrade { get; set; }
        public int LastGrade { get; set; }
        public string FirstGradeUrl { get; set; }
        public string LastGradeUrl { get; set; }
        public int Grade { get; set; }
        public string Rank { get; set; }    // 2014-06-24

        // 모바일용으로 추가
        public string UserGrade { get; set; } // 편집자 랭킹 정보
        public string FirstUserGrade { get; set; } // 최초작성자 랭킹 정보
        public string PlatformYN { get; set; } // 플랫폼 2015-09-09

        public string MarketingYN { get; set; } // 마케팅 2015-10-12
        public string TechTrendYN { get; set; } // 트렌드 2015-10-21
        public string JustOfficerYN { get; set; } // 트렌드 2015-11-09
        public string Gubun { get; set; } 

        //프로퍼티 용으로 추가 
        public string GatheringID { get; set; }
        public string PublicYN { get; set; }

        //DB블로그
        public string DTBlogFlag { get; set; }
        //T생활백서
        public string TWhiteFlag { get; set; }

    }

   public class GlossaryTagType
   {
       public string ID { get; set; }
       public string CommonID { get; set; }
       public string Title { get; set; }
       public string TagTitle { get; set; }
       public string UserID { get; set; }
       public string CreateDate { get; set; }

   }

    public class GlossaryTagToTalType
    {
        public string text { get; set; }
        public int size { get; set; }
    }

   public class GlossaryEventType
   {
       public string ID { get; set; } //아이디
       public string UserID { get; set; } //사번 
       public string Name { get; set; } //이름
       public string WriteCount { get; set; } //작성수
       public string AnswerCount { get; set; }  //답변수
       public string AttendanceCount { get; set; }  //출석카운트

   }    
   public class GlossaryPageRequestType
   {
       public string UserID { get; set; }
       public string Name { get; set; } 
       public string SessionID { get; set; } 
       public string UrlBefore { get; set; }
       public string UrlCurrent { get; set; }
       public string PathCurrent { get; set; }
       public int MobileFlag { get; set; }
       public string Type { get; set; }
   }

   public class GlossaryLoginType
   {
       public string UserID { get; set; }
       public string Name { get; set; }
       public string SessionID { get; set; }
       public string UrlBefore { get; set; }
       public string UrlCurrent { get; set; }
       public string PathCurrent { get; set; }
       public int MobileFlag { get; set; }
       public string LoginType { get; set; }
   }
}