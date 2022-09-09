using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryDirectoryFileType
    {
        public string UserID { get; set; }

        public string FileID { get; set; }
        public string DirID { get; set; }
        public string FileNM { get; set; }
        public string FileSize { get; set; }
        public string FileExt { get; set; }
        public string RegID { get; set; }
        public string RegNM { get; set; }
        public DateTime RegDTM { get; set; }
        public string AuditID { get; set; }
        public DateTime AuditDTM { get; set; }

        public string AuditNM { get; set; }

        public string HasEditedVersion { get; set; } 

    }


}
