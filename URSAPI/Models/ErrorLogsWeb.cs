using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class ErrorLogsWeb
    {
        public int Id { get; set; }
        public string ErrorDescription { get; set; }
        public string ModuleName { get; set; }
        public string MethodName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
