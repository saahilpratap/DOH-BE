using LockthreatCompliance.Enums;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.IRMRelations.Dtos;
using Abp.Domain.Entities;
using LockthreatCompliance.Authorization.Users.Dto;

namespace LockthreatCompliance.Exceptions.Dtos
{
    public class CreateOrEditExceptionDto : EntityDto<int?>
    {
        public string Code { get; set; }
        public int TypeId { get; set; }
        public string Title { get; set; }
        public DateTime NextReviewDate { get; set; }
        public DateTime ApprovedTillDate { get; set; }
        public string Comment { get; set; }
        public string ExceptionDetails { get; set; }
        public int BusinessEntityId { get; set; }
        public long? ExpertReviewerId { get; set; }
        public long? ApproverId { get; set; }
        public List<int> ImpactedControlRequirementIds { get; set; }
        public List<int> CompensatingControlIds { get; set; }
        public List<AttachmentWithTitleDto> Attachments { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime RequestDate { get; set; }
        public IRMRelationDto EntityIRMRelations { get; set; }
        public IRMRelationDto AuthorityIRMRelations { get; set; }

        public List<int> SelectedExceptionRemediations { get; set; }

        public List<int> ExceptionRelatedBusinessRisks { get; set; }

        public List<int> RelatedFindings { get; set; }

        public List<int> RelatedIncidents { get; set; }

        public int? CriticalityId { get; set; }

        public int? ReviewPriorityId { get; set; }

        public long RequestorId { get; set; }

        public UserListDto RequestorUser { get; set; }
    }

    public class ExceptionRemediationDto : Entity
    {
        public int ExceptionId { get; set; }
        public int? RemediationId { get; set; }
    }

}