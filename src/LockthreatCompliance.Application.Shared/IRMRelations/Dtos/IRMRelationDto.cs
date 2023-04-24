using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.IRMRelations.Dtos
{
    public class IRMRelationDto : FullAuditedEntityDto<long>
    {
        public int? TenantId { get; set; }
        public string ReviewComments { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Signature { get; set; }
        public int? IncidentId { get; set; }
        public int? ExceptionId { get; set; }
        public int? BusinessRiskId { get; set; }
        public int? FindingReportId { get; set; }
        public List<long> EntityReviewers { get; set; }
        public List<string> EntityReviewersSignature { get; set; }
        public List<long> EntityApprovers { get; set; }
        public List<string> EntityApproversSignature { get; set; }
        public List<long> AuthorityReviewers { get; set; }
        public List<string> AuthorityReviewersSignature { get; set; }
        public List<long> AuthorityApprovers { get; set; }
        public List<string> AuthorityApproversSignature { get; set; }
        public IRMUserType IRMUserType { get; set; }

    }
}
