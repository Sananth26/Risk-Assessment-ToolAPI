using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Department
    {
        public long Id { get; set; }
        public string DepartmentName { get; set; }
        public string IsActive { get; set; }
        public int NoOfLevel { get; set; }
        public string DeleteFlag { get; set; }
        public string SlaFlag { get; set; }
        public string SlaJson { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
