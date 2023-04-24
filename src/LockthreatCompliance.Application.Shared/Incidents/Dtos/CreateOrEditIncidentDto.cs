using LockthreatCompliance.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.IRMRelations.Dtos;
using Abp.Domain.Entities;

namespace LockthreatCompliance.Incidents.Dtos
{
    public class CreateOrEditIncidentDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }

        public string Code { get; set; }
        public IncidentStatus Status { get; set; }
        public int IncidentTypeId { get; set; }

        public int IncidentImpactId { get; set; }
        public DateTime DetectionDateTime { get; set; }
        public string RootCause { get; set; }
        public string Remediation { get; set; }
        public string Correction { get; set; }
        public string Prevention { get; set; }
        public string Comment { get; set; }
        public IncidentPriority Priority { get; set; }

        public IncidentSeverity Severity { get; set; }
        public string Description { get; set; }

        public DateTime? CloseDate { get; set; }
        public DateTime CreationTime { get; set; }

        public int BusinessEntityId { get; set; }

        public long? ClosedByUserId { get; set; }

        public List<long> Reviewers { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }

        public IRMRelationDto EntityIRMRelations { get; set; }

        public IRMRelationDto AuthorityIRMRelations { get; set; }

        public List<int> SelectedIncidentRemediations { get; set; }

        public List<int> RelatedBusinessRisks { get; set; }

        public List<int> RelatedExceptions { get; set; }

        public List<int> RelatedFindings { get; set; }
    }

    public class IncidentRemediationDto : Entity
    {
        public int IncidentId { get; set; }
        public int? RemediationId { get; set; }
    }

    public class CreateOrEditIncidentStatusLogDto : EntityDto<long>
    {
        public int IncidentId { get; set; }
        public IncidentStatus Status { get; set; }
        public long? UserActedId { get; set; }
        public DateTime? ActionDate { get; set; }
    }

    public class IdAndName {
        public int Id { get; set; }
        public string Name { get; set; }

    }

}