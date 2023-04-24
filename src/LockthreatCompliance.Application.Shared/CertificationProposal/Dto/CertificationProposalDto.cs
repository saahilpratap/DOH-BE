using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.CertificationProposal.Dto
{
    public class CertificationProposalDto : Entity<int>
    {
        public int? TenantId { get; set; }
        public long? AuditProjectId { get; set; }
        public virtual string CPIDCode { get; set; }
        public virtual string ReleaseNumber { get; set; }
        public int ReferenceStandard { get; set; }
        public int? EntityGroupId { get; set; }
        public DateTime? ProposalDate { get; set; }

        public string Scope { get; set; }
        public virtual string Grade { get; set; }

        public int TotalManDays { get; set; }
        public int FullyCompliantCount { get; set; }
        public int PartiallyCompliantCount { get; set; }
        public int NonCompliantCount { get; set; }
        public int NotApplicableCount { get; set; }
    }

    public class CertificationProposalInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id Desc";
            }

            Filter = Filter?.Trim();
        }
    }

    public class CertificationProposalOutputDto 
    {
        public CertificationProposalOutputDto()
        {
            EntityGroups = new List<IdNameAndPrimaryDto>();
            BusinessEntities = new List<EntityWithAssessmentDto>();
        }
        public CertificationProposalDto certificationProposalDto { get; set; }
    
        public List<IdNameAndPrimaryDto> EntityGroups { get; set; }
        public List<EntityWithAssessmentDto> BusinessEntities { get; set; }
    }

    public class CertificationProposalCalculation {
        public CertificationProposalCalculation() {
            QuestionInfo = new List<QuestionInfo>();
        }
        public int? ExternalAssessmentId { get; set; }
        public string DomainName { get; set; }
        public int NotSelectedCount { get; set; }
        public int NotApplicableCount { get; set; }
        public int NonCompliantCount { get; set; }
        public int PartiallyCompliantCount { get; set; }
        public int FullyCompliantCount { get; set; }
        public int TotalCount { get; set; }
        public List<QuestionInfo> QuestionInfo { get; set; }
    }

    public class QuestionInfo
    {
        public int? ExternalAssessmentId { get; set; }
        public int ControlRequirementId { get; set; }
        public string ControlRequirement { get; set; }
        public ReviewDataResponseType ResponseType { get; set; }

        
    }

}
