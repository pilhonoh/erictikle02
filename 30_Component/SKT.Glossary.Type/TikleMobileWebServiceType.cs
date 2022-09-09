using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SKT.Common;   // Mr.No 2015-07-01 

namespace SKT.Glossary.Type
{
    public class ResultCommon
    {
        public string IsSuccess;
        public string ErrorCode;
        public string ErrorMessage;
    }

    public class ResultTikleList : ResultCommon
    {
        public ResultTikleList()
        {
            TikleList = new List<TikleType>();
        }
        public List<TikleType> TikleList;
        public int ListTotalCount;
    }

    public class ResultTikleView : ResultCommon
    {
        public ResultTikleView()
        {
            TikleViewData = new TikleType();
        }
        public TikleType TikleViewData;
        //public List<GlossaryCommentTypeM> GlossaryComment;
        //public ResultAttachmentList GlossaryAttach;
        //public GlossaryTagType glossaryTag;
    }

    public class ResultTikleAllView : ResultCommon
    {
        public ResultTikleAllView()
        {
            TikleViewData = new TikleType();
        }
        public TikleType TikleViewData;
        public List<GlossaryCommentTypeM> GlossaryComment;
        public ResultAttachmentList GlossaryAttach;
        public string glossaryTags;
    }

    public class ResultQNAList : ResultCommon
    {
        public ResultQNAList()
        {
            QNAList = new List<GlossaryQnAType>();
        }
        public List<GlossaryQnAType> QNAList;
        public int ListTotalCount;
    }

    public class ResultQNAView : ResultCommon
    {
        public ResultQNAView()
        {
            QNAViewData = new GlossaryQnAType();
            QNASelectedBestCommentData = new GlossaryQnACommentType();
            CommentList = new List<GlossaryQnACommentType>();
            BestCommentList = new List<GlossaryQnACommentType>();
        }
        public GlossaryQnAType QNAViewData;
        public GlossaryQnACommentType QNASelectedBestCommentData;
        public List<GlossaryQnACommentType> BestCommentList;
        public List<GlossaryQnACommentType> CommentList;
    }

    public class ResultScrapList : ResultCommon
    {
        public ResultScrapList()
        {
            ScrapList = new List<GlossaryScrapType>();
        }
        public List<GlossaryScrapType> ScrapList;
        public int ListTotalCount;
    }

    public class TikleType
    {
        public TikleType()
        {
            TagList = new List<TikleTagType>();
        }
        //public string ID { get; set; }  //글 아이디 
        public string CommonID { get; set; } //글 아이디 commonid 로 모두 통일한다.
        public string Title { get; set; } //제목
        public string Contents { get; set; } //html 내용
        public string Summary { get; set; } //순수 text
        public string UserID { get; set; }  //사용자 아이디
        public string UserName { get; set; } //사용자 이름 
        public string DeptName { get; set; } //사용자 부서이름 

        public string CreateDate { get; set; } //작성일 = 최근편집일

        public string LikeCount { get; set; } //추천수.
        public string Hits { get; set; } //추천수.

        public string FirstUserID { get; set; } //최초 작성자
        public string FirstUserName { get; set; } //최조 작성자 이름 
        public string FirstDeptName { get; set; } //최초 작성자 부서이름
        public string FirstCreateDate { get; set; } //최초 작성일
        public string ModifyCount { get; set; } //편집 횟수
        public string ScrapCount { get; set; } // 스크랩 횟수

        public string ScrapYN { get; set; } // 현재사용자가 봤을떄  스크랩 여부
        public string LikeYN { get; set; } // 현재사용자가 봤을떄  추천 여부
        public string NoteYN { get; set; } // 현재사용자가 봤을떄  변경알림 설정 여부

        public string UserGrade { get; set; } // 편집자 랭킹 정보
        public string FirstUserGrade { get; set; } // 최초작성자 랭킹 정보  

        public string DTBlogFlag { get; set; } // DTBlogFlag
        public string TWhiteFlag { get; set; } // TWhiteFlag

