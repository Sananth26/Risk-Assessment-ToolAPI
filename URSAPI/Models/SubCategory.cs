using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class SubCategory
    {
        public long Id { get; set; }
        public string Risk { get; set; }
        public long MainCategoryId { get; set; }
        public long CategoryId { get; set; }
        public string QuestionandExplanation { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public long LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string QuestionNumber { get; set; }
        public string IsActive { get; set; }
    }
}
