using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RequestForm
    {
        public long Requestid { get; set; }
        public long CategoryId { get; set; }
        public long SubcategoryId { get; set; }
        public string BusinessJustification { get; set; }
        public string ManagedServices { get; set; }
        public long? FirewallRegion { get; set; }
        public long? NormalExpedited { get; set; }
        public long? BusinessOwnersId { get; set; }
        public long? ItownersId { get; set; }
        public string NameOfProject { get; set; }
        public string Description { get; set; }
        public string BusinessImpact { get; set; }
        public string SecurityPolicy { get; set; }
        public string ArchitectureDiagram { get; set; }
        public string Status { get; set; }
        public DateTime Createdtime { get; set; }
        public long? UserId { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public string RequestSno { get; set; }
        public string RiskandRankDetails { get; set; }
        public string BusinessOwners { get; set; }
        public string ItOwners { get; set; }
        public string PeerReviewId { get; set; }
        public long? LastUpdatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsEditing { get; set; }
        public long? EditorId { get; set; }
        public DateTime? Editortimeon { get; set; }
    }
}
