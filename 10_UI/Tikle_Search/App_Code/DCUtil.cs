using System;
using System.Text;
using com.konantech.search.module.SearchModule;
using com.konantech.search.data.ResultVO;

/// <summary>
/// @author Lee Jun-Hyeok (KONAN Technology)
/// </summary>
public class DCUtil
{
	public DCUtil()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    /**검색어에 대한 escape 처리.
	* @param kwd 검색어 
	* @return escape된 검색어
	**/

    public static string escapeQuery(string kwd)
    {
        string str = "";

        char[] arr = kwd.ToCharArray();
        
        foreach (char c in arr)
        {
            switch (c)
            {
                case '\"': str += "\\\""; break;
                case '\'': str += "\\'"; break;
                case '\\': str += "\\\\"; break;
                case '?': str += "\\?"; break;
                case '*': str += "\\*"; break;
                default: str += c; break;
            }
        }
        return str;
    }

    /** 검색엔진 로그정보 로그포맷. 
	* <br>[사이트명@카테고리명+사용자ID$|첫검색|페이지번호|정렬방법^키워드]##이전검색어|현재검색어] 
	* @param site 사이트명
	* @param nmSchCat 카테고리명
	* @param userId 사용자ID
	* @param kwd 키워드
	* @param pageNum 페이지번호
	* @param reSchFlag 재검색여부(true/false)
	* @param orderNm 정렬방법
	* @param recKwd 추천검색어('이전검색어|현재검색어')
	* 
	* @return 검색 로그 string
	*/
    public static string getLogInfo(string site, string nmSchCat, string userId, string kwd,
                    int pageNum, Boolean reSchFlag, string orderNm, string recKwd)
    {
        StringBuilder logInfo = new StringBuilder("");

        logInfo.Append(site);
        logInfo.Append("@");

        logInfo.Append(nmSchCat);
        logInfo.Append("+");

        // 페이지 이동은 검색으로 간주하지 않음
        if (pageNum > 1)
        {
            logInfo.Append("$||");
            logInfo.Append(pageNum);
            logInfo.Append("|");

        }
        else
        {
            logInfo.Append(userId);
            logInfo.Append("$|");

            if (reSchFlag)
            {
                logInfo.Append("재검색|");
            }
            else
            {
                logInfo.Append("첫검색|");
            }
            logInfo.Append(pageNum);
            logInfo.Append("|");
        }

        logInfo.Append(orderNm);
        logInfo.Append("^");
        logInfo.Append(kwd);

        // 추천어로그
        if (recKwd != null && recKwd.Length > 0)
        {
            logInfo.Append("]##").Append(recKwd);
        }
        return logInfo.ToString();
    }

    public static StringBuilder makeQuery(String[] nmFd, String kwd, String schMethod,
                                     StringBuilder query, String logicOp)
    {

        StringBuilder tempQuery = new StringBuilder("");

        if (query != null && query.Length > 0)
        {
            tempQuery.Append(query);
        }

        if (kwd != null && kwd.Length > 0)
        {
            if (tempQuery.Length > 0)
            {
                if ("".Equals(logicOp, StringComparison.OrdinalIgnoreCase))
                {
                    tempQuery.Append(" and ");
                }
                else
                {
                    tempQuery.Append(" " + logicOp + " ");
                }
            }

            tempQuery.Append("(");
            for (int j = 0; j < nmFd.Length - 1; j++)
            {
                tempQuery.Append(nmFd[j]);
                tempQuery.Append("='");
                tempQuery.Append(escapeQuery(kwd));
                tempQuery.Append("' ");
                tempQuery.Append(schMethod);
                if (j < nmFd.Length - 2) tempQuery.Append(" OR ");
            }
            tempQuery.Append(")");
        }
        return tempQuery;
    }

    /** 키워드/코드형식쿼리 생성. 
     *
     * @param logicOp 연결 논리연산자
     * @param nmFd 검색대상 필드명 또는 인덱스명
     * @param kwd 검색어
     * @param schMethod 검색메소드
     * @param query 쿼리 String 
     *
     * @return 쿼리 StringBuffer
     */
    public static StringBuilder makeQuery(String nmFd, String kwd, String schMethod,
                                         StringBuilder query, String logicOp)
    {

        StringBuilder tempQuery = new StringBuilder("");

        if (query != null && query.Length > 0)
        {
            tempQuery.Append(query);
        }

        if (kwd != null && kwd.Length > 0)
        {
            if (tempQuery.Length > 0)
            {
                if ("".Equals(logicOp, StringComparison.OrdinalIgnoreCase))
                {
                    tempQuery.Append(" and ");
                }
                else
                {
                    tempQuery.Append(" " + logicOp + " ");
                }
            }
            tempQuery.Append(nmFd);
            tempQuery.Append("='");
            
            tempQuery.Append(escapeQuery(kwd));
            
            tempQuery.Append("' ");
            tempQuery.Append(schMethod);
        }
        
        return tempQuery;
    }

    public static StringBuilder makePersonQuery(String nmFd, String kwd, String schMethod,
                                     StringBuilder query, String logicOp)
    {

        StringBuilder tempQuery = new StringBuilder("");

        if (query != null && query.Length > 0)
        {
            tempQuery.Append(query);
        }

        if (kwd != null && kwd.Length > 0)
        {
            if (tempQuery.Length > 0)
            {
                if ("".Equals(logicOp, StringComparison.OrdinalIgnoreCase))
                {
                    tempQuery.Append(" and ");
                }
                else
                {
                    tempQuery.Append(" " + logicOp + " ");
                }
            }
            tempQuery.Append(nmFd);
            tempQuery.Append("='");

            tempQuery.Append("*" + escapeQuery(kwd) + "*");

            tempQuery.Append("' ");
            tempQuery.Append(schMethod);
        }

        return tempQuery;
    }
 
