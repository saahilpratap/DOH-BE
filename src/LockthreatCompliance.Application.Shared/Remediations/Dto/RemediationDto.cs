using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Remediations.Dto
{
  public  class RemediationDto :FullAuditedEntityDto
    { 
        public int? TenantId { get; set; }        
        public virtual string Code { get; set; }
        public int BusinessEntityId { get; set; }       
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsRemediation { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string RemediationPlanDetail { get; set; }
        public DateTime? ActualClosureDate { get; set; }
        public long? ExpertReviewerId { get; set; }      
        public long? EntityApproverId { get; set; }      
        public string ReviewerComment { get; set; }
        public DateTime? ApprovedTillDate { get; set; }
        public string Signature { get; set; }
        public long? AuthorityExpertReviewerId { get; set; }      
        public long? AuthorityApproverId { get; set; }   
        public string ReviewComment { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public DateTime? AuthorityApprovedTillDate { get; set; }
        public string Authoritysignature { get; set; }
        public int? RemediationPlanStatusId { get; set; }
        public List<DynamicNameValueDto>  RemediationPlanStatusList  { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }
    }
}
