using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Ubiety.Dns.Core;
using URSAPI.ModelDTO;
using URSAPI.Models;
 using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.Controllers;

namespace URSAPI.DataAccessLayer
{
    public class NewRequestDAL
    {
        public static dynamic LoadDropDowns()
        {
            FinalResultDTO result = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.CategoryDetails = new ExpandoObject();
            resultDTO.SubCategoryDetails = new ExpandoObject();
            resultDTO.AccessTypeDetails = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();
            resultDTO.DecisionDetails = new ExpandoObject();

            try
            {
                var AccessCategory = LookUPMasterDAL.GetLookupMaster("Access Category");
                if (AccessCategory.Status)
                {
                    resultDTO.CategoryDetails = AccessCategory.ResultOP;
                }


                var AccessType = LookUPMasterDAL.GetLookupMaster("Access Type");
                if (AccessType.Status)
                {
                    resultDTO.AccessTypeDetails = AccessType.ResultOP;
                }
                var Decision = LookUPMasterDAL.GetLookupMaster("Decisions");
                if (Decision.Status)
                {
                    resultDTO.DecisionDetails = Decision.ResultOP;
                }

                result.ResultOP = resultDTO;
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Description = "Something went wrong, While getting the Record";
                result.Status = false;
                return result;
            }
        }

        //ViewRequest
        public static FinalResultDTO ViewRequest(long requestID)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from NR in db.UarRequestmaster
                                         join user in db.User on NR.EmployeeId equals user.Id
                                         join dept in db.Department on user.Department equals dept.Id
                                         where NR.Id == requestID
                                         select new MyRequestDTO
                                         {
                                             Id = NR.Id,
                                             UserId = NR.EmployeeId,
                                             Department = dept.DepartmentName,
                                             Name = user.FirstName,
                                             Location = user.FirstName,
                                             Email = user.Email,
                                             RequestDate = Convert.ToString(NR.RequestDate),
                                             Attachment = NR.Attachment,
                                             Remarks = NR.Remarks,
                                             WorkFlowSatge = NR.WorkflowStage,
                                             Status = NR.Status,
                                             Level = NR.Level
                                         }).FirstOrDefault();

                    List<BulkRequestDTo> DetailsTable = new List<BulkRequestDTo>();
                    if (templateNames != null)
                    {

                        DetailsTable = (from NR in db.UarRequestDetails
                                        join NRF in db.UarRequestmaster on NR.RequestId equals NRF.Id
                                        join user in db.User on NRF.EmployeeId equals user.Id
                                        join lookupItem in db.LookupItem on NR.AccessCategoryId equals lookupItem.Id
                                        join lookupsub in db.LookupSubitem on NR.SubCategoryId equals lookupsub.Id
                                        join lookupItemaccess in db.LookupItem on NR.AccessTypeId equals lookupItemaccess.Id
                                        join userDll in db.User on NR.UserId equals userDll.Id
                                        join dept in db.Department on user.Department equals dept.Id
                                        where NR.RequestId == requestID 
                                        select new BulkRequestDTo
                                        {
                                            Id = NR.Id,
                                            UserId = NRF.EmployeeId,
                                            Department = dept.DepartmentName,
                                            Name = user.FirstName,
                                            RequestId = NRF.Id,
                                            AccessCategoryId = NR.AccessCategoryId,
                                            AccessTypeId = NR.AccessTypeId,
                                            SubCategoryId = NR.SubCategoryId,
                                            AccessCategory = lookupItem.Value,
                                            SubCategory = lookupsub.Value,
                                            AccessType = lookupItemaccess.Value,
                                            UserName = userDll.FirstName,
                                            existingRecord = true,
                                            RequestDetailsId = NR.RequestDetailsId ?? "",
                                            WorkFlowSatge = NR.WorkflowStage,
                                            Status = NR.Status,
                                            level = NR.Level,
                                            SlaData = NR.SlaData
                                        }).ToList();

                        foreach (var item in DetailsTable)
                        {
                            string users = string.Empty;
                            if (item.Status == "Processing" || item.Status == "Open")
                            {
                                var SLA = JsonConvert.DeserializeObject<SLADetails>(item.SlaData);
                                foreach (var USR in SLA.Users)
                                {
                                    users = users + USR.key + ", ";
                                }
                                users = users.Remove(users.Length - 2);
                            }
                            item.AssignedTo = users;
                        }
                    }
                    templateNames.BulkRequest = DetailsTable;

