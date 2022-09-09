using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
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

        public string DTBlogFlag { get; set; } // DTBlogFlag
        public string TWhiteFlag { get; set; } // TWhiteFlag
    }

   

    public class TutorialInfo
    {
        public string UserID { get; set; }
        public string ResultYN { get; set; }
        public string ProfileYN { get; set; }
        public string QNAYN { get; set; }
        public string FirstWrite { get; set; }
    }
}