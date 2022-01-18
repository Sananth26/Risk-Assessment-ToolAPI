using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class Remediation
    {
        public class ReplyList
        {
            public int Id { get; set; }
            public string Comments { get; set; }
            public int ReplyId { get; set; }
            public string DocumentType { get; set; }
            public int ItemId { get; set; }
            public int RequestId { get; set; }
            public string UserName { get; set; }
            public string UserBadge { get; set; }
            public DateTime Date { get; set; }
            public string Diffdate   { get; set; }
            public bool UserLikeFlag { get; set; }
            public int UserLikeCount { get; set; }
            public bool ChildFlag { get; set; }
            public bool EditFlag { get; set; }
            public bool EditIndividualFlag { get; set; }
            public bool ReplyFlag { get; set; }
            public bool CheckedFlag { get; set; }
            public int totalCount { get; set; }
            public List<ReplyList> list { get; set; }
        }

        public class RemediationDTO
        {
            public int id { get; set; }
            public string comments { get; set; }
            public int replyId { get; set; }
            public string documentType { get; set; }
            public int itemId { get; set; }
            public int requestId { get; set; }
            public string userName { get; set; }
            public string userBadge { get; set; }
            public string date { get; set; }
            public bool userLikeFlag { get; set; }
            public int userLikesCount { get; set; }
            public bool childFlag { get; set; }
            public bool editFlag { get; set; }
            public bool editIndividualFlag { get; set; }
            public bool replyFlag { get; set; }
            public List<object> list { get; set; }
        }
        public class RemediationDTOList
        {
            public int RequestId { get; set; }
            public List<ReplyList> list { get; set; }
        }

        public class RemediationParams
        {
            public int RequestId { get; set; }
            public int PageNumber { get; set; }
            public string   Comment  {get;set;}
        }

        public class RemediationCheckedandlike
        {
            public int CommentId { get; set; }
            public Boolean Status { get; set; }
            
        }
        public class RemediationReplyDTO
        {
            public int RemediationReplyId { get; set; }
            public int CommentId { get; set; }
            public int CommentReplyId { get; set; }
            public int UserId { get; set; }
        }
    }
}
