using System;
using System.Collections.Generic;

namespace URSAPI.Models
{
    public partial class Holidayplanner
    {
        public long Id { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public string Title { get; set; }
        public long? Createdby { get; set; }
        public DateTime? Createddate { get; set; }
        public long? Modifiedby { get; set; }
        public DateTime? Modifieddate { get; set; }
        public long? IsDelete { get; set; }
    }
}
