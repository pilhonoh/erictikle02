using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryAdminType
    {
        public string Num { get; set; }
        public string Date { get; set; }
        public string Dow { get; set; }
        public string Vtotal { get; set; }
        public string WtWiki { get; set; }
        public string WtNateon { get; set; }
        public string WtEmail { get; set; }
        public string WtETotal { get; set; }
        public string EdTotal { get; set; }
        public string WtEdTotal { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Exe107 { get; set; }
        public string Request { get; set; }
    }

   public class GlossaryHallOfFameType
   {
       public string RowNum { get; set; }
       public string ID { get; set; }
       public string GlossaryID { get; set; }
       public string GlossaryFrom { get; set; }
       
       public string Title { get; set; }
       public string FirstWriteDate { get; set; }
       public string LastWriteUserID { get; set; }
       public string LastWriteUserName { get; set; }
       public string LastWriteUserDeptName { get; set; }
       public string LastWriteDate { get; set; }

       public string CreateUserID { get; set; }
       public string CreateUserIP { get; set; }
       public string CreateUserMachineName { get; set; }
       public string CreateDate { get; set; }
       public string CreateAutoYN { get; set; }

       public string LastModifiedUserID { get; set; }
       public string LastModifiedUserIP { get; set; }
       public string LastModifiedUserMachineName { get; set; }
       public string LastModifiedDate { get; set; }

       public int DisplayYN { get; set; }
       public string OrderDate { get; set; }
       public string FixYN { get; set; }
       public string LikeCount { get; set; }

       public string CssTitleBox { get; set; }

       public string photoURL { get; set; } //2014-06-17 Mr.No
   }

   public class MainNoticeType
   {
	   public Int64 NotID { get; set; }
	   public string Gubun { get; set; }
	   public string Title { get; set; }
	   public string Content { get; set; }
	   public string ImgFile { get; set; }
	   public string URL { get; set; }
	   public int SeqNo { get; set; }
	   public string Itemid { get; set; }
	   public string UseYn { get; set; }
	   public string UserID { get; set; }

       //ItemID값 추가
       public string ItemID { get; set; }
   }
}
