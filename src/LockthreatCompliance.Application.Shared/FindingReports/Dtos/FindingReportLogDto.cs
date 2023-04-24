using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    public class FindingReportLogDto : EntityDto
    {
        public int? TenantId { get; set; }
        public int FindingId { get; set; }
        public string UpdatedFieldName { get; set; }
        public bool CreateOrEditFlag { get; set; }

        public string Old_Title { get; set; }
        public string Old_Category { get; set; }
        public DateTime? Old_DateFound { get; set; }
        public string Old_FindingDetails { get; set; }
        public string Old_Reference { get; set; }
        public DateTime? Old_ActionResponseDate { get; set; }
        public DateTime? Old_DateClosed { get; set; }
        public string Old_CorrectiveActionResponse { get; set; }
        public string Old_ActualRootCause { get; set; }
        public string Old_Status { get; set; }
        public string Old_AuditorRemarks { get; set; }
        public bool Old_CAPAUpdateRequired { get; set; }
        public string Old_Attachment { get; set; }

        public string New_Title { get; set; }
        public string New_Category { get; set; }
        public DateTime? New_DateFound { get; set; }
        public string New_FindingDetails { get; set; }
        public string New_Reference { get; set; }
        public DateTime? New_ActionResponseDate { get; set; }
        public DateTime? New_DateClosed { get; set; }
        public string New_CorrectiveActionResponse { get; set; }
        public string New_ActualRootCause { get; set; }
        public string New_Status { get; set; }
        public string New_AuditorRemarks { get; set; }
        public bool New_CAPAUpdateRequired { get; set; }
        public string New_Attachment { get; set; }
        public long ModifyUserId { get; set; }
        public string UserName { get; set; }

    }

    public class FindingLogInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }


    public class LatestFindingByEntitIdDto {
        public int Id { get; set; }
        public int ControlRequirementId { get; set; }
        public string ControlRequirementName { get; set; }
        public FindingReportStatus Status { get; set; }

        public bool IsAttachmentExist { get; set; }
        public ReviewDataResponseType ExternalAssessmentResponseType { get; set; }


    }


    public class LatestFindingByEntitIdGroupDto : LatestFindingByEntitIdDto
    {
        public int ExternalAssessmentId { get; set; }
        public int BusinessEntityId { get; set; }
        public string LicenseNumber { get; set; }
        public ReviewDataResponseType ResponseType { get; set; }
    }

    public class OldAssessmentBusinessEntityIdExternalAssessmentId
    {
        public OldAssessmentBusinessEntityIdExternalAssessmentId() {
            OldAssessmentId = new List<int>();
        }
        public int AssessmentId { get; set; }
        public int ExternalAssessmentId { get; set; }
        public int BusinessEntityId { get; set; }
        public List<int> OldAssessmentId { get; set; }


    }

    public class FindingReviewDataDto 
    {
        public FindingReviewDataDto() {
            OpenFinding = new List<LatestFindingByEntitIdGroupDto>();
            FilterReviewData = new List<FindingGroupTestDto>();
        }
        public int Bid { get; set; }

        public List<LatestFindingByEntitIdGroupDto> OpenFinding { get; set; }
        public List<FindingGroupTestDto> FilterReviewData { get; set; }
        public int ReviewDataCount { get; set; }

    }

    public class Rdata {
        public int Id { get; set; }
        public ReviewDataResponseType ResponseType { get; set; }

    }

    public class FindingGroupTestDto
    {
        public int Id { get; set; }
        public int ControlRequirementId { get; set; }
        public ReviewDataResponseType ResponseType { get; set; }
        public int AttachmentsCount { get; set; }
    }



}
