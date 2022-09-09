using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;

namespace SKT.Glossary.Web
{
    public class ContentFeeds
    {
      
        public ContentFeeds()
	    {
		    
	    }

        #region 끌문서
        //public static void PostDirectoryFeeds(string dir_id, string userid, string filename, string fileext, string filesize)
        //{
        //    GlossaryDirectoryBiz _biz = new GlossaryDirectoryBiz();
        //    DataSet ds = _biz.GetDirectoryContentFeeds(dir_id, filename);

        //    string strDirNm = string.Empty;
        //    string strFileID = string.Empty;

        //    List<ReadRole> readRole = new List<ReadRole>();
        //    if(ds.Tables.Count > 0)
        //    {
        //        if(ds.Tables[0].Rows.Count > 0)
        //        {
        //            strDirNm = ds.Tables[0].Rows[0]["DIR_NM"].ToString();
        //        }

        //        if (ds.Tables[1].Rows.Count > 0)
        //        {
        //            strFileID = ds.Tables[1].Rows[0]["FILE_ID"].ToString();
        //        }
                
        //        string sCode = string.Empty;
        //        if (ds.Tables[2].Rows.Count > 0)
        //        {
        //            sCode = string.Empty;
        //            foreach(DataRow dr in ds.Tables[2].Rows)
        //            {
        //                sCode += dr["AUTH_ID"].ToString()+ ",";
        //            }
        //            ReadRole _readRole = new ReadRole();
        //            _readRole.type = "user";
        //            _readRole.code = sCode.Substring(0, sCode.Length -1);
        //            readRole.Add(_readRole);
        //        }

        //        if (ds.Tables[3].Rows.Count > 0)
        //        {
        //            sCode = string.Empty;
        //            foreach (DataRow dr in ds.Tables[3].Rows)
        //            {
        //                sCode += dr["AUTH_ID"].ToString() + ",";
        //            }
        //            ReadRole _readRole = new ReadRole();
        //            _readRole.type = "dept";
        //            _readRole.code = sCode.Substring(0, sCode.Length - 1);
        //            readRole.Add(_readRole);
        //        }

        //    }
        //    List<FeedFileInfo> feedFileInfo = new List<FeedFileInfo>();
        //    //////////////////////////////////////////////////////////
        //    FeedFileInfo tmpfeedFileInfo = new FeedFileInfo();
        //    tmpfeedFileInfo.Size = filesize;
        //    tmpfeedFileInfo.fileType = fileext;
        //    tmpfeedFileInfo.Name = filename;
        //    tmpfeedFileInfo.url = filename;
        //    feedFileInfo.Add(tmpfeedFileInfo);

        //    OpinionBoardFeed bsObj = new OpinionBoardFeed();
        //    bsObj.sbmId = "TKL@200@" + strFileID;
        //    bsObj.pushTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        //    bsObj.writeProfile = userid;
        //    bsObj.contentTitle = "["+strDirNm+"]" + filename;
        //    bsObj.contentCont = "";
        //    bsObj.linkUrl = BaseURL + "/Directory/FileOpenTransfer.aspx?file=" + dir_id + "/" + filename;
        //    bsObj.readRole = readRole;

        //    bsObj.replyYn = "N";
        //    bsObj.replyType = "N";
        //    bsObj.likeYn = "N";
        //    bsObj.nickName = "";
        //    bsObj.feedFileFInfo = feedFileInfo;

        //    SendFeeds(bsObj, "POST");
        //}

        //public static void DeleteDirectoryFeeds(string dir_id, string file_name)
        //{
        //    OpinionBoardFeed bsObj = new OpinionBoardFeed();
        //    bsObj.sbmId = "TKL@" + dir_id + "@" + file_name;

        //    SendFeeds(bsObj, "DELETE");
        //}
        #endregion

        public static void SendFeeds(OpinionBoardFeed bsObj, string method)
        {
            GlossaryBiz _biz = new GlossaryBiz();

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            string url = System.Configuration.ConfigurationManager.AppSettings["eTnetContentFeedsUrl"];

            string sbmid = string.Empty;
            string status = string.Empty;
            string errMsg = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = string.Empty;

            if (method.Equals("POST"))
            {
                jsonData = js.Serialize(bsObj);
            }
            else if (method.Equals("PUT"))
            {
                jsonData = js.Serialize(bsObj);
                url += "/" + bsObj.sbmId;
            }
            else if (method.Equals("DELETE"))
            {
                url += "/" + bsObj.sbmId;
            }
            else if (method.Equals("GET"))
            {
                url += "/" + bsObj.sbmId;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = method;
                request.Timeout = 3000;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                WebResponse response = request.GetResponse();
                var responseBody = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var ResponseMsg = new JavaScriptSerializer().Deserialize<ResponseMessage>(responseBody);

                sbmid = ResponseMsg.contentFeedId;
                status = ResponseMsg.status;
                errMsg = ResponseMsg.errMsg;

            }
            catch (WebException wex)
            {
                if (wex.Response == null)
                {
                    sbmid = bsObj.sbmId;
                    status = "999";
                    errMsg = wex.Message;
                }
                else
                {
                    var responseBody = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd(); 
                    var ResponseMsg = new JavaScriptSerializer().Deserialize<ResponseMessage>(responseBody);
                    
                    sbmid = bsObj.sbmId;
                    status = ResponseMsg.status;
                    errMsg = ResponseMsg.errMsg;
                }

            }
            catch (Exception ex)
            {
                sbmid = bsObj.sbmId;
                status = "999";
                errMsg = ex.Message.ToString();
            }
            finally
            {
                _biz.SetTnetContentFeedsLog(sbmid, method, status, errMsg, jsonData);
            }
        }
    }


}