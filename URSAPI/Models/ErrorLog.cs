using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class ErrorLog
    {
        public long Id { get; set; }
        public string Page { get; set; }
        public string Methodname { get; set; }
        public string ErrorMessage { get; set; }
        public long Userid { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
