using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryScheduleType
    {
        public string SCID { get; set; }
        public string YYMMDD { get; set; }
        public string YEAR { get; set; }
        public string MON { get; set; }
        public string DAY { get; set; }
        public string WEEK { get; set; }
        public string HOUR { get; set; }
        public string MIN { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public string URL { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string CREATEDATE { get; set; }
        public string AUDIT_ID { get; set; }
        public string AUDIT_DTM { get; set; }
        public string AuthYN { get; set; }
        //종료시간추가(모임용)
        public string EndHOUR { get; set; }
        public string EndMIN { get; set; }
    }
}
