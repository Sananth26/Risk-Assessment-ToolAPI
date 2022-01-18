using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using URSAPI.ModelDTO;
using URSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;

namespace URSAPI.DataAccessLayer
{
    public class AuditTrialDAL
    {
        public static string GettimezoneString()
        {
            return Startup.timezone;
        }
        public static  void CreateAuditTrial(AuditTrialDTO inputParams,AuditTrialJSONDTO jsondto)
        {
           // FinalResultDTO resut = new FinalResultDTO();
            string result = string.Empty;
            string timezone = GettimezoneString();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
              using (dbURSContext db = new dbURSContext())
                {
                   if (inputParams.existsentry == 0)
                    {
                        var audittrial = new AuditTrail
                        {
                            Browser = inputParams.browser,
                            CreatedTime = dateTime_Indian,
                            Event = inputParams.eventname,
                            IpAddress = inputParams.ipaddress,
                            Orgid = inputParams.orgid,
                            Userid = inputParams.userid,
                            Module = inputParams.module,
                            Description = inputParams.description,
                            RequestId = inputParams.RequestId,
                            Systemremarks = inputParams.Systemremarks,
                           // Attachments = inputParams.Attachments,
                        };
                        db.AuditTrail.Add(audittrial);
                        db.SaveChanges();
                    }
                    else
                    {
                     var auditdetails = new AuditTrail();
                     auditdetails = db.AuditTrail.Where(x => Convert.ToInt32( x.RequestId) == Convert.ToInt32(inputParams.RequestId)).FirstOrDefault();
                     List<AuditTrialJSONDTO> auditdto = JsonConvert.DeserializeObject<List<AuditTrialJSONDTO>>(auditdetails.Description);
                     auditdto.Add(jsondto);
                     auditdetails.Description = JsonConvert.SerializeObject(auditdto);
                     db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            //    resut.Status = false;
              //  resut.Description = "Something went wrong, unable to insert the data..";
                
            }
        }

        public static FinalResultDTO LoadAuditTrialData(DateTime fromdate,DateTime todate,string time)
        {
            FinalResultDTO resut = new FinalResultDTO();
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    List<MainAuditTrialBindDTO> Mainlist = new List<MainAuditTrialBindDTO>();
                    for (DateTime date = fromdate; date < todate; date = date.AddDays(1))
                    {
                        var addMainAuditTrialBindDTO = new MainAuditTrialBindDTO();
                      
                        var templateNames = (from AT in db.AuditTrail
                                             join us in db.User on AT.Userid equals us.Id
                                             join org in db.Organisation on AT.Orgid equals org.Id
                                             where AT.CreatedTime.Date == date && (AT.RequestId=="0"|| AT.RequestId ==null) && (AT.Browser !=null )&& (AT.IpAddress!=null)
                                             //  where AT.CreatedTime.Date >= fromdate.Date && AT.CreatedTime.Date <= todate.Date
                                             // where (AT.CreatedTime.Year >= fromdate.Year  && AT.CreatedTime.Month >= fromdate.Month && AT.CreatedTime.Day >= fromdate.Day && ////AT.CreatedTime.Year <= todate.Year && AT.CreatedTime.Month <= todate.Month  && AT.CreatedTime.Day <= todate.Day )

                                             select new AuditTrialBindDTO
                                             {
                                                 id = AT.Id,
                                                 browser = AT.Browser,
                                                 event1 = AT.Event,
                                                 ipAddress = AT.IpAddress,
                                                 //orgid = AT.Orgid,
                                                 //userid = AT.Userid,
                                                 loginUserName = us.FirstName,
                                                 userRemarks =  JsonConvert.DeserializeObject<List<string>>(AT.Description) ,
                                                 module = AT.Module,
                                                 projectName = org.OrgName,
                                                 createdTime = AT.CreatedTime,
                                                 systemRemarks = AT.Systemremarks
                                             }).ToList();
                        if (templateNames.Count>0)
                        {
                            addMainAuditTrialBindDTO.value = templateNames;
                            addMainAuditTrialBindDTO.date = (date).ToString("dd-MM-yyyy");
                            Mainlist.Add(addMainAuditTrialBindDTO);
                        }
                       
                    }
                                                              
                    resut.Status = true;
                    resut.ResultOP = Mainlist;
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

        public static FinalResultDTO LoadAuditTrialData(string requestid)
        {
            FinalResultDTO resut = new FinalResultDTO();
            ButtonPermisionDTO buttonPermissionDatas1 = new ButtonPermisionDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    var templateNames = (from AT in db.AuditTrail
                                         where Convert.ToInt32( AT.RequestId) ==Convert.ToInt32(requestid)
                                         select new
                                         {
                                           //  userRemarks = AT.Description.Cast<char>().Cast<string>().ToArray(),
                                             description =  AT.Description,
                                         }).ToList();
                    List<AuditTrialJSONDTO> auditdto = JsonConvert.DeserializeObject<List<AuditTrialJSONDTO>>(templateNames[0].description);
                    auditdto.Reverse();
                    resut.Status = true;
                    resut.ResultOP = auditdto;
                    return resut;
                }
            }
            catch (Exception ex)
            {
                resut.Status = false;
                resut.Description = "Something went wrong while fetching the New Request Data";
                return resut;
            }
        }

        public static void CreateErrorLog(ErrorLog inputParams)
        {
            // FinalResultDTO resut = new FinalResultDTO();
            string result = string.Empty;
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                 
                        var errorlog = new ErrorLog
                        {
                            Page = inputParams.Page,
                            CreatedTime = dateTime_Indian,
                            Methodname = inputParams.Methodname,
                            ErrorMessage = inputParams.ErrorMessage,
                            Userid = inputParams.Userid,
                        };
                        db.ErrorLog.Add(errorlog);
                        db.SaveChanges();
                 
                }
            }
            catch (Exception ex)
            {
                //    resut.Status = false;
                //  resut.Description = "Something went wrong, unable to insert the data..";

            }
        }

        public static string GetIP1()
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://api.ipify.org/?format=json");
                var JSONObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                wc.Dispose();
                return JSONObj["ip"];
            }
            //return myExternalIP.ToString();
        }


    }
}
