using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryGroupType
    {

        public string MyGrpID { get; set; }
        public string MyGrpNM { get; set; }
        public string RegID { get; set; }
        public string RegNM { get; set; }
        public string RegDTM { get; set; } 

        //public string ReaderUserID { get; set; }
        
        public string AudidID { get; set; }
        public string AudidNM { get; set; }
 


    }


    public class GlossaryGroupListType
    {
        public string ListNO { get; set; }
        public string MyGrpID { get; set; }
        public string MyGrpNM { get; set; }
        public string RegID { get; set; }
        public string RegNM { get; set; }

        public string AuthID { get; set; }
        public string AuthType { get; set; }
        public string PrtSEQ { get; set; }
        public string AudidID { get; set; }
        public string AudidNM { get; set; }

    }


    public class GlossaryGroupAuthType
    {

        public string MyGrpID { get; set; }
        public string MyGrpNM { get; set; }
        public string ToUserID { get; set; }
        public string ToUserName { get; set; }
   

    }



}
