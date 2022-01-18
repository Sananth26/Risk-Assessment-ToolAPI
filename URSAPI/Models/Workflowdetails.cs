using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Workflowdetails
    {
        public long Id { get; set; }
        public long CombinationId { get; set; }
        public long Level { get; set; }
        public string LevelName { get; set; }
        public string SelectedRoles { get; set; }
        public string SelectedUsers { get; set; }
        public string DeleteFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string SlaActive { get; set; }
        public int SlaDays { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ModifiedBy { get; set; }
        public string SelectedLevels { get; set; }
    }
}
