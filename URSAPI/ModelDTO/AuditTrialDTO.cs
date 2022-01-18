using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class AuditTrialDTO
    {
        public string browser { get; set; }
        public long orgid { get; set; }
        public long? userid { get; set; }
        public string eventname { get; set; }
        public string ipaddress { get; set; }
        public string module { get; set; }
        public DateTime createddate { get; set; }
        public string description { get; set; }
        public string Systemremarks { get; set; }
        public string RequestId { get; set; }
        public string Attachments { get; set; }
        public long existsentry { get; set; }

    }

    public class AuditTrailFilters
    {
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
    }

    public class AuditTrialJSONDTO
    {
        public string browser { get; set; }
        public long? userid { get; set; }
        public string eventname { get; set; }
        public string ipaddress { get; set; }
        public string module { get; set; }
        public DateTime createddate { get; set; }
        public List<AccessType> accesstype { get; set; }
        public List<string> categorydescription { get; set; }
        public string Systemremarks { get; set; }
        public string Userremark { get; set; }
        public string requestId { get; set; }
        public string username { get; set; }
        public List<string> basicRequest { get; set; }
        public List<AttachmentTable> filelist { get; set; }
    }

    public class AccessType
    {
        public long? accesscategoryid { get; set; }
        public string accesscategory { get; set; }
        public long? subcategoryid { get; set; }
        public string subcategory { get; set; }
        public long? accesstypeid { get; set; }
        public string accesstype { get; set; }
        public long? userid { get; set; }
        public string username { get; set; }
        public long? requestdetailid { get; set; }
        // public string accessdescription { get; set; }
        public List<string> categorydescription { get; set; }
        public List<securitypolicyAudit> securitypolicy { get; set; }
        public List<RiskAudit> riskaudit { get; set; }
        public List<RiskAudit> securityrank { get; set; }
        public List<RiskAudit> securitymitigation { get; set; }
        public List<RiskAudit> securityplan { get; set; }


    }

    public class AuditTrialBindDTO
    {
        public long id { get; set; }
        public string  browser{ get; set; }
        public string  event1 { get; set; }
        public  string  ipAddress { get; set; }
        public  string  loginUserName { get; set; }
        public  List<string> userRemarks { get; set; }
        public string module{ get; set; }
        public string  projectName { get; set; }
        public DateTime createdTime{ get; set; }
        public string systemRemarks { get; set; }
    }

    public class MainAuditTrialBindDTO
    {
        public string date { get; set; }
        public List<AuditTrialBindDTO> value { get; set; } 

    }

    public class FinalAudittrialDTO
    {
        public List<AuditTrialJSONDTO> value { get; set; }
    }

    public class AuditUser {
        public long id { get; set; }
        public string firstName { get; set; }
    }

    public class securitypolicyRoot
    {
        public int id { get; set; }
        public List<Protocal> Sourcevpcaccount { get; set; }
        public string Sourceipaddress { get; set; }
        public List<Protocal> Destinationvpcaccount { get; set; }
        public string Destination { get; set; }
        public string Application { get; set; }
        public string Portservice { get; set; }
       
        public List<Protocal> protocal { get; set; }
    }
    public class securitypolicyAudit
    {
         
        public string Sourcevpcaccount { get; set; }
        public string Sourceipaddress { get; set; }
        public  string Destinationvpcaccount { get; set; }
        public string Destination { get; set; }
        public string Application { get; set; }
        public string Portservice { get; set; }
        public string  Protocal { get; set; }
    }

    public class Protocal
    {
        public int categoryId { get; set; }
        public object type { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public int displayOrder { get; set; }
        public string value { get; set; }
    }

    public class RiskAudit
    {
        public string question { get; set; }
        public string riskyesno { get; set; }
        public string riskcomment { get; set; }
        public string likelyhood { get; set; }
        public string severity { get; set; }
        public string riskmitigationstrategy { get; set; }
        public string reposibleparty { get; set; }
        public string plantcompletion { get; set; }
        public string remediationaction { get; set; }
        public string status { get; set; }
        public string ranking { get; set; }
    }

    public class idname
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
