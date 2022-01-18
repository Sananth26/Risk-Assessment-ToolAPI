using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class UarRequestDetails
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long AccessCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public long AccessTypeId { get; set; }
        public long UserId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime DateOfRevoke { get; set; }
        public string Status { get; set; }
        public long Level { get; set; }
        public string WorkflowStage { get; set; }
        public string ApprovalDetails { get; set; }
        public string Attachment { get; set; }
        public string DeleteFlag { get; set; }
        public string SlaData { get; set; }
        public string RequestDetailsId { get; set; }
    }
}
