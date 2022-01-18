using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Organisation
    {
        public long Id { get; set; }
        public string OrgName { get; set; }
        public string OrgFormData { get; set; }
        public long SopCount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeleteFlag { get; set; }
        public string Filepath { get; set; }
    }
}