        public List<TikleTagType> TagList;


    }

    public class TikleTagType
    {
        public string ID { get; set; }  // 아이디 
        public string Title { get; set; } //제목
    }

    // 카테고리
    public class CategoryType
    {
        public string ID { get; set; }  // 아이디 
        public string Title { get; set; } //제목
    }

    public class ResultCategoryList : ResultCommon
    {
        public ResultCategoryList()
        {
            CategoryList = new List<CategoryType>();
        }
        public List<CategoryType> CategoryList;
        public int ListTotalCount;
    }

    // 첨부파일
    public class AttachmentType
    {
        public string FileName { get; set; } // 파일명 
        public string FilePath { get; set; } // 파일경로
        public string FileSize { get; set; } // 파일크기
        public string itemGuid { get; set; } // 파일크기        
    }

    public class ResultAttachmentList : ResultCommon
    {
        public ResultAttachmentList()
        {
            AttachmentList = new List<AttachmentType>();
        }
        public List<AttachmentType> AttachmentList;
        public int ListTotalCount;
    }

    // 랭킹
    public class RankingType : ResultCommon
    {
        public string Grade { get; set; } // 등급
        public string ID { get; set; } // 순위
        public string TotalScore { get; set; } // 총 점수
    }
    /*
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

        public string UserGrade { get; set; } // 편집자 랭킹 정보
        public string FirstUserGrade { get; set; } // 최초작성자 랭킹 정보

    }

    public class GlossaryControlType
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string GlossaryID { get; set; }
        public string CommonID { get; set; }
        public string LikeY { get; set; }
        public string ScrapsYN { get; set; }
        public string ReadYN { get; set; }
        public string CreateDate { get; set; }
        public string MailYN { get; set; }
        public string NoteYN { get; set; }
        public string LikeCount { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string Historycount { get; set; }
        public string ScrapCount { get; set; }
        public string Title { get; set; }
        public string TagTitle { get; set; }
        public string Type { get; set; }
        public string TagCount { get; set; }


    }

    public class GlossaryScrapType
    {
        public string ID { get; set; }
        public string RowNum { get; set; }
        public string UserID { get; set; }
        public string YouUserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string UserEmail { get; set; }
        public string GlossaryID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string ScrapsYN { get; set; }
        public string NoteYN { get; set; }
        public string MailYN { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string Type { get; set; }

        public string UserGrade { get; set; } // 편집자 랭킹 정보

    }

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
        public string BastReplyYN { get; set; }

        public string UserGrade { get; set; } // 편집자 랭킹 정보
    }

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

        public string UserGrade { get; set; } // 답변자 랭킹 정보
    }


    public class ImpersonUserinfo
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string DeptID { get; set; }
        public string DeptName { get; set; }
        public string CompanyID { get; set; }
        public string EmailAddress { get; set; }
        public string WorkArea { get; set; }
        public string TEL { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string Part { get; set; }
        public string JobCode { get; set; }

    }
    */

    //<!--2015.03 수정 -->
    #region MyWeekly
    public class ResultWeeklyType : ResultCommon
    {
        public ResultWeeklyType()
        {
            MyWeeklyList = new List<WeeklyType>();
        }

        public List<WeeklyType> MyWeeklyList { get; set; }
    }

    public class ResultWeeklyView : ResultCommon
    {
        public WeeklyType MyWeekly;
        public List<WeeklyCommentType> CommentList;
        //public List<Attach> attach; // Mr.No 2015-07-01
        public List<FILE> ROOT; // Mr.No 2015-07-07
    }

    public class ResultDeptWeeklyType : ResultCommon
    {
        public ResultDeptWeeklyType()
        {
            DeptWeeklyList = new List<DeptWeeklyType>(20);
        }

        public List<DeptWeeklyType> DeptWeeklyList { get; set; }
    }

    public class ResultWeeklyData : ResultCommon
    {
        public object Data { get; set; }
    }

