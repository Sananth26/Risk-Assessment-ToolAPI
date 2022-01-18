using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RemediationComment
    {
        public long CommentId { get; set; }
        public string Comments { get; set; }
        public long? ReplyId { get; set; }
        public long? RequestId { get; set; }
        public DateTime Date { get; set; }
        public bool? UserLikeFlag { get; set; }
        public long? UserLikeCount { get; set; }
        public bool? ChildFlag { get; set; }
        public bool? EditFlag { get; set; }
        public bool? ReplyFlag { get; set; }
        public bool? CheckedFlag { get; set; }
    }
}
