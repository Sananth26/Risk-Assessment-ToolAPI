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
    public class WorkFlowController : Controller
    {
        private readonly IConfiguration _config;

        public WorkFlowController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        [Route("api/WorkFlow/LoadWorkFlow")]
        public dynamic LoadWorkFlow()
        {
            return WorkFlowDAL.LoadWorkFlow();
        }
        [HttpGet]
        [Route("api/WorkFlow/LoadWorkFlowDetails")]
        public dynamic LoadWorkFlowDetails()
        {
            return WorkFlowDAL.LoadWorkFlowDetails();
        }


        // [Authorize]
        [HttpPost]
        [Route("api/WorkFlow/LoadWorkFlowDetails")]
        public dynamic LoadWorkFlowDetails([FromBody]UASWorkFlow inputParam)
        {
            return WorkFlowDAL.LoadWorkFlowDetails(inputParam);
        }

        [Authorize]
        [HttpGet]
        [Route("api/WorkFlow/LoadCompleteWorkFlowDetails")]
        public dynamic LoadCompleteWorkFlowDetails(Int32 combinationid)
        {
            return WorkFlowDAL.LoadCompleteWFDataView(combinationid);
        }

        [Authorize]
        [HttpGet]
        [Route("api/WorkFlow/loadRolesAndCategory")]
        public dynamic LoadRolesAndCategory()
        {
            return WorkFlowDAL.LoadRolesAndCategory();
        }

        [Authorize]
        [HttpPost]
        [Route("api/WorkFlow/LoadUsers")]
        public dynamic LoadUsers([FromBody]List<DropDownsDTO> inputParam)
        {
            return WorkFlowDAL.LoadUsers(inputParam);
        }

       [Authorize]
        [HttpPost]
        [Route("api/WorkFlow/LoadSubCategory")]
        public dynamic LoadSubCategory([FromBody]List<DropDownsDTO> inputParam)
        {
            return WorkFlowDAL.LoadSubCategory(inputParam);
        }

        [Authorize]
        [HttpPost]
        [Route("api/WorkFlow/SaveLevels")]
        public dynamic SaveLevels([FromBody]UASWorkFlow inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return WorkFlowDAL.SaveLevels(inputParam,userid, ipdetails,_timezone);
        }
        [HttpPost]
        [Route("api/WorkFlow/SaveLevelsMaster")]
        public dynamic SaveLevelsMaster([FromBody]WorkFlowMasterDTO inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return WorkFlowDAL.SaveLevelsMaster(inputParam, userid, ipdetails,_timezone);
        }

        //[Authorize]
        [HttpPost]
        [Route("api/WorkFlow/SaveLevelsNumber")]
        public dynamic SaveLevelsNumber([FromBody]List<WorkFlowMasterDetails> inputParam)
        {
            return WorkFlowDAL.SaveLevelsNumber(inputParam);
        }

        //[Authorize]
        [HttpGet]
        [Route("api/WorkFlow/loadLevelDetails")]
        public dynamic LoadLevelDetails(Int32 id)
        {
            return WorkFlowDAL.LoadLevelDetails(id);
        }

        //[Authorize]
        [HttpGet]
        [Route("api/WorkFlow/DeleteLevel")]
        public dynamic DeleteLevel(Int32 id)
        {
            return WorkFlowDAL.DeleteLevel(id);
        }

        [HttpPost]
        [Route("api/WorkFlow/DeleteLevelBasedonlevel")]
        public dynamic DeleteLevelBasedonlevel([FromBody]DeleteLevelBase inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();

            return WorkFlowDAL.DeleteLevelBasedonlevel(inputParam, userid, ipdetails);
        }

        // [Authorize]
        [HttpPost]
        [Route("api/WorkFlow/LoadWorkFlowCLDetails")]
        public dynamic LoadWorkFlowCLDetails([FromBody]UASWorkFlowCL inputParam)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            return WorkFlowDAL.LoadWorkFlowCLDetails(inputParam,_timezone);
           
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