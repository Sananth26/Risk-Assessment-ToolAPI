using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class TemplateForSop
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateStructure { get; set; }
        public string ActiveFlag { get; set; }
        public string DeleteFlag { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
