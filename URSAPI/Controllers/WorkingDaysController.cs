using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;

namespace URSAPI.Controllers
{
    public class WorkingDaysController : Controller
    {
        [Authorize]
        [HttpGet]
        [Route("api/WorkingDays/Weekdays")]
        public dynamic GetAuditData()
        {
            return WorkingDaysDAL.WeekDays();
        }

        [Authorize]
        [HttpPost]
        [Route("api/WorkingDays/Weekdays")]
        public dynamic GetAuditData([FromBody] List<WeekdaysDTO> weekdays)
        {
            return WorkingDaysDAL.WeekDaysSave(weekdays);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Workingdays/Holidayplanner")]
        public dynamic Getholidayplanner([FromBody] CalendarEventDTO weekdays)
        {
            var ipdetails = Getip();
            return WorkingDaysDAL.HolidaySave(weekdays, ipdetails);
        }

        [Authorize]
        [HttpPost]
        [Route("api/DeleteHoliday/DeleteData")]
        public dynamic GetDeleteData([FromBody] CalendarEventDTO weekdays)
        {
            var ipdetails = Getip();
            return WorkingDaysDAL.DeleteData(weekdays,ipdetails);
        }

        [Authorize]
        [HttpGet]
        [Route("api/WorkingDays/holidays")]
        public dynamic GetHolidayData()
        {
            return WorkingDaysDAL.Holidays();
        }

        public dynamic Getip()
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
            string userAgent = Request.Headers?.FirstOrDefault(s => s.Key.ToLower() == "user-agent").Value;
            var ua = YauaaSingleton.Analyzer.Parse(userAgent);
            var browserName = ua.Get(UserAgent.AGENT_NAME).GetValue();
            var version = ua.Get(UserAgent.AGENT_NAME_VERSION_MAJOR).GetValue();
            string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();

            //127.0.0.1    localhost
            //::1          localhost
            if (ip == "::1")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ipa in host.AddressList)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = ipa.ToString();
                    }
                }
            }
            List<string> output = new List<string>();
            string content = "";
            content = version + " , " + System.Environment.MachineName;
            output.Add(content);
            content = "";
            content = ip;
            output.Add(content);
            return output;
        }

    }
}