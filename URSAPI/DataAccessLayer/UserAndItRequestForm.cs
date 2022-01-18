using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;
using static URSAPI.ModelDTO.Remediation;
//using System.Linq;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace URSAPI.DataAccessLayer
{
    public class UserAndItRequestForm
    {
        public static FinalResultDTO CreateRequest(NewUserRequestDataAttachement input, dynamic Ipdetails, string url,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            List<AuditTrialJSONDTO> auditdto = new List<AuditTrialJSONDTO>();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            List<AccessType> accessType = new List<AccessType>();
            var newRequestDetails = new RequestForm();
            var newRequestLevel = new RequestLevel();
            ErrorLog errorLog = new ErrorLog();
            Int64 userid = 0;
            try
            {
                string sno = "";
                var newRequestData = JsonConvert.DeserializeObject<UserRequestForm>(input.NewRequestData);
                userid = newRequestData.UserId;
                List<AttachmentTable> staticAttachment = JsonConvert.DeserializeObject<List<AttachmentTable>>(input.StaticAttachmentsTable);
                EditRequestModel editRequestModel = new EditRequestModel();
                List<string> output = new List<string>();
                List<securitypolicyAudit> sprlist = new List<securitypolicyAudit>();
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                Boolean ismasedit = false;
                using (dbURSContext db = new dbURSContext())
                {
                    string attachmentlist = "";
                    if (newRequestData.Requestid > 0)
                    {
                        editRequestModel = (from req in db.RequestForm
                                            join reqorg in db.UrsOrg on req.Requestid equals reqorg.ReqId
                                            join LO in db.LookupItem on req.CategoryId equals LO.Id
                                            join LOS in db.LookupSubitem on req.SubcategoryId equals LOS.Id
                                            join USE in db.User on req.UserId equals USE.Id
                                            join LOOK in db.LookupItem on req.FirewallRegion equals LOOK.Id
                                            join LOOKNE in db.LookupItem on req.NormalExpedited equals LOOKNE.Id
                                            where req.Requestid == newRequestData.Requestid
                                            select new EditRequestModel
                                            {
                                                Id = Convert.ToInt32(req.Requestid),
                                                requestSno = req.RequestSno,
                                                categoryId = req.CategoryId,
                                                subCategoryId = req.SubcategoryId,
                                                categoryName = LO.Category,
                                                subcategoryName = LOS.Key,
                                                UserName = USE.FirstName,
                                                FirewallRegionName = LOOK.Key,
                                                NormalExpecticationName = LOOKNE.Key,
                                                businessJustification = req.BusinessJustification,
                                                managedServices = req.ManagedServices,
                                                firewallRegion = req.FirewallRegion,
                                                normalExpedited = req.NormalExpedited,
                                                businessownersId = req.BusinessOwnersId,
                                                // itownersId = req.ItownersId,
                                                nameOfProject = req.NameOfProject,
                                                description = req.Description,
                                                businessImpact = req.BusinessImpact,
                                                securityPolicy = req.SecurityPolicy,
                                                architectureDiagram = req.ArchitectureDiagram,
                                                status = req.Status,
                                                userId = req.UserId,
                                                attachment = req.Attachment,
                                                riskandRankDetails = Convert.ToString(req.RiskandRankDetails),
                                                remarks = req.Remarks,
                                                filepath = reqorg.FilePath,
                                                businessOwner = Convert.ToString(req.BusinessOwners),
                                                itOwner = Convert.ToString(req.ItOwners)
                                            }).FirstOrDefault();
                    }
                    if (newRequestData.Requestid == 0)
                    {
                        sno = GenerateSNO();
                        newRequestDetails = new RequestForm
                        {

                            Requestid = newRequestData.Requestid,
                            RequestSno = sno,
                            BusinessImpact = newRequestData.BusinessImpact,
                            BusinessOwnersId = newRequestData.BusinessOwnersId,
                            BusinessJustification = newRequestData.BusinessJustification,
                            CategoryId = newRequestData.CategoryId,
                            Description = newRequestData.Description,
                            FirewallRegion = newRequestData.FirewallRegion,
                            ItownersId = newRequestData.ItownersId,
                            ManagedServices = newRequestData.ManagedServices,
                            NameOfProject = newRequestData.NameOfProject,
                            NormalExpedited = newRequestData.NormalExpedited,
                            SecurityPolicy = newRequestData.SecurityPolicy,
                            Status = "Draft",
                            SubcategoryId = newRequestData.SubcategoryId,
                            UserId = newRequestData.UserId,
                            BusinessOwners = Convert.ToString(newRequestData.businessOwner).Trim(),
                            ItOwners = Convert.ToString(newRequestData.itOwner).Trim(),
                            // peerReviewId =Convert.ToString (newRequestData.UserId),
                            LastModifiedDate = dateTime_Indian,
                            LastUpdatedBy = newRequestData.UserId,
                            IsEditing = true,
                            Editortimeon = dateTime_Indian,
                            EditorId = 0

                            // status = "Draft",
                        };
                        db.RequestForm.Add(newRequestDetails);
                        db.SaveChanges();

                        var deleteuserDetails =
                           from details in db.RequestLevel
                           where (details.RequestId == newRequestDetails.Requestid)
                           select details;
                        foreach (var detail in deleteuserDetails)
                        {
                            var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                            db.RequestLevel.Remove(singleRec);
                            db.SaveChanges();
                        }
                        //for audit trial

                        var categoryname = db.LookupItem.Where(x => x.Id == newRequestData.CategoryId).Select(x => x.Key).FirstOrDefault();
                        var normalexpecti = db.LookupItem.Where(x => x.Id == newRequestData.NormalExpedited).Select(x => x.Key).FirstOrDefault();
                        var subcategory = db.LookupSubitem.Where(x => x.Id == newRequestData.SubcategoryId).Select(x => x.Key).FirstOrDefault();
                        var firewallregio = db.LookupItem.Where(x => x.Id == newRequestData.FirewallRegion).Select(x => x.Key).FirstOrDefault();
                        var usernamee = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                        masauditdto.browser = Ipdetails[0];
                        masauditdto.eventname = "Create";
                        masauditdto.module = "New Request";
                        masauditdto.userid = newRequestData.UserId;
                        masauditdto.ipaddress = Ipdetails[1];
                        auditjsonmdl.accesstype = accessType;
                        auditjsonmdl.requestId = Convert.ToString(newRequestDetails.RequestSno);
                        masauditdto.Systemremarks = "New Record Inserted";
                        masauditdto.RequestId = Convert.ToString(newRequestDetails.Requestid);
                        auditjsonmdl.browser = Ipdetails[0];
                        auditjsonmdl.createddate = dateTime_Indian;
                        auditjsonmdl.eventname = "Create";
                        auditjsonmdl.module = "New Request ";
                        auditjsonmdl.userid = newRequestData.UserId;
                        auditjsonmdl.username = usernamee;
                        auditjsonmdl.ipaddress = Ipdetails[1];
                        auditjsonmdl.Systemremarks = "New Record Inserted";
                        string strbusis = "";
                        string stritow = "";
                        if (newRequestData.businessOwner != null)
                        {
                            List<AuditUser> newbusiness = new List<AuditUser>();
                            newbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(newRequestData.businessOwner.ToString());
                            List<string> newbus = new List<string>();
                            foreach (var item in newbusiness)
                            {
                                newbus.Add(item.firstName);
                            }
                            strbusis = string.Join(",", newbus.ToArray());
                        }
                        if (newRequestData.itOwner != null)
                        {
                            List<AuditUser> newbusiness = new List<AuditUser>();
                            newbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(newRequestData.itOwner.ToString());
                            List<string> newli = new List<string>();
                            foreach (var item in newbusiness)
                            {
                                newli.Add(item.firstName);
                            }
                            stritow = string.Join(",", newli.ToArray());
                        }
                        if ((categoryname != null))
                        {
                            var content = " Category:" + categoryname + "";
                            output.Add(content);
                        }
                        if ((subcategory != null))

                        {
                            var content = " Sub-Category:" + subcategory + "";
                            output.Add(content);
                        }
                        if (newRequestData.BusinessJustification != null)
                        {
                            var content = " Business Justification:  " + newRequestData.BusinessJustification + "";
                            output.Add(content);
                        }
                        if (newRequestData.ManagedServices != null && newRequestData.ManagedServices != "")
                        {
                            var content = " Managed Services : " + newRequestData.ManagedServices + "";
                            output.Add(content);
                        }
                        if (firewallregio != null)

                        {
                            var content = " Firewall Region:  " + firewallregio + "";
                            output.Add(content);
                        }
                        if (normalexpecti != null)

                        {
                            var content = "Normal/Expedited:  " + normalexpecti + "";
                            output.Add(content);
                        }
                        if (strbusis != null)
                        {
                            var content = "Business Owners:" + strbusis;
                            output.Add(content);

                        }
                        if (stritow != null)
                        {
                            var content = "IT Owners:" + stritow;
                            output.Add(content);
                        }

                        if (newRequestData.Description != null && newRequestData.Description != "")

                        {
                            var content = " Description : " + newRequestData.Description + "";
                            output.Add(content);
                        }
                        if (newRequestData.BusinessImpact != null)
                        {
                            var content = " Business Impact:" + newRequestData.BusinessImpact + "";
                            output.Add(content);
                        }
                        if (newRequestData.NameOfProject != null)
                        {
                            var content = " Name of Project/Effort/Application : " + newRequestData.NameOfProject + "";
                            output.Add(content);
                        }
                        if (newRequestData.SecurityPolicy != null)
                        {

                            var data = JsonConvert.DeserializeObject<List<securitypolicyRoot>>(newRequestData.SecurityPolicy);
                            foreach (var item in data)
                            {
                                var spr = new securitypolicyAudit
                                {
                                    Sourcevpcaccount = item.Sourcevpcaccount[0].key,
                                    Sourceipaddress = item.Sourceipaddress,
                                    Destinationvpcaccount = item.Destinationvpcaccount[0].key,
                                    Destination = item.Destination,
                                    Application = item.Application,
                                    Portservice = item.Portservice,
                                    Protocal = item.protocal[0].key,
                                };
                                sprlist.Add(spr);
                            }

                        }


                    }
                    else
                    {
                        //update existing record
                        newRequestDetails = db.RequestForm.Where(x => x.Requestid == newRequestData.Requestid).FirstOrDefault();

                        if (newRequestData.ispeerreview == "yes")
                        {
                        }
                        else
                        {
                            newRequestDetails.Status = "Draft";
                        }

                        newRequestDetails.Requestid = newRequestData.Requestid;

                        newRequestDetails.BusinessImpact = newRequestData.BusinessImpact;
                        newRequestDetails.BusinessOwnersId = 0;
                        newRequestDetails.BusinessJustification = newRequestData.BusinessJustification;
                        newRequestDetails.CategoryId = newRequestData.CategoryId;
                        newRequestDetails.Description = newRequestData.Description;
                        newRequestDetails.FirewallRegion = newRequestData.FirewallRegion;
                        newRequestDetails.ItownersId = 0;
                        newRequestDetails.ManagedServices = newRequestData.ManagedServices;
                        newRequestDetails.NameOfProject = newRequestData.NameOfProject;
                        newRequestDetails.NormalExpedited = newRequestData.NormalExpedited;
                        newRequestDetails.SecurityPolicy = newRequestData.SecurityPolicy;
                        newRequestDetails.SubcategoryId = newRequestData.SubcategoryId;
                        newRequestDetails.UserId = newRequestDetails.UserId;
                        newRequestDetails.BusinessOwners = Convert.ToString(newRequestData.businessOwner).Trim();
                        newRequestDetails.ItOwners = Convert.ToString(newRequestData.itOwner).Trim();
                        newRequestDetails.IsEditing = true;
                        newRequestDetails.LastModifiedDate = dateTime_Indian;
                        db.SaveChanges();
                        //apiUrl = 'https://localhost:44344/';
                        var entryper = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                        masauditdto.browser = Ipdetails[0];
                        if (newRequestData.ispeerreview == "yes")
                        {
                            masauditdto.eventname = "Update Peerreview";
                            masauditdto.module = "Update Peerreview";
                            auditjsonmdl.username = entryper;
                        }
                        else
                        {
                            masauditdto.eventname = "Update";
                            masauditdto.module = "Update Request";
                            auditjsonmdl.username = entryper;
                        }

                        masauditdto.userid = newRequestData.UserId;
                        auditjsonmdl.accesstype = accessType;
                        auditjsonmdl.requestId = Convert.ToString(newRequestDetails.Requestid);
                        masauditdto.Systemremarks = "Update Request";
                        masauditdto.RequestId = Convert.ToString(newRequestDetails.Requestid);
                        auditjsonmdl.createddate = dateTime_Indian;
                        auditjsonmdl.browser = Ipdetails[0];
                        auditjsonmdl.eventname = "Update";
                        auditjsonmdl.module = "Update Request ";
                        auditjsonmdl.userid = newRequestData.UserId;
                        auditjsonmdl.ipaddress = Ipdetails[1];
                        auditjsonmdl.Systemremarks = "Update Request";

                        var categoryname = db.LookupItem.Where(x => x.Id == newRequestData.CategoryId).Select(x => x.Key).FirstOrDefault();
                        var firewallregio = db.LookupItem.Where(x => x.Id == newRequestData.FirewallRegion).Select(x => x.Key).FirstOrDefault();

                        var normalexpecti = db.LookupItem.Where(x => x.Id == newRequestData.NormalExpedited).Select(x => x.Key).FirstOrDefault();
                        var subcategory = db.LookupSubitem.Where(x => x.Id == newRequestData.SubcategoryId).Select(x => x.Key).FirstOrDefault();
                        var usernamee = db.User.Where(x => x.Id == newRequestData.UserId).Select(x => x.FirstName).FirstOrDefault();
                        Boolean busi = false;
                        Boolean itown = false;
                        string strbusicont = "";
                        string stritowner = "";

                        if (newRequestData.businessOwner != null && editRequestModel.businessOwner != null)
                        {
                            List<AuditUser> oldbusiness = new List<AuditUser>();
                            List<AuditUser> newbusiness = new List<AuditUser>();
                            oldbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(editRequestModel.businessOwner);
                            newbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(newRequestData.businessOwner.ToString());

                            List<string> old = new List<string>();
                            foreach (var item in oldbusiness)
                            {
                                old.Add(item.firstName);
                            }
                            List<string> newli = new List<string>();
                            foreach (var item in newbusiness)
                            {
                                newli.Add(item.firstName);
                            }
                            var firstNotSecond = old.Except(newli).ToList();
                            var secondNotFirst = newli.Except(old).ToList();
                            busi = !firstNotSecond.Any() && !secondNotFirst.Any();
                            if (!busi)
                            {
                                string oldstr = string.Join(",", old.ToArray());
                                string newstr = string.Join(",", newli.ToArray());
                                strbusicont = " Business Owners: Changed from " + oldstr + "  to:  " + newstr + "";
                            }
                        }

                        if (newRequestData.itOwner != null && editRequestModel.itOwner != null)
                        {
                            List<AuditUser> oldbusiness = new List<AuditUser>();
                            List<AuditUser> newbusiness = new List<AuditUser>();
                            oldbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(editRequestModel.itOwner);
                            newbusiness = JsonConvert.DeserializeObject<List<AuditUser>>(newRequestData.itOwner.ToString());

                            List<string> old = new List<string>();
                            foreach (var item in oldbusiness)
                            {
                                old.Add(item.firstName);
                            }
                            List<string> newli = new List<string>();
                            foreach (var item in newbusiness)
                            {
                                newli.Add(item.firstName);
                            }
                            var firstNotSecond = old.Except(newli).ToList();
                            var secondNotFirst = newli.Except(old).ToList();
                            itown = !firstNotSecond.Any() && !secondNotFirst.Any();
                            if (!busi)
                            {
                                string oldstr = string.Join(",", old.ToArray());
                                string newstr = string.Join(",", newli.ToArray());
                                stritowner = " IT Owners: Changed from " + oldstr + "  to:  " + newstr + "";
                            }
                        }                      
                        string eventname = "";
                        string description = "";

                        if (String.Equals(editRequestModel.subcategoryName, subcategory))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = "Sub-Category : Changed from  " + editRequestModel.subcategoryName + "  to:  " + subcategory + "";
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.businessJustification, newRequestData.BusinessJustification))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Business Justification: Changed from  " + editRequestModel.businessJustification + "  to:  " + newRequestData.BusinessJustification + "";
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.managedServices, newRequestData.ManagedServices))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Managed Services: Changed from  " + editRequestModel.managedServices + "  to:  " + newRequestData.ManagedServices + "";
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.FirewallRegionName, firewallregio))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Firewall Region : Changed from  " + editRequestModel.FirewallRegionName + "  to:  " + firewallregio + "";
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.NormalExpecticationName, normalexpecti))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Normal/Expedited: Changed from  " + editRequestModel.NormalExpecticationName + "  to:  " + normalexpecti + "";
                            output.Add(content);
                        }
                        if (!busi)
                        {
                            ismasedit = true;
                            var content = "Business Owners:" + strbusicont;
                            output.Add(content);
                        }
                        if (!itown)
                        {
                            ismasedit = true;
                            var content = "IT Owners: " + stritowner;
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.description, newRequestData.Description))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Description : Changed from  " + editRequestModel.description + "  to:  " + newRequestData.Description + "";
                            output.Add(content);
                        }
                        if (String.Equals(editRequestModel.businessImpact, newRequestData.BusinessImpact))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Business Impact: Changed from  " + editRequestModel.businessImpact + "  to:  " + newRequestData.BusinessImpact + "";
                            output.Add(content);
                        }


                        if (String.Equals(editRequestModel.nameOfProject, newRequestData.NameOfProject))
                        { }
                        else
                        {
                            ismasedit = true;
                            var content = " Name Of The Project : Changed from  " + editRequestModel.nameOfProject + "  to:  " + newRequestData.NameOfProject + "";
                            output.Add(content);
                        }
                       

                        if (newRequestData.SecurityPolicy != null && editRequestModel.securityPolicy != null)
                        {
                            var olddata = JsonConvert.DeserializeObject<List<securitypolicyRoot>>(editRequestModel.securityPolicy);
                            var data = JsonConvert.DeserializeObject<List<securitypolicyRoot>>(newRequestData.SecurityPolicy);
                            int lowlength = olddata.Count < data.Count ? olddata.Count : data.Count;
                            int highlength = olddata.Count > data.Count ? olddata.Count : data.Count;
                            for (int j = 0; j < lowlength; j++)
                            {
                                if (olddata[j].Sourcevpcaccount[0].key != data[j].Sourcevpcaccount[0].key 
                                   || olddata[j].Sourceipaddress != data[j].Sourceipaddress 
                                   || olddata[j].Destinationvpcaccount[0].key != data[j].Destinationvpcaccount[0].key
                                   || olddata[j].Destination != data[j].Destination
                                   || olddata[j].Application != data[j].Application
                                   || olddata[j].Portservice != data[j].Portservice
                                   || olddata[j].protocal[0].key != data[j].protocal[0].key)
                                {
                                    var obj1 = new securitypolicyAudit();
                                    string spc1 = "";
                                    if (olddata[j].Sourcevpcaccount[0].key != data[j].Sourcevpcaccount[0].key)
                                    {
                                        spc1 = " Changed from " + olddata[j].Sourcevpcaccount[0].key + " to " + data[j].Sourcevpcaccount[0].key;
                                        obj1.Sourcevpcaccount = spc1;
                                    }
                                    else
                                    {
                                        obj1.Sourcevpcaccount = olddata[j].Sourcevpcaccount[0].key;
                                    }
                                    if (olddata[j].Sourceipaddress != data[j].Sourceipaddress)
                                    {
                                        spc1 = "Changed from " + olddata[j].Sourceipaddress + " to " + data[j].Sourceipaddress;
                                        obj1.Sourceipaddress = spc1;
                                    }
                                    else
                                    {
                                        obj1.Sourceipaddress = olddata[j].Sourceipaddress;
                                    }
                                    if (olddata[j].Destinationvpcaccount[0].key != data[j].Destinationvpcaccount[0].key)
                                    {
                                        spc1 = "Changed from " + olddata[j].Destinationvpcaccount[0].key + " to " + data[j].Destinationvpcaccount[0].key;
                                        obj1.Destinationvpcaccount = spc1;
                                    }
                                    else { obj1.Destinationvpcaccount = olddata[j].Destinationvpcaccount[0].key; }

                                    if (olddata[j].Destination != data[j].Destination)
                                    {
                                        spc1 = "Changed from " + olddata[j].Destination + " to " + data[j].Destination;
                                        obj1.Destination = spc1;
                                    }
                                    else
                                    {
                                        obj1.Destination = olddata[j].Destination;
                                    }
                                    if (olddata[j].Application != data[j].Application)
                                    {
                                        spc1 = "Changed from " + olddata[j].Application + " to " + data[j].Application;
                                        obj1.Application = spc1;
                                    }
                                    else
                                    {
                                        obj1.Application = olddata[j].Application;
                                    }
                                    if (olddata[j].Portservice != data[j].Portservice)
                                    {
                                        spc1 = "Changed from " + olddata[j].Portservice + " to " + data[j].Portservice;
                                        obj1.Portservice = spc1;
                                    }
                                    else
                                    {
                                        obj1.Portservice = olddata[j].Portservice;
                                    }

                                    if (olddata[j].protocal[0].key != data[j].protocal[0].key)
                                    {
                                        spc1 = "Changed from " + olddata[j].protocal[0].key + " to " + data[j].protocal[0].key;
                                        obj1.Protocal = spc1;
                                    }
                                    else
                                    {
                                        obj1.Protocal = olddata[j].protocal[0].key;
                                    }
                                    sprlist.Add(obj1);
                                }
                            }
                            if (lowlength < highlength)
                            {
                                if (lowlength != highlength)
                                {
                                    for (int k = lowlength; k < highlength; k++)
                                    {
                                        var spr = new securitypolicyAudit
                                        {
                                            Sourcevpcaccount = data[k].Sourcevpcaccount[0].key,
                                            Sourceipaddress = data[k].Sourceipaddress,
                                            Destinationvpcaccount = data[k].Destinationvpcaccount[0].key,
                                            Destination = data[k].Destination,
                                            Application = data[k].Application,
                                            Portservice = data[k].Portservice,
                                            Protocal = data[k].protocal[0].key,
                                        };
                                        sprlist.Add(spr);
                                    }
                                }
                            }
                        }
                    }

                    //Attachment saving part
                    if (newRequestData.Requestid == 0)
                    {
                        var orgId = 1;
                        var orgDetails = db.Organisation.Where(x => x.Id == orgId).FirstOrDefault();
                        var orgPath = orgDetails.OrgName + string.Concat(orgDetails.Id);

                        var orgFolderpath = orgDetails.Filepath;
                        var filePath = orgFolderpath + orgPath + "\\" + newRequestDetails.Requestid + "\\";

                        bool exists = System.IO.Directory.Exists(filePath);

                        if (!exists)
                            System.IO.Directory.CreateDirectory(filePath);

                        var filePath1 = orgFolderpath + orgPath + "\\" + newRequestDetails.Requestid + "\\AuditTrial\\";

                        bool exists1 = System.IO.Directory.Exists(filePath1);

                        if (!exists1)
                            System.IO.Directory.CreateDirectory(filePath1);

                        var orgUrs = new UrsOrg
                        {
                            OrgId = orgId,
                            ReqId = newRequestDetails.Requestid,
                            FilePath = filePath
                        };
                        db.UrsOrg.Add(orgUrs);
                        db.SaveChanges();
                    }
                    var f = (from sop in db.RequestForm
                             join sopOrg in db.UrsOrg on sop.Requestid equals sopOrg.ReqId
                             where sop.Requestid == newRequestDetails.Requestid
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
                            var data = UploadFile(item, f.FilePath,time);
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
                    Int32 i = 0;
                    foreach (var fileItem in staticAttachment)
                    {
                        staticAttachment[i].Date = Convert.ToString(dateTime_Indian);
                        staticAttachment[i].AttachementType = "Static";
                        i = i + 1;
                    }
                    //  if (staticAttachment.Count > 0)
                    //  {
                    if (newRequestData.Requestid == 0)
                    {
                        List<string> filena = new List<string>();
                        foreach (var fileItem in staticAttachment)
                        {
                            filena.Add(fileItem.FileName);
                        }
                        if (filena.Count > 0)
                        {
                            ismasedit = true;
                            string newstr = string.Join(",", filena.ToArray());
                            newstr.TrimEnd(',');
                            var content = " Architecture Diagram: " + newstr;
                            output.Add(content);
                        }
                        //Architecture Diagram
                    }
                    table.StaticTB = staticAttachment;
                    var updateData = db.RequestForm.Where(x => x.Requestid == newRequestDetails.Requestid).FirstOrDefault();
                    AttachmentDB oldfiledata = new AttachmentDB();
                    if (newRequestData.Requestid > 0)
                    {
                        oldfiledata = JsonConvert.DeserializeObject<AttachmentDB>(updateData.Attachment);
                    }
                    updateData.Attachment = JsonConvert.SerializeObject(table);
                    updateData.Remarks = "Attach files";
                    db.SaveChanges();
                    if (newRequestData.Requestid > 0)
                    {
                        List<string> filena = new List<string>();
                        foreach (var fileItem in staticAttachment)
                        {
                            filena.Add(fileItem.FileName);
                        }

                        if (filena.Count > 0)
                        {
                            Boolean itown = false;
                            List<string> oldfileonly = new List<string>();
                            foreach (var item in oldfiledata.StaticTB)
                            {
                                oldfileonly.Add(item.FileName);
                            }
                            var firstNotSecond = oldfileonly.Except(filena).ToList();
                            var secondNotFirst = filena.Except(oldfileonly).ToList();
                            itown = !firstNotSecond.Any() && !secondNotFirst.Any();
                            if (!itown)
                            {
                                ismasedit = true;
                                string newstr = string.Join(",", filena.ToArray());
                                string oldstr = string.Join(",", oldfileonly.ToArray());
                                newstr.TrimEnd(',');
                                oldstr.TrimEnd(',');
                                var content = " Architecture Diagram: Changed from  " + oldstr + "  to:  " + newstr;
                                output.Add(content);
                            }
                        }
                        //Architecture Diagram
                    }

                    if (ismasedit == false)
                    {
                        var content = "";
                        output.Add(content);
                    }
                    // }
                    //for audit trial
                    var accty = new AccessType();
                    masauditdto.existsentry = newRequestData.Requestid;
                    auditjsonmdl.filelist = staticAttachment;
                    auditjsonmdl.basicRequest = (output);
                    accty.categorydescription = output;
                    accty.securitypolicy = sprlist;
                    accessType.Add(accty);
                    auditjsonmdl.accesstype = accessType;
                    auditjsonmdl.Systemremarks = "";

                    List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                    auditdto1.Add(auditjsonmdl);

                    masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                    masauditdto.createddate = dateTime_Indian;
                    masauditdto.RequestId = Convert.ToString(newRequestDetails.Requestid);

                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                    resut.Status = true;
                    resut.Description = newRequestDetails.Requestid + "*Request ID Is : " + sno + "";

                    if (newRequestData.Requestid == 0 && newRequestDetails.Requestid > 0)
                    {
                        var usermail = (from details in db.RequestForm
                                        join look in db.User on details.UserId equals look.Id
                                        where details.Requestid == newRequestDetails.Requestid
                                        select new
                                        {
                                            key = Convert.ToString(look.Email),
                                            username = Convert.ToString(look.FirstName),
                                        }).FirstOrDefault();
                        var mail = EmailDAL.EmailRequestCreation(userid, "New Request Created # " + sno, "Draft", "0", sno, Convert.ToString(usermail.key), newRequestDetails.Requestid, url, Convert.ToString(usermail.username),time);
                    }
                    else
                    {
                        if (newRequestData.ispeerreview == "yes")
                        {
                            string entrypercc = "";
                            if (userid > 0)
                            {
                                entrypercc = db.User.Where(x => x.Id == userid).Select(x => x.Email).FirstOrDefault();
                            }
                            var usermail = (from details in db.RequestForm
                                            join look in db.User on details.UserId equals look.Id
                                            where details.Requestid == newRequestDetails.Requestid
                                            select new
                                            {
                                                key = Convert.ToString(look.Email),
                                                username = Convert.ToString(look.FirstName),
                                                requestno = details.RequestSno
                                            }).FirstOrDefault();
                            if (usermail != null)
                            {
                                if (sno == "" || sno == null)
                                {
                                    sno = usermail.requestno;
                                }
                            }
                            var mail = EmailDAL.EmailPeerreviewcomplete(userid, "Peer Review Completed # " + sno, "Draft", "0", sno, usermail.key, newRequestDetails.Requestid, url, Convert.ToString(usermail.username), entrypercc,time);

                        }

                    }
                }
                return resut;
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Create Request";
                errorLog.Userid = userid;
                errorLog.Methodname = "CreateRequest";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong while creating the new request.";
                return resut;
            }
        }

        public static dynamic InsertRequestRiskandRank(ApproveDTO input, Int64 userid, dynamic ipdetails, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            List<RiskAudit> riskAuditlist = new List<RiskAudit>();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            List<AuditTrialJSONDTO> auditdto = new List<AuditTrialJSONDTO>();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            List<AccessType> accessType = new List<AccessType>();
            List<string> output = new List<string>();
         
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<FinalQuestion> finalQuestion = new List<FinalQuestion>();
                    List<FinalQuestion> finalQuestionnew = new List<FinalQuestion>();
                    List<FinalQuestionResponsible> FinalQuestionResponsibleold = new List<FinalQuestionResponsible>();
                    List<FinalQuestionResponsible> FinalQuestionResponsible = new List<FinalQuestionResponsible>();
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.requestId).FirstOrDefault();
                    finalQuestion = new List<FinalQuestion>();
                    finalQuestionnew = new List<FinalQuestion>();
                    if (levelDetails != null)
                    {
                        //input.status = "Draft";
                        string username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                        if (input.status == "securityrank" || input.status == "securitymitigation" || input.status == "securityplan")
                        {
                            //securityrank,securitymitigation//securityplan
                            if (levelDetails.RiskandRankDetails != "" && input.QuestionJSON != "")
                            {
                                FinalQuestionResponsibleold = JsonConvert.DeserializeObject<List<FinalQuestionResponsible>>(levelDetails.RiskandRankDetails);
                                FinalQuestionResponsible = JsonConvert.DeserializeObject<List<FinalQuestionResponsible>>(input.QuestionJSON);
                                if (input.status == "securitymitigation")
                                {
                                    foreach (var item in FinalQuestionResponsible)
                                    {
                                        if (Convert.ToInt64(item.CalcualtnRanking) >= 7)
                                        {
                                            string strrespons = "";
                                            List<string> newli = new List<string>();
                                            if (item.responsibleParty != null)
                                            {
                                                foreach (var item1 in item.responsibleParty)
                                                {
                                                    newli.Add(item1.name);
                                                }
                                                if (newli.Count > 0)
                                                {
                                                    strrespons = string.Join(",", newli.ToArray());
                                                }
                                            }
                                            var risk = new RiskAudit
                                            {
                                                question = item.RiskCategory,
                                                riskcomment = item.riskMitigationComments,
                                                likelyhood = item.CalcualtnRanking,
                                                ranking = item.ranking,
                                                reposibleparty = strrespons,
                                                plantcompletion = item.plannedCompletion,
                                                remediationaction = item.remediationPlan,
                                                status = item.status,
                                            };
                                            riskAuditlist.Add(risk);
                                        }
                                    } 
                                }
                                else if (input.status == "securityplan")
                                {
                                    foreach (var item in FinalQuestionResponsible)
                                    {
                                        if (Convert.ToInt64(item.CalcualtnRanking) >= 7)
                                        {
                                            string strrespons = "";
                                            List<string> newli = new List<string>();
                                            if (item.responsibleParty != null)
                                            {
                                                foreach (var item1 in item.responsibleParty)
                                                {
                                                    newli.Add(item1.name);
                                                }
                                                if (newli.Count > 0)
                                                {
                                                    strrespons = string.Join(",", newli.ToArray());
                                                }
                                            }
                                            var risk = new RiskAudit
                                            {
                                                question = item.RiskCategory,
                                                riskcomment = item.remediationPlan,
                                                reposibleparty = strrespons,
                                                plantcompletion = item.plannedCompletion,
                                                // remediationaction = item.remediationPlan,
                                                status = item.status,


                                            };
                                            riskAuditlist.Add(risk);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in FinalQuestionResponsibleold)//total new list
                                    {
                                        if (item.questions.Count > 0)
                                        {
                                            foreach (var itemqns in item.questions)//each new how many questions
                                            {
                                                string questi = itemqns.QuestionExplanation.question;
                                                string questionum = itemqns.QuestionExplanation.questionnumber;
                                                if (questionum != "")
                                                {
                                                    foreach (var itemnew in FinalQuestionResponsible)//total old list
                                                    {
                                                        if (itemnew.questions.Count > 0)
                                                        {
                                                            foreach (var itemqnsnew in itemnew.questions) //each old how many questions
                                                            {
                                                                string questinew = itemqnsnew.QuestionExplanation.question;
                                                                string questionumnew = itemqnsnew.QuestionExplanation.questionnumber;
                                                                if (questionum == questionumnew)
                                                                {
                                                                    if (itemqns.decisionLikelihood != itemqnsnew.decisionLikelihood || itemqns.decisionSeverity != itemqnsnew.decisionSeverity ||
                                                                     itemqns.riskMitigationComments != itemqnsnew.riskMitigationComments ||
                                                                      itemqns.remediationResponsibleParty != itemqnsnew.remediationResponsibleParty ||
                                                                      itemqns.plannedCompletion != itemqnsnew.plannedCompletion ||
                                                                       itemqns.remediationPlan != itemqnsnew.remediationPlan ||
                                                                        itemqns.status != itemqnsnew.status ||
                                                                          itemqns.ranking != itemqnsnew.ranking)
                                                                    {
                                                                        var risk = new RiskAudit
                                                                        {
                                                                            likelyhood = itemqnsnew.decisionLikelihood,
                                                                            severity = itemqnsnew.decisionSeverity,
                                                                            riskmitigationstrategy = itemqnsnew.riskMitigationComments,
                                                                            reposibleparty = itemnew.remediationResponsibleParty,
                                                                            plantcompletion = itemnew.plannedCompletion,
                                                                            remediationaction = itemnew.remediationPlan,
                                                                            status = itemnew.status,
                                                                            ranking = itemnew.ranking,
                                                                            question = questinew,
                                                                        };
                                                                        riskAuditlist.Add(risk);
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                                                                                 

                            levelDetails.RiskandRankDetails = input.QuestionJSON;
                            levelDetails.LastModifiedDate = dateTime_Indian;
                            db.SaveChanges();

                            var accty = new AccessType();
                            masauditdto.browser = ipdetails[0];
                            masauditdto.eventname = "Update";
                            if (input.status == "securityrank")
                            {
                                masauditdto.module = "Risk Detail and Ranking";
                                auditjsonmdl.module = "Risk Detail and Ranking ";
                            }
                            else if (input.status == "securitymitigation")
                            { masauditdto.module = "Risk Mitigation";
                                auditjsonmdl.module = "Risk Mitigation ";
                            }
                            else if (input.status == "securityplan")
                            { masauditdto.module = "Remediation Plan"; auditjsonmdl.module = "Remediation Plan "; }
                            else
                            { masauditdto.module = "Update Risk";
                                auditjsonmdl.module = "Update Risk ";
                            }
                            masauditdto.userid = userid;
                            masauditdto.ipaddress = ipdetails[1];
                            auditjsonmdl.accesstype = accessType;
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            masauditdto.Systemremarks = "Update Request";
                            masauditdto.RequestId = Convert.ToString(levelDetails.Requestid);
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.eventname = "Update";
                          
                            auditjsonmdl.userid = userid;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Systemremarks = "Update Record";
                            if (input.status.Trim() == "securityrank")
                            {
                                accty.securityrank = riskAuditlist;
                            }
                            else if (input.status.Trim() == "securitymitigation")
                            {
                                accty.securitymitigation = riskAuditlist;
                            }
                            else if (input.status.Trim() == "securityplan")
                            {
                                accty.securityplan = riskAuditlist;
                            }
                            else { accty.riskaudit = riskAuditlist; }
                         
                            accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                            auditdto1.Add(auditjsonmdl);
                            masauditdto.existsentry = levelDetails.Requestid;
                            masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(levelDetails.Requestid);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                        else
                        {
                           // if (input.status == "Draft")
                            {
                                if (levelDetails.RiskandRankDetails != "" && input.QuestionJSON != "")
                                {
                                    finalQuestion = JsonConvert.DeserializeObject<List<FinalQuestion>>(levelDetails.RiskandRankDetails);
                                    finalQuestionnew = JsonConvert.DeserializeObject<List<FinalQuestion>>(input.QuestionJSON);

                                    foreach (var item in finalQuestion)//total new list
                                    {
                                        if (item.questions.Count > 0)
                                        {
                                            foreach (var itemqns in item.questions)//each new how many questions
                                            {
                                                string questi = itemqns.QuestionExplanation.question;
                                                string questionum = itemqns.QuestionExplanation.questionnumber;
                                                if (questionum != "")
                                                {
                                                    foreach (var itemnew in finalQuestionnew)//total old list
                                                    {
                                                        if (itemnew.questions.Count > 0)
                                                        {
                                                            foreach (var itemqnsnew in itemnew.questions) //each old how many questions
                                                            {
                                                                string questinew = itemqnsnew.QuestionExplanation.question;
                                                                string questionumnew = itemqnsnew.QuestionExplanation.questionnumber;
                                                                if (questionum == questionumnew)
                                                                {
                                                                    if (itemqns.yesNo != itemqnsnew.yesNo || itemqns.Comments != itemqnsnew.Comments)
                                                                    {
                                                                        if ((itemqns.yesNo == "" || itemqns.yesNo == null)&&(itemqns.Comments == "" || itemqns.Comments == null))
                                                                        {
                                                                            var risk = new RiskAudit
                                                                            {
                                                                                riskyesno =  itemqnsnew.yesNo,
                                                                                riskcomment =  itemqnsnew.Comments,
                                                                                question = questinew,
                                                                            };
                                                                            riskAuditlist.Add(risk);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (itemqnsnew.Comments != "" && itemqnsnew.Comments != null)
                                                                            {
                                                                                var risk = new RiskAudit
                                                                                {

                                                                                    riskyesno = "Changed from " + itemqns.yesNo + " to " + itemqnsnew.yesNo,
                                                                                    riskcomment = "Changed from " + itemqns.Comments + " to " + itemqnsnew.Comments,
                                                                                    question = questinew,
                                                                                };
                                                                                riskAuditlist.Add(risk);
                                                                            }
                                                                            else {
                                                                                var risk = new RiskAudit
                                                                                {

                                                                                    riskyesno = "Changed from " + itemqns.yesNo + " to " + itemqnsnew.yesNo,
                                                                                 
                                                                                    question = questinew,
                                                                                };
                                                                                riskAuditlist.Add(risk);

                                                                            }
                                                                          
                                                                        }
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (input.QuestionJSON != "")
                                    {
                                        foreach (var itemnew in finalQuestionnew)//total old list
                                        {
                                            if (itemnew.questions.Count > 0)
                                            {
                                                foreach (var itemqnsnew in itemnew.questions) //each old how many questions
                                                {
                                                    string questinew = itemqnsnew.QuestionExplanation.question;
                                                    string questionumnew = itemqnsnew.QuestionExplanation.questionnumber;
                                                    // if (questionum == questionumnew)
                                                    {
                                                        //  if (itemqns.yesNo != itemqnsnew.yesNo || itemqns.Comments != itemqnsnew.Comments)
                                                        {
                                                            var risk = new RiskAudit
                                                            {
                                                                riskyesno = itemqnsnew.yesNo,
                                                                riskcomment = itemqnsnew.Comments,
                                                                question = questinew,
                                                            };
                                                            riskAuditlist.Add(risk);
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                                if (input.QuestionJSON != ""&& input.QuestionJSON != null)
                                {
                                    if (input.status == "nonaudit")
                                    {
                                        levelDetails.RiskandRankDetails = input.QuestionJSON;
                                        levelDetails.LastModifiedDate = dateTime_Indian;
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        if (input.status == "yes")
                                        {
                                            levelDetails.Status = "Draft";
                                        }
                                        levelDetails.RiskandRankDetails = input.QuestionJSON;
                                        levelDetails.LastModifiedDate = dateTime_Indian;
                                        db.SaveChanges();
                                        var accty = new AccessType();
                                        masauditdto.browser = ipdetails[0];
                                        masauditdto.eventname = "Update";
                                        masauditdto.module = "Update Risk";
                                        masauditdto.userid = userid;
                                        masauditdto.ipaddress = ipdetails[1];
                                        auditjsonmdl.accesstype = accessType;
                                        auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                                        masauditdto.Systemremarks = "Update Request";
                                        masauditdto.RequestId = Convert.ToString(levelDetails.Requestid);
                                        auditjsonmdl.browser = ipdetails[0];
                                        auditjsonmdl.createddate = dateTime_Indian;
                                        auditjsonmdl.eventname = "Update";
                                        auditjsonmdl.module = "Update Risk ";
                                        auditjsonmdl.userid = userid;
                                        auditjsonmdl.username = username;
                                        auditjsonmdl.ipaddress = ipdetails[1];
                                        auditjsonmdl.Systemremarks = "Update Record";
                                        accty.riskaudit = riskAuditlist;
                                        accessType.Add(accty);
                                        auditjsonmdl.accesstype = accessType;
                                        List<AuditTrialJSONDTO> auditdto1 = new List<AuditTrialJSONDTO>();
                                        auditdto1.Add(auditjsonmdl);
                                        masauditdto.existsentry = levelDetails.Requestid;
                                        masauditdto.description = JsonConvert.SerializeObject(auditdto1);
                                        masauditdto.createddate = dateTime_Indian;
                                        masauditdto.RequestId = Convert.ToString(levelDetails.Requestid);
                                        AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                                    }
                                }
                            }
                        }
                    }
                    result.Status = true;
                    result.Description = "Question Updated Successfully";
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Create Request";
                errorLog.Userid = userid;
                errorLog.Methodname = "InsertRequestRiskandRank";
                errorLog.ErrorMessage = ex.Message;
                result.Status = false;
                result.Description = "Something went wrong while Insert Risk and Rank  the details.";
                return result;
            }
        }

        public static dynamic RequestDraftToPublish(Int64 input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var newRequestLevel = new RequestLevel();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input).FirstOrDefault();
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    var emailcctocreator = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.Email).FirstOrDefault();
                    var immediatesupervisor = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.ImmediateSupervisor).FirstOrDefault();
                    var emailimmediatesupervisor = db.User.Where(x => x.Id == immediatesupervisor).Select(x => x.Email).FirstOrDefault();
                    List<Valueonly> val = new List<Valueonly>();
                    Valueonly bb = new Valueonly();
                    bb.Mailaddress = emailimmediatesupervisor;
                    val.Add(bb);

                    List<string> vall = new List<string>();
                    foreach (var item in val)
                    {
                        if (!vall.Contains(item.Mailaddress))
                        {
                            vall.Add(item.Mailaddress);
                        }
                    }
                    if (levelDetails != null)
                    {
                        levelDetails.Status = "userpublish";
                        levelDetails.LastModifiedDate = dateTime_Indian;
                        db.SaveChanges();

                        if (vall.Count > 0)
                        {
                            var mail = EmailDAL.EmailRequestpublishToManager(userid,
                                "Request # " + levelDetails.RequestSno + " - Published", "Published", levelDetails.RequestSno, vall, input, url, emailcctocreator, username,time);
                        }
                    }

                    List<string> output = new List<string>();
                    var accty = new AccessType();
                    List<AccessType> accessType = new List<AccessType>();
                    var content = "Request No: " + levelDetails.RequestSno + " Successfully Published";
                    output.Add(content);
                    auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                    auditjsonmdl.createddate = dateTime_Indian;
                    auditjsonmdl.browser = ipdetails[0];
                    auditjsonmdl.eventname = "Request Published";
                    auditjsonmdl.module = "Request Published";
                    auditjsonmdl.userid = levelDetails.UserId;
                    auditjsonmdl.username = username;
                    auditjsonmdl.ipaddress = ipdetails[1];
                    auditjsonmdl.Systemremarks = "";
                    auditjsonmdl.basicRequest = (output);
                    accty.categorydescription = output;
                    accessType.Add(accty);
                    auditjsonmdl.accesstype = accessType;
                    masauditdto.existsentry = input;
                    masauditdto.createddate = dateTime_Indian;
                    masauditdto.RequestId = Convert.ToString(input);
                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                }
                result.Status = true;
                result.Description = "Published Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }
        public static dynamic RequestUnlock(Int64 input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input).FirstOrDefault();

                    List<string> vall = new List<string>();

                    if (levelDetails != null)
                    {
                        levelDetails.IsEditing = true;
                        levelDetails.Editortimeon = dateTime_Indian;
                        db.SaveChanges();
                        //if (vall.Count > 0)
                        //{
                        //    var mail = EmailDAL.EmailRequestToPublish(userid, "Request Published  " + levelDetails.RequestSno, "Published", "0", levelDetails.RequestSno, vall);
                        //}
                    }
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    List<string> output = new List<string>();
                    var accty = new AccessType();
                    List<AccessType> accessType = new List<AccessType>();
                    var content = "Request No: " + levelDetails.RequestSno + " Successfully Unlocked";
                    output.Add(content);
                    auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                    auditjsonmdl.createddate = dateTime_Indian;
                    auditjsonmdl.browser = ipdetails[0];
                    auditjsonmdl.eventname = "Request unlocked";
                    auditjsonmdl.module = "Request unlocked";
                    auditjsonmdl.userid = levelDetails.UserId;
                    auditjsonmdl.username = username;
                    auditjsonmdl.ipaddress = ipdetails[1];
                    auditjsonmdl.Systemremarks = "";
                    auditjsonmdl.basicRequest = (output);
                    accty.categorydescription = output;
                    accessType.Add(accty);
                    auditjsonmdl.accesstype = accessType;
                    masauditdto.existsentry = input;
                    masauditdto.createddate = dateTime_Indian;
                    masauditdto.RequestId = Convert.ToString(input);
                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);

                }
                result.Status = true;
                result.Description = "unlocked Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while Unlock the request.";
                return result;
            }
        }

        public static dynamic RequestWithdraw(Int64 input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var requestDetails = db.RequestForm.Where(x => x.Requestid == input).FirstOrDefault();
                    if (requestDetails != null)
                    {
                        requestDetails.Status = "Withdrawn";
                        requestDetails.LastModifiedDate = dateTime_Indian;
                        db.SaveChanges();
                    }
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    List<string> output = new List<string>();
                    var content = requestDetails.RequestSno + "Withdrawn Successfully";
                    output.Add(content);
                    auditjsonmdl.requestId = Convert.ToString(requestDetails.RequestSno);
                    auditjsonmdl.createddate = dateTime_Indian;
                    auditjsonmdl.browser = ipdetails[0];
                    auditjsonmdl.eventname = "Request Withdraw";
                    auditjsonmdl.module = "Request Withdraw";
                    auditjsonmdl.userid = userid;
                    auditjsonmdl.username = username;
                    auditjsonmdl.ipaddress = ipdetails[1];
                    auditjsonmdl.Systemremarks = "";
                    auditjsonmdl.basicRequest = (output);
                    accty.categorydescription = output;
                    accessType.Add(accty);
                    auditjsonmdl.accesstype = accessType;
                    masauditdto.existsentry = input;
                    masauditdto.createddate = dateTime_Indian;
                    masauditdto.RequestId = Convert.ToString(input);
                    AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                }
                result.Status = true;
                result.Description = "Withdrawn Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }


        public static dynamic GenerateSNO()
        {
            string finalrslt = "";
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var exis = db.RequestForm.ToList();
                    if (exis.Count > 0)
                    {
                        var sn = db.RequestForm.ToList().Last().RequestSno;
                        if (sn == null) { finalrslt = "FR-" + DateTime.Today.Year + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0') + "-" + "000001"; }
                        else
                        {
                            var array = sn.Split('-');
                            if (int.Parse(array[1]) == DateTime.Today.Year && int.Parse(array[2]) == DateTime.Today.Month)
                            {
                                string oldstr = Convert.ToString(int.Parse(array[3]) + 1);
                                string strvalue = oldstr.PadLeft(6, '0');
                                finalrslt = "FR-" + array[1] + "-" + array[2] + "-" + (strvalue);
                            }
                            else
                            {
                                string oldstr = Convert.ToString(1);
                                string strvalue = oldstr.PadLeft(6, '0');
                                finalrslt = "FR-" + DateTime.Today.Year + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0') + "-" + (strvalue);
                            }
                        }
                    }
                    else
                    {
                        string oldstr = Convert.ToString(1);
                        string strvalue = oldstr.PadLeft(6, '0');
                        finalrslt = "FR-" + DateTime.Today.Year + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0') + "-" + (strvalue);

                    }
                    return finalrslt;
                }
            }
            catch (Exception)
            {
                string oldstr = Convert.ToString(1);
                string strvalue = oldstr.PadLeft(6, '0');
                finalrslt = "FR-" + DateTime.Today.Year + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0') + "-" + (strvalue);
                return finalrslt;
            }
        }

        public static dynamic UploadFile(IFormFile fileModel, string folderPath,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            dynamic resultDTO = new ExpandoObject();
            resultDTO.path = "";
            resultDTO.result = false;
            try
            {
                IFormFile Pdffile = fileModel;
                if (Pdffile != null)
                {
                    DateTime now = dateTime_Indian;
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

        public static dynamic LoadDropDowns()
        {
            FinalResultDTO result = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.CategoryDetails = new ExpandoObject();
            resultDTO.FirewallRegion = new ExpandoObject();
            resultDTO.NormalExpedited = new ExpandoObject();
            resultDTO.VPCAccount = new ExpandoObject();
            resultDTO.Protocol = new ExpandoObject();

            try
            {
                var Category = LookUPMasterDAL.GetLookupMaster("Category");
                if (Category.Status)
                {
                    resultDTO.CategoryDetails = Category.ResultOP;
                }
                var FirewallRegion = LookUPMasterDAL.GetLookupMaster("Firewall Region");
                if (FirewallRegion.Status)
                {
                    resultDTO.FirewallRegion = FirewallRegion.ResultOP;
                }
                var NormalExpedited = LookUPMasterDAL.GetLookupMaster("Normal/Expedited");
                if (NormalExpedited.Status)
                {
                    resultDTO.NormalExpedited = NormalExpedited.ResultOP;
                }
                var VPCAccount = LookUPMasterDAL.GetLookupMaster("VPC Account");
                if (VPCAccount.Status)
                {
                    resultDTO.VPCAccount = VPCAccount.ResultOP;
                }
                var Protocol = LookUPMasterDAL.GetLookupMaster("Protocol");
                if (Protocol.Status)
                {
                    resultDTO.Protocol = Protocol.ResultOP;
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

        public static dynamic EditRequestLoadonId(Int32 reqid, Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var reqDetails = db.RequestForm.Where(x => x.Requestid == reqid).FirstOrDefault();
                    if ((reqDetails.UserId > 0) && (userid > 0))
                    {
                        if (reqDetails.UserId != userid)
                        {
                            reqDetails.IsEditing = false;
                            reqDetails.EditorId = userid;
                            reqDetails.Editortimeon = dateTime_Indian;
                            db.SaveChanges();
                        }
                    }

                    var requestdataonId = (from req in db.RequestForm
                                           join reqorg in db.UrsOrg on req.Requestid equals reqorg.ReqId
                                           join LO in db.LookupItem on req.CategoryId equals LO.Id
                                           join LOS in db.LookupSubitem on req.SubcategoryId equals LOS.Id
                                           join USE in db.User on req.UserId equals USE.Id
                                           join LOOK in db.LookupItem on req.FirewallRegion equals LOOK.Id
                                           join LOOKNE in db.LookupItem on req.NormalExpedited equals LOOKNE.Id
                                           where req.Requestid == reqid
                                           select new
                                           {
                                               Id = Convert.ToInt32(req.Requestid),
                                               requestSno = req.RequestSno,
                                               categoryId = req.CategoryId,
                                               subCategoryId = req.SubcategoryId,
                                               categoryName = LO.Key,
                                               subcategoryName = LOS.Key,
                                               UserName = USE.FirstName,
                                               FirewallRegionName = LOOK.Key,
                                               NormalExpecticationName = LOOKNE.Key,
                                               businessJustification = req.BusinessJustification,
                                               managedServices = req.ManagedServices,
                                               firewallRegion = req.FirewallRegion,
                                               normalExpedited = req.NormalExpedited,
                                               businessownersId = req.BusinessOwnersId,
                                               itownersId = req.ItownersId,
                                               nameOfProject = req.NameOfProject,
                                               description = req.Description,
                                               businessImpact = req.BusinessImpact,
                                               securityPolicy = req.SecurityPolicy,
                                               architectureDiagram = req.ArchitectureDiagram,
                                               status = req.Status,
                                               userId = req.UserId,
                                               attachment = req.Attachment,
                                               riskandRankDetails = Convert.ToString(req.RiskandRankDetails),
                                               remarks = req.Remarks,
                                               filepath = reqorg.FilePath,
                                               businessOwner = Convert.ToString(req.BusinessOwners),
                                               itOwner = Convert.ToString(req.ItOwners),
                                               canEdit = req.IsEditing,
                                           }).FirstOrDefault();
                    result.Status = true;
                    result.ResultOP = requestdataonId;
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

        public static dynamic UnlockRequest(Int64 reqid, Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var reqDetails = db.RequestForm.Where(x => x.Requestid == reqid).FirstOrDefault();
                    if ((reqDetails.UserId > 0) != (userid > 0))
                    {
                        reqDetails.IsEditing = true;
                        //  reqDetails.EditorId = userid;
                        //reqDetails.Editortimeon = DateTime.Now;
                        db.SaveChanges();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Description = "Something went wrong, While fetching the file.";
                result.Status = false;
                return result;
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
                    result.Description = "Problem in fetching the file from the stored location.";
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

        public static dynamic ToChangePeerReview(PeerreviewList input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.id).FirstOrDefault();
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    var emailcctocreator = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.Email).FirstOrDefault();
                    var emailccname = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.FirstName).FirstOrDefault();

                    if (levelDetails != null)
                    {
                        levelDetails.PeerReviewId = Convert.ToString((JsonConvert.SerializeObject(input.userlist)));
                        levelDetails.Status = "Peer Review";
                        levelDetails.LastModifiedDate = dateTime_Indian;
                        levelDetails.IsEditing = false;
                        db.SaveChanges();
                        List<Valueonly> val = new List<Valueonly>();
                        Int32 peerreviewid = 0;
                        if (input.userlist.Count > 0)
                        {
                            peerreviewid = input.userlist[0].Id;
                            val = (from AT in input.userlist
                                   join USE in db.User on AT.Id equals USE.Id
                                   select new Valueonly
                                   {
                                       Mailaddress = USE.Email,

                                   }).ToList();
                        }
                        List<string> vall = new List<string>();
                        if (val.Count > 0)
                        {
                            foreach (var item in val)
                            {
                                if (!vall.Contains(item.Mailaddress))
                                {
                                    vall.Add(item.Mailaddress);
                                }
                            }
                        }
                        if (vall.Count > 0)
                        {
                            var mail = EmailDAL.EmailRequestToPeerreview(userid, "Request for Peer Review # " + levelDetails.RequestSno, "Peer Review", "0", levelDetails.RequestSno, vall, input.id, url, emailccname, emailcctocreator, peerreviewid,time);
                        }

                        List<string> output = new List<string>();
                        //var content = "Peer Reviewer Comments:" + input.key;
                        var content = "";
                        //output.Add(content);
                        auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                        auditjsonmdl.createddate = dateTime_Indian;
                        auditjsonmdl.browser = ipdetails[0];
                        auditjsonmdl.eventname = "Submitted for Peer Review";
                        auditjsonmdl.module = "Peer Review";
                        auditjsonmdl.userid = levelDetails.UserId;
                        auditjsonmdl.username = username;
                        auditjsonmdl.ipaddress = ipdetails[1];
                        auditjsonmdl.Systemremarks = "";
                       // auditjsonmdl.Userremark = input.key;
                        //auditjsonmdl.basicRequest = (output);
                        //accty.categorydescription = output;
                        //accessType.Add(accty);
                        //auditjsonmdl.accesstype = accessType;
                        masauditdto.existsentry = input.id;
                        masauditdto.createddate = dateTime_Indian;
                        masauditdto.RequestId = Convert.ToString(input.id);
                        AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                    }
                }
                result.Status = true;
                result.Description = "Peer Review Changed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }
        public static dynamic ManagerTONetwork(SubCategoryList input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            var newRequestLevel = new RequestLevel();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.Id).FirstOrDefault();
                    string emailtocccreator = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.Email).FirstOrDefault();
                    string emailtoname = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.FirstName).FirstOrDefault();

                    List<Valueonly> val = new List<Valueonly>();
                    if (levelDetails.Status == "userpublish")
                    {
                        if (levelDetails != null)
                        {
                            if (input.CategoryId == 1)
                            {

                                var deleteuserDetails = from details in db.RequestLevel
                                                        where (details.RequestId == levelDetails.Requestid && details.LevelId == 1)
                                                        select details;
                                foreach (var detail in deleteuserDetails)
                                {
                                    var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                    db.RequestLevel.Remove(singleRec);
                                    db.SaveChanges();
                                }
                                levelDetails.Status = "Publish";
                                levelDetails.LastModifiedDate = dateTime_Indian;
                                db.SaveChanges();
                                newRequestLevel = new RequestLevel
                                {
                                    RequestId = input.Id,
                                    LevelId = 1,
                                    UserId = userid,
                                    Status = "Publish",
                                    UpdateDate = dateTime_Indian
                                };
                                db.RequestLevel.Add(newRequestLevel);
                                db.SaveChanges();
                                val = (from AT in db.Workflowdetails
                                       join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                       join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                       join USE in db.User on WFU.UserId equals USE.Id
                                       where (WFU.LevelId == 2 && AT.CombinationId == levelDetails.CategoryId)
                                       select new Valueonly
                                       {
                                           Mailaddress = USE.Email,
                                       }).ToList();
                                List<string> vall = new List<string>();
                                foreach (var item in val)
                                {
                                    if (!vall.Contains(item.Mailaddress))
                                    {
                                        vall.Add(item.Mailaddress);
                                    }
                                }

                                if (vall.Count > 0)
                                {
                                    var mail = EmailDAL.EmailRequestToPublish(userid, "Request # " + levelDetails.RequestSno + " - Submitted for Network Admin", "Manager Approved", "0", levelDetails.RequestSno, vall, url, levelDetails.Requestid, emailtoname, emailtocccreator, levelDetails.Requestid, input.Key,time);
                                }
                                List<string> output = new List<string>();
                                var content = "Manager Comments:" + input.Key;
                                output.Add(content);
                                auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                                auditjsonmdl.createddate = dateTime_Indian;
                                auditjsonmdl.browser = ipdetails[0];
                                auditjsonmdl.eventname = "Manager Review";
                                auditjsonmdl.module = "Manager";
                                auditjsonmdl.userid = levelDetails.UserId;
                                auditjsonmdl.username = username;
                                auditjsonmdl.ipaddress = ipdetails[1];
                                auditjsonmdl.Systemremarks = "";
                                auditjsonmdl.Userremark = input.Key;
                               // auditjsonmdl.basicRequest = (output);
                               //accty.categorydescription = output;
                               // accessType.Add(accty);
                                auditjsonmdl.accesstype = accessType;
                                masauditdto.existsentry = input.Id;
                                masauditdto.createddate = dateTime_Indian;
                                masauditdto.RequestId = Convert.ToString(input.Id);
                                AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                            }
                            else if (input.CategoryId == 2)
                            {
                                var deleteuserDetails =
                              from details in db.RequestLevel
                              where (details.RequestId == input.Id )
                              select details;
                                foreach (var detail in deleteuserDetails)
                                {
                                    var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                    db.RequestLevel.Remove(singleRec);
                                    db.SaveChanges();
                                }

                                levelDetails.Status = "Draft"; db.SaveChanges();
                                val = (from AT in db.User

                                           //  join USE in db.User on WFU.UserId equals USE.Id
                                       where (AT.Id == levelDetails.UserId)
                                       select new Valueonly
                                       {
                                           Mailaddress = AT.Email,
                                       }).ToList();
                                List<string> vall = new List<string>();
                                foreach (var item in val)
                                {
                                    if (!vall.Contains(item.Mailaddress))
                                    {
                                        vall.Add(item.Mailaddress);
                                    }
                                }
                                if (vall.Count > 0)
                                {
                                    var mail = EmailDAL.EmailRejectManagerandIT(userid, "Request # " + levelDetails.RequestSno + " - Rejected by Manager", "Draft", levelDetails.RequestSno, input.Key, vall, input.Id, url, "Manager", emailtoname,time, levelDetails.UserId);
                                }

                                List<string> output = new List<string>();
                                var content = "Manager Comments:" + input.Key;
                                output.Add(content);
                                auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                                auditjsonmdl.createddate = dateTime_Indian;
                                auditjsonmdl.browser = ipdetails[0];
                                auditjsonmdl.eventname = "Manager Rejected";
                                auditjsonmdl.module = "Manager";
                                auditjsonmdl.userid = levelDetails.UserId;
                                auditjsonmdl.username = username;
                                auditjsonmdl.ipaddress = ipdetails[1];
                                auditjsonmdl.Userremark = input.Key;
                                auditjsonmdl.Systemremarks = "";
                                // auditjsonmdl.basicRequest = (output);
                                //accty.categorydescription = output;
                                // accessType.Add(accty);
                                auditjsonmdl.accesstype = accessType;
                                masauditdto.existsentry = input.Id;
                                masauditdto.createddate = dateTime_Indian;
                                masauditdto.RequestId = Convert.ToString(input.Id);
                                AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                            }
                            else
                            {
                                levelDetails.Status = "Rejected";
                                levelDetails.LastModifiedDate = dateTime_Indian;
                                db.SaveChanges();
                                newRequestLevel = new RequestLevel
                                {
                                    RequestId = input.Id,
                                    LevelId = 1,
                                    UserId = userid,
                                    Status = "Rejected",
                                    UpdateDate = dateTime_Indian
                                };
                                db.RequestLevel.Add(newRequestLevel);
                                db.SaveChanges();
                                val = (from AT in db.User

                                           //  join USE in db.User on WFU.UserId equals USE.Id
                                       where (AT.Id == levelDetails.UserId)
                                       select new Valueonly
                                       {
                                           Mailaddress = AT.Email,
                                       }).ToList();
                                List<string> vall = new List<string>();
                                foreach (var item in val)
                                {
                                    if (!vall.Contains(item.Mailaddress))
                                    {
                                        vall.Add(item.Mailaddress);
                                    }
                                }
                                if (vall.Count > 0)
                                {
                                    var mail = EmailDAL.EmailClosedManagerorIT(userid, "Requested # " + levelDetails.RequestSno + " - Closed by Manager", "Closed", input.Key, levelDetails.RequestSno, vall, input.Id, url, "Manager", emailtoname,time, levelDetails.UserId);
                                }
                                List<string> output = new List<string>();
                                var content = "Manager Comments:" + input.Key;
                                output.Add(content);
                                auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                                auditjsonmdl.createddate = dateTime_Indian;
                                auditjsonmdl.browser = ipdetails[0];
                                auditjsonmdl.eventname = "Manager Reverted";
                                auditjsonmdl.module = "Manager";
                                auditjsonmdl.userid = levelDetails.UserId;
                                auditjsonmdl.username = username;
                                auditjsonmdl.ipaddress = ipdetails[1];
                                auditjsonmdl.Userremark = input.Key;
                                // auditjsonmdl.basicRequest = (output);
                                //accty.categorydescription = output;
                                // accessType.Add(accty);
                                auditjsonmdl.accesstype = accessType;
                                masauditdto.existsentry = input.Id;
                                masauditdto.createddate = dateTime_Indian;
                                masauditdto.RequestId = Convert.ToString(input.Id);
                                AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                            }
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Description = "Already Approved";
                        return result;
                    }
                }
                result.Status = true;
                result.Description = "Manager Reviewed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic ManagerTOIT(SubCategoryList input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            var newRequestLevel = new RequestLevel();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.Id).FirstOrDefault();
                    string emailtocccreator = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.Email).FirstOrDefault();
                    string emailtoname = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.FirstName).FirstOrDefault();

                    List<Valueonly> val = new List<Valueonly>();

                    if (levelDetails != null)
                    {
                        if (input.CategoryId == 1)
                        {
                            var deleteuserDetails =
                           from details in db.RequestLevel
                           where (details.RequestId == levelDetails.Requestid && details.LevelId == 2)
                           select details;
                            foreach (var detail in deleteuserDetails)
                            {
                                var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                db.RequestLevel.Remove(singleRec);
                                db.SaveChanges();
                            }
                            levelDetails.Status = "ITApprove";
                            levelDetails.LastModifiedDate = dateTime_Indian;
                            db.SaveChanges();
                            newRequestLevel = new RequestLevel
                            {
                                RequestId = input.Id,
                                LevelId = 2,
                                UserId = userid,
                                Status = "ITApprove",
                                UpdateDate = dateTime_Indian
                            };
                            db.RequestLevel.Add(newRequestLevel);
                            db.SaveChanges();
                            val = (from AT in db.Workflowdetails
                                   join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                   join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                   join USE in db.User on WFU.UserId equals USE.Id
                                   where (WFU.LevelId == 3 && AT.CombinationId == levelDetails.CategoryId)
                                   select new Valueonly
                                   {
                                       Mailaddress = USE.Email,
                                   }).ToList();
                            List<string> vall = new List<string>();
                            foreach (var item in val)
                            {
                                if (!vall.Contains(item.Mailaddress))
                                {
                                    vall.Add(item.Mailaddress);
                                }
                            }
                            if (vall.Count > 0)
                            {
                                var mail = EmailDAL.EmailManagerToIT(userid, "Network Admin Approved # " + levelDetails.RequestSno + " - Submitted for Security Approval", "Network Approved", input.Key, levelDetails.RequestSno, vall, input.Id, url, emailtocccreator, emailtoname,time);
                            }
                            List<string> output = new List<string>();
                            var content = "Network Admin Comments:" + input.Key;
                            output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "Network Admin Review";
                            auditjsonmdl.module = "Network Admin";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Userremark = input.Key;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.Id;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.Id);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                        else if (input.CategoryId == 2)
                        {
                            var deleteuserDetails =
                          from details in db.RequestLevel
                          where (details.RequestId == input.Id )
                          select details;
                            foreach (var detail in deleteuserDetails)
                            {
                                var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                db.RequestLevel.Remove(singleRec);
                                db.SaveChanges();
                            }

                            levelDetails.Status = "Draft"; db.SaveChanges();
                            val = (from AT in db.User

                                       //  join USE in db.User on WFU.UserId equals USE.Id
                                   where (AT.Id == levelDetails.UserId)
                                   select new Valueonly
                                   {
                                       Mailaddress = AT.Email,
                                   }).ToList();
                            List<string> vall = new List<string>();
                            foreach (var item in val)
                            {
                                if (!vall.Contains(item.Mailaddress))
                                {
                                    vall.Add(item.Mailaddress);
                                }
                            }
                            if (vall.Count > 0)
                            {
                                var mail = EmailDAL.EmailRejectManagerandIT(userid, "Request # " + levelDetails.RequestSno + " -    Rejected by Network Admin", "Draft", levelDetails.RequestSno, input.Key, vall, input.Id, url, "Network", emailtoname,time,levelDetails.UserId);
                            }

                            List<string> output = new List<string>();
                            var content = "Network Admin Comments:" + input.Key;
                            output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "Network Admin Rejected";
                            auditjsonmdl.module = "Network Admin";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            //auditjsonmdl.Systemremarks = input.Key;
                            auditjsonmdl.Userremark = input.Key;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.Id;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.Id);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                        else
                        {
                            levelDetails.Status = "Rejected";
                            levelDetails.LastModifiedDate = dateTime_Indian;
                            db.SaveChanges();
                            newRequestLevel = new RequestLevel
                            {
                                RequestId = input.Id,
                                LevelId = 2,
                                UserId = userid,
                                Status = "Rejected",
                                UpdateDate = dateTime_Indian
                            };
                            db.RequestLevel.Add(newRequestLevel);
                            db.SaveChanges();
                            val = (from AT in db.User

                                       //  join USE in db.User on WFU.UserId equals USE.Id
                                   where (AT.Id == levelDetails.UserId)
                                   select new Valueonly
                                   {
                                       Mailaddress = AT.Email,
                                   }).ToList();
                            List<string> vall = new List<string>();
                            foreach (var item in val)
                            {
                                if (!vall.Contains(item.Mailaddress))
                                {
                                    vall.Add(item.Mailaddress);
                                }
                            }
                            if (vall.Count > 0)
                            {
                                var mail = EmailDAL.EmailClosedManagerorIT(userid, "Requested # " + levelDetails.RequestSno + " -  Closed by Network Admin", "Closed", input.Key, levelDetails.RequestSno, vall, input.Id, url, "Network", emailtoname,time,levelDetails.UserId);

                            }
                            List<string> output = new List<string>();
                            var content = "Network Admin Comments:" + input.Key;
                            output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "Network Admin Reverted";
                            auditjsonmdl.module = "NetworkAdmin";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Userremark = input.Key;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.Id;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.Id);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                    }
                }
                result.Status = true;
                result.Description = "Manager Reviewed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic ITApprove(ApproveDTO input, dynamic ipdetails, Int64 userid, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            var newRequestLevel = new RequestLevel();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.requestId).FirstOrDefault();
                    var username = db.User.Where(x => x.Id == userid).Select(x => x.FirstName).FirstOrDefault();
                    string emailtoname = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.FirstName).FirstOrDefault();

                    List<Valueonly> val = new List<Valueonly>();
                    val = (from AT in db.User

                           where (AT.Id == levelDetails.UserId)
                           select new Valueonly
                           {
                               Mailaddress = AT.Email
                           }).ToList();
                    List<string> vall = new List<string>();
                    foreach (var item in val)
                    {
                        if (!vall.Contains(item.Mailaddress))
                        {
                            vall.Add(item.Mailaddress);
                        }
                    }
                    if (levelDetails != null)
                    {
                        if (input.status == "Approve")
                        {
                            levelDetails.Status = "Completed";
                            levelDetails.RiskandRankDetails = input.QuestionJSON;
                            levelDetails.LastModifiedDate = dateTime_Indian; db.SaveChanges();
                            var deleteuserDetails =
                          from details in db.RequestLevel
                          where (details.RequestId == input.requestId && details.LevelId == 3)
                          select details;
                            foreach (var detail in deleteuserDetails)
                            {
                                var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                db.RequestLevel.Remove(singleRec);
                                db.SaveChanges();
                            }
                            newRequestLevel = new RequestLevel
                            {
                                RequestId = input.requestId,
                                LevelId = 3,
                                UserId = userid,
                                Status = "Completed",
                                UpdateDate = dateTime_Indian
                            };
                            db.RequestLevel.Add(newRequestLevel);
                            db.SaveChanges();

                            if (vall.Count > 0)
                            {
                                var mail = EmailDAL.EmailITDecesion(userid, "Request # " + levelDetails.RequestSno + " - Approved by Security Admin", "Completed", "SecurityAdmin", levelDetails.RequestSno, vall, url, input.comment, emailtoname, input.requestId, Convert.ToInt64(levelDetails.UserId),time);
                            }

                            List<string> output = new List<string>();
                            //var content = "Security Admin Comments:" + input.comment;
                            //output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "Security Admin Review";
                            auditjsonmdl.module = "Security Admin Approval";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Systemremarks = "";
                            auditjsonmdl.Userremark = input.comment;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.requestId;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.requestId);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                        else if (input.status == "Reject")
                        {
                            levelDetails.Status = "Draft";//that means revert
                            levelDetails.RiskandRankDetails = input.QuestionJSON;
                            levelDetails.LastModifiedDate = dateTime_Indian;
                            db.SaveChanges();

                            var deleteuserDetails =
                          from details in db.RequestLevel
                          where (details.RequestId == input.requestId )
                          select details;
                            foreach (var detail in deleteuserDetails)
                            {
                                var singleRec = db.RequestLevel.FirstOrDefault(x => x.Id == detail.Id);
                                db.RequestLevel.Remove(singleRec);
                                db.SaveChanges();
                            }

                            //var mail = EmailDAL.EmailITDecesion(userid, "IT Rejected " + levelDetails.RequestSno, "Draft", "0", levelDetails.RequestSno, vall, url, input.comment);
                            var mail = EmailDAL.EmailRejectManagerandIT(userid, "Request #  " + levelDetails.RequestSno + " - Rejected by SecurityAdmin", "Draft", levelDetails.RequestSno, input.comment, vall, input.requestId, url, "Security", emailtoname, time,Convert.ToInt64(levelDetails.UserId));
                            List<string> output = new List<string>();
                            //var content = "SecurityAdmin Comments:" + input.comment;
                            //output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "SecurityAdmin Review";
                            auditjsonmdl.module = "SecurityAdmin Approval";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Userremark = input.comment;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.requestId;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.requestId);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                        else
                        {
                            levelDetails.Status = "Rejected";
                            levelDetails.RiskandRankDetails = input.QuestionJSON;
                            levelDetails.LastModifiedDate = dateTime_Indian;
                            db.SaveChanges();
                            //var mail = EmailDAL.EmailITDecesion(userid, "IT Rejected " + levelDetails.RequestSno, "IT Rejected", "0", levelDetails.RequestSno, vall, url, input.comment, input.requestId);

                            List<string> output = new List<string>();
                            var content = "SecurityAdmin Comments:" + input.comment;
                            output.Add(content);
                            auditjsonmdl.requestId = Convert.ToString(levelDetails.RequestSno);
                            auditjsonmdl.createddate = dateTime_Indian;
                            auditjsonmdl.browser = ipdetails[0];
                            auditjsonmdl.eventname = "SecurityAdmin Review";
                            auditjsonmdl.module = "SecurityAdmin Approval";
                            auditjsonmdl.userid = levelDetails.UserId;
                            auditjsonmdl.username = username;
                            auditjsonmdl.ipaddress = ipdetails[1];
                            auditjsonmdl.Systemremarks = "";
                            auditjsonmdl.Userremark = input.comment;
                            auditjsonmdl.Systemremarks = "";
                            // auditjsonmdl.basicRequest = (output);
                            //accty.categorydescription = output;
                            // accessType.Add(accty);
                            auditjsonmdl.accesstype = accessType;
                            masauditdto.existsentry = input.requestId;
                            masauditdto.createddate = dateTime_Indian;
                            masauditdto.RequestId = Convert.ToString(input.requestId);
                            AuditTrialDAL.CreateAuditTrial(masauditdto, auditjsonmdl);
                        }
                    }
                }
                result.Status = true;
                result.Description = "Security Admin Reviewed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic StepperBasedOnRequest(DropDownsDTO input, dynamic ipdetails, Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
            AuditTrialDTO masauditdto = new AuditTrialDTO();
            var accty = new AccessType();
            List<AccessType> accessType = new List<AccessType>();
            var newRequestLevel = new RequestLevel();
            List<StepperDTO> stepperDTO = new List<StepperDTO>();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RequestForm.Where(x => x.Requestid == input.Id).FirstOrDefault();
                    if (levelDetails != null)
                    {
                        var val = (from AT in db.RequestForm
                                   join WFL in db.RequestLevel on AT.Requestid equals WFL.RequestId
                                   join WFU in db.LevelMaster on WFL.LevelId equals WFU.Id
                                   join USE in db.User on WFL.UserId equals USE.Id
                                   where (AT.Requestid == input.Id)
                                   //orderby WFL.UpdateDate ascending
                                   select new
                                   {
                                       RequestId = AT.Requestid,
                                       RequestSno = AT.RequestSno,
                                       MaserStatus = AT.Status,
                                       LevelId = WFL.LevelId,
                                       LevelName = WFU.WorkFlowLevelName,
                                       UserId = WFL.UserId,
                                       UserName = USE.FirstName,
                                       ActionDate = WFL.UpdateDate.ToString(("MM/dd/yyyy")),
                                       DetStatus = WFL.Status,
                                   }).ToList();
                        Int64 requestid = 0;
                        string requestno = "";
                        string username = "";
                        string actiondate = "";
                        string currentstatus = "";
                        Int64 maxit = 0;
                        username = db.User.Where(x => x.Id == levelDetails.UserId).Select(x => x.FirstName).FirstOrDefault();
                        requestno = levelDetails.RequestSno;
                        requestid = levelDetails.Requestid;
                        actiondate = levelDetails.Createdtime.ToString(("MM/dd/yyyy"));
                        currentstatus = levelDetails.Status;
                        if (val.Count == 0)
                        {
                            maxit = 4;
                            for (int i = 0; i < maxit; i++)
                            {
                                StepperDTO stepper = new StepperDTO();
                                stepper.RequestId = requestid;
                                stepper.requestSno = requestno;
                                stepper.LevelName = "";
                                stepper.ActionTakenBy = username;
                                stepper.ActionTakenOn = actiondate;
                                // stepper.currentStatus=""
                                //stepper.Status = "";
                                if (i == 0)
                                {
                                    if (currentstatus == "Draft")
                                    { stepper.Status = "Draft"; stepper.currentStatus = "Draft"; }
                                    else
                                    {
                                        stepper.Status = "Published";
                                        stepper.currentStatus = "Published";
                                    }
                                }
                                else
                                {
                                    stepper.currentStatus = "";
                                }
                                stepperDTO.Add(stepper);
                            }
                        }
                        else
                        {
                            StepperDTO stepper = new StepperDTO();
                            stepper.RequestId = requestid;
                            stepper.requestSno = requestno;
                            stepper.LevelName = "Draft";
                            stepper.ActionTakenBy = username;
                            stepper.ActionTakenOn = actiondate;
                            stepper.Status = "Draft";
                            stepper.currentStatus = currentstatus;
                            stepperDTO.Add(stepper);
                        }

                        foreach (var item in val)
                        {
                            if (item.MaserStatus != null)
                            {
                                requestid = item.RequestId;
                                requestno = item.RequestSno;
                                username = item.UserName;
                                actiondate = Convert.ToString(item.ActionDate);
                                currentstatus = item.MaserStatus;
                                StepperDTO stepper = new StepperDTO();
                                stepper.RequestId = item.RequestId;
                                stepper.requestSno = item.RequestSno;
                                stepper.LevelName = item.LevelName;
                                stepper.ActionTakenBy = item.UserName;
                                stepper.ActionTakenOn = Convert.ToString(item.ActionDate);
                                if (item.LevelId == 1)
                                {
                                    stepper.Status = "Manager Approved";
                                }
                                else if (item.LevelId == 2)
                                {
                                    stepper.Status = "NetworkAdmin Approved";
                                }
                                else if (item.LevelId == 3)
                                {
                                    stepper.Status = "SecurityAdmin Approved";
                                }
                                stepper.currentStatus = item.MaserStatus;
                                stepperDTO.Add(stepper);
                            }
                        }
                        if (stepperDTO.Count > 0 && val.Count > 0)
                        {
                            var tempdraft = val.Select(x => x.MaserStatus).FirstOrDefault();
                            //var tempmanager = val.Where(x => x.DetStatus == "userpublish").Select(x=>x.DetStatus) .FirstOrDefault();
                            //var tempnetwork = val.Where(x => x.DetStatus == "Publish").Select(x => x.DetStatus).FirstOrDefault();
                            //var tempsecurity = val.Where(x => x.DetStatus == "ITApprove").Select(x => x.DetStatus).FirstOrDefault();
                            //var tempcomple = val.Where(x => x.MaserStatus == "Completed").FirstOrDefault();

                            maxit = 0;
                            if (tempdraft == "Draft")
                            {
                                maxit = 4;
                            }
                            else if (tempdraft == "userpublish")
                            {
                                maxit = 3;
                            }
                            else if (tempdraft == "Publish")
                            {
                                maxit = 2;
                            }
                            else if (tempdraft == "ITApprove")
                            {
                                maxit = 1;
                            }

                            for (int i = 0; i < maxit; i++)
                            {
                                StepperDTO stepper = new StepperDTO();
                                stepper.RequestId = requestid;
                                stepper.requestSno = requestno;
                                stepper.LevelName = "";
                                stepper.ActionTakenBy = "";
                                stepper.ActionTakenOn = "";
                                stepper.Status = "";
                                stepper.currentStatus = currentstatus;
                                stepperDTO.Add(stepper);
                            }

                        }
                    }

                    if (stepperDTO.Count == 4)
                    {
                        string level1 = stepperDTO[0].LevelName;
                        if (level1 == "")
                        {
                            stepperDTO[0].LevelName = "Draft";
                        }
                        string level2 = stepperDTO[1].LevelName;
                        if (level2 == "")
                        {
                            stepperDTO[1].LevelName = "Manager";
                        }
                        string level3 = stepperDTO[2].LevelName;
                        if (level3 == "")
                        {
                            var levelname = db.LevelMaster.Where(x => x.Id == 2).Select(x => x.WorkFlowLevelName).FirstOrDefault();
                            stepperDTO[2].LevelName = levelname;
                        }
                        string level4 = stepperDTO[3].LevelName;
                        if (level4 == "")
                        {
                            var levelname = db.LevelMaster.Where(x => x.Id == 3).Select(x => x.WorkFlowLevelName).FirstOrDefault();
                            stepperDTO[3].LevelName = levelname;
                        }
                    }
                }
                result.ResultOP = stepperDTO;
                result.Status = true;
                result.Description = "Stepper Loaded Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic ManagerPermission(SubCategoryList input, Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            MenuaccessandLevelAccess moduleaccess = new MenuaccessandLevelAccess();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    if (input.Id > 0 && input.CategoryId > 0)
                    {
                        var requestDetailsuserid = db.RequestForm.Where(x => x.Requestid == input.Id).FirstOrDefault();
                        var userroles = db.UserRoles.Where(x => x.Id == input.CategoryId).Select(x => x.RoleId).FirstOrDefault();

                        //managerapproval==10; network ==3;security==4;
                        if (input.Key == "manager")
                        {
                            var rolesaccess = db.RolePermission.Where(x => x.RoleId == userroles && x.ModuleId == 9).Select(x => x.ActiveFlag).FirstOrDefault();
                            var supervisor = db.User.Where(x => x.Id == requestDetailsuserid.UserId).Select(x => x.ImmediateSupervisor).FirstOrDefault();

                            if (supervisor == input.CategoryId)
                            {
                                moduleaccess.levelaccess = 1;
                            }
                            else
                            { moduleaccess.levelaccess = 0; }
                            if (rolesaccess.Trim() == "Y")
                            {
                                moduleaccess.menuaccess = 1;
                            }
                            else
                            { moduleaccess.menuaccess = 0; }
                        }

                        else if (input.Key == "network")
                        {
                            var val = (from AT in db.Workflowdetails
                                       join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                       join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                       //join USE in db.User on WFU.UserId equals USE.Id
                                       where (WFU.LevelId == 2 && AT.CombinationId == requestDetailsuserid.CategoryId && WFU.UserId == input.CategoryId)
                                       select new
                                       {
                                           userid = WFU.UserId,
                                       }).ToList();
                            var rolesaccess = db.RolePermission.Where(x => x.RoleId == userroles && x.ModuleId == 3).Select(x => x.ActiveFlag).FirstOrDefault();
                            if (val.Count > 0)
                            {
                                moduleaccess.levelaccess = 1;
                            }
                            else
                            {
                                moduleaccess.levelaccess = 0;
                            }
                            if (rolesaccess.Trim() == "Y")
                            {
                                moduleaccess.menuaccess = 1;
                            }
                            else
                            { moduleaccess.menuaccess = 0; }
                        }
                        else if (input.Key == "security")
                        {
                            var rolesaccess = db.RolePermission.Where(x => x.RoleId == userroles && x.ModuleId == 4).Select(x => x.ActiveFlag).FirstOrDefault();
                            var val = (from AT in db.Workflowdetails
                                       join WFL in db.WorkFlowLevel on AT.CombinationId equals WFL.WorkFlowId
                                       join WFU in db.WorkflowUsers on WFL.LevelId equals WFU.LevelId
                                       //  join USE in db.User on WFU.UserId equals USE.Id
                                       where (WFU.LevelId == 3 && AT.CombinationId == requestDetailsuserid.CategoryId && WFU.UserId == input.CategoryId)
                                       select new
                                       {
                                           userid = WFU.UserId,
                                       }).ToList();

                            if (val.Count > 0)
                            {
                                moduleaccess.levelaccess = 1;
                            }
                            else
                            {
                                moduleaccess.levelaccess = 0;
                            }
                            if (rolesaccess.Trim() == "Y")
                            {
                                moduleaccess.menuaccess = 1;
                            }
                            else
                            { moduleaccess.menuaccess = 0; }
                        }
                    }
                }
                result.ResultOP = moduleaccess;
                result.Status = true;
                result.Description = "Accesspermision";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the details.";
                return result;
            }
        }


        public static dynamic RemediationDiarySaveandUpdate(RemediationDTOList input, Int64 userid, dynamic ipdetails, string url,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            MenuaccessandLevelAccess moduleaccess = new MenuaccessandLevelAccess();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            RemediationComment remediation = new RemediationComment();
            ReplyList replyList = new ReplyList();
            RemediationUser remediationUser = new RemediationUser();
            RemediationReply remediationReply = new RemediationReply();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    remediation = new RemediationComment
                    {
                        Comments = input.list[0].Comments,
                        ReplyId = input.list[0].ReplyId,
                        RequestId = input.RequestId,
                        Date = dateTime_Indian,
                        UserLikeFlag = input.list[0].UserLikeFlag,
                        UserLikeCount = input.list[0].UserLikeCount,
                        ChildFlag = input.list[0].ChildFlag,
                        EditFlag = input.list[0].EditFlag,
                        ReplyFlag = input.list[0].ReplyFlag,
                        CheckedFlag = false
                    };
                    db.RemediationComment.Add(remediation);
                    db.SaveChanges();

                    remediationUser = new RemediationUser
                    {
                        CommentId = remediation.CommentId,
                        UserId = userid,
                    };
                    db.RemediationUser.Add(remediationUser);
                    db.SaveChanges();

                    if (input.list[0].Id > 0 && input.list[0].ReplyFlag == true)
                    {
                        remediationReply = new RemediationReply
                        {
                            CommentId = input.list[0].Id,
                            CommentReplyId = remediation.CommentId,
                            UserId = userid,
                        };
                        db.RemediationReply.Add(remediationReply);
                        db.SaveChanges();
                    }
                }
                result.ResultOP = moduleaccess;
                result.Status = true;
                result.Description = "Messaged saved";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the details.";
                return result;
            }
        }

        public static dynamic RemediationDiaryLoad(RemediationParams input, Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            MenuaccessandLevelAccess moduleaccess = new MenuaccessandLevelAccess();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            RemediationComment remediation = new RemediationComment();
           List<ReplyList> replyList = new List<ReplyList>();
            RemediationDTO remediationDTO = new RemediationDTO();
           
            int startIndex = (input.PageNumber ) * 5;
               try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var totallist = (from remedi in db.RemediationComment
                                           
                                           where remedi.RequestId == input.RequestId && remedi.ReplyFlag == false
                                           orderby remedi.CommentId descending
                                           select new ReplyList
                                           {
                                               Id = Convert.ToInt32(remedi.CommentId),
                                               
                                           }).ToList();

                    var remediationList = (from remedi in db.RemediationComment
                                           join dept in db.RemediationUser on remedi.CommentId equals dept.CommentId
                                           join user in db.User on dept.UserId equals user.Id
                                           where remedi.RequestId == input.RequestId && remedi.ReplyFlag == false
                                           orderby remedi.CommentId descending
                                           select new ReplyList
                                           {
                                               Id = Convert.ToInt32(remedi.CommentId),
                                               Comments = Convert.ToString(remedi.Comments),
                                               ReplyId = Convert.ToInt32(remedi.ReplyId),
                                               RequestId = Convert.ToInt32(remedi.RequestId),
                                               UserName = user.FirstName,
                                               UserBadge = user.FirstName.Substring(0, 2),
                                               totalCount = totallist.Count(),
                                               //  Date = remedi.Date.ToString("MM/dd/yyyy"),
                                               Date = remedi.Date,

                                               UserLikeFlag = Convert.ToBoolean(remedi.UserLikeFlag),
                                               UserLikeCount = Convert.ToInt32(remedi.UserLikeCount),
                                               ChildFlag = Convert.ToBoolean(remedi.ChildFlag),
                                               EditFlag = Convert.ToBoolean(remedi.EditFlag),
                                               EditIndividualFlag = Convert.ToBoolean(remedi.EditFlag),
                                               ReplyFlag = Convert.ToBoolean(remedi.EditFlag),
                                               CheckedFlag = Convert.ToBoolean(remedi.CheckedFlag),
                                               list = ReturnReplylist(remedi.CommentId)
                                           }).Skip(startIndex).Take(5).ToList();
                    foreach (var item in remediationList)
                    {
                        var dd = new ReplyList();
                        dd.Id = Convert.ToInt32(item.Id);
                        dd.Comments = Convert.ToString(item.Comments);
                        dd.ReplyId = Convert.ToInt32(item.ReplyId);
                        dd.RequestId = Convert.ToInt32(item.RequestId);
                        dd.UserName = item.UserName;
                        dd.UserBadge = item.UserBadge.Substring(0, 2);
                        dd.totalCount = item.totalCount;
                        TimeSpan diffTime = dateTime_Indian - item.Date;
                        int days = diffTime.Days;
                        int hours = diffTime.Hours;
                        int minutes = diffTime.Minutes;
                        int seconds = diffTime.Seconds;
                        if (days > 0)
                        { dd.Diffdate = Convert.ToString(days)+"days ago"; }
                        else if (hours > 0)
                        { dd.Diffdate = Convert.ToString(hours) + "hours ago"; }
                        else if (minutes > 0)
                        { dd.Diffdate = Convert.ToString(minutes) + "minutes ago"; }
                        else if (seconds > 0)
                        { dd.Diffdate = Convert.ToString(seconds) + "seconds ago"; }
                        else { dd.Diffdate = Convert.ToString(0) + "seconds ago"; }
                        //  Date = remedi.Date.ToString("MM/dd/yyyy"),
                        //item.Date;
                        //dd.Diffdate = item.Date;
                        dd.UserLikeFlag = Convert.ToBoolean(item.UserLikeFlag);
                        dd.UserLikeCount = Convert.ToInt32(item.UserLikeCount);
                        dd.ChildFlag = Convert.ToBoolean(false);
                        dd.EditFlag = Convert.ToBoolean(true);
                        dd.EditIndividualFlag = Convert.ToBoolean(false);
                        dd.ReplyFlag = Convert.ToBoolean(false);
                        dd.CheckedFlag = Convert.ToBoolean(item.CheckedFlag);
                        if (item.list.Count > 0)
                        {
                            List<ReplyList> subreplies = new List<ReplyList>();
                            foreach (var item1 in item.list)
                            {
                               ReplyList dd1 = new ReplyList();
                                dd1.Id = Convert.ToInt32(item1.Id);
                                dd1.Comments = Convert.ToString(item1.Comments);
                                dd1.ReplyId = Convert.ToInt32(item1.ReplyId);
                                dd1.RequestId = Convert.ToInt32(item1.RequestId);
                                dd1.UserName = item1.UserName;
                                dd1.UserBadge = item1.UserBadge.Substring(0, 2);
                                TimeSpan diffTime1 = dateTime_Indian - item1.Date;
                                int days1 = diffTime1.Days;
                                int hours1 = diffTime1.Hours;
                                int minutes1 = diffTime1.Minutes;
                                int seconds1 = diffTime1.Seconds;
                                if (days1 > 0)
                                { dd1.Diffdate = Convert.ToString(days1)+"days ago"; }
                                else if (hours1 > 0)
                                { dd1.Diffdate = Convert.ToString(hours1)+"hours ago"; }
                                else if (minutes1 > 0)
                                { dd1.Diffdate = Convert.ToString(minutes1)+"minutes ago"; }
                                else if (seconds1 > 0)
                                { dd1.Diffdate = Convert.ToString(seconds1)+ "seconds ago"; }
                                else { dd1.Diffdate = Convert.ToString(0)+"seconds ago"; }
                                //  Date = remedi.Date.ToString("MM/dd/yyyy"),
                                //item.Date;
                                //dd.Diffdate = item.Date;

                                dd1.UserLikeFlag = Convert.ToBoolean(item1.UserLikeFlag);
                                dd1.UserLikeCount = Convert.ToInt32(item1.UserLikeCount);
                                dd1.ChildFlag = Convert.ToBoolean(true);
                                dd1.EditFlag = Convert.ToBoolean(true);
                                dd1.EditIndividualFlag = Convert.ToBoolean(false);
                                dd1.ReplyFlag = Convert.ToBoolean(false);
                                dd1.CheckedFlag = Convert.ToBoolean(item1.CheckedFlag);
                                List<ReplyList> subreplies1 = new List<ReplyList>();
                                dd1.list = subreplies1;
                                subreplies.Add(dd1);
                            //    subreplies.Add(subreplies);
                               
                            }                //dd.  list = ReturnReplylist(item.list)
                            dd.list = subreplies;
                        }
                        replyList.Add(dd);
                    }
               }
                result.ResultOP = JsonConvert.SerializeObject( replyList);
                result.Status = true;
                result.Description = "RemediationDiary";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching the details.";
                return result;
            }
        }

        public static  List<ReplyList> ReturnReplylist(long maincommentId)
        {
            using (dbURSContext db = new dbURSContext())
            {
                var dd=  (from remedi in db.RemediationComment
                        join dept in db.RemediationUser on remedi.CommentId equals dept.CommentId
                        join remireply in db.RemediationReply on remedi.CommentId equals remireply.CommentReplyId
                        join user in db.User on dept.UserId equals user.Id
                        where remireply.CommentId == maincommentId// && remedi.ReplyFlag==false //&& dept.UserId== 2
                        select new ReplyList
                        {
                            Id = Convert.ToInt32(remedi.CommentId),
                            Comments = Convert.ToString(remedi.Comments),
                            ReplyId = Convert.ToInt32(remedi.ReplyId),
                            RequestId = Convert.ToInt32(remedi.RequestId),
                            UserName = user.FirstName,
                            UserBadge = user.FirstName.Substring(0, 2),
                            Date = remedi.Date,
                            UserLikeFlag = Convert.ToBoolean(remedi.UserLikeFlag),
                            UserLikeCount = Convert.ToInt32(remedi.UserLikeCount),
                            ChildFlag = Convert.ToBoolean(remedi.ChildFlag),
                            EditFlag = Convert.ToBoolean(remedi.EditFlag),
                            EditIndividualFlag = Convert.ToBoolean(remedi.EditFlag),
                            ReplyFlag = Convert.ToBoolean(remedi.EditFlag),
                            CheckedFlag = Convert.ToBoolean(remedi.CheckedFlag),
                            // list = ReturnReplylist(remedi.CommentId)
                        }).ToList();
                return dd;
            }
        }

        public static dynamic ToChangeLikeStatus(RemediationCheckedandlike input,  Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
           
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RemediationComment.Where(x => x.CommentId == input.CommentId).FirstOrDefault();
                
                    if (levelDetails != null)
                    {
                        if (input.Status)
                        { levelDetails.UserLikeFlag = input.Status; levelDetails.UserLikeCount = levelDetails.UserLikeCount + 1; }
                        else
                        { if (levelDetails.UserLikeCount > 0) { levelDetails.UserLikeFlag = input.Status; levelDetails.UserLikeCount = levelDetails.UserLikeCount - 1; } }
                      
                        levelDetails.Date = dateTime_Indian;
                        db.SaveChanges();
                    }
                }
                result.Status = true;
                result.Description = "Like  Changed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while Update details.";
                return result;
            }
        }

        public static dynamic ToChangeStrikeStatus(RemediationCheckedandlike input,Int64 userid,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.RemediationComment.Where(x => x.CommentId == input.CommentId).FirstOrDefault();

                    if (levelDetails != null)
                    {
                        levelDetails.CheckedFlag = input.Status;
                        levelDetails.Date = dateTime_Indian;
                        db.SaveChanges();
                    }
                }
                result.Status = true;
                result.Description = "Peer Review Changed Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

    }

}

