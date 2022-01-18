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
using static URSAPI.ModelDTO.Remediation;

namespace URSAPI.Controllers
{
    public class RequestFormUserController : Controller
    {
        private readonly IConfiguration _config;

        public RequestFormUserController(IConfiguration config)
        {
            _config = config;
        }

        //[Authorize] don't use Anthroize
        [HttpPost]
        [Route("api/RequestFormUser/InsertRequest")]
        public FinalResultDTO InsetRequest(NewUserRequestDataAttachement input)
        {
            var ipdetails = Getip();
             string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            // string _url = "";
            return UserAndItRequestForm.CreateRequest(input, ipdetails,_url,_timezone);
        }

        [HttpPost]
        [Route("api/RequestFormUser/InsertRequestRiskandRank")]
        public dynamic InsertRequestRiskandRank([FromBody]ApproveDTO input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
           string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.InsertRequestRiskandRank(input, userid,ipdetails,_url,_timezone);
        }
        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/LoadDropDowns")]
        public FinalResultDTO LoadDropDowns()
        {
           return UserAndItRequestForm.LoadDropDowns();
        }

        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/EditRequestLoadonId")]
        public FinalResultDTO EditRequestLoadonId(Int32 reqid)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.EditRequestLoadonId(reqid,userid,_timezone);
        }
        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/UnlockRequest")]
        public FinalResultDTO UnlockRequest(Int32 reqid)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.UnlockRequest(reqid, userid,_timezone);
        }

        [HttpGet]
        [Route("api/RequestFormUser/FetchFile")]
        public dynamic FetchFile(string path)
        {
            return UserAndItRequestForm.FetchFile(path);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/ToChangePeerReview")]
        public dynamic ToChangePeerReview([FromBody]PeerreviewList input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
             string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ToChangePeerReview(input, ipdetails,userid,_url,_timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/DraftToPublish")]
        public FinalResultDTO DraftToPublish(Int32 id)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
             string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.RequestDraftToPublish(id,ipdetails,userid,_url,_timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/RequestUnlock")]
        public FinalResultDTO RequestUnlock(Int32 id)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.RequestUnlock(id, ipdetails, userid,_url,_timezone);
        }


        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/PublishTOIT")]
        public dynamic PublishTOIT([FromBody]SubCategoryList input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
           string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ManagerTOIT(input, ipdetails,userid,_url,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/ManagerTONetwork")]
        public dynamic ManagerTONetwork([FromBody]SubCategoryList input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ManagerTONetwork(input, ipdetails, userid, _url,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/ITApprove")]
        public dynamic ITApprove([FromBody]ApproveDTO input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
             string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ITApprove(input, ipdetails,userid,_url,_timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/RequestFormUser/Withdraw")]
        public dynamic Withdraw(Int32 id)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
              string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.RequestWithdraw(id, ipdetails, userid,_url,_timezone);
        }
       
        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/Stepper")]
        public dynamic ITApprove([FromBody]DropDownsDTO input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.StepperBasedOnRequest(input, ipdetails, userid,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/ManagerPermission")]
        public dynamic ManagerPermission([FromBody]SubCategoryList input)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ManagerPermission(input,  userid,_timezone);
        }

        [HttpPost]
        [Route("api/RequestFormUser/RemediationDiarySaveandUpdate")]
        public dynamic RemediationDiarySaveandUpdate([FromBody]RemediationDTOList input)
        {
            var ipdetails = Getip();
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.RemediationDiarySaveandUpdate(input, userid, ipdetails, _url,_timezone);
        }

        [HttpPost]
        [Route("api/RequestFormUser/RemediationDiaryLoad")]
        public dynamic RemediationDiaryLoad([FromBody]RemediationParams input)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.RemediationDiaryLoad(input, userid,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/RemediationLikeUpdate")]
        public dynamic RemediationLikeUpdate([FromBody]RemediationCheckedandlike input)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ToChangeLikeStatus(input, userid,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RequestFormUser/RemediationStrikeUpdate")]
        public dynamic ToChangeStrikeStatus([FromBody]RemediationCheckedandlike input)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            string _url = _config.GetValue<string>("URL");
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserAndItRequestForm.ToChangeStrikeStatus(input, userid,_timezone);
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
