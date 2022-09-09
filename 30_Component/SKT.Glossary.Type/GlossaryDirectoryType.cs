using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryDirectoryType
    {

        public string UserID { get; set; }

        public string DirID { get; set; }
        public string SupDirID { get; set; }
        public string DirNM { get; set; }
        public string Path { get; set; }
   
        public string RegID { get; set; }
        public string RegNM { get; set; }
        public DateTime RegDTM { get; set; }
        public string AuditID { get; set; }
        public DateTime AuditDTM { get; set; }


        public string AuthID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }

    }

    public class DirectoryMgrType
    {
        public string CommonID { get; set; }
        public string ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string ManagerType { get; set; }
        public string AUTH_ID { get; set; }
        public string AUTH_NM { get; set; }
    }


}
