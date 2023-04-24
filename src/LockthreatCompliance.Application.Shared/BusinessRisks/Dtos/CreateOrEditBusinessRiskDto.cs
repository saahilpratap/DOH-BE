
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.IRMRelations.Dtos;
using Abp.Domain.Entities;

namespace LockthreatCompliance.BusinessRisks.Dtos
{
    public class CreateOrEditBusinessRiskDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }


        public string Code { get; set; }
        public DateTime? IdentificationDate { get; set; }


        public string Vulnerability { get; set; }

        public DateTime? CreationTime { get; set; }

        public string RemediationPlan { get; set; }


        public DateTime? ExpectedClosureDate { get; set; }


        public DateTime? CompletionDate { get; set; }

        public virtual DateTime? ActualClosureDate { get; set; }

        public bool IsRemediationCompleted { get; set; }

        [Required]
        public int BusinessEntityId { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }

        public IRMRelationDto EntityIRMRelations { get; set; }

        public IRMRelationDto AuthorityIRMRelations { get; set; }

        public List<int> SelectedBusinessRisksCompensatingControls { get; set; }

        public List<int> SelectedBusinessRisksImpactedControls { get; set; }

        public List<int> SelectedBusinessRisksMonitoringControls { get; set; }

        public long? RiskOwnerId { get; set; }
        public long? RiskManagerId { get; set; }
        public string RiskDetail { get; set; }
        public int? CriticalityId { get; set; }
        public int? RiskImpactId { get; set; }
        public int? RiskLikelihoodId { get; set; }
        public int? RiskTreatmentId { get; set; }
        public int? RiskTypeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? RiskAssessmentDate { get; set; }
        public List<int> RelatedFindings { get; set; }
        public List<int> RelatedExceptions { get; set; }
        public List<int> RelatedIncidents { get; set; }
        public ICollection<int> SelectedBusinessRiskRemediations { get; set; }
    }

    public class BusinessRiskRemediationDto : Entity
    {
        public int BusinessRiskId { get; set; }
        public int? RemediationId { get; set; }
    }
}