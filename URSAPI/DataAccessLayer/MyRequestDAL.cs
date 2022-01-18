using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class MyRequestDAL
    {
        public static dynamic Loadopenrequest(string status,Int32 userid)
        {
            FinalResultDTO result = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                   List<RequestFormDTO> requestFormDTO = new List<RequestFormDTO>();
                    List<RequestFormDTO> requestFormTempDTO = new List<RequestFormDTO>();
                    if (status.Trim() == "DraftOpen")
                    {
                        requestFormDTO = (from AT in db.RequestForm
                                          join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                          join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                          join USE in db.User on AT.UserId equals USE.Id
                                          where (((AT.Status.Trim() == "Draft"  || AT.Status.Trim() == "Peer Review" )&& AT.UserId == userid))
                                          select new RequestFormDTO
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                              Description = AT.Description,
                                              Categoryname = LO.Key,
                                              Subcategoryname = LOS.Key,
                                              Status = AT.Status,
                                              Assignedto = USE.Email,
                                              Lastupdate =Convert.ToString (AT.LastModifiedDate),
                                              canEdit = Convert.ToBoolean(AT.IsEditing),
                                          }).OrderByDescending(x => x.Requestid).ToList();
                        List<RequestFormDTO> requestFormDTO1 = new List<RequestFormDTO>();
                    }
                    else if (status.Trim() == "closed")
                    {
                        requestFormDTO = (from AT in db.RequestForm
                                          join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                          join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                          join USE in db.User on AT.UserId equals USE.Id
                                          where ((AT.Status.Trim() == "Rejected"|| AT.Status.Trim() == "Withdrawn" || AT.Status.Trim() == "Completed" ) && (AT.UserId == userid))
                                          select new RequestFormDTO
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                              Description = AT.Description,
                                              Categoryname = LO.Key,
                                              Subcategoryname = LOS.Key,
                                              //orgid = AT.Orgid,
                                              //userid = AT.Userid,
                                              Status = AT.Status,
                                              Assignedto = USE.Email,
                                              canEdit = Convert.ToBoolean(AT.IsEditing),
                                              //projectName = org.OrgName,
                                              Lastupdate =Convert.ToString (AT.LastModifiedDate),
                                              // systemRemarks = AT.Systemremarks
                                          }).OrderByDescending(x => x.Requestid).ToList();
                    }
                    else if (status.Trim() == "publishandmanagerapprove")
                    {
                        requestFormTempDTO = new List<RequestFormDTO>();
                        requestFormTempDTO = (from AT in db.RequestForm
                                          join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                          join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                          join USE in db.User on AT.UserId equals USE.Id
                                          where (AT.Status.Trim() == "userpublish"|| AT.Status.Trim() == "Publish" || AT.Status.Trim() == "ITApprove") && ((AT.UserId == userid))
                                          select new RequestFormDTO
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                              Description = AT.Description,
                                              Categoryname = LO.Key,
                                              Subcategoryname = LOS.Key,
                                              //orgid = AT.Orgid,
                                              //userid = AT.Userid,
                                               
                                              Status = AT.Status,
                                              Assignedto = USE.Email,
                                              canEdit = Convert.ToBoolean(AT.IsEditing),
                                              //projectName = org.OrgName,
                                              Lastupdate = Convert.ToString(AT.LastModifiedDate),
                                              // systemRemarks = AT.Systemremarks
                                          }).OrderByDescending(x => x.Requestid).ToList();
                        foreach (var item in requestFormTempDTO)
                        {
                            string sta = "";
                            if (item.Status == "userpublish")
                            { sta = "Published"; }
                            else if (item.Status == "Publish") { sta = "Manager Approved"; }
                            else if (item.Status == "ITApprove")
                            { sta = "Network Approved"; }
                            var rr = new RequestFormDTO
                            {
                                Requestid = item.Requestid,
                                RequestSno = item.RequestSno,
                                Description = item.Description,
                                Categoryname = item.Categoryname,
                                Subcategoryname = item.Subcategoryname,
                                Status = sta,
                                canEdit =true,
                                Assignedto = item.Assignedto,
                                Lastupdate = item.Lastupdate,
                            };
                            requestFormDTO.Add(rr);
                        }
                    }
                    else if (status.Trim() == "Review")
                    {
                        requestFormTempDTO = new List<RequestFormDTO>();
                        var usermail = db.User.Where(x => x.Id == userid).Select(x => x.Email).FirstOrDefault();
                        requestFormTempDTO = (from AT in db.RequestForm
                                          join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                          join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                          //peerreview issue need to check
                                          // join USE in db.User on AT.peerReviewId equals USE.Id
                                          //  where ((AT.Status == "Peer Review")&& (AT.peerReviewId==userid))
                                          where ((AT.Status.Trim() == "Peer Review") )
                                          select new RequestFormDTO
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                              Description = AT.Description,
                                              Categoryname = LO.Key,
                                              Subcategoryname = LOS.Key,
                                              Status = AT.Status,
                                              peerreviewid = AT.PeerReviewId,
                                              canEdit = Convert.ToBoolean(AT.IsEditing),
                                              //  Assignedto = USE.Email,
                                              Lastupdate =Convert.ToString(AT.LastModifiedDate),
                                          }).OrderByDescending(x => x.Requestid).ToList();
                        Int64 peerrevieid = 0;
                        foreach (var item in requestFormTempDTO)
                        {
                            peerrevieid = 0;
                            if (item.peerreviewid != null)
                            {
                                peerrevieid = 0;
                             var   userdto = JsonConvert.DeserializeObject<List<UsersDTO>>(item.peerreviewid);
                                foreach (var item1 in userdto)
                                {
                                    if (userid == item1.Id)
                                    {
                                        item.Assignedto = usermail;
                                        requestFormDTO.Add(item);
                                    }
                                }
                               // 

                            }
                        }
                        
                    }
                    else if (status.Trim() == "manager")
                    {
                        var requestFormDTOtemp = (from AT in db.RequestForm
                                                  join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                                  join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                                  join USE in db.User on AT.UserId equals USE.Id
                                                  join WD in db.Workflowdetails on AT.CategoryId equals WD.CombinationId

                                                  where (AT.Status.Trim() == "Publish")
                                                  select new RequestFormDTO
                                                  {
                                                      Requestid = AT.Requestid,
                                                      RequestSno = AT.RequestSno,
                                                      Description = AT.Description,
                                                      CategoryId = AT.CategoryId,
                                                      Categoryname = LO.Key,
                                                      Subcategoryname = LOS.Key,
                                                      Status = AT.Status,
                                                      Assignedto = USE.Email,
                                                      canEdit = Convert.ToBoolean(AT.IsEditing),
                                                      Lastupdate =Convert.ToString (AT.LastModifiedDate),
                                                  }).ToList();
                        var leveltemp = (from AT in db.Workflowdetails
                                         join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                         join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                         where (WFU.UserId == userid && WFU.LevelId == 2)

                                         select new
                                         {
                                             CombinationId = Convert.ToInt64(AT.CombinationId),
                                             LevelId = Convert.ToInt64(WFL.LevelId),
                                             UserId = Convert.ToInt64(WFU.UserId),
                                         }).ToList();
                        foreach (var item in requestFormDTOtemp)
                        {
                            var exist = leveltemp.Where(x => x.CombinationId == item.CategoryId).FirstOrDefault();
                            if (exist != null)
                            {
                                var rr = new RequestFormDTO
                                {
                                    Requestid = item.Requestid,
                                    RequestSno = item.RequestSno,
                                    Description = item.Description,
                                    Categoryname = item.Categoryname,
                                    Subcategoryname = item.Subcategoryname,
                                    // Status = item.Status,
                                    Status = "Network Approval",
                                    Assignedto = item.Assignedto,
                                    Lastupdate = item.Lastupdate,
                                };
                                requestFormDTO.Add(rr);
                            }
                        }
                    }
                    else if (status.Trim() == "ITApprove")
                    {
                        var requestFormDTOTemp = (from AT in db.RequestForm
                                                 
                                                  join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                                  join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                                  join USE in db.User on AT.UserId equals USE.Id
                                                  join WD in db.Workflowdetails on AT.CategoryId equals WD.CombinationId
                                                  where (AT.Status.Trim() == "ITApprove")
                                                  select new RequestFormDTO
                                                  {
                                                      Requestid = AT.Requestid,
                                                      RequestSno = AT.RequestSno,
                                                      Description = AT.Description,
                                                      Categoryname = LO.Key,
                                                      CategoryId = AT.CategoryId,
                                                      //Status = AT.Status,
                                                      Status = "Security Approval",
                                                      Assignedto = USE.Email,
                                                      canEdit = Convert.ToBoolean(AT.IsEditing),
                                                      Lastupdate =Convert.ToString ( AT.LastModifiedDate),
                                                  }).OrderByDescending(x => x.Requestid).ToList();
                        foreach (var item in requestFormDTOTemp)
                        {
                            var leveltemp = (from AT in db.Workflowdetails
                                             join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                             join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId

                                             where (WFU.UserId == userid && WFU.LevelId == 3)

                                             select new
                                             {
                                                 CombinationId = Convert.ToInt64(AT.CombinationId),
                                                 LevelId = Convert.ToInt64(WFL.LevelId),
                                                 UserId = Convert.ToInt64(WFU.UserId),
                                             }).ToList();

                            var exist = leveltemp.Where(x => x.CombinationId == item.CategoryId).FirstOrDefault();
                            if (exist != null)
                            {
                                var rr = new RequestFormDTO
                                {
                                    Requestid = item.Requestid,
                                    RequestSno = item.RequestSno,
                                    Description = item.Description,
                                    Categoryname = item.Categoryname,
                                    Subcategoryname = item.Subcategoryname,
                                    Status = item.Status,
                                    Assignedto = item.Assignedto,
                                    Lastupdate = item.Lastupdate,
                                };
                                requestFormDTO.Add(rr);
                            }
                        }
                    }
                    else if (status.Trim() == "ITClosed")
                    {
                        var requestFormDTOTemp = (from AT in db.RequestForm
                                                  join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                                  join USE in db.User on AT.UserId equals USE.Id
                                                  join USEE in db.User on userid equals USEE.Id
                                                  join WD in db.Workflowdetails on AT.CategoryId equals WD.CombinationId
                                                  where AT.Status.Trim() == "Rejected" || AT.Status.Trim() == "Completed"
                                                  select new RequestFormDTO
                                                  {
                                                      Requestid = AT.Requestid,
                                                      RequestSno = AT.RequestSno,
                                                      Description = AT.Description,
                                                      Categoryname = LO.Key,
                                                      CategoryId = AT.CategoryId,
                                                      //    Subcategoryname = LOS.Key,
                                                      Status = AT.Status,
                                                      Assignedto = USE.Email,
                                                      Lastupdate = Convert.ToString (AT.LastModifiedDate),
                                                  }).OrderByDescending(x => x.Requestid).ToList();
                        foreach (var item in requestFormDTOTemp)
                        {
                            var leveltemp = (from AT in db.Workflowdetails
                                             join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                             join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId

                                             where (WFU.UserId == userid && WFU.LevelId == 3)

                                             select new
                                             {
                                                 CombinationId = Convert.ToInt64(AT.CombinationId),
                                                 LevelId = Convert.ToInt64(WFL.LevelId),
                                                 UserId = Convert.ToInt64(WFU.UserId),
                                             }).ToList();

                            var exist = leveltemp.Where(x => x.CombinationId == item.CategoryId).FirstOrDefault();
                            if (exist != null)
                            {
                                var rr = new RequestFormDTO
                                {
                                    Requestid = item.Requestid,
                                    RequestSno = item.RequestSno,
                                    Description = item.Description,
                                    Categoryname = item.Categoryname,
                                    Subcategoryname = item.Subcategoryname,
                                    Status = item.Status,
                                    Assignedto = item.Assignedto,
                                    Lastupdate = item.Lastupdate,
                                };
                                requestFormDTO.Add(rr);
                            }
                            
                        }
                    }
                    result.ResultOP = requestFormDTO.OrderByDescending(x => x.Requestid);
                    result.Status = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "GetRequestData";
                errorLog.Userid = userid;
                errorLog.Methodname = "Loadopenrequest";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Description = "Something went wrong, While getting the Record";
                result.Status = false;
                return result;
            }
        }

        public static dynamic Loadpublishrequest(string status, Int32 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RequestFormDTO> requestFormDTO = new List<RequestFormDTO>();
                    List<RequestFormDTO> requestFormTempDTO = new List<RequestFormDTO>();
                    if (status.Trim() == "userpublish")
                    {
                        requestFormDTO = (from AT in db.RequestForm
                                          join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                          join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                          join USE in db.User on AT.UserId equals USE.Id
                                          where (AT.Status.Trim() == "userpublish"  && USE.ImmediateSupervisor == userid)
                                          select new RequestFormDTO
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                              Description = AT.Description,
                                              Categoryname = LO.Key,
                                              Subcategoryname = LOS.Key,
                                              //Status = AT.Status,
                                              Status = "Published",
                                              Assignedto = USE.Email,
                                              Lastupdate = Convert.ToString(AT.LastModifiedDate),
                                              canEdit = Convert.ToBoolean(AT.IsEditing),
                                          }).OrderByDescending(x => x.Requestid).ToList();
                    }
                    result.ResultOP = requestFormDTO;
                    result.Status = true;
                    return result;
                }
              
            }
            catch (Exception ex)
            {

                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "GetRequestData";
                errorLog.Userid = userid;
                errorLog.Methodname = "Loadopenrequest";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Description = "Something went wrong, While getting the Record";
                result.Status = false;
                return result;
            }
        }

        public static dynamic LoadDashBoard(Int64 userId, Int32 userid)
        {
            FinalResultDTO result = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RequestFormDTO> requestFormDTO = new List<RequestFormDTO>();
                    List<RequestFormDTO> requestFormuserpublishDTO = new List<RequestFormDTO>();
                    List<RequestFormDTO> requestFormTempDTO = new List<RequestFormDTO>();
                  
                       var openrequest = (from AT in db.RequestForm
                                          join USE in db.User on AT.UserId equals USE.Id
                                          where ((AT.Status.Trim() == "Draft"  && AT.UserId == userid))
                                          select new  
                                          {
                                              Requestid = AT.Requestid,
                                              RequestSno = AT.RequestSno,
                                          }).OrderByDescending(x => x.Requestid).ToList().Distinct();
                    var peerrevi = (from AT in db.RequestForm
                                       join USE in db.User on AT.UserId equals USE.Id
                                       where ((AT.Status.Trim() == "Peer Review" && AT.UserId == userid))
                                       select new
                                       {
                                           Requestid = AT.Requestid,
                                           RequestSno = AT.RequestSno,
                                       }).OrderByDescending(x => x.Requestid).ToList().Distinct();
                    List<RequestFormDTO> requestFormDTO1 = new List<RequestFormDTO>();

                  var completed = (from AT in db.RequestForm
                                      join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                      join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                      join USE in db.User on AT.UserId equals USE.Id
                                      where ((AT.Status.Trim() == "Rejected" || AT.Status.Trim() == "Withdrawn" || AT.Status.Trim() == "Completed") && (AT.UserId == userid))
                                      select new RequestFormDTO
                                      {
                                          Requestid = AT.Requestid,
                                          RequestSno = AT.RequestSno,
                                      
                                      }).OrderByDescending(x => x.Requestid).ToList();
                    var usermail = db.User.Where(x => x.Id == userid).Select(x => x.Email).FirstOrDefault();
                        requestFormTempDTO = (from AT in db.RequestForm
                                              where ((AT.Status.Trim() == "Peer Review"))
                                              select new RequestFormDTO
                                              {
                                                  Requestid = AT.Requestid,
                                                  RequestSno = AT.RequestSno,
                                                  Status = AT.Status,
                                                  peerreviewid = AT.PeerReviewId,
                                                
                                              }).OrderByDescending(x => x.Requestid).ToList();
                        Int64 peerrevieid = 0;
                        foreach (var item in requestFormTempDTO)
                        {
                            peerrevieid = 0;
                            if (item.peerreviewid != null)
                            {
                                peerrevieid = 0;
                                var userdto = JsonConvert.DeserializeObject<List<UsersDTO>>(item.peerreviewid);
                                foreach (var item1 in userdto)
                                {
                                    if (userid == item1.Id)
                                    {
                                        item.Assignedto = usermail;
                                        requestFormDTO.Add(item);
                                    }
                                }
                            }
                        }
                    Int64 peerreviewcnt = 0;
                    Int64 manageroritcount = 0;
                    Int64 managercount = 0;
                    if (requestFormDTO.Count > 0)
                    {
                        peerreviewcnt = requestFormDTO.Count;
                    }
                    //
                    requestFormDTO = new List<RequestFormDTO>();

                    var usermaill = db.User.Where(x => x.Id == userid).FirstOrDefault();
                    //if (usermaill.ItProcessAccess.Trim() == "Y")
                    {

                        var requestFormDTOTemp = (from AT in db.RequestForm

                                                  join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                                  join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                                  join USE in db.User on AT.UserId equals USE.Id
                                                  join WD in db.Workflowdetails on AT.CategoryId equals WD.CombinationId
                                                  where (AT.Status.Trim() == "ITApprove")
                                                  select new RequestFormDTO
                                                  {
                                                      Requestid = AT.Requestid,
                                                      RequestSno = AT.RequestSno,
                                                      Description = AT.Description,
                                                      Categoryname = LO.Key,
                                                      CategoryId = AT.CategoryId,
                                                      Status = AT.Status,
                                                      Assignedto = USE.Email,
                                                      canEdit = Convert.ToBoolean(AT.IsEditing),
                                                      Lastupdate = Convert.ToString(AT.LastModifiedDate),
                                                  }).OrderByDescending(x => x.Requestid).ToList();
                        requestFormDTO = new List<RequestFormDTO>();
                        foreach (var item in requestFormDTOTemp)
                        {
                            var leveltemp = (from AT in db.Workflowdetails
                                             join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                             join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId

                                             where (WFU.UserId == userid && WFU.LevelId == 3)

                                             select new
                                             {
                                                 CombinationId = Convert.ToInt64(AT.CombinationId),
                                                 LevelId = Convert.ToInt64(WFL.LevelId),
                                                 UserId = Convert.ToInt64(WFU.UserId),
                                             }).ToList();

                            var exist = leveltemp.Where(x => x.CombinationId == item.CategoryId).FirstOrDefault();
                            if (exist != null)
                            {
                                var rr = new RequestFormDTO
                                {
                                  
                                    RequestSno = item.RequestSno,
                                  
                                };
                                requestFormDTO.Add(rr);
                            }
                        }
                        manageroritcount = requestFormDTO.Count();
                        //var itcount = (from AT in db.RequestForm
                        //               where (AT.Status.Trim() == "ITApprove")
                        //               select new
                        //               {
                        //                   Requestid = AT.Requestid,
                        //                   RequestSno = AT.RequestSno,
                        //                   Status = AT.Status,
                        //               }).ToList().Distinct();
                        //manageroritcount = itcount.Count();
                    }
                    //else
                    {
                        //if (usermaill.ManagerAccess.Trim() == "Y")
                        {
                            var requestFormDTOtemp = (from AT in db.RequestForm
                                                      join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                                      join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                                      join USE in db.User on AT.UserId equals USE.Id
                                                      join WD in db.Workflowdetails on AT.CategoryId equals WD.CombinationId

                                                      where (AT.Status.Trim() == "Publish")
                                                      select new RequestFormDTO
                                                      {
                                                          Requestid = AT.Requestid,
                                                          RequestSno = AT.RequestSno,
                                                          Description = AT.Description,
                                                          CategoryId = AT.CategoryId,
                                                          Categoryname = LO.Key,
                                                          Subcategoryname = LOS.Key,
                                                          Status = AT.Status,
                                                          Assignedto = USE.Email,
                                                          canEdit = Convert.ToBoolean(AT.IsEditing),
                                                          Lastupdate = Convert.ToString(AT.LastModifiedDate),
                                                      }).ToList();
                            var leveltemp = (from AT in db.Workflowdetails
                                             join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                             join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                             where (WFU.UserId == userid && WFU.LevelId == 2)

                                             select new
                                             {
                                                 CombinationId = Convert.ToInt64(AT.CombinationId),
                                                 LevelId = Convert.ToInt64(WFL.LevelId),
                                                 UserId = Convert.ToInt64(WFU.UserId),
                                             }).ToList();
                            requestFormDTO = new List<RequestFormDTO>();
                            foreach (var item in requestFormDTOtemp)
                            {
                                var exist = leveltemp.Where(x => x.CombinationId == item.CategoryId).FirstOrDefault();
                                if (exist != null)
                                {
                                    var rr = new RequestFormDTO
                                    {
                                     RequestSno = item.RequestSno,
                                    };
                                    requestFormDTO.Add(rr);
                                }
                            }
                            managercount = requestFormDTO.Count();

                            requestFormuserpublishDTO = (from AT in db.RequestForm
                                              join LO in db.LookupItem on AT.CategoryId equals LO.Id
                                              join LOS in db.LookupSubitem on AT.SubcategoryId equals LOS.Id
                                              join USE in db.User on AT.UserId equals USE.Id
                                              where (AT.Status.Trim() == "userpublish" && USE.ImmediateSupervisor == userid)
                                              select new RequestFormDTO
                                              {
                                                 RequestSno = AT.RequestSno,
                                              }).OrderByDescending(x => x.Requestid).ToList();
                        }
                    }
                    DashBoardCount dashBoardCount = new DashBoardCount();
                    Int64 opencount = 0;
                    Int64 peerrcoun = 0;
 
                    if (openrequest.Count() > 0)
                    { opencount = openrequest.Count(); }
                    if (peerrevi.Count() > 0)
                    { peerrcoun = peerrevi.Count(); }

                    dashBoardCount.opencount = opencount + peerrcoun;
                    dashBoardCount.peerreviewcount = peerreviewcnt;//another person assigned to you
                    dashBoardCount.approvalcount = manageroritcount;//securitycount
                    dashBoardCount.managercount = managercount;//networkcount
                    dashBoardCount.closedcount = completed.Count();//completed count
                    if (requestFormuserpublishDTO.Count > 0)//managercount
                    { dashBoardCount.userpublishcount = requestFormuserpublishDTO.Count(); }
                    else {
                        dashBoardCount.userpublishcount = 0;
                    }
                
                    result.ResultOP = dashBoardCount;
                    result.Status = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Dashboard";
                errorLog.Userid = userid;
                errorLog.Methodname = "LoadDashBoard";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Description = "Something went wrong, While getting the Record";
                result.Status = false;
                return result;
            }
        }
        //ViewRequest 
    }
}
