using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDecForms
{
  public  class TechnicalCommiteDto
    {
        public TechnicalCommiteDto()
        {
            Approval = new List<ApprovalDto>();
            Authority = new List<AuthorityDto>();
            AuditAgency = new List<AuditAgencyDto>();
        }

        public List<ApprovalDto> Approval { get; set; }
        public List<AuthorityDto> Authority { get; set; }
        public List<AuditAgencyDto> AuditAgency { get; set; }

    }

    public class ApprovalDto
    {
        public long? Id { get; set; }

        public string Name { get; set; }
        public string Signature { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }
    }

    public class ApprovalAndTypeDto : ApprovalDto {
        public string TypeofUser { get; set; }
    }
    public class AuthorityDto
    {
        public long? Id { get; set; }
        public long? MemberNameId { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public int? AuditDecFormId { get; set; }
        public BusinessEntityWorkflowActorType Type { get; set; }
    }
    public class AuditAgencyDto
    {
        public long? Id { get; set; }

        public long? MemberNameId { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }

        public int? AuditDecFormId { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }
    }

}
