using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class MainCategory
    {
        public long Id { get; set; }
        public string MainCategoryName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public long LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string IsActive { get; set; }
    }
}
