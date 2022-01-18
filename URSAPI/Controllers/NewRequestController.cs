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
    public class NewRequestController : Controller
    {
        private readonly IConfiguration _config;

        public NewRequestController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        [HttpGet]
        [Route("api/NewRequest/LoadDropDowns")]
        public FinalResultDTO LoadDropDowns()
        {
            return NewRequestDAL.LoadDropDowns();
        }

        [Authorize]
        [HttpGet]
        [Route("api/NewRequest/FillUserDetails")]
        public FinalResultDTO FillUserDetails(Int32 userId)
        {

            return NewRequestDAL.FillUserDetails(userId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/NewRequest/ViewRequestForEdit")]
        public FinalResultDTO ViewRequestForEdit(Int32 requestID)
        {
            return NewRequestDAL.ViewRequestForEdit(requestID);
        }

        [HttpGet]
        [Route("api/NewRequest/ViewRequest")]
        public FinalResultDTO ViewRequest(Int32 requestID)
        {
            return NewRequestDAL.ViewRequest(requestID);
        }

        // [Authorize]
        [HttpPost]
        [Route("api/NewRequest/CreateRequest")]
        public FinalResultDTO CreateRequest(NewRequestDataAttachement input)
        {
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return NewRequestDAL.CreateRequest(input,ipdetails,_timezone);
        }

        [HttpPost]
        [Route("api/NewRequest/DeleteData")]
        public dynamic DeleteData([FromForm]DeleteRequest deleteData)
        {
            var ipdetails = Getip();
            return NewRequestDAL.DeleteData(deleteData,ipdetails);
        }

        [HttpGet]
        [Route("api/NewRequest/getRequestDataForDelete")]
        public FinalResultDTO getRequestDataForDelete(Int32 ReqId)
        {
            return NewRequestDAL.getRequestDataForDelete(ReqId);
        }

        [HttpGet]
        [Route("api/NewRequest/FetchFile")]
        public dynamic FetchFile(string path)
        {
            return NewRequestDAL.FetchFile(path);
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