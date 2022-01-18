using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Workflow
    {
        public long Id { get; set; }
        public int? Level { get; set; }
        public string LevelName { get; set; }
        public string SelectedRoles { get; set; }
        public string SelectedUsers { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedSubCategory { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeleteFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string SlaActive { get; set; }
        public int? SlaDays { get; set; }
    }
}
