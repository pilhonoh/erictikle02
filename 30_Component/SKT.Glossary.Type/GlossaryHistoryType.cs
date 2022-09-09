using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class GlossaryHistoryType
    {
        public string ID { get; set; }
        public string CommonID { get; set; }
        public string GlossaryID { get; set; }
        public string RowNum { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentsModify { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Hits { get; set; }
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
        public string PrivateYN { get; set; }
        public string RootTitle { get; set; }
        public string RootSummary { get; set; }
        public string RootContentsModify { get; set; }
        public string LastCreateDate { get; set; }
        public string RootPrivateYN { get; set; }
        public string RootBoardUserID { get; set; }
        public string RootBoardUserName { get; set; }
        public string RootBoardDeptName { get; set; }
        public string Type { get; set; }
    }
}
