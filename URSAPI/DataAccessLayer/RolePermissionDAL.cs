using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;
using static URSAPI.ModelDTO.RolePermissionDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using URSAPI.DataAccessLayer;
using URSAPI.Controllers;

namespace URSAPI.DAL
{
    public class RolePermissionDAL
    {

        public static FinalResultDTO LoadRolePermission(Int32 roleId,string time)
        {
            //roleId = 2;
            FinalResultDTO resut = new FinalResultDTO();
            ButtonPermisionDTO buttonPermissionDatas1 = new ButtonPermisionDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RolePermissionDTO2> rolePermissionEditDTOs = new List<RolePermissionDTO2>();
                    var rolelist = db.RolePermission.Where(x => x.RoleId == roleId).ToList();
                    if (rolelist.Count > 0)
                    {
                        var templateNames = (from MO in db.Modules
                                             join RP in db.RolePermission on MO.Id equals RP.ModuleId
                                             join Ro in db.Roles on RP.RoleId equals Ro.Id
                                             where RP.RoleId == roleId
                                             select new
                                             {
                                                 moduleId = MO.Id,
                                                 moduleName = MO.Title,
                                                 type = MO.Type,
                                                 icon = MO.Icon,
                                                 companyid = MO.CompanyId,
                                                 displayorder = MO.DisplayOrder,
                                                 activeFlag = RP.ActiveFlag,
                                                 buttonPermissionDatas =  JsonConvert.DeserializeObject <ButtonPermisionDTO >(RP.ButtonPermissionData),
                                                 dynamicPresence = MO.DynamicPresence,
                                                 url = "",
                                                 roleid = RP.RoleId,
                                                 rolename = Ro.Name
                                             }).ToList();

                        foreach (var item in templateNames)
                        {
                            var rolepermissiondto = new RolePermissionDTO2();
                            rolepermissiondto.moduleId =Convert.ToInt32( item.moduleId);
                            rolepermissiondto.moduleName = item.moduleName;
                            rolepermissiondto.type = item.type;
                            rolepermissiondto.icon = item.icon;
                            rolepermissiondto.companyid = Convert.ToInt32(item.companyid);
                            rolepermissiondto.displayorder = Convert.ToInt32( item.displayorder);
                            rolepermissiondto.activeFlag =   item.activeFlag == "Y" ? true : false;
                            rolepermissiondto.buttonPermissionDatas = item.buttonPermissionDatas;
                            rolepermissiondto.dynamicPresence = item.dynamicPresence;
                            rolepermissiondto.roleid =Convert.ToInt32(item.roleid);
                            rolepermissiondto.rolename = item.rolename;
                            rolePermissionEditDTOs.Add(rolepermissiondto);
                        }
                        ButtondefaultDTO empt = new ButtondefaultDTO();
                        string empval = Newtonsoft.Json.JsonConvert.SerializeObject(empt);
                        var totalmoduleList = (from MO in db.Modules
                                                 // join RP in db.RolePermission on MO.Id equals RP.ModuleId
                                                 // join Ro in db.Roles on RP.RoleId equals Ro.Id
                                                 // where RP.ActiveFlag == "Y"
                                             select new
                                             {
                                                 moduleId = MO.Id,
                                                 moduleName = MO.Title,
                                                 type = MO.Type,
                                                 icon = MO.Icon,
                                                 companyid = MO.CompanyId,
                                                 displayorder = MO.DisplayOrder,
                                                 activeFlag = "Y",
                                                 buttonPermissionDatas = JsonConvert.DeserializeObject<ButtonPermisionDTO>(empval),
                                                 // buttonPermissionDatas = "",
                                                 dynamicPresence = MO.DynamicPresence,
                                                 url = "",
                                                 roleid = roleId,
                                                 rolename = "",
                                             }).ToList();
                        foreach (var item in totalmoduleList)
                        {
                            var exist = rolePermissionEditDTOs.Where(x => x.moduleId == item.moduleId).FirstOrDefault();
                            if (exist == null)
                            {
                                var rolepermissiondto = new RolePermissionDTO2();
                                rolepermissiondto.moduleId = Convert.ToInt32(item.moduleId);
                                rolepermissiondto.moduleName = item.moduleName;
                                rolepermissiondto.type = item.type;
                                rolepermissiondto.icon = item.icon;
                                rolepermissiondto.companyid = Convert.ToInt32(item.companyid);
                                rolepermissiondto.displayorder = Convert.ToInt32(item.displayorder);
                                rolepermissiondto.activeFlag = item.activeFlag == "Y" ? true : false;
                                rolepermissiondto.buttonPermissionDatas = item.buttonPermissionDatas;
                                rolepermissiondto.dynamicPresence = item.dynamicPresence;
                                rolepermissiondto.roleid = Convert.ToInt32(item.roleid);
                                rolepermissiondto.rolename = item.rolename;
                                rolePermissionEditDTOs.Add(rolepermissiondto);
                            }
                        }
                                                                     
                        resut.Status = true;
                        resut.ResultOP = rolePermissionEditDTOs;
                        return resut;
                    }
                    else
                    {
                        ButtondefaultDTO empt = new ButtondefaultDTO();
                        string empval =    Newtonsoft.Json.JsonConvert.SerializeObject(empt);
                        var templateNames = (from MO in db.Modules
                                                 // join RP in db.RolePermission on MO.Id equals RP.ModuleId
                                                 // join Ro in db.Roles on RP.RoleId equals Ro.Id
                                                 // where RP.ActiveFlag == "Y"
                                             select new
                                             {
                                                 moduleId = MO.Id,
                                                 moduleName = MO.Title,
                                                 type = MO.Type,
                                                 icon = MO.Icon,
                                                 companyid = MO.CompanyId,
                                                 displayorder = MO.DisplayOrder,
                                                 activeFlag = "Y",
                                                 buttonPermissionDatas = JsonConvert.DeserializeObject<ButtonPermisionDTO> (empval),
                                                // buttonPermissionDatas = "",
                                                 dynamicPresence = MO.DynamicPresence,
                                                 url = "",
                                                 roleid = roleId,
                                                 rolename = "",
                                             }).ToList();
                        resut.Status = true;
                        resut.ResultOP = templateNames;
                        return resut;
                    }
                }
                
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Rolespermission";
                errorLog.Userid = 0;
                errorLog.Methodname = "LoadRolePermission";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        //load role only
        public static FinalResultDTO LoadRole(string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from MO in db.Roles
                                         select new
                                         {
                                             id = MO.Id,
                                             key = MO.Name,
                                         }).ToList();
                    resut.Status = true;
                    resut.ResultOP = templateNames;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Loadrole";
                errorLog.Userid = 0;
                errorLog.Methodname = "LoadRole";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static FinalResultDTO CreateorUpdatePermission(List<RolePermissionDTO> inputParams,Int32 orgid,Int64 userid,dynamic Ipdetails,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            string result = string.Empty;
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                //For testing 
                using (dbURSContext db = new dbURSContext())
                {
                   db.RolePermission.Where(p => p.RoleId == inputParams[0].roleid).ToList().ForEach(p => db.RolePermission.Remove(p));
                    db.SaveChanges();

                    foreach (var item in inputParams)
                    {
                        if (item.activeFlag == "false")
                        { item.activeFlag = "N"; }
                        else
                        { item.activeFlag = "Y"; }
                        if (item.roleid == 0)
                        {
                            var rolepermission = new RolePermission
                            {
                                RoleId = item.roleid,
                                ModuleId = item.moduleId,
                                SubModuleId = 0,
                                ActiveFlag = item.activeFlag,
                                ButtonPermissionData = Newtonsoft.Json.JsonConvert.SerializeObject(item.buttonPermissionDatas),
                                CreatedTime =dateTime_Indian,
                                CreatedBy = userid,
                            };
                            db.RolePermission.Add(rolepermission);
                            db.SaveChanges();
                        }
                        else
                        {
                          var rolepermission = new RolePermission
                            {
                                RoleId = item.roleid,
                                ModuleId = item.moduleId,
                                SubModuleId = 0,
                                ActiveFlag = item.activeFlag,
                                ButtonPermissionData = Newtonsoft.Json.JsonConvert.SerializeObject(item.buttonPermissionDatas),
                                CreatedTime = dateTime_Indian,
                                CreatedBy = userid,
                            };
                            db.RolePermission.Add(rolepermission);
                            db.SaveChanges();
                        }
                    }
                    var role = db.Roles.Where(x => x.Id == inputParams[0].roleid).FirstOrDefault();
                    var rolename = role.Name;
                    List<string> output = new List<string>();
                    string content = "";
                    content = "Permission added for " + rolename + "";
                    output.Add(content);
                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditjsonmdl = new AuditTrialJSONDTO();
                    auditdto.browser = Ipdetails[0];
                    auditdto.eventname = "Update";
                    auditdto.module = "PermissionSettings";
                    auditdto.userid = userid;
                    auditdto.orgid = orgid;
                    auditdto.ipaddress = Ipdetails[1];
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks= "Permission added for " + rolename + "";
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditjsonmdl);
                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Rolesand Permission";
                errorLog.Userid = userid;
                errorLog.Methodname = "CreateorUpdatePermission";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the data..";
                return resut;
            }
        }

