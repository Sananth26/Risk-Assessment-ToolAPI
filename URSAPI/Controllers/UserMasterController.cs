using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using OrbintSoft.Yauaa.Analyzer;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;
using URSAPI.Models;

namespace URSAPI.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IConfiguration _config;

        public UserMasterController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadUserDatas")]
        public dynamic LoadUserDatas()
        {
            return UserMasterDAL.LoadUserDatas();
        }
        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadAllUserDatas")]
        public dynamic LoadAllUserDatas()
        {
            return UserMasterDAL.LoadAllUserDatas();
        }
        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadDepartments")]
        public dynamic LoadDepartments()
        {
           return UserMasterDAL.LoadDepartments();
        }

        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadRoles")]
        public dynamic LoadRoles()
        {
            return UserMasterDAL.LoadRoles();
        }

        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadUsers")]
        public dynamic LoadUsers(Int32 deptId)
        {
            return UserMasterDAL.LoadUsers(deptId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/LoadUsersForEdit")]
        public dynamic LoadUsersForEdit(Int32 userId)
        {
            return UserMasterDAL.LoadUsersForEdit(userId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/UserMaster/CreateOrUpdateUser")]
        public dynamic CreateOrUpdateUser([FromBody]UserDTO inputParam)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserMasterDAL.CreateOrUpdateUser(inputParam,userid, ipdetails,_timezone);
        }

        [Authorize]
        [HttpGet]
        [Route("api/UserMaster/DeleteUser")]
        public dynamic DeleteUser(Int32 userId)
        {
            Int64 userid = Convert.ToInt64(User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var ipdetails = Getip();
            string _timezone = _config.GetValue<string>("TimeZone");
            return UserMasterDAL.DeleteUser(userId,userid,ipdetails,_timezone);
        }

        
        [HttpGet]
        [Route("api/UserMaster/ExcelExport")]
        public dynamic ExcelExport()
        {
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    DataTable dtRoleID = new DataTable();

                    var roleDetails = (from t1 in db.Roles
                                       select new { t1.Id, t1.Name }).ToList();

                   dtRoleID = UserMasterDAL.ToDataTable(roleDetails);


                    DataTable dtDeptData = new DataTable();

                    var DeptDetails = (from t1 in db.Department where t1.DeleteFlag == "N"
                                       select new { t1.Id, t1.DepartmentName }).ToList();

                     dtDeptData = UserMasterDAL.ToDataTable(DeptDetails);



                    DataTable dtSuperviserData = new DataTable();

                    var SuperviserDetails = (from t1 in db.User
                                             join t2 in db.Department
                                             on t1.Department equals t2.Id where t1.DeleteFlag == "N"
                                             select new { t1.Id, t1.FirstName }).ToList();
                      dtSuperviserData = UserMasterDAL.ToDataTable(SuperviserDetails);


                    byte[] fileContents;
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        //Create a workSheets inside workbook
                        excel.Workbook.Worksheets.Add("User Data Entry");
                        excel.Workbook.Worksheets.Add("Roles Info");
                        excel.Workbook.Worksheets.Add("Department Info");

                        var worksheetUser = excel.Workbook.Worksheets["User Data Entry"];

                        var headerRow = new List<string[]>()
                    {
                        new string[] { "FirstName", "LastName", "location", "doj", "Email", "MobileNo", "roleName", "DepartmentName", "ImmediateSupervisorName", "ItProcessAccess" }
                    };

                        string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                        // Popular header row data
                        worksheetUser.Cells[headerRange].LoadFromArrays(headerRow);
                        worksheetUser.Cells[headerRange].Style.Font.Bold = true;
                        worksheetUser.Cells[headerRange].Style.Font.Size = 14;
                        worksheetUser.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Blue);

                        worksheetUser.Cells[worksheetUser.Dimension.Address].AutoFitColumns();

                        var unitmeasure = worksheetUser.DataValidations.AddListValidation(worksheetUser.Cells[2, 7, 10000, 7].Address);

                        string concatenate = string.Empty;
                        // var concate = new List<object>();

                        foreach (DataRow dtRow in dtRoleID.Rows)
                        {
                            foreach (DataColumn dc in dtRoleID.Columns)
                            {
                                concatenate += dtRow[dc].ToString() + " : ";
                            }
                            string finalName = concatenate.ToString();

                            finalName = finalName.TrimEnd();
                            finalName = finalName.Substring(0, finalName.Length - 1);

                            unitmeasure.Formula.Values.Add(finalName);
                            concatenate = string.Empty;
                        }

                        //unitmeasure.Formula.Values.Add("Admin");
                        //unitmeasure.Formula.Values.Add("SuperAdmin");
                        //unitmeasure.Formula.Values.Add("User");

                        unitmeasure.ShowErrorMessage = true;

                        unitmeasure.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        unitmeasure.ErrorTitle = "Error";
                        unitmeasure.Error = "Select from the List";
                        unitmeasure.ShowInputMessage = true;
                        unitmeasure.Prompt = "Select Role from the List";

                        var IsActivemeasure = worksheetUser.DataValidations.AddListValidation(worksheetUser.Cells[2, 10, 10000, 10].Address);

                        IsActivemeasure.Formula.Values.Add("Yes");
                        IsActivemeasure.Formula.Values.Add("No");
                        IsActivemeasure.ShowErrorMessage = true;

                        IsActivemeasure.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        IsActivemeasure.ErrorTitle = "Error";
                        IsActivemeasure.Error = "Select from the List";
                        IsActivemeasure.ShowInputMessage = true;
                        IsActivemeasure.Prompt = "Select Active Status from the List";


                        var DeptData = worksheetUser.DataValidations.AddListValidation(worksheetUser.Cells[2, 8, 10000, 8].Address);

                        string concatenateDept = string.Empty;
                        var concateDept = new List<object>();

                        foreach (DataRow dtDept in dtDeptData.Rows)
                        {
                            foreach (DataColumn dcDept in dtDeptData.Columns)
                            {
                                concatenateDept += dtDept[dcDept].ToString() + " : ";
                            }
                            string finalName = concatenateDept.ToString();

                            finalName = finalName.TrimEnd();
                            finalName = finalName.Substring(0, finalName.Length - 1);
                            DeptData.Formula.Values.Add(finalName);

                            concatenateDept = string.Empty;

                        }
                        DeptData.ShowErrorMessage = true;

                        DeptData.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        DeptData.ErrorTitle = "Error";
                        DeptData.Error = "Select from the List";
                        DeptData.ShowInputMessage = true;
                        DeptData.Prompt = "Select Department from the List";


                        var SuperviserData = worksheetUser.DataValidations.AddListValidation(worksheetUser.Cells[2, 9, 10000, 9].Address);

                        string concatenateSupervise = string.Empty;
                        var concateSupervise = new List<object>();

                        foreach (DataRow dtSuperwise in dtSuperviserData.Rows)
                        {
                            foreach (DataColumn dcSuperwise in dtSuperviserData.Columns)
                            {
                                concatenateDept += dtSuperwise[dcSuperwise].ToString() + " : ";
                            }
                            string finalName = concatenateDept.ToString();

                            finalName = finalName.TrimEnd();
                            finalName = finalName.Substring(0, finalName.Length - 1);
                            SuperviserData.Formula.Values.Add(finalName);

                            concatenateDept = string.Empty;

                        }
                        SuperviserData.ShowErrorMessage = true;

                        SuperviserData.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        SuperviserData.ErrorTitle = "Error";
                        SuperviserData.Error = "Select from the List";
                        SuperviserData.ShowInputMessage = true;
                        SuperviserData.Prompt = "Select Department from the List";


                        // sheet -2 
                        var worksheetRoles = excel.Workbook.Worksheets["Roles Info"];
                        var headerRowRoles = new List<string[]>()
                       {
                        new string[] {"ID", "RoleName"}
                       };

                        string headerRangeRoles = "A1:" + Char.ConvertFromUtf32(headerRowRoles[0].Length + 64) + "1";

                        // Dynamic RoleName Based on Orgnaization
                        //var cellRoleData = new List<object[]>()
                        //{
                        //      new object[] {1, "Admin"},
                        //      new object[] {2, "SuperAdmin"},
                        //      new object[] {3, "User"}
                        //};

                        worksheetRoles.Cells[2, 1].LoadFromDataTable(dtRoleID, false);

                        // Popular header row data
                        worksheetRoles.Cells[headerRange].LoadFromArrays(headerRowRoles);
                        worksheetRoles.Cells[headerRange].Style.Font.Bold = true;
                        worksheetRoles.Cells[headerRange].Style.Font.Size = 14;
                        worksheetRoles.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                        worksheetRoles.Protection.IsProtected = true;


                        // sheet - 3
                        var worksheetDept = excel.Workbook.Worksheets["Department Info"];
                        var headerRowDept = new List<string[]>()
                       {
                        new string[] {"ID", "DepartmentName"}
                       };

                        string headerRangeDept = "A1:" + Char.ConvertFromUtf32(headerRowDept[0].Length + 64) + "1";

                        worksheetDept.Cells[2, 1].LoadFromDataTable(dtDeptData, false);

                        // Popular header row data
                        worksheetDept.Cells[headerRange].LoadFromArrays(headerRowDept);
                        worksheetDept.Cells[headerRange].Style.Font.Bold = true;
                        worksheetDept.Cells[headerRange].Style.Font.Size = 14;
                        worksheetDept.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                        worksheetDept.Protection.IsProtected = true;



                        fileContents = excel.GetAsByteArray();
                        //excel.Save();

                        // FileInfo excelFile = new FileInfo(@"E:\test.xlsx");
                        // excel.SaveAs(excelFile);

                        if (fileContents == null || fileContents.Length == 0)
                        {
                            return NotFound();
                        }

                        dtoExcel dt = new dtoExcel();
                        dt.filecont = fileContents;
                        return dt;

                        //return File(
                        //    fileContents: fileContents,
                        //    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        //    fileDownloadName: "test.xlsx"
                        //);

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public class dtoExcel
        {
            public Byte[] filecont { get; set; }
        }
        

        //file upload
       // [Authorize]
        [HttpPost]
        [Route("api/UserCreation/ExcelImport")]
        public dynamic ExcelImport([FromForm] FileInputModel Files)
        {
            FinalResultDTO result = new FinalResultDTO();
            string _timezone = _config.GetValue<string>("TimeZone");
            try
            {
             
                Int64 userid = 2;
                string strReturn = "";
                using (var stream = new MemoryStream())
                {
                    Files.file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int totalRows = worksheet.Dimension.Rows;
                        List<string> mainCategorymissingList = new List<string>();
                        List<categorySaveDTO> categorySaveDTOList = new List<categorySaveDTO>();
                        string mainCategoryname = "";
                        Int64 headerNumber = 0;

                        string categoryName = "";
                        try
                        {
                            using (dbURSContext db = new dbURSContext())
                            {
                                var categoryData = (from lookItem in db.LookupItem
                                                    where lookItem.CategoryId == 1 && lookItem.ActiveFlag == "Y"
                                                    select new DropDownsDTO
                                                    {
                                                        Id = Convert.ToInt32(lookItem.Id),
                                                        key = lookItem.Key,
                                                    }).ToList();
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    mainCategoryname = Convert.ToString(worksheet.Cells[i, 1].Value).ToString();
                                   
                                    var str = categoryData.Where(x=>x.key==mainCategoryname).FirstOrDefault();
                                    if (str == null && mainCategoryname != "")
                                    {
                                        if (!mainCategorymissingList.Contains(mainCategoryname))
                                        {
                                            mainCategorymissingList.Add(mainCategoryname);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        if (mainCategorymissingList.Count==0)
                        {
                            using (dbURSContext db = new dbURSContext())
                            {
                                var maindatacate = db.MainCategory.ToList();
                                
                                maindatacate.ForEach(a => a.IsActive = "N");
                                db.SaveChanges();
                                var cateList = db.Category.ToList();

                                cateList.ForEach(a => a.IsActive = "N");
                                db.SaveChanges();
                                var SubcateList = db.SubCategory.ToList();

                                SubcateList.ForEach(a => a.IsActive = "N");
                                db.SaveChanges();
                                

                                for (int j = 2; j <= totalRows; j++)
                                {
                                    categorySaveDTOList = new List<categorySaveDTO>();
                                    mainCategoryname = Convert.ToString(worksheet.Cells[j, 1].Value).ToString();
                                    categoryName = Convert.ToString(worksheet.Cells[j, 3].Value).ToString();
                                    headerNumber = Convert.ToInt64(worksheet.Cells[j, 2].Value);
                                    if (mainCategoryname != "" && categoryName != "")
                                    {
                                        questionandexplanation jSONRiskexplanDTO = new questionandexplanation();
                                        jSONRiskexplanDTO.risk = Convert.ToString(worksheet.Cells[j, 4].Value).ToString();
                                        jSONRiskexplanDTO.explanation = Convert.ToString(worksheet.Cells[j, 7].Value).ToString();
                                        jSONRiskexplanDTO.question = Convert.ToString(worksheet.Cells[j, 6].Value).ToString();
                                        jSONRiskexplanDTO.questionnumber = Convert.ToString(worksheet.Cells[j, 5].Value).ToString();
                                        categorySaveDTOList.Add(new categorySaveDTO
                                        {
                                            categoryName = mainCategoryname,
                                            question = categoryName,
                                            risk = jSONRiskexplanDTO.risk,
                                            headernumber = headerNumber,
                                            questionnumber = Convert.ToString(worksheet.Cells[j, 5].Value).ToString(),
                                            explanation = Newtonsoft.Json.JsonConvert.SerializeObject(jSONRiskexplanDTO)
                                        });
                                        strReturn = UserMasterDAL.InsertCategory(categorySaveDTOList, userid,_timezone);
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.Status = true;
                            result.Description = "";
                            string combindedString = string.Join(",", mainCategorymissingList.ToArray());
                            result.ResultOP = ("Incorrect categories: "+combindedString);
                            return result;
                        }
                    }
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {

                result.Status = false;
                result.Description = "Something went wrong while inserting the user details.. Please check the Data. ";
                return result;
            }
        }

        public dynamic Getip()
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
            string userAgent = Request.Headers?.FirstOrDefault(s => s.Key.ToLower() == "user-agent").Value;
            var ua = YauaaSingleton.Analyzer.Parse(userAgent);
            var browserName = ua.Get(UserAgent.AGENT_NAME).GetValue();
            var version = ua.Get(UserAgent.AGENT_NAME_VERSION_MAJOR).GetValue();
            string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();

            //127.0.0.1    localhost
            //::1          localhost
            if (ip == "::1")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ipa in host.AddressList)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = ipa.ToString();
                    }
                }
            }
            List<string> output = new List<string>();
            string content = "";
            content = version + " , " + System.Environment.MachineName;
            output.Add(content);
            content = "";
            content = ip;
            output.Add(content);
            return output;
        }



    }
}