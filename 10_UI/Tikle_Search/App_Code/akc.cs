using System;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Configuration;


/// <summary>
/// AKC Gateway 에 해당하는 cs 코드.
/// Interop.MSXML2.dll, Interop.ATLDOCRUZER_X_XLib.dll 이 참조 라이브러리로 프로젝트에 등록되어 있어야 함
/// </summary>
public class akc
{


    #region wisenut
    // sf-1에서 url을 통해 직접 자동완성 리스트를 요청하는 메소드
    public string akc_call(string query)
    {

        // 추후 사용 ip와 port로 변경하여 사용
        string ip = ConfigurationManager.AppSettings["WISENUT_SERVER_IP"];
        string port = ConfigurationManager.AppSettings["WISENUT_SERVER_PORT_AUTOCOMPLETE"];

        //string ip = "150.19.42.89";
        //string port = "7800";


        // 기본적으로 자동완성 리스트를 받아 오도록 하는 parameter 설정
        string convert = "fw";
        string target = "common";
        string charset = "utf-8";
        string datatype = "json";

        // akc.js에서 사용할 변수 초기화
        //string body = "var myJSONObject = {\"LIST\": [";
        string body = "{\"LIST\": [";

        // 자동완성 리스트를 받아올 url 설정
        string url = "http://" + ip + ":" + port + "/manager/WNRun.do";
        string parameter = "query=" + query + "&convert=" + convert + "&target=" + target + "&charset=" + charset + "&datatype=" + datatype;


        url = url + "?" + parameter;

        try
        {

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Timeout = 10000;
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();



            // url에서 데이터를 string 형태로 받아서 JObject로 파싱
            string result = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                result = client.DownloadString(url);
            }


            JObject obj = JObject.Parse(result);
            JToken objResult = (JArray)obj["result"];

            // keyword간 구분자를 알맞게 넣어주기위해 필요한 변수
            int count = 0;

            // akc.js에서 사용할 json object를 만들어 주는 반복문
            foreach (JToken data in objResult)
            {
                int i = 0;
                string totalcount = (string)data["totalcount"];

                if (!totalcount.Equals("0") )
                {
                    JToken items = (JArray)data["items"];
                    if (count < 10)
                    {
                        foreach (JToken item in items)
                        {
                            if (count != 0)
                            {
                                body += ",";
                            }

                            body += "{\"KEYWORD\":\"";
                            body += item["keyword"];
                            body += "\"}";
                            i++;
                            count++;
                        }

                    }
                }

            }
            body += "]}";
            //body += "]};";
            //body += " eQuery=\"\";";
        }
        catch (Exception ex)
        {
            body = "";
        }

        return body;
    }

    #endregion


    #region konan
    /*
    //직접 Docruzer 서버에 접속하여 데이터를 받아오는 메소드
    public string akc_call(string serverAddr, string query, int flag, int maxKwdCount, int domainNo)
    {
        ATLDocruzer.RCW.ClientClass Docruzer = new ATLDocruzer.RCW.ClientClass();

        string body;

        try
        {
            if (Docruzer.BeginSession() < 0)
            {
                throw (new ATLDocruzerException("Begin Sessions : " + Docruzer.msg));
            }

            //int CompleteKeyword2(string bstrServiceAddr, out object vnKwdCount, out object vKwd,
            //out object vRank, out object vTag, out object vNum, out object vCnvStr,
            //int nMaxKwdCount, string SeedStr, int nFlag, int nDomainNo)
            //ATLDocruzer.RCW.IClient의 멤버

            object vnKwdCount;//int
            object vKwd;//string
            object vRank;//int
            object vTag;//string
            object vNum;//string
            object vCnvStr;//string

            //Docruzer.SetOption(Docruzer.OPTION_REQUEST_CHARSET_UTF8, 1);
            //Docruzer.SetOption(Docruzer.CS_EUCKR, 1);
            //query = HttpUtility.UrlEncode(query);

            if (Docruzer.CompleteKeyword2(serverAddr, out vnKwdCount, out vKwd, out vRank, out vTag, out vNum, out vCnvStr, maxKwdCount, query, flag, domainNo) < 0)
            {
                throw (new ATLDocruzerException("CompleteKeyword2 : " + Docruzer.msg));
            }

            if (Docruzer.EndSession() < 0)
            {
                throw (new ATLDocruzerException("End Sessions : " + Docruzer.msg));
            }

            int nKwdCount = Convert.ToInt32(vnKwdCount.ToString());
            string sCnvStr = vCnvStr.ToString();

            string[] asKwd = new string[nKwdCount];
            int[] anRank = new int[nKwdCount];
            string[] asTag = new string[nKwdCount];
            string[] asNum = new string[nKwdCount];

            for (int i = 0; i < nKwdCount; i++)
            {
                asKwd[i] = (((object[])(vKwd))[i]).ToString();
                anRank[i] = Convert.ToInt32((((object[])(vRank))[i]).ToString());
                asTag[i] = (((object[])(vTag))[i]).ToString();
                asNum[i] = (((object[])(vNum))[i]).ToString();
            }

            body = "";
            body = "{\"LIST\": [";

            for (int i = 0; i < nKwdCount; i++)
            {
                if (i != 0)
                {
                    body += ",";
                }
                body += "{\"KEYWORD\":\"";
                body += asKwd[i];
                body += "\"}";
            }

            body += "]}";

            if (sCnvStr.Length != 0)
            {
                //body += "eQuery=\"" + sCnvStr + "\";";
            }
            else
            {
                //body += "eQuery=\"\";";
            }
        }
        catch (KonanException ex)
        {
            body = "";

        }

        return body;
    }
    */
    #endregion
}

public class ATLDocruzerException : System.Web.HttpException
{
    public ATLDocruzerException(string message)
        :
        base(message) // 메시지를 기본 클래스에 전달한다.
    {

    }
}