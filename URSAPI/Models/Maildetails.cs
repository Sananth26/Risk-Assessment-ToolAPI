using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Maildetails
    {
        public long Id { get; set; }
        public string Mailaddress { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
    }
}
