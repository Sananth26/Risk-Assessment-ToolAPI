using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{

    public partial class MyRequestDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string RequestDate { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
        public string CreatedBy { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string WorkFlowSatge { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
 

        public List<IFormFile> StaticAttachments { get; set; }

        public List<BulkRequestDTo> BulkRequest { get; set; }
    }
    public partial class requestMasterDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
        public string CreatedBy { get; set; }
    }

    public partial class BulkRequestDTo
    {
        public long Id { get; set; }
        public long temptblId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public long RequestId { get; set; }
        public string RequestDate { get; set; }
        public long AccessCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public long AccessTypeId { get; set; }
        public long UserIdDDL { get; set; }
        public string AccessCategory { get; set; }
        public string SubCategory { get; set; }
        public string AccessType { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateOfRevoke { get; set; }
        public long? level { get; set; }
        public bool existingRecord { get; set; }
        public string RequestDetailsId { get; set; }
        public string Status { get; set; }
        public string WorkFlowSatge { get; set; }
        public bool isDeleted { get; set; }
        public string SlaData { get; set; }
        public string AssignedTo { get; set; }
    }
    public class AttachmentTable
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Date { get; set; }
        public string AttachementType { get; set; }
    }
    public class AttachmentDB
    {
        public List<AttachmentTable> StaticTB { get; set; }
    }

    public class NewRequestDataAttachement
    {
        public List<IFormFile> StaticAttachments { get; set; }
        public string StaticAttachmentsTable { get; set; }
        public string NewRequestData { get; set; }
    }
    public class flowRequestData
    {

        public string BulkRequest { get; set; }
    }

    public class flowDto
    {
        public List<BulkRequestDTo> BulkRequest { get; set; }

    }
    public class flowRequestDataBulk
    {
        public Int32 DecisionId { get; set; }
        public string Decision { get; set; }
        public string Remarks { get; set; }
        public string BulkRequest { get; set; }
    }
    public class UserLevels
    {
        public Int32 UserId { get; set; }
        public string FirstName { get; set; }
        public Int32 Level { get; set; }
        public Int32 Supervisor { get; set; }
        public bool SlaFlag { get; set; }
        public Int32 SlaDays { get; set; }
    }

    public class ApprovalRequestDTO
    {
        public long RequestMasterId { get; set; }
        public long RequestDetailsId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long RequestId { get; set; }
        public string AccessCategoryname { get; set; }
        public long AccessCategoryId { get; set; }
        public string SubaccessCategoryname { get; set; }
        public long SubaccessCategoryId { get; set; }
        public string AccessTypename { get; set; }
        public long AccessTypeId { get; set; }
        public string UserIdDll { get; set; }
        public long Level { get; set; }
        public string WorkFlowStage { get; set; }
        public string Status { get; set; }
        public Boolean CompleteDeptProcess { get; set; }
        public string SlaData { get; set; }
        public string AssignedTo { get; set; }
    }

    public class LoadRequestData
    {
        public long RequestMasterId { get; set; }
        public long RequestDetailsId { get; set; }
        public long RequestId { get; set; }
        public long Level { get; set; }
        public string WorkFlowStage { get; set; }
        public Boolean CompleteDeptProcess { get; set; }
        public string status { get; set; }

    }

    public class ApproveRejectRequestDTO
    {
        public long RequestDetailsId { get; set; }
        public long RequestMasterId { get; set; }
         public long Level { get; set; }
        public string WorkFlowStage { get; set; }
        public string Remark { get; set; }
        public long ApproverId { get; set; }
        public string tableData { get; set; }
        public long DecisionId { get; set; }
        public string Decision{ get; set; }
        public Boolean CompleteDeptProcess { get; set; }
        public long LoginUserID { get; set; }
 
        public List<DetailTableData> TableData { get; set; }


    }
    public class ApproveRejectRequest
    {
        public List<IFormFile> StaticAttachments { get; set; }
        public string StaticAttachmentsTable { get; set; }
        public string ApproveRejectDTO { get; set; }
    }
    public partial class DetailTableData
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string userName { get; set; }
        public long RequestId { get; set; }
        public long AccessCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public long AccessTypeId { get; set; }
        public long UserIdDDL { get; set; }
        public string accessCategoryname { get; set; }
        public string subaccessCategoryname { get; set; }
        public string accessTypename { get; set; }
         public string workflowStage { get; set; }
         public long? level { get; set; } 


    }

    public class ApproveDetailsJson
    {
        public long Id { get; set; }
        public long Level { get; set; }
        public string WorkFlowStage { get; set; }
        public string Remark { get; set; }
        public string status { get; set; }
        public long? approverId { get; set; }
        public string approverName { get; set; }
        public DateTime approverDatetime { get; set; }
    }

    public class AttachmentJson
    {
        public List<AttachmentTable> StaticTB { get; set; }
        public long Id { get; set; }
    }

    public class StepperObj
    {
        public Int32 RequestMasterId { get; set; }
        public List<ApproveDetailsJsonNew> DepartmentWFList { get; set; }
        //public bool Processing { get; set; }
        //public SLADetails SLAData { get; set; }
        public bool ITWFFlag { get; set; }
        public List<RequestDetailsObj> ITWFList { get; set; }
    }

    public class RequestDetailsObj
    {
        public Int32 RequestDetailId { get; set; }
        public string RequestDetailSNId { get; set; }
        public List<ApproveDetailsJsonNew> ApprovalDetails { get; set; }
    }

    public class ApproveDetailsJsonNew
    {
        public long Id { get; set; }
        public long Level { get; set; }
        public string WorkFlowStage { get; set; }
        public string Remark { get; set; }
        public string status { get; set; }
        public long? approverId { get; set; }
        public string approverName { get; set; }
        public DateTime approverDatetime { get; set; }
        public SLADetails SLAData { get; set; }
    }


    public class DeleteRequest
    {
        public List<IFormFile> StaticAttachments { get; set; }
        public string StaticAttachmentsTable { get; set; }
        public string DeleteDetails { get; set; }
    }
    public class DeleteDto
    {
        public long requestDetailsId { get; set; }
        public string remark { get; set; }
        public long UserId { get; set; }
    }

    public class DeleteDto1
    {
        public long requestDetailsId { get; set; }
        public string Remarks { get; set; }
        public long UserId { get; set; }
    }

    public class DeptLevelEmployess
    {
        public Int32 Level { get; set; }
        public Int32 EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool SLAFlag { get; set; }
       
    }

    public class RequestInfoDTO
    {
        public long userId { get; set; }
        public string status { get; set; }

    }

    public class ApproveRequestDTO
    {
        public long id { get; set; }
        public string subRequestId { get; set; }
        public long userID { get; set; }
        public string userName { get; set; }
        public long requestId { get; set; }
        public string accessCategoryname { get; set; }
        public long accessCategoryId { get; set; }
        public string subaccessCategoryname { get; set; }
        public long subaccessCategoryId { get; set; }
        public string accessTypename { get; set; }
        public long accessTypeId { get; set; }
        public string workflowStage { get; set; }
        public string userIdDll { get; set; }
        public Int32 level { get; set; }
        public string status { get; set; }
        public string SlaData { get; set; }
        public string AssignedTo { get; set; }
    }

    public class Question
    {
        public long id { get; set; }
        public string Risk { get; set; }
        public string QuestionToAsk { get; set; }
        public string Explanation { get; set; }
        public string yesNo { get; set; }
        public string Comments { get; set; }
        public string decisionLikelihood { get; set; }
        public string decisionSeverity { get; set; }
        public string decisionRanking { get; set; }
        public string QuestionNumber { get; set; }
        public long CategoryId {get;set;}
        public string  score { get; set; }
        public string ranking { get; set; }
        public string riskMitigationComments { get; set; }
        public string responsibleParty { get; set; }
        public string plannedCompletion { get; set; }
        public string status { get; set; }
        public string remediationResponsibleParty { get; set; }
        public string remediationPlan { get; set; }
        public string color { get; set; }
        public QuestionExplanationDTO QuestionExplanation  { get; set; }

    }
    public class QuestionExplanationDTO
    {
        public string question { get; set; }
        public string explanation { get; set; }
        public string risk { get; set; }
        public string questionnumber { get; set; }
    }

    public class FinalQuestion
    {
        public string id { get; set; }
        public string RiskCategory { get; set; }
        public string CalcualtnLikelihood { get; set; }
        public string CalcualtnSeverity { get; set; }
        public string CalcualtnRanking { get; set; }
        public string score { get; set; }
        public string ranking { get; set; }
        public string riskMitigationComments { get; set; }
        public string responsibleParty { get; set; }
        public string plannedCompletion { get; set; }
        public string status { get; set; }
        public string remediationResponsibleParty { get; set; }
        public string remediationPlan { get; set; }
        public string color { get; set; }
        public long categoryId { get; set; }
        public List<Question> questions { get; set; }
    }

    public class FinalQuestionResponsible
    {
        public string id { get; set; }
        public string RiskCategory { get; set; }
        public string CalcualtnLikelihood { get; set; }
        public string CalcualtnSeverity { get; set; }
        public string CalcualtnRanking { get; set; }
        public string score { get; set; }
        public string ranking { get; set; }
        public string riskMitigationComments { get; set; }
       // public string responsibleParty { get; set; }
        public string plannedCompletion { get; set; }
        public string status { get; set; }
        public string remediationResponsibleParty { get; set; }
        public string remediationPlan { get; set; }
        public string color { get; set; }
        public long categoryId { get; set; }
        public List<ResponsibleParty> responsibleParty { get; set; }

        public List<Question> questions { get; set; }
    }

    public class ResponsibleParty
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}