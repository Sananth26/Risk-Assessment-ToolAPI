using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.DataAccessLayer
{
    public class UserMasterDAL
    {
        public static dynamic LoadDepartments()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var departments = (from dept in db.Department
                                       where dept.IsActive == "Y" && dept.DeleteFlag == "N"
                                       select new DropDownsDTO
                                       {
                                           Id = Convert.ToInt32(dept.Id),
                                           key = dept.DepartmentName
                                       }).ToList();
                    result.Status = true;
                    result.ResultOP = departments;
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


        public static dynamic LoadRoles()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var departments = (from dept in db.Roles
                                       select new DropDownsDTO
                                       {
                                           Id = Convert.ToInt32(dept.Id),
                                           key = dept.Name
                                       }).ToList();
                    result.Status = true;
                    result.ResultOP = departments;
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


        public static dynamic LoadUsers(Int32 deptID)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var users = (from user in db.User
                                 where user.DeleteFlag == "N"
                                select new DropDownsDTO
                                 {
                                     Id = Convert.ToInt32(user.Id),
                                     key = String.Concat(user.CorporateId, " - ", user.FirstName)
                                 }).ToList();

                    result.Status = true;
                    result.ResultOP = users;
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

        public static dynamic LoadUserDatas()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userList = (from users in db.User
                                    join dept in db.Department on users.Department equals dept.Id
                                    join userSupName in db.User on users.ImmediateSupervisor equals userSupName.Id
                                    join userRole in db.UserRoles on users.Id equals userRole.UserId
                                    join role in db.Roles on userRole.RoleId equals role.Id
                                    where users.IsActive == "Y"
                                    orderby users.Id descending
                                    select new UserDTO
                                    {
                                        Id = Convert.ToInt32(users.Id),
                                        corporateId = users.CorporateId,
                                        FirstName = users.FirstName,
                                        LastName = users.LastName,
                                        Email = users.Email,
                                        MobileNo = users.MobileNumber,
                                        DepartmentName = dept.DepartmentName,
                                        DepartmentId = Convert.ToInt32(dept.Id),
                                        CreatedDate = users.CreatedTime,
                                        ModifiedDate = users.UpdatedTime,
                                        ImmediateSupervisorId = Convert.ToInt64(users.ImmediateSupervisor),
                                        ImmediateSupervisorName = userSupName.FirstName,
                                        roleId = role.Id,
                                        roleName = role.Name,
                                        ItProcessAccess = users.ItProcessAccess.Trim(),
                                        ManagerAccess = users.ManagerAccess.Trim()
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

        public static dynamic LoadUsersForEdit(Int32 userId)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userList = (from users in db.User
                                    join dept in db.Department on users.Department equals dept.Id
                                    join userSupName in db.User on users.ImmediateSupervisor equals userSupName.Id
                                    join userRole in db.UserRoles on users.Id equals userRole.UserId
                                    where users.Id == userId
                                    select new UserDTO
                                    {
                                        Id = Convert.ToInt32(users.Id),
                                        corporateId=Convert.ToString(users.CorporateId),
                                        FirstName = users.FirstName,
                                        Email = users.Email,
                                        MobileNo = users.MobileNumber,
                                        DepartmentName = dept.DepartmentName,
                                        DepartmentId = Convert.ToInt32(dept.Id),
                                        ImmediateSupervisorId = Convert.ToInt64(users.ImmediateSupervisor),
                                        ImmediateSupervisorName = userSupName.FirstName,
                                        location = users.Location,
                                        doj = users.Doj.ToString("yyyy/MM/dd"),
                                        roleId =  userRole.RoleId ,
                                        ItProcessAccess = users.ItProcessAccess.Trim(),
                                        password=users.Password,
                                        IsActive=users.IsActive.Trim(),
                                        ManagerAccess = users.ManagerAccess.Trim(),
                                        //ItProcessAccess = users.ItProcessAccess == "Y" ? true : false,

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

        public static dynamic CreateOrUpdateUser(UserDTO input,Int64 userid,dynamic Ipdetails,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int64 newid = 0;
                    var useroldData = (from users in db.User
                                       join dept in db.Department on users.Department equals dept.Id
                                       join userSupName in db.User on users.ImmediateSupervisor equals userSupName.Id
                                       join userRole in db.UserRoles on users.Id equals userRole.UserId
                                       join rp in db.Roles on userRole.RoleId equals rp.Id
                                       where users.Id == input.Id
                                       select new UserDTO
                                       {
                                           Id = Convert.ToInt32(users.Id),
                                           FirstName = users.FirstName,
                                           LastName = users.LastName,
                                           Email = users.Email,
                                           MobileNo = users.MobileNumber,
                                           DepartmentName = dept.DepartmentName,
                                           DepartmentId = Convert.ToInt32(dept.Id),
                                           ImmediateSupervisorId = Convert.ToInt64(users.ImmediateSupervisor),
                                           ImmediateSupervisorName = userSupName.FirstName,
                                           location = users.Location,
                                           doj = users.Doj.ToString("yyyy/MM/dd"),
                                           roleId = userRole.RoleId,
                                           ItProcessAccess = users.ItProcessAccess.Trim(),
                                           ManagerAccess = users.ManagerAccess.Trim(),
                                           roleName = rp.Name,
                                           password=users.Password,
                                           //IsActive = "Y",
                                           //corporateId = "rtrt",
                                           //ItProcessAccess = users.ItProcessAccess == "Y" ? true : false,

                                       }).FirstOrDefault();
                   

                    if (input.Id == 0)
                    {
                        var exist = db.User.Where(x => x.Email == input.Email).FirstOrDefault();
                        if (exist == null)
                        {
                            var newUser = new User
                            {
                                FirstName = input.FirstName,
                                LastName = input.LastName,
                                Email = input.Email,
                                MobileNumber = input.MobileNo,
                                Department = input.DepartmentId,
                                ImmediateSupervisor = input.ImmediateSupervisorId,
                                CreatedTime = dateTime_Indian,
                                UpdatedTime = dateTime_Indian,
                                DeleteFlag = "N",
                                Location = input.location,
                                Doj = Convert.ToDateTime(input.doj),
                                ItProcessAccess = input.ItProcessAccess.Trim(),
                                ManagerAccess = input.ManagerAccess.Trim(),
                                IsActive = input.IsActive.Trim(),
                                CorporateId = input.corporateId,
                                Password = input.password,
                                //ItProcessAccess = input.ItProcessAccess == true ? "Y" : "N",
                            };
                            db.User.Add(newUser);
                            db.SaveChanges();
                            newid = newUser.Id;
                            var newRole = new UserRoles
                            {
                                UserId = newUser.Id,
                                RoleId = input.roleId > 0 ? input.roleId : 0,

                            };
                            db.UserRoles.Add(newRole);
                            db.SaveChanges();
                        }
                        else
                        {
                            result.Status = false;
                            result.Description = "Already EmailId Exist.";
                            return result;
                        }
                    }
                    else
                    {

                        var userData = db.User.Where(x => x.Id == input.Id).FirstOrDefault();
                        userData.FirstName = input.FirstName;
                        userData.LastName = input.LastName;
                        userData.Email = input.Email;
                        userData.MobileNumber = input.MobileNo;
                        userData.Department = input.DepartmentId;
                        userData.ImmediateSupervisor = input.ImmediateSupervisorId;
                        userData.UpdatedTime = dateTime_Indian;
                        userData.Location = input.location;
                        userData.Doj = Convert.ToDateTime(input.doj);
                        userData.ItProcessAccess = input.ItProcessAccess.Trim();
                        userData.ManagerAccess = input.ManagerAccess.Trim();
                        userData.IsActive = input.IsActive.Trim();
                        userData.CorporateId = input.corporateId.Trim();
                        userData.Password = input.password;
                        // userData.ItProcessAccess = input.ItProcessAccess == true ? "Y" : "N";

                        db.SaveChanges();

                        var newrole = db.UserRoles.Where(x => x.UserId == input.Id).FirstOrDefault();
                        newrole.RoleId = input.roleId;
                        db.SaveChanges();
                    }

                    var department = db.Department.Where(x => x.Id == input.DepartmentId).FirstOrDefault();
                    var rolepermission = db.Roles.Where(x => x.Id == input.roleId).FirstOrDefault();
                    var immediatesuper = db.User.Where(x => x.Id == input.ImmediateSupervisorId).FirstOrDefault();
                   
                    string eventname = "";
                    string description = "";
                    List<string>  output = new List<string>();
                   
                    if (input.Id == 0)
                    {
                        eventname = "  Save";
                        string content = "";
                        content = " First Name: " + input.FirstName;
                        output.Add(content);

                        content = "";
                        content = " Email: " + input.Email;
                        output.Add(content);

                        content = "";
                        content = " MobileNumber: " + input.MobileNo;
                        output.Add(content);

                        content = "";
                        content = " Department: " + department.DepartmentName;
                        output.Add(content);

                        content = "";
                        content = " Supervisior: " + immediatesuper.FirstName;
                        output.Add(content);

                        content = "";
                        content = " Location: " + input.location;
                        output.Add(content);

                        content = "";
                        content = "  RoleName: " + rolepermission.Name;
                        output.Add(content);

                        content = "";
                        content =  input.ItProcessAccess=="Y"? "Itprocess: Yes" : "Itprocess: No";
                        output.Add(content);
                        content = "";
                        content = input.ManagerAccess == "Y" ? "ManagerAccess: Yes" : "ManagerAccess: No";
                        output.Add(content);

                        content = "";
                        content = input.IsActive == "Y" ? "IsActive: Yes" : "IsActive: No";
                        output.Add(content);
                    }
                    else
                    {
                        eventname = " Update";
                        if (String.Equals(useroldData.FirstName, input.FirstName))
                        { }
                        else
                        {
                           var content = " First Name: Changed from  "+ useroldData.FirstName + "  to:  " + input.FirstName + "";
                           output.Add(content);    
                        }
                                                
                        if (String.Equals(useroldData.Email, input.Email))
                            { }
                            else
                        {
                           var content = " Email: Changed from " + useroldData.Email + "  to:  " + input.Email + "";
                           output.Add(content);
                        }
                        if (String.Equals(useroldData.MobileNo, input.MobileNo))
                            { }
                            else
                            {
                                 
                               var  content = " Mobilenumber: Changed from " + useroldData.MobileNo + "  to:  " + input.MobileNo + "";
                                output.Add(content);
                               
                            }

                            if (String.Equals(useroldData.DepartmentName, department.DepartmentName))
                            { }
                            else
                            {
                                var content = " Department: Changed from  "+ useroldData.DepartmentName + "  to:  " + department.DepartmentName + "";
                                output.Add(content);
                                
                            }

                            if (String.Equals(useroldData.ImmediateSupervisorName, immediatesuper.FirstName))
                            { }
                            else
                            {
                                
                                var content = " Supervisor: Changed from " + useroldData.ImmediateSupervisorName + "   to:  " + immediatesuper.FirstName + "";
                                output.Add(content);
                                
                            }

                            if (String.Equals(useroldData.location, input.location))
                            { }
                            else
                            {
                                var content = "Location: Changed from  " + useroldData.location + "  to:  " + input.location + "";
                                output.Add(content);
                                
                            }
                          

                            if (String.Equals(useroldData.ItProcessAccess, input.ItProcessAccess))
                            { }
                            else
                            {
                            if (input.ItProcessAccess == "Y")
                            {
                                var content = " IT Process: Changed from   NO  to: YES";
                                output.Add(content);
                            }
                            else
                            {
                                var content = " IT Process: Changed from  YES  to: NO";
                                output.Add(content);
                            }
                                
                            }

                        if (String.Equals(useroldData.ManagerAccess, input.ManagerAccess))
                        { }
                        else
                        {
                            if (input.ManagerAccess == "Y")
                            {
                                var content = " ManagerAcess: Changed from   NO  to: YES";
                                output.Add(content);
                            }
                            else
                            {
                                var content = " ManagerAccess: Changed from  YES  to: NO";
                                output.Add(content);
                            }

                        }

                        if (String.Equals(useroldData.roleName, rolepermission.Name))
                            { }
                            else
                            {
                                var content = "Role Name: Changed from  " + useroldData.roleName + "  to:  " + rolepermission.Name + "";
                                output.Add(content);
                                
                            }
                        }

                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "UserManagement";
                        auditdto.userid = userid;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                                       
                        FinalResultDTO resultLoadUserDatas = LoadUserDatas();
                        if (resultLoadUserDatas.Status)
                        {
                            result.Status = true;
                            result.ResultOP = resultLoadUserDatas.ResultOP;
                        }
                        else
                        {
                            result.Status = false;
                            result.Description = "The User details are inserted Succesfully, but failed to load the data. Please refresh the page..";
                        }
                    }
                    return result;
                
                
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while inserting the user details.";
                return result;
            }
        }

        public static dynamic DeleteUser(Int32 userId, Int64 userid,dynamic Ipdetails,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    
                        var deleteroleDetails =
                         db.WorkflowUsers.Where(x => x.UserId == userid).Select(x => x.UserId).FirstOrDefault();
                    if (Convert.ToInt32(deleteroleDetails)==0)
                    {
                        var userData = db.User.Where(x => x.Id == userId).FirstOrDefault();
                        userData.DeleteFlag = "Y";
                        db.SaveChanges();

                        var department = db.Department.Where(x => x.Id == userData.Department).FirstOrDefault();
                        //var rolepermission = db.Roles.Where(x => x.Id == userData.RolePermission).FirstOrDefault();
                        var immediatesuper = db.User.Where(x => x.Id == userData.ImmediateSupervisor).FirstOrDefault();

                        string eventname = "";
                        string description = "";
                        eventname = "Delete";

                        List<string> output = new List<string>();
                        string content = "";
                        content = " First Name: " + userData.FirstName;
                        output.Add(content);

                        /* description = " First Name :" + userData.FirstName + " , Last Name: " + userData.LastName + ", Email: " + userData.Email + ", MobileNumber : " + userData.MobileNumber + " ,Department:  " + department.DepartmentName + ", Immediate Supervisior : " + immediatesuper.FirstName + ", Location : " + userData.Location + ", Doj:  " + userData.Doj + ", Itprocess:  " + userData.ItProcessAccess + " ,DeletedId :"+userId+"";*/


                        AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "EmployeeManagement";
                        auditdto.userid = userid;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);

                        FinalResultDTO resultLoadUserDatas = LoadUserDatas();
                        if (resultLoadUserDatas.Status)
                        {
                            result.Status = true;
                            result.ResultOP = resultLoadUserDatas.ResultOP;
                        }
                        else
                        {
                            result.Status = false;
                            result.Description = "The User has been deleted Succesfully, but failed to load the data. Please refresh the page..";
                        }
                       
                    }
                    else
                    {
                        result.Status = false;
                        result.Description = "This user already used in Workflow";

                    }
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


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string BulkInsert(UserDTO input,dynamic Ipdetails)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {


                    if (input.Id == 0)
                    {
                        var newUser = new User
                        {
                            FirstName = input.FirstName,
                            LastName = input.LastName,
                            Email = input.Email,
                            MobileNumber = input.MobileNo,
                            Department = input.DepartmentId,
                            ImmediateSupervisor = input.ImmediateSupervisorId,
                            CreatedTime = dateTime_Indian,
                            UpdatedTime = dateTime_Indian,
                            DeleteFlag = "N",
                            Location = input.location,
                            Doj = Convert.ToDateTime(input.doj),
                            ItProcessAccess = input.ItProcessAccess,
                            //ItProcessAccess = input.ItProcessAccess == true ? "Y" : "N",

                        };
                        db.User.Add(newUser);
                        db.SaveChanges();

                        var newRole = new UserRoles
                        {
                            UserId = newUser.Id,
                            RoleId = input.roleId

                        };
                        db.UserRoles.Add(newRole);
                        db.SaveChanges();

                        var department = db.Department.Where(x => x.Id == input.DepartmentId).FirstOrDefault();
                        var rolepermission = db.Roles.Where(x => x.Id == input.roleId).FirstOrDefault();
                        var immediatesuper = db.User.Where(x => x.Id == input.ImmediateSupervisorId).FirstOrDefault();



                        string eventname = "";
                        string description = "";
                        string content = "";
                        List<string> output = new List<string>();
                            eventname = "Create";
                            description = " First Name :" + input.FirstName + " , Last Name: " + input.LastName + ", Email: " + input.Email + ", MobileNumber : " + input.MobileNo + " ,Department:  " + department.DepartmentName + ", Immediate Supervisior : " + immediatesuper.FirstName + ", Location : " + input.location + ", Doj:  " + input.doj + ", Itprocess:  " + input.ItProcessAccess + ", RoleName: " + rolepermission.Name + " Using bulk insert ";
                        output.Add(description); 

                       AuditTrialDTO auditdto = new AuditTrialDTO();
                        AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                        auditdto.browser = Ipdetails[0];
                        auditdto.eventname = eventname;
                        auditdto.module = "UserManagement";
                        auditdto.userid = input.Id;
                        auditdto.orgid = 1;
                        auditdto.ipaddress = Ipdetails[1];
                        auditdto.description = JsonConvert.SerializeObject(output);
                        auditdto.Systemremarks = description;
                        AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                    }

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while inserting the user details.";
                return null;
            }
        }

        public static string InsertCategory(List<categorySaveDTO> input,Int64 userid,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<string> mainCategoryList = new List<string>();
                    List<string> categoryList = new List<string>();
                    Int32 mainCatid = 0;
                    Int32 categoryId = 0;
                    if (input.Count > 0)
                    {
                        foreach (var item in input)
                        {
                             var maincatego = db.MainCategory.Where(x => x.MainCategoryName == item.categoryName&&x.IsActive.Trim()=="Y").FirstOrDefault();
                               if (maincatego != null)
                                {
                                    mainCatid = Convert.ToInt32(maincatego.Id);
                                }
                                else {
                                var maincate = new MainCategory
                                {
                                    MainCategoryName = item.categoryName,
                                    IsActive = "Y",
                                    CreatedBy = userid,
                                    CreatedTime = dateTime_Indian,
                                    LastUpdatedBy =userid,
                                };
                                db.MainCategory.Add(maincate);
                                db.SaveChanges();
                                mainCatid = Convert.ToInt32(maincate.Id);
                                }
                              var catego = db.Category.Where(x => x.Question == item.question && x.MainCategoryId== mainCatid&& x.IsActive.Trim() == "Y").FirstOrDefault();

                            if (catego != null)
                            {
                                categoryId = Convert.ToInt32(catego.Id);
                            }
                            else
                            {
                                categoryId = 0;
                                categoryList.Add(item.question);
                                var cate = new Category
                                {
                                    Question = item.question,
                                    MainCategoryId = mainCatid,
                                    CreatedBy = userid,
                                    IsActive = "Y",
                                    CreatedTime = dateTime_Indian,
                                    LastUpdatedBy = userid,
                                     
                                };
                                db.Category.Add(cate);
                                db.SaveChanges();
                                categoryId = Convert.ToInt32(cate.Id);
                            }
                           var subcatego = db.SubCategory.Where(x => x.QuestionNumber.Trim() == item.questionnumber.Trim()).ToList();
                            if (subcatego != null && subcatego.Count > 0)
                            {
                                string isactive = string.Empty;
                                foreach (var subitem in subcatego)
                                {
                                    var qn = subitem.IsActive.Trim();
                                    if (qn == "Y")
                                    {
                                        var subchan = db.SubCategory.Where(x => x.Id == subitem.Id).FirstOrDefault();
                                        subchan.IsActive = "N";
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else { }
                            var subcate = new SubCategory
                            {
                                CategoryId = categoryId,
                                MainCategoryId = mainCatid,
                                Risk = item.risk,
                                QuestionandExplanation = item.explanation,
                                CreatedBy = userid,
                                QuestionNumber=item.questionnumber,
                                CreatedTime =dateTime_Indian,
                                LastUpdatedBy = userid,
                                IsActive="Y"
                            };
                            db.SubCategory.Add(subcate);
                            db.SaveChanges();

                        }
                    }
                  return "Success";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Description = "Something went wrong while inserting the user details.";
                return null;
            }
        }

        public static dynamic LoadAllUserDatas()
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var userList = (from users in db.User

                                    where users.IsActive == "Y"
                                    orderby users.Id descending

                                    select new UserDTO
                                    {
                                        Id = Convert.ToInt32(users.Id),
                                        FirstName = users.FirstName,
                                        LastName = users.LastName,
                                        Email = users.Email,
                                        MobileNo = users.MobileNumber,

                                        CreatedDate = users.CreatedTime,
                                        ModifiedDate = users.UpdatedTime,
                                        ImmediateSupervisorId = Convert.ToInt64(users.ImmediateSupervisor),

                                        ItProcessAccess = users.ItProcessAccess,
                                        ManagerAccess = users.ManagerAccess,
                                        //ItProcessAccess = users.ItProcessAccess == "Y" ? true : false


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

    }
}
