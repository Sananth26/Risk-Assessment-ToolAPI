using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class UarRequestmaster
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string WorkflowStage { get; set; }
        public int Level { get; set; }
        public string Status { get; set; }
        public string ApprovalDetails { get; set; }
        public string SlaData { get; set; }
        public string ApprovalAttachment { get; set; }
    }
}
