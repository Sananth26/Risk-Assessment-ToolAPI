using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RequestSecurityPolicy
    {
        public long Id { get; set; }
        public long RequestFormId { get; set; }
        public long SourceVpcid { get; set; }
        public string SourceIpaddress { get; set; }
        public long DestinationVpcaccount { get; set; }
        public string DestinationAddress { get; set; }
        public string Application { get; set; }
        public string PortService { get; set; }
        public long Protocol { get; set; }
    }
}
