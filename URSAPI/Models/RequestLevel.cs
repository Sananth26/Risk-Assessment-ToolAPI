using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RequestLevel
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long LevelId { get; set; }
        public long UserId { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
