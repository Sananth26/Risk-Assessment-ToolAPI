using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Department { get; set; }
        public string CorporateId { get; set; }

    }

    public class FinalResultDTO
    {
        public bool Status { get; set; }
        public string Description { get; set; }
        public dynamic ResultOP { get; set; }
    }

    public class AccessApproval
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class UserDTO
    {
        public Int32 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string DepartmentName { get; set; }
        public Int32 DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ImmediateSupervisorId { get; set; }
        public string ImmediateSupervisorName { get; set; }
        public string doj { get; set; }
        public string location { get; set; }
        public string ItProcessAccess { get; set; }
        public string ManagerAccess { get; set; }
        public string IsActive { get; set; }
        public string corporateId { get; set; }
        // public Boolean ItProcessAccess { get; set; }
        public string password { get; set; }
        public long? roleId { get; set; }
        public string roleName { get; set; }

    }

    public class DepartmentDTO
    {
        public long Id { get; set; }
        public string DepartmentName { get; set; }
        //public Boolean IsActive { get; set; }
        public string IsActive { get; set; }
        public string SlaFlag { get; set; }
        public string SlaJson { get; set; }
        public string SlaJsondata { get; set; }
        public int NoOfLevel { get; set; }
        public string DeleteFlag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class FileInputModel
    {
        public IFormFile file { get; set; }
        public string Param { get; set; }
    }

    public class SlaJson
    {
        public Int32 levelNo { get; set; }
        public Int32 slaDays { get; set; }

    }


    public class SidemenuDTO
    {
        public string path { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string @class { get; set; }
        public string badge { get; set; }
        public string badgeClass { get; set; }
        public Boolean isExternalLink { get; set; }
        public long displayorder { get; set; }
        public List<string> submenu { get; set; }
    }

    public class RolePermissionDTO1
    {
        public Int32 userId { get; set; }
        public Int32 orgId { get; set; }
        public string url { get; set; }
    }

    public class UserAccess
    {
        public Int32 menuId { get; set; }
        public string url { get; set; }
        public Int32 roleId { get; set; }
        public Int32 add { get; set; }
        public Int32 view { get; set; }
        public Int32 delete { get; set; }
        public Int32 edit { get; set; }
        public Int32 export { get; set; }
        public Int32 print { get; set; }
        public Int32 menuAccess { get; set; }

    }

    public class ButtonAccess
    {
        public Boolean add { get; set; }
        public Boolean view { get; set; }
        public Boolean delete { get; set; }
        public Boolean edit { get; set; }
        public Boolean export { get; set; }
        public Boolean print { get; set; }

    }

    public class SLADetails
    {
        public bool SlaFlag { get; set; }
        public Int32 SlaDays { get; set; }
        public string WorkflowStage { get; set; }
        public Int32 LevelNo { get; set; }
        public DateTime SlaTargetDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public Int32 RemainingSLADays { get; set; }
        public bool OverDueFlag { get; set; }
        public Int32 OverDueDays { get; set; }
        public List<DropDownsDTO> Users { get; set; }
    }
    public class Password
    {
        public Int64 userId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
