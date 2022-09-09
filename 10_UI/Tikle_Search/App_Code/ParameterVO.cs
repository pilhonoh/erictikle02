using System;

namespace com.konantech.search.data.ParameterVO
{

    /// <summary>
    /// Summary description for ParameterVO
    /// </summary>
    public class ParameterVO : System.Web.UI.UserControl
    {
        public ParameterVO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string Supex_code { get; set; }
 
        public string DateDetailSearch { get; set; }

        public int CountTotal { get; set; }

        public String Site { get; set; }

        public String Uuid { get; set; }
 
        public String NickName { get; set; }

        public String Kwd { get; set; }

        public String[] PreKwds { get; set; }

        public String Category { get; set; }

        public String RecKwd { get; set; }
 
        public Boolean ReSrchFlag { get; set; }

        public String SrchFd { get; set; }

        public int PageSize { get; set; }

        public int PageNum { get; set; }

        public String Sort { get; set; }

        public String DetailSearch { get; set; }

        public String Date { get; set; }

        public String SDate { get; set; }

        public String EDate { get; set; }
      
        public String IncludeKwd { get; set; }

        public String SearchFlag { get; set; }

        #region wisenut
        public string WisenutSearchField { get; set; }
        
        public string WisenutCommonQuery { get; set; }
       
        public string WisenutCollectionQuery { get; set; }
       
        public string WisenutPrefixQuery { get; set; }
       
        public string WisenutFilterQuery { get; set; }
       
        public string UserID { get; set; }
        
        #endregion

    }
}
