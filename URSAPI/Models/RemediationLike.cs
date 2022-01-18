using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RemediationLike
    {
        public long RemediationLikeId { get; set; }
        public long? CommentId { get; set; }
        public long? UserId { get; set; }
    }
}
