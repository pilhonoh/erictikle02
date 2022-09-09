using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Common
{
    public class CommentHelper
    {
    }

    public class StdCommentBiz : IDisposable
    {
        public void CommentInsert(StdCommentType Comment)
        {
            using (StdCommentDac dac = new StdCommentDac())
            {
                dac.CommentInsert(Comment);
            }
        }
        public void CommentDelete(StdCommentType Comment)
        {
            using (StdCommentDac dac = new StdCommentDac())
            {
                dac.CommentDelete(Comment);
            }
        }
        public void CommentUpdate(StdCommentType Comment)
        {
            using (StdCommentDac dac = new StdCommentDac())
            {
                dac.CommentUpdate(Comment);
            }
        }
        public void Dispose()
        {
        }
    }

    public class StdCommentDac : IDisposable
    {
        public void CommentInsert(StdCommentType Comment)
        {
            
        }
        public void CommentDelete(StdCommentType Comment)
        {

        }
        public void CommentUpdate(StdCommentType Comment)
        {

        }
        public void Dispose()
        {
        }
    }

    public class StdCommentType
    {
        public string CommentID { get; set; }
        public string BoardID { get; set; }
        public string ItemID { get; set; }
        public int CommentDepth { get; set; }
        public string ParentCommentID { get; set; }
        public string Comment { get; set; }
        public string CommentCnt { get; set; }
        public string LikeCnt { get; set; }
        public string UnLikeCnt { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorDeptID { get; set; }
        public string AuthorDeptName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string EditorID { get; set; }
        public string EditorName { get; set; }
        public string EditorDeptID { get; set; }
        public string EditorDeptName { get; set; }
        public string DeleteYN { get; set; }
        public string CreatedDT { get; set; }
        public string ModifiedDT { get; set; }
        public string PresentBoardName { get; set; }
        public string PresentBoardItemID { get; set; }
        public string CONTENT { get; set; }
        public string ImagePath { get; set; }
        public string radio { get; set; }

    }
}
