using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class UserRequestForm
    {
        public long Requestid { get; set; }
        public long CategoryId { get; set; }
        public long SubcategoryId { get; set; }
        public string BusinessJustification { get; set; }
        public string ManagedServices { get; set; }
        public long? FirewallRegion { get; set; }
        public long? NormalExpedited { get; set; }
        public long? BusinessOwnersId { get; set; }
        public long? ItownersId { get; set; }
        public string NameOfProject { get; set; }
        public string Description { get; set; }
        public string BusinessImpact { get; set; }
        public string SecurityPolicy { get; set; }
        public string ArchitectureDiagram { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
        public string riskandRankDetails { get; set; }
        public object businessOwner { get; set; }
        public object itOwner { get; set; }
        public string ispeerreview { get; set; }
    }
    public class NewUserRequestDataAttachement
    {
        public List<IFormFile> StaticAttachments { get; set; }
        public string StaticAttachmentsTable { get; set; }
        public string NewRequestData { get; set; }
      //  public List<SecurityPolicy> securityPolicies { get; set; }

    }

    public class SecurityPolicy
    {
        public long Id { get; set; }
        public long RequestFormId { get; set; }
        public long SourceVpcid { get; set; }
        public string SourceVpcname { get; set; }
        public string SourceIpaddress { get; set; }
        public long DestinationVpcaccount { get; set; }
        public string DestinationVpcaccountname { get; set; }
        public string DestinationAddress { get; set; }
        public string Application { get; set; }
        public string PortService { get; set; }
        public long Protocol { get; set; }
        public string ProtocolName { get; set; }

    }
    public class EditRequestModel
    {
        public long Id { get; set; }
        public string requestSno { get; set; }
        public long categoryId { get; set; }
        public long subCategoryId { get; set; }
        public string categoryName { get; set; }
        public string subcategoryName { get; set; }
        public string UserName { get; set; }
        public string FirewallRegionName { get; set; }
        public string NormalExpecticationName { get; set; }
        public string businessJustification { get; set; }
        public string managedServices { get; set; }
        public long? firewallRegion { get; set; }
        public long? normalExpedited { get; set; }
        public long? businessownersId { get; set; }
        // itownersId = req.ItownersId,
        public string nameOfProject { get; set; }
        public string description { get; set; }
        public string businessImpact { get; set; }
        public string securityPolicy { get; set; }
        public string architectureDiagram { get; set; }
        public string status { get; set; }
        public long? userId { get; set; }
        public string attachment { get; set; }
        public string riskandRankDetails { get; set; }
        public string remarks { get; set; }
        public string filepath { get; set; }
        public string businessOwner { get; set; }
        public string itOwner { get; set; }
    }
}
