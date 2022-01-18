using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;

namespace URSAPI.Controllers
{
    public class RequestMethodsController : Controller
    {
      
        [Authorize]
        [HttpGet]
        [Route("api/RequestMethods/getMyRequest")]
        public FinalResultDTO GetListOfMyRequest(Int32 Id)
        {
            return RequestMethodDAL.getListOfMyRequest(Id);
        }

        //Approve Request screens API
        [Authorize]
        [HttpPost]
        [Route("api/RequestMethods/getRequestApproval")]
        public FinalResultDTO GetRequestIdDetailsForApproval([FromBody]LoadRequestData requestDetails)
        {
            return RequestMethodDAL.GetRequestIdDetailsForApproval(requestDetails);
        }

        [HttpGet]
        [Route("api/RequestMethods/getApproveRequest")]
        public FinalResultDTO GetListToRequest(Int32 userID)
        {
            return RequestMethodDAL.GetListOfRequestToApprove(userID);
        }

        [HttpPost]
        [Route("api/RequestMethods/SaveDecision")]
        public FinalResultDTO SaveDecision([FromForm]ApproveRejectRequest requestDetails)
        {
            var ipdetails = Getip();
            return RequestMethodDAL.SaveDecision(requestDetails, ipdetails[0],ipdetails[1]);
        }

        //Stepper related API
        [Authorize]
        [HttpGet]
        [Route("api/RequestMethods/FetchStepperDetails")]
        public FinalResultDTO FetchStepperDetails(Int32 RequestMasterID)
        {
            return RequestMethodDAL.FetchStepperDetails(RequestMasterID);
        }

        [HttpPost]
        [Route("api/RequestMethods/FetchStepperDetailsApproval")]
        public FinalResultDTO FetchStepperDetailsApproval([FromBody]LoadRequestData requestDetails)
        {
            return RequestMethodDAL.FetchStepperDetailsApproval(requestDetails);
        }

        // For Approval Screen
        [HttpPost]
        [Route("api/RequestMethods/getRequestInfo")]
        public FinalResultDTO getRequestInfo([FromBody]RequestInfoDTO requestInfoDTO)
        {
            return RequestMethodDAL.getRequestInfo(requestInfoDTO);
        }

        [HttpPost]
        [Route("api/RequestMethods/getRequestITInfo")]
       
        public FinalResultDTO getRequestITInfo([FromBody]SubCategoryList requestInfoDTO)
        {
            return RequestMethodDAL.getRequestITInfo(requestInfoDTO);
        }


        private readonly IHttpContextAccessor httpContextAccessor;
        public RequestMethodsController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public   IActionResult Get()
        {
            return Content(this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
        }


        public  dynamic Getip()
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