using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.IRMRelations.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class CreateOrEditFindingReportDto : EntityDto<int?>
    {
        public CreateOrEditFindingReportDto()
        {
            CAPAUpdateRequired = false;
            Attachments = new List<AttachmentWithTitleDto>();
        }
        public int? TenantId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DateFound { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime? ActionResponseDate { get; set; }
        public int? FindingStatusId { get; set; }
        public int? CriticalityId { get; set; }
        public FindingReportType Type { get; set; }
        public string OtherCategoryName { get; set; }

        public CAPAStatus FindingCAPAStatus { get; set; }
        public int BusinessEntityId { get; set; }
        public FindingReportCategory Category { get; set; }
            
        public FindingReportAction FindingAction { get; set; }
        public int? FindingReportClassificationId { get; set; }

        public int ControlRequirementId { get; set; }

        public string Details { get; set; }

        public int? AssessmentId { get; set; }

        public long? FinderId { get; set; }
        public int? VendorId { get; set; }

        public long? AuditorId { get; set; }
        public long? AssignedToUserId { get; set; }

        public long? FindingManagerId { get; set; }

        public long? FindingOwnerId { get; set; }

        public long? FindingCoordinatorId { get; set; }

        public List<int> RelatedBusinessRisks { get; set; }

        public List<int> RelatedExceptions { get; set; }

        public List<int> RelatedIncidents { get; set; }

        public FindingReportStatus Status { get; set; }

        public string AuditorRemark { get; set; }

        public bool CAPAUpdateRequired { get; set; }
        public List<AttachmentWithTitleDto> Attachments { get; set; }
        public IRMRelationDto EntityIRMRelations { get; set; }

        public List<int> SelectedFindingRemediations { get; set; }

        public IRMRelationDto AuthorityIRMRelations { get; set; }

        public string Reference { get; set; }
        public string ReviewComment { get; set; }
        public DateTime? FindingCloseDate { get; set; }
        public int? ExternalAssessmentId { get; set; }
        public ReviewDataResponseType ExternalAssessmentResponseType { get; set; }

    }

    public class AttachmentWithTitleDto
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public bool Static { get; set; }
    }

   

    public class FindingRemediationDto : Entity
    {
        public int FindingReportId { get; set; }
        public int? RemediationId { get; set; }
    }

    public class FindingInputDto {
        public FindingInputDto() {
            CreateOrEditFindingReportDto = new CreateOrEditFindingReportDto();
            FindingReportLogDto = new FindingReportLogDto();
        }
        public CreateOrEditFindingReportDto CreateOrEditFindingReportDto { get; set; }
        public FindingReportLogDto FindingReportLogDto { get; set; }
    }
}
