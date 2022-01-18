using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class CompanyInfo
    {
        public long Id { get; set; }
        public string FormData { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public long LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string DeleteFlag { get; set; }
    }
}