    public class AdditionalDeptType
    {
        public string AdditionalViewLevel { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
    }

    public class ResultExtendUserInfo : ResultCommon
    {
        public ResultExtendUserInfo()
        {
            AdditionalJobDeptNumberList = new List<AdditionalDeptType>(5);
        }

        public bool HasAuthWriteWeekly { get; set; }
        public ImpersonUserinfo UserInfo { get; set; }
        public List<AdditionalDeptType> AdditionalJobDeptNumberList { get; set; }
    }

    public class ResultOutlookWeeklyType : ResultCommon
    {
        public bool IsVisibleAddin { get; set; }
    }
    #endregion
    //<!-- 수정 끝 -->

    //2015-12-14 ksh Techtrend 권한 확인
    public class ResultTikleUserPermission : ResultCommon
    {
        public string UserPermission;
    }

    public class ResultGatheringList : ResultCommon
    {
        public ResultGatheringList()
        {
            GatheringList = new List<GlossaryGatheringListType>();
        }
        public List<GlossaryGatheringListType> GatheringList;
        public int ListTotalCount;
    }

    public sealed class WeeklyNoteEmailType
    {
        public WeeklyNoteEmailType()
        {
        }

        public string FromUserName { get; set; }
        public string FromDeptName { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromPositionName { get; set; }
        public string ToUserName { get; set; }
        public string ToDeptName { get; set; }
        public string ToEmailAddress { get; set; }
        public int Year { get; set; }
        public int YearWeek { get; set; }
        public string LinkUrl { get; set; }
        public string SubJect { get; set; }
        public string MemoContents { get; set; }
        public string MyConfirmMailFlag { get; set; }

    }


    public class ResultDeptMonthlyType : ResultCommon
    {
        public ResultDeptMonthlyType()
        {
            DeptMonthlyList = new List<DeptMonthlyType>(20);
        }

        public List<DeptMonthlyType> DeptMonthlyList { get; set; }
    }

    public class ResultMonthlyView : ResultCommon
    {
        public MonthlyType monthlyType;
        public List<FILE> ROOT; 
    }

    public class ResultWeeklyExceptionList : ResultCommon
    {
        public ResultWeeklyExceptionList()
        {
            WeeklyExceptionList = new List<ResultWeeklyExceptionType>();
        }
        public List<ResultWeeklyExceptionType> WeeklyExceptionList ;
    }

    public class ResultWeeklyExceptionType
    {
        public long WeeklyID { get; set; }
        public string TargetUserID { get; set; }
        public string TargetDeptID { get; set; }
        public string TargetDeptNM { get; set; } 
        
    }


    public class ExecutiveWeeklyType
    {
        public long WeeklyID { get; set; }
        public string TextContents { get; set; }
        public string TempYN { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string PositionName { get; set; }
        public string DisplayOrder { get; set; }
        public string DisplayLevel { get; set; }
        public string HasChild { get; set; }
        public string UpperDepartmentNumber { get; set; }
        //public string Path { get; set; }
        public int ViewLevel { get; set; }
        public string PermissionYN { get; set; }
        public string PermissionsUserID { get; set; }
    }

    public class ExecutiveMonthlyType
    {
        public long WeeklyID { get; set; }
        public string TextContents { get; set; }
        public string TempYN { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string PositionName { get; set; }
        public string DisplayOrder { get; set; }
        public string DisplayLevel { get; set; }
        public string HasChild { get; set; }
        public string UpperDepartmentNumber { get; set; }
        //public string Path { get; set; }
        public int ViewLevel { get; set; }
        public string PermissionYN { get; set; }
        public string PermissionsUserID { get; set; }
    }

    public class ResultExecutiveWeeklyType : ResultCommon
    {
        public List<ExecutiveWeeklyType> ExecutiveWeeklyList { get; set; }
    }

    public class ResultExecutiveMonthlyType : ResultCommon
    {
        public List<ExecutiveMonthlyType> ExecutiveMonthlyList { get; set; }
    }
}