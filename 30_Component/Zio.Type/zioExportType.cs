using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Zio.Type
{
    public class ZioExcelTableType
    {
        public string headTrTdTag { get; set;}
        public DataTable bodyRecords { get; set; }
        public string tableStyleAttributes { get; set; }
        public string excelFilename { get; set; }
        public string footTrTdTag { get; set; }
    }
}
