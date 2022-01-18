using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;

namespace URSAPI.Controllers
{
   
    public class AuditTrialController : Controller
    {
        private readonly IConfiguration _config;

        public AuditTrialController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        [HttpPost]
        [Route("api/Audit/GetAuditData")]
        public dynamic  GetAuditData([FromBody]AuditTrailFilters trailFilters)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            return AuditTrialDAL.LoadAuditTrialData(trailFilters.Fromdate.Date, trailFilters.Todate.AddDays(1).Date, _timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/Audit/GetRequestData")]
        public dynamic GetRequestData(string  requestid)
        {

            return AuditTrialDAL.LoadAuditTrialData(requestid);
        }


    }
}