                    resut.Status = true;
                    resut.ResultOP = templateNames;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }


        public static FinalResultDTO ViewRequestForEdit(long requestID)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from NR in db.UarRequestmaster
                                         join user in db.User on NR.EmployeeId equals user.Id
                                         join dept in db.Department on user.Department equals dept.Id
                                         where NR.Id == requestID
                                         select new MyRequestDTO
                                         {
                                             Id = NR.Id,
                                             UserId = NR.EmployeeId,
                                             Department = dept.DepartmentName,
                                             Name = user.FirstName,
                                             Location = user.FirstName,
                                             Email = user.Email,
                                             RequestDate = Convert.ToString(NR.RequestDate),
                                             WorkFlowSatge = NR.WorkflowStage,
                                             Status = NR.Status,
                                             Level = NR.Level
                                         }).FirstOrDefault();

                    List<BulkRequestDTo> DetailsTable = new List<BulkRequestDTo>();
                    if (templateNames != null && templateNames.WorkFlowSatge == "Department" && (templateNames.Status == "Open" || templateNames.Status == "Rejected"))
                    {
                        DetailsTable = (from NR in db.UarRequestDetails
                                        join NRF in db.UarRequestmaster on NR.RequestId equals NRF.Id
                                        join user in db.User on NRF.EmployeeId equals user.Id
                                        join lookupItem in db.LookupItem on NR.AccessCategoryId equals lookupItem.Id
                                        join lookupsub in db.LookupSubitem on NR.SubCategoryId equals lookupsub.Id
                                        join lookupItemaccess in db.LookupItem on NR.AccessTypeId equals lookupItemaccess.Id
                                        join userDll in db.User on NR.UserId equals userDll.Id
                                        join dept in db.Department on user.Department equals dept.Id
                                        where NR.RequestId == requestID && NR.DeleteFlag != "Y"
                                        select new BulkRequestDTo
                                        {
                                            Id = NR.Id,
                                            UserId = NRF.EmployeeId,
                                            Department = dept.DepartmentName,
                                            Name = user.FirstName,
                                            RequestId = NRF.Id,
                                            AccessCategoryId = NR.AccessCategoryId,
                                            AccessTypeId = NR.AccessTypeId,
                                            SubCategoryId = NR.SubCategoryId,
                                            AccessCategory = lookupItem.Value,
                                            SubCategory = lookupsub.Value,
                                            AccessType = lookupItemaccess.Value,
                                            UserName = userDll.FirstName,
                                            level = NR.Level,
                                            existingRecord = true,
                                            RequestDetailsId = NR.RequestDetailsId ?? ""

                                        }).ToList();
                    }
                    else
                    {
                        DetailsTable = (from NR in db.UarRequestDetails
                                        join NRF in db.UarRequestmaster on NR.RequestId equals NRF.Id
                                        join user in db.User on NRF.EmployeeId equals user.Id
                                        join lookupItem in db.LookupItem on NR.AccessCategoryId equals lookupItem.Id
                                        join lookupsub in db.LookupSubitem on NR.SubCategoryId equals lookupsub.Id
                                        join lookupItemaccess in db.LookupItem on NR.AccessTypeId equals lookupItemaccess.Id
                                        join userDll in db.User on NR.UserId equals userDll.Id
                                        join dept in db.Department on user.Department equals dept.Id
                                        where NR.RequestId == requestID && NR.DeleteFlag != "Y" && 
                                        NR.WorkflowStage == "Department" && (NR.Status == "Open" || NR.Status == "Rejected") 
                                        select new BulkRequestDTo
                                        {
                                            Id = NR.Id,
                                            UserId = NRF.EmployeeId,
                                            Department = dept.DepartmentName,
                                            Name = user.FirstName,
                                            RequestId = NRF.Id,
                                            AccessCategoryId = NR.AccessCategoryId,
                                            AccessTypeId = NR.AccessTypeId,
                                            SubCategoryId = NR.SubCategoryId,
                                            AccessCategory = lookupItem.Value,
                                            SubCategory = lookupsub.Value,
                                            AccessType = lookupItemaccess.Value,
                                            UserName = userDll.FirstName,
                                            level = NR.Level,
                                            existingRecord = true,
                                            RequestDetailsId = NR.RequestDetailsId ?? ""

                                        }).ToList();
                    }
                    templateNames.BulkRequest = DetailsTable;

                    resut.Status = true;
                    resut.ResultOP = templateNames;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static FinalResultDTO CreateRequest(NewRequestDataAttachement input,dynamic Ipdetails,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
           
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            List<AuditTrialJSONDTO> auditdto = new List<AuditTrialJSONDTO>();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            List<AccessType> accessType = new List<AccessType>();
            try
            {
                var newRequestData = JsonConvert.DeserializeObject<MyRequestDTO>(input.NewRequestData);
                List<AttachmentTable> staticAttachment = JsonConvert.DeserializeObject<List<AttachmentTable>>(input.StaticAttachmentsTable);

                using (dbURSContext db = new dbURSContext())
                {
                    var newRequestDetails = new UarRequestmaster();
                                       
                   
                    string content = "";

                    // Checking for WorkFLow Exist for perticular category.
                    foreach (var item in newRequestData.BulkRequest)
                    {
                        var wfData = db.Workflowcategory.Where(x => x.CategoryId == item.AccessCategoryId && x.SubCategoryId == item.SubCategoryId).ToList();
                        if (wfData.Count == 0)
                        {
                            resut.Description = "Workflow is not Created for Category: " + item.AccessCategory + " and Sub-Category: " + item.SubCategory + ". Please contact admin for further details.";
                            resut.Status = false;
                            return resut;
                        }
                    }

                    string attachmentlist = "";

                    var userDetails = db.User.Where(x => x.Id == newRequestData.UserId).FirstOrDefault();
                    FinalResultDTO levelWiseUsers = new FinalResultDTO();
                    List<UserLevels> userLevels = new List<UserLevels>();

                    if (newRequestData.Id == 0)
                    {
                        levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForUSer(Convert.ToInt32(newRequestData.UserId), 1);
                        if (levelWiseUsers.Status)
                        {
                            userLevels = levelWiseUsers.ResultOP;
                        }

                        List<ApproveDetailsJson> approveDetail = new List<ApproveDetailsJson>
                        {
                           new ApproveDetailsJson
                           {
                            Id = 0,
                            approverDatetime = DateTime.Now,
                            Level = 0,
                            approverId = userDetails.Id,
                            approverName = String.Concat(userDetails.FirstName, " ", userDetails.LastName),
                            status = "Created",
                            Remark = newRequestData.Remarks
                           }
                        };
                        string WorkflowStage = string.Empty;
                        string Status = string.Empty;
                        SLADetails slaData = new SLADetails();
                        if (userLevels.Count >= 1)
                        {
                            WorkflowStage = "Department";
                            Status = "Open";

                            FinalResultDTO SlaDataOBJ = CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                            if (SlaDataOBJ.Status)
                            {
                                slaData = SlaDataOBJ.ResultOP;
                            }
                        }
                        else
                        {
                            WorkflowStage = "IT";
                            Status = "Processing";
                            approveDetail.Add(
                                 new ApproveDetailsJson
                                 {
                                     Id = 0,
                                     approverDatetime = DateTime.Now,
                                     Level = 0,
                                     approverId = userDetails.Id,
                                     approverName = String.Concat(userDetails.FirstName, " ", userDetails.LastName),
                                     status = "deptComplete",
                                 });
                        }

                        newRequestDetails = new UarRequestmaster
                        {
                            EmployeeId = newRequestData.UserId,
                            CreatedBy =Convert.ToInt32( newRequestData.UserId),
                            RequestDate = DateTime.Now,
                            Remarks = newRequestData.Remarks,
                            WorkflowStage = WorkflowStage,
                            Status = Status,
                            Level = 1,
                            ApprovalDetails = JsonConvert.SerializeObject(approveDetail),
                            SlaData = JsonConvert.SerializeObject(slaData)
                        };
                        db.UarRequestmaster.Add(newRequestDetails);
                        db.SaveChanges();

                        Int32 SubRequestsUniqueID = 0;
                        foreach (var item in newRequestData.BulkRequest)
                        {
                            SubRequestsUniqueID = SubRequestsUniqueID + 1;

                            if (WorkflowStage == "IT")
                            {
                                userLevels = new List<UserLevels>();
                                levelWiseUsers = new FinalResultDTO();
                                levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForIT(1, Convert.ToInt32(item.AccessCategoryId), Convert.ToInt32(item.SubCategoryId));
                                if (levelWiseUsers.Status)
                                {
                                    userLevels = levelWiseUsers.ResultOP;
                                }

                                slaData = new SLADetails();
                                if (userLevels.Count >= 1)
                                {
                                    FinalResultDTO SlaDataOBJ = CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                                    if (SlaDataOBJ.Status)
                                    {
                                        slaData = SlaDataOBJ.ResultOP;
                                    }
                                }
                            }

                            var RequestDetails = new UarRequestDetails();
                            var accty = new AccessType();
                            RequestDetails = new UarRequestDetails
                            {
                                RequestId = newRequestDetails.Id,
                                RequestDetailsId = String.Concat(newRequestDetails.Id, "-", SubRequestsUniqueID),
                                AccessCategoryId = item.AccessCategoryId,
                                SubCategoryId = item.SubCategoryId,
                                AccessTypeId = item.AccessTypeId,
                                UserId = item.UserIdDDL,
                                CreatedBy = newRequestData.UserId,
                                Level = 1,
                                DateOfRevoke = DateTime.Now,
                                WorkflowStage = WorkflowStage,
                                Status = Status,
                                DeleteFlag = "N",
                                SlaData = JsonConvert.SerializeObject(slaData),
                            };
                            db.UarRequestDetails.Add(RequestDetails);
                            db.SaveChanges();

                            //for audit trial
                            List<string> output = new List<string>();
                            var acccategory = db.LookupItem.Where(x => x.Id == item.AccessCategoryId).Select(x => x.Key).FirstOrDefault();
                            var subcategory1 = db.LookupSubitem.Where(x => x.Id == item.SubCategoryId).Select(x => x.Key).FirstOrDefault();
                            var accctype = db.LookupItem.Where(x => x.Id == item.AccessTypeId).Select(x => x.Key).FirstOrDefault();
                            var usernam = db.User.Where(x => x.Id == item.UserIdDDL).Select(x => x.FirstName).FirstOrDefault();
                            var entryper = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();

                            accty.accesscategory = acccategory;
                            accty.accesscategoryid = item.AccessCategoryId;
                            accty.accesstype = accctype;
                            accty.accesstypeid = item.AccessTypeId;
                            accty.subcategory = subcategory1;
                            accty.subcategoryid = item.SubCategoryId;
                            accty.username = usernam;
                            accty.userid = item.UserIdDDL;
                            accty.requestdetailid = RequestDetails.Id;
                            
                            content = "";
                            content = "Sub-requestid= " + String.Concat(newRequestDetails.Id, "-", SubRequestsUniqueID) + "  is created.";
                            output.Add(content);
                            content = "";
                            content = "Access Category=  " + acccategory + ",  Sub Category=  " + subcategory1 + ",  Access Type=  " + accctype + ", User Name=  " + usernam +"";
                            output.Add(content);
                            content = "Current Workflow Stage=  " + WorkflowStage + ", Current Level= " + 1 + ", Status=  " + Status;//oldstatus
                            output.Add(content);
                            accty.categorydescription = output;
                            accessType.Add(accty);
                        }

                        //for audit trial
                        var usernamee = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                        masauditdto.browser = Ipdetails[0];
                        masauditdto.eventname = "Create";
                        masauditdto.module = "New Request";
                        masauditdto.userid = newRequestData.UserId;
                        masauditdto.ipaddress = Ipdetails[1];
                        auditjsonmdl.accesstype = accessType;
                        // auditjsonmdl.categorydescription = "Access Category=" + accesscategory[0].name + ",Sub Category=" + subcategory[0].name + ",Access Type=" + accestype [0].name+"User Name=" +userdetails[0].name;
                        auditjsonmdl.requestId = Convert.ToString(newRequestDetails.Id);
                        masauditdto.Systemremarks = newRequestData.Remarks;
                        masauditdto.RequestId = Convert.ToString(newRequestDetails.Id);
                        auditjsonmdl.browser = Ipdetails[0];
                        auditjsonmdl.createddate = DateTime.Now;
                        auditjsonmdl.eventname = "Create";
                        auditjsonmdl.module = "New Request ";
                        auditjsonmdl.userid = newRequestData.UserId;
                        auditjsonmdl.username = usernamee;
                        auditjsonmdl.ipaddress = Ipdetails[1];
                        auditjsonmdl.Systemremarks = newRequestData.Remarks;
                    }
                    else
                    {
                        //update existing record
                        newRequestDetails = db.UarRequestmaster.Where(x => x.Id == newRequestData.Id).FirstOrDefault();
                        var listOfSubRequests = db.UarRequestDetails.Where(x => x.RequestId == newRequestData.Id).ToList();
                        Int32 countOfSerialNumber = listOfSubRequests.Count;

                        string WorkflowStage = string.Empty;
                        string Status = string.Empty;
                        SLADetails slaData = new SLADetails();

                        levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForUSer(Convert.ToInt32(newRequestData.UserId), 1);
                        if (levelWiseUsers.Status)
                        {
                            userLevels = levelWiseUsers.ResultOP;
                        }

                        if (userLevels.Count >= 1)
                        {
                            WorkflowStage = "Department";
                            Status = "Processing";

                            FinalResultDTO SlaDataOBJ = CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                            if (SlaDataOBJ.Status)
                            {
                                slaData = SlaDataOBJ.ResultOP;
                            }
                        }
                        else
                        {
                            WorkflowStage = "IT";
                            Status = "Processing";
                        }

                        if (newRequestDetails.WorkflowStage == "Department")
                        {
                            newRequestDetails.WorkflowStage = WorkflowStage;
                            newRequestDetails.Status = Status;
                            newRequestDetails.Level = 1;
                            newRequestDetails.SlaData = JsonConvert.SerializeObject(slaData);
                            db.SaveChanges();
                        }

                        bool existnotchange = false;
                        foreach (var item in newRequestData.BulkRequest)
                        {
                            if (WorkflowStage == "IT")
                            {
                                userLevels = new List<UserLevels>();
                                levelWiseUsers = new FinalResultDTO();
                                levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForIT(1, Convert.ToInt32(item.AccessCategoryId), Convert.ToInt32(item.SubCategoryId));
                                if (levelWiseUsers.Status)
                                {
                                    userLevels = levelWiseUsers.ResultOP;
                                }

                                slaData = new SLADetails();
                                if (userLevels.Count >= 1)
                                {
                                    FinalResultDTO SlaDataOBJ = CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                                    if (SlaDataOBJ.Status)
                                    {
                                        slaData = SlaDataOBJ.ResultOP;
                                    }
                                }
                            }

                            var accty = new AccessType();
                            if (item.existingRecord != true)
                            {
                                countOfSerialNumber = countOfSerialNumber + 1;
                                List<ApproveDetailsJson> approveDetail = new List<ApproveDetailsJson>();

                                if (newRequestDetails.WorkflowStage == "IT")
                                {
                                   approveDetail = new List<ApproveDetailsJson>
                                   {
                                     new ApproveDetailsJson
                                     {
                                         Id = 0,
                                         approverDatetime = DateTime.Now,
                                         Level = 0,
                                         approverId = userDetails.Id,
                                         approverName = String.Concat(userDetails.FirstName, " ", userDetails.LastName),
                                         status = "Created",
                                     }
                                   };
                                }

                                var RequestDetails = new UarRequestDetails
                                {
                                    RequestId = newRequestDetails.Id,
                                    RequestDetailsId = String.Concat(newRequestDetails.Id, "-", countOfSerialNumber),
                                    AccessCategoryId = item.AccessCategoryId,
                                    SubCategoryId = item.SubCategoryId,
                                    AccessTypeId = item.AccessTypeId,
                                    UserId = item.UserIdDDL,
                                    CreatedBy = newRequestData.UserId,
                                    Level = 1,
                                    DateOfRevoke = DateTime.Now,
                                    WorkflowStage = WorkflowStage,
                                    Status = Status,
                                    DeleteFlag = "N",
                                    SlaData = JsonConvert.SerializeObject(slaData),
                                    ApprovalDetails = JsonConvert.SerializeObject(approveDetail)
                                };
                                db.UarRequestDetails.Add(RequestDetails);
                                db.SaveChanges();

                                // Audit Trail 
                                existnotchange = true;
                                List<string> output = new List<string>();
                                accty.accesscategory = item.AccessCategory;
                                accty.accesscategoryid = item.AccessCategoryId;
                                accty.accesstype = item.AccessType;
                                accty.accesstypeid = item.AccessTypeId;
                                accty.subcategory = item.SubCategory;
                                accty.subcategoryid = item.SubCategoryId;
                                accty.username = item.UserName;
                                accty.userid = item.UserIdDDL;
                                 

                                var acccategory = db.LookupItem.Where(x => x.Id == item.AccessCategoryId).Select(x => x.Key).FirstOrDefault();
                                var subcategory1 = db.LookupSubitem.Where(x => x.Id == item.SubCategoryId).Select(x => x.Key).FirstOrDefault();
                                var accctype = db.LookupItem.Where(x => x.Id == item.AccessTypeId).Select(x => x.Key).FirstOrDefault();
                                var usernam = db.User.Where(x => x.Id == item.UserIdDDL).Select(x => x.FirstName).FirstOrDefault();

                                content = "";
                                content = "Sub-requestid= " + String.Concat(newRequestDetails.Id, "-", countOfSerialNumber) + " is created.";
                                output.Add(content);
                                content = "";
                                content = "Access Category= " + acccategory + ",Sub Category= " + subcategory1 + ", Access Type= " + accctype + ", User Name= " + usernam+ "";
                                output.Add(content);
                                content = "";
                                content = "Current Workflow Stage=  " + WorkflowStage + ", Current Level= " + 1 + ", Status=  " + Status; 
                                output.Add(content);
                                accty.categorydescription = output;
                                accessType.Add(accty);
                                
                            }
                            else
                            {
                                var fetchSubRequest = db.UarRequestDetails.Where(x => x.Id == item.Id  && x.DeleteFlag != "Y").FirstOrDefault();
                                if (fetchSubRequest != null)
                                {
                                    fetchSubRequest.SlaData = JsonConvert.SerializeObject(slaData);
                                    fetchSubRequest.WorkflowStage = WorkflowStage;
                                    fetchSubRequest.Status = Status;
                                    fetchSubRequest.Level = 1;
                                    db.SaveChanges();

                                    //Audit trial
                                    if (existnotchange == false)
                                    {
                                        existnotchange = true;
                                        List<string> output = new List<string>();
                                        content = "";
                                        content = "Requestid= " + newRequestData.Id + " has been updated";
                                        output.Add(content);
                                        accty.categorydescription = output;
                                        accessType.Add(accty);
                                    }
                                }
                            }
                        }
                        var entryper = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                        masauditdto.browser = Ipdetails[0]; 
                        masauditdto.eventname = "Update";
                        masauditdto.module = "Update Request";
                        masauditdto.userid = newRequestData.UserId;
                        auditjsonmdl.accesstype = accessType;
                        auditjsonmdl.requestId = Convert.ToString(newRequestDetails.Id);
                        masauditdto.Systemremarks = newRequestData.Remarks;
                        masauditdto.RequestId = Convert.ToString(newRequestDetails.Id);
                        auditjsonmdl.createddate = DateTime.Now;
                        auditjsonmdl.browser = Ipdetails[0]; 
                        auditjsonmdl.eventname = "Update";
                        auditjsonmdl.module = "Update Request ";
                        auditjsonmdl.userid = newRequestData.UserId;
                        auditjsonmdl.username = entryper;
                        auditjsonmdl.ipaddress = Ipdetails[1];
                        auditjsonmdl.Systemremarks = newRequestData.Remarks;

                    }

                    //Attachment saving part
                    if (newRequestData.Id == 0)
                    {
                        var orgId = 1;
                        var orgDetails = db.Organisation.Where(x => x.Id == orgId).FirstOrDefault();
                        var orgPath = orgDetails.OrgName + string.Concat(orgDetails.Id);

                        var orgFolderpath = orgDetails.Filepath;
                        var filePath = orgFolderpath + orgPath + "\\" + newRequestDetails.Id + "\\";

                        bool exists = System.IO.Directory.Exists(filePath);

                        if (!exists)
                            System.IO.Directory.CreateDirectory(filePath);

                        var filePath1 = orgFolderpath + orgPath + "\\" + newRequestDetails.Id + "\\AuditTrial\\";

                        bool exists1 = System.IO.Directory.Exists(filePath1);

                        if (!exists1)
                            System.IO.Directory.CreateDirectory(filePath1);

                        var orgUrs = new UrsOrg
                        {
                            OrgId = orgId,
                            ReqId = newRequestDetails.Id,
                            FilePath = filePath
                        };

                        db.UrsOrg.Add(orgUrs);
                        db.SaveChanges();
                    }



                    var f = (from sop in db.UarRequestmaster
                             join sopOrg in db.UrsOrg on sop.Id equals sopOrg.ReqId
                             where sop.Id == newRequestDetails.Id
                             select new UrsOrg
                             {
                                 ReqId = sopOrg.ReqId,
                                 FilePath = sopOrg.FilePath
                             }).FirstOrDefault();

                    if (input.StaticAttachments != null)
                    {
                        foreach (var item in input.StaticAttachments)
                        {
                            attachmentlist = attachmentlist + item.FileName + ",";
                            var data = UploadFile(item, f.FilePath);
                            if (data != null && data.result)
                            {
                                foreach (var st in staticAttachment.Where(w => w.FileName == item.FileName))
                                {
                                    st.FilePath = data.path;
                                }
                            }
                        }
                    }

                    AttachmentDB table = new AttachmentDB();
                    table.StaticTB = staticAttachment;

                    var updateData = db.UarRequestmaster.Where(x => x.Id == newRequestDetails.Id).FirstOrDefault();
                    updateData.Attachment = JsonConvert.SerializeObject(table);
                    updateData.Remarks = newRequestData.Remarks;
                    db.SaveChanges();

                    //for audit trial
                    masauditdto.existsentry = newRequestData.Id;
                    auditjsonmdl.filelist = staticAttachment;
                    auditjsonmdl.Systemremarks = newRequestData.Remarks;

                    List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                    auditdto1.Add(auditjsonmdl);
                    masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                    masauditdto.createddate = DateTime.Now;
                    masauditdto.RequestId = Convert.ToString(newRequestDetails.Id);

                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                    resut.Status = true;
                    resut.Description = "Request ID Is : " + newRequestDetails.Id + ".";
                }

                return resut;
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while creating the new request.";
                return resut;
            }
        }

        public static FinalResultDTO CreateSLADataObj(Int32 LevelNo, bool SlaFlag, Int32 SlaDays, List<UserLevels> userLevels, string workflowStage)
        {
            FinalResultDTO result = new FinalResultDTO();
            SLADetails slaData = new SLADetails();
            try
            {
                slaData.WorkflowStage = workflowStage;
                slaData.LevelNo = LevelNo;
                slaData.SlaFlag = SlaFlag;
                slaData.SlaDays = SlaDays;
                slaData.RemainingSLADays = SlaDays;
                slaData.ApprovalDate = DateTime.Now;
                if (SlaFlag)
                {
                    FinalResultDTO SlaTargetDateOP = RequestMethodDAL.GetSLATargetDate(SlaDays, DateTime.Now);
                    if (SlaTargetDateOP.Status)
                    {
                        slaData.SlaTargetDate = SlaTargetDateOP.ResultOP;
                    }
                }

                List<DropDownsDTO> users = new List<DropDownsDTO>();
                foreach (var itemIT in userLevels)
                {
                    users.Add(new DropDownsDTO
                    {
                        Id = itemIT.UserId,
                        key = itemIT.FirstName
                    });
                }
                slaData.Users = users;

                result.Status = true;
                result.ResultOP = slaData;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                return result;
            }
        }

        public static dynamic UploadFile(IFormFile fileModel, string folderPath)
        {
            dynamic resultDTO = new ExpandoObject();
            resultDTO.path = "";
            resultDTO.result = false;
            try
            {
                IFormFile Pdffile = fileModel;
                if (Pdffile != null)
                {
                    DateTime now = DateTime.Now;
                    string dateTo = now.ToString("ddMMyyhhmmss");
                    string orgfilename = Pdffile.FileName;
                    string forauditfilename = Pdffile.FileName;
                    string ext = Path.GetExtension(orgfilename);
                    string orifilename = orgfilename.Substring(0, orgfilename.Length - 4);

                    string filefullname = orifilename + dateTo + ext;
                    var filePath = Path.Combine(folderPath, filefullname);
                    var fileaudit = Path.Combine(folderPath + "AuditTrial", forauditfilename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        Pdffile.CopyTo(fs);
                        fs.Flush();
                    }

                    using (FileStream fsaudit = System.IO.File.Create(fileaudit))
                    {
                        Pdffile.CopyTo(fsaudit);
                        fsaudit.Flush();
                    }
                    resultDTO.path = filePath;
                    resultDTO.result = true;
                }
                else
                {
                    resultDTO.result = false;
                }
                return resultDTO;
            }
            catch (Exception ex)
            {
                resultDTO.result = false;
                return resultDTO;
            }
        }

        public static dynamic FillUserDetails(int userID)
        {
            FinalResultDTO resut = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.templateNames = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();

            try
            {
                var newDate = DateTime.Now;
                var Reqdate = newDate.Date;



                string RequestDate = String.Format("{0:d/M/yyyy}", Reqdate);

                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from user in db.User
                                         join usermas in db.User on user.ImmediateSupervisor equals usermas.Id
                                         join dept in db.Department on user.Department equals dept.Id

                                         where user.Id == userID

                                         select new
                                         {
                                             id = user.Id,
                                             name = String.Concat(user.FirstName, " ", user.LastName),
                                             emailId = user.Email,
                                             location = user.Location,
                                             Department = dept.DepartmentName,
                                             deptId = user.Department,
                                             requestDate = RequestDate,
                                             manager = usermas.FirstName

                                         }).FirstOrDefault();
                    resultDTO.templateNames = templateNames;

                    var UserDetails = GetUserNames(templateNames.deptId, templateNames.id);
                    if (UserDetails.Status)
                    {
                        resultDTO.UserDetails = UserDetails.ResultOP;
                    }
                    resut.ResultOP = resultDTO;
                    resut.Status = true;

                }

                return resut;
            }
            catch (Exception ex)
            {

                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static dynamic GetUserNames(long? dept, long userID)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {


                    var LookUpData = (from sa in db.User
                                      where sa.DeleteFlag == "N" && sa.Department == dept
                                      //&& sa.Id != userID
                                      select new LookUpItemDTO
                                      {
                                          Id = sa.Id,
                                          Key = sa.FirstName,
                                          Value = sa.LastName

                                      }).ToList();

                    resut.Status = true;
                    resut.ResultOP = LookUpData;

                    return resut;

                }
            }
            catch (Exception ex)
            {

                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the Look Up Master Data..";
                return resut;
            }
        }

        public static dynamic DeleteData(DeleteRequest deleteData,dynamic Ipdetails)
        {
            FinalResultDTO result = new FinalResultDTO();
            string attachmentlist = "";
         

            try
            {
                var delRequestData = JsonConvert.DeserializeObject<DeleteDto>(deleteData.DeleteDetails);
                var delRequestData1 = JsonConvert.DeserializeObject<DeleteDto1>(deleteData.DeleteDetails);
                List<AttachmentTable> staticAttachment = JsonConvert.DeserializeObject<List<AttachmentTable>>(deleteData.StaticAttachmentsTable);
                using (dbURSContext db = new dbURSContext())
                {
                    var f = (from sop in db.UarRequestmaster
                             join sopOrg in db.UrsOrg on sop.Id equals sopOrg.ReqId
                             where sop.EmployeeId == delRequestData.UserId
                             select new UrsOrg
                             {
                                 ReqId = sopOrg.ReqId,
                                 FilePath = sopOrg.FilePath
                             }).FirstOrDefault();

                    //string filePathDoc = db.SopOrg.Where(x => x.SopId == inputParams.Id).Select(x => x.FilePath).FirstOrDefault();
                    if (deleteData.StaticAttachments != null)
                    {
                        foreach (var item in deleteData.StaticAttachments)
                        {
                            attachmentlist = attachmentlist + item.FileName + ",";
                            var data = UploadFile(item, f.FilePath);
                            if (data != null && data.result)
                            {
                                foreach (var st in staticAttachment.Where(w => w.FileName == item.FileName))
                                {
                                    st.FilePath = data.path;
                                }
                            }
                        }
                    }

                   
                    AttachmentJson table = new AttachmentJson();
                    table.Id = delRequestData.requestDetailsId;
                    table.StaticTB = staticAttachment;

                    var updateData = db.UarRequestDetails.Where(x => x.Id == delRequestData.requestDetailsId).FirstOrDefault();
                    updateData.Status = "Deleted";
                    updateData.DeleteFlag = "Y";
                    updateData.Attachment = JsonConvert.SerializeObject(table);
                    db.SaveChanges();

                    var completeDetails = db.UarRequestDetails.Where(x => x.RequestId == updateData.RequestId).ToList();
                    bool flag = false;
                    foreach (var item in completeDetails)
                    {
                        if (item.Status != "Deleted" && item.Status != "Approved")
                        {
                            flag = true;
                            break;
                        }
                    }
                
                    if (!flag)
                    {
                        var masterData = db.UarRequestmaster.Where(x => x.Id == updateData.RequestId).FirstOrDefault();
                        if (completeDetails.Any(x=>x.Status == "Approved"))
                        {
                            masterData.Status = "Closed";
                        }
                        else
                        {
                            masterData.Status = "Rejected";
                        }
                        
                        db.SaveChanges();
                    }

                    //audit trial
                    AuditTrialDTO masauditdto = new AuditTrialDTO();
                    List<AuditTrialJSONDTO> auditdto = new List<AuditTrialJSONDTO>();
                    AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
                    var accty = new AccessType();
                    List<string> output = new List<string>();
                    List<AccessType> accessType = new List<AccessType>();
                    var acccategory = db.LookupItem.Where(x => x.Id == updateData.AccessCategoryId).Select(x => x.Key).FirstOrDefault();
                    var subcategory1 = db.LookupSubitem.Where(x => x.Id == updateData.SubCategoryId).Select(x => x.Key).FirstOrDefault();
                    var accctype = db.LookupItem.Where(x => x.Id == updateData.AccessTypeId).Select(x => x.Key).FirstOrDefault();
                    var usernam = db.User.Where(x => x.Id == updateData.UserId).Select(x => x.FirstName).FirstOrDefault();
                    
                    string content = "";
                    //content = "Access Category= " + acccategory + ", Sub Category= " + subcategory1 + ", Access Type= " + accctype + ", User Name= " + usernam + "";
                    //output.Add(content);
                    content = " Sub-requestid=  " + updateData.RequestDetailsId + " is Deleted";
                    output.Add(content);
                    accty.categorydescription = output;
                    accessType.Add(accty);

                    var entryper = db.User.Where(x => x.Id == delRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                    masauditdto.browser = Ipdetails[0];
                    masauditdto.eventname = "Delete";
                    masauditdto.module = "Delete  Request";
                    masauditdto.userid = delRequestData.UserId;
                    auditjsonmdl.accesstype = accessType;
                    auditjsonmdl.requestId = Convert.ToString(updateData.RequestId);
                    masauditdto.Systemremarks = delRequestData1.Remarks;
                    masauditdto.RequestId = Convert.ToString(updateData.RequestId);
                    auditjsonmdl.createddate = DateTime.Now;
                    auditjsonmdl.browser = Ipdetails[0];
                    auditjsonmdl.eventname = "Delete";
                    auditjsonmdl.module = "Delete Request ";
                    auditjsonmdl.userid = delRequestData.UserId;
                    auditjsonmdl.username = entryper;
                    auditjsonmdl.ipaddress = Ipdetails[1];
                   // auditjsonmdl.Systemremarks = delRequestData1.Remarks;

                    //for audit trial
                    masauditdto.existsentry = updateData.RequestId;
                    auditjsonmdl.filelist = staticAttachment;
                    auditjsonmdl.Systemremarks = delRequestData1.Remarks;
                    List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                    auditdto1.Add(auditjsonmdl);
                    masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                    masauditdto.createddate = DateTime.Now;
                    masauditdto.RequestId = Convert.ToString(updateData.RequestId);
                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                    var masterDataaudi = db.UarRequestmaster.Where(x => x.Id == updateData.RequestId).FirstOrDefault();
                    if (masterDataaudi.Status== "Closed")
                    {
                        AuditTrialJSONDTO auditjsonmdl1 = new AuditTrialJSONDTO();
                        var accty1 = new AccessType();
                        List<string> output1 = new List<string>();
                        List<AccessType> accessType1 = new List<AccessType>();

                        masauditdto.description = "";
                        content = "";
                        //content = "Access Category= " + acccategory + ", Sub Category= " + subcategory1 + ", Access Type= " + accctype + ", User Name= " + usernam + "";
                        //output.Add(content);
                        content = " Requestid=  " + updateData.RequestId + " is Closed";
                        output1.Add(content);
                       
                        accty1.categorydescription = output1;
                        accessType1.Add(accty1);

                        masauditdto.browser = Ipdetails[0];
                        masauditdto.eventname = "Closed";
                        masauditdto.module = "Approve  Request";
                        masauditdto.userid = delRequestData.UserId;
                        auditjsonmdl1.accesstype = accessType1;
                        auditjsonmdl1.requestId = Convert.ToString(updateData.RequestId);
                        masauditdto.Systemremarks = delRequestData.remark;
                        masauditdto.RequestId = Convert.ToString(updateData.RequestId);
                        auditjsonmdl1.createddate = DateTime.Now;
                        auditjsonmdl1.browser = Ipdetails[0];
                        auditjsonmdl1.eventname = "Closed";
                        auditjsonmdl1.module = "Close Request ";
                        auditjsonmdl1.userid = delRequestData.UserId;
                        auditjsonmdl1.username = entryper;
                        auditjsonmdl1.ipaddress = Ipdetails[1];
                        auditjsonmdl1.Systemremarks = masterDataaudi.Remarks;
                        //for audit trial
                        masauditdto.existsentry = updateData.RequestId;
                        //  auditjsonmdl.filelist = staticAttachment;
                        auditjsonmdl1.Systemremarks = delRequestData.remark;
                        List<AuditTrialJSONDTO> auditdto2 = new List<AuditTrialJSONDTO>();
                        auditdto2.Add(auditjsonmdl1);
                        masauditdto.description = JsonConvert.SerializeObject(auditdto2);
                        masauditdto.createddate = DateTime.Now;
                        masauditdto.RequestId = Convert.ToString(updateData.RequestId);
                        AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl1);
                    }


                    result.Status = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting selected User.";
                return result;
            }
        }


        public static FinalResultDTO getRequestDataForDelete(long requestID)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {


                    List<BulkRequestDTo> DetailsTable = new List<BulkRequestDTo>();

                    DetailsTable = (from NR in db.UarRequestDetails

                                    join lookupItem in db.LookupItem on NR.AccessCategoryId equals lookupItem.Id
                                    join lookupsub in db.LookupSubitem on NR.SubCategoryId equals lookupsub.Id
                                    join lookupItemaccess in db.LookupItem on NR.AccessTypeId equals lookupItemaccess.Id
                                    join userDll in db.User on NR.UserId equals userDll.Id
                                    where NR.Id == requestID && NR.DeleteFlag != "Y"
                                    select new BulkRequestDTo
                                    {
                                        AccessCategoryId = NR.AccessCategoryId,
                                        AccessTypeId = NR.AccessTypeId,
                                        SubCategoryId = NR.SubCategoryId,
                                        AccessCategory = lookupItem.Value,
                                        SubCategory = lookupsub.Value,
                                        AccessType = lookupItemaccess.Value,
                                        UserName = userDll.FirstName,
                                        level = NR.Level,
                                        existingRecord = true,
                                        RequestDetailsId = NR.RequestDetailsId ?? "",
                                        WorkFlowSatge = NR.WorkflowStage,
                                        Status = NR.Status,

                                    }).ToList();


                    resut.Status = true;
                    resut.ResultOP = DetailsTable;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static dynamic FetchFile(string filePath)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
               bool Folder = File.Exists(filePath); // System.IO.Directory.Exists(filePath);
                if (Folder)
                {
                    Byte[] filecontent = System.IO.File.ReadAllBytes(filePath);
                    result.ResultOP = filecontent;
                }
                else
                {
                    result.Status = false;
                    result.Description = "Problem in fetching the PDF file from the stored location.";
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Description = "Something went wrong, While fetching the file.";
                result.Status = false;
                return result;
            }
        }
               
    }
}

