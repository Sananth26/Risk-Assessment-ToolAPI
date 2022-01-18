using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class UserRoles
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? RoleId { get; set; }
    }
}
