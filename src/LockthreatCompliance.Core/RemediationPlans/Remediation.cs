using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Authorization.Users;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.RemediationPlans
{
    [Table("Remediations")] 
    public class Remediation: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "AD-" + Id.GetCodeEnding(); } }
        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public string Title { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string RemediationPlanDetail { get; set; }

        public bool IsRemediation { get; set; }
        public DateTime? ActualClosureDate { get; set; }
        public long? ExpertReviewerId   { get; set; }
        public User ExpertReviewer { get; set; }
        public long? EntityApproverId  { get; set; }
        public User EntityApprover { get; set; }
        public string ReviewerComment { get; set; }
        public DateTime? ApprovedTillDate { get; set; }
        public string Signature { get; set; }
        public long? AuthorityExpertReviewerId { get; set; }
        public User AuthorityExpertReviewer { get; set; }
        public long? AuthorityApproverId { get; set; }
        public User AuthorityApprover  { get; set; }
        public string ReviewComment { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public DateTime? AuthorityApprovedTillDate  { get; set; }
        public string Authoritysignature { get; set; }
        public int? RemediationPlanStatusId  { get; set; }
        public DynamicParameterValue RemediationPlanStatus  { get; set; }

      //  public List<RemediationDocument> Attachments { get; set; }
    }
}
