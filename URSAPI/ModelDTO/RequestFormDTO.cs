using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class RequestFormDTO
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
        public string Categoryname { get; set; }
        public string Subcategoryname { get; set; }
        public string Assignedto { get; set; }
        public string Lastupdate { get; set; }
        public string RequestSno { get; set; }
        public string peerreviewid { get; set; }
        public Boolean canEdit { get; set; }
    }

    public class DashBoardCount
    {
        public long opencount { get; set; }
        public long peerreviewcount { get; set; }
        public long approvalcount { get; set; }
        public long closedcount { get; set; }
        public long managercount { get; set; }
        public long userpublishcount { get; set; }
    }

    public class StepperDTO
    {
        public long RequestId { get; set; }
        public string requestSno { get; set; }
        public string LevelName { get; set; }
        public string ActionTakenBy   { get; set; }
        public string ActionTakenOn { get; set; }
        public string Status { get; set; }
        public string currentStatus { get; set; }

    }
}
