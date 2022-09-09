using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SKT.Glossary.Type;

namespace Tikle_ContentFeeds
{
    [ServiceContract]
    public interface IContentFeeds
    {
        //[OperationContract]
        //[WebGet(UriTemplate = "GetData/{value}", BodyStyle = WebMessageBodyStyle.WrappedResponse,  ResponseFormat = WebMessageFormat.Json)]
        //IList<Member> GetData(string value);


        //[OperationContract]
        //[WebInvoke(UriTemplate = "GetDataPost"
        //    , Method = "POST"
        //    , BodyStyle = WebMessageBodyStyle.Bare
        //    , RequestFormat = WebMessageFormat.Json
        //    , ResponseFormat = WebMessageFormat.Json
        //)]
        //ResponseMessage GetDataPost(Member mb);

        [OperationContract]
        [WebInvoke(UriTemplate = "Feeds"
            , Method = "POST"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ResponseMessage Feed(OpinionBoardFeed mb);

        [OperationContract]
        [WebInvoke(UriTemplate = "Feeds/{sbmid}"
            , Method = "DELETE"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ResponseMessage FeedDelete(string sbmId);

        [OperationContract]
        [WebInvoke(UriTemplate = "Feeds/{sbmid}"
            , Method = "PUT"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ResponseMessage FeedUpdate(string sbmId, OpinionBoardFeed mb);

        /// <summary>
        /// 댓글 저장
        /// </summary>
        /// <param name="ry"></param>
        /// <returns>SetReply</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "ReplyFeeds"
            , Method = "POST"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ReplyResponseMessage InsertReply(Reply ry);

        /// <summary>
        /// 댓글 수정
        /// </summary>
        /// <param name="ry"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateReplyFeeds"
            , Method = "POST"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ReplyResponseMessage UpdateReply(Reply ry);

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="ry"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteReplyFeeds"
            , Method = "POST"
            , BodyStyle = WebMessageBodyStyle.Bare
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
        )]
        ReplyResponseMessage DeleteReply(Reply ry);

        /// <summary>
        /// 댓글 리스트
        /// </summary>
        /// <param name="smbid"></param>
        /// <returns></returns>
        [OperationContract]
        [return: MessageParameter(Name = "result")]
        [WebGet(UriTemplate = "ReplyFeeds/{sbmid}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        ReplyList GetReply(string sbmid);

    }


    [DataContract]
    public class Member
    {
        [DataMember(Order = 0)]
        public string name { get; set; }

        [DataMember(Order = 1)]
        public int age { get; set; }
    }



}
