using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class LookupSubitem
    {
        public long Id { get; set; }
        public long SubcategoryId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public short DisplayOrder { get; set; }
        public string ActiveFlag { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long UpdatedBy { get; set; }
    }
}
