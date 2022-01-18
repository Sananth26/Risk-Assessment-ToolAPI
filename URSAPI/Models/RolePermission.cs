using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class RolePermission
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long ModuleId { get; set; }
        public long? SubModuleId { get; set; }
        public string ActiveFlag { get; set; }
        public string ButtonPermissionData { get; set; }
        public DateTime CreatedTime { get; set; }
        public long CreatedBy { get; set; }
    }
}
