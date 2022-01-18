using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Workflowcategory
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public int TotalLevel { get; set; }
    }
}
