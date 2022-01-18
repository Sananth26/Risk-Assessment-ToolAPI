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
    public class LookUPMasterDAL
    {
        //Fetch the Category List for Dropdown
        public static FinalResultDTO GetLockUpMaster()
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var LookUpData = (from sa in db.LookupCategory
                                      select new
                                      {
                                          id = sa.Id,
                                          key = sa.Name,
                                      }).ToList();
                    resut.Status = true;
                    resut.ResultOP = LookUpData;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = DateTime.Now;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = 0;
                errorLog.Methodname = "GetLockUpMaster";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to Fetch the data..";
                return resut;
            }
        }

        //Create/Insert the category and sub category
        public static FinalResultDTO CreateLookupMaster(AddNewCategory inputParams,Int64 userid,string timezone)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int64 newid = 0;
                    if (inputParams.Type == "category")
                    {
                        var lookupMaster = new LookupCategory
                        {
                            Name = inputParams.Name,
                            Description = inputParams.Description,
                            CreatedBy = userid,
                            CreatedTime = dateTime_Indian,
                            UpdatedBy = userid,
                            UpdatedTime = dateTime_Indian
                        };
                        db.LookupCategory.Add(lookupMaster);
                        db.SaveChanges();
                        newid = lookupMaster.Id;
                        var LookUpData = (from sa in db.LookupCategory
                                          select new
                                          {
                                              id = sa.Id,
                                              key = sa.Name,
                                          }).ToList();

                        resut.ResultOP = LookUpData;
                    }
                    else
                    {
                        var lookupSubMaster = new LookupSubitem
                        {
                            SubcategoryId = inputParams.MainCategoryId,
                            Value = inputParams.Name,
                            Key = inputParams.Description,
                            CreatedBy = userid,
                            CreatedTime = dateTime_Indian,
                            UpdatedBy = userid,
                            UpdatedTime = dateTime_Indian
                        };
                        db.LookupSubitem.Add(lookupSubMaster);
                        db.SaveChanges();
                        newid = lookupSubMaster.Id;
                        var LookUpData = (from sa in db.LookupSubitem
                                          where sa.SubcategoryId == inputParams.MainCategoryId
                                          select new
                                          {
                                              id = sa.Id,
                                              key = sa.Value,
                                          }).ToList();

                        resut.ResultOP = LookUpData;
                    }

                    string eventname = "";
                    string description = "";
                    List<string> output = new List<string>();
                    if (inputParams.Type == "category")
                    {
                      eventname = "Create";
                      string  content = "";
                        content = "New Category= " + inputParams.Name + "";
                        output.Add(content);
                        content = "";
                        content = "Description= " + inputParams.Description + "";
                        output.Add(content);
                        //description = "New Category Name =" + inputParams.Name + " , Description= " + inputParams.Description + ", InsertedId :" + newid + " ";
                    }
                    else
                    {
                        var LookUpData = (from sa in db.LookupCategory
                                          where sa.Id == inputParams.MainCategoryId
                                          select new
                                          {
                                              id = sa.Id,
                                              key = sa.Name,
                                          }).ToList();
                        eventname = "Create";
                        string content = "";
                        content = "MainCategory Name=  " + LookUpData[0].key + "";
                        output.Add(content);

                        content = "";
                        content = " Subcategory Name=  " + inputParams.Name + "";
                        output.Add(content);
                        content = "";
                        content = " Description=  " + inputParams.Description + "";
                        output.Add(content);
                        //description = " MainCategory Name =" + LookUpData[0].key + " , Subcategory Name =" + inputParams.Name + ", Description=" + inputParams.Description + " ";
                    }

                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = System.Environment.MachineName;
                    auditdto.eventname = eventname;
                    auditdto.module = "Lookup Master";
                    auditdto.userid = userid;
                    auditdto.orgid = 1;
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime =dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = userid;
                errorLog.Methodname = "CreateLookupMaster";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the Look Up Master Data..";
                return resut;
            }
        }

        //On category dropdown change
        public static FinalResultDTO GetLookupItems(Int32 categoryId)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    if (categoryId == 8)
                    {
                        var categoryItem = (from sa in db.Department
                                            where sa.IsActive == "Y"  
                                            select new LookUpItemDTO
                                            {
                                                Id = sa.Id,
                                                Key = sa.DepartmentName,
                                                Value = sa.DepartmentName,
                                                DisplayOrder =0,
                                            }).ToList();

                        resut.Status = true;
                        resut.ResultOP = categoryItem;
                        return resut;
                    }
                    else {
                        var categoryItem = (from sa in db.LookupItem
                                            where sa.ActiveFlag == "Y" && sa.CategoryId == Convert.ToInt64(categoryId)
                                            select new LookUpItemDTO
                                            {
                                                Id = sa.Id,
                                                Key = sa.Key,
                                                Value = sa.Value,
                                                DisplayOrder = Convert.ToInt16(sa.DisplayOrder),
                                            }).ToList();

                        resut.Status = true;
                        resut.ResultOP = categoryItem;
                        return resut;
                    }
                }
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime =dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = 0;
                errorLog.Methodname = "GetLookupItems";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to Fetch the data..";
                return resut;
            }
        }


        //On Sub-category dropdown change
        public static FinalResultDTO GetLookupSubItems(Int32 subCategoryId)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var categoryItem = (from sa in db.LookupSubitem
                                        where sa.ActiveFlag == "Y" && sa.SubcategoryId == Convert.ToInt64(subCategoryId)
                                        select new LookUpItemDTO
                                        {
                                            Id = sa.Id,
                                            Key = sa.Key,
                                            Value = sa.Value,
                                            DisplayOrder = sa.DisplayOrder,
                                        }).ToList();
 
                    resut.Status = true;
                    resut.ResultOP = categoryItem;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = 0;
                errorLog.Methodname = "GetLookupSubItems";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to Fetch the data..";
                return resut;
            }
        }

        //Delete the Category/Sub-cat Items
        public static FinalResultDTO LookUpDeleteData(Int32 id, string type,Int64 userid,dynamic Ipdetails,string timezone)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    if (type == "category")
                    {
                        var singleRec = db.LookupItem.FirstOrDefault(x => x.Id == id);
                        singleRec.ActiveFlag = "N";
                        db.SaveChanges();

                        string eventname = "";
                        string description = "";
                        List<string> output = new List<string>();
                        eventname = "Delete";
                        description = " Key =" + singleRec.Key + " , Value= " + singleRec.Value + " ,DeletedId :" + id + "";
                        string content = "";
                        content = " Key =" + singleRec.Key + "";
                        output.Add(content);

                        content = "";
                        content = "  Value= " + singleRec.Value + "";
                        output.Add(content);

                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "Lookup Master";
                        auditdto.userid = userid;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);

                        var categoryItem = (from sa in db.LookupItem
                                            where sa.ActiveFlag == "Y" && sa.CategoryId == Convert.ToInt64(id)
                                            select new LookUpItemDTO
                                            {
                                                Id = sa.Id,
                                                Key = sa.Key,
                                                Value = sa.Value,
                                                DisplayOrder = Convert.ToInt16(sa.DisplayOrder),
                                            }).ToList();

                        resut.ResultOP = categoryItem;
                    }
                    else
                    {
                        var singleRec = db.LookupSubitem.FirstOrDefault(x => x.Id == id);
                        singleRec.ActiveFlag = "N";
                        db.SaveChanges();

                        var categoryItem = (from sa in db.LookupSubitem
                                            where sa.ActiveFlag == "Y" && sa.SubcategoryId == Convert.ToInt64(id)
                                            select new LookUpItemDTO
                                            {
                                                Id = sa.Id,
                                                Key = sa.Key,
                                                Value = sa.Value,
                                                DisplayOrder = sa.DisplayOrder,
                                            }).ToList();

                        string eventname = "";
                        string description = "";
                        List<string> output = new List<string>();
                        eventname = "Delete";
                        // description = " Key =" + singleRec.Key + " , Value= " + singleRec.Value + " ,DeletedId :" + id + " deleted sub item";
                        string content = "";
                        content = " Key =" + singleRec.Key + "";
                        output.Add(content);

                        content = "";
                        content = "  Value= " + singleRec.Value + "";
                        output.Add(content);

                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "Lookup Master";
                        auditdto.userid = userid;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.orgid = 1;
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                        resut.ResultOP = categoryItem;
                    }
                  
                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = userid;
                errorLog.Methodname = "LookUpDeleteData";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, While Deleting the Record";
                return resut;
            }
        }

        //Insert Update the category items..
        public static FinalResultDTO InsertLookUpItem(LookUpItemDTO inputParams,Int64 userid,dynamic Ipdetails,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var lookupMaster1 = db.LookupItem.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                    Int64 newid = 0;
                    if (inputParams.Type == "category")
                    {
                        if (inputParams.CategoryId == 8)
                        {
                            if (inputParams.Id == 0)
                            {
                                var newDept = new Department
                                {
                                    DepartmentName = inputParams.Key,
                                    NoOfLevel = 0,
                                    IsActive = "Y",
                                    // IsActive = input.IsActive == true ? "Y" : "N",
                                    DeleteFlag = "N",
                                    CreatedBy = Convert.ToInt32(userid),
                                    CreatedDate = dateTime_Indian,
                                    SlaFlag = "",
                                    SlaJson = "",
                                };
                                    db.Department.Add(newDept);
                                db.SaveChanges();
                                                       
                               

                            }
                            else
                            {
                                var DeptartmentData = db.Department.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                                DeptartmentData.DepartmentName = inputParams.Key;
                                DeptartmentData.NoOfLevel = 0;
                                DeptartmentData.IsActive = "Y";
                                DeptartmentData.SlaFlag = "";
                                DeptartmentData.SlaJson = "";
                                //DeptartmentData.IsActive = input.IsActive == true ? "Y" : "N";
                                DeptartmentData.ModifiedBy = Convert.ToInt32(userid);
                                DeptartmentData.ModifiedDate = dateTime_Indian;
                                db.SaveChanges();
                            }

                        }
                        else
                        {

                            if (inputParams.Id == 0)
                            {
                                var lookupMaster = new LookupItem
                                {
                                    CategoryId = inputParams.CategoryId,
                                    Key = inputParams.Key,
                                    Value = inputParams.Value,
                                    DisplayOrder = inputParams.DisplayOrder,
                                    ActiveFlag = "Y",
                                    CreatedBy = userid,
                                    CreatedTime = dateTime_Indian,
                                };
                                db.LookupItem.Add(lookupMaster);
                                db.SaveChanges();
                                newid = lookupMaster.Id;
                            }
                            else
                            {
                                var lookupMaster = db.LookupItem.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                                {
                                    lookupMaster.DisplayOrder = inputParams.DisplayOrder;
                                    lookupMaster.Key = inputParams.Key;
                                    lookupMaster.Value = inputParams.Value;
                                    lookupMaster.UpdatedBy = userid;
                                    lookupMaster.UpdatedTime = dateTime_Indian;
                                };
                                db.SaveChanges();
                            }
                        }

                        if (inputParams.CategoryId == 8)
                        {
                            var userList = (from dept in db.Department
                                            where dept.DeleteFlag == "N"
                                            orderby dept.Id descending
                                            select new LookUpItemDTO
                                            {
                                                Id = Convert.ToInt32(dept.Id),
                                                Key = dept.DepartmentName,
                                                Value = dept.DepartmentName,
                                               
                                                // IsActive = dept.IsActive == "Y" ? true : false

                                            }).ToList();

                            resut.Status = true;
                            resut.ResultOP = userList;
                            return resut;
                        }
                        else
                        {
                            var categoryItem = (from sa in db.LookupItem
                                                where sa.ActiveFlag == "Y" && sa.CategoryId == Convert.ToInt64(inputParams.CategoryId)
                                                select new LookUpItemDTO
                                                {
                                                    Id = sa.Id,
                                                    Key = sa.Key,
                                                    Value = sa.Value,
                                                    DisplayOrder = Convert.ToInt16(sa.DisplayOrder),
                                                }).ToList();

                            resut.ResultOP = categoryItem;
                        }
                        string eventname = "";
                        string description = "";
                        List<string> output = new List<string>();
                        string content = "";
                        if (inputParams.Type == "category")
                        {
                            if (inputParams.Id == 0)
                            {
                                var LookUpData = db.LookupItem.Where(x => x.Id == newid).FirstOrDefault();
                                var categoryname = db.LookupCategory.Where(x => x.Id == LookUpData.CategoryId).Select(x => x.Name).FirstOrDefault();
                                eventname = "Create";
                                content = "";
                                content = " Lookup Category Name= " + categoryname + "";
                                output.Add(content);

                                content = "";
                                content = " Lookup Item Name =  " + inputParams.Key + "";
                                output.Add(content);

                                content = "";
                                content = " Lookup Item Value=  " + inputParams.Value + "";
                                output.Add(content);

                                description = "Lookup Category Name =" + categoryname + " ,Lookup Item Name =" + inputParams.Key + " ,Lookup Item Value =" + inputParams.Value + " ";
                            }
                            else
                            {
                                
                                eventname = "Update";
                                if (String.Equals(lookupMaster1.Key, inputParams.Key))
                                { }
                                else
                                {
                                    //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                                    content = "";
                                    content = " Lookup Item Name: Changed from   " + lookupMaster1.Key + "  to:  " + inputParams.Key + "";
                                    output.Add(content);
                                }

                                if (String.Equals(lookupMaster1.Value, inputParams.Value))
                                { }
                                else
                                {
                                    //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                                    content = "";
                                    content = " Lookup Item Value: Changed from   " + lookupMaster1.Value + "  to:  " + inputParams.Value + "";
                                    output.Add(content);
                                }

                                //description = "Lookup Category Name =" + LookUpData.Name + " ,Lookup Item Name =" + inputParams.Key + " ,Lookup Item Value =" + inputParams.Value + " ,  UpdatedId :" + inputParams.Id + " ";
                            }

                            AuditTrialDTO auditdto = new AuditTrialDTO();
                            AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                            auditdto.browser = Ipdetails[0];
                            auditdto.eventname = eventname;
                            auditdto.module = "Lookup Item";
                            auditdto.userid = userid;
                            auditdto.orgid = 1;
                            auditdto.ipaddress = Ipdetails[1];
                            auditdto.description = JsonConvert.SerializeObject(output);
                            auditdto.Systemremarks = description;
                            AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                        }
                        else
                        {
                           
                        }
                    }
                    else
                    {
                        string eventname = "";
                        string description = "";
                        if (inputParams.Id == 0)
                        {
                            var lookupMaster = new LookupSubitem
                            {
                                SubcategoryId = inputParams.CategoryId,
                                Key = inputParams.Key,
                                Value = inputParams.Value,
                                DisplayOrder = inputParams.DisplayOrder,
                                ActiveFlag = "Y",
                                CreatedBy = userid,
                                CreatedTime = dateTime_Indian,
                                UpdatedTime = dateTime_Indian
                            };
                            db.LookupSubitem.Add(lookupMaster);
                            db.SaveChanges();
                           
                            newid = lookupMaster.Id;
                        }
                        else
                        {
                            var lookupMaster = db.LookupSubitem.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                            {
                                lookupMaster.DisplayOrder = inputParams.DisplayOrder;
                                lookupMaster.Key = inputParams.Key;
                                lookupMaster.Value = inputParams.Value;
                                lookupMaster.UpdatedBy = userid;
                                lookupMaster.UpdatedTime = dateTime_Indian;
                            };
                            db.SaveChanges();
                        
                        }

                        //Audit trial
                        string content = "";
                        List<string> output = new List<string>();
                        if (inputParams.Id == 0)
                        {
                            var LookUpitem = db.LookupItem.Where(x => x.Id == inputParams.CategoryId).FirstOrDefault();
                            var LookUpData = db.LookupCategory.Where(x => x.Id == LookUpitem.CategoryId).FirstOrDefault();
                           
                            eventname = "Create";
                            content = "";
                            content = " Lookup Subcategory Name=  " + LookUpData.Name + "";
                            output.Add(content);

                            content = "";
                            content = " Lookup Subitem Name=  " + inputParams.Key + "";
                            output.Add(content);

                            content = "";
                            content = " Lookup Subitem Value=  " + inputParams.Value + "";
                            output.Add(content);

                            //description = "Lookup Category Name =" + LookUpData.Name + " ,Sub category Name =" + inputParams.Key + " ,Sub Category Name =" + inputParams.Value + " ,  InsertedId :" + newid + " ";
                        }
                        else
                        {
                            var LookUpData = db.LookupCategory.Where(x => x.Id == inputParams.CategoryId).FirstOrDefault();
                            var lookupsubitem = db.LookupSubitem.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                            eventname = "Update";

                            if (String.Equals(lookupsubitem.Key, inputParams.Key))
                            { }
                            else
                            {
                                //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                                content = "";
                                content = " Lookup Subitem Name: Changed from   " + lookupsubitem.Key + "  to:  " + inputParams.Key + "";
                                output.Add(content);
                            }

                            if (String.Equals(lookupsubitem.Value, inputParams.Value))
                            { }
                            else
                            {
                                //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                                content = "";
                                content = " Lookup Subitem Value: Changed from   " + lookupsubitem.Value + "  to:  " + inputParams.Value + "";
                                output.Add(content);
                            }

                            description = "Lookup Category Name =" + LookUpData.Name + " ,Sub category Name =" + inputParams.Key + " ,Sub Category Name =" + inputParams.Value + " ,  UpdatedId :" + inputParams.Id + " ";
                        }

                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "Lookup subitem";
                        auditdto.userid = userid;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);

                        var categoryItem = (from sa in db.LookupSubitem
                                            where sa.ActiveFlag == "Y" && sa.SubcategoryId == Convert.ToInt64(inputParams.CategoryId)
                                            select new LookUpItemDTO
                                            {
                                                Id = sa.Id,
                                                Key = sa.Key,
                                                Value = sa.Value,
                                                DisplayOrder = sa.DisplayOrder,
                                            }).ToList();

                        resut.ResultOP = categoryItem;

                    }

                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime =dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = userid;
                errorLog.Methodname = "InsertLookUpItem";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the Look Up Master Data..";
                return resut;
            }
        }

        public static FinalResultDTO GetLookupMaster(string LookUpName)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var lookUpDetails = db.LookupCategory.Where(x => x.Name == LookUpName).FirstOrDefault();

                    if (lookUpDetails != null)
                    {
                        var LookUpData = (from sa in db.LookupItem
                                          where sa.ActiveFlag == "Y" && sa.CategoryId == lookUpDetails.Id
                                          select new LookUpItemDTO
                                          {
                                              Id = sa.Id,
                                              Key = sa.Key,
                                              Value = sa.Value,
                                              DisplayOrder = Convert.ToInt16(sa.DisplayOrder),

                                          }).ToList();

                        resut.Status = true;
                        resut.ResultOP = LookUpData;
                    }
                    else
                    {
                        resut.Description = "No Data available For this LookupCategory";
                    }
                    return resut;
 
                }
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = 0;
                errorLog.Methodname = "GetLookupMaster";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the Look Up Master Data..";
                return resut;
            }
        }

        public static FinalResultDTO GetsubLookupMaster(string LookUpName)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                      var LookUpData = (from sa in db.LookupSubitem
                                          where sa.ActiveFlag == "Y" 
                                          select new LookUpItemDTO
                                          {
                                              Id = sa.Id,
                                              Key = sa.Key,
                                              Value = sa.Value,
                                              DisplayOrder = Convert.ToInt16(sa.DisplayOrder),

                                          }).ToList();
                        resut.Status = true;
                        resut.ResultOP = LookUpData;
                        return resut;

                }
            }
            catch (Exception ex)
            {
                TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "LockUpMaster";
                errorLog.Userid = 0;
                errorLog.Methodname = "GetsubLookupMaster";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the Look Up Master Data..";
                return resut;
            }
        }





    }
}
