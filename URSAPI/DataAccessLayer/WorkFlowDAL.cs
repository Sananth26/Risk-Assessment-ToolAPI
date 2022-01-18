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
    public class WorkFlowDAL
    {

        public static dynamic LoadWorkFlowDetails()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = (from wf in db.Workflowdetails
                                        join lo in db.LookupItem  on wf.CombinationId equals lo.Id
                                        where wf.ActiveFlag == "Y"  && (lo.CategoryId == 1)
                                        select new WFCategoryDTO
                                        {
                                            Id = Convert.ToInt32(wf.CombinationId),
                                            key = lo.Key,
                                           
                                        }).ToList();
                    result.Status = true;
                    result.ResultOP = levelDetails;
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

        public static dynamic LoadWorkFlow( )
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                     
                    var levelDetails = (from wf in db.LevelMaster
                                      
                                        where wf.ActiveFlag == "Y"  
                                        select new WorkFlowMasterDTO
                                        {
                                            Id = Convert.ToInt32(wf.Id),
                                            WorkFlowLevelName = wf.WorkFlowLevelName,
                                            ActiveFlag = wf.ActiveFlag ,
                                        }).Where (x=>x.Id>1).ToList();
                    result.Status = true;
                    result.ResultOP = levelDetails;
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

        public static dynamic LoadWorkFlowDetails(UASWorkFlow inputParam)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int32 categoryId = inputParam.SelectedCategory[0].Id;
                    Int32 subCategoryId = inputParam.SelectedSubCategory[0].Id;
                    var levelDetails = (from wf in db.Workflowcategory
                                        join wfDetails in db.Workflowdetails on wf.Id equals wfDetails.CombinationId
                                        where wf.CategoryId == categoryId && wf.SubCategoryId == subCategoryId
                                        select new WorkFlowMasterDetails
                                        {
                                            Id = Convert.ToInt32(wfDetails.Id),
                                            Level = Convert.ToInt32(wfDetails.Level),
                                            LevelName = wfDetails.LevelName,
                                            IsActiveCopy = wfDetails.ActiveFlag,
                                            CombinationId = Convert.ToInt32(wfDetails.CombinationId),
                                            selectedCheck = false,
                                            users = "Rajkumar,Vimal,Ravi",
                                    }).ToList();
                    foreach (var item in levelDetails)
                    {
                        item.IsActive = item.IsActiveCopy == "Y" ? true : false;
                    }
                    result.Status = true;
                    result.ResultOP = levelDetails;
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

        public static dynamic LoadCompleteWFDataView(Int32 combinationid)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<WorkFlowUserDetails> workFlows = new List<WorkFlowUserDetails>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {

                  //  var completeWFS = db.LookupItem.Where(x=>x.CategoryId==1).ToList();

                   // foreach (var WFS in completeWFS)
                    {
                        var levelDetails = db.Workflowdetails.Where(x => x.DeleteFlag == "N" && x.ActiveFlag == "Y" && x.CombinationId== combinationid).ToList();
                        var levelmasli = db.LevelMaster.ToList();
                            // levelDetails = levelDetails.OrderBy(x => x.Level).ToList();

                        foreach (var item in levelDetails)
                        {
                            //var SelectedUsers = JsonConvert.DeserializeObject<List<UsersDTO>>(item.SelectedUsers);
                            var SelectedLevels = JsonConvert.DeserializeObject<List<DropDownsDTO>>(item.SelectedUsers);
                          
                            List<DropDownsDTO> levell = new List<DropDownsDTO>();
                            
                            var leveli = db.WorkFlowLevel.Where(x => x.WorkFlowId == item.CombinationId).ToList();
                            
                            foreach (var itemlev in leveli)
                            {
                                var usrnameli = levelmasli.Where(x => x.Id == itemlev.LevelId).FirstOrDefault();
                                //usrlist.Add(usrnameli.WorkFlowLevelName);
                                var SelectedUsers = db.WorkflowUsers.Where(x => x.WorkFlowId == item.CombinationId && x.LevelId== itemlev.LevelId).ToList();
                                List<string> usrlist = new List<string>();
                                List<DropDownsDTO> usersIds = new List<DropDownsDTO>();
                                foreach (var itemusr in SelectedUsers)
                            {
                                var usrnamelis = db.User.Where(x => x.Id == itemusr.UserId).FirstOrDefault();
                                usrlist.Add(usrnamelis.FirstName);
                                    DropDownsDTO dropDownsDTO = new DropDownsDTO();
                                    dropDownsDTO.Id = Convert.ToInt32(usrnamelis.Id);
                                    dropDownsDTO.key = usrnamelis.FirstName;
                                    usersIds.Add(dropDownsDTO);
                            }

                           // foreach (var leveitem in SelectedLevels)
                            {
                               // var workflow = db.LevelMaster.Where(x => x.Id == leveitem.Id).FirstOrDefault();
                                var workflowCategoryData = new WorkFlowUserDetails
                                {
                                    Id = item.Id,
                                    combinationId = item.CombinationId,
                                    LevelName = usrnameli.WorkFlowLevelName,
                                    Level =Convert.ToInt32(itemlev.LevelId),
                                    activeFlag = item.ActiveFlag,
                                    users= usrlist,
                                    usersIds = usersIds
                                    //CreatedBy = userid,
                                    //CreatedTime = DateTime.Now
                                };
                                workFlows.Add(workflowCategoryData);
                            }
                            }
                        }
                   }
                    result.Status = true;
                    result.ResultOP = workFlows.OrderBy(x=>x.Level);
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


        public static dynamic LoadRolesAndCategory()
        {
            FinalResultDTO result = new FinalResultDTO();
            dynamic resultDto = new ExpandoObject();
            resultDto.roles = new List<DropDownsDTO>();
            resultDto.categoriesItem = new List<DropDownsDTO>();
            resultDto.users = new List<DropDownsDTO>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var roleData = (from roles in db.LevelMaster
                                    select new DropDownsDTO
                                    {
                                        Id = Convert.ToInt32(roles.Id),
                                        key = roles.WorkFlowLevelName,
                                    }).Where(x=>x.Id>1).ToList();

                    resultDto.roles = roleData;

                    var categoryData = (from lookItem in db.LookupItem
                                        where lookItem.CategoryId == 1 && lookItem.ActiveFlag == "Y"
                                        select new DropDownsDTO
                                        {
                                            Id = Convert.ToInt32(lookItem.Id),
                                            key = lookItem.Key,
                                        }).ToList();

                    resultDto.categoriesItem = categoryData;

                    var userData = (from user in db.User
                                    where user.IsActive == "Y"
                                    //     join user in db.User on userRoles.UserId equals user.Id
                                    select new UsersDTO
                                    {
                                        Id = Convert.ToInt32(user.Id),
                                        UserFirstName = user.FirstName,
                                        UserRoleId = 0,
                                 }).ToList();
                    resultDto.users = userData;
                    result.Status = true;
                    result.ResultOP = resultDto;
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

        public static dynamic LoadUsers(List<DropDownsDTO> input)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var roleData = (from userRoles in db.UserRoles
                                    join user in db.User on userRoles.UserId equals user.Id
                                    join selectedRole in input on userRoles.RoleId equals selectedRole.Id
                                    select new UsersDTO
                                    {
                                        Id = Convert.ToInt32(user.Id),
                                        UserFirstName = user.FirstName,
                                        UserRoleId = Convert.ToInt32(userRoles.RoleId),
                                    }).ToList();

                    result.Status = true;
                    result.ResultOP = roleData;
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

        public static dynamic LoadSubCategory(List<DropDownsDTO> input)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var subCategoryData = (from subCategory in db.LookupSubitem
                                    join selectedCategory in input on subCategory.SubcategoryId equals selectedCategory.Id
                                    select new SubCategoryList
                                    {
                                        Id = Convert.ToInt32(subCategory.Id),
                                        Key = subCategory.Key,
                                        CategoryId = Convert.ToInt32(subCategory.SubcategoryId),
                                    }).ToList();

                    result.Status = true;
                    result.ResultOP = subCategoryData;
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
        public static dynamic SaveLevelsMaster(WorkFlowMasterDTO item, Int64 userid, dynamic Ipdetails,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                     
                    LevelMaster workflowcategory = new LevelMaster();
                   
                    if (item.Id == 0)
                    {
                       var workflowCategoryData = new LevelMaster
                        {
                            WorkFlowLevelName = item.WorkFlowLevelName,
                            ActiveFlag = item.ActiveFlag,
                            CreatedBy = userid,
                            CreatedTime = dateTime_Indian
                        };
                        db.LevelMaster.Add(workflowCategoryData);
                        db.SaveChanges();
                    }
                    else
                    {
                        var workflow = db.LevelMaster.Where(x => x.Id == item.Id).FirstOrDefault();


                        workflow.WorkFlowLevelName = item.WorkFlowLevelName;
                        workflow.ActiveFlag = item.ActiveFlag;
                        workflow.UpdatedBy = userid;
                        workflow.UpdatedTime = dateTime_Indian;
                        db.SaveChanges();
                    }
                    result.Status = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while saving the details.";
                return result;
            }
        }

        public static dynamic SaveLevels(UASWorkFlow item, Int64 userid,dynamic Ipdetails,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int32 level = item.IsActive == true ? item.Level : 0;
                    Workflowcategory workflowcategory = new Workflowcategory();
                    UASWorkFlow1 uASWorkFlow = new UASWorkFlow1();
                    
                    if (item.Id == 0)
                    {
                        var existcombi = db.Workflowdetails.Where(x => x.CombinationId == item.SelectedCategory[0].Id).FirstOrDefault();
                        if (existcombi == null)
                        {
                            var workflow = new Workflowdetails
                            {
                                ActiveFlag = item.IsActive == true ? "Y" : "N",
                                DeleteFlag = "N",
                                CombinationId = item.SelectedCategory[0].Id,
                                // selectedLevels = JsonConvert.SerializeObject(item.SelectedRoles),
                                SelectedRoles = JsonConvert.SerializeObject(item.SelectedRoles),
                                SelectedUsers = JsonConvert.SerializeObject(item.SelectedUsers),
                                CreatedBy = userid,
                                CreatedDate = dateTime_Indian,
                                ModifiedBy = userid,
                                ModifiedDate = dateTime_Indian
                            };
                            db.Workflowdetails.Add(workflow);
                            db.SaveChanges();
                        }
                        else
                        {
                            existcombi.ModifiedBy = userid;
                            existcombi.ModifiedDate = dateTime_Indian;
                            db.SaveChanges();
                        }
                       // var SelectedUsers = JsonConvert.DeserializeObject<List<UsersDTO>>(item.SelectedUsers);
                        if (item.SelectedRoles.Count>0 )
                        {
                            foreach (var itemlevel in item.SelectedRoles)
                            {
                                // Query the database for the rows to be deleted.
                                var deletelevelDetails =
                                    from details in db.WorkFlowLevel
                                        // where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == itemlevel.Id)
                                    where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == Convert.ToInt64(item.SelectedRoles[0].Id))
                                    select details;

                                foreach (var detail in deletelevelDetails)
                                {
                                    var singleRec = db.WorkFlowLevel.FirstOrDefault(x => x.Id == detail.Id);
                                    db.WorkFlowLevel.Remove(singleRec);
                                    db.SaveChanges();
                                }
                                var deleteuserDetails =
                                    from details in db.WorkflowUsers
                                    where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == Convert.ToInt64(item.SelectedRoles[0].Id))
                                    select details;

                                foreach (var detail in deleteuserDetails)
                                {
                                    var singleRec = db.WorkflowUsers.FirstOrDefault(x => x.Id == detail.Id);
                                    db.WorkflowUsers.Remove(singleRec);
                                    db.SaveChanges();
                                }

                                                                                             
                                var workflowlevel = new WorkFlowLevel
                                {
                                    WorkFlowId = item.SelectedCategory[0].Id,
                                    LevelId = itemlevel.Id,
                                };
                                db.WorkFlowLevel.Add(workflowlevel);
                                db.SaveChanges();
                                if (item.SelectedUsers.Count > 0)
                                {
                                    foreach (var itemusr in item.SelectedUsers)
                                    {
                                        var workuser = new WorkflowUsers
                                        {
                                            WorkFlowId = item.SelectedCategory[0].Id,
                                            LevelId = itemlevel.Id,
                                            UserId = itemusr.Id,
                                        };
                                        db.WorkflowUsers.Add(workuser);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                     var existcombi=db.Workflowdetails.Where(x => x.CombinationId == item.SelectedCategory[0].Id).FirstOrDefault();
                     existcombi.ModifiedBy = userid;
                     existcombi.ModifiedDate = dateTime_Indian;
                     db.SaveChanges();
                        if (item.SelectedRoles.Count > 0)
                        {
                            foreach (var itemlevel in item.SelectedRoles)
                            {
                                // Query the database for the rows to be deleted.
                                var deletelevelDetails =
                                    from details in db.WorkFlowLevel
                                        // where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == itemlevel.Id)
                                    where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == Convert.ToInt64(item.SelectedRoles[0].Id))
                                    select details;
                                 
                                foreach (var detail in deletelevelDetails)
                                {
                                    var singleRec = db.WorkFlowLevel.FirstOrDefault(x => x.Id == detail.Id);
                                    db.WorkFlowLevel.Remove(singleRec);
                                    db.SaveChanges();
                                }
                                var deleteuserDetails =
                                    from details in db.WorkflowUsers
                                    where (details.WorkFlowId == item.SelectedCategory[0].Id) && (details.LevelId == Convert.ToInt64(item.SelectedRoles[0].Id))
                                    select details;

                                foreach (var detail in deleteuserDetails)
                                {
                                    var singleRec = db.WorkflowUsers.FirstOrDefault(x => x.Id == detail.Id);
                                    db.WorkflowUsers.Remove(singleRec);
                                    db.SaveChanges();
                                }
                                var workflowlevel = new WorkFlowLevel
                                {
                                    WorkFlowId = item.SelectedCategory[0].Id,
                                    LevelId = itemlevel.Id,
                                };
                                db.WorkFlowLevel.Add(workflowlevel);
                                db.SaveChanges();
                                if (item.SelectedUsers.Count > 0)
                                {
                                    foreach (var itemusr in item.SelectedUsers)
                                    {
                                        var workuser = new WorkflowUsers
                                        {
                                            WorkFlowId = item.SelectedCategory[0].Id,
                                            LevelId = itemlevel.Id,
                                            UserId = itemusr.Id,
                                        };
                                        db.WorkflowUsers.Add(workuser);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    //for audit trial
                    var audithis = (from details in db.Workflowdetails
                                    join look in db.LookupItem on details.CombinationId equals look.Id
                                    where details.CombinationId== item.SelectedCategory[0].Id
                                    select new
                                    {
                                        key = look.Key,
                                    }).FirstOrDefault();
                    string lookupname = "";
                    if (audithis != null)
                    {
                        lookupname = Convert.ToString(audithis.key);
                    }
                    var levelname = item.SelectedRoles[0].key;
                     

                    //   var workflowaud = db.Workflowcategory.Where(x => x.Id == item.CombinationId).FirstOrDefault();
                    //  var maincategory = db.LookupItem.Where(x => x.Id == workflowaud.CategoryId ).FirstOrDefault();
                    //   var subcategory = db.LookupSubitem.Where(x => x.Id == workflowaud.SubCategoryId ).FirstOrDefault();

                    string eventname = "";
                    string description = "";
                    List<string> output = new List<string>();

                    eventname = "Level Updated";
                    string content = "";
                    content = " Category: " + lookupname;
                    output.Add(content);

                    content = "";
                    content = " Level: " + levelname;
                    output.Add(content);
 
                    //if (item.Id == 0)
                    //{
                    //    string variable = "";
                    //    foreach (var it in item.SelectedRoles)
                    //    {
                    //        variable = variable + it.key+",";
                    //    }
                    //    string variable1 = "";
                    //    foreach (var ite in item.SelectedUsers)
                    //    {
                    //        variable1 = variable1 + ite.UserFirstName+",";
                    //    }
                    //    eventname = "  Create";
                    //    string content = "";
                    //    content = " Main Category: " + maincategory.Value ;
                    //    output.Add(content);

                    //    content = "";
                    //    content = " Sub Category: " + subcategory.Value;
                    //    output.Add(content);

                    //    content = "";
                    //    content = " Level: " + level;
                    //    output.Add(content);

                    //    content = "";
                    //    content = " LevelName: " + item.LevelName;
                    //    output.Add(content);

                    //    content = "";
                    //    content = " SlaActive: " + item.SlaActive;

                    //    output.Add(content);
                    //    content = "";
                    //    content = " SLADays: " + item.SlaDays;
                    //    output.Add(content);

                    //    content = "";
                    //    content = " Selected Roles: " + variable;
                    //    output.Add(content);

                    //    content = "";
                    //    content = "  Selected Users: " + variable1;
                    //    output.Add(content);

                    //    content = "";
                    //    content = "  Active Flag: " + item.IsActive;
                    //    output.Add(content);

                    //}
                    //else
                    //{
                    //        string content = "";
                    //        eventname = "  Update";
                    //        string slaa =Convert.ToString (item.SlaActive==true?"true":"false");
                    //        string isact = Convert.ToString(item.IsActive == true ? "true" : "false");
                    //        var maincategory1 = db.LookupItem.Where(x => x.Id == workflowcategory.CategoryId  ).FirstOrDefault();
                    //        var subcategory1 = db.LookupSubitem.Where(x => x.Id == workflowcategory.SubCategoryId ).FirstOrDefault();
                    //        if (String.Equals(maincategory.Value, maincategory1.Value))
                    //        {}
                    //        else
                    //        {   content = " Main Category: " + maincategory.Value + "changed to "+ maincategory1.Value;
                    //            output.Add(content);
                    //        }
                    //        if (String.Equals(subcategory.Value, subcategory1.Value))
                    //        {}
                    //        else
                    //        {   content = ""; content = " Sub Category: " + subcategory.Value + "changed to " + subcategory1.Value;
                    //            output.Add(content);
                    //        }

                    //        if (String.Equals(uASWorkFlow.Level, level))
                    //        {}
                    //        else
                    //        { content = "";
                    //          content = " Level: " + uASWorkFlow.Level + "changed to " + level;
                    //          output.Add(content);
                    //        }
                    //        if (String.Equals(uASWorkFlow.LevelName, item.LevelName))
                    //        {  }
                    //        else
                    //        {    content = "";
                    //            content = " Level: " + uASWorkFlow.LevelName + "changed to " + item.LevelName;
                    //            output.Add(content);
                    //        }
                    //        if (String.Equals(uASWorkFlow.SlaDays, item.SlaDays))
                    //        { }
                    //        else
                    //        { content = "";
                    //            content = " SLA Days: " + uASWorkFlow.SlaDays + "changed to " + item.SlaDays;
                    //            output.Add(content);
                    //        }
                    //       if (String.Equals(uASWorkFlow.SlaActive, Convert.ToString (slaa)))
                    //        { }
                    //        else
                    //        {
                    //            if (Convert.ToString(slaa) == "true")
                    //            {
                    //                content = "";
                    //                  content = " SLA Active:  NO  changed to : YES";
                    //                output.Add(content);
                    //            }
                    //            else
                    //            {
                    //                content = ""; content = " SLA Active:  YES  changed to : NO";
                    //                output.Add(content);
                    //            }
                    //            //   description = description + " It Process : " + useroldData.ItProcessAccess + "  changed to :" + input.ItProcessAccess + "";
                    //        }
                    //        if (String.Equals(uASWorkFlow.IsActive, Convert.ToString(isact)))
                    //        {}
                    //        else
                    //        {
                    //            if (Convert.ToString(isact) == "true")
                    //            {
                    //                content = "";
                    //                content = " Is Active:  NO  changed to : YES";
                    //                output.Add(content);
                    //            }
                    //            else
                    //            {
                    //                content = ""; content = " Is Active:  YES  changed to : NO";
                    //                output.Add(content);
                    //            }
                    //         }

                    //        string variable = "";
                    //    foreach (var it in item.SelectedRoles)
                    //    {
                    //        variable = variable + it.key + ",";
                    //    }
                    //    string variable1 = "";
                    //    foreach (var ite in item.SelectedUsers)
                    //    {
                    //        variable1 = variable1 + ite.UserFirstName + ",";
                    //    }
                    
                    //    content = "";
                    //    content = " Selected Roles: " + variable;
                    //    output.Add(content);

                    //    content = "";
                    //    content = "  Selected Users: " + variable1;
                    //    output.Add(content);
                    //}

                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser =Ipdetails[0];
                    auditdto.eventname = eventname;
                    auditdto.module = "Workflow";
                    auditdto.userid = userid;
                    auditdto.orgid = 1;
                    auditdto.ipaddress= Ipdetails[1];
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                }
                result.Description = "Level Updated Successfully";
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while saving the details.";
                return result;
            }
        }

        public static dynamic LoadLevelDetails(Int32 id)
        {
            FinalResultDTO result = new FinalResultDTO();
            List<UASWorkFlow> levelAvail = new List<UASWorkFlow>();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var levelDetails = db.Workflowdetails.Where(x => x.Id == id).ToList();

                    if (levelDetails.Count <= 0)
                    {
                        result.Status = true;
                        result.ResultOP = levelAvail;
                        return result;
                    }

                    foreach (var item in levelDetails)
                    {
                        UASWorkFlow data = new UASWorkFlow();
                        data.Id = Convert.ToInt32(item.Id);
                        data.CombinationId = Convert.ToInt32(item.CombinationId);
                        data.Level = Convert.ToInt32(item.Level);
                        data.LevelName = item.LevelName;
                        data.SelectedRoles = JsonConvert.DeserializeObject<List<DropDownsDTO>>(item.SelectedRoles);
                        data.SelectedUsers = JsonConvert.DeserializeObject<List<UsersDTO>>(item.SelectedUsers);
                        data.SlaActive = item.SlaActive == "Y" ? true : false;
                        data.SlaDays = item.SlaDays;
                        data.IsActive = item.ActiveFlag == "Y" ? true : false;
                        data.Delete = item.DeleteFlag == "Y" ? true : false;
                        FinalResultDTO usersResult = LoadUsers(data.SelectedRoles);
                        if (usersResult.Status)
                        {
                            data.UserList = usersResult.ResultOP;
                        }
                        levelAvail.Add(data);
                    }

                }
                result.Status = true;
                result.ResultOP = levelAvail;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while saving the details.";
                return result;
            }
        }

        public static dynamic DeleteLevelBasedonlevel(DeleteLevelBase item,Int64 userid,dynamic ipdetails)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var fetchlevelDetails = (from details in db.RequestLevel
                                     join req in db.RequestForm on details.RequestId equals req.Requestid
                                     where (details.LevelId == item.level && req.CategoryId == item.combinationId)
                                     select details).ToList();
                    if (fetchlevelDetails.Count==0)

                    {
                        var audithis = (from details in db.Workflowdetails
                                        join look in db.LookupItem on details.CombinationId equals look.Id
                                        select new
                                        {
                                            key = look.Key,
                                        }).FirstOrDefault();
                        string lookupname = "";
                        if (audithis != null)
                        {
                            lookupname = Convert.ToString(audithis.key);
                        }
                        var levelmasli = db.LevelMaster.Where(x => x.Id == item.level).ToList().FirstOrDefault();
                        string levelname = "";
                        if (levelmasli != null)
                        {
                            levelname = Convert.ToString(levelmasli.WorkFlowLevelName);
                        }
                        var deleteuserDetails =
                           from details in db.WorkflowUsers
                           where (details.LevelId == item.level && details.WorkFlowId == item.combinationId)
                           select details;
                        foreach (var detail in deleteuserDetails)
                        {
                            var singleRec = db.WorkflowUsers.FirstOrDefault(x => x.Id == detail.Id);
                            db.WorkflowUsers.Remove(singleRec);
                            db.SaveChanges();
                        }
                        var deletelevelDetails =
                                        from details in db.WorkFlowLevel
                                        where (details.LevelId == item.level && details.WorkFlowId == item.combinationId)
                                        select details;

                        foreach (var detail in deletelevelDetails)
                        {
                            var singleRec = db.WorkFlowLevel.FirstOrDefault(x => x.Id == detail.Id);
                            db.WorkFlowLevel.Remove(singleRec);
                            db.SaveChanges();
                        }
                        string eventname = "";
                        string description = "";
                        List<string> output = new List<string>();

                        {
                            eventname = "  Delete level";
                            string content = "";
                            content = " Category Name: " + lookupname;
                            output.Add(content);

                            content = "";
                            content = " Level: " + levelname;
                            output.Add(content);
                        }
                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "WorkFlow";
                        auditdto.userid = userid;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);

                        if (deletelevelDetails == null)
                        {
                            result.Status = false;
                            result.Description = "Something went wrong while deleting the details.";
                            return result;
                        }
                    }
                    else {
                        result.Status = false;
                        result.Description = "This workflow already we used in request page.";
                        return result;
                    }
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic DeleteLevel(Int32 id)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    if (id > 0)
                    {
                        var exist =
                          from details in db.RequestForm
                          where (details.CategoryId == id)
                          select details;
                        if (exist == null)
                        {
                            var deleteuserDetails =
                               from details in db.WorkflowUsers
                               where (details.WorkFlowId == id)
                               select details;

                            foreach (var detail in deleteuserDetails)
                            {
                                var singleRec = db.WorkflowUsers.FirstOrDefault(x => x.Id == detail.Id);
                                db.WorkflowUsers.Remove(singleRec);
                                db.SaveChanges();
                            }
                            var deletelevelDetails =
                                            from details in db.WorkFlowLevel
                                            where (details.WorkFlowId == id)
                                            select details;

                            foreach (var detail in deletelevelDetails)
                            {
                                var singleRec = db.WorkFlowLevel.FirstOrDefault(x => x.Id == detail.Id);
                                db.WorkFlowLevel.Remove(singleRec);
                                db.SaveChanges();
                            }
                            var singleRecc = db.Workflowdetails.FirstOrDefault(x => x.CombinationId == id);
                            db.Workflowdetails.Remove(singleRecc);
                            db.SaveChanges();

                            if (singleRecc == null)
                            {
                                result.Status = false;
                                result.Description = "Something went wrong while deleting the details.";
                                return result;
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.Description = "Already category used in workflow.";
                            return result;
                        }
                    }
                    result.Status = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic SaveLevelsNumber(List<WorkFlowMasterDetails> input)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    foreach (var item in input)
                    {
                        var levelDetails = db.Workflowdetails.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (levelDetails != null)
                        {
                            levelDetails.Level = item.Level;
                            db.SaveChanges();
                        }
                    }
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting the details.";
                return result;
            }
        }

        public static dynamic LoadWorkFlowCLDetails(UASWorkFlowCL inputParam,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int32 fromCategoryId = inputParam.FromSelectedCategory[0].Id;
                    Int32 fromSubCategoryId = inputParam.FromSelectedSubCategory[0].Id;

                    var selectedList = inputParam.SelectedLevels.Where(x => x.selectedCheck == true).ToList();
                    foreach (var item in inputParam.ToSelectedSubCategory)
                    {
                        if (item.Id == fromSubCategoryId && item.CategoryId == fromCategoryId)
                        {
                            continue;
                        }

                        var wfCategoryData = db.Workflowcategory.Where(x => x.CategoryId == item.CategoryId && x.SubCategoryId == item.Id).FirstOrDefault();
                        Int32 CombinationId = 0;
                        Int32 levelNo = 1;
                        if (wfCategoryData == null)
                        {
                            var workflowCategoryData = new Workflowcategory
                            {
                                CategoryId = item.CategoryId,
                                SubCategoryId = item.Id
                            };
                            db.Workflowcategory.Add(workflowCategoryData);
                            db.SaveChanges();

                            CombinationId = Convert.ToInt32(workflowCategoryData.Id);
                        }
                        else
                        {
                            CombinationId = Convert.ToInt32(wfCategoryData.Id);
                            var existingLevel = db.Workflowdetails.Where(x => x.CombinationId == CombinationId && x.DeleteFlag == "N" && x.ActiveFlag == "Y").OrderByDescending(x => x.Level).FirstOrDefault();
                            if (existingLevel != null)
                            {
                                levelNo = Convert.ToInt32(existingLevel.Level + 1);
                            }
                        }

                        foreach (var level in selectedList)
                        {
                            var levelDetails = db.Workflowdetails.Where(x => x.Id == level.Id).FirstOrDefault();

                            if (levelDetails != null)
                            {
                                var workflow = new Workflowdetails
                                {
                                    CombinationId = CombinationId,
                                    Level = levelNo,
                                    LevelName = levelDetails.LevelName,
                                    SlaActive = levelDetails.SlaActive,
                                    SlaDays = levelDetails.SlaDays,
                                    ActiveFlag = levelDetails.ActiveFlag,
                                    DeleteFlag = "N",
                                    SelectedRoles = levelDetails.SelectedRoles,
                                    SelectedUsers = levelDetails.SelectedUsers,
                                    CreatedBy = 1,
                                    CreatedDate = dateTime_Indian,
                                    ModifiedBy = 1,
                                    ModifiedDate = dateTime_Indian
                                };
                                db.Workflowdetails.Add(workflow);
                                db.SaveChanges();

                                levelNo = levelNo + 1;
                            }
                        }
                    }

                    result.Status = true;
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


    }
}
