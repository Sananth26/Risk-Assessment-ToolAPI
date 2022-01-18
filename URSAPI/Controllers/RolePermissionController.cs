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
using URSAPI.DAL;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.Controllers
{
    public class RolePermissionController : Controller
    {
        private readonly IConfiguration _config;

        public RolePermissionController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        [HttpGet]
        [Route("api/RolePermission/LoaduserRole")]
        public dynamic LoaduserRole(Int32 roleId)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            return RolePermissionDAL.LoadRolePermission(Convert.ToInt32( roleId), _timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/RolePermission/LoadRole")]
        public dynamic LoadRole()
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            return RolePermissionDAL.LoadRole(_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RolePermission/CreateorUpdatePermission")]
        public dynamic CreateorUpdateRolePermission([FromBody] List<RolePermissionDTO> rolePermission)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return RolePermissionDAL.CreateorUpdatePermission(rolePermission,Startup.Orgid , userid, ipdetails,_timezone);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RolePermission/CreateorUpdateRole")]
        public dynamic CreateorUpdateRole([FromBody] RolesDTO role)
        {
            Int64 userid =Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return RolePermissionDAL.CreateorUpdateRole(role, Startup.Orgid, userid, ipdetails,_timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/RolePermission/Roledelete")]
        public FinalResultDTO Roledelete(Int32 id)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            return RolePermissionDAL.Roledelete(id,_timezone);
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