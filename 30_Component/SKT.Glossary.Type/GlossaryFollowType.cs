using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryFollowType
    {
        public string ID { get; set; }
        public string RowNum { get; set; }
        public string UserID { get; set; }
        public string ReaderUserID { get; set; }
        public string ReaderUserName { get; set; }
        public string ReaderUserPhoto { get; set; }
        public string ReaderDeptName { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string Type { get; set; }
    }
}