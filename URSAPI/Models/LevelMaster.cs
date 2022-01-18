using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class LevelMaster
    {
        public long Id { get; set; }
        public string WorkFlowLevelName { get; set; }
        public string ActiveFlag { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
