using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class AuditTrail
    {
        public long Id { get; set; }
        public string Browser { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Event { get; set; }
        public string IpAddress { get; set; }
        public long? Orgid { get; set; }
        public long? Userid { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public string Systemremarks { get; set; }
        public string RequestId { get; set; }
        public string Attachments { get; set; }
    }
}
