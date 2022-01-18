using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class MyRequest
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
        public string CreatedBy { get; set; }

        public virtual BulkRequest BulkRequest { get; set; }
    }
}
