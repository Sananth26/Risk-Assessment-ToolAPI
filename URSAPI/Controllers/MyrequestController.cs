using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.Controllers
{
    public class MyrequestController : Controller
    {
        private readonly IConfiguration _config;

        public MyrequestController(IConfiguration config)
        {
            _config = config;
        }
        // [Authorize]
        [HttpGet]
        [Route("api/Myrequest/Myrequest")]
        public dynamic Myrequest(string status)
        {
            try
            {
                Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                //string _timezone = _config.GetValue<string>("TimeZone");
                return MyRequestDAL.Loadopenrequest(status, Convert.ToInt32(userid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/Myrequest/Publishrequest")]
        public dynamic Publishrequest(string status)
        {
            try
            {
                Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                string _timezone = _config.GetValue<string>("TimeZone");
                return MyRequestDAL.Loadpublishrequest(status, Convert.ToInt32(userid),_timezone);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/Myrequest/DashBoard")]
        public dynamic DashBoard(Int64 userId)
        {
            try
            {
                Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                
             //   PasswordRequiredLength = _config.GetValue<int>(               "AppIdentitySettings:Password:RequiredLength")
                return MyRequestDAL.LoadDashBoard(userId, Convert.ToInt32(userid));
            }
            catch (Exception ex)
            {
               throw ex;
            }
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