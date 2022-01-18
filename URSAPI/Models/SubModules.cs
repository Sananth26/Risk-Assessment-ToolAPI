using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class SubModules
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long TbdModuleId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public long DisplayOrder { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
