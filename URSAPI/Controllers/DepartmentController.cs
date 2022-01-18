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
    public class DepartmentController : Controller
    {
        
        [Authorize]
        [HttpGet]
        [Route("api/Department/LoadDeptDatas")]
        public dynamic LoadDeptDatas()
        {
            return DepartmentMasterDAL.LoadDeptDatas();
        }

        [Authorize]
        [HttpGet]
        [Route("api/Department/LoadDeptForEdit")]
        public dynamic LoadDeptForEdit(Int32 deptId)
        {
            return DepartmentMasterDAL.LoadDeptForEdit(deptId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Department/CreateOrUpdateDpet")]
        public dynamic CreateOrUpdateDpet([FromBody]DepartmentDTO inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            return DepartmentMasterDAL.CreateOrUpdateDept(inputParam,userid, ipdetails);
        }

        [Authorize]
        [HttpGet]
        [Route("api/Department/DeleteDept")]
        public dynamic DeleteDept(Int32 userId)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            return DepartmentMasterDAL.DeleteDept(userId,userid);
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