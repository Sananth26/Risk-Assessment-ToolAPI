using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using URSAPI.Controllers;
using URSAPI.ModelDTO;
using URSAPI.Models;
 

namespace URSAPI.DataAccessLayer
{
    public class LoginAuthDAL
    {
        public static FinalResultDTO CheckUserCredential(UserModel input)
        {
            FinalResultDTO result = new FinalResultDTO();
            
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    UserModel userDetails = new UserModel();
                    var op = db.User.Where(x => x.Email == input.UserEmail && x.Password == input.Password).FirstOrDefault();
                    var templateNames = (from user in db.User
                                         join ur in db.Department on user.Department equals ur.Id

                                         where user.Email == input.UserEmail && user.Password == input.Password
                                         select new UserModel
                                         {
                                             UserId = Convert.ToInt32(op.Id),
                                             UserName = op.FirstName,
                                             UserEmail = op.Email,
                                             MobileNo = op.MobileNumber,
                                             CorporateId = op.CorporateId,
                                             Department = ur.DepartmentName
                                           }).FirstOrDefault();
                    //string sourceJsonString = "{'name':'John Doe','age':'25','hitcount':34}";
                    //string targetJsonString = "{'name':'John Doe','age':'26','hitcount':30}";

                    //JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
                    //JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);

                    //if (!JToken.DeepEquals(sourceJObject, targetJObject))
                    //{
                    //    foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                    //    {
                    //        JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                    //        if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                    //        {
                    //            string ggg = string.Format("{0} property value is changed", sourceProperty.Key);
                    //            //Console.WriteLine(string.Format("{0} property value is changed", sourceProperty.Key));
                    //        }
                    //        else
                    //        {
                    //            string ggg = string.Format("{0} property value is changed", sourceProperty.Key);
                    //            //Console.WriteLine(string.Format("{0} property value didn't change", sourceProperty.Key));
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //   // Console.WriteLine("Objects are same");
                    //}
                    if (templateNames != null)
                    {
                        //userDetails.UserId = Convert.ToInt32(op.Id);
                        //userDetails.UserName = op.FirstName;
                        //userDetails.UserEmail = op.Email;
                        //userDetails.MobileNo = op.MobileNumber;
                        //userDetails.CorporateId = op.CorporateId;
                        result.ResultOP = templateNames;
                        result.Status = true;
                    }
                    else
                    {
                        result.Status = false;
                        result.Description = "Incorrect Email or Password...";
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
              
                result.Description = "Something went wrong while checking the credentials..";
                return result;
            }
        }

        public static FinalResultDTO LoadMenuBasedUser(UserModel input,string time)
        {
            FinalResultDTO result = new FinalResultDTO();
            ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                string[] a = new string[] { };
                List<string> empt = new List<string>();
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from user in db.User
                                         join ur in db.UserRoles on user.Id equals ur.UserId
                                         join ro in db.Roles on ur.RoleId equals ro.Id
                                         join roper in db.RolePermission on ro.Id equals roper.RoleId
                                         join modu in db.Modules on roper.ModuleId equals modu.Id
                                         where Convert.ToInt32(user.Id) == Convert.ToInt32(input.UserId) && roper.ActiveFlag=="Y"
                                         select new SidemenuDTO
                                         {
                                             path = modu.Url,
                                             title = modu.Title,
                                             icon = modu.Icon,
                                             @class = "",
                                             badgeClass = "",
                                             isExternalLink = false,
                                             submenu = empt,
                                             displayorder = modu.DisplayOrder,
                                         }).ToList();
                    result.Status = true;
                    result.ResultOP = templateNames.OrderBy(x=>x.displayorder);
                       
                    }