        //Edit particular role

        public static FinalResultDTO EditRole(Int32 roleId)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<RolePermissionEditDTO> rolePermissionEditDTOs = new List<RolePermissionEditDTO>();
                    var templateNames = (from MO in db.Modules
                                             join RP in db.RolePermission on MO.Id equals RP.ModuleId
                                             join Ro in db.Roles on RP.RoleId equals Ro.Id
                                             where RP.RoleId==roleId
                                             select new
                                             {
                                                 id = MO.Id,
                                                 title = MO.Title,
                                                 type = MO.Type,
                                                 icon = MO.Icon,
                                                 companyid = MO.CompanyId,
                                                 displayorder = MO.DisplayOrder,
                                                 activeflag = RP.ActiveFlag,
                                                 buttonpermissiondata = RP.ButtonPermissionData,
                                                 dynamicpresence = MO.DynamicPresence,
                                                 roleid = RP.RoleId,
                                                 rolename = Ro.Name
                                             }).ToList();
                    foreach (var item in templateNames)
                    {
                        var rolepermissiondto = new RolePermissionEditDTO();
                        rolepermissiondto.id = item.id;
                        rolepermissiondto.title = item.title;
                        rolepermissiondto.type = item.type;
                        rolepermissiondto.icon = item.icon;
                        rolepermissiondto.companyid =Convert.ToInt64( item.companyid);
                        rolepermissiondto.displayorder = item.displayorder;
                        rolepermissiondto.activeflag = item.activeflag=="Y"? true : false;
                        rolepermissiondto.buttonpermissiondata = item.buttonpermissiondata;
                        rolepermissiondto.dynamicpresence = item.dynamicpresence;
                        rolepermissiondto.roleid = item.roleid;
                        rolepermissiondto.rolename = item.rolename;
                        rolePermissionEditDTOs.Add(rolepermissiondto);
                    }
                        resut.Status = true;
                        resut.ResultOP = rolePermissionEditDTOs;
                        return resut;
              }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Roles and Permission";
                errorLog.Userid = 0;
                errorLog.Methodname = "EditRole";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the Template Names";
                return resut;
            }
        }

        public static FinalResultDTO DeleteData(Int32 id)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var singleRec = db.RolePermission.FirstOrDefault(x => x.Id == id);// object your want to delete
                   // singleRec.DeleteFlag = "y";
                    db.SaveChanges();
                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Roles and Permission";
                errorLog.Userid = 0;
                errorLog.Methodname = "DeleteData";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong, While Deleting the Record";
                return resut;
            }
        }

        public static FinalResultDTO CreateorUpdateRole(RolesDTO inputParams,Int32 orgid,Int64 userid,dynamic Ipdetails,string time)
        {
            
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            string result = string.Empty;
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                //For testing 
                using (dbURSContext db = new dbURSContext())
                {
                    string description = "";
                    List<string> output = new List<string>();
                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    if (inputParams.Id == 0)
                    {
                       var rolepermission = new Roles
                        {
                            Name = inputParams.Name
                        };
                        db.Roles.Add(rolepermission);
                        db.SaveChanges();

                        var templateNames = (from MO in db.Roles
                                             select new
                                             {
                                                 id = MO.Id,
                                                 key = MO.Name,
                                             }).ToList();
                        resut.ResultOP = templateNames;
                        {
                            // description = "  New Role added: " + inputParams.Name + "";
                            string content = "";
                            content = "  New Role added: " + inputParams.Name + "";
                            output.Add(content);
                            auditdto.eventname = "Create";
                        }
                    }
                    else
                    {
                        // List<string> output = new List<string>();
                        auditdto.eventname = "Update";
                        var rolepermission = db.Roles.Where(x => x.Id == inputParams.Id).FirstOrDefault();
                        var oldname = rolepermission.Name;

                        if (String.Equals(oldname, inputParams.Name))
                        {  }
                        else
                        { description = "  Role name: Changed from  " + oldname + " to: " + inputParams.Name + ""; }

                        string content = "";
                        content = description;
                        output.Add(content);
                        rolepermission.Name = inputParams.Name;
                        db.SaveChanges();
                                               
                        var templateNames = (from MO in db.Roles
                                             select new
                                             {
                                                 id = MO.Id,
                                                 key = MO.Name,
                                             }).ToList();
                        resut.ResultOP = templateNames;
                    }
                   
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = Ipdetails[0];
                    auditdto.ipaddress = Ipdetails[1];
                    auditdto.module = "PermissionSettings";
                    auditdto.userid = userid;
                    auditdto.orgid = orgid;
                 
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                    resut.Status = true;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Roles and Permission";
                errorLog.Userid = userid;
                errorLog.Methodname = "CreateorUpdateRole";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong, unable to insert the data..";
                return resut;
            }
        }

        public static FinalResultDTO Roledelete(Int32 id,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var deleteroleDetails =
                         db.UserRoles.Where(x => x.RoleId == id).Select(x => x.RoleId).FirstOrDefault();
                    if (deleteroleDetails==null)
                    {
                        db.Roles.Where(p => p.Id == id).ToList().ForEach(p => db.Roles.Remove(p));
                        db.SaveChanges();
                        var templateNames = (from MO in db.Roles
                                             select new
                                             {
                                                 id = MO.Id,
                                                 key = MO.Name,
                                             }).ToList();
                        resut.ResultOP = templateNames;
                        resut.Status = true;
                        return resut;
                    }
                    else
                    {
                        resut.Description = "This role is already mapped to a User and cannot be deleted'";
                        resut.Status = false;
                        return resut;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Roles and Permission";
                errorLog.Userid = 0;
                errorLog.Methodname = "Roledelete";
                errorLog.ErrorMessage = ex.Message;
                resut.Status = false;
                resut.Description = "Something went wrong, While Deleting the Record";
                return resut;
            }
        }


    }
}
