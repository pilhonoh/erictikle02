using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    public class CommonAuthType
    {
        public string UserID { get; set; }

        public string ItemID { get; set; }
		public string AuthID { get; set; }
        public string AuthType { get; set; }
		public string SeqNO { get; set; }
		public string AuthName { get; set; }

		public string AuthRWX { get; set; }

		public string RegID { get; set; }
		public DateTime RegDTM { get; set; }

		public string TeamName { get; set; }
		public string DeptName { get; set; }
        public string DeptNumber { get; set; }

		public string AuditID { get; set; }
		public DateTime AuditDTM { get; set; }
    }
}