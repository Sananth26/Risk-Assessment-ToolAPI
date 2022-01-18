using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class RequestMethodDAL
    {

        public static FinalResultDTO getListOfMyRequest(Int32 Id)
        {

            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from NR in db.UarRequestmaster
                                         join user in db.User on NR.EmployeeId equals user.Id
                                         where NR.EmployeeId == Id
                                         orderby NR.Id descending
                                         select new
                                         {
                                             userID = user.Id,
                                             userName = user.FirstName,
                                             requestId = NR.Id,
                                             createdDate = user.CreatedTime,
                                             level = NR.Level,
                                             workflowstage = NR.WorkflowStage,
                                             requestDate = NR.RequestDate,
                                             status = NR.Status
                                         }).ToList().Distinct();

                    resut.ResultOP = templateNames;
                    resut.Status = true;
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

        public static FinalResultDTO getRequest_date(AccessApproval dateRange)
        {
            FinalResultDTO resut = new FinalResultDTO();

            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from NR in db.UarRequestmaster
                                         join NRD in db.UarRequestDetails on NR.Id equals NRD.RequestId
                                         join lookUpsub in db.LookupSubitem on NRD.SubCategoryId equals lookUpsub.Id
                                         join lookup in db.LookupItem on NRD.AccessCategoryId equals lookup.Id
                                         join lookupacTyp in db.LookupItem on NRD.AccessTypeId equals lookupacTyp.Id
                                         join user in db.User on NRD.UserId equals user.Id
                                         where (NRD.DateOfRevoke >= Convert.ToDateTime(dateRange.fromDate) && NRD.DateOfRevoke <= Convert.ToDateTime(dateRange.toDate))
                                         select new
                                         {
                                             id = NRD.Id,
                                             userID = user.FirstName,
                                             userName = user.FirstName,
                                             requestId = NRD.RequestId,
                                             accessCategoryname = lookup.Value,
                                             accessCategoryId = NRD.AccessCategoryId,
                                             subaccessCategoryname = lookUpsub.Value,
                                             subaccessCategoryId = NRD.SubCategoryId,
                                             accessTypename = lookupacTyp.Value,
                                             accessTypeId = NRD.AccessTypeId,
                                             dateOfRevoke = NRD.DateOfRevoke

                                         }).ToList();
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

        public static FinalResultDTO SaveDecision(ApproveRejectRequest input, string browser, string ipaddress)
        {
            FinalResultDTO resut = new FinalResultDTO();

            try
            {
                var ApproveRejctdata = JsonConvert.DeserializeObject<ApproveRejectRequestDTO>(input.ApproveRejectDTO);
                List<AttachmentTable> staticAttachment = JsonConvert.DeserializeObject<List<AttachmentTable>>(input.StaticAttachmentsTable);
                long jsonId = 0;
                long RequestID = 0;

                AuditTrialDTO masauditdto = new AuditTrialDTO();
                List<AuditTrialJSONDTO> auditdto = new List<AuditTrialJSONDTO>();
                AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
                List<AccessType> accessType = new List<AccessType>();
                // var ipdetails = Getip();
                Int32 oldlevel = 0;
                string oldworkflow = string.Empty;
                string olddecision = string.Empty;
                using (dbURSContext db = new dbURSContext())
                {
                    var approverData = db.User.Where(x => x.Id == ApproveRejctdata.LoginUserID).FirstOrDefault();
                    if (ApproveRejctdata.CompleteDeptProcess == true)
                    {
                        List<ApproveDetailsJson> approveDetails = new List<ApproveDetailsJson>();

                        var detailedRow = db.UarRequestmaster.Where(x => x.Id == ApproveRejctdata.RequestMasterId).FirstOrDefault();
                        oldlevel = detailedRow.Level;
                        oldworkflow = detailedRow.WorkflowStage;
                        olddecision = detailedRow.Status;
                        if (detailedRow.ApprovalDetails != null)
                        {
                            approveDetails = JsonConvert.DeserializeObject<List<ApproveDetailsJson>>(detailedRow.ApprovalDetails);
                        }

                        var approveDetailsJson = new ApproveDetailsJson
                        {
                            approverDatetime = DateTime.Now,
                            approverId = approverData.Id,
                            approverName = String.Concat(approverData.FirstName, " ", approverData.LastName),
                            Id = approveDetails.Count + 1,
                            Level = detailedRow.Level,
                            Remark = ApproveRejctdata.Remark,
                            status = ApproveRejctdata.Decision,
                            WorkFlowStage = ApproveRejctdata.WorkFlowStage
                        };

                        approveDetails.Add(approveDetailsJson);

                        string WorkflowStage = string.Empty;
                        string Status = string.Empty;
                        Int32 Level = 0;

                        List<UserLevels> userLevels = new List<UserLevels>();
                        FinalResultDTO levelWiseUsers = new FinalResultDTO();

                        if (ApproveRejctdata.Decision == "Accepted")
                        {
                            Level = detailedRow.Level + 1;
                            WorkflowStage = "Department";
                            Status = "Processing";

                            userLevels = new List<UserLevels>();
                            levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForUSer(Convert.ToInt32(ApproveRejctdata.LoginUserID), Level);
                            if (levelWiseUsers.Status)
                            {
                                userLevels = levelWiseUsers.ResultOP;
                            }
                        }
                        else
                        {
                            Level = 1;
                            WorkflowStage = "Department";
                            Status = "Rejected";

                            userLevels = new List<UserLevels>();
                            levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForUSer(Convert.ToInt32(detailedRow.EmployeeId), Level);
                            if (levelWiseUsers.Status)
                            {
                                userLevels = levelWiseUsers.ResultOP;
                            }
                        }

                        SLADetails slaData = new SLADetails();
                        if (userLevels.Count >= 1)
                        {
                            FinalResultDTO SlaDataOBJ = NewRequestDAL.CreateSLADataObj(Level, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                            if (SlaDataOBJ.Status)
                            {
                                slaData = SlaDataOBJ.ResultOP;
                            }
                            detailedRow.SlaData = JsonConvert.SerializeObject(slaData);
                        }
                        else
                        {
                            if (ApproveRejctdata.Decision == "Accepted")
                            {
                                Level = detailedRow.Level;
                            }

                            WorkflowStage = "IT";
                            Status = "Processing";

                            approveDetails.Add(
                                 new ApproveDetailsJson
                                 {
                                     Id = 0,
                                     approverDatetime = DateTime.Now,
                                     Level = 0,
                                     approverId = approverData.Id,
                                     approverName = String.Concat(approverData.FirstName, " ", approverData.LastName),
                                     status = "deptComplete",
                                 });
                        }

                        detailedRow.ApprovalDetails = JsonConvert.SerializeObject(approveDetails);
                        detailedRow.Level = Level;
                        detailedRow.WorkflowStage = WorkflowStage;
                        detailedRow.Status = Status;
                        db.SaveChanges();

                        var fetchedSubRequests = db.UarRequestDetails.Where(x => x.RequestId == detailedRow.Id && x.DeleteFlag != "Y").ToList();
                        foreach (var subReq in fetchedSubRequests)
                        {
                            if (WorkflowStage == "IT")
                            {
                                Level = 1;
                                userLevels = new List<UserLevels>();
                                levelWiseUsers = new FinalResultDTO();
                                levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForIT(1, Convert.ToInt32(subReq.AccessCategoryId), Convert.ToInt32(subReq.SubCategoryId));
                                if (levelWiseUsers.Status)
                                {
                                    userLevels = levelWiseUsers.ResultOP;
                                }

                                slaData = new SLADetails();
                                if (userLevels.Count >= 1)
                                {
                                    FinalResultDTO SlaDataOBJ = NewRequestDAL.CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                                    if (SlaDataOBJ.Status)
                                    {
                                        slaData = SlaDataOBJ.ResultOP;
                                    }
                                }
                            }

                            subReq.SlaData = JsonConvert.SerializeObject(slaData);
                            subReq.Level = Level;
                            subReq.WorkflowStage = WorkflowStage;
                            subReq.Status = Status;
                            db.SaveChanges();
                        }

                        jsonId = approveDetailsJson.Id;
                        var subrequestid = db.UarRequestDetails.Where(x => x.Id == ApproveRejctdata.RequestDetailsId).Select(x => x.RequestDetailsId).FirstOrDefault();

                        //For audit trial
                        List<string> output = new List<string>();
                        string content = "";
                        var accty = new AccessType();
                        content = "Current Workflow Stage=  " + oldworkflow + ", Current Level= " + oldlevel + ", Status=  " + olddecision;//oldstatus
                        output.Add(content);
                        content = "";
                        content = "Decision taken at current level is:  " + ApproveRejctdata.Decision;
                        output.Add(content);
                        content = "";
                        content = "Next Workflow Stage=  " + WorkflowStage + ", Next Level= " + Level + ", Status=  " + Status;
                        output.Add(content);

                        accty.categorydescription = output;
                        accessType.Add(accty);

                        var usernamee = db.User.Where(x => x.Id == ApproveRejctdata.LoginUserID).Select(x => x.FirstName).FirstOrDefault();
                        auditjsonmdl.accesstype = accessType;

                        auditjsonmdl.browser = browser;
                        auditjsonmdl.eventname = "Approve/Reject";
                        auditjsonmdl.module = "Approve Request ";
                        auditjsonmdl.userid = ApproveRejctdata.LoginUserID;
                        //  auditjsonmdl.categorydescription = description;
                        auditjsonmdl.requestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                        auditjsonmdl.Systemremarks = ApproveRejctdata.Remark;
                        auditjsonmdl.username = usernamee;
                        auditjsonmdl.ipaddress = ipaddress;
                        auditjsonmdl.createddate = DateTime.Now;
                        masauditdto.existsentry = ApproveRejctdata.RequestMasterId;
                        auditjsonmdl.filelist = staticAttachment;
                        auditjsonmdl.Systemremarks = ApproveRejctdata.Remark;

                        //List<AuditTrialJSONDTO> auditdto2 = new List<AuditTrialJSONDTO>();
                        //auditdto2.Add(auditjsonmdl);
                        //masauditdto.description = JsonConvert.SerializeObject(auditdto2);
                        //masauditdto.createddate = DateTime.Now;
                        //masauditdto.RequestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                        //AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                    }
                    else
                    {
                        //IT Process
                        var detailedRow = db.UarRequestDetails.Where(x => x.Id == ApproveRejctdata.RequestDetailsId).FirstOrDefault();
                        oldlevel = Convert.ToInt32(detailedRow.Level);
                        oldworkflow = detailedRow.WorkflowStage;
                        olddecision = detailedRow.Status;
                        List<ApproveDetailsJson> approveDetails = new List<ApproveDetailsJson>();

                        if (detailedRow.ApprovalDetails != null)
                        {
                            approveDetails = JsonConvert.DeserializeObject<List<ApproveDetailsJson>>(detailedRow.ApprovalDetails);
                        }

                        var approveDetailsJson = new ApproveDetailsJson
                        {
                            approverDatetime = DateTime.Now,
                            approverId = ApproveRejctdata.LoginUserID,
                            approverName = String.Concat(approverData.FirstName, " ", approverData.LastName),
                            Id = approveDetails.Count + 1,
                            Level = detailedRow.Level,
                            Remark = ApproveRejctdata.Remark,
                            status = ApproveRejctdata.Decision,
                            WorkFlowStage = ApproveRejctdata.WorkFlowStage
                        };
                        approveDetails.Add(approveDetailsJson);

                        string WorkflowStage = string.Empty;
                        string Status = string.Empty;
                        Int32 Level = 0;
                        List<UserLevels> userLevels = new List<UserLevels>();
                        FinalResultDTO levelWiseUsers = new FinalResultDTO();

                        if (ApproveRejctdata.Decision == "Accepted")
                        {
                            Level = Convert.ToInt32(detailedRow.Level + 1);
                            WorkflowStage = detailedRow.WorkflowStage;
                            Status = "Processing";

                            if (detailedRow.WorkflowStage == "IT")
                            {
                                levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForIT(Level, Convert.ToInt32(detailedRow.AccessCategoryId), Convert.ToInt32(detailedRow.SubCategoryId));
                            }
                            else
                            {
                                levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForUSer(Convert.ToInt32(ApproveRejctdata.LoginUserID), Level);
                            }

                            if (levelWiseUsers.Status)
                            {
                                userLevels = levelWiseUsers.ResultOP;
                            }
                            SLADetails slaData = new SLADetails();
                            if (userLevels.Count >= 1)
                            {
                                FinalResultDTO SlaDataOBJ = NewRequestDAL.CreateSLADataObj(Level, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                                if (SlaDataOBJ.Status)
                                {
                                    slaData = SlaDataOBJ.ResultOP;
                                }

                                detailedRow.SlaData = JsonConvert.SerializeObject(slaData);
                            }
                            else
                            {
                                Level = WorkflowStage == "IT" ? Convert.ToInt32(detailedRow.Level) : 1;
                                Status = WorkflowStage == "IT" ? "Approved" : "Processing";
                                WorkflowStage = WorkflowStage == "IT" ? WorkflowStage : "IT";

                                approveDetails.Add(
                                new ApproveDetailsJson
                                {
                                    Id = 0,
                                    approverDatetime = DateTime.Now,
                                    Level = 0,
                                    approverId = ApproveRejctdata.LoginUserID,
                                    approverName = String.Concat(approverData.FirstName, " ", approverData.LastName),
                                    status = Status == "Processing" ? "deptComplete" : "Approved",
                                });

                                if (Status == "Processing")
                                {
                                    userLevels = new List<UserLevels>();
                                    levelWiseUsers = new FinalResultDTO();
                                    levelWiseUsers = RequestMethodDAL.GetImmediateSupervisorForIT(1, Convert.ToInt32(detailedRow.AccessCategoryId), Convert.ToInt32(detailedRow.SubCategoryId));
                                    if (levelWiseUsers.Status)
                                    {
                                        userLevels = levelWiseUsers.ResultOP;
                                    }

                                    slaData = new SLADetails();
                                    if (userLevels.Count >= 1)
                                    {
                                        FinalResultDTO SlaDataOBJ = NewRequestDAL.CreateSLADataObj(1, userLevels[0].SlaFlag, userLevels[0].SlaDays, userLevels, WorkflowStage);
                                        if (SlaDataOBJ.Status)
                                        {
                                            slaData = SlaDataOBJ.ResultOP;
                                        }
                                    }
                                    detailedRow.SlaData = JsonConvert.SerializeObject(slaData);
                                }
                            }
                        }
                        else
                        {
                            Level = 1;
                            WorkflowStage = "Department";
                            Status = "Rejected";
                        }

                        detailedRow.ApprovalDetails = JsonConvert.SerializeObject(approveDetails);
                        detailedRow.Level = Level;
                        detailedRow.WorkflowStage = WorkflowStage;
                        detailedRow.Status = Status;
                        db.SaveChanges();

                        if (Status == "Approved")
                        {
                            var completeDetails = db.UarRequestDetails.Where(x => x.RequestId == detailedRow.RequestId).ToList();
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
                                var masterData = db.UarRequestmaster.Where(x => x.Id == detailedRow.RequestId).FirstOrDefault();
                                masterData.Status = "Closed";
                                db.SaveChanges();
                            }
                        }

                        jsonId = approveDetailsJson.Id;
                        var subrequestid = db.UarRequestDetails.Where(x => x.Id == ApproveRejctdata.RequestDetailsId).Select(x => x.RequestDetailsId).FirstOrDefault();

                        //For audit trial
                        List<string> output = new List<string>();
                        string content = "";
                        var accty = new AccessType();
                        content = "Sub-requestid=  " + subrequestid;
                        output.Add(content);
                        content = "";
                        content = "Current Workflow Stage=  " + oldworkflow + ", Current Level= " + oldlevel + ", Status=  " + olddecision;//oldstatus
                        output.Add(content);
                        content = "";
                        content = "Decision taken at current level is:  " + ApproveRejctdata.Decision;
                        output.Add(content);
                        content = "";
                        if (Status == "Approved")
                        {
                            content = "Status=  " + Status;
                        }
                        else
                        {
                            content = "Next Workflow Stage=  " + WorkflowStage + ", Next Level= " + Level + ", Status=  " + Status;
                        }
                        output.Add(content);

                        accty.categorydescription = output;
                        accessType.Add(accty);

                        var usernamee = db.User.Where(x => x.Id == ApproveRejctdata.LoginUserID).Select(x => x.FirstName).FirstOrDefault();
                        auditjsonmdl.accesstype = accessType;

                        auditjsonmdl.browser = browser;
                        auditjsonmdl.eventname = "Approve/Reject";
                        auditjsonmdl.module = "Approve Request ";
                        auditjsonmdl.userid = ApproveRejctdata.LoginUserID;
                        //  auditjsonmdl.categorydescription = description;
                        auditjsonmdl.requestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                        auditjsonmdl.Systemremarks = ApproveRejctdata.Remark;
                        auditjsonmdl.username = usernamee;
                        auditjsonmdl.ipaddress = ipaddress;
                        auditjsonmdl.createddate = DateTime.Now;

                    }


                    var f = (from sop in db.UarRequestmaster
                             join sopOrg in db.UrsOrg on sop.Id equals sopOrg.ReqId
                             where sop.Id == ApproveRejctdata.RequestMasterId
                             select new UrsOrg
                             {
                                 ReqId = sopOrg.ReqId,
                                 FilePath = sopOrg.FilePath
                             }).FirstOrDefault();
                    if (f == null)
                    {
                        var orgId = 1;
                        var orgDetails = db.Organisation.Where(x => x.Id == orgId).FirstOrDefault();

                        var orgPath = orgDetails.OrgName + string.Concat(orgDetails.Id);
                        // string filePath = "E:\\SAPFiles\\" + orgPath + "\\" + inputParams.DocumentNumber + "\\";

                        var orgFolderpath = orgDetails.Filepath;
                        var filePath = orgFolderpath + orgPath + "\\" + RequestID + "\\ApproveReject" + "\\";

                        bool exists = System.IO.Directory.Exists(filePath);

                        if (!exists)
                            System.IO.Directory.CreateDirectory(filePath);

                        var orgUrs = new UrsOrg
                        {
                            OrgId = orgId,
                            ReqId = RequestID,
                            FilePath = filePath
                        };

                        db.UrsOrg.Add(orgUrs);
                        db.SaveChanges();
                    }



                    if (input.StaticAttachments != null)
                    {
                        foreach (var item in input.StaticAttachments)
                        {

                            if (f != null)
                            {
                                var data = NewRequestDAL.UploadFile(item, f.FilePath);

                                if (data != null && data.result)
                                {
                                    foreach (var st in staticAttachment.Where(w => w.FileName == item.FileName))
                                    {
                                        st.FilePath = data.path;
                                    }
                                }
                            }
                        }
                    }

                    AttachmentJson table = new AttachmentJson();
                    table.Id = jsonId;
                    table.StaticTB = staticAttachment;

                    if (ApproveRejctdata.CompleteDeptProcess == true)
                    {
                        var updateData = db.UarRequestmaster.Where(x => x.Id == ApproveRejctdata.RequestMasterId).FirstOrDefault();
                        updateData.ApprovalAttachment = JsonConvert.SerializeObject(table);
                        db.SaveChanges();
                    }
                    else
                    {
                        var updateData = db.UarRequestDetails.Where(x => x.Id == ApproveRejctdata.RequestDetailsId).FirstOrDefault();
                        updateData.Attachment = JsonConvert.SerializeObject(table);
                        db.SaveChanges();
                    }

                    //for audit trial
                    masauditdto.existsentry = ApproveRejctdata.RequestMasterId;
                    auditjsonmdl.filelist = staticAttachment;
                    auditjsonmdl.Systemremarks = ApproveRejctdata.Remark;

                    List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                    auditdto1.Add(auditjsonmdl);
                    masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                    masauditdto.createddate = DateTime.Now;
                    masauditdto.RequestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                    var masterDataaudi = db.UarRequestmaster.Where(x => x.Id == ApproveRejctdata.RequestMasterId).FirstOrDefault();
                    if (masterDataaudi.Status == "Closed")
                    {
                        AuditTrialJSONDTO auditjsonmdl1 = new AuditTrialJSONDTO();
                        AuditTrialDTO masauditdto1 = new AuditTrialDTO();
                        List<AccessType> accessType1 = new List<AccessType>();
                        string content = "";
                        List<string> output = new List<string>();
                        var accty1 = new AccessType();
                        content = "Request Id=" + ApproveRejctdata.RequestMasterId + " Closed";
                        output.Add(content);
                        accty1.categorydescription = output;
                        accessType1.Add(accty1);
                        var usernamee = db.User.Where(x => x.Id == ApproveRejctdata.LoginUserID).Select(x => x.FirstName).FirstOrDefault();
                        auditjsonmdl1.accesstype = accessType1;

                        auditjsonmdl1.browser = browser;
                        auditjsonmdl1.eventname = "Closed";
                        auditjsonmdl1.module = "Close Request ";
                        auditjsonmdl1.userid = ApproveRejctdata.LoginUserID;
                        //  auditjsonmdl.categorydescription = description;
                        auditjsonmdl1.requestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                        auditjsonmdl1.Systemremarks = "";
                        auditjsonmdl1.username = usernamee;
                        auditjsonmdl1.ipaddress = ipaddress;
                        auditjsonmdl1.createddate = DateTime.Now;
                        //for audit trial
                        masauditdto1.existsentry = ApproveRejctdata.RequestMasterId;
                        auditjsonmdl1.filelist = staticAttachment;
                        auditjsonmdl1.Systemremarks = "";

                        List<AuditTrialJSONDTO> auditdto2 = new List<AuditTrialJSONDTO>();
                        auditdto2.Add(auditjsonmdl1);
                        masauditdto1.description = JsonConvert.SerializeObject(auditdto2);
                        masauditdto1.createddate = DateTime.Now;
                        masauditdto1.RequestId = Convert.ToString(ApproveRejctdata.RequestMasterId);
                        AuditTrialDAL.CreateAuditTrial(masauditdto1, auditjsonmdl1);
                    }

                }

                resut.Status = true;
                return resut;
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static FinalResultDTO GetListOfRequestToApprove(Int32 userID)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<ApprovalRequestDTO> levelAvail = new List<ApprovalRequestDTO>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<UserLevels> userLevels = new List<UserLevels>();
                    FinalResultDTO userDeptData = GetUserLevelForDeptWF(userID);
                    if (userDeptData.Status)
                    {
                        userLevels = userDeptData.ResultOP;
                    }

                    foreach (var item in userLevels)
                    {
                        List<ApprovalRequestDTO> requestMasterData = (from NR in db.UarRequestmaster
                                                                      join employee in db.User on NR.EmployeeId equals employee.Id
                                                                      where NR.WorkflowStage == "Department"
                                                                      && NR.EmployeeId == item.UserId && NR.Level == item.Level
                                                                      && (NR.Status == "Open" || NR.Status == "Processing")
                                                                      select new ApprovalRequestDTO
                                                                      {
                                                                          RequestMasterId = NR.Id,
                                                                          EmployeeId = employee.Id,
                                                                          EmployeeName = employee.FirstName,
                                                                          RequestId = NR.Id,
                                                                          Level = Convert.ToInt64(NR.Level),
                                                                          WorkFlowStage = NR.WorkflowStage,
                                                                          Status = NR.Status,
                                                                          CompleteDeptProcess = true,
                                                                          SlaData = NR.SlaData
                                                                      }).ToList();

                        foreach (var temp in requestMasterData)
                        {
                            levelAvail.Add(temp);
                        }

                        List<ApprovalRequestDTO> requestDetailsData = new List<ApprovalRequestDTO>();

                        requestDetailsData = (from NR in db.UarRequestmaster
                                             // join removeSameRequset in requestMasterData on NR.Id equals removeSameRequset.RequestMasterId
                                              join NRD in db.UarRequestDetails on NR.Id equals NRD.RequestId
                                              join employee in db.User on NR.EmployeeId equals employee.Id
                                              where NRD.WorkflowStage == "Department"
                                              && NR.EmployeeId == item.UserId && NRD.Level == item.Level
                                              && NRD.Status == "Processing"
                                              select new ApprovalRequestDTO
                                              {
                                                  RequestMasterId = NR.Id,
                                                  RequestDetailsId = NRD.Id,
                                                  EmployeeId = employee.Id,
                                                  EmployeeName = employee.FirstName,
                                                  RequestId = NR.Id,
                                                  Level = Convert.ToInt64(NRD.Level),
                                                  WorkFlowStage = NRD.WorkflowStage,
                                                  Status = NRD.Status,
                                                  CompleteDeptProcess = false,
                                                  SlaData = NRD.SlaData
                                              }).ToList();


                        foreach (var temp in requestDetailsData)
                        {
                            if (!requestMasterData.Any(x => x.RequestMasterId == temp.RequestMasterId))
                            {
                                levelAvail.Add(temp);
                            }
                        }

                    }

                    var userITAccess = db.User.Where(x => x.Id == userID).FirstOrDefault();
                    if (userITAccess != null && userITAccess.ItProcessAccess == "Y")
                    {
                        List<WorkFlowUserDetails> userItLevels = new List<WorkFlowUserDetails>();
                        FinalResultDTO userItData = GetUserLevelForITWF(userID);
                        if (userItData.Status)
                        {
                            userItLevels = userItData.ResultOP;
                        }

                        foreach (var item in userItLevels)
                        {
                            long LevelNum = item.Level;
                            long subcatId = item.SubCategoryID;
                            long catId = item.CategoryID;

                            var templateNames = (from NR in db.UarRequestmaster
                                                 join NRD in db.UarRequestDetails on NR.Id equals NRD.RequestId
                                                 join lookUpsub in db.LookupSubitem on NRD.SubCategoryId equals lookUpsub.Id
                                                 join lookup in db.LookupItem on NRD.AccessCategoryId equals lookup.Id
                                                 join lookupacTyp in db.LookupItem on NRD.AccessTypeId equals lookupacTyp.Id
                                                 join user in db.User on NRD.UserId equals user.Id
                                                 join employee in db.User on NR.EmployeeId equals employee.Id
                                                 where NRD.SubCategoryId == subcatId && NRD.AccessCategoryId == catId
                                                 && NRD.Level == LevelNum && NRD.WorkflowStage == "IT"
                                                 && NRD.Status == "Processing" && NR.EmployeeId != userID
                                                 select new ApprovalRequestDTO
                                                 {
                                                     RequestMasterId = NR.Id,
                                                     RequestDetailsId = NRD.Id,
                                                     EmployeeId = employee.Id,
                                                     EmployeeName = employee.FirstName,
                                                     UserID = user.Id,
                                                     UserName = user.FirstName,
                                                     RequestId = NRD.RequestId,
                                                     AccessCategoryname = lookup.Key,
                                                     AccessCategoryId = NRD.AccessCategoryId,
                                                     SubaccessCategoryname = lookUpsub.Key,
                                                     SubaccessCategoryId = NRD.SubCategoryId,
                                                     AccessTypename = lookupacTyp.Key,
                                                     AccessTypeId = NRD.AccessTypeId,
                                                     Level = NRD.Level,
                                                     WorkFlowStage = NRD.WorkflowStage,
                                                     CompleteDeptProcess = false,
                                                     Status = NRD.Status,
                                                     SlaData = NRD.SlaData
                                                 }).ToList();
                            foreach (var temp in templateNames)
                            {
                                levelAvail.Add(temp);
                            }
                        }
                    }

                    foreach (var item in levelAvail)
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
                    levelAvail = levelAvail.OrderByDescending(x => x.RequestMasterId).ToList();
                    result.Status = true;
                    result.ResultOP = levelAvail;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static FinalResultDTO GetUserLevelForDeptWF(Int32 userId)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<UserLevels> userLevels = new List<UserLevels>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var deptDetails = (from dept in db.Department
                                       join user in db.User on dept.Id equals user.Department
                                       where user.Id == userId
                                       select new DepartmentDTO
                                       {
                                           Id = dept.Id,
                                           NoOfLevel = dept.NoOfLevel,
                                           SlaFlag = dept.SlaFlag,
                                           SlaJson = dept.SlaJson,
                                       }).FirstOrDefault();

                    if (deptDetails == null)
                    {
                        result.Status = false;
                        return result;
                    }

                    var userdata = db.User.Where(x => x.Department == deptDetails.Id).ToList();
                    if (userdata.Count > 0)
                    {
                        bool slaFlag = deptDetails.SlaFlag == "Y" ? true : false;
                        Int32 sladays = 0;
                        List<SlaJson> slaList = new List<SlaJson>();
                        if (slaFlag)
                        {
                            slaList = JsonConvert.DeserializeObject<List<SlaJson>>(deptDetails.SlaJson);
                        }

                        sladays = slaList.Where(x => x.levelNo == 1).Select(x => x.slaDays).FirstOrDefault();
                        List<UserLevels> data = new List<UserLevels>();
                        data = (from userList in userdata
                                where userList.ImmediateSupervisor == userId && userList.Id != userId
                                && userList.DeleteFlag == "N"
                                select new UserLevels
                                {
                                    Level = 1,
                                    UserId = Convert.ToInt32(userList.Id),
                                    FirstName = userList.FirstName,
                                    Supervisor = Convert.ToInt32(userList.ImmediateSupervisor),
                                    SlaFlag = slaFlag,
                                    SlaDays = sladays
                                }).ToList();

                        foreach (var users in data)
                        {
                            userLevels.Add(users);
                        }

                        if (data.Count >= 1)
                        {
                            for (int i = 2; i <= deptDetails.NoOfLevel; i++)
                            {
                                sladays = 0;
                                sladays = slaList.Where(x => x.levelNo == i).Select(x => x.slaDays).FirstOrDefault();
                                var userLevelTemp = (from userList in userdata
                                                     join dt in data on userList.ImmediateSupervisor equals dt.UserId
                                                     where userList.DeleteFlag == "N"
                                                     select new UserLevels
                                                     {
                                                         UserId = Convert.ToInt32(userList.Id),
                                                         Level = i,
                                                         FirstName = userList.FirstName,
                                                         Supervisor = Convert.ToInt32(userList.ImmediateSupervisor),
                                                         SlaFlag = slaFlag,
                                                         SlaDays = sladays
                                                     }).ToList();

                                foreach (var users in userLevelTemp)
                                {
                                    userLevels.Add(users);
                                }
                                data = new List<UserLevels>();
                                data = userLevelTemp;
                                if (data.Count <= 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                result.Status = true;
                result.ResultOP = userLevels;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }

        }

        public static dynamic GetUserLevelForITWF(Int32 userId)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<WorkFlowUserDetails> workFlows = new List<WorkFlowUserDetails>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var completeWFS = db.Workflowcategory.ToList();

                    foreach (var WFS in completeWFS)
                    {
                        var levelDetails = db.Workflowdetails.Where(x => x.CombinationId == WFS.Id && x.DeleteFlag == "N" && x.ActiveFlag == "Y").ToList();
                        levelDetails = levelDetails.OrderBy(x => x.Level).ToList();

                        foreach (var item in levelDetails)
                        {
                            var SelectedUsers = JsonConvert.DeserializeObject<List<UsersDTO>>(item.SelectedUsers);
                            var SelectedRoles = JsonConvert.DeserializeObject<List<DropDownsDTO>>(item.SelectedRoles);

                            var wfUserData = (from roles in SelectedRoles
                                              join users in SelectedUsers on roles.Id equals users.UserRoleId
                                              select new WorkFlowUserDetails
                                              {
                                                  Level = Convert.ToInt32(item.Level),
                                                  LevelName = item.LevelName,
                                                  CategoryID = Convert.ToInt32(WFS.CategoryId),
                                                  SubCategoryID = Convert.ToInt32(WFS.SubCategoryId),
                                                  User = users.UserFirstName,
                                                  UserID = users.Id,
                                                  UserRole = roles.key,
                                                  SlaFlag = item.SlaActive == "Y" ? true : false,
                                                  SlaDays = item.SlaDays
                                              }).ToList();

                            foreach (var finalData in wfUserData)
                            {
                                workFlows.Add(finalData);
                            }
                        }
                    }
                    workFlows = workFlows.Where(x => x.UserID == userId).ToList();
                    result.Status = true;
                    result.ResultOP = workFlows;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching neccessary details.";
                return result;
            }
        }

        public static dynamic GetImmediateSupervisorForUSer(Int32 userId, Int32 currentLevel)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<UserLevels> userLevels = new List<UserLevels>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var deptDetails = (from dept in db.Department
                                       join user in db.User on dept.Id equals user.Department
                                       where user.Id == userId
                                       select new DepartmentDTO
                                       {
                                           Id = dept.Id,
                                           NoOfLevel = dept.NoOfLevel,
                                           SlaFlag = dept.SlaFlag,
                                           SlaJson = dept.SlaJson,
                                       }).FirstOrDefault();

                    if (deptDetails == null)
                    {
                        result.Status = false;
                        return result;
                    }

                    if (deptDetails.NoOfLevel < currentLevel)
                    {
                        result.Status = true;
                        result.ResultOP = userLevels;
                        return result;
                    }

                    var userdata = db.User.Where(x => x.Department == deptDetails.Id).ToList();
                    if (userdata.Count > 0)
                    {
                        bool slaFlag = deptDetails.SlaFlag == "Y" ? true : false;
                        Int32 sladays = 0;
                        List<SlaJson> slaList = new List<SlaJson>();
                        if (slaFlag)
                        {
                            slaList = JsonConvert.DeserializeObject<List<SlaJson>>(deptDetails.SlaJson);
                        }

                        sladays = slaList.Where(x => x.levelNo == currentLevel).Select(x => x.slaDays).FirstOrDefault();

                        userLevels = (from userList in userdata
                                      join userSupervisor in userdata on userList.ImmediateSupervisor equals userSupervisor.Id
                                      where userList.Id == userId && userSupervisor.Id != userId
                                      select new UserLevels
                                      {
                                          Level = currentLevel,
                                          UserId = Convert.ToInt32(userSupervisor.Id),
                                          FirstName = userSupervisor.FirstName,
                                          Supervisor = Convert.ToInt32(userSupervisor.ImmediateSupervisor),
                                          SlaFlag = slaFlag,
                                          SlaDays = sladays
                                      }).ToList();
                    }
                }

                result.Status = true;
                result.ResultOP = userLevels;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static dynamic GetImmediateSupervisorForIT(Int32 currentLevel, Int32 categoryId, Int32 subCategory)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<UserLevels> userLevels = new List<UserLevels>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {

                    var combinationDetails = db.Workflowcategory.Where(x => x.SubCategoryId == subCategory && x.CategoryId == categoryId).FirstOrDefault();

                    if (combinationDetails == null || combinationDetails.TotalLevel < currentLevel)
                    {
                        result.Status = true;
                        result.ResultOP = userLevels;
                        return result;
                    }

                    var data = db.Workflowdetails.Where(x => x.CombinationId == combinationDetails.Id && x.Level == currentLevel && x.DeleteFlag == "N" && x.ActiveFlag == "Y").FirstOrDefault();

                    var users = JsonConvert.DeserializeObject<List<UsersDTO>>(data.SelectedUsers);
                    foreach (var item in users)
                    {
                        userLevels.Add(new UserLevels
                        {
                            Level = currentLevel,
                            FirstName = item.UserFirstName,
                            UserId = item.Id,
                            SlaDays = data.SlaDays,
                            SlaFlag = data.SlaActive == "Y" ? true : false
                        });
                    }
                }

                result.Status = true;
                result.ResultOP = userLevels;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static dynamic GetSLATargetDate(Int32 SlaDays, DateTime startDate)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<DateTime> holidays = new List<DateTime>();
                    DateTime end = startDate;
                    var holidayList = db.Holidayplanner.Where(x => Convert.ToDateTime(x.Startdate) > startDate || Convert.ToDateTime(x.Enddate) > startDate).ToList();
                    foreach (var item in holidayList)
                    {
                        DateTime srt = Convert.ToDateTime(item.Startdate);
                        end = Convert.ToDateTime(item.Enddate);
                        for (DateTime i = srt; i <= end; i = i.AddDays(1.0))
                        {
                            if (!holidays.Contains(i))
                            {
                                holidays.Add(i.Date);
                            }
                        }
                    }

                    DateTime checKDate = startDate.AddDays(SlaDays);
                    if (checKDate > end)
                    {
                        end = checKDate;
                    }
                    var workingDays = db.Weekdays.Where(x => x.Isactive == 0).ToList();

                    for (DateTime i = startDate; i <= end; i = i.AddDays(1.0))
                    {
                        string date = i.ToString("dddd");
                        if (workingDays.Any(x => x.Days == date))
                        {
                            if (!holidays.Contains(i))
                            {
                                holidays.Add(i.Date);
                            }
                        }
                    }

                    DateTime firstDate = startDate;
                    DateTime targetDate = startDate.AddDays(SlaDays);

                    var newList = holidays.Where(x => x >= firstDate && x <= targetDate).ToList();
                    firstDate = targetDate;
                    if (newList.Count >= 1)
                    {
                        Int32 holidayCount = newList.Count;
                        DateTime NewEndDate = targetDate.AddDays(holidayCount);
                        for (DateTime i = targetDate; i <= NewEndDate; i = i.AddDays(1.0))
                        {
                            string date = i.ToString("dddd");
                            if (workingDays.Any(x => x.Days == date))
                            {
                                holidayCount = holidayCount + 1;
                            }
                        }
                        targetDate = targetDate.AddDays(holidayCount);
                    }

                    result.ResultOP = targetDate.Date;
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static dynamic CalculateRemainingSLA(SLADetails SLAData)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    FinalResultDTO getSlaTargetDate = GetSLATargetDate(SLAData.SlaDays, SLAData.ApprovalDate);
                    if (getSlaTargetDate.Status)
                    {
                        SLAData.SlaTargetDate = getSlaTargetDate.ResultOP;
                    }

                    List<DateTime> holidays = new List<DateTime>();
                    List<DateTime> workingdays = new List<DateTime>();
                    var holidayList = db.Holidayplanner.Where(x => Convert.ToDateTime(x.Startdate) > SLAData.ApprovalDate || Convert.ToDateTime(x.Enddate) > SLAData.ApprovalDate).ToList();
                    foreach (var item in holidayList)
                    {
                        DateTime srt = Convert.ToDateTime(item.Startdate);
                        DateTime end = Convert.ToDateTime(item.Enddate);
                        for (DateTime i = srt; i <= end; i = i.AddDays(1.0))
                        {
                            if (!holidays.Contains(i))
                            {
                                holidays.Add(i.Date);
                            }
                        }
                    }

                    DateTime tempEndDate = SLAData.SlaTargetDate.AddDays(SLAData.SlaDays);
                    if (tempEndDate < DateTime.Now)
                    {
                        tempEndDate = DateTime.Now;
                    }
                    var weekEndList = db.Weekdays.Where(x => x.Isactive == 0).ToList();
                    for (DateTime i = SLAData.ApprovalDate; i <= tempEndDate; i = i.AddDays(1.0))
                    {
                        string date = i.ToString("dddd");
                        if (weekEndList.Any(x => x.Days == date))
                        {
                            if (!holidays.Contains(i))
                            {
                                holidays.Add(i.Date);
                            }
                        }
                        else
                        {
                            if (!holidays.Contains(i))
                            {
                                if (!workingdays.Contains(i))
                                {
                                    workingdays.Add(i.Date);
                                }
                            }
                        }
                    }

                    if (DateTime.Now.Date <= SLAData.SlaTargetDate)
                    {
                        SLAData.OverDueFlag = false;
                        var btwnWorkingDays = workingdays.Where(x => x >= DateTime.Now && x <= SLAData.SlaTargetDate).Count();
                        SLAData.RemainingSLADays = btwnWorkingDays;
                    }
                    else
                    {
                        SLAData.OverDueFlag = true;
                        var overdueCount = workingdays.Where(x => x > SLAData.SlaTargetDate && x <= DateTime.Now).Count();
                        SLAData.OverDueDays = overdueCount;
                    }

                    result.ResultOP = SLAData;
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static FinalResultDTO GetRequestIdDetailsForApproval(LoadRequestData requestDetais)
        {
            FinalResultDTO resut = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<ApproveRequestDTO> templateNames = new List<ApproveRequestDTO>();
                    if (requestDetais.CompleteDeptProcess == true)
                    {
                        templateNames = (from NR in db.UarRequestmaster
                                         join NRD in db.UarRequestDetails on NR.Id equals NRD.RequestId
                                         join lookUpsub in db.LookupSubitem on NRD.SubCategoryId equals lookUpsub.Id
                                         join lookup in db.LookupItem on NRD.AccessCategoryId equals lookup.Id
                                         join lookupacTyp in db.LookupItem on NRD.AccessTypeId equals lookupacTyp.Id
                                         join user in db.User on NRD.UserId equals user.Id
                                         join userRequest in db.User on NR.EmployeeId equals userRequest.Id
                                         where NR.Id == requestDetais.RequestId && NRD.DeleteFlag != "Y"
                                         select new ApproveRequestDTO
                                         {
                                             id = NRD.Id,
                                             subRequestId = NRD.RequestDetailsId,
                                             userID = user.Id,
                                             userName = userRequest.FirstName,
                                             requestId = NRD.RequestId,
                                             accessCategoryname = lookup.Value,
                                             accessCategoryId = NRD.AccessCategoryId,
                                             subaccessCategoryname = lookUpsub.Value,
                                             subaccessCategoryId = NRD.SubCategoryId,
                                             accessTypename = lookupacTyp.Value,
                                             accessTypeId = NRD.AccessTypeId,
                                             workflowStage = NRD.WorkflowStage,
                                             userIdDll = user.FirstName,
                                             level = NR.Level,
                                             status = NR.Status,
                                             SlaData = NRD.SlaData,
                                         }).ToList();
                    }
                    else
                    {
                        templateNames = (from NR in db.UarRequestmaster
                                         join NRD in db.UarRequestDetails on NR.Id equals NRD.RequestId
                                         join lookUpsub in db.LookupSubitem on NRD.SubCategoryId equals lookUpsub.Id
                                         join lookup in db.LookupItem on NRD.AccessCategoryId equals lookup.Id
                                         join lookupacTyp in db.LookupItem on NRD.AccessTypeId equals lookupacTyp.Id
                                         join user in db.User on NRD.UserId equals user.Id
                                         join userRequest in db.User on NR.EmployeeId equals userRequest.Id
                                         where NRD.Id == requestDetais.RequestDetailsId && NRD.DeleteFlag != "Y"
                                         select new ApproveRequestDTO
                                         {
                                             id = NRD.Id,
                                             subRequestId = NRD.RequestDetailsId,
                                             userID = user.Id,
                                             userName = userRequest.FirstName,
                                             requestId = NRD.RequestId,
                                             accessCategoryname = lookup.Value,
                                             accessCategoryId = NRD.AccessCategoryId,
                                             subaccessCategoryname = lookUpsub.Value,
                                             subaccessCategoryId = NRD.SubCategoryId,
                                             accessTypename = lookupacTyp.Value,
                                             accessTypeId = NRD.AccessTypeId,
                                             workflowStage = NRD.WorkflowStage,
                                             userIdDll = user.FirstName,
                                             level = Convert.ToInt32(NRD.Level),
                                             status = NRD.Status,
                                             SlaData = NRD.SlaData,
                                         }).ToList();
                    }

                    foreach (var item in templateNames)
                    {
                        string users = string.Empty;
                        if (item.status == "Processing" || item.status == "Open")
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

                    resut.ResultOP = templateNames;
                    resut.Status = true;
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


        public static dynamic FetchStepperDetails(Int32 RequestMasterID)
        {
            FinalResultDTO result = new FinalResultDTO();
            FinalResultDTO CalRemainingSLAObj = new FinalResultDTO();
            StepperObj completeData = new StepperObj();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RequestDetailsObj> ITFlows = new List<RequestDetailsObj>();
                    var requestMasterData = db.UarRequestmaster.Where(x => x.Id == RequestMasterID).FirstOrDefault();

                    if (requestMasterData == null)
                    {
                        result.Status = false;
                        result.Description = "Request Id details are missing, Please contact Admin for more details...";
                        return result;
                    }

                    completeData.RequestMasterId = Convert.ToInt32(requestMasterData.Id);
                    completeData.DepartmentWFList = JsonConvert.DeserializeObject<List<ApproveDetailsJsonNew>>(requestMasterData.ApprovalDetails);

                    FinalResultDTO levelWiseUsers = new FinalResultDTO();
                    List<UserLevels> userLevels = new List<UserLevels>();
                    SLADetails slaData = new SLADetails();

                    if (requestMasterData.WorkflowStage == "Department")
                    {
                        if (requestMasterData.Status != "Rejected")
                        {
                            var slaDetails = JsonConvert.DeserializeObject<SLADetails>(requestMasterData.SlaData);
                            slaDetails.LevelNo = requestMasterData.Level;
                            if (slaDetails.SlaFlag)
                            {
                                CalRemainingSLAObj = CalculateRemainingSLA(slaDetails);
                                if (CalRemainingSLAObj.Status)
                                {
                                    slaDetails = CalRemainingSLAObj.ResultOP;
                                }
                            }
                            completeData.DepartmentWFList.Add(new ApproveDetailsJsonNew
                            {
                                WorkFlowStage = "Department",
                                SLAData = slaDetails,
                                status = "Current",
                                Level = slaDetails.LevelNo
                            });

                            List<SLADetails> levelWiseDeptUsers = new List<SLADetails>();
                            Int32 deptCycleTopLevel = 0;
                            FinalResultDTO deptCycleLevel = GetDeptWFCycle(Convert.ToInt32(requestMasterData.EmployeeId));
                            if (deptCycleLevel.Status)
                            {
                                levelWiseDeptUsers = deptCycleLevel.ResultOP;
                                deptCycleTopLevel = levelWiseDeptUsers.OrderByDescending(x => x.LevelNo).Select(x => x.LevelNo).FirstOrDefault();
                            }

                            for (int i = (requestMasterData.Level + 1); i <= deptCycleTopLevel; i++)
                            {
                                completeData.DepartmentWFList.Add(new ApproveDetailsJsonNew
                                {
                                    WorkFlowStage = "Department",
                                    status = "Upcoming",
                                    Level = i,
                                    SLAData = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault()
                                });
                            }
                        }
                    }
                    else
                    {
                        completeData.ITWFFlag = true;
                        var requestDetailsData = db.UarRequestDetails.Where(x => x.RequestId == requestMasterData.Id && x.DeleteFlag != "Y").ToList();
                        foreach (var item in requestDetailsData)
                        {
                            RequestDetailsObj dat = new RequestDetailsObj();
                            dat.RequestDetailSNId = item.RequestDetailsId;
                            dat.RequestDetailId = Convert.ToInt32(item.Id);

                            dat.ApprovalDetails = new List<ApproveDetailsJsonNew>();
                            if (item.ApprovalDetails != null)
                            {
                                dat.ApprovalDetails = JsonConvert.DeserializeObject<List<ApproveDetailsJsonNew>>(item.ApprovalDetails);
                            }

                            if (item.Status != "Approved" && item.Status != "Rejected")
                            {
                                List<SLADetails> levelWiseDeptUsers = new List<SLADetails>();
                                Int32 topLevel = 0;
                                if (item.WorkflowStage == "IT")
                                {
                                    var completeWFS = db.Workflowcategory.Where(x => x.CategoryId == item.AccessCategoryId && x.SubCategoryId == item.SubCategoryId).FirstOrDefault();
                                    var completeWFSDetails = db.Workflowdetails.Where(x => x.CombinationId == completeWFS.Id && x.DeleteFlag != "Y" && x.ActiveFlag != "N").OrderBy(x => x.Level).ToList();

                                    foreach (var ITUsers in completeWFSDetails)
                                    {
                                        var users = JsonConvert.DeserializeObject<List<UsersDTO>>(ITUsers.SelectedUsers);
                                        List<DropDownsDTO> userList = new List<DropDownsDTO>();
                                        foreach (var USR in users)
                                        {
                                            userList.Add(new DropDownsDTO
                                            {
                                                Id = USR.Id,
                                                key = USR.UserFirstName
                                            });
                                        }

                                        levelWiseDeptUsers.Add(new SLADetails
                                        {
                                            LevelNo = Convert.ToInt32(ITUsers.Level),
                                            Users = userList,
                                            SlaDays = ITUsers.SlaDays,
                                            SlaFlag = ITUsers.SlaActive == "Y" ? true : false
                                        });
                                    }
                                    topLevel = completeWFS.TotalLevel;
                                }
                                else
                                {
                                    FinalResultDTO deptCycleLevel = GetDeptWFCycle(Convert.ToInt32(requestMasterData.EmployeeId));
                                    if (deptCycleLevel.Status)
                                    {
                                        levelWiseDeptUsers = deptCycleLevel.ResultOP;
                                        topLevel = levelWiseDeptUsers.OrderByDescending(x => x.LevelNo).Select(x => x.LevelNo).FirstOrDefault();
                                    }
                                }

                                var slaDetails = JsonConvert.DeserializeObject<SLADetails>(item.SlaData);
                                slaDetails.LevelNo = Convert.ToInt32(item.Level);
                                if (slaDetails.SlaFlag)
                                {
                                    CalRemainingSLAObj = CalculateRemainingSLA(slaDetails);
                                    if (CalRemainingSLAObj.Status)
                                    {
                                        slaDetails = CalRemainingSLAObj.ResultOP;
                                    }
                                }
                                dat.ApprovalDetails.Add(new ApproveDetailsJsonNew
                                {
                                    WorkFlowStage = item.WorkflowStage,
                                    SLAData = slaDetails,
                                    status = "Current",
                                    Level = slaDetails.LevelNo
                                });

                                for (int i = (Convert.ToInt32(item.Level) + 1); i <= topLevel; i++)
                                {
                                    SLADetails slaDatas = new SLADetails();
                                    if (item.WorkflowStage == "Department")
                                    {
                                        slaDatas = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault();
                                    }
                                    else
                                    {
                                        slaDatas = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault();
                                    }

                                    dat.ApprovalDetails.Add(new ApproveDetailsJsonNew
                                    {
                                        WorkFlowStage = item.WorkflowStage,
                                        status = "Upcoming",
                                        Level = i,
                                        SLAData = slaDatas

                                    });
                                }
                            }
                            ITFlows.Add(dat);
                        }
                    }
                    completeData.ITWFList = ITFlows;
                }

                result.ResultOP = completeData;
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }

        public static dynamic GetDeptWFCycle(Int32 userId)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<SLADetails> levelWiseEmployee = new List<SLADetails>();
            Int32 level = 0;
            try
            {
                using (dbURSContext db = new dbURSContext())
                {

                    var deptDetails = (from dept in db.Department
                                       join user in db.User on dept.Id equals user.Department
                                       where user.Id == userId
                                       select new DepartmentDTO
                                       {
                                           Id = dept.Id,
                                           NoOfLevel = dept.NoOfLevel,
                                           SlaFlag = dept.SlaFlag,
                                           SlaJson = dept.SlaJson,
                                       }).FirstOrDefault();

                    if (deptDetails == null)
                    {
                        result.Status = true;
                        result.ResultOP = level;
                        return result;
                    }

                    Int32 deptId = Convert.ToInt32(deptDetails.Id);
                    bool slaFlag = deptDetails.SlaFlag == "Y" ? true : false;
                    Int32 sladays = 0;
                    List<SlaJson> slaList = new List<SlaJson>();
                    if (slaFlag)
                    {
                        slaList = JsonConvert.DeserializeObject<List<SlaJson>>(deptDetails.SlaJson);
                    }

                    var userdata = db.User.Where(x => x.Department == deptId).ToList();

                    bool flag = false;
                    Int32 empManagerID = 0;
                    var empDetail = userdata.Where(x => x.Id == userId).FirstOrDefault();
                    if (empDetail != null && empDetail.ImmediateSupervisor != userId)
                    {
                        level = level + 1;
                        var users = new List<DropDownsDTO>();
                        users.Add(new DropDownsDTO()
                        {
                            Id = Convert.ToInt32(empDetail.ImmediateSupervisor),
                            key = userdata.Where(x => x.Id == empDetail.ImmediateSupervisor).Select(x => x.FirstName).FirstOrDefault()
                        });

                        sladays = slaList.Where(x => x.levelNo == level).Select(x => x.slaDays).FirstOrDefault();
                        levelWiseEmployee.Add(
                            new SLADetails
                            {
                                LevelNo = level,
                                SlaFlag = slaFlag,
                                SlaDays = sladays,
                                Users = users
                            }
                        );
                        empManagerID = Convert.ToInt32(empDetail.ImmediateSupervisor);
                    }
                    else
                    {
                        result.Status = true;
                        result.ResultOP = levelWiseEmployee;
                        return result;
                    }

                    while (!flag)
                    {
                        var dt = userdata.Where(x => x.Id == empManagerID).FirstOrDefault();
                        if (dt != null && dt.ImmediateSupervisor != empManagerID)
                        {
                            level = level + 1;
                            var users = new List<DropDownsDTO>();
                            users.Add(new DropDownsDTO()
                            {
                                Id = Convert.ToInt32(dt.ImmediateSupervisor),
                                key = userdata.Where(x => x.Id == dt.ImmediateSupervisor).Select(x => x.FirstName).FirstOrDefault()
                            });
                            sladays = slaList.Where(x => x.levelNo == level).Select(x => x.slaDays).FirstOrDefault();
                            levelWiseEmployee.Add(
                            new SLADetails
                            {
                                LevelNo = level,
                                SlaFlag = slaFlag,
                                SlaDays = sladays,
                                Users = users
                            });
                            empManagerID = Convert.ToInt32(dt.ImmediateSupervisor);
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                }

                result.Status = true;
                result.ResultOP = levelWiseEmployee;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }


        public static dynamic FetchStepperDetailsApproval(LoadRequestData requestDetails)
        {
            FinalResultDTO result = new FinalResultDTO();
            FinalResultDTO CalRemainingSLAObj = new FinalResultDTO();
            StepperObj completeData = new StepperObj();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RequestDetailsObj> ITFlows = new List<RequestDetailsObj>();
                    var requestMasterData = db.UarRequestmaster.Where(x => x.Id == requestDetails.RequestMasterId).FirstOrDefault();

                    if (requestMasterData == null)
                    {
                        result.Status = false;
                        result.Description = "Request Id details are missing, Please contact Admin for more details...";
                        return result;
                    }

                    completeData.RequestMasterId = Convert.ToInt32(requestMasterData.Id);
                    completeData.DepartmentWFList = JsonConvert.DeserializeObject<List<ApproveDetailsJsonNew>>(requestMasterData.ApprovalDetails);

                    FinalResultDTO levelWiseUsers = new FinalResultDTO();
                    List<UserLevels> userLevels = new List<UserLevels>();
                    SLADetails slaData = new SLADetails();

                    if (requestMasterData.WorkflowStage == "Department")
                    {
                        if (requestMasterData.Status != "Rejected")
                        {
                            var slaDetails = JsonConvert.DeserializeObject<SLADetails>(requestMasterData.SlaData);
                            slaDetails.LevelNo = requestMasterData.Level;
                            if (slaDetails.SlaFlag)
                            {
                                CalRemainingSLAObj = CalculateRemainingSLA(slaDetails);
                                if (CalRemainingSLAObj.Status)
                                {
                                    slaDetails = CalRemainingSLAObj.ResultOP;
                                }
                            }
                            completeData.DepartmentWFList.Add(new ApproveDetailsJsonNew
                            {
                                WorkFlowStage = "Department",
                                SLAData = slaDetails,
                                status = "Current",
                                Level = slaDetails.LevelNo
                            });

                            List<SLADetails> levelWiseDeptUsers = new List<SLADetails>();
                            Int32 deptCycleTopLevel = 0;
                            FinalResultDTO deptCycleLevel = GetDeptWFCycle(Convert.ToInt32(requestMasterData.EmployeeId));
                            if (deptCycleLevel.Status)
                            {
                                levelWiseDeptUsers = deptCycleLevel.ResultOP;
                                deptCycleTopLevel = levelWiseDeptUsers.OrderByDescending(x => x.LevelNo).Select(x => x.LevelNo).FirstOrDefault();
                            }

                            for (int i = (requestMasterData.Level + 1); i <= deptCycleTopLevel; i++)
                            {
                                completeData.DepartmentWFList.Add(new ApproveDetailsJsonNew
                                {
                                    WorkFlowStage = "Department",
                                    status = "Upcoming",
                                    Level = i,
                                    SLAData = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault()
                                });
                            }
                        }
                    }
                    else
                    {
                        completeData.ITWFFlag = true;

                        List<UarRequestDetails> requestDetailsData = new List<UarRequestDetails>();
                        if (requestDetails.CompleteDeptProcess)
                        {
                            requestDetailsData = db.UarRequestDetails.Where(x => x.RequestId == requestMasterData.Id).ToList();
                        }
                        else
                        {
                            requestDetailsData = db.UarRequestDetails.Where(x => x.Id == requestDetails.RequestDetailsId).ToList();
                        }

                        foreach (var item in requestDetailsData)
                        {
                            RequestDetailsObj dat = new RequestDetailsObj();
                            dat.RequestDetailSNId = item.RequestDetailsId;
                            dat.RequestDetailId = Convert.ToInt32(item.Id);

                            dat.ApprovalDetails = new List<ApproveDetailsJsonNew>();
                            if (item.ApprovalDetails != null)
                            {
                                dat.ApprovalDetails = JsonConvert.DeserializeObject<List<ApproveDetailsJsonNew>>(item.ApprovalDetails);
                            }

                            if (item.Status != "Approved" && item.Status != "Rejected")
                            {
                                List<SLADetails> levelWiseDeptUsers = new List<SLADetails>();
                                Int32 topLevel = 0;
                                if (item.WorkflowStage == "IT")
                                {
                                    var completeWFS = db.Workflowcategory.Where(x => x.CategoryId == item.AccessCategoryId && x.SubCategoryId == item.SubCategoryId).FirstOrDefault();
                                    var completeWFSDetails = db.Workflowdetails.Where(x => x.CombinationId == completeWFS.Id && x.DeleteFlag != "Y" && x.ActiveFlag != "N").OrderBy(x => x.Level).ToList();

                                    foreach (var ITUsers in completeWFSDetails)
                                    {
                                        var users = JsonConvert.DeserializeObject<List<UsersDTO>>(ITUsers.SelectedUsers);
                                        List<DropDownsDTO> userList = new List<DropDownsDTO>();
                                        foreach (var USR in users)
                                        {
                                            userList.Add(new DropDownsDTO
                                            {
                                                Id = USR.Id,
                                                key = USR.UserFirstName
                                            });
                                        }

                                        levelWiseDeptUsers.Add(new SLADetails
                                        {
                                            LevelNo = Convert.ToInt32(ITUsers.Level),
                                            Users = userList,
                                            SlaDays = ITUsers.SlaDays,
                                            SlaFlag = ITUsers.SlaActive == "Y" ? true : false
                                        });
                                    }
                                    topLevel = completeWFS.TotalLevel;
                                }
                                else
                                {
                                    FinalResultDTO deptCycleLevel = GetDeptWFCycle(Convert.ToInt32(requestMasterData.EmployeeId));
                                    if (deptCycleLevel.Status)
                                    {
                                        levelWiseDeptUsers = deptCycleLevel.ResultOP;
                                        topLevel = levelWiseDeptUsers.OrderByDescending(x => x.LevelNo).Select(x => x.LevelNo).FirstOrDefault();
                                    }
                                }

                                var slaDetails = JsonConvert.DeserializeObject<SLADetails>(item.SlaData);
                                slaDetails.LevelNo = Convert.ToInt32(item.Level);
                                if (slaDetails.SlaFlag)
                                {
                                    CalRemainingSLAObj = CalculateRemainingSLA(slaDetails);
                                    if (CalRemainingSLAObj.Status)
                                    {
                                        slaDetails = CalRemainingSLAObj.ResultOP;
                                    }
                                }
                                dat.ApprovalDetails.Add(new ApproveDetailsJsonNew
                                {
                                    WorkFlowStage = item.WorkflowStage,
                                    SLAData = slaDetails,
                                    status = "Current",
                                    Level = slaDetails.LevelNo
                                });

                                for (int i = (Convert.ToInt32(item.Level) + 1); i <= topLevel; i++)
                                {
                                    SLADetails slaDatas = new SLADetails();
                                    if (item.WorkflowStage == "Department ")
                                    {
                                        slaDatas = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault();
                                    }
                                    else
                                    {
                                        slaDatas = levelWiseDeptUsers.Where(x => x.LevelNo == i).FirstOrDefault();
                                    }

                                    dat.ApprovalDetails.Add(new ApproveDetailsJsonNew
                                    {
                                        WorkFlowStage = item.WorkflowStage,
                                        status = "Upcoming",
                                        Level = i,
                                        SLAData = slaDatas

                                    });
                                }
                            }
                            ITFlows.Add(dat);
                        }
                    }
                    completeData.ITWFList = ITFlows;
                }

                result.ResultOP = completeData;
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the Template Names";
                return result;
            }
        }


        public static FinalResultDTO getRequestInfo(RequestInfoDTO requestInfoDTO)
        {
            FinalResultDTO resut = new FinalResultDTO();
            List<dynamic> finalResult = new List<dynamic>();
            ApproveDetailsJson completeData = new ApproveDetailsJson();

            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from NR in db.UarRequestmaster
                                         join user in db.User on NR.EmployeeId equals user.Id
                                         where NR.Status == requestInfoDTO.status && NR.EmployeeId != requestInfoDTO.userId
                                         orderby NR.Id descending
                                         select new
                                         {
                                             userID = user.Id,
                                             userName = user.FirstName,
                                             requestId = NR.Id,
                                             userIdDll = user.FirstName,
                                             createdDate = user.CreatedTime,
                                             level = NR.Level,
                                             workflowstage = NR.WorkflowStage,
                                             requestDate = NR.RequestDate,
                                             status = NR.Status,
                                             approveDetails = NR.ApprovalDetails
                                         }).ToList();

                    foreach (var item in templateNames)
                    {
                        if (item.approveDetails != null)
                        {
                            var Approvedata = JsonConvert.DeserializeObject<List<ApproveDetailsJson>>(item.approveDetails);

                            completeData = Approvedata.Where(x => x.approverId == requestInfoDTO.userId).FirstOrDefault();
                            if (completeData != null)
                            {
                                finalResult.Add(item);
                                continue;
                            }
                            var templateNamesDetails = (from NR in db.UarRequestDetails
                                                        where NR.RequestId == item.requestId
                                                        orderby NR.Id descending
                                                        select new
                                                        {

                                                            requestId = NR.Id,
                                                            level = NR.Level,
                                                            workflowstage = NR.WorkflowStage,
                                                            status = NR.Status,
                                                            approveDetails = NR.ApprovalDetails
                                                        }).ToList();
                            foreach (var itemVal in templateNamesDetails)
                            {
                                if (itemVal.approveDetails != null)
                                {
                                    var ApprovedataValue = JsonConvert.DeserializeObject<List<ApproveDetailsJson>>(itemVal.approveDetails);

                                    completeData = ApprovedataValue.Where(x => x.approverId == requestInfoDTO.userId).FirstOrDefault();
                                    if (completeData != null)
                                    {
                                        finalResult.Add(item);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    resut.ResultOP = finalResult;

                    resut.Status = true;
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


        public static FinalResultDTO getRequestITInfo(SubCategoryList requestInfoDTO)
        {
            FinalResultDTO resut = new FinalResultDTO();
            List<FinalQuestion> finalResult = new List<FinalQuestion>();
            List<Question> questiondata = new List<Question>();
            QuestionExplanationDTO questionExplanationDTO = new QuestionExplanationDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {

                    var completeData = db.RequestForm.Where(x => x.Requestid == requestInfoDTO.Id).FirstOrDefault();
                    if (completeData != null)
                    {
                        var categoryData = (from lookItem in db.LookupItem
                                            where lookItem.CategoryId == 1 && lookItem.ActiveFlag == "Y" && lookItem.Id == completeData.CategoryId
                                            select new DropDownsDTO
                                            {
                                                Id = Convert.ToInt32(lookItem.Id),
                                                key = lookItem.Key,
                                            }).ToList();

                        string catename = categoryData[0].key;
                        var templateNames = (from cat in db.Category
                                             join Maincat in db.MainCategory on cat.MainCategoryId equals Maincat.Id
                                             where (Maincat.MainCategoryName.Trim() == catename.Trim() && cat.IsActive.Trim()=="Y" &&  Maincat.IsActive.Trim() == "Y")
                                             orderby cat.Id ascending
                                             select new
                                             {
                                                 id = cat.Id,
                                                 RiskCategory = cat.Question,
                                                 CalcualtnLikelihood = "0",
                                                 CalcualtnSeverity = "0",
                                                 MainCategoryid = cat.MainCategoryId,
                                                 CalcualtnRanking = "",
                                             }).ToList();
                        var sublist = (from NR in db.SubCategory
                                       where NR.IsActive == "Y"
                                       orderby NR.Id ascending
                                       select new Question
                                       {
                                           id = NR.Id,
                                           Risk = NR.Risk,
                                           QuestionToAsk = NR.QuestionandExplanation,
                                           Explanation = "",
                                           yesNo = "",
                                           Comments = "",
                                           decisionLikelihood = "",
                                           decisionSeverity = "",
                                           decisionRanking = "",
                                           color = "",
                                           score = "",
                                           ranking = "",
                                           riskMitigationComments = "",
                                           responsibleParty = "",
                                           plannedCompletion = "",
                                           status = "",
                                           remediationResponsibleParty = "",
                                           remediationPlan = "",

                                           QuestionNumber = NR.QuestionNumber,
                                           CategoryId = NR.CategoryId,
                                           QuestionExplanation = JsonConvert.DeserializeObject<QuestionExplanationDTO>(NR.QuestionandExplanation)
                                       }).ToList();
                        foreach (var item in templateNames)
                        {
                            if (item.id != null && item.id > 0)
                            {
                                questiondata = sublist.Where(x => x.CategoryId == item.id).ToList();
                                FinalQuestion finalQuestion = new FinalQuestion();
                                finalQuestion.id = Convert.ToString(item.id);
                                finalQuestion.RiskCategory = item.RiskCategory;
                                finalQuestion.CalcualtnLikelihood = item.CalcualtnLikelihood;
                                finalQuestion.CalcualtnRanking = item.CalcualtnRanking;
                                finalQuestion.CalcualtnSeverity = item.CalcualtnSeverity;
                                finalQuestion.score = "";
                                finalQuestion.ranking = "";
                                finalQuestion.riskMitigationComments = "";
                                finalQuestion.responsibleParty = "";
                                finalQuestion.plannedCompletion = "";
                                finalQuestion.status = "";
                                finalQuestion.remediationResponsibleParty = "";
                                finalQuestion.remediationPlan = "";
                                finalQuestion.color = "";
                                finalQuestion.questions = questiondata;
                                //var Approvedata = JsonConvert.DeserializeObject<List<Question>>(item.approveDetails);
                                finalResult.Add(finalQuestion);
                            }
                        }
                    }
                    resut.ResultOP = finalResult;
                    resut.Status = true;
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





    }
}
