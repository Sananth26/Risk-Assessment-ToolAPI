using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;

namespace URSAPI.Controllers
{
    public class LookUPMasterController : Controller
    {
        private readonly IConfiguration _config;

        public LookUPMasterController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        [HttpGet]
        [Route("api/LookUPMaster/getLookUpMaster")]
        public FinalResultDTO GetLockUpMaster()
        {
            return LookUPMasterDAL.GetLockUpMaster();
        }

        [Authorize]
        [HttpGet]
        [Route("api/LookUPMaster/getLookupItems")]
        public FinalResultDTO GetLookupItems(Int32 categoryId)
        {
            return LookUPMasterDAL.GetLookupItems(categoryId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/LookUPMaster/getLookupSubItems")]
        public FinalResultDTO GetLookupSubItems(Int32 subCategoryId)
        {
            return LookUPMasterDAL.GetLookupSubItems(subCategoryId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/LookUPMaster/LookUpDeleteData")]
        public FinalResultDTO LookUpDeleteData(Int32 id, string type)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return LookUPMasterDAL.LookUpDeleteData(id, type, userid, ipdetails,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/LookUPMaster/CreateLookUpMaster")]
        public FinalResultDTO CreateLookUpMaster([FromBody]AddNewCategory inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _timezone = _config.GetValue<string>("TimeZone");
            return LookUPMasterDAL.CreateLookupMaster(inputParam, userid,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/LookUPMaster/CreateLookUpItem")]
        public FinalResultDTO insertLookUpItem([FromBody]LookUpItemDTO inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return LookUPMasterDAL.InsertLookUpItem(inputParam, userid,ipdetails,_timezone);

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