    /** 
	* 
	* @param nmFd 		- 
	* @param startVal	- 
	* @param endVal 	- 
	* @param query 		- 
	* @return String
	*/
    public static StringBuilder makeRangeQuery(String nmFd, String startVal, String endVal,
                                                StringBuilder query)
    {

        StringBuilder tempQuery = new StringBuilder("");

        if ("".Equals(startVal) && "".Equals(endVal))
        {
            return query;
        }

        if (query != null && query.Length > 0)
        {
            tempQuery.Append("(" + query + ")");
            tempQuery.Append(" AND ");
        }

        tempQuery.Append("(");

        if (!startVal.Equals(""))
        {
            tempQuery.Append(nmFd);
            tempQuery.Append(" >= '");
            tempQuery.Append(startVal);
            tempQuery.Append("'");
        }

        if (!endVal.Equals(""))
        {
            if (!startVal.Equals(""))
            {
                tempQuery.Append(" AND ");
            }
            tempQuery.Append(nmFd);
            tempQuery.Append(" <= '");
            tempQuery.Append(endVal);
            tempQuery.Append("'");
        }

        tempQuery.Append(")");

        return tempQuery;
    }
    
    /** 
	* 
	* @param nmFd 
	* @param kwd 
	* @param prevKwd 
	* @param prevKwdLength 
	* @param schMethod 
	* 
	* @return query 
	*/

    public static StringBuilder makePreQuery(string nmFd, string kwd, string[] prevKwd,
                                            int prevKwdLength, string schMethod)
    {
        StringBuilder query = new StringBuilder("");

        if (prevKwd != null && prevKwdLength > 0)
        {
            for (int i = 0; i < prevKwdLength; i++)
            {
                if (!escapeQuery(prevKwd[i]).Equals(kwd, StringComparison.OrdinalIgnoreCase))
                {
                    if (query.Length > 0)
                    {
                        query.Append(" AND ");
                    }
                    query.Append(nmFd);
                    query.Append("='");
                    query.Append(escapeQuery(prevKwd[i]));
                    query.Append("' ");
                    query.Append(schMethod);
                }
            }
            if (query.Length > 0)
            {
                query = new StringBuilder("(").Append(query).Append(")");
            }
        }

        return query;
    }


    public static StringBuilder makePreQuery(string[] nmFd, string kwd, string[] prevKwd,
                                        int prevKwdLength, string schMethod)
    {
        StringBuilder query = new StringBuilder("");

        if (prevKwd != null && prevKwdLength > 0)
        {
            for (int i = 0; i < prevKwdLength; i++)
            {
                if (!escapeQuery(prevKwd[i]).Equals(kwd, StringComparison.OrdinalIgnoreCase) && !("").Equals(prevKwd[i]))
                {
                    if (query.Length > 0)
                    {
                        query.Append(" AND ");
                    }

                    query.Append("(");
                    for (int j = 0; j < nmFd.Length - 1; j++)
                    {
                        query.Append(nmFd[j]);
                        query.Append("='");
                        query.Append(escapeQuery(prevKwd[i]));
                        query.Append("' ");
                        query.Append(schMethod);
                        if (j < nmFd.Length - 2) query.Append(" OR ");
                    }
                    query.Append(")");
                }
            }
            if (query.Length > 0)
            {
                query = new StringBuilder("(").Append(query).Append(")");
            }
        }

        return query;
    }

    public static StringBuilder makePreQuery(string kwd, string[] prevKwd, int prevKwdLength)
    {
        StringBuilder query = new StringBuilder("");

        if (prevKwd != null && prevKwdLength > 0)
        {
            for (int i = 0; i < prevKwdLength; i++)
            {
                if (!escapeQuery(prevKwd[i]).Equals(kwd, StringComparison.OrdinalIgnoreCase))
                {
                    if (query.Length > 0)
                    {
                        query.Append(" AND ");
                    }                    
                    query.Append(escapeQuery(prevKwd[i]));
                }
            }
            if (query.Length > 0)
            {
                query = new StringBuilder("").Append(query).Append("");
            }
        }

        return query;
    }

    public static StringBuilder makeDetailQuery(string srchFd, string srchMethod, StringBuilder query, string includeQuery, string exactQuery)
    {
        if(includeQuery.Length > 0)
        {
            query.Insert(0, "(");
            query = query.Append(")");
            query = DCUtil.makeQuery(srchFd, includeQuery, "ALLWORD" , query, "AND");
        }
        if(exactQuery.Length > 0)
        {
            query.Insert(0, "(");
            query = query.Append(")");
            query = DCUtil.makeQuery(srchFd, exactQuery, "ANYWORD", query, "ANDNOT");
        }
        return query;
    }

    public static string setDateFd(string category)
    {
        string temp = "";

        if ("edms".Equals(category) || "sotong".Equals(category))
        {
            temp = "MDATE";
        }
        else if ("infonet".Equals(category) || "dictionary".Equals(category) || "library".Equals(category))
        {
            temp = "REG_DATE";
        }
        else if ("notice".Equals(category))
        {
            temp = "REG_DT";
        }
        else if ("brand".Equals(category))
        {
            temp = "regdate";
        }
        return temp;
    }

    public static string setSortFd(string sort, string dateFd)
    {
        string temp = "";

        if ("date".Equals(sort) && dateFd.Length > 0)
        {
            temp = " order by " + dateFd + " desc";
        }

        return temp;
    }
}
