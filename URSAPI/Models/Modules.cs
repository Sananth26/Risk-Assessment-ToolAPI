using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Modules
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Children { get; set; }
        public long DisplayOrder { get; set; }
        public string DynamicPresence { get; set; }
        public string DynamicTemplateName { get; set; }
    }
}
