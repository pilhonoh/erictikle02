using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Net;

namespace Tikle_ContentFeeds
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContentFeeds : IContentFeeds
    {
        #region test
        public ResponseMessage Feed(OpinionBoardFeed mb)
        {
            ResponseMessage res = new ResponseMessage();

            res.status = "202";
            //res.contentFeedId = mb.sbmId;
            res.errMsg = "정상 등록되었습니다.";

            return res;
        }

        public ResponseMessage FeedDelete(string sbmId)
        {
            ResponseMessage res = new ResponseMessage();

            res.status = "202";
            //res.contentFeedId = sbmId;
            res.errMsg = "정상 삭제되었습니다.";

            return res;
        }

        public ResponseMessage FeedUpdate(string sbmId, OpinionBoardFeed mb)
        {
            ResponseMessage res = new ResponseMessage();

            res.status = "202";
            //res.contentFeedId = sbmId;
            res.errMsg = "정상 수정되었습니다.";

            return res;
        }
        #endregion

        #region InsertReply
        public ReplyResponseMessage InsertReply(Reply ry)
        {
            string methodName = "InsertReply";

            GlossaryBiz _biz = new GlossaryBiz();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(ry);

            if (ry.sbmId.IndexOf('@') < 0)
            {
                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), "sbmId is required", jsonData);

                throw new WebFaultException<ResponseMessage>
                    (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = "sbmId is required" }, HttpStatusCode.InternalServerError);
            }

            //GlossaryType Board = new GlossaryType();
            //Board = _biz.GlossarySelect(ry.sbmId.Split('@')[2].ToString(), "", "");

            //if (Board.ID == null)
            //{
            //    _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), "not exists sbmid", jsonData);

            //    throw new WebFaultException<ResponseMessage>
            //        (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = "not exists sbmid" }, HttpStatusCode.InternalServerError);
            //}

            CommCommentType Comment = new CommCommentType();

            Comment.USERID = ry.writeEmpNo;
            Comment.COMMENTTYPE = "Glossary";
            Comment.COMMONID = ry.sbmId.Split('@')[2].ToString();
            Comment.LIKECOUNT = "0";
            Comment.ID = ry.parentId == "0" ? "" : ry.parentId;

            Comment.PUBLICYN = ry.nickNameYN.Equals("N") ? "Y" : "N";
            Comment.CONTENTS = MakeURLLink(SKT.Common.Utility.BREncode2(SKT.Common.SecurityHelper.Clear_XSS_CSRF(ry.writeCont)));
            Comment.USERIP = "";
            Comment.USERMACHINENAME = "ContentFeeds";

            GlossaryControlBiz biz = new GlossaryControlBiz();

            DataSet ds = new DataSet();

            ReplyResponseMessage res = new ReplyResponseMessage();

            if (ry.parentId.Equals("0"))
            {
                try
                {
                    CommCommentType cmm = biz.commCommentInsert(Comment);

                    if (!String.IsNullOrEmpty(cmm.ID) && cmm.ID != "0")
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.Accepted);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = cmm.ID;
                        res.errMsg = "OK";
                    }
                    else
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.InternalServerError);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = "0";
                        res.errMsg = "Save Error";
                    }

                    _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, res.status, "", jsonData);
                }
                catch (Exception ex)
                {

                    _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), ex.Message.ToString(), jsonData);

                    throw new WebFaultException<ResponseMessage>
                         (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = ex.Message.ToString() }, HttpStatusCode.InternalServerError);
                }

            }
            else
            {
                try
                {
                    ds = biz.commCommentSupInsert(Comment);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["DBFLAG"].ToString().Equals("0"))
                        {
                            res.status = Convert.ToString((int)HttpStatusCode.Accepted);
                            res.contentFeedId = ry.sbmId;
                            res.msgId = ds.Tables[0].Rows[0]["ID"].ToString();
                            res.errMsg = "OK";
                        }
                        else
                        {
                            res.status = Convert.ToString((int)HttpStatusCode.InternalServerError);
                            res.contentFeedId = ry.sbmId;
                            res.msgId = "0";
                            res.errMsg = "Save Error";
                        }

                        _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, res.status, "", jsonData);
                    }
                }
                catch (Exception ex)
                {
                    _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), ex.Message.ToString(), jsonData);

                    throw new WebFaultException<ResponseMessage>
                         (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = ex.Message.ToString() }, HttpStatusCode.InternalServerError);
                }
            }

            return res;
        }
        #endregion

        #region UpdateReply
        public ReplyResponseMessage UpdateReply(Reply ry)
        {
            string methodName = "UpdateReply";

            GlossaryBiz _biz = new GlossaryBiz();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(ry);

            if (ry.sbmId.IndexOf('@') < 0)
            {
                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), "sbmId is required", jsonData);

                throw new WebFaultException<ResponseMessage>
                    (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = "sbmId is required" }, HttpStatusCode.InternalServerError);
            }

            CommCommentType Comment = new CommCommentType();

            Comment.USERID = ry.writeEmpNo;
            Comment.COMMENTTYPE = "Glossary";
            Comment.COMMONID = ry.sbmId.Split('@')[2].ToString();
            Comment.LIKECOUNT = "0";
            Comment.ID = ry.writeMsgId;
            Comment.PUBLICYN = ry.nickNameYN.Equals("N") ? "Y" : "N";
            Comment.CONTENTS = MakeURLLink(SKT.Common.Utility.BREncode2(SKT.Common.SecurityHelper.Clear_XSS_CSRF(ry.writeCont)));
            Comment.USERIP = "";
            Comment.USERMACHINENAME = "ContentFeeds";

            GlossaryControlBiz biz = new GlossaryControlBiz();
  
            ReplyResponseMessage res = new ReplyResponseMessage();
            
            try
            {
                DataSet ds = biz.commCommentUpdate(Comment);

                if (ds != null && ds.Tables.Count > 0)
                {
                    string dbflag = ds.Tables[0].Rows[0]["DBFLAG"].ToString();

                    if (dbflag.Equals("0"))
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.Accepted);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = ry.writeMsgId;
                        res.errMsg = "OK";
                    }
                    else
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.InternalServerError);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = ry.writeMsgId;
                        res.errMsg = "Save Error";
                    }
                }

                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, res.status, "", jsonData);
            }
            catch (Exception ex)
            {

                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), ex.Message.ToString(), jsonData);

                throw new WebFaultException<ResponseMessage>
                        (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = ex.Message.ToString() }, HttpStatusCode.InternalServerError);
            }

            return res;
        }
        #endregion

        #region DeleteReply
        public ReplyResponseMessage DeleteReply(Reply ry)
        {
            string methodName = "DeleteReply";

            GlossaryBiz _biz = new GlossaryBiz();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(ry);

            if (ry.sbmId.IndexOf('@') < 0)
            {
                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), "sbmId is required", jsonData);

                throw new WebFaultException<ResponseMessage>
                    (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = "sbmId is required" }, HttpStatusCode.InternalServerError);
            }

            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            CommCommentType Comment = new CommCommentType();

            Comment.COMMONID = ry.sbmId.Split('@')[2].ToString();
            Comment.ID = ry.writeMsgId;
            Comment.LASTMODIFIERID = "ContentFeeds";

            ReplyResponseMessage res = new ReplyResponseMessage();
            try
            {
                ds = biz.commCommentDelete(Comment);
                
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["DBFLAG"].ToString().Equals("0"))
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.Accepted);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = string.Empty;
                        res.errMsg = "OK";
                    }
                    else
                    {
                        res.status = Convert.ToString((int)HttpStatusCode.InternalServerError);
                        res.contentFeedId = ry.sbmId;
                        res.msgId = string.Empty;
                        res.errMsg = "Save Error";
                    }

                    _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, res.status, string.Empty, jsonData);
                }

            }
            catch (Exception ex)
            {
                _biz.SetTnetContentFeedsLog(ry.sbmId, methodName, Convert.ToString((int)HttpStatusCode.InternalServerError), ex.Message.ToString(), jsonData);

                throw new WebFaultException<ResponseMessage>
                     (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = ex.Message.ToString() }, HttpStatusCode.InternalServerError);
            }
            return res;
        }
        #endregion

        #region GetReply
        public ReplyList GetReply(string sbmid)
        {
            ReplyList replyList = new ReplyList();

            if(sbmid.Length == 0 || sbmid.IndexOf('@') < 0)
            {
                new GlossaryBiz().SetTnetContentFeedsLog(sbmid, "GetReply", Convert.ToString((int)HttpStatusCode.InternalServerError), "sbmId is required", sbmid);

                throw new WebFaultException<ResponseMessage>
                    (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = "sbmId is required" }, HttpStatusCode.InternalServerError);
            }

            string commIdx = sbmid.Split('@')[2].ToString();

            DataSet ds = new DataSet();
            GlossaryControlBiz biz = new GlossaryControlBiz();

            try
            {
                ds = biz.commCommentListSelect("glossary", commIdx, "", 1, 5);


                if (ds != null && ds.Tables.Count > 0)
                {
                    replyList.status = Convert.ToString((int)HttpStatusCode.Accepted);
                    replyList.contentFeedId = sbmid;

                    string changeTag = string.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ReplyMember _mem = new ReplyMember();
                        _mem.writeMsgId = dr["ID"].ToString();
                        _mem.writeEmpNo = dr["UserID"].ToString();
                        _mem.writeDepart = dr["DeptName"].ToString();

                        changeTag = dr["CONTENTS"].ToString();
                        changeTag = SKT.Common.Utility.BREncode2(changeTag);
                        changeTag = MakeURLLink(SKT.Common.SecurityHelper.Add_XSS_CSRF(changeTag));

                        _mem.writeCont = changeTag;

                        if (dr["ID"].ToString().Equals(dr["SUP_ID"].ToString()))
                        {
                            _mem.depth = "0";
                            _mem.parentMsgId = "0";
                        }
                        else
                        {
                            _mem.depth = "1";
                            _mem.parentMsgId = dr["SUP_ID"].ToString();
                        }

                        _mem.createDt = dr["CREATEDATE"].ToString();

                        if (String.IsNullOrEmpty(dr["UserID"].ToString().Trim()))
                        {
                            _mem.nickNameYN = "Y";
                            _mem.writeNickName = dr["UserName"].ToString();
                            _mem.writeName = string.Empty;

                            //CHG610000078167 / 2018-12-31 추가
                            _mem.modifyYN = "N";
                            _mem.deleteYN = "N";
                        }
                        else
                        {
                            _mem.nickNameYN = "N";
                            _mem.writeNickName = string.Empty;
                            _mem.writeName = dr["UserName"].ToString();

                            //CHG610000078167 / 2018-12-31 추가
                            _mem.modifyYN = "Y";
                            _mem.deleteYN = "Y";
                        }
                        replyList.result.Add(_mem);
                    }
                }
                else
                {
                    replyList.status = Convert.ToString((int)HttpStatusCode.Accepted);
                    replyList.contentFeedId = sbmid;
                }
            }
            catch (Exception ex)
            {
                new GlossaryBiz().SetTnetContentFeedsLog(sbmid, "GetReply", Convert.ToString((int)HttpStatusCode.InternalServerError), ex.Message.ToString(), sbmid);

                throw new WebFaultException<ResponseMessage>
                     (new ResponseMessage { status = Convert.ToString((int)HttpStatusCode.InternalServerError), errMsg = ex.Message.ToString() }, HttpStatusCode.InternalServerError);
            }

           return replyList;
        }
        #endregion

        #region MakeURLLink
        public static string MakeURLLink(string Contents)
        {
            string strContent = Contents;
            Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)",
                             RegexOptions.IgnoreCase | RegexOptions.Compiled);

            strContent = urlregex.Replace(strContent,
                         "<a href=\"$1\" target=\"_blank\">$1</a>");

            Regex emailregex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)",
                               RegexOptions.IgnoreCase | RegexOptions.Compiled);

            strContent = emailregex.Replace(strContent, "<a href=mailto:$1>$1</a>");

            strContent = strContent.Replace("\n", "<br />");

            return strContent;
        }
        #endregion
    }
}
