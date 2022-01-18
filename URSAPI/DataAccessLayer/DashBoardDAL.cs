using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class DashBoardDAL
    {

        public static FinalResultDTO GetDashBoardData(Int32 userId)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    DashBoardDTO dashBoard = new DashBoardDTO();
                    dashBoard.UserID = userId;

                    var myRequests = (from NR in db.UarRequestmaster
                                         where NR.EmployeeId == userId && NR.Status != "Closed"
                                         orderby NR.Id descending
                                         select new LoadRequestData
                                         {
                                             status = NR.Status
                                         }).ToList();

                    dashBoard.MyRequestCount = myRequests.Count;

                    FinalResultDTO approvalList =  RequestMethodDAL.GetListOfRequestToApprove(userId);

                    if (approvalList.Status)
                    {
                        if (approvalList.ResultOP.Count >= 1)
                        {

                            List<ApprovalRequestDTO> levelAvail = approvalList.ResultOP;
                            dashBoard.ToApproveCount = levelAvail.Count();
                        }

                    }

                    resut.Status = true;
                    resut.ResultOP = dashBoard;
                    return resut;
                }

            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Counts";
                return resut;
            }
        }



    }
}
