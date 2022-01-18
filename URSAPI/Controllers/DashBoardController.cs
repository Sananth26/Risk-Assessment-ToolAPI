using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;

namespace URSAPI.Controllers
{
    public class DashBoardController : Controller
    {

        [HttpGet]
        [Route("api/RequestMethods/getDashBoardData")]
        public FinalResultDTO GetDashBoardData(Int32 userId)
        {
            return DashBoardDAL.GetDashBoardData(userId);
        }


    }
}