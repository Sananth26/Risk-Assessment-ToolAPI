using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Location { get; set; }
        public int Department { get; set; }
        public string EmailAlertFlag { get; set; }
        public string DeleteFlag { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long? ImmediateSupervisor { get; set; }
        public DateTime Doj { get; set; }
        public string ItProcessAccess { get; set; }
        public string CorporateId { get; set; }
        public string IsActive { get; set; }
        public string ManagerAccess { get; set; }
    }
}
