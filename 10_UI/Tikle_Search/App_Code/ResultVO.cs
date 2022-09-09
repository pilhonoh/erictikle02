using System;

namespace com.konantech.search.data.ResultVO
{

    /// <summary>
    /// Summary description for ResultVO
    /// </summary>
    public class ResultVO
    {
        public ResultVO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int Cols { get; set; }

        public object[,] Fdata { get; set; }

        public object[] RowIds { get; set; }
 
        public int Rows { get; set; }

        public object[] Scores { get; set; }

        public int Total { get; set; }
 
    }
}
