using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RemediationReply
    {
        public long RemediationReplyId { get; set; }
        public long? CommentId { get; set; }
        public long? CommentReplyId { get; set; }
        public long? UserId { get; set; }
    }
}
