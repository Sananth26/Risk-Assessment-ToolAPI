using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Category
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public long MainCategoryId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public long LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public long? HeaderNumber { get; set; }
        public string IsActive { get; set; }
    }
}
