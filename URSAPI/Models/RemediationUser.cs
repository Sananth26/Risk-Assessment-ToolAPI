using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RemediationUser
    {
        public long RemediationUserId { get; set; }
        public long? CommentId { get; set; }
        public long? UserId { get; set; }
    }
}
