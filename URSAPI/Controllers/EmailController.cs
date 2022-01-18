using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URSAPI.DataAccessLayer;

namespace URSAPI.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
        [Route("api/Email/Emailapproval")]
        public dynamic SendEmail()
        {
            Int64 userid = 2; string sub = "Request FR-01-2021-00001"; string status = "Approval"; string levelno = "2";
            string reqno = "FR-01-2021-00001";
             return EmailDAL.Emailtest(userid,sub,status,levelno,reqno);
        }
    }
}