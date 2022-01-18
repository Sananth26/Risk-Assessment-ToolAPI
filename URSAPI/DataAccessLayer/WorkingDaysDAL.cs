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
    public class WorkingDaysDAL
    {
        public static dynamic WeekDays()
        {
            FinalResultDTO resut = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.templateNames = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();
            List<WeekdaysDTO> weekdto = new List<WeekdaysDTO>();

            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    weekdto = (from week in db.Weekdays
                               select new WeekdaysDTO
                               {
                                   weekdayId = week.Id,
                                   weekday = week.Days,
                                   selectedFlag = Convert.ToBoolean(week.Isactive),
                               }).ToList();

                    resultDTO.templateNames = weekdto;

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

        public static dynamic WeekDaysSave(List<WeekdaysDTO> weekdays)
        {
            FinalResultDTO resut = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.templateNames = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();

            try
            {

                using (dbURSContext db = new dbURSContext())
                {
                    string content = "";
                    List<string> output = new List<string>();
                    foreach (var item in weekdays)
                    {
                        var weekdetails = new Weekdays();

                        weekdetails = db.Weekdays.Where(x => x.Id == item.weekdayId).FirstOrDefault();
                        weekdetails.Isactive = Convert.ToInt32(item.selectedFlag);
                        // RequestDetails.ReviewRemark = item.ReviewRemark;
                        db.SaveChanges();
                        string isactive = weekdetails.Isactive==1 ? "true" : "false";

                        content = content + ", Day = " + weekdetails.Days + ", Active = " + isactive;
                        
                    }
                    output.Add(content);
                    var templateNames = (from week in db.Weekdays
                                         select new
                                         {
                                             weekdayId = week.Id,
                                             weekday = week.Days,
                                             selectedFlag = Convert.ToBoolean(week.Isactive),
                                         }).ToList();
                    resultDTO.templateNames = templateNames;
                    resut.ResultOP = resultDTO;
                    resut.Status = true;
                    
                    string eventname = "";
                                          
                    eventname = "Update";

                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = System.Environment.MachineName;
                    auditdto.eventname = eventname;
                    auditdto.module = "WeekDays Plan";
                    auditdto.userid = 0;
                    auditdto.orgid = 1;

                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = content;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
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

        public static dynamic HolidaySave(CalendarEventDTO holidays,dynamic Ipdetails)
        {
            FinalResultDTO resut = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.templateNames = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();

            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    Int64 newid = 0;
                    //var oldholidaydetails = new Holidayplanner();
                    //oldholidaydetails = db.Holidayplanner.Where(x => x.Id == holidays.id).FirstOrDefault();
                    var oldholidaydetails = (from sa in db.Holidayplanner
                                              where sa.Id == holidays.id
                                              select new Holidayplanner
                                              {
                                                   Startdate = sa.Startdate,
                                                   Enddate = sa.Enddate,
                                                      Title = sa.Title,
                                               }).FirstOrDefault();

                    if (holidays.id == 0)
                    {
                        var holidaydetails = new Holidayplanner();
                        //   holidaydetails = db.Holidayplanner.Where(x => x.Id == item.id).FirstOrDefault();
                        holidaydetails.Startdate = holidays.start;
                        holidaydetails.Enddate = holidays.end;
                        holidaydetails.Title = holidays.title;
                        holidaydetails.IsDelete = 0;
                        holidaydetails.Createdby = Convert.ToInt32(holidays.userid);
                        holidaydetails.Createddate = DateTime.Now;
                        db.Holidayplanner.Add(holidaydetails);
                        db.SaveChanges();
                        newid = holidaydetails.Id;
                    }
                    else
                    {
                        var holidaydetails = new Holidayplanner();
                        holidaydetails = db.Holidayplanner.Where(x => x.Id == holidays.id).FirstOrDefault();
                        holidaydetails.Startdate = holidays.start;
                        holidaydetails.Enddate = holidays.end;
                        holidaydetails.Title = holidays.title;
                        holidaydetails.IsDelete = 0;
                        holidaydetails.Modifiedby = Convert.ToInt32(holidays.userid);
                        holidaydetails.Modifieddate = DateTime.Now;
                        // db.Holidayplanner.Add(holidaydetails);
                        db.SaveChanges();
                        
                    }

                    List<string> output = new List<string>();
                    string eventname = "";
                    string description = "";
                    if (holidays.id == 0)
                    {
                        eventname = "Create";
                        string content = "";
                        content = "Start Date :" + holidays.start + "" ;
                        output.Add(content);

                        content = "";
                        content =  " End Date: " + holidays.end + "";
                        output.Add(content);

                        content = "";
                        content = "Title: " + holidays.title + " ";
                        output.Add(content);

                    }
                    else
                    {
                        string content = "";
                        eventname = "Update";
                        if (String.Equals(oldholidaydetails.Startdate, holidays.start))
                        { }
                        else
                        { //description = " Start date: " + oldholidaydetails.Startdate + "  changed to :" + holidays.start + ""; 
                           content = " Start date Changed from  " + oldholidaydetails.Startdate + "  to:  " + holidays.start + "";
                            output.Add(content);
                        }

                        if (String.Equals(oldholidaydetails.Enddate, holidays.end))
                        { }
                        else
                        {
                            //description = description + " End date : " + oldholidaydetails.Enddate + "  changed to :" + holidays.end + ""; 
                            content = "";
                            content = " End date Changed from  " + oldholidaydetails.Enddate + "   to:  " + holidays.end + "";
                            output.Add(content);
                        }

                        if (String.Equals(oldholidaydetails.Title, holidays.title))
                        { }
                        else
                        {
                            //description = description + " Title : " + oldholidaydetails.Title + "  changed to :" + holidays.title + ""; 
                            content = "";
                            content = " Title Changed from  " + oldholidaydetails.Title + "  to:  " + holidays.title + "";
                            output.Add(content);
                        }

                        
                        
                    }

                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = Ipdetails[0];
                    auditdto.eventname = eventname;
                    auditdto.module = "Holiday plan";
                    auditdto.userid =Convert.ToInt64( holidays.userid);
                    auditdto.orgid = 1;
                    auditdto.ipaddress = Ipdetails[1];
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = description;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);
                    var templateNames = (from week in db.Holidayplanner
                                         select new
                                         {
                                             id = week.Id,
                                             start = week.Startdate,
                                             end = week.Enddate,
                                             title = week.Title

                                         }).ToList();
                    resultDTO.templateNames = templateNames;
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

        public static dynamic DeleteData(CalendarEventDTO DetailedID,dynamic Ipdetails)
        {
            FinalResultDTO result = new FinalResultDTO();
            try
            {
                using (dbURSContext db = new dbURSContext())
                {

                    var userData = db.Holidayplanner.Where(x => x.Id == DetailedID.id).FirstOrDefault();
                    userData.Modifiedby = Convert.ToInt32(DetailedID.userid);
                    userData.Modifieddate = DateTime.Now;
                    userData.IsDelete = 1;
                    db.SaveChanges();

                    string eventname = "";
                    string content = "";
                    List<string> output = new List<string>();
                    eventname = "Delete";
                    content = "Start Date :" + userData.Startdate + " , End Date: " + userData.Enddate + ", Title: " + userData.Title + ", Deletedid :" + DetailedID.id + " ";
                    output.Add(content);
                    AuditTrialDTO auditdto = new AuditTrialDTO();
                    AuditTrialJSONDTO auditTrialJSONDTO = new AuditTrialJSONDTO();
                    auditdto.browser = Ipdetails[0];
                    auditdto.eventname = eventname;
                    auditdto.module = "Holiday plan";
                    auditdto.userid = Convert.ToInt64(DetailedID.userid);
                    auditdto.orgid = 1;
                    auditdto.ipaddress = Ipdetails[1];
                    auditdto.description = JsonConvert.SerializeObject(output);
                    auditdto.Systemremarks = content;
                    AuditTrialDAL.CreateAuditTrial(auditdto, auditTrialJSONDTO);


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


        public static dynamic Holidays()
        {
            FinalResultDTO resut = new FinalResultDTO();
            dynamic resultDTO = new ExpandoObject();
            resultDTO.templateNames = new ExpandoObject();
            resultDTO.UserDetails = new ExpandoObject();
            List<CalendarEventDTO> weekdto = new List<CalendarEventDTO>();

            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    weekdto = (from week in db.Holidayplanner
                               where week.IsDelete == 0
                               select new CalendarEventDTO
                               {
                                   id = week.Id,
                                   start = (week.Startdate),
                                   end = (week.Enddate),
                                   title = week.Title,
                               }).ToList();

                    resultDTO.templateNames = weekdto;

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


    }
}
