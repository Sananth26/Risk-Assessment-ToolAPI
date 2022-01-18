using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace URSAPI.Controllers
{
  
    public class IPController : Controller
    {
      
        public IActionResult Index()
        {
          
            return View();
        }
        private readonly IHttpContextAccessor httpContextAccessor;
        public   IPController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Content(this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
        }
    }
}