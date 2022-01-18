using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class DepartmentMasterDAL
    {


        public static dynamic LoadDeptDatas()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userList = (from  dept in db.Department 
                                    where dept.DeleteFlag == "N"
                                    orderby dept.Id descending
                                    select new DepartmentDTO
                                    {
                                        Id = Convert.ToInt32(dept.Id),
                                        DepartmentName = dept.DepartmentName,
                                        NoOfLevel = dept.NoOfLevel,
                                        IsActive = dept.IsActive, 
                                        SlaFlag = dept.SlaFlag,
                                        SlaJson = dept.SlaJson ?? "",
                                       // IsActive = dept.IsActive == "Y" ? true : false

                                    }).ToList();

                    result.Status = true;
                    result.ResultOP = userList;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching necessary details.";
                return result;
            }
        }

        public static dynamic LoadDeptForEdit(Int32 deptId)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userList = (from  dept in db.Department 

                                    where dept.Id == deptId
                                    select new DepartmentDTO
                                    {
                                        Id = Convert.ToInt32(dept.Id),
                                        DepartmentName = dept.DepartmentName,
                                        NoOfLevel = dept.NoOfLevel,
                                        IsActive = dept.IsActive,
                                        SlaJson = dept.SlaJson ?? "",
                                        SlaFlag = dept.SlaFlag,
                                        //IsActive = dept.IsActive == "Y" ? true : false

                                    }).FirstOrDefault();

                    result.Status = true;
                    result.ResultOP = userList;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while fetching necessary details.";
                return result;
            }
        }

        public static dynamic CreateOrUpdateDept(DepartmentDTO input,Int64 userid,dynamic Ipdetails)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    
                  //  var OldDeptartmentData = db.Department.Where(x => x.Id == input.Id).FirstOrDefault();
                    var OldDeptartmentData = (from sa in db.Department
                                        where   sa.Id == input.Id
                                        select new Department
                                        {
                                            DepartmentName = sa.DepartmentName,
                                            NoOfLevel = sa.NoOfLevel,
                                            IsActive = sa.IsActive,
                                           
                                            SlaFlag = sa.SlaFlag,
                                            SlaJson = sa.SlaJson
                                        }).FirstOrDefault();
                    if (input.Id == 0)
                    {
                        var newDept = new Department
                        {
                            DepartmentName = input.DepartmentName,
                            NoOfLevel = input.NoOfLevel,
                            IsActive = input.IsActive, 
                           // IsActive = input.IsActive == true ? "Y" : "N",
                            DeleteFlag = "N",
                            CreatedBy = Convert.ToInt32(userid),
                            CreatedDate = DateTime.Now,
                            SlaFlag = input.SlaFlag,
                            SlaJson = input.SlaJson
                        
                        };
                        db.Department.Add(newDept);
                        db.SaveChanges();
                       
                    }
                    else
                    {
                        var DeptartmentData = db.Department.Where(x => x.Id == input.Id).FirstOrDefault();
                        DeptartmentData.DepartmentName = input.DepartmentName;
                        DeptartmentData.NoOfLevel = input.NoOfLevel;
                        DeptartmentData.IsActive = input.IsActive;
                        DeptartmentData.SlaFlag = input.SlaFlag;
                        DeptartmentData.SlaJson = input.SlaJson;
                        //DeptartmentData.IsActive = input.IsActive == true ? "Y" : "N";
                        DeptartmentData.ModifiedBy = Convert.ToInt32(userid);
                        DeptartmentData.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                    }

                    string eventname = "";
                    string description = "";
                    List<string> output = new List<string>();
                    string content = "";
                    if (input.Id == 0)
                    {
                             eventname = "Create";
                            content = "Departmentname : " + input.DepartmentName + "";
                            output.Add(content);

                            content = "";
                            content = " NoOfLevel: " + input.NoOfLevel + "";
                            output.Add(content);

                            content = "";
                            content = input.IsActive == "Y" ? "IsActive: Yes" : "IsActive: No";
                            output.Add(content);

                            content = "";
                            content = input.SlaFlag == "Y" ? "SLA: Yes" : "SLA: No";
                            output.Add(content);

                        /*description = " Department Name :" + input.DepartmentName + " , Level: " + input.NoOfLevel + ", IsActive: " + input.IsActive + ", SLA Flag : " + input.SlaFlag + " ,SLA:  " + input.SlaJson + "   ";*/
                    }
                    else
                    {
                         
                        eventname = "Update";

                        if (String.Equals(OldDeptartmentData.DepartmentName, input.DepartmentName))
                        { }
                        else
                        { //description = " Start date: " + oldholidaydetails.Startdate + "  changed to :" + holidays.start + ""; 
                            content = " Department: Changed from  " + OldDeptartmentData.DepartmentName + " to: " + input.DepartmentName + "";
                            output.Add(content);
                        }

                        if (String.Equals(OldDeptartmentData.NoOfLevel, input.NoOfLevel))
                        { }
                        else
                        {
                            //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                            content = "";
                            content = " NoOfLevel: Changed from   " + OldDeptartmentData.NoOfLevel + "  to:  " + input.NoOfLevel + "";
                            output.Add(content);
                        }

                        if (String.Equals(OldDeptartmentData.IsActive, input.IsActive))
                        { }
                        else
                        {
                            //description = description + " Title : " + oldholidaydetails.Title + "  changed to :" + holidays.title + ""; 
                            content = "";

                            if (input.IsActive == "Y")
                            {
                                content = " It Active: Changed from  NO  to : YES";
                                output.Add(content);
                            }
                            else
                            {
                                content = " It Active: Changed from  YES  to : NO";
                                output.Add(content);
                            }
                                                      
                        }

                        if (String.Equals(OldDeptartmentData.SlaFlag, input.SlaFlag))
                        { }
                        else
                        {
                            //description = description + " Title : " + oldholidaydetails.Title + "  changed to :" + holidays.title + ""; 
                            content = "";

                            if (input.SlaFlag == "Y")
                            {
                                content = " SLA: Changed from    NO  to : YES";
                                output.Add(content);
                            }
                            else
                            {
                                content = " SLA: Changed from  YES  to : NO";
                                output.Add(content);
                            }
                        }
                    }

                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = Ipdetails[0];
                    auditdto.eventname = eventname;
                    auditdto.module = "Department";
                    auditdto.userid = Convert.ToInt32(userid);
                    auditdto.orgid = 1;
                    auditdto.ipaddress= Ipdetails[1];
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                                       
                    FinalResultDTO resultLoadUserDatas = LoadDeptDatas();
                    if (resultLoadUserDatas.Status)
                    {
                        result.Status = true;
                        result.ResultOP = resultLoadUserDatas.ResultOP;
                    }
                    else
                    {
                        result.Status = false;
                        result.Description = "The Department details are inserted Succesfully, but failed to load the data. Please refresh the page..";
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while inserting the Department details.";
                return result;
            }
        }

        public static dynamic DeleteDept(Int32 deptId,Int64 userid)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userData = db.Department.Where(x => x.Id == deptId).FirstOrDefault();
                    if(userData.IsActive == "Y")
                    {
                        result.Description = "The Department is Active.Can not Delete Active Data";
                        result.Status = false;
                    }
                    else
                    {
                        userData.DeleteFlag = "Y";
                        db.SaveChanges();
                        result.Description = "The Department has been deleted Succesfully.";
                        result.Status = true;
                    }

                    string eventname = "";
                    string description = "";
                    List<string> output = new List<string>();
                     eventname = "Delete";
                    string content = "";
                    content = "Departmentname : " + userData.DepartmentName + "";
                    output.Add(content);
                    //description = " Department Name :" + userData.DepartmentName + " , Level: " + userData.NoOfLevel + ", IsActive: " + userData.IsActive + ", SLA Flag : " + userData.SlaFlag + " ,SLA:  " + userData.SlaJson + " ,DeletedId :" + deptId + " ";
                    
                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = System.Environment.MachineName;
                    auditdto.eventname = eventname;
                    auditdto.module = "Department";
                    auditdto.userid =userid ;
                    auditdto.orgid = 1;

                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);


                    //FinalResultDTO resultLoadUserDatas = LoadDeptDatas();
                    //if (resultLoadUserDatas.Status)
                    //{
                    //    result.Status = true;
                    //    result.ResultOP = resultLoadUserDatas.ResultOP;
                    //}
                    //else
                    //{
                    //    result.Status = false;
                    //    result.Description = "The Department has been deleted Succesfully, but failed to load the data. Please refresh the page..";
                    //}

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while deleting selected Department.";
                return result;
            }
        }
    }
}
