using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class UrsOrg
    {
        public long Id { get; set; }
        public long? ReqId { get; set; }
        public long? OrgId { get; set; }
        public string FilePath { get; set; }
    }
}
