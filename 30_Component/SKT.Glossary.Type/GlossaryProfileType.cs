using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
   public class GlossaryProfileType
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }    
        public string Contents { get; set; }
        public string ContentsModify { get; set; }
        public string Summary { get; set; }
        public string CreateDate { get; set; }
        public string LastCreateDate { get; set; }
        public string FirstCreateDate { get; set; }
        public string PositionCode { get; set; }
        public string Type { get; set; }
        public string WorkStatus { get; set; }
        public string floorNumber { get; set; }
        public string PositionName { get; set; }
    }

   public class ImpersonUserinfo
   {
       public string UserID { get; set; }
       public string Name { get; set; }
       public string DeptID { get; set; }
       public string DeptName { get; set; }
       public string CompanyID { get; set; }
       public string EmailAddress { get; set; }
       public string WorkArea { get; set; }
       public string TEL { get; set; }
       public string Phone { get; set; }
       public string PhotoUrl { get; set; }
       public string Part { get; set; }
       public string Part2 { get; set; }
       public string Part3 { get; set; }
       public string JobCode { get; set; }
       public string JobCodeName { get; set; }
       public string floorNumber { get; set; }
       public string PositionName { get; set; }

       //<!--2015.03 수정 -->
       public string ViewLevel { get; set; }
       public string ManagerEmployeeID { get; set; }
       //<!-- 수정 끝 -->

       public string Level { get; set; }
   }

   public class GlossaryProfileCareerAfterType
   {
       public string ID { get; set; }
       public string UserID { get; set; }
       public string Date { get; set; }
       public string Status { get; set; }
       public string Depart { get; set; }
       public string Message { get; set; }

   }

   public class GlossaryProfileCareerBeforeType
   {
       public string ID { get; set; }
       public string UserID { get; set; }
       public string Company { get; set; }
       public string BeginDate { get; set; }
       public string EndDate { get; set; }
       public string SKGYN { get; set; }
       public string Position { get; set; }
       public string Depart { get; set; }
       public string Job { get; set; }
   }
}
