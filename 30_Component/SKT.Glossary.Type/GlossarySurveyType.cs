using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
	    

    public class GlossarySurveyType
    {
        public string UserID { get; set; }

        //설문 리스트  
        public string SvID { get; set; }
        public string SvNM { get; set; }
        public string SvSummary { get; set; }
        public string StaDT { get; set; }
        public string EndDT { get; set; }
        public string Status { get; set; }
        public string TopImg { get; set; }
        public string RegID { get; set; }
        public string RegNM { get; set; }
        public DateTime RegDTM { get; set; }

        public string AuthID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; } 


        public string UseYN { get; set; }
        public string AuditID { get; set; }
        public DateTime AuditDTM { get; set; }

        public string CommentCnt { get; set; }
        public string CommentUseCnt { get; set; }
        public string DISPLAYNAME { get; set; }
        public string AUTHUSERCNT { get; set; }
        public string VoteType { get; set; }

        public string QstId { get; set; }





        //
        public string Hits { get; set; }
        public string CommentHits { get; set; }
        public string ItemState { get; set; }
        public string BestReplyYN { get; set; }
        public string HallOfFameYN { get; set; } 


    }


}