                    return result;
                
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Login";
                errorLog.Userid = input.UserId;
                errorLog.Methodname = "LoadMenuBasedUser";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Status = false;
                result.Status = false;
                result.Description = "Something went wrong while checking the credentials..";
                return result;
            }
        }
            
        public static FinalResultDTO LoadButtonpermission(RolePermissionDTO1 input,string time)
        {
            FinalResultDTO result = new FinalResultDTO();ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    // ButtonAccess
                    var buttonacc = (from user in db.User
                                         join ur in db.UserRoles on user.Id equals ur.UserId
                                         join ro in db.Roles on ur.RoleId equals ro.Id
                                         join roper in db.RolePermission on ro.Id equals roper.RoleId
                                         join modu in db.Modules on roper.ModuleId equals modu.Id
                                         where Convert.ToInt32(user.Id) == Convert.ToInt32(input.userId) && modu.Url == input.url
                                         select new 
                                         {
                                             menuId = 0,
                                             url = modu.Url,
                                             roleId = Convert.ToInt32(ur.RoleId),
                                             buttonaccess = roper.ButtonPermissionData,
                                             acitiveflag = roper.ActiveFlag
                                         }).ToList();
                   
                    if (buttonacc != null)
                    {
                        ButtonAccess buttondto = JsonConvert.DeserializeObject<ButtonAccess>(buttonacc[0].buttonaccess);

                        Int32 add = Convert.ToInt32(buttondto.add);
                        Int32 view =  Convert.ToInt32(buttondto.view);
                        Int32 edit = Convert.ToInt32 (buttondto.edit);
                        Int32 dele = Convert.ToInt32 (buttondto.delete);
                        Int32 prin = Convert.ToInt32( buttondto.print);
                        Int32 expo = Convert.ToInt32( buttondto.export);
                        string acitiveflag = Convert.ToString(buttonacc[0].acitiveflag);
                        var templateNames = (from user in db.User
                                             join ur in db.UserRoles on user.Id equals ur.UserId
                                             join ro in db.Roles on ur.RoleId equals ro.Id
                                             join roper in db.RolePermission on ro.Id equals roper.RoleId
                                             join modu in db.Modules on roper.ModuleId equals modu.Id
                                             where Convert.ToInt32(user.Id) == Convert.ToInt32(input.userId) && modu.Url == input.url
                                             select new UserAccess
                                             {
                                                 menuId = 0,
                                                 url = modu.Url,
                                                 roleId = Convert.ToInt32(ur.RoleId),
                                                 add = add,
                                                 view = view,
                                                 delete = dele,
                                                 edit = edit,
                                                 export = expo,
                                                 print = prin,
                                                 menuAccess = acitiveflag == "Y" ? 1 : 0,
                                             }).ToList();
                        result.Status = true;
                        result.ResultOP = templateNames;
                    }
                }


                return result;

            }
            catch (Exception ex)
            {
                errorLog.CreatedTime =dateTime_Indian;
                errorLog.Page = "Login";
                errorLog.Userid = input.userId;
                errorLog.Methodname = "LoadButtonpermission";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Status = false;
                result.Description = "Something went wrong while checking the credentials..";
                return result;
            }
        }
        public static FinalResultDTO ChangePassword(Password input,string time)
        {
            FinalResultDTO result = new FinalResultDTO(); ErrorLog errorLog = new ErrorLog();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    //check password
                    var output = db.User.Where(x => x.Id == input.userId && x.Password == input.oldPassword).FirstOrDefault();
                    if (output != null && input.newPassword != null && input.newPassword != "")
                    {
                        output.Password = input.newPassword;
                        output.UpdatedTime = dateTime_Indian;
                        db.SaveChanges();
                        result.Status = true;
                        result.Description = "Successfully Password Updated";
                    }
                    else
                    {
                        result.Status = false;
                        result.Description = "  Old Password wrong...";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                errorLog.CreatedTime = dateTime_Indian;
                errorLog.Page = "Login";
                errorLog.Userid = input.userId;
                errorLog.Methodname = "change Password";
                errorLog.ErrorMessage = ex.Message;
                AuditTrialDAL.CreateErrorLog(errorLog);
                result.Status = false;
                result.Description = "Something went wrong while checking the credentials..";
                return result;
            }
        }

        

    }
}


