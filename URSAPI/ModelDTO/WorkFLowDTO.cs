using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class WorkFLowDTO
    {
    }
    public class WFCategoryDTO
    {
        public Int32 Id { get; set; }
        public string key { get; set; }
        //public string SubCategoryName { get; set; }
    }
    public class WorkFlowMasterDTO
    {
        public Int32 Id { get; set; }
        public string WorkFlowLevelName { get; set; }
        public string ActiveFlag { get; set; }
    }
    public class UASWorkFlow
    {
        public Int32 Id { get; set; }
        public Int32 CombinationId { get; set; }
        public Int32 Level { get; set; }
        public string LevelName { get; set; }
        public Int32 deleteLevel { get; set; }
        public List<DropDownsDTO> SelectedRoles { get; set; }
        public List<DropDownsDTO> SelectedCategory { get; set; }
        public List<SubCategoryList> SelectedSubCategory { get; set; }
        public List<SubCategoryList> SubCategoryList { get; set; }
        public List<UsersDTO> UserList { get; set; }
        public List<UsersDTO> SelectedUsers { get; set; }
        public bool SlaActive { get; set; }
        public Int32 SlaDays { get; set; }
        public bool IsActive { get; set; }
        public bool Delete { get; set; }
    }

    public class UsersDTO
    {
        public Int32 Id { get; set; }
        public string UserFirstName { get; set; }
        public Int32 UserRoleId { get; set; }
    }

    public class SubCategoryList
    {
       public Int32 Id { get; set; }
       public string Key { get; set; }
       public Int32 CategoryId { get; set; }
    }
    public class PeerreviewList
    {
        public List<UsersDTO> userlist { get; set; }
        public Int64 id { get; set; }
        public string key { get; set; }
        public Int64 categoryId { get; set; }
    }
    public class SubCategoryList1
    {
        public Int32 Id { get; set; }
        public string Key { get; set; }
        public Int32 CategoryId { get; set; }
    }

    public class DropDownsDTO
    {
        public Int32 Id { get; set; }
        public string key { get; set; }
    }
    

    public class WorkFlowMasterDetails
    {
      public Int32  Id { get; set; }
      public Int32  Level { get; set; }
      public string  LevelName { get; set; }
       public Boolean IsActive { get; set; }
        public string IsActiveCopy { get; set; }
        public Int32 CombinationId { get; set; }
        public Boolean selectedCheck { get; set; }
        public string users { get; set; }
    }

    public class WorkFlowUserDetails
    {
        public Int32 Level { get; set; }
        public string LevelName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string User { get; set; }
        public Int32 CategoryID { get; set; }
        public Int32 SubCategoryID { get; set; }
        public Int32 UserID { get; set; }
        public string UserRole { get; set; }
        public List<string> users { get; set; }
        public Int64 combinationId { get; set; }
        public Int64 Id { get; set; }
        public string activeFlag { get; set; }
        public Boolean SlaFlag { get; set; }
        public Int32 SlaDays { get; set; }
       public List<DropDownsDTO> usersIds { get; set; }


    }

    public class UASWorkFlowCL
    {
         
        public List<DropDownsDTO> FromSelectedCategory { get; set; }
        public List<SubCategoryList> FromSelectedSubCategory { get; set; }
        public List<SubCategoryList> FromSubCategoryList { get; set; }
        public List<DropDownsDTO> ToSelectedCategory { get; set; }
        public List<SubCategoryList> ToSelectedSubCategory { get; set; }
        public List<SubCategoryList> ToSubCategoryList { get; set; }
        public List<WorkFlowMasterDetails> SelectedLevels { get; set; }
         
    }

    public class UASWorkFlow1
    {
        public Int32 Id { get; set; }
        public Int32 CombinationId { get; set; }
        public Int32 Level { get; set; }
        public string LevelName { get; set; }
        public string SlaActive { get; set; }
        public Int32 SlaDays { get; set; }
        public string IsActive { get; set; }
        
    }
    public class DeleteLevelBase
    {
        public Int32 combinationId { get; set; }
        public Int32 level { get; set; }
    }
    public class MenuaccessandLevelAccess
    {
        public Int32 menuaccess { get; set; }
        public Int32 levelaccess { get; set; }
    }

    public class   ApproveDTO
    {
        public Int32  requestId { get; set; }
        public string  status { get; set; }
        public string   QuestionJSON { get; set; }
        public string comment { get; set; }
    }
    public class Valueonly
    {
        public string Mailaddress { get; set; }
    }
}
