using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class WorkingDaysDTO
    {
      
    }

    public class WeekdaysDTO
    {
        public long weekdayId { get; set; }
        public string weekday { get; set; }
        public Boolean selectedFlag { get; set; }
    }

    public class CalendarEventDTO
    {
        public long id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public long? IsDelete { get; set; }
        public string userid { get; set; }

    }
    public class CalendarEventDTO1
    {
        public long id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string userid { get; set; }

    }

    public class DashBoardDTO
    {
        public Int32 UserID { get; set; }
        public Int32 ToApproveCount { get; set; }
        public Int32 MyRequestCount { get; set; }

    }
    public class CalendarEventDTO2
    {
      
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
      

    }
}
