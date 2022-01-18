using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class BulkRequest
    {
        public long Id { get; set; }
        public long? RequestId { get; set; }
        public long? AccessCategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? AccessTypeId { get; set; }
        public long? UserId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual MyRequest IdNavigation { get; set; }
    }
}
