using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class LookupSubcategory
    {
        public LookupSubcategory()
        {
            LookupSubitem = new HashSet<LookupSubitem>();
        }

        public long Id { get; set; }
        public long MainCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

        public virtual LookupCategory MainCategory { get; set; }
        public virtual ICollection<LookupSubitem> LookupSubitem { get; set; }
    }
}
