using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SKT.Glossary.Type
{
    public class ContentsFeedType
    {
    }

   
    [DataContract]
    public class OpinionBoardFeed
    {
        [DataMember(Order = 0)]
        //[StringLength(100)]
        public string sbmId { get; set; }
        [DataMember(Order = 1)]
        //[StringLength(15)]
        public string pushTime { get; set; }
        [DataMember(Order = 2)]
        //[StringLength(7)]
        public string writeProfile { get; set; }
        [DataMember(Order = 3)]
        //[StringLength(100)]
        public string contentTitle { get; set; }
        [DataMember(Order = 4)]
        //[StringLength(500)]
        public string contentCont { get; set; }
        [DataMember(Order = 5)]
        //[StringLength(300)]
        public string linkUrl { get; set; }
        [DataMember(Order = 6)]
        //[StringLength(200)]
        public List<ReadRole> readRole = new List<ReadRole>();
        [DataMember(Order = 7)]
        //[StringLength(1)]
        public string replyYn { get; set; }
        [DataMember(Order = 8)]
        //[StringLength(3)]
        public string replyType { get; set; }
        [DataMember(Order = 9)]
        //[StringLength(1)]
        public LikeYN likeYn { get; set; }
        [DataMember(Order = 10)]
        //[StringLength(20)]
        public string nickName { get; set; }
        [DataMember(Order = 11)]
        //[StringLength(500)]
        public List<FeedFileInfo> feedFileInfo = new List<FeedFileInfo>();
        [DataMember(Order = 12)]
        //[StringLength(500)]
        public List<ThumbnailFile> thumbnailFile = new List<ThumbnailFile>();
    }
    [DataContract]
    public class ReadRole
    {
        [DataMember(Order = 0)]
        public string type { get; set; }
        [DataMember(Order = 1)]
        public string code { get; set; }

    }

    [DataContract]
    public class LikeYN
    {
        [DataMember(Order = 0)]
        public string ADD { get; set; }
        [DataMember(Order = 1)]
        public string CANCEL { get; set; }

    }
    [DataContract]
    public class FeedFileInfo
    {
        [DataMember(Order = 0)]
        public string fileType { get; set; }
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 2)]
        public string Size { get; set; }
        [DataMember(Order = 3)]
        public string url { get; set; }
    }
    [DataContract]
    public class ThumbnailFile
    {
        [DataMember(Order = 0)]
        public string altText { get; set; }
        [DataMember(Order = 1)]
        public string imgName { get; set; }
        [DataMember(Order = 2)]
        public string imgUrl { get; set; }
    }

    [DataContract]
    public class ReplyList
    {
        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string contentFeedId { get; set; }

        public ReplyList()
        {
            result = new List<ReplyMember>();
        }
        [DataMember]
        public List<ReplyMember> result;

    }

    [DataContract]
    public class ReplyMember
    {
        [DataMember]
        public string writeMsgId { get; set; }
        [DataMember]
        public string writeEmpNo { get; set; }
        [DataMember]
        public string writeName { get; set; }
        [DataMember]
        public string writeNickName { get; set; }
        [DataMember]
        public string writeDepart { get; set; }
        [DataMember]
        public string writeCont { get; set; }
        [DataMember]
        public string parentMsgId { get; set; }
        [DataMember]
        public string depth { get; set; }
        [DataMember]
        public string createDt { get; set; }
        [DataMember]
        public string nickNameYN { get; set; }
        [DataMember]
        public string modifyYN { get; set; }
        [DataMember]
        public string deleteYN { get; set; }
    }

    [DataContract]
    public class Reply
    {
        [DataMember]
        public string sbmId { get; set; }
        [DataMember]
        public string parentId { get; set; }
        [DataMember]
        public string depth { get; set; }
        [DataMember]
        public string writeEmpNo { get; set; }
        [DataMember]
        public string writeName { get; set; }
        [DataMember]
        public string writeNickName { get; set; }
        [DataMember]
        public string writeDepart { get; set; }
        [DataMember]
        public string writeCont { get; set; }
        [DataMember]
        public string secretKey { get; set; }
        [DataMember]
        public string nickNameYN { get; set; }
        [DataMember]
        public string writeMsgId { get; set; }
    }

    [DataContract]
    public class ResponseMessage
    {
        [DataMember(Order = 0)]
        public string status { get; set; }

        [DataMember(Order = 1)]
        public string contentFeedId { get; set; }

        [DataMember(Order = 1)]
        public string errMsg { get; set; }
    }

    [DataContract]
    public class ReplyResponseMessage
    {
        [DataMember(Order = 0)]
        public string status { get; set; }

        [DataMember(Order = 1)]
        public string contentFeedId { get; set; }

        [DataMember(Order = 2)]
        public string msgId { get; set; }

        [DataMember(Order = 3)]
        public string errMsg { get; set; }

    }
}
