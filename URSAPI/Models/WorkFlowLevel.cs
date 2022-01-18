using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class WorkFlowLevel
    {
        public long Id { get; set; }
        public long WorkFlowId { get; set; }
        public long LevelId { get; set; }
    }
}
